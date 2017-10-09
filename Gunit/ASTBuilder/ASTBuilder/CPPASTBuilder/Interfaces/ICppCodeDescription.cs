using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface ICppCodeDescription
    {
        string FileName
        {
            get;
            set;
        }
        List<INamespace> NameSpaces
        {
            get;
            set;
        }
        List<IClass> Classes
        {
            get;
            set;
        }
    }
}
