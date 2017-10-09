using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class ReferenceType:IReferenceType
    {
        ICppDataType m_referenceTo;
        string m_Name;
        public ICppDataType ReferenceTo
        {
            get
            {
                return m_referenceTo;
            }
            set
            {
                m_referenceTo = value;
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
            get { return CppDataTypeKind.ReferenceType; }
        }
    }
}
