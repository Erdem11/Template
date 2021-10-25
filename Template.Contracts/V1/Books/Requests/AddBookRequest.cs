using System;
using System.Collections.Generic;

namespace Template.Contracts.V1.Books.Requests
{
    public class AddBookRequest
    {
        public DateTime PublishDate { get; set; }
        public Guid AuthorId { get; set; }
        public List<BookLanguageRequest> Languages { get; set; }
    }

}