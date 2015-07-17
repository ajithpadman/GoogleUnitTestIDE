using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace GUnit
{
    public class ObjectList
    {
        public string objectPath;
        public string sourceFile;
    }
    public class SolutionBuilder
    {
        ProjectInfo m_Project;
        GUnitData m_data;
        string m_offset = " ";
        string m_ProjectName = "";
        List<string> m_includePaths;
        List<string> m_LibPaths ;
        bool m_coverageEnabled = true;
        string m_SolnType = "gmake";
        string m_BinPath = Directory.GetCurrentDirectory();
        List<string> m_srcPaths ;
        List<string> m_Libraries;
        string m_SolutionPath;
        List<string> m_commonheaderList;
        private string m_IncludeString = "";
        private string m_buildOptionsString = "";
        private string m_LdFlags = "";
        private string m_libs = "";
        private List<ObjectList> m_objectList = new List<ObjectList>();
        public List<string> m_buildCommandList = new List<string>();
        public SolutionBuilder(ProjectInfo project,GUnitData data)
        {
            m_Project = project;
            m_data = data;
            m_ProjectName = project.m_ProjectName;
            m_includePaths =project.m_SolnData.m_includePaths;
            m_LibPaths = project.m_SolnData.LibPaths;
            m_coverageEnabled = project.m_SolnData.m_coverageEnabled;
            m_SolnType = project.m_SolnData.m_SolnType;
            m_BinPath = project.m_SolnData.m_BinPath;
            m_srcPaths = project.m_SolnData.m_srcPaths;
            m_Libraries = project.m_SolnData.m_Libraries;
            m_SolutionPath = Path.GetDirectoryName(project.m_ProjectPath);
            m_commonheaderList = project.m_SolnData.m_CommonHeadersList;
        }
        public void SolnBldr_BuildSolution(string scriptType)
        {
            SolnBldr_createPremakeScript(scriptType);
            SolnBldr_createGccCommandLine();
        }
        private void SolnBldr_createGccCommandLine()
        {
            string buildDir = Path.GetDirectoryName(m_Project.m_ProjectPath)+"\\Build";
            string objDir = Path.GetDirectoryName(m_Project.m_ProjectPath)+"\\Build\\obj";
            if (Directory.Exists(buildDir) == false)
            {
                Directory.CreateDirectory(buildDir);
            }
            if (Directory.Exists(objDir) == false)
            {
                Directory.CreateDirectory(objDir);
            }
            m_IncludeString = "";
            foreach (string str in m_includePaths.Distinct())
            {
                m_IncludeString += " -I" + str;
            }
            m_buildOptionsString = " -Wall -MMD -MP " + m_IncludeString + " -ffast-math -g -fprofile-arcs -ftest-coverage -x c++";
            for (int i = 0; i < m_commonheaderList.Count; i++)
            {
                m_buildOptionsString += " -include " + Path.GetFileName(m_commonheaderList[i]);
            }
            m_LdFlags = "-fprofile-arcs ";
            foreach (string str in m_LibPaths.Distinct())
            {
                m_LdFlags += " -L" + str;
            }
            m_libs = "";
            foreach (string lib in m_Libraries.Distinct())
            {
                m_libs += " -l" + lib;
            }
            m_objectList.Clear();
            foreach (string str in m_srcPaths.Distinct())
            {
                var s = Path.Combine(Path.GetDirectoryName(m_data.m_Project.m_ProjectPath), str);
                s = Path.GetFullPath(s);
                string fileName = Path.GetFileName(s);
                ObjectList obj = new ObjectList();
                obj.sourceFile = str;
                try
                {
                    if (fileName.Contains('.'))
                    {
                        fileName = fileName.Split('.')[0];
                    }
                }
                catch
                {

                }
                fileName = objDir+"\\"+fileName + ".o";
                fileName = m_data.getReleativePath(fileName);
                obj.objectPath = fileName;
                m_objectList.Add(obj);
            }
            m_buildCommandList.Clear();
            string objects = "";
            foreach(ObjectList obj in m_objectList)
            {
                string command = " " + m_buildOptionsString + " -o " + obj.objectPath + " -c " + obj.sourceFile;
                m_buildCommandList.Add(command);
                objects += " "+obj.objectPath;
            }
            string linkCommand = " -o " + m_BinPath + "/" + m_ProjectName + ".exe " + objects + " " + m_LdFlags + " " + m_libs;
            m_buildCommandList.Add(linkCommand);
        }
        private  void SolnBldr_createPremakeScript(string ScriptType)
        {
            string commonInclude = "";
            for (int i = 0; i < m_commonheaderList.Count; i++)
            {

                if (i != m_commonheaderList.Count - 1)
                {
                    commonInclude += " \"-include " + Path.GetFileName(m_commonheaderList[i]) + "\",";
                }
                else
                {
                    commonInclude += " \"-include " + Path.GetFileName(m_commonheaderList[i])+"\"" ;
                }
            }
            StreamWriter writer = new StreamWriter(m_SolutionPath + "\\premake4.lua");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Premake Script for unittest------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            if (string.IsNullOrWhiteSpace(m_ProjectName) == false)
            {

                writer.WriteLine("solution \"" + m_ProjectName + "\"");
            }
            else
            {
                m_ProjectName = "UTEST";
                writer.WriteLine("solution \"UTEST\"");
            }
            writer.Write(m_offset + "INCLUDE_PATHS={");
            int count = 0;
            foreach (string str in m_includePaths.Distinct())
            {
                if (count == (m_includePaths.Count - 1))
                {
                    writer.Write("\"" + str + "\"");
                }
                else
                {
                    writer.Write("\"" + str + "\"" + ",");
                }
                count++;
            }
            writer.Write("}\n");
            writer.Write(m_offset + "LIB_PATHS={");
            count = 0;
            foreach (string str in m_LibPaths.Distinct())
            {
                if (count == (m_LibPaths.Count - 1))
                {
                    writer.Write("\"" + str + "\"");
                }
                else
                {
                    writer.Write("\"" + str + "\"" + ",");
                }
                count++;
            }
            writer.Write("}\n");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Common Settings------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine(m_offset + "configurations { \"Debug\"}");
            writer.WriteLine(m_offset + "flags {\"FloatFast\" ,\"StaticRuntime\"}");
            if (m_coverageEnabled == true && m_SolnType == "gmake")
            {
                if (commonInclude != "")
                {
                    writer.WriteLine(m_offset + "buildoptions {\"-fprofile-arcs\", \"-ftest-coverage\", '-x c++'," + commonInclude + "}");
                }
                else
                {
                    writer.WriteLine(m_offset + "buildoptions {\"-fprofile-arcs\", \"-ftest-coverage\", '-x c++'}");
                }
                writer.WriteLine(m_offset + "linkoptions {\"-fprofile-arcs\"}");
            }
            writer.WriteLine(m_offset + "includedirs {INCLUDE_PATHS}");
            writer.WriteLine(m_offset + "libdirs {LIB_PATHS}");
            writer.WriteLine(m_offset + "location \"Build\"");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Build configuration------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine(m_offset + "configuration \"Debug\"");
            writer.WriteLine(m_offset + m_offset + "flags {\"Symbols\"}");
            writer.WriteLine(m_offset + m_offset + "libdirs {\"libs\"}");
            writer.WriteLine(m_offset + m_offset + "targetsuffix \"_d\"");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Project Setting------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine(m_offset + "project \"" + m_ProjectName + "\"");
            writer.WriteLine(m_offset + m_offset + "kind \"" + ScriptType + "\"");
            writer.WriteLine(m_offset + m_offset + "language \"C++\"");
            if (string.IsNullOrWhiteSpace(m_BinPath) == false)
            {
                writer.WriteLine(m_offset + m_offset + "targetdir \"" + m_BinPath + "\"");
            }
            else
            {
                writer.WriteLine(m_offset + m_offset + "targetdir \"Bin\"");
            }
            count = 0;
            writer.Write(m_offset + m_offset + "files {");
            foreach (string str in m_srcPaths.Distinct())
            {
                if (count == (m_srcPaths.Count - 1))
                {
                    writer.Write("\"" + str + "\"");
                }
                else
                {
                    writer.Write("\"" + str + "\"" + ",");
                }
                count++;
            }
            writer.Write("}\n");
            foreach (string lib in m_Libraries.Distinct())
            {
                writer.WriteLine(m_offset + m_offset + "links {\"" + lib + "\"}");
            }
            writer.Close();

        }

    }
}
