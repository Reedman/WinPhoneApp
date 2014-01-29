using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using Windows.Phone.Media.Capture;

namespace NyAppWP.Pages.Camera
{
    class CameraDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static CameraDataContext _singleton;
        private static IsolatedStorageSettings _settings = IsolatedStorageSettings.ApplicationSettings;
        private PhotoCaptureDevice _device = null;
        private ObservableCollection<CameraParameter> _parameters = new ObservableCollection<CameraParameter>();

        public static CameraDataContext Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new CameraDataContext();
                }

                return _singleton;
            }
        }

        /// <summary>
        /// Collection of camera parameters.
        /// </summary>
        public ObservableCollection<CameraParameter> Parameters
        {
            get
            {
                return _parameters;
            }

            private set
            {
                if (_parameters != value)
                {
                    _parameters = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Parameters"));
                    }
                }
            }
        }

        /// <summary>
        /// Camera instance. Setting new camera instance to this property causes the Parameters
        /// property to be updated as well with the new parameters from the new camera.
        /// </summary>
        public PhotoCaptureDevice Device
        {
            get
            {
                return _device;
            }

            set
            {
                if (_device != value)
                {
                    _device = value;

                    if (_device != null)
                    {
                        ObservableCollection<CameraParameter> newParameters = new ObservableCollection<CameraParameter>();

                        Action<CameraParameter> addParameter = (CameraParameter parameter) =>
                        {
                            if (parameter.Supported && parameter.Modifiable)
                            {
                                try
                                {
                                    parameter.Refresh();
                                    parameter.SetSavedOrDefault();

                                    newParameters.Add(parameter);
                                }
                                catch (Exception)
                                {
                                    System.Diagnostics.Debug.WriteLine("Setting default to " + parameter.Name.ToLower() + " failed");
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Parameter " + parameter.Name.ToLower() + " is not supported or not modifiable");
                            }
                        };

                        addParameter(new SceneModeParameter(_device));
                        addParameter(new WhiteBalancePresetParameter(_device));
                        addParameter(new FlashModeParameter(_device));
                        addParameter(new FlashPowerParameter(_device));
                        addParameter(new IsoParameter(_device));
                        addParameter(new ExposureCompensationParameter(_device));
                        addParameter(new ExposureTimeParameter(_device));
                        addParameter(new AutoFocusRangeParameter(_device));
                        addParameter(new FocusIlluminationModeParameter(_device));
                        addParameter(new CaptureResolutionParameter(_device));

                        Parameters = newParameters;
                    }

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Device"));
                    }
                }
            }
        }

        /// <summary>
        /// Settings accessors.
        /// </summary>
        public static IsolatedStorageSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        /// <summary>
        /// Memory stream to hold the image data captured in MainPage but consumed in PreviewPage.
        /// </summary>
        public MemoryStream ImageStream { get; set; }

    }
}
