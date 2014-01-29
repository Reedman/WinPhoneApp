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
    /// <summary>
    /// 行政区域的Restful访问层
    /// </summary>
    public class RegionService:AppClientBase
    {

        public RegionService() : base() { }

        public Task<ObservableCollection<Region>> GetRegionInfo()
        {
            var request = new RestRequest("GetAllRegions");

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);
            ObservableCollection<Region> regions = null;

            var taskCompletionSource = new TaskCompletionSource<ObservableCollection<Region>>();

            AppClient.ExecuteAsyncGet(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    regions = JsonConvert.DeserializeObject<ObservableCollection<Region>>(resultJson, AppJsonSerializerSettings);
                }
                taskCompletionSource.TrySetResult(regions);
            }, "GET");

            return taskCompletionSource.Task;
        }

    }
}
