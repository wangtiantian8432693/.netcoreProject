using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        protected virtual RepositoryContext nWriteContext { get; set; }
        protected virtual RepositoryContext nReadContext { get; set; }
        public RepositoryBase(IServiceProvider serviceProvider, RepositoryContext studyDbContext)
        {
            this.nWriteContext = studyDbContext;
            this.nReadContext = this.nWriteContext;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(T model)
        {
            //对象添加到ef中
            var entry = nWriteContext.Add<T>(model);
            //保存
            return nWriteContext.SaveChanges();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Delete(T model)
        {
            //对象添加到ef中
            var entry = nWriteContext.Remove<T>(model);
            //保存
            return nWriteContext.SaveChanges();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update(T model)
        {
            //对象添加到ef中
            var entry = nWriteContext.Entry<T>(model);
            //保存
            return nWriteContext.SaveChanges();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTrack"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> qt = nReadContext.Set<T>();
            if (predicate != null)
            {
                var info = qt.Where(predicate).FirstOrDefault();
                return info;
            }
            return null;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTrack"></param>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public IQueryable<T> FindList(Expression<Func<T, bool>> predicate, bool isTrack, params Expression<Func<T, dynamic>>[] expressions)
        {
            IQueryable<T> qt = nReadContext.Set<T>();
            if (expressions != null && expressions.Any())
            {
                foreach (var exp in expressions)
                {
                    qt = qt.Include(exp);
                }
            }
            if (!isTrack)
            {
                qt = qt.AsNoTracking();
            }
            if (predicate != null)
            {
                qt = qt.Where(predicate);
            }
            return qt;
        }
    }
}
