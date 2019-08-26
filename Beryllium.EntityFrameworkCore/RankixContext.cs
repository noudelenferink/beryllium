namespace Beryllium.EntityFrameworkCore
{
   using Beryllium.Core.Players;
   using Beryllium.Core.Trainings;
   using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.Metadata;
   using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
   using System.Collections.Generic;
   using System.Reflection;
   using System;

   public class RankixContext : DbContext
   {
      public RankixContext(DbContextOptions options)
          : base(options)
      {
      }

      public DbSet<Training> Trainings { get; set; }
      public DbSet<Player> Players { get; set; }
      public DbSet<TeamPlayer> TeamPlayers { get; set; }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
         base.OnModelCreating(modelBuilder);

         modelBuilder.Entity<PlayerTraining>()
            .HasKey(pt => new { pt.TrainingID, pt.PlayerID});

         modelBuilder.Entity<PlayerTraining>()
             .HasOne(pt => pt.Training)
             .WithMany(p => p.TrainingPlayers)
             .HasForeignKey(pt => pt.TrainingID);

         // Iterate over every DbSet<> found in the current DbContext
         foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
         {
            // Iterate over each property found on the Entity class
            foreach (IMutableProperty property in entityType.GetProperties())
            {
               if (property.PropertyInfo == null)
               {
                  continue;
               }

               if (property.IsPrimaryKey() && IsPrimaryKey(property.PropertyInfo))
               {
                  // At this point we know that the property is a primary key
                  // let's set it to AutoIncrement on insert.
                  modelBuilder.Entity(entityType.ClrType)
                              .Property(property.Name)
                              .ValueGeneratedOnAdd()
                              .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore;
               }
               else if (property.PropertyInfo.PropertyType.IsBoolean())
               {
                  // Since MySQL stores bool as tinyint, let's add a converter so the tinyint is treated as boolean
                  modelBuilder.Entity(entityType.ClrType)
                              .Property(property.Name)
                              .HasConversion(new BoolToZeroOneConverter<short>());
               }
            }

         };
      }

      private static bool IsPrimaryKey(PropertyInfo property)
      {
         var identityTypes = new List<Type> {
            typeof(short),
            typeof(int),
            typeof(long)
         };

         return property.Name.Equals("Id", StringComparison.CurrentCultureIgnoreCase) && identityTypes.Contains(property.PropertyType);
      }
   }
}
