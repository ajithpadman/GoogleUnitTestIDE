using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Gunit.Interfaces;

namespace TestExecuter
{
    public class ResultViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string color = "yellow";
            if (value is TestStatus)
            {
               
                TestStatus status = (TestStatus)value;
                switch (status)
                {
                    case TestStatus.NotRun:
                        color= "yellow";
                        break;
                    case TestStatus.Error:
                        color = "red";
                        break;
                    case TestStatus.OK:
                        color = "green";
                        break;
                    default:
                        color = "yellow";
                        break;

                }
                
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
