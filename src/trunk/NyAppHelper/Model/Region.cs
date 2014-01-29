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
    /// 行政区域的信息
    /// </summary>
    [Table(Name = "RegionInfo")]
    public class Region:SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _regionId;
        private string _regionName;
        private int _regionLevel;
        private string _regionCode;
        private int? _parentId;
        private DateTime _latest;

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
        /// 行政区域的ID
        /// </summary>
        [Column(Name = "RegionID", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int RegionID
        {
            get
            {
                return _regionId;
            }
            set
            {
                if (_regionId != value)
                {
                    this.NotifyPropertyChanging("RegionID");
                    _regionId = value;
                    this.NotifyPropertyChanged("RegionID");
                }
            }
        }

        /// <summary>
        /// 行政区域的名称
        /// </summary>
        [Column(Name = "RegionName", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
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
        /// 行政区域的等级
        /// </summary>
        [Column(Name = "RegionLevel", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int RegionLevel
        {
            get
            {
                return _regionLevel;
            }
            set
            {
                if (_regionLevel != value)
                {
                    this.NotifyPropertyChanging("RegionLevel");
                    _regionLevel = value;
                    this.NotifyPropertyChanged("RegionLevel");
                }
            }
        }

        /// <summary>
        /// 行政区域的编码
        /// </summary>
        [Column(Name = "RegionCode", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string RegionCode
        {
            get
            {
                return _regionCode;
            }
            set
            {
                if (_regionCode != value)
                {
                    this.NotifyPropertyChanging("RegionCode");
                    _regionCode = value;
                    this.NotifyPropertyChanged("RegionCode");
                }
            }
        }

        /// <summary>
        /// 父级区域ID，如果当前区域为顶级，则该字段为空
        /// </summary>
        [Column(Name = "ParentID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? ParentID
        {
            get
            {
                return _parentId;
            }
            set
            {
                if (_parentId != value)
                {
                    this.NotifyPropertyChanging("ParentID");
                    _parentId = value;
                    this.NotifyPropertyChanged("ParentID");
                }
            }
        }

        /// <summary>
        /// 最新的数据插入时间
        /// 用于检查数据更新
        /// </summary>
        [Column(Name = "Latest", DbType = "DATETIME", CanBeNull = true, AutoSync = AutoSync.Default)]
        public DateTime Latest
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

    }
}
