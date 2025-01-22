namespace ChatClientApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Введите IP-адрес сервера:");
        string serverIp = Console.ReadLine();

        var client = new ChatClient();
        client.Connect(serverIp, 5000);
    }
}