using ApiTemplate.Core.Entities.ChatAggregate;
using ApiTemplate.Core.Entities.ChatAggregate.Specifications;
using ApiTemplate.Core.Models;
using ApiTemplate.SharedKernel.Interfaces;
using ApiTemplate.Web.Endpoints.ChatEndpoints;
using Ardalis.Result;
using Microsoft.AspNetCore.SignalR;

namespace ApiTemplate.Web.Hubs;

public interface IChatHub
{
  /// <summary>
  /// Get Chat Messages
  /// </summary>
  /// <returns></returns>
  Task GetChatMessages(GetChatMessagesRequest request);
}

public class ChatHub : Hub<IChatHub>
{
  private readonly IRepository<Chat> _chatRepository;
  protected IHubContext<ChatHub> _context;
  public ChatHub(IRepository<Chat> chatRepository, IHubContext<ChatHub> context)
  {
    _chatRepository = chatRepository; 
    _context = context;
  }
  public async Task GetChatMessages(GetChatMessagesRequest request)
  {
    GetMessagesWithSenderSpec getMessagesWithSenderSpec = new GetMessagesWithSenderSpec(request.ChatId, request.ContentFilter);
    var chat = await _chatRepository.GetBySpecAsync(getMessagesWithSenderSpec);
    if (chat == null)
    {
      await _context.Clients.All.SendAsync("ReceiveMessages", Result<List<MessageResponseDto>>.Error(new string[] { "Chat not found" }));
      return;
    }

    var messages = chat.Messages
      .Where(m => string.IsNullOrEmpty(request.ContentFilter) || m.Content.Contains(request.ContentFilter))
      .Select(m => new MessageResponseDto
      {
        Id = m.Id,
        Content = m.Content,
        Timestamp = m.Timestamp,
        SenderId = m.SenderId,
        SenderName = m.Sender.Name // Assuming that the Sender navigation property is loaded
      }).ToList().OrderBy(x => x.Timestamp);

    await _context.Clients.All.SendAsync("ReceiveMessages", messages);
  }
}
