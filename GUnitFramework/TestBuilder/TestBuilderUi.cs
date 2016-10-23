using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUnitFramework.Interfaces;
using ASTBuilder.Interfaces;
using System.IO;
namespace TestBuilder
{
    public partial class TestBuilderUi : Form
    {
        TestBuilder m_plugin;
        public TestBuilderUi()
        {
            InitializeComponent();
        }
        public TestBuilderUi(TestBuilder plugin)
        {
            InitializeComponent();
            m_plugin = plugin;
        }

        private void TestBuilderUi_Load(object sender, EventArgs e)
        {
            foreach (ICCodeDescription desc in m_plugin.Owner.CodeDescriptions)
            {
                TreeNode FileNode = new TreeNode(Path.GetFileName(desc.FileName));
                FileNode.ImageIndex = 0;
                FileNode.SelectedImageIndex = 0;
                treeTestCases.Nodes.Add(FileNode);

            }
        }
    }
}
