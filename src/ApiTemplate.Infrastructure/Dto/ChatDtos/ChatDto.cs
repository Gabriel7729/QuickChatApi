using ApiTemplate.Infrastructure.Dto.UserDtos;

namespace ApiTemplate.Infrastructure.Dto.ChatDtos;
public class ChatResponseDto : BaseResponseDto
{
  public bool IsGroupChat { get; set; }
  public string GroupName { get; set; } = string.Empty;
  public ICollection<UserResponseDto> Participants { get; set; } = new List<UserResponseDto>();
  public string LastMessage { get; set; } = string.Empty;
  public DateTime? LastMessageTimestamp { get; set; }
}

public class ChatDetailsResponseDto : BaseResponseDto
{
  public bool IsGroupChat { get; set; }
  public string? GroupName { get; set; }
  public List<UserResponseDto> Members { get; set; } = new List<UserResponseDto>();
  public List<string>? CommonGroups { get; set; }
}
