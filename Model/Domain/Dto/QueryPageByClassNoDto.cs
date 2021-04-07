using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 学生信息
    /// </summary>
    public class QueryPageByClassNoDto
    {
        /// <summary>
        /// 列表
        /// </summary>
        public List<GetStudentByIdDto> list { get; set; }

        /// <summary>
        /// 分页列表
        /// </summary>
        public PaginatedList<GetStudentByIdDto> pageList { get; set; }
    }
}
