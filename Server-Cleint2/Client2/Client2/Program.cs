using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client2
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress host = IPAddress.Parse("127.0.0.1");
            IPEndPoint hostep = new IPEndPoint(host, 8000);
            //Khai bao IP, Port
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // Môi trương tạo kết nối - Kiểu kết nối - Kiểu giao thức
            sock.Connect(hostep);
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes("Ban tên là gì:");//Muốn gửi dữ liệu thì Chuyển dữ liệu sang byte. 
            int n = SendVarData(sock, data);
            sock.Close();
            Console.WriteLine("Gui thanh cong %d" + n + "ki tu");
        }
        private static int SendVarData(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;
            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size); //Convert do dai du lieu tu int sang bit
            sent = s.Send(datasize);//Gui do dai du kieu cho server
            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None); // dữ liệu - tổng bit đã gửi - số bit gửi - Cờ
                total += sent;
                dataleft -= sent;
            }
            return total;
        }

    }
}
