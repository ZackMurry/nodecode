using CodeDesigner.UI.Node.Blocks;
using CodeDesigner.UI.Node.Blocks.Nodes;
using CodeDesigner.UI.Node.Blocks.Types;
using CodeDesigner.UI.Node.Canvas;
using CodeDesigner.UI.Node.Interaction;
using CodeDesigner.UI.Node.Interaction.Elements;

namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    public partial class CorePanel : UserControl
    {
        public CorePanel()
        {
            InitializeComponent();
        }

        private void CorePanel_Load(object sender, EventArgs e)
        {

        }

        private void NewFunction_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new FunctionDefinition());
        }

        private void InvokeFunction_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new FunctionInvocation());
        }

        private void TestExpression_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new VariableExpression());
            /*BlockProperties properties = new BlockProperties();
            properties.Height = 100;
            properties.OutputType = Parameter.ParameterType.Object;
            properties.Width = 300;
            properties.TextColor = Color.White;
            properties.Name = "Test Node";
            properties.FillColor = Color.FromArgb(85,85,85);
            properties.SecondaryColor = Color.FromArgb(69,69,69);
            properties.BorderColor = Color.FromArgb(69,69,69);
            BlockBase? blockbase = new (properties);
            blockbase.Parameters.Add(new Parameter()
            {
                Type = Parameter.ParameterType.Object,
                Connected = false,
                Name = "Random Variable 1"
            });
            blockbase.Parameters.Add(new Parameter()
            {
                Type = Parameter.ParameterType.String,
                Connected = false,
                Name = "Random Variable 2"
            });

            ElementProperties elementProperties = new()
            {
                BlockCoordinates = new PointF(30, 30),
                Size = new SizeF(100, 50)
            };

            Element textbox =
                new TextBoxElement(elementProperties, Color.LightGray, Color.Gray, Color.White, () => MessageBox.Show("button clicked"));

            blockbase.UseSecondaryOutput = true;

            ((TextBoxElement)textbox).Text = "Test String lalala";

            blockbase.Elements.Add(textbox);
            Canvas.AddNode(blockbase);*/
        }

        private void BinaryExpression_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new BinaryExpression());
        }

        private void StringExpression_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new StringExpression());
        }

        private void ReturnExpression_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new ReturnExpression());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new IfStatement());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Canvas.AddNode(new WhileLoop());
        }
    }
}
