using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gunit.DataModel;
using WeifenLuo.WinFormsUI.Docking;
namespace Gunit.Ui
{
    public partial class GUnitSideBarBase :DockContent
    {
        protected DockPanel m_panel;
        protected DataModelBase m_model;
        public GUnitSideBarBase()
        {
            InitializeComponent();
        }
        public GUnitSideBarBase(DataModelBase model)
        {
            InitializeComponent();
            m_model = model;
        }
        /// <summary>
        /// parent pannel where the View is docked
        /// </summary>
        public DockPanel ParentPanel
        {
            get
            {
                return m_panel;
            }
            set
            {
                m_panel = value;
            }
        }
        public virtual void AddFile(string file)
        {

        }

        /// <summary>
        /// Method to be called when a project is opened
        /// </summary>
        public virtual void OpenProject()
        {
           
        }
        /// <summary>
        /// Opening the Project is complete
        /// </summary>
        public virtual void OpenProjectComplete()
        {

        }
        /// <summary>
        /// method to be called when the project is closed
        /// </summary>
        public virtual void closeProject()
        {
           
        }
        /// <summary>
        /// method to be called when a project is created
        /// </summary>
        public virtual void newProject()
        {

        }
        /// <summary>
        /// method to be called when a project need to be saved
        /// </summary>
        public virtual void saveProject()
        {
            m_model.IsDirty = false;
        }
        /// <summary>
        /// Method to be called when a file is opened
        /// </summary>
        /// <param name="l_currentFile"></param>
        public virtual void OpenFile(string l_currentFile)
        {
            
        }
        /// <summary>
        /// Method to be called when a file is closed
        /// </summary>
        /// <param name="l_currentFile"></param>
        public virtual void CloseFile(string l_currentFile)
        {

        }
        /// <summary>
        /// Method to be called when a file is removed from the Project
        /// </summary>
        /// <param name="l_currentFile"></param>
        public virtual void RemoveFile(string l_currentFile)
        {

        }
        public virtual void ParserRunning()
        {
        }
        public virtual void ParserComplete()
        {

        }
        public virtual void propertyChanged(string propertyName)
        {

        }

        private void GUnitSideBarBase_Load(object sender, EventArgs e)
        {
            this.CloseButtonVisible = false;
        }
    }
}
