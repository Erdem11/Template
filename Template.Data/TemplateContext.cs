using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Template.Common.SettingsConfigurationFiles;
using Template.Common.Structs;
using Template.Domain.Dto;
using Template.Domain.Dto.Abstract;
using Template.Domain.Dto.IdentityModels;

namespace Template.Data
{
    // installing dotnet ef to machine
    // dotnet tool install --global dotnet-ef

    // add migration
    // dotnet ef --startup-project ..\Template.Api migrations add InitialCreate --context TemplateContext

    // remove migration
    // dotnet ef --startup-project ..\Template.Api migrations remove --context TemplateContext

    // update database
    // dotnet ef --startup-project ..\Template.Api database update --context TemplateContext

    /// <summary>
    /// Primary DB
    /// </summary>
    public class TemplateContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly SettingsHolder _settingsHolder;
        public TemplateContext(DbContextOptions<TemplateContext> options, SettingsHolder settingsHolder)
            : base(options)
        {
            _settingsHolder = settingsHolder;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbOptions = _settingsHolder.SqlSettings.GetPrimary();
            
            switch (dbOptions.DbType)
            {
                case DbTypes.Mssql:
                    optionsBuilder.UseSqlServer(dbOptions.ConnectionString);
                    break;
                case DbTypes.Npgsql:
                    optionsBuilder.UseNpgsql(dbOptions.ConnectionString);
                    break;
                case DbTypes.Sqlite:
                    optionsBuilder.UseSqlite(dbOptions.ConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookLanguage> BookLanguages { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagLanguage> TagLanguages { get; set; }
    }
}