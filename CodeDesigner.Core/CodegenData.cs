using LLVMSharp;

namespace CodeDesigner.Core;

public class CodegenData
{
    public LLVMBuilderRef Builder;
    public LLVMContextRef Context;
    public LLVMValueRef? Func;
    public Dictionary<string, LLVMValueRef> NamedValues;
    public LLVMModuleRef Module;
    public string NamespaceName;
    public Dictionary<string, ClassData> Classes;
    public Dictionary<string, string> ObjectTypes;
    public Dictionary<string, string> Generics;
    public List<ErrorDescription> Errors;

    public CodegenData(LLVMBuilderRef builder, LLVMContextRef context, LLVMValueRef? func,
        LLVMModuleRef module, string namespaceName)
    {
        Builder = builder;
        Context = context;
        Func = func;
        NamedValues = new Dictionary<string, LLVMValueRef>();
        Module = module;
        NamespaceName = namespaceName;
        Classes = new Dictionary<string, ClassData>();
        ObjectTypes = new Dictionary<string, string>();
        Generics = new Dictionary<string, string>();
        Errors = new List<ErrorDescription>();
    }
    
}