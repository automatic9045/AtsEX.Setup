using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AtsEx.Setup.Converters
{
    internal class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string parameterString)) return DependencyProperty.UnsetValue;
            if (!Enum.IsDefined(value.GetType(), value)) return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString);
            return value.Equals(parameterValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string parameterString)) return DependencyProperty.UnsetValue;
            if (!value.Equals(true)) return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(targetType, parameterString);
            return parameterValue;
        }
    }
}
