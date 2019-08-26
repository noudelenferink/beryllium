namespace Beryllium.EntityFrameworkCore
{
   using Beryllium.Core;
   using Microsoft.EntityFrameworkCore;
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using System.Threading.Tasks;

   public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
   {
      private readonly RankixContext context;

      public BaseRepository(RankixContext context)
      {
         this.context = context;
      }

      public TEntity GetById(object id)
      {
         return this.context.Set<TEntity>().Find(id);
      }

      public IEnumerable<TEntity> GetAll()
      {
         return this.context.Set<TEntity>();
      }

      public async Task<TEntity> UpdateAsync(TEntity entityToUpdate)
      {
         var result = this.context.Set<TEntity>().Update(entityToUpdate);
         await this.context.SaveChangesAsync();

         return result.Entity;
      }

      public async Task<TEntity> Create(TEntity entity)
      {
         var result =  await this.context.Set<TEntity>().AddAsync(entity);
         await this.context.SaveChangesAsync();

         return result.Entity;
      }

      public async Task Delete(object id)
      {
         var entity = this.GetById(id);
         this.context.Set<TEntity>().Remove(entity);
         await this.context.SaveChangesAsync();
      }
   }
}
