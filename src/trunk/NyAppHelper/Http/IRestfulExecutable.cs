using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppHelper.Http
{
    /// <summary>
    /// 使用Restful服务的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IRestfulExecutable<T>
    {
        /// <summary>
        /// 与服务同步对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<T> Sync(int Id);
        /// <summary>
        /// 向服务器更新对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<bool> Update(T t);
        /// <summary>
        /// 向服务器添加对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        Task<T> Add(T t);
    }
}
