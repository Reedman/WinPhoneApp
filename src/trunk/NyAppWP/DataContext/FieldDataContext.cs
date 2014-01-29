using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NyAppHelper;
using NyAppHelper.Model;
using NyAppHelper.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NyAppHelper.Http;
using System.Globalization;

namespace NyAppWP.DataContext
{
    /// <summary>
    /// 农户田块的DataContext
    /// </summary>
    public class FieldDataContext
    {
        private FieldService _service;
        private FieldDataAccessLayer _dataAccessLayer;
        private FieldCropDataAccessLayer _cropDataAccessLayer;

        
        public FieldDataContext()
        {
            _service = new FieldService();
            _dataAccessLayer = new FieldDataAccessLayer();
            _cropDataAccessLayer = new FieldCropDataAccessLayer();
        }

        public delegate void InitFieldDataCallBackEventHandler(ObservableCollection<FieldWarpper> data);

        public async void GetFieldDataContext(InitFieldDataCallBackEventHandler callback)
        {
            var ownerId = int.Parse(_service.UserId);
            ObservableCollection<FieldWarpper> contextData = new ObservableCollection<FieldWarpper>();
            ObservableCollection<Field> fields = _dataAccessLayer.GetAll(ownerId);
            if (fields == null)
            {
                fields = await _service.GetFields();
                _dataAccessLayer.AddAll(fields);
            }
            ObservableCollection<FieldCrop> fieldCrops = null;
            foreach (var field in fields)
            {
                fieldCrops = _cropDataAccessLayer.GetAll(field.FarmlandID);
                if (field.Plantings != null)
                {
                    if (fieldCrops.Count <= 0)
                    {
                        fieldCrops = new ObservableCollection<FieldCrop>(field.Plantings);
                        for (int i = 0; i < fieldCrops.Count; i++)
                        {
                            fieldCrops[i].FarmlandID = field.FarmlandID;
                        }
                        _cropDataAccessLayer.AddAll(fieldCrops);
                    }
                }
                else
                {
                    field.Plantings = fieldCrops.ToList<FieldCrop>();
                }
                contextData.Add(new FieldWarpper(field));
            }
            callback(contextData);
        }

    }

    /// <summary>
    /// 农田信息的包装类
    /// </summary>
    public class FieldWarpper : Field
    {
        private string _coverImagePath;
        private string _plantingCropName;
        private DateTime _plantingDate;

        public FieldWarpper(Field field)
        {
            this.UniqueId = field.UniqueId;
            this.Area = field.Area;
            this.CreatedDate = field.CreatedDate;
            this.FarmlandID = field.FarmlandID;
            this.FarmlandName = field.FarmlandName;
            this.Geo = field.Geo;
            this.OwnerID = field.OwnerID;
            this.Plantings = field.Plantings;
            this.RegionID = field.RegionID;
            this.RegionName = field.RegionName;
        }

        public string CoverImagePath
        {
            get
            {
                _coverImagePath = this.Plantings.OrderByDescending(c=>c.PlantingDate).Select(c => c.ImagePath).FirstOrDefault<string>();
                if (String.IsNullOrEmpty(_coverImagePath))
                {
                    _coverImagePath = AppSettings.DefaultFieldImagePath;
                }
                return _coverImagePath;
            }
            set
            {
                if (_coverImagePath != value)
                {
                    _coverImagePath = value;
                    NotifyPropertyChanged("CoverImagePath");
                }
            }
        }

        public string PlantingCropName
        {
            get
            {
                _plantingCropName = this.Plantings.OrderByDescending(c => c.PlantingDate).Select(c => c.PlantingCropName).FirstOrDefault<string>();
                return _plantingCropName;
            }
            set
            {
                if (_plantingCropName != value)
                {
                    _plantingCropName = value;
                    NotifyPropertyChanged("PlantingCropName");
                }
            }
        }

        public DateTime PlantingDate
        {
            get
            {
                _plantingDate = this.Plantings.OrderByDescending(c => c.PlantingDate).Select(c => c.PlantingDate).FirstOrDefault<DateTime>();
                return _plantingDate;
            }
            set
            {
                if (_plantingDate != value)
                {
                    _plantingDate = value;
                    NotifyPropertyChanged("PlantingDate");
                }
            }
        }

    }

}

