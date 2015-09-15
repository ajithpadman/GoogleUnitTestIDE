using ClangSharp;
using Gunit.DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnit_IDE2010.JobHandler
{
    public class TranslationUnitJobHandler:JobHandlerBase,IDisposable
    {
        TranslationUnit m_unit = null;
        
        Index m_Index = null;
       
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataBase">DbContext Object</param>
        public TranslationUnitJobHandler() :base()
        {
           
            m_Index = new Index(true, true);
        }
     
        /// <summary>
        /// 
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
            if (m_Index != null)
            {
                m_Index.Dispose();
                m_Index = null;

            }
            if (m_unit != null)
            {
                m_unit.Dispose();
                m_unit = null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="job"></param>
        public override void HandleJob(Job job)
        {
            try
            {
                string fileName = job.Command as string;
                ListofStrings cmdLines = job.Argument as ListofStrings;
                TranslationUnit unit = TranslateFile(fileName, cmdLines);
                job.Result = unit;
            }
            catch
            {
                job.Result = null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="cmdLines"></param>
        private TranslationUnit TranslateFile(string fileName, ListofStrings cmdLines)
        {
            try
            {
                //lock (m_unit)
                {
                    m_unit = CreateTU(m_Index, fileName, cmdLines);
                }
                return m_unit;
                
            }
            catch
            {
                return null;
            }
            
        }
        private TranslationUnit CreateTU(Index l_index, string fileName, ListofStrings cmdLines)
        {
           
            try
            {
               return l_index.CreateTranslationUnit(fileName, cmdLines.ToArray(), null, TranslationUnitFlags.DetailedPreprocessingRecord);
            }
            catch 
            {
                return null;
            }
        }
    }
}
