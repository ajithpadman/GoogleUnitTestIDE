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
using ASTBuilder.Interfaces;
namespace MockGenerator
{
    public partial class MockeGeneratorUi : DockContent
    {
        IMockGenrator m_mockGenerator;
        public MockeGeneratorUi(IMockGenrator mockGenerator)
        {
            InitializeComponent();
            m_mockGenerator = mockGenerator;
        }
        public MockeGeneratorUi()
        {
            InitializeComponent();
        }

        private void MockeGeneratorUi_Load(object sender, EventArgs e)
        {
            if (null != m_mockGenerator)
            {
                if (null != m_mockGenerator.Owner)
                {
                    m_mockGenerator.Owner.PropertyChanged += new PropertyChangedEventHandler(Owner_PropertyChanged);
                     m_mockGenerator.Owner.evProjectStatus+=new onProjectStatus(Owner_evProjectStatus);
                }
            }
        }
        void Owner_evProjectStatus(ProjectStatus status, object data)
        {
            switch (status)
            {
                case ProjectStatus.OPEN:
                    treeMock.Nodes.Clear();
                    break;
                case ProjectStatus.NEW:
                    treeMock.Nodes.Clear();
                    break;
                case ProjectStatus.CLOSE:
                    treeMock.Nodes.Clear();
                    break;
                case ProjectStatus.SAVE:
                   //do nothing
                    break;
                default:
                    break;
            }
        }
        void Owner_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SELECTED_FILE":
                    addFileTree();
                    break;
                default:
                    break;
            }

        }
        private void addFileTree()
        {
            if (null != m_mockGenerator)
            {
                if (null != m_mockGenerator.Owner)
                {
                    treeMock.Nodes.Clear();
                    foreach (string file in m_mockGenerator.Owner.SelectedFiles)
                    {
                        if(File.Exists(file) )
                        {
                            TreeNode node = new TreeNode(Path.GetFileName(file));
                            node.Tag = file;
                            treeMock.Nodes.Add(node);
                        }
                    }
                }
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (m_mockGenerator.Owner.CodeDescriptions.Count != 0)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.SelectedPath = m_mockGenerator.Owner.ProjectData.ProjectPath;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    m_mockGenerator.Path = dlg.SelectedPath;
                    foreach (ICCodeDescription desc in m_mockGenerator.Owner.CodeDescriptions)
                    {
                        m_mockGenerator.generateMock(desc);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Parsed Files description found. Parse files before generating mock");
            }

        }
    }
}
