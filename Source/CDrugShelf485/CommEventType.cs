using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialService.libs
{
    public enum CommEventType
    {
        分包机发药下位机回复上位机收到立即回复,
        分包机发药下位机回复上位机收到完成回复,
        分包机药盒拿走主动上报,
        分包机药盒放上主动上报,
        分包机计数传感器异常,
        分包机电机强制停止,
        电源电压欠压报警_下位机向上位机,
        继电器动作后电机未转动报警_下位机向上位机,
        单个抽屉进或者出时间超过时认为超时报警_下位机向上位机,
        锁打开报警_下位机向上位机,
        冰箱温度异常报警_下位机向上位机,
        急停按下报警_下位机向上位机,
        电源电压欠压报警上位机向下位机,
        继电器动作后电机未转动报警_上位机向下位机,
        单个抽屉进或者出时间超过时认为超时报警_上位机向下位机,
        锁打开报警_上位机向下位机,
        冰箱温度异常报警_上位机向下位机,
        急停按下报警_上位机向下位机,
        接收超时_上位机向下位机,
        抽屉出_上位机向下位,
        抽屉出_下位机回复上位机立即,
        抽屉出_下位机回复上位机完成,
        抽屉进_上位机向下位,
        抽屉进_下位机回复上位机立即,
        抽屉进_下位机回复上位机完成,
        获取5V电源电压值_上位机向下位,
        获取5V电源电压值_下位机回复上位,
        发药结束取药提醒_上位机向下位,
        发药结束取药提醒_下位机回复上位立即,
        发药结束取药提醒_下位机回复上位完成,
        锁开启_上位机向下位,
        锁开启_下位机回复上位立即,
        锁开启_下位机回复上位完成,
        门上锁信号采集_上位机向下位,
        门上锁信号采集_下位机回复上位机,
        冰箱温度采集_上位机向下位,
        冰箱温度采集_下位机回复上位机,
        急停信号采集_上位机向下位,
        急停信号采集_下位机回复上位机,
        TimeOut,
        未知类型
    }
}
