using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IStudentService
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<GetStudentByIdDto>> QueryStudentsByClassNo(GetStudentByIdInput input);

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QueryPageByClassNoDto> QueryPageByClassNo(QueryPageByClassNoDtoInput input);



        #region base

        string Getoutref(out string outName, ref string refName);
   
        #endregion
    }
}
