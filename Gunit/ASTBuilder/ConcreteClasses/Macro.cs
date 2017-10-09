using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder.Interfaces;
namespace ASTBuilder.ConcreteClasses
{
    public class Macro:IMacros
    {
        string m_MacroName;
        string m_value;
        public string MacroName
        {
            get
            {
                return m_MacroName;
            }
            set
            {
                m_MacroName = value;
            }
        }

        public string MacroValue
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
            }
        }
    }
}
