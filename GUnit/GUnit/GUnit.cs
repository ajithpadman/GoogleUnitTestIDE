using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using System.Xml.Xsl; 
namespace GUnit
{
    public partial class GUnit : Form
    {

        #region MEMBER VARIABLES
        SaveFileDialog m_SaveFileDlg;
        string m_lastChangedFile = "";
        OpenFileDialog m_OpenprojectDlg;
        Semaphore m_mutex = new Semaphore(0, 1);

        #region EventDeclairation
        public delegate void onStatusInfoUpdate(string fileinfo);
        public event onStatusInfoUpdate evNewStatus = delegate { };

        public delegate void onFileRemovedFromOutside(string fileinfo);
        public event onFileRemovedFromOutside evFileRemovedfromOutside = delegate { };

        public delegate void onFileOpenComplete(string fileinfo);
        public event onFileOpenComplete evfileOpenComplete = delegate { };

        public delegate void onNewProject(ProjectInfo project);
        public event onNewProject evNewProject = delegate { };
        public delegate void onOpenProject(ProjectInfo project);
        public event onOpenProject evOpenProject = delegate { };

        public delegate void onSaveProject(ProjectInfo project);
        public event onSaveProject evSaveProject  = delegate { };

        public delegate void onCalledFunctionList(FunctionalInterface function);
        public event onCalledFunctionList evCalledFunctionListAvailable = delegate { };

        public delegate void onCloseAllForms();
        public event onCloseAllForms evCloseAllForms = delegate { };

        public delegate void onUpdateNodeRequired();
        public event onUpdateNodeRequired evUpdateFunctionIf = delegate { };
        public delegate void onProcessComplete();
        public event onProcessComplete evProcessComplete = delegate { };

        public delegate void onFileReceived(string FilePath);
        public event onFileReceived evFileReceived = delegate { };

        public delegate void onFileInfoAvailable(FileInfo FileInfo);
        public event onFileInfoAvailable evSendFileInfo = delegate { };

        public delegate void onFileDeleted(string FilePath);
        public event onFileDeleted evRemoveFileInfo = delegate { };

        public delegate void onFileFocusChange(FileInfo file);
        public event onFileFocusChange evFileFocusChange = delegate { };

        /*Event to Close All Documents*/
        public delegate void onDocumentClose(FileInfo data);
        public event onDocumentClose evCloseDocument = delegate { };

        /*Event to update Syntax Highlighting on Scitilla Text Box*/
        public delegate void onSyntaxHighlightUpdate();
        public event onSyntaxHighlightUpdate evTextHighlightUpdate = delegate { };

        /*Event to update Console Text Box*/
        public delegate void onConsoleUpdate(string Text);
        public event onConsoleUpdate evConsoleUpdate = delegate { };

        public delegate void onConsoleTextColorUpdate(string newValue, Color color);
        public event onConsoleTextColorUpdate evConsoleTextColorUpdate = delegate { };
        /*Event to update Function Tree View */
        public delegate void onFunctionUpdate(FileInfo file);
        public event onFunctionUpdate evFunctionListUpdate = delegate { };


        #endregion

        string[] variable_qualifiers = { "signed", "unsigned", "long", "short", "long long", "const", "volatile", "static", "auto", "extern", "register" };
        string[] StandardTypes = { "char", "short", "int",  "short int","long","long int", "long long","float","double","long double"};
        string [] utestMacros = {"TEST","TEST_F","TEST_P","EXPECT_EQ","ASSERT_TRUE","ASSERT_FALSE","EXPECT_TRUE","EXPECT_FALSE",
                                 "ASSERT_EQ","ASSERT_NE","ASSERT_LT","ASSERT_LE","ASSERT_GT","ASSERT_GE",
                                "EXPECT_NE","EXPECT_LT","EXPECT_LE","EXPECT_GT","EXPECT_GE",
                                "ASSERT_STREQ","ASSERT_STRNE","ASSERT_STRCASEEQ","ASSERT_STRCASENE","EXPECT_STREQ",
                                "EXPECT_STRNE","EXPECT_STRCASEEQ","EXPECT_STRCASENE",
                                "EXPECT_CALL","Times","AnyNumber","WillOnce","Return","RetiresOnSaturation","WillRepeatedly",
                                "Ge"};
        public GUnitData m_data;
        #endregion MEMBER VARIABLES

        #region CHILD FORMS Declairation
        ActiveFunctions frmActiveFunc;
        Project frmProject ;
        Console frmConsole;
        DataIf frmDataIf ;
        FunctionIF frmFunctIf;
        SolutionBuilder frmSolution;
        Loading frmLoading;
        ExtFunctionIf frmExtFuncIf;
        #endregion

       
        public GUnit()
        {
            InitializeComponent();
            m_mutex.Release();
        }

