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
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NyAppHelper.Data;

namespace NyAppHelper.Http
{
    /// <summary>
    /// 用户的Restful服务类
    /// </summary>
    public class UserService:AppClientBase
    {

        public UserService():base(){}

        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="userPass">密码</param>
        /// <returns></returns>
        public Task<User> AuthanticateUser(string userName,string userPass)
        {
            var request = new RestRequest("Authentication");
            var passMD5 = MD5Core.GetHashString(userPass).ToLowerInvariant();

            request.AddHeader("UserName", userName);
            request.AddHeader("Key", HMACSHAEncryptor.GetHASMString(passMD5));

            var taskCompletionSource=new TaskCompletionSource<User>();

            User user = null;

            AppClient.ExecuteAsyncGet(request,(response,e)=>{
            
                if(response.StatusCode==HttpStatusCode.OK)
                {
                    response.ContentEncoding = "utf-8";
                    var resultJson = response.Content;
                    JObject jObject = JObject.Parse(resultJson);
                    var loginResult =int.Parse(jObject["Result"].ToString());
                    if (loginResult > 0)
                    {
                        user = new JsonParser().Deserialize<User>(resultJson);
                    }
                }
                taskCompletionSource.TrySetResult(user);
            },"Get");
            return taskCompletionSource.Task;
        }

    }
}
