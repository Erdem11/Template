using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Template.Common.Structs;
using Template.Common.Types;
using Template.Domain.Dto.Abstract;

namespace Template.Service.Helpers
{
    public static class LocalizableServiceExtensions
    {
        public static TLocalizable GetWitLanguage<TLocalizable, TLanguage>(this ServiceBase<TLocalizable> service, Guid id)
            where TLanguage : class, IEntityBase, ILanguage<TLocalizable>
            where TLocalizable : class, IEntityBase, ILocalizable<TLocalizable, TLanguage>
        {
            var result = service.Context.Set<TLocalizable>()
                .Include(x => x.Languages)
                .FirstOrDefault(x => x.Id == id);

            return result;
        }
        public static TLocalizable GetWitLanguage<TLocalizable, TLanguage>(this ServiceBase<TLocalizable> service, Guid id, Languages language)
            where TLanguage : class, IEntityBase, ILanguage<TLocalizable>
            where TLocalizable : class, IEntityBase, ILocalizable<TLocalizable, TLanguage>
        {
            var result = service.Context.Set<TLocalizable>()
                .Include(x => x.Languages.FirstOrDefault(tLanguage => tLanguage.Language == language))
                .FirstOrDefault(x => x.Id == id);

            return result;
        }
    }
}