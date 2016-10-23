namespace CPPParser
{
    partial class CPPParserUi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CPPParserUi));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnParse = new System.Windows.Forms.ToolStripButton();
            this.treeOutline = new System.Windows.Forms.TreeView();
            this.imgClass = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnParse});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(405, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnParse
            // 
            this.btnParse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnParse.Image = ((System.Drawing.Image)(resources.GetObject("btnParse.Image")));
            this.btnParse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(23, 22);
            this.btnParse.Text = "Parse C++ File";
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // treeOutline
            // 
            this.treeOutline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeOutline.ImageIndex = 0;
            this.treeOutline.ImageList = this.imgClass;
            this.treeOutline.Location = new System.Drawing.Point(0, 25);
            this.treeOutline.Name = "treeOutline";
            this.treeOutline.SelectedImageIndex = 0;
            this.treeOutline.Size = new System.Drawing.Size(405, 475);
            this.treeOutline.TabIndex = 1;
            // 
            // imgClass
            // 
            this.imgClass.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgClass.ImageStream")));
            this.imgClass.TransparentColor = System.Drawing.Color.Transparent;
            this.imgClass.Images.SetKeyName(0, "cpp.ico");
            this.imgClass.Images.SetKeyName(1, "NameSpace.ico");
            this.imgClass.Images.SetKeyName(2, "classIcon.ico");
            this.imgClass.Images.SetKeyName(3, "Interface.ico");
            this.imgClass.Images.SetKeyName(4, "Template.ico");
            this.imgClass.Images.SetKeyName(5, "MethodsIcon.ico");
            this.imgClass.Images.SetKeyName(6, "Arguments.ico");
            this.imgClass.Images.SetKeyName(7, "Return.ico");
            this.imgClass.Images.SetKeyName(8, "memberVariablesIcon.ico");
            this.imgClass.Images.SetKeyName(9, "private.ico");
            this.imgClass.Images.SetKeyName(10, "Protected.ico");
            this.imgClass.Images.SetKeyName(11, "public.ico");
            // 
            // CPPParserUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 500);
            this.Controls.Add(this.treeOutline);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CPPParserUi";
            this.Text = "CPPParserUi";
            this.Load += new System.EventHandler(this.CPPParserUi_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnParse;
        private System.Windows.Forms.TreeView treeOutline;
        private System.Windows.Forms.ImageList imgClass;
    }
}