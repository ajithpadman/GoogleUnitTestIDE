using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Model;
using Gunit.Utils;
using Gunit.Interfaces;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace MinGWCompiler.SolutionBuilder.ViewModel
{
    public enum SolutionType
    {
        Makefile,
        VisualStudio
    }
    public class CodeWriter : StreamWriter
    {
        public CodeWriter(string path)
            : base(path)
        {

        }
        public void WriteCodeLine(int indentationLevel, string value)
        {
            string input = value;
            int i = indentationLevel;
            while (i != 0)
            {
                input = " " + input;
                i--;
            }
            base.WriteLine(input);
        }

    }
    public class SolutionBuilderModel : ViewModelBase
    {
        IProjectModel _model;
        MinGWBuilder _builder;
        public event onProcessComplete evProcessComplete = delegate { };
        ExternalProcessHandler _processHandler = new ExternalProcessHandler();
        SolutionType m_solnType = SolutionType.Makefile;
        OutputTypes m_outputType = OutputTypes.ConsoleApplication;
        string m_projectPath = "";
        string m_outputPath = "";
        string m_buildPath = "";
        public SolutionBuilderModel(IProjectModel model,MinGWBuilder builder)
        {
            _model = model;
            _builder = builder;
           
            m_projectPath = Path.GetDirectoryName(model.ProjectPath);
            OutPutPath = _builder.BuildDirectory;
            BuildPath = _builder.BuildDirectory;
            Status = "Choose the solution Type";
        }
        public SolutionType SolutionType
        {
            get { return m_solnType; }
            set
            {
                m_solnType = value;
                Status = "Click on the button to generate solution";
                OnPropertyChanged("SolutionType");
            }
        }
        string _Status;
        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                OnPropertyChanged("Status");
            }
        }
        public string OutPutPath
        {
            get { return m_outputPath; }
            set
            {
                m_outputPath = value;
                OnPropertyChanged("OutPutPath");
            }

        }
        public string BuildPath
        {
            get { return IOUtils.getReleativePathWith(m_buildPath, m_projectPath); }
            set
            {
                m_buildPath = value;
                OnPropertyChanged("BuildPath");
            }

        }

        private string getRelative(ObservableCollection<string> list)
        {
            ListofStrings relativeIncludes = new ListofStrings();
            foreach (string include in list)
            {
                relativeIncludes += "\"" + IOUtils.getReleativePathWith(include, m_projectPath) + "\"";
            }
            return String.Join(",", relativeIncludes.ToArray());
        }

        private string getRelativeWithAppendString(ObservableCollection<string> list, string append)
        {
            ListofStrings relativeIncludes = new ListofStrings();
            foreach (string include in list)
            {
                relativeIncludes += "\"" + append + " " + Path.GetFileName(include) + "\"";
            }
            return String.Join(",", relativeIncludes.ToArray());
        }
        private string Includes
        {
            get
            {
                return getRelative(_model.IncludePaths);

            }
        }
        private string Projectheaders
        {
            get
            {
                if (SolutionType == SolutionType.Makefile)
                    return getRelativeWithAppendString(_model.PreHeaderFiles, "-include");
                else
                    return getRelativeWithAppendString(_model.PreHeaderFiles, "/FI");

            }
        }
        private string LibraryPath
        {
            get
            {
                return getRelative(_model.LibraryPaths);

            }
        }
        private string SrcPath
        {
            get
            {
                return getRelative(_model.SourceFiles);

            }
        }
        private string BuildOptions
        {
            get
            {
                if (SolutionType == SolutionType.Makefile)
                {
                    return "\"-fprofile-arcs\", \"-ftest-coverage\", '-x c++'";
                }
                else
                {
                    return "";
                }
            }
        }
        public void CreateSolution()
        {
            Job job = new Job();
            job.Command = "premake4.exe";
            job.WorkingDirectory = Directory.GetCurrentDirectory();
            job.StdErrCallBack = callback;
            job.StdOutCallBack = callback;
            if (SolutionType == SolutionType.Makefile)
            {

                job.Argument = " --file=" + m_projectPath + "\\premake4.lua  gmake";
            }
            else
            {
                job.Argument = "--file=" + m_projectPath + "\\premake4.lua  vs2010";
            }
            _processHandler.evProcessComplete -= ProcessHandler_evProcessComplete;
            _processHandler.evProcessComplete += ProcessHandler_evProcessComplete;

            _processHandler.JobList.Add(job);
            _processHandler.Start();

        }
        private void callback(object sender, DataReceivedEventArgs e)
        {
            Console.Write(e.Data);
        }

        void ProcessHandler_evProcessComplete()
        {
            evProcessComplete();
        }
        public string createPremakeScript()
        {
            string commonInclude = Projectheaders;
            string scriptPath = m_projectPath + "\\premake4.lua";
            if (Directory.Exists(m_outputPath) == false)
                Directory.CreateDirectory(m_outputPath);

            CodeWriter writer = new CodeWriter(scriptPath);
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Premake Script for unittest------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            if (string.IsNullOrWhiteSpace(_model.Name) == false)
            {

                writer.WriteLine("solution \"" + _model.Name + "\"");
            }
            else
            {

                writer.WriteLine("solution \"UTEST\"");
            }
            writer.WriteCodeLine(1, "INCLUDE_PATHS={");
            writer.WriteCodeLine(2, Includes);
            writer.WriteCodeLine(1, "}\n");
            writer.WriteCodeLine(1, "LIB_PATHS={");
            writer.WriteCodeLine(2, LibraryPath);
            writer.WriteCodeLine(1, "}");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Common Settings------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteCodeLine(1, "configurations { \"Debug\"}");
            writer.WriteCodeLine(1, "flags {\"FloatFast\" ,\"StaticRuntime\"}");
            ListofStrings buildOptions = new ListofStrings();

            if (string.IsNullOrWhiteSpace(BuildOptions) == false)
            {
                buildOptions += BuildOptions;
            }
            if (string.IsNullOrWhiteSpace(commonInclude) == false)
            {
                buildOptions += commonInclude;
            }
            writer.WriteCodeLine(1, "buildoptions {" + String.Join(",", buildOptions.ToArray()) + "}");

            if (SolutionType == SolutionType.Makefile)
            {
                writer.WriteCodeLine(1, "linkoptions {\"-fprofile-arcs\"}");
            }

            writer.WriteCodeLine(1, "includedirs {INCLUDE_PATHS}");
            writer.WriteCodeLine(1, "libdirs {LIB_PATHS}");
            writer.WriteCodeLine(1, "location \"" + BuildPath + "\"");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Build configuration------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteCodeLine(1, "configuration \"Debug\"");
            writer.WriteCodeLine(2, "flags {\"Symbols\"}");
            writer.WriteCodeLine(2, "libdirs {\"libs\"}");
            writer.WriteCodeLine(2, "targetsuffix \"_d\"");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Project Setting------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteCodeLine(1, "project \"" + _model.Name + "\"");
            if (m_outputType == OutputTypes.ConsoleApplication)
            {
                writer.WriteCodeLine(2, "kind \"ConsoleApp\"");
            }
            else if (m_outputType == OutputTypes.SharedLibrary)
            {
                writer.WriteCodeLine(2, "kind \"SharedLib\"");
            }
            writer.WriteCodeLine(2, "language \"C++\"");
            writer.WriteCodeLine(2, "targetdir \"Bin\"");


            writer.WriteCodeLine(2, "files {");
            writer.WriteCodeLine(3, SrcPath);
            writer.WriteCodeLine(2, "}");

            foreach (string lib in _model.LibNames)
            {
                writer.WriteCodeLine(2, "links {\"" + lib + "\"}");
            }
            writer.Close();
            return scriptPath;
        }

    }
}
