using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GUnitFramework.Interfaces;
namespace Gunit.Ui
{
    public partial class Plugins : DockContent
    {
        ICGunitHost m_host = null;
        public Plugins()
        {
            InitializeComponent();
        }
        public Plugins(ICGunitHost host)
        {
            InitializeComponent();
            m_host = host;
            m_host.evPluginLoaded+=new onPluginLoaded(Host_evPluginLoaded);
        }
        void Host_evPluginLoaded(ICGunitPlugin plugin)
        {
            updatePlugins();
        }

        public void updatePlugins()
        {
            if (null != m_host)
            {
                if (null != m_host.CodeParser)
                {
                    txtParser.Text = m_host.CodeParser.PluginName;
                }
                if (null != m_host.CPPCodeParser)
                {
                    txtCPPParser.Text = m_host.CPPCodeParser.PluginName;
                }
                if (null != m_host.CoverageAnalyser)
                {
                    txtCoverage.Text = m_host.CoverageAnalyser.PluginName;
                }
                if (null != m_host.CurrentTestReportGenerator)
                {
                    txtReportGen.Text = m_host.CurrentTestReportGenerator.PluginName;
                }
                if (null != m_host.BoundaryTestGenerator)
                {
                    txtBoundaryTest.Text = m_host.BoundaryTestGenerator.PluginName;
                }
                if (null != m_host.TestRunner)
                {
                    txtTestRunner.Text = m_host.TestRunner.PluginName;
                }
                if (null != m_host.ProjectBuilder)
                {
                    txtBuilder.Text = m_host.ProjectBuilder.PluginName;
                }
                if (null != m_host.MockGenerator)
                {
                    txtMockGenerator.Text = m_host.MockGenerator.PluginName;
                }
              
            }
        }

        private void Plugins_Load(object sender, EventArgs e)
        {
            updatePlugins();
        }

    }
}
