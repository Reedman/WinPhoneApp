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
    /// 病虫草害知识库对象
    /// </summary>
    [Table(Name="PestInfo")]
    public class Pest:SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _pestID;
        private string _pestName;
        private int _pestType;
        private int? _cropId;
        private string _collectionRequirement;
        private string _photoRequirement;
        private string _specialRequirement;
        private int? _parentID;
        private DateTime? _latest;

        #endregion


        #region 公有变量

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
        /// 需要采集的病虫害ID
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
        /// 需要采集的病虫害的名称
        /// </summary>
        [Column(Name = "PestName", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string PestName
        {
            get
            {
                return _pestName;
            }
            set
            {
                if (_pestName != value)
                {
                    this.NotifyPropertyChanging("PestName");
                    _pestName = value;
                    this.NotifyPropertyChanged("PestName");
                }
            }
        }

        /// <summary>
        /// 需要采集的病虫害类型
        /// </summary>
        [Column(Name = "PestType", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int PestType
        {
            get
            {
                return _pestType;
            }
            set
            {
                if (_pestType != value)
                {
                    this.NotifyPropertyChanging("PestType");
                    _pestType = value;
                    this.NotifyPropertyChanged("PestType");
                }
            }
        }

        /// <summary>
        /// 需要采集的作物ID
        /// </summary>
        [Column(Name = "CropID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? CropID
        {
            get
            {
                return _cropId;
            }
            set
            {
                if (_cropId != value)
                {
                    this.NotifyPropertyChanging("CropID");
                    _cropId = value;
                    this.NotifyPropertyChanged("CropID");
                }
            }
        }

        /// <summary>
        /// 病虫害采集要求
        /// </summary>
        [Column(Name = "CollectionRequirement", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string CollectionRequirement
        {
            get
            {
                return _collectionRequirement;
            }
            set
            {
                if (_collectionRequirement != null)
                {
                    this.NotifyPropertyChanging("CollectionRequirement");
                    _collectionRequirement = value;
                    this.NotifyPropertyChanged("CollectionRequirement");
                }
            }
        }


        /// <summary>
        /// 病虫害图像采集要求
        /// </summary>
        [Column(Name = "PhotoRequirement", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string PhotoRequirement
        {
            get
            {
                return _photoRequirement;
            }
            set
            {
                if (_photoRequirement != null)
                {
                    this.NotifyPropertyChanging("PhotoRequirement");
                    _photoRequirement = value;
                    this.NotifyPropertyChanged("PhotoRequirement");
                }
            }
        }

        /// <summary>
        /// 病虫害图像特殊采集要求
        /// </summary>
        [Column(Name = "SpecialRequirement", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string SpecialRequirement
        {
            get
            {
                return _specialRequirement;
            }
            set
            {
                if (_specialRequirement != null)
                {
                    this.NotifyPropertyChanging("SpecialRequirement");
                    _specialRequirement = value;
                    this.NotifyPropertyChanged("SpecialRequirement");
                }
            }
        }


        /// <summary>
        /// ParentID
        /// </summary>
        [Column(Name = "ParentID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? ParentID
        {
            get
            {
                return _parentID;
            }
            set
            {
                if (_parentID != value)
                {
                    this.NotifyPropertyChanging("ParentID");
                    _parentID = value;
                    this.NotifyPropertyChanged("ParentID");
                }
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column(Name = "Latest", DbType = "DATETIME", CanBeNull = true, AutoSync = AutoSync.Default)]
        public DateTime? Latest
        {
            get
            {
                return _latest;
            }
            set
            {
                if (_latest != value)
                {
                    this.NotifyPropertyChanging("Latest");
                    _latest = value;
                    this.NotifyPropertyChanged("Latest");
                }
            }
        }

        #endregion




        #endregion

    }


}
