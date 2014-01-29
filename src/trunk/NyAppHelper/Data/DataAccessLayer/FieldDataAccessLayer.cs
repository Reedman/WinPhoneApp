using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Collections.ObjectModel;
using Microsoft.Phone.Data.Linq;

using NyAppHelper.Model;

namespace NyAppHelper.Data
{
    /// <summary>
    /// 农田的数据访问层
    /// </summary>
    public class FieldDataAccessLayer : IDataAccessible<Field>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);


        /// <summary>
        /// 获得农田
        /// </summary>
        /// <param name="filed">农田的ID</param>
        /// <returns></returns>
        public Field Get(int filed)
        {
            return _dataContextInstance.FieldTable.FirstOrDefault(f => f.FarmlandID == filed);
        }

        /// <summary>
        /// 不支持获得全部的农田信息，必须输入农户的ID进行查询
        /// </summary>
        /// <returns></returns>
        [Obsolete("不支持获得全部的农田信息，必须输入农户的ID进行查询")]
        public ObservableCollection<Field> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得农户的所有农田信息
        /// </summary>
        /// <param name="ownerId">农户的ID</param>
        /// <returns></returns>
        public ObservableCollection<Field> GetAll(int ownerId)
        {
            ObservableCollection<Field> farms = null;
            var farmList = _dataContextInstance.FieldTable.Where(f => f.OwnerID == ownerId).ToList<Field>();
            if (farmList != null && farmList.Count() > 0)
            {
                farms = new ObservableCollection<Field>(farmList);
            }
            return farms;
        }

        /// <summary>
        /// 添加一个农田信息
        /// </summary>
        /// <param name="farm"></param>
        /// <returns></returns>
        public bool Add(Field field)
        {
            bool result = false;
            try
            {
                _dataContextInstance.FieldTable.InsertOnSubmit(field);
                _dataContextInstance.SubmitChanges();
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 更新农田信息
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool Update(Field field)
        {
            bool result = false;
            try
            {
                _dataContextInstance.SubmitChanges();
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 删除农田信息
        /// </summary>
        /// <param name="filed"></param>
        /// <returns></returns>
        public bool Remove(Field filed)
        {
            bool result = false;
            try
            {
                _dataContextInstance.FieldTable.DeleteOnSubmit(filed);
                _dataContextInstance.SubmitChanges();
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }


        /// <summary>
        /// 添加所有田块
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<Field> fields)
        {
            bool result = false;
            if (fields != null)
            {
                try
                {
                    _dataContextInstance.FieldTable.InsertAllOnSubmit<Field>(fields);
                    _dataContextInstance.SubmitChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }


        public bool RemoveAll(ObservableCollection<Field> t)
        {
            throw new NotImplementedException();
        }
    }
}
