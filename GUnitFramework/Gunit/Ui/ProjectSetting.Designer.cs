namespace Gunit.Ui
{
    partial class ProjectSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectSetting));
            this.treeProject = new System.Windows.Forms.TreeView();
            this.srcFileMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addsrcNew = new System.Windows.Forms.ToolStripMenuItem();
            this.addExistSrc = new System.Windows.Forms.ToolStripMenuItem();
            this.HeaderFIleMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addExistingFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjectProperties = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preIncludeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewPreInclude = new System.Windows.Forms.ToolStripMenuItem();
            this.addExistingPreInclude = new System.Windows.Forms.ToolStripMenuItem();
            this.FileNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFile = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFile = new System.Windows.Forms.ToolStripMenuItem();
            this.imgProject = new System.Windows.Forms.ImageList(this.components);
            this.srcFileMenu.SuspendLayout();
            this.HeaderFIleMenu.SuspendLayout();
            this.ProjectProperties.SuspendLayout();
            this.preIncludeMenu.SuspendLayout();
            this.FileNode.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeProject
            // 
            this.treeProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeProject.ImageIndex = 0;
            this.treeProject.ImageList = this.imgProject;
            this.treeProject.Location = new System.Drawing.Point(0, 0);
            this.treeProject.Name = "treeProject";
            this.treeProject.SelectedImageIndex = 0;
            this.treeProject.Size = new System.Drawing.Size(282, 255);
            this.treeProject.TabIndex = 0;
            this.treeProject.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.File_Checked);
            this.treeProject.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.File_openClick);
            this.treeProject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProjectTree_RightClick);
            // 
            // srcFileMenu
            // 
            this.srcFileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addsrcNew,
            this.addExistSrc});
            this.srcFileMenu.Name = "srcFileMenu";
            this.srcFileMenu.Size = new System.Drawing.Size(189, 52);
            // 
            // addsrcNew
            // 
            this.addsrcNew.Name = "addsrcNew";
            this.addsrcNew.Size = new System.Drawing.Size(188, 24);
            this.addsrcNew.Text = "Add New File";
            this.addsrcNew.Click += new System.EventHandler(this.addsrcNew_Click);
            // 
            // addExistSrc
            // 
            this.addExistSrc.Name = "addExistSrc";
            this.addExistSrc.Size = new System.Drawing.Size(188, 24);
            this.addExistSrc.Text = "Add Existing File";
            this.addExistSrc.Click += new System.EventHandler(this.addExistSrc_Click);
            // 
            // HeaderFIleMenu
            // 
            this.HeaderFIleMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFileToolStripMenuItem,
            this.addExistingFileToolStripMenuItem});
            this.HeaderFIleMenu.Name = "HeaderFIleMenu";
            this.HeaderFIleMenu.Size = new System.Drawing.Size(189, 52);
            // 
            // addFileToolStripMenuItem
            // 
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
            this.addFileToolStripMenuItem.Text = "Add New File";
            this.addFileToolStripMenuItem.Click += new System.EventHandler(this.addFileToolStripMenuItem_Click);
            // 
            // addExistingFileToolStripMenuItem
            // 
            this.addExistingFileToolStripMenuItem.Name = "addExistingFileToolStripMenuItem";
            this.addExistingFileToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
            this.addExistingFileToolStripMenuItem.Text = "Add Existing File";
            this.addExistingFileToolStripMenuItem.Click += new System.EventHandler(this.addExistingFileToolStripMenuItem_Click);
            // 
            // ProjectProperties
            // 
            this.ProjectProperties.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.ProjectProperties.Name = "ProjectProperties";
            this.ProjectProperties.Size = new System.Drawing.Size(132, 28);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(131, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // preIncludeMenu
            // 
            this.preIncludeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewPreInclude,
            this.addExistingPreInclude});
            this.preIncludeMenu.Name = "preIncludeMenu";
            this.preIncludeMenu.Size = new System.Drawing.Size(189, 52);
            // 
            // addNewPreInclude
            // 
            this.addNewPreInclude.Name = "addNewPreInclude";
            this.addNewPreInclude.Size = new System.Drawing.Size(188, 24);
            this.addNewPreInclude.Text = "Add New File";
            this.addNewPreInclude.Click += new System.EventHandler(this.addNewPreInclude_Click);
            // 
            // addExistingPreInclude
            // 
            this.addExistingPreInclude.Name = "addExistingPreInclude";
            this.addExistingPreInclude.Size = new System.Drawing.Size(188, 24);
            this.addExistingPreInclude.Text = "Add Existing File";
            this.addExistingPreInclude.Click += new System.EventHandler(this.addExistingPreInclude_Click);
            // 
            // FileNode
            // 
            this.FileNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFile,
            this.removeFile});
            this.FileNode.Name = "FileNode";
            this.FileNode.Size = new System.Drawing.Size(133, 52);
            // 
            // openFile
            // 
            this.openFile.Name = "openFile";
            this.openFile.Size = new System.Drawing.Size(132, 24);
            this.openFile.Text = "Open";
            this.openFile.Click += new System.EventHandler(this.openFile_Click);
            // 
            // removeFile
            // 
            this.removeFile.Name = "removeFile";
            this.removeFile.Size = new System.Drawing.Size(132, 24);
            this.removeFile.Text = "Remove";
            this.removeFile.Click += new System.EventHandler(this.removeFile_Click);
            // 
            // imgProject
            // 
            this.imgProject.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgProject.ImageStream")));
            this.imgProject.TransparentColor = System.Drawing.Color.Transparent;
            this.imgProject.Images.SetKeyName(0, "Project.ico");
            this.imgProject.Images.SetKeyName(1, "Folder-Open.ico");
            this.imgProject.Images.SetKeyName(2, "File.ico");
            // 
            // ProjectSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.ControlBox = false;
            this.Controls.Add(this.treeProject);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectSetting";
            this.Text = "Project Settings";
            this.Load += new System.EventHandler(this.ProjectSetting_Load);
            this.srcFileMenu.ResumeLayout(false);
            this.HeaderFIleMenu.ResumeLayout(false);
            this.ProjectProperties.ResumeLayout(false);
            this.preIncludeMenu.ResumeLayout(false);
            this.FileNode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeProject;
        private System.Windows.Forms.ContextMenuStrip srcFileMenu;
        private System.Windows.Forms.ToolStripMenuItem addsrcNew;
        private System.Windows.Forms.ContextMenuStrip HeaderFIleMenu;
        private System.Windows.Forms.ToolStripMenuItem addFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addExistSrc;
        private System.Windows.Forms.ToolStripMenuItem addExistingFileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ProjectProperties;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip preIncludeMenu;
        private System.Windows.Forms.ToolStripMenuItem addNewPreInclude;
        private System.Windows.Forms.ToolStripMenuItem addExistingPreInclude;
        private System.Windows.Forms.ContextMenuStrip FileNode;
        private System.Windows.Forms.ToolStripMenuItem openFile;
        private System.Windows.Forms.ToolStripMenuItem removeFile;
        private System.Windows.Forms.ImageList imgProject;
    }
}