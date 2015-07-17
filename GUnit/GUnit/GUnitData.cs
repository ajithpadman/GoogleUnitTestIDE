using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.IO;
namespace GUnit
{
    public enum GUnitProjectType
    {
        C_PROJECT,
        CPP_PROJECT
    };
    public enum GUnitTestSuitType
    {
        NORMAL_TESTFIXTURE,
        VALUE_PARAM_TEST
    };
  
    public class ClassDef
    {
        public string ClassName = "";
        public List<memberDeclairation> objectList = new List<memberDeclairation>();
        public List<FunctionDeclairation> funcList = new List<FunctionDeclairation>();
    }
    public class LineStatus
    {
        public UInt32 m_lineNumber;
        public bool m_isExecutable;
        public UInt32 m_ExecutionCount;

    }
    public class Coverage
    {
        public string m_fileName = "";
        public UInt32 m_LineCount = 0;
        public  List<LineStatus> m_LineStatus = new List<LineStatus>();
      
    }
    public class CoverageFiles
    {
        public string m_srcFile = "";
        public string m_gcovFile = "";
    }
    public class UnittestCase
    {
        public string m_FileName = "";
        public string m_TestCaseName = "";
        public bool m_Result;
        
    }
    public class UnitTestSuit
    {
        public string m_FileName = "";
        public string m_TestSuitName = "";
        public GUnitTestSuitType m_testSuitType = GUnitTestSuitType.NORMAL_TESTFIXTURE;
        public List<UnittestCase> m_TestCases = new List<UnittestCase>();
    }
    public class UnitTestData
    {
        public List<UnitTestSuit> m_TestSuits = new List<UnitTestSuit>();
    }
   
    public class FileRead
    {
        public string content = "";
        public string path = "";
    }
    class orderedVariables
    {
        public uint order = 0;
        public string declairation = "";
    }
    public class orderedFunctions
    {
        public uint order = 0;
        public string declairation = "";
    }
    public class FunctionDeclairation
    {
        public string returnvalue = "";
        public string functionName = "";
        public List<string> args = new List<string>();
    }

    public class EnumValue
    {
        public string name = "";
        public string value = "";

    }
    public class EnumDef
    {
        public string m_EnumName = "";
        public List<EnumValue> m_EnumValues = new List<EnumValue>();
         
    }
    public class TypeDefs
    {
        public string standardDefine = "";
        public string userDefine = "";
    }
     public class memberDeclairation
    {
        public string m_AccessScope = "";
        public string m_DataType = "";
        public string m_VariableName = "";
    }
    public class StructureDefine
    {
        public string m_StructureName = "";
        public List<memberDeclairation> objectList = new List<memberDeclairation>();
    }
    public class PreprocessorDirectives
    {
        public string m_Define = "";
        public string m_Value = "";
    }
    public class ClassDefine
    {
        public string m_className = "";
        public List<string> m_parentClass = new List<string>();
        public List<memberDeclairation> m_dataMembers = new List<memberDeclairation>();
    }
    public class FunctionalInterface
    {
        
        public string m_FileName = "";
        public int m_LineNo = 0;
        public string m_ClassName = "";
        public string m_AccessScope = "";
        public string m_Signature = "";
        public int m_argumentCount = 0;
        public string m_FunctionName = "";
        public string m_ReturnType = "";
        public bool m_IsVirtual = false;
        public List<string> m_argumentTypes = new List<string>();
        public string args_string = "";
        public string passed_args = "";
        public List<FunctionalInterface> m_CalledFunctionList = new List<FunctionalInterface>();
       
    }
    public class DataInterface
    {
        public List<EnumDef> m_Enumerations = new List<EnumDef>();
        public List<TypeDefs> m_UserDefinedTypes = new List<TypeDefs>();
        public List<StructureDefine> m_Structures = new List<StructureDefine>();
        public List<StructureDefine> m_Unions = new List<StructureDefine>();
        public List<PreprocessorDirectives> m_Defines = new List<PreprocessorDirectives>();
        public List<ClassDefine> m_Classes = new List<ClassDefine>();
        

