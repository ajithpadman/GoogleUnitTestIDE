using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace GUnit
{
    public partial class ExtFunctionIf : DockContent
    {
        GUnit m_parent;
        public ExtFunctionIf(GUnit l_parent)
        {
            InitializeComponent();
            m_parent = l_parent;
        }

        private void ExtFunctionIf_Load(object sender, EventArgs e)
        {
            m_parent.evCalledFunctionListAvailable += (ExtFunctionIf_CalledfunctionList);
            m_parent.evCloseAllForms += (CloseThisForm);
            treeExtFunctionIF.CheckBoxes = true;
        }
        private void ExtFunctionIf_CloseEvents()
        {
            m_parent.evCalledFunctionListAvailable -= (ExtFunctionIf_CalledfunctionList);
            m_parent.evCloseAllForms -= (CloseThisForm);
        }
        private void CloseThisForm()
        {
            ExtFunctionIf_CloseEvents();
            this.Close();
        }
        private void ExtFunctionIf_CalledfunctionList(FunctionalInterface function)
        {
            treeExtFunctionIF.Nodes.Clear();
            foreach (FunctionalInterface called in function.m_CalledFunctionList)
            {
                
                TreeNode CalledFunction = new TreeNode(called.m_FunctionName);
                CalledFunction.Tag = called;
                CalledFunction.ImageIndex = 0;
                CalledFunction.SelectedImageIndex = 0;
                if (ExtFunctionIf_checkIfFunctionPresent(called.m_FunctionName))
                {
                    CalledFunction.ForeColor = Color.Green;
                }
                else
                {
                    CalledFunction.ForeColor = Color.Red;
                }
                treeExtFunctionIF.Nodes.Add(CalledFunction);
            }
        }
        private bool ExtFunctionIf_checkIfFunctionPresent(string functionName)
        {
            bool l_result = false;
            foreach (FileInfo file in m_parent.m_data.m_ProjectHashTable.Values)
            {
                foreach (UnitInfo unit in file.m_UnitList)
                {
                    foreach (FunctionalInterface function in unit.m_functionDefinitionList)
                    {
                        if (functionName == function.m_FunctionName)
                        {
                            l_result = true;
                            return true;

                        }
                    }
                }
            }
            return l_result;

        }

        private void ExtFunctIf_NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is FunctionalInterface)
            {
                FunctionalInterface function = e.Node.Tag as FunctionalInterface;
                AddNewFunction frmAddFunc = new AddNewFunction(m_parent, function);
                frmAddFunc.ShowDialog();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeExtFunctionIF.Nodes)
            {
                if (node.Checked == true)
                {
                    if (node.Tag is FunctionalInterface)
                    {
                        FunctionalInterface function = node.Tag as FunctionalInterface;
                        AddNewFunction frmAddFunc = new AddNewFunction(m_parent, function);
                        frmAddFunc.ShowDialog();
                    }
                }
            }
        }
       
    }
}
