using System.Drawing.Drawing2D;
using CodeDesigner.UI.Resources.Controls;

namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    public partial class InfoPanel : Panel
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
        public int BorderRadius { get; set; }
        public Color TextColor { get; set; }
        public string Content { get; set; }


        public InfoPanel()
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
            int radius = 8;

            if (BorderRadius != 0)
                radius = BorderRadius;

            try
            {
                Graphics graphics = pe.Graphics;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;


                using (LinearGradientBrush brush =
                    new LinearGradientBrush(ClientRectangle, GradientOne, GradientTwo, GradientAngle))
                {
                    graphics.FillRoundedRectangle(brush, 0, 0, Width, Height, radius);
                    //graphics.FillRoundedRectangle(brush, 12, 12 + ((this.Height - 64) / 2), this.Width - 44, (this.Height - 64)/2, 10);
                }

                using (LinearGradientBrush brush = new LinearGradientBrush(ClientRectangle, BorderGradientOne,
                    BorderGradientTwo, BorderGradientAngle))
                {
                    graphics.DrawRoundedRectangle(new Pen(brush), 0, 0, Width - 1, Height - 1, radius);
                    //pe.Graphics.DrawRectangle(new Pen(brush), 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                }
            }
            catch { }

            if (!string.IsNullOrEmpty(Content))
            {
                StringFormat stringFormat = new();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                pe.Graphics.DrawString(Content, Font, new SolidBrush(TextColor), this.ClientRectangle, stringFormat);
            }

            base.OnPaint(pe);
        }
    }
}
