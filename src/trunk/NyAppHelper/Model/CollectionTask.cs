using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using NyAppHelper.Data;
using Newtonsoft.Json;

namespace NyAppHelper.Model
{
    /// <summary>
    /// 采集任务的Model类
    /// </summary>
    [Table(Name = "TaskInfo")]
    public class CollectionTask : SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _id;
        private int _farmlandID;
        private string _farmlandName;
        private DateTime _createdTime;
        private int _regionID;
        private string _regionName;
        private int _cropID;
        private string _cropName;
        private int? _plantingMethod;
        private string _geo;
        private DateTime _collectionStartTime;
        private DateTime _collectionEndTime;
        private int _taskStatus;
        private int _resultCount;
        private List<CollectionTaskPestView> _pestList;
        private List<CollectionTaskNature> _natureInfoList;

        #endregion

        #region 公有变量

        /// <summary>
        /// 记录ID，主键
        /// </summary>
        [Column(Name = "UniqueId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int UniqueId
        {
            get
            {
                return _uniqueId;
            }
            set
            {
                if (_uniqueId != value)
                {
                    this.NotifyPropertyChanging("UniqueId");
                    _uniqueId = value;
                    this.NotifyPropertyChanged("UniqueId");
                }
            }
        }

        /// <summary>
        /// 采集任务的ID
        /// </summary>
        [Column(Name = "ID", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                {
                    this.NotifyPropertyChanging("ID");
                    _id = value;
                    this.NotifyPropertyChanged("ID");
                }
            }
        }

        /// <summary>
        /// 采集任务的田块ID
        /// </summary>
        [Column(Name = "FarmlandID", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int FarmlandID
        {
            get
            {
                return _farmlandID;
            }
            set
            {
                if (_farmlandID != value)
                {
                    this.NotifyPropertyChanging("FarmlandID");
                    _farmlandID = value;
                    this.NotifyPropertyChanged("FarmlandID");
                }
            }
        }

        /// <summary>
        /// 采集任务的田块名字
        /// </summary>
        [Column(Name = "FarmlandName", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string FarmlandName
        {
            get
            {
                return _farmlandName;
            }
            set
            {
                if (_farmlandName != value)
                {
                    this.NotifyPropertyChanging("FarmlandName");
                    _farmlandName = value;
                    this.NotifyPropertyChanged("FarmlandName");
                }
            }
        }

        /// <summary>
        /// 采集任务的接收的时间
        /// </summary>
        [Column(Name = "CreatedTime", DbType = "DATETIME NOT NULL", CanBeNull = false)]
        public DateTime CreatedTime
        {
            get
            {
                return _createdTime;
            }
            set
            {
                if (_createdTime != value)
                {
                    this.NotifyPropertyChanging("CreatedTime");
                    _createdTime = value;
                    this.NotifyPropertyChanged("CreatedTime");
                }
            }
        }

        /// <summary>
        /// 采集任务的田块的行政区域ID
        /// </summary>
        [Column(Name = "RegionID", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int RegionID
        {
            get
            {
                return _regionID;
            }
            set
            {
                if (_regionID != value)
                {
                    this.NotifyPropertyChanging("RegionID");
                    _regionID = value;
                    this.NotifyPropertyChanged("RegionID");
                }
            }
        }

        /// <summary>
        /// 采集任务的田块的行政区域名字
        /// </summary>
        [Column(Name = "RegionName", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string RegionName
        {
            get
            {
                return _regionName;
            }
            set
            {
                if (_regionName != value)
                {
                    this.NotifyPropertyChanging("RegionName");
                    _regionName = value;
                    this.NotifyPropertyChanged("RegionName");
                }
            }
        }

        /// <summary>
        /// 采集任务的田块的种植作物种类ID
        /// </summary>
        [Column(Name = "CropID", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int CropID
        {
            get
            {
                return _cropID;
            }
            set
            {
                if (_cropID != value)
                {
                    this.NotifyPropertyChanging("CropID");
                    _cropID = value;
                    this.NotifyPropertyChanged("CropID");
                }
            }
        }

        /// <summary>
        /// 采集任务的田块的种植作物种类名称
        /// </summary>
        [Column(Name = "CropName", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string CropName
        {
            get
            {
                return _cropName;
            }
            set
            {
                if (_cropName != value)
                {
                    this.NotifyPropertyChanging("CropName");
                    _cropName = value;
                    this.NotifyPropertyChanged("CropName");
                }
            }
        }

        /// <summary>
        /// 采集任务的田块的种植作物种类名称
        /// </summary>
        [Column(Name = "Geo", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string Geo
        {
            get
            {
                return _geo;
            }
            set
            {
                if (_geo != value)
                {
                    this.NotifyPropertyChanging("Geo");
                    _geo = value;
                    this.NotifyPropertyChanged("Geo");
                }
            }
        }

        /// <summary>
        /// 任务状态
        /// (未采集/处理中/已采集　0/1/2)
        /// </summary>
        [Column(Name = "TaskStatus", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int TaskStatus
        {
            get
            {
                return _taskStatus;
            }
            set
            {
                if (_taskStatus != value)
                {
                    this.NotifyPropertyChanging("TaskStatus");
                    _taskStatus = value;
                    this.NotifyPropertyChanged("TaskStatus");
                }
            }
        }

        /// <summary>
        /// 已经采集并上传的结果个数
        /// </summary>
        [Column(Name = "ResultCount", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int ResultCount
        {
            get
            {
                return _resultCount;
            }
            set
            {
                if (_resultCount != value)
                {
                    this.NotifyPropertyChanging("ResultCount");
                    _resultCount = value;
                    this.NotifyPropertyChanged("ResultCount");
                }
            }
        }

        /// <summary>
        /// 采集任务的自然环境要求列表
        /// </summary>
        public List<CollectionTaskNature> NatureInfoList
        {
            get
            {
                return _natureInfoList;
            }
            set
            {
                if (_natureInfoList != value)
                {
                    _natureInfoList = value;
                }
            }
        }

        /// <summary>
        /// 采集任务的开始的时间
        /// </summary>
        [Column(Name = "CollectionStartTime", DbType = "DATETIME NOT NULL", CanBeNull = false)]
        public DateTime CollectionStartTime
        {
            get
            {
                return _collectionStartTime;
            }
            set
            {
                if (_collectionStartTime != value)
                {
                    this.NotifyPropertyChanging("CollectionStartTime");
                    _collectionStartTime = value;
                    this.NotifyPropertyChanged("CollectionStartTime");
                }
            }
        }

        /// <summary>
        /// 采集任务的结束的时间
        /// </summary>
        [Column(Name = "CollectionEndTime", DbType = "DATETIME NOT NULL", CanBeNull = false)]
        public DateTime CollectionEndTime
        {
            get
            {
                return _collectionEndTime;
            }
            set
            {
                if (_collectionEndTime != value)
                {
                    this.NotifyPropertyChanging("CollectionEndTime");
                    _collectionEndTime = value;
                    this.NotifyPropertyChanged("CollectionEndTime");
                }
            }
        }

        /// <summary>
        /// 作物的种植方式
        /// </summary>
        [Column(Name = "PlantingMethod", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? PlantingMethod
        {
            get
            {
                return _plantingMethod;
            }
            set
            {
                if (_plantingMethod != value)
                {
                    this.NotifyPropertyChanging("PlantingMethod");
                    _plantingMethod = value;
                    this.NotifyPropertyChanged("PlantingMethod");
                }
            }
        }

        /// <summary>
        /// 病虫害类型列表
        /// </summary>
        public List<CollectionTaskPestView> PestList
        {
            get
            {
                return _pestList;
            }
            set
            {
                if (_pestList != value)
                {
                    _pestList = value;
                }
            }
        }

        #endregion

    }

}
