using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.NETCoreAPI
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    [Route("api/File")]
    public class FileController : Controller
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [Route("UploadImg")]
        [HttpPost]
        public IActionResult UploadImg(List<IFormFile> files)
        {
            if (files.Count < 1)
            {
                return Ok("文件为空");
            }
            //返回的文件地址
            List<string> filenames = new List<string>();
            var now = DateTime.Now;
            //获取文件存储路径
            var webRootPath = @"H:\Wangtiantian\资料\.NET Core\Web.NET Core.Demo\Web.NETCoreAPI\UploadFile\File\";
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }
            try
            {
                foreach (var item in files)
                {
                    if (item != null)
                    {
                        #region  图片文件的条件判断
                        //文件后缀
                        var fileExtension = Path.GetExtension(item.FileName);
                        //判断后缀是否是图片
                        const string fileFilt = ".gif|.jpg|.jpeg|.png";
                        if (fileExtension == null)
                        {
                            break;
                            //return Error("上传的文件没有后缀");
                        }
                        if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                        {
                            break;
                            //return Error("请上传jpg、png、gif格式的图片");
                        }
                        //判断文件大小    
                        long length = item.Length;
                        if (length > 1024 * 1024 * 2) //2M
                        {
                            break;
                            //return Error("上传的文件不能大于2M");
                        }
                        #endregion
                        var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                        var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                        var saveName = strDateTime + strRan + fileExtension;
                        using (FileStream fs = System.IO.File.Create(webRootPath + saveName))
                        {
                            item.CopyTo(fs);
                            fs.Flush();
                        }
                        filenames.Add(webRootPath + saveName);
                    }
                }
                return Ok(filenames);
            }
            catch (Exception ex)
            {
                LogOperation.WriteLog(ex.ToString());
                return Ok("上传失败");
            }
        }

        /// <summary>
        /// 文件上传，Base64
        /// </summary>
        /// <param name="fileBase64">Base64</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadBase64")]
        public IActionResult UploadBase64(string fileBase64, string fileName)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(fileBase64);
                var fileExtension = Path.GetExtension(fileName);
                var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff");
                var strRan = Convert.ToString(new Random().Next(100, 999));
                var saveName = strDateTime + strRan + fileExtension;
                var savePath = @"H:\Wangtiantian\资料\.NET Core\Web.NET Core.Demo\Web.NETCoreAPI\UploadFile\File\" + saveName;
                FileStream fs = new FileStream(savePath, FileMode.CreateNew);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
                return Ok(savePath);
            }
            catch (Exception ex)
            {
                LogOperation.WriteLog(ex.ToString());
                return Ok("上传失败！");
            }
        }
    }
}
