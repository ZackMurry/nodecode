using CodeDesigner.UI.Node.Blocks;

namespace CodeDesigner.UI.Node.Interaction
{
    [Serializable]
    public abstract class Element
    {
        public ElementProperties Properties;
        public bool IsClickedOn = false;

        public abstract void Draw(BlockBase block, Graphics g, float zoom);
    }
}
