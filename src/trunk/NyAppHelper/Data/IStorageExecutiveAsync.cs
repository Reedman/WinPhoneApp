using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppHelper.Data
{
    /// <summary>
    /// 用于异步操作本地文件的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStorageExecutiveAsync<T>
    {
        /// <summary>
        /// 获得本地存储
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> Get(string key);

        /// <summary>
        /// 新增一个本地存储
        /// </summary>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> Set(T t, string key);

        /// <summary>
        /// 判断本地存储的文件是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsExists(string key);

    }
}
