using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gunit.Interfaces
{
    public delegate void onProcessComplete();
    public delegate void onProcessProgress(int index);
    public delegate void onProcesslog(string log);

    public interface IProcessHandler
    {
        event onProcessComplete evProcessComplete;
        event onProcessProgress evProgress;
        event onProcesslog evLog;
        List<IJob> JobList
        {
            get;
            set;
        }
         IJob CurrentJob
        {
            get;
            set;
        }
         void Start();
        bool RunExternalProcess(IJob job);
    }
}
