using System;

namespace TcpHolePunchingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip;
            Console.WriteLine("Enter the ip Address");
            ip = Console.ReadLine();
            CommunicationLayer server = new CommunicationLayer(ip,6000);
            server.AcceptConnection();
            server.SendData("Connected To the Network");
            string s = server.ReceiveData();
            Console.WriteLine("Data Received {0}", s);
        }

    }
}
