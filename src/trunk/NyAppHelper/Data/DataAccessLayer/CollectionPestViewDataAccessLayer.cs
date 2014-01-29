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
    /// 采集任务病虫害要求的数据访问层
    /// </summary>
    public class CollectionTaskPestViewDataAccessLayer:IDataAccessible<CollectionTaskPestView>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);

        public ObservableCollection<CollectionTaskPestView> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得采集任务的病虫害要求
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ObservableCollection<CollectionTaskPestView> GetAll(int taskId)
        {
            ObservableCollection<CollectionTaskPestView> pests = new ObservableCollection<CollectionTaskPestView>();
            var pestList = _dataContextInstance.CollectionPestViewTable.Where(p => p.ID == taskId).ToList();
            if (pestList != null && pestList.Count() > 0)
            {
                pests = new ObservableCollection<CollectionTaskPestView>(pestList);
            }
            return pests;
        }

        public bool Add(CollectionTaskPestView t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加采集任务的所有病虫害要求
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<CollectionTaskPestView> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.CollectionPestViewTable.InsertAllOnSubmit<CollectionTaskPestView>(t);
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

        public bool Update(CollectionTaskPestView t)
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

        public bool Remove(CollectionTaskPestView t)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll(ObservableCollection<CollectionTaskPestView> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.CollectionPestViewTable.DeleteAllOnSubmit<CollectionTaskPestView>(t);
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
            if (taskId > 0)
            {
                try
                {
                    var list = _dataContextInstance.CollectionPestViewTable.Where(n => n.ID == taskId);
                    _dataContextInstance.CollectionPestViewTable.DeleteAllOnSubmit<CollectionTaskPestView>(list);
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
