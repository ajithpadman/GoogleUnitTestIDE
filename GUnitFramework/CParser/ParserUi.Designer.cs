namespace CParser
{
    partial class ParserUi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParserUi));
            this.parserToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnParse = new System.Windows.Forms.ToolStripButton();
            this.treeOutline = new System.Windows.Forms.TreeView();
            this.imgOutline = new System.Windows.Forms.ImageList(this.components);
            this.parserToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // parserToolStrip
            // 
            this.parserToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnParse});
            this.parserToolStrip.Location = new System.Drawing.Point(0, 0);
            this.parserToolStrip.Name = "parserToolStrip";
            this.parserToolStrip.Size = new System.Drawing.Size(384, 25);
            this.parserToolStrip.TabIndex = 2;
            this.parserToolStrip.Text = "toolStrip1";
            // 
            // btnParse
            // 
            this.btnParse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnParse.Image = ((System.Drawing.Image)(resources.GetObject("btnParse.Image")));
            this.btnParse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(23, 22);
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // treeOutline
            // 
            this.treeOutline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeOutline.ImageIndex = 0;
            this.treeOutline.ImageList = this.imgOutline;
            this.treeOutline.Location = new System.Drawing.Point(0, 25);
            this.treeOutline.Name = "treeOutline";
            this.treeOutline.SelectedImageIndex = 0;
            this.treeOutline.Size = new System.Drawing.Size(384, 612);
            this.treeOutline.TabIndex = 3;
            // 
            // imgOutline
            // 
            this.imgOutline.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgOutline.ImageStream")));
            this.imgOutline.TransparentColor = System.Drawing.Color.Transparent;
            this.imgOutline.Images.SetKeyName(0, "File.ico");
            this.imgOutline.Images.SetKeyName(1, "FunctionIcon.ico");
            this.imgOutline.Images.SetKeyName(2, "Return.ico");
            this.imgOutline.Images.SetKeyName(3, "Arguments.ico");
            this.imgOutline.Images.SetKeyName(4, "GlobalVariableIcon.ico");
            this.imgOutline.Images.SetKeyName(5, "LocalVariable.ico");
            this.imgOutline.Images.SetKeyName(6, "CalledMethodIcon.ico");
            // 
            // ParserUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 637);
            this.CloseButtonVisible = false;
            this.ControlBox = false;
            this.Controls.Add(this.treeOutline);
            this.Controls.Add(this.parserToolStrip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ParserUi";
            this.Text = "ParserUi";
            this.Load += new System.EventHandler(this.ParserUi_Load);
            this.parserToolStrip.ResumeLayout(false);
            this.parserToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip parserToolStrip;
        private System.Windows.Forms.ToolStripButton btnParse;
        private System.Windows.Forms.TreeView treeOutline;
        private System.Windows.Forms.ImageList imgOutline;
    }
}