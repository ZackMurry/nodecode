using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTWhileLoop : ASTNode
{
    public ASTNode Condition;
    public List<ASTNode> Body;

    public ASTWhileLoop(ASTNode condition, List<ASTNode> body)
    {
        Condition = condition;
        Body = body;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (!data.Func.HasValue)
        {
            data.Errors.Add(new("Error: while loop expected to be inside a function", id));
            return null;
        }
        var initialCondition = Condition.Codegen(data);
        if (initialCondition == null) return null;
        var loopBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "whilebody");
        var mergeBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergewhile");
        LLVM.BuildCondBr(data.Builder, (LLVMValueRef) initialCondition, loopBlock, mergeBlock);
        
        var oldValues = new Dictionary<string, LLVMValueRef>(data.NamedValues);
        
        LLVM.PositionBuilderAtEnd(data.Builder, loopBlock);
        foreach (var node in Body)
        {
            node.Codegen(data);
        }

        var terminationVal = Condition.Codegen(data);
        if (terminationVal == null) return null;
        LLVM.BuildCondBr(data.Builder, (LLVMValueRef) terminationVal, loopBlock, mergeBlock);
        
        LLVM.PositionBuilderAtEnd(data.Builder, mergeBlock);
        data.NamedValues = oldValues;
        return mergeBlock;
    }
}
