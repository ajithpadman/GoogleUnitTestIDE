using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gunit.DataModel;
using System.IO;
using ScintillaNET;
using GUnit_IDE2010.Ui;
namespace Gunit.Ui
{
    public partial class Document : GUnitSideBarBase
    {
        private ScintillaSetUp m_scintillaSetUp;
        private Document_Search m_searchUi;
        private string m_codeCompletion = "";
        private FileSystemWatcher m_watcher = null;
        DateTime m_lastWriteTime;
        /// <summary>
        /// Constructor for each document
        /// </summary>
        /// <param name="model"></param>
        public Document(DocumentDataModel model)
            : base(model)
        {
            InitializeComponent();
            if (System.IO.File.Exists(model.CurrentFile))
            {
                m_lastWriteTime = System.IO.File.GetLastWriteTime(model.CurrentFile);
                m_watcher = new FileSystemWatcher();
                m_watcher.Path = Path.GetDirectoryName(model.CurrentFile);
                m_watcher.Filter = Path.GetFileName(model.CurrentFile);
                m_watcher.NotifyFilter = NotifyFilters.LastWrite;
                m_watcher.Changed += Filewatcher_Changed;
                m_watcher.Deleted += Filewatcher_Deleted;
                m_watcher.EnableRaisingEvents = true;
            }
        }

