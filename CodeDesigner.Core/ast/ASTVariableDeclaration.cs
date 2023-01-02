using LLVMSharp;

namespace CodeDesigner.Core.ast;

using Core;

public class ASTVariableDeclaration : ASTNode
{

    public string Name;
    public VariableType Type;
    public ASTNode Value;

    public ASTVariableDeclaration(string name, VariableType type, ASTNode value)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (!Type.IsPrimitive)
        {
            data.Errors.Add(new("Error: classes aren't implemented yet", id));
        }

        var llvmType = Type.GetLLVMType(data);
        var alloca = LLVM.BuildAlloca(data.Builder, llvmType, Name);
        var val = Value.Codegen(data);
        if (val == null) return null;
        LLVM.BuildStore(data.Builder, (LLVMValueRef) val, alloca);
        data.NamedValues.Add(Name, alloca);
        return alloca;
    }
}