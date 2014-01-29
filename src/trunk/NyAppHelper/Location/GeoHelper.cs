using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppHelper.Location
{
    public class GeoHelper
    {
        public const double MaxTolerate = 0.00001;

        /// <summary>
        /// 判断多边形edge是否为顺时针
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static bool IsClockWise(GeoCoordinateCollection points)
        {
            if (points.Count() < 4)
                throw new ArgumentException("多边形构成错误");
            
            double area = 0;
            for (var i = 1; i < points.Count(); i++)
            {
                area += (points[i].Longitude - points[i - 1].Longitude) * (points[i].Latitude + points[i - 1].Latitude);
            }

            if(area == 0)
                throw new ArgumentException("多边形构成错误");
            
            //正数，顺时针
            return area > 0 ? true : false;
        }

        public static GeoCoordinateCollection ParsePointsFromString(String pts,Char pairSplitChar,Char coordinateSplitChar)
        {
            var geoPts = new GeoCoordinateCollection();
            if(pts != null && pts != "")
            {
                var ptArray = pts.Split(pairSplitChar);
                foreach(var pt  in ptArray)
                {
                    geoPts.Add(new System.Device.Location.GeoCoordinate(double.Parse(pt.Split(coordinateSplitChar)[1]),double.Parse(pt.Split(coordinateSplitChar)[0])));
                }
            }

            return geoPts;
        }
    }
}
