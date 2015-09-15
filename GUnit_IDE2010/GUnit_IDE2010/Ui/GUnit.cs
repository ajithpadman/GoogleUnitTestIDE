using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Gunit.DataModel;
using Gunit.Controller;
using System.IO;
using System.Diagnostics;
using GUnit_IDE2010.Ui;
using GUnit_IDE2010.DataModel;
using GUnit_IDE2010.Controller;
using GUnit_IDE2010.GunitParser;
using GUnit_IDE2010.CodeGenerator;
using System.Xml.Xsl;
using System.Threading;
using GUnit_IDE2010.JobHandler;

namespace Gunit.Ui
{
    public partial class GUnit : Form
    {
       
        delegate void SetStatusCallback(String text);
        ProjectUi m_frmProjectUi;
        ProjectDataModel m_ProjectModel;
        ProjectUiController m_ProjectController;

        DocumentMgrDataModel m_DocumentMgrDataModel;
        DocumentManagerController m_DcumentMgrController;
        DocumentManagerUiAdapter m_DocumentMgrUi;

        ConsoleController m_consoleController;
        ConsoleDataModel m_consoleDataModel;
        ConsoleUi m_ConsoleUi;

        OutlineUiController m_OutlineUiController;
        OutlineDataModel m_OutlineDataModel;
        OutlineUi m_OutlineUi;

        

