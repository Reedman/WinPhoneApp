using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Device.Location;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;
using System.Windows.Controls.Primitives; 

using NyAppWP.Controls;
using NyAppHelper.Model;
using GeoOffset4China;
using Microsoft.Phone.Maps.Services;

namespace NyAppWP
{
    public enum MapViewMode {Generic,Survey,ShowRoute}
    public enum MapSurveyStatus {None, Initializing, Ready, Anormaly }

    public static class CoordinateConverter
    {
        public static System.Device.Location.GeoCoordinate ConvertGeocoordinate(Windows.Devices.Geolocation.Geocoordinate geocoordinate)
        {
            return new System.Device.Location.GeoCoordinate
                (
                geocoordinate.Latitude,
                geocoordinate.Longitude,
                geocoordinate.Altitude ?? Double.NaN,
                geocoordinate.Accuracy,
                geocoordinate.AltitudeAccuracy ?? Double.NaN,
                geocoordinate.Speed ?? Double.NaN,
                geocoordinate.Heading ?? Double.NaN
                );
        }
    }

    public partial class MapPage : PhoneApplicationPage
    {
        //private AMap amap = null;
        //private AMapGeolocator amapLocator;
        private static DependencyProperty flashColor = DependencyProperty.Register("FlashColor", typeof(Boolean), typeof(MapOverlay), null);
        
        private Timer flashTimer = null;
        
        private MapOverlay flashOverlay = null;
        private Ellipse myCircle = null;

        private MapPolyline surveyLine = null;
        private MapPolygon surveyPoly = null;
        private FieldSurveyResult _surverResult = null;

        private Geolocator myGeolocator = null;
        private CartographicModeSelector cartoSelector = null;

        private MapViewMode mapMode = MapViewMode.Generic;
        private MapSurveyStatus suveryStatus = MapSurveyStatus.None;
        private bool surverying = false;


        public FieldSurveyResult SurveryResult
        {
            set
            {
                if (value != null)
                {
                    try
                    {
                        _surverResult = value;
                        GeoCoordinate[] arrGeo = new GeoCoordinate[_surverResult.FieldPoints.Count()];

                        if (arrGeo.Count() > 0)
                        {
                            _surverResult.FieldPoints.CopyTo(arrGeo, 0);

                            foreach (var geo in arrGeo)
                            {
                                surveyPoly.Path.Add(GeoCoordinateOffset4China.GetOffsetCoordinate(geo));
                            }

                            map.MapElements.Add(surveyPoly);
                        }

                    }
                    catch
                    {

                    }
                }
            }
            get
            {
                return _surverResult;
            }
        }

        public MapPage()
        {
            InitializeComponent();
            InitGeolocator();

            ShowMyLocationOnTheMap();
            ShowMyLocationPin();
            ShowSurveyLine();
            ShowCartoSelector();
        }


        #region initialization
        private bool InitGeolocator()
        {
            myGeolocator = new Geolocator();
            myGeolocator.DesiredAccuracy = PositionAccuracy.High;
            myGeolocator.DesiredAccuracyInMeters = 10;
            myGeolocator.MovementThreshold = 5;
            myGeolocator.ReportInterval = 1 * 1000;

            if (myGeolocator.LocationStatus == PositionStatus.Disabled)
            {
                MessageBox.Show("定位服务未打开，请在系统设置中打开定位服务");
                return false;
            }
            else
            {
                myGeolocator.StatusChanged += (s, e) => {
                    switch (e.Status)
                    {
                        case PositionStatus.Disabled:
                            suveryStatus = MapSurveyStatus.Anormaly;
                            Dispatcher.BeginInvoke(() =>
                            {
                                UpdateIndicator("定位服务未打开");
                            });
                            break;

                        case PositionStatus.NoData:
                            suveryStatus = MapSurveyStatus.Anormaly;
                            Dispatcher.BeginInvoke(() =>
                            {
                                UpdateIndicator("定位服务数据异常");
                            });
                            break;

                        case PositionStatus.Ready:
                            suveryStatus = MapSurveyStatus.Ready;
                            Dispatcher.BeginInvoke(() =>
                            {
                                UpdateIndicator("定位服务工作正常");
                            });
                            break;

                        case PositionStatus.Initializing:
                            suveryStatus = MapSurveyStatus.Initializing;
                            Dispatcher.BeginInvoke(() =>
                            {
                                UpdateIndicator("定位服务初始化中");
                            });
                            break;

                        case PositionStatus.NotInitialized:
                            suveryStatus = MapSurveyStatus.Anormaly;
                            Dispatcher.BeginInvoke(() =>
                            {
                                UpdateIndicator("定位服务为初始化");
                            });
                            break;

                        case PositionStatus.NotAvailable:
                            suveryStatus = MapSurveyStatus.Anormaly;
                            Dispatcher.BeginInvoke(() =>
                            {
                                UpdateIndicator("本设备不支持定位服务");
                            });
                            break;
                    }
                };

                return true;
            }
        }

