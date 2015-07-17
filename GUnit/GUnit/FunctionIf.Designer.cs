namespace GUnit
{
    partial class FunctionIF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionIF));
            this.treeClasses = new System.Windows.Forms.TreeView();
            this.imgLstClasses = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeClasses
            // 
            this.treeClasses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeClasses.ImageIndex = 0;
            this.treeClasses.ImageList = this.imgLstClasses;
            this.treeClasses.Location = new System.Drawing.Point(0, 0);
            this.treeClasses.Name = "treeClasses";
            this.treeClasses.SelectedImageIndex = 0;
            this.treeClasses.Size = new System.Drawing.Size(282, 255);
            this.treeClasses.TabIndex = 0;
            this.treeClasses.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FunctionIf_NodeClick);
            this.treeClasses.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FunctionIf_CorrectFunctionDoubleClick);
            // 
            // imgLstClasses
            // 
            this.imgLstClasses.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstClasses.ImageStream")));
            this.imgLstClasses.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstClasses.Images.SetKeyName(0, "Database.ico");
            this.imgLstClasses.Images.SetKeyName(1, "Classes.ico");
            this.imgLstClasses.Images.SetKeyName(2, "private.ico");
            this.imgLstClasses.Images.SetKeyName(3, "Public.ico");
            this.imgLstClasses.Images.SetKeyName(4, "Global.ico");
            this.imgLstClasses.Images.SetKeyName(5, "GlobalFunctions.ico");
            this.imgLstClasses.Images.SetKeyName(6, "VirtualPrivate.ico");
            this.imgLstClasses.Images.SetKeyName(7, "VirtualPublic.ico");
            // 
            // FunctionIF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.Add(this.treeClasses);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FunctionIF";
            this.Text = "Functional Interfaces";
            this.Load += new System.EventHandler(this.FunctionIF_Load_1);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeClasses;
        private System.Windows.Forms.ImageList imgLstClasses;
    }
}