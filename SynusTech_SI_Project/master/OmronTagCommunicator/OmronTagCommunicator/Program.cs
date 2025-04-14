using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace OmronTagCommunicator
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

            string asemName = "OmronTagCommunicator";
            if (IsMyProgRunning(asemName))
            {
                MessageBox.Show("OmronTagCommunicator is already running.", "Information");
                return;
            }

            try
            {
                Application.Run(new Form1());
            }
            catch (Exception ex) //ObjectDisposedException
            {
                MessageBox.Show(ex.ToString(), "Exception Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsMyProgRunning(string progName)
        {
            int progRunningCount;
            int checkCount = 0;
            bool bExitCheck = false;

            do
            {
                Process[] procList = Process.GetProcessesByName(progName);
                progRunningCount = procList.Count();

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
