using Template.Entities.Types;

namespace Template.Entities.Abstract
{
    public interface ILanguage<TLocalizable>
    {
        public int SourceId { get; set; }
        public TLocalizable Source { get; set; }
        public Languages Language { get; set; }
    }
}