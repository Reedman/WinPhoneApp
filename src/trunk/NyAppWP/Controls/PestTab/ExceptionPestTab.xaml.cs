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
    public partial class ExceptionPestTab : UserControl, ITabInfoCanBeSaved, ITabInfoCanBeCaptured
    {

        private TaskException _taskException;

        public ImageListViewDataContext ImgDataContext = new ImageListViewDataContext();

        public event EventHandler EventForMoveToGalleryPage;

        public ExceptionPestTab()
        {
            InitializeComponent();
        }

        public ExceptionPestTab(TaskException taskException)
        {
            _taskException = taskException;
            InitializeComponent();
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

        private void btnRemove_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var item = (PivotItem)this.Parent;
            var pivot = (Pivot)item.Parent;
            int currentIndex = pivot.SelectedIndex;
            pivot.Items.RemoveAt(currentIndex);
            pivot.SelectedIndex = 0;
        }

        private void txtExceptionDesc_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Text = String.Empty;
        }

        private void txtExceptionDesc_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (String.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "请输入文字描述";
            }
        }

        public bool IsInputValueVailed()
        {
            if (String.Equals("请输入文字描述", txtExceptionDesc.Text, StringComparison.InvariantCulture))
            {
                return false;
            }
            else
            {
                _taskException.Exception = txtExceptionDesc.Text;
                return true;
            }
        }

        public int PhtotoListIndex
        {
            get { return _taskException.ExceptionID.Value; }
        }


        public int TypeValue
        {
            get { return (int)PestType.Exception; }
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
