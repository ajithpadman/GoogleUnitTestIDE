using Gunit.DataModel;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Gunit.Ui
{
    public partial class ProjectSetting : Form
    {
        ProjectDataModel m_model;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">DataModel from ProjectUi</param>
        public ProjectSetting(ProjectDataModel model)
        {
            InitializeComponent();
            m_model = model;
            
        }
        /// <summary>
        /// Add new Library name
        /// </summary>
        /// <param name="Name">Name of the Library</param>
        public void ProjectSetting_addLibraryName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name) == false)
            {
                m_model.ProjectDataModel_addLibraryName(Name);
            }
        }
        public void ProjectSetting_addMacroName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name) == false)
            {
                m_model.ProjectDataModel_addMacroName(Name);
            }
        }
        /// <summary>
        /// Form Load Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectSetting_Load(object sender, EventArgs e)
        {
            ProjectSetting_SetupDataGridView(dtGridInclude);
            ProjectSetting_SetupDataGridView(dtGridLibrary);
            ProjectSetting_SetupDataGridView(dtGridLibNames);
            ProjectSetting_SetupDataGridView(dtmacros);
            dtGridInclude.DataSource = m_model.IncludePathsTable;
            dtGridLibrary.DataSource = m_model.LibraryPathsTable;
            dtGridLibNames.DataSource = m_model.LibraryNameTable;
            dtmacros.DataSource = m_model.MacroDataTable;
            ProjectSetting_makeDataRowReadonly(dtGridInclude);
            ProjectSetting_makeDataRowReadonly(dtGridLibrary);
            ProjectSetting_makeDataRowReadonly(dtGridLibNames);
            ProjectSetting_makeDataRowReadonly(dtmacros);
            comboWarningLevel.DataSource = Enum.GetValues(typeof(WarningLevel));
            cbOutputType.DataSource = Enum.GetValues(typeof(OutputTypes));

        }
        /// <summary>
        /// Make the DataTable ReadOnly
        /// </summary>
        /// <param name="view">DataTable Object</param>
        private void ProjectSetting_makeDataRowReadonly(DataGridView view)
        {
            foreach (DataGridViewBand band in view.Columns)
            {
                band.ReadOnly = true;
            }
        } 
        /// <summary>
        /// SetUpThe Datagrid for display
        /// </summary>
        /// <param name="view"></param>
        private void ProjectSetting_SetupDataGridView(DataGridView view)
        {
            view.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            view.AllowUserToAddRows = false;
            view.AllowUserToDeleteRows = false;
            view.AllowUserToResizeRows = false;
            view.Columns.Clear();
            view.RowHeadersWidthSizeMode =
            DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            view.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
         }
        /// <summary>
        /// Event handler for add newInclude Path Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInclude_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.SelectedPath = Path.GetDirectoryName(m_model.ProjectPath);
            DialogResult result =  browser.ShowDialog();
            if(result == DialogResult.OK)
            {
                m_model.ProjectDataModel_addIncludePath(browser.SelectedPath);
            }
            
        }
        /// <summary>
        /// Remove added Include Path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveInclude_Click(object sender, EventArgs e)
        {
            if (null != dtGridInclude.CurrentRow)
            {
                DataRowView currentDataRowView = (DataRowView)dtGridInclude.CurrentRow.DataBoundItem;
                DataRow row = currentDataRowView.Row;
                m_model.ProjectDataModel_RemoveIncludePath(row, dtGridInclude.CurrentRow.Cells[0].Value.ToString());
            }

        }
        /// <summary>
        /// Add new Library Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLibName_Click(object sender, EventArgs e)
        {
            UserInput frmname = new UserInput(this);
            frmname.ShowDialog();
        }
        /// <summary>
        /// Add macro Defines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddMacros_Click(object sender, EventArgs e)
        {
            UserInput frmname = new UserInput(this,SubWindowType.MACRONAME);
            frmname.ShowDialog();
        }
        /// <summary>
        /// Remove the added Library Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveLibName_Click(object sender, EventArgs e)
        {
            if (null != dtGridLibNames.CurrentRow)
            {
                DataRowView currentDataRowView = (DataRowView)dtGridLibNames.CurrentRow.DataBoundItem;
                DataRow row = currentDataRowView.Row;
                m_model.ProjectDataModel_RemoveLibraryName(row, dtGridLibNames.CurrentRow.Cells[0].Value.ToString());
            }
        }
        /// <summary>
        /// Remove Macro Defines
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveMacro_Click(object sender, EventArgs e)
        {
            if (null != dtmacros.CurrentRow)
            {
                DataRowView currentDataRowView = (DataRowView)dtmacros.CurrentRow.DataBoundItem;
                DataRow row = currentDataRowView.Row;
                m_model.ProjectDataModel_RemoveMacroName(row, dtmacros.CurrentRow.Cells[0].Value.ToString());
            }
        }
        /// <summary>
        /// Add new Library path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLibPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.SelectedPath = Path.GetDirectoryName(m_model.ProjectPath);
            DialogResult result = browser.ShowDialog();
            if(result == DialogResult.OK)
            {
                m_model.ProjectDataModel_addLibraryPath(browser.SelectedPath);
            }
            
        }
        /// <summary>
        /// Remove Library Path added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveLibPath_Click(object sender, EventArgs e)
        {
            DataRowView currentDataRowView = (DataRowView)dtGridLibrary.CurrentRow.DataBoundItem;
            DataRow row = currentDataRowView.Row;
            m_model.ProjectDataModel_RemoveLibraryPath(row, dtGridLibrary.CurrentRow.Cells[0].Value.ToString());
        }

        private void btnMInGW_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = Path.GetDirectoryName(m_model.ProjectPath);
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (Directory.Exists(dialog.SelectedPath))
                {
                    m_model.CompilorPath = dialog.SelectedPath;
                    txtMinGW.Text = dialog.SelectedPath;
                    txtMinGW.TextAlign = HorizontalAlignment.Right;
                }

            }
            
        }
    }
}
