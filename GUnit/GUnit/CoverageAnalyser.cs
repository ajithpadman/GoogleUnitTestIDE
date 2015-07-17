using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace GUnit
{
    public class CoverageAnalyser
    {
        GUnit m_parent;
        string m_GcovFile = "";
        Coverage m_CoverageReport = new Coverage();
        public CoverageAnalyser(GUnit parent)
        {
            m_parent = parent;
            
        }
        private string findFileName(string gcovFileName)
        {
            
            string fileName = Path.GetFileName(gcovFileName);
            if (fileName.Contains(".gcov"))
            {
                string[] file = fileName.Split('.');
                if (file.Count() == 3)
                {
                    fileName = file[0] + "." + file[1];
                }
            }
            return fileName;
        }
        private List<string> removeEmptyStrings(string[] stringlist)
        {
            List<string> restOfWord = stringlist.ToList<string>();
            restOfWord.RemoveAll(string.IsNullOrWhiteSpace);
            return restOfWord;

        }
        public Coverage Coverage_AnalyseStatementCoverage(string fileName)
        {

            if(File.Exists(fileName))
            {
                m_CoverageReport.m_LineStatus.Clear(); 
                m_GcovFile = fileName;
                StreamReader reader = new StreamReader(fileName);
                m_CoverageReport.m_fileName = fileName;
                string line = "";
               
                while ((line = reader.ReadLine()) != null)
                {
                    LineStatus lineInfo = new LineStatus();
                    if (line.Contains("#####:"))
                    {
                        lineInfo.m_ExecutionCount = 0;
                        lineInfo.m_isExecutable = true;
                        
                    }
                    else if (line.Contains("-:"))
                    {
                        lineInfo.m_ExecutionCount = 0;
                        lineInfo.m_isExecutable = false;
                    
                    }
                    else
                    {
                        lineInfo.m_isExecutable = true;
                       
                        string execCount = line.Trim().Split(':')[0];
                        try
                        {
                            lineInfo.m_ExecutionCount = Convert.ToUInt32(execCount);
                           
                        }
                        catch
                        {
                            lineInfo.m_ExecutionCount = 1;
                        }
                    }
                    List<string> splitArray = removeEmptyStrings(line.Trim().Split(':'));
                    try
                    {
                        lineInfo.m_lineNumber = Convert.ToUInt32(splitArray[1]);

                    }
                    catch
                    {
                        
                    }
                    m_CoverageReport.m_LineStatus.Add(lineInfo);
                  
                }
                

            }
            return m_CoverageReport;
        }
        
    }
}
