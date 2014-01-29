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
    /// 病虫草害数据字典信息的数据访问层
    /// </summary>
    public class PestDataAccessLayer:IDataAccessible<Pest>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);

        /// <summary>
        /// 获取本地的病虫草害数据字典信息
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Pest> GetAll()
        {
            ObservableCollection<Pest> pests = new ObservableCollection<Pest>();
            var pestList = _dataContextInstance.PestTable.Select(p => p).ToList();
            if (pestList != null && pestList.Count() > 0)
            {
                pests = new ObservableCollection<Pest>(pestList);
            }
            return pests;
        }

        public bool Add(Pest t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加本地的病虫草害数据字典信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<Pest> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.PestTable.InsertAllOnSubmit<Pest>(t);
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

        public bool Update(Pest t)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Pest t)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(ObservableCollection<Pest> t)
        {
            throw new NotImplementedException();
        }
    }
}
