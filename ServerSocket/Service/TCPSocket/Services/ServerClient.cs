using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerSocket.Service.TCPSocket.Entity;

namespace ServerSocket.Service.TCPSocket.Services
{
    public class ServerClient
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private string name;
        public Socket currentSocket=null;
        /// <summary>
        /// 客户端的ip
        /// </summary>
        private IPAddress IPAdress;
        /// <summary>
        /// 用户手动可停止
        /// </summary>
        private bool serverRunFlag = true;
        /// <summary>
        /// 连接状态
        /// </summary>
        //private bool status = false;
        public ServerClient(Socket socket)
        {
            this.currentSocket = socket;
            this.IPAdress = ((IPEndPoint)socket.RemoteEndPoint).Address;
        }
        /// <summary>
        /// 运行服务端，对接收的数据进行解析
        /// </summary>
        public void runServer()
        {
            byte[] receives = new byte[1024];
            string receiveContent = "";
            string[] commands;
            bool status = false;

            while (serverRunFlag)
            {
                if (currentSocket.Available<1)
                {
                    Thread.Sleep(300);
                    continue;
                }
                int len = currentSocket.Receive(receives);
                //有数据传输过来
                if (len>0)
                {
                    receiveContent = System.Text.Encoding.Default.GetString(receives);
                    commands = receiveContent.Split(new char[] { '|' });
                    decodeCommands(commands,ref status);
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        }
        /// <summary>
        /// 解析收到的数据
        /// </summary>
        /// <param name="commands"></param>
        private void decodeCommands(string[] commands,ref bool status)
        {
            if (status==false)
            {
                if ((COMMAND)(Convert.ToInt32(commands[0]))== COMMAND.CONN)
                {
                    conn(commands,ref status);
                }
                else
                {
                    sendMsg(currentSocket, "ERR|Please connect first");
                }
            }
            else
            {
                if (commands != null)
                {
                    COMMAND com = (COMMAND)(Convert.ToInt32(commands[0]));
                    switch (com)
                    {
                        case COMMAND.LIST:
                            list(commands);
                            break;
                        case COMMAND.PUB:
                            pub(commands);
                            break;
                        case COMMAND.PRI:
                            pri(commands);
                            break;
                        case COMMAND.EXIT:
                            exit(commands);
                            break;
                        default:
                            break;
                    }
                }
            }
            
        }
        /// <summary>
        /// 连接
        /// 此时接收到的命令格式为：命令标志符（CONN）|发送者的用户名|，tokens[1]中保存了发送者的用户名
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="status"></param>
        private void conn(string[] commands, ref bool status)
        {
            this.name = commands[1];
            if (GlobalVariable.tcpClients.ContainsKey(this.name))
            {
                string msg = "ERR|用户已存在";
                sendMsg(this.currentSocket, msg);
            }
            else
            {
                byte[] locker = new byte[0];
                //多线程还是用hashtable好一些
                lock (locker)
                {
                    GlobalVariable.tcpClients.Add(name, currentSocket);
                    string msg = "LIST|" + GlobalVariable.getClientList();
                    //遍历 向所有用户刷新用户列表
                    foreach (var item in GlobalVariable.tcpClients)
                    {
                        sendMsg(item.Value, msg);
                    }
                }
                status = true;
            }
        }
        /// <summary>
        /// 刷新用户列表
        /// </summary>
        /// <param name="commands"></param>
        private void list(string[] commands)
        {
            string msg = "LIST|" + GlobalVariable.getClientList();
            sendMsg(this.currentSocket,msg);
        }
        /// <summary>
        /// 向所有用户发消息
        /// 此时接收到的命令格式为：命令标志符（PUB）|发送者的用户名|msg
        /// </summary>
        /// <param name="commands"></param>
        private void pub(string[] commands)
        {
            foreach (var item in GlobalVariable.tcpClients)
            {
                sendMsg(item.Value, "CONT|"+item.Key+ "|"+commands[2]);
            }
        }
        /// <summary>
        /// 单发消息
        /// 此时接收到的命令格式为：命令标志符（PRI）|发送者用户名|接收者用户名|发送内容
        /// </summary>
        /// <param name="commands"></param>
        private void pri(string[] commands)
        {
            string msg;
            Socket otherSide = null;
            if (!GlobalVariable.tcpClients.TryGetValue(commands[2],out otherSide))
            {
                msg = "ERR|it has off_line";
                sendMsg(this.currentSocket, msg);
            }
            else
            {
                msg = "CONT|" + commands[1] + "|" + commands[3];
                sendMsg(this.currentSocket, msg);
                sendMsg(otherSide, msg);
            }
        }
        /// <summary>
        /// 退出
        /// 命令标志符（EXIT）|发送者的用户名
        /// </summary>
        /// <param name="commands"></param>
        private void exit(string[] commands)
        {
            
            byte[] locker = new byte[0];
            lock (locker)
            {
                GlobalVariable.tcpClients.Remove(commands[1]);
                string msg = "LIST|" + GlobalVariable.getClientList();
                //遍历 向所有用户刷新用户列表
                foreach (var item in GlobalVariable.tcpClients)
                {
                    sendMsg(item.Value, msg);
                }
            }
            this.currentSocket.Close();
            Thread.CurrentThread.Abort();
        }
        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        private  void sendMsg(Socket socket,string msg)
        {
            byte[] content = System.Text.Encoding.Default.GetBytes(msg);
            socket.Send(content);
        }

        /// <summary>
        /// 发送错误信息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="msg"></param>
        public void sendErrMsg(Socket socket, string msg)
        {
            sendMsg(socket, "ERR|" + msg);
        }
        
    }
}
