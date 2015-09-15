using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUnit_IDE2010.JobHandler
{
    public class RunCoverageJobHandler: ExtProcessHandlerWithConsole
    {
        string m_OutputFile = "CoverageReport.html";
        ProjectDataModel m_ProjectModel = null;
        string CommonPath = "";
        string m_coverageScript = "";
        string m_workingDirectory = "";
        string m_ObjectDir = "";
        public RunCoverageJobHandler(ConsoleDataModel model,ProjectDataModel prjModel,string coverageGenScript): base(model)
        {
            if (prjModel != null)
            {
                m_ProjectModel = prjModel;
                m_coverageScript = coverageGenScript;
                string runDir = m_ProjectModel.BuildPath;//Path.GetDirectoryName(m_ProjectModel.ProjectPath);
                m_ObjectDir = DataModelBase.getReleativePathWith(m_ProjectModel.BuildPath+"\\obj", runDir);
                if (Directory.Exists(runDir + "\\CoverageReport") == false)
                {
                    Directory.CreateDirectory(runDir + "\\CoverageReport");
                }
               

                CommonPath = FindCommonPath("\\", prjModel.SourceFiles);
                m_OutputFile = runDir + "\\CoverageReport\\" + "CoverageReport.html";
                m_workingDirectory = runDir;
            }

        }
        private string FindCommonPath(string Separator, List<string> Paths)
        {
            string CommonPath = String.Empty;
            List<string> SeparatedPath = Paths
                .First(str => str.Length == Paths.Max(st2 => st2.Length))
                .Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            foreach (string PathSegment in SeparatedPath.AsEnumerable())
            {
                if (CommonPath.Length == 0 && Paths.All(str => str.StartsWith(PathSegment)))
                {
                    CommonPath = PathSegment;
                }
                else if (Paths.All(str => str.StartsWith(CommonPath + Separator + PathSegment)))
                {
                    CommonPath += Separator + PathSegment;
                }
                else
                {
                    break;
                }
            }

            return CommonPath;
        }
        public override Job JobFactory(string command, uint Id)
        {
            Job job = base.JobFactory(command, Id);
            job.Command = "python";
            job.JobKind = JobKind.RunExeJob;
            job.Argument = m_coverageScript + " -r " + CommonPath + "  --object-directory=obj --html --html-details -o " + m_OutputFile;
            job.Result = m_OutputFile;
            if (Directory.Exists(m_workingDirectory))
            {
                job.WorkingDirectory = m_workingDirectory;
            }
            return job;
        }
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
