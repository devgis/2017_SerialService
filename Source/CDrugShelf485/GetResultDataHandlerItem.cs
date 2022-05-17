using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialService.libs
{
    public class GetResultDataHandlerItem
    {
        private string id = string.Empty;
        public GetResultDataHandlerItem()
        {
            id = Guid.NewGuid().ToString();
        }
        
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Guid.NewGuid().ToString();
                }
                return id;
            }
        }

        /// <summary>
        /// 发送者
        /// </summary>
        public int SenderID
        {
            get;
            set;
        }

        /// <summary>
        /// 接收者
        /// </summary>
        public int ReceiveID
        {
            get;
            set;
        }

        /// <summary>
        /// 命令
        /// </summary>
        public int CMD
        {
            get;
            set;
        }

        /// <summary>
        /// 是否锁
        /// </summary>
        public bool IsLock = false;

        /// <summary>
        /// 是否同步
        /// </summary>
        public bool IsSyncAction = false;

        public int LockNO
        {
            get;
            set;
        }

        /// <summary>
        /// 接收handler
        /// </summary>
        public Action<DataByte> Handler
        {
            get;
            set;
        }

        public DateTime ReceiveTime
        {
            get;
            set;
        }
    }
}
