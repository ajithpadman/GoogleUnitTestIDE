using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class Namespace:INamespace
    {
        INamespace m_parent;
        List<INamespace> m_ChildNameSpaces = new List<INamespace>();
        List<IClass> m_Classes = new List<IClass>();
        string m_Name;
        
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public List<INamespace> Namespaces
        {
            get
            {
                return m_ChildNameSpaces;
            }
            set
            {
                m_ChildNameSpaces = value;
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

        public INamespace ParentNameSpace
        {
            get
            {
                return m_parent;
            }
            set
            {
                m_parent = value;
            }
        }
    }
}
