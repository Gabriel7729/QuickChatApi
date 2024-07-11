using ApiTemplate.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiTemplate.Core.Interfaces;
public interface IChatService
{
  FileStreamResult GenerateChatMessagesReport(List<MessageResponseDto> messages);
}
