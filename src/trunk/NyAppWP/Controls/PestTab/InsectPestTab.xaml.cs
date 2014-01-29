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
    public partial class InsectPestTab : UserControl, ITabInfoCanBeSaved,ITabInfoCanBeCaptured
    {

        private CollectionTaskResultPest _result;

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

        /// <summary>
        /// 采集详情页虫害Tab
        /// </summary>
        public InsectPestTab()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 采集详情页虫害Tab
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="pestName"></param>
        public InsectPestTab(string pestName, CollectionTaskResultPest result)
        {
            InitializeComponent();
            txtPestTypeName.Text = pestName;
            _result = result;
            ImageList.DataContext = ImgDataContext;
        }

        public bool IsInputValueVailed()
        {
            if (String.IsNullOrEmpty(txtAdultPests.Text) || String.IsNullOrEmpty(txtBadStems.Text)
                || String.IsNullOrEmpty(txtDeadHeartStems.Text) || String.IsNullOrEmpty(txtFoundPestEggs.Text)
                || String.IsNullOrEmpty(txtFoundPests.Text)
                || String.IsNullOrEmpty(txtTotalHoles.Text) || String.IsNullOrEmpty(txtTotalPlant.Text))
            {
                MessageBox.Show("请填写完整");
                return false;
            }
            else
            {
                _result.AdultPests = int.Parse(txtAdultPests.Text);
                _result.BadStems= int.Parse(txtBadStems.Text);
                _result.DeadHeartStems = int.Parse(txtDeadHeartStems.Text);
                _result.FoundPestEggs = int.Parse(txtFoundPestEggs.Text);
                _result.FoundPests = int.Parse(txtFoundPests.Text);
                _result.InvestigatedHoles = int.Parse(txtTotalHoles.Text);
                _result.InvestigatedStems = int.Parse(txtTotalPlant.Text);
                return true;
            }
        }

        public int PhtotoListIndex
        {
            get { return _result.PestID; }
        }


        public int TypeValue
        {
            get { return (int)PestType.InssectPest; }
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
