using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Core.Entities.ChatAggregate.Specifications;
using ApiTemplate.Core.Interfaces;
using ApiTemplate.Core.Models;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

[Authorize]
public class GetChatMessagesReport : EndpointBaseAsync
  .WithRequest<GetChatMessagesRequest>
  .WithoutResult
{
  private readonly IRepository<Chat> _chatRepository;
  private readonly IChatService _chatService;

  public GetChatMessagesReport(IRepository<Chat> chatRepository, IChatService chatService)
  {
    _chatRepository = chatRepository;
    _chatService = chatService;
  }

  [HttpPost("/api/Chat/{chatId:guid}/Messages/Report")]
  [SwaggerOperation(
          Summary = "Get the messages report of a chat",
          Description = "Get the messages report of a chat by chat ID",
          OperationId = "Chat.GetMessagesReport",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult> HandleAsync([FromRoute] GetChatMessagesRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      GetMessagesWithSenderSpec getMessagesWithSenderSpec = new GetMessagesWithSenderSpec(request.ChatId, request.ContentFilter);
      var chat = await _chatRepository.GetBySpecAsync(getMessagesWithSenderSpec, cancellationToken);
      if (chat == null)
        return NotFound(Result<FileStreamResult>.Error("Chat not found"));

      var messages = chat.Messages
        .Where(m => string.IsNullOrEmpty(request.ContentFilter) || m.Content.Contains(request.ContentFilter))
        .Select(m => new MessageResponseDto
        {
          Id = m.Id,
          Content = m.Content,
          Timestamp = m.Timestamp,
          SenderId = m.SenderId,
          SenderName = m.Sender.Name // Assuming that the Sender navigation property is loaded
        }).ToList();

      var file = _chatService.GenerateChatMessagesReport(messages);
      return File(file.FileStream, file.ContentType);
    }
    catch (Exception ex)
    {
      return BadRequest(Result<FileStreamResult>.Error(ex.Message));
    }
  }
}
