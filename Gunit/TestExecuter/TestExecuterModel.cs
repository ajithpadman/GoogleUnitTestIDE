using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Interfaces;
using Gunit.Model;
using Gunit.Utils;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
namespace TestExecuter
{
   [Serializable]
    public class TestExecuterModel:ViewModelBase
    {
        string m_PathToExe;
        string m_PathtoGcov;
        string m_PathtoObjects;
        string m_pathToTestReport;
        string m_pathToCoverageReport;
        [XmlIgnore]
        ObservableCollection<ITestSuit> m_testSuits = new ObservableCollection<ITestSuit>();
        [XmlIgnore]
        ObservableCollection<ItestCase> m_SelectedTests = new ObservableCollection<ItestCase>();
        [XmlIgnore]
        ListOfConsoleData m_ConsoleOutput = new ListOfConsoleData();
        [XmlIgnore]
        IProjectModel m_HostModel;
        [XmlIgnore]
        bool m_isIndeterminate = false;
       [XmlIgnore]
        public bool IsIndeterminate
        {
            get { return m_isIndeterminate; }
            set
            {
                m_isIndeterminate = value;
                OnPropertyChanged("IsIndeterminate");
            }
        }
         public TestExecuterModel()
         {

        }
        public TestExecuterModel(IProjectModel model)
        {
            m_HostModel = model;
        }
       [XmlIgnore]
        public IProjectModel HostModel
        {
            get { return m_HostModel; }
            set {  m_HostModel = value; }
        }
       [XmlIgnore]
        public ListOfConsoleData Output
        {
            get { return m_ConsoleOutput; }
            set { m_ConsoleOutput = value; }
        }
        public string PathtoExecutable
        {
            get
            {
                return m_PathToExe;
            }
            set
            {
                m_PathToExe = value;
                OnPropertyChanged("PathtoExecutable");
            }
        }
        public string PathToGcov
        {
            get { return m_PathtoGcov; }
            set
            {
                m_PathtoGcov = value;
                OnPropertyChanged("PathToGcov");
            }

        }
        public string PathtoObjects
        {
            get { return m_PathtoObjects; }
            set
            {
                m_PathtoObjects = value;
                OnPropertyChanged("PathtoObjects");
            }
        }
        public string PathToTestReport
        {
            get { return m_pathToTestReport; }
            set 
            {
                m_pathToTestReport = value;
                OnPropertyChanged("PathToTestReport");
            }
        }
        public string PathToCoverageReport
        {
            get { return m_pathToCoverageReport; }
            set
            {
                m_pathToCoverageReport = value;
                OnPropertyChanged("PathToCoverageReport");
            }
        }
       [XmlIgnore]
        public ObservableCollection<ITestSuit> TestSuits
        {
            get { return m_testSuits; }
        }
       [XmlIgnore]
        public ObservableCollection<ItestCase> SelectedTests
        {
            get { return m_SelectedTests; }
        }
        public void AddTestSuit(ITestSuit suit)
        {
            TestSuits.Add(suit);
            OnPropertyChanged("TestSuits");
        }
        public void addSelectedTests(ItestCase test)
        {
            SelectedTests.Add(test);
            OnPropertyChanged("SelectedTests");
        }
       [XmlIgnore]
        int m_progress;
       [XmlIgnore]
        public int Progress
        {
            get { return m_progress; }
            set
            {
                m_progress = value;
                OnPropertyChanged("Progress");
            }
        }
       [XmlIgnore]
        int m_MaxValue;
       [XmlIgnore]
        public int MaxProgress
        {
            get { return m_MaxValue; }
            set
            {
                m_MaxValue = value;
                OnPropertyChanged("MaxProgress");
            }
        }

    }
}
