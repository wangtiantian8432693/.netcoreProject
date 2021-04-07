using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Common
{
    public class ExcelExtend
    {
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            var dt = new DataTable();

            //检查实体集合不能为空 todo:
            if (entitys == null || entitys.Count < 1)
            {
                return dt;
            }
            //取出第一个实体的所有Propertie
            var entityType = entitys[0].GetType();
            var entityProperties = entityType.GetProperties();

            for (var i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                var entityValues = new object[entityProperties.Length];
                for (var i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;

        }

        //获取列名委托方法
        public delegate string GetColumnName(string columnName);

        public static byte[] GetExcelForXLSX(DataTable dt, GetColumnName getColumnName)
        {
            var xssfworkbook = new XSSFWorkbook();
            var sheet = xssfworkbook.CreateSheet("Sheet");
            //表头
            var row = sheet.CreateRow(0);

            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var cell = row.CreateCell(i);
                //列名称,数据库中字段
                var columnName = dt.Columns[i].ColumnName;
                var convertColumnName = getColumnName(columnName);
                cell.SetCellValue(convertColumnName);
            }

            //数据
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var row1 = sheet.CreateRow(i + 1);
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    var cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            //转为字节数组
            var stream = new MemoryStream();
            xssfworkbook.Write(stream);
            var buf = stream.ToArray();
            return buf;

        }
    }
}
