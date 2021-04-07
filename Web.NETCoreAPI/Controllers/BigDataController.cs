using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Data.SqlClient;
using Model;
using Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.NETCoreAPI.Controllers
{
    /// <summary>
    /// 批量操作数据
    /// </summary>
    [Route("api/BigData")]
    public class BigDataController : Controller
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string connString = "Server=172.16.20.150;Database=Test; User=sa;Password=123456;";

        /// <summary>
        /// 批量新增信息（datatable底层 SqlDataAdapter Update 内存映射）
        /// </summary>
        /// <returns></returns>
        [Route("Inserts")]
        [HttpPost]
        public bool Inserts()
        {
            var list = new List<Tb_Student>();
            for (int i = 20; i < 31; i++)
            {
                var student = new Tb_Student() { ID = i, Name = "张三" + i, Age = 30, CreateTime = DateTime.Now, ClassNo = 31 };
                list.Add(student);
            }
            //添加
            var dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
            new DataColumn("Name",typeof(string)),
            new DataColumn("Age",typeof(int)),
            new DataColumn("CreateTime",typeof(DateTime)),
            new DataColumn("ClassNo",typeof(int)),
            });
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                dt.Rows.Add(item.Name, item.Age, item.CreateTime, item.ClassNo);
            }
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sql = string.Format("select {0} from {1} where id=0", "*", "Tb_Student");
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(sql, conn);
                    SqlCommandBuilder scb = new SqlCommandBuilder(adapter);
                    scb.ConflictOption = ConflictOption.OverwriteChanges;
                    scb.SetAllValues = true;
                    adapter.Update(dt);
                    dt.AcceptChanges();
                    adapter.Dispose();
                }
                catch (SqlException e)
                {
                    conn.Close();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将datatable写入数据库
        /// </summary>
        /// <returns></returns>
        [Route("InsertInfos")]
        [HttpPost]
        public bool InsertInfos()
        {
            var list = new List<Tb_Student>();
            for (int i = 60; i < 70; i++)
            {
                var student = new Tb_Student() { ID = i, Name = "张三" + i, Age = 30, CreateTime = DateTime.Now, ClassNo = 31 };
                list.Add(student);
            }
            //添加
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{
            new DataColumn("Name",typeof(string)),
            new DataColumn("Age",typeof(int)),
            new DataColumn("CreateTime",typeof(DateTime)),
            new DataColumn("ClassNo",typeof(int)),
            });
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                dt.Rows.Add(item.Name, item.Age, item.CreateTime, item.ClassNo);
            }
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connString;
            conn.Open();
            SqlTransaction sqlbulkTransaction = conn.BeginTransaction();
            //请在插入数据的同时检查约束，如果发生错误调用sqlbulkTransaction事务  
            SqlBulkCopy copy = new SqlBulkCopy(conn, SqlBulkCopyOptions.CheckConstraints, sqlbulkTransaction);
            copy.DestinationTableName = "Tb_Student";
            foreach (DataColumn dc in dt.Columns)
            {
                copy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
            }
            try
            {
                copy.WriteToServer(dt);
                sqlbulkTransaction.Commit();
            }
            catch (Exception ex)
            {
                sqlbulkTransaction.Rollback();
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                copy.Close();
                conn.Close();
            }
            return true;
        }

        /// <summary>
        /// 批量更新信息（datatable底层 SqlDataAdapter Update 内存映射）
        /// </summary>
        /// <returns></returns>
        [Route("Updatas")]
        [HttpPost]
        public bool Updatas()
        {
            var list = new List<Tb_Student>();
            for (int i = 20; i < 31; i++)
            {
                var student = new Tb_Student() { ID = i, Name = "张三" + i, Age = 30, CreateTime = DateTime.Now, ClassNo = 31 };
                list.Add(student);
            }
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string sql = string.Format("select {0} from {1}", "ID,Name,Age,CreateTime,ClassNo", "Tb_Student");
                try
                {
                    DataSet ds = new DataSet();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(sql, conn);
                    SqlCommandBuilder scb = new SqlCommandBuilder(adapter);
                    scb.ConflictOption = ConflictOption.OverwriteChanges;
                    scb.SetAllValues = true;
                    adapter.Fill(ds);
                    var dtInfo = ds.Tables[0];
                    foreach (var item in list)
                    {
                        var info = dtInfo.Select("ID=" + item.ID).FirstOrDefault();
                        if (info != null)
                        {
                            info["Name"] = item.Name;
                            info["Age"] = item.Age;
                        }
                    }
                    adapter.Update(dtInfo);
                    adapter.Dispose();
                }
                catch (SqlException e)
                {
                    conn.Close();
                    return false;
                }
            }
            return true;
        }



    }
}
