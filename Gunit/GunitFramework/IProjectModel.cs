using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Gunit.Interfaces
{
    public enum ProjectState
    {
        INIT,
        NEW,
        OPEN,
        SAVE,
        DIRTY
        
    }
    public interface IProjectModel
    {
        event PropertyChangedEventHandler PropertyChanged;
        string Name { get; set; }
        string ProjectPath { get; set; }
        string BinaryPath { get; set; }
        ProjectState State { get; set; }
        string SelectedFile { get; set; }
        bool isProjectOpen { get; set; }
        string ConsoleLog { get; set; }
        ObservableCollection<string> IncludePaths{ get; }
        ObservableCollection<string> LibraryPaths { get; }
        ObservableCollection<string> LibNames{get;}
        ObservableCollection<string> Defines { get;  }
        ObservableCollection<string> SourceFiles { get;  }
        ObservableCollection<string> HeaderFiles { get;  }
        ObservableCollection<string> PreHeaderFiles { get;}
        ObservableCollection<string> CompilerArguments { get;  }
    }
}
