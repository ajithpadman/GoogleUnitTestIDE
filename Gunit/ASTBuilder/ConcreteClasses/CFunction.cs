using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class CFunction:ICFunction
    {
        string m_Name;
        CAccessSpecifier m_accessSpecifier;
        CStorageClass m_StorageClass;
        List<ICVariable> m_LocalVariables = new List<ICVariable>();
        List<ICVariable> m_Parameters = new List<ICVariable>();
        List<ICFunction> m_calledFunctions = new List<ICFunction>();
        ICDataType m_ReturnValue;
        bool m_IsDefinition;
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
        bool m_isConstQualified = false;

        public CAccessSpecifier AccessSpecifier
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

        public CStorageClass StorageClass
        {
            get
            {
                return m_StorageClass;
            }
            set
            {
                m_StorageClass = value;
            }
        }

        public List<ICVariable> LocalVariables
        {
            get
            {
                return m_LocalVariables;
            }
            set
            {
                m_LocalVariables = value;
            }
        }

        public List<ICVariable> Parameters
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

        public ICDataType ReturnValue
        {
            get
            {
                return m_ReturnValue;
            }
            set
            {
                m_ReturnValue = value;
            }
        }


        public DataTypeKind Kind
        {
            get
            {
                return DataTypeKind.Function;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public List<ICFunction> CalledFunctions
        {
            get
            {
                return m_calledFunctions;
            }
            set
            {
                m_calledFunctions = value;
            }
        }

        public bool Isdefinition
        {
            get
            {
                return m_IsDefinition;
            }
            set
            {
                m_IsDefinition = value;
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
