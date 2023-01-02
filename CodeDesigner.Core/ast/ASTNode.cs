using LLVMSharp;

namespace CodeDesigner.Core;

public abstract class ASTNode
{
    protected Guid? id;
    public ASTNode SetId(Guid id)
    {
        this.id = id;
        return this;
    }
    
    public abstract LLVMValueRef? Codegen(CodegenData data);
}