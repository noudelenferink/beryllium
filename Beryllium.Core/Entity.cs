namespace Beryllium.Core
{
   using System.ComponentModel.DataAnnotations;

   public abstract class Entity
   {
      [Key]
      public virtual int Id { get; set; }
   }
}
