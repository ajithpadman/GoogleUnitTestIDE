using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GUnit_IDE2010.HTML_Creator;
namespace GUnit_IDE2010.JobHandler
{
    public class GCovParserJobHandler:ExtProcessHandlerWithConsole
    {
        ProjectDataModel m_prjModel = null;
        string m_gcovPath = "";
        string m_workingdirectory = "";
        bool m_analyseLineCoverage = false;
        ListofStrings m_gcovOutput = new ListofStrings();
        CoverageAnalyser m_analyser = new CoverageAnalyser();
        List<CoverageSummary> m_summary = new List<CoverageSummary>();
        List<Coverage> m_FileCoverage = new List<Coverage>();
        public delegate void onCoverageChange(GCovParserJobHandler ptr);
        public event onCoverageChange evCoverageComplete = delegate { };
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prjModel"></param>
        public GCovParserJobHandler(ConsoleDataModel model,ProjectDataModel prjModel):base(model)
        {
            m_prjModel = prjModel;
            if(Directory.Exists(m_prjModel.CompilorPath))
            {
                m_gcovPath = m_prjModel.CompilorPath + "\\gcov.exe";
            }
            else
            {
                m_gcovPath = "gcov.exe";
            }
            m_workingdirectory = m_prjModel.BuildPath + "\\obj";
        }
        /// <summary>
        /// Add the coverage Analysis
        /// </summary>
        public virtual void RunJob()
        {
            ListofStrings commands = new ListofStrings();
            foreach(string file in m_prjModel.SourceFiles)
            {
                commands+= "-b " + Path.GetFileName(file);


            }
            RunJobs(commands);
        }
        public override Job JobFactory(string command, uint Id)
        {
            Job job =  base.JobFactory(command, Id);
            job.Argument = job.Command;
            job.Command = m_gcovPath;
            job.WorkingDirectory = m_workingdirectory;
            job.JobKind = JobKind.CoverageAnalysisJob;
            return job;

        }
        protected override void JobCompleteCallBack(Job job)
        {
            base.JobCompleteCallBack(job);
            if (m_maxJobs == m_completedJobs && m_analyseLineCoverage == false)
            {
                m_maxJobs = 0;
                m_completedJobs = 0;
                m_analyseLineCoverage = true;
                ListofStrings commands = new ListofStrings();
                foreach (string file in m_prjModel.SourceFiles)
                {
                    commands +=  Path.GetFileName(file);


                }
                RunJobs(commands);
            }
            else if (m_maxJobs == m_completedJobs && m_analyseLineCoverage == true)
            {
                foreach (string file in m_prjModel.SourceFiles)
                {
                    m_FileCoverage.Add( m_analyser.Coverage_AnalyseStatementCoverage(m_workingdirectory + "\\" + Path.GetFileName(file) + ".gcov",file));
                }
                GenerateCoverageReport htmlWriter = new GenerateCoverageReport(m_summary, m_FileCoverage,m_prjModel);
                htmlWriter.Generate();
                evCoverageComplete(this);

            }
          
        }
        protected override void error_CallBack(object sender, DataReceivedEventArgs e)
        {
           
             
            base.error_CallBack(sender, e);

        }
        protected override void Output_CallBack(object sender, DataReceivedEventArgs e)
        {
            if (m_analyseLineCoverage == false)
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
           // base.Output_CallBack(sender, e);
        }

    }
}
