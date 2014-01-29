using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NyAppWP.DataContext;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NyAppHelper.Data;
using NyAppHelper.Model;
using System.Collections.ObjectModel;
using NyAppWP.Resources;
using NyAppHelper.Http;

namespace NyAppWP.Pages
{
    public partial class FieldList : PhoneApplicationPage
    {
        private FieldDataAccessLayer _dataAccessLayer;
        private FieldDataContext _dataContext;
        private ObservableCollection<FieldWarpper> _data;
        private FieldService _service;

        public FieldList()
        {
            InitializeComponent();
            _service = new FieldService();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = AppResources.FarmerFieldLoadingTxt;
            _dataAccessLayer = new FieldDataAccessLayer();
            _dataContext = new FieldDataContext();
            _dataContext.GetFieldDataContext((data) =>
            {
                _data = data;
                listFarm.ItemsSource = _data;
                SystemTray.ProgressIndicator.Text = AppResources.FarmerFieldLoadedComplementTxt;
                SetProgressIndicator(false);
            });
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var image = (Image)sender;
            var selectedGrid = (Grid)((Grid)((Image)sender).Parent).Parent;
            var moreInfoTab = (Grid)selectedGrid.Children[1];
            if (moreInfoTab.Visibility == Visibility.Collapsed)
            {
                image.Source = new BitmapImage(new Uri(@"/Assets/Farmer/more-icon-selected.png", UriKind.Relative));
                moreInfoTab.Visibility = Visibility.Visible;
            }
            else
            {
                image.Source = new BitmapImage(new Uri(@"/Assets/Farmer/more-icon.png", UriKind.Relative));
                moreInfoTab.Visibility = Visibility.Collapsed;
            }
        }

        private async void btnDelete_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var fieldId = int.Parse(((Button)sender).Tag.ToString());
            var field = _dataAccessLayer.Get(fieldId);
            if (await _service.Remove(field))
            {
                bool result = _dataAccessLayer.Remove(field);
                if (result)
                {
                    var removeItem = _data.FirstOrDefault(d => d.FarmlandID == field.FarmlandID);
                    _data.Remove(removeItem);
                    listFarm.ItemsSource = _data;
                    MessageBox.Show("删除农田成功");
                }
                else
                {
                    MessageBox.Show("删除农田失败");
                }
            }
            else
            {
                MessageBox.Show("在服务器端删除农田失败");
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Farmer/FieldCreator.xaml", UriKind.Relative));
        }

        private void imgPlant_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var fieldId = int.Parse(((Image)sender).Tag.ToString());
            NavigationService.Navigate(new Uri("/Pages/Farmer/FieldDetail.xaml?fid="+fieldId, UriKind.Relative));
        }

        private static void SetProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        private void FieldListItem_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(e.OriginalSource == sender)
            {

            }
            System.Diagnostics.Debug.WriteLine("FieldListItemTap");
        }


    }
}