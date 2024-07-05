using AutoMapper;

namespace ApiTemplate.Infrastructure.Config;
public class MainMapperProfile : Profile
{
  public MainMapperProfile()
  {
    //CreateMap<Project, ProjectDTO>().ReverseMap().ForMember(dto => dto.Items, config => config.MapFrom(entity => entity.Items));
  }
}