        ProjectBuilder m_builder;
        RunOutputJobHandler m_RunOutput;
        RunCoverageJobHandler m_CoverageAnalyser;
        ParserManager m_parserMgr;
        ParserStatus m_parserStatus = ParserStatus.ParserComplete;
      
        
        /// <summary>
        /// 
        /// </summary>
        private ParserStatus ParserStatus
        {
            get { return m_parserStatus; }
            set { 
                   m_parserStatus = value;
                   if (value == DataModel.ParserStatus.ParserComplete)
                   {
                       m_ProjectModel.Session = Sessions.PARSER_COMPLETE;
                       m_DocumentMgrDataModel.Session = Sessions.PARSER_COMPLETE;
                       m_consoleDataModel.Session = Sessions.PARSER_COMPLETE;
                       m_OutlineDataModel.Session = Sessions.PARSER_COMPLETE;
                      
                   }
                   else if (value == DataModel.ParserStatus.ParserRunning)
                   {
                       m_ProjectModel.Session = Sessions.PARSER_RUNNING;
                       m_DocumentMgrDataModel.Session = Sessions.PARSER_RUNNING;
                       m_consoleDataModel.Session = Sessions.PARSER_RUNNING;
                       m_OutlineDataModel.Session = Sessions.PARSER_RUNNING;
                      
                   }
                }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public GUnit()
        {
            InitializeComponent();
           
            
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GUnit_Load(object sender, EventArgs e)
        {
            GUnit_setUpProjectUi();
           
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            

            if (disposing && (components != null))
            {
                try
                {
                    if (null != m_parserMgr)
                    {
                        m_parserMgr.Dispose();
                        m_parserMgr = null;
                    }
                    if (null != m_builder)
                    {
                        m_builder.Dispose();
                        m_builder = null;
                    }
                    if (null != m_RunOutput)
                    {
                        m_RunOutput.Dispose();
                        m_RunOutput = null;
                    }
                    if (null != m_ProjectModel)
                    {
                        m_ProjectModel.Dispose();
                        m_ProjectModel = null;
                    }
                }
                catch
                {

                }
                components.Dispose();
            }
            else
            {
               

            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        public void GUnit_displayFunctions(string FileName)
        {
           
           
        }
        /// <summary>
        /// Create all Child Form Objects
        /// </summary>
        private void GUnit_setUpProjectUi() 
        {
            
            //Start the ProjectExplorer View
            m_ProjectModel = new ProjectDataModel();
            m_frmProjectUi = new ProjectUi(m_ProjectModel);
            m_frmProjectUi.evFileAdded += ProjectUi_evFileAdded;
            m_frmProjectUi.evFileOpened += FileOpened;
            m_frmProjectUi.evFileRemoved += ProjectUi_evFileRemoved;
            m_frmProjectUi.evOpenprojectComplete += ProjectUi_evOpenprojectComplete;
            m_frmProjectUi.evMacroUpdate += ProjectUi_evMacroUpdate;
            m_ProjectController = new ProjectUiController(m_frmProjectUi, m_ProjectModel);
            m_ProjectController.StartView(GunitWorkSpace, DockState.DockLeft);

            //Start the document Manager
            m_DocumentMgrDataModel = new DocumentMgrDataModel();
            m_DocumentMgrUi = new DocumentManagerUiAdapter(m_DocumentMgrDataModel);
            m_DocumentMgrUi.evFileFocused += DocumentMgr_evFileFocused;
            m_DcumentMgrController = new DocumentManagerController(m_DocumentMgrUi, m_DocumentMgrDataModel);
            m_DcumentMgrController.StartView(GunitWorkSpace, DockState.Document, true);


            m_OutlineDataModel = new OutlineDataModel(m_ProjectModel);
            m_OutlineUi = new OutlineUi(m_OutlineDataModel);
            m_OutlineUi.evCalledLocation += FunctionUi_evCalledLocation;
            m_OutlineUi.evRefresh +=FunctionUi_evRefresh;
            m_OutlineUiController = new OutlineUiController(m_OutlineUi, m_OutlineDataModel);
            m_OutlineUiController.StartView(GunitWorkSpace, DockState.DockRight);

            //Start the Consols
            m_consoleDataModel = new ConsoleDataModel();
            m_ConsoleUi = new ConsoleUi(m_consoleDataModel);
            m_consoleController = new ConsoleController(m_ConsoleUi, m_consoleDataModel);
            m_consoleController.StartView(GunitWorkSpace, DockState.DockBottom);

        }

        private void ProjectUi_evMacroUpdate(ListofStrings macros)
        {
            m_DocumentMgrUi.DocumentMgr_macroUpdate(macros);
        }

        /// <summary>
        /// When a file get focused in the document Pane
        /// </summary>
        /// <param name="filename"></param>
        private void DocumentMgr_evFileFocused(string filename)
        {
            GUnit_displayFunctions(filename);
        }
        /// <summary>
        /// When the refresh button in the Project Outline is clicked
        /// </summary>
        private void FunctionUi_evRefresh()
        {
            ParseFiles();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        private void FunctionUi_evCalledLocation(CodeLocation location)
        {
            m_DocumentMgrDataModel.FocusLocation = location;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        private void ProjectUi_evFileAdded(string file)
        {
            m_DocumentMgrDataModel.CurrentFile = file;
            m_DocumentMgrDataModel.Session = Sessions.ADD_FILE;
            m_OutlineDataModel.CurrentFile = file;
            m_OutlineDataModel.Session = Sessions.ADD_FILE;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        private void ProjectUi_evOpenprojectComplete(string file)
        {
            GUnitDB DbCtx = new GUnitDB(m_ProjectModel.DBPath);
            try
            {
                m_DocumentMgrDataModel.Session = Sessions.OPEN_PROJECT_COMPLETE;
                m_OutlineDataModel.Session = Sessions.OPEN_PROJECT_COMPLETE;
                m_consoleDataModel.Session = Sessions.OPEN_PROJECT_COMPLETE;
              
                DBManager dbmgr = new DBManager();
             
               
                if (DbCtx == null)
                {
                    MessageBox.Show("DatabaseConnection Cannot be established");
                    Application.Exit();
                }
                DbCtx.TruncateAllTable();

               



                string outputxmlPath = Path.GetDirectoryName(m_ProjectModel.ProjectPath) + "\\" + m_ProjectModel.ProjectName + "_TestReport";
                if(Directory.Exists(outputxmlPath) == false)
                {
                    Directory.CreateDirectory(outputxmlPath);
                }
                m_RunOutput = new RunOutputJobHandler(outputxmlPath+"\\Output.xml", m_consoleDataModel);
                m_RunOutput.evMaxJobs += setProgressMax;
                m_RunOutput.evProgress += setProgressVal;
                
                m_RunOutput.evJobStatus += TestRun_evJobStatus;
                m_RunOutput.Start("GunitRunOutput", ThreadPriority.Normal);



                
                m_parserMgr = new ParserManager(m_ProjectModel.DBPath, m_OutlineDataModel);
                m_parserMgr.Start("GlobalParser", ThreadPriority.Lowest);
                m_parserMgr.evParseComplete += Parser_evParseComplete;

               

            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        private void Parser_evParseComplete(Job job)
        {
            ParserStatus = ParserStatus.ParserComplete;
            enableParsermenuItems(true);

        }
        /// <summary>
        /// enable the menu items
        /// </summary>
        /// <param name="status"></param>
        private void enableParsermenuItems(bool status )
        {
            if (menuStrip.InvokeRequired)
            {
                menuStrip.Invoke((MethodInvoker)delegate
                {
                    generateMockToolStripMenuItem.Enabled = status;
                    generateBoundaryTestToolStripMenuItem.Enabled = status;
                });
            }
            else
            {
                generateMockToolStripMenuItem.Enabled = status;
                generateBoundaryTestToolStripMenuItem.Enabled = status;
            }
        }
        /// <summary>
        /// event to file remove
        /// </summary>
        /// <param name="file"></param>
        private void ProjectUi_evFileRemoved(string file)
        {
            GUnitDB dbCtx = new GUnitDB(m_ProjectModel.DBPath);
           m_DocumentMgrDataModel.CurrentFile = file;
           m_DocumentMgrDataModel.Session = Sessions.REMOVE_FILE;
           m_OutlineDataModel.CurrentFile = file;
           m_OutlineDataModel.Session = Sessions.REMOVE_FILE;
          
            var files =
                        from fileCurrent in dbCtx.ProjectFiles
                        where fileCurrent.FilePath == file
                        select fileCurrent;
            
            foreach (ProjectFiles removedFile in files)
            {
                dbCtx.ProjectFiles.DeleteOnSubmit(removedFile);
            }
            dbCtx.SubmitChanges();
        }

        /// <summary>
        /// Handle the File Opened from other windows
        /// </summary>
        /// <param name="filePath">absolute path to the File</param>
        private void FileOpened(string filePath)
        {
            m_DocumentMgrDataModel.CurrentFile = filePath;
            m_DocumentMgrDataModel.Session = Sessions.OPEN_FILE;
            m_OutlineDataModel.CurrentFile = filePath;
            m_OutlineDataModel.Session = Sessions.OPEN_FILE;
           
            GUnit_displayFunctions(filePath);
            
        }
        /// <summary>
        /// parse the files
        /// </summary>
        public void ParseFiles()
        {
            ListofFiles fileList = m_frmProjectUi.ProjectUi_getParseRequest();
            ListofStrings cmdLines = m_ProjectModel.ProjectDataModel_GetClangCommandLine();
            if (fileList.Count() > 0)
            {
                enableParsermenuItems(false);
                ParserStatus = ParserStatus.ParserRunning;
                m_parserMgr.AddJobs(fileList, cmdLines);
               
            }
           
           
        }
        

        
       
       
      
      
        
       

      
        /// <summary>
        /// Handle New Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            m_ProjectModel.Session= Sessions.NEW_PROJECT;
            m_DocumentMgrDataModel.Session = Sessions.NEW_PROJECT;
            m_OutlineDataModel.Session = Sessions.NEW_PROJECT;
        }
        /// <summary>
        /// Handle Open Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            
            m_ProjectModel.Session = Sessions.OPEN_PROJECT;
            m_DocumentMgrDataModel.Session = Sessions.OPEN_PROJECT;
            m_OutlineDataModel.Session = Sessions.OPEN_PROJECT;
           

        }
        /// <summary>
        /// Handle Save Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveProject();
        }
        private void saveProject()
        {
            m_ProjectModel.Session = Sessions.SAVE_PROJECT;
            m_DocumentMgrDataModel.Session = Sessions.SAVE_PROJECT;
            m_OutlineDataModel.Session = Sessions.SAVE_PROJECT;
            
        }
        /// <summary>
        /// Handle New Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_ProjectModel.Session = Sessions.NEW_PROJECT;
            m_DocumentMgrDataModel.Session = Sessions.NEW_PROJECT;
            m_OutlineDataModel.Session = Sessions.NEW_PROJECT;

        }
        /// <summary>
        /// Handle Open Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_ProjectModel.Session = Sessions.OPEN_PROJECT;
            m_DocumentMgrDataModel.Session = Sessions.OPEN_PROJECT;
            m_OutlineDataModel.Session = Sessions.OPEN_PROJECT;
           

        }
        /// <summary>
        /// Hanlde the Save Button Press From Tool Strip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveProject();

        }
        /// <summary>
        /// Handle File Drop event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GUnit_FileDroped(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                FileOpened(file);
            }
        }
        /// <summary>
        /// Handle File Drag enter Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GUnit_FileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void GUnit_Closing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void GUnit_Closed(object sender, FormClosedEventArgs e)
        {
           
        }
       
        
        
       
       
        private void setStatusLabel(string status)
        {
            if (statusStrip.InvokeRequired)
            {
                
                    try
                    {
                        SetStatusCallback d = new SetStatusCallback(setStatusLabel);
                        this.Invoke(d, new object[] { status });
                    }
                    catch 
                    {

                    }
                

            }
            else
            {
                toolStripStatus.Spring = true;
                toolStripStatus.TextAlign = ContentAlignment.MiddleLeft;

                toolStripStatus.Text = status;
                toolStripStatus.Invalidate();
                statusStrip.Refresh();
            }
        }
        private void setProgressMax(int max)
        {
            if (progress.InvokeRequired)
            {
                progress.Invoke((MethodInvoker)delegate
                {
                    progress.Maximum = max;
                });

            }
            else
            {
                progress.Maximum = max;
            }
        }
        private void setProgressVal(int val)
        {
            if (progress.InvokeRequired)
            {
                progress.Invoke((MethodInvoker)delegate
                {
                    progress.Value = val;
                });

            }
            else
            {
                progress.Value = val;
            }
        }
        private void IncrementProgressVal()
        {
            lock (ConsoleDataModel._lock)
            {
                if (progress.InvokeRequired)
                {
                    progress.Invoke((MethodInvoker)delegate
                    {
                        progress.Value++;
                    });

                }
                else
                {
                    progress.Value++;
                }
            }
        }
        private void BuildCompleteStatusUpdate()
        {
            if (m_consoleDataModel.Errors.Count() != 0)
            {
                setStatusLabel("Build Completed with errors");
            }
            else if (m_consoleDataModel.Warnings.Count() != 0)
            {
                setStatusLabel("Build Completed with Warnings");
            }
            else
            {
                setStatusLabel("Build Completed Successfully");
            }
        }
        /// <summary>
        /// BuildComplete Event handler
        /// </summary>
        private void BuildComplete()
       {
           
            if (menuStrip.InvokeRequired)
            {
                menuStrip.Invoke((MethodInvoker)delegate
                {
                    buildToolStripMenuItem.Enabled = true;
                    BuildCompleteStatusUpdate();
                });
            }
            else
            {
                buildToolStripMenuItem.Enabled = true;
                BuildCompleteStatusUpdate();
            }
            m_builder.Dispose();
            m_builder = null;
        }
      
        /// <summary>
        /// Build Menu Item Event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(m_builder != null)
            {
                m_builder.Dispose();
                m_builder = null;
            }
            m_ConsoleUi.ConsoleUi_Clear();
            setProgressVal(0);
            m_builder = new ProjectBuilder(m_ProjectModel, m_consoleDataModel);
            m_builder.evMaxJobs += setProgressMax;
            m_builder.evProgress += setProgressVal;
            m_builder.evAllJobsComplete += BuildComplete;
            m_builder.Start("GUnitBuilder", ThreadPriority.Normal);
            setStatusLabel("Build in Progress");
            m_builder.RunJobs(m_ProjectModel.BuildCommands);
            buildToolStripMenuItem.Enabled = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void generateMockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Object> selected =  m_OutlineUi.getSelectedNodes();
            List<GlobalMethods> GlobalMethods = new List<GlobalMethods>();
            string ModuleName = "";
            foreach (Object obj in selected)
            {
                if (obj is Classes)
                {
                    CodeGenDataModel model = new CodeGenDataModel();
                    MockGenerator mockGen = new MockGenerator(Path.GetDirectoryName(m_ProjectModel.ProjectPath) + "\\" + m_ProjectModel.ProjectName + "_Mocks", model);
                    mockGen.generateMockClass(obj as Classes);
                }
                if (obj is GlobalMethods)
                {
                    GlobalMethods m = obj as GlobalMethods;
                    ModuleName = Path.GetFileNameWithoutExtension(m.Methods.FilePath);
                    if (GlobalMethods.Contains(obj as GlobalMethods) == false)
                    {
                        GlobalMethods.Add(obj as GlobalMethods);
                    }
                }
            }
            if (GlobalMethods.Count() > 0)
            {
                UserInput name = new UserInput();
                name.Text = "Enter the ModuleName";
                if (DialogResult.OK == name.ShowDialog())
                {
                    ModuleName = name.Result;
                }
                CodeGenDataModel Globalmodel = new CodeGenDataModel();
                MockGenerator GlobalmockGen = new MockGenerator(Path.GetDirectoryName(m_ProjectModel.ProjectPath) + "\\" + m_ProjectModel.ProjectName + "_Mocks", Globalmodel);
                GlobalmockGen.generateMockClass(GlobalMethods, ModuleName);
            }
            MessageBox.Show("Mocks generated in " +  m_ProjectModel.ProjectName + "_Mocks folder");
        }

