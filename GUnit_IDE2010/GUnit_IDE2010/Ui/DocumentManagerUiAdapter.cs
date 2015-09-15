using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gunit.DataModel;
using Gunit.Controller;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;
using ClangSharp;
namespace Gunit.Ui
{
    public class DocumentManagerUiAdapter:GUnitSideBarBase
    {
        public delegate void onFIleFocused(string file);
        public event onFIleFocused evFileFocused = delegate { };
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model"></param>
        public DocumentManagerUiAdapter(DataModelBase model):base(model)
        {
            m_model.PropertyChanged += DocumentMgr_PropertyChanged;
        }
        private void DocumentMgr_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "FocusLocation":
                    FocusToPosition(((DocumentMgrDataModel)m_model).FocusLocation);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Handle when a Document changes its session 
        /// Convert Document Session to Document Manager Session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentMgr_DocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SessionChange":
                    if (sender is DocumentDataModel)
                    {
                        DocumentDataModel documentModel = sender as DocumentDataModel;
                        switch (documentModel.Session)
                        {
                            case Sessions.CLOSE_FILE:
                                m_model.CurrentFile = documentModel.CurrentFile;
                                m_model.Session = Sessions.CLOSE_FILE;
                                break;
                                
                            case Sessions.REMOVE_FILE:
                                m_model.CurrentFile = documentModel.CurrentFile;
                                m_model.Session = Sessions.REMOVE_FILE;
                                break;
                            case Sessions.FOCUS_FILE:
                                m_model.CurrentFile = documentModel.CurrentFile;
                                m_model.Session = Sessions.FOCUS_FILE;
                                evFileFocused(m_model.CurrentFile);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "Text":
                case "Dirty":
                    m_model.IsDirty = true;
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// Called when a project is opened
        /// </summary>
        public override void OpenProject()
        {
            base.OpenProject();
            //When a new project is opened all the Current Documents need to be closed
            closeProject();
        }
        /// <summary>
        /// Called when a Project is closed
        /// </summary>
        public override void closeProject()
        {
            base.closeProject();
            List<GUnitSideBarBase> viewsTobeRemoved = new List<GUnitSideBarBase>();
            foreach (DocumentCollection document in ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Values)
            {
                viewsTobeRemoved.Add(document.View);
                document.Model.PropertyChanged -= DocumentMgr_DocumentPropertyChanged;
            }
            foreach (GUnitSideBarBase view in viewsTobeRemoved)
            {
                view.Close();
            }
            ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Clear();
        }
        /// <summary>
        /// Called when a new Project is created
        /// </summary>
        public override void newProject()
        {
            base.newProject();
            //When a new project is opened all the Current Documents need to be closed
            closeProject();
        }
        /// <summary>
        /// called when the project is saved
        /// </summary>
        public override void saveProject()
        {
            base.saveProject();
            foreach (DocumentCollection document in ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Values)
            {
               
                document.Model.Session = Sessions.SAVE_PROJECT;
            }
        }
        public override void ParserComplete()
        {
            base.ParserComplete();
            foreach (Object key in ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Keys)
            {
                string l_currentFile = key as string;
                if (((DocumentMgrDataModel)m_model).m_DocumentHashTable.ContainsKey(l_currentFile))
                {
                    DocumentCollection document = ((DocumentMgrDataModel)m_model).m_DocumentHashTable[l_currentFile] as DocumentCollection;
                    try
                    {
                       
                        UpdateDocumentErrorLines(document.Model);
                        UpdateDocumentWarningLines(document.Model);
                    }
                    catch
                    {
                        ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Remove(l_currentFile);
                    }
                }
            }
        }
        public void FocusToPosition(CodeLocation location)
        {
            OpenFile(location.fileName);
            if (((DocumentMgrDataModel)m_model).m_DocumentHashTable.ContainsKey(location.fileName))
            {
                DocumentCollection document = ((DocumentMgrDataModel)m_model).m_DocumentHashTable[location.fileName] as DocumentCollection;
                try
                {
                    document.Model.CurrentLine = location;
                    
                }
                catch
                {
                    ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Remove(location.fileName);
                }
            }

        }
        private void UpdateDocumentErrorLines(DocumentDataModel Model)
        {
            Model.DocumentErrors.Clear();
            foreach (CodeLocation Errorlocation in ((DocumentMgrDataModel)m_model).ErrorLocation)
            {
                if (Errorlocation.fileName == Model.CurrentFile)
                {
                   Model.DocumentErrors += Errorlocation;
    
                }
            }
        }
        private void UpdateDocumentWarningLines(DocumentDataModel Model)
        {
            Model.DocumentWarnings.Clear();
            foreach (CodeLocation Warninglocation in ((DocumentMgrDataModel)m_model).WarningLocation)
            {
                if (Warninglocation.fileName == Model.CurrentFile)
                {
                    Model.DocumentWarnings += Warninglocation;
                }
            }
        }
        /// <summary>
        /// Call when file is Opened 
        /// </summary>
        /// <param name="l_currentFile"></param>
        public override void OpenFile(string l_currentFile)
        {
            m_model.CurrentFile = l_currentFile;
            base.OpenFile(l_currentFile);
            if (((DocumentMgrDataModel)m_model).m_DocumentHashTable.ContainsKey(l_currentFile))
            {
                DocumentCollection document = ((DocumentMgrDataModel)m_model).m_DocumentHashTable[l_currentFile] as DocumentCollection;
                try
                {
                    document.View.Focus();
                    UpdateDocumentErrorLines(document.Model);
                    UpdateDocumentWarningLines(document.Model);
                }
                catch
                {
                    ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Remove(l_currentFile);
                }
            }
            else
            {
                DocumentDataModel documentModel = new DocumentDataModel(l_currentFile);
                Document document = new Document(documentModel);
                Documentcontroller documentController = new Documentcontroller(document, documentModel);
                DocumentCollection documentDbc = new DocumentCollection();
                documentModel.CodeCompletionWords = ((DocumentMgrDataModel)m_model).CodeCompletionWords;
                documentDbc.Controller = documentController;
                documentDbc.Model = documentModel;
                documentDbc.View = document;
                ((DocumentMgrDataModel)m_model).m_DocumentHashTable[l_currentFile] = documentDbc;
                documentModel.PropertyChanged += (DocumentMgr_DocumentPropertyChanged);
                documentController.StartView(this.ParentPanel, DockState.Document);
                UpdateDocumentErrorLines(documentModel);
                UpdateDocumentWarningLines(documentModel);
            }
        }
        
        /// <summary>
        /// Call when file is closed
        /// </summary>
        /// <param name="l_currentFile"></param>
        public override void CloseFile(string l_currentFile)
        {
            base.CloseFile(l_currentFile);
            if (
                ((DocumentMgrDataModel)m_model).m_DocumentHashTable.ContainsKey(l_currentFile)
              )
            {
                ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Remove(l_currentFile);
            }
        }
        /// <summary>
        /// Call When File is removed
        /// </summary>
        /// <param name="l_currentFile"></param>
        public override void RemoveFile(string l_currentFile)
        {
            base.RemoveFile(l_currentFile);
            if (
               ((DocumentMgrDataModel)m_model).m_DocumentHashTable.ContainsKey(l_currentFile)
             )
            {
                DocumentCollection document = ((DocumentMgrDataModel)m_model).m_DocumentHashTable[l_currentFile] as DocumentCollection;
                document.View.Close();
            }
            //CloseFile(l_currentFile);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DocumentManagerUiAdapter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Name = "DocumentManagerUiAdapter";
            this.Load += new System.EventHandler(this.DocumentManagerUiAdapter_Load);
            this.ResumeLayout(false);

        }
        public void DocumentMgr_macroUpdate(ListofStrings macros)
        {
            foreach (DocumentCollection document in ((DocumentMgrDataModel)m_model).m_DocumentHashTable.Values)
            {
                document.View.updateMacro(macros);
            }
        }
        private void DocumentManagerUiAdapter_Load(object sender, EventArgs e)
        {

        }
    }
}
