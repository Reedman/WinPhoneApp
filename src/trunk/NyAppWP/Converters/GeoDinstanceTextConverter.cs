using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NyAppWP.Converters
{
    /// <summary>
    /// 显示Geo的距离，如果GPS未开
    /// 显示距离时，表示成未获取
    /// 否则显示距离，以米为单位
    /// </summary>
    public class GeoDinstanceTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = String.Empty;
            if (value is int)
            {
                int distance = (int)value;
                if(distance==-1)
                {
                    result = "GPS定位失败";
                }
                else if (distance == 0)
                {
                    result = "正在获取";
                }
                else
                {
                    result = String.Format("{0}米", distance);
                }
                return result;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
