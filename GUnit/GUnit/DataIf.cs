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
    public partial class DataIf : DockContent
    {
        private GUnitData m_data;
        private GUnit m_parent;
        TreeNode main;
        TreeNode Typedefs;
        TreeNode structure;
        TreeNode Unions;
        TreeNode classes;
        TreeNode Enumeration;
        TreeNode Macro;
        TreeNode GlobalVariable;
       
        public DataIf(GUnit parent)
        {
            InitializeComponent();
            m_data = parent.m_data;
            m_parent = parent;
          
        }
        private void Dataif_removeDuplicateNode(TreeNode parentNode, string fileName)
        {
            for (int i = parentNode.Nodes.Count - 1; i >= 0;i-- )
                {
                    if (parentNode.Nodes[i].Tag is string)
                    {
                        string tfile = parentNode.Nodes[i].Tag as string;
                        if (tfile == fileName)
                        {
                            parentNode.Nodes.RemoveAt(i);
                        }
                    }
                }
        }
        public void DataIf_UpdateDataIfNodes(FileInfo file)
        {
            string fileName = Path.GetFileName(file.m_fileName);
            Dataif_removeDuplicateNode(Enumeration, file.m_fileName);
            Dataif_removeDuplicateNode(structure, file.m_fileName);
            Dataif_removeDuplicateNode(Unions, file.m_fileName);
            Dataif_removeDuplicateNode(Typedefs, file.m_fileName);
            Dataif_removeDuplicateNode(Macro, file.m_fileName);
            Dataif_removeDuplicateNode(classes, file.m_fileName);
            Dataif_removeDuplicateNode(GlobalVariable, file.m_fileName);
            if (file.m_DataInterfaceList.m_enumValues.Count != 0)
            {
                
                TreeNode FileEnumNode = new TreeNode(fileName);
                FileEnumNode.Tag = file.m_fileName;
                Enumeration.Nodes.Add(FileEnumNode);
                foreach (string enumvValue in file.m_DataInterfaceList.m_enumValues)
                {
                    FileEnumNode.Nodes.Add(enumvValue);
                }
            }
            if (file.m_DataInterfaceList.m_structuresNames.Count != 0)
            {
                
                TreeNode FileStructNode = new TreeNode(fileName);
                FileStructNode.Tag = file.m_fileName;
                structure.Nodes.Add(FileStructNode);
                foreach (string str in file.m_DataInterfaceList.m_structuresNames)
                {
                    FileStructNode.Nodes.Add(str);
                }
            }
            if (file.m_DataInterfaceList.m_UnionNames.Count != 0)
            {
                
                TreeNode FileUnionNode = new TreeNode(fileName);
                FileUnionNode.Tag = file.m_fileName;
                Unions.Nodes.Add(FileUnionNode);
                foreach (string str in file.m_DataInterfaceList.m_UnionNames)
                {
                    FileUnionNode.Nodes.Add(str);
                }
            }
            if (file.m_DataInterfaceList.m_Typedefs.Count != 0)
            {
               
                TreeNode FileTypedefNode = new TreeNode(fileName);
                FileTypedefNode.Tag = file.m_fileName;
                Typedefs.Nodes.Add(FileTypedefNode);
                foreach (string str in file.m_DataInterfaceList.m_Typedefs)
                {
                    FileTypedefNode.Nodes.Add(str);
                }
            }
            if (file.m_DataInterfaceList.m_MacroNames.Count != 0)
            {

               
                TreeNode FileMacroNode = new TreeNode(fileName);
                FileMacroNode.Tag = file.m_fileName;
                Macro.Nodes.Add(FileMacroNode);
                foreach (string str in file.m_DataInterfaceList.m_MacroNames)
                {
                    FileMacroNode.Nodes.Add(str);
                }
            }
            if (file.m_DataInterfaceList.m_ClassNames.Count != 0)
            {
               
                TreeNode FileClassNode = new TreeNode(fileName);
                FileClassNode.Tag = file.m_fileName;
                classes.Nodes.Add(FileClassNode);
                foreach (string str in file.m_DataInterfaceList.m_ClassNames)
                {
                    FileClassNode.Nodes.Add(str);
                }
            }
            if (file.m_DataInterfaceList.m_GlobalVariables.Count != 0)
            {
              
                TreeNode FileGlobNode = new TreeNode(fileName);
                FileGlobNode.Tag = file.m_fileName;
                GlobalVariable.Nodes.Add(FileGlobNode);
                foreach (string str in file.m_DataInterfaceList.m_GlobalVariables)
                {
                    FileGlobNode.Nodes.Add(str);
                }
            }
           
        }
        public void DataIf_RemoveFileData(string FileName)
        {
            foreach (TreeNode node in treeDataType.Nodes)
            {
                foreach (TreeNode dataType in node.Nodes)
                {
                   for(int i = dataType.Nodes.Count -1; i >= 0 ;i--)
                   {
                       TreeNode file = dataType.Nodes[i];
                       if (file.Tag is string)
                       {
                            string tagValue = file.Tag as string;
                            if (tagValue  == FileName) 
                            {
                                dataType.Nodes.RemoveAt(i);
                            }
                       }
                   }
                    
                    
                }
            }
        }
        private void DataIf_CloseEvents()
        {
            m_parent.evCloseAllForms -= (CloseThisForm);
            m_parent.evSendFileInfo -= (DataIf_UpdateDataIfNodes);
            m_parent.evRemoveFileInfo -= (DataIf_RemoveFileData);
        }
        private void CloseThisForm()
        {
            DataIf_CloseEvents();
            this.Close();
        }
        private void DataTypes_Load(object sender, EventArgs e)
        {
            m_parent.evCloseAllForms += (CloseThisForm);
            m_parent.evSendFileInfo += (DataIf_UpdateDataIfNodes);
            m_parent.evRemoveFileInfo += (DataIf_RemoveFileData);
            main = new TreeNode("Data Interfaces");
            main.ImageIndex = 0;
            main.SelectedImageIndex = 0;
            Typedefs = new TreeNode("User TypeDef");
            Typedefs.ImageIndex = 1;
            Typedefs.SelectedImageIndex = 1;
            structure = new TreeNode("Structure");
            structure.ImageIndex = 2;
            structure.SelectedImageIndex = 2;
            Unions = new TreeNode("Unions");
            Unions.ImageIndex = 2;
            Unions.SelectedImageIndex = 2;
            classes = new TreeNode("Class");
            classes.ImageIndex = 3;
            classes.SelectedImageIndex = 3;
            Enumeration = new TreeNode("Enumeration");
            Enumeration.ImageIndex = 4;
            Enumeration.SelectedImageIndex = 4;
            Macro = new TreeNode("Macro Definition");
            Macro.ImageIndex = 5;
            Macro.SelectedImageIndex = 5;
            GlobalVariable = new TreeNode("Global Variables");
            GlobalVariable.ImageIndex = 6;
            GlobalVariable.SelectedImageIndex = 6;
            main.Nodes.Add(GlobalVariable);
            main.Nodes.Add(Typedefs);
            main.Nodes.Add(structure);
            main.Nodes.Add(Unions);
            main.Nodes.Add(classes);
            main.Nodes.Add(Enumeration);
            main.Nodes.Add(Macro);
            treeDataType.Nodes.Add(main);
        }
           

      
    }
}
