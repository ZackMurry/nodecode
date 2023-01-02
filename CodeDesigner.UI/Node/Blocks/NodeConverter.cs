using CodeDesigner.Core;
using CodeDesigner.Core.ast;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Blocks.Types;
using CodeRunner.UI;

namespace CodeDesigner.UI.Node.Blocks;

public static class NodeConverter
{
    public static void CompileNodes(List<BlockBase> nodes)
    {
        Console.WriteLine("compiling");
        var ast = new List<ASTNode>();
        foreach (var map in Canvas.Canvas.NodeMap.Dependencies)
        {
            Console.WriteLine("map: " + map.Name);
            ConvertToAST(map.Blocks, ast, true);
        }
        ConvertToAST(nodes, ast);
        if (Program.dash.HasErrors())
        {
            return;
        }
        ast.Insert(0, new ASTPrototypeDeclaration("printf", new List<VariableType>
        {
            new(PrimitiveVariableType.STRING)
        }, new VariableType(PrimitiveVariableType.VOID), true));
        CodeGenerator.Run(ast);
    }

    private static void ConvertToAST(List<BlockBase> nodes, List<ASTNode> parentChildren, bool topLevelOnly = false)
    {
        foreach (var node in nodes)
        {
            if (topLevelOnly && node.NodeType != NodeType.FUNCTION_DEFINITION)
            {
                continue;
            }
            AnalyzeNode(node, parentChildren);
        }
    }

