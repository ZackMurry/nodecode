using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTVariableExpression : ASTNode
{
    private readonly string Name;

    public ASTVariableExpression(string name)
    {
        Name = name;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (!data.NamedValues.ContainsKey(Name))
        {
            data.Errors.Add(new("Error: unknown variable identifier " + Name + ". Maybe the variable isn't in the correct context?", id));
        }
        return LLVM.BuildLoad(data.Builder, data.NamedValues[Name], Name);
    }
}