using AutoMapper;
using Curriculo4Dev.Core.Application.DataTransferObjects.Templates;
using Curriculo4Dev.Core.Domain.Entities;

namespace Curriculo4Dev.Core.Application.Profiles
{
    public class TemplateMappingProfile : Profile
    {
        public TemplateMappingProfile()
        {
            CreateMap<Template, TemplateGetDto>().ReverseMap();
            CreateMap<TemplateAtributos, TemplateGetDtoAtributos>().ReverseMap();
        }
    }
}
