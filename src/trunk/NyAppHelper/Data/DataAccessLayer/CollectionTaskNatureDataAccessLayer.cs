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
    /// 采集任务自然环境要求的数据访问层
    /// </summary>
    public class CollectionTaskNatureDataAccessLayer : IDataAccessible<CollectionTaskNature>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);


        public ObservableCollection<CollectionTaskNature> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得采集任务的自然环境要求
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ObservableCollection<CollectionTaskNature> GetAll(int taskId)
        {
            ObservableCollection<CollectionTaskNature> natures = new ObservableCollection<CollectionTaskNature>();
            var natureList = _dataContextInstance.CollectionTaskNatureTable.Where(n => n.ID == taskId).ToList();
            if (natureList != null && natureList.Count() > 0)
            {
                natures = new ObservableCollection<CollectionTaskNature>(natureList);
            }
            return natures;
        }

        public bool Add(CollectionTaskNature t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加采集任务的所有自然环境要求
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<CollectionTaskNature> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.CollectionTaskNatureTable.InsertAllOnSubmit<CollectionTaskNature>(t);
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

        public bool Update(CollectionTaskNature t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
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

        public bool Remove(CollectionTaskNature t)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(ObservableCollection<CollectionTaskNature> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.CollectionTaskNatureTable.AttachAll<CollectionTaskNature>(t,false);
                    _dataContextInstance.CollectionTaskNatureTable.DeleteAllOnSubmit<CollectionTaskNature>(t);
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

        public bool RemoveAll(int taskId)
        {
            bool result = false;
            if (taskId >0)
            {
                try
                {
                    var list = _dataContextInstance.CollectionTaskNatureTable.Where(n => n.ID == taskId).ToList<CollectionTaskNature>();
                    _dataContextInstance.CollectionTaskNatureTable.DeleteAllOnSubmit<CollectionTaskNature>(list);
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

    }
}
