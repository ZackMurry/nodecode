using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTForLoop : ASTNode
{
    public ASTNode? Initializer;
    public ASTNode? Condition;
    public ASTNode? Action;
    public List<ASTNode> Body;

    public ASTForLoop(List<ASTNode> body, ASTNode? initializer = null, ASTNode? condition = null, ASTNode? action = null)
    {
        Initializer = initializer;
        Condition = condition;
        Action = action;
        Body = body;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (!data.Func.HasValue)
        {
            data.Errors.Add(new("Error: for loops need to be inside a function", id));
            return null;
        }

        Initializer?.Codegen(data);
        var loopBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "forbody");
        var mergeBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergefor");
        if (Condition != null)
        {
            var initialCondition = Condition.Codegen(data);
            if (initialCondition == null) return null;
            LLVM.BuildCondBr(data.Builder, (LLVMValueRef) initialCondition, loopBlock, mergeBlock);
        }
        else
        {
            LLVM.BuildBr(data.Builder, loopBlock);
        }
        
        var oldValues = new Dictionary<string, LLVMValueRef>(data.NamedValues);
        
        LLVM.PositionBuilderAtEnd(data.Builder, loopBlock);
        foreach (var node in Body)
        {
            node.Codegen(data);
        }

        data.NamedValues = oldValues;

        Action?.Codegen(data);
        if (Condition != null)
        {
            var terminationVal = Condition.Codegen(data);
            if (terminationVal == null) return null;
            LLVM.BuildCondBr(data.Builder, (LLVMValueRef) terminationVal, loopBlock, mergeBlock);
        }
        else
        {
            LLVM.BuildBr(data.Builder, loopBlock);
        }
        LLVM.PositionBuilderAtEnd(data.Builder, mergeBlock);
        return mergeBlock;
    }
}