using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface IEnumElements
    {
        string Name
        {
            get;
            set;
        }
        UInt32 Value
        {
            get;
            set;
        }
    }
}
