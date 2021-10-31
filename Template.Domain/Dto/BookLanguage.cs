using System;
using Template.Common.Structs;
using Template.Common.Types;
using Template.Domain.Dto.Abstract;

namespace Template.Domain.Dto
{
    public class BookLanguage : EntityBase, ILanguage<Book>
    {
        public string Name { get; set; }

        public Guid SourceId { get; set; }
        public Book Source { get; set; }
        public Languages Language { get; set; }
    }
}