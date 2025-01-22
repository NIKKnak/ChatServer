using System.Net;
using System.Net.Sockets;

namespace ChatServer;

public class Server
{
    //test
    private TcpListener _server;
    private readonly List<ClientHandler> _clients = new();

    public void Start(int port)
    {
        _server = new TcpListener(IPAddress.Any, port);
        _server.Start();

        Console.WriteLine($"Сервер запущен на порту {port}. Ожидание подключений...");

        while (true)
        {
            var clientSocket = _server.AcceptTcpClient();
            var clientHandler = new ClientHandler(clientSocket, this);
            _clients.Add(clientHandler);

            Console.WriteLine("Новый клиент подключился.");
            var thread = new Thread(clientHandler.HandleClient);
            thread.Start();
        }
    }

    public void BroadcastMessage(string message, ClientHandler sender)
    {
        foreach (var client in _clients)
        {
            if (client != sender)
            {
                client.SendMessage(message);
            }
        }
    }

    public void RemoveClient(ClientHandler client)
    {
        _clients.Remove(client);
    }
}