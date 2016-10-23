using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnitFramework.Interfaces
{
    public delegate void onProcessComplete();
    public delegate void onProcessProgress(int index);

    public interface IProcessHandler
    {
        event onProcessComplete evProcessComplete;
        event onProcessProgress evProgress;
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
