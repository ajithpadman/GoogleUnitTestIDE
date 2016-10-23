using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUnitFramework.Interfaces;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using CPPASTBuilder.Interfaces;
namespace Gunit.Ui
{
    public partial class Gunit : Form,ICGunitHost
    {
        

        public Gunit()
        {
            InitializeComponent();
        }
        #region members
         Splash splash = new Splash();
        
      
        ProjectSetting m_frmPrj;
        Plugins m_frmPlugins;
        Editor m_frmEditor;
        List<ITestReportGenerator> m_availableTestReportGen = new List<ITestReportGenerator>();
        IBoundaryTestGenerator m_BoundaryTester = null;
        ICppParser m_CppParser = null;
        IParser m_parser = null;
        IMockGenrator m_mockGenerator = null;
        ICoverageAnalyser m_coverageAnalyser = null;
        ITestReportGenerator m_testReportGen = null;
        ICodeBuilder m_builder = null;
        ICProjectData m_projectData = null;
        List<ICGunitPlugin> m_pluginList = new List<ICGunitPlugin>();
        List<ICGunitPlugin> m_SpecialpluginList = new List<ICGunitPlugin>();
        List<string> m_selectedFiles = new List<string>();
        ItestRunner m_TestRunner = null;
        string m_CurrentFileInEditor = null;
        private DataTable m_IncludeDataTable;
        private DataTable m_librarypathTable;
        private DataTable m_LibNameDataTable;
        private DataTable m_MacroDataTable;
        List<ASTBuilder.Interfaces.ICCodeDescription> m_codeDescriptions = new List<ASTBuilder.Interfaces.ICCodeDescription>();
        List<ICppCodeDescription> m_CppcodeDescriptions = new List<ICppCodeDescription>();
        #endregion
        #region Events
        public event onBuidComplete evBuildComplete = delegate { };

        public event onBuildStarted evBuildStarted = delegate { };

        public event onParserOutput evParserOutput = delegate { };

        public event onProjectStatus evProjectStatus = delegate { };
        public event onPluginLoaded evPluginLoaded = delegate { };

        protected event PropertyChangedEventHandler m_propertyChanged = delegate { };

        #endregion Events

        #region Properties
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
        /// <summary>
        /// 
        /// </summary>
        public string CurrentFileInEditor
        {
            get{return m_CurrentFileInEditor;}
            set
            {
                m_CurrentFileInEditor = value;
                FirePropertyChange("CurrentFile");
            }
        }
        /// <summary>
        /// Event for PropertChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { m_propertyChanged += value; }
            remove { m_propertyChanged -= value; }
        }
        /// <summary>
        /// List of avialble Test Report Generator from which user can choose
        /// </summary>
        public List<ITestReportGenerator> AvailableTestReportGenerators
        {
            get
            {
                return m_availableTestReportGen;
            }
           
        }

        /// <summary>
        /// code generator for boundary test cases
        /// </summary>
        public IBoundaryTestGenerator BoundaryTestGenerator
        {
            get
            {
                return m_BoundaryTester;
            }
           
        }

        /// <summary>
        /// parser for C file to identify functions and global variables involved in Unit test
        /// </summary>
        public IParser CodeParser
        {
            get
            {
                return m_parser;
            }
            
        }
        public ICppParser CPPCodeParser
        {
            get
            {
                return m_CppParser;
            }

        }

        /// <summary>
        /// Coverage Analyser after the test run complete
        /// </summary>
        public ICoverageAnalyser CoverageAnalyser
        {
            get
            {
                return m_coverageAnalyser;
            }
           
        }

        /// <summary>
        /// Chosen Test Report Generator
        /// </summary>
        public ITestReportGenerator CurrentTestReportGenerator
        {
            get
            {
                return m_testReportGen;
            }
            
        }
        /// <summary>
        /// List of all plugins loaded in the GUnitHost
        /// </summary>
        public List<ICGunitPlugin> PluginList
        {
            get
            {
                return m_pluginList;
            }
           
        }
        public List<ICGunitPlugin> SpecialPluginList
        {
            get
            {
                return m_SpecialpluginList;
            }
           
        }
        /// <summary>
        /// the Code builder plugin
        /// </summary>
        public ICodeBuilder ProjectBuilder
        {
            get
            {
                return m_builder;
            }
            
        }
        /// <summary>
        /// the data containing information on source files include paths etc. 
        /// </summary>
        public ICProjectData ProjectData
        {
            get
            {
                return m_projectData;
            }
            set
            {
                m_projectData = value;
                FirePropertyChange("ProjectData");
            }
        }
        /// <summary>
        ///  the files selected for parsing mock generation etc.
        /// </summary>
        public List<string> SelectedFiles
        {
            get
            {
                return m_selectedFiles;
            }
            
        }
        
