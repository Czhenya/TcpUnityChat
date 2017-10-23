using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CharManager : MonoBehaviour {

    public string ipaddress = "127.0.0.1";
    public int port = 3355;

    private Socket clientScorket;       // 服务端
    private Thread t;                   // 接收消息的线程
    private byte[] data = new byte[1024]; // 接收消息的数组
    private string message = "";          //接收到的消息

    //UI引用  输入的消息
    public InputField inputTest;
    public Text Char;

	void Start () {
        ConnectedToServer();

    }
	
	// Update is called once per frame
	void Update () {
        if (message != null && message !="")
        {
            Char.text += "\n" + message;
            message = "";    //清空消息
        }
	}

    /// <summary>
    /// 连接服务端
    /// </summary>
    void ConnectedToServer()
    {
        clientScorket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //发起连接
        clientScorket.Connect(new IPEndPoint(IPAddress.Parse(ipaddress), port));

        //创建一个线程来添加消息
        t = new Thread(ReceiveMessage);
        t.Start();
    }

    /// <summary>
    /// 这个线程的方法，用来循环接收消息
    /// </summary>
    void ReceiveMessage()
    {
        while(true)
        {
            if(clientScorket.Connected == false)
                break;
            int length = clientScorket.Receive(data);
            message = Encoding.UTF8.GetString(data,0,length);
            //Unity 不允许在单独的线程中，
            //Char.text +="\n" + message;
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="message">发送内容</param>
    void sendMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        clientScorket.Send(data);
    }

    /// <summary>
    /// 发送按钮的处理方法
    /// </summary>
    public void OnSendBtnonClick()
    {
        string value = inputTest.text;
        sendMessage(inputTest.text);
        inputTest.text = "";
    }

    private void OnDestroy()
    {
        //既不接受也不发送，，，
        clientScorket.Shutdown(SocketShutdown.Both);
        clientScorket.Close();
    }
}
