using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace Gunit.DataModel
{

    /// <summary>
    /// List Containing the FileNames.
    /// </summary>
    public class ListofFiles : List<string>
    {

        /// <summary>
        /// Overloaded operator to add new FileName to the List
        /// Only Existing file Name is added
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListofFiles operator +(ListofFiles l_list, string listElement)
        {
            if (File.Exists(listElement))
            {
                if (l_list.Contains(listElement) == false)
                {
                    l_list.Add(listElement);
                }
            }
            return l_list;

        }
        /// <summary>
        /// Overloaded operator - to remove a file name from the list
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListofFiles operator -(ListofFiles l_list, string listElement)
        {

            if (l_list.Contains(listElement) == true)
            {
                l_list.Remove(listElement);
            }

            return l_list;

        }
        
    }
}