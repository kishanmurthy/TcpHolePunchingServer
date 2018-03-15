using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TcpHolePunchingServer
{
    class Server
    {
        CommunicationLayer server;
        private List<CommunicationModule> communicationModule;
        private Object moduleAddLock = new Object();
        int count;
        public List<string> IpPorts { get;}

        public Server(string ip)
        {
           server = new CommunicationLayer(ip, 6000);
            communicationModule = new List<CommunicationModule>();
            IpPorts = new List<string>();
            count = 0;
        }

        public void StartServer(int instance)
        {
            Thread[] threads = new Thread[instance];
            for (int i = 0; i < instance; i++)
            {
                threads[i] = new Thread(run);
                threads[i].Start();
            }
            for (int i = 0; i < instance; i++)
            {
                threads[i].Join();
            }

            communicationModule[0].SendData(communicationModule[1].socket.RemoteEndPoint.ToString());
            communicationModule[1].SendData(communicationModule[0].socket.RemoteEndPoint.ToString());

        }


        public void run()
        {
            var module = server.AcceptConnection();
            lock (moduleAddLock)
            {
                communicationModule.Add(module);
            }
        }
    }
}
