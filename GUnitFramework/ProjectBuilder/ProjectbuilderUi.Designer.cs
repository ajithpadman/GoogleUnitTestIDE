namespace ProjectBuilder
{
    partial class ProjectbuilderUi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectbuilderUi));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnBuild = new System.Windows.Forms.ToolStripButton();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chIsInstrumented = new System.Windows.Forms.CheckBox();
            this.cbWarningLevel = new System.Windows.Forms.ComboBox();
            this.cbOutputType = new System.Windows.Forms.ComboBox();
            this.btnBuildDirectory = new System.Windows.Forms.Button();
            this.btnCompilorBrowse = new System.Windows.Forms.Button();
            this.txtBuildDirectory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCompilorPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtConsole = new System.Windows.Forms.RichTextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.toolStrip1.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBuild});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1098, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // btnBuild
            // 
            this.btnBuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnBuild.Image = ((System.Drawing.Image)(resources.GetObject("btnBuild.Image")));
            this.btnBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(23, 22);
            this.btnBuild.Text = "toolStripButton1";
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSettings.Controls.Add(this.tabPage1);
            this.tabSettings.Controls.Add(this.tabPage2);
            this.tabSettings.Enabled = false;
            this.tabSettings.Location = new System.Drawing.Point(0, 28);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(1098, 396);
            this.tabSettings.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tabPage1.Controls.Add(this.chIsInstrumented);
            this.tabPage1.Controls.Add(this.cbWarningLevel);
            this.tabPage1.Controls.Add(this.cbOutputType);
            this.tabPage1.Controls.Add(this.btnBuildDirectory);
            this.tabPage1.Controls.Add(this.btnCompilorBrowse);
            this.tabPage1.Controls.Add(this.txtBuildDirectory);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtCompilorPath);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1090, 367);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            // 
            // chIsInstrumented
            // 
            this.chIsInstrumented.AutoSize = true;
            this.chIsInstrumented.Checked = true;
            this.chIsInstrumented.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chIsInstrumented.Location = new System.Drawing.Point(431, 162);
            this.chIsInstrumented.Name = "chIsInstrumented";
            this.chIsInstrumented.Size = new System.Drawing.Size(238, 21);
            this.chIsInstrumented.TabIndex = 4;
            this.chIsInstrumented.Text = "Instrument for Coverage Analysis";
            this.chIsInstrumented.UseVisualStyleBackColor = true;
            // 
            // cbWarningLevel
            // 
            this.cbWarningLevel.FormattingEnabled = true;
            this.cbWarningLevel.Items.AddRange(new object[] {
            "Abort Compilation on Error",
            "Enable All Error",
            "Check Only Syntax",
            "Treat all warning as Error"});
            this.cbWarningLevel.Location = new System.Drawing.Point(786, 163);
            this.cbWarningLevel.Name = "cbWarningLevel";
            this.cbWarningLevel.Size = new System.Drawing.Size(296, 24);
            this.cbWarningLevel.TabIndex = 3;
            // 
            // cbOutputType
            // 
            this.cbOutputType.FormattingEnabled = true;
            this.cbOutputType.Items.AddRange(new object[] {
            "Console Application",
            "Shared Library"});
            this.cbOutputType.Location = new System.Drawing.Point(124, 160);
            this.cbOutputType.Name = "cbOutputType";
            this.cbOutputType.Size = new System.Drawing.Size(272, 24);
            this.cbOutputType.TabIndex = 3;
            this.cbOutputType.SelectedIndexChanged += new System.EventHandler(this.cbOutputType_SelectedIndexChanged);
            // 
            // btnBuildDirectory
            // 
            this.btnBuildDirectory.Location = new System.Drawing.Point(1003, 104);
            this.btnBuildDirectory.Name = "btnBuildDirectory";
            this.btnBuildDirectory.Size = new System.Drawing.Size(81, 32);
            this.btnBuildDirectory.TabIndex = 2;
            this.btnBuildDirectory.Text = "Browse";
            this.btnBuildDirectory.UseVisualStyleBackColor = true;
            this.btnBuildDirectory.Click += new System.EventHandler(this.btnBuildDirectory_Click);
            // 
            // btnCompilorBrowse
            // 
            this.btnCompilorBrowse.Location = new System.Drawing.Point(1003, 46);
            this.btnCompilorBrowse.Name = "btnCompilorBrowse";
            this.btnCompilorBrowse.Size = new System.Drawing.Size(81, 32);
            this.btnCompilorBrowse.TabIndex = 2;
            this.btnCompilorBrowse.Text = "Browse";
            this.btnCompilorBrowse.UseVisualStyleBackColor = true;
            this.btnCompilorBrowse.Click += new System.EventHandler(this.btnCompilorBrowse_Click);
            // 
            // txtBuildDirectory
            // 
            this.txtBuildDirectory.Location = new System.Drawing.Point(124, 109);
            this.txtBuildDirectory.Name = "txtBuildDirectory";
            this.txtBuildDirectory.Size = new System.Drawing.Size(871, 22);
            this.txtBuildDirectory.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(685, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "WarningLevel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "outputType";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Build Directory";
            // 
            // txtCompilorPath
            // 
            this.txtCompilorPath.Location = new System.Drawing.Point(124, 51);
            this.txtCompilorPath.Name = "txtCompilorPath";
            this.txtCompilorPath.Size = new System.Drawing.Size(871, 22);
            this.txtCompilorPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path to compilor";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1090, 367);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Console";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtConsole);
            this.panel1.Location = new System.Drawing.Point(3, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1081, 216);
            this.panel1.TabIndex = 1;
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.Location = new System.Drawing.Point(0, 0);
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.Size = new System.Drawing.Size(1081, 216);
            this.txtConsole.TabIndex = 0;
            this.txtConsole.Text = "";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(1, 359);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            this.lblStatus.TabIndex = 2;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(0, 430);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1098, 33);
            this.progressBar.TabIndex = 3;
            // 
            // ProjectbuilderUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 499);
            this.CloseButtonVisible = false;
            this.ControlBox = false;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.tabSettings);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1116, 517);
            this.Name = "ProjectbuilderUi";
            this.Text = "Gnu Code Builder";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ProjectbuilderUi_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnBuild;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCompilorBrowse;
        private System.Windows.Forms.TextBox txtCompilorPath;
        private System.Windows.Forms.Button btnBuildDirectory;
        private System.Windows.Forms.TextBox txtBuildDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbOutputType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chIsInstrumented;
        private System.Windows.Forms.ComboBox cbWarningLevel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox txtConsole;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}