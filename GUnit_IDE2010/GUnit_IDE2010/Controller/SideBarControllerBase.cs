using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.Ui;
using Gunit.DataModel;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
namespace Gunit.Controller
{
    public class SideBarControllerBase
    {
        
        protected GUnitSideBarBase m_view;
        protected DataModelBase m_model;
        public SideBarControllerBase(GUnitSideBarBase view,DataModelBase model)
        {
            m_view = view;
            m_model = model;
            
        }
        /// <summary>
        /// start the View From controller
        /// </summary>
        /// <param name="panel">DocingPanel On which the View need to be displayed</param>
        /// <param name="state">Docking state in which the view need to be displayed</param>
        /// <param name="isFakeView">
        /// If the Start view is a fake view which wrapps many
        /// child views and not actually displayed
        /// </param>
        public virtual void StartView(DockPanel panel,DockState state,bool isFakeView = false)
        {
            m_view.ParentPanel = panel;
            if (false == isFakeView)
            {
                m_view.Show(panel, state);
            }
            m_model.PropertyChanged += new PropertyChangedEventHandler(propertyChanged);   
        }
        /// <summary>
        /// Event handler for the property Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void propertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SessionChange" && m_model.Session != Sessions.IDLE)
            {
                StartControllerSession(m_model.Session);
            }
            //also send the change to view
            m_view.propertyChanged(e.PropertyName);
            
        }
        /// <summary>
        /// Change the View Session based on the Session property change
        /// </summary>
        /// <param name="session">Session value</param>
        private  void StartControllerSession(Sessions session)
        {
            switch (session)
            {
                case Sessions.NEW_PROJECT:
                    
                    m_view.newProject();
                    m_model.Datamodel_newProject();

                    break;
                case Sessions.CLOSE_PROJECT:
                   
                    m_view.closeProject();
                    m_model.Datamodel_closeProject();

                    break;
                case Sessions.OPEN_PROJECT:
                 
                    m_view.OpenProject();
                    m_model.Datamodel_openProject();

                    break;
                case Sessions.OPEN_PROJECT_COMPLETE:
                    
                    m_view.OpenProjectComplete();
                    m_model.Datamodel_openProjectComplete();

                    break;
                case Sessions.SAVE_PROJECT:
                   // if (m_model.IsDirty)
                    {
                       
                        m_view.saveProject();
                        m_model.Datamodel_saveProject();

                    }
                    break;
                case Sessions.ADD_FILE:
                  
                    m_view.AddFile(m_model.CurrentFile);
                    m_model.Datamodel_addFile();

                    break;
                case Sessions.OPEN_FILE:
                   
                    m_view.OpenFile(m_model.CurrentFile);
                    m_model.Datamodel_openFile(m_model.CurrentFile);

                    break;
                case Sessions.CLOSE_FILE:
                    
                    m_view.CloseFile(m_model.CurrentFile);
                    m_model.Datamodel_closeFile(m_model.CurrentFile);

                    break;
                case Sessions.REMOVE_FILE:
                    
                    m_view.RemoveFile(m_model.CurrentFile);
                    m_model.Datamodel_removeFile(m_model.CurrentFile);

                    break;
                case Sessions.PARSER_RUNNING:
                    
                    m_view.ParserRunning();
                    m_model.Datamodel_ParserRunning();

                    break;
                case Sessions.PARSER_COMPLETE:
                   
                    m_view.ParserComplete();
                    m_model.Datamodel_ParserComplete();

                    break;
                default:
                    break;
            }
            //m_model.Session = Sessions.IDLE;
        }
    }
}
