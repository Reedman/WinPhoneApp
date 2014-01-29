using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NyAppHelper.Data
{
    /// <summary>
    /// 数据访问层接口
    /// </summary>
    public interface IDataAccessible<T>
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        ObservableCollection<T> GetAll();

        /// <summary>
        ///添加数据 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Add(T t);

        /// <summary>
        /// 添加所有数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool AddAll(ObservableCollection<T> t);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Update(T t);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool Remove(T t);

        /// <summary>
        /// 删除所有数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool RemoveAll(ObservableCollection<T> t);

    }
}
