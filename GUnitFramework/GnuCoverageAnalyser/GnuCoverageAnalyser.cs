using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
using GUnitFramework.Implementation;
using System.Diagnostics;
using System.IO;
using System.Reflection;
namespace GnuCoverageAnalyser
{
    public class GnuCoverageAnalyser:PropertyNotifier, ICoverageAnalyser
    {
        IProcessHandler m_processHandler;
        string m_report;
        ICGunitHost m_owner;
        string m_gcovPath;
        List<CoverageSummary> m_summary = new List<CoverageSummary>();
        List<Coverage> m_FileCoverage = new List<Coverage>();
        ListofStrings m_gcovOutput = new ListofStrings();
        string m_objDirectory;
        CoverageAnalyser m_analyser = new CoverageAnalyser();
        GnuCoverageAnalyserUi m_ui = null;
        public List<CoverageSummary> CoverageSummary
        {
            get { return m_summary; }
        }
        public List<Coverage> DetailedCoverage
        {
            get { return m_FileCoverage; }
        }
        public GnuCoverageAnalyser()
        {
            m_processHandler = new ExternalProcessHandler();
            m_processHandler.evProcessComplete += new onProcessComplete(CoverageAnalysis_Complete);
            
        }
        private IJob createJob(string fileName)
        {
            IJob job = new Job();
            job.Command = m_gcovPath;
            job.Argument = "-b " + Path.GetFileName(fileName);
            job.StdOutCallBack += Output_CallBack;
            job.WorkingDirectory = m_objDirectory;
            return job;
        }
        private IJob createLineJob(string fileName)
        {
            IJob job = new Job();
            job.Command = m_gcovPath;
            job.Argument = fileName;
            job.StdOutCallBack += Output_CallBack;
            job.WorkingDirectory = m_objDirectory;
            return job;
        }
        void Output_CallBack(object sender, DataReceivedEventArgs e)
        {
          
                if (e != null)
                {
                    if (e.Data != null)
                    {
                        if (e.Data is string)
                        {
                            string line = e.Data as string;
                            if (line.Contains("Creating "))
                            {
                                m_summary.Add(m_analyser.CoverageAnalyseSummary(m_gcovOutput));
                                m_gcovOutput.Clear();
                            }
                            else
                            {
                                m_gcovOutput += line;
                            }
                        }
                    }
                }
            
        }

        public void AnalyseCoverage()
        {
            m_processHandler.JobList.Clear();
            if (m_ui != null)
            {
                m_ui.enableButton(false);
            }
            foreach (string file in Owner.SelectedFiles)
            {
                if (Path.GetExtension(file) == ".c" || Path.GetExtension(file) == ".cpp")
                {
                    m_processHandler.JobList.Add(createJob(file));
                }
            }
            m_processHandler.Start();
          
        }
        private void CoverageAnalysis_Complete()
        {
            foreach (string file in Owner.SelectedFiles)
            {
                m_FileCoverage.Add( m_analyser.Coverage_AnalyseStatementCoverage(ObjectsPath + "\\" + Path.GetFileName(file) + ".gcov", file));
            }
            if (m_ui != null)
            {
                m_ui.enableButton(true);
            }
            generateReport(ReportPath);
        }
        public IProcessHandler coverageAnalyser
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
        public string GcovPath
        {
            get { return m_gcovPath; }
            set 
            {
                m_gcovPath = value;
                FirePropertyChange("GcovPath");
            }
        }
        public string ObjectsPath
        {
            get { return m_objDirectory; }
            set 
            {
                m_objDirectory = value;
                FirePropertyChange("ObjectsPath");
            }
        }
        public string ReportPath
        {
            get
            {
                return m_report;
            }
            set
            {
                m_report = value;
                FirePropertyChange("ReportPath");
            }
        }

        public void generateReport(string fileName)
        {
            GenerateCoverageReport gen = new GenerateCoverageReport(this);
            gen.Generate();
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
                return "GnuCoverageAnalyser" + Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
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
                return GUnitFramework.Interfaces.PluginType.CoverageAnalyser;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Show(DockPanel dock, DockState state)
        {
            m_ui = new GnuCoverageAnalyserUi(this);
            m_ui.Show(dock, DockState.DockBottomAutoHide);
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
           
            return true;
        }
    }
}