        public List<string> m_enumValues = new List<string>();
        public List<string> m_Typedefs = new List<string>();
        public List<string> m_structuresNames = new List<string>();
        public List<string> m_UnionNames = new List<string>();
        public List<string> m_MacroNames = new List<string>();
        public List<string> m_ClassNames = new List<string>();
        public List<string> m_GlobalVariables = new List<string>();

    } 


    public class UnitInfo
    {
        public string m_className = "";
        public List<FunctionalInterface> m_functionPrototypeList = new List<FunctionalInterface>();
        public List<FunctionalInterface> m_MockFunctionList = new List<FunctionalInterface>();
        public List<FunctionalInterface> m_functionDefinitionList = new List<FunctionalInterface>();
        public string m_fileName = "";
        public bool m_IsClass = true;


    }
    public class FileInfo
    {
        public string m_fileName = "";
        public FileOpen m_Document = null;
        public string m_text = "";
        public DataInterface m_DataInterfaceList = new DataInterface();
        public List<UnitInfo> m_UnitList = new List<UnitInfo>();
        public bool m_IsDirty = false;
        public Coverage m_CoverageInfo = new Coverage();
        public FileSystemWatcher m_fileWatcher = new FileSystemWatcher();
        public string m_lastWriteTime = "";
        
    }
    public class ProjectInfo
    {
        public List<string> m_HeaderFileNames = new List<string>();
        public List<string> m_SourceFileNames = new List<string>();
        public string m_ProjectName = "MyProject";
        public string m_ProjectPath = "MyProject";
        public SolutionBuilderData m_SolnData = new SolutionBuilderData();

        
    }
    public class SolutionBuilderData
    {

