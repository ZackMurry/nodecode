using System.Runtime.InteropServices;
using LLVMSharp;

namespace CodeDesigner.Core.ast;
using Core;

public class ASTFunctionDefinition : ASTNode
{

    public string Name;
    public List<ASTVariableDefinition> Params;
    public List<ASTNode> Body;
    public VariableType ReturnType;

    public ASTFunctionDefinition(string name, List<ASTVariableDefinition> @params, List<ASTNode> body,
        VariableType returnType)
    {
        Name = name;
        Params = @params;
        Body = body;
        ReturnType = returnType;
    }

    public override LLVMValueRef? Codegen(CodegenData data)
    {
        if (data.Func.HasValue)
        {
            data.Errors.Add(new("Error: cannot define a function within a function", id));
        }

        var fullName = Name;
        if (!Name.Contains('.'))
        {
            fullName = Name == "main" ? "__main_designer" : $"{data.NamespaceName}.{Name}";
        }
        var paramTypes = new LLVMTypeRef[Params.Count];

        for (var i = 0; i < Params.Count; i++)
        {
            paramTypes[i] = Params[i].Type.GetLLVMType(data);
        }

        var functionType = LLVM.FunctionType(ReturnType.GetLLVMType(data), paramTypes, false);
        // var isAlreadyDefined = func.Pointer.ToInt64() != 0;
        // if (!isAlreadyDefined)
        // {
        var func = LLVM.AddFunction(data.Module, fullName, functionType);
        // }

        var funcEntryBlock = LLVM.AppendBasicBlockInContext(data.Context, func, "entry");
        LLVM.PositionBuilderAtEnd(data.Builder, funcEntryBlock);
        data.NamedValues.Clear();
        data.Func = func;
        
        for (var i = 0; i < Params.Count; i++)
        {
            var arg = LLVM.GetParam(func, (uint) i);
            var alloca = LLVM.BuildAlloca(data.Builder, paramTypes[i], Params[i].Name);
            LLVM.BuildStore(data.Builder, arg, alloca);
            data.NamedValues.Add(Params[i].Name, alloca);
            // todo: check if object
        }
        
        foreach (var node in Body)
        {
            node.Codegen(data);
        }
        
        if ((ReturnType.IsPrimitive && ReturnType.PrimitiveType == PrimitiveVariableType.VOID) || Body.Count == 0 || Body[^1].GetType().Name != "ASTReturn")
        {
            LLVM.BuildRetVoid(data.Builder);
        }
        data.NamedValues.Clear();
        data.Func = null;
        return func;
    }
}