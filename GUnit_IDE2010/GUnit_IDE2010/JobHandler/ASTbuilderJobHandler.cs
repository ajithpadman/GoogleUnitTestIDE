using ClangSharp;
using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUnit_IDE2010.DataModel;
using System.Windows.Forms;
using System.IO;

namespace GUnit_IDE2010.JobHandler
{
    public class ASTbuilderJobHandler : JobHandlerBase,IDisposable
    {


        GunitParser.GUnitParser m_parser = null;
        string m_dbpath = null;
        string connectionString = "";
       
        public ASTbuilderJobHandler(string dataBase) : base()
        {
            m_parser = new GunitParser.GUnitParser(dataBase);
            m_dbpath = dataBase;
            connectionString = "Data Source = " + dataBase + ";File Mode=read write;";
        }
       
        public bool isFileParsingNeeded(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {

               GUnitDB database = new GUnitDB(connectionString);
                DateTime time = System.IO.File.GetLastWriteTime(fileName);
                IEnumerable<ProjectFiles> CurrentFileList = from file in database.ProjectFiles
                                                            where file.FilePath == fileName && file.LastModifiedTime == time
                                                            select file;


                if (CurrentFileList.Count() == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        public override void HandleJob(Job job)
        {
            string fileName = job.Command as string;
            TranslationUnit unit = job.Argument as TranslationUnit;
            GUnitDB database =  new GUnitDB(connectionString);


            if (System.IO.File.Exists(fileName))
            {
                try {
                    var projectFiles = from p in database.ProjectFiles
                                       where p.FilePath == fileName
                                       select p;
                    var methods = from p in database.Methods
                                  where p.FilePath == fileName
                                  select p;
                    var variables = from p in database.Variables
                                    where p.FilePath == fileName
                                    select p;
                    database.ProjectFiles.DeleteAllOnSubmit(projectFiles);
                    database.Methods.DeleteAllOnSubmit(methods);
                    database.Variables.DeleteAllOnSubmit(variables);
                    database.SubmitChanges();
                }
                catch
                {

                }
                try
                {
                    TreeNode node = new TreeNode(Path.GetFileName(fileName));
                    ProjectFiles file = new ProjectFiles();
                    file.FilePath = fileName;
                    file.LastModifiedTime = System.IO.File.GetLastWriteTime(fileName);
                    node.Tag = file;
                    job.Result = m_parser.CreateAST(unit, fileName, node, file);
                    database.ProjectFiles.InsertOnSubmit(file);
                    database.SubmitChanges();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
        }

        public new void Dispose()
        {
            base.Dispose();
            if (null != m_parser)
            {
                m_parser.Dispose();
                m_parser = null;
            }
        }
    }


}

