using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface ICFunction:ICDataType,ICElement
    {
        bool Isdefinition
        {
            get;
            set;
        }
        List<ICFunction> CalledFunctions
        {
            get;
            set;
        }
        List<ICVariable> LocalVariables
        {
            get;
            set;

        }
        List<ICVariable> Parameters
        {
            get;
            set;
        }
        ICDataType ReturnValue
        {
            get;
            set;
        }
    }
}
