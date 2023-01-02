using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTVariableAssignment : ASTNode
{
    public string Name;
    public ASTNode Value;

    public ASTVariableAssignment(string name, ASTNode value)
    {
        Name = name;
        Value = value;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (!data.NamedValues.ContainsKey(Name))
        {
            data.Errors.Add(new("Error: unknown variable " + Name, id));
            return null;
        }

        var llvmVar = data.NamedValues[Name];
        var val = Value.Codegen(data);
        if (val == null) return null;
        LLVM.BuildStore(data.Builder, (LLVMValueRef) val, llvmVar);
        return llvmVar;
    }
}