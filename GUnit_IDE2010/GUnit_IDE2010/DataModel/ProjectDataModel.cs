using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using System.Collections;

namespace Gunit.DataModel
{
    public class ObjectList
    {
        public string objectPath;
        public string sourceFile;
    }
    public class MethodPrototype
    {
        public string m_MethodName = "";
        public string m_returnType = "";
        public string m_parameters = "";
        public string m_parent = "";
        public CodeLocation m_location = new CodeLocation();
    }
    public enum WarningLevel
    {
        SyntaxCheckOnly,
        TreatAllWarningAsError,
        EnableAllWarning,
        AbortCompilationOnFirstError
    }
    public enum OutputTypes
    {
        ConsoleApplication,
        SharedLibrary
       
    }
    /// <summary>
    /// The Model for the List of Files
    /// </summary>
    public class ProjectDataModel : DataModelBase,IDisposable
    {
        #region members
        public const string CurrentDBVersion = "V1.9";
        public string m_DBVersion = "";
        private string m_DBPath = "";
        private string m_projectName = "";
        private string m_projectPath = "";
        private string m_projectBuildPath = "";
        private string m_compilerPath = "";
        private string m_ExecutablePath = "";
        private string m_TestReportPath = "";
        private string m_CoverageReportPath = "";
        private string m_ClangPath = "";
        private string m_STDHeaderpath = "";
        private WarningLevel m_WarningLevel = WarningLevel.EnableAllWarning;
        private bool m_EnableCoverage = true;
        private OutputTypes m_OutputType = new OutputTypes();
        /// <summary>
        /// List of Source Files
        /// </summary>
        private ListofFiles m_sourceFiles;
        /// <summary>
        /// List of Header Files
        /// </summary>
        private ListofFiles m_HeaderFiles;
        /// <summary>
        /// List of Common Header files
        /// </summary>
        private ListofFiles m_CommonHeaderFiles;
        /// <summary>
        /// List of All Include Paths
        /// </summary>
        private ListOfDirectories m_IncludePaths;
        /// <summary>
        /// List of all Library Paths
        /// </summary>
        private ListOfDirectories m_LibraryPaths;
        /// <summary>
        /// List of Library Names
        /// </summary>
        private ListofStrings m_LibNames;

        private ListofStrings m_MacroNames;

        private DataTable m_IncludeDataTable;
        private DataTable m_librarypathTable;
        private DataTable m_LibNameDataTable;
        private DataTable m_MacroDataTable;
        
        #endregion members
        #region constructor
        public ProjectDataModel() : base()
        {
            m_sourceFiles = new ListofFiles();
            m_HeaderFiles = new ListofFiles();
            m_CommonHeaderFiles = new ListofFiles();
            m_IncludePaths = new ListOfDirectories();
            m_LibraryPaths = new ListOfDirectories();
            m_LibNames = new ListofStrings();
            m_MacroNames = new ListofStrings();
            m_IncludeDataTable = new DataTable();
            m_IncludeDataTable.Columns.Add("IncludePaths");
            m_librarypathTable = new DataTable();
            m_librarypathTable.Columns.Add("LibraryPaths"); 
            m_LibNameDataTable = new DataTable();
            m_LibNameDataTable.Columns.Add("LibraryNames");
            m_MacroDataTable = new DataTable();
            m_MacroDataTable.Columns.Add("MacroDefines");
            

        }
        #endregion
        #region Properties
        public string Coveragereport
        {
            get { return m_CoverageReportPath; }
            set { m_CoverageReportPath = value; }
        }
        public string BuildPath
        {
            get { return m_projectBuildPath; }
            set { m_projectBuildPath = value; }
        }
        public string Testreport
        {
            get { return m_TestReportPath; }
            set { m_TestReportPath = value; }
        }
        public string Executable
        {
            get { return m_ExecutablePath; }
        }
        public OutputTypes ApplicationType
        {
            get { return m_OutputType; }
            set { m_OutputType = value; }
        }
        public string CompilorPath
        {
            get { return m_compilerPath; }
            set { m_compilerPath = value; }
        }
        public string ClangPath
        {
            get { return m_ClangPath; }
            set { m_ClangPath = value; }
        }
        public string STDHeaderPath
        {
            get { return m_STDHeaderpath; }
            set { m_STDHeaderpath = value; }
        }
        public WarningLevel WarningLevel
        {
            get { return m_WarningLevel; }
            set { m_WarningLevel = value; }
        }
        public bool EnableCoverage
        {
            get { return m_EnableCoverage; }
            set { m_EnableCoverage = value; }
        }
        public ListofStrings BuildCommands
        {
            get { return ProjectDataModel_GetGnuCommands(); }
        }

