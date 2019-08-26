namespace Beryllium.Core
{
   using System.Collections.Generic;
   using System.Threading.Tasks;

   public interface IRepository<TEntity>
   {
      IEnumerable<TEntity> GetAll();
      TEntity GetById(object id);
      Task<TEntity> Create(TEntity entity);
      Task<TEntity> UpdateAsync(TEntity entityToUpdate);
      Task Delete(object id);
   }
}