using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerSocket.Entity;
using ServerSocket.Service.TCPSocket.Entity;
using static ServerSocket.Service.DelegateCollection.UIDeletegate;

namespace ServerSocket.Service
{
    /// <summary>
    /// serverSocket 基类
    /// 应具备 监听、发送、刷新、关闭,踢人 功能
    /// </summary>
    public class SocketServicesBase
    {

        public StringBuilder msg = new StringBuilder("");
        /// <summary>
        /// 最大连接socket数
        /// </summary>
        protected readonly int MAX_NUM = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="max_num">最大连接socket数</param>
        public SocketServicesBase(int max_num)
        {
            this.MAX_NUM = max_num;
        }
        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="listener">TCP or UDP</param>
        public virtual void startSocketListen(object listener)
        {

        }
        /// <summary>
        /// 发消息
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="senderName">发送者姓名</param>
        /// <param name="message">消息</param>
        /// <param name="receiverName">接受者姓名</param>
        public virtual void sendMessage(TOSERVERCOMMAND command, string senderName = null, string message = null, string receiverName = null)
        {
            switch (command)
            {
                case TOSERVERCOMMAND.LIST:
                    msg.Append(TOSERVERCOMMAND.LIST.ToString());
                    break;
                case TOSERVERCOMMAND.PUB:
                    msg.Append(TOSERVERCOMMAND.LIST.ToString()).Append("|").Append(senderName).Append(":|").Append(message);
                    break;
                case TOSERVERCOMMAND.PRI:
                    msg.Append(TOSERVERCOMMAND.LIST.ToString()).Append("|").Append(senderName).Append(":|").Append(receiverName).Append("|").Append(message);
                    break;
                case TOSERVERCOMMAND.EXIT:
                    msg.Append(TOSERVERCOMMAND.LIST.ToString());
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 踢人
        /// </summary>
        /// <param name="clientName"></param>
        public virtual void getOut(string clientName)
        {

        }

    }
}
