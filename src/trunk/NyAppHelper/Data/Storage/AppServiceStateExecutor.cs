using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppHelper.Data
{
    /// <summary>
    /// 操作PhoneApplicationService的state全局对象
    /// 中存储的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppServiceStateExecutor<T>:IStorageExecutive<T>
    {
        /// <summary>
        ///获得数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get(string key)
        {
            return (T)PhoneApplicationService.Current.State[key];
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Set(T t, string key)
        {
            PhoneApplicationService.Current.State[key] = t;
            return true;
        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExists(string key)
        {
            if(PhoneApplicationService.Current.State.ContainsKey(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
