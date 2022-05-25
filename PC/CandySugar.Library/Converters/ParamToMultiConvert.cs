using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace CandySugar.Library.Converters
{
    public class ParamToMultiConvert : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Dictionary<int, object> {
                { (int)values[0],values[1] }
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
