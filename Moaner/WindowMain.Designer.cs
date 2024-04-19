
namespace Moaner
{
    partial class WindowMain
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
            this.BToggle = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BToggle
            // 
            this.BToggle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BToggle.Location = new System.Drawing.Point(95, 42);
            this.BToggle.Name = "BToggle";
            this.BToggle.Size = new System.Drawing.Size(75, 23);
            this.BToggle.TabIndex = 0;
            this.BToggle.Text = "Activate";
            this.BToggle.UseVisualStyleBackColor = true;
            this.BToggle.Click += new System.EventHandler(this.BToggle_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "NOTE/BUG: Sometimes it crashes randomly";
            // 
            // WindowMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(262, 105);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BToggle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::Moaner.Properties.Resources.app;
            this.MaximizeBox = false;
            this.Name = "WindowMain";
            this.Text = "Moaner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BToggle;
        private System.Windows.Forms.Label label1;
    }
}