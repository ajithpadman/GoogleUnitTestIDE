using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using System.ComponentModel;
using System.Reflection;
namespace TestRunner
{
    public class TestRunner:ItestRunner
    {
        ICGunitHost m_host;
        List<ITestSuit> m_suits = new List<ITestSuit>();
        string m_gtestExeutable;
        IProcessHandler m_processHandler;
        TestRunnerUi m_ui;
        List<ItestCase> m_SelectedtestCases = new List<ItestCase>();
        protected event PropertyChangedEventHandler m_propertyChanged = delegate { };
        /// <summary>
        /// Event for PropertChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { m_propertyChanged += value; }
            remove { m_propertyChanged -= value; }
        }
        protected void FirePropertyChange(string propertyName)
        {
            if (null != m_propertyChanged)
            {
                m_propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void RunTests()
        {
            
        }
        public TestRunner()
        {
            m_processHandler = new GUnitFramework.Implementation.ExternalProcessHandler();
        }
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
                return "Test Runner "+Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
                return GUnitFramework.Interfaces.PluginType.TestRunner;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public void addTestSuit(ITestSuit suit)
        {
            TestSuits.Add(suit);
            
        }
        public void Show(WeifenLuo.WinFormsUI.Docking.DockPanel dock, WeifenLuo.WinFormsUI.Docking.DockState state)
        {
            m_ui = new TestRunnerUi(this);
            m_ui.Show(dock, state);
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
            return true;
        }

        public string GTestExecutable
        {
            get
            {
                return m_gtestExeutable;
            }
            set
            {
                m_gtestExeutable = value;
                FirePropertyChange("EXE");
            }
        }


        public IProcessHandler Processhandler
        {
            get
            {
                return m_processHandler;
            }
            set
            {
                m_processHandler = value;
            }
        }


        public List<ITestSuit> TestSuits
        {
            get
            {
                return m_suits;
            }
            set
            {
                m_suits = value;
                FirePropertyChange("TESTSUIT");
            }
        }

        public void addSelectedTests(ItestCase test)
        {
            SelectedTests.Add(test);
            FirePropertyChange("SELECTED_TEST");
        }
        public List<ItestCase> SelectedTests
        {
            get
            {
                return m_SelectedtestCases;
            }
            set
            {
                m_SelectedtestCases = value;
            }
        }
    }
}
