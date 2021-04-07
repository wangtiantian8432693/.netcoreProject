using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 学生分页
    /// </summary>
    public class QueryPageByClassNoDtoInput
    {
        public int ClassNo { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
