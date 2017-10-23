using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _005_聊天室_tcp客户端
{
    class Program
    {
        //所有客户端的列表
        static List<Client> clientList = new List<Client>();
        
        /// <summary>
        /// 广播消息
        /// </summary>
        public static void BroadcastMessage(string message)
        {
            //
            var notList = new List<Client>();
            foreach (var item in clientList)
            {
                if (item.Connected)
                {
                    item.SendMessage(message);
                }
                else
                {   //断开链接的客户端
                    notList.Add(item);
                }              
            }
            //移除断开链接的客户端
            foreach (var temp in notList)
            {
                clientList.Remove(temp);
            }

        }
        static void Main(string[] args)
        {
            //创建socket对象
            Socket tcpSever = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //绑定
            tcpSever.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),3355));

            //监听
            tcpSever.Listen(100);
            Console.WriteLine("服务器启动成功");


            while (true)
            {      
                Socket clientSocket = tcpSever.Accept();     //接收连接

                Console.WriteLine("连接一个客户端");

                Client client = new Client(clientSocket);    //把每个客户端的通信逻辑（收发message）使用Client类处理

                clientList.Add(client);     //连接客户端
                
            }

        }
    }
}
