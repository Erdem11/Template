using System.Collections.Generic;
using AutoMapper;
using Template.Common.Structs;
using Template.Contracts.V1;
using Template.Contracts.V1.Books.Requests;
using Template.Contracts.V1.Tag;
using Template.Domain.Dto;

namespace Template.Mappers
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            // Global
            CreateMap<PaginationQuery, PaginationFilter>();
            
            // Book
            CreateMap<AddBookRequest, Book>()
                .ForMember(x => x.Languages, o => o.MapFrom(x=>x.Languages));
            CreateMap<BookLanguageRequest, BookLanguage>();

            // Tag
            CreateMap<AddTagLanguageRequest, TagLanguage>();
            CreateMap<AddTagRequest, List<TagLanguage>>();
        }
    }
}