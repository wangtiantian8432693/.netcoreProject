using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IRepositoryBase<T>
    {
      
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(T model);
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Delete(T model);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(T model);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTrack"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        IQueryable<T> FindList(Expression<Func<T, bool>> predicate, bool isTrack, params Expression<Func<T, dynamic>>[] expressions);
    }
}
