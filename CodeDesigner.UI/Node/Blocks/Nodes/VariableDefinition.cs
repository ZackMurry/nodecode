using CodeDesigner.UI.Designer.Toolbox;

namespace CodeDesigner.UI.Node.Blocks.Nodes;

[Serializable]
public class VariableDefinition : BlockBase
{
    public string Name;
    public Parameter.ParameterType Type;
    public string? ObjectType;
    
    public VariableDefinition(string name, Parameter.ParameterType type, string? objectType = null) : base (new BlockProperties
    {
        BorderColor = Color.FromArgb(69, 69, 69),
        FillColor = Color.FromArgb(85, 85, 85),
        SecondaryColor = Color.FromArgb(69, 69, 69),
        Height = 50,
        Width = 70,
        TextColor = Color.FromArgb(255, 255, 255),
        Name = "Variable Definition",
        OutputType = Parameter.ParameterType.Object
    })
    {
        Name = name;
        Type = type;
        ObjectType = objectType;

        NodeType = NodeType.VARIABLE_DEFINITION;
        CheckNext();
    }
}