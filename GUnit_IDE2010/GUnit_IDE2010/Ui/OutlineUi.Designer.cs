namespace Gunit.Ui
{
    partial class OutlineUi
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutlineUi));
            this.treeFunctions = new System.Windows.Forms.TreeView();
            this.imgFunctions = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeFunctions
            // 
            this.treeFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeFunctions.ImageIndex = 0;
            this.treeFunctions.ImageList = this.imgFunctions;
            this.treeFunctions.Location = new System.Drawing.Point(1, 30);
            this.treeFunctions.Name = "treeFunctions";
            this.treeFunctions.SelectedImageIndex = 0;
            this.treeFunctions.Size = new System.Drawing.Size(281, 247);
            this.treeFunctions.TabIndex = 0;
            this.treeFunctions.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FunctionsUi_NodeDoubleClick);
            // 
            // imgFunctions
            // 
            this.imgFunctions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFunctions.ImageStream")));
            this.imgFunctions.TransparentColor = System.Drawing.Color.Transparent;
            this.imgFunctions.Images.SetKeyName(0, "File.ico");
            this.imgFunctions.Images.SetKeyName(1, "GlobalVariableIcon.ico");
            this.imgFunctions.Images.SetKeyName(2, "FunctionIcon.ico");
            this.imgFunctions.Images.SetKeyName(3, "CalledMethodIcon.ico");
            this.imgFunctions.Images.SetKeyName(4, "NamespaceIcon.ico");
            this.imgFunctions.Images.SetKeyName(5, "classIcon.ico");
            this.imgFunctions.Images.SetKeyName(6, "memberVariablesIcon.ico");
            this.imgFunctions.Images.SetKeyName(7, "MethodsIcon.ico");
            this.imgFunctions.Images.SetKeyName(8, "TypedefIcon.ico");
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(282, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(24, 24);
            this.btnRefresh.Text = "toolStripButton1";
            this.btnRefresh.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // FunctionsUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 276);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.treeFunctions);
            this.Name = "FunctionsUi";
            this.Text = "Project Outline";
            this.Load += new System.EventHandler(this.FunctionsUi_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeFunctions;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ImageList imgFunctions;
    }
}