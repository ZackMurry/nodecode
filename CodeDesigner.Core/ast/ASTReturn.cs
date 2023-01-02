using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTReturn : ASTNode
{
    public ASTNode? Expression; // null means void

    public ASTReturn(ASTNode? expression = null)
    {
        Expression = expression;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (Expression == null)
        {
            return LLVM.BuildRetVoid(data.Builder);
        }
        var val = Expression.Codegen(data);
        if (val == null) return null;
        return LLVM.BuildRet(data.Builder, (LLVMValueRef) val);
    }
}