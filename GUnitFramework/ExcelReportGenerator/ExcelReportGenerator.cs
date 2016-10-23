using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using GUnitFramework.Implementation;
using System.Windows.Forms;
namespace ExcelReportGenerator
{
    public class ExcelReportGenerator : PropertyNotifier,ITestReportGenerator
    {
        ICGunitHost m_host;
        string m_ReportPath;
        Microsoft.Office.Interop.Excel.Application m_xlApp;
        Workbook m_xlWorkBook;
        Worksheet m_xlWorkSheet;
        object misValue = System.Reflection.Missing.Value;
        private Range oRange;
        public bool HandleProjectSession(ProjectStatus status)
        {
            return true;
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;

            }
            finally
            {
                GC.Collect();
            }
        }
        public ICGunitHost Owner
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        public string PluginName
        {
            get
            {
                return "ExcelreportGenerator " + Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public PluginType PluginType
        {
            get
            {
                return GUnitFramework.Interfaces.PluginType.SpecialPlugin;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Show(DockPanel dock, DockState state)
        {
            ExcelReportGeneratorUi ui = new ExcelReportGeneratorUi(this);
            ui.Show(dock, state);
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
            return true;
        }

        public string ReportPath
        {
            get
            {
                return m_ReportPath;
            }
            set
            {
                m_ReportPath = value;
                FirePropertyChange("ReportPath");
            }
        }

        public void generateReport(string fileName)
        {
            try
            {
                if (Owner != null)
                {
                    if (Owner.TestRunner != null)
                    {
                        m_xlApp = new Microsoft.Office.Interop.Excel.Application();
                        m_xlWorkBook = m_xlApp.Workbooks.Add(misValue);
                        m_xlWorkSheet = (Worksheet)m_xlWorkBook.Worksheets.get_Item(1);
                        m_xlWorkSheet.Cells[1, 1] = "_Test_Phase_Number";
                        m_xlWorkSheet.Cells[1, 2] = "_Type";
                        m_xlWorkSheet.Cells[1, 3] = "Object Text";
                        m_xlWorkSheet.Cells[1, 4] = "_Expected_Result";
                        m_xlWorkSheet.Cells[1, 5] = "TestCaseName";
                        (m_xlWorkSheet.Range[m_xlWorkSheet.Cells[1, 1], m_xlWorkSheet.Cells[1, 5]]).Interior.Color = XlRgbColor.rgbLightCyan;
                        int j = 2;

                        foreach (ITestSuit suit in Owner.TestRunner.TestSuits)
                        {
                            foreach (ItestCase test in suit.TestCases)
                            {
                                
                                m_xlWorkSheet.Cells[j, 1] = test.TestPhaseNumber;
                                m_xlWorkSheet.Cells[j, 2] = "Test Phase Number";
                                m_xlWorkSheet.Cells[j, 3] = test.TestPhaseNumber;
                                j++;
                                m_xlWorkSheet.Cells[j, 2] = "Test Description";
                                m_xlWorkSheet.Cells[j, 3] = test.TestDescription;
                                


                                foreach (string precondition in test.TestPrecondition)
                                {
                                    j++;
                                    m_xlWorkSheet.Cells[j, 2] = "Test Pre-Condition";
                                    m_xlWorkSheet.Cells[j, 3] = precondition + "\n";

                                }

                                foreach (string TestSteps in test.TestSteps)
                                {
                                    j++;
                                    m_xlWorkSheet.Cells[j, 2] = "Test Step";
                                    m_xlWorkSheet.Cells[j, 3] = TestSteps + "\n";

                                }
                                string ExpectedResultString = "";
                                foreach (string ExpectedResult in test.ExpectedResult)
                                {
                                    ExpectedResultString += ExpectedResult + "\n";

                                }
                                m_xlWorkSheet.Cells[j, 4] = ExpectedResultString;
                                m_xlWorkSheet.Cells[j, 5] = test.Name;
                                j++;

                            }

                        }
                        oRange = m_xlWorkSheet.Range[m_xlWorkSheet.Cells[1, 1], m_xlWorkSheet.Cells[j, 5]];
                        oRange.Borders.get_Item(XlBordersIndex.xlEdgeLeft).LineStyle = XlLineStyle.xlContinuous;
                        oRange.Borders.get_Item(XlBordersIndex.xlEdgeRight).LineStyle = XlLineStyle.xlContinuous;
                        oRange.Borders.get_Item(XlBordersIndex.xlInsideHorizontal).LineStyle = XlLineStyle.xlContinuous;
                        oRange.Borders.get_Item(XlBordersIndex.xlInsideVertical).LineStyle = XlLineStyle.xlContinuous;


                        m_xlWorkBook.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                        m_xlWorkBook.Close(true, misValue, misValue);
                        m_xlApp.Quit();

                        releaseObject(m_xlWorkSheet);
                        releaseObject(m_xlWorkBook);
                        releaseObject(m_xlApp);
                        MessageBox.Show("Excel report Generated");

                    }
                }
            }
            catch
            {

            }
        }
    }
}
