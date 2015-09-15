using System;
using System.Windows.Forms;
using System.IO;
using Gunit.DataModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Gunit.Ui
{


    public partial class ProjectUi : GUnitSideBarBase
    {
        TreeNode m_SourceNode;
        TreeNode m_HeaderNode;
        TreeNode m_ProjectHeaderNode;
        TreeNode m_ProjectNode;
        ProjectSetting m_projectSetting;
        public delegate void onFIleOpened(string file);
        public event onFIleOpened evFileOpened = delegate { };
        public delegate void onFIleRemoved(string file);
        public event onFIleRemoved evFileRemoved = delegate { };
        public delegate void onFIleAdded(string file);
        public event onFIleAdded evFileAdded = delegate { };
        public delegate void onOpenProjectComplete(string file);
        public event onOpenProjectComplete evOpenprojectComplete = delegate { };
        public delegate void onMacroUpdated(ListofStrings macros);
        public event onMacroUpdated evMacroUpdate = delegate { };

        /// <summary>
        /// Constructor for ProjectUi
        /// </summary>
        /// <param name="model">Model Object for the ProjectUi</param>
        public ProjectUi(DataModelBase model):base(model)
        {
            InitializeComponent();
            
        }
        void treeview1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Tag is string)
            {
                string path = e.Node.Tag as string;
                if (File.Exists(path) == false)
                {
                    e.Node.HideCheckBox();
                }
            }
            else
            {
                e.Node.HideCheckBox();
            }
            e.DrawDefault = true;
        }
        
        public ListofFiles ProjectUi_getParseRequest()
        {
            ListofFiles files = new ListofFiles();
            foreach (TreeNode node in m_SourceNode.Nodes)
            {
                if (node.Tag is string)
                {
                    if (node.Checked)
                    {
                        files += node.Tag as string;
                    }
                }
            }
            foreach (TreeNode node in m_HeaderNode.Nodes)
            {
                if (node.Tag is string)
                {
                    if (node.Checked)
                    {
                        files += node.Tag as string;
                    }
                }
            }
            foreach (TreeNode node in m_ProjectHeaderNode.Nodes)
            {
                if (node.Tag is string)
                {
                    if (node.Checked)
                    {
                        files += node.Tag as string;
                    }
                }
            }
            return files;
        }
        /// <summary>
        /// Form Load Event Handler for ProjectUi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectUi_Load(object sender, EventArgs e)
        {
            this.ProjectTree.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.ProjectTree.DrawNode += new DrawTreeNodeEventHandler(treeview1_DrawNode);
            ProjectTree.CheckBoxes = true;
            m_SourceNode = new TreeNode("Unit Source Files");
            m_SourceNode.Tag = FileType.SOURCE_FILE;
            m_SourceNode.ImageIndex = 0;
            m_SourceNode.SelectedImageIndex = 0;
            m_HeaderNode = new TreeNode("Unit Header Files");
            m_HeaderNode.Tag = FileType.HEADER_FILE;
            m_HeaderNode.ImageIndex = 0;
            m_HeaderNode.SelectedImageIndex = 0;
            m_ProjectHeaderNode = new TreeNode("Project Header Files");
            m_ProjectHeaderNode.ImageIndex = 0;
            m_ProjectHeaderNode.SelectedImageIndex = 0;
            m_ProjectHeaderNode.Tag = FileType.PROJECT_HEADER_FILE;
            m_ProjectNode = new TreeNode(((ProjectDataModel)m_model).ProjectName);
            m_ProjectNode.ImageIndex = 1;
            m_ProjectNode.SelectedImageIndex = 1;
            
            
        }
        /// <summary>
        /// Update the Project Name in the Project Tree Node
        /// </summary>
        public void ProjectUi_updateProjectNameNode()
        {
            m_ProjectNode.Text = ((ProjectDataModel)m_model).ProjectName;
            
        }
        /// <summary>
        /// Adding the Source files Node , Header files node and Project Header 
        /// Files Nodes in the Project Node
        /// </summary>
        private void ProjectUi_AddProjectNodes()
        {
            m_ProjectNode.Nodes.Clear();
            m_SourceNode.Nodes.Clear();
            m_HeaderNode.Nodes.Clear();
            m_ProjectHeaderNode.Nodes.Clear();
            ProjectTree.Nodes.Add(m_ProjectNode);
            m_ProjectNode.Nodes.Add(m_SourceNode);
            m_ProjectNode.Nodes.Add(m_HeaderNode);
            m_ProjectNode.Nodes.Add(m_ProjectHeaderNode);
        }
        /// <summary>
        /// Update the Tag given for the ProjectNode
        /// </summary>
        public void ProjectUi_UpdateProjectPath()
        {
            m_ProjectNode.Tag = ((ProjectDataModel)m_model).ProjectPath;
        }
        /// <summary>
        /// Update the Sourse files in the SourceTreeNode
        /// </summary>
        public void ProjectUi_UpdateSourceFileNodes()
        {
            m_SourceNode.Nodes.Clear();
            foreach (string file in ((ProjectDataModel)m_model).SourceFiles)
            {
                TreeNode nodeSourceFile = new TreeNode(Path.GetFileName(file));
                nodeSourceFile.Tag = file;
                if(Path.GetExtension(file) == ".cpp")
                {
                    nodeSourceFile.ImageIndex = 3;
                    nodeSourceFile.SelectedImageIndex = 3;
                }
                else
                {
                    nodeSourceFile.ImageIndex = 2;
                    nodeSourceFile.SelectedImageIndex = 2;
                }
                
                m_SourceNode.Nodes.Add(nodeSourceFile);
            }
        }
        /// <summary>
        /// Update the Files in the HeaderFiles node
        /// </summary>
        public void ProjectUi_UpdateHeaderFileNodes()
        {
            m_HeaderNode.Nodes.Clear();
            foreach (string file in ((ProjectDataModel)m_model).HeaderFiles)
            {
                TreeNode nodeFile = new TreeNode(Path.GetFileName(file));
                nodeFile.Tag = file;
                nodeFile.ImageIndex = 4;
                nodeFile.SelectedImageIndex = 4;
                m_HeaderNode.Nodes.Add(nodeFile);
            }
        }

        /// <summary>
        /// Update the Project Header files in the Project Header file Node
        /// </summary>
        public void ProjectUi_UpdateProjectHeaderFileNodes()
        {
            m_ProjectHeaderNode.Nodes.Clear();
            foreach (string file in ((ProjectDataModel)m_model).ProjectHeaderFiles)
            {
                TreeNode nodeFile = new TreeNode(Path.GetFileName(file));
                nodeFile.Tag = file;
                nodeFile.ImageIndex = 4;
                nodeFile.SelectedImageIndex = 4;
                m_ProjectHeaderNode.Nodes.Add(nodeFile);
            }
        }
        /// <summary>
        /// Update the Model Properties here upon receiving 
        /// a new Projet file
        /// </summary>
        /// <param name="fileName"></param>
        private void ProjectUi_UpdateModel(string ProjectName)
        {
            if (File.Exists(ProjectName))
            {
               
                ProjectUi_AddProjectNodes();
                ((ProjectDataModel)m_model).ProjectDataModel_ReadProjectData(ProjectName);
                #region OLD_CODE
                //try
                //{
                //    DBManager.DBManager_Connect(((ProjectDataModel)m_model).DBPath);
                //}
                //catch
                //{
                    
                //    try
                //    {
                //        ((ProjectDataModel)m_model).DBPath = DBManager.CreateDataBase(((ProjectDataModel)m_model).ProjectName, ((ProjectDataModel)m_model).ProjectPath);
                //        DBManager.DBManager_Connect(((ProjectDataModel)m_model).DBPath);
                //    }
                //    catch
                //    {
                //        MessageBox.Show("Unable to create to Project Database");
                //    }
                //}
                #endregion
                evOpenprojectComplete(ProjectName);
            }

        }
        /// <summary>
        /// Called when a project is Opened
        /// </summary>
        public override void OpenProject()
        {
            base.OpenProject();
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Project File (*.xml)|*.xml";
            DialogResult result =  file.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(file.FileName))
                {
                    closeProject();
                    ProjectUi_UpdateModel(file.FileName);
                    
                }
            }
        }
      
        /// <summary>
        /// Close the Project and clear the Nodes
        /// </summary>
        public override void closeProject()
        {
            base.closeProject();
            m_ProjectNode.Nodes.Clear();
            ProjectTree.Nodes.Clear();
            //DBManager.DBManager_disconnect();
        }
        
            
        
        /// <summary>
        /// Add a new project
        /// </summary>
        public override void newProject()
        {
            base.newProject();
            closeProject();
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "Project File (*.xml)|*.xml";
            
            DialogResult result = file.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ((ProjectDataModel)m_model).ProjectPath = file.FileName;
                ((ProjectDataModel)m_model).ProjectName = Path.GetFileNameWithoutExtension(file.FileName);
                try
                {
                    ((ProjectDataModel)m_model).DBPath = Path.GetDirectoryName(file.FileName)+"\\" + Path.GetFileNameWithoutExtension(file.FileName)+".sdf";
                     DBManager dbmanager = new DBManager(((ProjectDataModel)m_model).DBPath);
                    //((ProjectDataModel)m_model).DBPath = DBManager.CreateDataBase(((ProjectDataModel)m_model).ProjectName, file.FileName);
                    ((ProjectDataModel)m_model).ProjectDatamodel_writeProjectData();
                    ProjectUi_UpdateModel(file.FileName);
                }
                catch(Exception err)
                {
                    //((ProjectDataModel)m_model).DBPath = "";
                    MessageBox.Show("Cannot create Database"+ err.ToString());
                }
                
               
            }
        }
        /// <summary>
        /// Save the Project
        /// </summary>
        public override void saveProject()
        {
            base.saveProject();
            ((ProjectDataModel)m_model).ProjectDatamodel_writeProjectData();
              
        }
        /// <summary>
        /// When a new File is Opened this method will be called by the 
        /// Controller
        /// </summary>
        /// <param name="l_currentFile"></param>
        public override void OpenFile(string l_currentFile)
        {
            base.OpenFile(l_currentFile);


        }
        /// <summary>
        /// property Changed Indication
        /// </summary>
        /// <param name="propertyName">Name of the changed property</param>
        public override void propertyChanged(string propertyName)
        {
            m_model.IsDirty = true;
            switch (propertyName)
            {
                case "ProjectName":
                    ProjectUi_updateProjectNameNode();
                    break;
                case "ProjectPath":
                    ProjectUi_UpdateProjectPath();
                    break;
                case "SourceFiles":
                    ProjectUi_UpdateSourceFileNodes();
                    break;
                case "HeaderFiles":
                    ProjectUi_UpdateHeaderFileNodes();
                    break;
                case "ProjectHeaderFiles":
                    ProjectUi_UpdateProjectHeaderFileNodes();
                    break;
                default:
                    break;
            }
            
        }
        
        /// <summary>
        /// Add new or Existing file to the project
        /// </summary>
        /// <param name="l_fileType"> Source File,HeaderFile or Project header File</param>
        /// <param name="isFileExisting">Is the file Already existing in the disk or 
        /// need to be created newly</param>
        private void ProjectUi_AddFile(FileType l_fileType,bool isFileExisting = false)
        {
            string filter = "";
            FileDialog file = new SaveFileDialog();
            
            DialogResult result = new DialogResult();
            switch (l_fileType)
            {
                case FileType.HEADER_FILE:
                    filter = "Header File (*.h)|*.h";
                    break;
                case FileType.PROJECT_HEADER_FILE:
                    filter = "Header File (*.h)|*.h|(*.pch)|*.pch";
                    break;
                case FileType.SOURCE_FILE:
                    filter = "Source File (*.c)|*.c|(*.cpp)|*.cpp";
                    break;
                default:
                    break;
            }
            file.Filter = filter;
            if (isFileExisting == true)
            {

                file = new OpenFileDialog();
                file.Filter = filter;
               ((OpenFileDialog)file).Multiselect = true;
                result = file.ShowDialog();
            }
            else
            {
                 
                
                result  = file.ShowDialog();
                
            }
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (isFileExisting == false)
                {
                    StreamWriter writer = new StreamWriter(file.FileName);
                    writer.Close();
                }
                if (isFileExisting)
                {
                    foreach (string filename in ((OpenFileDialog)file).FileNames)
                    {
                        ((ProjectDataModel)m_model).ProjectDataModel_AddOrRemoveFile(true, l_fileType, filename);
                        evFileAdded(filename);
                    }

                }
                else
                {
                    ((ProjectDataModel)m_model).ProjectDataModel_AddOrRemoveFile(true, l_fileType, file.FileName);
                    evFileAdded(file.FileName);
                }
            }
           
           
            
        }
        /// <summary>
        /// On clicking add new File this function will be called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ProjectTree.SelectedNode.Tag is FileType)
            {
                FileType type = (FileType)ProjectTree.SelectedNode.Tag;
                ProjectUi_AddFile(type);
            }
        }
        /// <summary>
        /// Handle Mouse down event on the Project Tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectUi_MouseDownOnTree(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ProjectTree.SelectedNode = ProjectTree.GetNodeAt(e.X, e.Y);
                if (ProjectTree.SelectedNode != null && ProjectTree.SelectedNode != m_ProjectNode)
                {
                    ProjectMenu.Show(Cursor.Position);
                }
                else if (ProjectTree.SelectedNode == m_ProjectNode)
                {
                    projectProperties.Show(Cursor.Position);
                }
            }
        }
        /// <summary>
        /// Event handler for Deleting a file from Project through Context Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProjectTree.SelectedNode.Tag is string)
            {
                if (ProjectTree.SelectedNode.Parent.Tag is FileType)
                {
                    FileType l_FileType = (FileType)ProjectTree.SelectedNode.Parent.Tag;
                    string fileName = ProjectTree.SelectedNode.Tag as string;
                    ((ProjectDataModel)m_model).
                        ProjectDataModel_AddOrRemoveFile(false, l_FileType,fileName );
                        evFileRemoved(fileName);
                }
            }
        }
        /// <summary>
        /// Event handler for the ContextMenu Add Existing file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void existingFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProjectTree.SelectedNode.Tag is FileType)
            {
                FileType type = (FileType)ProjectTree.SelectedNode.Tag;
                ProjectUi_AddFile(type,true);
            }
        }
        /// <summary>
        /// Event handler for double clicking the TreeNode in the Project Tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectUi_NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string)
            {
                string filePath = e.Node.Tag as string;
                if (evFileOpened != null)
                {
                    evFileOpened(filePath);
                    evMacroUpdate(((ProjectDataModel)m_model).MacroNames);
                }
                
            }
        }
        /// <summary>
        /// Enter Project Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_projectSetting = new ProjectSetting(((ProjectDataModel)m_model));
            m_projectSetting.ShowDialog();
            evMacroUpdate(((ProjectDataModel)m_model).MacroNames);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectTree.LabelEdit = true;

            if (!ProjectTree.SelectedNode.IsEditing)
            {
                ProjectTree.SelectedNode.BeginEdit();
            }
        }
        /// <summary>
        /// Event handler after Editing the Node Label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectUi_afterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        // Stop editing without canceling the label change.
                        e.Node.EndEdit(false);
                        e.Node.Tag = e.Label;
                        ((ProjectDataModel)m_model).ProjectName = e.Label;
                    }
                    else
                    {
                        /* Cancel the label edit action, inform the user, and 
                           place the node in edit mode again. */
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid tree node label.\n" +
                           "The invalid characters are: '@','.', ',', '!'",
                           "Node Label Edit");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    /* Cancel the label edit action, inform the user, and 
                       place the node in edit mode again. */
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid tree node label.\nThe label cannot be blank",
                       "Node Label Edit");
                    e.Node.BeginEdit();
                }
            }
        }
    }
}
