using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace RackMaster.SEQ.CLS {
    public static class ProcessControl {
        /// <summary>
        /// 현재 실행되는 이 프로그램이 관리자 권한으로 실행되었는지 판단하는 함수
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator() {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if(identity != null) {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }
        /// <summary>
        /// 특정 위치에 있는 프로그램을 관리자 권한으로 실행하는 함수
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool RunAsAdministrator(string filePath, string fileName) {
            if (!IsAdministrator()) {
                try {
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.UseShellExecute = true;
                    procInfo.FileName = fileName;
                    procInfo.WorkingDirectory = filePath;
                    procInfo.Verb = "runas";
                    Process.Start(procInfo);

                    return true;
                }catch(Exception ex) {
                    return false;
                }
            }
            else {
                string name = $"{filePath}\\{fileName}";
                if (Process.Start(name) == null)
                    return false;

                return true;
            }
        }
        /// <summary>
        /// 툭정 위치에 있는 프로그램을 실행시키는 함수
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool RunProgram(string filePath, string fileName) {
            try {
                string name = $"{filePath}\\{fileName}";
                if (Process.Start(name) == null)
                    return false;

                return true;
            }catch(Exception ex) {
                return false;
            }
        }
        /// <summary>
        /// 특정 Process 이름의 프로그램이 실행되고 있는지 판단하는 함수
        /// </summary>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static bool IsRunningProcess(string procName) {
            try {
                Process[] processList = Process.GetProcessesByName(procName);

                if (processList.Length == 0)
                    return false;
                else
                    return true;
            }catch(Exception ex) {
                return false;
            }
        }
    }
}
