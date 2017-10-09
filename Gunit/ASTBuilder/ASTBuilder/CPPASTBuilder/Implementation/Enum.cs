using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class Enumeration:IEnum
    {
        List<IEnumElements> m_elements = new List<IEnumElements>();
        string m_Name;
        public List<IEnumElements> Elements
        {
            get
            {
                return m_elements;
            }
            set
            {
                m_elements = value;
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
            get { return CppDataTypeKind.EnumType; }
        }
    }
}
