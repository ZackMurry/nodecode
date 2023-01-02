using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeDesigner.UI.Windows.Resources.Controls.Buttons
{
    public partial class PictureBoxButton : PictureBox
    {
        public double ScaleFactorOnHover { get; set; }

        private Point oldSize;

        public PictureBoxButton()
        {
            InitializeComponent();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            oldSize = new Point(Size);

            Size = new Size((int)(oldSize.X * ScaleFactorOnHover), (int)(oldSize.Y * ScaleFactorOnHover));

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Size = (Size)oldSize;
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
