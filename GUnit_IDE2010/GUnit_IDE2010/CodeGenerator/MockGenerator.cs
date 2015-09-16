using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using GUnit_IDE2010.DataModel;

namespace GUnit_IDE2010.CodeGenerator
{
    public class MockGenerator:BoundaryValueGenerator
    {
        string m_Path = "";
        public MockGenerator(string Path,CodeGenDataModel model):base(model)
        {
            m_Path = Path;
            if (Directory.Exists(m_Path) == false)
            {
                Directory.CreateDirectory(m_Path);
            }
        }
        /// <summary>
        /// Generate the Mock Class
        /// </summary>
        /// <param name="globalMethods"></param>
        /// <param name="moduleName"></param>
        public void generateMockClass(List<GlobalMethods> globalMethods,string moduleName )
        {
            string mockName = "Mock_" + moduleName;
            StreamWriter mock_header = new StreamWriter(m_Path + "\\" + mockName + ".h");
            StreamWriter mock_source = new StreamWriter(m_Path + "\\" + mockName + ".cpp");
            mock_header.WriteLine(writeFileHeader(mockName + ".h"));
            mock_source.WriteLine(writeFileHeader(mockName + ".cpp"));
           
            mock_source.WriteLine("#include \"" + mockName + ".h\"");
            mock_source.WriteLine(mockName + "* " + mockName + "::mp_Instance = 0;");
            mock_header.WriteLine("#ifndef " + mockName.ToUpper());
            mock_header.WriteLine("#define " + mockName.ToUpper());
            mock_header.WriteLine("#include \"gmock/gmock.h\"");
            mock_source.WriteLine(writeModuleHeader("\\class "+ mockName,"Mock class for the module "+ moduleName));
            mock_header.WriteLine("class " + mockName + "{");
            mock_header.WriteLine("public:");
            mock_header.WriteLine(" static " + mockName + " *mp_Instance;");
            mock_header.WriteLine(" " + mockName + "(){mp_Instance = NULL;}");
            mock_header.WriteLine(" virtual" + "~" + mockName + "(){}");
            mock_header.WriteLine(" inline void RegisterMock(" + mockName + " *mock){mp_Instance = mock;}");
            mock_header.WriteLine(" inline void UnRegisterMock(){mp_Instance = NULL;}");
            
         
            foreach (GlobalMethods method in globalMethods)
            {
                List<string> arguments = method.Methods.Parameters.Split(',').ToList();
                arguments.RemoveAll(str => String.IsNullOrEmpty(str));
                int argumentCount = arguments.Count();
                if (argumentCount <= 10 && argumentCount > 0)
                {
                    mock_header.WriteLine(" MOCK_METHOD" + argumentCount + "(mocked_" + method.Methods.EntityName + "," + method.Methods.ReturnType + "(" + method.Methods.Parameters + "));");
                    writeFunctionDefinition(mock_source, method, mockName);
                }
                else
                {
                    if (argumentCount == 0)
                    {
                        mock_header.WriteLine(" MOCK_METHOD" + argumentCount + "(mocked_" + method.Methods.EntityName + "," + method.Methods.ReturnType + "());");
                        writeFunctionDefinition(mock_source, method, mockName);
                    }
                    else
                    {
                        MessageBox.Show(method.Methods.ReturnType + " " + method.Methods.EntityName + " (" + method.Methods.Parameters + ")\nCannot be Mocked as Number of Arguments Exceeds 10");
                    }
                }
            }

            mock_header.WriteLine("};");
            mock_header.WriteLine("#endif");
            mock_header.Close();
            mock_source.Close();
            m_model.FilePath = m_Path + "\\" + mockName + ".h";
            StyleCode();
            m_model.FilePath = m_Path + "\\" + mockName + ".cpp";
            StyleCode();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="method"></param>
        /// <param name="mockClassName"></param>
        private void writeFunctionDefinition(StreamWriter writer, GlobalMethods method,string mockClassName)
        {
            List<string> arguments = method.Methods.Parameters.Split(',').ToList();
            arguments.RemoveAll(str => String.IsNullOrEmpty(str));
            string returntype = "void";
            if (string.IsNullOrWhiteSpace(method.Methods.ReturnType) == false)
            {
                returntype = method.Methods.ReturnType;
            }
            writer.Write(returnFunctionHeader(method.Methods.EntityName));
            string parameters = "";
            
            if (arguments.Count() > 0)
            {
                writer.Write(returntype + " " + method.Methods.EntityName + "");
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
                writer.WriteLine(returntype + " " + method.Methods.EntityName + "()");
                
              
            }
            writer.WriteLine("{");
            if (returntype != "void")
            {
                writer.WriteLine("  if(NULL != "+mockClassName+"::mp_Instance)");
                writer.WriteLine("  {");
                writer.WriteLine("    return " + mockClassName + "::mp_Instance->mocked_" + method.Methods.EntityName + "(" + parameters + ");");
                writer.WriteLine("  }");
                writer.WriteLine("  else");
                writer.WriteLine("  {");
                writer.WriteLine("    " + mockClassName + " l_MockObject;");
                Member valueMember = null;
                valueMember = DatatypeMinMember(method.Methods.DataType, mockClassName + "RetVal");
                if(valueMember!= null)
                {
                    writer.WriteLine(valueMember.member);
                    writer.WriteLine(valueMember.value);
                    writer.WriteLine("EXPECT_CALL");
                    writer.WriteLine("(");
                    writer.WriteLine("l_MockObject,");
                    writer.WriteLine("mocked_" + method.Methods.EntityName + "(" + parameters + ")");
                    writer.WriteLine(")");
                    writer.WriteLine(".Times(::testing::AnyNumber())");
                    writer.WriteLine(".WillRepeatedly(::testing::Return(" + valueMember.memberVariable + "));");
                   
                }
                writer.WriteLine("    return l_MockObject.mocked_" + method.Methods.EntityName + "(" + parameters + ");");
                writer.WriteLine("  }");
            }
            else
            {
                writer.WriteLine("  if(NULL != " + mockClassName + "::mp_Instance)");
                writer.WriteLine("  {");
                writer.WriteLine("     " + mockClassName + "::mp_Instance->mocked_" + method.Methods.EntityName + "(" + parameters + ");");
                writer.WriteLine("  }");
                writer.WriteLine("  else");
                writer.WriteLine("  {");
                writer.WriteLine("    " + mockClassName + " l_MockObject;");
                writer.WriteLine("      l_MockObject.mocked_" + method.Methods.EntityName + "(" + parameters + ");");
                writer.WriteLine("  }");
            }

            writer.WriteLine("}");

        }
        public void generateMockClass(Classes classObject)
        {

            string mockName = "Mock_" + classObject.EntityName;
            StreamWriter mock_header = new StreamWriter(m_Path + "\\" + mockName + ".h");
            addFileHeader(
              mock_header,
              mockName + ".h",
              "This file implements the mock for the " + classObject.EntityName,
              System.Security.Principal.WindowsIdentity.GetCurrent().Name,
              "1.0",
              DateTime.UtcNow.Date.ToString()
          );
            mock_header.WriteLine("#ifndef " + mockName.ToUpper());
            mock_header.WriteLine("#define " + mockName.ToUpper());
            mock_header.WriteLine("#include \"gmock/gmock.h\"");
            mock_header.WriteLine("class " + mockName + ":public " + classObject.EntityName + "{");
            mock_header.WriteLine("public:");
            mock_header.WriteLine(" " + mockName + "(){}");
            mock_header.WriteLine(" " + "~" + mockName + "(){}");
            foreach (MemberMethods method in classObject.MemberMethods)
            {
                if (method.IsStaticCxxMethod == false)
                {
                    string mockString = " MOCK_METHOD";
                    if (method.Methods.IsConstMethod)
                    {
                        mockString = " MOCK_CONST_METHOD";
                    }
                    List<string> arguments = method.Methods.Parameters.Split(',').ToList();
                    arguments.RemoveAll(str => String.IsNullOrEmpty(str));
                    int argumentCount = arguments.Count();
                    if (argumentCount <= 10 && argumentCount > 0)
                    {
                        mock_header.WriteLine(mockString + argumentCount + "(" + method.Methods.EntityName + "," + method.Methods.ReturnType + "(" + method.Methods.Parameters + "));");
                    }
                    else
                    {
                        if (argumentCount == 0)
                        {
                            mock_header.WriteLine(mockString + argumentCount + "(" + method.Methods.EntityName + "," + method.Methods.ReturnType + "());");
                        }
                        else
                        {
                            MessageBox.Show(method.Methods.ReturnType + " " + method.Methods.EntityName + " (" + method.Methods.Parameters + ")\nCannot be Mocked as Number of Arguments Exceeds 10");
                        }
                    }
                }
            }
            mock_header.WriteLine("};");
            mock_header.WriteLine("#endif");

            mock_header.Close();
            m_model.FilePath = m_Path + "\\" + mockName + ".h";
            StyleCode();
           


        }
        public static  void addFileHeader(StreamWriter writer, string filename, string desc, string author, string version, string date)
        {
            writer.WriteLine("/*********************************************************************/");
            writer.WriteLine("/*! ");
            writer.WriteLine("* \\file " + filename);
            writer.WriteLine("* " + desc);
            writer.WriteLine("* \\author " + author);
            writer.WriteLine("* \\version " + version);
            writer.WriteLine("* \\date " + date);
            writer.WriteLine("*/");
            writer.WriteLine("/*********************************************************************/");
        }
        public static void addClassHeader(StreamWriter writer, string className, string desc, string author, string version, string date)
        {
            writer.WriteLine("/*********************************************************************/");
            writer.WriteLine("/*! ");
            writer.WriteLine("* \\class " + className);
            writer.WriteLine("* " + desc);
            writer.WriteLine("* \\author " + author);
            writer.WriteLine("* \\version " + version);
            writer.WriteLine("* \\date " + date);
            writer.WriteLine("*/");
            writer.WriteLine("/*********************************************************************/");
        }
        public static string returnFunctionHeader(string functionName,string description = null)
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
    }
}
