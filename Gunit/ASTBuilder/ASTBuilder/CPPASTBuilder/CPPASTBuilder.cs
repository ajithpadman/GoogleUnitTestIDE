using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPPASTBuilder.Interfaces;
using CPPASTBuilder.Implementation;
using ClangSharp;
namespace CPPASTBuilder
{
    public class CPPASTBuilder
    {
        IClangSettings m_ProjectSettings = null;
        ICppCodeDescription m_CodeDescription = null;
        Index m_Index = null;
        TranslationUnit m_unit = null;
        CppParser m_AstParser = new CppParser();
        public CPPASTBuilder(IClangSettings settings)
        {
            m_ProjectSettings = settings;
            m_Index = new Index(true, true);
        }
        public ICppCodeDescription ParseFile(string fileName)
        {
            try
            {
                if(null != m_ProjectSettings)
                {
                    m_unit = createTranslationUnit(fileName, m_ProjectSettings.GetClangCommandLine());
                    if (null != m_unit)
                    {
                        return createAst(m_unit,fileName);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                     return null;
                }


            }
            catch
            {
                return null;
            }

          
        }
        protected ICppCodeDescription createAst( TranslationUnit unit,string fileName)
        {
            Cursor root = null;
            if(null!= unit)
            {
                m_CodeDescription = new CppCodeDescription();
                m_CodeDescription.FileName = fileName;
                root = unit.Cursor;
                IEnumerable<ClangSharp.Cursor> childList = from child in root.Children
                                                           where (child.Location.File.Name.Replace("/", "\\") == fileName)
                                                           select child;
                IEnumerable<ClangSharp.Cursor> desendants = from child in root.Descendants
                                                            where (child.Location.File.Name.Replace("/", "\\") == fileName)
                                                            select child;
                foreach (Cursor cursor in childList)
                {
                    visitChildren(cursor, m_CodeDescription);
                }
            }
            else
            {

            }
            return m_CodeDescription;

        }
        protected void visitChildren(Cursor cursor, ICppCodeDescription description)
        {
            IClass Class = null;
            INamespace nameSpace = null;
            switch (cursor.Kind)
            {
                case CursorKind.ClassDecl:
                case CursorKind.ClassTemplate:
                    Class = visitClass(cursor);
                    if (null != Class)
                    {
                        description.Classes.Add(Class);
                    }
                    break;
             
                
                case CursorKind.Namespace:
                    
                    nameSpace = visitNameSpace(cursor);
                    if (null != nameSpace)
                    {
                        description.NameSpaces.Add(nameSpace);
                    }
                    break;
                default:
                    break;
            }
        }
     
        IClass visitClass(Cursor cursor)
        {
          
           return  m_AstParser.ParseClass( cursor);
           
        }
        INamespace visitNameSpace(Cursor cursor)
        {
            
            return m_AstParser.ParseNameSpace( cursor);
            
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

    }
}