        private void Filewatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            this.Close();
        }
        private void readFile()
        {
            if(this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = Path.GetFileName(((DocumentDataModel)m_model).FileName);
                    Document_readFileThread(((DocumentDataModel)m_model).FileName);
                });
            }
            else
            {
                this.Text = Path.GetFileName(((DocumentDataModel)m_model).FileName);
                Document_readFileThread(((DocumentDataModel)m_model).FileName);
            }
        }
        private void Filewatcher_Changed(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = System.IO.File.GetLastWriteTime(((DocumentDataModel)m_model).CurrentFile);
            m_watcher.EnableRaisingEvents = false;
            if (lastWriteTime != m_lastWriteTime)
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Changed:
                        DialogResult result = MessageBox.Show(Path.GetFileName(e.FullPath) + "  is modified. Do you like to reload?");
                        if (result == DialogResult.OK)
                        {
                            readFile();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Start the file read thread to display on the document
        /// </summary>
        /// <param name="l_fileName">absolute path to the File to be displayed</param>
        private void Document_readFileThread(string l_fileName)
        {
            BackgroundWorker reader = new BackgroundWorker();
            reader.DoWork += new DoWorkEventHandler(Document_readFile);
            reader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Document_readFileComplete);
            reader.RunWorkerAsync(((DocumentDataModel)m_model).FileName);
        }
        /// <summary>
        /// Worker Function to read file executed from Worker thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_readFile(object sender, DoWorkEventArgs e)
        {
            string line = "";
            string text = "";
            string filePath = e.Argument as string;
            if (System.IO.File.Exists(filePath))
            {
                StreamReader reader = new StreamReader(filePath);
                while ((line = reader.ReadLine()) != null)
                {
                    text += line + "\n";
                }
                reader.Close();
            }

            e.Result = text;
        }
        private void ClearOldErrorLines()
        {
            foreach (CodeLocation location in ((DocumentDataModel)m_model).CurrentErrorLines)
            {
                m_scintillaSetUp.clearHighLight(location);
            }
        }
        private void ClearOldWarningLines()
        {
            foreach (CodeLocation location in ((DocumentDataModel)m_model).CurrentWarningLines)
            {
                m_scintillaSetUp.clearHighLight(location);
            }
        }
        public void UpdateErrorLines()
        {
            ClearOldErrorLines();
            ((DocumentDataModel)m_model).CurrentErrorLines = ((DocumentDataModel)m_model).DocumentErrors;
            foreach (CodeLocation location in ((DocumentDataModel)m_model).DocumentErrors)
            {
                Document_HighlightLine(location, HighLightType.ERROR);

            }
        }
        public void UpdateWarningLines()
        {
            ClearOldWarningLines();
            ((DocumentDataModel)m_model).CurrentWarningLines = ((DocumentDataModel)m_model).DocumentWarnings;
            foreach (CodeLocation location in ((DocumentDataModel)m_model).DocumentWarnings)
            {
                Document_HighlightLine(location, HighLightType.WARNING);

            }
        }
        /// <summary>
        /// Read file thread completed Indication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_readFileComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            ((DocumentDataModel)m_model).Text = e.Result as string;
            UpdateErrorLines();
            UpdateWarningLines();
            m_lastWriteTime = System.IO.File.GetLastWriteTime(((DocumentDataModel)m_model).CurrentFile);
            m_watcher.EnableRaisingEvents = true;
        }
        /// <summary>
        /// Form Load Event handler for Document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_Load(object sender, EventArgs e)
        {
            this.CloseButtonVisible = true;
            m_model.Session = Sessions.OPEN_FILE;
            m_scintillaSetUp = new ScintillaSetUp(scintilla);
            m_searchUi = new Document_Search(((DocumentDataModel)m_model));
            m_scintillaSetUp.Scintilla_Init();
            this.Text = Path.GetFileName(((DocumentDataModel)m_model).FileName);
            Document_readFileThread(((DocumentDataModel)m_model).FileName);
            foreach (string line in ((DocumentDataModel)m_model).CodeCompletionWords)
            {
                m_codeCompletion += line + " ";
            }
            foreach (string line in m_scintillaSetUp.googleKeyWords)
            {
                m_codeCompletion += line + " ";
            }
        }

      
        /// <summary>
        /// Open file call from controller
        /// </summary>
        /// <param name="l_currentFile"></param>
        public override void OpenFile(string l_currentFile)
        {
            base.OpenFile(l_currentFile);
            ((DocumentDataModel)m_model).IsActive = true;  
        }
        public override void CloseFile(string l_currentFile)
        {
            base.CloseFile(l_currentFile);
            ((DocumentDataModel)m_model).IsActive = false;
        }
        public override void saveProject()
        {
            base.saveProject();
            if (scintilla.Text != ((DocumentDataModel)m_model).Text)
            {
                m_watcher.EnableRaisingEvents = false;
                StreamWriter writer = new StreamWriter(m_model.CurrentFile);
                writer.Write(scintilla.Text);
                writer.Close();
                m_watcher.EnableRaisingEvents = true;
                this.Text = Path.GetFileName(m_model.CurrentFile);
            }
        }
        private void SearchText()
        {
           ((DocumentDataModel)m_model).SearchStart =  m_scintillaSetUp.Scintialla_search(((DocumentDataModel)m_model).SearchStart, ((DocumentDataModel)m_model).SearchText);
           if (((DocumentDataModel)m_model).SearchStart == -1)
           {
               MessageBox.Show("Reached End of Document");
           }
           else
           {
               CodeLocation location = m_scintillaSetUp.GetCodeLocationFromPosition(((DocumentDataModel)m_model).SearchStart);
               m_scintillaSetUp.scrollToLine(location);
               m_scintillaSetUp.HighLightLine(location, HighLightType.NORMAL_HIGHLIGHT);
           }

        }
        /// <summary>
        /// property changed indication to the Document
        /// </summary>
        /// <param name="propertyName"></param>
        public override void propertyChanged(string propertyName)
        {
            base.propertyChanged(propertyName);
            switch (propertyName)
            {
                case "Text":
                    scintilla.Text = ((DocumentDataModel)m_model).Text;
                    this.Text = Path.GetFileName(m_model.CurrentFile);
                    break;
                case "FileName":
                    Document_readFileThread(((DocumentDataModel)m_model).FileName);
                    break;
                case "FocusLine":
                    Document_scrollToLine(((DocumentDataModel)m_model).CurrentLine);
                    break;
                case "ErrorUpdate":
                    UpdateErrorLines();
                    break;
                case "Search":
                    SearchText();
                    
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Event handler for Document Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_CloseForm(object sender, FormClosedEventArgs e)
        {
            m_model.Session = Sessions.CLOSE_FILE;
            m_watcher.Changed -= Filewatcher_Changed;
            m_watcher.Deleted -= Filewatcher_Deleted;
        }
        /// <summary>
        /// Scintilla Text change Indication
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_TextChanged(object sender, EventArgs e)
        {
            this.Text = "*" + Path.GetFileName(m_model.CurrentFile);
        }
        /// <summary>
        /// Scroll to a specific line in file
        /// </summary>
        /// <param name="line"></param>
        private void Document_scrollToLine(CodeLocation location)
        {
            m_scintillaSetUp.scrollToLine(location);
            m_scintillaSetUp.HighLightLine(location, HighLightType.NORMAL_HIGHLIGHT);
        }
        private void Document_HighlightLine(CodeLocation location,HighLightType type)
        {
            m_scintillaSetUp.HighLightLine(location, type);
        }

        private void definitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string currentWord = m_scintillaSetUp.getCurrentWord();

        }

        private void Document_activated(object sender, EventArgs e)
        {
            m_model.Session = Sessions.FOCUS_FILE;
        }

        private void Document_UIupdate(object sender, UpdateUIEventArgs e)
        {
           
        }

        private void Document_painted(object sender, EventArgs e)
        {
         
            
        }

        private void Document_Click(object sender, EventArgs e)
        {
          
        }
        private void scintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            // Find the word start
            var currentPos = scintilla.CurrentPosition;
            var wordStartPos = scintilla.WordStartPosition(currentPos, true);

            string currentWord = scintilla.GetWordFromPosition(currentPos);
            uint Line = (uint)scintilla.LineFromPosition(currentPos);
            uint Column = (uint)scintilla.GetColumn(currentPos);
            try
            {
                // Display the autocompletion list
                var lenEntered = currentPos - wordStartPos;
                if (lenEntered > 0)
                {
                    scintilla.AutoCShow(lenEntered, m_codeCompletion);
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void Document_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.F)
            {
                m_searchUi = new Document_Search(((DocumentDataModel)m_model));
                m_searchUi.Show();
            }
        }
        public void updateMacro(ListofStrings macros)
        {
            m_scintillaSetUp.Scintialla_updateMacros(macros);
        }
        private void Scintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.F)
            {
                if (m_searchUi.IsDisposed)
                {
                    m_searchUi = new Document_Search(((DocumentDataModel)m_model));
                    m_searchUi.Show();
                }
                else
                {
                    m_searchUi.Show();
                }
            }
        }
    }
}
