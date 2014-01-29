using NyAppHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppWP.DataContext
{
    public class ImageListViewDataContext
    {
        private ObservableCollection<Photo> _imageList;

        public ObservableCollection<Photo> ImageList {
            get { return _imageList; }
        }

        public ImageListViewDataContext()
        {
            _imageList = new ObservableCollection<Photo>();
        }

        public void AddPhoto(Photo p)
        {
            _imageList.Add(p);
        }

        public bool RemovePhoto(Photo p)
        {
            return _imageList.Remove(p);
        }

        public void Clear()
        {
            _imageList.Clear();
        }
    }
}
