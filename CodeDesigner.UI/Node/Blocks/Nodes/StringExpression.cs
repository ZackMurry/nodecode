using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class StringExpression : BlockBase
{
    public String Value;
    
    public StringExpression() : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 70,
        Width = 140,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "String Expression",
        OutputType = Parameter.ParameterType.Object
    })
    {
        if (Value == null)
        {
            Value = "";
        }
        NodeType = NodeType.STRING_EXPRESSION;
        CanHavePrevious = false;
        AddElements();
    }

    public override void AddElements()
    {
        Elements = new List<Element>();
        TextBoxElement element = new TextBoxElement(new ElementProperties
        {
            BlockCoordinates = new PointF(10, 30),
            Size = new SizeF(90, 30)
        }, Color.Gray, Color.DarkGray, Color.Beige, () =>
        {
            Value = ((TextBoxElement) Elements[0]).Text;
        });
        element.Text = Value;
        Elements.Add(element);
    }
}