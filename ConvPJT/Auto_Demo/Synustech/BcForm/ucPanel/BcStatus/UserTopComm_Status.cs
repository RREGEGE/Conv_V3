using Synustech.BcForm;
using Synustech.ucPanel.BcAlarm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Synustech.G_Var;

namespace Synustech.ucPanel.BcStatus
{
    
    public partial class UserTopComm_Status : UserControl
    {
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float iniFontSize_lbl_Main;
        private Size iniSize_____lbl_Main;
        private Point iniLocation_lbl_Main;

        private float iniFontSize_lbl_IO;
        private Size  iniSize_____lbl_IO;
        private Point iniLocation_lbl_IO;

        private float iniFontSize_lbl_RFID;
        private Size iniSize_____lbl_RFID;
        private Point iniLocation_lbl_RFID;

        private float iniFontSize_lbl_Conv;
        private Size iniSize_____lbl_Conv;
        private Point iniLocation_lbl_Conv;


        private bool isResizing = false;
        private bool btn_MainClick = false;
        private bool btn_IOClick = false;
        private bool btn_SettingClick = false;

        private bool btn_RFIDClick = false;
        private bool btn_ConvClick = false;
        private bool btn_AlarmClick = false;


        public static event delMonitorLogView del_ELogSender;

        public delegate void delMonitorIO();
        public delegate void delMonitorSetting();
        public delegate void delMonitorDrive();
        public delegate void delMonitorRFID();
        public delegate void delMonitorConv();
        public delegate void delMonitorMain();
        
        public delMonitorIO del_PanelIO;
        public delMonitorSetting del_PanelSetting;
        public delMonitorDrive del_PanelDrive;
        public delMonitorRFID del_PanelRFID;
        public delMonitorConv del_PanelConv;
        public delMonitorMain del_PanelMain;

        public UserTopComm_Status()
        {
            InitializeComponent();
            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            iniFontSize_lbl_Main = btn_Main.Font.Size;
            iniSize_____lbl_Main = btn_Main.Size;
            iniLocation_lbl_Main = btn_Main.Location;

            iniFontSize_lbl_IO = btn_IO.Font.Size;
            iniSize_____lbl_IO = btn_IO.Size;
            iniLocation_lbl_IO = btn_IO.Location;

            iniFontSize_lbl_RFID = btn_RFID.Font.Size;
            iniSize_____lbl_RFID = btn_RFID.Size;
            iniLocation_lbl_RFID = btn_RFID.Location;

            iniFontSize_lbl_Conv = btn_Conv.Font.Size;
            iniSize_____lbl_Conv = btn_Conv.Size;
            iniLocation_lbl_Conv = btn_Conv.Location;

            btn_Main.Click += btn_Main_Click;
            btn_IO.Click += btn_IO_Click;
            btn_RFID.Click += btn_RFID_Click;

            this.Resize += Panel_Resize;
        }
        public void init()
        {
            btn_Main.BackColor = Color.FromArgb(37, 41, 64);
            btn_MainClick = true;
        }
        public void ResetButtonStates()
        {
            Color BackColor = Color.FromArgb(24, 30, 54);

            btn_MainClick = false;
            btn_IOClick = false;
            btn_SettingClick = false;
            btn_RFIDClick = false;
            btn_ConvClick = false;

            // 원래 버튼 색상으로 초기화
            btn_Main.BackColor = BackColor;
            btn_IO.BackColor = BackColor;
            btn_RFID.BackColor = BackColor;
            btn_Conv.BackColor = BackColor;
        }
        private void Panel_Resize(object sender, EventArgs e)
        {
            // 이미 크기 조정 중이면 중복 처리 방지
            if (isResizing)
                return;
            try
            {
                isResizing = true;

                if (initialPanelWidth == 0 || initialPanelHeight == 0)
                {
                    return; // 초기 패널 크기가 0인 경우 아무 것도 하지 않음
                }

                // Panel 크기 비율 계산
                float widthRatio = (float)this.Width / initialPanelWidth;
                float heightRatio = (float)this.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 사이즈 조절
                UIFunction.AdjustButtonFontSize(btn_Main, iniFontSize_lbl_Main, ratio);
                UIFunction.AdjustButtonFontSize(btn_IO, iniFontSize_lbl_IO, ratio);
                UIFunction.AdjustButtonFontSize(btn_RFID, iniFontSize_lbl_RFID, ratio);
                UIFunction.AdjustButtonFontSize(btn_Conv, iniFontSize_lbl_Conv, ratio);
                
                UIFunction.AdjustButton(btn_Main, iniFontSize_lbl_Main, iniLocation_lbl_Main, iniSize_____lbl_Main, ratio);
                UIFunction.AdjustButton(btn_IO, iniFontSize_lbl_IO, iniLocation_lbl_IO, iniSize_____lbl_IO, heightRatio);
                                 
                UIFunction.AdjustButton(btn_RFID, iniFontSize_lbl_RFID, iniLocation_lbl_RFID, iniSize_____lbl_RFID, ratio);
                UIFunction.AdjustButton(btn_Conv, iniFontSize_lbl_Conv, iniLocation_lbl_Conv, iniSize_____lbl_Conv, ratio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
       
        private void btn_IO_Click(object sender, EventArgs e)
        {
            if (!btn_IOClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_IO.BackColor = Color.FromArgb(37, 41, 64);
                del_PanelIO?.Invoke();
                btn_IOClick = true;
            }
        }

        private void btn_RFID_Click(object sender, EventArgs e)
        {
            if (!btn_RFIDClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_RFID.BackColor = Color.FromArgb(37, 41, 64);
                del_PanelRFID?.Invoke();
                btn_RFIDClick = true;
            }
        }

        private void btn_Conv_Click(object sender, EventArgs e)
        {
            Form ConveyorLine = new ConveyorLine();
            ConveyorLine.ShowDialog();
        }

        private void btn_Main_Click(object sender, EventArgs e)
        {
            if (!btn_MainClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();

                //ELogSender(sender, enLogType.Application, enLogLevel.Nomal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_Main.BackColor = Color.FromArgb(37, 41, 64);
                del_PanelMain?.Invoke();
                btn_MainClick = true;
            }
        }
    }
}
