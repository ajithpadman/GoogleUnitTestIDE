using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Gunit.DataModel
{
    /// <summary>
    /// Different Sessions possible in the IDE
    /// </summary>
    public enum Sessions
    {
        IDLE,
        NEW_PROJECT,
        OPEN_PROJECT,
        OPEN_PROJECT_COMPLETE,
        CLOSE_PROJECT, 
        SAVE_PROJECT,
        OPEN_FILE,
        FOCUS_FILE,
        ADD_FILE,
        REMOVE_FILE,
        CLOSE_FILE,
        PARSER_RUNNING,
        PARSER_COMPLETE,
        BUILD_RUNNING,
        BUILD_COMPLETE,
        TEST_RUNNING,
        TEST_RUNCOMPLETE,
        COVERAGE_RUN,
        COVERAGE_RUNCOMPLETE

    }
    /// <summary>
    /// Different Types of Files Possible in the IDE
    /// </summary>
    public enum FileType
    {
        SOURCE_FILE,
        HEADER_FILE,
        PROJECT_HEADER_FILE
    }
    public enum ParserStatus
    {
        ParserRunning,
        ParserComplete
    }
    /// <summary>
    /// The Base calss for all the Model Classes
    /// Implemeting the INotifyPropertyChanged Interface
    /// </summary>
    public class DataModelBase : INotifyPropertyChanged 
    {
        private bool m_Isdirty;
        private string m_currentFile;
        private Sessions m_session;
        ParserStatus m_ParserStatus = ParserStatus.ParserComplete;
        protected event PropertyChangedEventHandler m_propertyChanged = delegate { };
        public DataModelBase()
        {
            m_Isdirty = false;
            m_session = Sessions.IDLE;
        }
        public string CurrentFile
        {
            get
            {
                return m_currentFile;
            }
            set
            {
                m_currentFile = value;
            }
        }
        public bool IsDirty
        {
            get
            {
                return m_Isdirty;
            }
            set
            {
                if (value != m_Isdirty)
                {
                    m_Isdirty = value;
                    FirePropertyChange("Dirty");
                }
               
            }
        }
        public ParserStatus ParserStatus
        {
            get { return m_ParserStatus; }
           
        }
        public Sessions Session
        {
            get { return m_session;}
            set {
                  //  if (value != m_session)
                    {
                        m_session = value;
                        FirePropertyChange("SessionChange");
                    }
                }
        }
        #region  Member methods
        /// <summary>
        /// Event for PropertChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { m_propertyChanged += value; }
            remove { m_propertyChanged -= value; }
        }
        /// <summary>
        /// Fire the Property Change event
        /// </summary>
        /// <param name="propertyName">Name of the Proerty Changed</param>
        protected void FirePropertyChange(string propertyName)
        {
            if (null != m_propertyChanged)
            {
                m_propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// Get the relative path with respect to the Root path
        /// </summary>
        /// <param name="SelectedPath">Absolute path for which relative path needs to be foundout</param>
        /// <param name="rootPath">Absolute path to the Root Directory</param>
        /// <returns>Relative Path</returns>
        public static string getReleativePathWith(string SelectedPath, string rootPath)
        {
            string relPath = "";
            try
            {

                System.Uri path = new Uri(SelectedPath);
                System.Uri cur = new Uri(rootPath + "\\");
                relPath = cur.MakeRelativeUri(path).ToString();
                if (string.IsNullOrWhiteSpace(relPath))
                {
                    relPath = ".";
                }
            }
            catch
            {
                
            }

            return relPath;
        }
        public virtual void Datamodel_addFile()
        {

        }
        public virtual void Datamodel_openProject()
        {

        }
        public virtual void Datamodel_closeProject()
        {

        }
        public virtual void Datamodel_newProject()
        {

        }
        public virtual void Datamodel_saveProject()
        {

        }
        public virtual void Datamodel_openProjectComplete()
        {

        }
        public virtual void Datamodel_openFile(string fileName)
        {

        }
        public virtual void Datamodel_closeFile(string fileName)
        {

        }
        public virtual void Datamodel_removeFile(string fileName)
        {

        }
        public virtual void Datamodel_ParserRunning()
        {
            m_ParserStatus = DataModel.ParserStatus.ParserRunning;
        }
        public virtual void Datamodel_ParserComplete()
        {
            m_ParserStatus = DataModel.ParserStatus.ParserComplete;
        }
        #endregion

    }
}
