using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDesigner.UI.Resources.Controls
{
    public static class GraphicsExtensions
    {
        private static GraphicsPath GenerateRoundedRectangle(this Graphics graphics, RectangleF rectangle, float radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0.0F)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }

            if (radius >= Math.Min(rectangle.Width, rectangle.Height) / 2.0)
                return graphics.GenerateCapsule(rectangle);
            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(rectangle.Location, sizeF);
            path.AddArc(arc, 180, 90);
            arc.X = rectangle.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        private static GraphicsPath GenerateCapsule(this Graphics graphics, RectangleF baseRect)
        {
            GraphicsPath path = new GraphicsPath();
            try
            {
                float diameter;
                RectangleF arc;
                if (baseRect.Width > baseRect.Height)
                {
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    path.AddEllipse(baseRect);
                }
            }
            catch
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }

            return path;
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, float x, float y, float width,
            float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = old;
        }

        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, int x, int y, int width, int height,
            int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }

        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, float x, float y, float width,
            float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.SmoothingMode = old;
        }

        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, int x, int y, int width,
            int height, int radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }
    }
}
