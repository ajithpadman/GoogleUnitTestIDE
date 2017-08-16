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
using Gunit.Interfaces;

using System.Windows.Forms;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using MahApps.Metro;
namespace MinGWCompiler
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MinGWCompiler : System.Windows.Controls.UserControl,IGunitPlugin
    {
        IProjectModel m_model;
        MinGWBuilder m_Builder;
       
        public MinGWCompiler()
        {
            InitializeComponent();
            
        }
        private void Serialise(string path)
        {
            XmlSerializer SerializerObj = new XmlSerializer(typeof(MinGWBuilder));
            TextWriter WriteFileStream = new StreamWriter(path, false);

            SerializerObj.Serialize(WriteFileStream, m_Builder);
            WriteFileStream.Close();
        }
        private void DeSerialise(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(MinGWBuilder));
            TextReader reader = new StreamReader(path);
            this.m_Builder = (MinGWBuilder)deserializer.Deserialize(reader);
            reader.Close();
        }
        private void btnBrowseGcc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_Builder.CompilorPath = dlg.FileName;
                
            }
        }

        private void btnBrowseBuild_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = m_model.ProjectPath;
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_Builder.BuildDirectory = dlg.SelectedPath;
            }
        }



        public string PluginName
        {
            get { return "MINGW Compiler Plugin"; }
        }

        public string Description
        {
            get { return "This plugin can be used to Compile the source code using MINGW"; }
        }

        public void Init(IProjectModel model)
        {
            m_model = model;
            try
            {
                if (System.IO.File.Exists("MINGWCompiler.xml"))
                {
                    DeSerialise("MINGWCompiler.xml");
                    m_Builder.HostModel = m_model;
                }
                else
                {
                    m_Builder = new MinGWBuilder(m_model);
                }
            }
            catch
            {
                m_Builder = new MinGWBuilder(m_model);
            }
            
            DataContext = m_Builder;
            cbOutputType.ItemsSource = Enum.GetValues(typeof(OutputTypes)).Cast<OutputTypes>();
            cbWarningLevel.ItemsSource = Enum.GetValues(typeof(WarningLevel)).Cast<WarningLevel>();
            
          
        }

        public void DeInit()
        {
            
        }
       
        

      


        System.Windows.Controls.UserControl IGunitPlugin.getView()
        {
            return this;
        }

       

        private void btnBuildCode_Click(object sender, RoutedEventArgs e)
        {
            m_Builder.buildProject();
        }

     

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            m_Builder.CompilorOutput.Clear();
        }


        public void Save()
        {
            try
            {
                Serialise("MINGWCompiler.xml");
            }
            catch
            {

            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SolutionBuilder.View.SolutionBuilder solun = new SolutionBuilder.View.SolutionBuilder();
            solun.Init(m_model,m_Builder);
            solun.Show();

        }


        public void ChangeTheme(string color)
        {
            var theme = ThemeManager.DetectAppStyle(System.Windows.Application.Current);
            ThemeManager.ChangeAppStyle(System.Windows.Application.Current, ThemeManager.GetAccent(("Orange")), theme.Item1);
        }
    }
}
