using System;
using Template.Common.Structs;
using Template.Entities.Types;

namespace Template.Entities.Abstract
{
    public interface ILanguage<TLocalizable>
    {
        public MyKey SourceId { get; set; }
        public TLocalizable Source { get; set; }
        public Languages Language { get; set; }
    }
}