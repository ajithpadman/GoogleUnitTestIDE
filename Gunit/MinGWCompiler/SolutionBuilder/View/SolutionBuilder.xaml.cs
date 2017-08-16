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
using MinGWCompiler.SolutionBuilder.ViewModel;
using System.Windows.Forms;
namespace MinGWCompiler.SolutionBuilder.View
{
    /// <summary>
    /// Interaction logic for SolutionBuilder.xaml
    /// </summary>
    public partial class SolutionBuilder : MahApps.Metro.Controls.MetroWindow
    {
      
        IProjectModel m_model;
        
        SolutionBuilderModel m_SolModel;
        public SolutionBuilder()
        {
            InitializeComponent();
        }

    

        public void Init(IProjectModel model,MinGWBuilder builder)
        {
            m_model = model;
            m_SolModel = new SolutionBuilderModel(model,builder);
            m_SolModel.evProcessComplete += SolnBuilder_evProcessComplete;
            cbSolutionType.ItemsSource = Enum.GetValues(typeof(SolutionType)).Cast<SolutionType>();
            DataContext = m_SolModel;
        }

       
        void SolnBuilder_evProcessComplete()
        {
            progressSolution.IsIndeterminate = false;
            m_SolModel.Status = "Solution Generated";
        }
       
       
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            m_SolModel.Status = "Generating Solution";
            progressSolution.IsIndeterminate = true;
            m_SolModel.createPremakeScript();
            m_SolModel.CreateSolution();
            
        }

       
     

        
    
    }
}
