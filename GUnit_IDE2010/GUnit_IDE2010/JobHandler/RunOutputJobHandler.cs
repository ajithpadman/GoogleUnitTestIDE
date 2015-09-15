
using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GUnit_IDE2010.JobHandler
{
    public class RunOutputJobHandler:ExtProcessHandlerWithConsole
    {
        string m_OutputFile = "Output.xml";
        
        public RunOutputJobHandler(string outputPath, ConsoleDataModel model):base(model)
        {

            m_OutputFile = outputPath;

        }
        public override Job JobFactory(string command, uint Id)
        {
           
            Job job =  base.JobFactory(command, Id);
            job.JobKind = JobKind.RunExeJob;
            job.Argument ="--gtest_output=\"xml:" + m_OutputFile + "\"";
            job.Result = m_OutputFile;
            if (File.Exists(command))
            {
                job.WorkingDirectory = Path.GetDirectoryName(command);
            }
            return job;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public override void RunJobs(ListofStrings commands)
        {
            if (File.Exists(m_OutputFile))
            {
                File.Delete(m_OutputFile);
            }
            base.RunJobs(commands);

        }

    }
}
