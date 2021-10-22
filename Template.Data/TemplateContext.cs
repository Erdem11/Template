using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Entities.Abstract;
using Template.Entities.Concrete;

namespace Template.Data
{
    // installing dotnet ef to machine
    // dotnet tool install --global dotnet-ef

    // add migration
    // dotnet ef --startup-project ..\Template.Api migrations add InitialCreate

    // remove migration
    // dotnet ef --startup-project ..\Template.Api migrations remove

    // update database
    // dotnet ef --startup-project ..\Template.Api database update

    public class TemplateContext : IdentityDbContext<User, CustomUserRole, Guid>
    {
        public TemplateContext(DbContextOptions<TemplateContext> options)
            : base(options)
        {
        }

        private void AddCreatedDateToAddedEntities()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is not IEntityBase entity)
                    continue;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddCreatedDateToAddedEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AddCreatedDateToAddedEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookLanguage> BookLanguages { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}