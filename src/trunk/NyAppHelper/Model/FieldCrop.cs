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
    /// 农田作物的种植方式
    /// </summary>
    public enum PlantingMethodEnum : int
    {
        /// <summary>
        /// 无信息
        /// </summary>
        None=0,
        /// <summary>
        /// 机械种植
        /// </summary>
        Machine = 1,
        /// <summary>
        /// 手工种植
        /// </summary>
        Manual = 2,
    }

    /// <summary>
    /// 种植方式详情
    /// </summary>
    public class FieldPlantingMethod
    {
        /// <summary>
        /// 种植方式的字典对象
        /// </summary>
        public static readonly Dictionary<PlantingMethodEnum, string> PlantingMethodDic = new Dictionary<PlantingMethodEnum, string>()
        {
            {PlantingMethodEnum.None,"无信息"},
            {PlantingMethodEnum.Manual,"手工种植"},
            {PlantingMethodEnum.Machine,"机械种植"},
        };
    }

    /// <summary>
    /// 农田种植信息表
    /// </summary>
    [Table(Name = "FieldCropInfo")]
    public class FieldCrop : SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _farmlandId;
        private DateTime _plantingDate;
        private DateTime? _harvestDate;
        private string _plantingCropName;
        private string _imagePath;
        private int _plantingCropId;
        private int _isCashCrop;
        private int? _plantingMethod;
        private int? _plantingArea;

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
        /// Farm的种植作物ID
        /// </summary>
        [Column(Name = "PlantingCropId", CanBeNull = false, AutoSync = AutoSync.Default)]
        public int PlantingCropId
        {
            get
            {
                return _plantingCropId;
            }
            set
            {
                if (_plantingCropId != value)
                {
                    this.NotifyPropertyChanging("PlantingCropId");
                    _plantingCropId = value;
                    this.NotifyPropertyChanged("PlantingCropId");
                }
            }
        }

        /// <summary>
        /// Farm的创建或者更改时间
        /// </summary>
        [Column(Name = "PlantingDate", DbType = "DATETIME", CanBeNull = true, AutoSync = AutoSync.Default)]
        public DateTime PlantingDate
        {
            get
            {
                return _plantingDate;
            }
            set
            {
                if (_plantingDate != value)
                {
                    this.NotifyPropertyChanging("PlantingDate");
                    _plantingDate = value;
                    this.NotifyPropertyChanged("PlantingDate");
                }
            }
        }


        /// <summary>
        /// Farm的收割时间
        /// </summary>
        [Column(Name = "HarvestDate", DbType = "DATETIME", CanBeNull = true, AutoSync = AutoSync.Default)]
        public DateTime? HarvestDate
        {
            get
            {
                return _harvestDate;
            }
            set
            {
                if (_harvestDate != value)
                {
                    this.NotifyPropertyChanging("HarvestDate");
                    _harvestDate = value;
                    this.NotifyPropertyChanged("HarvestDate");
                }
            }
        }

        /// <summary>
        /// Farm的种植种类
        /// </summary>
        [Column(Name = "PlantingCropName", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string PlantingCropName
        {
            get
            {
                return _plantingCropName;
            }
            set
            {
                if (_plantingCropName != value)
                {
                    this.NotifyPropertyChanging("PlantingCropName");
                    _plantingCropName = value;
                    this.NotifyPropertyChanged("PlantingCropName");
                }
            }
        }

        /// <summary>
        /// Field的图片
        /// </summary>
        [Column(Name = "ImagePath", CanBeNull = true, AutoSync = AutoSync.Default)]
        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                if (_imagePath != value)
                {
                    this.NotifyPropertyChanging("ImagePath");
                    _imagePath = value;
                    this.NotifyPropertyChanged("ImagePath");
                }
            }
        }

        /// <summary>
        /// 是否是经济作物
        /// </summary>
        [Column(Name = "IsCashCrop", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int IsCashCrop
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
        /// 作物的种植面积
        /// </summary>
        [Column(Name = "PlantingArea", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? PlantingArea
        {
            get
            {
                return _plantingArea;
            }
            set
            {
                if (_plantingArea != value)
                {
                    this.NotifyPropertyChanging("PlantingArea");
                    _plantingArea = value;
                    this.NotifyPropertyChanged("PlantingArea");
                }
            }
        }

        #endregion

    }

}
