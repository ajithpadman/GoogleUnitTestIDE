using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;

namespace Gunit.Utils
{
    public class ListOfConsoleData : ObservableCollection<string>
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

            Application.Current.Dispatcher.BeginInvoke(new Action(() => l_list.Add(listElement)));
            //l_list.Add(listElement);
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

            Application.Current.Dispatcher.BeginInvoke(new Action(() => l_list.Remove(listElement)));
           
            return l_list;

        }



    }
}
