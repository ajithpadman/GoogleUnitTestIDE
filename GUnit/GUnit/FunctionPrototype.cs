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
    public partial class FunctionPrototype : Form
    {
        FunctionalInterface m_function;
        int m_Unitindex;
        int m_FunctionIndex;
        GUnit m_parent;
        BindingList<string> ArgList;
        public FunctionPrototype(FunctionalInterface function,GUnit parent)
        {
            InitializeComponent();
            m_function = function;
            m_parent = parent;
           
        }

        private void FunctionPrototype_Load(object sender, EventArgs e)
        {
            if(m_function!= null)
            {
                txtFileName.Text = m_function.m_FileName;
                txtClassName.Text = m_function.m_ClassName;
                comboAccess.Text = m_function.m_AccessScope;
                txtReturnValue.Text = m_function.m_ReturnType;
                txtxFunctionName.Text = m_function.m_FunctionName;
                dtArgs.Enabled = false;
                DataTable dt = new DataTable();
                dt.Columns.Add("Arguments");
                foreach (string argument in m_function.m_argumentTypes)
                {
                    dt.Rows.Add(argument);
                }
                 
                dtArgs.DataSource = dt;
                
                txtArgCount.Text = m_function.m_argumentCount.ToString();
                if (m_function.m_IsVirtual)
                {
                    comboIsVirtual.SelectedIndex = 0;
                }
                else
                {
                    comboIsVirtual.SelectedIndex = 1;
                }

            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            txtFileName.Enabled = true;
            txtClassName.Enabled = true;
            comboAccess.Enabled = true;
            txtReturnValue.Enabled = true;
            txtxFunctionName.Enabled = true;
            dtArgs.Enabled = true;
            txtArgCount.Enabled = true;
            comboIsVirtual.Enabled = true;
            btnSubmit.Enabled = true;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            FileInfo data = m_parent.m_data.GUnitData_getFileInformation(m_function.m_FileName);
            if (data != null)
            {
               


                m_function.m_FileName = txtFileName.Text;
                m_function.m_ClassName = txtClassName.Text;
                try
                {
                    m_function.m_argumentCount = Convert.ToInt16(txtArgCount.Text);
                }
                catch
                {
                }
                m_function.m_AccessScope = comboAccess.Text;
                if (comboIsVirtual.SelectedIndex == 0)
                {
                    m_function.m_IsVirtual = true;
                }
                else
                {
                    m_function.m_IsVirtual = false;
                }
                m_function.m_FunctionName = txtxFunctionName.Text;
                m_function.m_ReturnType = txtReturnValue.Text;
               
                List<string> args = new List<string>();
                for (int i = 0; i < dtArgs.Rows.Count; i++)
                {
                    if (dtArgs.Rows[i].Cells[0].Value != null)
                    {
                        args.Add(dtArgs.Rows[i].Cells[0].Value.ToString());
                     
                    }
                }
                m_function.m_argumentTypes.Clear();
                m_function.m_argumentTypes.AddRange(args);
                m_parent.m_data.GUnitData_UpdateProjectTable(m_function.m_FileName, data);
                m_parent.GUnit_UpdateDocumentFocusChange(data);

                this.Close();
            }
        }
    }
}
