using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Gunit.DataModel
{
    public class ListOfDirectories:List<string>
    {
        /// <summary>
        /// Overloaded operator to add new Directory Name to the List
        /// Only Existing Directory Name is added
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListOfDirectories operator +(ListOfDirectories l_list, string listElement)
        {
            if (Directory.Exists(listElement))
            {
                if (l_list.Contains(listElement) == false)
                {
                    l_list.Add(listElement);
                }
            }
            return l_list;

        }
        /// <summary>
        /// Overloaded operator - to remove a Directory name from the list
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListOfDirectories operator -(ListOfDirectories l_list, string listElement)
        {

            if (l_list.Contains(listElement) == true)
            {
                l_list.Remove(listElement);
            }

            return l_list;

        }
    }
}
