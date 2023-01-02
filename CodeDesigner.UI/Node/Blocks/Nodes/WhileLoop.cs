using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class WhileLoop : BlockBase
{
    public WhileLoop() : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 110,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "While Loop",
        OutputType = Parameter.ParameterType.Void
    })
    {
        NodeType = NodeType.WHILE_LOOP;

        UseOutput = true;
        
        Parameters.Add(new Parameter
        {
            Type = Parameter.ParameterType.Bool
        });

        CheckNext();
    }
}