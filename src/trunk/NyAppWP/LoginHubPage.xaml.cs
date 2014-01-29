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
using Microsoft.Phone.Net.NetworkInformation;
using NyAppWP.Pages;
using NyAppHelper.Data;
using NyAppHelper.Model;
using NyAppHelper.Http;

using NyAppWP.Resources;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.IO;
using Windows.Storage.Streams;

namespace NyAppWP
{
    public partial class LoginHubPage : PhoneApplicationPage
    {

        private UserService _userService;

        public LoginHubPage()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("检查不到网络，请重新连网");
            }



        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.ProgressIndicator = new ProgressIndicator();

        }

        private static void SetProgressIndicator(bool isVisible)
        {
            SystemTray.ProgressIndicator.IsIndeterminate = isVisible;
            SystemTray.ProgressIndicator.IsVisible = isVisible;
        }

        private void PickerEntry_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.PvtMain.IsLocked = false;
            this.PvtMain.SelectedIndex = 1;
            this.PvtMain.IsLocked = true;
        }

        private void FarmerEntry_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            this.PvtMain.IsLocked = false;
            this.PvtMain.SelectedIndex = 2;
            this.PvtMain.IsLocked = true;
        }

        private void TxtBoxUser_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBoxUser.Text == "用户名")
            {
                TxtBoxUser.Text = "";
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Black;
                TxtBoxUser.Foreground = Brush1;
            }

        }

        private void TxtBoxUser_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBoxUser.Text == String.Empty)
            {
                TxtBoxUser.Text = "用户名";
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Gray;
                TxtBoxUser.Foreground = Brush2;
            }
        }

        private void TxtBoxPass_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBoxPass.Password == String.Empty)
            {
                TxtBoxPass.Password = String.Empty;
                SolidColorBrush Brush1 = new SolidColorBrush();
                Brush1.Color = Colors.Black;
                TxtBoxPass.Foreground = Brush1;
            }
        }

        private void TxtBoxPass_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBoxPass.Password == String.Empty)
            {
                TxtBoxPass.Password = String.Empty;
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Gray;
                TxtBoxPass.Foreground = Brush2;
            }
        }

        private void TxbForgetPass_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void PvtMain_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            this.PvtMain.IsLocked = true;
        }

        private async void BtnLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("/Pages/Farmer/FarmerHubPage.xaml", UriKind.Relative));
            //return;
            if (txtPhone.Text == "")
            {
                txtNumError.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            if (txtCaptcha.Password == "")
            {
                txtPassError.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            int roleType = int.Parse(((Image)sender).Tag.ToString());
            string phoneNum = txtPhone.Text;
            string captcha = txtCaptcha.Password;

            string hubPage = @"/Pages/Farmer/FarmerHubPage.xaml";
            //验证农户登录
            await authenticateUser(phoneNum, captcha, hubPage, roleType,"农户");

        }

        private void BtnReg_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.PvtMain.IsLocked = false;
            switch (this.PvtMain.SelectedIndex)
            {
                case 0:
                    if (MessageBox.Show(AppResources.MsgConfirmExit, AppResources.MsgConfirmExitTitle,
                            MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                    {
                        e.Cancel = true;
                    }
                    break;
                case 1:
                    this.PvtMain.SelectedIndex = 0;
                    this.PvtMain.IsLocked = true;
                    e.Cancel = true;
                    break;
                case 2:
                    this.PvtMain.SelectedIndex = 0;
                    this.PvtMain.IsLocked = true;
                    e.Cancel = true;
                    break;
                case 3:
                    this.PvtMain.SelectedIndex = 2;
                    this.PvtMain.IsLocked = true;
                    e.Cancel = true;
                    break;
            }

        }

        private async void BtnPickerLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TxtBoxUser.Text == "")
            {
                txtNumError.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            if (TxtBoxPass.Password == "")
            {
                txtPassError.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            int roleType = int.Parse(((Image)sender).Tag.ToString());
            string phoneNum = TxtBoxUser.Text;
            string captcha = TxtBoxPass.Password.Trim();

            string hubPage = @"/Pages/CollectionStaff/PickerHubPage.xaml";
            //验证采集员登录
            await authenticateUser(phoneNum, captcha, hubPage, roleType,"采集员");

        }

        private void txtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtNumError.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void txtCaptcha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPassError.Visibility = System.Windows.Visibility.Collapsed;
        }

        private async Task authenticateUser(string userName, string pass, string hubPage, int roleType,string roleName)
        {
            SetProgressIndicator(true);
            SystemTray.ProgressIndicator.Text = "正在登录";

            var authenticatedUser = await _userService.AuthanticateUser(userName, pass);
            if (authenticatedUser != null)
            {
                if (authenticatedUser.RoleID == roleType)
                {
                    var isolateStorageExecutor = new IsolateStorageFileExecutor<User>();
                    var appServiceStateExcutor = new AppServiceStateExecutor<string>();
                    string key = "user" + authenticatedUser.UserId;
                    appServiceStateExcutor.Set(authenticatedUser.UserId.ToString().Trim(), "uid");
                    appServiceStateExcutor.Set(authenticatedUser.Token, "token");
                    if (isolateStorageExecutor.IsExists(key))
                    {
                        var localUser = await isolateStorageExecutor.Get(key);
                        if (localUser.UserId == authenticatedUser.UserId)
                        {
                            NavigationService.Navigate(new Uri(hubPage, UriKind.Relative));
                        }
                        else
                        {
                            if (await isolateStorageExecutor.Set(authenticatedUser, key))
                            {
                                if (roleType == 4)
                                {
                                    NavigationService.Navigate(new Uri(hubPage, UriKind.Relative));
                                }
                                else
                                {
                                    NavigationService.Navigate(new Uri("/Pages/Farmer/SplashPage.xaml", UriKind.Relative));
                                }
                            }
                            else
                            {
                                MessageBox.Show("登录失败");
                            }
                        }
                    }
                    else
                    {
                        if (await isolateStorageExecutor.Set(authenticatedUser, key))
                        {
                            if (roleType == 4)
                            {
                                NavigationService.Navigate(new Uri(hubPage, UriKind.Relative));
                            }
                            else
                            {
                                NavigationService.Navigate(new Uri("/Pages/Farmer/SplashPage.xaml", UriKind.Relative));
                            }
                        }
                        else
                        {
                            MessageBox.Show("登录失败");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("登录角色必须是" + roleName);
                }
            }
            else
            {
                MessageBox.Show("用户认证失败");
            }

            SetProgressIndicator(false);

        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PivotPageMockup.xaml", UriKind.Relative));
        }

    }
}