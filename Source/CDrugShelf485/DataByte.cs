using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialService.libs
{
    public class DataByte
    {
        private byte[] byteArray = new byte[12] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        
        /// <summary>
        /// 发送方add
        /// </summary>
        public int sendadd
        {
            get{
                return BitConverter.ToInt16(new byte[] { byteArray[1], byteArray[0] }, 0);
            }
            set{
                byte[] temp = BitConverter.GetBytes(value);
                byteArray[0] = temp[1];
                byteArray[1] = temp[0];
            }
        }
        
        /// <summary>
        /// 接收方add
        /// </summary>
        public int receiveadd
        {
            get{
                return BitConverter.ToInt16(new byte[] { byteArray[3], byteArray[2] }, 0);
            }
            set{
                byte[] temp = BitConverter.GetBytes(value);
                byteArray[2] = temp[1];
                byteArray[3] = temp[0];
            }
        }

        /// <summary>
        /// 命令头 4
        /// </summary>
        public int Byte0
        {
            get
            {
                return byteArray[0];
            }
            set
            {
                byteArray[0] = Convert.ToByte(value);
            }
        }

        /// <summary>
        /// 命令头 1
        /// </summary>
        public int Byte1
        {
            get
            {
                return byteArray[1];
            }
            set
            {
                byteArray[1] = Convert.ToByte(value);
            }
        }

        /// <summary>
        /// 命令头 2
        /// </summary>
        public int Byte2
        {
            get
            {
                return byteArray[2];
            }
            set
            {
                byteArray[2] = Convert.ToByte(value);
            }
        }

        /// <summary>
        /// 命令头 3
        /// </summary>
        public int Byte3
        {
            get
            {
                return byteArray[3];
            }
            set
            {
                byteArray[3] = Convert.ToByte(value);
            }
        }

        /// <summary>
        /// 命令头 4
        /// </summary>
        public int Byte4
        {
            get
            {
                return byteArray[4];
            }
            set
            {
                byteArray[4] = Convert.ToByte(value);
            }
        }
        
        /// <summary>
        /// 帧序号 5
        /// </summary>
        public int Byte5
        {
            get
            {
                return byteArray[5];
            }
            set
            {
                byteArray[5] = Convert.ToByte(value);
            }
        }

        /// <summary>
        /// 命令 6
        /// </summary>
        public int Byte6
        {
            get
            {
                return byteArray[6];
            }
            set
            {
                byteArray[6] = Convert.ToByte(value);
            }
        }
        /// <summary>
        /// 保留 7
        /// </summary>
        public int Byte7
        {
            get
            {
                return byteArray[7];
            }
            set
            {
                byteArray[7] = Convert.ToByte(value);
            }
        }
        /// <summary>
        /// 保留 8
        /// </summary>
        public int Byte8
        {
            get
            {
                return byteArray[8];
            }
            set
            {
                byteArray[8] = Convert.ToByte(value);
            }
        }
        /// <summary>
        /// 保留 9
        /// </summary>
        public int Byte9
        {
            get
            {
                return byteArray[9];
            }
            set
            {
                byteArray[9] = Convert.ToByte(value);
            }
        }
        /// <summary>
        /// 保留 10
        /// </summary>
        public int Byte10
        {
            get
            {
                return byteArray[10];
            }
            set
            {
                byteArray[10] = Convert.ToByte(value);
            }
        }
        /// <summary>
        /// CRC8 11
        /// </summary>
        public int Byte11
        {
            get
            {
                return byteArray[11];
            }
            set
            {
                byteArray[11] = Convert.ToByte(value);
            }
        }
        public byte[] ByteArray
        {
            get{
                return getByteArray();
            }
            
        }
        private byte[] getByteArray()
        {
            byte[] temp = BitConverter.GetBytes(sendadd);
            byteArray[0] = temp[1];
            byteArray[1] = temp[0];

            temp = BitConverter.GetBytes(receiveadd);
            byteArray[2] = temp[1];
            byteArray[3] = temp[0];

            byteArray[4] = Convert.ToByte(Byte4);
            byteArray[5] = Convert.ToByte(Byte5);
            byteArray[6] = Convert.ToByte(Byte6);
            byteArray[7] = Convert.ToByte(Byte7);
            byteArray[8] = Convert.ToByte(Byte8);
            byteArray[9] = Convert.ToByte(Byte9);
            byteArray[10] = Convert.ToByte(Byte10);
            byte[] bcrcbyte = new byte[7];
            Array.Copy(byteArray,4, bcrcbyte, 0, 7);
            if (byteArray[11] == 0x00)
            {
                byte crcValue = CRC8.CRC(bcrcbyte, 0, 7);
                byteArray[11] = crcValue;
            }
            return byteArray;
        }

        public override string ToString()
        {
            string temp = "[";
            for (int i = 0; i < ByteArray.Length; i++)
            {
                temp += string.Format("{0}", ByteArray[i].ToString("X2"));
            }
            temp += "]";
            return temp;
        }

        public CommEventType EventType
        {
            get
            {
                return getEventType();
            }
        }

        private CommEventType getEventType()
        {
            switch (Byte6)
            {
                case 0x21:
                    return CommEventType.分包机药盒拿走主动上报;
                case 0x22:
                    return CommEventType.分包机药盒放上主动上报;
                case 0xA0:
                    return CommEventType.分包机计数传感器异常;
                case 0xA1:
                    return CommEventType.分包机电机强制停止;
                case 0x15:
                    return CommEventType.电源电压欠压报警_下位机向上位机;
                case 0x16:
                    return CommEventType.继电器动作后电机未转动报警_下位机向上位机;
                case 0x23:
                    return CommEventType.单个抽屉进或者出时间超过时认为超时报警_下位机向上位机;
                case 0x25:
                    return CommEventType.锁打开报警_下位机向上位机;
                case 0x26:
                    return CommEventType.冰箱温度异常报警_下位机向上位机;
                case 0x27:
                    return CommEventType.急停按下报警_下位机向上位机;
                case 0x01:
                    if (Byte5 == 0x01)
                    {
                        return CommEventType.分包机发药下位机回复上位机收到立即回复;
                    }
                    else if (Byte5 == 0x02)
                    {
                        return CommEventType.分包机发药下位机回复上位机收到完成回复;
                    }
                    else
                    {
                        return CommEventType.未知类型;
                    }
                case 0x03:
                    if (Byte5 == 0x01)
                    {
                        return CommEventType.抽屉出_下位机回复上位机立即;
                    }
                    else if (Byte5 == 0x02)
                    {
                        return CommEventType.抽屉出_下位机回复上位机完成;
                    }
                    else
                    {
                        return CommEventType.未知类型;
                    }
                case 0x04:
                    if (Byte5 == 0x01)
                    {
                        return CommEventType.抽屉进_下位机回复上位机立即;
                    }
                    else if (Byte5 == 0x02)
                    {
                        return CommEventType.抽屉进_下位机回复上位机完成;
                    }
                    else
                    {
                        return CommEventType.未知类型;
                    }
                case 0x05:
                    if (Byte5 == 0x00)
                    {
                        return CommEventType.获取5V电源电压值_上位机向下位;
                    }
                    else
                    {
                        return CommEventType.获取5V电源电压值_下位机回复上位;
                    }
                case 0x06:
                    if (Byte5 == 0x00)
                    {
                        return CommEventType.发药结束取药提醒_上位机向下位;
                    }
                    else if (Byte5 == 0x01)
                    {
                        return CommEventType.发药结束取药提醒_下位机回复上位立即;
                    }
                    else if (Byte5 == 0x02)
                    {
                        return CommEventType.发药结束取药提醒_下位机回复上位完成;
                    }
                    break;
                case 0x10:
                    if (Byte5 == 0x00)
                    {
                        return CommEventType.锁开启_上位机向下位;
                    }
                    else if (Byte5 == 0x01)
                    {
                        return CommEventType.锁开启_下位机回复上位立即;
                    }
                    else if (Byte5 == 0x02)
                    {
                        return CommEventType.锁开启_下位机回复上位完成;
                    }
                    break;
                case 0x09:
                    if (Byte5 == 0x00)
                    {
                        return CommEventType.门上锁信号采集_上位机向下位;
                    }
                    else
                    {
                        return CommEventType.门上锁信号采集_下位机回复上位机;
                    }

                case 0x07:
                    if (Byte5 == 0x00)
                    {
                        return CommEventType.冰箱温度采集_上位机向下位;
                    }
                    else
                    {
                        return CommEventType.冰箱温度采集_下位机回复上位机;
                    }

                case 0x08:
                    if (Byte5 == 0x00)
                    {
                        return CommEventType.急停信号采集_上位机向下位;
                    }
                    else
                    {
                        return CommEventType.急停信号采集_下位机回复上位机;
                    }
                }
                return CommEventType.TimeOut;
            }
        
    }
}
