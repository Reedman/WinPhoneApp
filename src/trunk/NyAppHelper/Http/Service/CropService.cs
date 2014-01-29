using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NyAppHelper.Model;
using System.Collections.ObjectModel;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;
using NyAppHelper.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace NyAppHelper.Http
{
    public class CropService:AppClientBase
    {

        public CropService() : base() { }

        public Task<ObservableCollection<Crop>> GetCropInfo()
        {
            var request = new RestRequest("GetAllCrops");

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);
            ObservableCollection<Crop> crops = null;

            var taskCompletionSource = new TaskCompletionSource<ObservableCollection<Crop>>();

            AppClient.ExecuteAsyncGet(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    crops = JsonConvert.DeserializeObject<ObservableCollection<Crop>>(resultJson, AppJsonSerializerSettings);
                }
                taskCompletionSource.TrySetResult(crops);
            }, "GET");

            return taskCompletionSource.Task;
        }

    }
}
