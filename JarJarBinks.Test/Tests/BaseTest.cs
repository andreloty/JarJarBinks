using JarJarBinks.Infra.Context;
using JarJarBinks.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace JarJarBinks.Test.Tests
{
    public class BaseTest
    {
        protected MyContext ctx;

        protected void GetContext(bool useInMemory, bool resetDb = true)
        {
            if (useInMemory)
            {
                this.ctx = GetInMemoryDBContext(resetDb);
            }
            else
            {
                this.ctx = GetSqlServerDbContext(resetDb);
            }
        }

        private MyContext GetSqlServerDbContext(bool resetDb)
        {
            var builder = new DbContextOptionsBuilder<MyContext>();
            var options = builder.UseSqlServer("Server=localhost;Database=JarJarBinks;Trusted_Connection=true;MultipleActiveResultSets=true")
                                 .Options;

            var dbContext = new MyContext(options);
            
            if (resetDb)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated(); 
            }

            return dbContext;
        }

        private MyContext GetInMemoryDBContext(bool resetDb)
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<MyContext>();
            var options = builder.UseInMemoryDatabase("test")
                                 .UseInternalServiceProvider(serviceProvider)
                                 .Options;

            var dbContext = new MyContext(options);

            if (resetDb)
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }

            return dbContext;
        }
    }
}
