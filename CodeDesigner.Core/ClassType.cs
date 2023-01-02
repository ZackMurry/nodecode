using CodeDesigner.Core.ast;

namespace CodeDesigner.Core;

public class ClassType
{
    public string Name;
    public readonly List<ClassType> GenericTypes;

    public ClassType(string name, List<ClassType> genericTypes)
    {
        Name = name;
        GenericTypes = genericTypes;
    }

    public ClassType(string name)
    {
        Name = name;
        GenericTypes = new List<ClassType>();
    }
    
    public string GetGenericName()
    {
        var genericName = Name;
        if (GenericTypes.Count == 0)
        {
            return genericName;
        }

        genericName += "<";
        for (var i = 0; i < GenericTypes.Count; i++)
        {
            genericName += $"{GenericTypes[i].GetGenericName()}";
            if (i + 1 < GenericTypes.Count)
            {
                genericName += ",";
            }
        }

        return genericName + ">";
    }

    public static List<ClassType> ConvertGenericUsage(List<VariableType> genericUsage)
    {
        var genericUsageClass = new List<ClassType>();
        foreach (var vt in genericUsage)
        {
            if (vt.IsPrimitive)
            {
                if (vt.PrimitiveType == null)
                {
                    throw new Exception("expected primitive to contain primitive type");
                }

                if (vt.PrimitiveType == PrimitiveVariableType.INTEGER)
                {
                    genericUsageClass.Add(new ClassType("Integer"));
                } else if (vt.PrimitiveType == PrimitiveVariableType.DOUBLE)
                {
                    genericUsageClass.Add(new ClassType("Double"));
                } else if (vt.PrimitiveType == PrimitiveVariableType.STRING)
                {
                    genericUsageClass.Add(new ClassType("String"));
                } else if (vt.PrimitiveType == PrimitiveVariableType.BOOLEAN)
                {
                    genericUsageClass.Add(new ClassType("Boolean"));
                } else
                {
                    throw new InvalidCodeException("invalid primitive as generic usage");
                }
            }
            else if (vt.ClassType != null)
            {
                genericUsageClass.Add(vt.ClassType);
            }
            else
            {
                throw new Exception("expected object type to have a ClassType");
            }
        }

        return genericUsageClass;
    }

    public static ClassType Of(string name, List<ClassType> genericUsageClass)
    {
        return new ClassType(name, genericUsageClass);
    }
    
}