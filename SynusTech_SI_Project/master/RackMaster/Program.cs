using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace RackMaster {
    static class Program {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main() {
            try {
                Process[] processList = Process.GetProcessesByName("RackMaster");

                if (processList.Length < 2) {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FrmMainForm());
                }
                else {
                    MessageBox.Show("이미 프로그램이 실행중입니다.");
                    return;
                }
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
                return;
            }
        }
    }
}