        private void tsBtnRun_Click(object sender, EventArgs e)
        {
            ListofStrings commands = m_ProjectModel.BuildCommands;
            if (File.Exists(m_ProjectModel.Executable))
            {
                if (m_ProjectModel.ApplicationType == OutputTypes.ConsoleApplication)
                {
                    ListofStrings outputs = new ListofStrings();
                    outputs.Add(m_ProjectModel.Executable);
                    m_RunOutput.RunJobs(outputs);
                    tsBtnRun.Enabled = false;
                    ChangeCoverageButtonStatus(false);
                }
            }
        }

       
        private void TestRun_evJobStatus(Job job)
        {
            if (toolStrip.InvokeRequired)
            {
                toolStrip.Invoke((MethodInvoker)delegate
                {
                    tsBtnRun.Enabled = true;

                });
            }
            else
            {
                tsBtnRun.Enabled = true;
            }
            if (job.Result is string)
            {
                string OutputFile = job.Result as string;
                GUnit_TransformXML(OutputFile);
            }
            ChangeCoverageButtonStatus(true);
        }
        private void ChangeCoverageButtonStatus(bool status)
        {
            if (toolStrip.InvokeRequired)
            {
                toolStrip.Invoke((MethodInvoker)delegate
                {
                    tsBtnGenerateCoverage.Enabled = status;

                });
            }
            else
            {
                tsBtnGenerateCoverage.Enabled = status;
            }
        }
      
