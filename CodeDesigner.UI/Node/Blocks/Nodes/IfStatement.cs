using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class IfStatement : BlockBase
{
    public IfStatement() : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 110,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "If Statement",
        OutputType = Parameter.ParameterType.Void
    })
    {
        NodeType = NodeType.IF_STATEMENT;

        UseOutput = true;
        UseSecondaryOutput = true;
        
        Parameters.Add(new Parameter
        {
            Type = Parameter.ParameterType.Bool
        });

        CheckNext();
    }
}