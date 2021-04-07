using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class StudentRepository : RepositoryBase<Tb_Student>, IStudentRepository
    {
        private static readonly RepositoryContext _repositoryContext;

        public StudentRepository(IServiceProvider serviceProvider, RepositoryContext studyDbContext):base(serviceProvider, studyDbContext)
        {
            
        }
    }
}
