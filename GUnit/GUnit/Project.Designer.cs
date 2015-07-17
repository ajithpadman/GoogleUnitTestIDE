namespace GUnit
{
    partial class Project
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Project));
            this.imgLstProject = new System.Windows.Forms.ImageList(this.components);
            this.treeProject = new System.Windows.Forms.TreeView();
            this.ContextMenu1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCoverageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextProject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextCommon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addCommonHeaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenu1.SuspendLayout();
            this.contextProject.SuspendLayout();
            this.contextCommon.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgLstProject
            // 
            this.imgLstProject.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstProject.ImageStream")));
            this.imgLstProject.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstProject.Images.SetKeyName(0, "folder.ico");
            this.imgLstProject.Images.SetKeyName(1, "folder.ico");
            this.imgLstProject.Images.SetKeyName(2, "Project.ico");
            this.imgLstProject.Images.SetKeyName(3, "source-c.ico");
            this.imgLstProject.Images.SetKeyName(4, "source-cpp.ico");
            this.imgLstProject.Images.SetKeyName(5, "source-h.ico");
            // 
            // treeProject
            // 
            this.treeProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeProject.ImageIndex = 0;
            this.treeProject.ImageList = this.imgLstProject;
            this.treeProject.Location = new System.Drawing.Point(0, 0);
            this.treeProject.Name = "treeProject";
            this.treeProject.SelectedImageIndex = 0;
            this.treeProject.Size = new System.Drawing.Size(282, 498);
            this.treeProject.TabIndex = 0;
            this.treeProject.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.Project_FileDoubleClick);
            this.treeProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProcessFileBrowse);
            // 
            // ContextMenu1
            // 
            this.ContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewFileToolStripMenuItem,
            this.addFileToolStripMenuItem,
            this.deleteFileToolStripMenuItem,
            this.showCoverageToolStripMenuItem});
            this.ContextMenu1.Name = "ContextMenu1";
            this.ContextMenu1.Size = new System.Drawing.Size(182, 122);
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.addFileToolStripMenuItem.Text = "Add File";
            this.addFileToolStripMenuItem.Click += new System.EventHandler(this.addFileToolStripMenuItem_Click);
            // 
            // deleteFileToolStripMenuItem
            // 
            this.deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
            this.deleteFileToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.deleteFileToolStripMenuItem.Text = "Delete File";
            this.deleteFileToolStripMenuItem.Click += new System.EventHandler(this.deleteFileToolStripMenuItem_Click);
            // 
            // showCoverageToolStripMenuItem
            // 
            this.showCoverageToolStripMenuItem.Name = "showCoverageToolStripMenuItem";
            this.showCoverageToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.showCoverageToolStripMenuItem.Text = "Show Coverage";
            this.showCoverageToolStripMenuItem.Click += new System.EventHandler(this.showCoverageToolStripMenuItem_Click);
            // 
            // contextProject
            // 
            this.contextProject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.propertiesToolStripMenuItem});
            this.contextProject.Name = "contextProject";
            this.contextProject.Size = new System.Drawing.Size(146, 76);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.renameToolStripMenuItem.Text = "Rename";
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.removeToolStripMenuItem.Text = "Remove";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(145, 24);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // contextCommon
            // 
            this.contextCommon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCommonHeaderToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.contextCommon.Name = "contextCommon";
            this.contextCommon.Size = new System.Drawing.Size(219, 52);
            // 
            // addCommonHeaderToolStripMenuItem
            // 
            this.addCommonHeaderToolStripMenuItem.Name = "addCommonHeaderToolStripMenuItem";
            this.addCommonHeaderToolStripMenuItem.Size = new System.Drawing.Size(218, 24);
            this.addCommonHeaderToolStripMenuItem.Text = "Add Common File";
            this.addCommonHeaderToolStripMenuItem.Click += new System.EventHandler(this.addCommonHeaderToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(218, 24);
            this.deleteToolStripMenuItem.Text = "Delete Common  File";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // addNewFileToolStripMenuItem
            // 
            this.addNewFileToolStripMenuItem.Name = "addNewFileToolStripMenuItem";
            this.addNewFileToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.addNewFileToolStripMenuItem.Text = "Add New File";
            this.addNewFileToolStripMenuItem.Click += new System.EventHandler(this.addNewFileToolStripMenuItem_Click);
            // 
            // Project
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 498);
            this.Controls.Add(this.treeProject);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Project";
            this.Text = "Project";
            this.Load += new System.EventHandler(this.Project_Load);
            this.ContextMenu1.ResumeLayout(false);
            this.contextProject.ResumeLayout(false);
            this.contextCommon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imgLstProject;
        private System.Windows.Forms.TreeView treeProject;
        private System.Windows.Forms.ContextMenuStrip ContextMenu1;
        private System.Windows.Forms.ToolStripMenuItem addFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextProject;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextCommon;
        private System.Windows.Forms.ToolStripMenuItem addCommonHeaderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCoverageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewFileToolStripMenuItem;
    }
}