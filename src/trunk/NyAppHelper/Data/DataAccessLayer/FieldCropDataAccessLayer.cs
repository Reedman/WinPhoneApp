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
    /// 农田作物信息数据操作类
    /// </summary>
    public class FieldCropDataAccessLayer : IDataAccessible<FieldCrop>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);

        public ObservableCollection<FieldCrop> GetAll()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<FieldCrop> GetAll(int farmlandId)
        {
            ObservableCollection<FieldCrop> fieldCrops = new ObservableCollection<FieldCrop>();
            var fieldCropList = _dataContextInstance.FieldCropTable.Where(c => c.FarmlandID == farmlandId).ToList<FieldCrop>();
            if (fieldCropList != null && fieldCropList.Count > 0)
            {
                foreach (var fieldCrop in fieldCropList)
                {
                    fieldCrops.Add(fieldCrop);
                }
            }
            return fieldCrops;
        }

        /// <summary>
        /// 添加一条农田的作物信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Add(FieldCrop t)
        {
            bool result = false;
            try
            {
                _dataContextInstance.FieldCropTable.InsertOnSubmit(t);
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
        /// 添加一个农田的所有种植作物
        /// </summary>
        /// <param name="fieldCrops"></param>
        /// <returns></returns>
        public bool AddAll(ObservableCollection<FieldCrop> fieldCrops)
        {
            bool result = false;
            if (fieldCrops != null)
            {
                try
                {
                    _dataContextInstance.FieldCropTable.InsertAllOnSubmit<FieldCrop>(fieldCrops);
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

        /// <summary>
        /// 更新一个作物信息
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update(FieldCrop t)
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
        /// 更新所有作物信息，
        /// 如果存在则更新，如果不存在，则插入
        /// </summary>
        /// <param name="fieldCrops"></param>
        /// <returns></returns>
        public bool UpdateAll(ObservableCollection<FieldCrop> fieldCrops)
        {
            bool result = false;
            try
            {
                foreach (var crop in fieldCrops)
                {
                    if (crop.UniqueId > 0)
                    {
                        var oldCrop = _dataContextInstance.FieldCropTable.FirstOrDefault(c => c.UniqueId == crop.UniqueId);
                        oldCrop.PlantingCropId = crop.PlantingCropId;
                        oldCrop.IsCashCrop = crop.IsCashCrop;
                        oldCrop.HarvestDate = crop.HarvestDate;
                        oldCrop.PlantingCropName = crop.PlantingCropName;
                        oldCrop.PlantingMethod = crop.PlantingMethod;
                        oldCrop.PlantingDate = crop.PlantingDate;
                        Update(oldCrop);
                    }
                    else
                    {
                        Add(crop);
                    }
                }
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public bool Remove(FieldCrop t)
        {
            throw new NotImplementedException();
        }


        public bool RemoveAll(ObservableCollection<FieldCrop> t)
        {
            throw new NotImplementedException();
        }
    }
}
