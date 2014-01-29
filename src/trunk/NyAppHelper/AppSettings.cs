using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppHelper
{
    /// <summary>
    /// AppSettings for WP App
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// 数据库连接字符串模板
        /// </summary>
        public static readonly string DBConStr = @"Data Source=isostore:/ny.sdf";

        /// <summary>
        /// App版本号，用于更新数据库
        /// </summary>
        public static readonly int AppDBVersion = 1;

        /// <summary>
        ///  IsolateStorage数据的文件路径
        /// </summary>
        public static readonly string IsolateStorageDataFolderPath = "ISLocal";

        /// <summary>
        /// IsolateStorage照片的文件路径
        /// </summary>
        public static readonly string IsolateStoragePhotoFolderPath = "Photos";

        /// <summary>
        /// Restful Api url
        /// </summary>
        public static readonly String RestUrl = @"http://jtny.chinacloudsites.cn/API";
        
        /// <summary>
        /// 百度天气的API URL
        /// </summary>
        public static readonly String WeatherUrl = @"http://api.map.baidu.com/telematics/v3";

        /// <summary>
        /// 百度的Appkey
        /// </summary>
        public static readonly String BaiduApiKey = @"IRrVY8bWUmxlwjbQEyqUGvlr";

        /// <summary>
        /// 加密的hash key
        /// </summary>
        public static readonly string HashKey = @"W&(+)*&F8823";

        /// <summary>
        /// 默认为空的时间，
        /// 保证在JSON反序列化时，当时间对象为空，不会报错
        /// </summary>
        public static readonly DateTime DefaultNullableDate = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 默认的农户田块图片
        /// </summary>
        public static readonly string DefaultFieldImagePath = @"/Assets/Farmer/strawberry-field.jpg";



    }
}
