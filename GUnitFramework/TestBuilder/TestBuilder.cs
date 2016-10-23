using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
namespace TestBuilder
{
    public class TestBuilder:ICGunitPlugin
    {
        ICGunitHost m_host;
        List<ITestSuit> m_testSuits = new List<ITestSuit>();
        public bool HandleProjectSession(ProjectStatus status)
        {
            return true;
        }

        public ICGunitHost Owner
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        public string PluginName
        {
            get
            {
                return "TestBuilder1.0";
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
                return GUnitFramework.Interfaces.PluginType.SpecialPlugin;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Show(DockPanel dock, DockState state)
        {
            
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
            return true;
        }
    }
}
