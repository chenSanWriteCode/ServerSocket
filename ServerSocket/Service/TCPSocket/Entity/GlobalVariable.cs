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
        public static Dictionary<string, Socket> tcpClients = null;
        /// <summary>
        /// 获取所有用户，以逗号分割
        /// </summary>
        /// <returns></returns>
        public static string getClientList()
        {
            return string.Join(",", tcpClients.Keys.ToArray());
        }
    }
    /// <summary>
    /// 命令
    /// </summary>
    public enum COMMAND
    {
        CONN=0,
        LIST,
        PUB,
        PRI,
        EXIT
    }
    

}