        public string m_SolutionPath;
        public List<string> m_includePaths = new List<string>();
        public List<string> LibPaths = new List<string>();
        public bool m_coverageEnabled = true;
        public string m_SolnType = "gmake";
        public string m_BinPath = "./";
        public string m_ActualBinPath = Directory.GetCurrentDirectory();
        public List<string> m_srcPaths = new List<string>();
        public List<string> m_Libraries = new List<string>();
        public List<string> m_CommonHeadersList = new List<string>();
    }
    public class GUnitData
    {
        private string m_fileName = "";
        private GUnitProjectType m_ProjectType;
        public bool m_IsProjectChanged = false;
        public List<UnitInfo> m_UnitsForGeneration = new List<UnitInfo>();
        UnitInfo m_TestUnit = new UnitInfo();
        public Hashtable m_ProjectHashTable = new Hashtable();
        public ProjectInfo m_Project = new ProjectInfo();
        public List<string> m_standardTypes = new List<string>();
        public List<string> m_standardUtestMacros = new List<string>();
        public List<string> m_standardQualifiers = new List<string>();
        public UnitTestData m_UtestData = new UnitTestData();
        public GUnitProjectType ProjectType { get { return m_ProjectType; } set { m_ProjectType = value; } }
        public string CurrentFileName { get{return m_fileName;} set{m_fileName = value;} }
        public int m_ErrorCount = 0;
        public int m_WarningCount = 0;
        public List<string> m_buildErrors = new List<string>();
        public List<string> m_buildWarnings = new List<string>();
        public GUnitData()
        {
            
        }
        public GUnitData(GUnitProjectType type)
        {
            m_ProjectType = type;
        }
        public string getReleativePathWith(string SelectedPath,string rootPath)
        {
            string relPath = ".";
            System.Uri path = new Uri(SelectedPath);
            System.Uri cur = new Uri(rootPath + "\\");
            relPath = cur.MakeRelativeUri(path).ToString();
            if (string.IsNullOrWhiteSpace(relPath))
            {
                relPath = ".";
            }
            return relPath;
        }
        public string getReleativePath(string SelectedPath)
        {
            string relPath = ".";
            System.Uri path = new Uri(SelectedPath);
            System.Uri cur = new Uri(Path.GetDirectoryName(this.m_Project.m_ProjectPath )+ "\\");
            relPath = cur.MakeRelativeUri(path).ToString();
            if (string.IsNullOrWhiteSpace(relPath))
            {
                relPath = ".";
            }
            return relPath;
        }
        public void GUnitDat_UpdateSolutionData()
        {
            m_Project.m_SolnData.m_coverageEnabled = true;
            m_Project.m_SolnData.m_SolnType = "gmake";
            foreach (string src in m_Project.m_SourceFileNames.Distinct())
            {
                if (File.Exists(src))
                {
                    m_Project.m_SolnData.m_srcPaths.Add(getReleativePath(src));
                }
            }
         
        }
        public void GUnitDat_AddCommonHeaders(string path, bool noRelative = false)
        {
            if (File.Exists(path))
            {
                if (string.IsNullOrWhiteSpace(path) == false)
                {
                    string incPath = "";
                    if (noRelative == true)
                    {
                        incPath = path;
                    }
                    else
                    {
                        incPath = getReleativePath(path);
                    }

                    if (m_Project.m_SolnData.m_CommonHeadersList.Contains(incPath) == false)
                    {
                        m_Project.m_SolnData.m_CommonHeadersList.Add(incPath);
                    }
                }
            }
        }
        public void GUnitDat_AddIncludePaths(string path,bool noRelative = false)
        {
         

            if (string.IsNullOrWhiteSpace(path) == false)
            {
                string incPath = "";
                if (noRelative == true)
                {
                    incPath = path;
                }
                else
                {
                    incPath = getReleativePath(path);
                }
               
                if (m_Project.m_SolnData.m_includePaths.Contains(incPath) == false)
                {
                    m_Project.m_SolnData.m_includePaths.Add(incPath);
                }
            }
        }
        public void GUnitDat_AddLibPaths(string path, bool noRelative = false)
        {
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                string incPath = "";
                if (noRelative == true)
                {
                    incPath = path;
                }
                else
                {
                    incPath = getReleativePath(path);
                }
                if (m_Project.m_SolnData.LibPaths.Contains(incPath) == false)
                {
                    m_Project.m_SolnData.LibPaths.Add(incPath);
                }
            }
        }
        public void GUnitDat_AddBinPath(string path, bool noRelative = false)
        {
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                m_Project.m_SolnData.m_ActualBinPath = path;
                if (noRelative == true)
                {
                    m_Project.m_SolnData.m_BinPath = path;
                }
                else
                {
                    m_Project.m_SolnData.m_BinPath = getReleativePath(path);
                }
            }
        }
        public void GUnitDat_AddHeaderFiles(string path)
        {
            if (File.Exists(path))
            {
                if (m_Project.m_HeaderFileNames.Contains(path) == false)
                {
                    m_Project.m_HeaderFileNames.Add(path);
                }
            }
        }
        public void GUnitDat_AddSourceFiles(string path)
        {
            if (File.Exists(path))
            {
                if (m_Project.m_SourceFileNames.Contains(path) == false)
                {
                    m_Project.m_SourceFileNames.Add(path);
                }
            }
        }
      
        public void GUnitDat_AddLibNames(string path)
        {
            if (string.IsNullOrWhiteSpace(path) == false)
            {
                if (m_Project.m_SolnData.m_Libraries.Contains(path) == false)
                {
                    m_Project.m_SolnData.m_Libraries.Add(path);
                }
            }
        }
        public void GUnitData_UpdateProjectTable(string filepath , FileInfo fileInformation)
        {
            if (string.IsNullOrWhiteSpace(filepath) == false)
            {
                m_ProjectHashTable[filepath] = fileInformation;
                
            }
        }
        public FileInfo GUnitData_getFileInformation(string filepath)
        {
            return m_ProjectHashTable[filepath] as FileInfo;
        }
        public void GUnitDat_removeEntry(string filepath)
        {
            if (m_ProjectHashTable.ContainsKey(filepath))
            {
                m_ProjectHashTable.Remove(filepath);
            }
            if (m_Project.m_SourceFileNames.Contains(filepath))
            {
                m_Project.m_SourceFileNames.Remove(filepath);
            }
            if (m_Project.m_HeaderFileNames.Contains(filepath))
            {
                m_Project.m_HeaderFileNames.Remove(filepath);
            }
        }


        
     
       
        
         

    }
}
