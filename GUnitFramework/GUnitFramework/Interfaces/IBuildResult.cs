using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnitFramework.Interfaces
{
    public enum BuildStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
    public interface IBuildResult
    {
        List<string> Errors
        {
            get;
            set;
        }
        List<string> Warnings
        {
            get;
            set;
        }
        BuildStatus Status
        {
            get;
            set;
        }

    }
}
