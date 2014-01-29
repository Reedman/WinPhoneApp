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
    public class PestService : AppClientBase
    {

        public PestService() : base() { }


        public Task<ObservableCollection<Pest>> GetAllPests()
        {
            var request = new RestRequest("GetAllPests");

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);
            ObservableCollection<Pest> pests = null;

            var taskCompletionSource = new TaskCompletionSource<ObservableCollection<Pest>>();

            AppClient.ExecuteAsyncGet(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    pests = JsonConvert.DeserializeObject<ObservableCollection<Pest>>(resultJson, AppJsonSerializerSettings);
                }
                taskCompletionSource.TrySetResult(pests);
            }, "GET");

            return taskCompletionSource.Task;
        }

    }
}
