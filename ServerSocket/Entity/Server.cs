using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ServerSocket.Entity
{
    public class Server
    {
        /// <summary>
        /// 服务器ip
        /// </summary>
        private List<IPAddress> serverIp;
        /// <summary>
        /// 服务器端口
        /// </summary>
        private int serverPort;
        /// <summary>
        /// 主机名
        /// </summary>
        private string hostName;
        /// <summary>
        /// 监听状态
        /// </summary>
        private ServerSocketSatus status;
        /// <summary>
        /// 监听器
        /// 可以是TCPListener 
        /// 也可以是udp
        /// </summary>
        private object listener;
        /// <summary>
        /// 最大连接数
        /// </summary>
        private int max_num;
        public Server()
        {
            this.hostName = Dns.GetHostName();
            ServerIp = new List<IPAddress>();
            foreach (IPAddress item in Dns.GetHostAddresses(hostName))
            {
                if (item.AddressFamily==AddressFamily.InterNetwork)
                {
                    this.ServerIp.Add(item);
                }
            }
            this.serverPort = Convert.ToInt32(ConfigurationManager.AppSettings.GetValues("port")[0]);
        }

        public int ServerPort
        {
            get
            {
                return serverPort;
            }

            set
            {
                serverPort = value;
            }
        }

        public string HostName
        {
            get
            {
                return hostName;
            }
        }

        

        public ServerSocketSatus Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public List<IPAddress> ServerIp
        {
            get
            {
                return serverIp;
            }

            set
            {
                serverIp = value;
            }
        }

        public object Listener
        {
            get
            {
                return listener;
            }

            set
            {
                listener = value;
            }
        }

        public int Max_num
        {
            get
            {
                return max_num;
            }

            set
            {
                max_num = value;
            }
        }
    }
    /// <summary>
    /// server socket status  
    /// 监听/停止监听
    /// </summary>
    public enum ServerSocketSatus
    {
        Listen=0,
        StopListen
    }
}
