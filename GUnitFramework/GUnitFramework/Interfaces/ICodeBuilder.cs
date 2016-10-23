using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GUnitFramework.Implementation;

namespace GUnitFramework.Interfaces
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
   
    public interface ICodeBuilder : ICGunitPlugin, INotifyPropertyChanged
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
