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
    /// 农户田块的服务类
    /// </summary>
    public class FieldService : AppClientBase, IRestfulExecutable<Field>
    {
        public FieldService() : base() { }

        /// <summary>
        /// 获得农户的所有田块
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<ObservableCollection<Field>> GetFields()
        {
            var request = new RestRequest("Farmland");

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);
            ObservableCollection<Field> fields = null;

            var taskCompletionSource = new TaskCompletionSource<ObservableCollection<Field>>();

            AppClient.ExecuteAsyncGet(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;

                    AppJsonSerializerSettings.MaxDepth = 4;
                    fields = JsonConvert.DeserializeObject<ObservableCollection<Field>>(resultJson, AppJsonSerializerSettings);
                }
                taskCompletionSource.TrySetResult(fields);
            }, "GET");


            return taskCompletionSource.Task;
        }

        public Task<Field> Sync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(Field field)
        {
            bool isRemoveFiledSuccess = false;
            var request = new RestRequest("DeleteFarmland");

            request.AddParameter("FarmlandID", field.FarmlandID.ToString());
            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);

            var taskCompletionSource = new TaskCompletionSource<bool>();

            AppClient.ExecuteAsync(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    JObject json = JObject.Parse(resultJson);
                    int resultValue = int.Parse(json["Result"].ToString());
                    if (resultValue > 0)
                    {
                        isRemoveFiledSuccess = true;
                    }
                }
                taskCompletionSource.TrySetResult(isRemoveFiledSuccess);
            });
            return taskCompletionSource.Task;
        }

        public Task<bool> Update(Field field)
        {
            bool isUpdateFiledSuccess = false;
            var request = new RestRequest("UpdateFarmland");

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);

            field.OwnerID = int.Parse(base.UserId);

            AppJsonSerializerSettings.MaxDepth = 4;
            string postJson = JsonConvert.SerializeObject(field, Formatting.None, AppJsonSerializerSettings);

            request.AddParameter(new Parameter()
            {
                Name = "text/json",
                Type = ParameterType.RequestBody,
                Value = postJson,
            });

            var taskCompletionSource = new TaskCompletionSource<bool>();

            AppClient.ExecuteAsyncPost(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    JObject json = JObject.Parse(resultJson);
                    int resultValue = int.Parse(json["Result"].ToString());
                    if (resultValue == 1)
                    {
                        isUpdateFiledSuccess = true;
                    }
                }
                taskCompletionSource.TrySetResult(isUpdateFiledSuccess);
            }, "POST");
            return taskCompletionSource.Task;
        }

        public Task<Field> Add(Field field)
        {

            int fieldId = -1;
            var request = new RestRequest("UpdateFarmland");

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);

            field.OwnerID = int.Parse(base.UserId);

            AppJsonSerializerSettings.MaxDepth = 4;
            string postJson = JsonConvert.SerializeObject(field, Formatting.None, AppJsonSerializerSettings);

            request.AddParameter(new Parameter()
            {
                Name = "text/json",
                Type = ParameterType.RequestBody,
                Value = postJson,
            });

            var taskCompletionSource = new TaskCompletionSource<Field>();

            AppClient.ExecuteAsyncPost(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    JObject json = JObject.Parse(resultJson);
                    int resultValue = int.Parse(json["Result"].ToString());
                    if (resultValue > 0)
                    {
                        fieldId = int.Parse(json["FarmlandID"].ToString());
                        field.FarmlandID = fieldId;
                    }
                }
                taskCompletionSource.TrySetResult(field);
            }, "POST");
            return taskCompletionSource.Task;
        }

    }
}
