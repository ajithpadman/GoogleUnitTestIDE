using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnitFramework.Interfaces;
using System.IO;
using ASTBuilder.Interfaces;
using GUnitFramework.Implementation;
using System.Reflection;
namespace MockGenerator
{
    public class MockGenerator:IMockGenrator
    {
        ICGunitHost m_host;
        string m_Path;
        
        public MockGenerator()
        {

        }

        public string Path
        {
            get
            {
                return m_Path;
            }
            set
            {
                m_Path = value;
            }
        }
        protected  string writeFileHeader(string fileName,string description)
        {
            StringWriter writer = new StringWriter();
            writer.WriteLine("/*********************************************************************/");
            writer.WriteLine("/*! ");
            writer.WriteLine("* \\file " + fileName);
            writer.WriteLine("* " + description);
            writer.WriteLine("* \\author " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            writer.WriteLine("* \\version 1.0");
            writer.WriteLine("* \\date " + DateTime.UtcNow.Date.ToString());
            writer.WriteLine("*/");
            writer.WriteLine("/*********************************************************************/");
            return writer.ToString();
        }
        protected virtual string writeModuleHeader(string moduleNameTag, string description = "")
        {
            StringWriter writer = new StringWriter();
            writer.WriteLine("/*********************************************************************/");
            writer.WriteLine("/*! ");
            writer.WriteLine("*  " + moduleNameTag);
            writer.WriteLine("* " + description);
            writer.WriteLine("* \\author " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            writer.WriteLine("* \\version 1.0");
            writer.WriteLine("* \\date " + DateTime.UtcNow.Date.ToString());
            writer.WriteLine("*/");
            writer.WriteLine("/*********************************************************************/");
            return writer.ToString();
        }
        public static string returnFunctionHeader(string functionName, string description = null)
        {
            string functionHeader = "";
            functionHeader += "/*********************************************************************/\n";
            functionHeader += "/*! \\fn " + functionName + "\n";
            if (description != null)
            {
                functionHeader += "* \\brief " + description + "()\n";
            }
            else
            {
                functionHeader += "* \\brief Mock for the function " + functionName + "()\n";
            }
            functionHeader += "*/\n";
            functionHeader += "/*********************************************************************/\n";
            return functionHeader;
        }
        private List<string> functionArgumentTypes(ICFunction function)
        {
            List<string> arguments = new List<string>();
            foreach (ICVariable param in function.Parameters)
            {
                arguments.Add(param.Type.Name);
            }
            return arguments;
        }
        private void writeFunctionDefinition(StreamWriter writer, ICFunction method, string mockClassName)
        {
            List<string> arguments = functionArgumentTypes(method);
            arguments.RemoveAll(str => String.IsNullOrEmpty(str));
            string returntype = "void";
            if (string.IsNullOrWhiteSpace(method.ReturnValue.Name) == false)
            {
                returntype = method.ReturnValue.Name;
            }
            writer.Write(returnFunctionHeader(method.Name));
            string parameters = "";

            if (arguments.Count() > 0)
            {
                writer.Write(returntype + " " + method.Name + "");
                int count = 0;
                List<string> withValues = new List<string>();
                List<string> Values = new List<string>();
                foreach (string argument in arguments)
                {
                    withValues.Add(argument + " l_arg" + count);
                    Values.Add(" l_arg" + count);
                    count++;
                }
                writer.Write("(" + String.Join(",", withValues.ToArray()) + ")");
                parameters = String.Join(",", Values.ToArray());





            }
            else
            {
                writer.WriteLine(returntype + " " + method.Name + "()");


            }
            writer.WriteLine("{");
            if (returntype != "void")
            {
                writer.WriteLine("  if(NULL != " + mockClassName + "::mp_Instance)");
                writer.WriteLine("  {");
                writer.WriteLine("    return " + mockClassName + "::mp_Instance->mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
                writer.WriteLine("  else");
                writer.WriteLine("  {");
                writer.WriteLine("    " + mockClassName + " l_MockObject;");

                writer.WriteLine("    return l_MockObject.mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
            }
            else
            {
                writer.WriteLine("  if(NULL != " + mockClassName + "::mp_Instance)");
                writer.WriteLine("  {");
                writer.WriteLine("     " + mockClassName + "::mp_Instance->mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
                writer.WriteLine("  else");
                writer.WriteLine("  {");
                writer.WriteLine("    " + mockClassName + " l_MockObject;");
                writer.WriteLine("      l_MockObject.mocked_" + method.Name + "(" + parameters + ");");
                writer.WriteLine("  }");
            }

            writer.WriteLine("}");

        }
        public void generateMock(ICCodeDescription description)
        {
            string mockName = "Mock_"+System.IO.Path.GetFileNameWithoutExtension(description.FileName);
            
            StreamWriter mock_header = new StreamWriter(m_Path + "\\" + mockName + ".h");
            StreamWriter mock_source = new StreamWriter(m_Path + "\\" + mockName + ".cpp");
            mock_header.WriteLine(writeFileHeader(mockName + ".h","mock for Google test creation"));
            mock_source.WriteLine(writeFileHeader(mockName + ".cpp","mock for Google test creation"));

            mock_source.WriteLine("#include \"" + mockName + ".h\"");
            mock_source.WriteLine(mockName + "* " + mockName + "::mp_Instance = 0;");
            mock_header.WriteLine("#ifndef " + mockName.ToUpper());
            mock_header.WriteLine("#define " + mockName.ToUpper());
            mock_header.WriteLine("#include \"gmock/gmock.h\"");
            mock_source.WriteLine(writeModuleHeader("\\class " + mockName, "Mock class for the module " + mockName));
            mock_header.WriteLine("class " + mockName + "{");
            mock_header.WriteLine("public:");
            mock_header.WriteLine(" static " + mockName + " *mp_Instance;");
            mock_header.WriteLine(" " + mockName + "(){mp_Instance = NULL;}");
            mock_header.WriteLine(" virtual" + "~" + mockName + "(){}");
            mock_header.WriteLine(" inline void RegisterMock(" + mockName + " *mock){mp_Instance = mock;}");
            mock_header.WriteLine(" inline void UnRegisterMock(){mp_Instance = NULL;}");


            foreach (ICFunction function in description.Functions)
            {
                List<string> arguments = functionArgumentTypes(function);
                arguments.RemoveAll(str => String.IsNullOrEmpty(str));
                int argumentCount = arguments.Count();
                if (argumentCount <= 10 && argumentCount > 0)
                {
                    mock_header.WriteLine(" MOCK_METHOD" + argumentCount + "(mocked_" + function.Name + "," + function.ReturnValue.Name + "(" + String.Join(",",arguments.ToArray()) + "));");
                    writeFunctionDefinition(mock_source, function, mockName);
                }
                else
                {
                    if (argumentCount == 0)
                    {
                        mock_header.WriteLine(" MOCK_METHOD" + argumentCount + "(mocked_" + function.Name + "," + function.ReturnValue.Name + "());");
                        writeFunctionDefinition(mock_source, function, mockName);
                    }
                    else
                    {
                        Console.WriteLine(function.ReturnValue.Name + " " + function.Name + " (" + String.Join(",", arguments.ToArray()) + ")\nCannot be Mocked as Number of Arguments Exceeds 10");
                    }
                }
            }

            mock_header.WriteLine("};");
            mock_header.WriteLine("#endif");
            mock_header.Close();
            mock_source.Close();

            StyleCode(m_Path + "\\" + mockName + ".h");

            StyleCode(m_Path + "\\" + mockName + ".cpp");
        }
        protected virtual void StyleCode(string fileName)
        {
            if (File.Exists("AStyle.exe"))
            {
                Job job = new Job();
                job.Command = "AStyle.exe";
                job.Argument = "--style=gnu --indent-classes --mode=c " + fileName;
                ExternalProcessHandler process = new ExternalProcessHandler(job);
                process.RunExternalProcess(job);
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
                return "MockGenerator " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
                return GUnitFramework.Interfaces.PluginType.MockGenerator;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Show(WeifenLuo.WinFormsUI.Docking.DockPanel dock, WeifenLuo.WinFormsUI.Docking.DockState state)
        {
            MockeGeneratorUi ui = new MockeGeneratorUi(this);
            ui.Show(dock, state);
        }

        public bool registerCallBack(ICGunitHost host)
        {
            Owner = host;
            return true;
        }

      
    }
}
