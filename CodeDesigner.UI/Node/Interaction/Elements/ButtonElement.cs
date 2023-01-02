using System.Xml.Serialization;
using CodeDesigner.UI.Node.Blocks;

namespace CodeDesigner.UI.Node.Interaction.Elements
{
    [Serializable]
    public class ButtonElement : Element
    {
        public Color ButtonColor { get; set; }
        public Color BorderColor { get; set; }
        public Color TextColor { get; set; }
        public Action Method { get; set; }
        public string ButtonText { get; set; }


        public ButtonElement(ElementProperties properties, string text, Color color, Color borderColor, Color textColor, Action action)
        {
            Properties = properties;
            ButtonColor = color;
            BorderColor = borderColor;
            TextColor = textColor;
            Method = action;
            ButtonText = text;
        }

        public override void Draw(BlockBase block, Graphics g, float zoom)
        {
            float x = (block.Coordinates.X + Properties.BlockCoordinates.X) * zoom;
            float y = (block.Coordinates.Y + Properties.BlockCoordinates.Y) * zoom;
            float width = Properties.Size.Width * zoom;
            float height = Properties.Size.Height * zoom;

            g.DrawRectangle(new Pen(BorderColor, 1 * zoom), x, y, width, height);
            g.FillRectangle(new SolidBrush(ButtonColor), x, y, width, height);
            // g.DrawImage(CodeDesigner.UI.Properties.Resources.);

            Rectangle rect = new ((int)x, (int)y, (int)width, (int)height);

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.WordBreak;

            TextRenderer.DrawText(g, ButtonText, new Font("Gilroy-Bold", 9f * zoom, FontStyle.Regular, GraphicsUnit.Point), rect, Color.White, flags);
        }
    }
}
