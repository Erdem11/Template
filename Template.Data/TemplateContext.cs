using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Template.Common.Structs;
using Template.Domain.Dto;
using Template.Domain.Dto.Abstract;
using Template.Domain.Dto.IdentityModels;

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
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagLanguage> TagLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            AddCustomKeyConversation(builder);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DoOperationsForChangeEntities(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new())
        {
            DoOperationsForChangeEntities(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
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

        private static void DoOperationsForEveryProperty(object entity)
        {
            foreach (var property in entity.GetType().GetProperties())
                if (property.PropertyType == typeof(MyKey))
                    if (property.GetValue(entity) is MyKey myKey)
                    {
                        if (property.Name == "SourceId")
                        {
                            var source = property.GetValue("Source");
                            var sourceId = source.GetType().GetProperty("Id").GetValue(source);
                            property.SetValue(entity, sourceId);
                            continue;
                        }
                        SetNewIdToMyKey(entity, myKey, property);
                    }
        }

        private static void SetNewIdToMyKey(object entity, MyKey myKey, PropertyInfo property)
        {
            if (!myKey.IsDefault())
                return;

            var newId = myKey.GenerateNewId();
            property.SetValue(entity, newId);
        }

        private static void DoOperationsForChangeEntities(ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    DoOperationsForAddedEntities(entry);
                }
            }
        }

        private static void DoOperationsForAddedEntities(EntityEntry entry)
        {
            if (entry.Entity is ICreatedAt createdAt)
            {
                createdAt.CreatedAt = DateTime.UtcNow;
            }

            var memberInfo = entry.Entity.GetType().BaseType;
            if (entry.Entity is IMyKey || (memberInfo != null && memberInfo.GenericTypeArguments.Contains(typeof(MyKey))))
            {
                DoOperationsForEveryProperty(entry.Entity);
            }
        }
    }
}