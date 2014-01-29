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
    public partial class DiseasePestTab : UserControl, ITabInfoCanBeSaved, ITabInfoCanBeCaptured
    {

        public event EventHandler EventForMoveToGalleryPage;

        private CollectionTaskResultDisease _result;

        public ImageListViewDataContext ImgDataContext = new ImageListViewDataContext();

        /// <summary>
        /// 采集详情页虫害Tab
        /// </summary>
        public DiseasePestTab()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 采集详情页虫害Tab
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="pestName"></param>
        public DiseasePestTab(string pestName, CollectionTaskResultDisease result)
        {
            InitializeComponent();
            txtPestTypeName.Text = pestName;
            _result = result;
            ImageList.DataContext = ImgDataContext;
        }

        public void MethodToNavigate(object content, Uri uri)
        {
            var e = new NavigationEventArgs(content, uri);
            if (EventForMoveToGalleryPage != null)
            {
                EventForMoveToGalleryPage(this, e);
            }
        }

        public bool IsInputValueVailed()
        {
            if (String.IsNullOrEmpty(txtSickPlant.Text) || String.IsNullOrEmpty(txtSickHoles.Text) || String.IsNullOrEmpty(txtTotalHoles.Text) || String.IsNullOrEmpty(txtTotalPlant.Text))
            {
                return false;
            }
            else
            {
                _result.HarmfulHoles = int.Parse(txtSickHoles.Text);
                _result.InvestigatedHoles = int.Parse(txtTotalHoles.Text);
                _result.InvestigatedStems = int.Parse(txtTotalPlant.Text);
                _result.SickStems = int.Parse(txtSickPlant.Text);
                return true;
            }
        }

        int ITabInfoCanBeCaptured.PhtotoListIndex
        {
            get { return _result.PestID; }
        }


        public int TypeValue
        {
            get { return (int)PestType.Disease; }
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