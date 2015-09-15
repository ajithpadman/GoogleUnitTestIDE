using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClangSharp;
namespace Gunit.DataModel
{
    public enum LanguageType
    {
        c,
        cpp,
        cs,
        css,
        html,
        java
       
    }
    public class DocumentDataModel : DataModelBase
    {
        private string m_FileText = "";
        private string m_fileName = "";
        private string m_SearchText = "";
        private int m_currentSearchStart = 0;
        private bool m_IsDocumentActive = false;
        private CodeLocation m_CurrentLine = null;
      
        private ListofStrings m_CodeCompletionStrings = new ListofStrings();
        private ListOfCodeLocation m_errorLines = new ListOfCodeLocation();
        private ListOfCodeLocation m_WarningLines = new ListOfCodeLocation();
        private ListOfCodeLocation m_OlderrorLines = new ListOfCodeLocation();
        private ListOfCodeLocation m_OldWarningLines = new ListOfCodeLocation();
        public void Document_ResetData()
        {
            m_FileText = "";
            m_fileName = "";
            m_SearchText = "";
            m_currentSearchStart = 0;
            m_IsDocumentActive = false;
            m_CurrentLine = null;
            m_CodeCompletionStrings.Clear();
            m_errorLines.Clear();
            m_WarningLines.Clear();
            m_OlderrorLines.Clear();
            m_OldWarningLines.Clear();
        }
        public override void Datamodel_closeProject()
        {
            base.Datamodel_closeProject();
            Document_ResetData();
        }
        public override void Datamodel_openProject()
        {
            base.Datamodel_openProject();
            Document_ResetData();
        }
        public override void Datamodel_newProject()
        {
            base.Datamodel_newProject();
            Document_ResetData();
        }

        /// <summary>
        /// Constructor for the Document DataModel
        /// </summary>
        public DocumentDataModel(string fileName):base()
        {
            m_fileName = fileName;
            CurrentFile = fileName;
            
        }
        public ListofStrings CodeCompletionWords
        {
            get { return m_CodeCompletionStrings; }
            set {
                  m_CodeCompletionStrings = value;
                  
                 }
        }
        
        public string SearchText
        {
            get { return m_SearchText; }
            set
            {
                m_SearchText = value;
                FirePropertyChange("Search");
            }
        }
        public int SearchStart
        {
            get { return m_currentSearchStart; }
            set
            {
                m_currentSearchStart = value;
                
            }
        }
        public ListOfCodeLocation CurrentErrorLines
        {
            get { return m_OlderrorLines; }
            set
            {
                m_OlderrorLines = value;
               
            }

        }
        public ListOfCodeLocation CurrentWarningLines
        {
            get { return m_OldWarningLines; }
            set
            {
                m_OldWarningLines = value;

            }

        }
        public ListOfCodeLocation DocumentErrors
        {
            get { return m_errorLines; }
            set
            {
                m_errorLines = value;
                //FirePropertyChange("ErrorUpdate");
            }
        }
        public ListOfCodeLocation DocumentWarnings
        {
            get { return m_errorLines; }
            set
            {
                m_errorLines = value;
                //FirePropertyChange("WarningUpdate");
            }
        }
        public CodeLocation CurrentLine
        {
            get { return m_CurrentLine; }
            set 
            { 
                m_CurrentLine = value;
                FirePropertyChange("FocusLine");
            }
        }
        /// <summary>
        /// Content of the File
        /// </summary>
        public string Text
        {
            get
            {
                return m_FileText;
            }
            set
            {
                m_FileText = value;
                IsDirty = true;
                FirePropertyChange("Text");
            }
        }
        public bool IsActive
        {
            get { return m_IsDocumentActive; }
            set {
                  m_IsDocumentActive = value;
                  FirePropertyChange("IsActive");
                }
        }
        /// <summary>
        /// Absolute path to the file
        /// </summary>
        public string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
                FirePropertyChange("FileName");
            }
        }
     
       
    }
}
