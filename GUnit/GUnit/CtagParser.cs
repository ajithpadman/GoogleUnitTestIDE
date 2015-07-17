using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace GUnit
{
    public class CtagParser
    {
        string[] Functionqualifiers = { "static", "extern", "inline" };
        GUnit m_parent;
        enum DeclType
        {
            FUNCTION_PROTOTYPE,
            FUNCTION_DEFINITION
        };
        public delegate void onRedirectData(string Text);
        public event onRedirectData evRedirectData = delegate { };

        public CtagParser(GUnit parent)
        {
            m_parent = parent;
        }
        /*********************************************************************/
        /*! \fn ExtractResource
        * \brief To extract the CTag Exe embeeded in the application
        * \return void
        */
        /*********************************************************************/
        public void ExtractResource(string resource, string path)
        {
            string Dir = Path.GetDirectoryName(path);
            if (Directory.Exists(Dir))
            {
                Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
                byte[] bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                File.WriteAllBytes(path, bytes);
            }

        }
        /*********************************************************************/
        /*! \fn RunExternalExe
        * \brief for executing the Ctag Exe
        * \return void
        */
        /*********************************************************************/
        public string RunExternalExe(string filename, string arguments = null)
        {
            var process = new Process();

            process.StartInfo.FileName = filename;
            if (!string.IsNullOrEmpty(arguments))
            {
                process.StartInfo.Arguments = arguments;
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            var stdOutput = new StringBuilder();
            process.OutputDataReceived += (sender, args) => stdOutput.AppendLine(args.Data);
            // see below for output handler
            // process.ErrorDataReceived += proc_DataReceived;
            // process.OutputDataReceived += proc_DataReceived;
            string stdError = null;
            try
            {
                process.Start();
                process.BeginOutputReadLine();
                stdError = process.StandardError.ReadToEnd();

                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception("OS error while executing " + Format(filename, arguments) + ": " + e.Message, e);
            }

            if (process.ExitCode == 0)
            {

                return stdOutput.ToString();

            }
            else
            {
                var message = new StringBuilder();

                if (!string.IsNullOrEmpty(stdError))
                {
                    message.AppendLine(stdError);
                }

                if (stdOutput.Length != 0)
                {
                    message.AppendLine("Std output:");
                    message.AppendLine(stdOutput.ToString());
                }
                process.Kill();
                throw new Exception(Format(filename, arguments) + " finished with exit code = " + process.ExitCode + ": " + message);
            }

        }
        void proc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                m_parent.GUnit_updateConsole((Environment.NewLine + e.Data));
        }
        /*********************************************************************/
        /*! \fn Format
        * \brief Formating the Command to System to execute an external Exe
        * \return void
        */
        /*********************************************************************/
        private string Format(string filename, string arguments)
        {
            return "'" + filename +
                ((string.IsNullOrEmpty(arguments)) ? string.Empty : " " + arguments) +
                "'";
        }
        public List<FunctionalInterface> GUnit_findFunctionDefinitions(string filePath)
        {
            List<FunctionalInterface> FuncList = new List<FunctionalInterface>();
            
                string prevDir = Directory.GetCurrentDirectory();
                prepareforExecution();
               
                string output = RunExternalExe("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe", "  --c++-kinds=f  --fields=+znatsSK −−tag−relative=yes -f functions" + Thread.CurrentThread.ManagedThreadId + ".txt " + filePath);
                File.Delete("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe");
                Directory.SetCurrentDirectory(prevDir);
                if (File.Exists("functions" + Thread.CurrentThread.ManagedThreadId + ".txt"))
                {
                    string tagLine = "";


                    StreamReader readTag = new StreamReader("functions" + Thread.CurrentThread.ManagedThreadId + ".txt");
                    while ((tagLine = readTag.ReadLine()) != null)
                    {
                        if (tagLine.Contains("!_TAG_"))
                        {
                            continue;
                        }
                        else
                        {
                            FunctionalInterface functoAdd = GUnit_getFunctionInformation(filePath, tagLine, DeclType.FUNCTION_DEFINITION);
                            m_parent.GUnit_correctFunctionInterface(functoAdd);
                            FuncList.Add(functoAdd);
                        }
                    }
                    readTag.Close();
                }
                File.Delete("functions" + Thread.CurrentThread.ManagedThreadId + ".txt");

           
            return FuncList;
        }
        private FunctionalInterface GUnit_getFunctionInformation(string filePath, string tagLine, DeclType type)
        {
            FunctionalInterface functionDefine = new FunctionalInterface();
            
                functionDefine.m_FileName = filePath;
                string[] stringSeparators = new string[] { "\t", " " };
                string[] tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                functionDefine.m_FunctionName = tagElement[0];
                tagLine = tagLine.Substring(tagElement[0].Length, tagLine.Length - tagElement[0].Length);

                stringSeparators = new string[] { "/^" };
                tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                tagLine = tagLine.Substring(tagElement[0].Length + 2, tagLine.Length - tagElement[0].Length - 2);
                stringSeparators = new string[] { functionDefine.m_FunctionName };
                tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (tagElement[0].Trim().StartsWith("virtual"))
                {
                    functionDefine.m_IsVirtual = true;
                    stringSeparators = new string[] { "virtual" };
                    string[] values = tagElement[0].Trim().Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Count() > 0)
                    {
                        functionDefine.m_ReturnType = values[0];
                    }
                    else
                    {
                        functionDefine.m_ReturnType = "";
                    }
                }
                else
                {
                    functionDefine.m_ReturnType = tagElement[0].Trim();
                    functionDefine.m_IsVirtual = false;
                }
                string returnType = "";
                foreach (string returnElement in functionDefine.m_ReturnType.Split(' '))
                {
                    if (Functionqualifiers.Contains(returnElement) == false)
                    {
                        returnType += returnElement;
                    }
                }
                functionDefine.m_ReturnType = returnType;

                if (tagLine.Contains("line:"))
                {
                    stringSeparators = new string[] { "line:" };
                    tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    tagLine = tagLine.Substring(tagElement[0].Length + 5, tagLine.Length - tagElement[0].Length - 5);
                    stringSeparators = new string[] { "\t", " " };
                    tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    tagLine = tagLine.Substring(tagElement[0].Length, tagLine.Length - tagElement[0].Length);
                    try
                    {
                        functionDefine.m_LineNo = Convert.ToInt16(tagElement[0]);
                    }
                    catch
                    {
                        functionDefine.m_LineNo = 0;
                    }
                }
                if (tagLine.Contains("class:"))
                {
                    stringSeparators = new string[] { "class:" };
                    tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    tagLine = tagLine.Substring(tagElement[0].Length + 6, tagLine.Length - tagElement[0].Length - 6);
                    stringSeparators = new string[] { "\t", " " };
                    tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    tagLine = tagLine.Substring(tagElement[0].Length, tagLine.Length - tagElement[0].Length);
                    functionDefine.m_ClassName = tagElement[0];
                }
                else
                {
                    functionDefine.m_ClassName = Path.GetFileName(functionDefine.m_FileName);
                }
                if (type == DeclType.FUNCTION_PROTOTYPE)
                {
                    if (tagLine.Contains("access:"))
                    {
                        stringSeparators = new string[] { "access:" };
                        tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        tagLine = tagLine.Substring(tagElement[0].Length + 7, tagLine.Length - tagElement[0].Length - 7);
                        stringSeparators = new string[] { "\t", " " };
                        tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                        tagLine = tagLine.Substring(tagElement[0].Length, tagLine.Length - tagElement[0].Length);
                        functionDefine.m_AccessScope = tagElement[0];
                    }
                    else
                    {
                        functionDefine.m_AccessScope = "public";
                    }
                }

                if (tagLine.Contains("signature:"))
                {
                    stringSeparators = new string[] { "signature:" };
                    tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    tagLine = tagLine.Substring(tagElement[0].Length + 10, tagLine.Length - tagElement[0].Length - 10);
                    functionDefine.m_Signature = tagLine;
                    stringSeparators = new string[] { "(", ")" };
                    string[] paramList = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (paramList.Count() != 0)
                    {
                        tagLine = paramList[0];
                    }
                    else
                    {
                        tagLine = "void";
                    }
                    tagLine = tagLine.Trim();
                    if (tagLine.Contains(','))
                    {
                        string[] count = tagLine.Split(',');
                        functionDefine.m_argumentCount = count.Count();
                    }
                    else
                    {
                        functionDefine.m_argumentCount = 1;
                    }

                }
                else
                {
                    functionDefine.m_Signature = ("(void)");
                    functionDefine.m_argumentCount = 0;
                }
            
           
            return functionDefine;
        }
        public List<FunctionalInterface> GUnit_findFunctionList(string filePath)
        {
            List<FunctionalInterface> FuncList = new List<FunctionalInterface>();
           
                string prevDir = Directory.GetCurrentDirectory();
                prepareforExecution();

                string output = RunExternalExe("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe", "  --c++-kinds=p  --fields=+znatsSK −−tag−relative=yes -f functionsDef" + Thread.CurrentThread.ManagedThreadId + ".txt " + filePath);
                File.Delete("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe");
                Directory.SetCurrentDirectory(prevDir);
                if (File.Exists("functionsDef" + Thread.CurrentThread.ManagedThreadId + ".txt"))
                {
                    string tagLine = "";
                    StreamReader readTag = new StreamReader("functionsDef" + Thread.CurrentThread.ManagedThreadId + ".txt");
                    while ((tagLine = readTag.ReadLine()) != null)
                    {
                        if (tagLine.Contains("!_TAG_"))
                        {
                            continue;
                        }
                        else
                        {
                            FunctionalInterface funcToAdd = GUnit_getFunctionInformation(filePath, tagLine, DeclType.FUNCTION_PROTOTYPE);
                            m_parent.GUnit_correctFunctionInterface(funcToAdd);
                            FuncList.Add(funcToAdd);
                        }
                    }
                    readTag.Close();

                }

                File.Delete("functionsDef" + Thread.CurrentThread.ManagedThreadId + ".txt");

           
            return FuncList;
        }

        public DataInterface GUnit_getDataInterfaceList(string filePath)
        {
            string[] ctagArg = { "e", "t", "s", "u", "d", "c", "v" };

            DataInterface data = new DataInterface();

            data.m_enumValues = GUnit_getDataElementList(filePath, ctagArg[0]);
            data.m_Typedefs = GUnit_getDataElementList(filePath, ctagArg[1]);
            data.m_structuresNames = GUnit_getDataElementList(filePath, ctagArg[2]);
            data.m_UnionNames = GUnit_getDataElementList(filePath, ctagArg[3]);
            data.m_MacroNames = GUnit_getDataElementList(filePath, ctagArg[4]);
            data.m_ClassNames = GUnit_getDataElementList(filePath, ctagArg[5]);
            data.m_GlobalVariables = GUnit_getDataElementList(filePath, ctagArg[6]);
            return data;
        }
        private void prepareforExecution()
        {
            string currentDirect = Directory.GetCurrentDirectory();
            string ctagPath = currentDirect + "\\ctags" + Thread.CurrentThread.ManagedThreadId + ".exe";
            string prevDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(currentDirect);
            ExtractResource("GUnit.ctags.exe", ctagPath);


        }
        List<string> GUnit_getDataElementList(string FilePath, string arg)
        {
            List<string> enums = new List<string>();

          

                string prevDir = Directory.GetCurrentDirectory();
                prepareforExecution();
                string output = RunExternalExe("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe", "  --c++-kinds=" + arg + "  --fields=+znatsSK −−tag−relative=yes -f DataDef" + Thread.CurrentThread.ManagedThreadId + ".txt " + FilePath);
                File.Delete("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe");
                Directory.SetCurrentDirectory(prevDir);

                if (File.Exists("DataDef" + Thread.CurrentThread.ManagedThreadId + ".txt"))
                {
                    string tagLine = "";
                    StreamReader readTag = new StreamReader("DataDef" + Thread.CurrentThread.ManagedThreadId + ".txt");
                    while ((tagLine = readTag.ReadLine()) != null)
                    {
                        if (tagLine.Contains("!_TAG_"))
                        {
                            continue;
                        }
                        else
                        {

                            string[] stringSeparators = new string[] { "\t", " " };
                            string[] tagElement = tagLine.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                            enums.Add(tagElement[0]);
                        }
                    }
                    readTag.Close();
                }
                File.Delete("DataDef" + Thread.CurrentThread.ManagedThreadId + ".txt");

        
            return enums;
        }

        public List<UnitInfo> GUnit_findFileFunctionIF(string filePath)
        {

            List<UnitInfo> classList = new List<UnitInfo>();
           

                List<string> Classes = new List<string>();
                m_parent.GUnit_UpdateStatus("Finding Function Prototypes in" + Path.GetFileName(filePath));
                List<FunctionalInterface> funcList = GUnit_findFunctionList(filePath);
                m_parent.GUnit_UpdateStatus("Finding Function Definitions in" + Path.GetFileName(filePath));
                List<FunctionalInterface> funcDefList = GUnit_findFunctionDefinitions(filePath);

                string prevDir = Directory.GetCurrentDirectory();
                prepareforExecution();

                string output = RunExternalExe("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe", " -x  --c++-kinds=c  --fields=+znatsSK " + filePath);
                File.Delete("ctags" + Thread.CurrentThread.ManagedThreadId + ".exe");
                Directory.SetCurrentDirectory(prevDir);

                string[] ctagLines = output.Split('\n');

                foreach (string line in ctagLines)
                {

                    if (string.IsNullOrWhiteSpace(line) == false)
                    {
                        UnitInfo info = new UnitInfo();
                        Classes.Add(line.Split(' ')[0].ToString());
                        info.m_className = line.Split(' ')[0].ToString();
                        info.m_fileName = filePath;
                        info.m_IsClass = true;

                        classList.Add(info);
                    }
                }
                UnitInfo Fileinfo = new UnitInfo();
                Fileinfo.m_fileName = filePath;
                Fileinfo.m_className = Path.GetFileName(filePath);
                Fileinfo.m_IsClass = false;
                classList.Add(Fileinfo);
                foreach (FunctionalInterface function in funcList)
                {
                    foreach (UnitInfo classInfo in classList)
                    {
                        if (classInfo.m_className == function.m_ClassName)
                        {
                            classInfo.m_functionPrototypeList.Add(function);
                        }

                    }


                }
                foreach (FunctionalInterface function in funcDefList)
                {
                    foreach (UnitInfo classInfo in classList)
                    {
                        if (classInfo.m_className == function.m_ClassName)
                        {
                            classInfo.m_functionDefinitionList.Add(function);
                        }

                    }


                }

           
            return classList;

        }
    }
}
