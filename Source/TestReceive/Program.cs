using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TestReceive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服务已启动！");
            UdpClient udpClient = new UdpClient(10001);
            while (true)
            {
                IPEndPoint ipendpoint = null;
                byte[] bytes = udpClient.Receive(ref ipendpoint); //停在这等待数据  
                string data = Encoding.Default.GetString(bytes, 0, bytes.Length);

                Console.WriteLine("{0:HH:mm:ss}->接收数据(from {1}:{2})：{3}", DateTime.Now, ipendpoint.Address, ipendpoint.Port, data);
                Thread.Sleep(1);
            }
            udpClient.Close(); 
            Console.Read();
        }
    }
}
