using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface INamespace
    {
        string Name
        {
            get;
            set;
        }
        INamespace ParentNameSpace
        {
            get;
            set;
        }
        List<INamespace> Namespaces
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
