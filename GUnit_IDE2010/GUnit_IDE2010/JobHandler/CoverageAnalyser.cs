using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gunit.DataModel;

namespace GUnit_IDE2010.JobHandler
{
    public class LineStatus
    {
        public string m_line = "";
        public UInt32 m_lineNumber;
        public bool m_isExecutable;
        public UInt32 m_ExecutionCount;

    }
    public class Coverage
    {
        public string m_fileName = "";
        public string m_ActualfileName = "";
        public UInt32 m_LineCount = 0;
        public List<LineStatus> m_LineStatus = new List<LineStatus>();

    }
    public class CoverageSummary
    {
        public string m_fileName = "";
        public string LineCoverage = "";
        public UInt32 TotalLines = 0;
        public UInt32 CoveredLines = 0;
        public Double LineCoveragePrecentage = 0.0;
        public string FunctionCoverage = "";
        public UInt32 TotalFunctions = 0;
        public UInt32 CoveredFunctions = 0;
        public Double FunctionCoveragePrecentage = 0.0;
        public string BranchCoverage = "";
        public UInt32 TotalBranches = 0;
        public UInt32 CoveredBranches = 0;
        public Double BranchCoveragePrecentage = 0.0;
    }
    public class CoverageAnalyser
    {
      
        string m_GcovFile = "";
       
        /// <summary>
        /// 
        /// </summary>
        public CoverageAnalyser()
        {
          
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gcovFileName"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringlist"></param>
        /// <returns></returns>
        private List<string> removeEmptyStrings(string[] stringlist)
        {
            List<string> restOfWord = stringlist.ToList<string>();
            restOfWord.RemoveAll(string.IsNullOrWhiteSpace);
            return restOfWord;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public CoverageSummary CoverageAnalyseSummary(ListofStrings gcovOutput)
        {
            string[] summaryaplitOption = { "% of " };
            CoverageSummary summary = new CoverageSummary();
            foreach(string line in gcovOutput)
            {
                if(line.Contains("File"))
                {
                    string[] splitOPtion = { "File", "'" };
                    string[] patharray = line.Split(splitOPtion,StringSplitOptions.RemoveEmptyEntries);
                   
                    if (patharray.Count() != 0)
                    {
                        
                        summary.m_fileName = patharray[patharray.Count()-1].Trim().Replace('/','\\');
                    }

                }
                else if(line.Contains("Lines executed"))
                {
                    string[] splitOPtion = { ":", "Lines executed" };
                   
                    string[] patharray = line.Split(splitOPtion, StringSplitOptions.RemoveEmptyEntries);
                    if (patharray.Count() != 0)
                    {
                        summary.LineCoverage = patharray[patharray.Count()-1].Trim();
                        string[] elements = summary.LineCoverage.Split(summaryaplitOption, StringSplitOptions.RemoveEmptyEntries);
                        if(elements.Count() > 1)
                        {
                            try
                            {
                                Double covered = Convert.ToDouble(elements[0]);
                                Double total = Convert.ToDouble(elements[1]);
                                Double coveredNumber = covered * total / 100;
                                summary.TotalLines = Convert.ToUInt32(total);
                                summary.CoveredLines = Convert.ToUInt32(coveredNumber);
                                summary.LineCoveragePrecentage = covered;
                            }
                            catch
                            {

                            }


                        }
                    }
                }
                else if(line.Contains("Taken at least once"))
                {
                    string[] splitOPtion = { ":", "Taken at least once" };
                    string[] patharray = line.Split(splitOPtion, StringSplitOptions.RemoveEmptyEntries);
                    if (patharray.Count() != 0)
                    {
                        summary.BranchCoverage = patharray[patharray.Count()-1].Trim();
                        string[] elements = summary.BranchCoverage.Split(summaryaplitOption, StringSplitOptions.RemoveEmptyEntries);
                        if (elements.Count() > 1)
                        {
                            try
                            {
                                Double covered = Convert.ToDouble(elements[0]);
                                Double total = Convert.ToDouble(elements[1]);
                                Double coveredNumber = covered * total / 100;
                                summary.TotalBranches = Convert.ToUInt32(total);
                                summary.CoveredBranches = Convert.ToUInt32(coveredNumber);
                                summary.BranchCoveragePrecentage = covered;
                            }
                            catch
                            {

                            }


                        }
                    }
                }
                else if (line.Contains("Calls executed"))
                {
                    string[] splitOPtion = { ":", "Calls executed" };
                    string[] patharray = line.Split(splitOPtion, StringSplitOptions.RemoveEmptyEntries);
                    if (patharray.Count() != 0)
                    {
                        summary.FunctionCoverage = patharray[patharray.Count()-1].Trim();
                        string[] elements = summary.FunctionCoverage.Split(summaryaplitOption, StringSplitOptions.RemoveEmptyEntries);
                        if (elements.Count() > 1)
                        {
                            try
                            {
                                Double covered = Convert.ToDouble(elements[0]);
                                Double total = Convert.ToDouble(elements[1]);
                                Double coveredNumber = covered * total / 100;
                                summary.TotalFunctions = Convert.ToUInt32(total);
                                summary.CoveredFunctions = Convert.ToUInt32(coveredNumber);
                                summary.FunctionCoveragePrecentage = covered;
                            }
                            catch
                            {

                            }


                        }
                    }
                }
            }

            return summary;
        }
        private string getLineFromFile(string fileName, uint lineNumber)
        {
            string[] lines = File.ReadAllLines(fileName);
            int count = 0;
            foreach (string line in lines)
            {
                count++;
                if (count == lineNumber)
                    return line;
            }
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Coverage Coverage_AnalyseStatementCoverage(string fileName,string originalFilename)
        {
            Coverage m_CoverageReport = new Coverage();
            if (File.Exists(fileName))
            {
                m_CoverageReport.m_LineStatus.Clear(); 
                m_GcovFile = fileName;
                StreamReader reader = new StreamReader(fileName);
                m_CoverageReport.m_fileName = fileName;
                m_CoverageReport.m_ActualfileName = originalFilename;
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
                        lineInfo.m_line = getLineFromFile(originalFilename, lineInfo.m_lineNumber );
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
