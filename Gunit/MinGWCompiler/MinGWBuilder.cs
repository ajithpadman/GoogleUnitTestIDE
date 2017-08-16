using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Interfaces;
using Gunit.Model;
using Gunit.Utils;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
namespace MinGWCompiler
{
    [Serializable]
    public class MinGWBuilder:ViewModelBase, ICodeBuilder
    {
        
        private string m_compilerPath = "";
        bool m_IsInstrumented = true;
        WarningLevel m_warningLevel = WarningLevel.EnableAllWarning;
        OutputTypes m_outputType = OutputTypes.ConsoleApplication;
        string m_BuildDirectory;
        [XmlIgnore]
        IProcessHandler m_externalProcessHandler = new ExternalProcessHandler();
        [XmlIgnore]
        ListOfConsoleData m_consoleData = new ListOfConsoleData();
        [XmlIgnore]
        IProjectModel m_model;
        [XmlIgnore]
        int m_progress = 0;
        [XmlIgnore]
        int m_MaxProgress = 0;
        [XmlIgnore]
        ListofStrings m_commands = new ListofStrings();
        public MinGWBuilder()
        {

        }
        public MinGWBuilder(IProjectModel model)
        {
            m_model = model;
        }
        [XmlIgnore]
        public IProjectModel HostModel
        {
            get { return m_model; }
            set { m_model = value; }
        }
             
        public string CompilorPath
        {
            get
            {
                return m_compilerPath;
            }
            set 
            {
                m_compilerPath = value;
                OnPropertyChanged("CompilorPath");
            }
        }

        public bool IsCodeInstrumented
        {
            get
            {
                return m_IsInstrumented;
            }
            set
            {
                m_IsInstrumented = value;
                OnPropertyChanged("Instrumented");
            }
        }

        public WarningLevel WarningLevel
        {
            get
            {
                return m_warningLevel;
            }
            set
            {
                m_warningLevel = value;
                OnPropertyChanged("warningLevel");
            }
        }

        public OutputTypes OutputType
        {
            get
            {
                return m_outputType;
            }
            set
            {
                m_outputType = value;
                OnPropertyChanged("outputType");
            }
        }

        public string BuildDirectory
        {
            get
            {
                return m_BuildDirectory;
            }
            set
            {
                m_BuildDirectory = value;
                OnPropertyChanged("BuildDirectory");
            }
        }
        [XmlIgnore]
        public IProcessHandler buildJobHandler
        {
            get
            {
                return m_externalProcessHandler;
            }
            set
            {
                m_externalProcessHandler = value;
                OnPropertyChanged("processHandler");
            }
        }
        [XmlIgnore]
        public Gunit.Utils.ListOfConsoleData CompilorOutput
        {
            get
            {
                return m_consoleData;
            }
            set
            {
                m_consoleData = value;
                OnPropertyChanged("consoleData");
            }
        }
        
        private void incrementProgress(int value)
        {
            m_progress = value;
        }
      
        public int BuildProgress
        {
            get{return m_progress;}
            set
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => incrementProgress(value)));

