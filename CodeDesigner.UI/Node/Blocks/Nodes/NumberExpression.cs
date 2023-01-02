using CodeDesigner.Core;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class NumberExpression : BlockBase
{
    public string Value;
    
    public NumberExpression() : base (new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 60,
        Width = 90,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Number",
        OutputType = Parameter.ParameterType.Object
    })
    {
        if (Value == null)
        {
            Value = "0";
        }
        NodeType = NodeType.NUMBER_EXPRESSION;
    }

    public override void AddElements()
    {
        Elements = new List<Element>();
        TextBoxElement element = new TextBoxElement(new ElementProperties
        {
            BlockCoordinates = new PointF(10, 30),
            Size = new SizeF(60, 25)
        }, Color.Gray, Color.DarkGray, Color.Beige, () =>
        {
            Value = ((TextBoxElement) Elements[0]).Text;
        });
        Elements.Add(element);
    }
    
}