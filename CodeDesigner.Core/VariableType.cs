using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class VariableType
{

    public readonly bool IsPrimitive;

    public readonly PrimitiveVariableType? PrimitiveType;

    public readonly ClassType? ClassType;

    public VariableType(bool isPrimitive, PrimitiveVariableType? primitiveType, ClassType? classType)
    {
        IsPrimitive = isPrimitive;
        PrimitiveType = primitiveType;
        ClassType = classType;
    }

    public VariableType(PrimitiveVariableType primitiveType)
    {
        IsPrimitive = true;
        PrimitiveType = primitiveType;
    }

    public VariableType(ClassType classType)
    {
        IsPrimitive = false;
        ClassType = classType;
    }

    public LLVMTypeRef GetLLVMType(CodegenData data)
    {
        if (IsPrimitive)
        {
            if (!PrimitiveType.HasValue)
            {
                throw new Exception("expected primitive type to have value");
            }
            return PrimitiveType.Value switch
            {
                PrimitiveVariableType.INTEGER => LLVM.Int64TypeInContext(data.Context),
                PrimitiveVariableType.DOUBLE => LLVM.DoubleTypeInContext(data.Context),
                PrimitiveVariableType.BOOLEAN => LLVM.Int1TypeInContext(data.Context),
                PrimitiveVariableType.VOID => LLVM.VoidTypeInContext(data.Context),
                PrimitiveVariableType.STRING => LLVM.PointerType(LLVM.Int8TypeInContext(data.Context), 0),
                _ => throw new Exception("unimplemented primitive type")
            };
        }

        if (ClassType == null)
        {
            throw new Exception("expected object type to have a class type");
        }

        var classTypeName = data.Generics.ContainsKey(ClassType.Name) ? data.Generics[ClassType.Name] : ClassType.Name;

        switch (classTypeName)
        {
            case "Integer":
            {
                return new VariableType(PrimitiveVariableType.INTEGER).GetLLVMType(data);
            }
            case "Double":
            {
                return new VariableType(PrimitiveVariableType.DOUBLE).GetLLVMType(data);
            }
            case "Boolean":
            {
                return new VariableType(PrimitiveVariableType.BOOLEAN).GetLLVMType(data);
            }
            case "String":
            {
                return new VariableType(PrimitiveVariableType.STRING).GetLLVMType(data);
            }
        }
        
        var genericName = new ClassType(classTypeName, ClassType.GenericTypes).GetGenericName();
        var fullClassName = classTypeName.Contains('.') ? genericName : $"default.{genericName}";
        if (!data.Classes.ContainsKey(fullClassName))
        {
            throw new InvalidCodeException("unknown class " + fullClassName);
        }

        return LLVM.PointerType(data.Classes[fullClassName].Type, 0);
    }

    public static string GetClassNameOfObject(LLVMValueRef obj)
    {
        var className = LLVM.TypeOf(obj).ToString();
        if (className.StartsWith('%'))
        {
            className = className[1..];
        }

        if (className.EndsWith('*'))
        {
            className = className[..^1];
        }

        if (className.StartsWith('"') && className.EndsWith('"'))
        {
            className = className[1..^1];
        }

        return className;
    }
    
}