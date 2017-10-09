using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface IPointer:ICDataType
    {
        ICDataType PointTo
        {
            get;
            set;
        }
     
    }
}
