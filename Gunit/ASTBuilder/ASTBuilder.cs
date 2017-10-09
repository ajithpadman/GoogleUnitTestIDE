using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClangSharp;
using ASTBuilder.Interfaces;
using ASTBuilder.ConcreteClasses;

namespace ASTBuilder
{
  
    public class ASTBuilder:IAST,IDisposable
    {
        ICCodeDescription m_Description = new CCodeDescriptioncs();
        List<string> m_CommandLine = new List<string>();
        List<string> m_IncludePaths = new List<string>();
        List<string> m_defines = new List<string>();
        List<string> m_preIncludeFiles = new List<string>();
        TranslationUnit m_unit = null;
        IDataParser m_DataParser;
        Index m_Index = null;
        public ICCodeDescription CodeDescription
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }
        public ASTBuilder(string fileName)
        {
            m_DataParser = new DataParser();
            m_Description.FileName = fileName;
            m_Index = new Index(true, true);
        }
        public bool ParseFile()
        {
            try
            {

                m_unit = createTranslationUnit(m_Description.FileName, GetClangCommandLine());
                if (null != m_unit)
                {
                    return createAst();
                }
                else
                {
                    return false;
                }
             
                
            }
            catch
            {
                return false;
            }
           
        }
        protected bool createAst()
        {
            Cursor root = null;
            try
            {
                if (m_unit != null)
                {
                    root = m_unit.Cursor;
                     IEnumerable<ClangSharp.Cursor> childList = from child in root.Children
                                                               // where (child.Location.File.Name.Replace("/", "\\") == m_Description.FileName)
                                                           select child;
                     IEnumerable<ClangSharp.Cursor> desendants = from child in root.Descendants
                                                               // where (child.Location.File.Name.Replace("/", "\\") == m_Description.FileName)
                                                                select child;
                     foreach (Cursor cursor in childList)
                     {
                         visitChildren(cursor, CodeDescription);
                     }
                     foreach (Cursor cursor in desendants)
                     {
                         visitFunctions(cursor, CodeDescription);
                     }

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        protected void visitFunctions(Cursor cursor, ICCodeDescription description)
        {
            switch (cursor.Kind)
            {
                case CursorKind.FunctionDecl:
                    description.Functions.Add(m_DataParser.visitFunctionType(cursor));
                    break;
                default:
                    break;
            }
        }
        protected void visitChildren(Cursor cursor,ICCodeDescription description)
        {
            switch (cursor.Kind)
            {
                case CursorKind.VarDecl:
                   
                       ICVariable variable = m_DataParser.visitVariableType(cursor);
                       variable.AccessSpecifier = CAccessSpecifier.Global;
                       description.GlobalVariables.Add(variable);
                    
                    break;
               
                default:
                    break;
            }
        }
       
        protected ICDataType visitDataType(ClangSharp.Type dataType)
        {
            ICDataType data = null;
            switch (dataType.TypeKind)
            {
                case ClangSharp.Type.Kind.BlockPointer:
                case ClangSharp.Type.Kind.Pointer:
                case ClangSharp.Type.Kind.MemberPointer:
                  data =   visitPointer(dataType);
                    break;
                case ClangSharp.Type.Kind.Bool:
                case ClangSharp.Type.Kind.CharU:
                case ClangSharp.Type.Kind.UChar:
                case ClangSharp.Type.Kind.Char16:
                case ClangSharp.Type.Kind.Char32:
                case ClangSharp.Type.Kind.UShort:
                case ClangSharp.Type.Kind.UInt:
                case ClangSharp.Type.Kind.ULong:
                case ClangSharp.Type.Kind.ULongLong:
                case ClangSharp.Type.Kind.UInt128:
                case ClangSharp.Type.Kind.CharS:
                case ClangSharp.Type.Kind.SChar:
                case ClangSharp.Type.Kind.WChar:
                case ClangSharp.Type.Kind.Short:
                case ClangSharp.Type.Kind.Int:
                case ClangSharp.Type.Kind.Long:
                case ClangSharp.Type.Kind.LongLong:
                case ClangSharp.Type.Kind.Int128:
                case ClangSharp.Type.Kind.Float:
                case ClangSharp.Type.Kind.Double:
                case ClangSharp.Type.Kind.LongDouble:
                    break;
                case ClangSharp.Type.Kind.Record:
                    
                    break;
                case ClangSharp.Type.Kind.Enum:
                    break;
                case ClangSharp.Type.Kind.Typedef:
                    break;
                case ClangSharp.Type.Kind.Unexposed:
                    if (dataType.Declaration.Kind == CursorKind.StructDecl)
                    {
                       
                    }
                    else if (dataType.Declaration.Kind == CursorKind.EnumDecl)
                    {
                      
                    }
                    break;


            }
            return data;
        }
        protected IPointer visitPointer(ClangSharp.Type dataType)
        {
            IPointer pointer = null;
            return pointer;
        }
        protected List<string> GetClangCommandLine()
        {
            List<string> l_CommandLine = new List<string>();

            l_CommandLine.Add("-fno-ms-compatibility");
            l_CommandLine.Add("-std=c++11");

            foreach (string str in IncludePaths)
            {
                l_CommandLine.Add("-I");
                l_CommandLine.Add(str);
            }

            foreach (string str in PreIncludeFiles)
            {
               
                    l_CommandLine.Add("-include");
                    l_CommandLine.Add(str);
               

            }
           
            foreach (string macro in Defines)
            {
                l_CommandLine .Add("-D");
                l_CommandLine .Add(macro);
            }
            l_CommandLine.Add("-Wall");
            l_CommandLine.Add("-MMD");
            l_CommandLine.Add("-MP");
            l_CommandLine.Add("-x");
            l_CommandLine.Add("c++");

            return l_CommandLine;
        }
        protected TranslationUnit createTranslationUnit(string fileName, List<string> cmdLine)
        {
            try
            {
                return m_Index.CreateTranslationUnit(fileName, cmdLine.ToArray(), null, TranslationUnitFlags.DetailedPreprocessingRecord);
            }
            catch
            {
                return null;
            }
           
        }


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

        public List<string> Defines
        {
            get
            {
                return m_defines;
            }
            set
            {
                m_defines = value;
            }
        }

        public List<string> PreIncludeFiles
        {
            get
            {
                return m_preIncludeFiles;
            }
            set
            {
                m_preIncludeFiles = value;
            }
        }

        public void Dispose()
        {
            
            if (m_Index != null)
            {
                m_Index.Dispose();
                m_Index = null;

            }
            if (m_unit != null)
            {
                m_unit.Dispose();
                m_unit = null;
            }
        }
    }
}
