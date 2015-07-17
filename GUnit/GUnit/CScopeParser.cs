using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
namespace GUnit
{
    class CScopeParser
    {
        GUnit m_Parent;
        string m_files;
        CtagParser m_ctagParser;
        List<string> calledFunctionsList = new List<string>();
        public CScopeParser(GUnit parent)
        {
            m_Parent = parent;
            m_ctagParser = new CtagParser(m_Parent);
        }
        public void Cscope_CreateDataBase()
        {
            try
            {
                StreamWriter writer = new StreamWriter(Directory.GetCurrentDirectory() + "\\cscope.files");
                foreach (string file in m_Parent.m_data.m_ProjectHashTable.Keys)
                {
                    writer.WriteLine(file);
                }
                writer.Close();
                string cscope = prepareforExecution();
                string output = m_ctagParser.RunExternalExe(cscope, " -R -b -i cscope.files");
                clearExecution();
            }
            catch
            {

            }
        }
        public void Cscope_getFunctionList(FunctionalInterface function)
        {
            try
            {
                string cscope = prepareforExecution();
                string output = m_ctagParser.RunExternalExe(cscope, " -L2 " + function.m_FunctionName);
                string[] cscopeLines = output.Split('\n');
                function.m_CalledFunctionList.Clear();
                foreach (string line in cscopeLines)
                {
                    if (string.IsNullOrWhiteSpace(line) == false)
                    {
                        FunctionalInterface calledFunction = new FunctionalInterface();
                        string Cscopeline = line.Trim();
                        string[] stringSeparators = new string[] { function.m_FileName, "\t", " " };
                        string[] tagElement = Cscopeline.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        if (calledFunctionsList.Contains(tagElement[0]) == false)
                        {
                            calledFunction.m_FunctionName = tagElement[0];
                            Cscopeline = Cscopeline.Substring(tagElement[0].Length, Cscopeline.Length - tagElement[0].Length);
                            stringSeparators = new string[] { "\t", " " };
                            tagElement = Cscopeline.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            try
                            {
                                calledFunction.m_LineNo = Convert.ToInt16(tagElement[0]);
                            }
                            catch
                            {
                                calledFunction.m_LineNo = function.m_LineNo;
                            }

                            function.m_CalledFunctionList.Add(calledFunction);
                            calledFunctionsList.Add(calledFunction.m_FunctionName);
                        }
                    }


                }
                clearExecution();
                calledFunctionsList.Clear();
            }
            catch
            {

            }

        }
        
        private string prepareforExecution()
        {
            try
            {
                string currentDirect = Directory.GetCurrentDirectory();
                string cscopePath = currentDirect + "\\cscope" + Thread.CurrentThread.ManagedThreadId + ".exe";
                string prevDir = Directory.GetCurrentDirectory();
                Directory.SetCurrentDirectory(currentDirect);
                m_ctagParser.ExtractResource("GUnit.cscope.exe", cscopePath);
                return cscopePath;
            }
            catch
            {
                return null;
            }

        }
        private void clearExecution()
        {
            string currentDirect = Directory.GetCurrentDirectory();
            if (File.Exists(currentDirect + "\\cscope" + Thread.CurrentThread.ManagedThreadId + ".exe"))
            {
                File.Delete(currentDirect + "\\cscope" + Thread.CurrentThread.ManagedThreadId + ".exe");
            }
            
        }
    }
}
