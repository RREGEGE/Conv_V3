using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Test {
    class VKeyboard {
        [DllImport("User32.DLL")]
        public static extern Boolean PostMessage(Int32 hWnd, Int32 Msg, Int32 wParam, Int32 lParam);
        public const Int32 WM_USER = 1024;
        public const Int32 WM_CSKEYBOARD = WM_USER + 192;
        public const Int32 WM_CSKEYBOARDMOVE = WM_USER + 193;
        public const Int32 WM_CSKEYBOARDRESIZE = WM_USER + 197;

        public static Process keyboardPs = null;
        public static void showKeyboard() {
            if(keyboardPs == null) {
                string filePath;
                if(Environment.Is64BitOperatingSystem) {
                    filePath = Path.Combine(Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "winsxs"),
                        "amd64_microsoft-windows-osk_*")[0],
                        "osk.exe");
                } else {
                    filePath = @"C:\windows\system32\osk.exe";
                }
                if(File.Exists(filePath)) {
                    keyboardPs = Process.Start(filePath);
                }
            }

        }
        public static void hideKeyboard() {
            //if(keyboardPs != null) {
            //    keyboardPs.Kill();
            //    keyboardPs = null;
            //}
            if (keyboardPs != null && !keyboardPs.HasExited) // 프로세스가 null이 아니고, 종료되지 않았는지 확인
            {
                try
                {
                    keyboardPs.Kill(); // 프로세스 종료
                    keyboardPs = null; // 종료 후 null로 설정
                }
                catch (InvalidOperationException ex)
                {
                    // 프로세스가 이미 종료된 경우 발생할 수 있는 예외 처리
                    Console.WriteLine("프로세스가 이미 종료되었습니다: " + ex.Message);
                    keyboardPs = null;
                }
            }
        }

        public static void moveWindow(int x, int y, int w, int h) {
            if(keyboardPs.Handle != null) {
                PostMessage(keyboardPs.Handle.ToInt32(), WM_CSKEYBOARDMOVE, x, y); // Move to 0, 0
                PostMessage(keyboardPs.Handle.ToInt32(), WM_CSKEYBOARDRESIZE, w, h); // Resize to 600, 300
            }
        }
    }
}
