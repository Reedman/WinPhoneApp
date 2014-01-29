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
    public class CropDataAccessLayer:IDataAccessible<Crop>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);

        /// <summary>
        /// 加载本地的种植种类知识库
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Crop> GetAll()
        {
            ObservableCollection<Crop> crops = new ObservableCollection<Crop>();
            var cropList = _dataContextInstance.CropTable.Select(c => c).ToList();
            if (cropList != null && cropList.Count > 0)
            {
                foreach (var crop in cropList)
                {
                    crops.Add(crop);
                }
            }
            return crops;
        }

        /// <summary>
        /// 添加全部的种植种类到本地
        /// </summary>
        /// <param name="crops"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<Crop> crops)
        {
            bool result = false;
            if (crops != null)
            {
                try
                {
                    _dataContextInstance.CropTable.InsertAllOnSubmit<Crop>(crops);
                    _dataContextInstance.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        /// <summary>
        /// 添加种植种类
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Add(Crop crop)
        {
            bool result = false;
            if (crop != null)
            {
                try
                {
                    _dataContextInstance.CropTable.InsertOnSubmit(crop);
                    _dataContextInstance.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        /// <summary>
        /// 修改种植种类信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update(Crop crop)
        {
            bool result = false;
            try
            {
                _dataContextInstance.SubmitChanges();
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 删除种植种类信息
        /// </summary>
        /// <param name="crop"></param>
        /// <returns></returns>
        public bool Remove(Crop crop)
        {
            bool result = false;
            try
            {
                _dataContextInstance.CropTable.DeleteOnSubmit(crop);
                _dataContextInstance.SubmitChanges();
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        /// <summary>
        /// 删除这个表的所有本地数据
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool RemoveAll(ObservableCollection<Crop> t)
        {
            bool result = false;
            try
            {
                _dataContextInstance.CropTable.DeleteAllOnSubmit<Crop>(t);
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
