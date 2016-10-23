using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GUnitFramework.Interfaces;
using System.IO;
using GUnitFramework.Implementation;
namespace Gunit.Ui
{
  
    public partial class ProjectSetting : DockContent
    {
        ICGunitHost m_host = null;
        
        
        public ProjectSetting()
        {
            InitializeComponent();
        }
        public ProjectSetting(ICGunitHost host)
        {
            InitializeComponent();
            m_host = host;
            m_host.evProjectStatus+=new onProjectStatus(Handle_evProjectStatus);
            m_host.PropertyChanged+=new PropertyChangedEventHandler(Host_PropertyChanged);
        }
        void Host_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ProjectData":
                    clearProjectSetting();
                    showProjectSetting();
                    break;
                default:
                    break;
            }
        }
        void Handle_evProjectStatus(ProjectStatus status,object data)
        {
            switch (status)
            {
                case ProjectStatus.CLOSE:
                    clearProjectSetting();
                    
                    break;
                case ProjectStatus.NEW:
                    
                    showProjectSetting();
                    break;
                case ProjectStatus.OPEN:
                    
                    showProjectSetting();
                    break;
             
                default:
                    break;
                
            }
        }
        private void ProjectSetting_Load(object sender, EventArgs e)
        {
            treeProject.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeProject.DrawNode += new DrawTreeNodeEventHandler(treeProject_DrawNode);
            treeProject.CheckBoxes = true;
            
        }

        private void clearProjectSetting()
        {
            treeProject.Nodes.Clear();
        }
        private void showProjectSetting()
        {
            clearProjectSetting();
            if (m_host != null)
            {
                
                treeProject.Nodes.Add(displayProjectSetting(m_host.ProjectData));
            }
        }
        
        void treeProject_DrawNode(object sender, DrawTreeNodeEventArgs e)
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
        private TreeNode displayProjectSetting(ICProjectData projectData)
        {
            if (projectData != null)
            {
                
                TreeNode Root = new TreeNode(projectData.ProjectName);
                Root.ImageIndex = 0;
                Root.SelectedImageIndex = 0;
                Root.Tag = FileType.Root;
                TreeNode srcFiles = new TreeNode("Source Files");
                srcFiles.ImageIndex = 1;
                srcFiles.SelectedImageIndex = 1;
                srcFiles.Tag = FileType.SourceFile;
                TreeNode header = new TreeNode("Header Files");
                header.ImageIndex = 1;
                header.SelectedImageIndex = 1;
                header.Tag = FileType.HeaderFile;
                TreeNode preInclude = new TreeNode("PreInclude Files");
                preInclude.ImageIndex = 1;
                preInclude.SelectedImageIndex = 1;
                preInclude.Tag = FileType.PreInclude;
                foreach (string file in projectData.SourceFiles)
                {
                    try
                    {
                        TreeNode node = new TreeNode(Path.GetFileName(file));
                        node.Tag = file;
                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                        if (m_host.SelectedFiles.Contains(file))
                        {
                            node.Checked = true;
                        }
                        srcFiles.Nodes.Add(node);
                    }
                    catch
                    {

                    }
                }
                foreach (string file in projectData.HeaderFiles)
                {
                    try
                    {
                        TreeNode node = new TreeNode(Path.GetFileName(file));
                        node.Tag = file;
                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                        if (m_host.SelectedFiles.Contains(file))
                        {
                            node.Checked = true;
                        }
                        header.Nodes.Add(node);
                    }
                    catch
                    {

                    }
                }
                foreach (string file in projectData.PreIncludes)
                {
                    try
                    {
                        TreeNode node = new TreeNode(Path.GetFileName(file));
                        node.Tag = file;
                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                        if (m_host.SelectedFiles.Contains(file))
                        {
                            node.Checked = true;
                        }
                        preInclude.Nodes.Add(node);
                    }
                    catch
                    {

                    }
                }
                Root.Nodes.Add(srcFiles);
                Root.Nodes.Add(header);
                Root.Nodes.Add(preInclude);
                return Root;

            }
            else
            {
                return null;
            }
        }

        private void ProjectTree_RightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeProject.SelectedNode = treeProject.GetNodeAt(e.X, e.Y);
               if (treeProject.SelectedNode.Tag is  FileType)
                {
                    FileType tag = (FileType)treeProject.SelectedNode.Tag  ;
                    if (tag == FileType.HeaderFile)
                    {
                        HeaderFIleMenu.Show(Cursor.Position);
                    }
                    else if (tag == FileType.SourceFile)
                    {
                        srcFileMenu.Show(Cursor.Position);
                    }
                    else if (tag == FileType.Root)
                    {
                        ProjectProperties.Show(Cursor.Position);
                    }
                    else if (tag == FileType.PreInclude)
                    {
                        preIncludeMenu.Show(Cursor.Position);
                    }
                }
               else if (treeProject.SelectedNode.Tag is string)
               {
                   FileNode.Show(Cursor.Position);
               }

            }
        }
        private void  addExistingFiles(string filter,List<string> list)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = filter;
            dlg.Multiselect = true;
            DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in dlg.FileNames)
                {
                    list.Add(file);

                    
                }
            }
        }
        private string addNewFile(string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = filter;
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                StreamWriter writer = new StreamWriter(dlg.FileName);
                writer.Close();
                return (dlg.FileName);


            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Add existing src file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addExistSrc_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>();
            addExistingFiles("C Files (*.c)|*.c|CPP Files (*.cpp)|*.cpp|CC files (*.cc)|*.cc", files);

            foreach (string file in files)
            {
                m_host.AddOrRemoveFile(true, FileType.SourceFile, file);
                m_host.ProjectDataModel_addIncludePath(Path.GetDirectoryName(file));

              
            }

        }
        /// <summary>
        /// add new src file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addsrcNew_Click(object sender, EventArgs e)
        {
            string file = addNewFile("C Files (*.c)|*.c");
            if (null != file)
            {
                m_host.AddOrRemoveFile(true, FileType.SourceFile, file);
                m_host.ProjectDataModel_addIncludePath(Path.GetDirectoryName(file));

              
            }
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string file = addNewFile("Header Files (*.h)|*.h");
            if (null != file)
            {
                m_host.AddOrRemoveFile(true, FileType.HeaderFile, file);
                m_host.ProjectDataModel_addIncludePath(Path.GetDirectoryName(file));
                
            }

            
        }

        private void addExistingFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>();
            addExistingFiles("Header Files (*.h)|*.h", files);

            foreach (string file in files)
            {
                m_host.AddOrRemoveFile(true, FileType.HeaderFile, file);
                m_host.ProjectDataModel_addIncludePath(Path.GetDirectoryName(file));

                
            }
        }

        private void File_Checked(object sender, TreeViewEventArgs e)
        {
            lock (m_host.SelectedFiles)
            {
                if (e.Node.Checked)
                {
                    if (e.Node.Tag is string)
                    {
                        m_host.addSelectedFiles(e.Node.Tag as string);
                    }
                }
                else
                {
                    if (e.Node.Tag is string)
                    {
                        if (m_host.SelectedFiles.Contains(e.Node.Tag as string))
                        {
                            string file = e.Node.Tag as string;
                            m_host.removeSelectedFiles(file);
                        }
                    }
                }
            }
        }

        private void File_openClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string)
            {
                string filePath = e.Node.Tag as string;
                m_host.CurrentFileInEditor = filePath;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectSettings settings = new ProjectSettings(m_host);
            settings.ShowDialog();
        }

        private void addNewPreInclude_Click(object sender, EventArgs e)
        {
            string file = addNewFile("Header Files (*.h)|*.h");
            if (null != file)
            {
                m_host.AddOrRemoveFile(true, FileType.PreInclude, file);
                m_host.ProjectDataModel_addIncludePath(Path.GetDirectoryName(file));
                
            }

        }

        private void addExistingPreInclude_Click(object sender, EventArgs e)
        {
            List<string> files = new List<string>();
            addExistingFiles("Header Files (*.h)|*.h", files);

            foreach (string file in files)
            {

                m_host.AddOrRemoveFile(true, FileType.PreInclude, file);
                m_host.ProjectDataModel_addIncludePath(Path.GetDirectoryName(file));
               
            }
        }

        private void openHeader_Click(object sender, EventArgs e)
        {

        }

        private void openFile_Click(object sender, EventArgs e)
        {

        }

        private void removeFile_Click(object sender, EventArgs e)
        {
            if (treeProject.SelectedNode.Tag is string)
            {
                if (treeProject.SelectedNode.Parent.Tag is FileType)
                {
                    FileType l_FileType = (FileType)treeProject.SelectedNode.Parent.Tag;
                    string fileName = treeProject.SelectedNode.Tag as string;
                    m_host.AddOrRemoveFile(false, l_FileType, fileName);
                    
                }
            }
        }
       
       
        

    }
}
