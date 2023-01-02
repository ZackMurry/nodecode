using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accessibility;
using CodeDesigner.UI.Node.Canvas;
using CodeRunner.UI;

namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    public partial class CanvasPanel : Panel
    {
        public float ZoomFactor = 1;
        public float OffsetY = 0;
        public float OffsetX = 0;
        
        public CanvasPanel()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            DrawGrid(pe, Size);
            
            if (!DesignMode)
            {
                RenderEngine.DrawBlocks(pe.Graphics, new PointF(OffsetX, OffsetY), Size);
            }

            base.OnPaint(pe);
        }

        public void DrawGrid(PaintEventArgs pe, Size size)
        {
            float width = Size.Width;
            float height = Size.Height;

            float x = (OffsetX % (20 * ZoomFactor));
            float y = (OffsetY % (20 * ZoomFactor));
            
            Graphics g = pe.Graphics;

            Pen pen = new (Color.FromKnownColor(KnownColor.ControlLight));
            
            while (x < width)
            {
                g.DrawLine(pen, x, 0, x, height);
                x += (20 * ZoomFactor);
            }

            while (y < height)
            {
                g.DrawLine(pen, 0, y, width, y);
                y += (20 * ZoomFactor);
            }
        }
    }
}
