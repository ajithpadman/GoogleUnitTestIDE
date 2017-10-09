using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASTBuilder.Interfaces
{
    public class EnumElement
    {
        string m_Name;
        UInt64 m_value;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public UInt64 Value
        {
            get { return m_value; }
            set { m_value = value; }
        }


    }
    public interface IEnum:ICDataType
    {
        List<EnumElement> Elements
        {
            get;
            set;
        }

    }
}
