using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Gunit.Model;
using System.Xml.Serialization;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ICSharpCode.AvalonEdit.Document;
using System.Windows.Forms;
using System.Reflection;
using Gunit.Interfaces;
using System.ComponentModel;
using ICSharpCode.AvalonEdit;
using System.Windows.Threading;
using MahApps.Metro;
using Gunit.Utils;
namespace Gunit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
       
        ProjectViewModel model = null;
        public ProjectViewModel Model
        {
            get { return model; }
        }
       
        public delegate void ReadyToShowDelegate(object sender, EventArgs args);
        public event ReadyToShowDelegate ReadyToShow;
        private DispatcherTimer timer;
        View.CodeEditor txtCode ;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(7);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            model = new ProjectViewModel();
            txtCode = new View.CodeEditor(this);
            //this.Colors = typeof(Colors)
            // .GetProperties()
            // .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
            // .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null, null)))
            // .ToList();

            //var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);
            //ThemeManager.ChangeAppStyle(this, theme.Item2, theme.Item1);
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (ReadyToShow != null)
            {
                ReadyToShow(this, null);
            }
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //allow to load the plugins only when a valid project is active
            btnPlugin.IsEnabled = false;
            
        }
        void model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedLine")
            {
                txtCode.HighlightLine(Colors.LightCoral, model.SelectedLine);
            }
        }
        /// <summary>
        /// In order to have backward compatability the new GUnit allows to load the old project file
        /// </summary>
        /// <param name="fileName"> Path to the old project file</param>
        private void loadOldProjectFile(string fileName)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Project));
                TextReader reader = new StreamReader(fileName);
                Project objectProject = (Project)deserializer.Deserialize(reader);
                objectProject.ProjectPath = fileName;
                this.model = new ProjectViewModel(objectProject);
                model.PropertyChanged += new PropertyChangedEventHandler(model_PropertyChanged);
                resetPluginView();

                DataContext = model;
                reader.Close();
                btnPlugin.IsEnabled = true;
            }
            catch (Exception err)
            {
                System.Windows.MessageBox.Show(err.ToString());
                
            }
        }
      
        /// <summary>
        /// Function to save a project
        /// </summary>
        public void SaveProject()
        {
            if (string.IsNullOrWhiteSpace(model.ProjectPath) == false)
            {
                XmlSerializer SerializerObj = new XmlSerializer(typeof(ProjectViewModel));
                TextWriter WriteFileStream = new StreamWriter(model.ProjectPath, false);
                ProjectViewModel modelClone = (ProjectViewModel)model.Clone();
                modelClone.modifyPathsinModel(ProjectState.SAVE, System.IO.Path.GetDirectoryName(model.ProjectPath));
                SerializerObj.Serialize(WriteFileStream, modelClone);
                WriteFileStream.Close();
                modelClone.modifyPathsinModel(ProjectState.OPEN, System.IO.Path.GetDirectoryName(model.ProjectPath));

               
            }
            if (System.IO.File.Exists(model.SelectedFile))
            {
                
                this.txtCode.Save(model.SelectedFile);
            }
            foreach (IGunitPlugin plugin in model.PluginDlls)
            {
               plugin.Save();
            }
            model.State = ProjectState.SAVE;
           

        }
        
        /// <summary>
        /// Function to create a new project
        /// </summary>
        public void NewProject()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Gunit Project File (*.xml)|*.xml";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.model = new ProjectViewModel();
                model.ProjectPath = dlg.FileName;
                model.Name = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName);
                SaveProject();
                DataContext = model;//assinging data context to the View
                model.State = ProjectState.NEW;//project state is New Project
                resetPluginView();
                btnPlugin.IsEnabled = true;
            }
            else
            {
                //Do nothing
            }
        }

        private void openProject(string path)
        {
            try
            {
                //try serialising the Project XML file to the new Project Model. If it fails that means 
                //it is old project configuration
                XmlSerializer deserializer = new XmlSerializer(typeof(ProjectViewModel));
                TextReader reader = new StreamReader(path);
                this.model = (ProjectViewModel)deserializer.Deserialize(reader);
                model.PropertyChanged += new PropertyChangedEventHandler(model_PropertyChanged);
                model.modifyPathsinModel(ProjectState.OPEN, System.IO.Path.GetDirectoryName(path));
                reader.Close();
                resetPluginView();
                DataContext = model;
                model.ProjectPath = path;
                //project state is canged to open project
                model.State = ProjectState.OPEN;

                //load all the plugins configured for the project
                foreach (string dllPath in model.Plugins)
                {
                    if (System.IO.File.Exists(dllPath))
                    {
                        loadPlugin(dllPath);
                    }
                }
                btnPlugin.IsEnabled = true;
            }
            catch (Exception err)
            {
                loadOldProjectFile(path);
                Console.WriteLine(err.ToString());
            }
        }
        /// <summary>
        /// Open an existing project
        /// </summary>
        public void OpenProject()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Gunit Project File (*.xml)|*.xml";
            
            DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                openProject(dlg.FileName);
                
                //try
                //{
                //    //try serialising the Project XML file to the new Project Model. If it fails that means 
                //    //it is old project configuration
                //    XmlSerializer deserializer = new XmlSerializer(typeof(ProjectViewModel));
                //    TextReader reader = new StreamReader(dlg.FileName);
                //    this.model = (ProjectViewModel)deserializer.Deserialize(reader);
                //    model.modifyPathsinModel(ProjectState.OPEN, System.IO.Path.GetDirectoryName(dlg.FileName));
                //    reader.Close();
                //    resetPluginView();
                //    DataContext = model;
                //    model.ProjectPath = dlg.FileName;
                //   //project state is canged to open project
                //    model.State = ProjectState.OPEN;
                   
                //    //load all the plugins configured for the project
                //    foreach (string dllPath in model.Plugins)
                //    {
                //        if (System.IO.File.Exists(dllPath))
                //        {
                //            loadPlugin(dllPath);
                //        }
                //    }
                //    btnPlugin.IsEnabled = true;
                //}
                //catch(Exception err)
                //{
                //    loadOldProjectFile(dlg.FileName);
                //    Console.WriteLine(err.ToString());
                //}
            }



        }

        private void resetPluginView()
        {
            model.resetMainContentControl();
            model.addMainContentControl(txtCode);
           
        }

        /// <summary>
        /// Load the plugin Dll and add the User control from the Plugin to Main Content Flipview
        /// </summary>
        /// <param name="path"> path to the Plugin dll</param>
        private void loadPlugin(string path)
        {
            try
            {
                var DLL = Assembly.LoadFile(path);
                //find all Exposed types of IGUnitPlugin
                var results = from type in DLL.GetTypes()
                              where typeof(IGunitPlugin).IsAssignableFrom(type)
                              select type;
                foreach (Type type in results)
                {
                    if (model.Plugins.Contains(path) == false)
                    {
                        model.Plugins.Add(path);
                    }
                    IGunitPlugin plugin = (IGunitPlugin)Activator.CreateInstance(type);
                    model.PluginDlls.Add(plugin);
                    plugin.Init(model);
                    model.addMainContentControl(plugin.getView());
                    model.ConsoleLog += plugin.PluginName;

                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// load plugin 
        /// </summary>
        public void loadPlugin()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Gunit plugin dll(*.dll)|*.dll";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                loadPlugin(dlg.FileName);
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewProject();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenProject();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveProject();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }

        private void btnPlugin_Click(object sender, RoutedEventArgs e)
        {
            loadPlugin();
        }

        private void tbPlugins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipview = ((FlipView)sender);
            if (flipview.SelectedIndex == 0)
            {
                tbPlugins.BannerText = "Code Editor";
            }
            else
            {
                if (flipview.SelectedIndex <= model.PluginDlls.Count)
                {
                    tbPlugins.BannerText = model.PluginDlls[flipview.SelectedIndex - 1].PluginName;
                }
            }

        }
        private static Action EmptyDelegate = delegate() { };
      

        private void btnTheme_Click(object sender, RoutedEventArgs e)
        {
            Gunit.View.ChangeTheme theme = new View.ChangeTheme();
            theme.Show();
        }







      
    }
}
