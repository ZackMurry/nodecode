using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTBooleanExpression : ASTNode
{
    public bool Value;

    public ASTBooleanExpression(bool value)
    {
        Value = value;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        return LLVM.ConstInt(LLVM.Int1TypeInContext(data.Context), (ulong) (Value ? 1 : 0), false);
    }
}