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
    /// 采集结果之病害
    /// </summary>
    [Table(Name = "CollectionTaskResultDiseaseInfo")]
    public class CollectionTaskResultDisease:SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _taskId;
        private int? _collectionResultID;
        private int? _tabID;
        private int? _diseaseCollectionResultID;
        private int _relatedResultID;
        private int _pestID;
        private int? _investigatedHoles;
        private int? _harmfulHoles;
        private int? _investigatedStems;
        private int? _sickStems;   

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
        /// 采集结果病害的ID
        /// 由服务器返回值
        /// </summary>
        [Column(Name = "DiseaseCollectionResultID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? DiseaseCollectionResultID
        {
            get
            {
                return _diseaseCollectionResultID;
            }
            set
            {
                if (_diseaseCollectionResultID != value)
                {
                    this.NotifyPropertyChanging("DiseaseCollectionResultID");
                    _diseaseCollectionResultID = value;
                    this.NotifyPropertyChanged("DiseaseCollectionResultID");
                }
            }
        }

        /// <summary>
        /// 病虫害种类中病害的ID
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
        /// 病穴数
        /// </summary>
        [Column(Name = "HarmfulHoles", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? HarmfulHoles
        {
            get
            {
                return _harmfulHoles;
            }
            set
            {
                if (_harmfulHoles != value)
                {
                    this.NotifyPropertyChanging("HarmfulHoles");
                    _harmfulHoles = value;
                    this.NotifyPropertyChanged("HarmfulHoles");
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
        /// 采集结果的ID
        /// </summary>
        [Column(Name = "SickStems", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? SickStems
        {
            get
            {
                return _sickStems;
            }
            set
            {
                if (_sickStems != value)
                {
                    this.NotifyPropertyChanging("SickStems");
                    _sickStems = value;
                    this.NotifyPropertyChanged("SickStems");
                }
            }
        }

        #endregion

    }
}
