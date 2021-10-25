using System;
using System.Collections.Generic;
using Template.Common.Structs;
using Template.Domain.Dto.Abstract;
using Template.Domain.Dto.IdentityModels;

namespace Template.Domain.Dto
{
    public class Book : EntityBase, ISoftDelete, ILocalizable<Book, BookLanguage>
    {
        public DateTime PublishDate { get; set; }

        public MyKey AuthorId { get; set; }
        public MyKey AddedUserId { get; set; }

        public virtual Author Author { get; set; }
        public virtual User AddedUser { get; set; }
        public List<BookLanguage> Languages { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}