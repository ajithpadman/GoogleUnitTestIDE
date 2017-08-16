
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using Gunit.Utils;

namespace TestExecuter
{
    public class GenerateCoverageReport
    {
        private List<CoverageSummary> m_summary = null;
        private List<Coverage> m_DetailedCoverage = null;
        CoverageModel m_analyser;
        private string m_coverageReportPath = "";
        private Double m_LineCoverageMin = 30.0;
        private Double m_FunctionCoverageMin = 30.0;
        private Double m_branchCoverageMin = 30.0;

        private Double m_LineCoverageMedium= 70.0;
        private Double m_FunctionCoverageMedium = 70.0;
        private Double m_branchCoverageMedium = 70.0;



        public GenerateCoverageReport(CoverageModel analyser,string path)
        {
            m_analyser = analyser;
            m_summary = analyser.CoverageSummary;
            m_DetailedCoverage = analyser.DetailedCoverage;

            m_coverageReportPath = path;
        }
       
        public void Generate()
        {
            StringWriter stringWriter = new StringWriter();
            
            if (Directory.Exists(m_coverageReportPath))
            {
               
                using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Html);
                    writer.RenderBeginTag(HtmlTextWriterTag.Head);
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, "Unit test Coverage Report");
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

                    writer.AddAttribute(HtmlTextWriterAttribute.Class,"MainHeading");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center");
                    writer.RenderBeginTag(HtmlTextWriterTag.H1);
                    writer.WriteLine("Unit Test Coverage Report");
                    writer.RenderEndTag();//H1

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Information");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.WriteLine("Time:" + DateTime.UtcNow.Date.ToString());
                    writer.RenderEndTag();//H3

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "Information");
                    writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "left");
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.WriteLine("Project:" + m_analyser.Model.HostModel.Name);
                    writer.RenderEndTag();//H3

                    
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "MainTable");
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "4px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Table);
                    writer.RenderBeginTag(HtmlTextWriterTag.Thead);
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.WriteLine("Filename");
                        writer.RenderEndTag();//td filename
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.WriteLine("Line Coverage");
                        writer.RenderEndTag();//td line coverage 
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.WriteLine("Branch Coverage");
                        writer.RenderEndTag();//td branch coverage
                    writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                        writer.WriteLine("Function Coverage");
                        writer.RenderEndTag();//td function coverage
                    writer.RenderEndTag();//tr
                  
                    writer.RenderEndTag();//thead
                    writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
                    
                    foreach(CoverageSummary summary in m_summary)
                    {
                        if (m_analyser.Model.HostModel.SourceFiles.Contains(summary.m_fileName))
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.AddAttribute(HtmlTextWriterAttribute.Href,  Path.GetFileName(summary.m_fileName) + ".html");
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            
                            writer.WriteLine(Path.GetFileName(summary.m_fileName));
                            writer.RenderEndTag();//A
                            writer.RenderEndTag();//td filename
                            if (summary.LineCoveragePrecentage < m_LineCoverageMin)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMin");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "red");
                            }
                            else if (summary.LineCoveragePrecentage >= m_LineCoverageMin && summary.LineCoveragePrecentage < m_LineCoverageMedium)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMedium");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "yellow");
                            }
                            else 
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMaximum");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "green");
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(summary.LineCoverage);
                            writer.RenderEndTag();//td line coverage 

                            if (summary.BranchCoveragePrecentage < m_branchCoverageMin)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMin");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "red");
                            }
                            else if (summary.BranchCoveragePrecentage >= m_branchCoverageMin && summary.BranchCoveragePrecentage < m_branchCoverageMedium)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMedium");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "yellow");
                            }
                            else 
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMaximum");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "green");
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(summary.BranchCoverage);
                            writer.RenderEndTag();//td branch coverage

                            if (summary.FunctionCoveragePrecentage < m_FunctionCoverageMin)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMin");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "red");
                            }
                            else if (summary.FunctionCoveragePrecentage >= m_FunctionCoverageMin && summary.FunctionCoveragePrecentage < m_FunctionCoverageMedium)
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMedium");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "yellow");
                            }
                            else
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "CoverageMaximum");
                                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "green");
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Border, "1px solid #3A2F0B");
                            writer.RenderBeginTag(HtmlTextWriterTag.Td);
                            writer.WriteLine(summary.FunctionCoverage);
                            writer.RenderEndTag();//td function coverage
                            writer.RenderEndTag();//Tr
                        }
                    }
                   
                    writer.RenderEndTag();//Tbody
                    writer.RenderEndTag();//table
                    writer.RenderEndTag();//body

                    writer.RenderEndTag();//html
                }
                foreach(Coverage coverage in m_DetailedCoverage)
                {
                    StringWriter stringDetailed = new StringWriter();
                    using (HtmlTextWriter writer = new HtmlTextWriter(stringDetailed))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Html);
                        writer.RenderBeginTag(HtmlTextWriterTag.Head);
                        writer.AddAttribute(HtmlTextWriterAttribute.Title, Path.GetFileName(coverage.m_fileName));
                        writer.RenderBeginTag(HtmlTextWriterTag.Title);
                        writer.RenderEndTag();//title
                        writer.RenderEndTag();//head
                        writer.RenderBeginTag(HtmlTextWriterTag.Body);
                       
                            foreach (LineStatus line in coverage.m_LineStatus)
                            {   
                                if(line.m_isExecutable == false)
                                {
                                   writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#FAFAFA");
                                }
                                else if(line.m_ExecutionCount == 0)
                                {
                                   writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#F5BCA9");
                                }
                                else if(line.m_ExecutionCount > 0)
                                {
                                   writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "#58FA58");
                                }
                                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                                writer.WriteLine(line.m_line);
                                writer.RenderEndTag();//Div
                            }
                       
                        writer.RenderEndTag();//body
                        writer.RenderEndTag();//html
                        StreamWriter file = new StreamWriter(m_coverageReportPath + "\\" + Path.GetFileNameWithoutExtension(coverage.m_fileName) + ".html");
                        file.Write(stringDetailed.ToString());
                        file.Close();
                    }
                   
                }
               
            }
            StreamWriter htmlFile = new StreamWriter(m_coverageReportPath + "\\CoverageReport.html");
            htmlFile.Write(stringWriter.ToString());
            htmlFile.Close();
        }

    }
}
