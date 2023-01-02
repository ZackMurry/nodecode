namespace CodeDesigner.UI.Windows
{
    partial class ProjectManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.neoPanel1 = new CodeDesigner.UI.Windows.Resources.Controls.Panels.NeoPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ProjectsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.CreateNewProject = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.neoPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // neoPanel1
            // 
            this.neoPanel1.BorderGradientAngle = 0F;
            this.neoPanel1.BorderGradientOne = System.Drawing.Color.Empty;
            this.neoPanel1.BorderGradientTwo = System.Drawing.Color.Empty;
            this.neoPanel1.Controls.Add(this.label6);
            this.neoPanel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.neoPanel1.DragControl = false;
            this.neoPanel1.DragForm = null;
            this.neoPanel1.GradientAngle = 45F;
            this.neoPanel1.GradientOne = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
            this.neoPanel1.GradientTwo = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(123)))), ((int)(((byte)(213)))));
            this.neoPanel1.Location = new System.Drawing.Point(0, 0);
            this.neoPanel1.Name = "neoPanel1";
            this.neoPanel1.Size = new System.Drawing.Size(51, 450);
            this.neoPanel1.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Gilroy-Black", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(1, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 34);
            this.label6.TabIndex = 1;
            this.label6.Text = "NC";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ProjectsPanel);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(51, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(749, 450);
            this.panel1.TabIndex = 9;
            // 
            // ProjectsPanel
            // 
            this.ProjectsPanel.ColumnCount = 2;
            this.ProjectsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ProjectsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ProjectsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectsPanel.Location = new System.Drawing.Point(1, 53);
            this.ProjectsPanel.Name = "ProjectsPanel";
            this.ProjectsPanel.RowCount = 2;
            this.ProjectsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ProjectsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ProjectsPanel.Size = new System.Drawing.Size(748, 397);
            this.ProjectsPanel.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(1, 52);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(748, 1);
            this.panel4.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.CreateNewProject);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(1, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(748, 52);
            this.panel3.TabIndex = 9;
            // 
            // CreateNewProject
            // 
            this.CreateNewProject.BackColor = System.Drawing.Color.Transparent;
            this.CreateNewProject.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.CreateNewProject.FlatAppearance.BorderSize = 0;
            this.CreateNewProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreateNewProject.Font = new System.Drawing.Font("Gilroy-SemiBold", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CreateNewProject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.CreateNewProject.Location = new System.Drawing.Point(619, 13);
            this.CreateNewProject.Name = "CreateNewProject";
            this.CreateNewProject.Size = new System.Drawing.Size(119, 27);
            this.CreateNewProject.TabIndex = 8;
            this.CreateNewProject.Text = "Create New Project";
            this.CreateNewProject.UseVisualStyleBackColor = false;
            this.CreateNewProject.Click += new System.EventHandler(this.CreateNewProject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Gilroy-SemiBold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 33);
            this.label1.TabIndex = 7;
            this.label1.Text = "Project Manager";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1, 450);
            this.panel2.TabIndex = 8;
            // 
            // ProjectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.neoPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectManager";
            this.Text = "Project Manager";
            this.Load += new System.EventHandler(this.ProjectManager_Load);
            this.neoPanel1.ResumeLayout(false);
            this.neoPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Resources.Controls.Panels.NeoPanel neoPanel1;
        private Label label6;
        private Panel panel1;
        private Label label1;
        private Panel panel4;
        private Panel panel3;
        private Panel panel2;
        private TableLayoutPanel ProjectsPanel;
        private Button CreateNewProject;
    }
}