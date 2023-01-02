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
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Canvas;

namespace CodeDesigner.UI.Windows.Interaction.Functions
{
    public partial class FunctionDefinitionConfiguration : Form
    {
        public FunctionDefinition Function;

        public FunctionDefinitionConfiguration()
        {
            InitializeComponent();
        }

        public void LoadFunction(FunctionDefinition function)
        {
            listView1.Items.Clear();
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;
            textBox4.Text = string.Empty;
            comboBox1.SelectedItem = "Void";
            Function = function;

            foreach (Parameter p in function.Parameters)
            {
                AddParamToList(p.Name, p.ObjectType);
            }

            textBox2.Text = function.Name;

            bool found = false;
            string name = string.Empty;
            foreach (string s in comboBox1.Items)
            {
                if (s != function.ObjectReturnType) continue;
                found = true;
                name = s;
            }

            if (!found && name != string.Empty)
            {
                checkBox1.Checked = true;
                textBox3.Text = name;
                textBox3.ReadOnly = false;
            }
            else
            {
                checkBox1.Checked = false;
                comboBox1.SelectedItem = name;
            }


        }

        private void FunctionDefinitionConfiguration_Load(object sender, EventArgs e) { }

        private void AddParamToList(string name, string type)
        {
            ListViewItem lvi = new(new[] { name, type });
            listView1.Items.Add(lvi);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.ReadOnly = false;
                comboBox1.Enabled = false;
            }
            else
            {
                textBox3.ReadOnly = true;
                comboBox1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddParamToList(textBox1.Text, textBox4.Text);
        }

        private void deleteParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Canvas.FunctionData.Remove(Function.Name);
            Function.Name = textBox2.Text;
            Function.Properties.Name = "Function " + Function.Name;
            Function.Parameters.Clear();


            foreach (ListViewItem lvi in listView1.Items)
            {
                Parameter.ParameterType type = ValidType(lvi.SubItems[1].Text);

                Parameter p = new()
                {
                    Name = lvi.SubItems[0].Text,
                    ObjectType = lvi.SubItems[1].Text,
                    Type = type
                };
                
                Function.Parameters.Add(p);
            }

            Function.ObjectReturnType = checkBox1.Checked ? textBox3.Text : comboBox1.SelectedItem.ToString();
            Function.ReturnType = checkBox1.Checked ? ValidType(textBox3.Text) : ValidType(comboBox1.SelectedItem.ToString());
            Canvas.FunctionData[Function.Name] = new FunctionInformation(Function.Parameters, Function.ReturnType);

            Canvas.CanvasControl.Refresh();

            Hide();
        }

        private Parameter.ParameterType ValidType(string type)
        {
            switch (type.ToLower())
            {
                case "int":
                    return Parameter.ParameterType.Int;
                case "double":
                    return Parameter.ParameterType.Double;
                case "string":
                    return Parameter.ParameterType.String;
                case "bool":
                    return Parameter.ParameterType.Bool;
                case "object":
                    return Parameter.ParameterType.Double;
                case "void":
                    return Parameter.ParameterType.Void;
                default:
                    return Parameter.ParameterType.Object;
            }
        }

        private void FunctionDefinitionConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
