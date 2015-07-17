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
    public partial class FunctionIF : DockContent
    {
        GUnit m_parent;
        TreeNode ClassListData;
        Loading load;
        public delegate void onFileParsed(FileInfo FileInfo);
        public event onFileParsed evFileParsed;

        public FunctionIF(GUnit parent)
        {
            InitializeComponent();
            m_parent = parent;
           
            //ClassListData.Nodes.Clear();
        }
        public void FunctionIf_fileRemoved(string filepath)
        {

            for (int i = ClassListData.Nodes.Count -1; i >=0; i--)
           {
               TreeNode node = ClassListData.Nodes[i];
               if (node.Tag is UnitInfo)
               {
                   UnitInfo file = node.Tag as UnitInfo;
                   if (filepath == file.m_fileName)
                   {
                       ClassListData.Nodes.RemoveAt(i);
                       
                   }
               }
           }

        }
        public List<UnitInfo> Functionif_getMockList()
        {
            List<UnitInfo> m_stubUnitList = new List<UnitInfo>();
            foreach (TreeNode units in ClassListData.Nodes)
            {
                if (units.Checked == true)
                {
                     
                    if (units.Tag is UnitInfo)
                    {
                        UnitInfo unitInf = units.Tag as UnitInfo;
                        unitInf.m_MockFunctionList.Clear();
                        foreach (TreeNode function in units.Nodes)
                        {
                            if (function.Checked == true)
                            {
                                FunctionalInterface func = function.Tag as FunctionalInterface;
                                unitInf.m_MockFunctionList.Add(func);

                            }
                        }
                        m_stubUnitList.Add(unitInf);
                    }
                }
            }
            return m_stubUnitList;
        }

        public void FunctioniF_startParserThread(string filePath)
        {

            
            BackgroundWorker parser = new BackgroundWorker();
            parser.DoWork += new DoWorkEventHandler(FunctionIF_ParseFile);
            parser.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FunctionIF_ParseFileComplete);
            parser.RunWorkerAsync(filePath);
        }
        public void FunctionIF_updateFunctionNode()
        {
            foreach (TreeNode unit in ClassListData.Nodes)
            {
                foreach (TreeNode function in unit.Nodes)
                {
                    if (function.Tag is FunctionalInterface)
                    {
                        FunctionalInterface functIf = function.Tag as FunctionalInterface;
                        if (FunctionIF_checkifFunctionDefined(functIf))
                        {
                            function.ForeColor = Color.Green;
                        }
                        else
                        {
                            function.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
        public void CloseThisForm()
        {
            FunctionIf_CloseEvents();
            this.Close();
        }
        public bool FunctionIF_checkifFunctionDefined(FunctionalInterface function)
        {
            bool result = false;
           
            foreach (FileInfo file in m_parent.m_data.m_ProjectHashTable.Values)
            {
                foreach (UnitInfo unit in file.m_UnitList)
                {
                    foreach (FunctionalInterface functionDefine in unit.m_functionDefinitionList)
                    {
                        if (
                            function.m_FunctionName == functionDefine.m_FunctionName &&
                            function.m_ReturnType == functionDefine.m_ReturnType &&
                            function.m_argumentTypes.Count == functionDefine.m_argumentTypes.Count
                            )
                        {
                            for (int i = 0; i < function.m_argumentTypes.Count; i++)
                            {
                                if (function.m_argumentTypes[i] != functionDefine.m_argumentTypes[i])
                                {
                                    return false;
                                }
                            }
                            return true;

                        }
                        
                    }
                   

                }
            }
            return result;
        }
        private void updateFunctionNodeImage(UnitInfo Class, FunctionalInterface function, TreeNode functionNode)
        {
            if (Class.m_IsClass == true)
            {
                if (function.m_AccessScope == "public")
                {
                    if (function.m_IsVirtual == true)
                    {
                        functionNode.ImageIndex = 7;
                        functionNode.SelectedImageIndex = 7;
                    }
                    else
                    {
                        functionNode.ImageIndex = 3;
                        functionNode.SelectedImageIndex = 3;
                    }
                }
                else
                {
                    if (function.m_IsVirtual == true)
                    {
                        functionNode.ImageIndex = 6;
                        functionNode.SelectedImageIndex = 6;
                    }
                    else
                    {
                        functionNode.ImageIndex = 2;
                        functionNode.SelectedImageIndex = 2;
                    }
                }
            }
            else
            {
                functionNode.ImageIndex = 5;
                functionNode.SelectedImageIndex = 5;
            }
        }
        
        public void FunctionIF_ParseFileComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                FileInfo fileInformation = e.Result as FileInfo;
                FileInfo data = m_parent.m_data.GUnitData_getFileInformation(fileInformation.m_fileName);
                if (data != null)
                {
                    for (int i = ClassListData.Nodes.Count - 1; i >= 0; i--)
                    {

                        if (ClassListData.Nodes[i].Tag is UnitInfo)
                        {
                            UnitInfo className = ClassListData.Nodes[i].Tag as UnitInfo;
                            if (className.m_fileName == fileInformation.m_fileName)
                            {
                                ClassListData.Nodes.Remove(ClassListData.Nodes[i]);
                                break;
                            }
                        }
                    }
                }
                foreach (UnitInfo Class in fileInformation.m_UnitList)
                {
                    if (Class.m_functionPrototypeList.Count != 0)
                    {
                        TreeNode newClass = new TreeNode(Class.m_className);
                        newClass.Tag = Class;

                        if (Class.m_IsClass == true)
                        {
                            newClass.ImageIndex = 1;
                            newClass.SelectedImageIndex = 1;
                        }
                        else
                        {
                            newClass.ImageIndex = 4;
                            newClass.SelectedImageIndex = 4;
                        }

                        foreach (FunctionalInterface function in Class.m_functionPrototypeList)
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
                            TreeNode functionNode = new TreeNode(functionPrototype);

                            if (FunctionIF_checkifFunctionDefined(function))
                            {
                                functionNode.ForeColor = Color.Green;
                            }
                            else
                            {
                                functionNode.ForeColor = Color.Red;
                            }
                            functionNode.Tag = function;
                            updateFunctionNodeImage(Class, function, functionNode);
                            newClass.Nodes.Add(functionNode);


                        }
                        ClassListData.Nodes.Add(newClass);
                    }
                }
                evFileParsed(fileInformation);
            }
            catch (Exception err)
            {
                m_parent.GUnit_updateConsole(err.ToString());
            }
          
        }
        public void FunctionIF_ParseFileAgain(FileInfo file)
        {
            CtagParser parser = new CtagParser(m_parent);
            file.m_UnitList = parser.GUnit_findFileFunctionIF(file.m_fileName);
            file.m_DataInterfaceList = parser.GUnit_getDataInterfaceList(file.m_fileName);
            m_parent.m_data.GUnitData_UpdateProjectTable(file.m_fileName, file);
           
        }
        public void FunctionIF_ParseFile(object sender, DoWorkEventArgs e)
        {
            
            string filePath = e.Argument as string;
            CtagParser parser = new CtagParser(m_parent);
            FileInfo fileInformation = new FileInfo();
            fileInformation.m_fileName = filePath;
            try
            {
                m_parent.GUnit_UpdateStatus("Finding Function list for " + Path.GetFileName(filePath));
                fileInformation.m_UnitList = parser.GUnit_findFileFunctionIF(filePath);
                m_parent.GUnit_UpdateStatus("Finding Data Interface list for " + Path.GetFileName(filePath));
                fileInformation.m_DataInterfaceList = parser.GUnit_getDataInterfaceList(filePath);
            }
            catch(Exception error)
            {
                m_parent.GUnit_updateConsole(error.ToString());
            }
            e.Result = fileInformation;
            
        }
        private void FunctionIf_CloseEvents()
        {
            m_parent.evFileReceived -= (FunctioniF_startParserThread);
            m_parent.evRemoveFileInfo -= (FunctionIf_fileRemoved);
            m_parent.evUpdateFunctionIf -= (FunctionIF_updateFunctionNode);
            m_parent.evCloseAllForms -= (CloseThisForm);
        }
        private void FunctionIF_Load_1(object sender, EventArgs e)
        {
            m_parent.evFileReceived += (FunctioniF_startParserThread);
            m_parent.evRemoveFileInfo += (FunctionIf_fileRemoved);
            m_parent.evUpdateFunctionIf += (FunctionIF_updateFunctionNode);
            m_parent.evCloseAllForms += (CloseThisForm);
            ClassListData = new TreeNode("Functional Interfaces");
            ClassListData.ImageIndex = 0;
            ClassListData.SelectedImageIndex = 0;
            treeClasses.Nodes.Add(ClassListData);
            treeClasses.CheckBoxes = true;
            ClassListData.Nodes.Clear();
            ClassListData.Tag = "ParentFunctions";
        }

        private void FunctionIf_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            if (e.Node.Checked == true)
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = true;
                    
                }
            }
            else
            {
                foreach (TreeNode node in e.Node.Nodes)
                {
                    node.Checked = false;

                }
            }
            if (e.Node.Tag is FunctionalInterface)
            {
                if (e.Node.Checked == true)
                {
                    e.Node.Parent.Checked = true;
                }
               
            }
                
        }

        private void FunctionIf_CorrectFunctionDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is FunctionalInterface)
            {
                int functionIndex = -1;
                int unitIndex = -1;
                FunctionalInterface function = e.Node.Tag as FunctionalInterface;
                FileInfo data = m_parent.m_data.GUnitData_getFileInformation(function.m_FileName);
               
                foreach (UnitInfo unit in data.m_UnitList)
                {
                    functionIndex = unit.m_functionPrototypeList.Select((item, index) => new
                    {
                        ItemName = item.m_FunctionName,
                        ItemReturn = item.m_ReturnType,
                        ItemSignature = item.m_Signature,
                        Position = index
                    }).Where(i => i.ItemName == function.m_FunctionName && i.ItemReturn == function.m_ReturnType && i.ItemSignature == function.m_Signature)
                     .First()
                    .Position;
                    unitIndex++;
                }
                FunctionPrototype functionCorrection = new FunctionPrototype(function, m_parent);
                functionCorrection.ShowDialog();
                if (functionIndex != -1)
                {
                    data.m_UnitList[unitIndex].m_functionPrototypeList[functionIndex] = function;
                }
                m_parent.m_data.GUnitData_UpdateProjectTable(function.m_FileName, data);
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
                e.Node.Text = functionPrototype;
            }
        }
    }
}
