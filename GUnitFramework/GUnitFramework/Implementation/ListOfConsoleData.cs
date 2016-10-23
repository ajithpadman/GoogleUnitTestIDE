using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUnitFramework.Implementation
{
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
}
