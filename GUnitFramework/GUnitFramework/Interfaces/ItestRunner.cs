using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GUnitFramework.Interfaces
{


    public interface ItestRunner : ICGunitPlugin, INotifyPropertyChanged
    {
        List<ItestCase> SelectedTests
        {
            get;
            set;
        }
        List<ITestSuit> TestSuits
        {
            get;
            set;
        }
        IProcessHandler Processhandler
        {
            get;
            set;
        }
         string GTestExecutable
        {
            get;
            set;
        }
         void addTestSuit(ITestSuit suit);
         void addSelectedTests(ItestCase test);
        void RunTests();
      
    }
}
