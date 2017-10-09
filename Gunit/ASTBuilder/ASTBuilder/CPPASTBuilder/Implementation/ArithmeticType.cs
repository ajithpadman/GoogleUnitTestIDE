using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class ArithmeticType:IArithMeticType
    {
        string m_name;
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public CppDataTypeKind Kind
        {
            get { return CppDataTypeKind.ArithMeticType;}
        }
        public IArithMeticType GetUnknownType()
        {
            IArithMeticType type = new ArithmeticType();
            type.Name = "UNKNOWN";
            return type;

        }
    }
}
