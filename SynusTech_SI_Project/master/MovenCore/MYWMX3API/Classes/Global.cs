using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using NetConfigurator;
using WMX3ApiCLR.EcApiCLR;

namespace MYWMX3API.Classes
{
    

    public static class Global
    {
        static readonly public string ModuleINIPath = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + @"\Module.ini";
        static readonly public string ModuleINISimulationPlatformLocation       = @".\platform\simu\";
        static readonly public string ModuleINIEcPlatformLocation               = @".\platform\ethercat\";
        static readonly public string ModuleINICCLinkPlatformLocation           = @".\platform\cclink\";
        static readonly public string ModuleINIM4PlatformLocation               = @".\platform\m4\";

        static readonly public string ModuleINISimulationPlatformDllName    = "simuplatform";
        static readonly public string ModuleINIEcPlatformDllName            = "ecplatform";
        static readonly public string ModuleINICCLinkPlatformDllName        = "cclinkplatform";
        static readonly public string ModuleINIM4PlatformDllName            = "m4platform";

        static readonly public string DefFilePath_EtherCAT = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + ModuleINIEcPlatformLocation + "ec_network.def";
        static readonly public string DefFilePath_CCLink = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + ModuleINICCLinkPlatformLocation + "cclink_network.def";
        static readonly public string DefFilePath_Mecha4 = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + ModuleINIM4PlatformLocation + "m4_network.def";

        static readonly public string RtxTcpIpFilePath_EtherCAT = MYWMX3API.WMXLib.Datas.Engine.WMXInstallDirectory + ModuleINIEcPlatformLocation + "RtxTcpIp.ini";

        public const int MAX_Platform_Num = 4;
        public const int MAX_Module_Num = WMX3ApiCLR.Constants.ModuleLen;

        static public RichTextBox gLogTextBox = null;
        static public TreeView gDeviceTree = null;
        static public MainForm gMainForm = null;
    }

    public static class ConvertEx
    {
        public static byte[] HexStringToByteArray(string hex)
        {
            byte[] bytes = Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
            
            Array.Reverse(bytes);

            return bytes;
        }

        public static byte[] StringtoByteArray(string text)
        {
            return Encoding.Default.GetBytes(text);
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes).Trim('\0');
        }

