using System.Collections.Generic;

namespace Template.Entities.Abstract
{
    public interface ILocalizable<TLocalizable, TLanguage>
        where TLanguage : EntityBase, ILanguage<TLocalizable>
        where TLocalizable : EntityBase, ILocalizable<TLocalizable, TLanguage>
    {
        public List<TLanguage> Languages { get; set; }
    }
}