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
using System.Diagnostics;
using System.IO;
using System.Web.UI;
using System.Xml.Serialization;

namespace TestExecuter
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TestExecuter : System.Windows.Controls.UserControl, IGunitPlugin
    {
        IProjectModel m_model;
        TestExecuterModel m_testModel;
        
        IProcessHandler m_ProcessHandler = new Gunit.Utils.ExternalProcessHandler();
        public TestExecuter()
        {
            InitializeComponent();
        }

        public string PluginName
        {
            get { return "Test Executer"; }
        }
        private void DeSerialise(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(TestExecuterModel));
            TextReader reader = new StreamReader(path);
            this.m_testModel = (TestExecuterModel)deserializer.Deserialize(reader);
            reader.Close();
            ListTestSuits();
        }
        private void Serialise(string path)
        {
            XmlSerializer SerializerObj = new XmlSerializer(typeof(TestExecuterModel));
            TextWriter WriteFileStream = new StreamWriter(path, false);

            SerializerObj.Serialize(WriteFileStream, m_testModel);
            WriteFileStream.Close();
        }
        public string Description
        {
            get { return "This plugin can be used to Run the test Cases and to analyse the coverage of the test"; }
        }

        public void Init(IProjectModel model)
        {
            m_model = model;
            try
            {
                if (System.IO.File.Exists("TestExecuter.xml"))
                {
                    DeSerialise("TestExecuter.xml");
                    m_testModel.HostModel = m_model;
                }
                else
                {
                    m_testModel = new TestExecuterModel(m_model);
                }
            }
            catch
            {
                m_testModel = new TestExecuterModel(m_model);
            }
            
            DataContext = m_testModel;
            
        }

        public void DeInit()
        {
           
        }

        public System.Windows.Controls.UserControl getView()
        {
            return this;
        }

        private void btnBrowseExe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "EXE file (*.exe)|*.exe";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_testModel.PathtoExecutable = dlg.FileName;
                ListTestSuits();
            }
        }

        private void btnBrowseGcov_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "EXE file (*.exe)|*.exe";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_testModel.PathToGcov = dlg.FileName;
            }
        }

        private void btnBrowserObj_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_testModel.PathtoObjects = dlg.SelectedPath;
            }
        }

        private void btnBrowseTestReport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Test Report file (*.html)|*.html";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_testModel.PathToTestReport = dlg.FileName;
            }
        }

        private void btnBrowseCoverageReport_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();
           
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_testModel.PathToCoverageReport = dlg.SelectedPath;
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    row.DetailsVisibility = row.DetailsVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    break;
                }
        }



        private void btnRunAll_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(m_testModel.PathtoExecutable) == false)
            {
                RunAllTests();
            }
            
        }
        private void addTest(ItestCase test)
        {


            IJob job = new Gunit.Utils.Job();
            job.Command = m_testModel.PathtoExecutable;
            job.Argument = " --gtest_filter=" + test.Name;
            job.StdErrCallBack = StdErrCallBack;
            job.StdOutCallBack = StdOutCallBack;
            m_ProcessHandler.JobList.Add(job);



        }
        private void UpdateSelectedTests()
        {
            foreach (ITestSuit suit in m_testModel.TestSuits)
            {
                foreach(ItestCase test in suit.TestCases )
                {
                    if (test.IsSelected)
                    {
                        m_testModel.SelectedTests.Add(test);
                    }
                    
                }
            }
        }
        private void RunAllTests()
        {

            m_testModel.Output.Clear();
            m_testModel.MaxProgress = 0;
            m_testModel.Progress = 0;
            m_ProcessHandler.JobList.Clear();
            UpdateSelectedTests();
            foreach (ITestSuit suit in m_testModel.TestSuits)
            {
                foreach (ItestCase test in suit.TestCases)
                {
                    test.Status = TestStatus.NotRun;
                    test.ErrorString = "";
                }
            }
            if (m_testModel.SelectedTests.Count == 0)
            {
                foreach (ITestSuit suit in m_testModel.TestSuits)
                {
                    foreach (ItestCase test in suit.TestCases)
                    {
                        addTest(test);
                        m_testModel.addSelectedTests(test);

                    }
                }

            }
            else
            {
                foreach (TestCase test in m_testModel.SelectedTests)
                {
                    addTest(test);
                }


            }
            m_ProcessHandler.evProcessComplete -= (Processhandler_TestRunComplete);
            m_ProcessHandler.evProgress -= onProgress;
            m_ProcessHandler.evProcessComplete += (Processhandler_TestRunComplete);
            m_ProcessHandler.evProgress += onProgress;
            m_testModel.MaxProgress = m_ProcessHandler.JobList.Count;
            m_ProcessHandler.Start();


        }
        void onProgress(int index)
        {
            m_testModel.Progress = index;
        }
        public void generateReport(string fileName)
        {
            if (m_testModel != null)
            {

                StringWriter stringWriter = new StringWriter();
                using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Html);
                    writer.RenderBeginTag(HtmlTextWriterTag.Head);
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, System.IO.Path.GetFileNameWithoutExtension(fileName));
                    writer.RenderBeginTag(HtmlTextWriterTag.Title);
                    writer.RenderEndTag();//title
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text / css");
                    writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "stylesheet.css");
                    writer.RenderBeginTag(HtmlTextWriterTag.Link);
                    writer.RenderEndTag();//link
                    writer.RenderEndTag();//head
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#ECF6CE");
                    writer.RenderBeginTag(HtmlTextWriterTag.Body);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "MainHeading");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                    writer.RenderBeginTag(HtmlTextWriterTag.H1);
                    writer.WriteLine(System.IO.Path.GetFileNameWithoutExtension(fileName));
                    writer.RenderEndTag();//H1

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Information");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.WriteLine("Time:" + DateTime.UtcNow.Date.ToString());
                    writer.RenderEndTag();//H3

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Information");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.WriteLine("Project:" + m_model.Name);
                    writer.RenderEndTag();//H3

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "MainTable");
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "4px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("TestCase Name");
                    writer.RenderEndTag();//td TestCase Name

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Test Phase number");
                    writer.RenderEndTag();//td Test Phase number

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Description");
                    writer.RenderEndTag();//td Description

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Preconditions");
                    writer.RenderEndTag();//td Preconditions

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Test Steps");
                    writer.RenderEndTag();//td TestSteps

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Expected Result");
                    writer.RenderEndTag();//td Expected Result

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Actual Result");
                    writer.RenderEndTag();//td Actual Result 

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Comments");
                    writer.RenderEndTag();//td Comments
                    writer.RenderEndTag();//tr
                    foreach (ITestSuit suit in m_testModel.TestSuits)
                    {
                        foreach (ItestCase test in suit.TestCases)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.Name);
                            writer.RenderEndTag();//td test name


                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.TestPhaseNumber);
                            writer.RenderEndTag();//td TestPhaseNumber

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.TestDescription);
                            writer.RenderEndTag();//td TestDescription

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            foreach (string precondition in test.TestPrecondition)
                            {
                                writer.WriteLine(precondition);
                            }

                            writer.RenderEndTag();//td TestPrecondition

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            foreach (string step in test.TestSteps)
                            {
                                writer.WriteLine(step);
                            }

                            writer.RenderEndTag();//td TestSteps

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            foreach (string exp in test.ExpectedResult)
                            {
                                writer.WriteLine(exp);
                            }

                            writer.RenderEndTag();//td ExpectedResult


                            string status = "";
                            if (test.Status == TestStatus.Error)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Failure");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "red");
                                status = "FAILURE";
                            }
                            else if (test.Status == TestStatus.NotRun)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NotRun");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "yellow");
                                status = "NOT RUN";
                            }
                            else
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMaximum");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "green");
                                status = "SUCCESS";
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);

                            writer.WriteLine(status);
                            writer.RenderEndTag();//td Actual Result


                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.ErrorString);
                            writer.RenderEndTag();//td Comments
                            writer.RenderEndTag();//tr Table Row
                        }
                    }
                    writer.RenderEndTag();//thead
                    writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
                    writer.RenderEndTag();//Tbody
                    writer.RenderEndTag();//table
                    writer.RenderEndTag();//body

                    writer.RenderEndTag();//html

                }
                StreamWriter htmlFile = new StreamWriter(fileName);
                htmlFile.Write(stringWriter.ToString());
                htmlFile.Close();
            }

        }
        void Processhandler_TestRunComplete()
        {
            processTestResult();
            m_ProcessHandler.evProcessComplete -= (Processhandler_TestRunComplete);
            if (string.IsNullOrWhiteSpace(m_testModel.PathToTestReport) == false)
            {
                generateReport(m_testModel.PathToTestReport);
            }
            if (string.IsNullOrWhiteSpace(m_testModel.PathToCoverageReport) == false)
            {
                CoverageModel Coverage = new CoverageModel(m_testModel);
                Coverage.AnalyseCoverage();
            }
        }
        private void processTestResult()
        {
            bool l_started = false;
            foreach (ItestCase test in m_testModel.SelectedTests)
            {
                test.ExpectedResult.Clear();
                test.TestSteps.Clear();
                test.TestPrecondition.Clear();
                test.TestDescription = "";
                test.TestPhaseNumber = "Unknown";
            }
            foreach (ItestCase test in m_testModel.SelectedTests)
            {
                foreach (string line in m_testModel.Output)
                {
                    if (line.Contains(" RUN ") && line.Contains(test.Name))
                    {
                        l_started = true;
                    }

                    else if (line.Contains(" OK ") && line.Contains(test.Name))
                    {
                        test.Status = TestStatus.OK;
                        l_started = false;
                    }
                    else if (line.Contains(" FAILED ") && line.Contains(test.Name))
                    {
                        test.Status = TestStatus.Error;
                        l_started = false;
                    }
                    else if (l_started == true)
                    {
                        if (IsTestSpecification(line, test) == false)
                        {
                            test.ErrorString += line + "\n";
                        }
                    }
                }
            }
           
        }
        private bool IsTestSpecification(string line, ItestCase test)
        {
            string[] splitArray = { "@#" };
            string[] result;
            bool retval = false;
            if (line.Contains("@#TESTPHASENAME@#"))
            {
                result = line.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
                if (result.Length == 2)
                {
                    test.TestPhaseNumber = result[1];
                }
                else
                {
                    test.TestPhaseNumber = "Unknown";
                }
                retval = true;
            }
            else if (line.Contains("@#DESCRIPTION@#"))
            {
                result = line.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
                if (result.Length == 2)
                {
                    test.TestDescription = (result[1]);
                }
                else
                {
                    test.TestDescription = "";
                }
                retval = true;

            }
            else if (line.Contains("@#EXPECTATION@#"))
            {
                result = line.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
                if (result.Length == 2)
                {
                    test.ExpectedResult.Add(result[1]);
                }
                retval = true;

            }
            else if (line.Contains("@#PRECONDITION@#"))
            {
                result = line.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
                if (result.Length == 2)
                {
                    test.TestPrecondition.Add(result[1]);
                }

                retval = true;
            }
            else if (line.Contains("@#TESTSTEP@#"))
            {
                result = line.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
                if (result.Length == 2)
                {
                    test.TestSteps.Add(result[1]);
                }
                retval = true;
            }

            return retval;
        }
        private void ListTestSuits()
        {
            if (System.IO.File.Exists(m_testModel.PathtoExecutable))
            {
                m_testModel.Output.Clear();
                m_testModel.TestSuits.Clear();
                IJob job = new Gunit.Utils.Job();
                job.Command = m_testModel.PathtoExecutable;
                job.Argument = " --gtest_list_tests";
                job.StdErrCallBack = StdErrCallBack;
                job.StdOutCallBack = StdOutCallBack;
                m_ProcessHandler.JobList.Clear();
                m_ProcessHandler.JobList.Add(job);
                m_ProcessHandler.evProcessComplete -= (Processhandler_evProcessComplete);
                m_ProcessHandler.evProcessComplete += (Processhandler_evProcessComplete);
                m_ProcessHandler.Start();
            }
        }
        void Processhandler_evProcessComplete()
        {
            processOutput();
           m_ProcessHandler.evProcessComplete -= (Processhandler_evProcessComplete);


        }
        void processOutput()
        {
            ITestSuit currentSuit = null;
            //List<ITestSuit> suitList = new List<ITestSuit>();
            foreach (string str in m_testModel.Output)
            {
                if (str.Contains("Running main()"))
                {
                    continue;
                }
                else
                {
                    if (str.EndsWith("."))
                    {

                        ITestSuit suit = new TestSuit();
                        suit.TestSuitName = str.Split('.')[0];
                        currentSuit = suit;
                        if (null != currentSuit)
                        {
                            m_testModel.AddTestSuit(currentSuit);
                        }
                    }
                    else if (str.StartsWith(" "))
                    {
                        ItestCase testcase = new TestCase();
                        string[] arr = { " # GetParam()" };
                        if (str.Contains(" # GetParam()"))
                        {
                            testcase.Name = currentSuit.TestSuitName.Trim() + "." + str.Split(arr, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                        }
                        else
                        {
                            testcase.Name = currentSuit.TestSuitName.Trim() + "." + str.Trim();
                        }
                        currentSuit.TestCases.Add(testcase);

                    }

                }
            }
           
        }
        void StdErrCallBack(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Data != null)
                    {
                        m_testModel.Output +=(e.Data.ToString());
                    }
                }
            }
            catch
            {

            }
        }
        void StdOutCallBack(object sender, DataReceivedEventArgs e)
        {

            try
            {
                if (e != null)
                {
                    if (e.Data != null)
                    {
                        m_testModel.Output +=(e.Data.ToString());
                    }
                }
            }
            catch
            {

            }
        }


        public void Save()
        {
            try
            {
                Serialise("TestExecuter.xml");
            }
            catch
            {

            }
        }


        public void ChangeTheme(string color)
        {
            throw new NotImplementedException();
        }
    }
}
