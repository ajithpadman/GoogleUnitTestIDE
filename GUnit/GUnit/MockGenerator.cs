using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
namespace GUnit
{
    public class MockGenerator
    {
        GUnit m_parent;
        public MockGenerator(GUnit parent)
        {
            m_parent = parent;
        }
        public void MockGen_GenerateMock(UnitInfo unit)
        {
            if (unit.m_IsClass == true)
            {
                MockGen_GenerateCppMock(unit);
            }
            else
            {
                MockGen_GenerateCMock(unit);
            }
        }
        private void MockGen_GenerateCppMock(UnitInfo unit)
        {
            string mockName = "";
            string mockPath = Path.GetDirectoryName(m_parent.m_data.m_Project.m_ProjectPath) + "\\" + m_parent.m_data.m_Project.m_ProjectName + "_Stubs";
            if (Directory.Exists(mockPath) == false)
            {
                Directory.CreateDirectory(mockPath);
            }
            string file = unit.m_className;
            if (file.Contains('.'))
            {
                mockName = "Mock_" + file.Split('.')[0];
            }
            else
            {
                mockName = "Mock_" + file;
            }
            StreamWriter mock_header = new StreamWriter(mockPath + "\\" + mockName + ".h");
            StreamWriter mock_source = new StreamWriter(mockPath + "\\" + mockName + ".cpp");
            addFileHeader(
                mock_header,
                mockName + ".h",
                "This file implements the mock for the " + file.Split('.')[0],
                System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                "1.0",
                DateTime.UtcNow.Date.ToString()
            );
            mock_header.WriteLine("#ifndef " + mockName.ToUpper());
            mock_header.WriteLine("#define " + mockName.ToUpper());
            mock_header.WriteLine("#include \"gmock/gmock.h\"");
            mock_header.WriteLine("#include \""+Path.GetFileName(unit.m_fileName)+"\"");
            mock_header.WriteLine("class " + mockName + ": public " + file + "{");
            mock_header.WriteLine("private:");
            mock_header.WriteLine(" static " + mockName + " *mp_" + mockName + "Instance;");
            mock_header.WriteLine("public:");
            mock_header.WriteLine(" " + mockName + "();");
            mock_header.WriteLine(" virtual " + "~" + mockName + "();");
            foreach (FunctionalInterface function in unit.m_MockFunctionList)
            {
                string mockFunctionName = "mocked_" + function.m_FunctionName;

                if (function.m_argumentTypes.Count <= 10 && function.m_argumentTypes.Count > 0)
                {
                    if (function.m_IsVirtual == true)
                    {
                        mockFunctionName = function.m_FunctionName;
                    }
                    mock_header.Write(" MOCK_METHOD" + function.m_argumentTypes.Count + "(" + mockFunctionName + "," + function.m_ReturnType + "(");
                    function.args_string = "";
                    function.passed_args = "";
                    for (int i = 0; i < function.m_argumentTypes.Count; i++)
                    {


                        if (i == (function.m_argumentTypes.Count - 1))
                        {
                            mock_header.Write(function.m_argumentTypes[i]);
                            function.args_string = function.args_string + " arg" + i;
                            function.passed_args = function.passed_args + function.m_argumentTypes[i] + " arg" + i;
                        }
                        else
                        {
                            mock_header.Write(function.m_argumentTypes[i] + ",");
                            function.args_string = function.args_string + " arg" + i + ",";
                            function.passed_args = function.passed_args + function.m_argumentTypes[i] + " arg" + i + ",";
                        }
                    }
                    mock_header.Write("));");

                }
                else
                {
                    if (function.m_argumentTypes.Count == 0)
                    {
                        mock_header.Write(" MOCK_METHOD" + function.m_argumentTypes.Count + "(" + mockFunctionName + "," + function.m_ReturnType + "());");
                    }
                    else
                    {
                        mock_header.Write("//"+function.m_ReturnType + " " + function.m_FunctionName + " " + function.m_Signature + " Cannot be Mocked as Number of Arguments Exceeds 10");
                    }
                    
                }
                mock_header.Write("\n");
            }
            mock_header.WriteLine("  static " + mockName + " * instance ()");
            mock_header.WriteLine("  {");

            mock_header.WriteLine("    return mp_" + mockName + "Instance;\n");

            mock_header.WriteLine("  }");
            mock_header.WriteLine("};");
            mock_header.WriteLine("#endif");
            mock_header.Close();
            mock_source.WriteLine("#include \"" + mockName + ".h\"");
            mock_source.WriteLine(mockName + " *" + mockName + "::mp_" + mockName + "Instance = 0;");
            mock_source.WriteLine(mockName + "::" + mockName + "()");
            mock_source.WriteLine("{\n  mp_" + mockName + "Instance = this;\n}");
            mock_source.WriteLine(" "+mockName + "::~" + mockName + "()");
            mock_source.WriteLine("{\n  mp_" + mockName + "Instance = 0;\n}");
            foreach (FunctionalInterface function in unit.m_MockFunctionList)
            {
                if (function.m_IsVirtual == false)
                {
                    mock_source.WriteLine(returnFunctionHeader(function.m_FunctionName));
                    mock_source.WriteLine(function.m_ReturnType + " " + function.m_FunctionName + "(" + function.passed_args + ")");
                    mock_source.WriteLine("{");
                    if (function.m_ReturnType != "void")
                    {
                        mock_source.WriteLine("   " + function.m_ReturnType + " l_returnValue;");
                        mock_source.WriteLine("  if(" + mockName + "::" + "instance() != 0)");
                        mock_source.WriteLine("  {");
                        mock_source.WriteLine("    l_returnValue =  " + mockName + "::" + "instance()->mocked_" + function.m_FunctionName + "(" + function.args_string + ");");
                        mock_source.WriteLine("  }");
                        mock_source.WriteLine("  return l_returnValue;");
                    }
                    else
                    {
                        mock_source.WriteLine("  if(" + mockName + "::" + "instance() != 0)");
                        mock_source.WriteLine("  {");
                        mock_source.WriteLine("    " + mockName + "::" + "instance()->mocked_" + function.m_FunctionName + "(" + function.args_string + ");");
                        mock_source.WriteLine("  }");
                    }


                    mock_source.WriteLine("}");
                }
            }
            mock_source.Close();
        }
        public void MockGen_GenerateCMock(UnitInfo unit)
        {
            string mockName = "";
            string mockPath = Path.GetDirectoryName(m_parent.m_data.m_Project.m_ProjectPath) + "\\" + m_parent.m_data.m_Project .m_ProjectName+ "_Stubs";
            if (Directory.Exists(mockPath) == false)
            {
                Directory.CreateDirectory(mockPath);
            }
            string file = unit.m_className;
            if (file.Contains('.'))
            {
                mockName = "Mock_" + file.Split('.')[0];
            }
            StreamWriter mock_header = new StreamWriter(mockPath+"\\" + mockName + ".h");
            StreamWriter mock_source = new StreamWriter(mockPath+"\\" + mockName + ".cpp");
            addFileHeader(
                mock_header,
                mockName + ".h",
                "This file implements the mock for the " + file.Split('.')[0],
                System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                "1.0",
                DateTime.UtcNow.Date.ToString()
            );
            mock_header.WriteLine("#ifndef " + mockName.ToUpper());
            mock_header.WriteLine("#define " + mockName.ToUpper());
            mock_header.WriteLine("#include \"gmock/gmock.h\"");
            mock_header.WriteLine("class " + mockName + "{");
            mock_header.WriteLine("private:");
            mock_header.WriteLine(" static " + mockName + " *mp_" + mockName + "Instance;");
            mock_header.WriteLine("public:");
            mock_header.WriteLine(" " + mockName + "();");
            mock_header.WriteLine(" " + "~" + mockName + "();");
            foreach (FunctionalInterface function in unit.m_MockFunctionList)
            {
                if (function.m_argumentTypes.Count <= 10 && function.m_argumentTypes.Count > 0)
                {
                    mock_header.Write(" MOCK_METHOD" + function.m_argumentTypes.Count + "(mocked_" + function.m_FunctionName + "," + function.m_ReturnType + "(");
                    function.args_string = "";
                    function.passed_args = "";
                    for (int i = 0; i < function.m_argumentTypes.Count; i++)
                    {


                        if (i == (function.m_argumentTypes.Count - 1))
                        {
                            mock_header.Write(function.m_argumentTypes[i]);
                            function.args_string = function.args_string + " arg" + i;
                            function.passed_args = function.passed_args + function.m_argumentTypes[i] + " arg" + i;
                        }
                        else
                        {
                            mock_header.Write(function.m_argumentTypes[i] + ",");
                            function.args_string = function.args_string + " arg" + i + ",";
                            function.passed_args = function.passed_args + function.m_argumentTypes[i] + " arg" + i + ",";
                        }
                    }
                    mock_header.Write("));");
                }
                else
                {
                    if (function.m_argumentTypes.Count == 0)
                    {
                        mock_header.Write(" MOCK_METHOD" + function.m_argumentTypes.Count + "(mocked_" + function.m_FunctionName + "," + function.m_ReturnType + "());");
                    }
                    else
                    {
                        MessageBox.Show(function.m_ReturnType + " " + function.m_FunctionName + " " + function.m_Signature + "\nCannot be Mocked as Number of Arguments Exceeds 10");
                    }
                    
                }
                mock_header.Write("\n");
            }
            mock_header.WriteLine("  static " + mockName + " * instance ()");
            mock_header.WriteLine("  {");
          
            mock_header.WriteLine("    return mp_" + mockName + "Instance;\n");
          
            mock_header.WriteLine("  }");
            mock_header.WriteLine("};");
            mock_header.WriteLine("#endif");
            
            mock_header.Close();
            addFileHeader(
                mock_source,
                mockName + ".cpp",
                "This file implements the mock for the " + file.Split('.')[0],
                System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                "1.0",
                DateTime.UtcNow.Date.ToString()
            );
            mock_source.WriteLine("#include \"" + mockName + ".h\"");
            mock_source.WriteLine(mockName + " *" + mockName + "::mp_" + mockName + "Instance = 0;");
            mock_source.WriteLine(mockName + "::" + mockName + "()");
            mock_source.WriteLine("{\n  mp_" + mockName + "Instance = this;\n}");
            mock_source.WriteLine(mockName + "::~" + mockName + "()");
            mock_source.WriteLine("{\n  mp_" + mockName + "Instance = 0;\n}");
            foreach (FunctionalInterface function in unit.m_MockFunctionList)
            {
                mock_source.WriteLine(returnFunctionHeader(function.m_FunctionName));
                mock_source.WriteLine(function.m_ReturnType + " " + function.m_FunctionName + "(" + function.passed_args + ")");
                mock_source.WriteLine("{");
                if (function.m_ReturnType != "void")
                {
                    mock_source.WriteLine("   " + function.m_ReturnType+" l_returnValue;");
                    mock_source.WriteLine("  if(" + mockName + "::" + "instance() != 0)");
                    mock_source.WriteLine("  {");
                    mock_source.WriteLine("    l_returnValue =  " + mockName + "::" + "instance()->mocked_" + function.m_FunctionName + "(" + function.args_string + ");");
                    mock_source.WriteLine("  }");
                    mock_source.WriteLine("  return l_returnValue;");
                }
                else
                {
                    mock_source.WriteLine("  if(" + mockName + "::" + "instance() != 0)");
                    mock_source.WriteLine("  {");
                    mock_source.WriteLine("    "+ mockName + "::" + "instance()->mocked_" + function.m_FunctionName + "(" + function.args_string + ");");
                    mock_source.WriteLine("  }");
                }
               
                
                mock_source.WriteLine("}");

            }
            mock_source.Close();


        }
        private string returnFunctionHeader(string functionName)
        {
            string functionHeader = "";
            functionHeader += "/*********************************************************************/\n";
            functionHeader += "/*! \\fn " + functionName + "\n";
            functionHeader += "* \\brief Mock for the function " + functionName + "()\n";
            functionHeader += "*/\n";
            functionHeader += "/*********************************************************************/\n";
            return functionHeader;
        }
        /*********************************************************************/
        /*! \fn addFileHeader
        * \brief To add the File header in the generated class
        * \return void
        */
        /*********************************************************************/
        private void addFileHeader(StreamWriter writer, string filename, string desc, string author, string version, string date)
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

    }
}
