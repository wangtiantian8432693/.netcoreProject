using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    public class LogOperation
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="value"></param>
        public static void WriteLog(string value)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + "\\UploadFile\\File\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt";
                FileStream fileStream = new FileStream(path, FileMode.Create);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = reader.ReadLine();
                    byte[] info = new UTF8Encoding(true).GetBytes(value);
                    fileStream.Write(info, 0, info.Length);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
