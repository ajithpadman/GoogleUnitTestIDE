using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using CPPASTBuilder.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.IO;
namespace CPPMockGenerator
{
    public class CPPMockGenerator:ICppMockGenerator
    {
        ImplementationMode m_mode;
        ICGunitHost m_Host;
        public ImplementationMode ImplementationMode
        {
            get
            {
                return m_mode;
            }
            set
            {
                m_mode = value;
            }
        }

        public void GenerateMock(IClass Class)
        {
            
        }
        private void generateFakeImplementation(IClass Class)
        {
            StreamWriter header = new StreamWriter("Fake_" + Class.Name + ".h");
         
            header.Close();
        }

        public PluginType PluginType
        {
            get
            {
                return GUnitFramework.Interfaces.PluginType.MockGenerator;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string PluginName
        {
            get
            {
                return "C++ MockGenerator " + Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ICGunitHost Owner
        {
            get
            {
                return m_Host;
            }
            set
            {
                m_Host = value;
            }
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
            return true;
        }

        public bool HandleProjectSession(ProjectStatus status)
        {
            return true;
        }

        public void Show(DockPanel dock, DockState state)
        {
           
        }
    }
}
