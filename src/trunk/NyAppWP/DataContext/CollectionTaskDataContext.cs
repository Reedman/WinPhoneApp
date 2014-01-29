using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NyAppHelper;
using NyAppHelper.Model;
using NyAppHelper.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NyAppHelper.Http;
using NyAppHelper.Location;

namespace NyAppWP.DataContext
{
    public class CollectionTaskDataContext
    {

        private CollectionTaskDataAccessLayer _dataAccessLayer;
        private CollectionTaskPestViewDataAccessLayer _pestDataAccessLayer;
        private CollectionTaskNatureDataAccessLayer _natureDataAccessLayer;
        private CollectionTaskService _service;
        private ObservableCollection<TaskSortedByRegion> _tasks;

        public delegate void InitCollectionTasksCallbackHandler(ObservableCollection<TaskSortedByRegion> data);

        public CollectionTaskDataContext()
        {
            _dataAccessLayer = new CollectionTaskDataAccessLayer();
            _service = new CollectionTaskService();
            _pestDataAccessLayer = new CollectionTaskPestViewDataAccessLayer();
            _natureDataAccessLayer = new CollectionTaskNatureDataAccessLayer();
        }

        public async void InitCollectionTasks(InitCollectionTasksCallbackHandler callback)
        {
            _tasks = new ObservableCollection<TaskSortedByRegion>();
            var collectionTasks = new CollectionTaskDataAccessLayer().GetAll();
            if (collectionTasks.Count <= 0)
            {
                collectionTasks = await _service.GetAllTasks();
                _dataAccessLayer.AddAll(collectionTasks);
                foreach (var task in collectionTasks)
                {
                    if (task.NatureInfoList!=null&&task.NatureInfoList.Count > 0)
                    {
                        for (int i=0;i<task.NatureInfoList.Count;i++)
                        {
                            var natureInfo=task.NatureInfoList[i];
                            natureInfo.ID = task.ID;
                        }
                        _natureDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskNature>(task.NatureInfoList));
                    }
                    if (task.PestList!=null&&task.PestList.Count > 0)
                    {
                        for (int i=0;i<task.PestList.Count;i++)
                        {
                            var pestItem = task.PestList[i];
                            pestItem.ID = task.ID;
                        }
                        _pestDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskPestView>(task.PestList));
                    }
                }
            }
            var locationList = collectionTasks.Select(t => t.RegionName).Distinct().ToList<string>();
            locationList.Sort();
            var bindTaskModels = new ObservableCollection<CollectionTaskWrapper>();
            foreach (var locationItem in locationList)
            {
                var locationTasks = collectionTasks.Where(t => t.RegionName == locationItem).ToList<CollectionTask>();
                if (locationTasks != null && locationTasks.Count > 0)
                {
                    var contextItem = new TaskSortedByRegion(locationItem);
                    foreach (var locationTask in locationTasks)
                    {
                        contextItem.Add(new CollectionTaskWrapper(locationTask));
                    }
                    _tasks.Add(contextItem);
                }
            }
            callback(_tasks);
        }

        public async void RefreshCollectionTasks(InitCollectionTasksCallbackHandler callback)
        {
            _tasks = new ObservableCollection<TaskSortedByRegion>();
            var collectionTasks = _dataAccessLayer.GetAll();
            if (collectionTasks.Count <= 0)
            {
                collectionTasks = await _service.GetAllTasks();
                _dataAccessLayer.AddAll(collectionTasks);
                foreach (var task in collectionTasks)
                {
                    if (task.NatureInfoList != null && task.NatureInfoList.Count > 0)
                    {
                        for (int i = 0; i < task.NatureInfoList.Count; i++)
                        {
                            var natureInfo = task.NatureInfoList[i];
                            natureInfo.ID = task.ID;
                        }
                        _natureDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskNature>(task.NatureInfoList));
                    }
                    if (task.PestList != null && task.PestList.Count > 0)
                    {
                        for (int i = 0; i < task.PestList.Count; i++)
                        {
                            var pestItem = task.PestList[i];
                            pestItem.ID = task.ID;
                        }
                        _pestDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskPestView>(task.PestList));
                    }
                }
            }
            else
            {
                var newCollectionTasks = await _service.GetAllTasks();
                //更新本地的数据
                foreach (var task in collectionTasks)
                {
                    if (newCollectionTasks.Select(t => t.ID).Contains(task.ID))
                    {

                        var newTask = newCollectionTasks.FirstOrDefault(t => t.ID == task.ID);

                        task.TaskStatus = newTask.TaskStatus;
                        task.CreatedTime = newTask.CreatedTime;
                        task.FarmlandID = newTask.FarmlandID;
                        task.FarmlandName = newTask.FarmlandName;
                        task.CropID = newTask.CropID;
                        task.CropName = newTask.CropName;
                        task.RegionID = newTask.RegionID;
                        task.RegionName = newTask.RegionName;
                        task.ResultCount = newTask.ResultCount;
                        task.PlantingMethod = newTask.PlantingMethod;

                        _natureDataAccessLayer.RemoveAll(task.ID);
                        _pestDataAccessLayer.RemoveAll(task.ID);

                        if (newTask.NatureInfoList != null && newTask.NatureInfoList.Count > 0)
                        {
                            for (int i = 0; i < newTask.NatureInfoList.Count; i++)
                            {
                                var natureInfo = newTask.NatureInfoList[i];
                                natureInfo.ID = task.ID;
                            }
                            _natureDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskNature>(newTask.NatureInfoList));
                            task.NatureInfoList = newTask.NatureInfoList;
                        }
                        else
                        {
                            task.NatureInfoList = new List<CollectionTaskNature>();
                        }
                        if (newTask.PestList != null && newTask.PestList.Count > 0)
                        {
                            for (int i = 0; i < newTask.PestList.Count; i++)
                            {
                                var pestItem = newTask.PestList[i];
                                pestItem.ID = task.ID;
                            }
                            _pestDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskPestView>(newTask.PestList));
                            task.PestList = newTask.PestList;
                        }
                        else
                        {
                            task.PestList = new List<CollectionTaskPestView>();
                        }
                        _dataAccessLayer.Update(task);
                    }
                    else
                    {
                        //已经被审批的任务，无需显示
                        task.TaskStatus = 3;
                        _dataAccessLayer.Update(task);
                    }
                }
                //添加新数据
                foreach (var newTask in newCollectionTasks)
                {
                    if (!(collectionTasks.Select(t => t.ID).Contains(newTask.ID)))
                    {
                        _dataAccessLayer.Add(newTask);
                        if (newTask.NatureInfoList != null && newTask.NatureInfoList.Count > 0)
                        {
                            for (int i = 0; i < newTask.NatureInfoList.Count; i++)
                            {
                                var natureInfo = newTask.NatureInfoList[i];
                                natureInfo.ID = newTask.ID;
                            }
                            _natureDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskNature>(newTask.NatureInfoList));
                        }
                        if (newTask.PestList != null && newTask.PestList.Count > 0)
                        {
                            for (int i = 0; i < newTask.PestList.Count; i++)
                            {
                                var pestItem = newTask.PestList[i];
                                pestItem.ID = newTask.ID;
                            }
                            _pestDataAccessLayer.AddAll(new ObservableCollection<CollectionTaskPestView>(newTask.PestList));
                        }
                    }
                }
                collectionTasks = _dataAccessLayer.GetAll();
            }
            var locationList = collectionTasks.Select(t => t.RegionName).Distinct().ToList<string>();
            locationList.Sort();
            var bindTaskModels = new ObservableCollection<CollectionTaskWrapper>();
            foreach (var locationItem in locationList)
            {
                var locationTasks = collectionTasks.Where(t => t.RegionName == locationItem).ToList<CollectionTask>();
                if (locationTasks != null && locationTasks.Count > 0)
                {
                    var contextItem = new TaskSortedByRegion(locationItem);
                    foreach (var locationTask in locationTasks)
                    {
                        contextItem.Add(new CollectionTaskWrapper(locationTask));
                    }
                    _tasks.Add(contextItem);
                }
            }
            callback(_tasks);
        }

        public ObservableCollection<TaskSortedByRegion> GetCollectionTasks()
        {
            _tasks = new ObservableCollection<TaskSortedByRegion>();
            var collectionTasks = new CollectionTaskDataAccessLayer().GetAll();
            var locationList = collectionTasks.Select(t => t.RegionName).Distinct().ToList<string>();
            locationList.Sort();
            var bindTaskModels = new ObservableCollection<CollectionTaskWrapper>();
            foreach (var locationItem in locationList)
            {
                var locationTasks = collectionTasks.Where(t => t.RegionName == locationItem).ToList<CollectionTask>();
                if (locationTasks != null && locationTasks.Count > 0)
                {
                    var contextItem = new TaskSortedByRegion(locationItem);
                    foreach (var locationTask in locationTasks)
                    {
                        contextItem.Add(new CollectionTaskWrapper(locationTask));
                    }
                    _tasks.Add(contextItem);
                }
            }
            return _tasks;
        }

        public ObservableCollection<TaskSortedByRegion> GetCollectionTasks(int status, int distance = 100000)
        {
            _tasks = new ObservableCollection<TaskSortedByRegion>();
            var collectionTasks = new CollectionTaskDataAccessLayer().GetAll();
            var locationList = collectionTasks.Select(t => t.RegionName).Distinct().ToList<string>();
            locationList.Sort();
            var bindTaskModels = new ObservableCollection<CollectionTaskWrapper>();
            foreach (var locationItem in locationList)
            {
                var locationTasks = collectionTasks.Where(t => t.RegionName == locationItem).ToList<CollectionTask>();
                if (locationTasks != null && locationTasks.Count > 0)
                {
                    var contextItem = new TaskSortedByRegion(locationItem);
                    foreach (var locationTask in locationTasks)
                    {
                        var taskWarpper = new CollectionTaskWrapper(locationTask);
                        if (status >= 0)
                        {
                            if (taskWarpper.Status == status && taskWarpper.Distance <= distance)
                            {
                                contextItem.Add(new CollectionTaskWrapper(locationTask));
                            }
                        }
                        else
                        {
                            if (taskWarpper.Distance <= distance)
                            {
                                contextItem.Add(new CollectionTaskWrapper(locationTask));
                            }
                        }
                    }
                    if (contextItem.Count > 0)
                    {
                        _tasks.Add(contextItem);
                    }
                }
            }
            return _tasks;
        }

    }

    /// <summary>
    /// 对采集任务根据地点进行分类
    /// </summary>
    public class TaskSortedByRegion : ObservableCollection<CollectionTaskWrapper>
    {

        public TaskSortedByRegion(string regionName)
        {
            RegionName = regionName;
        }

        /// <summary>
        /// 采集地点
        /// </summary>
        public string RegionName { get; private set; }

    }

    /// <summary>
    /// 任务采集的包装类
    /// </summary>
    public class CollectionTaskWrapper : CollectionTask
    {
        private static string _assetPath = @"/Assets/Icons/{0}.png";
        private string _imagePath;
        private int _taskStatus;
        private int _collectedResultsCount;
        private int _finishedResultCount;
        private int _distance;

        public CollectionTaskWrapper(CollectionTask task)
        {
            this.ID = task.ID;
            this.Status = task.TaskStatus;
            this.CreatedTime = task.CreatedTime;
            this.CollectionStartTime = task.CollectionStartTime;
            this.CollectionEndTime = task.CollectionEndTime;
            this.PestList = task.PestList;
            this.CropID = task.CropID;
            this.CropName = task.CropName;
            this.FarmlandID = task.FarmlandID;
            this.FarmlandName = task.FarmlandName;
            this.Geo = task.Geo;
            this.PestList = task.PestList;
            this.TaskStatus = task.TaskStatus;
            this.ResultCount = task.ResultCount;
            this.NatureInfoList = task.NatureInfoList;
            this.RegionID = task.RegionID;
            this.RegionName = task.RegionName;
            this.PlantingMethod = task.PlantingMethod;
        }

        /// <summary>
        /// 距离，根据当前位置地理位置和采集点的直线距离
        /// </summary>
        public int Distance
        {
            get
            {
                if (_distance == 0)
                {
                    LocationHelper.GetLocation((data) =>
                   {
                       if (data != null)
                       {
                           var latitude = data.Coordinate.Latitude;
                           var longitude = data.Coordinate.Longitude;
                           if(String.IsNullOrEmpty(this.Geo))
                           {
                               _distance = 0;
                           }
                           else
                           {
                               var arrGeo = this.Geo.Split(';');
                               var lon = double.Parse(arrGeo[0].Split(',')[0]);
                               var lat = double.Parse(arrGeo[0].Split(',')[1]);

                               this.NotifyPropertyChanging("Distance");
                               _distance = Convert.ToInt32(LocationHelper.Distance(latitude, longitude, lat, lon));
                               this.NotifyPropertyChanged("Distance");

                               //var geoArray = this.Geo.Split(',').ToList<string>();
                               //var la2 = double.Parse(geoArray[0], System.Globalization.CultureInfo.InvariantCulture);
                               //var lo2 = double.Parse(geoArray[1].Replace(";", String.Empty).Trim(), System.Globalization.CultureInfo.InvariantCulture);
                           }
                       }
                       else
                       {
                           this.NotifyPropertyChanging("Distance");
                           _distance = -1;
                           this.NotifyPropertyChanged("Distance");
                       }
                   });
                }
                return _distance;
            }
            set
            {
                if (_distance != value)
                {
                    _distance = value;
                }
            }
        }

        /// <summary>
        /// 任务已采集的结果数
        /// </summary>
        public int CollectedResultsCount
        {
            get
            {
                var allResults = new CollectionTaskResultDataAccessLayer().GetAll(this.ID);
                if (allResults != null)
                {
                    _collectedResultsCount = allResults.Count();
                }
                else
                {
                    _collectedResultsCount = 0;
                }
                return _collectedResultsCount;
            }
            set
            {
                if (_collectedResultsCount != value)
                {
                    this.NotifyPropertyChanging("CollectedResultsCount");
                    _collectedResultsCount = value;
                    this.NotifyPropertyChanged("CollectedResultsCount");
                }
            }
        }

        /// <summary>
        /// 任务已提交的结果数
        /// </summary>
        public int FinishedResultCount
        {
            get
            {
                if (this.ResultCount <= 0)
                {
                    var allResults = new CollectionTaskResultDataAccessLayer().GetAll(this.ID);
                    _finishedResultCount = allResults.Count(r => r.CollectionResultID != null);
                }
                else
                {
                    var allResults = new CollectionTaskResultDataAccessLayer().GetAll(this.ID);
                    _finishedResultCount = allResults.Count(r => r.CollectionResultID != null);
                    _finishedResultCount += this.ResultCount;
                }
                return _finishedResultCount;
            }
            set
            {
                if (_finishedResultCount != value)
                {
                    this.NotifyPropertyChanging("FinishedResultCount");
                    _finishedResultCount = value;
                    this.NotifyPropertyChanged("FinishedResultCount");
                }
            }
        }

        /// <summary>
        /// 任务的总状态
        /// 0,如果已采集的结果等于已上传的结果等于0
        /// 1,已采集的结果等于大于已上传的结果
        /// 2,已采集的结果等于已上传的结果并且大于0
        /// </summary>
        public int Status
        {
            get
            {
                if (CollectedResultsCount == 0 && FinishedResultCount == 0)
                {
                    _taskStatus = 0;
                }
                else if (CollectedResultsCount > FinishedResultCount)
                {
                    _taskStatus = 1;
                }
                else
                {
                    _taskStatus = 2;
                }
                return _taskStatus;
            }
            set
            {
                if (_taskStatus != value)
                {
                    this.NotifyPropertyChanging("Status");
                    this._taskStatus = value;
                    this.NotifyPropertyChanged("Status");
                }
            }
        }


        public String ImagePath
        {
            get
            {
                string imageFilePath = String.Empty;
                switch (this.Status)
                {
                    case 0:
                        {
                            imageFilePath = String.Format(_assetPath, "Workload-ToDo-icon");
                            break;
                        }
                    case 1:
                        {
                            imageFilePath = String.Format(_assetPath, "Workload-ToUpload-icon");
                            break;
                        }
                    case 2:
                        {
                            imageFilePath = String.Format(_assetPath, "Workload-Done-icon");
                            break;
                        }
                }
                this._imagePath = imageFilePath;
                return this._imagePath;
            }
            set
            {
                this.NotifyPropertyChanging("ImagePath");
                this._imagePath = value;
                this.NotifyPropertyChanged("ImagePath");
            }
        }

    }

}
