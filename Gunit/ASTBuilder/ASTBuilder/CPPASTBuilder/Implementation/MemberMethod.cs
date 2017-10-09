using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class MemberMethod:IMemberMethod
    {
        bool m_IsVirtual = false;
        bool m_IsPureVirtual = false;
        ICppDataType m_Datatype;
        string m_MethodName;
        List<ICppDataType> m_Parameters = new List<ICppDataType>();
        ClangSharp.StorageClass m_storageClass;
        ClangSharp.AccessSpecifier m_accessSpecifier;
        bool m_IsConstQualified = false;
        bool m_IsVolatileQualified = false;
        bool m_IsRestrictQualified = false;
        public bool IsVirtual
        {
            get
            {
                return m_IsVirtual;
            }
            set
            {
                m_IsVirtual = value;
            }
        }

        public bool IsPureVirtual
        {
            get
            {
                return m_IsPureVirtual;
            }
            set
            {
                m_IsPureVirtual = value;
            }
        }

        public ICppDataType ReturnValue
        {
            get
            {
                return m_Datatype;
            }
            set
            {
                m_Datatype = value;
            }
        }

        public string MethodName
        {
            get
            {
                return m_MethodName;
            }
            set
            {
                m_MethodName = value;
            }
        }

        public List<ICppDataType> Parameters
        {
            get
            {
                return m_Parameters;
            }
            set
            {
                m_Parameters = value;
            }
        }

        public ClangSharp.StorageClass StorageClass
        {
            get
            {
                return m_storageClass;
            }
            set
            {
                m_storageClass = value;
            }
        }

        public ClangSharp.AccessSpecifier AccessScope
        {
            get
            {
                return m_accessSpecifier;
            }
            set
            {
                m_accessSpecifier = value;
            }
        }

        public bool IsConstQualified
        {
            get
            {
                return m_IsConstQualified;
            }
            set
            {
                m_IsConstQualified = value;
            }
        }

        public bool IsVolatileQualified
        {
            get
            {
                return m_IsVolatileQualified;
            }
            set
            {
                m_IsVolatileQualified = value;
            }
        }

        public bool IsRestrictQualified
        {
            get
            {
                return m_IsRestrictQualified;
            }
            set
            {
                m_IsRestrictQualified = value;
            }
        }
    }
}
