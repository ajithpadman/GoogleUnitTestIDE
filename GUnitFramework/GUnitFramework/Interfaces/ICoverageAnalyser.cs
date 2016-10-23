using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnitFramework.Interfaces
{
    public interface ICoverageAnalyser:ITestReportGenerator
    {
        IProcessHandler coverageAnalyser
        {
            get;
            set;
        }
        string ObjectsPath
        {
            get;
            set;
        }
        void AnalyseCoverage();
    }
}
