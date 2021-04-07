using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Common;
using AutoMapper;

namespace Service
{
    /// <summary>
    /// 日志
    /// </summary>
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _IStudentRepository;
        public StudentService(IStudentRepository IStudentRepository)
        {
            _IStudentRepository = IStudentRepository;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<GetStudentByIdDto>> QueryStudentsByClassNo(GetStudentByIdInput input)
        {
            var students = _IStudentRepository.FindList(null, true).ToList();
            if (input.ClassNo > 0)
            {
                students = _IStudentRepository.FindList(t => t.ClassNo == input.ClassNo, true).ToList();
            }
            var list = students.MapTo<Tb_Student, GetStudentByIdDto>();
            return await Task.FromResult(list);
        }

        /// <summary>
        /// 查询分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<QueryPageByClassNoDto> QueryPageByClassNo(QueryPageByClassNoDtoInput input)
        {
            try
            {
                var students = _IStudentRepository.FindList(null, true);
                if (input.ClassNo > 0)
                {
                    students = students.Where(t => t.ClassNo == input.ClassNo);
                }
                var list = students.MapTo<Tb_Student, GetStudentByIdDto>();
                var pageList = PaginatedList<GetStudentByIdDto>.CreatepagingAsync(list, input.PageIndex, input.PageSize);
                return await Task.FromResult(new QueryPageByClassNoDto() { list = list, pageList = pageList });
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            
        }

        #region base

        public string Getoutref(out string outName, ref string refName)
        {
            outName = "out：只出不进 ";
            refName = "ref：有进有出";
            return outName + refName;
        }

        #endregion
    }
}
