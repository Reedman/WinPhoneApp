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
    /// 种植种类信息，
    /// 来源于在线知识库
    /// </summary>
    [Table(Name="CropInfo")]
    public class Crop:SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _cropId;
        private string _cropName;
        private int? _isCashCrop;
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
        /// 种植作物的ID
        /// </summary>
        [Column(Name = "CropId", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int CropID
        {
            get
            {
                return _cropId;
            }
            set
            {
                if (_cropId != value)
                {
                    this.NotifyPropertyChanging("CropId");
                    _cropId = value;
                    this.NotifyPropertyChanged("CropId");
                }
            }
        }

        /// <summary>
        /// 种植作物的名称
        /// </summary>
        [Column(Name = "CropName", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
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
        /// 是否属于经济作物
        /// 1表示是属于经济作物
        /// </summary>
        [Column(Name = "IsCashCrop", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? IsCashCrop
        {
            get
            {
                return _isCashCrop;
            }
            set
            {
                if (_isCashCrop != value)
                {
                    this.NotifyPropertyChanging("IsCashCrop");
                    _isCashCrop = value;
                    this.NotifyPropertyChanged("IsCashCrop");
                }
            }
        }

        /// <summary>
        /// 父种类作物ID
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
