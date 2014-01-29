using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Phone.Net.NetworkInformation;
using NyAppHelper.Data;
using Newtonsoft.Json;
using System.Globalization;

namespace NyAppHelper.Http
{
    public class AppClientBase
    {

        /// <summary>
        /// AppSericeState的数据操作类
        /// </summary>
        private static AppServiceStateExecutor<string> _appServiceStateExcutor=new AppServiceStateExecutor<string>();

        /// <summary>
        /// App Restful服务客户端
        /// </summary>
        protected RestClient AppClient = new RestClient(AppSettings.RestUrl);

        protected JsonSerializerSettings AppJsonSerializerSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Culture = CultureInfo.InvariantCulture,
            DefaultValueHandling = DefaultValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            DateFormatString = "yyyy/MM/dd",
        };

        /// <summary>
        /// 用户的ID
        /// </summary>
        public string UserId
        {
            get
            {
                string userId = String.Empty;
                if (_appServiceStateExcutor.IsExists("uid"))
                {
                    userId= _appServiceStateExcutor.Get("uid");
                }
                return userId;
            }
        }

        /// <summary>
        /// 用户的Token，每次登录的时候更新
        /// </summary>
        public string Token
        {
            get
            {
                string token = String.Empty;
                if (_appServiceStateExcutor.IsExists("token"))
                {
                    token = _appServiceStateExcutor.Get("token");
                }
                return token;
            }
        }


        public AppClientBase()
        {
            AppClient.Timeout = 10000;
        }

        public static bool IsNetworkConnected()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }


    }
}
