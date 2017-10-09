using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface ITypeDef:ICppDataType
    {
        ICppDataType TypedefTo
        {
            get;
            set;
        }
    }
}
