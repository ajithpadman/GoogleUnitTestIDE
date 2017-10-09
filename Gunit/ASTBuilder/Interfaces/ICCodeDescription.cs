using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public interface ICCodeDescription
    {
        string FileName
        {
            get;
            set;
        }
        List<ICFunction> Functions
        {
            get;
            set;
        }
     
        List<ICVariable> GlobalVariables
        {
            get;
            set;
        }
    }
}
