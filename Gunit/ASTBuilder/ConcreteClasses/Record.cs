using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class Record:IRecord
    {
        List<ICVariable> m_Elements = new List<ICVariable>();
        string m_Name;
        bool m_isConstQualified = false;
        public List<ICVariable> Elements
        {
            get
            {
                return m_Elements;
            }
            set
            {
                m_Elements = value;
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
                return DataTypeKind.Record;
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
