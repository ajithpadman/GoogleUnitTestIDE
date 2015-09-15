using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUnit_IDE2010.CodeGenerator
{
    public class MethodParamFixtureBuilder
    {
        private ListofStrings m_ListOfParams = new ListofStrings();
        private ListofStrings m_ListOfActualParams = new ListofStrings();
        private ListofStrings m_membersWithBoundary = new ListofStrings();
        private List<int> m_IndexList = new List<int>();
        private string MethodName = "";
        private string m_workingDir = "";
      
        Methods m_method = null;
        private string[] m_arithmeticTypes =
        {
            "bool",
            "char",
            "char16_t",
            "char32_t",
            "wchar_t",
            "signed char",
            "short int",
            "int",
            "long int",
            "long long int",
            "unsigned char",
            "unsigned short int",
            "unsigned int",
            "unsigned long",
            "unsigned long int",
            "unsigned long long int",
            "float",
            "double",
            "long double",
            "unsigned short",
            "bool *",
            "char *",
            "char16_t *",
            "char32_t *",
            "wchar_t *",
            "signed char *",
            "short int *",
            "int *",
            "long int *",
            "long long int *",
            "unsigned char *",
            "unsigned short int *",
            "unsigned int *",
            "unsigned long *",
            "unsigned long int *",
            "unsigned long long int *",
            "float *",
            "double *",
            "long double *",
            "unsigned short *",
           };
        private string getReferenceType(string type)
        {
            if (type.Contains("&"))
            {
                return type.Replace('&', ' ').Trim();
            }
            else
            {
                return null;
            }
        }
        private void  fixTupleParam(Methods m)
        {
            string[] paramList = m.UnderlyingParamKind.Split(',');
            string[] ActualparamList = m.Parameters.Split(',');
            m_method = m;
            int count = 0;
            string refType  = "";
            foreach (string type in paramList)
            {
                if(IsLimitAvailable(type))
                {
                    m_ListOfParams += ActualparamList[count];
                    m_ListOfActualParams += ActualparamList[count];
                }
                else if((refType =getReferenceType(type))!= null)
                {
                    if (IsLimitAvailable(refType))
                    {
                        m_ListOfParams += refType;
                        m_ListOfActualParams += refType;
                    }
                }
                else
                m_ListOfActualParams += ActualparamList[count];
                count++;
            }
        }
        public MethodParamFixtureBuilder(Methods method,string workingDir)
        {
            MethodName = method.EntityName;
            m_workingDir = workingDir;
            if (Directory.Exists(workingDir) == false)
            {
                Directory.CreateDirectory(workingDir);
              
            }
         
            fixTupleParam(method);
        }
        public MethodParamFixtureBuilder(MemberMethods method, string workingDir)
        {
            MethodName = method.Methods.EntityName;
            m_workingDir = workingDir;
            if (Directory.Exists(workingDir) == false)
            {
                Directory.CreateDirectory(workingDir);

            }
            fixTupleParam(method.Methods);
        }
        
        private bool IsLimitAvailable(string type)
        {
            return m_arithmeticTypes.Contains(type);
        }
        private string getMin(string type)
        {
            return "std::numeric_limits<" + type + ">::min()";
          
        }
        private string getMax(string type)
        {
            return "std::numeric_limits<" + type + ">::max()";

        }
        private string buildLimits()
        {
            ListofStrings str = new ListofStrings();
            foreach (string type in m_ListOfParams)
            {
                string current = "";
                current = ("Values(");
                current += (getMin(type));
                current += (",");
                current += (getMax(type));
                current += (")");
                str += current;
            }
            return String.Join(",",str.ToArray());  
            
        }
        private string getCallString()
        {
            ListofStrings str = new ListofStrings();
            int i = 0;
            foreach (string type in m_ListOfActualParams)
            {
                if (m_ListOfParams.Contains(type))
                {
                    str +=( "m_member" + i);
                }
                else
                {
                    str += ("m_member" + i);
                }
               
                i++;
            }
            return String.Join(",", str.ToArray());
        }
        private string indent(string value)
        {
            return " " + value;
        }
        private string intend(int level,string value)
        {
            int i = level;
            string retVal = value;
            while (i!= 0)
            {
                retVal = indent(retVal);
                i--;
            }
            return retVal;
        }
        public void GenerateFixture(Classes l_class = null)
        {
            if (m_ListOfParams.Count() != 0)
            {
                string filename = MethodName + "_BoundaryFixture.cpp";
                string className = MethodName + "_BoundaryFixture";
                string MockclassName = MethodName + "_ParamsGenerator";
                string param = String.Join(",", m_ListOfParams.ToArray());

                StreamWriter writer = new StreamWriter(m_workingDir + "\\" + filename);
                ListofStrings includes = new ListofStrings();
                includes += "\"gtest/gtest.h\"";
                includes += "\"gmock/gmock.h\"";
                includes += "<tr1/tuple>";
                includes += "<limits>";
                ListofStrings usingStrings = new ListofStrings();
                usingStrings += "::testing::TestWithParam";
                usingStrings += "::testing::Values";
                usingStrings += "::testing::Combine";
                usingStrings += "::testing::Bool";
                usingStrings += "::testing::Range";
                usingStrings += "::testing::ExitedWithCode";

                MockGenerator.addFileHeader(
                  writer,
                  filename,
                  "This file implements Boundary Test Fixture for  " + MethodName,
                  System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                  "1.0",
                  DateTime.UtcNow.Date.ToString()
              );

                writer.WriteLine("/*Header Files*/");
                foreach (string header in includes)
                {
                    writer.WriteLine("#include " + header);
                }
                writer.WriteLine("/*Refereneces*/");
                foreach (string us in usingStrings)
                {
                    writer.WriteLine("using " + us + ";");
                }
                if (l_class == null)
                {
                    writer.WriteLine("extern " + m_method.ReturnType + " " + m_method.EntityName + "(" + m_method.Parameters + ");");
                }
                else
                {
                    writer.WriteLine("class " + l_class.EntityName+";");
                }
                MockGenerator.addClassHeader(
                    writer,
                    className,
                    "This class implements Boundary Test Fixture for  " + MethodName,
                    System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                    "1.0",
                    DateTime.UtcNow.Date.ToString()
                );
                if (m_ListOfParams.Count() > 1)
                {
                    writer.WriteLine("class " + className + ":public TestWithParam<std::tr1::tuple<" + param + "> >");
                }
                else
                {
                    writer.WriteLine("class " + className + ":public TestWithParam<" + param + " >");
                }
                writer.WriteLine("{");
                writer.WriteLine(intend(1, "public:"));
                int i = 0;
                int j = 0;
                foreach (string type in m_ListOfActualParams)
                {
                    writer.WriteLine(intend(1, type + " m_member" + i + ";"));
                    if (m_ListOfParams.Contains(type))
                    {
                        m_IndexList.Add(j);
                        m_membersWithBoundary += "m_member" + i + " = std::tr1::get<" + i + ">(GetParam());";
                        j++;
                    }
                    else
                    {
                        m_IndexList.Add(-1);
                    }
                        i++;
                }
                writer.WriteLine(intend(1, "virtual void SetUp()"));
                writer.WriteLine(intend(1, "{"));
                i = 0;
                foreach (string member in m_ListOfActualParams)
                {
                    if (m_IndexList[i] != -1)
                    {
                        string value = "m_member" + i + " = (GetParam());";
                        if (m_ListOfParams.Count() > 1)
                        {
                             value = "m_member" + i + " = std::tr1::get<" + m_IndexList[i] + ">(GetParam());";
                        }
                        
                        writer.WriteLine(intend(2, value));
                    }
                    i++;
                   
                }
                writer.WriteLine(intend(1, "}"));
                writer.WriteLine(intend(1, "virtual void TearDown()"));
                writer.WriteLine(intend(1, "{"));
                writer.WriteLine(intend(1, "}"));
                writer.WriteLine("};");
                writer.WriteLine("/*Instantiate Fixture*/");
                writer.WriteLine("INSTANTIATE_TEST_CASE_P");
                writer.WriteLine("(");
                writer.WriteLine(intend(1, MethodName + "_BoundaryTest_,"));
                writer.WriteLine(intend(1, className + ","));
                if (m_ListOfParams.Count() > 1)
                {
                    writer.Write(intend(1, "Combine"));
                    writer.Write(intend(2, "("));
                    writer.Write(intend(3, buildLimits()));
                    writer.Write(intend(2, ")"));
                }
                else
                {
                    writer.Write(intend(1, buildLimits()));
                }
                writer.WriteLine(");");
                writer.WriteLine(MockGenerator.returnFunctionHeader(
              MethodName + "_BoundaryTest","This test case implements the boundary test for the Function "+ MethodName
            ));
                writer.WriteLine("TEST_P(" + className + "," + MethodName + "_Test)");
                writer.WriteLine("{");
                if (l_class == null)
                {
                    writer.WriteLine(intend(1, "EXPECT_EXIT"));
                    writer.WriteLine(intend(2, "("));
                    writer.WriteLine(intend(3, MethodName + "("));
                    writer.WriteLine(intend(4, getCallString()));
                    writer.WriteLine(intend(3, ");exit(0),ExitedWithCode(0),\"\""));
                    writer.WriteLine(intend(2, ");"));
                }
                else
                {
                    writer.WriteLine(intend(1, "EXPECT_EXIT"));
                    writer.WriteLine(intend(2, "("));
                    writer.WriteLine(intend(3, l_class.EntityName + " l_object;"));
                    writer.WriteLine(intend(3, "l_object."+MethodName + "("));
                    writer.WriteLine(intend(4, getCallString()));
                    writer.WriteLine(intend(3, ");exit(0),ExitedWithCode(0),\"\""));
                    writer.WriteLine(intend(2, ");"));
                }
                
                writer.WriteLine("}");
                writer.Close();
            }
           
        }
        

    }
}