        public static string ByteArrayToHexString(byte[] bytes)
        {
            Array.Reverse(bytes);
            return BitConverter.ToString(bytes).Replace("-", String.Empty).ToLower();
        }
    }

    public static class MessageBoxEx
    {
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIDEvent, uint dwTime);

        private const int WH_CALLWNDPROCRET = 12;

        private enum CbtHookAction : int
        {
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("User32.dll")]
        private static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

        [DllImport("User32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        [DllImport("user32.dll")]
        private static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        [StructLayout(LayoutKind.Sequential)]
        private struct CWPRETSTRUCT
        {
            public IntPtr lResult;
            public IntPtr lParam;
            public IntPtr wParam;
            public uint message;
            public IntPtr hwnd;
        };

        private static IWin32Window _owner;
        private static HookProc _hookProc;
        private static IntPtr _hHook;

        static MessageBoxEx()
        {
            _hookProc = new HookProc(MessageBoxHookProc);
            _hHook = IntPtr.Zero;
        }

        private static void Initialize()
        {
            if (_hHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }

            if (_owner != null)
            {
                _hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
            }
        }
        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_hHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _hHook;

            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    UnhookWindowsHookEx(_hHook);
                    _hHook = IntPtr.Zero;
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }
        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hChildWnd, ref recChild);

            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = GetWindowRect(_owner.Handle, ref recParent);

            Point ptCenter = new Point(0, 0);
            ptCenter.X = recParent.X + ((recParent.Width - recParent.X) / 2);
            ptCenter.Y = recParent.Y + ((recParent.Height - recParent.Y) / 2);

            Point ptStart = new Point(0, 0);
            ptStart.X = (ptCenter.X - (width / 2));
            ptStart.Y = (ptCenter.Y - (height / 2));

            ptStart.X = (ptStart.X < 0) ? 0 : ptStart.X;
            ptStart.Y = (ptStart.Y < 0) ? 0 : ptStart.Y;

            int result = MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width,
                                    height, false);
        }


        delegate DialogResult ShowErrorMessage_Callback(string message);
        public static DialogResult ShowErrMessage(string message)
        {
            if (Global.gMainForm.InvokeRequired)
            {
                return (DialogResult)Global.gMainForm.Invoke(new ShowErrorMessage_Callback(InvokeShowErrMessage), message);
            }
            else
                return InvokeShowErrMessage(message);
        }

        private static DialogResult InvokeShowErrMessage(string message)
        {
            _owner = Global.gMainForm;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        delegate DialogResult ShowInfoMessage_Callback(string message);
        public static DialogResult ShowInfoMessage(string message)
        {
            if (Global.gMainForm.InvokeRequired)
            {
                return (DialogResult)Global.gMainForm.Invoke(new ShowInfoMessage_Callback(InvokeShowInfoMessage), message);
            }
            else
                return InvokeShowInfoMessage(message);
        }

        private static DialogResult InvokeShowInfoMessage(string message)
        {
            _owner = Global.gMainForm;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        delegate DialogResult ShowQuestionMessage_Callback(string message, MessageBoxButtons buttons);
        public static DialogResult ShowQuestionMessage(string message, MessageBoxButtons buttons)
        {
            if (Global.gMainForm.InvokeRequired)
            {
                return (DialogResult)Global.gMainForm.Invoke(new ShowQuestionMessage_Callback(InvokeShowQuestionMessage), message, buttons);
            }
            else
                return InvokeShowQuestionMessage(message, buttons);
        }
        private static DialogResult InvokeShowQuestionMessage(string message, MessageBoxButtons buttons)
        {
            _owner = Global.gMainForm;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Question",
                buttons,
                MessageBoxIcon.Question);
        }

        delegate DialogResult ShowWarningMessage_Callback(string message, MessageBoxButtons buttons);
        public static DialogResult ShowWarningMessage(string message, MessageBoxButtons buttons)
        {
            if (Global.gMainForm.InvokeRequired)
            {
                return (DialogResult)Global.gMainForm.Invoke(new ShowWarningMessage_Callback(InvokeShowWarningMessage), message, buttons);
            }
            else
                return InvokeShowWarningMessage(message, buttons);
        }
        private static DialogResult InvokeShowWarningMessage(string message, MessageBoxButtons buttons)
        {
            _owner = Global.gMainForm;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Warning",
                buttons,
                MessageBoxIcon.Warning);
        }


        delegate DialogResult ShowErrorMessageInForm_Callback(Form owner, string message);
        public static DialogResult ShowErrMessage(Form owner, string message)
        {
            if (owner.InvokeRequired)
            {
                //owner.pa
                return (DialogResult)Global.gMainForm.Invoke(new ShowErrorMessageInForm_Callback(InvokeShowErrMessage), owner, message);
            }
            else
                return InvokeShowErrMessage(owner, message);
        }

        private static DialogResult InvokeShowErrMessage(Form owner, string message)
        {
            _owner = owner;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        delegate DialogResult ShowInfoMessageInForm_Callback(Form owner, string message);
        public static DialogResult ShowInfoMessage(Form owner, string message)
        {
            if (owner.InvokeRequired)
            {
                return (DialogResult)Global.gMainForm.Invoke(new ShowInfoMessageInForm_Callback(InvokeShowInfoMessage), owner, message);
            }
            else
                return InvokeShowInfoMessage(owner, message);
        }

        private static DialogResult InvokeShowInfoMessage(Form owner, string message)
        {
            _owner = owner;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        delegate DialogResult ShowQuestionMessageInForm_Callback(Form owner, string message, MessageBoxButtons buttons);
        public static DialogResult ShowQuestionMessage(Form owner, string message, MessageBoxButtons buttons)
        {
            if (owner.InvokeRequired)
            {
                return (DialogResult)Global.gMainForm.Invoke(new ShowQuestionMessageInForm_Callback(InvokeShowQuestionMessage), owner, message, buttons);
            }
            else
                return InvokeShowQuestionMessage(owner, message, buttons);
        }
        private static DialogResult InvokeShowQuestionMessage(Form owner, string message, MessageBoxButtons buttons)
        {
            _owner = owner;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Question",
                buttons,
                MessageBoxIcon.Question);
        }

        delegate DialogResult ShowWarningMessageInForm_Callback(Form owner, string message, MessageBoxButtons buttons);
        public static DialogResult ShowWarningMessage(Form owner, string message, MessageBoxButtons buttons)
        {
            if (owner.InvokeRequired)
            {
                return (DialogResult)Global.gMainForm.Invoke(new ShowWarningMessageInForm_Callback(InvokeShowWarningMessage), owner, message, buttons);
            }
            else
                return InvokeShowWarningMessage(owner, message, buttons);
        }
        private static DialogResult InvokeShowWarningMessage(Form owner, string message, MessageBoxButtons buttons)
        {
            _owner = owner;
            Initialize();

            return MessageBox.Show(
                new Form() { TopMost = true },
                message,
                "Warning",
                buttons,
                MessageBoxIcon.Warning);
        }
    }
}
