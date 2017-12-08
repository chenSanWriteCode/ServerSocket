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
            stringMsg("kaishijianting");
            //测试delegate  开始监听
            if (stringMsg!=null)
            {
                //appendRichTB.BeginInvoke("开始监听", null, null);
            }
            TcpListener TcpListener = (TcpListener)listener;
            while (listenFlag)
            {
                try
                {
                    if (TcpListener.Pending())
                    {
                        Socket socket = TcpListener.AcceptSocket();
                        if (GlobalVariable.tcpClients.Count > MAX_NUM)
                        {
                            //TODO:通过委托对form中richTextbox添加注释  “超过最大连接数”

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
                    //TODO: 通过委托对richTextbox添加注释 “err”,并修改监听按钮

                    //TODO: 对所有socket发通知 监听异常  停止 （貌似不需要发消息）
                }
                Thread.Sleep(200);
            }
        }
    }
}
