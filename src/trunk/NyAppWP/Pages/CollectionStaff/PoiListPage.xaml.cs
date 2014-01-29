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
using System.Collections.ObjectModel;

namespace NyAppWP.Pages
{
    public partial class PoiListPage : PhoneApplicationPage
    {

        private ObservableCollection<TaskSortedByRegion> _data = new ObservableCollection<TaskSortedByRegion>();
        private CollectionTaskDataContext _dataContext;
        private int _status = -2;
        private int _distance;

        public PoiListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "采集任务加载中";
            _dataContext = new CollectionTaskDataContext();
            if (e.NavigationMode == NavigationMode.New)
            {
                _dataContext.RefreshCollectionTasks((data) =>
                {
                    _data = data;
                    taskList.ItemsSource = _data;
                    SystemTray.ProgressIndicator.Text = "采集任务加载完成";
                    SetProgressIndicator(false);
                });
            }
            else
            {
                _dataContext.InitCollectionTasks((data) =>
                {
                    _data = data;
                    taskList.ItemsSource = _data;
                    SystemTray.ProgressIndicator.Text = "采集任务加载完成";
                    SetProgressIndicator(false);
                });
            }
        }

        private void taskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedTask = taskList.SelectedItem as CollectionTaskWrapper;
            NavigationService.Navigate(new Uri("/Pages/CollectionStaff/PoiInfoPage.xaml?tid=" + selectedTask.ID, UriKind.Relative));
        }

        private void LstPkrCate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var statusIndex = ((ListPicker)sender).SelectedIndex;
            if (statusIndex > 0)
            {
                var selectedItem = (ListPickerItem)(((ListPicker)sender).SelectedItem);
                if (selectedItem.TabIndex == 4)
                {
                    _data = _dataContext.GetCollectionTasks();
                    _status = -2;
                }
                else
                {
                    _status = (selectedItem.TabIndex - 1);
                    _data = _dataContext.GetCollectionTasks(_status);
                }
                taskList.ItemsSource = _data;
            }
        }

        private void LstDistanceCate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var distanceIndex = ((ListPicker)sender).SelectedIndex;
            if (distanceIndex > 0)
            {
                var selectedItem = (ListPickerItem)(((ListPicker)sender).SelectedItem);
                _distance = selectedItem.TabIndex;
                _data = _dataContext.GetCollectionTasks(_status, _distance);
                taskList.ItemsSource = _data;
            }
        }

        private async void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            e.Handled = true;
            var button = (Button)sender;
            var grid = (StackPanel)button.Parent;
            if (button.Tag != null)
            {
                string[] geoArray = button.Tag.ToString().Split(',');
                var latitude = geoArray[0];
                var longitude = geoArray[1].Replace(";", String.Empty).Trim();
                string name = grid.Tag.ToString();

                // Assemble the Uri to launch.
                Uri uri = new Uri("ms-drive-to:?destination.latitude=" + latitude +
                    "&destination.longitude=" + longitude + "&destination.name=" + name);
                // The resulting Uri is: "ms-drive-to:?destination.latitude=47.6451413797194
                //  &destination.longitude=-122.141964733601&destination.name=Redmond, WA")

                // Launch the Uri.
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);

                if (success)
                {
                    // Uri launched.
                }
                else
                {
                    // Uri failed to launch.
                }
            }
            else
            {
                MessageBox.Show("导航失败");
            }

        }

        private static void SetProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "更新任务加载中";
            _dataContext = new CollectionTaskDataContext();
            _dataContext.RefreshCollectionTasks((data) =>
            {
                _data = data;
                taskList.ItemsSource = _data;
                SystemTray.ProgressIndicator.Text = "更新任务加载完成";
                SetProgressIndicator(false);
            });
        }


    }
}