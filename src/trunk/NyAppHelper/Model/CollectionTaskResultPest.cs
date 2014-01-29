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
    /// 采集结果之虫害
    /// </summary>
    [Table(Name = "CollectionTaskResultPestInfo")]
    public class CollectionTaskResultPest:SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _taskId;
        private int? _collectionResultID;
        private int? _tabID;
        private int? _pestCollectionResultID;
        private int _relatedResultID;
        private int _pestID;
        private int? _investigatedHoles;
        private int? _foundPests;
        private int? _investigatedStems;
        private int? _foundPestEggs;
        private int? _deadHeartStems;
        private int? _badStems;
        private int? _adultPests;   

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
        /// 采集结果未上传时，采集结果的的UniqueId
        ///本地保存时，相应的采集结果的UniqueId
        /// 已上传以后，保持不变
        /// </summary>
        [Column(Name = "RelatedResultID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int RelatedResultID
        {
            get
            {
                return _relatedResultID;
            }
            set
            {
                if (_relatedResultID != value)
                {
                    this.NotifyPropertyChanging("RelatedResultID");
                    _relatedResultID = value;
                    this.NotifyPropertyChanged("RelatedResultID");
                }
            }
        }

        /// <summary>
        /// 采集结果的ID
        /// 未上传的结果为NULL
        /// 已上传以后，保存数据库返回的结果
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
        /// 采集结果Tab的ID
        /// 由服务器返回值
        /// </summary>
        [Column(Name = "TabID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? TabID
        {
            get
            {
                return _tabID;
            }
            set
            {
                if (_tabID != value)
                {
                    this.NotifyPropertyChanging("TabID");
                    _tabID = value;
                    this.NotifyPropertyChanged("TabID");
                }
            }
        }

        /// <summary>
        /// 采集结果虫害的ID
        /// 由服务器返回值
        /// </summary>
        [Column(Name = "PestCollectionResultID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? PestCollectionResultID
        {
            get
            {
                return _pestCollectionResultID;
            }
            set
            {
                if (_pestCollectionResultID != value)
                {
                    this.NotifyPropertyChanging("PestCollectionResultID");
                    _pestCollectionResultID = value;
                    this.NotifyPropertyChanged("PestCollectionResultID");
                }
            }
        }

        /// <summary>
        /// 病虫害种类中虫害的ID
        /// </summary>
        [Column(Name = "PestID", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int PestID
        {
            get
            {
                return _pestID;
            }
            set
            {
                if (_pestID != value)
                {
                    this.NotifyPropertyChanging("PestID");
                    _pestID = value;
                    this.NotifyPropertyChanged("PestID");
                }
            }
        }

        /// <summary>
        /// 调查总穴数
        /// </summary>
        [Column(Name = "InvestigatedHoles", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? InvestigatedHoles
        {
            get
            {
                return _investigatedHoles;
            }
            set
            {
                if (_investigatedHoles != value)
                {
                    this.NotifyPropertyChanging("InvestigatedHoles");
                    _investigatedHoles = value;
                    this.NotifyPropertyChanged("InvestigatedHoles");
                }
            }
        }

        /// <summary>
        /// 查获虫数
        /// </summary>
        [Column(Name = "FoundPests", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? FoundPests          
        {
            get
            {
                return _foundPests;
            }
            set
            {
                if (_foundPests != value)
                {
                    this.NotifyPropertyChanging("FoundPests");
                    _foundPests = value;
                    this.NotifyPropertyChanged("FoundPests");
                }
            }
        }

        /// <summary>
        /// 调查总株数
        /// </summary>
        [Column(Name = "InvestigatedStems", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? InvestigatedStems
        {
            get
            {
                return _investigatedStems;
            }
            set
            {
                if (_investigatedStems != value)
                {
                    this.NotifyPropertyChanging("InvestigatedStems");
                    _investigatedStems = value;
                    this.NotifyPropertyChanged("InvestigatedStems");
                }
            }
        }

        /// <summary>
        /// 查获卵数
        /// </summary>
        [Column(Name = "FoundPestEggs", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? FoundPestEggs
        {
            get
            {
                return _foundPestEggs;
            }
            set
            {
                if (_foundPestEggs != value)
                {
                    this.NotifyPropertyChanging("FoundPestEggs");
                    _foundPestEggs = value;
                    this.NotifyPropertyChanged("FoundPestEggs");
                }
            }
        }

        /// <summary>
        /// 枯心数
        /// </summary>
        [Column(Name = "DeadHeartStems", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? DeadHeartStems
        {
            get
            {
                return _deadHeartStems;
            }
            set
            {
                if (_deadHeartStems != value)
                {
                    this.NotifyPropertyChanging("DeadHeartStems");
                    _deadHeartStems = value;
                    this.NotifyPropertyChanged("DeadHeartStems");
                }
            }
        }

        /// <summary>
        /// 鞘株数
        /// </summary>
        [Column(Name = "BadStems", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? BadStems
        {
            get
            {
                return _badStems;
            }
            set
            {
                if (_badStems != value)
                {
                    this.NotifyPropertyChanging("BadStems");
                    _badStems = value;
                    this.NotifyPropertyChanged("BadStems");
                }
            }
        }

        /// <summary>
        /// 调查总株数
        /// </summary>
        [Column(Name = "AdultPests", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? AdultPests
        {
            get
            {
                return _adultPests;
            }
            set
            {
                if (_adultPests != value)
                {
                    this.NotifyPropertyChanging("AdultPests");
                    _adultPests = value;
                    this.NotifyPropertyChanged("AdultPests");
                }
            }
        }

        #endregion

    }
}
