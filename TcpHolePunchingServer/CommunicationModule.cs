using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpHolePunchingServer
{
    class CommunicationModule
    {
        public Stream stream { get; set; }
        public Socket socket { get; set; }
        public IPEndPoint localEndPoint { get; set; }

        public void SetLocalEndPoint()
        {
            var localEndPointString = ReceiveData();
            var localEndPointList = localEndPointString.Split(':');
            var ipAddress = localEndPointList[0].Split('.').Select(i => Convert.ToByte(i)).ToArray();
            localEndPoint = new IPEndPoint(new IPAddress(ipAddress), int.Parse(localEndPointList[1]));
        }


        public String ReceiveData(int filesize)
        {
            var buffer = new byte[1024];
            int receivedSize = 0;
            int bytesReceived;
            StringBuilder stringBuilder = new StringBuilder(filesize);

            while (receivedSize < (filesize) && (bytesReceived = stream.Read(buffer, 0, 1024)) != 0)
            {
                String msg = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                stringBuilder.Append(msg, 0, bytesReceived);
                receivedSize += bytesReceived;
            }
            SendOk();
            return stringBuilder.ToString();
        }

        public void SendData(String str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
            ReceiveOk();
        }

        public String ReceiveData()
        {
            var bytes = new byte[1024];
            int received = stream.Read(bytes, 0, 1024);
            SendOk();
            return Encoding.ASCII.GetString(bytes, 0, received);
        }

        public double[][] ReceiveDataSet(int filesize)
        {
            var buffer = new byte[1024];
            var lines = new List<double[]>();
            int receivedSize = 0;
            int bytesReceived;
            StringBuilder stringBuilder = new StringBuilder(filesize);

            while (receivedSize < (filesize) && (bytesReceived = stream.Read(buffer, 0, 1024)) != 0)
            {
                String msg = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
                stringBuilder.Append(msg, 0, bytesReceived);
                receivedSize += bytesReceived;
            }
            SendOk();
            var Data = stringBuilder.ToString().Split("\n");
            for (int i = 0; i < Data.Length; i++)
            {
                string[] line = Data[i].Split(',');
                var lineValues = line.Select(e => Convert.ToDouble(e)).ToArray();
                lines.Add(lineValues);
            }
            return lines.ToArray();
        }

        public void SendDataSet(String dataSet)
        {
            int i = 0;
            int rem = dataSet.Length % 1024;
            while (i < (dataSet.Length - 1024))
            {
                byte[] msg = Encoding.ASCII.GetBytes(dataSet.Substring(i, 1024));
                stream.Write(msg, 0, 1024);
                i += 1024;
            }
            byte[] msg2 = Encoding.ASCII.GetBytes(dataSet.Substring(i, rem));
            stream.Write(msg2, 0, msg2.Length);
            ReceiveOk();
        }

        private void SendOk()
        {
            var bytes = Encoding.ASCII.GetBytes("Ok");
            stream.Write(bytes, 0, bytes.Length);
        }

        private void ReceiveOk()
        {
            var recBytes = new byte[2];
            stream.Read(recBytes, 0, recBytes.Length);

            if (!(Encoding.ASCII.GetString(recBytes) == "Ok"))
            {
                throw new Exception("Ok Not received");
            }
        }
    }
}
    

