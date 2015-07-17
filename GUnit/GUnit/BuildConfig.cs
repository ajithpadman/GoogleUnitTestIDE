using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace GUnit
{
    public partial class BuildConfig : Form
    {
        GUnit m_parent;
        List<string> m_IncludePaths = new List<string>();
        List<string> m_LibPaths = new List<string>();
        public BuildConfig(GUnit parent)
        {
            InitializeComponent();
            m_parent = parent;
        }
 
        private void BuildConfig_Load(object sender, EventArgs e)
        {


            DataGridViewButtonColumn oColQualif = new DataGridViewButtonColumn();
            oColQualif.Name = "Browse";
            dtGridInclude.Columns.Add("Include Paths", "Include Paths");
            dtGridInclude.AllowUserToAddRows = false;

            dtGridInclude.Columns.Add(oColQualif);
            dtGridInclude.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtGridInclude.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dtGridInclude.Rows.Add();

            DataGridViewButtonColumn oColLib = new DataGridViewButtonColumn();
            
            oColLib.Name = "Browse";
            dtLib.Columns.Add("Library Paths", "Library Paths");
            dtLib.Columns.Add(oColLib);
            dtLib.AllowUserToAddRows = false;
            dtLib.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtLib.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            dtLib.Rows.Add();

            dtLibNames.Columns.Add("Library names", "Library names");
            dtLibNames.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dtLibNames.AllowUserToAddRows = true;
            dtLibNames.Rows.Add();
            txtBinPath.Text = m_parent.m_data.m_Project.m_SolnData.m_BinPath;
            int count = 0;
            foreach (string inc in m_parent.m_data.m_Project.m_SolnData.m_includePaths)
            {
                dtGridInclude.Rows[count].Cells[0].Value = inc;
                dtGridInclude.Rows.Add();
                dtGridInclude.CurrentCell = dtGridInclude.Rows[dtGridInclude.Rows.Count - 1].Cells[0];
                count++;

            }
            count = 0;
            foreach (string lib in m_parent.m_data.m_Project.m_SolnData.LibPaths)
            {
                dtLib.Rows[count].Cells[0].Value = lib;
                dtLib.Rows.Add();
                dtLib.CurrentCell = dtLib.Rows[dtLib.Rows.Count - 1].Cells[0];
                //txtIncludePaths.Text += inc +"\n";
                count++;
                //txtLibPaths.Text += lib + "\n";

            }
            count = 0;
            dtLibNames.AllowUserToAddRows = false;
            foreach (string libName in m_parent.m_data.m_Project.m_SolnData.m_Libraries)
            {
                dtLibNames.Rows[count].Cells[0].Value = libName;
                dtLibNames.Rows.Add();
                dtLibNames.CurrentCell = dtLibNames.Rows[dtLibNames.Rows.Count - 1].Cells[0];
                count++;
                //txtLibName.Text += libName + "\n";
            }
            

        }

        private void btnInclude_Click(object sender, EventArgs e)
        {
            
            
        }

        private void btnLibPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog br = new FolderBrowserDialog();
            br.SelectedPath = Path.GetDirectoryName(m_parent.m_data.m_Project.m_ProjectPath);
            br.ShowDialog();
            if (Directory.Exists(br.SelectedPath))
            {
               // txtLibPaths.Text += br.SelectedPath;
                m_parent.m_data.GUnitDat_AddLibPaths(br.SelectedPath);
                
            }
        }

        private void txtLibName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddLib_Click(object sender, EventArgs e)
        {
            OpenFileDialog filedlg = new OpenFileDialog();
            filedlg.InitialDirectory = Path.GetDirectoryName(m_parent.m_data.m_Project.m_ProjectPath);
            filedlg.Multiselect = true;
            filedlg.ShowDialog();
            foreach (string fileName in filedlg.FileNames)
            {
                if (File.Exists(fileName))
                    m_parent.m_data.GUnitDat_AddLibNames(Path.GetFileName(fileName));
            }
            
        }

        private void btnBin_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog br = new FolderBrowserDialog();
            br.SelectedPath = Path.GetDirectoryName(m_parent.m_data.m_Project.m_ProjectPath);
            br.ShowDialog();
            if (Directory.Exists(br.SelectedPath))
            {
                txtBinPath.Text= br.SelectedPath;
                
                m_parent.m_data.GUnitDat_AddBinPath(br.SelectedPath);
                
            }
        }
       
        private void BuildCfg_IncludeCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >=0)
            {
                if (dtGridInclude.Columns[e.ColumnIndex].Name == "Browse")
                {
                    FolderBrowserDialog br = new FolderBrowserDialog();
                    br.SelectedPath = Path.GetDirectoryName(m_parent.m_data.m_Project.m_ProjectPath);
                    br.ShowDialog();
                    if (Directory.Exists(br.SelectedPath))
                    {

                        dtGridInclude.Rows[e.RowIndex].Cells[0].Value = m_parent.m_data.getReleativePath(br.SelectedPath);
                        dtGridInclude.Rows.Add();
                        dtGridInclude.CurrentCell = dtGridInclude.Rows[dtGridInclude.Rows.Count -1].Cells[0];
                        
                    }
                }
            }
        }
       
        private void CellValueChange(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BldCfg_LibAdded(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (dtLib.Columns[e.ColumnIndex].Name == "Browse")
                {
                 
                     FolderBrowserDialog br = new FolderBrowserDialog();
                    br.SelectedPath = Path.GetDirectoryName(m_parent.m_data.m_Project.m_ProjectPath);
                    br.ShowDialog();
                    if (Directory.Exists(br.SelectedPath))
                    {
                        string relatviePath = m_parent.m_data.getReleativePath(br.SelectedPath);
                        dtLib.Rows[e.RowIndex].Cells[0].Value = relatviePath;
                        dtLib.Rows.Add();
                        dtLib.CurrentCell = dtLib.Rows[dtLib.Rows.Count - 1].Cells[0];
                    }
    
                }
            }
        }

        private void BldCfg_LibNameChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                if (dtLibNames.Columns[e.ColumnIndex].Name == "Library names")
                {
                    dtLibNames.Rows.Add();
                    dtLibNames.CurrentCell = dtLibNames.Rows[dtLib.Rows.Count - 1].Cells[0];
                }
                
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            m_parent.m_data.m_Project.m_SolnData.m_Libraries.Clear();
            m_parent.m_data.m_Project.m_SolnData.m_includePaths.Clear();
            m_parent.m_data.m_Project.m_SolnData.LibPaths.Clear();
            for (int i = 0; i < dtLibNames.Rows.Count; i++)
            {
                if((dtLibNames.Rows[i].Cells[0].Value) != null)
                {
                    m_parent.m_data.GUnitDat_AddLibNames(dtLibNames.Rows[i].Cells[0].Value.ToString());
               
                }
            }
            for (int i = 0; i < dtLib.Rows.Count; i++)
            {
                if ((dtLib.Rows[i].Cells[0].Value) != null)
                {
                    m_parent.m_data.GUnitDat_AddLibPaths(dtLib.Rows[i].Cells[0].Value.ToString(),true);

                }
            }
            for (int i = 0; i < dtGridInclude.Rows.Count; i++)
            {
                if ((dtGridInclude.Rows[i].Cells[0].Value) != null)
                {
                    m_parent.m_data.GUnitDat_AddIncludePaths(dtGridInclude.Rows[i].Cells[0].Value.ToString(), true);

                }
            }
            this.Close();
        }

        private void dtGridInclude_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
