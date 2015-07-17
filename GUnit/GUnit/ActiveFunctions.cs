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
    public partial class ActiveFunctions : DockContent
    {
        public GUnitData m_data;
        private GUnit m_parent;
        FileInfo m_ActiveFileData;
        public delegate void onFunctionDisplayed(FileInfo file);
        public event onFunctionDisplayed evFunctionListDisplayed;
   
        public delegate void onFileScrollToLine(FunctionalInterface function);
        public event onFileScrollToLine evScrollDocument;

        


        public ActiveFunctions(GUnit main)
        {
            InitializeComponent();
            m_data = main.m_data;
            m_parent = main;
            
        }
        private void Functions_CloseEvents()
        {
            m_parent.evFunctionListUpdate -= (GUnit_displayFunctions);
            m_parent.evFileFocusChange -= (GUnit_UpdateNodes);
            m_parent.evCloseAllForms -= (CloseThisForm);
        }
        private void Functions_Load(object sender, EventArgs e)
        {
            treeFunctions.CheckBoxes = true;
            m_parent.evFunctionListUpdate += (GUnit_displayFunctions);
            m_parent.evFileFocusChange += (GUnit_UpdateNodes);
            m_parent.evCloseAllForms += (CloseThisForm);
        }
        public void CloseThisForm()
        {
            Functions_CloseEvents();
            this.Close();
        }
        public void GUnit_UpdateNodes(FileInfo fileData)
        {
            if (fileData == null)
            {
                treeFunctions.Nodes.Clear();
            }
            else
            {
                foreach (UnitInfo info in fileData.m_UnitList)
                {
                    foreach (FunctionalInterface function in info.m_functionDefinitionList)
                    {
                        string functionPrototype = function.m_ReturnType + " " + function.m_FunctionName + " (";
                        for (int i = 0; i < function.m_argumentTypes.Count; i++)
                        {
                            if (i == function.m_argumentTypes.Count - 1)
                            {
                                functionPrototype += function.m_argumentTypes[i];
                            }
                            else
                            {
                                functionPrototype += function.m_argumentTypes[i] + ",";
                            }
                        }
                        functionPrototype += ")";
                        TreeNode node = new TreeNode(functionPrototype.Trim());
                        node.Tag = function;
                        node.ImageIndex = 0;
                        node.SelectedImageIndex = 0;
                        treeFunctions.Nodes.Add(node);
                        
                    }
                }
                m_ActiveFileData = fileData;
                
            }
        }
        public void GUnit_displayFunctions(FileInfo fileData)
        {
            if (fileData != null)
            {
                GUnit_UpdateNodes(fileData);
                evFunctionListDisplayed(fileData);
            }
            else
            {
                treeFunctions.Nodes.Clear();
            }
        }
       
        public void clearTreeNodes()
        {
            treeFunctions.Nodes.Clear();
        }
        private void FunctionNode_Clicked(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is FunctionalInterface)
                {
                    FunctionalInterface function = e.Node.Tag as FunctionalInterface;
                    FunctionPrototype frmFunctionProto = new FunctionPrototype(function,m_parent);
                    frmFunctionProto.ShowDialog();
                }
        }

        private void FunctionNode_Click(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Tag is FunctionalInterface)
                {
                    FunctionalInterface function = e.Node.Tag as FunctionalInterface;
                    evScrollDocument(function);
                }
            }
            catch
            {

            }
        }
    }
}
