//------------------------------------------------------------------------------
//  此代码版权（除特别声明或在RRQMCore.XREF命名空间的代码）归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using Microsoft.Win32;
using RRQMSocket.FileTransfer;
using System.Windows;

namespace FileClientGUI.Win
{
    /// <summary>
    /// PushWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PushWindow : Window
    {
        public PushWindow()
        {
            InitializeComponent();
        }

        public bool SelectRequest(out FileRequest fileRequest)
        {
            this.ShowDialog();
            fileRequest = this.fileRequest;
            return this.go;
        }

        private FileRequest fileRequest;
        private bool go;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fileRequest = new FileRequest()
            {
                FileCheckerType = FileCheckerType.MD5,
                Path = this.tb1.Text,
                SavePath = this.tb2.Text
            };
            this.go = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (!string.IsNullOrEmpty(dialog.FileName))
            {
                this.tb1.Text = dialog.FileName;
                this.tb2.Text = System.IO.Path.GetFileName(dialog.FileName);
            }
        }
    }
}