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
    /// 照片的数据访问层
    /// </summary>
    public class PhotoDataAccessLayer : IDataAccessible<Photo>
    {

        private static AppDBDataContext _dataContextInstance = new AppDBDataContext(AppSettings.DBConStr);

        public ObservableCollection<Photo> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Add(Photo t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.PhotoTable.InsertOnSubmit(t);
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

        public bool Add(int id, Photo t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    t.ID = id;
                    _dataContextInstance.PhotoTable.InsertOnSubmit(t);
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

        public bool AddAll(ObservableCollection<Photo> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    _dataContextInstance.PhotoTable.InsertAllOnSubmit<Photo>(t);
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

        public bool AddAll(int id, ObservableCollection<Photo> t)
        {
            bool result = false;
            if (t != null)
            {
                try
                {
                    for (int i = 0; i < t.Count; i++)
                    {
                        t[i].ID = id;
                    }
                    _dataContextInstance.PhotoTable.InsertAllOnSubmit<Photo>(t);
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

        public bool Update(Photo t)
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

        public bool Remove(Photo t)
        {
            bool result = false;
            try
            {
                _dataContextInstance.PhotoTable.DeleteOnSubmit(t);
                _dataContextInstance.SubmitChanges();
                result = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public bool Remove(int id)
        {
            bool result = false;
            try
            {
                var photo = _dataContextInstance.PhotoTable.FirstOrDefault(p => p.UniqueId == id);
                if (photo != null)
                {
                    _dataContextInstance.PhotoTable.DeleteOnSubmit(photo);
                    _dataContextInstance.SubmitChanges();
                    result = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }


        public bool RemoveAll(int tempId)
        {
            bool result = false;

            try
            {
                if (tempId>0)
                {
                    var imageList = _dataContextInstance.PhotoTable.Where(p => p.ID == tempId).ToList<Photo>();
                    _dataContextInstance.PhotoTable.DeleteAllOnSubmit<Photo>(imageList);
                    _dataContextInstance.SubmitChanges();
                    result = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public bool RemoveAll(ObservableCollection<Photo> t)
        {
            bool result = false;

            try
            {
                if (t != null && t.Count > 0)
                {
                    _dataContextInstance.PhotoTable.DeleteAllOnSubmit<Photo>(t);
                    _dataContextInstance.SubmitChanges();
                    result = true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

    }
}
