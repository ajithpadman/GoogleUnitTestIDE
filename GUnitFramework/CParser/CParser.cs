using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using ASTBuilder.Interfaces;
using ASTBuilder.ConcreteClasses;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
namespace CParser
{
    public class CParser:IParser
    {
        ICCodeDescription m_codedescription;
        ICGunitHost m_owner;
        ASTBuilder.ASTBuilder m_ast;
        public CParser()
        {
            
        }
        public ASTBuilder.Interfaces.ICCodeDescription CodeDescription
        {
            get
            {
                return m_codedescription;
            }
            set
            {
                m_codedescription = value;
            }
        }

        public void ParseFile(string fileNames)
        {
            m_ast = new ASTBuilder.ASTBuilder(fileNames);
            m_ast.IncludePaths.AddRange(Owner.ProjectData.IncludePaths.ToArray());
            m_ast.Defines.AddRange(Owner.ProjectData.Defines.ToArray());
            m_ast.PreIncludeFiles.AddRange(Owner.ProjectData.PreIncludes.ToArray());
            m_ast.ParseFile();
            CodeDescription = m_ast.CodeDescription;
        }

        public ParserStatus Status
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool HandleProjectSession(ProjectStatus status)
        {
            return true;
        }

        public ICGunitHost Owner
        {
            get
            {
                return m_owner;
            }
            set
            {
                m_owner = value;
            }
        }

        public string PluginName
        {
            get
            {
                return "Parser " + Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public PluginType PluginType
        {
            get
            {
                return PluginType.Parser;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Show(WeifenLuo.WinFormsUI.Docking.DockPanel dock, WeifenLuo.WinFormsUI.Docking.DockState state)
        {
            ParserUi ui = new ParserUi(this,Owner);
            ui.Show(dock, state);
        }
        
        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
          
            return true;
        }
    }
}
