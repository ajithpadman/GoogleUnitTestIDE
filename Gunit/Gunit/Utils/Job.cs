﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Gunit.Interfaces;
namespace Gunit.Utils
{

    public class Job : IJob
    {

        private UInt32 m_JobId = 0;
        private object m_Command = null;
        private string m_WorkingDir = "";
        private object m_Argument = null;
        private object m_Result = null;
      
        private DataReceivedEventHandler m_StdErrCallback = null;
        private DataReceivedEventHandler m_StdOutCallback = null;
        private JobStatus m_Status = JobStatus.NOT_STARTED;

        public DataReceivedEventHandler StdErrCallBack
        {
            get { return m_StdErrCallback; }
            set { m_StdErrCallback = value; }
        }
        public DataReceivedEventHandler StdOutCallBack
        {
            get { return m_StdOutCallback; }
            set { m_StdOutCallback = value; }
        }
       
        public UInt32 ID
        {
            get { return m_JobId; }
            set { m_JobId = value; }
        }
        public object Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }
        public object Argument
        {
            get { return m_Argument; }
            set { m_Argument = value; }
        }
        public object Result
        {
            get { return m_Result; }
            set { m_Result = value; }
        }
        public string WorkingDirectory
        {
            get { return m_WorkingDir; }
            set { m_WorkingDir = value; }
        }
        public JobStatus Status
        {
            get { return m_Status; }
            set { m_Status = value; }
        }
    }
}
