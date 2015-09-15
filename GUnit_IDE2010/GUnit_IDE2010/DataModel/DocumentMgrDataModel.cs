using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Gunit.Controller;
using Gunit.Ui;

namespace Gunit.DataModel
{
    public class DocumentCollection
    {
        Documentcontroller m_controller;
        Document m_view;
        DocumentDataModel m_model;
        
        public Documentcontroller Controller
        {
            get { return m_controller; }
            set
            {
                m_controller = value;
            }
        }
        public Document View
        {
            get { return m_view; }
            set
            {
                m_view = value;
            }
        }
        public DocumentDataModel Model
        {
            get { return m_model; }
            set
            {
                m_model = value;
            }
        }

    }
    class DocumentMgrDataModel:DataModelBase
    {
        
        public Hashtable m_DocumentHashTable;
        private ListOfCodeLocation m_errorLocation = new ListOfCodeLocation();
        private ListOfCodeLocation m_warningLocation = new ListOfCodeLocation();
        private CodeLocation m_FocusLocation = null;
        private ListofStrings m_CodeCompletionStrings = new ListofStrings();
        private void DocumentMgr_ResetData()
        {
            m_DocumentHashTable.Clear();
            m_errorLocation.Clear();
            m_warningLocation.Clear();
            m_FocusLocation = null;
            m_CodeCompletionStrings.Clear();

        }
        public override void Datamodel_closeProject()
        {
            base.Datamodel_closeProject();
            DocumentMgr_ResetData();
        }
        public override void Datamodel_newProject()
        {
            base.Datamodel_newProject();
            DocumentMgr_ResetData();
        }
        public override void Datamodel_openProject()
        {
            base.Datamodel_openProject();
            DocumentMgr_ResetData();
        }
        /// <summary>
        /// Collection of all Document  MVC
        /// </summary>
        public ListofStrings CodeCompletionWords
        {
            get { return m_CodeCompletionStrings; }
            set
            {
                m_CodeCompletionStrings = value;

            }
        }
        public DocumentMgrDataModel():base()
        {
            m_DocumentHashTable = new Hashtable();
          
        }
       
        public CodeLocation FocusLocation
        {
            get
            {
                return m_FocusLocation;
            }
            set
            {
                m_FocusLocation = value;
                FirePropertyChange("FocusLocation");
            }
        }
        public ListOfCodeLocation ErrorLocation
        {
            get
            {
                return m_errorLocation;
            }
            set
            {
                m_errorLocation = value;
               
            }
        }
        public ListOfCodeLocation WarningLocation
        {
            get
            {
                return m_warningLocation;
            }
            set
            {
                m_warningLocation = value;
               
            }
        }

    }
}
