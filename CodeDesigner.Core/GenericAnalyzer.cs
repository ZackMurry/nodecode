using CodeDesigner.Core.ast;

namespace CodeDesigner.Core;

public class GenericAnalyzer : IAnalyzer
{

    private readonly Dictionary<string, ASTClassDefinition> _classDefs = new();
    private readonly Dictionary<string, Dictionary<string, List<int>>> _classLinks = new ();

    private readonly HashSet<string> _nodesTypes = new()
    {
        "ASTClassDefinition",
        "ASTClassInstantiation",
        "ASTVariableDefinition",
        "ASTVariableDeclaration"
    };
    
    public bool ShouldAnalyzeNode(ASTNode astNode)
    {
        return _nodesTypes.Contains(astNode.GetType().Name);
    }
    
    private void AddGenericUsageIfNotPresent(string classTypeName, List<ClassType> genericUsage, string? parentClass)
    {
        if (!_classDefs.ContainsKey(classTypeName))
        {
            Console.WriteLine("Skipping generic usages for class type " + classTypeName);
            return;
        }

        if (parentClass != null)
        {
            var pc = _classDefs[parentClass];
            for (int i = 0; i < genericUsage.Count; i++)
            {
                foreach (var gt in pc.ClassType.GenericTypes)
                {
                    if (genericUsage[i].Name.Equals(gt.Name))
                    {
                        // This could be actually implemented to allow for more complex generics, but I'm limiting
                        // the language to simple generics just so that lists (etc) can be implemented -- who is writing
                        // complex generics code with block code???
                        return;
                    }
                }
            }
        }
        foreach (var gu in _classDefs[classTypeName].GenericUsages)
        {
            if (genericUsage.Count != gu.Count)
            {
                continue;
            }

            int i = 0;
            for (; i < genericUsage.Count; i++)
            {
                if (!gu[i].GetGenericName().Equals(genericUsage[i].GetGenericName()))
                {
                    i = -1;
                    break;
                }
            }

            if (i != -1)
            {
                return;
            }
        }
        _classDefs[classTypeName].GenericUsages.Add(genericUsage);
        if (_classLinks.ContainsKey(classTypeName))
        {
            foreach (var (linkedClass, linkedGenerics) in _classLinks[classTypeName])
            {
                Console.WriteLine("propagating generic usage to " + linkedClass);
                var relevantGenerics = new List<ClassType>();
                foreach (var i in linkedGenerics)
                {
                    relevantGenerics.Add(genericUsage[i]);
                }
                AddGenericUsageIfNotPresent(linkedClass, relevantGenerics, parentClass);
            }
        }
    }

    public void Analyze(ASTNode node, string currentNamespace, string? parentClass)
    {
        switch (node.GetType().Name)
        {
            case "ASTClassDefinition":
            {
                var classDef = (ASTClassDefinition) node;
                _classDefs.Add($"{currentNamespace}.{classDef.ClassType.Name}", classDef);
                foreach (var field in classDef.Fields)
                {
                    Console.WriteLine("analyzing field " + field.Name + " of type " + (!field.Type.IsPrimitive ? field.Type.ClassType!.Name : field.Type.PrimitiveType.ToString()));
                    Console.WriteLine(classDef.ClassType.Name);
                    if (field.Type.IsPrimitive || field.Type.ClassType == null ||
                        field.Type.ClassType.Name.Equals(classDef.ClassType.Name))
                    {
                        continue;
                    }
                    foreach (var t in field.Type.ClassType.GenericTypes)
                    {
                        for (var i = 0; i < classDef.ClassType.GenericTypes.Count; i++)
                        {
                            var gt = classDef.ClassType.GenericTypes[i];
                            if (t.Name.Equals(gt.Name))
                            {
                                if (_classLinks.ContainsKey(classDef.ClassType.Name) &&
                                    _classLinks[classDef.ClassType.Name].ContainsKey(field.Type.ClassType.Name))
                                {
                                    _classLinks[classDef.ClassType.Name][field.Type.ClassType.Name].Add(i);
                                } else if (_classLinks.ContainsKey(classDef.ClassType.Name))
                                {
                                    var list = new List<int>();
                                    list.Add(i);
                                    _classLinks[classDef.ClassType.Name].Add(field.Type.ClassType.Name, list);
                                }
                                else
                                {
                                    var list = new List<int>();
                                    list.Add(i);
                                    var dict = new Dictionary<string, List<int>>();
                                    dict.Add(field.Type.ClassType.Name, list);
                                    _classLinks.Add(classDef.ClassType.Name, dict);
                                }
                                Console.WriteLine("added linked class");
                            }
                        }
                    }
                }
                break;
            }
            case "ASTClassInstantiation":
            {
                var classInst = (ASTClassInstantiation) node;
                var classTypeName = classInst.ClassType.Name.Contains('.')
                    ? classInst.ClassType.Name
                    : $"default.{classInst.ClassType.Name}";
                AddGenericUsageIfNotPresent(classTypeName, classInst.ClassType.GenericTypes, parentClass);
                break;
            }
            case "ASTVariableDefinition":
            {
                var varDef = (ASTVariableDefinition) node;
                if (varDef.Type.IsPrimitive || varDef.Type.ClassType == null)
                {
                    break;
                }
                var classTypeName = varDef.Type.ClassType.Name.Contains('.')
                    ? varDef.Type.ClassType.Name
                    : $"default.{varDef.Type.ClassType.Name}";
                AddGenericUsageIfNotPresent(classTypeName, varDef.Type.ClassType.GenericTypes, parentClass);
                break;
            }
            case "ASTVariableDeclaration":
            {
                var varDecl = (ASTVariableDeclaration) node;
                if (varDecl.Type.IsPrimitive || varDecl.Type.ClassType == null)
                {
                    break;
                }
                var classTypeName = varDecl.Type.ClassType.Name.Contains('.')
                    ? varDecl.Type.ClassType.Name
                    : $"default.{varDecl.Type.ClassType.Name}";
                AddGenericUsageIfNotPresent(classTypeName, varDecl.Type.ClassType.GenericTypes, parentClass);
                break;
            }
        }
    }

    public void Finalize(List<ASTNode> ast)
    {
        
    }
}