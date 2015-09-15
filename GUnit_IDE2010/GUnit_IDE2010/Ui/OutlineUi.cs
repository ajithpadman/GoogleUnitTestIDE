using Gunit.DataModel;
using GUnit_IDE2010.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gunit.Ui
{
    public partial class OutlineUi : GUnitSideBarBase
    {
        public delegate void onParserDiagnostics(IEnumerable<ClangSharp.Diagnostic> errors);
        public event onParserDiagnostics evParserDiagnostics = delegate { };
        public delegate void onCalledMethodLocation(CodeLocation location);
        public event onCalledMethodLocation evCalledLocation = delegate { };
        public delegate void onRefresh();
        public event onRefresh evRefresh = delegate { };
        public OutlineUi(DataModelBase model):base(model)
        {
            InitializeComponent();
        }

        private void FunctionsUi_Load(object sender, EventArgs e)
        {
            this.treeFunctions.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.treeFunctions.DrawNode += new DrawTreeNodeEventHandler(treeview1_DrawNode);
            m_model.PropertyChanged += FunctionUiModel_PropertyChanged;
            
        }
        void treeview1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if(e.Node.Tag is ProjectFiles)
            e.Node.HideCheckBox();
            e.DrawDefault = true;
        }
        

        /// <summary>
        /// Update the TreeNode on parser status
        /// </summary>
        /// <param name="status"></param>
        private void updateFunctionUi(ParserStatus status)
        {
            if (status == ParserStatus.ParserComplete)
            {

                btnRefresh.Enabled = true;
                updateUnitNodes();
            }
            else if (status == ParserStatus.ParserRunning)
            {
                btnRefresh.Enabled = false;
            }
        }
        private void FunctionUiModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            

        }
        public override void ParserRunning()
        {
            base.ParserRunning();
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    updateFunctionUi(ParserStatus.ParserRunning);
                });
            }
            else
            {
                updateFunctionUi(ParserStatus.ParserRunning);
            }
        }
        public override void ParserComplete()
        {
            base.ParserComplete();
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    updateFunctionUi(ParserStatus.ParserComplete);
                });
            }
            else
            {
                updateFunctionUi(ParserStatus.ParserComplete);
            }
        }
        public override void closeProject()
        {
            base.closeProject();
            treeFunctions.Nodes.Clear();
        }
        public override void OpenProject()
        {
            base.OpenProject();
            closeProject();
        }
        public override void newProject()
        {
            base.newProject();
            closeProject();
        }
        public override void OpenProjectComplete()
        {
            base.OpenProjectComplete();
           
        }
        public override void saveProject()
        {
            base.saveProject();
           
        }
        public override void AddFile(string file)
        {
            base.AddFile(file);
           

        }
        public override void RemoveFile(string l_currentFile)
        {
            base.RemoveFile(l_currentFile);
            
        }
         /// <summary>
        /// update the tree node of function on change 
        /// </summary>
        private void updateUnitNodes()
        {
            treeFunctions.Nodes.Clear();
            treeFunctions.CheckBoxes = true;

           
            foreach (TreeNode node in ((OutlineDataModel)m_model).Tree.Nodes)
            {

                treeFunctions.Nodes.Add(node);
                UpdateNodeColor(node);
            }
           
        }
        public List<Object> getSelectedNodes()
        {
            List<Object> nodes;
            nodes = treeFunctions.Nodes.Descendants()
                       .Where(n => n.Checked)
                       .Select(n => n.Tag)
                       .ToList<Object>();
            
            return nodes;
        }
        
        private void UpdateNodeColor(TreeNode treeNode)
        {
            //treeNode.HideCheckBox();
            DBManager dbmgr = new DBManager();
            GUnitDB Database = dbmgr.connectToDataBase(((OutlineDataModel)m_model).ProjectModel.DBPath);
            foreach (TreeNode tn in treeNode.Nodes)
            {
               if (tn.Tag is GlobalMethods)
               {
                   GlobalMethods m = tn.Tag as GlobalMethods;
                   if (m.Methods.IsDefined)
                   {
                       tn.HideCheckBox();
                   }
               }
               else if (tn.Tag is Classes == false)
               {
                   tn.HideCheckBox();
               }



                if (tn.Tag is MethodCalls)
                {
                    MethodCalls method = tn.Tag as MethodCalls;
                    IEnumerable<Methods> values = from child in Database.Methods
                                 where (
                                          child.ReturnType == method.ReturnType && 
                                          child.EntityName == method.EntitiyName &&
                                          child.Parameters == method.Parameters &&
                                          child.IsDefined == true
                                        )
                                 select child;
                    if (values.Count() > 0)
                    {
                       
                        foreach (Methods m in values)
                        {
                            if (method.IsCxxMethod)
                            {
                                IEnumerable<MemberMethods> MemberMethods = from member in Database.MemberMethods
                                                                           where (
                                                                                   member.MethodID == m.ID
                                                                                 )
                                                                           select member;
                                if (MemberMethods.Count() == 0)
                                {

                                    tn.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    tn.ForeColor = System.Drawing.Color.Green;
                                }

                            }
                            else
                            {
                                IEnumerable<GlobalMethods> globalMethods = from parent in Database.GlobalMethods
                                                                           where (
                                                                                   parent.MethodID == m.ID
                                                                                 )
                                                                           select parent;
                                if (globalMethods.Count() == 0)
                                {
                                    
                                    tn.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    tn.ForeColor = System.Drawing.Color.Green;
                                }
                            }
                        }
                    }
                    else
                    {
                        tn.ForeColor = System.Drawing.Color.Red;
                    }
                }
                UpdateNodeColor(tn);
            }
        }
        
        /// <summary>
        /// Refresh button press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                evRefresh();
            }
            catch
            {

            }
        }
        /// <summary>
        /// Event handler for Double click on the TreeNode 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionsUi_NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.Tag is MethodPrototype)
                {
                    MethodPrototype function = e.Node.Tag as MethodPrototype;
                    evCalledLocation(function.m_location);
                }
            }
            catch
            {

            }
        }
    }
}
