using CodeDesigner.Core.ast;
using LLVMSharp;

namespace CodeDesigner.Core;

public class ClassAnalyzer : IAnalyzer
{

    private readonly Dictionary<string, ASTClassDefinition> _classDefs = new();
    private readonly CodegenData _data;

    private readonly HashSet<string> _nodesTypes = new()
    {
        "ASTClassDefinition"
    };

    public ClassAnalyzer(CodegenData data)
    {
        _data = data;
    }
    
    public bool ShouldAnalyzeNode(ASTNode astNode)
    {
        return _nodesTypes.Contains(astNode.GetType().Name);
    }

    public void Analyze(ASTNode node, string currentNamespace, string? parentClass)
    {
        switch (node.GetType().Name)
        {
            case "ASTClassDefinition":
            {
                var classDef = (ASTClassDefinition) node;
                _classDefs.Add($"{currentNamespace}.{classDef.ClassType.Name}", classDef);
                break;
            }
        }
    }

    public void Finalize(List<ASTNode> ast)
    {
        foreach (var (name, classDef) in _classDefs)
        {
            foreach (var genericUsage in classDef.GenericUsages)
            {
                var genericClassName = ClassType.Of(name, genericUsage).GetGenericName();
                var classType = LLVM.StructCreateNamed(_data.Context, genericClassName);
                _data.Classes.Add(genericClassName, new ClassData(classType, null));
            }
        }
    }
}