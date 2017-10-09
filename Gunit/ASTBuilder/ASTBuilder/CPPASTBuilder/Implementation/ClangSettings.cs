using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
namespace CPPASTBuilder.Implementation
{
    public class ClangSettings:IClangSettings
    {
        List<string> m_IncludePaths = new List<string>();
        List<string> m_PreIncludes = new List<string>();
        List<string> m_Defines = new List<string>();
        List<string> m_LibPaths = new List<string>();
        List<string> m_LibNames = new List<string>();
        List<string> m_SpecialCommands = new List<string>();
        public List<string> IncludePaths
        {
            get
            {
                return m_IncludePaths;
            }
            set
            {
                m_IncludePaths = value;
            }
        }

        public List<string> PreIncludes
        {
            get
            {
                return m_PreIncludes;
            }
            set
            {
                m_PreIncludes = value;
            }
        }

        public List<string> Defines
        {
            get
            {
                return m_Defines;
            }
            set
            {
                m_Defines = value;
            }
        }

        public List<string> LibPaths
        {
            get
            {
                return m_LibPaths;
            }
            set
            {
                m_LibPaths = value;
            }
        }

        public List<string> LibNames
        {
            get
            {
                return m_LibNames;
            }
            set
            {
                m_LibNames = value;
            }
        }

        public List<string> SpecialCommands
        {
            get
            {
                return m_SpecialCommands;
            }
            set
            {
                m_SpecialCommands = value;
            }
        }

        public List<string> GetClangCommandLine()
        {
            List<string> l_CommandLine = new List<string>();

            l_CommandLine.Add("-fno-ms-compatibility");
            l_CommandLine.Add("-std=c++11");

            foreach (string str in IncludePaths)
            {
                l_CommandLine.Add("-I");
                l_CommandLine.Add(str);
            }

            foreach (string str in PreIncludes)
            {

                l_CommandLine.Add("-include");
                l_CommandLine.Add(str);


            }

            foreach (string macro in Defines)
            {
                l_CommandLine.Add("-D");
                l_CommandLine.Add(macro);
            }
            l_CommandLine.Add("-Wall");
            l_CommandLine.Add("-MMD");
            l_CommandLine.Add("-MP");
            l_CommandLine.Add("-x");
            l_CommandLine.Add("c++");

            return l_CommandLine;
        }
    }
}
