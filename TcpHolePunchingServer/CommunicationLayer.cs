using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace TcpHolePunchingServer
{
    internal class CommunicationLayer
    {
        private TcpListener tcpListener;
        private List<CommunicationModule> communicationModule;

        public CommunicationLayer(string ip , int portNo)
        {
            var Ip = ip.Split('.').Select(i => Convert.ToByte(i)).ToArray();

            communicationModule = new List<CommunicationModule>();
            try
            {
                tcpListener = new TcpListener(new IPAddress(Ip), portNo);
                tcpListener.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception {0}", e);
            }
        }

        public CommunicationModule AcceptConnection()
        {
            var socket = tcpListener.AcceptSocket();
            var stream = new NetworkStream(socket);
            var module = new CommunicationModule
            {
                socket = socket,
                stream = stream

            };
            module.SetLocalEndPoint();
            communicationModule.Add(module);
            return module;
        }

        
    }
}