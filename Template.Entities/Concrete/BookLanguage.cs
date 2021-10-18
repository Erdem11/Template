using Template.Entities.Abstract;
using Template.Entities.Types;

namespace Template.Entities.Concrete
{
    public class BookLanguage : EntityBase, ILanguage<Book>
    {
        public string Name { get; set; }
        
        public int SourceId { get; set; }
        public Book Source { get; set; }
        public Languages Language { get; set; }
    }
}