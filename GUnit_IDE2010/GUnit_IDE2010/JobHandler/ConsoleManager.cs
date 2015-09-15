using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using Gunit.Ui;
using Gunit.DataModel;
namespace GUnit_IDE2010.JobHandler
{
    public class ConsoleManager:JobHandlerBase
    {
        ConsoleDataModel m_ConsoleModel = null;
        public ConsoleManager(ConsoleDataModel consoleModel)
        {
            m_ConsoleModel = consoleModel;
        }
        public override void HandleJob(Job job)
        {
            if (m_ConsoleModel != null)
            {
                m_ConsoleModel.Console_addBuildOutput(job.Command as string);
            }
        }
        public override void Stop()
        {
            m_ConsoleModel.Console_ClearBuildOutput();
            base.Stop();
        }
    }
}