    private static void AnalyzeNode(BlockBase node, List<ASTNode> pc)
    {
        switch (node.NodeType)
        {
            case NodeType.BINARY_EXPRESSION:
            {
                var binExpNode = (BinaryExpression) node;
                ASTNode? left;
                if (binExpNode.Left != null)
                {
                    left = GetASTNodeFromString(binExpNode.Left);
                    if (left == null) return;
                } else if (binExpNode.Parameters.Count > 0 && binExpNode.Parameters[0] != null)
                {
                    left = GetASTNodeFromParam(binExpNode.Parameters[0] ?? throw new Exception(), binExpNode.Id);
                }
                else
                {
                    Program.dash.AddError("Error in binary expression: the left value must either be inline or passed as a parameter", node.Id);
                    return;
                }

                ASTNode? right;
                if (binExpNode.Right != null)
                {
                    right = GetASTNodeFromString(binExpNode.Right);
                    if (right == null) return;
                } else if (binExpNode.Parameters.Count > 1 && binExpNode.Parameters[1] != null)
                {
                    right = GetASTNodeFromParam(binExpNode.Parameters[1] ?? throw new Exception(), binExpNode.Id);
                }
                else
                {
                    Program.dash.AddError("Error in binary expression: the right value must either be inline or passed as a parameter", node.Id);
                    return;
                }
                pc.Add(new ASTBinaryExpression(ConvertBinOp(binExpNode.Operator), left ?? throw new Exception(), right ?? throw new Exception()).SetId(binExpNode.Id));
                break;
            }
            case NodeType.BOOLEAN_EXPRESSION:
            {
                var boolExpNode = (BooleanExpression) node;
                pc.Add(new ASTBooleanExpression(boolExpNode.Value).SetId(node.Id));
                break;
            }
            case NodeType.FUNCTION_DEFINITION:
            {
                var funDefNode = (FunctionDefinition) node;
                Console.WriteLine("generating " + funDefNode.Name + ": " + funDefNode.ReturnType);
                var astParams = new List<ASTVariableDefinition>();
                foreach (var p in funDefNode.Parameters)
                {
                    if (p == null)
                    {
                        Program.dash.AddError("Error: unexpected null parameter in definition of function " + funDefNode.Name + ": either remove the parameter or assign it a value", node.Id);
                        return;
                    }

                    var vt = GetVariableType(p.Type, p.ObjectType);
                    if (vt == null) return;
                    astParams.Add(new (p.Name, vt));
                }

                var body = new List<ASTNode>();
                if (funDefNode.NextBlock != null)
                {
                    AnalyzeNode(funDefNode.NextBlock, body);
                }
                var returnType = GetVariableType(funDefNode.ReturnType, funDefNode.ObjectReturnType);
                if (returnType == null) return;
                pc.Add(new ASTFunctionDefinition(funDefNode.Name, astParams, body, returnType).SetId(node.Id));
                break;
            }
            case NodeType.FUNCTION_INVOCATION:
            {
                var funInvNode = (FunctionInvocation) node;
                var astArgs = new List<ASTNode>();
                foreach (var arg in funInvNode.Parameters)
                {
                    if (arg == null)
                    {
                        Program.dash.AddError("Error: unexpected null parameter in invocation of function " + funInvNode.Name +
                                        ": either remove the parameter or assign it a value", node.Id);
                        return;
                    }
                    if (arg.Type == Parameter.ParameterType.Next)
                    {
                        continue;
                    }

                    var n = GetASTNodeFromParam(arg, funInvNode.Id);
                    if (n == null) return;
                    astArgs.Add(n);
                }

                pc.Add(new ASTFunctionInvocation(funInvNode.Name, astArgs).SetId(node.Id));
                if (funInvNode.NextBlock != null)
                {
                    AnalyzeNode(funInvNode.NextBlock, pc);
                }
                break;
            }
            case NodeType.IF_STATEMENT:
            {
                var ifNode = (IfStatement) node;
                if (ifNode.Parameters.Count != 2 || ifNode.Parameters[1] == null)
                {
                    Program.dash.AddError("Error: expected if statement to have a condition", ifNode.Id);
                    return;
                }

                var astCondition = GetASTNodeFromParam((Parameter) ifNode.Parameters[1], ifNode.Id);
                if (astCondition == null) return;
                var ifBody = new List<ASTNode>();
                if (ifNode.SecondaryOutput != null)
                {
                    AnalyzeNode(ifNode.SecondaryOutput, ifBody);
                }

                var elseBody = new List<ASTNode>();
                if (ifNode.Output != null)
                {
                    AnalyzeNode(ifNode.Output, elseBody);
                }
                pc.Add(new ASTIfStatement(astCondition, ifBody, elseBody));
                break;
            }
            case NodeType.NUMBER_EXPRESSION:
            {
                var numExpNode = (NumberExpression) node;
                pc.Add(new ASTNumberExpression(numExpNode.Value, numExpNode.Value.Contains('.') ? PrimitiveVariableType.DOUBLE : PrimitiveVariableType.INTEGER).SetId(node.Id));
                break;
            }
            case NodeType.PROTOTYPE_DECLARATION:
            {
                var protoNode = (PrototypeDeclaration) node;
                var astParams = new List<VariableType>();
                foreach (var p in protoNode.Parameters)
                {
                    if (p == null)
                    {
                        Program.dash.AddError("Unexpected null parameter in definition of prototype " + protoNode.Name +
                                            ": either remove the parameter or assign it a value", node.Id);
                        return;
                    }
                    var vt = GetVariableType(p.Type, p.ObjectType);
                    if (vt == null) return;
                    astParams.Add(vt);
                }

                var returnType = GetVariableType(protoNode.ReturnType, protoNode.ObjectReturnType);
                if (returnType == null) return;
                pc.Add(new ASTPrototypeDeclaration(protoNode.Name, astParams, returnType, false).SetId(node.Id));
                break;
            }
            case NodeType.STRING_EXPRESSION:
            {
                var stringExpNode = (StringExpression) node;
                pc.Add(new ASTStringExpression(stringExpNode.Value).SetId(node.Id));
                break;
            }
            case NodeType.RETURN:
            {
                var returnExp = (ReturnExpression) node;
                if (returnExp.Parameters.Count == 1)
                {
                    pc.Add(new ASTReturn().SetId(node.Id));
                } else if (returnExp.Parameters.Count == 2)
                {
                    var val = returnExp.Parameters[1]; // skip next
                    if (val == null)
                    {
                        Program.dash.AddError(
                            "Unexpected null parameter of return: either remove the parameter or assign it a value", node.Id);
                        return;
                    }
                    pc.Add(new ASTReturn(GetASTNodeFromParam(val, node.Id)).SetId(node.Id));
                }
                else
                {
                    Program.dash.AddError("Return statements should have at most one parameter!", node.Id);
                }

                break;
            }
            case NodeType.VARIABLE_ASSIGNMENT:
            {
                var varAssignNode = (VariableAssignment) node;
                if (varAssignNode.Parameters.Count != 2)
                {
                    Program.dash.AddError("Expected variable assignment to have one parameter but instead it has " + varAssignNode.Parameters.Count, node.Id);
                    return;
                }

                pc.Add(new ASTVariableAssignment(varAssignNode.Name, GetASTNodeFromParam(varAssignNode.Parameters[1] ?? throw new Exception("Unexpected null parameter in variable assignment"), node.Id).SetId(node.Id)));
                break;
            }
            case NodeType.VARIABLE_DECLARATION:
            {
                var varDeclNode = (VariableDeclaration) node;
                if (varDeclNode.Parameters.Count != 2)
                {
                    Program.dash.AddError("Expected variable assignment to have one parameter but instead it has " + varDeclNode.Parameters.Count, node.Id);
                    return;
                }

                var paramNode =
                    GetASTNodeFromParam(
                        varDeclNode.Parameters[1] ??
                        throw new Exception("Unexpected null parameter in variable assignment"), node.Id);
                
                pc.Add(new ASTVariableDeclaration(varDeclNode.Name, GetVariableType(varDeclNode.Type, varDeclNode.ObjectType), paramNode).SetId(node.Id));
                break;
            }
            case NodeType.VARIABLE_DEFINITION:
            {
                var varDefNode = (VariableDefinition) node;
                var vt = GetVariableType(varDefNode.Type, varDefNode.ObjectType);
                if (vt == null) return;
                pc.Add(new ASTVariableDefinition(varDefNode.Name, vt).SetId(node.Id));
                break;
            }
            case NodeType.VARIABLE_EXPRESSION:
            {
                var varExpNode = (VariableExpression) node;
                pc.Add(new ASTVariableExpression(varExpNode.Name).SetId(node.Id));
                break;
            }
            case NodeType.WHILE_LOOP:
            {
                var whileNode = (WhileLoop) node;
                if (whileNode.Parameters.Count != 2 || whileNode.Parameters[1] == null)
                {
                    Program.dash.AddError("Error: expected while loop to have a condition", whileNode.Id);
                    return;
                }

                var astCondition = GetASTNodeFromParam((Parameter) whileNode.Parameters[1], whileNode.Id);
                if (astCondition == null) return;
                var body = new List<ASTNode>();
                if (whileNode.Output != null)
                {
                    AnalyzeNode(whileNode.Output, body);
                }

                pc.Add(new ASTWhileLoop(astCondition, body));
                break;
            }
        }
    }