        private async void ShowMyLocationOnTheMap()
        {
            if(myGeolocator.LocationStatus == PositionStatus.Disabled)
            {
                return;
            }

            //Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await myGeolocator.GetGeopositionAsync(
                maximumAge: TimeSpan.FromMinutes(5),
                timeout: TimeSpan.FromSeconds(10)
                );
            
            Windows.Devices.Geolocation.Geocoordinate myGeocoordinate = myGeoposition.Coordinate;
            System.Device.Location.GeoCoordinate myGeoCoordinate = GeoCoordinateOffset4China.GetOffsetCoordinate(CoordinateConverter.ConvertGeocoordinate(myGeocoordinate));
            
            myCircle.Visibility = Visibility.Visible;
            flashOverlay.GeoCoordinate = myGeoCoordinate;
            map.SetView(myGeoCoordinate, 18, MapAnimationKind.Parabolic);

            if(flashTimer == null)
            {
                flashTimer = new Timer(TimerProc);
                flashTimer.Change(1000, 1000);
            }

        }

        private void ShowCartoSelector()
        {
            cartoSelector = new CartographicModeSelector();
            cartoSelector.Visibility = System.Windows.Visibility.Collapsed;
            cartoSelector.Margin = new Thickness(296, 500, 0, 0);
            cartoSelector.CartoRoad.Tap += (s, e) =>
            {
                map.CartographicMode = MapCartographicMode.Road;
            };

            cartoSelector.CartoHybrid.Tap += (s, e) =>
            {
                map.CartographicMode = MapCartographicMode.Hybrid;
            };

            cartoSelector.CartoTerrain.Tap += (s, e) =>
            {
                map.CartographicMode = MapCartographicMode.Terrain;
            };

            this.LayoutRoot.Children.Add(cartoSelector);
        }

        private void ShowSurveyLine()
        {
            surveyLine = new MapPolyline();
            surveyLine.StrokeColor = Colors.Red;
            surveyLine.StrokeThickness = 10;
            map.MapElements.Add(surveyLine);

            surveyPoly = new MapPolygon();
            Color col = Colors.Green;col.A = 150;
            surveyPoly.FillColor = col;
            surveyPoly.StrokeColor = Colors.Red;
            surveyPoly.StrokeThickness = 2;
        }

        private void ShowMyLocationPin()
        {
            myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;
            myCircle.Visibility = Visibility.Collapsed;

            flashOverlay = new MapOverlay();
            flashOverlay.PositionOrigin = new Point(0.5, 0.5);
            flashOverlay.SetValue(flashColor, true);

            // Create a MapOverlay to contain the circle.
            flashOverlay.Content = myCircle;
            //flashOverlay.GeoCoordinate = coor;
            flashOverlay.SetValue(flashColor, true);

            // Create a MapLayer to contain the MapOverlay.
            MapLayer myLocationLayer = new MapLayer();
            myLocationLayer.Add(flashOverlay);

            // Add the MapLayer to the Map.
            map.Layers.Add(myLocationLayer);

        }
        #endregion

