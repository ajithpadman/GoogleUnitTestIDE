using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class CppCodeDescription:ICppCodeDescription
    {
        List<INamespace> m_Namespaces = new List<INamespace>();
        List<IClass> m_Classes = new List<IClass>();
        string m_FileName;
        public List<INamespace> NameSpaces
        {
            get
            {
                return m_Namespaces;
            }
            set
            {
                m_Namespaces = value;
            }
        }

        public List<IClass> Classes
        {
            get
            {
                return m_Classes;
            }
            set
            {
                m_Classes = value;
            }
        }

        public string FileName
        {
            get
            {
                return m_FileName;
            }
            set
            {
                m_FileName = value;
            }
        }
    }
}
