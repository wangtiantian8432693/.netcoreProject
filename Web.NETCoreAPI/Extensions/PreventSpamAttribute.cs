using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.NET_Core_API
{
    /// <summary>
    /// api安全阻止请求类
    /// </summary>
    public class PreventSpamAttribute : ActionFilterAttribute
    {
        //防止相同参数频繁请求
        //处理请求之间的延迟
        public int DelayRequest = 10;
        //防止多次请求时的错误提示信息
        public string ErrorMessage = "Excessive Request Attempts Detected.";
        IMemoryCache cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        /// <summary>
        /// 控制器中加了该属性的方法中代码执行之前该方法。
        /// 所以可以用做权限校验。
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //循环获取在Controller的Action方法中定义的参数
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                var parameterName = parameter.Name;//获取Action方法中参数的名字
                var parameterType = parameter.ParameterType;//获取Action方法中参数的类型

                //判断该Controller的Action方法是否有类型为LoginLogoutRequest的参数
                if (parameterType == typeof(GetStudentByIdInput))
                {
                    //如果有，那么就获取LoginLogoutRequest类型参数的值
                    var pointRequest = context.ActionArguments[parameterName] as GetStudentByIdInput;
                    if (cache.TryGetValue(pointRequest.ClassNo, out _))
                    {
                        ErrorMessage = "网络拥挤，请稍后重试";
                        context.Result = new JsonResult(new  { Code=-1, Msg=ErrorMessage });
                    }
                    else
                    {
                        // 缓存保存10s
                        cache.Set(pointRequest.ClassNo, pointRequest, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(DelayRequest)));
                    }
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
