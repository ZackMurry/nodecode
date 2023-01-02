using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTStringExpression : ASTNode
{
    private string Value;

    public ASTStringExpression(string value)
    {
        Value = value;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        return LLVM.BuildGlobalStringPtr(data.Builder, Value, "");
    }
}