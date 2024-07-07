namespace ApiTemplate.Infrastructure.Dto.MessageDtos;
public class MessageResponseDto : BaseResponseDto
{
  public string Content { get; set; } = string.Empty;
  public DateTime Timestamp { get; set; }
  public Guid SenderId { get; set; }
  public string SenderName { get; set; } = string.Empty;
}

public class SendMessageResponseDto
{
  public Guid Id { get; set; }
  public string Content { get; set; } = string.Empty;
  public DateTime Timestamp { get; set; }
  public Guid SenderId { get; set; }
  public string SenderName { get; set; } = string.Empty;
}
