using Gunit.DataModel;
using GUnit_IDE2010.DataModel;
using GUnit_IDE2010.GunitParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GUnit_IDE2010.CodeGenerator
{
    public class TestGenerator : BoundaryValueGenerator
    {

      
        private enum TestGenMode
        {
            GlobalMethod,
            MemberMethod
        }
        
        public TestGenerator(TestGeneratorModel model) : base(model)
        {

        }
        protected override string WriteCodeGuard()
        {
            return "";
        }
        protected override string WriteCodeGuardEnd()
        {
            return "";
        }
        protected override string WriteCodeBody()
        {
            StringWriter writer = new StringWriter();
            if (IsBoundaryTestAllowed(((TestGeneratorModel)(m_model)).Method))
            {
                if (((TestGeneratorModel)(m_model)).Method.Arguments.Count() > 0)
                {
                    TestGenMode mode = TestGenMode.GlobalMethod;
                    string MethodName = ((TestGeneratorModel)(m_model)).Method.EntityName;
                    string className = MethodName + "_BoundaryFixture";
                    string param = ((TestGeneratorModel)(m_model)).Method.Parameters;
                    
                    param = param.Replace("const ", "");
                    if (((TestGeneratorModel)(m_model)).Method.MemberMethods.Count() > 0)
                    {
                        mode = TestGenMode.MemberMethod;
                    }
                    if(mode == TestGenMode.GlobalMethod)
                    {
                        writer.WriteLine("extern " + ((TestGeneratorModel)(m_model)).Method.ReturnType + " " + MethodName + "(" + ((TestGeneratorModel)(m_model)).Method.Parameters + ");");
                    }
                    writer.WriteLine(writeModuleHeader("\\class " + className, "Boundary test Suit for method " + MethodName));
                    if (((TestGeneratorModel)(m_model)).Method.Arguments.Count() > 1)
                    {

                        writer.WriteLine("class " + className + ":public TestWithParam<std::tr1::tuple<" + param + "> >");
                    }
                    else
                    {
                        writer.WriteLine("class " + className + ":public TestWithParam<" + param + " >");
                    }
                    writer.WriteLine("{");
                    writer.WriteLine("public:");
                    int i = 0;
                   
                    writer.WriteLine(writeFunctionHeader("void SetUp()", "Set up for each tests " ));
                    writer.WriteLine("virtual void SetUp()");
                    writer.WriteLine("{");
                    writer.WriteLine("}");
                    writer.WriteLine(writeFunctionHeader("void TearDown()", "Tear Down each tests "));
                    writer.WriteLine("virtual void TearDown()");
                    writer.WriteLine("{");
                    writer.WriteLine("}");
                    writer.WriteLine("};");
                    i = 0;
                    foreach (Arguments args in ((TestGeneratorModel)(m_model)).Method.Arguments)
                    {
                        writer.WriteLine(writeFunctionHeader(args.DataType.EntityName + " getParamMax_" + i + "()", "Get the maximum value for Parameter " + i));
                        writer.WriteLine("static "+args.DataType.EntityName + " getParamMax_" + i + "()");
                        writer.WriteLine("{");
                        Member member = argumentMaxMember(args, i);
                        writer.WriteLine(member.member);
                        writer.WriteLine(member.value);
                        writer.WriteLine("return " + member.memberVariable + ";");
                        writer.WriteLine("}");
                        writer.WriteLine(writeFunctionHeader(args.DataType.EntityName + " getParamMin_" + i + "()", "Get the minimum value for Parameter " + i));
                        writer.WriteLine("static "+args.DataType.EntityName + " getParamMin_" + i + "()");
                        writer.WriteLine("{");
                        Member membermin = argumentMinMember(args, i);
                        writer.WriteLine(membermin.member);
                        writer.WriteLine(membermin.value);
                        writer.WriteLine("return " + membermin.memberVariable + ";");
                        writer.WriteLine("}");
                        i++;

                    }

                    writer.WriteLine(writeFunctionHeader("INSTANTIATE_TEST_CASE_P", "Instantiate the Test value Fixture "));
                    writer.WriteLine("INSTANTIATE_TEST_CASE_P");
                    writer.WriteLine("(");
                    writer.WriteLine( MethodName + "_BoundaryTest_,");
                    writer.WriteLine( className + ",");
                    if (((TestGeneratorModel)(m_model)).Method.Arguments.Count() > 1)
                    {

                        writer.WriteLine("Combine");
                        writer.WriteLine("(");
                        ListofStrings ranges = new ListofStrings();
                        for(i = 0; i < ((TestGeneratorModel)(m_model)).Method.Arguments.Count();i++)
                        {
                            ranges += "Values(getParamMin_" + i + "(),getParamMax_" + i + "())";
                        }
                        writer.WriteLine(String.Join(",", ranges.ToArray()));
                        writer.WriteLine(")");

                    }
                    else
                    {
                        writer.WriteLine("Values(getParamMin_0(),getParamMax_0())");
                       
                       
                        
                    }
                    writer.WriteLine(");");
                    writer.WriteLine(writeFunctionHeader("TEST", "Test case for Boundary testing Method "+MethodName));
                    writer.WriteLine("TEST_P(" + className + "," + MethodName + "_Test)");
                    writer.WriteLine("{");
                    
                    
                    writer.WriteLine( "EXPECT_EXIT");
                    writer.WriteLine( "(");
                    writer.WriteLine( MethodName + "(");
                    if (((TestGeneratorModel)(m_model)).Method.Arguments.Count() > 1)
                    {
                        ListofStrings parameter = new ListofStrings();
                        for (i = 0; i < ((TestGeneratorModel)(m_model)).Method.Arguments.Count(); i++)
                        {
                            parameter += "std::tr1::get < " + i + " > (GetParam())";
                        }
                        writer.WriteLine(String.Join(",", parameter.ToArray()));
                    }
                    else
                    {
                        writer.WriteLine("GetParam()");
                    }
                    
                    writer.WriteLine( ");exit(0),ExitedWithCode(0),\"\"");
                    writer.WriteLine( ");");
                    
                  
                    writer.WriteLine("}");
                    
                    
                }
            }
           

            return writer.ToString();
        }
        public override string GenerateCode()
        {
            if (IsBoundaryTestAllowed(((TestGeneratorModel)(m_model)).Method))
            {
                if (((TestGeneratorModel)(m_model)).Method.Arguments.Count() > 0)
                {
                    string MethodName = ((TestGeneratorModel)(m_model)).Method.EntityName;
                    string workingDir = ((TestGeneratorModel)(m_model)).WorkingDir;
                    StreamWriter fileWriter = new StreamWriter(workingDir + "\\" + MethodName + "_test.cpp");
                    fileWriter.WriteLine(base.GenerateCode());
                    fileWriter.Close();
                }
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private bool IsBoundaryTestAllowed(Methods m)
        {
            
            foreach(Arguments arg in m.Arguments)
            {
                if(arg.DataType.TypeKind == (int)DataTypeKind.OtherType || arg.DataType.IsConstQualified == true)
                {
                    return false;
                }
                else
                {
                    continue;
                }
            }
            return true; 

        }
        private Member argumentMinMember(Arguments arg, int index)
        {
            Member member = null;
            member = DatatypeMinMember(arg.DataType, "member_" + index);
            
            return member;
        }
        private Member argumentMaxMember(Arguments arg, int index)
        {
            Member member = null;
            member = DatatypeMaxMember(arg.DataType, "member_" + index);
            return member;
        }
        
       
       
    }
}
