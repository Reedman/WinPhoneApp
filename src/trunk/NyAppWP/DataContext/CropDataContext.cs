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

namespace NyAppWP.DataContext
{
    public class CropDataContext
    {

        private CropDataAccessLayer _dataAccessLayer;
        private CropService _service;

        public delegate void InitCropDataCallBackEventHandler(ObservableCollection<Crop> data);

        public CropDataContext()
        {
            _service = new CropService();
            _dataAccessLayer = new CropDataAccessLayer();
        }

        public async void GetAllCrops(InitCropDataCallBackEventHandler callback)
        {
            var crops = _dataAccessLayer.GetAll();
            if (crops.Count <= 0)
            {
                crops = await _service.GetCropInfo();
                _dataAccessLayer.AddAll(crops);
            }
            callback(crops);
        }

    }
}
