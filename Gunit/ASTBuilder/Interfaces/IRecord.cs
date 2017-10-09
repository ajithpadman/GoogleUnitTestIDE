using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface IRecord:ICDataType
    {
        List<ICVariable> Elements
        {
            get;
            set;
        }

    }
}
