using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface ITypedef:ICDataType
    {
        ICDataType TypedefOf
        {
            get;
            set;
        }
    }
}
