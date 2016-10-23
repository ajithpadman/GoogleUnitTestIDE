using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GUnitFramework.Interfaces;
using System.IO;
namespace Gunit.Ui
{
    public partial class ProjectSettings : Form
    {
        ICGunitHost m_host;
        public ProjectSettings()
        {
            InitializeComponent();
        }
        public ProjectSettings(ICGunitHost host)
        {
            InitializeComponent();
            m_host = host;
        }

        private void ProjectSettings_Load(object sender, EventArgs e)
        {
            ProjectSetting_SetupDataGridView(dtGridInclude);
            ProjectSetting_SetupDataGridView(dtGridLibrary);
            ProjectSetting_SetupDataGridView(dtGridLibNames);
            ProjectSetting_SetupDataGridView(dtmacros);
            dtGridInclude.DataSource = m_host.IncludePathsTable;
            dtGridLibrary.DataSource = m_host.LibraryPathsTable;
            dtGridLibNames.DataSource = m_host.LibraryNameTable;
            dtmacros.DataSource = m_host.MacroDataTable;
            ProjectSetting_makeDataRowReadonly(dtGridInclude);
            ProjectSetting_makeDataRowReadonly(dtGridLibrary);
            ProjectSetting_makeDataRowReadonly(dtGridLibNames);
            ProjectSetting_makeDataRowReadonly(dtmacros);

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

        private void btnAddInclude_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.SelectedPath = Path.GetDirectoryName(m_host.ProjectData.ProjectPath);
            DialogResult result = browser.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_host.ProjectDataModel_addIncludePath(browser.SelectedPath);
            }
        }

        private void btnRemoveInclude_Click(object sender, EventArgs e)
        {
            if (null != dtGridInclude.CurrentRow)
            {
                DataRowView currentDataRowView = (DataRowView)dtGridInclude.CurrentRow.DataBoundItem;
                DataRow row = currentDataRowView.Row;
                m_host.ProjectDataModel_RemoveIncludePath(row, dtGridInclude.CurrentRow.Cells[0].Value.ToString());
            }
        }

        private void btnAddLibPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.SelectedPath = Path.GetDirectoryName(m_host.ProjectData.ProjectPath);
            DialogResult result = browser.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_host.ProjectDataModel_addLibraryPath(browser.SelectedPath);
            }
        }

        private void btnRemoveLibPath_Click(object sender, EventArgs e)
        {
            DataRowView currentDataRowView = (DataRowView)dtGridLibrary.CurrentRow.DataBoundItem;
            DataRow row = currentDataRowView.Row;
            m_host.ProjectDataModel_RemoveLibraryPath(row, dtGridLibrary.CurrentRow.Cells[0].Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserInput frmname = new UserInput();
            DialogResult result = frmname.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_host.ProjectDataModel_addLibraryName(frmname.m_Data);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (null != dtGridLibNames.CurrentRow)
            {
                DataRowView currentDataRowView = (DataRowView)dtGridLibNames.CurrentRow.DataBoundItem;
                DataRow row = currentDataRowView.Row;
                m_host.ProjectDataModel_RemoveLibraryName(row, dtGridLibNames.CurrentRow.Cells[0].Value.ToString());
            }
        }

        private void btnMacroAdd_Click(object sender, EventArgs e)
        {
            UserInput frmname = new UserInput();
            DialogResult result = frmname.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                m_host.ProjectDataModel_addMacroName(frmname.m_Data);
            }
        }

        private void btnMacroDelete_Click(object sender, EventArgs e)
        {
            if (null != dtmacros.CurrentRow)
            {
                DataRowView currentDataRowView = (DataRowView)dtmacros.CurrentRow.DataBoundItem;
                DataRow row = currentDataRowView.Row;
                m_host.ProjectDataModel_RemoveMacroName(row, dtmacros.CurrentRow.Cells[0].Value.ToString());
            }
        } 
    }
}
