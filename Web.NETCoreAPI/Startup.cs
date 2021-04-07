
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Service;
using System;
using System.IO;
using Model;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;

namespace Web.NET_Core_API
{
    /// <summary>
    /// 　Startup类的执行顺序：构造 -> ConfigureServices -> Configure
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 服务注册：配置应用需要的服务、将服务注册到容器中，可以是第三方组件配置依赖注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //将MVC服务添加到指定的Microsoft.Extensions.DependencyInjection。IServiceCollection。
            services.AddMvc();

            //UI配置
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "api文档",
                });
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Web.NETCoreAPI.xml");
                var xmlPathByModel = Path.Combine(basePath, "Model.xml");
                options.IncludeXmlComments(xmlPathByModel);
                //true表示生成控制器描述，包含true的IncludeXmlComments重载应放在最后，或者两句都使用true
                options.IncludeXmlComments(xmlPath, true);
            });

            //添加IOC注册
            services.AddTransient<IStudentService, StudentService>();
            services.AddTransient<IStudentRepository, StudentRepository>();

            //sqlserver数据库连接字符串
            var conn = Configuration.GetConnectionString("SqlServerConnection");
            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(conn));

            //需要从appsettings.json中加载配置
            services.AddOptions();
            // 存储IP计数器及配置规则
            services.AddMemoryCache();
            var ipRateLimiting = Configuration.GetSection("IpRateLimiting");
            services.Configure<IpRateLimitOptions>(ipRateLimiting);
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            //按照文档，这两个是3.x版的breaking change，要加上
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            //调用AddMemoryCache服务  
            services.AddMemoryCache();
        }

        /// <summary>
        /// 中间件：创建应用的请求处理处理管道
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //启用默认文件夹wwwroot
            app.UseStaticFiles();

            //启动SwaggerUI
            app.UseSwagger();
            app.UseSwaggerUI(action =>
            {
                action.ShowExtensions();
                action.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
            });

            //注意顺序放UseMvc上面
            app.UseIpRateLimiting();

            //路由
            app.UseMvc();
        }
    }
}
