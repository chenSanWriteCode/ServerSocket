using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using ServerSocket.Entity;
using ServerSocket.Service;
using ServerSocket.Service.DelegateCollection;
using ServerSocket.Service.TCPSocket.Services;

namespace ServerSocket
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 监听socket
        /// </summary>
        private TcpListener Listener = null;
        /// <summary>
        /// 服务器IP
        /// </summary>
        private IPAddress ServerIP = null;
        /// <summary>
        /// 服务器端口
        /// </summary>
        private int ServerPort = -1;

        public Server serverSocket;
        public SocketServicesBase socketServices;
        public Form1()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            serverSocket = new Server();
            combo_serverIP.DataSource = serverSocket.ServerIp;
            tb_serverPort.Text = serverSocket.ServerPort.ToString();
            btn_listen.Tag = ServerSocketSatus.StopListen;

            serverSocket.Max_num = int.Parse(tb_maxNum.Text);
        }
        /// <summary>
        /// 监听与停止监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_listen_Click(object sender, EventArgs e)
        {
            if ((ServerSocketSatus)btn_listen.Tag == ServerSocketSatus.Listen)
            {

                //TODO: 需要向各个socket发送关闭信息

                Listener.Stop();
                btn_listen.Text = "监听";
                btn_listen.Tag = ServerSocketSatus.StopListen;
            }
            else
            {
                try
                {
                    ServerIP = IPAddress.Parse(combo_serverIP.Text);
                    ServerPort = Convert.ToInt32(tb_serverPort.Text);
                    #region 实例化监听
                    /*实例化监听分为两种，
                     * 1. 通过socket进行监听，在实例化socket时指定监听的类型，例如：Socket sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)，然后socket。bind（ip,port啥的）
                       2. 是直接指定用哪一种方式进行监听（tcp或udp），本例使用的是第二种*/
                    #endregion
                    Listener = new TcpListener(ServerIP, ServerPort);
                    Listener.Start(10);

                    btn_listen.Tag = ServerSocketSatus.Listen;
                    btn_listen.Text = "停止";
                    RTB_serverContext.AppendText("Socket服务器已经启动，正在监听" + ServerIP + " 端口号：" + ServerPort + "\n");

                    socketServices = new TCPScoketServices(serverSocket.Max_num);
                    socketServices.stringMsg = new UIDeletegate.ChangeControlWithStr(appendRTBServerContent);
                    socketServices.nameList = new UIDeletegate.ChangeControlWithList<string>(refreshLBUsers);
                    Thread listenThread = new Thread(new ParameterizedThreadStart(socketServices.startSocketListen));
                    listenThread.IsBackground = true;
                    listenThread.Start(Listener);

                    
                }
                catch (Exception err)
                {
                    RTB_serverContext.AppendText(err.Message + "\n");
                    MessageBox.Show(err.Message);
                }
            }
        }
        /// <summary>
        /// 在rtb_serverContent上追加内容
        /// </summary>
        /// <param name="mgs"></param>
        private void appendRTBServerContent(string msg)
        {
            if (RTB_serverContext.InvokeRequired)
            {
                BeginInvoke(this.socketServices.stringMsg, new object[] { msg });
                //RTB_serverContext.AppendText(msg + "\n");
            }
            else
            {
                this.RTB_serverContext.AppendText(msg + "\n");
            }

        }
        /// <summary>
        /// 刷新用户列表
        /// </summary>
        /// <param name="nameList"></param>
        private void refreshLBUsers(List<string> nameList)
        {
            nameList.Insert(0, "所有用户");
            lb_users.DataSource = nameList;
        }
    }

}
