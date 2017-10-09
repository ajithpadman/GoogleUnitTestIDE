using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class CVariable:ICVariable
    {
        ICDataType m_type;
        bool m_IsInit;
        string m_Name;
        CAccessSpecifier m_access;
        CStorageClass m_storageClass;
        public ICDataType Type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }

        public bool IsInitialised
        {
            get
            {
                return m_IsInit;
            }
            set
            {
                m_IsInit = value;
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

        public CAccessSpecifier AccessSpecifier
        {
            get
            {
                return m_access;
            }
            set
            {
                m_access = value;
            }
        }

        public CStorageClass StorageClass
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
    }
}
