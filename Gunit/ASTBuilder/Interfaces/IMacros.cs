using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface IMacros
    {
        string MacroName
        {
            get;
            set;
        }
        string MacroValue
        {
            get;
            set;
        }


    }
}
