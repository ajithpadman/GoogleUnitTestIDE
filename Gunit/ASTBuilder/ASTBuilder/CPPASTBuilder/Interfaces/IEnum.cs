using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{
    public interface IEnum:ICppDataType
    {
        List<IEnumElements> Elements
        {
            get;
            set;
        }
    }
}
