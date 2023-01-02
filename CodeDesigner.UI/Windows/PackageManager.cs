using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Utility.Project;
using CodeRunner.UI;

namespace CodeDesigner.UI.Windows
{
    public partial class PackageManager : Form
    {
        public PackageManager()
        {
            InitializeComponent();
        }

        private void PackageManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = new WebClient().DownloadData(
                $"https://ncpm.zackmurry.com/api/v1/packages/name/{richTextBox1.Text}/versions/{richTextBox2.Text}/raw");

            using (MemoryStream ms = new (data))
            {
                var nodeMap = (NodeMap) new BinaryFormatter().Deserialize(ms);
                Program.dash.Map.Dependencies.Add(nodeMap);
                nodeMap.ScanForFunctions();
            }

            listBox1.Items.Clear();
            NodeMap map = Program.dash.Map;

            foreach (NodeMap pack in map.Dependencies)
            {
                listBox1.Items.Add(pack.Name);
            }

        }

        private void PackageManager_Shown(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            NodeMap map = Program.dash.Map;

            foreach (NodeMap pack in map.Dependencies)
            {
                listBox1.Items.Add(pack.Name);
            }
        }

        private void deletePackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (NodeMap map in Program.dash.Map.Dependencies)
            {
                if (map.Name == listBox1.SelectedItem.ToString())
                {
                    Program.dash.Map.Dependencies.Remove(map);
                    listBox1.Items.Remove(listBox1.SelectedItem);
                }
            }
        }
    }
}
