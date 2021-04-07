using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
using System.IO;
using Common;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Web.NET_Core_API.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        private IStudentService _studentService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public HomeController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        /// <summary>
        /// 根据Id查询信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// 
        //[PreventSpamAttribute(DelayRequest = 31)]
        [Route("QuerysByClassNo")]
        [HttpPost]
        public async Task<List<GetStudentByIdDto>> QuerysByClassNo(GetStudentByIdInput input)
        {
            var key = "QuerysByClassNo-" + input.ClassNo;
            if (MemoryCacheHelper.Exists(key))
            {
                var list = MemoryCacheHelper.Get(key);
                return (List<GetStudentByIdDto>)list;
            }
            else
            {
                var students = await _studentService.QueryStudentsByClassNo(input);
                MemoryCacheHelper.Set(key, students, TimeSpan.FromMinutes(1));
                return students;
            }
        }

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// 
        [Route("QueryPageByClassNo")]
        [HttpPost]
        public async Task<QueryPageByClassNoDto> QueryPageByClassNo(QueryPageByClassNoDtoInput input)
        {
            var students = await _studentService.QueryPageByClassNo(input);
            return students;
        }

        /// <summary>
        ///  两台机器相加到10
        /// </summary>
        /// <returns></returns>
        [Route("GetNumber")]
        [HttpPost]
        public async Task<string> GetNumber()
        {
            try
            {
                var key = "number";
                int number = 0;
                if (MemoryCacheHelper.Exists(key))
                {
                    number = (int)MemoryCacheHelper.Get(key);
                }
                if (number > 10)
                {
                    return await Task.FromResult("已经到10了不再相加");
                }
                //1.客户端传值
                //2.存数据库
                //3.存redis缓存
                var num = new Random().Next(1, 1000);
                if (num % 2 == 0)
                {
                    number++;
                }
                else
                {
                    number++;
                }
                MemoryCacheHelper.Set(key, number, TimeSpan.FromMinutes(10));
                return await Task.FromResult(number.ToString());
            }
            catch (Exception ex)
            {
                LogOperation.WriteLog(ex.ToString());
                throw ex;
            }
        }

        #region File操作

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [Route("DownloadOrders")]
        [HttpPost]
        public IActionResult DownloadOrders()
        {
            var input = new GetStudentByIdInput();
            var list = _studentService.QueryStudentsByClassNo(input).Result;
            var path = "./UploadFile/Export/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var name = "测试.xlsx";
            path += name;

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                var sheet = workbook.CreateSheet("orders");
                var header = sheet.CreateRow(0);
                header.CreateCell(0).SetCellValue("ID");
                header.CreateCell(1).SetCellValue("Name");
                header.CreateCell(2).SetCellValue("CreateTime");
                var rowIndex = 1;
                foreach (var item in list)
                {
                    var datarow = sheet.CreateRow(rowIndex);
                    datarow.CreateCell(0).SetCellValue(item.ID);
                    datarow.CreateCell(1).SetCellValue(item.Name);
                    datarow.CreateCell(2).SetCellValue(item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    rowIndex++;
                }
                workbook.Write(fs);
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.ms-excel", "order.xlsx");
        }

        #endregion

    }
}