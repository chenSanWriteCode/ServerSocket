using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerSocket.Entity;
using ServerSocket.Service.DelegateCollection;
using ServerSocket.Service.TCPSocket.Entity;

namespace ServerSocket.Service.TCPSocket.Services
{
    
    public class TCPScoketServices:SocketServicesBase
    {
        /// <summary>
        /// 监听flag
        /// true 可以监听
        /// false 不可监听
        /// </summary>
        private bool listenFlag = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="max_num">最大连接socket数</param>
        public TCPScoketServices(int max_num):base(max_num)
        {
            
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="listener"></param>
        public override void startSocketListen(object listener)
        {
            listenFlag = true;
            //测试delegate  开始监听
            DelegateCollectionImpl.returnStringMsg("开始监听");
            TcpListener TcpListener = (TcpListener)listener;
            while (listenFlag)
            {
                try
                {
                    ServerClient serverClient1 = new ServerClient();
                    Thread thread1 = new Thread(new ThreadStart(serverClient1.returnMsg));
                    thread1.Start();
                    if (TcpListener.Pending())
                    {
                        Socket socket = TcpListener.AcceptSocket();
                        if (GlobalVariable.tcpClients.Count > MAX_NUM)
                        {
                            //DONE:通过委托对form中richTextbox添加注释  “超过最大连接数”
                            DelegateCollectionImpl.returnStringMsg("超过最大连接数，连接失败");
                            //DONE: 给socket发信息， “超过最大连接数” 并关闭socket
                            ServerClient serverClient = new ServerClient(socket);
                            serverClient.sendErrMsg(socket, "超过最大连接数");
                            socket.Close();
                        }
                        else
                        {
                            //DONE: 开启一个新线程
                            ServerClient serverClient = new ServerClient(socket);
                            Thread serverThread = new Thread(new ThreadStart(serverClient.runServer));
                            serverThread.Start();
                        }
                    }
                }
                catch (Exception err)
                {
                    listenFlag = false;
                    //Done: 通过委托对richTextbox添加注释 “err”,并修改监听按钮
                    DelegateCollectionImpl.returnStringErrMsg("出现异常" + err.Message);
                }
                Thread.Sleep(200);
            }
        }
    }
}
