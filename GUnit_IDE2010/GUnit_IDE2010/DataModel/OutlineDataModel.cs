using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gunit.DataModel
{
    public enum FunctionTreeNodeType
    {
        FileNode,
        GlobalVariableNode,
        GlobalFunctionNode,
        CalledMethodNode,
        NamespaceNode,
        ClassNode,
        memberVariableNode,
        memberMethodNode,
        TypedefNode,
        Interface,
        FunctionNode,
        AccessNode,
        ReturnNode,
        ArgumentsNode,
        ArgumentNode,
        StorageClassNode,
        StructNode,
        DataType
    }
   
    public class OutlineDataModel:DataModelBase
    {
        TreeNode mainNode =null;
        
        ProjectDataModel m_ProjectDataModel;
        
     
        public TreeNode Tree
        {
            get { return mainNode; }
            set { mainNode = value;
           
            }
        }
   
        public OutlineDataModel(ProjectDataModel model):base()
        {
            m_ProjectDataModel = model;
            
        }
      
       
        public ProjectDataModel ProjectModel
        {
            get { return m_ProjectDataModel; }
        }
        
       
          #region method 
       
        
        public override void Datamodel_openProject()
        {
            base.Datamodel_openProject();
            Datamodel_closeProject();
        }
        public override void Datamodel_newProject()
        {
            base.Datamodel_newProject();
            Datamodel_closeProject();
        }
        public override void Datamodel_closeProject()
        {
            base.Datamodel_closeProject();
           
        }
        public override void Datamodel_saveProject()
        {
            base.Datamodel_saveProject();
        }
        public override void Datamodel_openProjectComplete()
        {
            base.Datamodel_openProjectComplete();
        }
        public override void Datamodel_openFile(string fileName)
        {
            base.Datamodel_openFile(fileName);
        }
        public override void Datamodel_closeFile(string fileName)
        {
            base.Datamodel_closeFile(fileName);
        }
        public override void Datamodel_removeFile(string fileName)
        {
            base.Datamodel_removeFile(fileName);
        }

        #endregion


    }
}
