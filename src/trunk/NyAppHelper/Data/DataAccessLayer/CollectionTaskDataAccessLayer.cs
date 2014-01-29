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
    public class CollectionTaskDataAccessLayer : IDataAccessible<CollectionTask>
    {


        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);


        /// <summary>
        /// 获得采集任务
        /// </summary>
        /// <param name="taskId">采集任务的Id</param>
        /// <returns></returns>
        public CollectionTask Get(int taskId)
        {
            var task = _dataContextInstance.CollectionTaskTable.FirstOrDefault(t => t.ID == taskId);
            var natureList = _dataContextInstance.CollectionTaskNatureTable.Where(n => n.ID == task.ID).ToList<CollectionTaskNature>();
            if (natureList != null && natureList.Count > 0)
            {
                task.NatureInfoList = natureList;
            }
            var pestList = _dataContextInstance.CollectionPestViewTable.Where(p => p.ID == task.ID).ToList<CollectionTaskPestView>();
            {
                if (pestList != null && pestList.Count > 0)
                {
                    task.PestList = pestList;
                }
            }
            return task;
        }

        /// <summary>
        /// 获得所有的采集任务
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<CollectionTask> GetAll()
        {
            ObservableCollection<CollectionTask> collectionTasks = new ObservableCollection<CollectionTask>();
            var collectionTaskList = _dataContextInstance.CollectionTaskTable.Where(t => t.TaskStatus < 3).ToList<CollectionTask>();
            if (collectionTaskList != null && collectionTaskList.Count() > 0)
            {
                foreach (var task in collectionTaskList)
                {
                    var natureList = _dataContextInstance.CollectionTaskNatureTable.Where(n => n.ID == task.ID).ToList<CollectionTaskNature>();
                    if (natureList != null && natureList.Count > 0)
                    {
                        task.NatureInfoList = natureList;
                    }
                    var pestList = _dataContextInstance.CollectionPestViewTable.Where(p => p.ID == task.ID).ToList<CollectionTaskPestView>();
                    {
                        if (pestList != null && pestList.Count > 0)
                        {
                            task.PestList = pestList;
                        }
                    }
                    collectionTasks.Add(task);
                }
            }
            return collectionTasks;
        }

        /// <summary>
        /// 添加采集任务信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Add(CollectionTask task)
        {
            bool result = false;
            try
            {
                _dataContextInstance.CollectionTaskTable.InsertOnSubmit(task);
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
        /// 更新采集任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool Update(CollectionTask task)
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
        /// 删除采集任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public bool Remove(CollectionTask task)
        {
            bool result = false;
            try
            {
                _dataContextInstance.CollectionTaskTable.DeleteOnSubmit(task);
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
        /// 添加所有采集任务
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<CollectionTask> tasks)
        {
            bool result = false;
            if (tasks != null)
            {
                try
                {
                    _dataContextInstance.CollectionTaskTable.InsertAllOnSubmit<CollectionTask>(tasks);
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


        public bool RemoveAll(ObservableCollection<Model.CollectionTask> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.CollectionTaskTable.DeleteAllOnSubmit<CollectionTask>(t);
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
