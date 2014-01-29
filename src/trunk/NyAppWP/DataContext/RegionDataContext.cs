using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NyAppHelper;
using NyAppHelper.Model;
using NyAppHelper.Data;
using System.Collections.ObjectModel;
using NyAppHelper.Http;

namespace NyAppWP.DataContext
{
    public class RegionDataContext
    {

        private RegionDataAccessLayer _dataAccessLayer;
        private RegionService _service;

        public delegate void InitRegionDataCallBackEventHandler(ObservableCollection<Region> data);

        public object SelectedItem;

        public RegionDataContext()
        {
            _service = new RegionService();
            _dataAccessLayer = new RegionDataAccessLayer();
        }

        public async void GetAllRegions(InitRegionDataCallBackEventHandler callback)
        {
            var regions = _dataAccessLayer.GetAll();
            if (regions.Count <= 0)
            {
                regions = await _service.GetRegionInfo();
                _dataAccessLayer.AddAll(regions);
            }
            callback(regions);
        }

    }
}
