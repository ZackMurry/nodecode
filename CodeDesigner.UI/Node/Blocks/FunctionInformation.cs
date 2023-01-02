namespace CodeDesigner.UI.Node.Blocks;

public class FunctionInformation
{
    public List<Parameter> Parameters;
    public Parameter.ParameterType ReturnType;

    public FunctionInformation(List<Parameter> parameters, Parameter.ParameterType returnType)
    {
        Parameters = parameters;
        ReturnType = returnType;
    }
    
}