    private static ASTNode? GetASTNodeFromParam(Parameter param, Guid nodeId)
    {
        if (param.RawValue != null)
        {
            return GetASTNodeFromString(param.RawValue);
        }

        if (param.ReferenceValue == null)
        {
            Program.dash.AddError("Empty parameter", nodeId);
            return null;
        }
        
        var l = new List<ASTNode>();
        AnalyzeNode(param.ReferenceValue, l);
        if (l.Count != 1)
        {
            Program.dash.AddError("Expected exactly one expression for one parameter; found " +
                                l.Count, nodeId);
            return null;
        }
        return l[0].SetId(param.ReferenceValue.Id);
    }
    
    private static VariableType? GetVariableType(Parameter.ParameterType pt, string? objectType)
    {
        if (pt == Parameter.ParameterType.Object)
        {
            if (objectType == null)
            {
                Program.dash.AddError("Parameter type is Object, but ParameterObjectType is null!");
                return null;
            }
            return new VariableType(new ClassType(objectType));
        }

        
        return pt switch
        {
            Parameter.ParameterType.Bool => new VariableType(PrimitiveVariableType.BOOLEAN),
            Parameter.ParameterType.Double => new VariableType(PrimitiveVariableType.DOUBLE),
            Parameter.ParameterType.String => new VariableType(PrimitiveVariableType.STRING),
            Parameter.ParameterType.Int => new VariableType(PrimitiveVariableType.INTEGER),
            Parameter.ParameterType.Void => new VariableType(PrimitiveVariableType.VOID),
            _ => throw new ArgumentOutOfRangeException(nameof(pt), pt, null)
        };
    }

    private static ASTNode? GetASTNodeFromString(string s)
    {
        if (s.StartsWith('"'))
        {
            if (s.EndsWith('"')) return new ASTStringExpression(s.Substring(1, s.Length - 2));
            Program.dash.AddError(
                "A quotation mark at the beginning of an inline value must be closed by another quotation mark at the end of the value: " + s);
            return null;

        }

        if (s.Contains('.'))
        {
            return new ASTNumberExpression(s, PrimitiveVariableType.DOUBLE);
        }

        var chars = s.ToCharArray();
        if (chars[0] == '-' || char.IsDigit(chars[0]))
        {
            return new ASTNumberExpression(s, PrimitiveVariableType.INTEGER);
        }

        if (s.Equals("true") || s.Equals("false"))
        {
            return new ASTBooleanExpression(s.Equals("true"));
        }

        return new ASTVariableExpression(s);
    }

    private static BinaryOperator ConvertBinOp(BinOp binOp)
    {
        return binOp switch
        {
            BinOp.Add => BinaryOperator.PLUS,
            BinOp.Subtract => BinaryOperator.MINUS,
            BinOp.Multiply => BinaryOperator.TIMES,
            BinOp.Divide => BinaryOperator.DIVIDE,
            BinOp.Modulo => BinaryOperator.MODULO,
            BinOp.LogicalAnd => BinaryOperator.AND,
            BinOp.LogicalOr => BinaryOperator.OR,
            BinOp.Equal => BinaryOperator.EQ,
            BinOp.NotEqual => BinaryOperator.NE,
            BinOp.GreaterThan => BinaryOperator.GT,
            BinOp.LessThan => BinaryOperator.LT,
            BinOp.GreaterThanOrEqual => BinaryOperator.GE,
            BinOp.LessThanOrEqual => BinaryOperator.LE,
            _ => throw new ArgumentOutOfRangeException(binOp.ToString())
        };
    }
    
}