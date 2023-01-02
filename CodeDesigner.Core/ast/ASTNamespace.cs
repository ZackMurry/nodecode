using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTNamespace : ASTNode
{
    public string Name;
    public List<ASTNode> Body;

    public ASTNamespace(string name, List<ASTNode> body)
    {
        Name = name;
        Body = body;
    }
    
    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (data.Func.HasValue)
        {
            data.Errors.Add(new("Error: a namespace cannot be declared inside of a function", id));
        }
        data.NamespaceName = Name;
        foreach (var node in Body)
        {
            node.Codegen(data);
        }

        data.NamespaceName = "default";
        return new LLVMValueRef(); // unused
    }
}