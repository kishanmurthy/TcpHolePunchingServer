using System;
using System.Threading;

namespace TcpHolePunchingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip;
            Console.WriteLine("Enter the ip Address");
            ip = Console.ReadLine();
            Server server = new Server(ip);
            server.StartServer(2);

        }



    }
}
