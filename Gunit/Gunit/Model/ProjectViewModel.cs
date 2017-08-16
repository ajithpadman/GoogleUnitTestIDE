using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Gunit.Model;
using System.Windows.Forms;
using System.Xml.Serialization;
using ICSharpCode.AvalonEdit.Document;
using System.IO;
using ICSharpCode.AvalonEdit.Utils;
using ICSharpCode.AvalonEdit.Highlighting;
using Gunit.Utils;
using Gunit.View;
using Gunit.Interfaces;
namespace Gunit.Model
{
    public enum LanguageType
    {
        CPPSourceFile,
        HeaderFile,
        HPPFile,
        OtherFile
    }


    [Serializable()]
    public class ProjectViewModel : ViewModelBase, IProjectModel, ICloneable
    {
      
        public ProjectViewModel(Project project)
        {
            initialise();
            Name = project.Name;
            ProjectPath = project.ProjectPath;
            if (null != project.Files)
            {
                if (null != project.Files.HeaderFiles)
                {
                    foreach (Gunit.Interfaces.File path in project.Files.HeaderFiles)
                    {
                        var s = Path.Combine(Path.GetDirectoryName(ProjectPath), path.Path);
                        s = Path.GetFullPath(s);

                        HeaderFiles.Add(s);
                    }
                }
            }
            if (null != project.Files)
            {
                if (null != project.Files.SourceFiles)
                {
                    foreach (Gunit.Interfaces.File path in project.Files.SourceFiles)
                    {
                        var s = Path.Combine(Path.GetDirectoryName(ProjectPath), path.Path);
                        s = Path.GetFullPath(s);

                        SourceFiles.Add(s);
                    }
                }
            }
            if (null != project.ProjectHeaders)
            {
                foreach (Gunit.Interfaces.Include path in project.ProjectHeaders)
                {
                    var s = Path.Combine(Path.GetDirectoryName(ProjectPath), path.Path);
                    s = Path.GetFullPath(s);

                    PreHeaderFiles.Add(s);
                }
            }
            if (null != project.IncludePaths)
            {
                foreach (Gunit.Interfaces.Include path in project.IncludePaths)
                {
                    var s = Path.Combine(Path.GetDirectoryName(ProjectPath), path.Path);
                    s = Path.GetFullPath(s);

                    IncludePaths.Add(s);
                }
            }
            if (null != project.LibPaths)
            {
                foreach (Gunit.Interfaces.Lib path in project.LibPaths)
                {
                    var s = Path.Combine(Path.GetDirectoryName(ProjectPath), path.Path);
                    s = Path.GetFullPath(s);

                    LibraryPaths.Add(s);
                }
            }
            if (null != project.LibNames)
            {
                foreach (Gunit.Interfaces.LibName path in project.LibNames)
                {

                    LibNames.Add(path.Path);
                }
            }
            if (project.Defines != null)
            {
                foreach (Gunit.Interfaces.Define path in project.Defines)
                {

                    Defines.Add(path.value);
                }
            }
            if (project.Plugins != null)
            {
                foreach (Gunit.Interfaces.Include path in project.Plugins)
                {
                    var s = Path.Combine(Path.GetDirectoryName(ProjectPath), path.Path);
                    s = Path.GetFullPath(s);
                    Plugins.Add(s);
                }
            }
            this.BinaryPath = project.Bin.Path;
            
        }
        public void modifyPathsinModel( ProjectState Nextstate,string rootPath)
        {
           
            if (Nextstate == ProjectState.OPEN)
            {
                var projPath = Path.Combine(rootPath, ProjectPath);
                projPath = Path.GetFullPath(projPath);
                ProjectPath = projPath;
                for (int i = 0; i < HeaderFiles.Count;i++ )
                {
                    var s = Path.Combine(rootPath, HeaderFiles[i]);
                    s = Path.GetFullPath(s);
                    HeaderFiles[i] = s;

                }
                for (int i = 0; i < SourceFiles.Count; i++)
                {
                    var s = Path.Combine(rootPath, SourceFiles[i]);
                    s = Path.GetFullPath(s);
                    SourceFiles[i] = s;

                }
                for (int i = 0; i < PreHeaderFiles.Count; i++)
                {
                    var s = Path.Combine(rootPath, PreHeaderFiles[i]);
                    s = Path.GetFullPath(s);
                    PreHeaderFiles[i] = s;

                }
                for (int i = 0; i < IncludePaths.Count; i++)
                {
                    var s = Path.Combine(rootPath, IncludePaths[i]);
                    s = Path.GetFullPath(s);
                    IncludePaths[i] = s;

                }
                for (int i = 0; i < LibraryPaths.Count; i++)
                {
                    var s = Path.Combine(rootPath, LibraryPaths[i]);
                    s = Path.GetFullPath(s);
                    LibraryPaths[i] = s;

                }
                for (int i = 0; i < Plugins.Count; i++)
                {
                    var s = Path.Combine(rootPath, Plugins[i]);
                    s = Path.GetFullPath(s);
                    Plugins[i] = s;

                }
                
                
            }
            else if (Nextstate == ProjectState.SAVE)
            {
                ProjectPath = IOUtils.getReleativePathWith(ProjectPath, rootPath);
                for (int i = 0; i < HeaderFiles.Count; i++)
                {
                    var s = IOUtils.getReleativePathWith(HeaderFiles[i], rootPath);
                   
                    HeaderFiles[i] = s;

                }
                for (int i = 0; i < SourceFiles.Count; i++)
                {
                    var s = IOUtils.getReleativePathWith(SourceFiles[i], rootPath);
                    SourceFiles[i] = s;

                }
                for (int i = 0; i < PreHeaderFiles.Count; i++)
                {
                    var s = IOUtils.getReleativePathWith(PreHeaderFiles[i], rootPath);
                    PreHeaderFiles[i] = s;

                }
                for (int i = 0; i < IncludePaths.Count; i++)
                {
                    var s = IOUtils.getReleativePathWith(IncludePaths[i], rootPath);
                    IncludePaths[i] = s;

                }
                for (int i = 0; i < LibraryPaths.Count; i++)
                {
                    var s = IOUtils.getReleativePathWith(LibraryPaths[i], rootPath);
                    LibraryPaths[i] = s;

                }
                for (int i = 0; i < Plugins.Count; i++)
                {
                    var s = IOUtils.getReleativePathWith(Plugins[i], rootPath);
                    Plugins[i] = s;

                }
            }
            else
            {

            }
        }
        public ProjectViewModel()
        {
            initialise();
        

        }
        private void initialise()
        {
            SelectedIncludePath = -1;
            SelectedLibraryPath = -1;
            Name = "";
            ProjectPath = "";
            m_InludePaths = new ObservableCollection<string>();
            m_LibPaths = new ObservableCollection<string>();
            m_LibNames = new ObservableCollection<string>();
            m_Defines = new ObservableCollection<string>();
            m_SourceFiles = new ObservableCollection<string>();
            m_HeaderFiles = new ObservableCollection<string>();
            m_PreHeaderFiles = new ObservableCollection<string>();
            m_CompilerArguments = new ObservableCollection<string>();
            m_PluginPaths = new ObservableCollection<string>();
            m_Plugins = new ObservableCollection<IGunitPlugin>();
            m_mainContent = new ObservableCollection<System.Windows.Controls.UserControl>();

            OpenFileCommand = new DelegateCommand(OpenFile);
            m_AddSourceFileCommand = new DelegateCommand(AddSourceFile);
            m_AddHeaderFileCommand = new DelegateCommand(AddHeaderFile);
            m_AddPreHeaderFileCommand = new DelegateCommand(AddPreIncludeFile);
            AddNewSourceFileCommand = new DelegateCommand(AddNewSourceFile);
            AddNewHeaderFileCommand = new DelegateCommand(AddNewHeaderFile);
            AddNewPreHeaderFileCommand = new DelegateCommand(AddNewPreHeaderFile);
            m_settingModel = new ProjectSettingModel(this);
            AddIncludePaths = new DelegateCommand(m_settingModel.addIncludePaths);
            AddLibraryPaths = new DelegateCommand(m_settingModel.addLibPaths);
            RemoveIncludePaths = new DelegateCommand(m_settingModel.RemoveIncludePath);
            RemoveLibraryPaths = new DelegateCommand(m_settingModel.RemoveLibPath);
            AddLibNames = new DelegateCommand(m_settingModel.AddLibName);
            RemoveLibNames = new DelegateCommand(m_settingModel.RemoveLibName);
            AddDefines = new DelegateCommand(m_settingModel.AddMacro);
            RemoveDefines = new DelegateCommand(m_settingModel.RemoveMacro);
           
        }
        ObservableCollection<string> m_InludePaths;
        ObservableCollection<string> m_LibPaths;
        ObservableCollection<string> m_LibNames;
        ObservableCollection<string> m_Defines;
        ObservableCollection<string> m_SourceFiles;
        ObservableCollection<string> m_HeaderFiles;
        ObservableCollection<string> m_PreHeaderFiles;
        ObservableCollection<string> m_CompilerArguments;
        ObservableCollection<string> m_PluginPaths;
        [XmlIgnore]
        ObservableCollection<System.Windows.Controls.UserControl> m_mainContent;
        [XmlIgnore]
        public ObservableCollection<System.Windows.Controls.UserControl> MainContentControls
        {
            get { return m_mainContent; }
        }
        public void addMainContentControl(System.Windows.Controls.UserControl control)
        {
            MainContentControls.Add(control);
            OnPropertyChanged("MainContentControls");
        }
        public void resetMainContentControl()
        {
            m_mainContent = new ObservableCollection<System.Windows.Controls.UserControl>();
            OnPropertyChanged("MainContentControls");
        }
        [XmlIgnore]
        ObservableCollection<IGunitPlugin> m_Plugins;
        [XmlIgnore]
        public ObservableCollection<IGunitPlugin> PluginDlls { get { return m_Plugins; } }

