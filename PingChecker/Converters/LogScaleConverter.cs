using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PingChecker.Converters
{
    [ValueConversion(typeof(int), typeof(int))]
    public class LogScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return (int)Math.Pow((double)parameter, System.Convert.ToDouble(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return (int)Math.Pow((double)parameter, 1/System.Convert.ToDouble(value));
        }
    }
}
