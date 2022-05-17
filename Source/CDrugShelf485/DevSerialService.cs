using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace SerialService.libs
{
    public class DevSerialService:IDisposable
    {
        #region 公有成员
        public static int sysaddress = 2; //系统板地址

        /// <summary>
        /// handler超时时间
        /// </summary>
        public int HandlerTimeOut = 1000;//1000毫秒
        private List<GetResultDataHandlerItem> listHanleritems = new List<GetResultDataHandlerItem>();
        public Dictionary<string, DataByte> dicSyncResult = new Dictionary<string, DataByte>();
        //public delegate void GetResultDataHandler(DataByte data);
        Thread tReleaseThread = null;
        private const int Tiemout = 1000; //10秒
        #region 单例对象
        private static DevSerialService instance = null;

        /// <summary>
        /// 单例对象
        /// </summary>
        public static DevSerialService Instance
        {
            get
            {
                if (instance == null)
                {
                    string comname=System.Configuration.ConfigurationManager.AppSettings["comname"];
                    if (string.IsNullOrEmpty(comname))
                    {
                        comname = "COM1";
                    }
                    int baudRate = 115200;
                    int.TryParse(System.Configuration.ConfigurationManager.AppSettings["baudrate"], out baudRate);
                    instance = new DevSerialService(comname, baudRate);
                }

                return instance;
            }
        }
        #endregion

        //接收到数据上报
        public delegate void DataReceivedHandle(object sender, CommEventArgs e);
        //设备完成报错
        public delegate void CommErrorHandle(object sender, CommErrorEventArgs e);
        //主动上报
        public delegate void ReportHandle(object sender, ReportEventArgs e);

        /// <summary>
        /// 接收到数据
        /// </summary>
        public event DataReceivedHandle DataReceived;

        /// <summary>
        /// 接收到数据
        /// </summary>
        public event CommErrorHandle CommErrorReceived;

        /// <summary>
        /// 接收到数据
        /// </summary>
        public event ReportHandle ReportReceived;
        #endregion

        #region 私有成员
        private SerialPort _sp = null;
        #endregion

        #region 私有方法
        void _sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //ThreadPool.QueueUserWorkItem(o =>
            //{
                if (DataReceived == null&&listHanleritems.Count <= 0)
                {
                    //没有监听 也没有异步返回结果则不处理数据
                    return;
                }
                //System.Diagnostics.Debug.Write("2接收" + "\r\n");
            //获取数据
            //while (true)
            //{
                #region ---------- 接收响应帧 ----------
                CommEventArgs args = new CommEventArgs();
                var errMsg = string.Empty;
                bool timeOut = false;
                bool rcvOk = false;
                
                byte[] rcvMsgTemp=new byte[0];
                int bufCount = 0;
                int rcvCount = 0;
                int totalRcvCount = 0;
                DateTime startDt = DateTime.Now;
                int durTime = 0;
                while (!timeOut && !rcvOk)
                {
                    bufCount = _sp.BytesToRead;
                    if (bufCount > 0)
                    {
                        rcvMsgTemp = new byte[bufCount];
                        rcvCount = _sp.Read(rcvMsgTemp, 0, bufCount);
                        //if (totalRcvCount + rcvCount < rcvMsg.Length)
                        //{
                        //    for (int i = 0; i < rcvCount; i++)
                        //    {
                        //        rcvMsg[totalRcvCount + i] = rcvMsg1[i];
                        //    }
                        //    totalRcvCount += rcvCount;
                        //}
                    }
                    if (bufCount > 0 && bufCount%12==0) //数据正确
                    {
                        //处理接收到的消息
                        int iCount = bufCount / 12;
                        for (int i = 0; i < iCount; i++)
                        {
                            byte[] rcvMsg = new byte[12];
                            Array.Copy(rcvMsgTemp, i * 12, rcvMsg, 0, 12);

                            args.Data = new DataByte();
                            args.Data.Byte0 = rcvMsg[0];//(int)(rcvMsg[0]*256) + (int)rcvMsg[1];
                            args.Data.Byte1 = rcvMsg[1];
                            args.Data.Byte2 = rcvMsg[2];
                            args.Data.Byte3 = rcvMsg[3];
                            args.Sendid = args.Data.sendadd;
                            args.ReceiveID = args.Data.receiveadd;
                            args.Data.Byte4 = rcvMsg[4];
                            args.Data.Byte5 = rcvMsg[5];
                            args.Data.Byte6 = rcvMsg[6];
                            args.Data.Byte7 = rcvMsg[7];
                            args.Data.Byte8 = rcvMsg[8];
                            args.Data.Byte9 = rcvMsg[9];
                            args.Data.Byte10 = rcvMsg[10];
                            args.Data.Byte11 = rcvMsg[11];
                            args.EventType = args.Data.EventType;
                            
                      
                            if (DataReceived != null)
                            {
                                DataReceived(this, args);
                            }
                        }
                            
                        break;
                    }

                    durTime = (int)(DateTime.Now.Subtract(startDt).TotalMilliseconds);
                    timeOut = durTime > Tiemout;
                }
                #endregion


                #region ---------- 处理结果 ----------
                if (timeOut)
                {
                    errMsg = "接收超时。";
                    args.Data = null;
                    args.EventType = CommEventType.TimeOut;
                    args.Message = "接收消息超时";
                    args.ReceiveID = -1;
                    args.Sendid = -1;
                }
                #endregion
                Thread.Sleep(10);
            //}
            //});

        }
        #endregion

        #region 共有方法
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="rtsEnable"></param>
        /// <param name="dtrEnable"></param>
        /// <param name="readTimeout"></param>
        /// <param name="writeTimeout"></param>
        /// <param name="receivedBytesThreshold"></param>
        public DevSerialService(string portName = "COM1", int baudRate = 1152000, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, bool rtsEnable = true, bool dtrEnable = false, int readTimeout = Tiemout, int writeTimeout = Tiemout, int receivedBytesThreshold = 20)
        {
            _sp = new SerialPort();
            _sp.PortName = portName;
            _sp.BaudRate = baudRate;
            _sp.Parity = 0;
            _sp.DataBits = 8;
            //_sp.Parity = parity;
            //_sp.DataBits = dataBits;
            //_sp.StopBits = stopBits;
            //_sp.RtsEnable = rtsEnable;
            //_sp.DtrEnable = dtrEnable;
            ////_sp.ReadTimeout = readTimeout;
            ////_sp.WriteTimeout = writeTimeout;
            //_sp.ReceivedBytesThreshold = receivedBytesThreshold;
            _sp.DataReceived += new SerialDataReceivedEventHandler(_sp_DataReceived);
            _sp.Open();
        }

        /// <summary>
        /// 接收发送数据
        /// </summary>
        /// <param name="sendFrame">发送帧</param>
        /// <param name="rcvFrame">接收帧</param>
        /// <param name="rcvBytes">接收到的数据（不含帧头）</param>
        /// <param name="errMsg">错误消息 成功为空</param>
        /// <param name="item">回调handler</param>
        /// <returns></returns>
        public bool SendAndReceive(byte[] sendFrame, ref byte[] rcvFrame, ref byte[] rcvBytes, out string errMsg,GetResultDataHandlerItem item=null)
        {
            #region ---------- 串口初始化 ---------
            if (!_sp.IsOpen)
            {
                _sp.Open();
                if (!_sp.IsOpen)
                {
                    errMsg = "端口未打开。";
                    return false;
                }
            }
            _sp.DiscardInBuffer();
            _sp.DiscardOutBuffer();
            #endregion

            #region ---------- 发送命令 ----------
            if (sendFrame.Length != 12)
            {
                errMsg = "发送帧长度错误。";
                return false;
            }
            _sp.Write(sendFrame, 0, sendFrame.Length);
            errMsg = string.Empty;
            //return true;
            #endregion

            if (item != null) //获取数据
            {
                listHanleritems.Add(item);
                //#region ---------- 接收响应帧 ----------
                //bool timeOut = false;
                //bool rcvOk = false;
                //byte[] rcvMsg = new byte[12];
                //byte[] rcvMsg1;
                //int bufCount = 0;
                //int rcvCount = 0;
                //int totalRcvCount = 0;
                //DateTime startDt = DateTime.Now;
                //int durTime = 0;
                //rcvFrame = new byte[12];
                //rcvBytes = new byte[8];
                //while (!timeOut && !rcvOk)
                //{
                //    bufCount = _sp.BytesToRead;
                //    if (bufCount > 0)
                //    {
                //        rcvMsg1 = new byte[bufCount];
                //        rcvCount = _sp.Read(rcvMsg1, 0, bufCount);
                //        if (totalRcvCount + rcvCount < rcvMsg.Length)
                //        {
                //            for (int i = 0; i < rcvCount; i++)
                //            {
                //                rcvMsg[totalRcvCount + i] = rcvMsg1[i];
                //            }
                //            totalRcvCount += rcvCount;
                //        }
                //        else
                //        {
                //            totalRcvCount = rcvCount;
                //        }
                //    }
                //    if (totalRcvCount >= rcvFrame.Length)
                //    {
                //        Array.Copy(rcvMsg, 0, rcvFrame, 0, rcvFrame.Length);
                //        if ((rcvFrame[0] == sendFrame[2]) && (rcvFrame[1] == sendFrame[3]) && (rcvFrame[2] == sendFrame[0]) && (rcvFrame[3] == sendFrame[1]))
                //        {
                //            Array.Copy(rcvMsg, 4, rcvBytes, 0, rcvBytes.Length);
                //            rcvOk = true;

                //        }
                //        else
                //        {
                //            errMsg = "接收帧错误。";
                //        }
                //        break;
                //    }

                //    durTime = (int)(DateTime.Now.Subtract(startDt).TotalMilliseconds);
                //    timeOut = durTime > Tiemout;
                //}
                //#endregion


                //#region ---------- 处理结果 ----------
                //if (timeOut)
                //{
                //    errMsg = "接收超时。";
                //}
                //else if (rcvOk)
                //{
                //    return true;
                //}
                //return false;
                //#endregion
            }
            return true;
        }
        #endregion


        public void Dispose()
        {
            _sp.Close();
            _sp = null;
            tReleaseThread.Abort();
            tReleaseThread.Join();
            tReleaseThread = null;
        }
    }
}
