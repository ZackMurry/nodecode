using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTMethodInvocation : ASTNode
{
    public ASTNode Object;
    public string MethodName;
    public List<ASTNode> Args;

    public ASTMethodInvocation(ASTNode obj, string methodName, List<ASTNode> args)
    {
        Object = obj;
        MethodName = methodName;
        Args = args;
    }
    
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        var obj = Object.Codegen(data);
        if (obj == null) return null;
        if (LLVM.GetTypeKind(LLVM.TypeOf((LLVMValueRef) obj)) != LLVMTypeKind.LLVMPointerTypeKind ||
            LLVM.GetTypeKind(LLVM.GetElementType(LLVM.TypeOf((LLVMValueRef) obj))) != LLVMTypeKind.LLVMStructTypeKind)
        {
            data.Errors.Add(new("unexpected method call on non-object variable", id));
            return null;
        }

        var className = VariableType.GetClassNameOfObject((LLVMValueRef) obj);
        if (!data.Classes.ContainsKey(className))
        {
            data.Errors.Add(new("Error: unknown class " + className, id));
            return null;
        }

        var classData = data.Classes[className];

        LLVMValueRef method;
        if (classData.Methods.ContainsKey(MethodName) && classData.Methods[MethodName].IsVirtual)
        {
            uint methodIndex = 0;
            foreach (var m in classData.MethodOrder)
            {
                if (m == MethodName)
                {
                    break;
                }
                methodIndex++;
            }

            if (methodIndex == classData.MethodOrder.Count)
            {
                data.Errors.Add(new ErrorDescription("Error: method registered in data as a method, but not included in method order", id));
                return null;
            }

            var vtable = LLVM.BuildLoad(data.Builder,
                LLVM.BuildStructGEP(data.Builder, (LLVMValueRef) obj, 0, ""), "");
            var gep = LLVM.BuildStructGEP(data.Builder, vtable, methodIndex, "");
            method = LLVM.BuildLoad(data.Builder, gep, "");
        }
        else
        {
            method = LLVM.GetNamedFunction(data.Module, $"{className}__{MethodName}");
            if (method.Pointer.ToInt64() == 0)
            {
                data.Errors.Add(new("unknown method " + MethodName + " of class " + className, id));
            }
        }

        var llvmArgs = new List<LLVMValueRef>();
        foreach (var arg in Args)
        {
            var cd = arg.Codegen(data);
            if (cd == null) return null;
            llvmArgs.Add((LLVMValueRef) cd);
        }
        llvmArgs.Add((LLVMValueRef) obj); // Add reference to object ("this")
        return LLVM.BuildCall(data.Builder, method, llvmArgs.ToArray(), "");
    }
}