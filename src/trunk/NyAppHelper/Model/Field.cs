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
    /// 农户的农田信息
    /// </summary>
    [Table(Name = "FieldInfo")]
    public class Field:SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _farmlandId;
        private string _farmlandName;
        private DateTime _createdDate;
        private float _area;
        private int _ownerId;
        private string _geo;
        private int _regionId;
        private string _regionName;

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
        /// Farm Id
        /// </summary>
        [Column(Name = "FarmlandID", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int FarmlandID
        {
            get
            {
                return _farmlandId;
            }
            set
            {
                if (_farmlandId != value)
                {
                    this.NotifyPropertyChanging("FarmlandID");
                    _farmlandId = value;
                    this.NotifyPropertyChanged("FarmlandID");
                }
            }
        }


        /// <summary>
        /// Farm的名字
        /// </summary>
        [Column(Name = "FarmlandName", CanBeNull = true, AutoSync = AutoSync.Default)]
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
        /// Farm的创建时间
        /// </summary>
        [Column(Name = "CreatedDate", DbType = "DATETIME", CanBeNull = true, AutoSync = AutoSync.Default)]
        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                if (_createdDate != value)
                {
                    this.NotifyPropertyChanging("CreatedDate");
                    _createdDate = value;
                    this.NotifyPropertyChanged("CreatedDate");
                }
            }
        }

        
        /// <summary>
        ///农田的面积
        /// </summary>
        [Column(Name = "Area", CanBeNull = true, AutoSync = AutoSync.Default)]
        public float Area
        {
            get
            {
                return _area;
            }
            set
            {
                if (_area != value)
                {
                    this.NotifyPropertyChanging("Area");
                    _area = value;
                    this.NotifyPropertyChanged("Area");
                }
            }
        }

        /// <summary>
        /// 用户的Id
        /// </summary>
        [Column(Name = "OwnerID", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int OwnerID
        {
            get
            {
                return _ownerId;
            }
            set
            {
                if (_ownerId != value)
                {
                    this.NotifyPropertyChanging("OwnerID");
                    _ownerId = value;
                    this.NotifyPropertyChanged("OwnerID");
                }
            }
        }

        /// <summary>
        /// Field的GPS点坐标集合
        /// </summary>
        [Column(Name = "Geo", CanBeNull = true, AutoSync = AutoSync.Default)]
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
        /// 行政区域的ID
        /// </summary>
        [Column(Name = "RegionID", CanBeNull = false, AutoSync = AutoSync.Default)]
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
        /// 行政区域的名字
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
        /// 种植作物列表
        /// 本字段不保存在数据库中
        /// </summary>
        public List<FieldCrop> Plantings
        { get; set; }

        #endregion

    }
}
