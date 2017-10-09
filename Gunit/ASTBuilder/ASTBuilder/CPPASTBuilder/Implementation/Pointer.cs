using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class Pointer:IPointerType
    {
        ICppDataType m_pointTo;
        string m_Name;
        public ICppDataType PointTo
        {
            get
            {
                return m_pointTo;
            }
            set
            {
                m_pointTo = value;
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
            get { throw new NotImplementedException(); }
        }
    }
}
