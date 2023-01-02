using LLVMSharp;

namespace CodeDesigner.Core.ast;

public class ASTFunctionInvocation : ASTNode
{
    public string Name;
    public List<ASTNode> Args;

    public ASTFunctionInvocation(string name, List<ASTNode> args)
    {
        Name = name;
        Args = args;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        string fullName;
        if (Name.StartsWith("extern."))
        {
            fullName = Name[7..];
        }
        else if (Name.Contains('.'))
        {
            fullName = Name;
        }
        else
        {
            fullName = $"{data.NamespaceName}.{Name}";
        }
        Console.WriteLine("calling " + fullName);
        var func = LLVM.GetNamedFunction(data.Module, fullName);
        var argsV = new List<LLVMValueRef>();
        foreach (var arg in Args)
        {
            var argCodegen = arg.Codegen(data);
            if (argCodegen == null) return null;
            argsV.Add((LLVMValueRef) argCodegen);
        }

        return LLVM.BuildCall(data.Builder, func, argsV.ToArray(), "");
    }
}