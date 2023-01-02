using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Blocks.Types;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class BinaryExpression : BlockBase
{
    public BinOp Operator;

    public string? Left;
    public string? Right;

    public BinaryExpression() : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 70,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Binary Expression",
        OutputType = Parameter.ParameterType.Object
    })
    {
        Operator = BinOp.GreaterThanOrEqual;

        Parameters.Add(new Parameter
        {
            Name = "Left Input",
            Type = Parameter.ParameterType.Object
        });

        Parameters.Add(new Parameter
        {
            Name = "Right Input",
            Type = Parameter.ParameterType.Object
        });

        CanHavePrevious = false;
        NodeType = NodeType.BINARY_EXPRESSION;
    }
}
