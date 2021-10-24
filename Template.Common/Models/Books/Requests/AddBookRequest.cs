using System;
using System.Collections.Generic;
using Template.Common.Models.Types;
using Template.Common.Structs;

namespace Template.Common.Models.Books.Requests
{
    public class AddBookRequest
    {
        public DateTime PublishDate { get; set; }
        public Guid AuthorId { get; set; }
        public List<BookLanguageRequest> Languages { get; set; }
    }

    public class BookLanguageRequest
    {
        public Languages Language { get; set; }
        public string Name { get; set; }
    }
}