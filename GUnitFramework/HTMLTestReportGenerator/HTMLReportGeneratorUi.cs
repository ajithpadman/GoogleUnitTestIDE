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
namespace HTMLTestReportGenerator
{
    public partial class HTMLReportGeneratorUi : DockContent
    {
        HTMLTestReportGenerator m_generator;
        public HTMLReportGeneratorUi()
        {
            InitializeComponent();
        }
        public HTMLReportGeneratorUi(HTMLTestReportGenerator generator)
        {
            InitializeComponent();
            m_generator = generator;
            
        }
        private void enableButton()
        {
            if (string.IsNullOrWhiteSpace(m_generator.ReportPath))
            {
              
                    btnGenerate.Enabled = false;
                
            }
            else
            {
                if (null != m_generator)
                {
                    if (null != m_generator.Owner)
                    {
                        if (null != m_generator.Owner.TestRunner)
                        {
                            if (null != m_generator.Owner.TestRunner.TestSuits)
                            {
                                if (m_generator.Owner.TestRunner.TestSuits.Count != 0)
                                {
                                    btnGenerate.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void HTMLReportGeneratorUi_Load(object sender, EventArgs e)
        {
            txtPath.Text = m_generator.ReportPath;

            m_generator.PropertyChanged += new PropertyChangedEventHandler(Generator_PropertyChanged);
            m_generator.Owner.TestRunner.PropertyChanged+=new PropertyChangedEventHandler(TestRunner_PropertyChanged);
            m_generator.Owner.evProjectStatus += new GUnitFramework.Interfaces.onProjectStatus(Owner_evProjectStatus);
            enableButton();
            this.Enabled = false;

        }
        void Owner_evProjectStatus(ProjectStatus status, object data)
        {
            switch (status)
            {
                case ProjectStatus.CLOSE:
                    this.Enabled = false;
                    break;
                case ProjectStatus.NEW:
                case ProjectStatus.OPEN:
                    this.Enabled = true;
                    break;

            }
        }
        void TestRunner_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "TESTSUIT":
                    enableButton();
                    break;
                default:
                    break;
            }
        }
        void Generator_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ReportPath":
                    txtPath.Text = m_generator.ReportPath;
                    enableButton();
                    break;
                default:
                    break;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "HTML File (*.html)|*.html";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_generator.ReportPath = dlg.FileName;
            }

        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_generator.ReportPath) == false)
            {
                m_generator.generateReport(m_generator.ReportPath);
                MessageBox.Show("TestReport Generation Complete");
            }
        }
    }
}
