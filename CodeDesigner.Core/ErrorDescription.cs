namespace CodeDesigner.Core;

public class ErrorDescription
{
    public string Message;
    public Guid? Id;

    public ErrorDescription(string message, Guid? id = null)
    {
        Message = message;
        Id = id;
    }
    
}