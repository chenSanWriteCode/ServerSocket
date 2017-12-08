using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerSocket.Entity;
using static ServerSocket.Service.DelegateCollection.UIDeletegate;

namespace ServerSocket.Service
{
    /// <summary>
    /// serverSocket 基类
    /// 应具备 监听、发送、刷新、关闭 功能
    /// </summary>
    public class SocketServicesBase
    {
        public ChangeControlWithStr stringMsg;

        public ChangeControlWithList<string> nameList;
        /// <summary>
        /// 最大连接socket数
        /// </summary>
        protected readonly int MAX_NUM = 0;
        
        public SocketServicesBase(int max_num)
        {
            this.MAX_NUM = max_num;
        }
        public virtual void startSocketListen(object listener)
        {
            
        }
        /// <summary>
        /// 用委托将msg返回
        /// </summary>
        /// <param name="msg"></param>
        public void returnStringMsg(string msg)
        {
            stringMsg?.BeginInvoke(msg, null, null);
        }
        /// <summary>
        /// 用委托将用户列表返回
        /// </summary>
        /// <param name="names"></param>
        public void returnNameList(List<string> names)
        {
            nameList?.BeginInvoke(names, null, null);
        }
    }
}
