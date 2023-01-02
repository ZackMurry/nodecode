using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTClassFieldAccess : ASTNode
{
    public ASTNode Object;
    public string FieldName;

    public ASTClassFieldAccess(ASTNode obj, string fieldName)
    {
        Object = obj;
        FieldName = fieldName;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        var obj = Object.Codegen(data);
        if (obj == null) return null;
        if (LLVM.GetTypeKind(LLVM.TypeOf(obj ?? throw new Exception())) != LLVMTypeKind.LLVMPointerTypeKind ||
            LLVM.GetTypeKind(LLVM.GetElementType(LLVM.TypeOf(obj ?? throw new Exception()))) != LLVMTypeKind.LLVMStructTypeKind)
        {
            data.Errors.Add(new("Error: unexpected field access of non-object variable", id));
            return null;
        }

        var className = VariableType.GetClassNameOfObject(obj ?? throw new Exception());
        if (!data.Classes.ContainsKey(className))
        {
            data.Errors.Add(new("Error: unknown class " + className, id));
            return null;
        }

        var fields = data.Classes[className].Fields;
        var fieldNumber = -1;
        for (var i = 0; i < fields.Count; i++)
        {
            if (fields[i].Name == FieldName)
            {
                fieldNumber = i;
                break;
            }
        }

        if (fieldNumber == -1)
        {
            data.Errors.Add(new("Error: unknown field of class " + className + ": " + FieldName, id));
            return null;
        }

        var gep = LLVM.BuildStructGEP(data.Builder, obj ?? throw new Exception(), (uint) fieldNumber + 1, "gep");
        return LLVM.BuildLoad(data.Builder, gep, "");

    }
}