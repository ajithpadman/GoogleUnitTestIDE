using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASTBuilder;
using CPPASTBuilder.Interfaces;
using CPPASTBuilder.Implementation;
using System.IO;
namespace ASTTester
{
    class Program
    {
        static void Main(string[] args)
        {
            ASTBuilder.ASTBuilder builder = new ASTBuilder.ASTBuilder("Test.c");
            builder.ParseFile();
            //ICppCodeDescription codeDescription = null;
            //ClangSettings settings = new ClangSettings();
            //if (File.Exists("Test.cpp"))
            //{
            //   CPPASTBuilder.CPPASTBuilder parser = new CPPASTBuilder.CPPASTBuilder(settings);
            //   codeDescription = parser.ParseFile("Test.cpp");
            //   Console.WriteLine("Done");
            //}
        }
    }
}
