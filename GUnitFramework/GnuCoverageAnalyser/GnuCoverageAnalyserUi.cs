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
namespace GnuCoverageAnalyser
{
    public partial class GnuCoverageAnalyserUi : DockContent
    {
        GnuCoverageAnalyser m_analyser; 
        public GnuCoverageAnalyserUi()
        {
            InitializeComponent();
        }
        public GnuCoverageAnalyserUi(GnuCoverageAnalyser analyser)
        {
            InitializeComponent();
            m_analyser = analyser;
        }
        private void enableButton()
        {
            if (
                string.IsNullOrWhiteSpace(m_analyser.ReportPath) ||
                string.IsNullOrWhiteSpace(m_analyser.ReportPath)||
                string.IsNullOrWhiteSpace(m_analyser.GcovPath)
                )
            {
                btnRun.Enabled = false;
            }
            else
            {
                btnRun.Enabled = true;
            }
        }
        public void enableButton(bool isEnabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                { btnRun.Enabled = isEnabled; });

            }
            else
            {
                btnRun.Enabled = isEnabled;
            }
        }
        private void GnuCoverageAnalyserUi_Load(object sender, EventArgs e)
        {
            txtReportPath.Text = m_analyser.ReportPath;
            txtObjects.Text = m_analyser.ObjectsPath;
            txtGcov.Text = m_analyser.GcovPath;
            m_analyser.PropertyChanged+=new PropertyChangedEventHandler(Analyser_PropertyChanged);
            m_analyser.Owner.evProjectStatus += new onProjectStatus(Owner_evProjectStatus);
            enableButton();
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
        void Analyser_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ReportPath":
                    txtReportPath.Text = m_analyser.ReportPath;
                    enableButton();
                    break;
                case "ObjectsPath":
                    txtObjects.Text = m_analyser.ObjectsPath;
                    enableButton();
                    break;
                case "GcovPath":
                    txtGcov.Text = m_analyser.GcovPath;
                    enableButton();
                    break;
                default:
                    break;
            }
        }

        private void btnReportPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_analyser.ReportPath = dlg.SelectedPath;

            }

        }

        private void btnObjects_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
          
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_analyser.ObjectsPath = dlg.SelectedPath;

            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            m_analyser.AnalyseCoverage();
           
        }

        private void btnGcov_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Exe File (*.exe)|*.exe";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_analyser.GcovPath = dlg.FileName;
            }
        }
    }
}
