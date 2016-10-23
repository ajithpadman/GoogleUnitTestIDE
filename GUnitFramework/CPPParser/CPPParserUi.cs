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
using CPPASTBuilder.Interfaces;
using System.IO;
namespace CPPParser
{
    public partial class CPPParserUi : DockContent
    {
        ICppParser m_Parser;
        ICGunitHost m_Host;
        List<ICppCodeDescription> m_codeDescriptions = new List<ICppCodeDescription>();
        public CPPParserUi()
        {
            InitializeComponent();
        }
        public CPPParserUi(ICppParser parser)
        {
            InitializeComponent();
            m_Parser = parser;
            m_Host = m_Parser.Owner;
        }
        void Owner_evProjectStatus(ProjectStatus status, object data)
        {
            switch (status)
            {
                case ProjectStatus.OPEN:
                    updateParseButton();
                    break;
                case ProjectStatus.NEW:
                    updateParseButton();
                    break;
                case ProjectStatus.CLOSE:
                    updateParseButton();
                    break;
                case ProjectStatus.SAVE:
                    updateParseButton();
                    break;
                default:
                    break;
            }
        }
        private void updateParseButton()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (m_Host.SelectedFiles.Count == 0)
                    {
                        btnParse.Enabled = false;
                    }
                    else
                    {
                        btnParse.Enabled = true;
                    }
                });
            }
            else
            {
                if (m_Host.SelectedFiles.Count == 0)
                {
                    btnParse.Enabled = false;
                }
                else
                {
                    btnParse.Enabled = true;
                }
            }

        }
        private void CPPParserUi_Load(object sender, EventArgs e)
        {
            updateParseButton();
            this.Text = m_Parser.PluginName;
            m_Host.evProjectStatus += new onProjectStatus(Owner_evProjectStatus);
            m_Host.PropertyChanged += new PropertyChangedEventHandler(Host_PropertyChanged);
        }
        void Host_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SELECTED_FILE":
                    updateParseButton();
                    break;
                default:
                    break;
            }
        }
        private void PerformParsing()
        {
            if (null != m_Parser)
            {
                if (null != m_Host)
                {
                    m_Host.CPPCodeDescriptions.Clear();
                    m_codeDescriptions.Clear();
                    btnParse.Enabled = false;
                    foreach (string file in m_Host.SelectedFiles)
                    {
                        ICppCodeDescription description = m_Parser.parseFile(file);
                        if (null != description)
                        {
                            m_codeDescriptions.Add(description);
                        }
                       


                    }
                    if (m_codeDescriptions.Count != 0)
                    {
                        m_Host.CPPCodeDescriptions.AddRange(m_codeDescriptions);
                    }
                    btnParse.Enabled = true;
                }

            }
        }
        private TreeNode createMemberVariableNode(IMemberVariable variable)
        {
            TreeNode memberFieldNode = new TreeNode(variable.VariableName);
            TreeNode type = new TreeNode(variable.DataType.Name);
            setScopeSpecificImageForNodes(variable, memberFieldNode);
            memberFieldNode.Nodes.Add(type);
            return memberFieldNode;
        }
        private void setScopeSpecificImageForNodes(IMember member, TreeNode node)
        {
            if (member.AccessScope == ClangSharp.AccessSpecifier.Private)
            {
                node.ImageIndex = 9;
                node.SelectedImageIndex = 9;

            }
            else if (member.AccessScope == ClangSharp.AccessSpecifier.Protected)
            {
                node.ImageIndex = 10;
                node.SelectedImageIndex = 10;
            }
            else if (member.AccessScope == ClangSharp.AccessSpecifier.Public)
            {
                node.ImageIndex = 11;
                node.SelectedImageIndex = 11;
            }
        }
        private TreeNode createMethodNode(IMemberMethod method)
        {
            TreeNode Methodnode = new TreeNode(method.MethodName);
            setScopeSpecificImageForNodes(method, Methodnode);
            foreach (ICppDataType type in method.Parameters)
            {
                TreeNode argument = new TreeNode(type.Name);
                argument.ImageIndex = 6;
                argument.SelectedImageIndex = 6;
                Methodnode.Nodes.Add(argument);
            }
            TreeNode returnValue = new TreeNode(method.ReturnValue.Name);
            returnValue.ImageIndex = 7;
            returnValue.SelectedImageIndex = 7;
            Methodnode.Nodes.Add(returnValue);
            return Methodnode;
        }
        private TreeNode createClassNode(IClass Class)
        {
            TreeNode Classnode = new TreeNode(Class.Name);
            if (Class.IsTempleteClass)
            {
                Classnode.ImageIndex = 4;
                Classnode.SelectedImageIndex = 4;

            }
            else
            {
                Classnode.ImageIndex = 2;
                Classnode.SelectedImageIndex = 2;
            }
            if (Class.MemeberMethods.Count != 0)
            {
                TreeNode methods = new TreeNode("Member Methods");
                methods.ImageIndex = 5;
                methods.SelectedImageIndex = 5;
                foreach (IMemberMethod method in Class.MemeberMethods)
                {

                    if (method.IsPureVirtual)
                    {
                        Classnode.ImageIndex = 3;
                        Classnode.SelectedImageIndex = 3;
                    }
                    methods.Nodes.Add(createMethodNode(method));
                }
                Classnode.Nodes.Add(methods);
            }
            if (Class.MemberVariables.Count != 0)
            {
                TreeNode Variables = new TreeNode("Member Variables");
                Variables.ImageIndex = 8;
                Variables.SelectedImageIndex = 8;
                foreach (IMemberVariable variable in Class.MemberVariables)
                {
                    Variables.Nodes.Add(createMemberVariableNode(variable));
                }
                Classnode.Nodes.Add(Variables);
            }
            return Classnode;
        }
        private TreeNode createNamespeceNode(INamespace Namespace)
        {
            TreeNode NameSpacenode = new TreeNode(Namespace.Name);
            NameSpacenode.ImageIndex = 1;
            NameSpacenode.SelectedImageIndex = 1;
            foreach (IClass Class in Namespace.Classes)
            {
                NameSpacenode.Nodes.Add(createClassNode(Class));
            }
            foreach (INamespace space in Namespace.Namespaces)
            {
                NameSpacenode.Nodes.Add(createNamespeceNode(space));
            }
            return NameSpacenode;
        }
        private TreeNode addOutlineTree(List<ICppCodeDescription> descList)
        {
            TreeNode root = new TreeNode("Outline");

            foreach (ICppCodeDescription desc in descList)
            {
                if (File.Exists(desc.FileName))
                {

                    TreeNode FileNode = new TreeNode(Path.GetFileName(desc.FileName));
                    FileNode.Tag = desc;
                    FileNode.ImageIndex = 0;
                    FileNode.SelectedImageIndex = 0;
                    foreach (INamespace space in desc.NameSpaces)
                    {
                        FileNode.Nodes.Add(createNamespeceNode(space));
                    }
                    foreach (IClass Class in desc.Classes)
                    {
                        FileNode.Nodes.Add(createClassNode(Class));
                    }
                    root.Nodes.Add(FileNode);
                }
            }

            return root;
        }
        private void btnParse_Click(object sender, EventArgs e)
        {
            try
            {
                treeOutline.Nodes.Clear();
                PerformParsing();
                treeOutline.Nodes.Add(addOutlineTree(m_Host.CPPCodeDescriptions));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
