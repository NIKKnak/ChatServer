using System.Net.Sockets;
using System.Text;

namespace ChatClientApp;

public class ChatClient
{
    private TcpClient _client;
    private NetworkStream _stream;

    public void Connect(string serverIp, int port)
    {
        try
        {
            _client = new TcpClient(serverIp, port);
            _stream = _client.GetStream();

            Console.WriteLine("Подключено к серверу. Вы можете начинать переписку.");

            var receiveThread = new Thread(ListenForMessages);
            receiveThread.Start();

            SendMessages();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка подключения: {ex.Message}");
        }
    }

    private void SendMessages()
    {
        while (true)
        {
            string message = Console.ReadLine();
            if (string.IsNullOrEmpty(message)) continue;

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            _stream.Write(buffer, 0, buffer.Length);
        }
    }

    private void ListenForMessages()
    {
        var buffer = new byte[1024];
        try
        {
            while (true)
            {
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("\nСообщение от другого пользователя: " + message);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка получения сообщений: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Отключено от сервера.");
            _client.Close();
        }
    }
}