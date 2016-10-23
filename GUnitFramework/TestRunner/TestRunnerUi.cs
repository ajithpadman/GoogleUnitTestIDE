using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using GUnitFramework.Interfaces;
using System.Diagnostics;
using System.IO;
using GUnitFramework.Implementation;

namespace TestRunner
{
  
    public partial class TestRunnerUi : DockContent
    {
        ItestRunner m_runner;
        List<string> m_output = new List<string>();
        public TestRunnerUi()
        {
            InitializeComponent();
        }
        public TestRunnerUi(ItestRunner runner)
        {
            InitializeComponent();
            m_runner = runner;
            m_runner.PropertyChanged+=new PropertyChangedEventHandler(Runner_PropertyChanged);
            treeTests.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeTests.DrawNode += new DrawTreeNodeEventHandler(treeTests_DrawNode);
            treeTests.CheckBoxes = true;
        }
        void treeTests_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Tag is ItestCase)
            {
                ItestCase path = e.Node.Tag as TestCase;
                if (path == null)
                {
                    e.Node.HideCheckBox();
                }
            }
            else
            {
                e.Node.HideCheckBox();
            }
            e.DrawDefault = true;
        }
        void Runner_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "EXE":
                    
                    if (File.Exists(m_runner.GTestExecutable))
                    {
                        btnList.Enabled = true;
                        txtPath.Text = m_runner.GTestExecutable;
                    }
                    else
                    {
                        btnList.Enabled = false;
                        txtPath.Text = "File does not exist!";
                    }
                    break;
                case "TESTSUIT":
                case "SELECTED_TEST":
                    PrintTestSuits();
                    
                    break;
                default:
                    break;
            }
        }
        private void PrintTestSuits()
        {
            treeTests.Nodes.Clear();
            treeTests.ShowNodeToolTips = true; 
            
            TreeNode root = new TreeNode("Tests");
            root.ImageIndex = 0;
            root.SelectedImageIndex = 0;
            if (m_runner.TestSuits.Count != 0)
            {
                btnRunAll.Enabled = true;
            }
            foreach (ITestSuit suit in m_runner.TestSuits)
            {

                TreeNode suitNode = new TreeNode(suit.TestSuitName);
                suitNode.ImageIndex = 1;
                suitNode.SelectedImageIndex = 1;
                foreach (ItestCase testcase in suit.TestCases)
                {
                   
                    TreeNode node = new TreeNode(testcase.Name);
                    node.Tag = testcase;
                    if (m_runner.SelectedTests.Contains(testcase))
                    {
                        node.Checked = true;
                    }
                    switch (testcase.Status)
                    {
                        case TestStatus.NotRun:
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;
                            break;
                        case TestStatus.OK:
                            node.ImageIndex = 3;
                            node.SelectedImageIndex = 3;
                            break;
                        case TestStatus.Error:
                            node.ImageIndex = 4;
                            node.SelectedImageIndex = 4;
                            break;
                        default:
                            break;

                    }
                    
                    node.ToolTipText = testcase.ErrorString;
                    suitNode.Nodes.Add(node);

                }
                root.Nodes.Add(suitNode);
            }
            treeTests.Nodes.Add(root);
            
        }
        private void TestRunnerUi_Load(object sender, EventArgs e)
        {
            btnRunAll.Enabled = false;
            if (File.Exists(m_runner.GTestExecutable))
            {
                btnList.Enabled = true;
              
                txtPath.Text = m_runner.GTestExecutable;
            }
            else
            {
                btnList.Enabled = false;
               
                txtPath.Text ="File does not exist!";
            }
        }

        private void ListTests()
        {
            m_output.Clear();
            m_runner.TestSuits.Clear();
            m_runner.SelectedTests.Clear();
            IJob job = new GUnitFramework.Implementation.Job();
            job.Command = m_runner.GTestExecutable;
            job.Argument = " --gtest_list_tests";
            job.StdErrCallBack = StdErrCallBack;
            job.StdOutCallBack = StdOutCallBack;
            m_runner.Processhandler.JobList.Clear();
            m_runner.Processhandler.JobList.Add(job);

            m_runner.Processhandler.evProcessComplete -= (Processhandler_evProcessComplete);
            m_runner.Processhandler.evProcessComplete+=(Processhandler_evProcessComplete);
            m_runner.Processhandler.Start();

        }
        void Processhandler_evProcessComplete()
        {
            processOutput();
            m_runner.Processhandler.evProcessComplete -= (Processhandler_evProcessComplete);
            

        }
        void Processhandler_TestRunComplete()
        {
            processTestResult();
            m_runner.Processhandler.evProcessComplete -= (Processhandler_TestRunComplete);
        }
        private bool IsTestSpecification(string line, ItestCase test)
        {
            string[] splitArray = {"@#"};
            string[] result;
            bool retval = false;
            if (line.Contains("@#TESTPHASENAME@#"))
            {
               result= line.Split(splitArray, StringSplitOptions.RemoveEmptyEntries);
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
                    test.TestDescription = ( result[1]);
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
        private void processTestResult()
        {
            bool l_started = false;
            foreach (ItestCase test in m_runner.SelectedTests)
            {
                test.ExpectedResult.Clear();
                test.TestSteps.Clear();
                test.TestPrecondition.Clear();
                test.TestDescription = "";
                test.TestPhaseNumber = "Unknown";
            }
            foreach (ItestCase test in m_runner.SelectedTests)
            {
                foreach (string line in m_output)
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
                        if (IsTestSpecification(line,test) == false)
                        {
                            test.ErrorString += line + "\n";
                        }
                    }
                }
            }
            PrintTestSuits();
            MessageBox.Show("Test Run Complete");
        }
        private void processOutput()
        {
            ITestSuit currentSuit = null;
            List<ITestSuit> suitList = new List<ITestSuit>();
            foreach (string str in m_output)
            {
                if (str.Contains("Running main()"))
                {
                    continue;
                }
                else
                {
                    if ( str.EndsWith("."))
                    {
                        
                        ITestSuit suit = new GUnitFramework.Implementation.TestSuit();
                        suit.TestSuitName = str.Split('.')[0];
                        currentSuit = suit;
                        if (null != currentSuit)
                        {
                            suitList.Add(currentSuit);
                        }
                    }
                    else if (str.StartsWith(" "))
                    {
                        ItestCase testcase = new GUnitFramework.Implementation.TestCase();
                        string[] arr = {" # GetParam()"};
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
            m_runner.TestSuits = suitList;
         
        }
        void StdErrCallBack(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    if (e.Data != null)
                    {
                        m_output.Add(e.Data.ToString());
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
                        m_output.Add(e.Data.ToString());
                    }
                }
            }
            catch
            {

            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "EXE file (*.exe)|*.exe";
            DialogResult result = dlg.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_runner.GTestExecutable = dlg.FileName;
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            ListTests();
        }

        private void Test_checked(object sender, TreeViewEventArgs e)
        {
            lock (m_runner.SelectedTests)
            {
                if (e.Node.Checked)
                {
                    if (e.Node.Tag is TestCase)
                    {
                        m_runner.SelectedTests.Add(e.Node.Tag as TestCase);
                    }
                }
                else
                {
                    if (e.Node.Tag is TestCase)
                    {
                        if (m_runner.SelectedTests.Contains(e.Node.Tag as TestCase))
                        {
                            TestCase test = e.Node.Tag as TestCase;
                            m_runner.SelectedTests.RemoveAll(item => item == test);
                        }
                    }
                }
            }
        }

        private void addTest(ItestCase test)
        {
            
            
            IJob job = new GUnitFramework.Implementation.Job();
            job.Command = m_runner.GTestExecutable;
            job.Argument = " --gtest_filter=" + test.Name;
            job.StdErrCallBack = StdErrCallBack;
            job.StdOutCallBack = StdOutCallBack;
            m_runner.Processhandler.JobList.Add(job);

            

        }

        private void btnRunAll_Click(object sender, EventArgs e)
        {
            m_output.Clear();
            m_runner.Processhandler.JobList.Clear();
            foreach (ITestSuit suit in m_runner.TestSuits)
            {
                foreach (ItestCase test in suit.TestCases)
                {
                    test.Status = TestStatus.NotRun;
                    test.ErrorString = "";
                }
            }
            PrintTestSuits();
            if (m_runner.SelectedTests.Count == 0)
            {
                foreach (ITestSuit suit in m_runner.TestSuits)
                {
                    foreach (ItestCase test in suit.TestCases)
                    {
                        addTest(test);
                        m_runner.addSelectedTests(test);
                       
                    }
                }
             
            }
            else
            {
                foreach (TestCase test in m_runner.SelectedTests)
                {
                    addTest(test);
                }
            
               
            }
            m_runner.Processhandler.evProcessComplete -= (Processhandler_TestRunComplete);
            m_runner.Processhandler.evProcessComplete += (Processhandler_TestRunComplete);
            m_runner.Processhandler.Start();
        }
        

    }
}
