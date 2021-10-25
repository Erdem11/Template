using System.Collections.Generic;

namespace Template.Domain.Dto.Abstract
{
    public interface ILocalizable
    {
    }
    
    public interface ILocalizable<TLocalizable, TLanguage> : ILocalizable
        where TLanguage : class, IEntityBase, ILanguage<TLocalizable>
        where TLocalizable : class, IEntityBase, ILocalizable<TLocalizable, TLanguage>
    {
        public List<TLanguage> Languages { get; set; }
    }
}