using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUnitFramework.Implementation;
using GUnitFramework.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
namespace ExcelReportGenerator
{
    public partial class ExcelReportGeneratorUi : DockContent
    {
        ExcelReportGenerator m_plugin;
        public ExcelReportGeneratorUi():base()
        {
            
            InitializeComponent();
        }
        public ExcelReportGeneratorUi(ExcelReportGenerator plugin)
        {
            InitializeComponent();
            m_plugin = plugin;
        }
        private void enableButton()
        {
            if (string.IsNullOrWhiteSpace(m_plugin.ReportPath))
            {

                btnGenerate.Enabled = false;

            }
            else
            {
                if (null != m_plugin)
                {
                    if (null != m_plugin.Owner)
                    {
                        if (null != m_plugin.Owner.TestRunner)
                        {
                            if (null != m_plugin.Owner.TestRunner.TestSuits)
                            {
                                if (m_plugin.Owner.TestRunner.TestSuits.Count != 0)
                                {
                                    btnGenerate.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void ExcelReportGeneratorUi_Load(object sender, EventArgs e)
        {
            txtReport.Text = m_plugin.ReportPath;
            enableButton();
            m_plugin.PropertyChanged += new PropertyChangedEventHandler(Plugin_PropertyChanged);
            m_plugin.Owner.TestRunner.PropertyChanged += new PropertyChangedEventHandler(TestRunner_PropertyChanged);
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
        void Plugin_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ReportPath":
                    txtReport.Text = m_plugin.ReportPath;
                    enableButton();
                    break;
                default:
                    break;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel File (*.xls)|*.xls";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_plugin.ReportPath = dlg.FileName;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            m_plugin.generateReport(m_plugin.ReportPath);
        }
       
    }
}
