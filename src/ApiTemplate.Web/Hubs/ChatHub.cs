using Microsoft.AspNetCore.SignalR;

namespace ApiTemplate.Web.Hubs;

public class ChatHub : Hub
{
  public async Task SendMessageToGroup(string groupName, string message)
  {
    await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
  }

  public async Task AddToGroup(string groupName)
  {
    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
  }

  public async Task RemoveFromGroup(string groupName)
  {
    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
  }
}
