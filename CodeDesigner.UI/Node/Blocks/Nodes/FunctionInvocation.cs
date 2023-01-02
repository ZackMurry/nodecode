using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class FunctionInvocation : BlockBase
{
    public string Name;
    
    public FunctionInvocation() : base(new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 110,
        Width = 215,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Function Invocation",
        OutputType = Parameter.ParameterType.Object
    })
    {
        if (Name == null)
        {
            Name = "extern.printf";
        }
        NodeType = NodeType.FUNCTION_INVOCATION;
        AddElements();
        UpdateFunction();
        CheckNext();
    }

    public override void AddElements()
    {
        Elements = new List<Element>();
        TextBoxElement element = new TextBoxElement(new ElementProperties
        {
            BlockCoordinates = new PointF(60, 38),
            Size = new SizeF(120, 30)
        }, Color.Gray, Color.DarkGray, Color.Beige, () =>
        {
            Name = ((TextBoxElement) Elements[0]).Text;
            UpdateFunction();
        });
        element.Text = Name;
        
        Elements.Add(element);
    }

    public void UpdateFunction()
    {
        Parameters.Clear();
        var textBox = (TextBoxElement) Elements[0];
        var name = textBox.Text;
        if (Canvas.Canvas.FunctionData.ContainsKey(name))
        {
            var fun = Canvas.Canvas.FunctionData[name];
            foreach (var parameter in fun.Parameters)
            {
                Parameters.Add(new Parameter
                {
                    Type = parameter.Type
                });
            }

            UseOutput = fun.ReturnType != Parameter.ParameterType.Void;
        } else if (name == "extern.printf")
        {
            Parameters.Add(new Parameter
            {
                Type = Parameter.ParameterType.String
            });
            UseOutput = false;
        }
        CheckNext();
    }
    
}