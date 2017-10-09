using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class Array:IArray
    {
        ICppDataType m_ArrayElementType;
        string m_Name;
        public ICppDataType ArrayElementType
        {
            get
            {
                return m_ArrayElementType;
            }
            set
            {
                m_ArrayElementType = value;
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

        public CppDataTypeKind Kind
        {
            get { return CppDataTypeKind.ArrayType; }
        }
    }
}
