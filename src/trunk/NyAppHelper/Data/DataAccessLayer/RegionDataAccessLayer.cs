using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Collections.ObjectModel;
using Microsoft.Phone.Data.Linq;
using NyAppHelper.Model;

namespace NyAppHelper.Data
{
    /// <summary>
    /// 行政区域的数据访问层
    /// </summary>
    public class RegionDataAccessLayer:IDataAccessible<Region>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);

        /// <summary>
        ///获得所有的行政区域
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Region> GetAll()
        {
            ObservableCollection<Region> regions = new ObservableCollection<Region>();
            var regionList = _dataContextInstance.RegionTable.Select(r => r).ToList();
            if (regionList != null && regionList.Count() > 0)
            {
                regions = new ObservableCollection<Region>(regionList);
            }
            return regions;
        }

        public bool Add(Region t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加所有的行政区域
        /// </summary>
        /// <param name="regions"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<Region> regions)
        {
            bool result = false;
            if (regions != null)
            {
                try
                {
                    _dataContextInstance.RegionTable.InsertAllOnSubmit<Region>(regions);
                    _dataContextInstance.SubmitChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        public bool Update(Region t)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Region t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除本地所有的行政区域信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool RemoveAll(ObservableCollection<Region> t)
        {
            bool result = false;
            try
            {
                _dataContextInstance.RegionTable.DeleteAllOnSubmit<Region>(t);
                _dataContextInstance.SubmitChanges();
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
    }

}
