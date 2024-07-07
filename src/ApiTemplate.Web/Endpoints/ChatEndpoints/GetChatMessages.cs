using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Core.Entities.ChatAggregate.Specifications;
using ApiTemplate.Infrastructure.Dto.MessageDtos;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

[Authorize]
public class GetChatMessages : EndpointBaseAsync
  .WithRequest<GetChatMessagesRequest>
  .WithActionResult<List<MessageResponseDto>>
{
  private readonly IRepository<Chat> _chatRepository;

  public GetChatMessages(IRepository<Chat> chatRepository)
  {
    _chatRepository = chatRepository;
  }

  [HttpGet("/api/Chat/{chatId:guid}/Messages")]
  [SwaggerOperation(
          Summary = "Get the messages of a chat",
          Description = "Get the messages of a chat by chat ID",
          OperationId = "Chat.GetMessages",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<List<MessageResponseDto>>> HandleAsync([FromRoute] GetChatMessagesRequest request, CancellationToken cancellationToken = default)
  {
    try
    {
      GetMessagesWithSenderSpec getMessagesWithSenderSpec = new GetMessagesWithSenderSpec(request.ChatId, request.ContentFilter);
      var chat = await _chatRepository.GetBySpecAsync(getMessagesWithSenderSpec, cancellationToken);
      if (chat == null)
        return NotFound(Result<List<MessageResponseDto>>.Error(new string[] { "Chat not found" }));

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

      return Ok(Result<List<MessageResponseDto>>.Success(messages));
    }
    catch (Exception ex)
    {
      return BadRequest(Result<List<MessageResponseDto>>.Error(new string[] { ex.Message }));
    }
  }
}
