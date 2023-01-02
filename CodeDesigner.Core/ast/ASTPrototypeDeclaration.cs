using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTPrototypeDeclaration : ASTNode
{
    public string Name; // full name of prototype (including namespace. for an extern function (like printf), leave out "extern."
    public List<VariableType> ParamTypes;
    public VariableType ReturnType;
    public bool IsVarArgs;

    public ASTPrototypeDeclaration(string name, List<VariableType> paramTypes, VariableType returnType,
        bool isVarArgs)
    {
        Name = name;
        ParamTypes = paramTypes;
        ReturnType = returnType;
        IsVarArgs = isVarArgs;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (LLVM.GetNamedFunction(data.Module, Name).Pointer.ToInt64() != 0)
        {
            return new LLVMValueRef();
        }
        var llvmParamTypes = new List<LLVMTypeRef>();
        foreach (var argType in ParamTypes)
        {
            llvmParamTypes.Add(argType.GetLLVMType(data));
        }

        if (!ReturnType.IsPrimitive)
        {
            data.Errors.Add(new("classes aren't implemented yet", id));
            return null;
        }
        var funcType = LLVM.FunctionType(ReturnType.GetLLVMType(data), llvmParamTypes.ToArray(), IsVarArgs);
        return LLVM.AddFunction(data.Module, Name, funcType);
    }
}