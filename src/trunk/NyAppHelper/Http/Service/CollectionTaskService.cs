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
using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json.Serialization;

namespace NyAppHelper.Http
{

    /// <summary>
    /// 采集任务的Restful服务类
    /// </summary>
    public class CollectionTaskService : AppClientBase, IRestfulExecutable<CollectionTask>
    {

        public CollectionTaskService() : base() { }

        public Task<ObservableCollection<CollectionTask>> GetAllTasks()
        {
            var request = new RestRequest("CollectionTask");

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);
            ObservableCollection<CollectionTask> tasks = null;

            var taskCompletionSource = new TaskCompletionSource<ObservableCollection<CollectionTask>>();

            AppClient.ExecuteAsyncGet(request, (response, e) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    string resultJson = response.Content;
                    AppJsonSerializerSettings.MaxDepth = 4;
                    tasks = JsonConvert.DeserializeObject<ObservableCollection<CollectionTask>>(resultJson,AppJsonSerializerSettings);
                }
                taskCompletionSource.TrySetResult(tasks);
            }, "GET");


            return taskCompletionSource.Task;
        }

        public Task<CollectionTask> Sync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(CollectionTask t)
        {
            throw new NotImplementedException();
        }

        public Task<CollectionTask> Add(CollectionTask t)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 上传采集结果照片
        /// </summary>
        /// <param name="photos"></param>
        /// <returns></returns>
        public Task<bool> UploadResultImage(Photo photo)
        {
            bool isUploadResultImageSuccess = false;

            var request = new RestRequest("SubmitImage?type=" + photo.ExtensionField1 + "&resultid=" + photo.ID);

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);

            byte[] imageByte = null;

            using (var file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(photo.LocalUri, FileMode.Open, file))
                {
                    imageByte = new byte[stream.Length];
                    stream.Read(imageByte, 0, (int)stream.Length);
                }
            }

            request.AddParameter(new Parameter()
                {
                    Type = ParameterType.RequestBody,
                    Value = imageByte,
                    Name="Photo",
                }
                );
            //request.AddFile("image/jpeg", imageByte, photo.Tag);

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
                        isUploadResultImageSuccess = true;
                    }
                }
                taskCompletionSource.TrySetResult(isUploadResultImageSuccess);
            }, "POST");
            return taskCompletionSource.Task;
        }

        /// <summary>
        /// 上传采集结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public Task<bool> UploadResult(CollectionTaskResult result)
        {

            bool isUploadResultSuccess = false;

            var request = new RestRequest("SubmitCollectionTask?taskid=" + result.TaskId);

            request.AddHeader("UserId", base.UserId);
            request.AddHeader("Token", base.Token);

            if (result.CollectionResultDiseaseList.Count == 0)
            {
                result.CollectionResultDiseaseList = null;
            }
            if (result.CollectionResultPestList.Count == 0)
            {
                result.CollectionResultPestList = null;
            }
            if (result.CollectionResultWeedList.Count == 0)
            {
                result.CollectionResultWeedList = null;
            }

            AppJsonSerializerSettings.MaxDepth = 5;
            string postJson = JsonConvert.SerializeObject(result, Formatting.None, AppJsonSerializerSettings);

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
                        int resultId = int.Parse(json["CollectionResultID"].ToString());
                        result.CollectionResultID = resultId;
                        result.CollectionPointID = int.Parse(json["CollectionPointID"].ToString());
                        if (result.ExceptionID.HasValue)
                        {
                            result.ExceptionID = int.Parse(json["ExceptionID"].ToString());
                        }
                        if (result.CollectionResultDiseaseList!=null)
                        {
                            for (int i = 0; i < result.CollectionResultDiseaseList.Count; i++)
                            {
                                var item = result.CollectionResultDiseaseList[i];
                                var jsonItem = json["ResultDiseaseList"].ToList()[i];
                                item.CollectionResultID = resultId;
                                item.TabID = int.Parse(jsonItem["TabID"].ToString());
                                item.DiseaseCollectionResultID = int.Parse(jsonItem["ResultID"].ToString());
                            }
                        }
                        if (result.CollectionResultWeedList!=null)
                        {
                            for (int i = 0; i < result.CollectionResultWeedList.Count; i++)
                            {
                                var item = result.CollectionResultWeedList[i];
                                var jsonItem = json["ResultWeedList"].ToList()[i];
                                item.TabID = int.Parse(jsonItem["TabID"].ToString());
                                item.CollectionResultID = resultId;
                                item.WeedCollectionResultID = int.Parse(jsonItem["ResultID"].ToString());
                                item.CollectionResultID = resultId;
                            }
                        }
                        if (result.CollectionResultPestList!=null)
                        {
                            for (int i = 0; i < result.CollectionResultDiseaseList.Count; i++)
                            {
                                var item = result.CollectionResultPestList[i];
                                var jsonItem = json["ResultPestList"].ToList()[i];
                                item.TabID = int.Parse(jsonItem["TabID"].ToString());
                                item.CollectionResultID = resultId;
                                item.PestCollectionResultID = int.Parse(jsonItem["ResultID"].ToString());
                                item.CollectionResultID = resultId;
                            }
                        }
                        isUploadResultSuccess = true;
                    }
                }
                taskCompletionSource.TrySetResult(isUploadResultSuccess);
            }, "POST");
            return taskCompletionSource.Task;
        }

    }
}
