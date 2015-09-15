using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUnit_IDE2010.CodeGenerator
{
    public class CodeWriter:StreamWriter
    {
        public CodeWriter(string path) : base(path)
        {
            
        }
        public void WriteCodeLine(int indentationLevel,string value)
        {
            string input = value;
            int i = indentationLevel;
            while (i!= 0)
            {
                input = " " + input;
                i--;
            }
            base.WriteLine(input);
        }
        
    }
}
