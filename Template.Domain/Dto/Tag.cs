using System;
using System.Collections.Generic;
using Template.Common.Structs;
using Template.Domain.Dto.Abstract;
using Template.Domain.Dto.IdentityModels;

namespace Template.Domain.Dto
{
    public class Tag : EntityBase, ILocalizable<Tag, TagLanguage>
    {
        public Guid AddedUserId { get; set; }
        public virtual User AddedUser { get; set; }
        public List<TagLanguage> Languages { get; set; }
    }
}