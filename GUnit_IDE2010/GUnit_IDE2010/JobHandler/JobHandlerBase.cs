using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace GUnit_IDE2010.JobHandler
{
    public class JobHandlerBase:IJobHandler,IDisposable
    {
        protected Thread m_MainThread;
        
        protected Process m_currentProcess = null;
        protected Job m_currentJob = null;
        protected AutoResetEvent _waitHandle = new AutoResetEvent(false);
        protected Queue<Job> m_jobs = new Queue<Job>();
        public delegate void onJobStatusChange(Job job);
        public event onJobStatusChange evJobStatus = delegate { };
        public virtual void AddJob(Job job)
        {
           
                lock (m_jobs)
                {
                    m_jobs.Enqueue(job);
                    _waitHandle.Set();
                }
            
        }

        protected  void Run()
        {
            while (true)
            {
                _waitHandle.WaitOne();
               // lock (m_jobs)
                {
                    while (m_jobs.Count > 0 )
                    {

                        lock(m_jobs)
                        m_currentJob = m_jobs.Dequeue();
                       
                        if (m_currentJob != null)
                        {
                            m_currentJob.Status = JobStatus.RUNNING;
                            HandleJob(m_currentJob);
                            m_currentJob.Status = JobStatus.COMPLETE;
                            evJobStatus(m_currentJob);
                        }
                    }
                }
            }
        }
        public virtual void HandleJob(Job job)
        {

        }
        public virtual void Start(string threadName,ThreadPriority priority = ThreadPriority.Highest)
        {
            m_MainThread = new Thread(new ThreadStart(Run));
            m_MainThread.IsBackground = true;
            m_MainThread.Name = threadName;
            m_MainThread.Priority = priority;
            m_MainThread.Start();
        }
        public virtual void Stop()
        {
            m_MainThread.Abort();
        }
        public virtual bool IsJobPending()
        {
            if (GetCurrentJobStatus() != JobStatus.COMPLETE)
            {
                return true;
            }
            else if (m_jobs.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public virtual JobStatus GetCurrentJobStatus()
        {
            if (m_currentJob != null)
            {
                return m_currentJob.Status;
            }
            else
            {
                return JobStatus.COMPLETE;
            }
        }

        public virtual void cancelAllJobs()
        {
            //lock (m_jobs)
            {
                
                m_jobs.Clear();
            }
        }

        public virtual void cancelJobs(JobKind kind)
        {
            //lock (m_jobs)
            {
              
                m_jobs = new Queue<Job>(m_jobs.Where(p => p.JobKind != kind));
                

            }
        }

        public void Dispose()
        {
            if (m_MainThread != null)
            {
                if (m_MainThread.IsAlive)
                {
                    Stop();
                }
            }
            if (_waitHandle != null)
            {
                _waitHandle.Dispose();
                _waitHandle = null;
            }
         
        }
    }
}
