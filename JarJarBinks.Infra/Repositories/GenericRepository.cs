using JarJarBinks.Domain.Entities;
using JarJarBinks.Domain.Interfaces;
using JarJarBinks.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JarJarBinks.Infra.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        public readonly MyContext dbContext;
        private readonly DbSet<TEntity> db;

        public GenericRepository(MyContext dbContext)
        {
            this.dbContext = dbContext;
            this.db = dbContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity, bool saveChanges = true)
        {
            await db.AddAsync(entity);

            await SaveChangesAsync(saveChanges);
        }

        public async Task DeleteAsync(int id, bool saveChanges = true)
        {
            var dbEntity = await db.FirstOrDefaultAsync(f => f.Id == id);
            if (dbEntity == null)
            {
                throw new Exception($"Entidade não encontrada com o id: {id}");
            }

            db.Remove(dbEntity);

            await SaveChangesAsync(saveChanges);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            var query = db.Where(predicate);
            return query;
        }

        public IQueryable<TEntity> GetAll()
        {
            var query = db.AsNoTracking();
            return query;
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            var query = await db.AsNoTracking().ToListAsync();
            return query;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var dbEntity = await db.FirstOrDefaultAsync(f => f.Id == id);
            return dbEntity;
        }

        public async Task UpdateAsync(TEntity entity, bool saveChanges = true)
        {
            //dbContext.Entry(entity).State = EntityState.Modified;
            db.Update(entity);

            await SaveChangesAsync(saveChanges);
        }

        public async Task SaveChangesAsync(bool confirm = true)
        {
            if (confirm)
            {
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
