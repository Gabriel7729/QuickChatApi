using ApiTemplate.Core.Abstracts;
using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.SharedKernel.Interfaces;
using Ardalis.ApiEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiTemplate.Web.Endpoints.ChatEndpoints;

[Authorize]
public class DeleteChat : EndpointBaseAsync
  .WithRequest<Guid>
  .WithActionResult<DeleteResponse>
{
  private readonly IRepository<Chat> _chatRepository;

  public DeleteChat(IRepository<Chat> chatRepository)
  {
    _chatRepository = chatRepository;
  }

  [HttpDelete("/api/Chat/{chatId:guid}")]
  [SwaggerOperation(
          Summary = "Delete a chat",
          Description = "Delete a chat by chat ID",
          OperationId = "Chat.Delete",
          Tags = new[] { "ChatEndpoints" })
  ]
  public override async Task<ActionResult<DeleteResponse>> HandleAsync([FromRoute] Guid chatId, CancellationToken cancellationToken = default)
  {
    try
    {
      var chat = await _chatRepository.GetByIdAsync(chatId, cancellationToken);
      if (chat == null)
        return NotFound(Result<DeleteResponse>.Error("Chat not found"));

      await _chatRepository.DeleteAsync(chat, cancellationToken);
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(Result<DeleteResponse>.Error(new string[] { ex.Message }));
    }
  }
}
