using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDesigner.UI.Designer.Toolbox;
using CodeDesigner.UI.Node.Interaction;

namespace CodeDesigner.UI.Node.Blocks
{
    [Serializable]
    public class BlockBase
    {
        public BlockBase? InputBlock; //This is the block that invoked this block.
        public BlockBase? NextBlock; //This is the block that this block is invoking after completion.

        public List<Parameter?> Parameters;
        public List<Parameter> ConnectedParameters;
        [NonSerialized]
        public List<Element> Elements;

        public BlockProperties Properties;

        public PointF Coordinates;

        public PointF[] NextPolygon;
        public PointF[] OutputPolygon;
        public PointF[] SecondaryPolygon;
        
        public bool Connecting;
        public bool SecondaryConnecting;
        public bool NextConnecting;
        public bool UseOutput = true;
        public BlockBase? Output;
        public bool UseSecondaryOutput;
        public BlockBase? SecondaryOutput;
        public bool CanHavePrevious = true;
        public bool CanHaveNext = true;
        public bool IsHighlighted = false;

        public NodeType NodeType = NodeType.DEFAULT;
        public Guid Id = Guid.NewGuid();

        public BlockBase(BlockProperties properties)
        {
            Coordinates = new Point(300, 25);
            Properties = properties;
            Parameters = new List<Parameter?>();
            ConnectedParameters = new List<Parameter>();
            Elements = new();
        }

        public BlockBase(BlockProperties properties, PointF coordinates)
        {
            Coordinates = coordinates;
            Properties = properties;
            Parameters = new List<Parameter?>();
            ConnectedParameters = new List<Parameter>();
            Elements = new();
        }

        public BlockBase()
        {
            Coordinates = new Point(300, 25);
            Parameters = new List<Parameter?>();
            ConnectedParameters = new List<Parameter>();
            Elements = new List<Element>();
        }

        public void CheckNext()
        {
            if (Parameters.Count != 0 && Parameters[0].Type == Parameter.ParameterType.Next)
            {
                Parameters.RemoveAt(0);
            }
            if (CanHavePrevious)
            {
                Parameters.Insert(0, new Parameter
                {
                    Type = Parameter.ParameterType.Next,
                    Parent = this
                });
            }
        }

        public void DestroyConnections()
        {
            foreach (Parameter p in ConnectedParameters)
            {
                p.Connected = false;
                p.SecondaryConnected = false;
                p.NextConnected = false;
                p.ReferenceValue = null;
            }

            ConnectedParameters.Clear();
        }
        
        public virtual void AddElements() {}
    }
}
