using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gunit.Utils
{
    public class CodeTemplates
    {
        public static string writeFileHeader(string fileName, string description)
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
                functionHeader += "* \\brief  the function " + functionName + "()\n";
            }
            functionHeader += "*/\n";
            functionHeader += "/*********************************************************************/\n";
            return functionHeader;
        }
        public static string writeModuleHeader(string moduleNameTag, string description = "")
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
    }
}
