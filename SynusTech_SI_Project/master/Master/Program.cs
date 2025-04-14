using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Master
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);

            //프로그램 이미 실행중인 경우 추가 실행 방지
            string asemName = "SynusTechMaster";
            if (IsMyProgRunning(asemName))
            {
                MessageBox.Show("SynusTechMaster is already running.", "Information");
                return;
            }

            try
            {
                Application.Run(new Frm_Main());
            }
            catch (Exception ex) //ObjectDisposedException
            {
                LogMsg.AddExceptionLog(ex, $"Main");
                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// progName의 프로그램 실행 여부 체크
        /// </summary>
        /// <param name="progName"></param>
        /// <returns></returns>
        public static bool IsMyProgRunning(string progName)
        {
            int progRunningCount = 0;
            int checkCount = 0;
            bool bExitCheck = false;

            do
            {
                Process[] procList = Process.GetProcessesByName(progName);

                foreach(Process proc in procList)
                {
                    if (proc.MainModule.FileVersionInfo.ProductName.ToLower().Contains("movensys"))
                        progRunningCount++;
                }
                //progRunningCount = procList.Count();

                //duplicate executed
                if (progRunningCount >= 2)
                {
                    Thread.Sleep(100);
                    checkCount++;

                    //if program is not release duplicate running state during 1sec
                    //program executed duplicate
                    if (checkCount > 10)
                    {
                        return true;
                    }
                }
                else
                {
                    bExitCheck = true;
                }
            }
            while (!bExitCheck);

            return false;
        }
    }
}
