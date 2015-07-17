namespace GUnit
{
    partial class ActiveFunctions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActiveFunctions));
            this.treeFunctions = new System.Windows.Forms.TreeView();
            this.imgActiveFunctions = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeFunctions
            // 
            this.treeFunctions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFunctions.ImageIndex = 0;
            this.treeFunctions.ImageList = this.imgActiveFunctions;
            this.treeFunctions.Location = new System.Drawing.Point(0, 0);
            this.treeFunctions.Name = "treeFunctions";
            this.treeFunctions.SelectedImageIndex = 0;
            this.treeFunctions.Size = new System.Drawing.Size(282, 255);
            this.treeFunctions.TabIndex = 0;
            this.treeFunctions.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FunctionNode_Click);
            this.treeFunctions.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FunctionNode_Clicked);
            // 
            // imgActiveFunctions
            // 
            this.imgActiveFunctions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgActiveFunctions.ImageStream")));
            this.imgActiveFunctions.TransparentColor = System.Drawing.Color.Transparent;
            this.imgActiveFunctions.Images.SetKeyName(0, "function.ico");
            // 
            // ActiveFunctions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.Add(this.treeFunctions);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ActiveFunctions";
            this.Text = "Function Definitions";
            this.Load += new System.EventHandler(this.Functions_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeFunctions;
        private System.Windows.Forms.ImageList imgActiveFunctions;
    }
}