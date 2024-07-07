using ApiTemplate.Core.Entities.UserAggregate;
using ApiTemplate.Infrastructure.Dto.UserDtos;
using AutoMapper;

namespace ApiTemplate.Infrastructure.Config;
public class MainMapperProfile : Profile
{
  public MainMapperProfile()
  {
    //CreateMap<Project, ProjectDTO>().ReverseMap().ForMember(dto => dto.Items, config => config.MapFrom(entity => entity.Items));
    CreateMap<User, UserDto>().ReverseMap();
    CreateMap<User, UserResponseDto>().ReverseMap();
  }
}
