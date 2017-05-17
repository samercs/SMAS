using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMAS.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SMAS.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<int>, int>, IDataContext
    {
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

#if DEBUG
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SMAS;Trusted_Connection=True;MultipleActiveResultSets=true");
            //optionsBuilder.UseSqlServer(Configuration.GetConnectionString("BloggingDatabase"));
        }
#endif

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Properties<decimal>().Configure(prop => prop.HasPrecision(18, 3));

            builder.Entity<EmailTemplate>(entity =>
            {
                entity.HasIndex(e => e.TemplateType).IsUnique();
            });
            base.OnModelCreating(builder);
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            var addedEntities = ChangeTracker.Entries()
                .Where(i => i.State == EntityState.Added)
                .Where(i => i.Entity is EntityBase);

            foreach (var entity in addedEntities)
            {
                ((EntityBase)entity.Entity).CreatedUtc = DateTime.UtcNow;
            }

            var modifiedEntities = ChangeTracker.Entries()
                .Where(i => i.State == EntityState.Modified)
                .Where(i => i.Entity is EntityBase);

            foreach (var entity in modifiedEntities)
            {
                ((EntityBase)entity.Entity).ModifiedUtc = DateTime.UtcNow;
            }

            //foreach (var entity in addedEntities)
            //{
            //    var createdUtc = entity.Entity.GetType().GetProperty("CreatedUtc");
            //    createdUtc?.SetValue(entity.Entity, DateTime.UtcNow);
            //}

            return await base.SaveChangesAsync();
        }
    }
}
