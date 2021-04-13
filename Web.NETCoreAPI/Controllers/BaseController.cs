using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.NETCoreAPI.Controllers
{
    /// <summary>
    /// 基础
    /// </summary>
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        private IStudentService _studentService;

        /// <summary>
        /// base controller
        /// </summary>
        /// <param name="studentService"></param>
        public BaseController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// out：只出不进 ref：有进有出
        /// </summary>
        /// <returns></returns>
        [Route("Getoutref")]
        [HttpPost]
        public async Task<string> Getoutref()
        {
            string outName = "out";
            string refName = "ref";
            var info = _studentService.Getoutref(out outName, ref refName);
            return await Task.FromResult(info);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [Route("QueryList")]
        [HttpPost]
        public async Task<List<QueryListDto>> QueryList()
        {
            var list = new List<QueryListDto>();
            list.Add(new QueryListDto { ID = 1, Name = "张三0" });
            list.Add(new QueryListDto { ID = 2, Name = "张三1" });
            list.Add(new QueryListDto { ID = 3, Name = "张三2" });
            list.Add(new QueryListDto { ID = 4, Name = "张三3" });

            //根据名称获取当前索引
            int i = list.Select(t => t.Name).ToList().BinarySearch("张三2");

            return await Task.FromResult(new List<QueryListDto>());
        }

        /// <summary>
        ///Viewmodel of query list
        /// </summary>
        public class QueryListDto
        {
            /// <summary>
            /// ID
            /// </summary>
            public int ID { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Name { get; set; }
        }
    }
}
