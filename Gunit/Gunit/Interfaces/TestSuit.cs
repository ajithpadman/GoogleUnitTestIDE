using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Interfaces;
using Gunit.Model;
namespace Gunit.Interfaces
{
    public class TestCase :ViewModelBase, ItestCase
    {
        string m_testCaseName = "";
        TestStatus m_status = TestStatus.NotRun;
        string m_Error = "";
        string m_executionTime = "";
        string m_TestPhaseNumber = "";
        string m_TestDescription = "";
        List<string> m_TestPrecondition = new List<string>();
        List<string> m_ExpectedResult = new List<string>();
        List<string> m_testSteps = new List<string>();
        bool m_IsSelected =false;
        public string Name
        {
            get
            {
                return m_testCaseName;
            }
            set
            {
                m_testCaseName = value;
            }
        }

        public TestStatus Status
        {
            get
            {
                return m_status;
            }
            set
            {
                m_status = value;
                OnPropertyChanged("Status");
            }
        }


        public string ErrorString
        {
            get
            {
                return m_Error;
            }
            set
            {
                m_Error = value;
                OnPropertyChanged("ErrorString");
            }
        }

        public string ExecutionTime
        {
            get
            {
                return m_executionTime;
            }
            set
            {
                m_executionTime = value;
                OnPropertyChanged("ExecutionTime");
            }
        }


        public string TestPhaseNumber
        {
            get
            {
                return m_TestPhaseNumber;
            }
            set
            {
                m_TestPhaseNumber = value;
            }
        }

        public string TestDescription
        {
            get
            {
                return m_TestDescription;
            }
            set
            {
                m_TestDescription = value;
            }
        }

        public List<string> TestPrecondition
        {
            get
            {
                return m_TestPrecondition;

            }
            set
            {
                m_TestPrecondition = value;
            }
        }

        public List<string> ExpectedResult
        {
            get
            {
                return m_ExpectedResult;
            }
            set
            {
                m_ExpectedResult = value;
            }
        }


        public List<string> TestSteps
        {
            get
            {
                return m_testSteps;
            }
            set
            {
                m_testSteps = value;
            }
        }


        public bool IsSelected
        {
            get
            {
                return m_IsSelected;
            }
            set
            {
                m_IsSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
    }
    public class TestSuit : ViewModelBase, ITestSuit
    {
        string m_testSuitName = "";
        bool m_IsSelected = false;
        List<ItestCase> m_testCases = new List<ItestCase>();
        public string TestSuitName
        {
            get
            {
                return m_testSuitName;
            }
            set
            {
                m_testSuitName = value;
            }
        }

        public List<ItestCase> TestCases
        {
            get
            {
                return m_testCases;
            }
            set
            {
                m_testCases = value;
            }
        }





        public bool IsSelected
        {
            get
            {
                return m_IsSelected;
            }
            set
            {
                m_IsSelected = value;
                foreach (ItestCase case1 in TestCases)
                {
                    case1.IsSelected = value;
                }
                OnPropertyChanged("IsSelected");
            }
        }
    }
}