        #region Func
        /// <summary>
        /// 
        /// </summary>
        /// <param name="started">是否已经开始测量</param>
        private void SwitchToolbarBtn(bool started)
        {
            var bar = ApplicationBar;
            bar.Buttons.RemoveAt(0);

            if(!started)
            {
                ApplicationBarIconButton startButton = new ApplicationBarIconButton();
                startButton.IconUri = new Uri("/Assets/i/start-icon.png", UriKind.Relative);
                startButton.Text = "开始";
                bar.Buttons.Insert(0, startButton);
                startButton.Click += BtnStart_Click;
            }
            else
            {
                ApplicationBarIconButton finishButton = new ApplicationBarIconButton();
                finishButton.IconUri = new Uri("/Assets/i/end-icon.png", UriKind.Relative);
                finishButton.Text = "结束";
                bar.Buttons.Insert(0, finishButton);
                finishButton.Click += BtnEnd_Click;
            }
        }

        private void BuildApplicationToolBar(MapViewMode mode)
        {
            var bar = ApplicationBar;
            if(bar != null)
            {
                switch (mode)
                {
                    case MapViewMode.Survey:
                        {
                            ApplicationBarIconButton startButton = new ApplicationBarIconButton();
                            startButton.IconUri = new Uri("/Assets/i/start-icon.png", UriKind.Relative);
                            startButton.Text = "开始";
                            bar.Buttons.Add(startButton);
                            startButton.Click += BtnStart_Click;

                            ApplicationBarIconButton restartButton = new ApplicationBarIconButton();
                            restartButton.IconUri = new Uri("/Assets/i/restart-icon.png", UriKind.Relative);
                            restartButton.Text = "重测";
                            bar.Buttons.Add(restartButton);
                            restartButton.Click += BtnReStart_Click;

                            //x:Name="BtnCommit" IconUri="/Assets/AppBarIcons/AppBar-save-icon.png" Text="保存" Click="BtnCommit_Click"
                            ApplicationBarIconButton commitButton = new ApplicationBarIconButton();
                            commitButton.IconUri = new Uri("/Assets/AppBarIcons/AppBar-save-icon.png", UriKind.Relative);
                            commitButton.Text = "保存";
                            bar.Buttons.Add(commitButton);
                            commitButton.Click += BtnCommit_Click;

                            //x:Name="BtnView" IconUri="/Assets/i/layer-icon.png" Text="图层" Click="BtnView_Click"
                            ApplicationBarIconButton viewButton = new ApplicationBarIconButton();
                            viewButton.IconUri = new Uri("/Assets/i/layer-icon.png", UriKind.Relative);
                            viewButton.Text = "图层";
                            bar.Buttons.Add(viewButton);
                            viewButton.Click += BtnView_Click;
                        }
                        break;
                    case MapViewMode.ShowRoute:
                        {
                            ApplicationBarIconButton viewButton = new ApplicationBarIconButton();
                            viewButton.IconUri = new Uri("/Assets/i/layer-icon.png", UriKind.Relative);
                            viewButton.Text = "图层";
                            bar.Buttons.Add(viewButton);
                            viewButton.Click += BtnView_Click;


                        }
                        break;
                }
            }

        }

