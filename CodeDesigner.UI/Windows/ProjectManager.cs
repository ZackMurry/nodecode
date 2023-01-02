using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using CodeDesigner.UI.Node.Canvas;
using CodeDesigner.UI.Utility.Project;
using CodeDesigner.UI.Windows.Resources.Controls.Panels;

namespace CodeDesigner.UI.Windows
{
    public partial class ProjectManager : Form
    {
        private bool loaded = false;
        private NameProject nameProject;
        
        public ProjectManager()
        {
            InitializeComponent();
            nameProject = new NameProject();
        }

        public void LoadProjects()
        {
            if (loaded)
            {
                ProjectsPanel.Controls.Clear();
            }
            
            NodeMap map;
            int c = 0;
            int r = 0;
            
            foreach (string s in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)))
            {
                if (s.EndsWith(".nodecode"))
                {
                    using (FileStream fs = new FileStream(s, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        map = (NodeMap)formatter.Deserialize(fs);
                        map.Blocks.ForEach(b => b.AddElements());
                        map.ScanForFunctions();
                        Canvas.NodeMap = map;
                    }

                    ProjectPanel panel = new ProjectPanel();
                    panel.Load(map);
                    panel.Dock = DockStyle.Fill;
                    ProjectsPanel.Controls.Add(panel, c, r);

                    if (c == 1 && r == 1)
                        break;
                    
                    if (c == 0)
                    {
                        c++;
                    }
                    else
                    {
                        r++;
                        c = 0;
                    }
                }
            }

            loaded = true;
        }

        private void ProjectManager_Load(object sender, EventArgs e)
        {
            LoadProjects();
        }

        private void CreateNewProject_Click(object sender, EventArgs e)
        {
            nameProject.Show();
        }
    }
}
