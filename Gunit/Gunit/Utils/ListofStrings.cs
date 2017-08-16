using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gunit.Utils
{
    public class ListofStrings : List<string>
    {
        /// <summary>
        /// Overloaded operator to add new string to the List
        /// 
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListofStrings operator +(ListofStrings l_list, string listElement)
        {



            l_list.Add(listElement);

            return l_list;

        }
        /// <summary>
        /// Overloaded operator - to remove a string name from the list
        /// </summary>
        /// <param name="l_list"></param>
        /// <param name="listElement"></param>
        /// <returns>ListofFiles</returns>
        public static ListofStrings operator -(ListofStrings l_list, string listElement)
        {

            if (l_list.Contains(listElement) == true)
            {
                l_list.Remove(listElement);
            }

            return l_list;

        }
    }
}
