using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public enum DataTypeKind
    {
        Record,
        Function,
        Pointer,
        Macro,
        Enum,
        Typedef,
        ArithMeticType,
        ArrayType
    }

    public interface ICDataType
    {
        string Name
        {
            get;
            set;
        }
        DataTypeKind Kind
        {
            get;
            set;
        }
        bool isConstQualified
        {
            get;
            set;
        }
    }
      
}
