using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Template.Entities.Abstract;
using Template.Entities.Types;
using Template.Service.Abstract;

namespace Template.Service.Helpers
{
    public static class LocalizableServiceExtensions
    {
        public static TLocalizable GetWitLanguage<TLocalizable, TLanguage>(this ServiceBase<TLocalizable> service, Guid id)
            where TLanguage : EntityBase, ILanguage<TLocalizable>
            where TLocalizable : EntityBase, ILocalizable<TLocalizable, TLanguage>
        {
            var result = service.Context.Set<TLocalizable>()
                .Include(x => x.Languages)
                .FirstOrDefault(x => x.Id == id);

            return result;
        }
        public static TLocalizable GetWitLanguage<TLocalizable, TLanguage>(this ServiceBase<TLocalizable> service, Guid id, Languages language)
            where TLanguage : EntityBase, ILanguage<TLocalizable>
            where TLocalizable : EntityBase, ILocalizable<TLocalizable, TLanguage>
        {
            var result = service.Context.Set<TLocalizable>()
                .Include(x => x.Languages.FirstOrDefault(tLanguage => tLanguage.Language == language))
                .FirstOrDefault(x => x.Id == id);

            return result;
        }
    }
}