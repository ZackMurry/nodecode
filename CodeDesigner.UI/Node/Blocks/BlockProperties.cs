using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Node.Blocks
{
    [Serializable]
    public struct BlockProperties
    {
        public string Name { get; set; }
        public Parameter.ParameterType OutputType { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Color BorderColor { get; set; }
        public Color SecondaryColor { get; set; }
        public Color FillColor { get; set; }
        public Color TextColor { get; set; }
    }
}
