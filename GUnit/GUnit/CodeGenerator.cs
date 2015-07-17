using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace GUnit
{
    public class CodeGenerator
    {
        
        MockGenerator m_MockGen;
        
        GUnit m_parent;
        public CodeGenerator(GUnit parent)
        {
            m_parent = parent;
          
            
        }
        public void CodeGen_generateUnit(string fileName)
        {
           

        }
      
       
        /*********************************************************************/
        /*! \fn addFileHeader
        * \brief To add the File header in the generated class
        * \return void
        */
        /*********************************************************************/
        public static void addFileHeader(StreamWriter writer, string filename, string desc, string author, string version, string date)
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
