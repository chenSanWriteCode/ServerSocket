using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServerSocket.Service.DelegateCollection.UIDeletegate;

namespace ServerSocket.Service.DelegateCollection
{
    public class DelegateCollectionImpl
    {
        public static ChangeControlWithStr stringMsg;

        public static ChangeControlWithStr stringErrMsg;

        public static ChangeControlWithList<string> nameList;

        /// <summary>
        /// 用委托将msg返回
        /// </summary>
        /// <param name="msg"></param>
        public static void returnStringMsg(string msg)
        {
            stringMsg?.BeginInvoke(msg, null, null);
        }
        /// <summary>
        /// 用委托将用户列表返回
        /// </summary>
        /// <param name="names"></param>
        public static void returnNameList(List<string> names)
        {
            nameList?.BeginInvoke(names, null, null);
        }
        /// <summary>
        /// 用委托将异常消息返回
        /// </summary>
        /// <param name="msg"></param>
        public static void returnStringErrMsg(string msg)
        {
            stringErrMsg?.BeginInvoke(msg, null, null);
        }
    }
}
