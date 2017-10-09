using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    class EnumElements:IEnumElements
    {
        string m_Name;
        uint m_value;
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

        public uint Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }
    }
}
