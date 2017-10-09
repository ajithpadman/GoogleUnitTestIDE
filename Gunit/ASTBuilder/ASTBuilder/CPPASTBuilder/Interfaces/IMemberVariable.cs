using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface IMemberVariable : IMember
    {
        string VariableName
        {
            get;
            set;
        }
        ICppDataType DataType
        {
            get;
            set;
        }
    }
}
