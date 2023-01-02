using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.Interface.Project
{
    [Serializable]
    public class DesignerProject
    {
        public readonly string Name;

        public DesignerProject(string name)
        {
            Name = name;
        }
    }
}