                OnPropertyChanged("BuildProgress");
            }
        }
        public int MaxProgress
        {
            get { return m_MaxProgress; }
            set
            {
                m_MaxProgress = value;
                OnPropertyChanged("MaxProgress");
            }
        }
        private void BuildJobs()
        {
             MaxProgress = m_commands.Count;
            m_externalProcessHandler.JobList.Clear();
            foreach (string command in m_commands)
            {
                m_externalProcessHandler.JobList.Add(createBuildJob(command));

            }
            m_externalProcessHandler.Start();

        }
        public void buildProject()
        {
            if (String.IsNullOrWhiteSpace(CompilorPath) == false)
            {
                MaxProgress = 0;
                BuildProgress = 0;
                m_commands = GetGnuCommands();
                m_externalProcessHandler.evLog -= (ProcessHandler_evlog);
                m_externalProcessHandler.evLog += (ProcessHandler_evlog);
                m_externalProcessHandler.evProcessComplete -= (ProcessHandler_evProcessComplete);
                m_externalProcessHandler.evProgress -= (ProcessHandler_evProgress);
                m_externalProcessHandler.evProcessComplete += (ProcessHandler_evProcessComplete);
                m_externalProcessHandler.evProgress += (ProcessHandler_evProgress);
                BuildJobs();

            }
            else
            {
               CompilorOutput+= ("No compilor settings Found");
            }
        }
        void ProcessHandler_evlog(string log)
        {
            CompilorOutput += (log);
        }
        void ProcessHandler_evProcessComplete()
        {
            
        }
        void ProcessHandler_evProgress(int value)
        {
            BuildProgress = value;
        }
        IJob createBuildJob(string command)
        {
            Job job = new Job();

            job.Command = CompilorPath;
            job.Argument = command;
            job.StdErrCallBack = StdErrCallBack;
            job.StdOutCallBack = StdOutCallBack;
            if (Directory.Exists(BuildDirectory))
            {
                job.WorkingDirectory = BuildDirectory;
            }
            return job;
        }
        void StdErrCallBack(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Data != null)
                    {
                        CompilorOutput+=(e.Data.ToString());
                    }
                }
            }
            catch
            {

            }
        }
        void StdOutCallBack(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Data != null)
                    {
                        CompilorOutput += (e.Data.ToString());
                    }
                }
            }
            catch
            {

            }
        }
        private ListofStrings GetGnuCommands()
        {
            ListofStrings buildCommands = new ListofStrings();
            try
            {

                ListofStrings LinkCommands = new ListofStrings();
                ListofStrings buildOptionCommandLine = new ListofStrings();
                ListofStrings LinkerOptionCommandLine = new ListofStrings();
                List<ObjectList> ObjectList = new List<ObjectList>();
                string buildDir = BuildDirectory;
                string objDir = BuildDirectory + "\\obj";
                if (Directory.Exists(buildDir) == false)
                {
                    Directory.CreateDirectory(buildDir);
                }
                if (Directory.Exists(objDir) == false)
                {
                    Directory.CreateDirectory(objDir);
                }
                switch (WarningLevel)
                {
                    case WarningLevel.AbortCompilationOnFirstError:
                        buildOptionCommandLine += "-Wfatal-errors";
                        break;
                    case WarningLevel.EnableAllWarning:
                        buildOptionCommandLine += "-Wall";
                        break;
                    case WarningLevel.SyntaxCheckOnly:
                        buildOptionCommandLine += "-fsyntax-only";
                        break;
                    case WarningLevel.TreatAllWarningAsError:
                        buildOptionCommandLine += "-Werror";
                        break;

                }

                buildOptionCommandLine += "-MMD";
                buildOptionCommandLine += "-MP";
                buildOptionCommandLine += "-ffast-math";
                buildOptionCommandLine += "-g";
                buildOptionCommandLine += "-O0";
                buildOptionCommandLine += "-fpermissive";
                if (IsCodeInstrumented == true)
                {
                    buildOptionCommandLine += "-fprofile-arcs";
                    buildOptionCommandLine += "-ftest-coverage";
                }
                //buildOptionCommandLine += "-x c++";

                foreach (string str in m_model.IncludePaths)
                {
                    buildOptionCommandLine += "-I";
                    buildOptionCommandLine += str;
                }
                foreach (string str in m_model.PreHeaderFiles)
                {

                    buildOptionCommandLine += "-include";
                    buildOptionCommandLine += (str);
                }
                foreach (string str in m_model.Defines)
                {
                    buildOptionCommandLine += "-D";
                    buildOptionCommandLine += str;
                }
                buildOptionCommandLine += "-D";

                buildOptionCommandLine += "GTEST_HAS_TR1_TUPLE=1";
                //buildOptionCommandLine += "-x c++";
                if (IsCodeInstrumented == true)
                {
                    LinkerOptionCommandLine += "-fprofile-arcs ";
                }
                foreach (string str in m_model.LibraryPaths)
                {
                    LinkerOptionCommandLine += "-L";
                    LinkerOptionCommandLine += str;
                }
                foreach (string lib in m_model.LibNames)
                {
                    LinkerOptionCommandLine += "-l";
                    LinkerOptionCommandLine += lib;
                }

                ObjectList.Clear();
                foreach (string str in m_model.SourceFiles)
                {

                    string fileName = Path.GetFileName(str);
                    ObjectList obj = new ObjectList();
                    obj.sourceFile = str;
                    try
                    {
                        if (fileName.Contains('.'))
                        {
                            fileName = fileName.Split('.')[0];
                        }
                    }
                    catch
                    {

                    }
                    fileName = objDir + "\\" + fileName + ".o";
                    obj.objectPath = fileName;
                    ObjectList.Add(obj);
                }

                buildCommands.Clear();
                LinkCommands.Clear();
                switch (OutputType)
                {
                    case OutputTypes.ConsoleApplication:
                        LinkCommands += "-o " + buildDir + "\\" + m_model.Name + ".exe";
                        m_model.BinaryPath = buildDir + "\\" + m_model.Name + ".exe";
                        break;
                    case OutputTypes.SharedLibrary:
                        LinkCommands += "-shared -o " + buildDir + "\\" + m_model.Name + ".dll";
                        m_model.BinaryPath = buildDir + "\\" + m_model.Name + ".dll";
                        break;
                    default:
                        LinkCommands += "-o " + buildDir + "\\" + m_model.Name + ".exe";
                        m_model.BinaryPath = buildDir + "\\" + m_model.Name + ".exe";
                        break;
                }

                foreach (ObjectList obj in ObjectList)
                {
                    string command = " " + String.Join(" ", buildOptionCommandLine.ToArray()) + " -o " + obj.objectPath + " -c " + obj.sourceFile;
                    buildCommands += command;
                    LinkCommands += obj.objectPath;
                }
                LinkCommands.AddRange(LinkerOptionCommandLine);
                string projectLink = String.Join(" ", LinkCommands);
                buildCommands += projectLink;
            }
            catch
            {

            }
            return buildCommands;
        }

        
    }
}
