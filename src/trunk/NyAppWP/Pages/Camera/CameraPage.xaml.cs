using Microsoft.Devices;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Phone.Media.Capture;

namespace NyAppWP.Pages.Camera
{
    public partial class CameraPage : PhoneApplicationPage
    {
        private readonly Windows.Foundation.Size DefaultCameraResolution =
        new Windows.Foundation.Size(640, 480);

        private CameraDataContext _dataContext = CameraDataContext.Singleton;
        private ProgressIndicator _progressIndicator = new ProgressIndicator();
        private bool _capturing = false;
        private Semaphore _focusSemaphore = new Semaphore(1, 1);
        private bool _manuallyFocused = false;
        private Windows.Foundation.Size _focusRegionSize = new Windows.Foundation.Size(80, 80);
        private SolidColorBrush _notFocusedBrush = new SolidColorBrush(Colors.Red);
        private SolidColorBrush _focusedBrush = new SolidColorBrush(Colors.Green);

        private CameraSensorLocation _sensorLocation = CameraSensorLocation.Back;

        public CameraPage()
        {
            InitializeComponent();

            VideoCanvas.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(videoCanvas_Tap);

            DataContext = _dataContext;
            _progressIndicator.IsIndeterminate = true;

        }

        /// <summary>
        /// If camera has not been initialized when navigating to this page, initialization
        /// will be started asynchronously in this method. Once initialization has been
        /// completed the camera will be set as a source to the VideoBrush element
        /// declared in XAML. On-screen controls are enabled when camera has been initialized.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("MainPage.OnNavigatedTo()");

            if (_dataContext.Device == null)
            {
                ShowProgress("正在初始化摄像头...");
                await InitializeCamera(_sensorLocation);
                HideProgress();
            }

            viewfinderTransform.Rotation = 90;


            var device = _dataContext.Device;

            videoBrush.SetSource(device);

            //overlayComboBox.Opacity = 1;

            SetScreenButtonsEnabled(true);
            SetCameraButtonsEnabled(true);

            base.OnNavigatedTo(e);
        }