        public string DBPath
        {
            get { return m_DBPath; }
            set { m_DBPath = value; }
        }
       
        public string ProjectName
        {
            get
            {
                return m_projectName;
            }
            set
            {
                m_projectName = value;
                FirePropertyChange("ProjectName");
            }
        }
        public string ProjectPath
        {
            get
            {
                return m_projectPath;
            }
            set
            {
                m_projectPath = value;
                if (File.Exists(m_projectPath))
                {
                    BuildPath = Path.GetDirectoryName(m_projectPath) +"\\"+ Path.GetFileNameWithoutExtension(m_projectPath) + "_output";
                    m_CoverageReportPath = BuildPath + "\\" + "CoverageReport";
                    if(Directory.Exists(m_CoverageReportPath) == false)
                    {
                        Directory.CreateDirectory(m_CoverageReportPath);
                    }
                }
                FirePropertyChange("ProjectPath");
            }
        }
        /// <summary>
        /// Get the Source files or Add a new Sourcefile to the list
        /// </summary>
        public ListofFiles SourceFiles
        {
            get
            {
                return m_sourceFiles;
            }
            set
            {
                m_sourceFiles = value;
                
                FirePropertyChange("SourceFiles");

            }
        }
        /// <summary>
        /// Get the header files or Add a new Header File to the list
        /// </summary>
        public ListofFiles HeaderFiles
        {
            get
            {
                return m_HeaderFiles;
            }
            set
            {
                m_HeaderFiles = value;
                if (m_HeaderFiles.Count > 0)
                {
                    ProjectDataModel_addIncludePath(Path.GetDirectoryName(m_HeaderFiles[m_HeaderFiles.Count - 1]));
                    
                }
                FirePropertyChange("HeaderFiles");

            }
        }
        /// <summary>
        /// Get the Project header files or Add a new Project Header File to the list
        /// </summary>
        public ListofFiles ProjectHeaderFiles
        {
            get
            {
                return m_CommonHeaderFiles;
            }
            set
            {
                m_CommonHeaderFiles = value;
                if (m_CommonHeaderFiles.Count > 0)
                {
                    ProjectDataModel_addIncludePath(Path.GetDirectoryName(m_CommonHeaderFiles[m_CommonHeaderFiles.Count - 1]));
                    
                }
                FirePropertyChange("ProjectHeaderFiles");

            }
        }
        /// <summary>
        /// Property for Include paths for the Project
        /// </summary>
        public ListOfDirectories IncludePaths
        {
            get
            { return m_IncludePaths; }
            set
            {
                m_IncludePaths = value;
                FirePropertyChange("IncludePaths");
            }
        }
        /// <summary>
        /// Property for Library Paths
        /// </summary>
        public ListOfDirectories LibraryPaths
        {
            get
            {
                return m_LibraryPaths;
            }
            set
            {
                m_LibraryPaths = value;
                FirePropertyChange("LibraryPaths");
            }
        }
        /// <summary>
        /// Library Names to be used for Linking
        /// </summary>
        public ListofStrings LibraryNames
        {
            get
            {
                return m_LibNames;
            }
            set
            {
                m_LibNames = value;
                FirePropertyChange("LibraryNames");
            }
        }
        public ListofStrings MacroNames
        {
            get
            {
                return m_MacroNames;
            }
            set
            {
                m_MacroNames = value;
                FirePropertyChange("MacroNames");
            }
        }

