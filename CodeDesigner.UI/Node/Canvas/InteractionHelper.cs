using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Interaction.Elements;
using CodeDesigner.UI.Windows.Interaction.Functions;
using CodeDesigner.UI.Windows.Interaction.TextBox;

namespace CodeDesigner.UI.Node.Canvas
{
    public static class InteractionHelper
    {
        public static FunctionDefinitionConfiguration FunctionConfigForm = new ();
        public static TextBoxForm TextBoxDialog = new ();

        public static void LoadFunctionConfig(FunctionDefinition function)
        {
            FunctionConfigForm.LoadFunction(function);
        }

        public static void LoadTextBoxConfig(TextBoxElement e)
        {
            TextBoxDialog.Load(e);
        }
    }
}
