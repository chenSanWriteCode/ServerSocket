using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Service.TCPSocket.Entity
{
    public class GlobalVariable
    {
        /// <summary>
        /// TCP类型的socket已连接的数量
        /// </summary>
        public static Dictionary<string, Socket> tcpClients = new Dictionary<string, Socket>();
        /// <summary>
        /// 获取所有用户，以逗号分割
        /// </summary>
        /// <returns></returns>
        public static string getClientStr()
        {
            return string.Join(",", tcpClients.Keys.ToArray());
        }
        /// <summary>
        /// 获取所有用户List
        /// </summary>
        /// <returns></returns>
        public static List<string> getClisentList()
        {
            return GlobalVariable.tcpClients.Keys.ToList();
        }

    }
    /// <summary>
    /// 客户端向服务器发送的命令
    /// </summary>
    public enum TOSERVERCOMMAND
    {
        /// <summary>
        /// 连接
        /// </summary>
        CONN = 0,
        /// <summary>
        /// 用户列表
        /// </summary>
        LIST,
        /// <summary>
        /// 公共窗口
        /// </summary>
        PUB,
        /// <summary>
        /// 私人窗口
        /// </summary>
        PRI,
        /// <summary>
        /// 退出
        /// </summary>
        EXIT,
        /// <summary>
        /// 错误
        /// </summary>
        ERR

    }
    /// <summary>
    /// 服务器向客户端发送的命令
    /// </summary>
    public enum TOCLIENCOMMAND
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        LIST = 0,
        /// <summary>
        /// 内容
        /// </summary>
        CONT,
        /// <summary>
        /// 退出
        /// </summary>
        EXIT,
        /// <summary>
        /// 错误
        /// </summary>
        ERR
    }


}
