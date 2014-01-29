using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NyAppHelper.Data;
using System.Data.Linq.Mapping;

namespace NyAppHelper.Model
{
    /// <summary>
    /// 拍照图片
    /// </summary>
    [Table(Name="PhotoInfo")]
    public class Photo : SQLLiteTableBase
    {
        private int _uniqueId;
        private int? _id;
        private String _localUri;
        private String _localThumbnailUri;
        private String _thumbnailDetailUri;
        private String _previewDetailUri;
        private DateTime _createDate;
        private int? _extensionField1;
        public readonly String Tag = Guid.NewGuid().ToString();


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
        /// 采集结果的ID
        /// </summary>
        [Column(Name = "ID", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? ID
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
        /// Int型的扩展字段1
        /// 在采集任务中，保存病虫害的种类
        /// </summary>
        [Column(Name = "ExtensionField1", CanBeNull = true, AutoSync = AutoSync.Default)]
        public int? ExtensionField1
        {
            get
            {
                return _extensionField1;
            }
            set
            {
                if (_extensionField1 != value)
                {
                    this.NotifyPropertyChanging("ExtensionField1");
                    _extensionField1 = value;
                    this.NotifyPropertyChanged("ExtensionField1");
                }
            }
        }

        /// <summary>
        /// image address on local device
        /// </summary>
        [Column(Name = "LocalUri", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string LocalUri
        {
            get
            {
                return _localUri;
            }
            set
            {
                if (_localUri != value)
                {
                    this.NotifyPropertyChanging("LocalUri");
                    _localUri = value;
                    this.NotifyPropertyChanged("LocalUri");
                }
            }
        }

        /// <summary>
        /// local storeage address for thumbnail image
        /// </summary>
        [Column(Name = "LocalThumbnailUri", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string LocalThumbnailUri
        {
            get
            {
                return _localThumbnailUri;
            }
            set
            {
                if (_localThumbnailUri != value)
                {
                    this.NotifyPropertyChanging("LocalThumbnailUri");
                    _localThumbnailUri = value;
                    this.NotifyPropertyChanged("LocalThumbnailUri");
                }
            }
        }


        /// <summary>
        /// local storeage detail path for Databinding
        /// </summary>
        [Column(Name = "ThumbnailDetailUri", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string ThumbnailDetailUri
        {
            get
            {
                return _thumbnailDetailUri;
            }
            set
            {
                if (_thumbnailDetailUri != value)
                {
                    this.NotifyPropertyChanging("ThumbnailDetailUri");
                    _thumbnailDetailUri = value;
                    this.NotifyPropertyChanged("ThumbnailDetailUri");
                }
            }
        }

        /// <summary>
        /// local storeage detail path for Databinding
        /// </summary>
        [Column(Name = "PreviewDetailUri", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string PreviewDetailUri
        {
            get
            {
                return _previewDetailUri;
            }
            set
            {
                if (_previewDetailUri != value)
                {
                    this.NotifyPropertyChanging("PreviewDetailUri");
                    _previewDetailUri = value;
                    this.NotifyPropertyChanged("PreviewDetailUri");
                }
            }
        }

        /// <summary>
        /// Image snap date
        /// </summary>
        [Column(Name = "SnapDate", DbType = "DATETIME NOT NULL", CanBeNull = false)]
        public DateTime CreatedTime
        {
            get
            {
                return _createDate;
            }
            set
            {
                if (_createDate != value)
                {
                    this.NotifyPropertyChanging("CreatedTime");
                    _createDate = value;
                    this.NotifyPropertyChanged("CreatedTime");
                }
            }
        }


    }
}
