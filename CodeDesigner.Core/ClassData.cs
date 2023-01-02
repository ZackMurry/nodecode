using LLVMSharp;

namespace CodeDesigner.Core;

public class ClassData
{
    public LLVMTypeRef Type;
    public List<ClassFieldType> Fields;
    public List<string> MethodOrder;
    public LLVMValueRef? VtableGlobal;
    public Dictionary<string, MethodAttributes> Methods;

    public ClassData(LLVMTypeRef type, LLVMValueRef? vtableGlobal)
    {
        Type = type;
        Fields = new List<ClassFieldType>();
        MethodOrder = new List<string>();
        VtableGlobal = vtableGlobal;
        Methods = new Dictionary<string, MethodAttributes>();
    }
    
    public ClassData(LLVMTypeRef type, List<ClassFieldType> fields, LLVMValueRef? vtableGlobal, List<string> methodOrder, Dictionary<string, MethodAttributes> methods)
    {
        Type = type;
        Fields = fields;
        MethodOrder = methodOrder;
        Methods = methods;
        VtableGlobal = vtableGlobal;
    }
    
    public ClassData(LLVMTypeRef type, List<ClassFieldType> fields, List<string> methodOrder, Dictionary<string, MethodAttributes> methods, LLVMValueRef? vtableGlobal)
    {
        Type = type;
        Fields = fields;
        MethodOrder = methodOrder;
        Methods = methods;
        VtableGlobal = vtableGlobal;
    }
}