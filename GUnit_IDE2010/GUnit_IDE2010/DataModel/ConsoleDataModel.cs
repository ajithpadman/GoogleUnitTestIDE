using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Gunit.DataModel
{
    public enum ConsoleMode
    {
        WARNING,
        ERROR,
        EXCEPTION,
        NORMAL
    }
    
    public class ListOfConsoleData : List<string>
    {
        /// <summary>
        /// Overloaded operator to add new FileName to the List
        /// Only Existing file Name is added
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListOfConsoleData operator +(ListOfConsoleData l_list, string listElement)
        {
              
            l_list.Add(listElement);
            return l_list;

        }
        /// <summary>
        /// Overloaded operator - to remove a file name from the list
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListOfConsoleData operator -(ListOfConsoleData l_list, string listElement)
        {

            l_list.Remove(listElement);
            return l_list;

        }
      

        
    }
    /// <summary>
    /// DataModel for the Console
    /// </summary>
    public class ConsoleDataModel:DataModelBase
    {
        #region members
        
        public readonly static object _lock = new object();
        private ConsoleMode m_mode = ConsoleMode.NORMAL;
        private ListOfConsoleData m_ConsloeLines = new ListOfConsoleData();
        private ListOfConsoleData m_WarningLines = new ListOfConsoleData();
        private ListOfConsoleData m_ErrorLines = new ListOfConsoleData();
        private ListOfConsoleData m_ExceptionLines = new ListOfConsoleData();
        #endregion
        /// <summary>
        /// Constructor
        /// </summary>
        public ConsoleDataModel():base()
        {
            Mode = ConsoleMode.NORMAL;
        }
        #region properties
        /// <summary>
        /// Property Mode indicating the current Display mode of a console
        /// </summary>
        public ConsoleMode Mode
        {
            get { return m_mode; }
            set
            {
                m_mode = value;
                FirePropertyChange("mode");
            }
        }
        /// <summary>
        /// Property ConsoleLines the lines displayed in Normal Mode
        /// </summary>
        public ListOfConsoleData ConsoleLines
        {
            get {
                //lock (_lock)
                {
                    return m_ConsloeLines;
                }
            }
            set {
                //lock (_lock)
                    {
                        m_ConsloeLines = value;
                        FirePropertyChange("ConsoleUpdate");
                    }
                }
        
        }
        /// <summary>
        /// Property Warnings the lines displayed in Warning Mode
        /// </summary>
        public ListOfConsoleData Warnings
        {
            get {
                //lock (m_WarningLines)
                {
                    return m_WarningLines;
                }
            }
            set 
            {
                //lock (_lock)
                {
                    m_WarningLines = value;
                    FirePropertyChange("WarningsUpdate");
                }
            }
        }
        /// <summary>
        /// Property Errors the lines displayed in Error Mode
        /// </summary>
        public ListOfConsoleData Errors
        {
            get {
                //lock (m_ErrorLines)
                {
                    return m_ErrorLines;
                }
            }
            set
             {
                // lock (_lock)
                 {
                     m_ErrorLines = value;
                     FirePropertyChange("ErrorUpdate");
                 }
             }
        }
        /// <summary>
        /// Property Exceptions displaying exceptions List
        /// </summary>
        public ListOfConsoleData Exceptions
        {
            get { return m_ExceptionLines; }
            set
            {
                m_ExceptionLines = value;
                FirePropertyChange("Exception");
            }
        }
        #endregion
        #region methods
        public void Console_addBuildOutput(string l_BuildOutput)
        {
           // lock (m_ConsloeLines)
            {
                if (l_BuildOutput.Contains("warning:"))
                {
                    Warnings += l_BuildOutput;
                }
                else if (l_BuildOutput.Contains("error:"))
                {
                    Errors += l_BuildOutput;
                }
                ConsoleLines += l_BuildOutput;
            }
           
        }
        public void Console_ClearBuildOutput()
        {
            ConsoleLines.Clear();
            Errors.Clear();
            Warnings.Clear();

        }
        public override void Datamodel_closeProject()
        {
            base.Datamodel_closeProject();
            ConsoleLines.Clear();
            Errors.Clear();
            Warnings.Clear();
            Mode = ConsoleMode.NORMAL;

        }
        #endregion
    }
}
