using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTClassDefinition : ASTNode
{
    public ClassType ClassType;
    public List<ASTVariableDefinition> Fields;
    public List<ASTFunctionDefinition> Methods;
    public Dictionary<string, MethodAttributes> MethodAttributesMap;
    public List<List<ClassType>> GenericUsages = new();

    public ASTClassDefinition(ClassType classType, List<ASTVariableDefinition> fields, List<ASTFunctionDefinition> methods, Dictionary<string, MethodAttributes> methodAttributesMap)
    {
        ClassType = classType;
        Fields = fields;
        Methods = methods;
        MethodAttributesMap = methodAttributesMap;
    }
    
    public ASTClassDefinition(ClassType classType, List<ASTVariableDefinition> fields, List<ASTFunctionDefinition> methods)
    {
        ClassType = classType;
        Fields = fields;
        Methods = methods;
        MethodAttributesMap = new();
    }


    public static VariableType MapVariableTypeToGenericType(VariableType v, List<ClassType> genericUsage, ClassType classType)
    {
        if (v.IsPrimitive || v.ClassType == null)
        {
            return v;
        }

        VariableType result = v;
        for (var i = 0; i < classType.GenericTypes.Count; i++)
        {
            if (v.ClassType.Name == classType.GenericTypes[i].Name)
            {
                Console.WriteLine("found generic type");
                result = new(genericUsage[i]);
                break;
            }
        }

        if (result.IsPrimitive || result.ClassType == null)
        {
            return result;
        }
        for (var i = 0; i < result.ClassType.GenericTypes.Count; i++)
        {
            var newType = MapVariableTypeToGenericType(new VariableType(result.ClassType.GenericTypes[i]), genericUsage, classType);
            if (newType.IsPrimitive || newType.ClassType == null)
            {
                throw new Exception("generic types should be classes");
            }
            result.ClassType.GenericTypes[i] = newType.ClassType;
        }

        return result;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        foreach (var genericUsage in GenericUsages)
        {
            var genericMap = new Dictionary<string, string>();
            for (var i = 0; i < ClassType.GenericTypes.Count; i++)
            {
                genericMap.Add(ClassType.GenericTypes[i].GetGenericName(), genericUsage[i].GetGenericName());
            }
            data.Generics = genericMap;

            var fullName = $"{data.NamespaceName}.{ClassType.Of(ClassType.Name, genericUsage).GetGenericName()}";
            Console.WriteLine("codegen for class def " + fullName);
            var fieldLlvmTypes = new List<LLVMTypeRef>();
            var classFieldTypes = new List<ClassFieldType>();

            if (!data.Classes.ContainsKey(fullName))
            {
                data.Errors.Add(new ErrorDescription("Error: expected class " + fullName + "to have a struct already generated during analysis", id));
                return null;
            }
            
            var classDef = data.Classes[fullName].Type;
            foreach (var field in Fields)
            {
                var fieldType = MapVariableTypeToGenericType(field.Type, genericUsage, ClassType).GetLLVMType(data);
                fieldLlvmTypes.Add(fieldType);
                classFieldTypes.Add(new ClassFieldType(field.Name, fieldType));
            }

            var methodOrder = new List<string>();
            var vtableTypeBody = new List<LLVMTypeRef>();
            var vtableValues = new List<LLVMValueRef>();
            foreach (var method in Methods)
            {
                methodOrder.Add(method.Name);
                var fullMethodName = $"{fullName}__{method.Name}";
                method.Params.Add(new ASTVariableDefinition("this", new VariableType(new ClassType(fullName))));
                var paramTypes = new List<LLVMTypeRef>();
                foreach (var param in method.Params)
                {
                    paramTypes.Add(param.Type.GetLLVMType(data));
                }

                var methodType = LLVM.FunctionType(method.ReturnType.GetLLVMType(data), paramTypes.ToArray(), false);
                var func = LLVM.AddFunction(data.Module, fullMethodName, methodType);
                if (MethodAttributesMap.ContainsKey(method.Name) && MethodAttributesMap[method.Name].IsVirtual)
                {
                    vtableTypeBody.Add(LLVM.PointerType(methodType, 0));
                    vtableValues.Add(func);
                }
            }
            var vtableType = LLVM.StructTypeInContext(data.Context, vtableTypeBody.ToArray(), false);
            fieldLlvmTypes.Insert(0, LLVM.PointerType(vtableType, 0));
            
            LLVM.StructSetBody(classDef, fieldLlvmTypes.ToArray(), false);
            data.Classes.Remove(fullName); // todo: see if this remove is needed, or if Dictionaries automatically replace old values
            data.Classes.Add(fullName, new ClassData(classDef, classFieldTypes, null, methodOrder, MethodAttributesMap));
            LLVM.StructSetBody(vtableType, vtableTypeBody.ToArray(), false);
            
            // todo: only add vtable to classes with 1+ virtual methods
            var vtableGlobal = LLVM.AddGlobal(data.Module, vtableType, $"{fullName}___vtable");
            LLVM.SetLinkage(vtableGlobal, LLVMLinkage.LLVMInternalLinkage);
            LLVM.SetInitializer(vtableGlobal, LLVM.ConstStructInContext(data.Context, vtableValues.ToArray(), false));

            // Initialize vtable
            LLVM.PositionBuilderAtEnd(data.Builder, LLVM.GetLastBasicBlock(LLVM.GetNamedFunction(data.Module, "__main")));
            for (var i = 0; i < vtableValues.Count; i++)
            {
                var gep = LLVM.BuildStructGEP(data.Builder, vtableGlobal, (uint) i, "");
                LLVM.BuildStore(data.Builder, vtableValues[i], gep);
            }

            data.Classes.Remove(fullName);
            data.Classes.Add(fullName, new ClassData(classDef, classFieldTypes, vtableGlobal, methodOrder, MethodAttributesMap));

            foreach (var method in Methods)
            {
                var originalMethodName = method.Name;
                method.Name = $"{fullName}__{method.Name}";
                method.Codegen(data);
                method.Name = originalMethodName;
                method.Params.RemoveAt(method.Params.Count - 1);
            }

            data.Generics = new Dictionary<string, string>(); // todo it doesnt seem like this would work with multiple generic classes??
        }
        return new LLVMValueRef();
    }
}