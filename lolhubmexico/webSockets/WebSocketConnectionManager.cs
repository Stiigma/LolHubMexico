using System.Net.WebSockets;
using System.Collections.Concurrent;

namespace LolHubMexico.WebSockets
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

        public void AddSocket(int userId, WebSocket socket)
        {
            _sockets[userId.ToString()] = socket;
        }

        public void RemoveSocket(int userId)
        {
            _sockets.TryRemove(userId.ToString(), out _);
        }

        public WebSocket? GetSocket(int userId)
        {
            return _sockets.TryGetValue(userId.ToString(), out var socket) ? socket : null;
        }
    }
}

