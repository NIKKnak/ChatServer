using System.Net.Sockets;
using System.Text;

namespace ChatServer;

public class ClientHandler
{
    private readonly TcpClient _client;
    private readonly Server _server;
    private readonly NetworkStream _stream;

    public ClientHandler(TcpClient client, Server server)
    {
        _client = client;
        _server = server;
        _stream = _client.GetStream();
    }

    public void HandleClient()
    {
        try
        {
            var buffer = new byte[1024];
            while (true)
            {
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Получено сообщение: " + message);

                _server.BroadcastMessage(message, this);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Клиент отключился.");
            _server.RemoveClient(this);
            _client.Close();
        }
    }

    public void SendMessage(string message)
    {
        try
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            _stream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
        }
    }
}