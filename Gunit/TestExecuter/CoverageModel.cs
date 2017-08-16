using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Utils;
using Gunit.Model;
using System.Collections.ObjectModel;
using Gunit.Interfaces;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using System.ComponentModel;
namespace TestExecuter
{
    public class CoverageModel
    {
        TestExecuterModel m_model;
        CoverageAnalyser m_analyser = new CoverageAnalyser();
        ListofStrings m_gcovOutput = new ListofStrings();
        IProcessHandler m_processHandler  = new Gunit.Utils.ExternalProcessHandler();
        public CoverageModel(TestExecuterModel model)
        {
            m_model = model;
           
            
            m_processHandler.evProcessComplete += (CoverageAnalysis_Complete);
            m_processHandler.evProgress += CoverageProgress;
        }
       
        List<CoverageSummary> m_summary = new List<CoverageSummary>();
        List<Coverage> m_FileCoverage = new List<Coverage>();
        public TestExecuterModel Model
        {
            get { return m_model; }
        }
        public List<CoverageSummary> CoverageSummary
        {
            get { return m_summary; }
        }
        public List<Coverage> DetailedCoverage
        {
            get { return m_FileCoverage; }
        }
        private IJob createJob(string fileName)
        {
            IJob job = new Job();
            job.Command = m_model.PathToGcov;
            job.Argument = "-b " + Path.GetFileName(fileName);
            job.StdOutCallBack += Output_CallBack;
            job.WorkingDirectory = m_model.PathtoObjects;
            return job;
        }
        private IJob createLineJob(string fileName)
        {
            IJob job = new Job();
            job.Command = m_model.PathToGcov;
            job.Argument = fileName;
            job.StdOutCallBack += Output_CallBack;
            job.WorkingDirectory = m_model.PathtoObjects;
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
            m_model.MaxProgress = 0;
            m_model.Progress = 0;
            if (System.IO.File.Exists(m_model.HostModel.SelectedFile))
            {
                if (Path.GetExtension(m_model.HostModel.SelectedFile) == ".c" || Path.GetExtension(m_model.HostModel.SelectedFile) == ".cpp")
                {
                    m_processHandler.JobList.Add(createJob(m_model.HostModel.SelectedFile));
                }
            }
            m_model.MaxProgress = m_processHandler.JobList.Count;
            m_processHandler.Start();

        }
        private void CoverageProgress(int value)
        {
            m_model.Progress = value;
        }
        private void UpdateProgressIndeterminate()
        {
           
            m_model.IsIndeterminate = true;
         
        }
        private void UpdateProgressdeterminate()
        {

            m_model.IsIndeterminate = false;

        }
        private void GeerateDetaiedReport()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += RunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, new Action(UpdateProgressdeterminate));
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            if (System.IO.File.Exists(m_model.HostModel.SelectedFile))
            {
                DetailedCoverage.Add(m_analyser.Coverage_AnalyseStatementCoverage(m_model.PathtoObjects + "\\" + Path.GetFileName(m_model.HostModel.SelectedFile) + ".gcov", m_model.HostModel.SelectedFile));
            }
            generateReport(m_model.PathToCoverageReport);
        }
        private void CoverageAnalysis_Complete()
        {
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Render, new Action(UpdateProgressIndeterminate));

            GeerateDetaiedReport();
         }
        public void generateReport(string fileName)
        {
            GenerateCoverageReport gen = new GenerateCoverageReport(this, fileName);
            gen.Generate();
        }
    }
}