        public DataTable IncludePathsTable
        {
            get { return m_IncludeDataTable; }
            
        }
        public DataTable LibraryPathsTable
        {
            get { return m_librarypathTable; }
        }
        public DataTable LibraryNameTable
        {
            get { return m_LibNameDataTable; }
        }
        public DataTable MacroDataTable
        {
            get { return m_MacroDataTable; }
        }

        #endregion Properties
        #region Methods
        public override void Datamodel_closeProject()
        {
            base.Datamodel_closeProject();
            ProjectDataModel_resetData();
        }
        public override void Datamodel_newProject()
        {
            base.Datamodel_newProject();
            ProjectDataModel_resetData();
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ListofStrings ProjectDataModel_GetGnuCommands()
        {
            ListofStrings buildCommands = new ListofStrings();
            try
            {

                ListofStrings LinkCommands = new ListofStrings();
                ListofStrings buildOptionCommandLine = new ListofStrings();
                ListofStrings LinkerOptionCommandLine = new ListofStrings();
                List<ObjectList> ObjectList = new List<DataModel.ObjectList>();
                string buildDir = BuildPath;
                string objDir = BuildPath+"\\obj";
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
                    case DataModel.WarningLevel.AbortCompilationOnFirstError:
                        buildOptionCommandLine += "-Wfatal-errors";
                        break;
                    case DataModel.WarningLevel.EnableAllWarning:
                        buildOptionCommandLine += "-Wall";
                        break;
                    case DataModel.WarningLevel.SyntaxCheckOnly:
                        buildOptionCommandLine += "-fsyntax-only";
                        break;
                    case DataModel.WarningLevel.TreatAllWarningAsError:
                        buildOptionCommandLine += "-Werror";
                        break;

                }

                buildOptionCommandLine += "-MMD";
                buildOptionCommandLine += "-MP";
                buildOptionCommandLine += "-ffast-math";
                buildOptionCommandLine += "-g";
                buildOptionCommandLine += "-O0";
                if (EnableCoverage == true)
                {
                    buildOptionCommandLine += "-fprofile-arcs";
                    buildOptionCommandLine += "-ftest-coverage";
                }
                buildOptionCommandLine += "-x c++";
                foreach (string str in IncludePaths)
                {
                    buildOptionCommandLine += "-I";
                    buildOptionCommandLine += str;
                }
                foreach (string str in ProjectHeaderFiles)
                {

                    buildOptionCommandLine += "-include";
                    buildOptionCommandLine += (str);
                }
                foreach (string str in MacroNames)
                {
                    buildOptionCommandLine += "-D";
                    buildOptionCommandLine += str;
                }
                buildOptionCommandLine += "-D";
                buildOptionCommandLine += "GTEST_HAS_TR1_TUPLE=1";
                if (EnableCoverage == true)
                {
                    LinkerOptionCommandLine += "-fprofile-arcs ";
                }
                foreach (string str in LibraryPaths)
                {
                    LinkerOptionCommandLine += "-L";
                    LinkerOptionCommandLine += str;
                }
                foreach (string lib in LibraryNames)
                {
                    LinkerOptionCommandLine += "-l";
                    LinkerOptionCommandLine += lib;
                }

                ObjectList.Clear();
                foreach (string str in SourceFiles)
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
                switch (ApplicationType)
                {
                    case OutputTypes.ConsoleApplication:
                        LinkCommands += "-o " + buildDir + "\\" + ProjectName + ".exe";
                        m_ExecutablePath = buildDir + "\\" + ProjectName + ".exe";
                        break;
                    case OutputTypes.SharedLibrary:
                        LinkCommands += "-shared -o " + buildDir + "\\" + ProjectName + ".dll";
                        m_ExecutablePath = buildDir + "\\" + ProjectName + ".dll";
                        break;
                    default:
                        LinkCommands += "-o " + buildDir + "\\" + ProjectName + ".exe";
                        m_ExecutablePath = buildDir + "\\" + ProjectName + ".exe";
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ListofStrings ProjectDataModel_GetClangCommandLine()
        {
            ListofStrings l_CommandLine = new ListofStrings();
           
            l_CommandLine += "-fno-ms-compatibility";
            l_CommandLine += "-std=c++11";
            
            foreach (string str in IncludePaths)
            {
                l_CommandLine += "-I";
                l_CommandLine += str;
            }
            
            foreach (string str in ProjectHeaderFiles)
            {
                if (Path.GetExtension(str) == ".pch")
                {
                    l_CommandLine += "-include";
                    l_CommandLine += (str);
                }
                else
                {
                    l_CommandLine += "-include";
                    l_CommandLine += (str);
                }
                
            }
            foreach (string str in LibraryPaths)
            {
                //l_CommandLine += "-L";
                //l_CommandLine += str;
            }
            foreach (string lib in LibraryNames)
            {
                //l_CommandLine += "-l";
                //l_CommandLine += lib;
            }
            foreach (string macro in MacroNames)
            {
                l_CommandLine += "-D";
                l_CommandLine += macro;
            }
            l_CommandLine += "-Wall";
            l_CommandLine += "-MMD";
            l_CommandLine += "-MP";
            //l_CommandLine += "-ffast-math";
            //l_CommandLine += "-g";
            //l_CommandLine += "-fprofile-arcs";
            //l_CommandLine += "-ftest-coverage";
            l_CommandLine += "-x";
            l_CommandLine += "c++";

            return l_CommandLine;
        }
        /// <summary>
        /// Adding the Include Path to the System
        /// </summary>
        /// <param name="path">absolute path to the Folder</param>
        public void ProjectDataModel_addIncludePath(string path)
        {
            if(Directory.Exists(path))
            {
                m_IncludeDataTable.Rows.Add(path);
                IncludePaths += path;
            }
            
        }
        /// <summary>
        /// Adding Library Path to the System
        /// </summary>
        /// <param name="path">Absolute path to the Directory</param>
        public void ProjectDataModel_addLibraryPath(string path)
        {
            if (Directory.Exists(path))
            {
                m_librarypathTable.Rows.Add(path);
                 LibraryPaths+= path;
            }

        }
        /// <summary>
        /// Adding the Library Name
        /// </summary>
        /// <param name="path">Name of the Library</param>
        public void ProjectDataModel_addLibraryName(string path)
        {
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                m_LibNameDataTable.Rows.Add(path);
                LibraryNames += path;
            }

        }
        public void ProjectDataModel_addMacroName(string path)
        {
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                m_MacroDataTable.Rows.Add(path);
                MacroNames += path;
            }

        }
        /// <summary>
        /// Remove the Include Path from the System
        /// </summary>
        /// <param name="index">DataRow to the Removed</param>
        /// <param name="value">Value of the Include Path to be removed</param>
        public void ProjectDataModel_RemoveIncludePath(DataRow index,string value)
        {
           
            m_IncludeDataTable.Rows.Remove(index);
            IncludePaths -= value;
         }
        /// <summary>
        /// Remove the Library Path from the System
        /// </summary>
        /// <param name="index">DataRow to the Removed</param>
        /// <param name="value">Value of the Include Path to be removed</param>
        public void ProjectDataModel_RemoveLibraryPath(DataRow index, string value)
        {

            m_librarypathTable.Rows.Remove(index);
            LibraryPaths -= value;
        }
        /// <summary>
        /// Remove the Library Name from the System
        /// </summary>
        /// <param name="index">DataRow to the Removed</param>
        /// <param name="value">Value of the Include Path to be removed</param>
        public void ProjectDataModel_RemoveLibraryName(DataRow index, string value)
        {

            m_LibNameDataTable.Rows.Remove(index);
            LibraryNames -= value;
        }
        public void ProjectDataModel_RemoveMacroName(DataRow index, string value)
        {

            m_MacroDataTable.Rows.Remove(index);
            MacroNames -= value;
        }
        private void ProjectDataModel_resetData()
        {
            this.HeaderFiles.Clear();
            this.SourceFiles.Clear();
            this.ProjectHeaderFiles.Clear();
            this.ProjectPath = "";
            this.ProjectName = "";
            this.IncludePaths.Clear();
            this.IncludePathsTable.Clear();
            this.LibraryNames.Clear();
            this.LibraryNameTable.Clear();
            this.LibraryPaths.Clear();
            this.LibraryPathsTable.Clear();
        }
        /// <summary>
        /// Read the Project XML file
        /// </summary>
        /// <param name="l_ProjectPath">Path to the Project XML File</param>
        public void ProjectDataModel_ReadProjectData(string l_ProjectPath)
        {
            if (File.Exists(l_ProjectPath))
            {
                ProjectDataModel_resetData();
                ProjectPath = l_ProjectPath;
                ProjectName = Path.GetFileNameWithoutExtension(l_ProjectPath);
               
                try
                {
                    XDocument doc = XDocument.Load(l_ProjectPath);
                    ProjectName = doc.Root.Attribute("name").Value.ToString();
                    ProjectPath = l_ProjectPath;
                    IEnumerable<XElement> DBPath = from files in doc.Root.Descendants("Database")
                                                            select files;
                  

                    foreach (XElement dbpath in DBPath)
                    {
                        string databasePath = hasAttribute(dbpath, "Path", "");
                        if (string.IsNullOrWhiteSpace(databasePath) == false)
                        {
                            var s = Path.Combine(Path.GetDirectoryName(l_ProjectPath), databasePath);
                            s = Path.GetFullPath(s);
                            this.DBPath = s;
                           
                        }

                    }
                    if (ProjectDataModel.CurrentDBVersion != m_DBVersion)
                    {
                        if (File.Exists(this.DBPath))
                        {
                            File.Delete(this.DBPath);
                        }
                    }
                    if (File.Exists(this.DBPath) == false )
                    {
                        this.DBPath = Path.GetDirectoryName(l_ProjectPath) + "\\" + ProjectName + ".sdf";
                        if (File.Exists(this.DBPath))
                        {
                            File.Delete(this.DBPath);
                        }
                        DBManager m = new DBManager(this.DBPath);

                    }
                    
                    IEnumerable<XElement> SourceFilesList = from files in doc.Root.Descendants("SourceFiles")
                                                            select files;
                    IEnumerable<XElement> l_SourceFiles = from files in SourceFilesList.Descendants("File")
                                                        select files;

                    foreach (XElement srcFile in l_SourceFiles)
                    {
                        string sourcePath = hasAttribute(srcFile, "Path", "");
                        if (string.IsNullOrWhiteSpace(sourcePath) == false)
                        {
                            var s = Path.Combine(Path.GetDirectoryName(l_ProjectPath), sourcePath);
                            s = Path.GetFullPath(s);
                            this.SourceFiles += s;
                           
                        }

                    }
                    IEnumerable<XElement> HeaderFilesList = from files in doc.Root.Descendants("HeaderFiles")
                                                            select files;
                    IEnumerable<XElement> l_HeaderFiles = from files in HeaderFilesList.Descendants("File")
                                                        select files;
                    foreach (XElement hedFile in l_HeaderFiles)
                    {
                        string headerPath = hasAttribute(hedFile, "Path", "");
                        if (string.IsNullOrWhiteSpace(headerPath) == false)
                        {
                            var s = Path.Combine(Path.GetDirectoryName(l_ProjectPath), headerPath);
                            s = Path.GetFullPath(s);
                            this.HeaderFiles += s;
                        }
                    }
                    IEnumerable<XElement> ProjHeaderList = from files in doc.Root.Descendants("ProjectHeaders")
                                                           select files;
                    IEnumerable<XElement> projHeaders = from files in ProjHeaderList.Descendants("Include")
                                                        select files;
                    foreach (XElement header in projHeaders)
                    {
                        string commonHeader = hasAttribute(header, "Path", "");
                        if (string.IsNullOrWhiteSpace(commonHeader) == false)
                        {
                            var s = Path.Combine(Path.GetDirectoryName(l_ProjectPath), commonHeader);
                            s = Path.GetFullPath(s);
                            this.ProjectHeaderFiles += s;
                        }


                    }
                    IEnumerable<XElement> includeList = from files in doc.Root.Descendants("IncludePaths")
                                                        select files;
                    IEnumerable<XElement> includes = from files in includeList.Descendants("Include")
                                                     select files;
                    foreach (XElement include in includes)
                    {
                        string includePath = hasAttribute(include, "Path", "");
                        if (string.IsNullOrWhiteSpace(includePath) == false)
                        {
                            var s = Path.Combine(Path.GetDirectoryName(l_ProjectPath), includePath);
                            s = Path.GetFullPath(s);
                            ProjectDataModel_addIncludePath(s);
                            

                        }
                            

                    }
                    IEnumerable<XElement> LibPathList = from files in doc.Root.Descendants("LibPaths")
                                                        select files;
                    IEnumerable<XElement> LibPaths = from files in LibPathList.Descendants("Lib")
                                                     select files;
                    foreach (XElement lib in LibPaths)
                    {
                        string includePath = hasAttribute(lib, "Path", "");
                        if (string.IsNullOrWhiteSpace(includePath) == false)
                        {
                            var s = Path.Combine(Path.GetDirectoryName(l_ProjectPath), includePath);
                            s = Path.GetFullPath(s);
                            ProjectDataModel_addLibraryPath(s);
                        }
                       

                    }
                    IEnumerable<XElement> LibNameList = from files in doc.Root.Descendants("LibNames")
                                                        select files;
                    IEnumerable<XElement> LibNames = from files in LibNameList.Descendants("LibName")
                                                     select files;
                    foreach (XElement libName in LibNames)
                    {
                        string includePath = hasAttribute(libName, "Path", "");
                        if (string.IsNullOrWhiteSpace(includePath) == false)
                        {
                            ProjectDataModel_addLibraryName(includePath);
                        }
                        m_DBVersion = hasAttribute(libName, "Version", "");
                        
                    }
                    IEnumerable<XElement> definesList = from files in doc.Root.Descendants("Defines")
                                                        select files;
                    IEnumerable<XElement> defineList = from files in definesList.Descendants("Define")
                                                     select files;
                    foreach (XElement define in defineList)
                    {
                        string macro = hasAttribute(define, "value", "");
                        if (string.IsNullOrWhiteSpace(macro) == false)
                        {
                            ProjectDataModel_addMacroName(macro);
                        }
                    }

                    
                }
                catch (Exception err)
                {
                    Console.WriteLine("Exception!!" + err.ToString());
                }
            }
        }
        /// <summary>
        /// Write the Project XML back to disk
        /// </summary>
        /// <param name="l_projectPath">Path to the Project XML file</param>
        public void ProjectDatamodel_writeProjectData()
        {
            try
            {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.CloseOutput = true;
                string rootPath = Path.GetDirectoryName(ProjectPath);

                using (XmlWriter writer = XmlWriter.Create(ProjectPath, settings))
                {

                    writer.WriteStartDocument();
                    writer.WriteStartElement("Project");
                    writer.WriteAttributeString("name", ProjectName);
                    writer.WriteStartElement("Files");
                    writer.WriteStartElement("SourceFiles");
                    foreach (string source in SourceFiles)
                    {

                        writer.WriteStartElement("File");
                        writer.WriteAttributeString("Path", getReleativePathWith(source, rootPath));
                        writer.WriteEndElement();//File
                    }
                    writer.WriteEndElement();//SourceFiles

                    writer.WriteStartElement("HeaderFiles");
                    foreach (string header in HeaderFiles)
                    {

                        writer.WriteStartElement("File");
                        writer.WriteAttributeString("Path", getReleativePathWith(header, rootPath));
                        writer.WriteEndElement();//File
                    }
                    writer.WriteEndElement();//HeaderFiles
                    writer.WriteEndElement();//Files
                    writer.WriteStartElement("ProjectHeaders");

                    foreach (string commonHeader in ProjectHeaderFiles)
                    {

                        writer.WriteStartElement("Include");
                        writer.WriteAttributeString("Path", getReleativePathWith(commonHeader, rootPath));
                        writer.WriteEndElement();//Include

                    }
                    writer.WriteEndElement();//ProjectHeaders
                    writer.WriteStartElement("IncludePaths");

                    foreach (string inc in m_IncludePaths)
                    {
                        writer.WriteStartElement("Include");
                        writer.WriteAttributeString("Path", getReleativePathWith(inc, rootPath));
                        writer.WriteEndElement();//Include
                    }
                    writer.WriteEndElement();//IncludePaths
                    writer.WriteStartElement("LibPaths");

                    foreach (string lib in m_LibraryPaths)
                    {
                        writer.WriteStartElement("Lib");
                        writer.WriteAttributeString("Path", getReleativePathWith(lib, rootPath));
                        writer.WriteEndElement();//Lib
                    }
                    writer.WriteEndElement();//LibPaths
                    writer.WriteStartElement("LibNames");

                    foreach (string libName in m_LibNames)
                    {
                        writer.WriteStartElement("LibName");
                        writer.WriteAttributeString("Path", libName);
                        writer.WriteEndElement();//LibName
                    }
                    writer.WriteEndElement();//LibNames
                    writer.WriteStartElement("Defines");

                    foreach (string macroName in m_MacroNames)
                    {
                        writer.WriteStartElement("Define");
                        writer.WriteAttributeString("value", macroName);
                        writer.WriteEndElement();//LibName
                    }
                    writer.WriteEndElement();//Defines
                    writer.WriteStartElement("Bin");
                    writer.WriteAttributeString("Path", "./");
                    writer.WriteEndElement();//Bin
                    writer.WriteStartElement("Database");

                    writer.WriteAttributeString("Path", getReleativePathWith(this.DBPath, rootPath));
                    writer.WriteAttributeString("Version", m_DBVersion);
                    writer.WriteEndElement();//Database
                    writer.WriteEndElement();//Project
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// Check if a pariculat XML element has an attribute and 
        /// return its value if existing
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="xns"></param>
        /// <returns></returns>
        public static string hasAttribute(XElement element, string attribute, XNamespace xns)
        {
            string result = "";
            result = element.Attribute(xns + attribute) != null ? element.Attribute(xns + attribute).Value.ToString() : "";
            return result;
        }
        /// <summary>
        /// Add or remove file from the Project
        /// </summary>
        /// <param name="IsFileAdded"> true if file to be added else false</param>
        /// <param name="l_fileType">SourceFile: HeaderFile:ProjectHeaderFile</param>
        /// <param name="fileName">Absolute Path to the File</param>
        public void ProjectDataModel_AddOrRemoveFile(bool IsFileAdded, FileType l_fileType, string fileName)
        {
            switch (l_fileType)
            {
                case FileType.HEADER_FILE:
                    if (IsFileAdded == true)
                    {
                        HeaderFiles += fileName;
                    }
                    else
                    {
                        HeaderFiles -= fileName;
                    }
                    break;
                case FileType.PROJECT_HEADER_FILE:
                    if (IsFileAdded == true)
                    {
                        ProjectHeaderFiles += fileName;
                    }
                    else
                    {
                        ProjectHeaderFiles -= fileName;
                    }
                    break;
                case FileType.SOURCE_FILE:
                    if (IsFileAdded == true)
                    {
                        SourceFiles += fileName;
                    }
                    else
                    {
                       SourceFiles -= fileName;
                    }
                    break;
                default:
                    break;
            }
            
          
        }

        public void Dispose()
        {
          m_IncludeDataTable.Dispose();
          m_librarypathTable.Dispose();
          m_LibNameDataTable.Dispose();
          m_MacroDataTable.Dispose();
        }

        #endregion

    }
}
