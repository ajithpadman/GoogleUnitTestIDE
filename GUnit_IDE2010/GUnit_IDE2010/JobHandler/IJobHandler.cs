using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace GUnit_IDE2010.JobHandler
{
    public enum JobPriority
    {
        Normal,
        Medium,
        High
    }
    public interface IJobHandler
    {

        void AddJob(Job job);
        
        void Start(string name,ThreadPriority priority);
        JobStatus GetCurrentJobStatus();
        void cancelAllJobs();
        void cancelJobs(JobKind kind);
       

    }
}
