using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Designer.Toolbox
{
    public enum NodeType
    {
        DEFAULT,
        BINARY_EXPRESSION,
        BOOLEAN_EXPRESSION,
        CLASS_DEFINITION,
        CLASS_FIELD_ACCESS,
        CLASS_FIELD_STORE,
        CLASS_INSTANTIATION,
        FUNCTION_DEFINITION,
        FUNCTION_INVOCATION,
        IF_STATEMENT,
        METHOD_INVOCATION,
        NUMBER_EXPRESSION,
        PROTOTYPE_DECLARATION,
        RETURN,
        STRING_EXPRESSION,
        VARIABLE_ASSIGNMENT,
        VARIABLE_DECLARATION,
        VARIABLE_DEFINITION,
        VARIABLE_EXPRESSION,
        WHILE_LOOP
    }
}
