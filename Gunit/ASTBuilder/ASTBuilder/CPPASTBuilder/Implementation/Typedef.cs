using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class Typedef:ITypeDef
    {
        ICppDataType m_typedefTo;
        string m_Name;
        public ICppDataType TypedefTo
        {
            get
            {
                return m_typedefTo;
            }
            set
            {
                m_typedefTo = value;
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
            get { return CppDataTypeKind.TypedefType; }
        }
    }
}
