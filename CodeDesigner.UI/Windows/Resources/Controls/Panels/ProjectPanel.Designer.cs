namespace CodeDesigner.UI.Windows.Resources.Controls.Panels
{
    partial class ProjectPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OpenProjectBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OpenProjectBtn
            // 
            this.OpenProjectBtn.BackColor = System.Drawing.Color.Transparent;
            this.OpenProjectBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.OpenProjectBtn.FlatAppearance.BorderSize = 0;
            this.OpenProjectBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OpenProjectBtn.Font = new System.Drawing.Font("Gilroy-SemiBold", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.OpenProjectBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.OpenProjectBtn.Location = new System.Drawing.Point(312, 8);
            this.OpenProjectBtn.Name = "OpenProjectBtn";
            this.OpenProjectBtn.Size = new System.Drawing.Size(54, 27);
            this.OpenProjectBtn.TabIndex = 0;
            this.OpenProjectBtn.Text = "Open";
            this.OpenProjectBtn.UseVisualStyleBackColor = false;
            this.OpenProjectBtn.Click += new System.EventHandler(this.OpenProjectBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.BackColor = System.Drawing.Color.Transparent;
            this.DeleteBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.DeleteBtn.FlatAppearance.BorderSize = 0;
            this.DeleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeleteBtn.Font = new System.Drawing.Font("Gilroy-SemiBold", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.DeleteBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(69)))), ((int)(((byte)(69)))));
            this.DeleteBtn.Location = new System.Drawing.Point(252, 8);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(54, 27);
            this.DeleteBtn.TabIndex = 1;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.UseVisualStyleBackColor = false;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // ProjectPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.OpenProjectBtn);
            this.Name = "ProjectPanel";
            this.Size = new System.Drawing.Size(374, 199);
            this.ResumeLayout(false);

        }

        #endregion

        private Button OpenProjectBtn;
        private Button DeleteBtn;
    }
}
