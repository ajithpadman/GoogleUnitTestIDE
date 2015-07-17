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
    public partial class AddUnit : Form
    {
        GUnit m_parent;
        SaveFileDialog fileSave;
        public AddUnit(GUnit parent)
        {
            InitializeComponent();
            m_parent = parent;
        }

        private void AddUnit_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUnitName.Text) == false)
            {
                fileSave = new SaveFileDialog();
                fileSave.Filter = "Unit Files (*.h)|*.h";
                fileSave.FileOk += new CancelEventHandler(SaveUnit);
                fileSave.ShowDialog();
            }
        }
        public void Updateunit( UnitInfo unit)
        {
            if(File.Exists(unit.m_fileName))
            {
                StreamWriter writer = new StreamWriter(unit.m_fileName);
                CodeGenerator.addFileHeader(
                writer,
                Path.GetFileName(unit.m_fileName),
                 "This file implements the Unit" + unit.m_className,
                 System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                "1.0",
                 DateTime.UtcNow.Date.ToString()
                );

                writer.WriteLine("#ifndef " + unit.m_className.ToUpper());
                writer.WriteLine("#define " + unit.m_className.ToUpper());
                if (unit.m_IsClass == true)
                {
                    writer.WriteLine("class " + unit.m_className);
                    writer.WriteLine("{");
                    writer.WriteLine("  private:");
                    foreach (FunctionalInterface function in unit.m_functionPrototypeList)
                    {
                        if (function.m_AccessScope == "private")
                        {
                            writer.WriteLine("  "+function.m_ReturnType + " " + function.m_FunctionName + function.m_Signature + ";");
                        }
                    }
                    writer.WriteLine("  public:");
                    foreach (FunctionalInterface function in unit.m_functionPrototypeList)
                    {
                        if (function.m_AccessScope == "public")
                        {
                            writer.WriteLine("  " + function.m_ReturnType + " " + function.m_FunctionName + function.m_Signature + ";");
                        }
                    }
                    writer.WriteLine("};//End of Class");
                    
                }
                else
                {
                    foreach (FunctionalInterface function in unit.m_functionPrototypeList)
                    {
                       
                        writer.WriteLine(function.m_ReturnType + " " + function.m_FunctionName + function.m_Signature + ";");
                        
                    }
                }
                writer.WriteLine("#endif");
                writer.Close();

            }
        }
        private void SaveUnit(object sender, CancelEventArgs e)
        {
            string fileName = fileSave.FileName;
            StreamWriter writer = new StreamWriter(fileName);
            CodeGenerator.addFileHeader(
              writer,
             Path.GetFileName(fileName),
              "This file implements the Unit" + txtUnitName.Text,
              System.Security.Principal.WindowsIdentity.GetCurrent().Name,
              "1.0",
              DateTime.UtcNow.Date.ToString()
          );
            
            
            UnitInfo unit = new UnitInfo();
            unit.m_className = txtUnitName.Text;
            unit.m_fileName = fileName;
            writer.WriteLine("#ifndef " + txtUnitName.Text.ToUpper());
            writer.WriteLine("#define " + txtUnitName.Text.ToUpper());
            if (comboUnitType.SelectedItem.ToString() == "Class")
            {
                unit.m_IsClass = true;
                writer.WriteLine("class " + txtUnitName.Text);
                writer.WriteLine("{");
                writer.WriteLine("};");
            }
            else
            {
                unit.m_IsClass = false;
            }
            writer.WriteLine("#endif");
            writer.Close();
            m_parent.m_data.m_UnitsForGeneration.Add(unit);
            this.Close();
        }
    }
}
