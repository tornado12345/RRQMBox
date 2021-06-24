//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMBox.Server.Common;
using RRQMCore.ByteManager;
using RRQMMVVM;
using RRQMSkin.Windows;
using RRQMSocket;
using System;
using System.Text;
using System.Windows;

namespace RRQMBox.Server.Win
{
    /// <summary>
    /// CreatProcotolWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateProcotolWindow : RRQMWindow
    {
        public CreateProcotolWindow()
        {
            InitializeComponent();
            this.Loaded += this.CreatProcotolWindow_Loaded;
        }

        private void ShowMsg(string msg)
        {
            this.UIInvoke(() =>
            {
                this.msgBox.AppendText($"{msg}\r\n");
            });
        }

        private void UIInvoke(Action action)
        {
            this.Dispatcher.Invoke(() =>
            {
                action.Invoke();
            });
        }

        private void CreatProcotolWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.onLineClient = new RRQMList<SocketClient>();
            this.Lb_OnlineClient.ItemsSource = this.onLineClient;
        }

        private SimpleProtocolService protocolService;
        private RRQMList<SocketClient> onLineClient;

        private void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            CreateProtocol();
        }

        private void Bt_ResetID_Click(object sender, RoutedEventArgs e)
        {
            if (this.protocolService != null)
            {
                try
                {
                    this.protocolService.ResetID("MyClientID", "MyServerID");
                    ShowMsg("修改成功");
                }
                catch (Exception ex)
                {
                    ShowMsg(ex.Message);
                }
            }
        }

        private void CreateProtocol()
        {
            if (protocolService == null)
            {
                protocolService = new SimpleProtocolService();
                //订阅事件
                protocolService.ClientConnected += Service_ClientConnected;//订阅连接事件
                protocolService.ClientDisconnected += Service_ClientDisconnected;//订阅断开连接事件
                protocolService.Received += this.ProtocolService_Received; ;
            }

            //属性设置
            var config = new ServerConfig();
            config.SetValue(ServerConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(this.Tb_iPHost.Text) })
                .SetValue(ServerConfig.LoggerProperty, new MsgLog(this.ShowMsg))//设置内部日志记录器
                .SetValue(ServerConfig.ThreadCountProperty, int.Parse(this.Tb_ThreadCount.Text))//设置多线程数量
                .SetValue(TcpServerConfig.ClearIntervalProperty, 300)//300秒无数据交互将被清理
                .SetValue(ServerConfig.BufferLengthProperty, 1024)//设置缓存池大小，该数值在框架中经常用于申请ByteBlock，所以该值会影响内存池效率。
                .SetValue(TokenServerConfig.VerifyTokenProperty, this.Tb_Token.Text);

            //方法
            protocolService.Setup(config);
            protocolService.Start();
            ShowMsg("绑定成功");
            ShowMsg($"请使用Token为{protocolService.VerifyToken}进行连接");
        }

        private void Service_ClientDisconnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Remove((SocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void Service_ClientConnected(object sender, MesEventArgs e)
        {
            this.UIInvoke(() =>
            {
                this.onLineClient.Add((SocketClient)sender);
                this.Tb_ClientNum.Text = this.onLineClient.Count.ToString();
            });
        }

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (protocolService != null && protocolService.ServerState == ServerState.Running)
            {
                protocolService.Stop();
                ShowMsg("解除绑定");
                this.onLineClient.Clear();
            }
            else
            {
                ShowMsg("服务器未绑定");
            }
        }

        private void Bt_Dispose_Click(object sender, RoutedEventArgs e)
        {
            if (protocolService != null && protocolService.ServerState == ServerState.Running)
            {
                protocolService.Dispose();
                ShowMsg("释放绑定");
                this.onLineClient.Clear();
            }
            else
            {
                ShowMsg("服务器未绑定");
            }
        }

        private void ProtocolService_Received(SimpleProtocolSocketClient arg1, short? arg2, ByteBlock byteBlock)
        {
            if (arg2 == null)
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 0, (int)byteBlock.Length);
                ShowMsg($"接收到无协议信息：ID={arg1.ID},信息：{mes}");
            }
            else
            {
                string mes = Encoding.UTF8.GetString(byteBlock.Buffer, 2, (int)byteBlock.Length - 2);
                ShowMsg($"接收到协议信息：ID={arg1.ID},协议={arg2},信息：{mes}");
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Lb_OnlineClient.SelectedItem != null)
            {
                if (this.Cb_IsAsync.IsChecked == true)
                {
                    ((SocketClient)this.Lb_OnlineClient.SelectedItem).SendAsync(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
                else
                {
                    ((SocketClient)this.Lb_OnlineClient.SelectedItem).Send(Encoding.UTF8.GetBytes(this.Tb_TestMsg.Text));
                }
            }
            else
            {
                ShowMsg("请先选择客户端列表");
            }
        }

        private void CorrugatedButton_Click(object sender, RoutedEventArgs e)
        {
            this.msgBox.Clear();
        }
    }
}