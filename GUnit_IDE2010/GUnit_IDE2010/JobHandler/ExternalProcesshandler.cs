using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Timers;
namespace GUnit_IDE2010.JobHandler
{
    public class ExternalProcesshandler : JobHandlerBase
    {

        public ExternalProcesshandler():base()
        {
           
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            KillProcess();
        }

        public override void cancelAllJobs()
        {
            KillProcess();
            base.cancelAllJobs();
        }
        public override void cancelJobs(JobKind kind)
        {
            if (m_currentJob.JobKind == kind)
            {
                KillProcess();
            }
            base.cancelJobs(kind);
        }
        public override void HandleJob(Job job)
        {
            
            RunExternalProcess(job);
          
        }
        /// <summary>
        /// Run an external Executable from the code
        /// </summary>
        /// <param name="PathtoExe">path to the external Executable</param>
        /// <param name="RunDir">Directory in which the Executable will be started</param>
        /// <param name="arg">Command line arguments to the Executable</param>
        /// <returns>if the Execution Was successfull</returns>
        protected  bool RunExternalProcess(Job job)
        {
            Process process = null;
            m_currentProcess = null;
            try
            {

                string prevDir = Directory.GetCurrentDirectory();
                if (Directory.Exists(job.WorkingDirectory))
                {
                    Directory.SetCurrentDirectory(job.WorkingDirectory);
                }
                
                
                if (job.Argument == null)
                {
                    process = new Process()

                    {
                        StartInfo = new ProcessStartInfo(job.Command as string)
                        {
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WorkingDirectory = job.WorkingDirectory

                        },
                        EnableRaisingEvents = true
                    };
                }
                else
                {
                    process = new Process()

                    {
                        StartInfo = new ProcessStartInfo(job.Command as string, job.Argument as string)
                        {
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            WorkingDirectory = job.WorkingDirectory
                        },
                        EnableRaisingEvents = true
                    };
                }



                // see below for output handler
                if (null != job.StdErrCallBack)
                {
                    process.ErrorDataReceived += job.StdErrCallBack;
                }
                if (null != job.StdOutCallBack)
                {
                    process.OutputDataReceived += job.StdOutCallBack;
                }
                process.Start();
                m_currentProcess = process;
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();
                Directory.SetCurrentDirectory(prevDir);
                return true;
            }
            catch 
            {

                return false;
            }
        }
        
         
        /// <summary>
        /// Kill the current active process
        /// </summary>
        /// <returns>Tru if Kill process was successful</returns>
        public  bool KillProcess()
        {
            try
            {
                if (null != m_currentProcess)
                {
                    if (m_currentProcess.HasExited == false)
                    {
                        m_currentProcess.Kill();

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
