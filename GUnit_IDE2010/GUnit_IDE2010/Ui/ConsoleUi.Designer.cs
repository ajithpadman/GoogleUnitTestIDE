namespace Gunit.Ui
{
    partial class ConsoleUi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleUi));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConsole = new System.Windows.Forms.ToolStripButton();
            this.btnWarning = new System.Windows.Forms.ToolStripButton();
            this.btnError = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.txtConsole);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1012, 255);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConsole,
            this.btnWarning,
            this.btnError,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1012, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnConsole
            // 
            this.btnConsole.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnConsole.Image = ((System.Drawing.Image)(resources.GetObject("btnConsole.Image")));
            this.btnConsole.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConsole.Name = "btnConsole";
            this.btnConsole.Size = new System.Drawing.Size(24, 24);
            this.btnConsole.Text = "Show Console";
            this.btnConsole.ToolTipText = "Show Console";
            this.btnConsole.Click += new System.EventHandler(this.btnConsole_Click);
            // 
            // btnWarning
            // 
            this.btnWarning.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnWarning.Image = ((System.Drawing.Image)(resources.GetObject("btnWarning.Image")));
            this.btnWarning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnWarning.Name = "btnWarning";
            this.btnWarning.Size = new System.Drawing.Size(24, 24);
            this.btnWarning.Text = "toolStripButton2";
            this.btnWarning.ToolTipText = "Show Warning";
            this.btnWarning.Click += new System.EventHandler(this.btnWarning_Click);
            // 
            // btnError
            // 
            this.btnError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnError.Image = ((System.Drawing.Image)(resources.GetObject("btnError.Image")));
            this.btnError.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnError.Name = "btnError";
            this.btnError.Size = new System.Drawing.Size(24, 24);
            this.btnError.Text = "toolStripButton3";
            this.btnError.ToolTipText = "show Errors";
            this.btnError.Click += new System.EventHandler(this.btnError_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.Location = new System.Drawing.Point(0, 30);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(1012, 225);
            this.txtConsole.TabIndex = 0;
            this.txtConsole.Text = "";
            // 
            // ConsoleUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 255);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.panel1);
            this.Name = "ConsoleUi";
            this.Text = "Console";
            this.Load += new System.EventHandler(this.ConsoleUi_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConsole;
        private System.Windows.Forms.ToolStripButton btnWarning;
        private System.Windows.Forms.ToolStripButton btnError;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}