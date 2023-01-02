using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Utility.Project;
using CodeRunner.UI;

namespace CodeDesigner.UI.Windows
{
    public partial class NameProject : Form
    {
        public NameProject()
        {
            InitializeComponent();
        }

        private void CreateBtn_Click(object sender, EventArgs e)
        {
            NodeMap map = new()
            {
                Blocks = new List<BlockBase>(),
                Name = ProjectNameTxt.Text,
                Thumbnail = Properties.Resources.template
            };

            ProjectUtil.Save(map.Blocks, map.Name, (Bitmap)map.Thumbnail);
            Program.pm.LoadProjects();
            ProjectNameTxt.Text = "";
            Hide();
        }
    }
}
