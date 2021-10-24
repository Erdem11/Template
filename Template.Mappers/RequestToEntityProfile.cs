using System.Collections.Generic;
using AutoMapper;
using Template.Common.Models.Books.Requests;
using Template.Common.Models.Types;
using Template.Entities.Concrete;

namespace Template.Mappers
{
    public class RequestToEntityProfile : Profile
    {
        public RequestToEntityProfile()
        {
            CreateMap<AddBookRequest, Book>()
                .ForMember(x => x.Languages, o => o.MapFrom(x=>x.Languages));
            
            CreateMap<BookLanguageRequest, BookLanguage>();
        }
    }
}