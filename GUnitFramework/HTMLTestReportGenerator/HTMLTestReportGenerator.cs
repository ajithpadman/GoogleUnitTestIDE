using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using System.Web.UI;
using System.IO;
using GUnitFramework.Implementation;
using System.Reflection;
namespace HTMLTestReportGenerator
{
    public class HTMLTestReportGenerator : GUnitFramework.Implementation.PropertyNotifier,ITestReportGenerator
    {
        ICGunitHost m_host;
        string m_path;
        
        public HTMLTestReportGenerator()
        {

        }
        public void generateReport(string fileName)
        {
            if (Owner.TestRunner != null)
            {

                StringWriter stringWriter = new StringWriter();
                using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Html);
                    writer.RenderBeginTag(HtmlTextWriterTag.Head);
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, Path.GetFileNameWithoutExtension(fileName));
                    writer.RenderBeginTag(HtmlTextWriterTag.Title);
                    writer.RenderEndTag();//title
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text / css");
                    writer.AddAttribute(HtmlTextWriterAttribute.Rel, "stylesheet");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "stylesheet.css");
                    writer.RenderBeginTag(HtmlTextWriterTag.Link);
                    writer.RenderEndTag();//link
                    writer.RenderEndTag();//head
                    writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#ECF6CE");
                    writer.RenderBeginTag(HtmlTextWriterTag.Body);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "MainHeading");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                    writer.RenderBeginTag(HtmlTextWriterTag.H1);
                    writer.WriteLine(Path.GetFileNameWithoutExtension(fileName));
                    writer.RenderEndTag();//H1

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Information");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.WriteLine("Time:" + DateTime.UtcNow.Date.ToString());
                    writer.RenderEndTag();//H3

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Information");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.WriteLine("Project:" + Owner.ProjectData.ProjectName);
                    writer.RenderEndTag();//H3

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "MainTable");
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "4px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("TestCase Name");
                    writer.RenderEndTag();//td TestCase Name

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Test Phase number");
                    writer.RenderEndTag();//td Test Phase number

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Description");
                    writer.RenderEndTag();//td Description

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Preconditions");
                    writer.RenderEndTag();//td Preconditions

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Test Steps");
                    writer.RenderEndTag();//td TestSteps

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Expected Result");
                    writer.RenderEndTag();//td Expected Result

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Actual Result");
                    writer.RenderEndTag();//td Actual Result 
                    
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteLine("Comments");
                    writer.RenderEndTag();//td Comments
                    writer.RenderEndTag();//tr
                    foreach (ITestSuit suit in Owner.TestRunner.TestSuits)
                    {
                        foreach (ItestCase test in suit.TestCases)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.Name);
                            writer.RenderEndTag();//td test name

                            
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.TestPhaseNumber);
                            writer.RenderEndTag();//td TestPhaseNumber

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.TestDescription);
                            writer.RenderEndTag();//td TestDescription

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            foreach (string precondition in test.TestPrecondition)
                            {
                                writer.WriteLine(precondition);
                            }

                            writer.RenderEndTag();//td TestPrecondition

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            foreach (string step in test.TestSteps)
                            {
                                writer.WriteLine(step);
                            }

                            writer.RenderEndTag();//td TestSteps

                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            foreach (string exp in test.ExpectedResult)
                            {
                                writer.WriteLine(exp);
                            }

                            writer.RenderEndTag();//td ExpectedResult


                            string status = "";
                            if (test.Status == TestStatus.Error)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "Failure");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "red");
                                status = "FAILURE";
                            }
                            else if (test.Status == TestStatus.NotRun)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "NotRun");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "yellow");
                                 status = "NOT RUN";
                            }
                            else
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMaximum");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "green");
                                status = "SUCCESS";
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);

                            writer.WriteLine(status);
                            writer.RenderEndTag();//td Actual Result
                            
                                                    
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(test.ErrorString);
                            writer.RenderEndTag();//td Comments
                            writer.RenderEndTag();//tr Table Row
                        }
                    }
                    writer.RenderEndTag();//thead
                    writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
                    writer.RenderEndTag();//Tbody
                    writer.RenderEndTag();//table
                    writer.RenderEndTag();//body

                    writer.RenderEndTag();//html
                  
                }
                StreamWriter htmlFile = new StreamWriter(fileName);
                htmlFile.Write(stringWriter.ToString());
                htmlFile.Close();
            }

        }

        public bool HandleProjectSession(ProjectStatus status)
        {
            return true;
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
                return "HTML ReportGenerator" + Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
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
                return GUnitFramework.Interfaces.PluginType.TestReportGenerator;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Show(WeifenLuo.WinFormsUI.Docking.DockPanel dock, WeifenLuo.WinFormsUI.Docking.DockState state)
        {
            HTMLReportGeneratorUi ui = new HTMLReportGeneratorUi(this);
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
                return m_path;
            }
            set
            {
                m_path = value;
                FirePropertyChange("ReportPath");
            }
        }
    }
}
