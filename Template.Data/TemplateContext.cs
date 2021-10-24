using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Template.Common.Structs;
using Template.Entities.Abstract;
using Template.Entities.Concrete;
using Template.Entities.Concrete.IdentityModels;

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

    public class TemplateContext : IdentityDbContext<User, Role, MyKey, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public TemplateContext(DbContextOptions<TemplateContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookLanguage> BookLanguages { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            AddCustomKeyConversation(builder);

            base.OnModelCreating(builder);
        }

        private static void AddCustomKeyConversation(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            foreach (var property in entityType.ClrType.GetProperties())
                if (property.PropertyType == typeof(MyKey))
                    builder.Entity(entityType.ClrType)
                        .Property<MyKey>(property.Name)
                        .HasConversion(MyKey.Converter);
        }

        private static void AddCreatedDateToAddedEntities(ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is ICreatedAt createdAt)
                    {
                        createdAt.CreatedAt = DateTime.UtcNow;
                    }

                    if (entry.Entity is IMyKey || (entry.Entity?.GetType().BaseType != null && entry.Entity.GetType().BaseType.GenericTypeArguments.Contains(typeof(MyKey))))
                    {
                        SetDefaultMyKeyToNewId(entry.Entity);
                    }
                }
            }
        }
        private static void SetDefaultMyKeyToNewId(object entity)
        {
            foreach (var property in entity.GetType().GetProperties())
                if (property.PropertyType == typeof(MyKey))
                    if (property.GetValue(entity) is MyKey myKey)
                        if (myKey.IsDefault())
                            property.SetValue(entity, myKey.GenerateNewId());
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddCreatedDateToAddedEntities(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
        {
            AddCreatedDateToAddedEntities(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}