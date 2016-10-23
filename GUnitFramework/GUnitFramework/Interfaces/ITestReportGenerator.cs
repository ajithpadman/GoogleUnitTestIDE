using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnitFramework.Interfaces
{
    public interface ITestReportGenerator:ICGunitPlugin
    {

        string ReportPath
        {
            get;
            set;
        }
        void generateReport(string fileName);
    }
}
