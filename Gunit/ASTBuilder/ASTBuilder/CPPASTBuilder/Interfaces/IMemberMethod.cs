using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPPASTBuilder.Interfaces
{

    public interface IMemberMethod : IMember
    {
      
        
        bool IsVirtual
        {
            get;
            set;
        }
        bool IsPureVirtual
        {
            get;
            set;
        }
        ICppDataType ReturnValue
        {
            get;
            set;
        }
        string MethodName
        {
            get;
            set;
        }
        List<ICppDataType> Parameters
        {
            get;
            set;
        }

    }
}
