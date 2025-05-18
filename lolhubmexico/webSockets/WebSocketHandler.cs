using System.Net.WebSockets;
using System.Text;
using System.Threading.Channels;

namespace LolHubMexico.API.WebSockets
{
    public class WebSocketHandler
    {
        private readonly WebSocketConnectionManager _connectionManager;

        public WebSocketHandler(WebSocketConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public async Task HandleConnectionAsync(int userId, WebSocket socket)
        {
            _connectionManager.AddSocket(userId, socket);

            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Conexión cerrada", CancellationToken.None);
                    _connectionManager.RemoveSocket(userId);
                }
                // Puedes manejar mensajes entrantes aquí si deseas
            }
        }
    }
}
