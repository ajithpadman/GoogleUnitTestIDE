using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUnit_IDE2010.CodeGenerator
{
    public enum  PremakeSolutionType
    {
        Gmake,
        VS2010
    }


    public class PremakeScriptBuilder
    {
        
        ProjectDataModel m_prjModel = null;
        PremakeSolutionType m_solnType = PremakeSolutionType.Gmake;
        OutputTypes m_outputType = OutputTypes.ConsoleApplication;
        string m_projectPath = "";
        string m_outputPath = "";
        string m_buildPath = "";
        public PremakeScriptBuilder(ProjectDataModel model, PremakeSolutionType solntype, OutputTypes outputType)
        {
            m_prjModel = model;
            m_solnType = solntype;
            m_outputType = outputType;
            m_outputPath = Path.GetDirectoryName(m_prjModel.ProjectPath) + "\\" + m_prjModel.ProjectName + "_Scripts";
            m_projectPath = Path.GetDirectoryName(m_prjModel.ProjectPath);
            m_buildPath = DataModelBase.getReleativePathWith(m_prjModel.BuildPath, m_projectPath);

        }
        private string getRelative(List<string> list)
        {
            ListofStrings relativeIncludes = new ListofStrings();
            foreach(string include in list)
            {
                relativeIncludes += "\""+ DataModelBase.getReleativePathWith(include, m_projectPath) + "\"";
            }
            return String.Join(",", relativeIncludes.ToArray());
        }
      
        private string getRelativeWithAppendString(List<string> list,string append)
        {
            ListofStrings relativeIncludes = new ListofStrings();
            foreach (string include in list)
            {
                relativeIncludes += "\"" + append + " " + Path.GetFileName(include) + "\"";
            }
            return String.Join(",", relativeIncludes.ToArray());
        }
        private string Includes
        {
            get
            {
                return getRelative(m_prjModel.IncludePaths);

            }
        }
        private string Projectheaders
        {
            get
            {
                if(m_solnType == PremakeSolutionType.Gmake)
                return getRelativeWithAppendString(m_prjModel.ProjectHeaderFiles,"-include");
                else
                    return getRelativeWithAppendString(m_prjModel.ProjectHeaderFiles, "/FI");

            }
        }
        private string LibraryPath
        {
            get
            {
                return getRelative(m_prjModel.LibraryPaths);

            }
        }
        private string SrcPath
        {
            get
            {
                return getRelative(m_prjModel.SourceFiles);

            }
        }
        private string BuildOptions
        {
            get
            {
                if (m_solnType == PremakeSolutionType.Gmake)
                {
                    return "\"-fprofile-arcs\", \"-ftest-coverage\", '-x c++'";
                }
                else
                {
                    return "";
                }
            }
        }

        public string  createPremakeScript()
        {
            string commonInclude = Projectheaders;
            string scriptPath = m_projectPath + "\\premake4.lua";
            if (Directory.Exists(m_outputPath) == false)
                Directory.CreateDirectory(m_outputPath);

            CodeWriter writer = new CodeWriter(scriptPath);
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Premake Script for unittest------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            if (string.IsNullOrWhiteSpace(m_prjModel.ProjectName) == false)
            {

                writer.WriteLine("solution \"" + m_prjModel.ProjectName + "\"");
            }
            else
            {

                writer.WriteLine("solution \"UTEST\"");
            }
            writer.WriteCodeLine(1, "INCLUDE_PATHS={");
            writer.WriteCodeLine(2, Includes);
            writer.WriteCodeLine(1, "}\n");
            writer.WriteCodeLine(1, "LIB_PATHS={");
            writer.WriteCodeLine(2, LibraryPath);
            writer.WriteCodeLine(1, "}");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Common Settings------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteCodeLine(1, "configurations { \"Debug\"}");
            writer.WriteCodeLine(1, "flags {\"FloatFast\" ,\"StaticRuntime\"}");
            if (commonInclude != "")
            {
                writer.WriteCodeLine(1, "buildoptions {"+ BuildOptions+"," + commonInclude + "}");
            }
            else
            {
                writer.WriteCodeLine(1, "buildoptions {"+ BuildOptions + "}");
            }
            if (m_solnType == PremakeSolutionType.Gmake)
            {
                writer.WriteCodeLine(1, "linkoptions {\"-fprofile-arcs\"}");
            }
        
            writer.WriteCodeLine(1, "includedirs {INCLUDE_PATHS}");
            writer.WriteCodeLine(1, "libdirs {LIB_PATHS}");
            writer.WriteCodeLine(1, "location \""+ m_buildPath + "\"");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Build configuration------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteCodeLine(1, "configuration \"Debug\"");
            writer.WriteCodeLine(2, "flags {\"Symbols\"}");
            writer.WriteCodeLine(2, "libdirs {\"libs\"}");
            writer.WriteCodeLine(2,"targetsuffix \"_d\"");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteLine("-----------Project Setting------------------------");
            writer.WriteLine("------------------------------------------------------------------");
            writer.WriteCodeLine(1, "project \"" + m_prjModel.ProjectName + "\"");
            if (m_outputType == OutputTypes.ConsoleApplication)
            {
                writer.WriteCodeLine(2, "kind \"ConsoleApp\"");
            }
            else if (m_outputType == OutputTypes.SharedLibrary)
            {
                writer.WriteCodeLine(2, "kind \"SharedLib\"");
            }
            writer.WriteCodeLine(2, "language \"C++\"");
            writer.WriteCodeLine(2, "targetdir \"Bin\"");
            
            
            writer.WriteCodeLine(2,"files {");
            writer.WriteCodeLine(3, SrcPath);
            writer.WriteCodeLine(2,"}");
         
            foreach (string lib in m_prjModel.LibraryNames)
            {
                writer.WriteCodeLine(2,"links {\"" + lib + "\"}");
            }
            writer.Close();
            return scriptPath;
        }

    }
}
