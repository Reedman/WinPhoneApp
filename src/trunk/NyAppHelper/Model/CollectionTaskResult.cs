using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using NyAppHelper.Data;

namespace NyAppHelper.Model
{

    /// <summary>
    /// 采集结果病虫草害异常的枚举
    /// </summary>
    public enum PestType : int
    {
        /// <summary>
        /// 虫害
        /// </summary>
        InssectPest = 0,
        /// <summary>
        /// 病害
        /// </summary>
        Disease=1,
        /// <summary>
        /// 草害
        /// </summary>
        Weed=2,
        /// <summary>
        /// 异常
        /// </summary>
        Exception=3,
    }


    public class TaskException
    {
        public int? ExceptionID { get; set; }
        public string Exception { get; set; }
    }

    /// <summary>
    /// 采集任务结果的Model类
    /// </summary>
    [Table(Name = "TaskResultInfo")]
    public class CollectionTaskResult : SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _taskId;
        private int? _collectionResultID;
        private int? _collectionPointID;
        private string _collectionLongitude;
        private string _collectionLatitude;
        private int _farmlandID;
        private float? _airTemperature;
        private float? _airHumidity;
        private float? _soilTemperature;
        private float? _soilMoisture;
        private float? _soilPH;
        private int? _soilIon;
        private float? _soilFertility;
        private float? _leafTemp;
        private string _remark;
        private int? _exceptionId;
        private string _exception;


        private List<CollectionTaskResultDisease> _collectionResultDiseaseList;
        private List<CollectionTaskResultPest> _collectionResultPestList;
        private List<CollectionTaskResultWeed> _collectionResultWeedList;

        #endregion

