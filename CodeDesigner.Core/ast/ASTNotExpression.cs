using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTNotExpression : ASTNode
{
    public ASTNode Value;

    public ASTNotExpression(ASTNode value)
    {
        Value = value;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        var val = Value.Codegen(data);
        if (val == null) return null;
        return LLVM.BuildNot(data.Builder, (LLVMValueRef) val, "not");
    }
}