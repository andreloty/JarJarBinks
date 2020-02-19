using JarJarBinks.Domain.Entities;
using JarJarBinks.Domain.Interfaces;
using JarJarBinks.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace JarJarBinks.Infra.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MyContext dbContext) : base(dbContext)
        {
        }
    }
}
