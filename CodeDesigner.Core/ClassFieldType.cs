using LLVMSharp;

namespace CodeDesigner.Core;

public class ClassFieldType
{
    public string Name;
    public LLVMTypeRef Type;

    public ClassFieldType(string name, LLVMTypeRef type)
    {
        Name = name;
        Type = type;
    }
}