        private void GUnit_TransformXML(string XML)
        {
            if (File.Exists(XML))
            {
                try
                {
                    string runDir = Path.GetDirectoryName(XML);
                    ExtractResource("GUnit_IDE2010.gtest-result.xsl", runDir + "\\x.xsl");
                    XslCompiledTransform myXslTransform;
                    myXslTransform = new XslCompiledTransform();
                    myXslTransform.Load(runDir + "\\x.xsl");
                    string reportLocation = runDir + "\\" + m_ProjectModel.ProjectName + "_TestReport"; 
                    if (Directory.Exists(reportLocation) == false)
                    {
                        Directory.CreateDirectory(reportLocation);
                    }

                    myXslTransform.Transform(XML, reportLocation + "\\" + m_ProjectModel .ProjectName+ "_TestReport.html");
                    m_ProjectModel.Testreport = reportLocation + "\\" + m_ProjectModel.ProjectName + "_TestReport.html";
                    showTestReport(m_ProjectModel.Testreport);
                }
                catch
                {

                }
            }
        }
        private void coverageHTML_Click(object sender, EventArgs e)
        {
            try
            {
                
               
                ChangeCoverageButtonStatus(false);
               
                GCovParserJobHandler coverageAnalyser = new GCovParserJobHandler(m_consoleDataModel, m_ProjectModel);
                coverageAnalyser.Start("GcovRunner", ThreadPriority.Normal);
                coverageAnalyser.evCoverageComplete += CoverageAnalyser_evCoverageComplete;
                coverageAnalyser.evProgress += setProgressVal;
                coverageAnalyser.evMaxJobs += setProgressMax;
                setStatusLabel("Running Coverage Analyser");
                setProgressVal(0);
                coverageAnalyser.RunJob();
            }
            catch
            {

            }



        }

       

