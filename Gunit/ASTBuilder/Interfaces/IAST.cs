using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface IAST
    {
        List<string> IncludePaths
        {
            get;
            set;
        }
        List<string> Defines
        {
            get;
            set;
        }
        List<string> PreIncludeFiles
        {
            get;
            set;
        }

        ICCodeDescription CodeDescription
        {
            get;
            set;
        }
        bool ParseFile();
        

    }
}
