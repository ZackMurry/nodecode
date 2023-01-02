using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Node.Canvas;
using CodeDesigner.UI.Utility.Project;
using CodeRunner.UI;

namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    public partial class ProjectPanel : UserControl
    {
        public float Scale = 1;

        public NodeMap? Map = null;

        public ProjectPanel()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (Map != null)
            {
                try
                {
                    pe.Graphics.DrawImage(ResizeImage(Map.Thumbnail, Width, Height), new Point(0, 0));
                }
                catch
                {
                    //Nothing
                }

                pe.Graphics.DrawString(Map.Name, new Font("Gilroy-SemiBold", 13 * Scale, FontStyle.Regular, GraphicsUnit.Point), new SolidBrush(Color.FromArgb(69,69,69)), new PointF(8 * Scale, 8 * Scale));
            }
            
            base.OnPaint(pe);
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width,image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public void Load(NodeMap map)
        {
            Map = map;
            Scale = 1;
            Refresh();
        }

        private void OpenProjectBtn_Click(object sender, EventArgs e)
        {
            Canvas.Blocks = Map.Blocks;
            Program.dash = new Dashboard();
            Program.dash.Map = Map;
            Program.pm.Hide();
            Program.dash.Closed += (s, args) => Program.pm.Close();
            Program.dash.Show();
        }
        
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deleted projects cannot be recovered! Are you sure you want to delete this project?", 
                "Delete Project", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, 
                MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.Yes)
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\" + Map.Name + ".nodecode");
            }

            Program.pm.LoadProjects();
        }
    }
}
