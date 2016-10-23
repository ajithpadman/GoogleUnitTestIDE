using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using CPPASTBuilder.Interfaces;
using CPPASTBuilder.Implementation;
using System.Reflection;
namespace CPPParser
{
 
    public class CPPParser:ICppParser
    {
        ICppCodeDescription m_codeDescription = null;
        CPPASTBuilder.CPPASTBuilder m_CPPParser;
        IClangSettings ParserSettings = null;
        ICGunitHost m_host;
        CPPParserUi m_ParserFrame;
        public CPPParser()
        {

        }

        public PluginType PluginType
        {
            get
            {
                return GUnitFramework.Interfaces.PluginType.Parser;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string PluginName
        {
            get
            {
                return "C++Parser " + Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ICGunitHost Owner
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
            return true;
        }

        public bool HandleProjectSession(ProjectStatus status)
        {
            return true;
        }

        public void Show(WeifenLuo.WinFormsUI.Docking.DockPanel dock, WeifenLuo.WinFormsUI.Docking.DockState state)
        {
            m_ParserFrame = new CPPParserUi(this);
            m_ParserFrame.Show(dock, state);
        }


        public ICppCodeDescription parseFile(string fileName)
        {

            m_codeDescription = new CppCodeDescription();
            ParserSettings = new ClangSettings();
            ParserSettings.Defines.AddRange(Owner.ProjectData.Defines);
            ParserSettings.IncludePaths.AddRange(Owner.ProjectData.IncludePaths);
            ParserSettings.LibNames.AddRange(Owner.ProjectData.LibNames);
            ParserSettings.LibPaths.AddRange(Owner.ProjectData.LibPaths);
            ParserSettings.PreIncludes.AddRange(Owner.ProjectData.PreIncludes);

            m_CPPParser = new CPPASTBuilder.CPPASTBuilder(ParserSettings);
            m_codeDescription = m_CPPParser.ParseFile(fileName);
            return m_codeDescription;
        
        }
    }
}
