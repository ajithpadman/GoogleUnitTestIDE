using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GUnitFramework.Implementation
{
    public class PropertyNotifier : INotifyPropertyChanged
    {
        protected event PropertyChangedEventHandler m_propertyChanged = delegate { };
        /// <summary>
        /// Event for PropertChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { m_propertyChanged += value; }
            remove { m_propertyChanged -= value; }
        }
        /// <summary>
        /// Fire the Property Change event
        /// </summary>
        /// <param name="propertyName">Name of the Proerty Changed</param>
        protected void FirePropertyChange(string propertyName)
        {
            if (null != m_propertyChanged)
            {
                m_propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
