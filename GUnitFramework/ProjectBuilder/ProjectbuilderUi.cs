using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUnitFramework.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
using GUnitFramework.Implementation;
using System.IO;
using System.Xml;
using System.Xml.Linq;
namespace ProjectBuilder
{
    public partial class ProjectbuilderUi : DockContent
    {
        ProjectBuilder m_builder;
        public ProjectbuilderUi()
        {
            InitializeComponent();
        }
        public ProjectbuilderUi(ProjectBuilder builder)
        {
            InitializeComponent();
            m_builder = builder;
           
        }
        private void updateWarningLevel()
        {
            if (m_builder.WarningLevel == WarningLevel.AbortCompilationOnFirstError)
            {
                cbWarningLevel.SelectedIndex = 0;
            }
            else if (m_builder.WarningLevel == WarningLevel.EnableAllWarning)
            {
                cbWarningLevel.SelectedIndex = 1;
            }
            else if (m_builder.WarningLevel == WarningLevel.SyntaxCheckOnly)
            {
                cbWarningLevel.SelectedIndex = 2;
            }
            else if (m_builder.WarningLevel == WarningLevel.TreatAllWarningAsError)
            {
                cbWarningLevel.SelectedIndex = 3;
            }
        }
        private void updateInstrumentation()
        {
            if (m_builder.IsCodeInstrumented)
            {
                chIsInstrumented.Checked = true;
            }
            else
            {
                chIsInstrumented.Checked = false;
            }
        }
        private void updateApplicationType()
        {
            if (m_builder.OutputType == OutputTypes.ConsoleApplication)
            {
                cbOutputType.SelectedIndex = 0;
            }
            else
            {
                cbOutputType.SelectedIndex = 1;
            }
            
        }
        private void updateBuildDirectory()
        {
            txtBuildDirectory.Text = m_builder.BuildDirectory;
        }
        private void updateCompilorPath()
        {
            txtCompilorPath.Text = m_builder.CompilorPath;
        }
        private void ProjectbuilderUi_Load(object sender, EventArgs e)
        {

           
            updateCompilorPath();
            updateBuildDirectory();
            updateApplicationType();
            updateInstrumentation();
            updateWarningLevel();

            m_builder.PropertyChanged+=new PropertyChangedEventHandler(Builder_PropertyChanged);
            m_builder.Owner.evProjectStatus +=new onProjectStatus(Owner_evProjectStatus);
            
        }
        void Owner_evProjectStatus(ProjectStatus status, object data)
        {
            try
            {
                switch (status)
                {
                    case ProjectStatus.OPEN:
                        btnBuild.Enabled = true;
                        tabSettings.Enabled = true;
                        m_builder.CompilorPath = "g++";
                        readBuilderSettings();
                        m_builder.BuildDirectory = Path.GetDirectoryName(m_builder.Owner.ProjectData.ProjectPath) + "\\Build";
                        break;
                    case ProjectStatus.NEW:
                        btnBuild.Enabled = true;
                        tabSettings.Enabled = true;
                        m_builder.CompilorPath = "g++";
                        m_builder.BuildDirectory = Path.GetDirectoryName(m_builder.Owner.ProjectData.ProjectPath) + "\\Build";

                        break;
                    case ProjectStatus.CLOSE:
                        btnBuild.Enabled = false;
                        tabSettings.Enabled = false;
                        writeBuilderSettings();
                        break;
                    case ProjectStatus.SAVE:
                        writeBuilderSettings();
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }
        }
        public static string hasAttribute(XElement element, string attribute, XNamespace xns)
        {
            string result = "";
            result = element.Attribute(xns + attribute) != null ? element.Attribute(xns + attribute).Value.ToString() : "";
            return result;
        }
        private void readBuilderSettings()
        {
            if (File.Exists("Projectbuilder.xml"))
            {
                try
                {
                    XDocument doc = XDocument.Load("Projectbuilder.xml");
                    IEnumerable<XElement> BuildDirectories = from files in doc.Root.Descendants("BuildDirectory")
                                                            select files;
                    IEnumerable<XElement> CompilerPaths = from files in doc.Root.Descendants("CompilerPath")
                                                             select files;
                    IEnumerable<XElement> InstrumentationStatus = from files in doc.Root.Descendants("InstrumentationStatus")
                                                          select files;
                    IEnumerable<XElement> WarningLevels = from files in doc.Root.Descendants("WarningLevel")
                                                                  select files;
                    foreach (XElement dir in BuildDirectories)
                    {
                        string Path = hasAttribute(dir, "Path", "");
                        if (string.IsNullOrWhiteSpace(Path) == false)
                        {
                            if (Directory.Exists(Path))
                            {
                                m_builder.BuildDirectory = Path;
                            }
                            else
                            {
                                Directory.CreateDirectory(Path);
                                m_builder.BuildDirectory = Path;

                            }

                        }
                    }
                    foreach (XElement dir in CompilerPaths)
                    {
                        string Path = hasAttribute(dir, "Path", "");
                        if (string.IsNullOrWhiteSpace(Path) == false)
                        {
                            if (File.Exists(Path))
                            {
                                m_builder.CompilorPath = Path;
                            }
                         

                        }
                    }
                    foreach (XElement dir in InstrumentationStatus)
                    {
                        string Path = hasAttribute(dir, "status", "");
                        if (string.IsNullOrWhiteSpace(Path) == false)
                        {
                            if (Path == "TRUE")
                            {
                                m_builder.IsCodeInstrumented = true;
                            }
                            else
                            {
                                m_builder.IsCodeInstrumented = false;
                            }

                        }
                    }
                    foreach (XElement dir in WarningLevels)
                    {
                        string Path = hasAttribute(dir, "Level", "");
                        if (string.IsNullOrWhiteSpace(Path) == false)
                        {
                            if (Path == "TreatAllWarningAsError")
                            {
                                m_builder.WarningLevel = WarningLevel.TreatAllWarningAsError;
                            }
                            else if (Path == "SyntaxCheckOnly")
                            {
                                m_builder.WarningLevel = WarningLevel.SyntaxCheckOnly;
                            }
                            else if (Path == "EnableAllWarning")
                            {
                                m_builder.WarningLevel = WarningLevel.EnableAllWarning;
                            }
                            else if (Path == "AbortCompilationOnFirstError")
                            {
                                m_builder.WarningLevel = WarningLevel.AbortCompilationOnFirstError;
                            }

                        }
                    }

                }
                catch
                {

                }
            }
        }
        private void writeBuilderSettings()
        {
            try
            {
                if (m_builder != null)
                {
                    var settings = new XmlWriterSettings();
                    settings.OmitXmlDeclaration = true;
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;
                    settings.CloseOutput = true;
                    using (XmlWriter writer = XmlWriter.Create("Projectbuilder.xml", settings))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("ProjectSettings");
                        writer.WriteStartElement("BuildDirectory");
                        writer.WriteAttributeString("Path", m_builder.BuildDirectory);
                        writer.WriteEndElement();//BuildDirectory
                        writer.WriteStartElement("CompilerPath");
                        writer.WriteAttributeString("Path", m_builder.CompilorPath);
                        writer.WriteEndElement();//CompilerPath
                        writer.WriteStartElement("InstrumentationStatus");
                        if (m_builder.IsCodeInstrumented)
                        {
                            writer.WriteAttributeString("status", "TRUE");
                        }
                        else
                        {
                            writer.WriteAttributeString("status", "FALSE");
                        }
                        writer.WriteEndElement();//InstrumentationStatus
                        writer.WriteStartElement("WarningLevel");
                        if (m_builder.WarningLevel == WarningLevel.TreatAllWarningAsError)
                        {
                            writer.WriteAttributeString("Level", "TreatAllWarningAsError");
                        }
                        else if (m_builder.WarningLevel == WarningLevel.SyntaxCheckOnly)
                        {
                            writer.WriteAttributeString("Level", "SyntaxCheckOnly");
                        }
                        else if (m_builder.WarningLevel == WarningLevel.EnableAllWarning)
                        {
                            writer.WriteAttributeString("Level", "EnableAllWarning");
                        }
                        else if (m_builder.WarningLevel == WarningLevel.AbortCompilationOnFirstError)
                        {
                            writer.WriteAttributeString("Level", "AbortCompilationOnFirstError");
                        }
                        writer.WriteEndElement();//WarningLevel
                        writer.WriteEndElement();//ProjectSettings
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
        public void setStatus(string label)
        {
            if (lblStatus.InvokeRequired)
            {
                  txtConsole.Invoke((MethodInvoker)delegate
                  { lblStatus.Text = label; });

            }
            else
            {
                lblStatus.Text = label;
            }
            
        }
        public void setMaxProgress(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    progressBar.Maximum = value;
                });

            }
            else
            {
                progressBar.Maximum = value;
            }
        }
        public void updateProgress(int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (value <= progressBar.Maximum)
                    {
                        progressBar.Value = value;
                    }
                    else
                    {
                        progressBar.Value = progressBar.Maximum;
                    }
                });

            }
            else
            {
                if (value < progressBar.Maximum)
                {
                    progressBar.Value = value;
                }
                else
                {
                    progressBar.Value = progressBar.Maximum;
                }
            }
        }
        public void EnableRunBtn(bool enable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                { btnBuild.Enabled = enable; });

            }
            else
            {
                btnBuild.Enabled = enable;
            }
           
        }
        void Builder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "OutputType":
                    updateApplicationType();
                    break;
                case "WarningLevel":
                    updateWarningLevel();
                    break;
                case "IsCodeInstrumented":
                    updateInstrumentation();
                    break;
                case "CompilorPath":
                    updateCompilorPath();
                    break;
                case "BuildDirectory":
                    updateBuildDirectory();
                    break;
                case "ComplorLine":
                    ConsoleUi_UpdateText();
                    break;
                default:
                    break;
            }
        }
        public void ConsoleUi_UpdateText()
        {

            if (this.txtConsole.InvokeRequired)
            {
                txtConsole.Invoke((MethodInvoker)delegate
                { ConsoleUi_UpdateConsole(); });

            }
            else
            {
                ConsoleUi_UpdateConsole();
            }
        }
        /// <summary>
        /// Update the Condsole Text
        /// </summary>
        /// <param name="mode"></param>
        private void ConsoleUi_UpdateConsole()
        {
            ListOfConsoleData l_list = new ListOfConsoleData();
            txtConsole.Text = "";
            
            l_list.AddRange(m_builder.CompilorOutput);
            
            if (l_list != null)
            {


                foreach (string listElement in l_list)
                {
                    try
                    {
                        txtConsole.AppendText(listElement);
                        txtConsole.AppendText("\n");
                        Application.DoEvents();
                    }
                    catch
                    {

                    }

                }
                //txtConsole.SelectionStart = txtConsole.Text.Length; //Set the current caret position at the end
                //txtConsole.ScrollToCaret();

            }
            else
            {
                txtConsole.Text = "";
            }
        }
        private void btnCompilorBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_builder.CompilorPath = dlg.FileName;
            }
        }

        private void btnBuildDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = m_builder.Owner.ProjectData.ProjectPath;
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_builder.BuildDirectory = dlg.SelectedPath;
            }

        }

        private void cbOutputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbOutputType.SelectedIndex == 0)
            {
                m_builder.OutputType = OutputTypes.ConsoleApplication;
            }
            else
            {
                m_builder.OutputType = OutputTypes.SharedLibrary;
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            btnBuild.Enabled = false;
  
            m_builder.clearCompilorOutput();
            m_builder.buildProject();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
