using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gunit.Interfaces
{
    public enum TestStatus
    {
        NotRun,
        OK,
        Error
    }
    public interface ItestCase
    {
        string Name
        {
            get;
            set;
        }
        TestStatus Status
        {
            get;
            set;
        }
        string ErrorString
        {
            get;
            set;
        }
        string ExecutionTime
        {
            get;
            set;
        }
        string TestPhaseNumber
        {
            get;
            set;
        }
        string TestDescription
        {
            get;
            set;
        }
        List<string> TestPrecondition
        {
            get;
            set;
        }
        List<string> ExpectedResult
        {
            get;
            set;
        }
        List<string> TestSteps
        {
            get;
            set;
        }
        bool IsSelected
        {
            get;
            set;
        }
        

    }
    public interface ITestSuit
    {
        string TestSuitName
        {
            get;
            set;
        }
        List<ItestCase> TestCases
        {
            get;
            set;
        }
        bool IsSelected
        {
            get;
            set;
        }
    }
}
