using System;
using System.Globalization;
using System.Windows.Data;

namespace NyAppWP.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime))
            {
                throw new ArgumentException("The input datetime value is an invaild argument!");
            }
            string result;
            DateTime datetime = (DateTime)value;
            result = datetime.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
