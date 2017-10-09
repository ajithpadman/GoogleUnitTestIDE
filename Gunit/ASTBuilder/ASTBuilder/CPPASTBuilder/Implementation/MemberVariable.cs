using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class MemberVariable:IMemberVariable
    {
        string m_VariableName;
        ICppDataType m_DataType;
        ClangSharp.StorageClass m_storageClass;
        ClangSharp.AccessSpecifier m_accessSpecifier;
        bool m_IsConstQualified = false;
        bool m_IsVolatileQualified = false;
        bool m_IsRestrictQualified = false;
        public string VariableName
        {
            get
            {
                return m_VariableName;
            }
            set
            {
                m_VariableName = value;
            }
        }

        public ICppDataType DataType
        {
            get
            {
                return m_DataType;
            }
            set
            {
                m_DataType = value;
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
