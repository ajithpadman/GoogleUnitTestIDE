using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class Pointer:IPointer
    {
        ICDataType m_PointTo = null;
        string m_Name = "";
        bool m_IsconstQualified = false;
        public ICDataType PointTo
        {
            get
            {
                return m_PointTo;
            }
            set
            {
                m_PointTo = value;
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
                return DataTypeKind.Pointer;
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
                return m_IsconstQualified;
            }
            set
            {
                m_IsconstQualified = value;
            }
        }
    }
}
