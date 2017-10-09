using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class CCodeDescriptioncs:ICCodeDescription
    {
        List<ICFunction> m_Functions = new List<ICFunction>();
       
        List<ICVariable> m_GlobalVariables = new List<ICVariable>();
        string m_fileName;
        public List<ICFunction> Functions
        {
            get
            {
                return m_Functions;
            }
            set
            {
                m_Functions = value;
            }
        }

       

        public List<ICVariable> GlobalVariables
        {
            get
            {
                return m_GlobalVariables;
            }
            set
            {
                m_GlobalVariables = value;
            }
        }

        public string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
            }
        }
    }
}
