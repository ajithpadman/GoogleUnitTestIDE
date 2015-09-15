namespace Gunit.Ui
{
    partial class ProjectUi 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectUi));
            this.ProjectTree = new System.Windows.Forms.TreeView();
            this.imgListProjectUi = new System.Windows.Forms.ImageList(this.components);
            this.ProjectMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectProperties = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjectMenu.SuspendLayout();
            this.projectProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProjectTree
            // 
            this.ProjectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectTree.ImageIndex = 0;
            this.ProjectTree.ImageList = this.imgListProjectUi;
            this.ProjectTree.Location = new System.Drawing.Point(0, 0);
            this.ProjectTree.Name = "ProjectTree";
            this.ProjectTree.SelectedImageIndex = 0;
            this.ProjectTree.Size = new System.Drawing.Size(282, 255);
            this.ProjectTree.TabIndex = 0;
            this.ProjectTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.ProjectUi_afterLabelEdit);
            this.ProjectTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.ProjectUi_NodeDoubleClick);
            this.ProjectTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ProjectUi_MouseDownOnTree);
            // 
            // imgListProjectUi
            // 
            this.imgListProjectUi.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListProjectUi.ImageStream")));
            this.imgListProjectUi.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListProjectUi.Images.SetKeyName(0, "folder.ico");
            this.imgListProjectUi.Images.SetKeyName(1, "Project.ico");
            this.imgListProjectUi.Images.SetKeyName(2, "source-c.ico");
            this.imgListProjectUi.Images.SetKeyName(3, "source-cpp.ico");
            this.imgListProjectUi.Images.SetKeyName(4, "source-h.ico");
            // 
            // ProjectMenu
            // 
            this.ProjectMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ProjectMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.ProjectMenu.Name = "ProjectMenu";
            this.ProjectMenu.Size = new System.Drawing.Size(129, 56);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolStripMenuItem,
            this.existingFileToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // newFileToolStripMenuItem
            // 
            this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            this.newFileToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.newFileToolStripMenuItem.Text = "New File";
            this.newFileToolStripMenuItem.Click += new System.EventHandler(this.newFileToolStripMenuItem_Click);
            // 
            // existingFileToolStripMenuItem
            // 
            this.existingFileToolStripMenuItem.Name = "existingFileToolStripMenuItem";
            this.existingFileToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.existingFileToolStripMenuItem.Text = "Existing File";
            this.existingFileToolStripMenuItem.Click += new System.EventHandler(this.existingFileToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(128, 26);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // projectProperties
            // 
            this.projectProperties.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.projectProperties.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.renameToolStripMenuItem});
            this.projectProperties.Name = "projectProperties";
            this.projectProperties.Size = new System.Drawing.Size(139, 56);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
            this.propertiesToolStripMenuItem.Text = "Settings";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(138, 26);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // ProjectUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.ProjectTree);
            this.Name = "ProjectUi";
            this.Text = "Project Explorer";
            this.Load += new System.EventHandler(this.ProjectUi_Load);
            this.ProjectMenu.ResumeLayout(false);
            this.projectProperties.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView ProjectTree;
        private System.Windows.Forms.ImageList imgListProjectUi;
        private System.Windows.Forms.ContextMenuStrip ProjectMenu;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip projectProperties;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
    }
}