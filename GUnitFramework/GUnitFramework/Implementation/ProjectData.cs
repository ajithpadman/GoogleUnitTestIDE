using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
namespace GUnitFramework.Implementation
{
    public class ProjectData:ICProjectData
    {
        string m_name = "ProjectName";
        string m_path;
        List<string> m_includes = new List<string>();
        List<string> m_preIncludes = new List<string>();
        List<string> m_libPaths = new List<string>();
        List<string> m_libNames = new List<string>();
        List<string> m_macros = new List<string>();
        List<string> m_srcFiles = new List<string>();
        List<string> m_HeaderFiles = new List<string>();
        List<string> m_specialArgs = new List<string>();
        string m_Output = "";
        public string ProjectName
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public string ProjectPath
        {
            get
            {
                return m_path;
            }
            set
            {
                m_path = value;
            }
        }

        public List<string> IncludePaths
        {
            get
            {
                return m_includes;
            }
            set
            {
                m_includes = value;
            }
        }

        public List<string> PreIncludes
        {
            get
            {
                return m_preIncludes;
            }
            set
            {
                m_preIncludes = value;
            }
        }

        public List<string> LibPaths
        {
            get
            {
                return m_libPaths;
            }
            set
            {
                m_libPaths = value;
            }
        }

        public List<string> LibNames
        {
            get
            {
                return m_libNames;
            }
            set
            {
                m_libNames = value;
            }
        }

        public List<string> Defines
        {
            get
            {
                return m_macros;
            }
            set
            {
                m_macros = value;
            }
        }

        public List<string> SourceFiles
        {
            get
            {
                return m_srcFiles;
            }
            set
            {
                m_srcFiles = value;
            }
        }

        public List<string> SpecialCompilorArguments
        {
            get
            {
                return m_specialArgs;
            }
            set
            {
                m_specialArgs = value;
            }
        }


        public List<string> HeaderFiles
        {
            get
            {
                return m_HeaderFiles;
            }
            set
            {
                m_HeaderFiles = value;
            }
        }
        public string Output
        {
            get { return m_Output; }
            set { m_Output = value; }
        }
    }
}
