using ClangSharp;
using Gunit.DataModel;
using Gunit.Ui;
using GUnit_IDE2010.JobHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GUnit_IDE2010.GunitParser
{
    
    public class ParserManager: TranslationUnitJobHandler,IDisposable
    {
        static Hashtable m_TUHashTable = new Hashtable();
        private ASTbuilderJobHandler m_AstBuilder = null;
        private OutlineDataModel m_OutlineModel = null;
        private int m_threashold = 0;
        private int m_JobCompleteCount = 0;
        public delegate void onParsingComplete(Job job);
        public event onParsingComplete evParseComplete = delegate { };
        private TreeNode m_resultTree = new TreeNode();

        public ParserManager(string  dbPath, OutlineDataModel outline)
        {
            m_AstBuilder = new ASTbuilderJobHandler(dbPath);
            m_OutlineModel = outline;

        }
       
        public Job ParserJobfactory(string filename, ListofStrings cmdLines)
        {
            Job job = new Job();
            job.Command = filename;
            job.Argument = cmdLines;
            job.Result = null;
            job.JobKind = JobKind.ParserJob;
            return job;
        }

        private void AddParserTriggerCount(int count)
        {
            m_threashold = count;
        }
        public void AddJobs(ListofFiles files, ListofStrings cmdLines)
        {
            AddParserTriggerCount(files.Count());
            foreach (string file in files)
            {
                AddJob(ParserJobfactory(file, cmdLines));

            }
        }
        public override void AddJob(Job job)
        {
           
            if (isParsingNeeded(job.Command as string) == false)
            {
                job.Result = m_TUHashTable[job.Command as string];
                m_AstBuilder.AddJob(ASTJobFactory(job));
            }
            else
            {
                base.AddJob(job);
            }
        }
        
        private bool isParsingNeeded(string fileName)
        {
            if (m_TUHashTable.ContainsKey(fileName))
            {
                return m_AstBuilder.isFileParsingNeeded(fileName);
                
            }
            else
            {
                return true;
            }
        }
        public override void Start(string threadName, ThreadPriority priority = ThreadPriority.Highest)
        {
            this.evJobStatus += TranslationUnit_evJobStatus;
            base.Start(threadName, priority);
            if(m_AstBuilder != null)
            {
                m_AstBuilder.evJobStatus += AstBuilder_evJobStatus;
                m_AstBuilder.Start(threadName + "_AST", priority);
            }
        }
        public virtual Job ASTJobFactory(Job job)
        {
            Job AstJob = new Job();
            AstJob.Command = job.Command;
            AstJob.Argument = job.Result;
            AstJob.JobKind = JobKind.ParserJob;
            return AstJob;
        }
        private void TranslationUnit_evJobStatus(Job job)
        {
            if(job != null)
            {
                lock(m_TUHashTable)
                {
                    m_TUHashTable[job.Command as string] = job.Result;
                }
                m_AstBuilder.AddJob(ASTJobFactory(job));
            }
        }

        private void AstBuilder_evJobStatus(Job job)
        {
            m_JobCompleteCount++;
            if (job != null)
            {
                if (job.Result is TreeNode)
                {
                    TreeNode node = job.Result as TreeNode;
                    ProjectFiles nodeFile = node.Tag as ProjectFiles;
                    m_resultTree.Nodes.Add(node);
                   
                }
            }
            if (m_JobCompleteCount >= m_threashold)
            {
                m_OutlineModel.Tree = m_resultTree.Clone() as TreeNode;
                evParseComplete(job);
                m_resultTree.Nodes.Clear();
                m_JobCompleteCount = 0;
               // Dispose();
            }
        }
        /// <summary>
        /// Dispose unmanaged Objects
        /// </summary>
        public new void  Dispose()
        {
            base.Dispose();
            List<TranslationUnit> listofUnits = new List<TranslationUnit>();
            if (null != m_AstBuilder)
            {
                m_AstBuilder.Dispose();
                m_AstBuilder = null;
            }
            foreach (TranslationUnit unit in m_TUHashTable.Values)
            {
                if (unit != null)
                {
                    listofUnits.Add(unit);
                }
            }
            m_TUHashTable.Clear();
            foreach (TranslationUnit unit in listofUnits)
            {
                unit.Dispose();
            }
           
        }
    }
}
