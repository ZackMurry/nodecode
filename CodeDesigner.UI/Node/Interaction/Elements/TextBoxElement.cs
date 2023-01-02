using CodeDesigner.UI.Node.Blocks;

namespace CodeDesigner.UI.Node.Interaction.Elements
{
    [Serializable]
    public class TextBoxElement : Element
    {
        public string Text = string.Empty;
        public Action? Method;
        private Color _color;
        private Color _buttonColor;
        private Color _strokeColor;

        public TextBoxElement(ElementProperties properties, Color color, Color buttonColor, Color stroke, Action? method)
        {
            Properties = properties;
            _color = color;
            _buttonColor = buttonColor;
            _strokeColor = stroke;
            Method = method;
        }
        
        public override void Draw(BlockBase block, Graphics g, float zoom)
        {
            float x = (block.Coordinates.X + Properties.BlockCoordinates.X) * zoom;
            float y = (block.Coordinates.Y + Properties.BlockCoordinates.Y) * zoom;
            float width = Properties.Size.Width * zoom;
            float height = Properties.Size.Height * zoom;

            g.FillRectangle(new SolidBrush(_color), x, y, width, height);

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.WordBreak;

            TextRenderer.DrawText(g, Text, new Font("Gilroy-Bold", 9f * zoom, FontStyle.Regular, GraphicsUnit.Point), new Rectangle((int)x, (int)y, (int)width, (int)height), Color.White, flags);

            //g.FillRectangle(new SolidBrush());
        }
    }
}
