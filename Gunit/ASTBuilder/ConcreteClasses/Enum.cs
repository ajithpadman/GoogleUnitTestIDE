using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class Enum:IEnum
    {
        List<EnumElement> m_elements = new List<EnumElement>();
        string m_Name;
        bool m_isConstQualified = false;
        public List<EnumElement> Elements
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

        public DataTypeKind Kind
        {
            get
            {
                return DataTypeKind.Enum;
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
