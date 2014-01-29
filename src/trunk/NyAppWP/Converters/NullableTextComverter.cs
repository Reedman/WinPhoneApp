using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NyAppWP.Converters
{
    /// <summary>
    /// 空的字符串转换器，当文字为空时，显示无
    /// </summary>
    public class NullableTextComverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = String.Empty;
            if(value is string)
            {
                string text = (string)value;
                result = String.IsNullOrEmpty(text) ? "无" : text;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
