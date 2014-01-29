using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Device.Location;
using Windows.Devices.Geolocation;
using System.Windows;

namespace NyAppHelper.Location
{
    public class LocationHelper
    {
        private const double EARTH_RADIUS = 6378.137;


        private static Geolocator locator = new Geolocator
        {
            DesiredAccuracy = PositionAccuracy.High,
            DesiredAccuracyInMeters = 1000,
            MovementThreshold = 10,
            ReportInterval = 1 * 1000
        };

        public static async Task<Geoposition> GetLocation()
        {

            if (locator.LocationStatus == PositionStatus.Disabled)
            {
                MessageBox.Show("定位服务未打开，请在系统设置中打开定位服务");
                return null;
            }
            else
            {
                Geoposition myGeoposition = await locator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                );
                return myGeoposition;
            }
        }

        public delegate void InitUserLocationCallbackHandler(Geoposition position);

        public static async void GetLocation(InitUserLocationCallbackHandler callback)
        {
            if (locator.LocationStatus == PositionStatus.Disabled)
            {
                MessageBox.Show("定位服务未打开，请在系统设置中打开定位服务");
                callback(null);
            }
            else
            {
                Geoposition myGeoposition = await locator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                );
                callback(myGeoposition);
            }
        }

        /// <summary>
        /// 计算当前位置到某点的直线距离
        /// </summary>
        /// <param name="lat">维度</param>
        /// <param name="lng">精度</param>
        /// <returns>距离，单位：公里</returns>
        public static async Task<double> DistanceToThere(double lat, double lng)
        {
            return await LocationHelper.GetLocation().ContinueWith<double>((r) =>
            {
                var loc = r.Result;
                if (loc != null)
                {
                    return LocationHelper.distance(lat, lng, loc.Coordinate.Latitude, loc.Coordinate.Longitude);
                }
                else
                    throw new SystemException("位置服务工作异常");
            });
        }

        public static double Distance(double lat1, double lng1, double lat2, double lng2)
        {
            var result = distance(lat1, lng1, lat2, lng2);
            return result;
        }

        private static double distance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

    }
}

