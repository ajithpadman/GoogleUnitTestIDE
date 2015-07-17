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
    public partial class AddNewFunction : Form
    {
        GUnit m_Parent;
        FunctionalInterface m_function;
        int m_Index = -1;
        public AddNewFunction(GUnit Parent,FunctionalInterface function)
        {
            InitializeComponent();
            m_Parent = Parent;
            m_function = function;
        }

        private void AddNewFunction_Load(object sender, EventArgs e)
        {
            updateUnitList();
            comboAccess.SelectedIndex = 0;
            
            txtxFunctionName.Text = m_function.m_FunctionName;
            dtArguments.Columns.Add("Arguments", "Arguments");
            dtArguments.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }
        private void updateUnitList()
        {
            comboUnits.Items.Clear();
            foreach (UnitInfo unit in m_Parent.m_data.m_UnitsForGeneration)
            {
                comboUnits.Items.Add(unit.m_className);
                
            }
            if (m_Parent.m_data.m_UnitsForGeneration.Count == 0)
            {
                btnSubmit.Enabled = false;
            }
            else
            {
                btnSubmit.Enabled = true;
            }
        }
        private void comboUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = 0;
           
            foreach (UnitInfo unit in m_Parent.m_data.m_UnitsForGeneration)
            {

                if (comboUnits.SelectedItem.ToString() == unit.m_className) 
                {
                    m_Index = count;
                    if (unit.m_IsClass == false)
                    {
                        comboAccess.Enabled = false;
                        lblAccess.Enabled = false;
                        comboIsVirtual.Enabled = false;
                        lblVirtual.Enabled = false;
                    }
                    else
                    {
                        comboAccess.Enabled = true;
                        lblAccess.Enabled = true;
                        comboIsVirtual.Enabled = true;
                        lblVirtual.Enabled = true;
                    }
                }
                else
                {
                    m_Index = -1;
                }

                count++;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (
                string.IsNullOrWhiteSpace(txtxFunctionName.Text) == false &&
                string.IsNullOrWhiteSpace(txtReturnValue.Text) == false 
                )
            {

            if (m_Index != -1)
            {
                UnitInfo currentUnit = m_Parent.m_data.m_UnitsForGeneration[m_Index];
                FunctionalInterface function = new FunctionalInterface();
                function.m_FunctionName = txtxFunctionName.Text;
                function.m_ReturnType = txtReturnValue.Text;
                string signature = "(";
                for (int i = 0; i < dtArguments.Rows.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace((string)dtArguments.Rows[i].Cells[0].Value) == false)
                    {
                        string currentArg = dtArguments.Rows[i].Cells[0].Value.ToString();
                        function.m_argumentTypes.Add(dtArguments.Rows[i].Cells[0].Value.ToString());
                        if (i != dtArguments.Rows.Count - 1)
                        {
                            if (currentArg != "void")
                            {
                                signature += currentArg + " arg" + i + ",";
                            }
                        }
                        else
                        {
                            if (currentArg != "void")
                            {
                                signature += currentArg + " arg" + i;
                            }
                        }
                    }
                }
                 signature += ")";
                 function.m_Signature = signature;
                 if (comboAccess.SelectedItem != null)
                 {
                     function.m_AccessScope = comboAccess.SelectedItem.ToString();
                 }
                 currentUnit.m_functionPrototypeList.Add(function);
                 AddUnit unitGen = new AddUnit(m_Parent);
                 unitGen.Updateunit(currentUnit);

            }
            }
            this.Close();
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            AddUnit frmUnit = new AddUnit(m_Parent);
            frmUnit.ShowDialog();
            updateUnitList();
        }
    }
}
