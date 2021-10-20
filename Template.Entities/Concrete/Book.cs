using System;
using System.Collections.Generic;
using Template.Entities.Abstract;

namespace Template.Entities.Concrete
{
    public class Book : EntityBase, ISoftDelete, ILocalizable<Book, BookLanguage>
    {
        public DateTime PublishDate { get; set; }

        public Guid AuthorId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<BookLanguage> Languages { get; set; }
        
        public virtual Author Author { get; set; }
    }
}