        private void PhoneApplicationPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if (_dataContext.Device != null)
            {
                switch (e.Orientation)
                {
                    case PageOrientation.Landscape:
                    case PageOrientation.LandscapeLeft:
                        viewfinderTransform.Rotation = 0;
                        break;
                    case PageOrientation.LandscapeRight:
                        viewfinderTransform.Rotation = 180;
                        break;
                    case PageOrientation.Portrait:
                    case PageOrientation.PortraitUp:
                        viewfinderTransform.Rotation = 90;
                        break;
                    case PageOrientation.PortraitDown:
                        viewfinderTransform.Rotation = 270;
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes camera. Once initialized the instance is set to the
        /// DataContext.Device property for further usage from this or other
        /// pages.
        /// </summary>
        /// <param name="sensorLocation">Camera sensor to initialize.</param>
        private async Task InitializeCamera(CameraSensorLocation sensorLocation)
        {
            // Find out the largest capture resolution available on device
            IReadOnlyList<Windows.Foundation.Size> availableResolutions =
                PhotoCaptureDevice.GetAvailableCaptureResolutions(sensorLocation);

            Windows.Foundation.Size captureResolution = new Windows.Foundation.Size(0, 0);

            for (int i = 0; i < availableResolutions.Count; ++i)
            {
                if (captureResolution.Width < availableResolutions[i].Width)
                {
                    Debug.WriteLine("MainPage.InitializeCamera(): New capture resolution: " + availableResolutions[i]);
                    captureResolution = availableResolutions[i];
                }
            }
            
            PhotoCaptureDevice device =
                await PhotoCaptureDevice.OpenAsync(sensorLocation, DefaultCameraResolution);

            await device.SetPreviewResolutionAsync(DefaultCameraResolution);
            await device.SetCaptureResolutionAsync(captureResolution);

            device.SetProperty(KnownCameraGeneralProperties.EncodeWithOrientation,
                          device.SensorLocation == CameraSensorLocation.Back ?
                          device.SensorRotationInDegrees : -device.SensorRotationInDegrees);

            _dataContext.Device = device;

        }

        /// <summary>
        /// Starts displaying progress indicator.
        /// </summary>
        /// <param name="msg">Text message to display.</param>
        private void ShowProgress(String msg)
        {
            _progressIndicator.Text = msg;
            _progressIndicator.IsVisible = true;

            SystemTray.SetProgressIndicator(this, _progressIndicator);
        }

        /// <summary>
        /// Stops displaying progress indicator.
        /// </summary>
        private void HideProgress()
        {
            _progressIndicator.IsVisible = false;

            SystemTray.SetProgressIndicator(this, _progressIndicator);
        }



        /// <summary>
        /// Set autofocus area to tap location and refocus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void videoCanvas_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            System.Windows.Point uiTapPoint = e.GetPosition(VideoCanvas);

            if (PhotoCaptureDevice.IsFocusRegionSupported(_dataContext.Device.SensorLocation) && _focusSemaphore.WaitOne(0))
            {
                // Get tap coordinates as a foundation point
                Windows.Foundation.Point tapPoint = new Windows.Foundation.Point(uiTapPoint.X, uiTapPoint.Y);

                double xRatio = VideoCanvas.ActualHeight / _dataContext.Device.PreviewResolution.Width;
                double yRatio = VideoCanvas.ActualWidth / _dataContext.Device.PreviewResolution.Height;

                // adjust to center focus on the tap point
                Windows.Foundation.Point displayOrigin = new Windows.Foundation.Point(
                            tapPoint.Y - _focusRegionSize.Width / 2,
                            (VideoCanvas.ActualWidth - tapPoint.X) - _focusRegionSize.Height / 2);

                // adjust for resolution difference between preview image and the canvas
                Windows.Foundation.Point viewFinderOrigin = new Windows.Foundation.Point(displayOrigin.X / xRatio, displayOrigin.Y / yRatio);
                Windows.Foundation.Rect focusrect = new Windows.Foundation.Rect(viewFinderOrigin, _focusRegionSize);

                // clip to preview resolution
                Windows.Foundation.Rect viewPortRect = new Windows.Foundation.Rect(0, 0, _dataContext.Device.PreviewResolution.Width, _dataContext.Device.PreviewResolution.Height);
                focusrect.Intersect(viewPortRect);

                _dataContext.Device.FocusRegion = focusrect;

                // show a focus indicator
                FocusIndicator.SetValue(Shape.StrokeProperty, _notFocusedBrush);
                FocusIndicator.SetValue(Canvas.LeftProperty, uiTapPoint.X - _focusRegionSize.Width / 2);
                FocusIndicator.SetValue(Canvas.TopProperty, uiTapPoint.Y - _focusRegionSize.Height / 2);
                FocusIndicator.SetValue(Canvas.VisibilityProperty, Visibility.Visible);

                CameraFocusStatus status = await _dataContext.Device.FocusAsync();

                if (status == CameraFocusStatus.Locked)
                {
                    FocusIndicator.SetValue(Shape.StrokeProperty, _focusedBrush);
                    _manuallyFocused = true;
                    _dataContext.Device.SetProperty(KnownCameraPhotoProperties.LockedAutoFocusParameters,
                        AutoFocusParameters.Exposure & AutoFocusParameters.Focus & AutoFocusParameters.WhiteBalance);
                }
                else
                {
                    _manuallyFocused = false;
                    _dataContext.Device.SetProperty(KnownCameraPhotoProperties.LockedAutoFocusParameters, AutoFocusParameters.None);
                }

                _focusSemaphore.Release();
            }
        }

        private async void BtnShutter_Click(object sender, EventArgs e)
        {
            if (!_manuallyFocused)
            {
                await AutoFocus();
            }

            await Capture();
        }

        /// <summary>
        /// Captures a photo. Photo data is stored to DataContext.ImageStream, and application
        /// is navigated to the preview page after capturing.
        /// </summary>
        private async Task Capture()
        {

            bool goToPreview = false;

            if (!_capturing)
            {
                _capturing = true;

                MemoryStream stream = new MemoryStream();

                CameraCaptureSequence sequence = _dataContext.Device.CreateCaptureSequence(1);
                sequence.Frames[0].CaptureStream = stream.AsOutputStream();

                await _dataContext.Device.PrepareCaptureSequenceAsync(sequence);
                await sequence.StartCaptureAsync();

                _dataContext.ImageStream = stream;

                _capturing = false;

                // Defer navigation as it will release the camera device and the
                // following Device calls must still work.
                goToPreview = true;

                //stream.Close();
            }

            _manuallyFocused = false;

            if (PhotoCaptureDevice.IsFocusRegionSupported(_dataContext.Device.SensorLocation))
            {
                _dataContext.Device.FocusRegion = null;
            }

            FocusIndicator.SetValue(Canvas.VisibilityProperty, Visibility.Collapsed);
            _dataContext.Device.SetProperty(KnownCameraPhotoProperties.LockedAutoFocusParameters, AutoFocusParameters.None);

            if (goToPreview)
            {
                NavigationService.Navigate(new Uri("/Pages/Camera/PreviewPage.xaml", UriKind.Relative));
            }
        }

        /// <summary>
        /// Starts autofocusing, if supported. Capturing buttons are disabled while focusing.
        /// </summary>
        private async Task AutoFocus()
        {
            if (!_capturing && PhotoCaptureDevice.IsFocusSupported(_dataContext.Device.SensorLocation))
            {
                SetScreenButtonsEnabled(false);
                SetCameraButtonsEnabled(false);

                await _dataContext.Device.FocusAsync();

                SetScreenButtonsEnabled(true);
                SetCameraButtonsEnabled(true);

                _capturing = false;
            }
        }

        /// <summary>
        /// Enables or disabled on-screen controls.
        /// </summary>
        /// <param name="enabled">True to enable controls, false to disable controls.</param>
        private void SetScreenButtonsEnabled(bool enabled)
        {
            foreach (ApplicationBarIconButton b in ApplicationBar.Buttons)
            {
                b.IsEnabled = enabled;
            }

            foreach (ApplicationBarMenuItem m in ApplicationBar.MenuItems)
            {
                m.IsEnabled = enabled;
            }
        }


        /// <summary>
        /// Enables or disables listening to hardware shutter release key events.
        /// </summary>
        /// <param name="enabled">True to enable listening, false to disable listening.</param>
        private void SetCameraButtonsEnabled(bool enabled)
        {
            if (enabled)
            {
                Microsoft.Devices.CameraButtons.ShutterKeyHalfPressed += ShutterKeyHalfPressed;
                Microsoft.Devices.CameraButtons.ShutterKeyPressed += ShutterKeyPressed;
            }
            else
            {
                Microsoft.Devices.CameraButtons.ShutterKeyHalfPressed -= ShutterKeyHalfPressed;
                Microsoft.Devices.CameraButtons.ShutterKeyPressed -= ShutterKeyPressed;
            }
        }

        /// <summary>
        /// Half-pressing the shutter key initiates autofocus.
        /// </summary>
        private async void ShutterKeyHalfPressed(object sender, EventArgs e)
        {
            if (_manuallyFocused)
            {
                _manuallyFocused = false;
            }

            FocusIndicator.SetValue(Canvas.VisibilityProperty, Visibility.Collapsed);
            await AutoFocus();
        }

        /// <summary>
        /// Completely pressing the shutter key initiates capturing a photo.
        /// </summary>
        private async void ShutterKeyPressed(object sender, EventArgs e)
        {
            try
            {
                await Capture();
            }
            catch(Exception exp)
            {
                System.Diagnostics.Debug.WriteLine(exp.Message);
            }
        }




    }
}