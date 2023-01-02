using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTIfStatement : ASTNode
{
    public ASTNode Condition;
    public List<ASTNode> IfBody;
    public List<ASTNode> ElseBody;

    public ASTIfStatement(ASTNode condition, List<ASTNode> ifBody, List<ASTNode> elseBody)
    {
        Condition = condition;
        IfBody = ifBody;
        ElseBody = elseBody;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        var conditionValue = Condition.Codegen(data);
        if (conditionValue == null) return null;
        if (!data.Func.HasValue)
        {
            data.Errors.Add(new("expected to be inside a function", id));
        }
        var ifBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "ifbody");
        LLVMBasicBlockRef? elseBlock = null;
        if (ElseBody.Count != 0)
        {
            elseBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "elsebody");
        }
        // todo: this doesn't work with nested if statements
        var mergeBlock = LLVM.AppendBasicBlockInContext(data.Context, data.Func.Value, "mergeif");
        LLVM.BuildCondBr(data.Builder, (LLVMValueRef) conditionValue, ifBlock, elseBlock ?? mergeBlock);

        var oldValues = new Dictionary<string, LLVMValueRef>(data.NamedValues);

        LLVM.PositionBuilderAtEnd(data.Builder, ifBlock);
        foreach (var node in IfBody)
        {
            node.Codegen(data);
        }
        
        data.NamedValues = oldValues;

        if (IfBody.Count == 0 || !IfBody[^1].GetType().Name.Equals("ASTReturn"))
        {
            LLVM.BuildBr(data.Builder, mergeBlock);
        }

        if (elseBlock.HasValue)
        {
            LLVM.PositionBuilderAtEnd(data.Builder, elseBlock.Value);
            foreach (var node in ElseBody)
            {
                node.Codegen(data);
            }

            if (ElseBody.Count != 0 && !ElseBody[^1].GetType().Name.Equals("ASTReturn"))
            {
                LLVM.BuildBr(data.Builder, mergeBlock);
            }
            data.NamedValues = oldValues;
        }
        LLVM.PositionBuilderAtEnd(data.Builder, mergeBlock);
        data.NamedValues = oldValues;
        return mergeBlock;
    }
}