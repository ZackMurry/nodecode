namespace CodeDesigner.Core;

public interface IAnalyzer
{
    bool ShouldAnalyzeNode(ASTNode astNode);
    
    void Analyze(ASTNode node, string currentNamespace, string? parentClass);

    void Finalize(List<ASTNode> ast);
}