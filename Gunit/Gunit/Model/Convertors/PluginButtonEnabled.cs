using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Gunit.Interfaces;
namespace Gunit.Model.Convertors
{
    public class PluginButtonEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ProjectState)
            {
                ProjectState state = (ProjectState)value ;
                if (state == ProjectState.OPEN || state == ProjectState.NEW)
                {
                    return true;
                
                }


            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
