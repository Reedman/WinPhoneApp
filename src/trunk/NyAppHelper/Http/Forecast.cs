using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RestSharp;
using System.Net;
using NyAppHelper.Data;
using NyAppHelper.Location;
using Newtonsoft.Json.Linq;
using Windows.Devices.Geolocation;

namespace NyAppHelper.Http
{
    public class Forecast : ObservableObjectBase
    {
        private static RestClient client = new RestClient(AppSettings.WeatherUrl);

        private String _city = null;
        private String _weather = null;
        private String _temperature = null;

        public String City
        {
            get
            {
                return _city;
            }
            set
            {
                if (_city != value)
                {
                    _city = value;
                    this.NotifyPropertyChanged("City");
                }
            }
        }

        public String Weather
        {
            get
            {
                return _weather;
            }
            set
            {
                if (_weather != value)
                {
                    _weather = value;
                    this.NotifyPropertyChanged("Weather");
                }
            }
        }

        public String Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    this.NotifyPropertyChanged("Temperature");
                }
            }
        }

        public Forecast(String city, String weather, String temperature)
        {
            City = city;
            Weather = weather;
            Temperature = temperature;
        }

        public async static Task<Forecast> GetWeather()
        {

            //Geolocator myGeolocator = new Geolocator();
            Geoposition myGeoposition = await LocationHelper.GetLocation();
            if(null != myGeoposition)
            {
                var strCoor = myGeoposition.Coordinate.Longitude.ToString() + "," + myGeoposition.Coordinate.Latitude.ToString();

                try
                {
                    var result = await Forecast.FetchWeather(strCoor);
                    return result;
                }
                catch (AggregateException e)
                {
                    throw e;
                }
            }
             
            return null;
        }

        public static Task<Forecast> FetchWeather(String loc)
        {
            var request = new RestRequest("weather");
            request.AddParameter("location", loc);
            request.AddParameter("output","json");
            request.AddParameter("ak",AppSettings.BaiduApiKey);

            var tcs = new TaskCompletionSource<Forecast>();

            //client.Timeout = 10 * 1000;
            client.ExecuteAsync(request, response =>
            {
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    String res = response.Content;

                    try
                    {
                        JObject routeData = JObject.Parse(res);

                        var status = routeData["status"];
                        if (null != status && (String)status == "success")
                        {
                            String city = (String)routeData["results"][0]["currentCity"];
                            String weather = (String)routeData["results"][0]["weather_data"][0]["weather"];
                            String temperature = (String)routeData["results"][0]["weather_data"][0]["temperature"];
                            tcs.TrySetResult(new Forecast(city, weather, temperature));
                        }
                        else
                        {
                            tcs.TrySetResult(null);
                        }

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    tcs.TrySetResult(null);
                }

            });

            return tcs.Task;

        }
    }
}
