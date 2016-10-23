using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using GUnitFramework.Implementation;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Reflection;
namespace ProjectBuilder
{
    public class ProjectBuilder : ICodeBuilder
    {
        IProcessHandler m_externalProcessHandler;
        ICGunitHost m_host;
        string m_BuildDirectory;
        string m_CompilorPath;
        WarningLevel m_warningLevel = WarningLevel.EnableAllWarning;
        OutputTypes m_outputType = OutputTypes.ConsoleApplication;
        bool m_isIntrumented = true;
        ListOfConsoleData m_currentLine = new ListOfConsoleData();

        ListofStrings m_commands = new ListofStrings();
        
        ProjectbuilderUi m_ui;
        protected event PropertyChangedEventHandler m_propertyChanged = delegate { };
        /// <summary>
        /// Event for PropertChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { m_propertyChanged += value; }
            remove { m_propertyChanged -= value; }
        }
        /// <summary>
        /// Fire the Property Change event
        /// </summary>
        /// <param name="propertyName">Name of the Proerty Changed</param>
        protected void FirePropertyChange(string propertyName)
        {
            if (null != m_propertyChanged)
            {
                m_propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public IProcessHandler buildJobHandler
        {
            get
            {
                return m_externalProcessHandler;
            }
            set
            {
                m_externalProcessHandler = value;
            }
        }

        public void buildProject()
        {
            if (String.IsNullOrWhiteSpace(CompilorPath) == false)
            {
               
                m_commands = GetGnuCommands();
              
                m_externalProcessHandler.evProcessComplete -= (ProcessHandler_evProcessComplete);
                m_externalProcessHandler.evProgress -= (ProcessHandler_evProgress);
                m_externalProcessHandler.evProcessComplete += (ProcessHandler_evProcessComplete);
                m_externalProcessHandler.evProgress+=(ProcessHandler_evProgress);
                BuildJobs();

            }
            else
            {
                m_ui.setStatus("No compilor settings Found");
            }
           
        }
        private void BuildJobs()
        {
            m_ui.setMaxProgress(m_commands.Count);
            m_externalProcessHandler.JobList.Clear();
            foreach (string command in m_commands)
            {
                m_externalProcessHandler.JobList.Add(createBuildJob(command));
               
            }
            m_externalProcessHandler.Start();
          
        }

        void ProcessHandler_evProgress(int value)
        {
            m_ui.updateProgress(value);
        }
        void ProcessHandler_evProcessComplete()
        {
            m_ui.EnableRunBtn(true);
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
                        addCompilorOutput(e.Data.ToString());
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
                        addCompilorOutput(e.Data.ToString());
                    }
                }
            }
            catch
            {

            }
        }

        public bool HandleProjectSession(ProjectStatus status)
        {
            return true;
        }

        public ICGunitHost Owner
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        public string PluginName
        {
            get
            {
                return "GNU Code Builder" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
                return GUnitFramework.Interfaces.PluginType.Builder;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Show(WeifenLuo.WinFormsUI.Docking.DockPanel dock, WeifenLuo.WinFormsUI.Docking.DockState state)
        {
            buildJobHandler = new GUnitFramework.Implementation.ExternalProcessHandler();
            m_ui = new ProjectbuilderUi(this);
            m_ui.Show(dock, WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide);
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
           
            return true;

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
                FirePropertyChange("BuildDirectory");
            }
        }

        public string CompilorPath
        {
            get
            {
                return m_CompilorPath;
            }
            set
            {
                m_CompilorPath = value;
                FirePropertyChange("CompilorPath");
            }
        }


        public bool IsCodeInstrumented
        {
            get
            {
                return m_isIntrumented;
            }
            set
            {
                m_isIntrumented = value;
                FirePropertyChange("IsCodeInstrumented");
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
                FirePropertyChange("OutputType");
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
                FirePropertyChange("WarningLevel");
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

                foreach (string str in Owner.ProjectData.IncludePaths)
                {
                    buildOptionCommandLine += "-I";
                    buildOptionCommandLine += str;
                }
                foreach (string str in Owner.ProjectData.PreIncludes)
                {

                    buildOptionCommandLine += "-include";
                    buildOptionCommandLine += (str);
                }
                foreach (string str in Owner.ProjectData.Defines)
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
                foreach (string str in Owner.ProjectData.LibPaths)
                {
                    LinkerOptionCommandLine += "-L";
                    LinkerOptionCommandLine += str;
                }
                foreach (string lib in Owner.ProjectData.LibNames)
                {
                    LinkerOptionCommandLine += "-l";
                    LinkerOptionCommandLine += lib;
                }

                ObjectList.Clear();
                foreach (string str in Owner.ProjectData.SourceFiles)
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
                        LinkCommands += "-o " + buildDir + "\\" + Owner.ProjectData.ProjectName + ".exe";
                        Owner.ProjectData.Output = buildDir + "\\" + Owner.ProjectData.ProjectName + ".exe";
                        break;
                    case OutputTypes.SharedLibrary:
                        LinkCommands += "-shared -o " + buildDir + "\\" + Owner.ProjectData.ProjectName + ".dll";
                        Owner.ProjectData.Output = buildDir + "\\" + Owner.ProjectData.ProjectName + ".dll";
                        break;
                    default:
                        LinkCommands += "-o " + buildDir + "\\" + Owner.ProjectData.ProjectName + ".exe";
                        Owner.ProjectData.Output = buildDir + "\\" + Owner.ProjectData.ProjectName + ".exe";
                        break;
                }

                foreach (ObjectList obj in ObjectList)
                {
                    string command = " " + String.Join(" ", buildOptionCommandLine.ToArray()) + " -o " + obj.objectPath + " -c " + obj.sourceFile;
                    buildCommands += command;
                    LinkCommands += obj.objectPath;
                }
                //adding google library files 
                //if (Directory.Exists(Path.GetFullPath("./GoogleLib")))
                //{
                //    if (Directory.Exists(Path.GetFullPath("./GoogleLib/gmock")) && Directory.Exists(Path.GetFullPath("./GoogleLib/gtest")))
                //    {
                //        string includeOption = " -DGTEST_HAS_TR1_TUPLE=1 -I" + Path.GetFullPath("./GoogleLib");
                //        if (File.Exists(Path.GetFullPath("./GoogleLib/gmock-gtest-all.cc")))
                //        {
                //            buildCommands += " " + String.Join(" ", includeOption + " -o " + objDir + "\\gmock-gtest-all.o" + " -c " + Path.GetFullPath("./GoogleLib/gmock-gtest-all.cc"));
                //            LinkCommands += objDir + "\\gmock-gtest-all.o";
                //        }
                //        if (File.Exists(Path.GetFullPath("./GoogleLib/gtest_main.cc")))
                //        {
                //            buildCommands += " " + String.Join(" ", includeOption + " -o " + objDir + "\\gtest_main.o" + " -c " + Path.GetFullPath("./GoogleLib/gtest_main.cc"));
                //            LinkCommands += objDir + "\\gtest_main.o";
                //        }
                //    }

                //}
                //end adding google library files


                LinkCommands.AddRange(LinkerOptionCommandLine);
                string projectLink = String.Join(" ", LinkCommands);
                buildCommands += projectLink;
            }
            catch
            {

            }
            return buildCommands;
        }
        public void addCompilorOutput(string value)
        {
            CompilorOutput += value;
            FirePropertyChange("ComplorLine");
        }
        public void clearCompilorOutput()
        {
            CompilorOutput.Clear();
            FirePropertyChange("ComplorLine");
        }
        public ListOfConsoleData CompilorOutput
        {
            get
            {
                return m_currentLine;
            }
            set
            {
                m_currentLine = value;
                
            }
        }
    }
}
