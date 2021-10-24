using Template.Common.Models.Types;
using Template.Common.Structs;

namespace Template.Entities.Abstract
{
    public interface ILanguage<TLocalizable>
    {
        public MyKey SourceId { get; set; }
        public TLocalizable Source { get; set; }
        public Languages Language { get; set; }
    }
}