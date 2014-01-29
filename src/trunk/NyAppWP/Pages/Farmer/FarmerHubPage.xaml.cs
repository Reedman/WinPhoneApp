using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using NyAppWP.Resources;
using NyAppHelper.Http;

namespace NyAppWP.Pages
{
    public partial class FarmerHubPage : PhoneApplicationPage
    {

        public FarmerHubPage()
        {
            InitializeComponent();
        }

        private void myFarm_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Farmer/FieldList.xaml", UriKind.Relative));
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
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
    }
}