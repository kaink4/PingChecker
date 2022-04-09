﻿using PingChecker.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PingChecker.Converters
{
    [ValueConversion(typeof(AlarmMode), typeof(bool))]
    public class AlarmModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (AlarmMode)parameter == (AlarmMode)value; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? (AlarmMode)parameter : Binding.DoNothing;
        }
    }
}
