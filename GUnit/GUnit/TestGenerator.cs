using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace GUnit
{
    public class TestGenerator
    {
        CodeGenerator m_parent;
        StreamWriter writer;
        public TestGenerator(CodeGenerator parent)
        {
            m_parent = parent;
        }
        public void TestGen_GenerateFixture(UnitInfo unit)
        {
            generateCode(unit.m_fileName, unit.m_className);
        }
        public void generateCode(string filename, string className)
        {


            writer = new StreamWriter(filename);
            CodeGenerator.addFileHeader(
               writer,
               className + ".cpp",
               "This file implements the Unittest Fixture for the  Test Case",
               System.Security.Principal.WindowsIdentity.GetCurrent().Name,
               "1.0",
               DateTime.UtcNow.Date.ToString()
           );
            writer.WriteLine("#include \"gmock/gmock.h\"");
            writer.WriteLine("#include \"gtest/gtest.h\"");
            writer.WriteLine("#include \"" + filename + "\"");
            writer.WriteLine("using namespace testing ;");
            writer.WriteLine("// The fixture for testing " + filename);
            writer.WriteLine("class " + className + ": public ::testing::Test");
            writer.WriteLine("{");
            writer.WriteLine("// You can remove any or all of the following functions if its body is empty.");
            writer.WriteLine(" " + "protected:");
            writer.WriteLine(" " + " " + className + "()");
            writer.WriteLine(" " + " " + "{");
            writer.WriteLine(" " + " " + " " + "// You can do set-up work for each test here.");
            writer.WriteLine(" " + " " + "}");
            writer.WriteLine(" " + " " + "virtual ~" + className + "()");
            writer.WriteLine(" " + " " + "{");
            writer.WriteLine(" " + " " + " " + "// You can do clean-up work that doesn't throw exceptions here.");
            writer.WriteLine(" " + " " + "}");
            writer.WriteLine(" " + "// If the constructor and destructor are not enough for setting up");
            writer.WriteLine(" " + "// and cleaning up each test, you can define the following methods:");
            writer.WriteLine(" " + " " + "virtual void SetUp()");
            writer.WriteLine(" " + " " + "{");
            writer.WriteLine(" " + " " + " " + "// Code here will be called immediately after the constructor (right before each test)");
            writer.WriteLine(" " + " " + "}");
            writer.WriteLine(" " + " " + "virtual void TearDown()");
            writer.WriteLine(" " + " " + "{");
            writer.WriteLine(" " + " " + " " + "// Code here will be called immediately after each test(right before the destructor).");
            writer.WriteLine(" " + " " + "}");
            writer.WriteLine("};");
            writer.Close();



        }
    }
}
