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
    /// 采集任务自然环境
    /// </summary>
    [Table(Name = "CollectionTaskNatureInfo")]
    public class CollectionTaskNature : SQLLiteTableBase
    {

        #region 私有变量

        private int _uniqueId;
        private int _id;
        private string _name;
        private string _displayName;

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
        /// 采集要求自然环境的名称
        /// </summary>
        [Column(Name = "Name", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    this.NotifyPropertyChanging("Name");
                    _name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }


        /// <summary>
        /// 采集要求自然环境的显示名称
        /// </summary>
        [Column(Name = "DisplayName", CanBeNull = false, AutoSync = AutoSync.Default)]
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName != value)
                {
                    this.NotifyPropertyChanging("DisplayName");
                    _displayName = value;
                    this.NotifyPropertyChanged("DisplayName");
                }
            }
        }


        #endregion

    }


}
