using CodeDesigner.UI.Node.Blocks;

namespace CodeDesigner.UI.Node.Interaction.Elements;

[Serializable]
public class IconButtonElement : Element
{
    public Color ButtonColor { get; set; }
    public Color BorderColor { get; set; }
    public Color TextColor { get; set; }
    public Action Method { get; set; }
    public Bitmap Image { get; set; }


    public IconButtonElement(ElementProperties properties, Bitmap image, Color color, Color borderColor, Color textColor, Action action)
    {
        Properties = properties;
        ButtonColor = color;
        BorderColor = borderColor;
        TextColor = textColor;
        Method = action;
        Image = image;
    }

    public override void Draw(BlockBase block, Graphics g, float zoom)
    {
        float x = (block.Coordinates.X + Properties.BlockCoordinates.X) * zoom;
        float y = (block.Coordinates.Y + Properties.BlockCoordinates.Y) * zoom;
        float width = Properties.Size.Width * zoom;
        float height = Properties.Size.Height * zoom;

        g.DrawImage(Image, x, y, width, height);
    }
}