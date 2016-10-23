using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using ASTBuilder.Interfaces;
using ASTBuilder.ConcreteClasses;
using GUnitFramework.Interfaces;
using System.IO;
namespace CParser
{
    public partial class ParserUi : DockContent
    {
        IParser m_parser;
        ICGunitHost m_Host;
        List<ICCodeDescription> m_codeDescriptions = new List<ICCodeDescription>();
        public ParserUi()
        {
            InitializeComponent();
        }
        public ParserUi(IParser parser,ICGunitHost host)
        {
            InitializeComponent();
            m_parser = parser;
            m_Host = host;
            m_Host.evProjectStatus += new onProjectStatus(Owner_evProjectStatus);

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
        private void ParserUi_Load(object sender, EventArgs e)
        {
            updateParseButton();
            this.Text = m_parser.PluginName;
            m_Host.PropertyChanged+=new PropertyChangedEventHandler(Host_PropertyChanged);
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
            if (null != m_parser)
            {
                if (null != m_Host)
                {
                    m_Host.CodeDescriptions.Clear();
                    m_codeDescriptions.Clear();
                    btnParse.Enabled = false;
                    foreach (string file in m_Host.SelectedFiles)
                    {

                        m_parser.ParseFile(file);
                        m_codeDescriptions.Add(m_parser.CodeDescription);


                    }
                    m_Host.CodeDescriptions.AddRange(m_codeDescriptions);
                    btnParse.Enabled = true;
                }

            }
        }
        private void btnRunParser_Click(object sender, EventArgs e)
        {
           
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            try
            {
                treeOutline.Nodes.Clear();
                PerformParsing();
                treeOutline.Nodes.Add(addOutlineTree(m_Host.CodeDescriptions));
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
        TreeNode getcalledFunctionNode(ICFunction function)
        {
            TreeNode functionNode1 = new TreeNode(function.Name);
            try
            {
                
                functionNode1.Tag = function;
                functionNode1.ImageIndex = 6;
                functionNode1.SelectedImageIndex = 6;
                TreeNode returnNode1 = new TreeNode(function.ReturnValue.Name);
                returnNode1.Tag = function.ReturnValue;
                returnNode1.ImageIndex = 2;
                returnNode1.SelectedImageIndex = 2;
                functionNode1.Nodes.Add(returnNode1);
                foreach (ICVariable arg in function.Parameters)
                {
                    if (null != arg)
                    {
                        if (arg.Type != null)
                        {
                            TreeNode argument1 = new TreeNode(arg.Type.Name + ":" + arg.Name);
                            argument1.Tag = arg;
                            argument1.ImageIndex = 3;
                            argument1.SelectedImageIndex = 3;
                            functionNode1.Nodes.Add(argument1);
                        }
                    }

                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            return functionNode1;
        }
        TreeNode getFunctionNode(ICFunction function)
        {
            TreeNode functionNode = new TreeNode(function.Name);
            try
            {
                
                functionNode.Tag = function;
                functionNode.ImageIndex = 1;
                functionNode.SelectedImageIndex = 1;

                TreeNode returnNode = new TreeNode(function.ReturnValue.Name);
                returnNode.Tag = function.ReturnValue;
                returnNode.ImageIndex = 2;
                returnNode.SelectedImageIndex = 2;
                functionNode.Nodes.Add(returnNode);
                foreach (ICVariable arg in function.Parameters)
                {
                    TreeNode argument = new TreeNode(arg.Type.Name + ":" + arg.Name);
                    argument.Tag = arg;
                    argument.ImageIndex = 3;
                    argument.SelectedImageIndex = 3;
                    functionNode.Nodes.Add(argument);

                }
                TreeNode Called = new TreeNode("CalledFunctions");

                foreach (ICFunction calledFunction in function.CalledFunctions)
                {

                    Called.Nodes.Add(getcalledFunctionNode(calledFunction));
                }

                TreeNode LocalVariables = new TreeNode("LocalVariables");
                foreach (ICVariable variable in function.LocalVariables)
                {
                    TreeNode variableNode1 = new TreeNode(variable.Name);
                    variableNode1.Tag = variable;
                    variableNode1.ImageIndex = 5;
                    variableNode1.SelectedImageIndex = 5;
                    LocalVariables.Nodes.Add(variableNode1);
                }

                functionNode.Nodes.Add(LocalVariables);
                functionNode.Nodes.Add(Called);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            return functionNode;


        }
        private TreeNode addOutlineTree(List<ICCodeDescription> descList)
        {
            TreeNode root = new TreeNode("Outline");

            foreach (ICCodeDescription desc in descList)
            {
                if (File.Exists(desc.FileName))
                {

                    TreeNode FileNode = new TreeNode(Path.GetFileName(desc.FileName));
                    FileNode.Tag = desc;
                    FileNode.ImageIndex = 0;
                    FileNode.SelectedImageIndex = 0;
                    foreach (ICVariable variable in desc.GlobalVariables)
                    {
                        TreeNode variableNode = new TreeNode(variable.Name);
                        variableNode.Tag = variable;
                        variableNode.ImageIndex = 4;
                        variableNode.SelectedImageIndex = 4;
                        FileNode.Nodes.Add(variableNode);
                    }
                    
                    foreach (ICFunction function in desc.Functions)
                    {
                        FileNode.Nodes.Add(getFunctionNode(function));

                    }
                   
                    root.Nodes.Add(FileNode);
                }
            }

            return root;
        }
    }
}
