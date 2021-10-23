using System;
using System.Collections.Generic;
using Template.Common.Structs;
using Template.Entities.Abstract;
using Template.Entities.Concrete.IdentityModels;

namespace Template.Entities.Concrete
{
    public class Book : EntityBase, ISoftDelete, ILocalizable<Book, BookLanguage>
    {
        public DateTime PublishDate { get; set; }

        public MyKey AuthorId { get; set; }
        public MyKey AddedUserId { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public List<BookLanguage> Languages { get; set; }
        
        public virtual Author Author { get; set; }
        public virtual User AddedUser { get; set; }
    }
}