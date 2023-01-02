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

namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    public partial class NeoPanel : Panel
    {
        private Point _lastClick;
        public bool DragControl { get; set; }
        public Form DragForm { get; set; }
        public Color GradientOne { get; set; }
        public Color GradientTwo { get; set; }
        public float GradientAngle { get; set; }
        public Color BorderGradientOne { get; set; }
        public Color BorderGradientTwo { get; set; }
        public float BorderGradientAngle { get; set; }

        public NeoPanel()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint, true);
            GradientAngle = 45F;
            BorderGradientAngle = 0F;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (DragControl)
                _lastClick = new Point(e.X, e.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!DragControl || e.Button != MouseButtons.Left)
                return;

            DragForm.Top += e.Y - _lastClick.Y;
            DragForm.Left += e.X - _lastClick.X;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                using (LinearGradientBrush brush =
                    new LinearGradientBrush(ClientRectangle, GradientOne, GradientTwo, GradientAngle))
                {
                    pe.Graphics.FillRectangle(brush, ClientRectangle);
                }

                using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, BorderGradientOne,
                    BorderGradientTwo, BorderGradientAngle))
                {
                    pe.Graphics.DrawRectangle(new Pen(brush), 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                }
            }
            catch
            {
                //Ignore error
            }

            base.OnPaint(pe);
        }
    }
}
