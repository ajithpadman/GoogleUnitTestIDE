using GUnit_IDE2010.DataModel;
using GUnit_IDE2010.JobHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GUnit_IDE2010.CodeGenerator
{
    public class CodeGenBase :ExternalProcesshandler
    {
        protected CodeGenDataModel m_model;
       
        public CodeGenBase(CodeGenDataModel model)
        {
            m_model = model;
        }
        public virtual string GenerateCode()
        {
            StringWriter writer = new StringWriter();
            writer.Write(writeFileHeader());
            writer.WriteLine(WriteCodeGuard());
            writer.WriteLine("/*Header Files*/");
            writer.Write(WriteIncludes());
            writer.WriteLine("/*Namespace References*/");
            writer.Write(WriteUsingNameSpace());
            writer.WriteLine(WriteUsing());
            writer.WriteLine(WriteCodeBody());
            writer.WriteLine(WriteCodeGuardEnd());
           
            return writer.ToString();

        }
        protected virtual void StyleCode()
        {
            if (File.Exists("AStyle.exe"))
            {
                Job job = new Job();
                job.Command = "AStyle.exe";
                job.Argument = "--style=gnu --indent-classes --mode=c " + m_model.FilePath;

                RunExternalProcess(job);
            }
        }
        protected virtual string WriteCodeGuard()
        {
            StringWriter writer = new StringWriter();
            if (File.Exists(m_model.FilePath))
            {
                string Guard = Path.GetFileNameWithoutExtension(m_model.FilePath).ToUpper();
                writer.WriteLine("#ifndef " + Guard);
                writer.WriteLine("#define " + Guard);
            }
            return writer.ToString();
        }
        protected virtual string WriteCodeGuardEnd()
        {
            StringWriter writer = new StringWriter();
            if (File.Exists(m_model.FilePath))
            {
                string Guard = Path.GetFileNameWithoutExtension(m_model.FilePath).ToUpper();
                writer.WriteLine("#endif " + Guard);
                
            }
            return writer.ToString();
        }
        protected virtual string  WriteCodeBody()
        {
            StringWriter writer = new StringWriter();
            return writer.ToString();
        }
        protected virtual string  WriteIncludes()
        {
            StringWriter writer = new StringWriter();
            foreach (string include in m_model.Includes)
            {
                writer.WriteLine("#include \"" + include + "\"");
            }
            return writer.ToString();
        }
        protected virtual string WriteUsing()
        {
            StringWriter writer = new StringWriter();
            foreach (string l_using in m_model.Using)
            {
                writer.WriteLine("using " + l_using+";");
            }
            return writer.ToString();
        }
        protected virtual string WriteUsingNameSpace()
        {
            StringWriter writer = new StringWriter();
            foreach (string l_using in m_model.UsingNamespace)
            {
                writer.WriteLine("using namespace" + l_using + ";");
            }
            return writer.ToString();
        }
        protected virtual string  writeFileHeader()
        {
            StringWriter writer = new StringWriter();
            string filePath = "";
            if (File.Exists(m_model.FilePath))
            {
                 filePath = Path.GetFileName(m_model.FilePath);
               
            }
           
            writer.WriteLine("/*********************************************************************/");
            writer.WriteLine("/*! ");
            writer.WriteLine("* \\file " + filePath);
            writer.WriteLine("* " + m_model.Description);
            writer.WriteLine("* \\author " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            writer.WriteLine("* \\version 1.0");
            writer.WriteLine("* \\date " + DateTime.UtcNow.Date.ToString());
            writer.WriteLine("*/");
            writer.WriteLine("/*********************************************************************/");
            return writer.ToString();
        }
        protected virtual string writeFileHeader(string fileName)
        {
            StringWriter writer = new StringWriter();
            writer.WriteLine("/*********************************************************************/");
            writer.WriteLine("/*! ");
            writer.WriteLine("* \\file " + fileName);
            writer.WriteLine("* " + m_model.Description);
            writer.WriteLine("* \\author " + System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            writer.WriteLine("* \\version 1.0");
            writer.WriteLine("* \\date " + DateTime.UtcNow.Date.ToString());
            writer.WriteLine("*/");
            writer.WriteLine("/*********************************************************************/");
            return writer.ToString();
        }
        protected virtual string writeFunctionHeader(string functionName,string functionDescription = "")
        {
            StringWriter writer = new StringWriter();
         
         

            writer.WriteLine("/*********************************************************************/");
            writer.WriteLine("/*! ");
            writer.WriteLine("* \\fn " + functionName);
            writer.WriteLine("* " + functionDescription);
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
    }
}
