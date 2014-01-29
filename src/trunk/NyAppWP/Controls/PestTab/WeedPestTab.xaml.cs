using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NyAppHelper.Model;
using NyAppWP.DataContext;

namespace NyAppWP.Controls
{
    public partial class WeedPestTab : UserControl, ITabInfoCanBeSaved, ITabInfoCanBeCaptured
    {

        private CollectionTaskResultWeed _result;

        public ImageListViewDataContext ImgDataContext = new ImageListViewDataContext();

        public event EventHandler EventForMoveToGalleryPage;


        public void MethodToNavigate(object content, Uri uri)
        {
            var e = new NavigationEventArgs(content, uri);
            if (EventForMoveToGalleryPage != null)
            {
                EventForMoveToGalleryPage(this, e);
            }
        }

        public WeedPestTab()
        {
            InitializeComponent();
        }

        public WeedPestTab(string pestName, CollectionTaskResultWeed result)
        {
            InitializeComponent();
            tbWeedTitle.Text = pestName;
            _result = result;
            ImageList.DataContext = ImgDataContext;
        }

        public bool IsInputValueVailed()
        {
            if (String.IsNullOrEmpty(txtExceptionDesc.Text))
            {
                return false;
            }
            else
            {
                _result.Remark = txtExceptionDesc.Text;
                return true;
            }
        }

        public int PhtotoListIndex
        {
            get { return _result.PestID; }
        }


        public int TypeValue
        {
            get { return (int)PestType.Weed; }
        }

        private void ImageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = 0;
            var photo = (sender as LongListSelector).SelectedItem as Photo;

            if (photo != null)
                index = ImgDataContext.ImageList.IndexOf(photo);

            var uri = String.Format("/Pages/Camera/ImageGalleryPage.xaml?StartIndex={0}", index);
            MethodToNavigate(photo, new Uri(uri, UriKind.Relative));
        }

    }
}
