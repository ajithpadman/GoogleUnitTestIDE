using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class Array:IArray
    {
        ICDataType m_arrayElementType;
        string m_Name;
        bool m_isConst = false;
        public ICDataType ArrayElementType
        {
            get
            {
                return m_arrayElementType;
            }
            set
            {
                m_arrayElementType = value;
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
                return DataTypeKind.ArrayType;
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
                return m_isConst;
            }
            set
            {
                m_isConst = value;
            }
        }
    }
}
