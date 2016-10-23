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
using WeifenLuo.WinFormsUI.Docking;
using Gunit.HelperClasses;
namespace Gunit.Ui
{
    public partial class Editor : DockContent
    {
        ICGunitHost m_host = null;
        private FileSystemWatcher m_watcher = null;
        DateTime m_lastWriteTime;
        private ScintillaSetUp m_scintillaSetUp;
        public Editor()
        {
            InitializeComponent();
        }
        public Editor(ICGunitHost host)
        {
            InitializeComponent();
            m_host = host;
        }
        public void Save()
        {
            if (File.Exists(m_host.CurrentFileInEditor))
            {
                m_watcher.EnableRaisingEvents = false;
                StreamWriter writer = new StreamWriter(m_host.CurrentFileInEditor);
                writer.Write(scintilla.Text);
                writer.Close();
                m_watcher.EnableRaisingEvents = true;
                this.Text = Path.GetFileName(m_host.CurrentFileInEditor);
            }
        }
        private void Editor_Load(object sender, EventArgs e)
        {
            m_scintillaSetUp = new ScintillaSetUp(scintilla);
            m_scintillaSetUp.Scintilla_Init();
            scintilla.Text = "";
            m_host.PropertyChanged += new PropertyChangedEventHandler(Host_PropertyChanged);
            m_watcher = new FileSystemWatcher();
            this.Enabled = false;
          
        }
        void Host_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentFile":
                    
                    if (File.Exists(m_host.CurrentFileInEditor))
                    {
                        enableWatcher();
                        readFile();
                    }
                    else
                    {
                        scintilla.Text = "";
                    }
                    break;
                default:
                    break;

            }
        }
        private void enableWatcher()
        {
            m_watcher.Path = Path.GetDirectoryName(m_host.CurrentFileInEditor);
            m_watcher.Filter = Path.GetFileName(m_host.CurrentFileInEditor);
            m_watcher.NotifyFilter = NotifyFilters.LastWrite;
            m_watcher.Changed -= Filewatcher_Changed;
            m_watcher.Deleted -= Filewatcher_Deleted;
            m_watcher.Changed += Filewatcher_Changed;
            m_watcher.Deleted += Filewatcher_Deleted;
            m_watcher.EnableRaisingEvents = true;
        }
        private void readFile()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Text = Path.GetFileName(m_host.CurrentFileInEditor);
                   
                    Document_readFileThread(m_host.CurrentFileInEditor);
                });
            }
            else
            {
                this.Text = Path.GetFileName(m_host.CurrentFileInEditor);
                
                Document_readFileThread(m_host.CurrentFileInEditor);
            }
        }
        private void Document_readFileThread(string l_fileName)
        {
            
            BackgroundWorker reader = new BackgroundWorker();
            reader.DoWork += new DoWorkEventHandler(Document_readFile);
            reader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Document_readFileComplete);
            reader.RunWorkerAsync(l_fileName);
        }
        private void Filewatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            scintilla.Text = "";
        }
        private void Filewatcher_Changed(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = System.IO.File.GetLastWriteTime(m_host.CurrentFileInEditor);
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
        private void Document_readFileComplete(object sender, RunWorkerCompletedEventArgs e)
        {
           
            scintilla.Text = e.Result as string;
            m_lastWriteTime = System.IO.File.GetLastWriteTime(m_host.CurrentFileInEditor);
            m_watcher.EnableRaisingEvents = true;
            this.Enabled = true;
        }
    }
}
    