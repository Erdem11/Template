using System;
using Template.Common.Structs;
using Template.Common.Types;

namespace Template.Domain.Dto.Abstract
{
    public interface ILanguage
    {
        public Guid SourceId { get; set; }
        public Languages Language { get; set; }
    }
    
    public interface ILanguage<TLocalizable> : ILanguage
    {
        public TLocalizable Source { get; set; }
    }
}