using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class ReturnExpression : BlockBase
{
    public ReturnExpression() : base (new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 70,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Return Expression",
        OutputType = Parameter.ParameterType.Object
    })
    {
        NodeType = NodeType.RETURN;
    }
}
