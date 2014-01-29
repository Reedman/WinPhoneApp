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
    public class DictionaryService : AppClientBase
    {

        public DictionaryService() : base() { }


        /// <summary>
        /// 检查字典信息的版本信息
        /// </summary>
        /// <param name="lastedDate"></param>
        /// <param name="typeTag">检查的类型：0/1/2 (种植／区域／各种害)</param>
        /// <returns></returns>
        public Task<bool> IsLastedVersion(DateTime lastedDate, int typeTag)
        {
            var request = new RestRequest("DictionaryVersion/?type="+typeTag);

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);
            request.AddHeader("VersionTime ", lastedDate.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
            bool isLasted = false;

            var taskCompletionSource = new TaskCompletionSource<bool>();

            AppClient.ExecuteAsyncGet(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    JObject json = JObject.Parse(resultJson);
                    bool resultValue = bool.Parse(json["IsLatest"].ToString());
                    if (resultValue)
                    {
                        isLasted = true;
                    }
                }
                taskCompletionSource.TrySetResult(isLasted);
            }, "GET");

            return taskCompletionSource.Task;
        }

    }
}
