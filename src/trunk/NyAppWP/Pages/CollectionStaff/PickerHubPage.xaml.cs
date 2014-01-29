using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

using NyAppWP.Resources;
using NyAppHelper.Http;
using NyAppWP.Controls.ExpanderSelector;

namespace NyAppWP.Pages
{
    public partial class PickerHubPage : PhoneApplicationPage
    {
        // Constructor
        public PickerHubPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void CatePoiList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CollectionStaff/PoiListPage.xaml", UriKind.Relative));
        }

        private void CatePoi_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //ExpanderSelector es = new ExpanderSelector();
            //this.LayoutRoot.Children.Add(es);
            //return;

            //NavigationService.Navigate(new Uri("/PoiInfoPage.xaml",UriKind.Relative));
            
        }

        private void CateMap_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/MapPage.xaml?viewMode=ShowRoute&ToLoc='131,30'", UriKind.Relative));
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Object value = null;
            PhoneApplicationService.Current.State.TryGetValue("fieldPoints",out value);

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(AppResources.MsgConfirmExit, AppResources.MsgConfirmExitTitle, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                Application.Current.Terminate();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Forecast.GetWeather().ContinueWith((r) =>
            {
                Dispatcher.BeginInvoke(() =>
                {

                    if (null != r.Result)
                        TxtWeather.Text = r.Result.City + "," + r.Result.Weather + " " + r.Result.Temperature;
                    else
                        TxtWeather.Text = AppResources.MsgWeatherError;
                });
            });
        }

        private async void TxtWeather_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TxtWeather.Text != AppResources.MsgWeatherError)
                return;

            var result = await Forecast.GetWeather();
            if (null != result)
                TxtWeather.Text = result.City + "," + result.Weather + " " + result.Temperature;
        
        }


    }
}