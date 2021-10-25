using AutoMapper;
using Template.Contracts.V1.Books.Requests;
using Template.Domain.Dto;

namespace Template.Mappers
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<AddBookRequest, Book>()
                .ForMember(x => x.Languages, o => o.MapFrom(x=>x.Languages));
            
            CreateMap<BookLanguageRequest, BookLanguage>();
        }
    }
}