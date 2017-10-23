using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _005_聊天室_tcp客户端
{
    class Client
    {
        private Socket clientSocket;
        private Thread t;
        private byte[] data = new byte[1024];   //数据容器

        //判断是否连接
        public bool Connected
        {
            get
            {
                return clientSocket.Connected;
            }
        }

        public Client(Socket s)
        {
            clientSocket = s;
            //启动一个线程，处理客户端的接收
            t = new Thread(ReceiveMessage);
            t.Start();
        }

        private void ReceiveMessage()
        {
            //一直接收客户端的数据
            while (true)
            {
                //在接收数据之前片段Socket连接是否断开，，
                if (clientSocket.Poll(10, SelectMode.SelectRead))
                {
                    clientSocket.Close();
                    break;  //跳出循环终止线程
                }
                int length =  clientSocket.Receive(data);
                string message = Encoding.UTF8.GetString(data, 0, length);
                //接收到数据，
                Console.WriteLine("收到消息:"+message);
                //将数据分发到客户端
                Program.BroadcastMessage(message);

            }
        }


        //发送消息
        public void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message); 
            clientSocket.Send(data);
        }

      
    }
}
