using LLVMSharp;

namespace CodeDesigner.Core.ast;
using Core;

public class ASTVariableDefinition : ASTNode
{

    public string Name;
    public VariableType Type;

    public ASTVariableDefinition(string name, VariableType type)
    {
        Name = name;
        Type = type;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        var llvmType = Type.GetLLVMType(data);
        var alloca = LLVM.BuildAlloca(data.Builder, llvmType, Name);
        data.NamedValues.Add(Name, alloca);
        return alloca;
    }
}