        private void CoverageAnalyser_evCoverageComplete(GCovParserJobHandler jobHandler)
        {
            ShowCoverageReport(m_ProjectModel.Coveragereport + "\\CoverageReport.html");
            if(jobHandler != null)
            {
                jobHandler.Stop();
            }
            ChangeCoverageButtonStatus(true);
        }

        private void showTestReport(string path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    HTMlBrowserModel browsermodel = new HTMlBrowserModel();
                    browsermodel.URL = path;
                    HtmlBrowser browser = new HtmlBrowser(browsermodel);
                    browser.Text = m_ProjectModel.ProjectName + "_TestReport.html";
                    HTMLBrowserController controller = new HTMLBrowserController(browser, browsermodel);
                    controller.StartView(GunitWorkSpace, DockState.Document);

                });

            }
            else
            {
                HTMlBrowserModel browsermodel = new HTMlBrowserModel();
                browsermodel.URL = path;
                HtmlBrowser browser = new HtmlBrowser(browsermodel);
                browser.Text = m_ProjectModel.ProjectName + "_TestReport.html";
                HTMLBrowserController controller = new HTMLBrowserController(browser, browsermodel);
                controller.StartView(GunitWorkSpace, DockState.Document);
            }
        }
        private void ShowCoverageReport(string path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    HTMlBrowserModel browsermodel = new HTMlBrowserModel();
                    browsermodel.URL = path;
                    HtmlBrowser browser = new HtmlBrowser(browsermodel);
                    browser.Text = m_ProjectModel.ProjectName + "_CoverageReport.html";
                    HTMLBrowserController controller = new HTMLBrowserController(browser, browsermodel);
                    controller.StartView(GunitWorkSpace, DockState.Document);

                });
               
            }
            else
            {
                HTMlBrowserModel browsermodel = new HTMlBrowserModel();
                browsermodel.URL = path;
                HtmlBrowser browser = new HtmlBrowser(browsermodel);
                browser.Text = m_ProjectModel.ProjectName + "_CoverageReport.html";
                HTMLBrowserController controller = new HTMLBrowserController(browser, browsermodel);
                controller.StartView(GunitWorkSpace, DockState.Document);
            }
        }
        private void CoverageAnalysis_evJobStatus(Job job)
        {
            if(job.Result is string)
            {
                string path = job.Result as string;
                ShowCoverageReport(path);
                if(m_CoverageAnalyser!= null)
                {
                    m_CoverageAnalyser.Stop();
                    m_CoverageAnalyser = null;
                }

            }
            ChangeCoverageButtonStatus(true);
        }

        private void CoverageAnalysis_evMaxJobs(int max)
        {
            setProgressMax(max);
        }

        private void CoverageAnalysis_evProgress(int progress)
        {
            setProgressVal(progress);
        }

        private void CoverageAnalysis_evAllJobsComplete()
        {
            
        }

        public void ExtractResource(string resource, string path)
        {
            string Dir = Path.GetDirectoryName(path);
            if (Directory.Exists(Dir))
            {
                Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
                byte[] bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                File.WriteAllBytes(path, bytes);
            }

        }

      

        private void generateBoundaryTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                BoundaryTestGenerator b = new BoundaryTestGenerator(m_ProjectModel.DBPath);
                ListofFiles files = m_frmProjectUi.ProjectUi_getParseRequest();
                string DirectoryName = Path.GetDirectoryName(m_ProjectModel.BuildPath) + "\\" + "GeneratedTests_" + m_ProjectModel.ProjectName;
                if (Directory.Exists(DirectoryName) == false)
                {
                    Directory.CreateDirectory(DirectoryName);
                }
                foreach (string file in files)
                {
                    b.processFile(file, DirectoryName);
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());

            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void generateMakeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptPath = "";
            PremakeScriptBuilder scriptBuilder = new PremakeScriptBuilder(m_ProjectModel, PremakeSolutionType.Gmake, OutputTypes.ConsoleApplication);
            scriptPath = scriptBuilder.createPremakeScript();
            
            if (File.Exists(scriptPath))
             {
                RunSolutionBuilder solnBldr = new RunSolutionBuilder(m_consoleDataModel, scriptPath, PremakeSolutionType.Gmake);
                solnBldr.evJobStatus += SolnBldr_evJobStatus;
                solnBldr.Start("SolutionBuilder", ThreadPriority.Normal);

                solnBldr.RunJob();
             }

        }

        private void SolnBldr_evJobStatus(Job job)
        {
            if(job.Result is RunSolutionBuilder)
            {
                RunSolutionBuilder solnBldr = job.Result as RunSolutionBuilder;
                solnBldr.Stop();
            }
        }

        private void generateVisualStudioSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string scriptPath = "";
            PremakeScriptBuilder scriptBuilder = new PremakeScriptBuilder(m_ProjectModel, PremakeSolutionType.VS2010, OutputTypes.ConsoleApplication);
            scriptPath = scriptBuilder.createPremakeScript();

            if (File.Exists(scriptPath))
            {
                RunSolutionBuilder solnBldr = new RunSolutionBuilder(m_consoleDataModel, scriptPath, PremakeSolutionType.VS2010);
                solnBldr.evJobStatus += SolnBldr_evJobStatus;
                solnBldr.Start("SolutionBuilder", ThreadPriority.Normal);

                solnBldr.RunJob();
            }
        }

        private void toolStripMenuParser_Click(object sender, EventArgs e)
        {
            ParseFiles();
        }
    }
}
