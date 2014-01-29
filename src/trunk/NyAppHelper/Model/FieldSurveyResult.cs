using Microsoft.Phone.Maps.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyAppHelper.Model
{
    public class FieldSurveyResult
    {
        private String _field_id;
        private float _field_area;
        private GeoCoordinateCollection _field_points = new GeoCoordinateCollection();

        public String FieldID
        {
            get
            {
                return _field_id;
            }
            set
            {
                if (_field_id != value)
                {
                    _field_id = value;
                }
            }
        }

        public float FieldArea
        {
            get
            {
                return _field_area;
            }
            set
            {
                if (_field_area != value)
                {
                    _field_area = value;
                }
            }
        }

        public GeoCoordinateCollection FieldPoints
        {
            get
            {
                return _field_points;
            }
            set
            {
                if (_field_points != value)
                {
                    _field_points = value;
                }
            }
        }

    }
}
