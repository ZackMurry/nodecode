using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Node.Blocks
{
    [Serializable]
    public class Parameter
    {
        public ParameterType Type { get; set; }
        
        // Either RawValue or Reference value must be null
        public string? RawValue { get; set; } // A string representing a primitive literal; ex: "1", "false", "hi", "1.2" (coalesced via Type)
        public BlockBase? ReferenceValue { get; set; } // The type of this is based on Type
        public BlockBase Parent;

        public PointF Coordinates { get; set; }
        public string Name { get; set; }
        public bool Connected { get; set; }
        public bool SecondaryConnected { get; set; }
        public bool NextConnected { get; set; }

        public enum ParameterType
        {
            Void,
            String,
            Bool,
            Double,
            Int,
            Object,
            Next
        }

        public string? ObjectType;
    }
}
