using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreRateLimit;

namespace Web.NET_Core_API
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 启动入口
        /// </summary>
        /// <param name="args"></param>
        public static   void Main(string[] args)
        {
            BuildWebHost(args).Run();

            IWebHost webHost = BuildWebHost(args);
            using (var scope = webHost.Services.CreateScope())
            {
                var ipPolicyStore = scope.ServiceProvider.GetRequiredService<IIpPolicyStore>();
                 ipPolicyStore.SeedAsync();
            }
             webHost.RunAsync();
        }

        //    1.CreateWebHostBuilder():
        //    构建web服务
        //    2.WebHost.CreateDefaulBuilder():
        //　　使用默认配置，包括
        //　　1.使用了Kestrel Web Server
        //　　2.集成了IIS
        //　　3.配置了Log
        //　　4.创建了实现IConfiguration接口的对象，该对象可获取appsettings.json文件配置信息
        //    3.UseStartup<Startup>()：使用Startup类来配置web应用
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
