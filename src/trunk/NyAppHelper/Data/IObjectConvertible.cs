using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppHelper.Data
{

    /// <summary>
    /// 数据转换器的接口
    /// 适用于序列化和反序列化的转换器l类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectConvertible
    {

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
         string Serialize(object objectToSerialize);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        T Deserialize<T>(string jsonString);
    }
}
