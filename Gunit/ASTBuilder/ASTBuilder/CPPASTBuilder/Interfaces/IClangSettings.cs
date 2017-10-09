using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface IClangSettings
    {
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
        List<string> Defines
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
        List<string> SpecialCommands
        {
            get;
            set;
        }
        List<string> GetClangCommandLine();

    }
}
