using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class PrototypeDeclaration : BlockBase
{
    public string Name;
    public Parameter.ParameterType ReturnType;
    public string? ObjectReturnType;

    public PrototypeDeclaration() : base( new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 50,
        Width = 70,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Prototype Declaration",
        OutputType = Parameter.ParameterType.Object
    })
    {
        Name = "main";
        ReturnType = Parameter.ParameterType.Void;
        ObjectReturnType = null;
        NodeType = NodeType.PROTOTYPE_DECLARATION;
    }
}