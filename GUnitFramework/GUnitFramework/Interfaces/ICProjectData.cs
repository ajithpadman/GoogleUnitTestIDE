using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnitFramework.Interfaces
{
    public interface ICProjectData
    {
        string ProjectName
        {
            get;
            set;
        }
        string ProjectPath
        {
            get;
            set;
        }
        List<string> IncludePaths
        {
            get;
            set;
        }
        List<string> PreIncludes
        {
            get;
            set;
        }
        List<string> LibPaths
        {
            get;
            set;
        }
        List<string> LibNames
        {
            get;
            set;
        }
        List<string> Defines
        {
            get;
            set;
        }
        List<string> SourceFiles
        {
            get;
            set;
        }
        List<string> HeaderFiles
        {
            get;
            set;
        }
        List<string> SpecialCompilorArguments
        {
            get;
            set;
        }
        string Output
        {
            get;
            set;
        }


    }
}
