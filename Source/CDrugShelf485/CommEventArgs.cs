using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialService.libs
{
    public class CommEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public CommEventType EventType
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 消息发送者
        /// </summary>
        public int Sendid
        {
            get;
            set;
        }

        /// <summary>
        /// 消息接收者
        /// </summary>
        public int ReceiveID
        {
            get;
            set;
        }

        /// <summary>
        /// 详细消息内容
        /// </summary>
        public DataByte Data
        {
            get;
            set;
        }
    }
}
