using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class Structure:IStructure
    {
        List<IMemberVariable> m_fields = new List<IMemberVariable>();
        string m_Name = "";
        public List<IMemberVariable> Fields
        {
            get
            {
                return m_fields;
            }
            set
            {
                m_fields = value;
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
            get { return CppDataTypeKind.StructureType; }
        }
    }
}
