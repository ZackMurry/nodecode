using CodeDesigner.Core.ast;

namespace CodeDesigner.Core;

/// <summary>
/// Adds prototype declarations for functions that need them. Prototypes are needed in order to call functions before
/// they are defined. In the LLVM IR, these prototypes are usually removed (by built-in LLVM optimizations) and the
/// prototypes and functions are just merged, with the functions in the correct order.
/// </summary>
public class PrototypeAnalyzer : IAnalyzer
{

    private readonly HashSet<string> _functionNames = new();
    private readonly HashSet<string> _neededPrototypes = new();
    private readonly Dictionary<string, ASTFunctionDefinition> _futurePrototypes = new();

    private readonly HashSet<string> _nodeTypes = new()
    {
        "ASTFunctionDefinition", "ASTFunctionInvocation"
    };

    public bool ShouldAnalyzeNode(ASTNode astNode)
    {
        return _nodeTypes.Contains(astNode.GetType().Name);
    }

    public void Analyze(ASTNode node, string currentNamespace, string? parentClass)
    {
        switch (node.GetType().Name)
        {
            case "ASTFunctionDefinition":
            {
                var funcDef = (ASTFunctionDefinition) node;
                var fullName = $"{currentNamespace}.{funcDef.Name}";
                _functionNames.Add(fullName);
                if (_neededPrototypes.Contains(fullName))
                {
                    _neededPrototypes.Remove(fullName);
                    _futurePrototypes.Add(fullName, funcDef);
                }
                break;
            }
            case "ASTFunctionInvocation":
            {
                var funcInv = (ASTFunctionInvocation) node;
                var fullName = funcInv.Name.Contains('.') ? funcInv.Name : $"{currentNamespace}.{funcInv.Name}";
                if (!funcInv.Name.StartsWith("extern.") && !_functionNames.Contains(fullName))
                {
                    _neededPrototypes.Add(fullName);
                }
                break;
            }
        }
    }

    public void Finalize(List<ASTNode> ast)
    {
        Console.WriteLine("finalizing");
        foreach (var (fullName, node) in _futurePrototypes)
        {
            Console.WriteLine("generating prototype for " + fullName);
            var paramTypes = node.Params.Select(param => param.Type).ToList();
            ast.Insert(0, new ASTPrototypeDeclaration(fullName, paramTypes, node.ReturnType, false));
        }
    }
    
}