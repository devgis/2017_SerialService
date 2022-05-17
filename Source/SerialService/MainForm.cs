using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SerialService.libs;
using System.Net.Sockets;
using System.Net;

namespace SerialService
{
    public partial class MainForm : Form
    {
        UdpClient udpClient = null;
        IPEndPoint ipEndPoint = null;
        public MainForm()
        {
            InitializeComponent();
            DevSerialService.Instance.DataReceived += new DevSerialService.DataReceivedHandle(Instance_DataReceived);

            string ip = System.Configuration.ConfigurationManager.AppSettings["ipaddress"];
            int udpport = 115200;
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["udpport"], out udpport);
            udpClient = new UdpClient(udpport);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), udpport);
            ShowLog("服务已启动！");
        }

        void Instance_DataReceived(object sender, CommEventArgs e)
        {
            if (e.Data != null)
            {
                ShowLog(e.Data.ToString());
                udpClient.Send(e.Data.ByteArray, e.Data.ByteArray.Length, ipEndPoint);
            }
            else
            {
                ShowLog("ERROR DATA!");
            }
        }

        private void ShowLog(string logcontent)
        {
            
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke((EventHandler)(delegate
                {
                    if (txtLog.Text.Length >= 65535)
                    {
                        txtLog.Clear();
                    }
                    txtLog.Text += (DateTime.Now.ToString("HH:mm:ss") + ":" + logcontent + "\r\n"); //yyyy-MM-dd 
                    txtLog.ScrollToCaret();
                }));
            }
            else
            {
                if (txtLog.Text.Length >= 65535)
                {
                    txtLog.Clear();
                }
                txtLog.Text += (DateTime.Now.ToString("HH:mm:ss") + ":" + logcontent + "\r\n"); //yyyy-MM-dd 
                txtLog.ScrollToCaret();
            }
        }
    }
}
