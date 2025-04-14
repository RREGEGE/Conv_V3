using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Reflection.Emit;
using static Synustech.G_Var;

namespace Synustech
{
    public partial class UserTop : UserControl
    {
        DateTime now = DateTime.Now;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private float totalMemory;
        private bool isMouseOverRamLabel = false;

        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_Time;
        private Size initialSize_lbl_Time;
        private Point initialLocation_lbl_Time;

        private float initialFontSize_lblCpuValue;
        private Size initialSize_lblCpuValue;
        private Point initialLocation_lblCpuValue;

        private float initialFontSize_lblRamValue;
        private Size initialSize_lblRamValue;
        private Point initialLocation_lblRamValue;

        private float iniFontSize_btn_Alarm;
        private Size iniSize_____btn_Alarm;
        private Point iniLocation_btn_Alarm;

        private float iniFontSize_btn_Buzzer;
        private Size iniSize_____btn_Buzzer;
        private Point iniLocation_btn_Buzzer;

        private bool buzzerOff = false;
        private bool isResizing = false;
        public UserTop()
        {
            InitializeComponent();
            Timer_Now = new Timer { Interval = 1000 };
            lblRamValue.MouseEnter += LblRamValue_MouseEnter;
            lblRamValue.MouseLeave += LblRamValue_MouseLeave;

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;


            initialFontSize_lbl_Time = lbl_Time.Font.Size;
            initialSize_lbl_Time = lbl_Time.Size;
            initialLocation_lbl_Time = lbl_Time.Location;

            initialFontSize_lblCpuValue = lblCpuValue.Font.Size;
            initialSize_lblCpuValue = lblCpuValue.Size;
            initialLocation_lblCpuValue = lblCpuValue.Location;

            initialFontSize_lblRamValue = lblRamValue.Font.Size;
            initialSize_lblRamValue = lblRamValue.Size;
            initialLocation_lblRamValue = lblRamValue.Location;

            iniFontSize_btn_Alarm = btn_Alarm.Font.Size;
            iniSize_____btn_Alarm = btn_Alarm.Size;
            iniLocation_btn_Alarm = btn_Alarm.Location;

            iniFontSize_btn_Buzzer = btn_Buzzer.Font.Size;
            iniSize_____btn_Buzzer = btn_Buzzer.Size;
            iniLocation_btn_Buzzer = btn_Buzzer.Location;


            this.Resize += Panel_Resize;
        }

        private void UserTop_Load(object sender, EventArgs e)
        {
            Timer_Now.Tick += Timer_Now_Tick;
            Timer_Now.Start();
            
            InitializePerformanceCounters();
            GetTotalMemory();

        }

        private void Timer_Now_Tick(object sender, EventArgs e)
        {
            var culture = new CultureInfo("en-US");
            lbl_Time.Text = DateTime.Now.ToString("yyyy-MM-dd", culture) + "\n" + DateTime.Now.ToString("tt hh:mm:ss", culture);


            UpdatePerformanceCounters();
        }
        private void InitializePerformanceCounters()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        private void GetTotalMemory()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            {
                ulong totalcapacity = 0;
                foreach (var item in searcher.Get())
                {
                    totalcapacity += (ulong)item["capacity"];
                }
                totalMemory = totalcapacity / (1024 * 1024); // mb
            }
        }
        private void UpdatePerformanceCounters()
        {
            float cpuUsage = cpuCounter.NextValue();
            float availableMemory = ramCounter.NextValue();
            float usedMemory = totalMemory - availableMemory;
            float ramUsagePercentage = (usedMemory / totalMemory) * 100;


            lblCpuValue.Text = $"CPU : {cpuUsage:0.0}%";

            if (!isMouseOverRamLabel) // Only update if the mouse is not over the label
            {
                lblRamValue.Text = $"RAM : {ramUsagePercentage:0.0}%";
            }
            lblRamValue.Tag = $"{usedMemory:0.0} / {totalMemory:0.0} MB"; // 

        }
        private void LblRamValue_MouseEnter(object sender, EventArgs e)
        {
            isMouseOverRamLabel = true;
            lblRamValue.Text = lblRamValue.Tag.ToString();// 태그에 저장된 RAM 사용량을 표시
        }

        private void LblRamValue_MouseLeave(object sender, EventArgs e)
        {
            isMouseOverRamLabel = false;
            float availableMemory = ramCounter.NextValue();
            float usedMemory = totalMemory - availableMemory;
            float ramUsagePercentage = (usedMemory / totalMemory) * 100;
            lblRamValue.Text = $"{ramUsagePercentage:0.0}%"; // 다시 %로 표시
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

                // 라벨 폰트 사이즈 조절
                UIFunction.AdjustLabelFontSize(lbl_Time, initialFontSize_lbl_Time, ratio);
                UIFunction.AdjustLabelFontSize(lblCpuValue, initialFontSize_lblCpuValue, ratio);
                UIFunction.AdjustLabelFontSize(lblRamValue, initialFontSize_lblRamValue, ratio);
                UIFunction.AdjustButtonFontSize(btn_Alarm, iniFontSize_btn_Alarm, ratio);
                UIFunction.AdjustButtonFontSize(btn_Buzzer, iniFontSize_btn_Buzzer, ratio);

                // 라벨 크기 및 위치 조절
                UIFunction.AdjustLabel(lbl_Time, initialSize_lbl_Time, initialLocation_lbl_Time, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblCpuValue, initialSize_lblCpuValue, initialLocation_lblCpuValue, widthRatio, heightRatio);
                UIFunction.AdjustLabel(lblRamValue, initialSize_lblRamValue, initialLocation_lblRamValue, widthRatio, heightRatio);
                UIFunction.AdjustButton(btn_Alarm, iniFontSize_btn_Alarm, iniLocation_btn_Alarm, iniSize_____btn_Alarm, ratio);
                UIFunction.AdjustButton(btn_Buzzer, iniFontSize_btn_Buzzer, iniLocation_btn_Buzzer, iniSize_____btn_Buzzer, ratio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void btn_Buzzer_Click_1(object sender, EventArgs e)
        {
            buzzerOff = !buzzerOff;
            if (buzzerOff == true)
            {
                btn_Buzzer.Text = "Buzzer Off";
                btn_Buzzer.ForeColor = Color.Red;
                G_Var.isBuzzerSound = false;
            }
            else
            {
                btn_Buzzer.Text = "Buzzer On";
                btn_Buzzer.ForeColor = Color.FromArgb(50, 226, 178);
                G_Var.isBuzzerSound = true;
            }
            Console.WriteLine("Buzzer:" + buzzerOff);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_Alarm_Click(object sender, EventArgs e)
        {
            G_Var.isAlarm = false;
            G_Var.isSafetyAlarm = false;
            G_Var.isMainPowerAlarm = false;
            G_Var.isEMOAlarm = false;
            G_Var.isEMSAlarm = false;
            G_Var.isWMX_E_Stop = false;
            G_Var.Software_E_Stop = false;
            G_Var.Master_Mode_Change_Error = false;
            foreach (var conv in G_Var.conveyors)
            {
                conv.mode = Mode.Manual;
            }
        }
    }
}
