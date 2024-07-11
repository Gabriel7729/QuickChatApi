namespace ApiTemplate.Core.Models;
public class MessageResponseDto
{
  public Guid Id { get; set; }
  public string Content { get; set; } = string.Empty;
  public DateTime Timestamp { get; set; }
  public Guid SenderId { get; set; }
  public string SenderName { get; set; } = string.Empty;
}
