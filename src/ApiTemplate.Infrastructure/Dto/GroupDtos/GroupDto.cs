using ApiTemplate.Infrastructure.Dto.UserDtos;

namespace ApiTemplate.Infrastructure.Dto.GroupDtos;
public class GroupChatResponseDto : BaseResponseDto
{
  public Guid ChatId { get; set; }
  public string GroupName { get; set; } = string.Empty;
  public List<UserResponseDto> Members { get; set; } = new List<UserResponseDto>();
}
