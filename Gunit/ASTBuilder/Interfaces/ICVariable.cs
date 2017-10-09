using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface ICVariable:ICElement
    {
        ICDataType Type
        {
            get;
            set;
        }
        bool IsInitialised
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
    }
}
