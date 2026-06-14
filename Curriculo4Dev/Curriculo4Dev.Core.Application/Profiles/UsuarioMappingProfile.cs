using AutoMapper;
using Curriculo4Dev.Core.Domain.DataTransferObjects;
using Curriculo4Dev.Core.Domain.Entities;

namespace Curriculo4Dev.Core.Application.Profiles
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            CreateMap<Usuario, UsuarioGetDto>();

            CreateMap<UsuarioAtributos, UsuarioGetAtributos>();

            CreateMap<Usuario, Usuario>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                {
                    if (srcMember is null) return false;

                    if (srcMember is string str && string.IsNullOrWhiteSpace(str)) return false;

                    return true;
                }));
        }
    }
}
