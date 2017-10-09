using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class ArithmeticType:IArithmeticType
    {
        string m_Name = "UnKnown";
        bool m_isConstQualified = false;

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }


        public DataTypeKind Kind
        {
            get
            {
                return DataTypeKind.ArithMeticType;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public bool isConstQualified
        {
            get
            {
                return m_isConstQualified;
            }
            set
            {
                m_isConstQualified = value;
            }
        }
    }
}