        #region 公共变量

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
        [Column(Name = "TaskId", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int TaskId
        {
            get
            {
                return _taskId;
            }
            set
            {
                if (_taskId != value)
                {
                    this.NotifyPropertyChanging("TaskId");
                    _taskId = value;
                    this.NotifyPropertyChanged("TaskId");
                }
            }
        }

        /// <summary>
        /// 采集任务的ID
        /// </summary>
        [Column(Name = "CollectionResultID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? CollectionResultID
        {
            get
            {
                return _collectionResultID;
            }
            set
            {
                if (_collectionResultID != value)
                {
                    this.NotifyPropertyChanging("CollectionResultID");
                    _collectionResultID = value;
                    this.NotifyPropertyChanged("CollectionResultID");
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
        /// 空气温度
        /// </summary>
        [Column(Name = "AirTemperature", CanBeNull = true)]
        public float? AirTemperature
        {
            get
            {
                return _airTemperature;
            }
            set
            {
                if (_airTemperature != value)
                {
                    this.NotifyPropertyChanging("AirTemperature");
                    _airTemperature = value;
                    this.NotifyPropertyChanged("AirTemperature");
                }
            }
        }

        /// <summary>
        /// 空气湿度
        /// </summary>
        [Column(Name = "AirHumidity", CanBeNull = true)]
        public float? AirHumidity
        {
            get
            {
                return _airHumidity;
            }
            set
            {
                if (_airHumidity != value)
                {
                    this.NotifyPropertyChanging("AirHumidity");
                    _airHumidity = value;
                    this.NotifyPropertyChanged("AirHumidity");
                }
            }
        }

        /// <summary>
        /// 土壤温度
        /// </summary>
        [Column(Name = "SoilTemperature", CanBeNull = true)]
        public float? SoilTemperature
        {
            get
            {
                return _soilTemperature;
            }
            set
            {
                if (_soilTemperature != value)
                {
                    this.NotifyPropertyChanging("SoilTemperature");
                    _soilTemperature = value;
                    this.NotifyPropertyChanged("SoilTemperature");
                }
            }
        }

        /// <summary>
        /// 土壤湿度
        /// </summary>
        [Column(Name = "SoilMoisture", CanBeNull = true)]
        public float? SoilMoisture
        {
            get
            {
                return _soilMoisture;
            }
            set
            {
                if (_soilMoisture != value)
                {
                    this.NotifyPropertyChanging("SoilMoisture");
                    _soilMoisture = value;
                    this.NotifyPropertyChanged("SoilMoisture");
                }
            }
        }

        /// <summary>
        /// 土壤PH
        /// </summary>
        [Column(Name = "SoilPH", CanBeNull = true)]
        public float? SoilPH
        {
            get
            {
                return _soilPH;
            }
            set
            {
                if (_soilPH != value)
                {
                    this.NotifyPropertyChanging("SoilPH");
                    _soilPH = value;
                    this.NotifyPropertyChanged("SoilPH");
                }
            }
        }

        /// <summary>
        /// 土壤总离子数
        /// </summary>
        [Column(Name = "SoilIon", CanBeNull = true)]
        public int? SoilIon
        {
            get
            {
                return _soilIon;
            }
            set
            {
                if (_soilIon != value)
                {
                    this.NotifyPropertyChanging("SoilIon");
                    _soilIon = value;
                    this.NotifyPropertyChanged("SoilIon");
                }
            }
        }

        /// <summary>
        /// 土壤肥力
        /// </summary>
        [Column(Name = "SoilFertility", CanBeNull = true)]
        public float? SoilFertility
        {
            get
            {
                return _soilFertility;
            }
            set
            {
                if (_soilFertility != value)
                {
                    this.NotifyPropertyChanging("SoilFertility");
                    _soilFertility = value;
                    this.NotifyPropertyChanged("SoilFertility");
                }
            }
        }

        /// <summary>
        /// 页面温度
        /// </summary>
        [Column(Name = "LeafTemp", CanBeNull = true)]
        public float? LeafTemp
        {
            get
            {
                return _leafTemp;
            }
            set
            {
                if (_leafTemp != value)
                {
                    this.NotifyPropertyChanging("LeafTemp");
                    _leafTemp = value;
                    this.NotifyPropertyChanged("LeafTemp");
                }
            }
        }

        /// <summary>
        /// 采集点的经度
        /// </summary>
        [Column(Name = "CollectionLongitude", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string CollectionLongitude
        {
            get
            {
                return _collectionLongitude;
            }
            set
            {
                if (_collectionLongitude != value)
                {
                    this.NotifyPropertyChanging("CollectionLongitude");
                    _collectionLongitude = value;
                    this.NotifyPropertyChanged("CollectionLongitude");
                }
            }
        }

        /// <summary>
        /// 采集点的纬度
        /// </summary>
        [Column(Name = "CollectionLatitude", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string CollectionLatitude
        {
            get
            {
                return _collectionLatitude;
            }
            set
            {
                if (_collectionLatitude != value)
                {
                    this.NotifyPropertyChanging("CollectionLatitude");
                    _collectionLatitude = value;
                    this.NotifyPropertyChanged("CollectionLatitude");
                }
            }
        }

        /// <summary>
        /// 采集任务的采集点ID
        /// 由服务器返回结果
        /// </summary>
        [Column(Name = "CollectionPointID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? CollectionPointID
        {
            get
            {
                return _collectionPointID;
            }
            set
            {
                if (_collectionPointID != value)
                {
                    this.NotifyPropertyChanging("CollectionPointID");
                    _collectionPointID = value;
                    this.NotifyPropertyChanged("CollectionPointID");
                }
            }
        }

        /// <summary>
        /// 异常的ID
        /// </summary>
        [Column(Name = "ExceptionID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? ExceptionID
        {
            get
            {
                return _exceptionId;
            }
            set
            {
                if (_exceptionId != value)
                {
                    this.NotifyPropertyChanging("ExceptionID");
                    _exceptionId = value;
                    this.NotifyPropertyChanged("ExceptionID");
                }
            }
        }

        /// <summary>
        /// 异常内容
        /// </summary>
        [Column(Name = "Exception", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                if (_exception != value)
                {
                    this.NotifyPropertyChanging("Exception");
                    _exception = value;
                    this.NotifyPropertyChanged("Exception");
                }
            }
        }

        /// <summary>
        /// 备注内容
        /// </summary>
        [Column(Name = "Remark", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                if (_remark != value)
                {
                    this.NotifyPropertyChanging("Remark");
                    _remark = value;
                    this.NotifyPropertyChanged("Remark");
                }
            }
        }

        /// <summary>
        /// 采集结果的病害列表
        /// </summary>
        public List<CollectionTaskResultDisease> CollectionResultDiseaseList
        {
            get
            {
                return _collectionResultDiseaseList;
            }
            set
            {
                if (_collectionResultDiseaseList != value)
                {
                    _collectionResultDiseaseList = value;
                }
            }
        }

        /// <summary>
        /// 采集结果的虫害列表
        /// </summary>
        public List<CollectionTaskResultPest> CollectionResultPestList
        {
            get
            {
                return _collectionResultPestList;
            }
            set
            {
                if (_collectionResultPestList != value)
                {
                    _collectionResultPestList = value;
                }
            }
        }

        /// <summary>
        /// 采集结果的草害列表
        /// </summary>
        public List<CollectionTaskResultWeed> CollectionResultWeedList
        {
            get
            {
                return _collectionResultWeedList;
            }
            set
            {
                if (_collectionResultWeedList != value)
                {
                    _collectionResultWeedList = value;
                }
            }
        }

        #endregion

    }

}
