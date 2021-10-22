using System;
using System.Collections.Generic;

namespace Template.Entities.Abstract
{
    public interface ILocalizable<TLocalizable, TLanguage>
        where TLanguage : class, IEntityBase, ILanguage<TLocalizable> 
        where TLocalizable : class, IEntityBase, ILocalizable<TLocalizable, TLanguage> 
    {
        public List<TLanguage> Languages { get; set; }
    }
}