        private void StartSurvey()
        {
            if (suveryStatus != MapSurveyStatus.Ready)
            {
                MessageBox.Show("定位服务工作异常，请确保定位服务工作正常后，开始田块测量工作");
                return;
            }

            if (surveyPoly.Path.Count() > 0)
            {
                if (MessageBox.Show("本田块已测量，重新测量将覆盖原有数据，是否要重新测量？", "请确认", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            if (MessageBox.Show("即将开始田块测量", "", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                return;
            }

            SwitchToolbarBtn(true);

            surveyPoly.Path.Clear();
            map.MapElements.Remove(surveyPoly);
            if (null != _surverResult)
                _surverResult.FieldPoints.Clear();
            
            surverying = true;

            myGeolocator.PositionChanged += myGeolocator_PositionChanged;

        }

        /// <summary>
        /// 开始测量
        /// </summary>
        private bool FinishSurvery()
        {

            if (surveyLine.Path.Count() <= 5)
            {
                return false;
            }

            if (surveyLine.Path[0].GetDistanceTo(surveyLine.Path[surveyLine.Path.Count() - 1]) > 20)
            {
                if (MessageBox.Show("您当前位置与出发点相差20米以上，测量数据可能不准确，是否立即完成丈量？", "请确认", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }
            
            surverying = false;
            SwitchToolbarBtn(false);

            surveyLine.Path.Clear();
            surveyPoly.Path.Add(surveyPoly.Path[0]);
            map.MapElements.Add(surveyPoly);

            _surverResult.FieldPoints.Add(_surverResult.FieldPoints[0]);

            myCircle.Visibility = System.Windows.Visibility.Collapsed;

            return true;
        }

        private void UpdateIndicator(String text)
        {
            if(mapMode == MapViewMode.Survey)
                MsgIndicator.Text = text;
        }

        private void UpdateAccuracy(String text)
        {
            if (mapMode == MapViewMode.Survey)
                MsgAccuracy.Text = text;
        }

        private void SaveResults()
        {
            try
            {
                //GeoCoordinate[] arrGeo = new GeoCoordinate[surveyPoly.Path.Count()];

                //if (arrGeo.Count() > 0)
                //{
                //    _surverResult.FieldPoints.Clear();

                //    surveyPoly.Path.CopyTo(arrGeo, 0);
                //    foreach (var geo in arrGeo)
                //    {
                //        _surverResult.FieldPoints.Add(GeoCoordinateOffset4China.GetOffsetCoordinate(geo));
                //    }
                //}

                if (NyAppHelper.Location.GeoHelper.IsClockWise(_surverResult.FieldPoints))
                    _surverResult.FieldPoints.Reverse();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Events
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            String strViewMode = null;
            if (NavigationContext.QueryString.TryGetValue("viewMode", out strViewMode))
            {
                if (strViewMode == "Survey")
                {
                    mapMode = MapViewMode.Survey;
                    GpsInfoPanel.Visibility = System.Windows.Visibility.Visible;

                    String Msg = "请等待GPS设备工作正常后，点击工具栏中“开始”，围绕您的农田一周，按“结束”完成测量";
                    GenericPopup pop = new GenericPopup(Msg, false);
                    this.LayoutRoot.Children.Add(pop);
                }
                else if(strViewMode == "ShowRoute")
                {
                    mapMode = MapViewMode.ShowRoute;
                    GpsInfoPanel.Visibility = System.Windows.Visibility.Collapsed;

                    String toLoc = null;
                    if (NavigationContext.QueryString.TryGetValue("ToLoc", out toLoc))
                    {
                        var coor = toLoc.Split(',');
                        var target =  new GeoCoordinate(double.Parse(coor[1]), double.Parse(coor[0]));
                    }
                }
            }
            else 
            {
                mapMode = MapViewMode.Generic;
                GpsInfoPanel.Visibility = System.Windows.Visibility.Collapsed;
            }

            BuildApplicationToolBar(mapMode);
        }

        void myGeolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            //args.Position
            System.Diagnostics.Debug.WriteLine("Geolocator_PositionChanged");

            Windows.Devices.Geolocation.Geocoordinate coorWin = args.Position.Coordinate;
            System.Device.Location.GeoCoordinate geoCoorUnOffset = CoordinateConverter.ConvertGeocoordinate(coorWin);
            System.Device.Location.GeoCoordinate geoCoorOffseted = null;

            try
            {
                geoCoorOffseted = GeoCoordinateOffset4China.GetOffsetCoordinate(CoordinateConverter.ConvertGeocoordinate(coorWin));
                //System.Diagnostics.Debug.WriteLine(coorSys.Longitude.ToString() + ";" + coorSys.Latitude.ToString());
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            Dispatcher.BeginInvoke(() =>
            {
                UpdateAccuracy("定位精确度: " + coorWin.Accuracy.ToString() + "米");
            });

            if (coorWin.Accuracy > 12)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    UpdateIndicator("警告！定位精度超过误差容许范围");
                });

                return;
            }
            else
            {
                if(sender.LocationStatus == PositionStatus.Ready)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        UpdateIndicator("定位服务工作正常");
                    });
                }
            }

            if (surverying)
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (_surverResult != null)
                    {
                        _surverResult.FieldPoints.Add(geoCoorUnOffset);
                    }

                    surveyPoly.Path.Add(geoCoorOffseted);
                    surveyLine.Path.Add(geoCoorOffseted);

                    if (surveyLine.Path.Count() > 10 && surveyLine.Path[0].GetDistanceTo(surveyLine.Path[surveyLine.Path.Count() - 1]) < 5)
                    {
                        myGeolocator.PositionChanged -= myGeolocator_PositionChanged;

                        GenericPopup pop = new GenericPopup("是否完成测量？");
                        this.LayoutRoot.Children.Add(pop);

                        pop.OnDisMiss += (s, argv) =>
                        {
                            if (argv.result == PopupResult.PROK)
                            {
                                myGeolocator.PositionChanged -= myGeolocator_PositionChanged;
                                if(!FinishSurvery())
                                {
                                    myGeolocator.PositionChanged += myGeolocator_PositionChanged;
                                }

                            }
                            else
                            {
                                myGeolocator.PositionChanged += myGeolocator_PositionChanged;
                            }

                        };

                    }

                    myCircle.Visibility = Visibility.Visible;
                    flashOverlay.GeoCoordinate = geoCoorOffseted;
                    map.SetView(geoCoorOffseted, 18, MapAnimationKind.Parabolic);
                });
            }

        }

        private void TimerProc(object state)
        {
            //System.Diagnostics.Debug.WriteLine("TimerProc");

            Dispatcher.BeginInvoke(() =>
            {
                if (flashOverlay != null)
                {
                    Boolean color = (Boolean)flashOverlay.GetValue(flashColor);

                    if (color)
                    {
                        myCircle.Fill = new SolidColorBrush(Colors.Blue);
                    }
                    else
                    {
                        myCircle.Fill = new SolidColorBrush(Colors.Yellow);
                    }

                    flashOverlay.SetValue(flashColor, !color);
                }
            });
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            StartSurvey();
        }

        private void BtnEnd_Click(object sender, EventArgs e)
        {
            myGeolocator.PositionChanged -= myGeolocator_PositionChanged;
            if (!FinishSurvery())
            {
                myGeolocator.PositionChanged += myGeolocator_PositionChanged;
            }
        }

        private void BtnReStart_Click(object sender, EventArgs e)
        {
            if (surveyPoly.Path.Count() == 0)
                return;

            if (MessageBox.Show("是否清除原有测量数据？", "请确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                surveyLine.Path.Clear();
                surveyPoly.Path.Clear();

                map.MapElements.Remove(surveyPoly);

                SwitchToolbarBtn(false);
                surverying = false;
            }

        }

        private void BtnView_Click(object sender, EventArgs e)
        {
            var cs = cartoSelector;
            cs.Visibility = cs.Visibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        private void map_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            cartoSelector.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            //this.LayoutRoot.Children.Clear();
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        private void BtnCommit_Click(object sender, EventArgs e)
        {
            if (surveyPoly.Path.Count() == 0 || surverying)
            {
                MessageBox.Show("请先完成测量后保存", "提示", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("是否保存测量结果并返回？", "请确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                SaveResults();
                NavigationService.GoBack();
            }

        }

        private void btnFindMe_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ShowMyLocationOnTheMap();
        }


        #region Interface

        private void SetSurveryResult()
        {

        }

        #endregion

        private void btnZoomIn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                map.ZoomLevel = map.ZoomLevel + 1; 
            }
            catch
            {

            }
        }

        private void btnZoomOut_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                map.ZoomLevel = map.ZoomLevel - 1;
            }
            catch
            {

            }
        }



    }

}