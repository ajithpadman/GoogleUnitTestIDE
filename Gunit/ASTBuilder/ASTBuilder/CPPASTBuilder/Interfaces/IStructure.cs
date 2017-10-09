using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface IStructure:ICppDataType
    {
        List<IMemberVariable> Fields
        {
            get;
            set;
        }
    }
}