        /// <summary>
        /// the plugin which run the tests
        /// </summary>
        public ItestRunner TestRunner
        {
            get
            {
                return m_TestRunner;
            }
           
        }
        public IMockGenrator MockGenerator
        {
            get
            {
                return m_mockGenerator;
            }
           
        }
        public List<ASTBuilder.Interfaces.ICCodeDescription> CodeDescriptions
        {
            get
            {
                return m_codeDescriptions;
            }
            set
            {
                m_codeDescriptions = value;
            }
        }
        #endregion Properties

        #region Methods
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

        #region Project Sessions
        /// <summary>
        /// handling closing of Project. All plugins need to be notified about the project closure
        /// </summary>
        /// <param name="fileName">project File name</param>
        protected void Host_CloseProjectSession(string fileName)
        {
           
        }
        /// <summary>
        /// Called to create a new project. all plugins are notified about the new project creation
        /// </summary>
        /// <param name="fileName">project file name</param>
        protected void Host_CreateProjectSession()
        {

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML Files (*.xml)|*.xml";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ProjectData = new GUnitFramework.Implementation.ProjectData();
                ProjectData.ProjectPath = dlg.FileName;
                ProjectData.ProjectName = Path.GetFileNameWithoutExtension( dlg.FileName);
                evProjectStatus(ProjectStatus.NEW, dlg.FileName);
            }
        }
        /// <summary>
        /// called when an existing project is opened by the Host
        /// Notified to all plugins about project open
        /// </summary>
        /// <param name="fileName"></param>
        protected void Host_OpenProjectSession()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML Files (*.xml)|*.xml";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ProjectData = new GUnitFramework.Implementation.ProjectData();
                if (Host_ReadProjectData(dlg.FileName))
                {
                    evProjectStatus(ProjectStatus.OPEN, dlg.FileName);
                }
            }

        }
        /// <summary>
        /// called when the project is saved. notified to all plugins
        /// </summary>
        /// <param name="fileName"></param>
        protected void Host_SaveProjectSession(string fileName)
        {
             Host_writeProjectData();
             evProjectStatus(ProjectStatus.SAVE, fileName);
        
        }
        #endregion Project Sessions
        public void addSelectedFiles(string file)
        {
            
            SelectedFiles.Add(file);
            FirePropertyChange("SELECTED_FILE");
        }
        public void removeSelectedFiles(string file)
        {
            SelectedFiles.RemoveAll(item => item == file);
            FirePropertyChange("SELECTED_FILE");
        }

        #region LoadPlugins
        /// <summary>
        /// function to load a dll for the special user defined plugin. the function is called by the host after reading 
        /// the config file
        /// </summary>
        /// <param name="dllPath">path to the dll</param>
        /// <param name="className">class name of the Plugin</param>
        protected void LoadPlugin(string dllPath, string className)
        {
             Type ObjType = null;
             if (File.Exists(dllPath))
             {
                 
                 try
                 {
                     Assembly ass = null;
                     ass = Assembly.LoadFile(dllPath);
                     if (ass != null)
                     {
                         ObjType = ass.GetType(className);
                         if (ObjType != null)
                         {
                             ICGunitPlugin plugin = (ICGunitPlugin)Activator.CreateInstance(ObjType);
                             if (plugin != null)
                             {
                                 plugin.registerCallBack(this);
                                 PluginList.Add(plugin);
                                 plugin.Owner = this;
                                 m_SpecialpluginList.Add(plugin);
                                 plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                 evPluginLoaded(plugin);
                                 


                             }
                             else
                             {
                                 //do nothing
                             }
                         }
                     }
                 }
                 catch (Exception err)
                 {
                     MessageBox.Show(err.ToString());

                 }
             }
        }
      
        /// <summary>
        /// Load the Boundary test generation plugin
        /// </summary>
        /// <param name="dllPath">path to the plugin Dll</param>
        /// <param name="className">class name of the plugin </param>
        protected void LoadBoundaryTestPlugin(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {
               
                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            IBoundaryTestGenerator plugin = (IBoundaryTestGenerator)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                plugin.Owner = this;
                                m_BoundaryTester = plugin;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);
                              

                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }
        /// <summary>
        /// Load the Boundary test generation plugin
        /// </summary>
        /// <param name="dllPath">path to the plugin Dll</param>
        /// <param name="className">class name of the plugin </param>
        protected void LoadCPPParserPlugin(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {

                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            ICppParser plugin = (ICppParser)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                plugin.Owner = this;
                                m_CppParser = plugin;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);


                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }
      /// <summary>
      /// Load the ProjectBuilder plugin which compiles the project
      /// </summary>
      /// <param name="dllPath">Path to the Dll</param>
      /// <param name="className">Class name of the plugin</param>
        protected void LoadBuilderPlugin(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {
               
                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            ICodeBuilder plugin = (ICodeBuilder)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                m_builder = plugin;
                                plugin.Owner = this;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);

                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }
        /// <summary>
        /// Load the coverage analysis plugin
        /// </summary>
        /// <param name="dllPath">Path to the dll</param>
        /// <param name="className">Class Name of the dll</param>
        protected void LoadCoverageAnalyserPlugin(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {
               
                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            ICoverageAnalyser plugin = (ICoverageAnalyser)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                m_coverageAnalyser = plugin;
                                plugin.Owner = this;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);

                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }
        /// <summary>
        /// Load the parser plugin used to parse C code using ASTBuilder library
        /// </summary>
        /// <param name="dllPath">Path to the dll</param>
        /// <param name="className">Class name of the plugin</param>
        protected void LoadParserPlugin(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {
               
                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            IParser plugin = (IParser)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                m_parser = plugin;
                                plugin.Owner = this;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);

                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }
        /// <summary>
        /// load the report generator plugin
        /// </summary>
        /// <param name="dllPath">path to the dll</param>
        /// <param name="className">class name of the Plugin</param>
        protected void LoadTestReportGenerator(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {
              
                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            ITestReportGenerator plugin = (ITestReportGenerator)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                AvailableTestReportGenerators.Add(plugin);
                                m_testReportGen = plugin;
                                plugin.Owner = this;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);

                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }
        /// <summary>
        /// Load the test runner plugin which run the test executable
        /// </summary>
        /// <param name="dllPath">Path to the Dll</param>
        /// <param name="className">Class name of the plugin</param>
        protected void LoadTestRunnerPlugin(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {
               
                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            ItestRunner plugin = (ItestRunner)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                m_TestRunner = plugin;
                                plugin.Owner = this;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);

                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }
        /// <summary>
        /// Load the mock generator plugin
        /// </summary>
        /// <param name="dllPath">path to the dll</param>
        /// <param name="className">Class name of the plugin</param>
        protected void LoadMockGenerator(string dllPath, string className)
        {
            Type ObjType = null;
            if (File.Exists(dllPath))
            {
            
                try
                {
                    Assembly ass = null;
                    ass = Assembly.LoadFile(dllPath);
                    if (ass != null)
                    {
                        ObjType = ass.GetType(className);
                        if (ObjType != null)
                        {
                            IMockGenrator plugin = (IMockGenrator)Activator.CreateInstance(ObjType);
                            if (plugin != null)
                            {
                                plugin.registerCallBack(this);
                                PluginList.Add(plugin);
                                m_mockGenerator = plugin;
                                plugin.Owner = this;
                                plugin.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide);
                                evPluginLoaded(plugin);

                            }
                            else
                            {
                                //do nothing
                            }
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());

                }
            }
        }

        #endregion LoadPlugins


        /// <summary>
        /// Adding the Include Path to the System
        /// </summary>
        /// <param name="path">absolute path to the Folder</param>
        public void ProjectDataModel_addIncludePath(string path)
        {
            if (Directory.Exists(path))
            {
                if (m_projectData.IncludePaths.Contains(path) == false)
                {
                    m_IncludeDataTable.Rows.Add(path);
                    m_projectData.IncludePaths.Add(path);
                }
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
                if (m_projectData.LibPaths.Contains(path) == false)
                {
                    m_librarypathTable.Rows.Add(path);
                    m_projectData.LibPaths.Add(path);
                }
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
                if (m_projectData.LibNames.Contains(path) == false)
                {
                    m_LibNameDataTable.Rows.Add(path);
                    m_projectData.LibNames.Add(path);
                }
            }

        }
        public void ProjectDataModel_addMacroName(string path)
        {
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                if (m_projectData.Defines.Contains(path) == false)
                {
                    m_MacroDataTable.Rows.Add(path);
                    m_projectData.Defines.Add(path);
                }
            }

        }
        /// <summary>
        /// Remove the Include Path from the System
        /// </summary>
        /// <param name="index">DataRow to the Removed</param>
        /// <param name="value">Value of the Include Path to be removed</param>
        public void ProjectDataModel_RemoveIncludePath(DataRow index, string value)
        {

            m_IncludeDataTable.Rows.Remove(index);
            if (m_projectData.IncludePaths.Contains(value) == true)
            {
                m_projectData.IncludePaths.Remove(value);
            }
            
        }
        /// <summary>
        /// Remove the Library Path from the System
        /// </summary>
        /// <param name="index">DataRow to the Removed</param>
        /// <param name="value">Value of the Include Path to be removed</param>
        public void ProjectDataModel_RemoveLibraryPath(DataRow index, string value)
        {

            m_librarypathTable.Rows.Remove(index);
            if (m_projectData.LibPaths.Contains(value) == true)
            {
                m_projectData.LibPaths.Remove(value);
            }
        }
        /// <summary>
        /// Remove the Library Name from the System
        /// </summary>
        /// <param name="index">DataRow to the Removed</param>
        /// <param name="value">Value of the Include Path to be removed</param>
        public void ProjectDataModel_RemoveLibraryName(DataRow index, string value)
        {

            m_LibNameDataTable.Rows.Remove(index);
            if (m_projectData.LibNames.Contains(value) == true)
            {
                m_projectData.LibNames.Remove(value);
            }
        }
        /// <summary>
        /// Remove the define
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void ProjectDataModel_RemoveMacroName(DataRow index, string value)
        {

            m_MacroDataTable.Rows.Remove(index);
            if (m_projectData.Defines.Contains(value) == true)
            {
                m_projectData.Defines.Remove(value);
            }
        }
        /// <summary>
        /// Add or remove file from the Project
        /// </summary>
        /// <param name="IsFileAdded"> true if file to be added else false</param>
        /// <param name="l_fileType">SourceFile: HeaderFile:ProjectHeaderFile</param>
        /// <param name="fileName">Absolute Path to the File</param>
        public void AddOrRemoveFile(bool IsFileAdded, FileType l_fileType, string fileName)
        {
            if (null != ProjectData)
            {
                switch (l_fileType)
                {
                    case FileType.HeaderFile:
                        if (IsFileAdded == true)
                        {
                            ProjectData.HeaderFiles.Add(fileName);
                            FirePropertyChange("HeaderFilesAdd");
                        }
                        else
                        {
                            ProjectData.HeaderFiles.RemoveAll(item => item == fileName);
                            FirePropertyChange("HeaderFilesRemove");
                        }
                        FirePropertyChange("ProjectData");
                        break;
                    case FileType.PreInclude:
                        if (IsFileAdded == true)
                        {
                            ProjectData.PreIncludes.Add(fileName);
                            FirePropertyChange("PreIncludesAdd");
                        }
                        else
                        {
                            ProjectData.PreIncludes.RemoveAll(item => item == fileName);
                            FirePropertyChange("PreIncludesRemove");
                        }
                        FirePropertyChange("ProjectData");
                        break;
                    case FileType.SourceFile:
                        if (IsFileAdded == true)
                        {
                            ProjectData.SourceFiles.Add(fileName);
                            FirePropertyChange("SourceFilesAdd");
                        }
                        else
                        {
                            ProjectData.SourceFiles.RemoveAll(item => item == fileName);
                            FirePropertyChange("SourceFilesRemove");
                        }
                        FirePropertyChange("ProjectData");
                        break;
                    default:
                        break;
                }
            }


        }

        private void readPluginConfig()
        {
            try
            {
                if (File.Exists("Plugins.cnf"))
                {
                    string[] array = File.ReadAllLines("Plugins.cnf");
                    foreach (string plugindata in array)
                    {
                        string[] plugin = plugindata.Split('#');
                        if (plugin[0] == "Parser")
                        {

                            LoadParserPlugin(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "CPPParser")
                        {
                            LoadCPPParserPlugin(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "BoundaryTester")
                        {
                            LoadBoundaryTestPlugin(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "MockGenerator")
                        {
                            LoadMockGenerator(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "TestReportGenerator")
                        {
                            LoadTestReportGenerator(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "TestRunner")
                        {
                            LoadTestRunnerPlugin(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "CoverageAnalyser")
                        {
                            LoadCoverageAnalyserPlugin(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "CodeBuilder")
                        {
                            LoadBuilderPlugin(Path.GetFullPath(plugin[1]), plugin[2]);
                        }
                        else if (plugin[0] == "SpecialPlugin")
                        {
                            LoadPlugin(plugin[1], plugin[2]);
                        }
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Error reading plugin configuration from  Plugins.cnf file");
            }
        }
        /// <summary>
        /// Get the relative path with respect to the Root path
        /// </summary>
        /// <param name="SelectedPath">Absolute path for which relative path needs to be foundout</param>
        /// <param name="rootPath">Absolute path to the Root Directory</param>
        /// <returns>Relative Path</returns>
        public static string getReleativePathWith(string SelectedPath, string rootPath)
        {
            string relPath = "";
            try
            {

                System.Uri path = new Uri(SelectedPath);
                System.Uri cur = new Uri(rootPath + "\\");
                relPath = cur.MakeRelativeUri(path).ToString();
                if (string.IsNullOrWhiteSpace(relPath))
                {
                    relPath = ".";
                }
            }
            catch
            {

            }

            return relPath;
        }
        private void ProjectDataModel_resetData()
        {
            ProjectData.HeaderFiles.Clear();
            this.SelectedFiles.Clear();
            ProjectData.SourceFiles.Clear();
            ProjectData.PreIncludes.Clear();
            ProjectData.ProjectPath = "";
            ProjectData.ProjectName = "";
            ProjectData.IncludePaths.Clear();
            this.IncludePathsTable.Clear();
            ProjectData.LibNames.Clear();
            this.LibraryNameTable.Clear();
            ProjectData.LibPaths.Clear();
            this.LibraryPathsTable.Clear();
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
        /// Read the Project XML file
        /// </summary>
        /// <param name="l_ProjectPath">Path to the Project XML File</param>
        public bool Host_ReadProjectData(string l_ProjectPath)
        {
            bool returnVal = true;
            if (File.Exists(l_ProjectPath))
            {
                ProjectDataModel_resetData();
                
                ProjectData.ProjectPath = l_ProjectPath;
                ProjectData.ProjectName = Path.GetFileNameWithoutExtension(l_ProjectPath);

                try
                {
                    XDocument doc = XDocument.Load(l_ProjectPath);
                    ProjectData.ProjectName = doc.Root.Attribute("name").Value.ToString();
                    ProjectData.ProjectPath = l_ProjectPath;
                  

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
                            ProjectData.SourceFiles.Add( s);

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
                            ProjectData.HeaderFiles.Add(s);
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
                            ProjectData.PreIncludes.Add(s);
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
                    MessageBox.Show(err.ToString());

                    returnVal = false;

                }

            }
            else
            {
                returnVal = false;
            }
            return returnVal;
        }
        public void Host_writeProjectData()
        {
            try
            {
                if (ProjectData != null)
                {
                    var settings = new XmlWriterSettings();
                    settings.OmitXmlDeclaration = true;
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    settings.CloseOutput = true;
                    string rootPath = Path.GetDirectoryName(ProjectData.ProjectPath);

                    using (XmlWriter writer = XmlWriter.Create(ProjectData.ProjectPath, settings))
                    {

                        writer.WriteStartDocument();
                        writer.WriteStartElement("Project");
                        writer.WriteAttributeString("name", ProjectData.ProjectName);
                        writer.WriteStartElement("Files");
                        writer.WriteStartElement("SourceFiles");
                        foreach (string source in ProjectData.SourceFiles)
                        {

                            writer.WriteStartElement("File");
                            writer.WriteAttributeString("Path", getReleativePathWith(source, rootPath));
                            writer.WriteEndElement();//File
                        }
                        writer.WriteEndElement();//SourceFiles

                        writer.WriteStartElement("HeaderFiles");
                        foreach (string header in ProjectData.HeaderFiles)
                        {

                            writer.WriteStartElement("File");
                            writer.WriteAttributeString("Path", getReleativePathWith(header, rootPath));
                            writer.WriteEndElement();//File
                        }
                        writer.WriteEndElement();//HeaderFiles
                        writer.WriteEndElement();//Files
                        writer.WriteStartElement("ProjectHeaders");

                        foreach (string commonHeader in ProjectData.PreIncludes)
                        {

                            writer.WriteStartElement("Include");
                            writer.WriteAttributeString("Path", getReleativePathWith(commonHeader, rootPath));
                            writer.WriteEndElement();//Include

                        }
                        writer.WriteEndElement();//ProjectHeaders
                        writer.WriteStartElement("IncludePaths");

                        foreach (string inc in ProjectData.IncludePaths)
                        {
                            writer.WriteStartElement("Include");
                            writer.WriteAttributeString("Path", getReleativePathWith(inc, rootPath));
                            writer.WriteEndElement();//Include
                        }
                        writer.WriteEndElement();//IncludePaths
                        writer.WriteStartElement("LibPaths");

                        foreach (string lib in ProjectData.LibPaths)
                        {
                            writer.WriteStartElement("Lib");
                            writer.WriteAttributeString("Path", getReleativePathWith(lib, rootPath));
                            writer.WriteEndElement();//Lib
                        }
                        writer.WriteEndElement();//LibPaths
                        writer.WriteStartElement("LibNames");

                        foreach (string libName in ProjectData.LibNames)
                        {
                            writer.WriteStartElement("LibName");
                            writer.WriteAttributeString("Path", libName);
                            writer.WriteEndElement();//LibName
                        }
                        writer.WriteEndElement();//LibNames
                        writer.WriteStartElement("Defines");

                        foreach (string macroName in ProjectData.Defines)
                        {
                            writer.WriteStartElement("Define");
                            writer.WriteAttributeString("value", macroName);
                            writer.WriteEndElement();//LibName
                        }
                        writer.WriteEndElement();//Defines
                        writer.WriteStartElement("Bin");
                        writer.WriteAttributeString("Path", "./");
                        writer.WriteEndElement();//Bin
                        writer.WriteEndElement();//Project
                        writer.WriteEndDocument();
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
            catch
            {

            }
        }
        #endregion Methods
        #region Event Handlers
        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Gunit_Load(object sender, EventArgs e)
        {
           
            
            m_IncludeDataTable = new DataTable();
            m_IncludeDataTable.Columns.Add("IncludePaths");
            m_librarypathTable = new DataTable();
            m_librarypathTable.Columns.Add("LibraryPaths");
            m_LibNameDataTable = new DataTable();
            m_LibNameDataTable.Columns.Add("LibraryNames");
            m_MacroDataTable = new DataTable();
            m_MacroDataTable.Columns.Add("MacroDefines");
            m_frmEditor = new Editor(this);
            m_frmEditor.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            m_frmEditor.CloseButtonVisible = false;
            m_frmPlugins = new Plugins(this);
            m_frmPlugins.CloseButtonVisible = false;
            m_frmPlugins.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            m_frmPrj = new ProjectSetting(this);
            m_frmPrj.CloseButtonVisible = false;
            m_frmPrj.Show(dockParent, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
            splash.ShowDialog();
            readPluginConfig();
            splash.Close();
            
           
         
        }
        
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Host_CreateProjectSession();
            
        }
       

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != ProjectData)
            {
                m_frmEditor.Save();
                Host_SaveProjectSession(ProjectData.ProjectPath);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Host_OpenProjectSession();
        }
        #endregion Event Handlers

        private void dockParent_ActiveContentChanged(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Gunit IDE is useful for developing Unit tests using Google test framework for C code\n for more information please refer to the document "+Directory.GetCurrentDirectory()+"\\GUnit.docx\n");
        }



        public List<ICppCodeDescription> CPPCodeDescriptions
        {
            get
            {
                return m_CppcodeDescriptions;
            }
            set
            {
                m_CppcodeDescriptions = value;
            }
        }
    }
}
