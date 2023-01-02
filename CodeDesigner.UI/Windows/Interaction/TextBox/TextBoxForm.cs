using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeDesigner.UI.Node.Canvas;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Windows.Interaction.TextBox
{
    public partial class TextBoxForm : Form
    {
        private TextBoxElement _e;

        public TextBoxForm()
        {
            InitializeComponent();
        }

        public void Load(TextBoxElement e)
        {
            _e = e;
            textBox1.Text = e.Text;
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _e.Text = textBox1.Text.Replace("\\n", "\n");
            _e.Method?.Invoke();
            Canvas.CanvasControl.Refresh();
            Hide();
        }
    }
}
