using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gunit.DataModel;
using GUnit_IDE2010.DataModel;
using GUnit_IDE2010.JobHandler;
using Gunit.Ui;
using System.IO;
using System.Diagnostics;
using System.Threading;
namespace GUnit_IDE2010.JobHandler
{
    public class ProjectBuilder: ExtProcessHandlerWithConsole
    {
       
        
        ProjectDataModel m_ProjectModel;
        string m_gccpath = "g++";
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prjModel">ProjectDataModel</param>
        /// <param name="consolemodel">Console Data Model</param>
        public ProjectBuilder(ProjectDataModel prjModel, ConsoleDataModel consolemodel):base(consolemodel)
        {
            m_ProjectModel = prjModel;
          
            if (Directory.Exists(m_ProjectModel.CompilorPath))
            {
                m_gccpath = m_ProjectModel.CompilorPath + "\\g++.exe";
            }
           
            
        }
        /// <summary>
        /// Generate Job Objects
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public override Job JobFactory(string command, uint Id)
        {
            Job job =  base.JobFactory(command, Id);
            job.JobKind = JobKind.BuildJob;
            job.Command = m_gccpath;
            job.Argument = command;
            if (Directory.Exists(m_ProjectModel.BuildPath))
            {
                job.WorkingDirectory = m_ProjectModel.BuildPath;
            }
            return job;
        }


   
       
    
    
     

     
    }
}
