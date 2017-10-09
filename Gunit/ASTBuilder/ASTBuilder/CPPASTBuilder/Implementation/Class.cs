using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class Class:IClass
    {
        bool m_IsTemplate = false;
        List<IClass> m_parents = new List<IClass>();
        List<IClass> m_InnerClasses = new List<IClass>();
        List<IMemberMethod> m_MemberMethods = new List<IMemberMethod>();
        List<IMemberVariable> m_MemberVariables = new List<IMemberVariable>();
        string m_Name = "";
        List<string> m_typeTemplateParam = new List<string>();
        public bool IsTempleteClass
        {
            get
            {
                return m_IsTemplate;
            }
            set
            {
                m_IsTemplate = value;
            }
        }

        public List<IClass> Parents
        {
            get
            {
                return m_parents;
            }
            set
            {
                m_parents = value;
            }
        }

        public List<IClass> ChildClasses
        {
            get
            {
                return m_InnerClasses;
            }
            set
            {
                m_InnerClasses = value;
            }
        }

        public List<IMemberMethod> MemeberMethods
        {
            get
            {
                return m_MemberMethods;
            }
            set
            {
                m_MemberMethods = value;
            }
        }

        public List<IMemberVariable> MemberVariables
        {
            get
            {
                return m_MemberVariables;
            }
            set
            {
                m_MemberVariables = value;
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
            get
            {
                return CppDataTypeKind.ClassType;
            }
           
        }

        public List<string> TypeTemplateParam
        {
            get
            {
                return m_typeTemplateParam;
            }
            set
            {
                m_typeTemplateParam = value;
            }
        }
    }
}
