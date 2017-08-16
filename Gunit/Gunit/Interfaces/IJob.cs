using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Gunit.Interfaces
{
    public enum JobStatus
    {
        NOT_STARTED,
        RUNNING,
        COMPLETE
    }
    public interface IJob
    {
         DataReceivedEventHandler StdErrCallBack
        {
            get;
            set;
        }
         DataReceivedEventHandler StdOutCallBack
        {
            get;
            set;
        }

         UInt32 ID
        {
            get;
            set;
        }
         object Command
        {
            get;
            set;
        }
         object Argument
        {
            get;
            set;
        }
         object Result
        {
            get;
            set;
        }
         string WorkingDirectory
        {
            get;
            set;
        }
         JobStatus Status
        {
            get;
            set;
        }
    }
}
