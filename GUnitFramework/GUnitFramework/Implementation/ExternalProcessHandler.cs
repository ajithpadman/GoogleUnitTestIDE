using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GUnitFramework.Interfaces;

namespace GUnitFramework.Implementation
{

    public class ExternalProcessHandler : IProcessHandler
    {
        string m_command;
        string m_args;
        IJob m_job;
        int m_progress = 0;
        public event onProcessComplete evProcessComplete = delegate { };
        public event onProcessProgress evProgress = delegate { };
        BackgroundWorker m_worker;
        List<IJob> m_JobList = new List<IJob>();
        public List<IJob> JobList
        {
            get { return m_JobList; }
            set { m_JobList = value; }
        }

        public IJob CurrentJob
        {
            get { return m_job; }
            set { m_job = value; }
        }
       
       
        public ExternalProcessHandler(IJob job)
        {
            m_job = job;
        }
        public ExternalProcessHandler()
        {
        }
        public void Start()
        {
            m_progress = 0;
            m_worker = new BackgroundWorker();
            m_worker.DoWork += DoWork;
            m_worker.RunWorkerCompleted += RunWorkerCompleted;
            m_worker.RunWorkerAsync();

        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            evProcessComplete();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (IJob job in JobList)
            {
                m_progress++;
                CurrentJob = job;
                RunExternalProcess(job);
                evProgress(m_progress);
                
                
                
            }
        }

        private DataReceivedEventArgs CreateMockDataReceivedEventArgs(string TestData)
        {

            if (String.IsNullOrEmpty(TestData))
                throw new ArgumentException("Data is null or empty.", "Data");

            DataReceivedEventArgs MockEventArgs =
                (DataReceivedEventArgs)System.Runtime.Serialization.FormatterServices
                 .GetUninitializedObject(typeof(DataReceivedEventArgs));

            FieldInfo[] EventFields = typeof(DataReceivedEventArgs)
                .GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly);

            if (EventFields.Count() > 0)
            {
                EventFields[0].SetValue(MockEventArgs, TestData);
            }
            else
            {
                throw new ApplicationException(
                    "Failed to find _data field!");
            }

            return MockEventArgs;

        }
        public bool RunExternalProcess(IJob job)
        {
            Process process = null;

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

                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();
                Directory.SetCurrentDirectory(prevDir);
                return true;
            }
            catch (Exception err)
            {
                DataReceivedEventArgs arg = CreateMockDataReceivedEventArgs(err.ToString());
                job.StdErrCallBack(this, arg);
                return false;
            }
        }


        
    }
}
