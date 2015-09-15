using Gunit.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUnit_IDE2010.JobHandler
{
    public class ExtProcessHandlerWithConsole:ExternalProcesshandler
    {
        
        protected ConsoleDataModel m_consoleModel;
        protected int m_maxJobs = 0;
        protected int m_completedJobs = 0;
        public delegate void onJobProgress(int progress);
        public event onJobProgress evProgress = delegate { };
        public delegate void onJobMax(int max);
        public event onJobMax evMaxJobs = delegate { };
        public delegate void onAllJobsComplete();
        public event onAllJobsComplete evAllJobsComplete = delegate { };
        ConsoleManager m_consoleManager = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consoleModel"></param>
        public ExtProcessHandlerWithConsole(ConsoleDataModel consoleModel):base()
        {
            m_consoleModel = consoleModel;
            m_consoleManager = new ConsoleManager(consoleModel);
            this.evJobStatus += this.JobCompleteCallBack;
        }
        public override void Start(string threadName, ThreadPriority priority = ThreadPriority.Highest)
        {
            base.Start(threadName, priority);
            m_consoleManager.Start(threadName + "_Console", ThreadPriority.Normal);

        }
        public override void Stop()
        {
            base.Stop();
            m_consoleManager.Stop();
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual Job JobFactory(string command, UInt32 Id)
        {
            Job job = new Job();
            job.ID = Id;
            job.Command = command;
            job.Argument = "";
            job.Status = JobStatus.NOT_STARTED;
            job.JobKind = JobKind.OtherJob;
            job.WorkingDirectory = "";
            job.StdErrCallBack = error_CallBack;
            job.StdOutCallBack = Output_CallBack;
            return job;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual Job ConsoleJobFactory(DataReceivedEventArgs e)
        {
            Job job = null;
            if (e != null)
            {
                if (e.Data != null)
                {
                    job = new Job();
                    job.Command = e.Data.ToString();
                    job.Argument = "";
                    job.Status = JobStatus.NOT_STARTED;
                    job.JobKind = JobKind.ConsoleWrite;
                    job.WorkingDirectory = "";
                    job.StdErrCallBack = null;
                    job.StdOutCallBack = null;
                }
            }
            return job;
        }
        protected void SendMaxJobs(int value)
        {
            evMaxJobs(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        public virtual void RunJobs(ListofStrings commands)
        {
            m_maxJobs = commands.Count();
            SendMaxJobs(m_maxJobs);
            m_completedJobs = 0;
            UInt32 count = 0;
            foreach (string command in commands)
            {
                AddJob(JobFactory(command, count));
                count++;
            }
        }
        public virtual void RunJobs(List<Job> commands)
        {
            m_maxJobs = commands.Count();
            evMaxJobs(m_maxJobs);
            m_completedJobs = 0;
           
            foreach (Job command in commands)
            {
                AddJob(command);
                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        protected virtual void JobCompleteCallBack(Job job)
        {
            if(m_completedJobs < m_maxJobs)
            {
                m_completedJobs++;
            }
            evProgress(m_completedJobs);
            if (m_maxJobs == m_completedJobs)
            {
                evAllJobsComplete();
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void error_CallBack(object sender, DataReceivedEventArgs e)
        {
            m_consoleManager.AddJob(ConsoleJobFactory(e));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Output_CallBack(object sender, DataReceivedEventArgs e)
        {
            m_consoleManager.AddJob(ConsoleJobFactory(e));
        }

    }
}
