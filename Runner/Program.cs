using CodeDesigner.Core;
using CodeDesigner.Core.ast;

var ast = new List<ASTNode>();

ast.Add(new ASTPrototypeDeclaration("printf", new List<VariableType>
{
    new(PrimitiveVariableType.STRING)
}, new VariableType(PrimitiveVariableType.VOID), true));

ast.Add(new ASTFunctionDefinition("main", new List<ASTVariableDefinition>(), new List<ASTNode>
{
    new ASTForLoop(new List<ASTNode>
     {
    new ASTVariableDeclaration("i", new VariableType(PrimitiveVariableType.INTEGER), new ASTNumberExpression("2", PrimitiveVariableType.INTEGER)),
    }, new ASTVariableDeclaration("j", new VariableType(PrimitiveVariableType.INTEGER), new ASTNumberExpression("0", PrimitiveVariableType.INTEGER)),
    new ASTBinaryExpression(BinaryOperator.GT, new ASTNumberExpression("0", PrimitiveVariableType.INTEGER), new ASTNumberExpression("10", PrimitiveVariableType.INTEGER)),
    new ASTVariableAssignment("j", new ASTBinaryExpression(BinaryOperator.PLUS, new ASTVariableExpression("j"), new ASTNumberExpression("1", PrimitiveVariableType.INTEGER)))),
    new ASTFunctionInvocation("extern.printf", new List<ASTNode>
    {
        new ASTStringExpression("i: %d\n"),
        new ASTVariableExpression("i")
    })
}, new(PrimitiveVariableType.VOID)));

CodeGenerator.Run(ast);
