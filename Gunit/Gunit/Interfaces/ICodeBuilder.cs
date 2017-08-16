using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Gunit.Utils;

namespace Gunit.Interfaces
{
    public enum OutputTypes
    {
        ConsoleApplication,
        SharedLibrary

    }
    public enum WarningLevel
    {
        SyntaxCheckOnly,
        TreatAllWarningAsError,
        EnableAllWarning,
        AbortCompilationOnFirstError
    }
   
    public interface ICodeBuilder :  INotifyPropertyChanged
    {
        bool IsCodeInstrumented
        {
            get;
            set;
        }
        WarningLevel WarningLevel
        {
            get;
            set;
        }
        OutputTypes OutputType
        {
            get;
            set;
        }
        string BuildDirectory
        {
            get;
            set;
        }
        string CompilorPath
        {
            get;
            set;
        }
        IProcessHandler buildJobHandler
        {
            get;
            set;
        }
        ListOfConsoleData CompilorOutput
        {
            get;
            set;
        }
        void buildProject();
        
    }
}
