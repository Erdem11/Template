using AutoMapper;
using Template.Contracts.V1.Identity.Responses;
using Template.Domain.Identity;

namespace Template.Mappers
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            // Identity
            CreateMap<AuthResult, AuthResponse>();
        }
    }
}