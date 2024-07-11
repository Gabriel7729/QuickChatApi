using ApiTemplate.Infrastructure.Dto.UserDtos;

namespace ApiTemplate.Infrastructure.Dto.GroupDtos;
public class GroupChatResponseDto : BaseResponseDto
{
  public Guid ChatId { get; set; }
  public string GroupName { get; set; } = string.Empty;
  public List<UserResponseDto> Members { get; set; } = new List<UserResponseDto>();
}
public class GroupResponseDto : BaseResponseDto
{
  public string Name { get; set; } = string.Empty;
  public int TotalMembers { get; set; }
}
