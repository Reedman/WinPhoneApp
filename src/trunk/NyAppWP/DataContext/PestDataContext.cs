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
    public class PestDataContext
    {

        private PestDataAccessLayer _dataAccessLayer;
        private PestService _service;

        public PestDataContext() 
        {
            _dataAccessLayer = new PestDataAccessLayer();
            _service = new PestService();
        }

        public delegate void InitPestDataCallBackEventHandler(ObservableCollection<Pest> data);

        public async void GetAllPests(InitPestDataCallBackEventHandler callback)
        {
            var pests = _dataAccessLayer.GetAll();
            if(pests.Count<=0)
            {
                pests = await _service.GetAllPests();
                _dataAccessLayer.AddAll(pests);
            }
            callback(pests);
        }

        public async Task<ObservableCollection<Pest>> GetAllPests()
        {
            var pests = _dataAccessLayer.GetAll();
            if (pests.Count <= 0)
            {
                pests = await _service.GetAllPests();
                _dataAccessLayer.AddAll(pests);
            }
            return pests;
        }

    }
}