        #region GUI_INTERACTION_FUNCTIONS

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }
        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }
        private void GUnit_Load(object sender, EventArgs e)
        {
            GUnit_startForm();
            enableCoverageAnalysis(false);

        }
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        private void comboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboList.SelectedIndex == 0)
            {
                m_data.ProjectType = GUnitProjectType.C_PROJECT;

            }
            else
            {
                m_data.ProjectType = GUnitProjectType.CPP_PROJECT;
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUnit_WriteProjectData(m_data.m_Project.m_ProjectPath);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            GUnit_WriteProjectData(m_data.m_Project.m_ProjectPath);
        }

        private void cUnitTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            evCloseAllForms();
            GUnit_startForm();
            GUnit_SaveFile();
            
        }
     
        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            evCloseAllForms();
            GUnit_startForm();
            m_OpenprojectDlg = new OpenFileDialog();
            m_OpenprojectDlg.Filter = "GUnitProject files (*.xml)|*.xml";
            m_OpenprojectDlg.ShowDialog();
            GUnit_openProject();

        }
        private void toolStripBuild_Click(object sender, EventArgs e)
        {
            GUnit_build();
        }

        private void generateStubsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUnit_CreateMockUnit();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            GUnit_Run();

        }
        private void coverageAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUnit_AnalyseCoverage();
        }

        private void Coverage_Click(object sender, EventArgs e)
        {
            GUnit_AnalyseCoverage();
        }
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUnit_Run();
        }
        /*********************************************************************/
        /*! \fn private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUnit_build();

        }
        #endregion GUI_INTERACTION_FUNCTIONS
        /*********************************************************************/
        /*! \fn public void GUnit_updateConsole(string text)
        * \brief Update the text on the Console
        * \return void
        */
        /*********************************************************************/
        public void GUnit_updateConsole(string text)
        {
            evConsoleUpdate(text);
        }
        /*********************************************************************/
        /*! \fn public void GUnit_FunctionDisplayed(FileInfo fileData)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_FunctionDisplayed(FileInfo fileData)
        {
            
            FileInfo l_data = m_data.GUnitData_getFileInformation(fileData.m_fileName);
            GUnit_readFileThread(fileData);
            m_data.GUnitData_UpdateProjectTable(fileData.m_fileName, fileData);
            evUpdateFunctionIf();
        }
        /*********************************************************************/
        /*! \fn private void GUnit_startForm()
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_startForm()
        {
           
            m_data = new GUnitData(GUnitProjectType.C_PROJECT);
            this.WindowState = FormWindowState.Maximized;
            frmActiveFunc = new ActiveFunctions(this);
            frmActiveFunc.CloseButtonVisible = false;
            frmActiveFunc.evFunctionListDisplayed += new ActiveFunctions.onFunctionDisplayed(GUnit_FunctionDisplayed);
            frmActiveFunc.Show(dockPanel1, DockState.DockLeft);
            frmActiveFunc.evScrollDocument += new ActiveFunctions.onFileScrollToLine(GUnit_ScrollDocument);

            frmFunctIf = new FunctionIF(this);
            frmFunctIf.CloseButtonVisible = false;
            frmFunctIf.Show(dockPanel1, DockState.DockRight);
            frmFunctIf.evFileParsed += new FunctionIF.onFileParsed(GUnit_FunctionalIfAvailable);


            frmProject = new Project(this);
            frmProject.CloseButtonVisible = false;
            frmProject.evSourceFileAdded += new Project.onSourceFileAdded(GUnit_SourceFileAdded);
            frmProject.evHeaderFileAdded += new Project.onheaderFileAdded(GUnit_HeaderFileAdded);
            frmProject.evFileOpened += new Project.onFileOpened(GUnit_openDocument);
            frmProject.evFileRemoved += new Project.onFileRemoved(GUnit_FileRemoved);
            frmProject.evCoverageRequest += GUnit_DisplayCoverage;
            frmProject.Show(dockPanel1, DockState.DockLeft);


            frmConsole = new Console(this);
            frmConsole.CloseButtonVisible = false;
            frmConsole.Show(dockPanel1, DockState.DockBottom);

            frmDataIf = new DataIf(this);
            frmDataIf.CloseButtonVisible = false;
            frmDataIf.Show(dockPanel1, DockState.DockRight);

            frmExtFuncIf = new ExtFunctionIf(this);
            frmExtFuncIf.CloseButtonVisible = false;
            frmExtFuncIf.Show(dockPanel1, DockState.DockRight);
            
            comboList.SelectedIndex = 0;
            m_data.m_standardTypes.AddRange(StandardTypes);
            m_data.m_standardQualifiers.AddRange(variable_qualifiers);
            m_data.m_standardUtestMacros.AddRange(utestMacros);
            evFunctionListUpdate(null);
            
        }
        /*********************************************************************/
        /*! \fn  public void GUnit_FileRemoved(FileInfo file)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_FileRemoved(FileInfo file)
        {
            m_data.GUnitDat_removeEntry(file.m_fileName);
            evCloseDocument(file);
            evFileFocusChange(null);
            evRemoveFileInfo(file.m_fileName);
        }
        /*********************************************************************/
        /*! \fn  public void GUnit_ScrollDocument(FunctionalInterface function)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_ScrollDocument(FunctionalInterface function)
        {
            FileInfo data = m_data.GUnitData_getFileInformation(function.m_FileName);
            GUnit_openDocument(data);
            data.m_Document.FileOpen_DisplayFunction(function.m_LineNo);
            GUnit_RunCScope(function);
          
        }
        /*********************************************************************/
        /*! \fn  private void GUnit_RunCScope(FunctionalInterface function)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_RunCScope(FunctionalInterface function)
        {
            BackgroundWorker cscopeParser = new BackgroundWorker();
            cscopeParser.DoWork += new DoWorkEventHandler(GUnit_ParseFunction);
            cscopeParser.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GUnit_ParseFunctionComplete);
            cscopeParser.RunWorkerAsync(function);
        }
        /*********************************************************************/
        /*! \fn  private void GUnit_ParseFunction(object sender, DoWorkEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_ParseFunction(object sender, DoWorkEventArgs e)
        {
            FunctionalInterface func = e.Argument as FunctionalInterface;
            CScopeParser cscope = new CScopeParser(this);
            cscope.Cscope_CreateDataBase();
            cscope.Cscope_getFunctionList(func);
            e.Result = func;
        }
        /*********************************************************************/
        /*! \fn private void GUnit_ParseFunctionComplete(object sender, RunWorkerCompletedEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_ParseFunctionComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            FunctionalInterface function = e.Result as FunctionalInterface;
            evCalledFunctionListAvailable(function);
        }
        /*********************************************************************/
        /*! \fn public void GUnit_openDocument(FileInfo data)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_openDocument(FileInfo data)
        {
            m_data.CurrentFileName = data.m_fileName;
            if (data != null)
            {
                if (data.m_Document != null)
                {
                    if (data.m_Document.IsDisposed != true)
                    {
                        data.m_Document.Show(dockPanel1, DockState.Document);

                    }
                    else
                    {
                        data.m_Document = new FileOpen(this, data.m_fileName);
                        data.m_Document.Show(dockPanel1, DockState.Document);
                        data.m_Document.updateSourceText(data.m_text);
                        m_data.GUnitData_UpdateProjectTable(m_data.CurrentFileName, data);
                    }
                }
                else
                {
                    data.m_Document = new FileOpen(this, data.m_fileName);
                    data.m_Document.Show(dockPanel1, DockState.Document);
                    data.m_Document.updateSourceText(data.m_text);
                    m_data.GUnitData_UpdateProjectTable(m_data.CurrentFileName, data);
                }

            }
        }
        /*********************************************************************/
        /*! \fn public void GUnit_FunctionalIfAvailable(FileInfo fileData)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_FunctionalIfAvailable(FileInfo fileData)
        {
            evFunctionListUpdate(null);
            evSendFileInfo(fileData);
            evFunctionListUpdate(fileData);
            fileData.m_lastWriteTime = File.GetLastWriteTime(fileData.m_fileName).ToString();
            fileData.m_fileWatcher.Path = Path.GetDirectoryName(fileData.m_fileName);
            fileData.m_fileWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite;
            fileData.m_fileWatcher.Changed += new FileSystemEventHandler(GUnit_fileChanged);
            fileData.m_fileWatcher.Deleted += new FileSystemEventHandler(GUnit_fileDeleted);
            fileData.m_fileWatcher.Renamed += new RenamedEventHandler(GUnit_fileRenamed);
            fileData.m_fileWatcher.IncludeSubdirectories = false;
           // fileData.m_fileWatcher.SynchronizingObject = this;
            fileData.m_fileWatcher.EnableRaisingEvents = true;
        }
        private void GUnit_processFileChange(FileSystemEventArgs e, FileInfo fileData)
        {
            if (fileData.m_IsDirty == true)
            {

                evFileReceived(e.FullPath);
                Application.DoEvents();
                fileData.m_IsDirty = false;
                m_data.GUnitData_UpdateProjectTable(e.FullPath, fileData);
            }
            else
            {
                DialogResult result = MessageBox.Show(Path.GetFileName(fileData.m_fileName) + " Changed Outside the Workspace Do You wish to Reload the file?", "File Change ", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    frmLoading = new Loading(this);
                    frmLoading.Loading_setProgressMax(1);
                    frmLoading.Show();
                    evCloseDocument(fileData);
                    evFileReceived(e.FullPath);
                    Application.DoEvents();
                    fileData.m_IsDirty = false;
                    m_data.GUnitData_UpdateProjectTable(e.FullPath, fileData);
                }
            }
        }
        private void GUnit_fileChanged(object sender, FileSystemEventArgs e)
        {
            m_mutex.WaitOne();
            FileInfo fileData = m_data.GUnitData_getFileInformation(e.FullPath);
            string lastwriteTime = File.GetLastWriteTime(e.FullPath).ToString();
            if (fileData != null && fileData.m_lastWriteTime != lastwriteTime)
            {
                fileData.m_lastWriteTime = lastwriteTime;
                m_data.GUnitData_UpdateProjectTable(e.FullPath, fileData);
                    if (this.InvokeRequired)
                    {
                        try
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                GUnit_processFileChange(e, fileData);
                                //if (fileData.m_IsDirty == true)
                                //{
                                    
                                //    evFileReceived(e.FullPath);
                                //    Application.DoEvents();
                                //    fileData.m_IsDirty = false;
                                //    m_data.GUnitData_UpdateProjectTable(e.FullPath, fileData);
                                //}
                                //else
                                //{
                                //    DialogResult result = MessageBox.Show(Path.GetFileName(fileData.m_fileName) + " Changed Outside the Workspace Do You wish to Reload the file?", "File Change ", MessageBoxButtons.YesNo);
                                //    if (result == DialogResult.Yes)
                                //    {
                                //        frmLoading = new Loading(this);
                                //        frmLoading.Loading_setProgressMax(1);
                                //        frmLoading.Show();
                                //        evCloseDocument(fileData);
                                //        evFileReceived(e.FullPath);
                                //        Application.DoEvents();
                                //        fileData.m_IsDirty = false;
                                //        m_data.GUnitData_UpdateProjectTable(e.FullPath, fileData);
                                //    }
                                //}
                            });
                        }
                        catch
                        {

                        }
                        
                        
                      
                    }
                    else
                    {
                        try
                        {
                            GUnit_processFileChange(e, fileData);
                        }
                        catch (Exception err)
                        {

                        }

                    }
                    m_lastChangedFile = e.FullPath;
                    m_data.GUnitData_UpdateProjectTable(e.FullPath, fileData);
                   
            }
           
            m_mutex.Release();
        }
        private void GUnit_fileDeleted(object sender, FileSystemEventArgs e)
        {
            m_mutex.WaitOne();
            FileInfo fileData = m_data.GUnitData_getFileInformation(e.FullPath);
            if (fileData != null)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                            {
                                DialogResult result = MessageBox.Show(Path.GetFileName(fileData.m_fileName) + " Deleted from the disk ", "File Deleted ", MessageBoxButtons.OK);
                                evFileRemovedfromOutside(e.FullPath);
                               
                            });
                }
            }
            m_mutex.Release();
           
            
        }
        private void GUnit_fileRenamed(object source, RenamedEventArgs e)
        {
            evConsoleUpdate("File: " + e.OldFullPath + " renamed to " + e.FullPath);
            
        }
        /*********************************************************************/
        /*! \fn public void GUnit_UpdateDocumentFocusChange(FileInfo fileData)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_UpdateDocumentFocusChange(FileInfo fileData)
        {
            evFileFocusChange(null);
            m_data.CurrentFileName = fileData.m_fileName;
            evFileFocusChange(fileData);
        }
        public void GUnit_UpdateStatus(string str)
        {
            evNewStatus(str);
        }
        private void GUnit_HeaderFileAdded(string filePath)
        {
            m_data.GUnitDat_AddHeaderFiles(filePath);
            GUnit_FileAddedToProject(filePath);
        }
        private void GUnit_SourceFileAdded(string filePath)
        {
            m_data.GUnitDat_AddSourceFiles(filePath);
            GUnit_FileAddedToProject(filePath);
        }
        /*********************************************************************/
        /*! \fn public void GUnit_FileAddedToProject(string filePath)
        * \brief 
        * \return void
        */
        /*********************************************************************/

        public void GUnit_FileAddedToProject(string filePath)
        {

            evNewStatus("Processing file " + Path.GetFileName(filePath));
            if (File.Exists(filePath))
            {
                evFunctionListUpdate(null);
                m_data.CurrentFileName = filePath;
                evFileReceived(filePath);
            }
        }
        /*********************************************************************/
        /*! \fn public void GUnit_readFileThread(FileInfo file)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_readFileThread(FileInfo file)
        {
            BackgroundWorker reader = new BackgroundWorker();
            reader.DoWork += new DoWorkEventHandler(GUnit_ReadFile);
            reader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GUnit_ReadFileComplete);
            reader.RunWorkerAsync(file);
        }
        /*********************************************************************/
        /*! \fn public void GUnit_ReadFile(object sender, DoWorkEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_ReadFile(object sender, DoWorkEventArgs e)
        {
            string line = "";
            
            FileInfo file = e.Argument as FileInfo;
            
            string filePath = file.m_fileName;
            if (File.Exists(filePath))
            {
                StreamReader reader = new StreamReader(filePath);
                while ((line = reader.ReadLine()) != null)
                {
                    file.m_text += line + "\n";
                }
                reader.Close();
            }
            e.Result = file;
        }
        /*********************************************************************/
        /*! \fn public void GUnit_endLoading()
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_endLoading()
        {
            evProcessComplete();
        }
        /*********************************************************************/
        /*! \fn public void GUnit_ReadFileComplete(object sender, RunWorkerCompletedEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_ReadFileComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            FileInfo fileInfo = e.Result as FileInfo;
            m_data.GUnitData_UpdateProjectTable(fileInfo.m_fileName, fileInfo);
            //evProcessComplete();
            Application.UseWaitCursor = false;
            evfileOpenComplete(fileInfo.m_fileName);

        }
        /*********************************************************************/
        /*! \fn public void GUnit_syntaxHighLighting()
        * \brief 
        * \return void
        */
        /*********************************************************************/
        public void GUnit_syntaxHighLighting()
        {
            evTextHighlightUpdate();
        }
        /*********************************************************************/
        /*! \fn private void GUnit_SaveFile()
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_SaveFile()
        {
             
             m_SaveFileDlg = new SaveFileDialog();
             m_SaveFileDlg.Filter = "GUnitProject files (*.xml)|*.xml";
             m_SaveFileDlg.FileOk += new CancelEventHandler(save_FileOk);
             m_SaveFileDlg.ShowDialog();
            
        }
        /*********************************************************************/
        /*! \fn private void GUnit_WriteProjectData(string ProjectName)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_WriteProjectData(string ProjectName)
        {
            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            settings.CloseOutput = true;
            
            frmLoading = new Loading(this);
            frmLoading.Loading_setProgressMax(7);
            frmLoading.Show();
            evNewStatus("Saving Project");
            using (XmlWriter writer = XmlWriter.Create(ProjectName, settings))
            {

                writer.WriteStartDocument();
                writer.WriteStartElement("Project");
                writer.WriteAttributeString("name", m_data.m_Project.m_ProjectName);
                writer.WriteStartElement("Files");
                writer.WriteStartElement("SourceFiles");
                foreach (string source in m_data.m_Project.m_SourceFileNames.Distinct())
                {
                    FileInfo data = m_data.GUnitData_getFileInformation(source);
                    if (data != null)
                    {
                        if (data.m_IsDirty == true)
                        {
                            File.WriteAllText(source, data.m_text);
                            //evFileReceived(data.m_fileName);
                            //data.m_IsDirty = false;
                            
                        }
                      
                    }
                    writer.WriteStartElement("File");
                    writer.WriteAttributeString("Path", m_data.getReleativePath(source));
                    writer.WriteEndElement();//File
                }
                writer.WriteEndElement();//SourceFiles
                evfileOpenComplete("");
                writer.WriteStartElement("HeaderFiles");
                foreach (string header in m_data.m_Project.m_HeaderFileNames.Distinct())
                {
                    FileInfo data = m_data.GUnitData_getFileInformation(header);
                    if (data != null)
                    {
                        if (data.m_IsDirty == true)
                        {
                            File.WriteAllText(header, data.m_text);
                             //evFileReceived(data.m_fileName);
                             
                            //data.m_IsDirty = false;
                        }
                      
                    }
                    writer.WriteStartElement("File");
                    writer.WriteAttributeString("Path", m_data.getReleativePath(header));
                    writer.WriteEndElement();//File
                }
                writer.WriteEndElement();//HeaderFiles
                writer.WriteEndElement();//Files
                evfileOpenComplete("");
                writer.WriteStartElement("IncludePaths");
               
                foreach (string inc in m_data.m_Project.m_SolnData.m_includePaths)
                {
                    writer.WriteStartElement("Include");
                    writer.WriteAttributeString("Path", inc);
                    writer.WriteEndElement();//Include
                }
                writer.WriteEndElement();//IncludePaths
                evfileOpenComplete("");
                writer.WriteStartElement("LibPaths");

                foreach (string lib in m_data.m_Project.m_SolnData.LibPaths)
                {
                    writer.WriteStartElement("Lib");
                    writer.WriteAttributeString("Path", lib);
                    writer.WriteEndElement();//Lib
                }
                writer.WriteEndElement();//LibPaths
                evfileOpenComplete("");
                writer.WriteStartElement("LibNames");

                foreach (string libName in m_data.m_Project.m_SolnData.m_Libraries)
                {
                    writer.WriteStartElement("LibName");
                    writer.WriteAttributeString("Path", libName);
                    writer.WriteEndElement();//LibName
                }
                writer.WriteEndElement();//LibNames
                evfileOpenComplete("");
                writer.WriteStartElement("ProjectHeaders");

                foreach (string commonHeader in m_data.m_Project.m_SolnData.m_CommonHeadersList)
                {
                    FileInfo data = m_data.GUnitData_getFileInformation(commonHeader);
                    if (data != null)
                    {
                        if (data.m_IsDirty == true)
                        {
                            File.WriteAllText(commonHeader, data.m_text);
                            //evFileReceived(data.m_fileName);

                            //data.m_IsDirty = false;
                        }
                       
                    }
                    writer.WriteStartElement("Include");
                    writer.WriteAttributeString("Path", m_data.getReleativePath(commonHeader));
                    writer.WriteEndElement();//Include
                                 
                }
                writer.WriteEndElement();//ProjectHeaders
                evfileOpenComplete("");
                writer.WriteStartElement("Bin");
                writer.WriteAttributeString("Path", m_data.m_Project.m_SolnData.m_BinPath);
                writer.WriteEndElement();//Bin
                evfileOpenComplete("");
                writer.WriteEndElement();//Project
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            
            evSaveProject(m_data.m_Project);
            
        }
        /*********************************************************************/
        /*! \fn private void save_FileOk(object sender, CancelEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void save_FileOk(object sender, CancelEventArgs e)
        {
            string ProjectName = m_SaveFileDlg.FileName;
            m_data.m_Project.m_ProjectPath = ProjectName;
            m_data.m_Project.m_ProjectName = Path.GetFileName(ProjectName).Split('.')[0];
            GUnit_WriteProjectData(ProjectName);
            evNewProject(m_data.m_Project);
    
        }
        /*********************************************************************/
        /*! \fn private string hasAttribute(XElement element, string attribute, XNamespace xns)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private string hasAttribute(XElement element, string attribute, XNamespace xns)
        {
            string result = "";
            result = element.Attribute(xns + attribute) != null ? element.Attribute(xns + attribute).Value.ToString() : "";
            return result;
        }
        /*********************************************************************/
        /*! \fn private void GUnit_openProject()
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_openProject()
        {
            string ProjectName = m_OpenprojectDlg.FileName;
            if (File.Exists(ProjectName))
            {
               
                frmLoading = new Loading(this);
                
                
                XDocument doc = XDocument.Load(ProjectName);
                
                m_data.m_Project.m_ProjectName = doc.Root.Attribute("name").Value.ToString();
                m_data.m_Project.m_ProjectPath = ProjectName;
                IEnumerable<XElement> SourceFilesList = from files in doc.Root.Descendants("SourceFiles")
                                                    select files;
                IEnumerable<XElement> SourceFiles = from files in SourceFilesList.Descendants("File")
                                                        select files;
                
                foreach (XElement srcFile in SourceFiles)
                {
                    string sourcePath = hasAttribute(srcFile, "Path", "");
                    if (string.IsNullOrWhiteSpace(sourcePath) == false)
                    {
                        var s = Path.Combine(Path.GetDirectoryName(m_data.m_Project.m_ProjectPath), sourcePath);
                        s = Path.GetFullPath(s);
                        m_data.GUnitDat_AddSourceFiles(s);
                    }
                    
                }
                IEnumerable<XElement> HeaderFilesList = from files in doc.Root.Descendants("HeaderFiles")
                                                        select files;
                IEnumerable<XElement> HeaderFiles = from files in HeaderFilesList.Descendants("File")
                                                    select files;
                foreach (XElement hedFile in HeaderFiles)
                {
                    string headerPath = hasAttribute(hedFile, "Path", "");
                    if (string.IsNullOrWhiteSpace(headerPath) == false)
                    {
                        var s = Path.Combine(Path.GetDirectoryName(m_data.m_Project.m_ProjectPath), headerPath);
                        s = Path.GetFullPath(s);
                        m_data.GUnitDat_AddHeaderFiles(s);
                    }
                }
                IEnumerable<XElement> includeList = from files in doc.Root.Descendants("IncludePaths")
                                                        select files;
                IEnumerable<XElement> includes = from files in includeList.Descendants("Include")
                                                    select files;
                foreach (XElement include in includes)
                {
                    m_data.GUnitDat_AddIncludePaths(hasAttribute(include, "Path", ""),true);
                    
                }
                IEnumerable<XElement> LibPathList = from files in doc.Root.Descendants("LibPaths")
                                                    select files;
                IEnumerable<XElement> LibPaths = from files in LibPathList.Descendants("Lib")
                                                 select files;
                foreach (XElement lib in LibPaths)
                {
                    m_data.GUnitDat_AddLibPaths(hasAttribute(lib, "Path", ""),true);
                    
                }
                IEnumerable<XElement> LibNameList = from files in doc.Root.Descendants("LibNames")
                                                    select files;
                IEnumerable<XElement> LibNames = from files in LibNameList.Descendants("LibName")
                                                 select files;
                foreach (XElement libName in LibNames)
                {
                    m_data.GUnitDat_AddLibNames(hasAttribute(libName, "Path", ""));
                    
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
                        var s = Path.Combine(Path.GetDirectoryName(m_data.m_Project.m_ProjectPath), commonHeader);
                        s = Path.GetFullPath(s);
                        m_data.GUnitDat_AddCommonHeaders(s,true);
                    }
                   

                }
                IEnumerable<XElement> Bin = from files in doc.Root.Descendants("Bin")
                                                    select files;
                
                foreach (XElement BinPath in Bin)
                {
                    m_data.GUnitDat_AddBinPath(hasAttribute(BinPath, "Path", ""),true);
                }
                if (
                     m_data.m_Project.m_HeaderFileNames.Count() != 0 ||
                    m_data.m_Project.m_SourceFileNames.Count() != 0 ||
                     m_data.m_Project.m_SolnData.m_CommonHeadersList.Count()!= 0
                    )
                {
                    Application.UseWaitCursor = true;
                    int count = m_data.m_Project.m_HeaderFileNames.Count() + m_data.m_Project.m_SourceFileNames.Count() + m_data.m_Project.m_SolnData.m_CommonHeadersList.Count();
                    frmLoading.Loading_setProgressMax(count);
                    frmLoading.Show();
                    evNewStatus("Start Processing " + count + " Files");
                    this.Enabled = false;
                }
                evOpenProject(m_data.m_Project);
               
            }
        }
        /*********************************************************************/
        /*! \fn string extractPremake()
        * \brief 
        * \return void
        */
        /*********************************************************************/
        string extractPremake()
        {
            try
            {
                CtagParser obj = new CtagParser(this);

                string premake = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) + "\\premake4" + Thread.CurrentThread.ManagedThreadId + ".exe";
                string prevDir = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(Path.GetDirectoryName(m_data.m_Project.m_ProjectPath));

                obj.ExtractResource("GUnit.premake4.exe", premake);
                Directory.SetCurrentDirectory(prevDir);
                return premake;
            }
            catch
            {
                return null;
            }
        }
        /*********************************************************************/
        /*! \fn void extractmake(string runPath)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        void extractmake(string runPath)
        {
            CtagParser obj = new CtagParser(this);
            obj.ExtractResource("GUnit.gnumake.cmd", runPath + "\\gnumake.cmd");
            obj.ExtractResource("GUnit.gnumake2.cmd", runPath + "\\gnumake2.cmd");
         }
        /*********************************************************************/
        /*! \fn void RunWithRedirect(string cmdPath,string RunDir,string arg,bool deletefile =true)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        void RunWithRedirect(string cmdPath,string RunDir,string arg,bool deletefile =true)
        {
            try
            {
                string prevDir = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(RunDir);
                string pathvar = System.Environment.GetEnvironmentVariable("PATH");
                Process proc;
                if (arg == null)
                {
                    proc = new Process()

                    {
                        StartInfo = new ProcessStartInfo(cmdPath)
                        {
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WorkingDirectory = RunDir

                        },
                        EnableRaisingEvents = true
                    };
                }
                else
                {
                    proc = new Process()

                   {
                       StartInfo = new ProcessStartInfo(cmdPath, arg)
                       {
                           RedirectStandardOutput = true,
                           RedirectStandardError = true,
                           CreateNoWindow = true,
                           UseShellExecute = false,
                           WorkingDirectory = RunDir
                       },
                       EnableRaisingEvents = true
                   };
                }



                // see below for output handler
                proc.ErrorDataReceived += proc_ErrorDataReceived;
                proc.OutputDataReceived += proc_DataReceived;

                proc.Start();

                proc.BeginErrorReadLine();
                proc.BeginOutputReadLine();

                proc.WaitForExit();
                if (deletefile)
                {
                    File.Delete(cmdPath);
                }
                Directory.SetCurrentDirectory(prevDir);
            }
            catch
            {

            }
        }
        void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {

                evConsoleUpdate(e.Data + "\n");

                if (e.Data.Contains("error:"))
                {
                    m_data.m_ErrorCount++;
                    evConsoleTextColorUpdate(e.Data, Color.Red);
                    m_data.m_buildErrors.Add(e.Data);
                }
                else
                {
                    m_data.m_WarningCount++;
                    evConsoleTextColorUpdate(e.Data, Color.YellowGreen);
                    m_data.m_buildWarnings.Add(e.Data);
                }

            }
        }
        /*********************************************************************/
        /*! \fn void proc_DataReceived(object sender, DataReceivedEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/
        void proc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {

                evConsoleUpdate(e.Data+"\n");
                
                
            }
        }
        /*********************************************************************/
        /*! \fn private void GUnit_build()
        * \brief 
        * \return void
        */
        /*********************************************************************/
        private void GUnit_build()
        {
            m_data.m_ErrorCount = 0;
            m_data.m_buildErrors.Clear();
            m_data.m_buildWarnings.Clear();
            evNewStatus("Starting the Build");
            string OutputFile = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath)+"\\Output.xml";
            coverageAnalysisToolStripMenuItem.Enabled = false;
            m_data.GUnitDat_UpdateSolutionData();
            frmSolution = new SolutionBuilder(m_data.m_Project,m_data);
            evNewStatus("Creating the Make File");
            frmSolution.SolnBldr_BuildSolution("ConsoleApp");
            frmLoading = new Loading(this);
            frmLoading.Loading_setProgressMax(frmSolution.m_buildCommandList.Count()+1);
            frmLoading.Show();
            frmConsole.Console_ClearConsole();
            try
            {
                if (File.Exists(OutputFile))
                {
                    File.Delete(OutputFile);
                }
                BackgroundWorker Builder = new BackgroundWorker();
                Builder.DoWork += new DoWorkEventHandler(runBuilder);
                Builder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runBuilderComplete);
                Builder.RunWorkerAsync(null);
            }
            catch (Exception err)
            {
                evConsoleUpdate(err.ToString());
                Application.UseWaitCursor = false;
            }
        }
        /*********************************************************************/
        /*! \fn public void runBuilder(object sender, DoWorkEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        public void runBuilder(object sender, DoWorkEventArgs e)
        {
            Application.UseWaitCursor = true;
            string runDir = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) + "\\Build";
            RunWithRedirect(extractPremake(), Path.GetDirectoryName(m_data.m_Project.m_ProjectPath), " gmake ");
            extractmake(runDir);
            evNewStatus("Running Build");
           // RunWithRedirect(runDir + "\\gnumake.cmd", runDir, "all");
            foreach (string command in frmSolution.m_buildCommandList)
            {
                
                RunWithRedirect("g++", Path.GetDirectoryName(m_data.m_Project.m_ProjectPath), command);
                frmLoading.Loading_setProgressValue();
            }
            //RunWithRedirect(" g++", runDir, " ");
            

        }
        
        /*********************************************************************/
        /*! \fn public void runBuilderComplete(object sender, RunWorkerCompletedEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        public void runBuilderComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            
           
            string runDir = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) + "\\Build";
            Cursor.Current = Cursors.Default;
            if (m_data.m_ErrorCount != 0 || m_data.m_WarningCount != 0)
            {
                evConsoleUpdate("Build Completed With "+m_data.m_ErrorCount+" Errors and "+m_data.m_WarningCount+" Warnings");
                foreach (string str in m_data.m_buildErrors)
                {
                    evConsoleTextColorUpdate(str, Color.Red);
                }
                foreach (string str in m_data.m_buildWarnings)
                {
                    evConsoleTextColorUpdate(str, Color.YellowGreen);
                }
            }
            else
            {
                evConsoleUpdate("Build Completed Successfully");
                evNewStatus("Build Completed SuccessFully");
            }
            frmLoading.Loading_setProgressValue();
            Application.UseWaitCursor = false;
           
        }
        /*********************************************************************/
        /*! \fn private void GUnit_CreateMockUnit()
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void GUnit_CreateMockUnit()
        {
            
            List<UnitInfo> m_StubUnitList = frmFunctIf.Functionif_getMockList();
            foreach (UnitInfo Unit in m_StubUnitList)
            {
                foreach (FunctionalInterface function in Unit.m_MockFunctionList)
                {
                    GUnit_correctFunctionInterface(function);
                }
                MockGenerator mock = new MockGenerator(this);
                mock.MockGen_GenerateMock(Unit);
            }
            MessageBox.Show("Stubs are gnerated in the "+m_data.m_Project.m_ProjectName+"_Stubs Folder");


        }
        /*********************************************************************/
        /*! \fn private List<string> removeEmptyStrings(string[] stringlist)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private List<string> removeEmptyStrings(string[] stringlist)
        {
            List<string> restOfWord = stringlist.ToList<string>();
            restOfWord.RemoveAll(string.IsNullOrWhiteSpace);
            return restOfWord;

        }
        /*********************************************************************/
        /*! \fn public void GUnit_correctFunctionInterface(FunctionalInterface function)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        public void GUnit_correctFunctionInterface(FunctionalInterface function)
        {
            string[] stringSeparators = new string[] { "(", ")", ";" };
            List <string>funcionProtoList = (function.m_Signature.Split(stringSeparators, StringSplitOptions.None)).ToList();
            funcionProtoList.RemoveAll(string.IsNullOrWhiteSpace);
            if (funcionProtoList.Count > 0)
            {
                List<string> args_z = new List<string>();
                /*Seperate each argument by splitting based on Comma*/
                List<string> seperateVariableName = removeEmptyStrings(funcionProtoList[0].Split(','));
                foreach (string st in seperateVariableName)
                {
                    /*Seperate the variable name from the type declairation for each argument*/
                    List<string> args = removeEmptyStrings(st.Split(' '));
                    string argument = "";
                    /*Always the last element is the variable name so
                     * Append all other elements till the last element to 
                     * Build the fully qualified Type declairation
                     */
                    for (int i = 0; i < args.Count - 1; i++)
                    {
                        argument += " " + args[i];
                    }
               
                    /*
                         * In case the Pointer De referencing is attached to the 
                         * variable name need to re attach it to the type found using 
                         * the previous step(eg. decl such as Datatype *VariableName should be interpreted as 
                         * Datatype* VariableName)
                         */
                    if (args[args.Count - 1].StartsWith("*"))
                    {
                        argument += " *";
                    }
                    args_z.Add(argument);
                    
                }
                function.m_argumentTypes.Clear();
                foreach (string arg in args_z)
                {
                    /*Add the argument only if it is not void*/
                   
                    if (arg != "void" && string.IsNullOrWhiteSpace(arg) == false)
                    {
                        {
                            function.m_argumentTypes.Add(arg);
                        }
                    }

                }
               
            }

        }
        /*********************************************************************/
        /*! \fn private void runGcov(string fileName)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void runGcov(string fileName)
        {
            string runDir = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) + "\\Build";
            if (Directory.Exists(runDir))
            {
                RunWithRedirect("gcov", runDir, ".\\obj\\" + Path.GetFileName(fileName),false);

            }
        }
        /*********************************************************************/
        /*! \fn private void analyseCoverage(object sender, DoWorkEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void analyseCoverage(object sender, DoWorkEventArgs e)
        {
            if (e.Argument is CoverageFiles)
            {
                Application.UseWaitCursor = true;
            
                CoverageFiles file = e.Argument as CoverageFiles;
                string runDir = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) + "\\Build";
               
                runGcov(file.m_srcFile);
                file.m_gcovFile = runDir + "\\" + file.m_gcovFile; 
                e.Result = file;
            }
        }
        /*********************************************************************/
        /*! \fn private void analyseCoverageComplete(object sender, RunWorkerCompletedEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void analyseCoverageComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is CoverageFiles)
            {
                CoverageFiles filename = e.Result as CoverageFiles;
                CoverageAnalyser analyser = new CoverageAnalyser(this);
                Coverage coverage = analyser.Coverage_AnalyseStatementCoverage(filename.m_gcovFile);
                FileInfo data = m_data.GUnitData_getFileInformation(filename.m_srcFile);
                if (data != null)
                {
                    data.m_CoverageInfo = coverage;
                    m_data.GUnitData_UpdateProjectTable(filename.m_srcFile, data);
                  


                }
                evfileOpenComplete(filename.m_srcFile);
            }
           
            coverageHTML.Enabled = true;
            Application.UseWaitCursor = false;
        }
        void extractGcovr(string runPath)
        {
            CtagParser obj = new CtagParser(this);
            obj.ExtractResource("GUnit.gcovr_MM.py", runPath + "\\gcovr_MM.py");
            
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
        private void GUnit_DisplayCoverage(FileInfo data)
        {
                     
            if (data != null)
            {
                string fileName = data.m_fileName;
                FileOpen fileOp = new FileOpen(this, fileName);
                fileOp.Text = "Coverage Report:" + Path.GetFileName(fileName);
                fileOp.Show(dockPanel1, DockState.Document);
                fileOp.updateSourceText(data.m_text);
                fileOp.FileOpen_HighlightCoverage(data.m_CoverageInfo);
            }
        }
        void extractXSL(string runPath)
        {
            CtagParser obj = new CtagParser(this);
            obj.ExtractResource("GUnit.gtest-result.xsl", runPath + "\\x.xsl");
           
        }
        private void GUnit_TransformXML(string XML)
        {
            if (File.Exists(XML))
            {
                try
                {
                    string runDir = Path.GetDirectoryName(XML);
                    extractXSL(runDir);
                    XslTransform myXslTransform;
                    myXslTransform = new XslTransform();
                    myXslTransform.Load(runDir + "\\x.xsl");
                    myXslTransform.Transform(XML, runDir + "\\TestReport.html");
                    if (File.Exists(runDir + "\\TestReport.html"))
                    {
                        TestReport report = new TestReport(runDir + "\\TestReport.html");
                        report.Text = "Unit Test Report";
                        report.Show(dockPanel1, DockState.Document);
                    }
                }
                catch
                {

                }
            }
        }

        /*********************************************************************/
        /*! \fn private void runexe(object sender, DoWorkEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void runexe(object sender, DoWorkEventArgs e)
        {
            string OutputFile = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) + "\\Output.xml";
            try
            {
                if (File.Exists(OutputFile))
                {
                    File.Delete(OutputFile);
                }
                string file = e.Argument as string;
                RunWithRedirect(file, Path.GetDirectoryName(file), "--gtest_output=\"xml:" + OutputFile+"\"", false);
            }
            catch(Exception err)
            {
                evConsoleUpdate(err.ToString());
            }

        }
        /*********************************************************************/
        /*! \fn public void runExeComplete(object sender, RunWorkerCompletedEventArgs e)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        public void runExeComplete(object sender, RunWorkerCompletedEventArgs e)
        {
             string OutputFile = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) + "\\Output.xml";
             enableCoverageAnalysis(true);
             GUnit_TransformXML(OutputFile);
        }
        /*********************************************************************/
        /*! \fn private void enableCoverageAnalysis(bool enable)
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void enableCoverageAnalysis(bool enable)
        {
            coverageAnalysisToolStripMenuItem.Enabled = enable;
            Coverage.Enabled = enable;
        }
        /*********************************************************************/
        /*! \fn private void GUnit_Run()
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void GUnit_Run()
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "GUnit Executables (*.exe)|*.exe";
            file.ShowDialog();
            if (File.Exists(file.FileName))
            {
                BackgroundWorker runner = new BackgroundWorker();
                runner.DoWork += new DoWorkEventHandler(runexe);
                runner.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runExeComplete);
                runner.RunWorkerAsync(file.FileName);
            }
        }
        /*********************************************************************/
        /*! \fn private void GUnit_AnalyseCoverage()
        * \brief 
        * \return void
        */
        /*********************************************************************/  
        private void GUnit_AnalyseCoverage()
        {
            Application.UseWaitCursor = true;
            frmLoading = new Loading(this);
            frmLoading.Loading_setProgressMax(m_data.m_Project.m_SourceFileNames.Distinct().Count());
            frmLoading.Show();
            evNewStatus("starting Coverage Analysis ");
            foreach (string srcfilePath in m_data.m_Project.m_SourceFileNames.Distinct())
            {
                
                CoverageFiles coverageFiles = new CoverageFiles();
                coverageFiles.m_srcFile = srcfilePath;
                coverageFiles.m_gcovFile = Path.GetFileName(srcfilePath) + ".gcov";
                BackgroundWorker CoverageAnalyser = new BackgroundWorker();
                CoverageAnalyser.DoWork += new DoWorkEventHandler(analyseCoverage);
                CoverageAnalyser.RunWorkerCompleted += new RunWorkerCompletedEventHandler(analyseCoverageComplete);
                CoverageAnalyser.RunWorkerAsync(coverageFiles);
            }
            
        }

        private void coverageHTML_Click(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            frmLoading = new Loading(this);
            frmLoading.Loading_setProgressMax(1);
            frmLoading.Show();
            evNewStatus("Generating Test Coverage Report");
            BackgroundWorker CoverageAnalyser = new BackgroundWorker();
            CoverageAnalyser.DoWork += new DoWorkEventHandler(GenerateCoverageReport);
            CoverageAnalyser.RunWorkerCompleted += new RunWorkerCompletedEventHandler(GenerateCoverageReportComplete);
            CoverageAnalyser.RunWorkerAsync();
            
        }
        private void GenerateCoverageReportComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            string runDir = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath) ;
            evfileOpenComplete(runDir + "\\CoverageReport\\CoverageReport.html");
            if (File.Exists(runDir + "\\CoverageReport\\CoverageReport.html"))
            {
                TestReport obj = new TestReport(runDir + "\\CoverageReport\\CoverageReport.html");
                obj.Text = "Unit Test Coverage Report";
                obj.Show(dockPanel1, DockState.Document);
            }
            Application.UseWaitCursor = false;
            evProcessComplete();

        }
        private void GenerateCoverageReport(object sender, DoWorkEventArgs e)
        {
            
            string runDir = Path.GetDirectoryName(m_data.m_Project.m_ProjectPath);
            if (Directory.Exists(runDir + "\\CoverageReport") == false)
            {
                Directory.CreateDirectory(runDir + "\\CoverageReport");
            }

            extractGcovr(runDir);
            string CommonPath = FindCommonPath("\\", m_data.m_Project.m_SourceFileNames);
            CommonPath = m_data.getReleativePathWith(CommonPath, runDir);
            RunWithRedirect("python", runDir, runDir + "\\gcovr_MM.py -r " + CommonPath + "  --html --html-details -o .\\CoverageReport\\CoverageReport.html");
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

  

      
   }
}
