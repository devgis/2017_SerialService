using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SerialService.libs;

namespace SerialService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool createdNew;
            var mutex = new System.Threading.Mutex(false, "DevSerialService", out createdNew);
            if (!createdNew)
            {
                Loger.ShowError("程序已启动！");
                return;
            }

            try
            {

                Application.Run(new MainForm());
            }
            catch(Exception ex)
            {
                Loger.ShowError("启动程序失败："+ex.Message);
            }
        }
    }
}
