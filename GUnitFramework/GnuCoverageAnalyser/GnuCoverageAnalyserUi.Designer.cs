namespace GnuCoverageAnalyser
{
    partial class GnuCoverageAnalyserUi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GnuCoverageAnalyserUi));
            this.label1 = new System.Windows.Forms.Label();
            this.txtReportPath = new System.Windows.Forms.TextBox();
            this.btnReportPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtObjects = new System.Windows.Forms.TextBox();
            this.btnObjects = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGcov = new System.Windows.Forms.TextBox();
            this.btnGcov = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Coverage Report Directory";
            // 
            // txtReportPath
            // 
            this.txtReportPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtReportPath.Location = new System.Drawing.Point(186, 41);
            this.txtReportPath.Name = "txtReportPath";
            this.txtReportPath.ReadOnly = true;
            this.txtReportPath.Size = new System.Drawing.Size(554, 22);
            this.txtReportPath.TabIndex = 1;
            // 
            // btnReportPath
            // 
            this.btnReportPath.Location = new System.Drawing.Point(746, 39);
            this.btnReportPath.Name = "btnReportPath";
            this.btnReportPath.Size = new System.Drawing.Size(34, 24);
            this.btnReportPath.TabIndex = 2;
            this.btnReportPath.Text = "...";
            this.btnReportPath.UseVisualStyleBackColor = true;
            this.btnReportPath.Click += new System.EventHandler(this.btnReportPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Objects Directory";
            // 
            // txtObjects
            // 
            this.txtObjects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtObjects.Location = new System.Drawing.Point(186, 82);
            this.txtObjects.Name = "txtObjects";
            this.txtObjects.ReadOnly = true;
            this.txtObjects.Size = new System.Drawing.Size(554, 22);
            this.txtObjects.TabIndex = 1;
            // 
            // btnObjects
            // 
            this.btnObjects.Location = new System.Drawing.Point(746, 80);
            this.btnObjects.Name = "btnObjects";
            this.btnObjects.Size = new System.Drawing.Size(34, 24);
            this.btnObjects.TabIndex = 2;
            this.btnObjects.Text = "...";
            this.btnObjects.UseVisualStyleBackColor = true;
            this.btnObjects.Click += new System.EventHandler(this.btnObjects_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRun});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(889, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRun
            // 
            this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(23, 22);
            this.btnRun.Text = "Run Coverage Analysis";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Path to Gcov";
            // 
            // txtGcov
            // 
            this.txtGcov.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtGcov.Location = new System.Drawing.Point(186, 119);
            this.txtGcov.Name = "txtGcov";
            this.txtGcov.ReadOnly = true;
            this.txtGcov.Size = new System.Drawing.Size(554, 22);
            this.txtGcov.TabIndex = 1;
            // 
            // btnGcov
            // 
            this.btnGcov.Location = new System.Drawing.Point(746, 117);
            this.btnGcov.Name = "btnGcov";
            this.btnGcov.Size = new System.Drawing.Size(34, 24);
            this.btnGcov.TabIndex = 2;
            this.btnGcov.Text = "...";
            this.btnGcov.UseVisualStyleBackColor = true;
            this.btnGcov.Click += new System.EventHandler(this.btnGcov_Click);
            // 
            // GnuCoverageAnalyserUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 179);
            this.CloseButtonVisible = false;
            this.ControlBox = false;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.btnGcov);
            this.Controls.Add(this.btnObjects);
            this.Controls.Add(this.btnReportPath);
            this.Controls.Add(this.txtGcov);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtObjects);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReportPath);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GnuCoverageAnalyserUi";
            this.Text = "GnuCoverageAnalyserUi";
            this.Load += new System.EventHandler(this.GnuCoverageAnalyserUi_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReportPath;
        private System.Windows.Forms.Button btnReportPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtObjects;
        private System.Windows.Forms.Button btnObjects;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGcov;
        private System.Windows.Forms.Button btnGcov;
    }
}