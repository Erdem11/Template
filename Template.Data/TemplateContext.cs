using Microsoft.EntityFrameworkCore;
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
    
    public class TemplateContext : DbContext
    {
        public TemplateContext(DbContextOptions<TemplateContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookLanguage> BookLanguages { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}