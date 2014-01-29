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
    /// 采集任务结果的数据访问层
    /// </summary>
    public class CollectionTaskResultDataAccessLayer : IDataAccessible<CollectionTaskResult>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);

        [Obsolete("查询任务结果必须传入任务的ID")]
        public ObservableCollection<CollectionTaskResult> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获得采集任务的所有结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ObservableCollection<CollectionTaskResult> GetAll(int taskId)
        {
            ObservableCollection<CollectionTaskResult> results = new ObservableCollection<CollectionTaskResult>();
            try
            {
                var resultList = _dataContextInstance.CollectionTaskResultTable.Where(r => r.TaskId == taskId).ToList<CollectionTaskResult>();
                if (resultList != null && resultList.Count() > 0)
                {
                    foreach (var result in resultList)
                    {
                        int resultId = result.CollectionResultID.HasValue ? result.CollectionResultID.Value : result.UniqueId;
                        result.CollectionResultDiseaseList = GetAllDisease(resultId).ToList<CollectionTaskResultDisease>();
                        result.CollectionResultPestList = GetAllPests(resultId).ToList<CollectionTaskResultPest>();
                        result.CollectionResultWeedList = GetAllWeed(resultId).ToList<CollectionTaskResultWeed>();
                        results.Add(result);
                    }
                }
                return results;
            }
            catch 
            {
                return results;
            }
        }

        /// <summary>
        /// 获得所有的病害结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ObservableCollection<CollectionTaskResultDisease> GetAllDisease(int id)
        {
            ObservableCollection<CollectionTaskResultDisease> results = new ObservableCollection<CollectionTaskResultDisease>();
            var resultList = _dataContextInstance.CollectionTaskResultDiseaseTable.Where(r => r.RelatedResultID == id).ToList<CollectionTaskResultDisease>();
            if (resultList != null && resultList.Count > 0)
            {
                foreach (var result in resultList)
                {
                    results.Add(result);
                }
            }
            return results;
        }

        /// <summary>
        /// 获得所有的虫害结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ObservableCollection<CollectionTaskResultPest> GetAllPests(int id)
        {
            ObservableCollection<CollectionTaskResultPest> results = new ObservableCollection<CollectionTaskResultPest>();
            var resultList = _dataContextInstance.CollectionTaskResultPestTable.Where(r => r.RelatedResultID == id).ToList<CollectionTaskResultPest>();
            if (resultList != null && resultList.Count > 0)
            {
                foreach (var result in resultList)
                {

                    results.Add(result);
                }
            }
            return results;
        }

        /// <summary>
        /// 获得所有的草害结果
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ObservableCollection<CollectionTaskResultWeed> GetAllWeed(int id)
        {
            ObservableCollection<CollectionTaskResultWeed> results = new ObservableCollection<CollectionTaskResultWeed>();
            var resultList = _dataContextInstance.CollectionTaskResultWeedTable.Where(r => r.RelatedResultID == id).ToList<CollectionTaskResultWeed>();
            if (resultList != null && resultList.Count > 0)
            {
                foreach (var result in resultList)
                {
                    results.Add(result);
                }
            }
            return results;
        }

        /// <summary>
        /// 添加采集结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool Add(CollectionTaskResult taskResult)
        {
            bool result = false;
            try
            {
                _dataContextInstance.CollectionTaskResultTable.InsertOnSubmit(taskResult);
                _dataContextInstance.SubmitChanges();
                var id = taskResult.UniqueId;
                if (taskResult.CollectionResultDiseaseList != null && taskResult.CollectionResultDiseaseList.Count > 0)
                {
                    AddAllDisease(id, taskResult.CollectionResultDiseaseList);
                }
                if (taskResult.CollectionResultPestList != null && taskResult.CollectionResultPestList.Count > 0)
                {
                    AddAllPest(id, taskResult.CollectionResultPestList);
                }
                if (taskResult.CollectionResultWeedList != null && taskResult.CollectionResultWeedList.Count > 0)
                {
                    AddAllWeed(id, taskResult.CollectionResultWeedList);
                }
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public bool AddAllDisease(int id, List<CollectionTaskResultDisease> list)
        {
            bool result = false;
            if (list != null && list.Count > 0)
            {
                try
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].RelatedResultID = id;
                    }
                    _dataContextInstance.CollectionTaskResultDiseaseTable.InsertAllOnSubmit<CollectionTaskResultDisease>(list);
                    _dataContextInstance.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        public bool AddAllPest(int id, List<CollectionTaskResultPest> list)
        {
            bool result = false;
            if (list != null && list.Count > 0)
            {
                try
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].RelatedResultID = id;
                    }
                    _dataContextInstance.CollectionTaskResultPestTable.InsertAllOnSubmit<CollectionTaskResultPest>(list);
                    _dataContextInstance.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        public bool AddAllWeed(int id, List<CollectionTaskResultWeed> list)
        {
            bool result = false;
            if (list != null && list.Count > 0)
            {
                try
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].RelatedResultID = id;
                    }
                    _dataContextInstance.CollectionTaskResultWeedTable.InsertAllOnSubmit<CollectionTaskResultWeed>(list);
                    _dataContextInstance.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        /// <summary>
        /// 更新采集结果
        /// </summary>
        /// <param name="taskResult"></param>
        /// <returns></returns>
        public bool Update(CollectionTaskResult taskResult)
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
        /// 删除采集结果
        /// </summary>
        /// <param name="taskResult"></param>
        /// <returns></returns>
        public bool Remove(CollectionTaskResult taskResult)
        {
            bool result = false;
            try
            {
                _dataContextInstance.CollectionTaskResultTable.DeleteOnSubmit(taskResult);
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
        /// 添加所有的采集结果
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<CollectionTaskResult> results)
        {
            bool result = false;
            if (results != null)
            {
                try
                {
                    _dataContextInstance.CollectionTaskResultTable.InsertAllOnSubmit<CollectionTaskResult>(results);
                    _dataContextInstance.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }


        public bool RemoveAll(ObservableCollection<CollectionTaskResult> t)
        {
            throw new NotImplementedException();
        }
    }
}
