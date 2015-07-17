namespace GUnit
{
    partial class DataIf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataIf));
            this.treeDataType = new System.Windows.Forms.TreeView();
            this.imgLstDataTypes = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeDataType
            // 
            this.treeDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeDataType.ImageIndex = 0;
            this.treeDataType.ImageList = this.imgLstDataTypes;
            this.treeDataType.Location = new System.Drawing.Point(0, 0);
            this.treeDataType.Name = "treeDataType";
            this.treeDataType.SelectedImageIndex = 0;
            this.treeDataType.Size = new System.Drawing.Size(282, 255);
            this.treeDataType.TabIndex = 0;
            // 
            // imgLstDataTypes
            // 
            this.imgLstDataTypes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstDataTypes.ImageStream")));
            this.imgLstDataTypes.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstDataTypes.Images.SetKeyName(0, "Database.ico");
            this.imgLstDataTypes.Images.SetKeyName(1, "userTypes.ico");
            this.imgLstDataTypes.Images.SetKeyName(2, "structure.ico");
            this.imgLstDataTypes.Images.SetKeyName(3, "class.ico");
            this.imgLstDataTypes.Images.SetKeyName(4, "Enumeration.ico");
            this.imgLstDataTypes.Images.SetKeyName(5, "Macro.ico");
            this.imgLstDataTypes.Images.SetKeyName(6, "Global.ico");
            // 
            // DataIf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.Add(this.treeDataType);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DataIf";
            this.Text = "Data interfaces";
            this.Load += new System.EventHandler(this.DataTypes_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeDataType;
        private System.Windows.Forms.ImageList imgLstDataTypes;
    }
}