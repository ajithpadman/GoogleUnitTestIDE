using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;

namespace ASTBuilder.Interfaces
{
    public interface IArray:ICDataType
    {
        ICDataType ArrayElementType
        {
            get;
            set;
        }
    }
}
