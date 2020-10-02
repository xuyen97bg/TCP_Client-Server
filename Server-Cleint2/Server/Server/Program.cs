using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            IPHostEntry local = Dns.GetHostByName(Dns.GetHostName());
            IPAddress host = IPAddress.Parse("127.0.0.1");
            IPEndPoint hostep = new IPEndPoint(host, 8000);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newsock.Bind(hostep); // Gan socket voi dia chi IP,Port cu the
            newsock.Listen(2); // Toi da 2 ket noi client
            Socket newclient = newsock.Accept();
            byte[] data = new byte[17];
            data = ReceiveVarData(newclient);
            string quesion = Encoding.UTF8.GetString(data);
            newsock.Close();
            Console.WriteLine(quesion);
           
        }
        private static byte[] ReceiveVarData(Socket s)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];
            recv = s.Receive(datasize, 0, 4, 0); // Nhận dữ liệu độ dài
            int size = BitConverter.ToInt32(datasize);
            int dataleft = size;
            byte[] data = new byte[size];
            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    data = Encoding.ASCII.GetBytes("exit ");
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }
    }
}
