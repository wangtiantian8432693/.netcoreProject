using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    /// <summary>
    /// 访问数据层
    /// </summary>
    public class RepositoryContext : DbContext
    {

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        //添加表实体
        public DbSet<Tb_Student> Tb_Student { get; set; }
    }
}
