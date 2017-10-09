using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class Typedef:ITypedef
    {
        ICDataType m_TypedefOf;
        string m_Name;
        bool m_isConstQualified = false;
        public ICDataType TypedefOf
        {
            get
            {
                return m_TypedefOf;
            }
            set
            {
                m_TypedefOf = value;
            }
        }

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
                return DataTypeKind.Typedef;
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
