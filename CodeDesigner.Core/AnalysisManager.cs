using CodeDesigner.Core.ast;

namespace CodeDesigner.Core;

// todo: eventually, some steps may have to be done sequentially (i.e., not all concurrently)
public class AnalysisManager
{

    private List<IAnalyzer> _analyzers;
    private string _currentNamespace = "default";
    private string? _currentClass = null;

    public AnalysisManager()
    {
        _analyzers = new List<IAnalyzer>();
    }

    public AnalysisManager AddAnalyzer(IAnalyzer analysisStep)
    {
        _analyzers.Add(analysisStep);
        return this;
    }

    public void ClearAnalyzers()
    {
        _analyzers.Clear();
    }
    
    private void RunAnalysisOnNode(ASTNode node)
    {
        foreach (var analyzer in _analyzers)
        {
            if (analyzer.ShouldAnalyzeNode(node))
            {
                analyzer.Analyze(node, _currentNamespace, _currentClass);
            }
        }
    }

    private void Traverse(ASTNode node)
    {
        RunAnalysisOnNode(node);
        switch (node.GetType().Name)
        {
            case "ASTBinaryExpression":
            {
                var binExp = (ASTBinaryExpression) node;
                Traverse(binExp.Lhs);
                Traverse(binExp.Rhs);
                break;
            }
            case "ASTClassDefinition":
            {
                var classDef = (ASTClassDefinition) node;
                _currentClass = $"{_currentNamespace}.{classDef.ClassType.Name}";
                foreach (var field in classDef.Fields)
                {
                    Traverse(field);
                }
                foreach (var method in classDef.Methods)
                {
                    Traverse(method);
                }

                _currentClass = null;
                break;
            }
            case "ASTClassFieldAccess":
            {
                var fieldAcc = (ASTClassFieldAccess) node;
                Traverse(fieldAcc.Object);
                break;
            }
            case "ASTClassFieldStore":
            {
                var fieldStore = (ASTClassFieldStore) node;
                Traverse(fieldStore.Object);
                Traverse(fieldStore.Value);
                break;
            }
            case "ASTClassInstantiation":
            {
                var classInst = (ASTClassInstantiation) node;
                foreach (var arg in classInst.Args)
                {
                    Traverse(arg);
                }

                break;
            }
            case "ASTForLoop":
            {
                var forLoop = (ASTForLoop) node;
                if (forLoop.Initializer != null)
                {
                    Traverse(forLoop.Initializer);
                }
                if (forLoop.Condition != null)
                {
                    Traverse(forLoop.Condition);
                }
                foreach (var n in forLoop.Body)
                {
                    Traverse(n);
                }
                if (forLoop.Action != null)
                {
                    Traverse(forLoop.Action);
                }

                break;
            }
            case "ASTFunctionDefinition":
            {
                var funcDef = (ASTFunctionDefinition) node;
                foreach (var b in funcDef.Body)
                {
                    Traverse(b);
                }

                foreach (var p in funcDef.Params)
                {
                    Traverse(p);
                }
                break;
            }
            case "ASTFunctionInvocation":
            {
                var funcInv = (ASTFunctionInvocation) node;
                foreach (var arg in funcInv.Args)
                {
                    Traverse(arg);
                }

                break;
            }
            case "ASTIfStatement":
            {
                var ifStmt = (ASTIfStatement) node;
                Traverse(ifStmt.Condition);
                foreach (var n in ifStmt.IfBody)
                {
                    Traverse(n);
                }

                foreach (var n in ifStmt.ElseBody)
                {
                    Traverse(n);
                }

                break;
            }
            case "ASTMethodInvocation":
            {
                var methodInv = (ASTMethodInvocation) node;
                Traverse(methodInv.Object);
                foreach (var arg in methodInv.Args)
                {
                    Traverse(arg);
                }
                break;
            }
            case "ASTNamespace":
            {
                var nmspc = (ASTNamespace) node;
                _currentNamespace = nmspc.Name;
                foreach (var n in nmspc.Body)
                {
                    Traverse(n);
                }

                _currentNamespace = "default";
                break;
            }
            case "ASTNotExpression":
            {
                var not = (ASTNotExpression) node;
                Traverse(not.Value);
                break;
            }
            case "ASTReturn":
            {
                var ret = (ASTReturn) node;
                if (ret.Expression != null)
                {
                    Traverse(ret.Expression);
                }

                break;
            }
            case "ASTVariableAssignment": {
                var varAssign = (ASTVariableAssignment) node;
                Traverse(varAssign.Value);
                break;
            }
            case "ASTVariableDeclaration":
            {
                var varDecl = (ASTVariableDeclaration) node;
                Traverse(varDecl.Value);
                break;
            }
            case "ASTWhileLoop":
            {
                var whileLoop = (ASTWhileLoop) node;
                Traverse(whileLoop.Condition);
                foreach (var n in whileLoop.Body)
                {
                    Traverse(n);
                }

                break;
            }
        }
    }

    public void RunAnalysis(List<ASTNode> ast)
    {
        foreach (var node in ast)
        {
            Traverse(node);
        }

        foreach (var analyzer in _analyzers)
        {
            analyzer.Finalize(ast);
        }
    }
    
}