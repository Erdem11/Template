using System;
using Template.Common.Structs;
using Template.Common.Types;
using Template.Domain.Dto.Abstract;

namespace Template.Domain.Dto
{
    public class TagLanguage : EntityBase, ILanguage<Tag>
    {
        public string Name { get; set; }

        public Guid SourceId { get; set; }
        public Languages Language { get; set; }
        public Tag Source { get; set; }
    }
}