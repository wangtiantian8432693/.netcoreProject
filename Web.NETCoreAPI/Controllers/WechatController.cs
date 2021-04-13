using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Common;
using Model.Domain.Dto;

namespace Web.NETCoreAPI.Controllers
{
    /// <summary>
    /// 微信
    /// </summary>
    [Route("api/Wechat")]
    public class WechatController : Controller
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        [Route("GetUserInfo")]
        [HttpPost]
        public IActionResult GetUserInfo(string openId)
        {
            //参数
            string postdata = JsonConvert.SerializeObject(new { });

            //---------第一步------------
            //appid=wx878696657f714582
            //secret=313ed037bf4e927a6d225317e775d1b9

            //---------第二步------------
            //获取access_token值
            string toekn = "";

            if (MemoryCacheHelper.Exists("access_token"))
            {
                toekn = MemoryCacheHelper.Get("access_token").ToString();
            }
            else
            {
                var tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx878696657f714582&secret=313ed037bf4e927a6d225317e775d1b9";
                var bupo = HttpClient.HttpPostWeChatApi(tokenUrl, postdata);
                TokenDto info = JsonConvert.DeserializeObject<TokenDto>(bupo);
                toekn = info.access_token;
                MemoryCacheHelper.Set("access_token", toekn, TimeSpan.FromMinutes(60));
            }

            //---------第三步------------
            //获取openId值
            if (string.IsNullOrEmpty(openId))
            {
                openId = "oXriy6Dh8McEmYtZJx4xr6GABuvU";
            }

            //---------第四步------------
            //获取用户基本信息
            string url = " https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + toekn + "&openid=" + openId + "&lang=zh_CN";
            var user = HttpClient.HttpPostWeChatApi(url, postdata);
            return Ok(user);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [Route("GetUserInfos")]
        [HttpPost]
        public IActionResult GetUserInfos()
        {
            //参数
            string postdata = JsonConvert.SerializeObject(new { });
            string token = "";
            if (MemoryCacheHelper.Exists("access_token"))
            {
                token = MemoryCacheHelper.Get("access_token").ToString();
            }
            else
            {
                var tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx878696657f714582&secret=313ed037bf4e927a6d225317e775d1b9";
                var bupo = HttpClient.HttpPostWeChatApi(tokenUrl, postdata);
                TokenDto tokenInfo = JsonConvert.DeserializeObject<TokenDto>(bupo);
                token = tokenInfo.access_token;
                MemoryCacheHelper.Set("access_token", token, TimeSpan.FromMinutes(60));
            }
            string url = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + token + "&next_openid=";
            var user = HttpClient.HttpPostWeChatApi(url, postdata);
            return Ok(user);
        }

        /// <summary>
        /// 添加客服
        /// </summary>
        /// <returns></returns>
        [Route("InsertCsd")]
        [HttpPost]
        public IActionResult InsertCsd()
        {
            string toekn = "";
            if (MemoryCacheHelper.Exists("access_token"))
            {
                toekn = MemoryCacheHelper.Get("access_token").ToString();
            }
            else
            {
                var tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx878696657f714582&secret=313ed037bf4e927a6d225317e775d1b9";
                var bupo = HttpClient.HttpPostWeChatApi(tokenUrl, JsonConvert.SerializeObject(new { }));
                TokenDto info = JsonConvert.DeserializeObject<TokenDto>(bupo);
                toekn = info.access_token;
                MemoryCacheHelper.Set("access_token", toekn, TimeSpan.FromMinutes(60));
            }
            //参数
            string postdata = JsonConvert.SerializeObject(new { kf_account = "test1@test", nickname = "客服1", password = "pswmd5" });
            string url = " https://api.weixin.qq.com/customservice/kfaccount/add?access_token=" + toekn;
            var result = HttpClient.HttpPostWeChatApi(url, postdata);
            return Ok(result);
        }

        /// <summary>
        /// 获取所有客服信息
        /// </summary>
        /// <returns></returns>
        [Route("QueryCsds")]
        [HttpPost]
        public IActionResult QueryCsds()
        {
            string toekn = "";
            
            //string url1 = "";
            //var result1 = HttpClient.HttpPostWeChatApi(url1, "");
            //LogOperation.WriteLog(result1);

            if (MemoryCacheHelper.Exists("access_token"))
            {
                toekn = MemoryCacheHelper.Get("access_token").ToString();
            }
            else
            {
                var tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx878696657f714582&secret=313ed037bf4e927a6d225317e775d1b9";
                var bupo = HttpClient.HttpPostWeChatApi(tokenUrl, JsonConvert.SerializeObject(new { }));
                TokenDto info = JsonConvert.DeserializeObject<TokenDto>(bupo);
                toekn = info.access_token;
                MemoryCacheHelper.Set("access_token", toekn, TimeSpan.FromMinutes(60));
            }
            //参数
            string postdata = JsonConvert.SerializeObject(new { });
            string url = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token=" + toekn;
            var result = HttpClient.HttpPostWeChatApi(url, postdata);
            return Ok(result);
        }
   
    }
}
