using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public static class Program
{

    public async static void Main(string[] args)
    {
        var listener = new TcpListener(IPAddress.Any, 5555);
        Console.WriteLine("listening on 0.0.0.0:5555");
        listener.Start(int.MaxValue);
        Loop(listener);
        Console.ReadLine();
    }
    
    public async static void Loop(TcpListener listener)
    {
        var client = await listener.AcceptTcpClientAsync();
        Console.WriteLine("one client accepted");
        Handle(client);
        Loop(listener);
    }
    
    public async static void Handle(TcpClient client)
    {
        using (var stream = client.GetStream())
        {
            var buf = new byte[1024];
        
            while (client.Connected)
            {
                var length = await stream.ReadAsync(buf, 0, buf.Length);
                if (length <= 0)
                {
                    break;
                }
                var data = Encoding.UTF8.GetString(buf, 0, length).TrimEnd();
                
                Console.WriteLine("received: {0}", data);
                
                if (data == "exit")
                {
                    break;
                }
                
                stream.WriteAsync(buf, 0, length);
            }
            
            Console.WriteLine("one client exited");
        }
    }
}