        [XmlIgnore]
        ProjectState m_state = ProjectState.INIT;
        [XmlIgnore]
        public ProjectState State
        {
            get { return m_state; }
            set
            {
                m_state = value;
               // isProjectOpen = (m_state == ProjectState.OPEN || m_state == ProjectState.NEW);
              //  OnPropertyChanged("State");
            }
        }
        [XmlIgnore]
        ProjectSettingModel m_settingModel;
        [XmlIgnore]
        private bool _isDirty = false;
        [XmlIgnore]
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    OnPropertyChanged("IsDirty");
                    OnPropertyChanged("FileName");
                    OnPropertyChanged("Title");
                }
            }
        }
        private TextDocument _document = new TextDocument();
        [XmlIgnore]
        public TextDocument Document
        {
            get { return this._document; }
            set
            {
                if (this._document != value)
                {
                    this._document = value;
                    OnPropertyChanged("Document");
                    IsDirty = true;
                }
            }
        }
        [XmlIgnore]
        int m_IncludePath = 0;
        [XmlIgnore]
        public int SelectedIncludePath { get { return m_IncludePath; } set { m_IncludePath = value; OnPropertyChanged("IncludePath"); } }
        [XmlIgnore]
        int m_LibPath = 0;
        [XmlIgnore]
        public int SelectedLibraryPath { get { return m_LibPath; } set { m_LibPath = value; OnPropertyChanged("LibPath"); } }

        [XmlIgnore]
        int m_LibName = 0;
        [XmlIgnore]
        public int SelectedLibraryName { get { return m_LibName; } set { m_LibName = value; OnPropertyChanged("LibName"); } }

        [XmlIgnore]
        int m_Macro = 0;
        [XmlIgnore]
        public int SelectedDefine { get { return m_Macro; } set { m_Macro = value; OnPropertyChanged("Defines"); } }

        [XmlIgnore]
        private IHighlightingDefinition _highlightdef = null;
        [XmlIgnore]
        public IHighlightingDefinition HighlightDef
        {
            get { return this._highlightdef; }
            set
            {
                if (this._highlightdef != value)
                {
                    this._highlightdef = value;
                    OnPropertyChanged("HighlightDef");
                    IsDirty = true;
                }
            }
        }
        [XmlIgnore]
        string m_Console;
        [XmlIgnore]
        public string ConsoleLog
        {
            get
            {
                return m_Console;
            }
            set
            {
                m_Console = value + "\n";
                OnPropertyChanged("ConsoleOutput");
            }
        }

        [XmlIgnore]
        bool m_IsProjOpen;
        [XmlIgnore]
        public bool isProjectOpen
        {
            get { return m_IsProjOpen; }
            set { m_IsProjOpen = value; OnPropertyChanged("EnableChange"); }
        }
        [XmlIgnore]
        int _SelectedLine = -1;
        [XmlIgnore]
        public int SelectedLine
        {
            get { return _SelectedLine; }
            set { _SelectedLine = value; OnPropertyChanged("SelectedLine"); }
        }

        [XmlIgnore]
        private string _filePath = null;
        [XmlIgnore]
        public string SelectedFile
        {
            get { return _filePath; }
            set
            {
                //     if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged("SelectedFile");
                    OnPropertyChanged("FileName");
                    OnPropertyChanged("Title");

                    if (System.IO.File.Exists(_filePath))
                    {
                        this._document = new TextDocument();
                        this.HighlightDef = HighlightingManager.Instance.GetDefinition("C++");

                        using (FileStream fs = new FileStream(this._filePath,
                                                   FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            using (StreamReader reader = FileReader.OpenStream(fs, Encoding.UTF8))
                            {
                                this.Document = new TextDocument(reader.ReadToEnd());
                            }
                        }


                    }
                }
            }
        }
        public string Name { get; set; }
        public string ProjectPath { get; set; }
        public string BinaryPath { get; set; }

        public ObservableCollection<string> IncludePaths { get { return m_InludePaths; } }
        public ObservableCollection<string> LibraryPaths { get { return m_LibPaths; } }
        public ObservableCollection<string> LibNames { get { return m_LibNames; } }
        public ObservableCollection<string> Defines { get { return m_Defines; } }
        public ObservableCollection<string> SourceFiles { get { return m_SourceFiles; } }

        public ObservableCollection<string> HeaderFiles { get { return m_HeaderFiles; } }
        public ObservableCollection<string> PreHeaderFiles { get { return m_PreHeaderFiles; } }
        public ObservableCollection<string> CompilerArguments { get { return m_CompilerArguments; } }
        public ObservableCollection<string> Plugins { get { return m_PluginPaths; } }

        #region Commands



        [XmlIgnore]
        ICommand m_OpenFileCommand;
        [XmlIgnore]
        public ICommand OpenFileCommand { get { return m_OpenFileCommand; } set { m_OpenFileCommand = value; } }

        [XmlIgnore]
        ICommand m_AddSourceFileCommand;
        [XmlIgnore]
        public ICommand AddSourceFileCommand { get { return m_AddSourceFileCommand; } set { m_AddSourceFileCommand = value; } }

        [XmlIgnore]
        ICommand m_AddNewSourceFileCommand;
        [XmlIgnore]
        public ICommand AddNewSourceFileCommand { get { return m_AddNewSourceFileCommand; } set { m_AddNewSourceFileCommand = value; } }



        [XmlIgnore]
        ICommand m_AddHeaderFileCommand;
        [XmlIgnore]
        public ICommand AddHeaderFileCommand { get { return m_AddHeaderFileCommand; } set { m_AddHeaderFileCommand = value; } }

        [XmlIgnore]
        ICommand m_AddNewHeaderFileCommand;
        [XmlIgnore]
        public ICommand AddNewHeaderFileCommand { get { return m_AddNewHeaderFileCommand; } set { m_AddNewHeaderFileCommand = value; } }



        [XmlIgnore]
        ICommand m_AddPreHeaderFileCommand;
        [XmlIgnore]
        public ICommand AddPreHeaderFileCommand { get { return m_AddPreHeaderFileCommand; } set { m_AddPreHeaderFileCommand = value; } }

        [XmlIgnore]
        ICommand m_AddNewPreHeaderFileCommand;
        [XmlIgnore]
        public ICommand AddNewPreHeaderFileCommand { get { return m_AddNewPreHeaderFileCommand; } set { m_AddNewPreHeaderFileCommand = value; } }

        [XmlIgnore]
        ICommand m_AddIncludePaths;
        [XmlIgnore]
        public ICommand AddIncludePaths { get { return m_AddIncludePaths; } set { m_AddIncludePaths = value; } }

        [XmlIgnore]
        ICommand m_AddLibraryPaths;
        [XmlIgnore]
        public ICommand AddLibraryPaths { get { return m_AddLibraryPaths; } set { m_AddLibraryPaths = value; } }

        [XmlIgnore]
        ICommand m_RemoveIncludePaths;
        [XmlIgnore]
        public ICommand RemoveIncludePaths { get { return m_RemoveIncludePaths; } set { m_RemoveIncludePaths = value; } }

        [XmlIgnore]
        ICommand m_RemoveLibraryPaths;
        [XmlIgnore]
        public ICommand RemoveLibraryPaths { get { return m_RemoveLibraryPaths; } set { m_RemoveLibraryPaths = value; } }

        [XmlIgnore]
        ICommand m_AddLibNames;
        [XmlIgnore]
        public ICommand AddLibNames { get { return m_AddLibNames; } set { m_AddLibNames = value; } }

        [XmlIgnore]
        ICommand m_RemoveLibNames;
        [XmlIgnore]
        public ICommand RemoveLibNames { get { return m_RemoveLibNames; } set { m_RemoveLibNames = value; } }

        [XmlIgnore]
        ICommand m_AddDefines;
        [XmlIgnore]
        public ICommand AddDefines { get { return m_AddDefines; } set { m_AddDefines = value; } }

        [XmlIgnore]
        ICommand m_RemoveDefines;
        [XmlIgnore]
        public ICommand RemoveDefines { get { return m_RemoveDefines; } set { m_RemoveDefines = value; } }





        #endregion

      
        public void OpenFile(object path)
        {
            if (path is string)
            {
                SelectedFile = path.ToString();
            }
        }

        public void AddNewSourceFile(object path)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "C Files (*.c)|*.c|CPP Files (*.cpp)|*.cpp|CC files (*.cc)|*.cc";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                StreamWriter writer = new StreamWriter(dlg.FileName);
                string fileName = Path.GetFileName(dlg.FileName);
                writer.WriteLine(CodeTemplates.writeFileHeader(fileName, "Source code for GUnit"));
                writer.Close();
                SourceFiles.Add(dlg.FileName);


            }
            else
            {
                //Do nothing
            }
        }
        public void AddSourceFile(object path)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "C Files (*.c)|*.c|CPP Files (*.cpp)|*.cpp|CC files (*.cc)|*.cc";
            dlg.Multiselect = true;
            DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in dlg.FileNames)
                {
                    SourceFiles.Add(file);


                }
            }

        }
        public void AddNewHeaderFile(object path)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Header Files (*.h)|*.h|(*.hpp)|*.hpp";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                StreamWriter writer = new StreamWriter(dlg.FileName);
                string fileName = Path.GetFileName(dlg.FileName);
                writer.WriteLine(CodeTemplates.writeFileHeader(fileName, "Header file for GUnit"));
                writer.Close();
                HeaderFiles.Add(dlg.FileName);


            }
            else
            {
                //Do nothing
            }
        }
        public void AddHeaderFile(object path)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Header Files (*.h)|*.h|(*.hpp)|*.hpp";
            dlg.Multiselect = true;
            DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in dlg.FileNames)
                {
                    HeaderFiles.Add(file);


                }
            }

        }
        public void AddNewPreHeaderFile(object path)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Header Files (*.h)|*.h|(*.hpp)|*.hpp";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {

                StreamWriter writer = new StreamWriter(dlg.FileName);
                string fileName = Path.GetFileName(dlg.FileName);
                writer.WriteLine(CodeTemplates.writeFileHeader(fileName, "Header file for GUnit"));
                writer.Close();
                PreHeaderFiles.Add(dlg.FileName);


            }
            else
            {
                //Do nothing
            }
        }
        public void AddPreIncludeFile(object path)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Header Files (*.h)|*.h|(*.hpp)|*.hpp";
            dlg.Multiselect = true;
            DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in dlg.FileNames)
                {
                    PreHeaderFiles.Add(file);


                }
            }

        }










        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
