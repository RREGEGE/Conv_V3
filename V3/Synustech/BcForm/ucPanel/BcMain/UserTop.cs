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

namespace Synustech
{
    public partial class UserTop : UserControl
    {
        //DateTime now = DateTime.Now;
        //private PerformanceCounter cpuCounter;
        //private PerformanceCounter ramCounter;
        //private float totalMemory;
        //private bool isMouseOverRamLabel = false;

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

        private float iniFontSize_lbl_Alarm;
        private Size iniSize_____lbl_Alarm;
        private Point iniLocation_lbl_Alarm;

        private float iniFontSize_lbl_Buzzer;
        private Size iniSize_____lbl_Buzzer;
        private Point iniLocation_lbl_Buzzer;

        private bool BuzzerOff = true;
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

            iniFontSize_lbl_Alarm = btn_Alarm.Font.Size;
            iniSize_____lbl_Alarm = btn_Alarm.Size;
            iniLocation_lbl_Alarm = btn_Alarm.Location;

            iniFontSize_lbl_Buzzer = btn_Buzzer.Font.Size;
            iniSize_____lbl_Buzzer = btn_Buzzer.Size;
            iniLocation_lbl_Buzzer = btn_Buzzer.Location;


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
            //cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        private void GetTotalMemory()
        {
            //using (var searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory"))
            //{
            //    ulong totalCapacity = 0;
            //    foreach (var item in searcher.Get())
            //    {
            //        totalCapacity += (ulong)item["Capacity"];
            //    }
            //    totalMemory = totalCapacity / (1024 * 1024); // MB
            //}
        }
        private void UpdatePerformanceCounters()
        {
            //float cpuUsage = cpuCounter.NextValue();
            //float availableMemory = ramCounter.NextValue();
            //float usedMemory = totalMemory - availableMemory;
            //float ramUsagePercentage = (usedMemory / totalMemory) * 100;


            //lblCpuValue.Text = $"CPU : {cpuUsage:0.0}%";

            //if (!isMouseOverRamLabel) // Only update if the mouse is not over the label
            //{
            //    lblRamValue.Text = $"RAM : {ramUsagePercentage:0.0}%";
            //}
            //lblRamValue.Tag = $"{usedMemory:0.0} / {totalMemory:0.0} MB"; // 

        }
        private void LblRamValue_MouseEnter(object sender, EventArgs e)
        {
            //isMouseOverRamLabel = true;
            //lblRamValue.Text = lblRamValue.Tag.ToString();// 태그에 저장된 RAM 사용량을 표시
        }

        private void LblRamValue_MouseLeave(object sender, EventArgs e)
        {
            //isMouseOverRamLabel = false;
            //float availableMemory = ramCounter.NextValue();
            //float usedMemory = totalMemory - availableMemory;
            //float ramUsagePercentage = (usedMemory / totalMemory) * 100;
            //lblRamValue.Text = $"{ramUsagePercentage:0.0}%"; // 다시 %로 표시
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
                AdjustFontSize(lbl_Time, initialFontSize_lbl_Time, ratio);
                AdjustFontSize(lblCpuValue, initialFontSize_lblCpuValue, ratio);
                AdjustFontSize(lblRamValue, initialFontSize_lblRamValue, ratio);
                AdjustFontSize(btn_Alarm, iniFontSize_lbl_Alarm, ratio);
                AdjustFontSize(btn_Buzzer, iniFontSize_lbl_Buzzer, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lbl_Time, initialSize_lbl_Time, initialLocation_lbl_Time, widthRatio, heightRatio);
                AdjustLabel(lblCpuValue, initialSize_lblCpuValue, initialLocation_lblCpuValue, widthRatio, heightRatio);
                AdjustLabel(lblRamValue, initialSize_lblRamValue, initialLocation_lblRamValue, widthRatio, heightRatio);
                AdjustLabel(btn_Alarm, iniSize_____lbl_Alarm, iniLocation_lbl_Alarm, widthRatio, heightRatio);
                AdjustLabel(btn_Buzzer, iniSize_____lbl_Buzzer, iniLocation_lbl_Buzzer, widthRatio, heightRatio);
            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void AdjustFontSize(System.Windows.Forms.Button button, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;
            float maxFontSize = newFontSize * 3;
            newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                button.Font = new Font(button.Font.FontFamily, newFontSize, button.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio;
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                label.Font = new Font(label.Font.FontFamily, newFontSize, label.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }
        private void AdjustLabel(System.Windows.Forms.Button button, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                button.Width = (int)(initialSize.Width * widthRatio);
                button.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                button.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void AdjustLabel(System.Windows.Forms.Label label, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                label.Width = (int)(initialSize.Width * widthRatio);
                label.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                label.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        private void btn_Buzzer_Click_1(object sender, EventArgs e)
        {
            BuzzerOff = !BuzzerOff;
            if (BuzzerOff == true)
            {
                btn_Buzzer.Text = "Buzzer Off";
                btn_Buzzer.ForeColor = Color.Red;
                G_Var.IsBuzzerSound = false;
            }
            else
            {
                btn_Buzzer.Text = "Buzzer On";
                btn_Buzzer.ForeColor = Color.Green;
                G_Var.IsBuzzerSound = true;
            }
        }
    }
}
