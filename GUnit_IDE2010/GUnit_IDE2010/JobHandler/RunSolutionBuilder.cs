using Gunit.DataModel;
using GUnit_IDE2010.CodeGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUnit_IDE2010.JobHandler
{
    public class RunSolutionBuilder: ExtProcessHandlerWithConsole
    {
        string m_argument = "";
        string workingDirectory = "";
        public RunSolutionBuilder(ConsoleDataModel model,string premakeScript,PremakeSolutionType solnType) :base(model)
        {
            if (solnType == PremakeSolutionType.Gmake)
            {
                
                m_argument = "gmake --file " + premakeScript;
            }
            else
            {
                m_argument = "vs2010 --file " + premakeScript;
            }
            if (File.Exists(premakeScript))
                workingDirectory = Path.GetDirectoryName(premakeScript);

        }
        public override Job JobFactory(string command, uint Id)
        {
            Job job = base.JobFactory(command, Id);
            job.Command = "premake4.exe";
            job.Argument = m_argument;
            job.Result = this;
            if (Directory.Exists(workingDirectory))
                job.WorkingDirectory = workingDirectory;
            return job;
        }
        public void RunJob()
        {
            ListofStrings commands = new ListofStrings();
            commands.Add("premake4.exe");
            base.RunJobs(commands);
        }
    }
}
