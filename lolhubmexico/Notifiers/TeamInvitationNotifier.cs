using LolHubMexico.WebSockets;
using LolHubMexico.Domain.Notifications;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;


namespace LolHubMexico.Notifiers
{
    public class TeamInvitationNotifier : INotifier
    {
        private readonly WebSocketConnectionManager _manager;

        public TeamInvitationNotifier(WebSocketConnectionManager manager) => _manager = manager;

        public async Task NotifyAsync(int userId, object data)
        {
            var socket = _manager.GetSocket(userId);
            if (socket is { State: WebSocketState.Open })
            {
                var payload = JsonSerializer.Serialize(new
                {
                    Type = "TeamInvitation",
                    Data = data
                });

                var buffer = Encoding.UTF8.GetBytes(payload);
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
