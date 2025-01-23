using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using WebSocketSharp;
using LBC.ViewModels;
using WebFirst.Entities;
using Newtonsoft.Json;

namespace LBC.Class
{
    public class WebSocketTest
    {
        private WebSocket _ws;
        private bool _isConnected = false; // 新增变量，用于跟踪连接状态
        private System.Windows.Threading.DispatcherTimer _reconnectTimer; // 定时器用于重连
                                                                          // 新增变量，标记是否是手动关闭连接
        private bool _isManualClose = false;
        public string macid = "";
        public WebSocketTest()
        {
            macid = App.global.mac; //GetMacAddress().Replace("\r\n","") + ":" + Getrandom();
            _reconnectTimer = new System.Windows.Threading.DispatcherTimer();
            _reconnectTimer.Interval = TimeSpan.FromSeconds(10); // 设置重连间隔为10秒
            _reconnectTimer.Tick += ReconnectTimer_Tick;
            ConnectButton_Click(null, null);
        }
        private void ReconnectTimer_Tick(object sender, EventArgs e)
        {
            // 尝试重连
            ConnectButton_Click(null, null);
            // 停止定时器，直到再次断开连接
            _reconnectTimer.Stop();
        }
        public static string GetMacAddress()
        {
            string macAddresses = "";
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface adapter in nics)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();
                    StringBuilder macAddress = new StringBuilder();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        macAddress.Append(bytes[i].ToString("X2")); // Convert each byte to hexadecimal string
                        if (i != bytes.Length - 1)
                            macAddress.Append(":");
                    }

                    if (macAddress.Length > 0)
                    {
                        macAddresses += /*adapter.Name + ": " + */macAddress.ToString() /*+ Environment.NewLine*/;
                    }
                }
            }

            return macAddresses.Trim();
        }
        private string Getrandom()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] randomChars = new char[2];

            for (int i = 0; i < 2; i++)
            {
                randomChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomChars);
        }
        // 连接到服务器
        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isConnected)
            {
                Log("WebSocket 已经连接。");
                return;
            }

            // 尝试连接到服务器
            try
            {
                await Task.Run(() =>
                {
                    _ws = new WebSocket("ws://lbc.77zs.com:4448/echo?clientId=" + macid);
                    //_ws = new WebSocket("ws://127.0.0.1:4448/echo?clientId=" + macid);
                    _ws.OnOpen += OnOpen;
                    _ws.OnMessage += OnMessage;
                    _ws.OnError += OnError;
                    _ws.OnClose += OnClose;

                    // 连接到服务器，这里是同步操作，但因为放在了后台线程，所以不会阻塞UI
                    _ws.Connect();
                });

                
            }
            catch (Exception ex)
            {
                Log($"连接失败: {ex.Message}");
                _isConnected = false;
            }
        }
        private void OnOpen(object sender, EventArgs e)
        {
            // 连接成功后的逻辑可以放在这里，此时已经回到UI线程
            _isConnected = true;
            Log("已连接到服务器");
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            MyMessage myMessage = JsonConvert.DeserializeObject<MyMessage>(e.Data);
            if (myMessage == null)
                return;
            if (myMessage.l_BucketClient.servermac == "")
                return;
            if (myMessage.type == 12)
            {
                //Workspace.This.Git.Insert("错误信息：" + myMessage.ProgressBarValue+":"+ myMessage.ProgressBarValueMax);
            }
            //Log($"收到消息: {e.Data}");
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Log($"发生错误: {e.Message}");
        }

        private void OnClose(object sender, CloseEventArgs e)
        {
            Log("连接已关闭");
            _isConnected = false; // 更新连接状态
                                  // 如果不是因为用户点击关闭按钮，则开始重连
            //if (!_isManualClose)
            //{
                _reconnectTimer.Start();
            //}
        }

        // 发送消息到服务器
        public void SendMessage(MyMessage myMessage)
        {
            if (_isConnected && _ws != null && _ws.IsAlive)
            {
                    _ws.Send(JsonConvert.SerializeObject(myMessage));
                    //Log($"发送消息: {JsonConvert.SerializeObject(myMessage)}");
            }
            else
            {
                Log("请先连接到服务器");
            }
        }

        // 日志输出方法
        private void Log(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Workspace.This.Git.Insert(message);
            });
            
            //logTextBox.AppendText($"{message}\n");
            //logTextBox.ScrollToEnd();
        }

        // 窗口关闭时的清理工作
        protected void OnClosed(EventArgs e)
        {
            if (_ws != null)
            {
                _ws.Close();
            }
            _reconnectTimer.Stop(); // 停止定时器
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

            _isManualClose = true; // 标记为手动关闭
            if (_isConnected && _ws != null)
            {
                _ws.Close();
                Log("已主动断开与服务器的连接");
                _isConnected = false;
            }
            else
            {
                Log("当前没有活动连接");
            }
            _isConnected = false; // 更新连接状态
            _isManualClose = false; // 重置标记
        }
    }
    public class MyMessage
    {
        /// <summary>
        /// 0下载自动加载所有npc 1,上传 2是下载
        /// </summary>
        public int type { get; set; }
        public string filename { get; set; }

        public L_bucketClient l_BucketClient { get; set; }
        public string autojson { get; set; }
        public int ProgressBarValue { get; set; }
        public int ProgressBarValueMax { get; set; }
    }
}
