using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using NyAppHelper.Data;

namespace NyAppHelper.Model
{

    /// <summary>
    /// 用户
    /// </summary>
    public class User 
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户访问服务的AccessToken
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 用户类型
        /// 4 是采集员
        /// 5 是农户
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 登录结果
        /// 0表示失败
        /// 1表示成功
        /// </summary>
        public int Result { get; set; }

    }

}
