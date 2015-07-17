using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
namespace GUnit
{
    public partial class Project : DockContent
    {
        ProjectInfo m_Project;
        SaveFileDialog m_file;
        enum ProjectFolderType
        {
            SOURCE,
            HEADER,
            COMMONHEADER
        }
        public delegate void onSourceFileAdded(string FilePath);
        public event onSourceFileAdded evSourceFileAdded;
        public delegate void onheaderFileAdded(string FilePath);
        public event onheaderFileAdded evHeaderFileAdded;
        public delegate void onFileOpened(FileInfo File);
        public event onFileOpened evFileOpened;

        public delegate void onCoverageRequest(FileInfo File);
        public event onCoverageRequest evCoverageRequest;

        public delegate void onFileRemoved(FileInfo File);
        public event onFileRemoved evFileRemoved;

        GUnit m_parent;
        TreeNode project;
        TreeNode source;
        TreeNode header;
        TreeNode Commonheader;
        public Project(GUnit parent)
        {
            InitializeComponent();
            m_parent = parent;
        }
        
        public void Project_AddNewProject(ProjectInfo projectData)
        {
            treeProject.Nodes.Clear();
            m_Project = projectData;
            project = new TreeNode(m_Project.m_ProjectName);
            project.ImageIndex = 2;
            project.SelectedImageIndex = 2;
            project.Tag = "Project";
            source = new TreeNode("Unit Source Files");
            source.ImageIndex = 0;
            source.SelectedImageIndex = 0;
            source.Tag = "Source";
            header = new TreeNode("Unit Header Files");
            header.Tag = "Header";
            header.ImageIndex = 1;
            header.SelectedImageIndex = 1;
            Commonheader = new TreeNode("Project Header Files");
            Commonheader.Tag = "Common";
            Commonheader.ImageIndex = 1;
            Commonheader.SelectedImageIndex = 1;
            project.Nodes.Add(source);
            project.Nodes.Add(header);
            project.Nodes.Add(Commonheader);
            treeProject.Nodes.Add(project);
            foreach (string file in projectData.m_HeaderFileNames)
            {
                Project_AddFilesToFolder(file, ProjectFolderType.HEADER);
            }
            foreach (string file in projectData.m_SourceFileNames)
            {
                Project_AddFilesToFolder(file, ProjectFolderType.SOURCE);
            }
            foreach (string file in projectData.m_SolnData.m_CommonHeadersList)
            {
                Project_AddFilesToFolder(file, ProjectFolderType.COMMONHEADER);
            }

        }
        private void Project_CloseEvents()
        {
            m_parent.evNewProject -= (Project_AddNewProject);
            m_parent.evOpenProject -= (Project_AddNewProject);
            m_parent.evCloseAllForms -=(CloseThisForm);
            m_parent.evFileRemovedfromOutside -= Project_fileRemovedFromOutside;
        }
        private void Project_Load(object sender, EventArgs e)
        {
            m_parent.evNewProject += (Project_AddNewProject);
            m_parent.evOpenProject += (Project_AddNewProject);
            m_parent.evCloseAllForms += (CloseThisForm);
            m_parent.evFileRemovedfromOutside += Project_fileRemovedFromOutside;
        }
        public void CloseThisForm()
        {
            Project_CloseEvents();
            this.Close();
        }
        private void Project_removeNode(TreeNode node,string fileInfo)
        {
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                if (node.Nodes[i].Tag is string)
                {
                    string tagValue = node.Nodes[i].Tag as string;
                    if (tagValue == fileInfo)
                    {
                        node.Nodes.RemoveAt(i);
                        FileInfo data = m_parent.m_data.GUnitData_getFileInformation(fileInfo);
                        if (data != null)
                        {
                            evFileRemoved(data);
                        }
                    }
                }
            }
        }
        private void Project_fileRemovedFromOutside(string fileInfo)
        {
            Project_removeNode(source, fileInfo);
            Project_removeNode(header, fileInfo);
            Project_removeNode(Commonheader, fileInfo);
        }
        private void ProcessFileBrowse(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeProject.SelectedNode = treeProject.GetNodeAt(e.X, e.Y);
                // Display context menu for eg:
                if (treeProject.SelectedNode != null)
                {
                    string projecttag =  treeProject.SelectedNode.Tag as string ;
                    if (projecttag  == "Project")
                    {
                        contextProject.Show(Cursor.Position);
                    }
                    else if (
                                  projecttag == "Source" ||
                                  projecttag == "Header"
                              
                                )
                        {
                            ContextMenu1.Show(Cursor.Position);
                        }
                    else if (projecttag == "Common")
                    {
                        contextCommon.Show(Cursor.Position);
                    }
                    else
                    {
                        if (treeProject.SelectedNode.Parent.Tag is string)
                        {
                            string tag = treeProject.SelectedNode.Parent.Tag as string;
                            if (tag == "Source" || tag == "Header")
                            {
                                ContextMenu1.Show(Cursor.Position);
                            }
                            else if (tag == "Common")
                            {
                                contextCommon.Show(Cursor.Position);
                            }
                        }
                    }
                  
                }
            }
        }
        private void Project_AddFilesToFolder(string filePath,ProjectFolderType fileFolder)
        {
            if (File.Exists(filePath))
            {
               
                m_parent.m_data.GUnitDat_AddIncludePaths(Path.GetDirectoryName(filePath));
                
                TreeNode file = new TreeNode(Path.GetFileName(filePath));
                file.Tag = filePath;
                string ext = Path.GetExtension(filePath);
                if (ext == ".cpp")
                {
                    file.ImageIndex = 4;
                    file.SelectedImageIndex = 4;
                }
                else if (ext == ".c")
                {
                    file.ImageIndex = 3;
                    file.SelectedImageIndex = 3;
                }
                else if (ext == ".h")
                {
                    file.ImageIndex = 5;
                    file.SelectedImageIndex = 5;
                }
                if (fileFolder == ProjectFolderType.HEADER)
                {
                    header.Nodes.Add(file);
                    evHeaderFileAdded(filePath);
                  
                    
                }
                else if (fileFolder == ProjectFolderType.SOURCE)
                {
                    source.Nodes.Add(file);
                    evSourceFileAdded(filePath);
                   
                }
                else if (fileFolder == ProjectFolderType.COMMONHEADER)
                {
                    Commonheader.Nodes.Add(file);
                    evHeaderFileAdded(filePath);
                }

            }
        }
     
        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            if (treeProject.SelectedNode.Tag as string == "Source")
            {
                fileDlg.Filter = "C files (*.c)|*.c|CPP files (*.cpp)|*.cpp";
                fileDlg.Multiselect = true;
                fileDlg.ShowDialog();
                
                foreach (string srcfilepath in fileDlg.FileNames)
                {
                    if (File.Exists(srcfilepath))
                    {
                        m_parent.m_data.GUnitDat_AddSourceFiles(srcfilepath);
                        Project_AddFilesToFolder(srcfilepath, ProjectFolderType.SOURCE);
                    }
                }
               
            }
            else if (treeProject.SelectedNode.Tag as string == "Header")
            {
                fileDlg.Filter = "Header files (*.h)|*.h";
                fileDlg.Multiselect = true;
                fileDlg.ShowDialog();
                foreach (string hedfilepath in fileDlg.FileNames)
                {
                    if (File.Exists(hedfilepath))
                    {
                        m_parent.m_data.GUnitDat_AddHeaderFiles(hedfilepath);
                       
                        Project_AddFilesToFolder(hedfilepath, ProjectFolderType.HEADER);
                    }
                }
              
            }
            
           
        }

        private void Project_FileDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is string)
            {
                string filePath = e.Node.Tag as string;
                FileInfo data = m_parent.m_data.GUnitData_getFileInformation(filePath);
                if (data != null)
                {
                    evFileOpened(data);
                }
            }
        }
        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeProject.SelectedNode.Tag is string)
            {
                string filePath = treeProject.SelectedNode.Tag as string;
                FileInfo data = m_parent.m_data.GUnitData_getFileInformation(filePath);
                if (data != null)
                {
                    evFileRemoved(data);
                    
                    treeProject.Nodes.Remove(treeProject.SelectedNode);
                }
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuildConfig bldConfig = new BuildConfig(m_parent);
            bldConfig.ShowDialog();
        }

        private void addCommonHeaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Header files (*.h)|*.h";
            fileDlg.Multiselect = true;
            fileDlg.ShowDialog();
            foreach (string hedfilepath in fileDlg.FileNames)
            {
                if (File.Exists(hedfilepath))
                {
                    m_parent.m_data.GUnitDat_AddCommonHeaders(hedfilepath,true);
                    m_parent.m_data.GUnitDat_AddIncludePaths(Path.GetDirectoryName(hedfilepath));
                    Project_AddFilesToFolder(hedfilepath, ProjectFolderType.COMMONHEADER);
                    
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = treeProject.SelectedNode.Tag as string;
            if (file != "Common")
            {
                m_parent.m_data.m_Project.m_SolnData.m_CommonHeadersList.Remove(file);
                treeProject.Nodes.Remove(treeProject.SelectedNode);
            }
        }

        private void showCoverageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeProject.SelectedNode.Tag is string)
            {
                string filePath = treeProject.SelectedNode.Tag as string;
                FileInfo data = m_parent.m_data.GUnitData_getFileInformation(filePath);
                if (data != null)
                {
                    evCoverageRequest(data);
                }
            }
            
        }

        private void addNewFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_file = new SaveFileDialog();
            m_file.CheckFileExists = false;
            m_file.Filter = "C++ Files (*.cpp)|*.cpp|C Files (*.c)|*.c|Header Files (*.h)|*.h";
           

            if (m_file.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(m_file.FileName))
                {
                    CodeGenerator.addFileHeader(
                                                 writer,
                                                 Path.GetFileName(m_file.FileName),
                                                 "New Gunit File" ,
                                                 System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                                                 "1.0",
                                                 DateTime.UtcNow.Date.ToString()
                                                );
                    writer.Close();
                }
                if (treeProject.SelectedNode.Tag is string)
                {
                    string tagValue = treeProject.SelectedNode.Tag as string;
                    if (tagValue == "Source")
                    {
                        Project_AddFilesToFolder(m_file.FileName, ProjectFolderType.SOURCE);
                    }
                    else if (tagValue == "Header")
                    {
                        Project_AddFilesToFolder(m_file.FileName, ProjectFolderType.HEADER);
                    }
                }
            }
        }
        private void AddNewFileOK(object sender, CancelEventArgs e)
        {
            
        }
    }
}
