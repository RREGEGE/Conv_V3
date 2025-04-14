using Synustech.BcForm;
using Synustech.ucPanel.BcStatus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static Synustech.ucPanel.BcStatus.UserTopComm_Status;


namespace Synustech
{
    public partial class UserMenu : UserControl
    {


        /// <summary>
        /// IntialSize를 위한 변수
        /// </summary>
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lbl_Login;
        private float initialFontSize_lblMaker;

        private Point initialLocation_lbl_Login;
        private Point initialLocation_lblMaker;
        private Point initialLocation_PictureLogin;


        private Size initialSize_lbl_Login;
        private Size initialSize_lblMaker;
        private Size initialSize_PictureLogin;


        private float iniFontSize_Menubtn;
        private Point iniLocation_Menubtn;
        private Size  iniSize_____Menubtn;

        private bool isResizing         = false;
        private bool btn_ExitClick      = false;
        private bool btn_TeachingClick  = false;
        private bool btn_SettingClick   = false;
        private bool btn_AlarmClick     = false;
        private bool btn_MotionClick    = false;
        private bool btn_MainClick      = false;
        private bool btn_StatusClick    = false;



        public static event delMonitorLogView ELogSender;
        public delegate void SubReset();

        public SubReset Subreset_Status;
        public SubReset Subreset_Motion;
        public SubReset Subreset_Setting;
        public delMenuBtnVisible    buttonVisible;
        public delLoginlblIdChanged lblIdChange;
        public delMonitorMain       PanelMain;
        public delMonitorStatus     PanelStatus;
        public delMonitorMotion     PanelMotion;
        public delMonitorAlarm      PanelAlarm;
        public delMonitorSetting    PanelSetting;
        public delMonitorChange     PanelChange;



        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]


        private static extern IntPtr CreateRoundRectRgn
        (
          int nLeftRect,
          int nTopRect,
          int nRightRect,
          int nBottomRect,
          int nWidthEllipse,
          int nHeightEllipse
        );

        /// <summary>
        /// 최초 이미지들에 대한 정보 저장.
        /// </summary>

        private Dictionary<Label, float>      originalLabelFontSizes   = new Dictionary<Label, float>();
        private Dictionary<Button, Image> originalButtonImages = new Dictionary<Button, Image>();

        public UserMenu()
        {
     
            InitializeComponent();
            IntButtonVisibility();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            initialPanelWidth            = tableLayoutPanel1.Width;
            initialPanelHeight           = tableLayoutPanel1.Height;
            initialFontSize_lbl_Login    = lbl_Login.Font.Size;
            initialFontSize_lblMaker     = lblMaker.Font.Size;


            initialSize_lbl_Login        = lbl_Login.Size;
            initialSize_lblMaker         = lblMaker.Size;
            initialSize_PictureLogin     = PictureLogin.Size;

            initialLocation_lbl_Login    = lbl_Login.Location;
            initialLocation_lblMaker     = lblMaker.Location;
            initialLocation_PictureLogin = PictureLogin.Location;


            iniFontSize_Menubtn        = btn_Exit.Font.Size;
            iniLocation_Menubtn        = btn_Exit.Location;
            iniSize_____Menubtn        = btn_Exit.Size;

           
            /// <summary>
            /// Nav바 크기 정의.
            /// </summary>
            PanelNav.Height = btn_Main.Height;
            PanelNav.Top    = btn_Main.Top;
            PanelNav.Left   = btn_Main.Left;
            //btn_Main.BackColor = Color.FromArgb(46, 51, 73);

            /// <summary>
            /// 초기화 함수
            /// </summary>
            StoreOriginalButtonImages();
            tableLayoutPanel1.Resize += Panel_Resize;

        }
        private void ResetButtonStates()
        {
            // 원래 버튼 색상으로 초기화
            btn_ExitClick = false;
            btn_TeachingClick = false;
            btn_SettingClick = false;
            btn_AlarmClick = false;
            btn_MotionClick = false;
            btn_MainClick = false;
            btn_StatusClick = false;

            // 원래 스플릿 크기로 줄이기
            PanelChange?.Invoke();
        }
        private void StoreOriginalButtonImages()
        {
            StoreButtonImage(btn_Exit);
            //StoreButtonImage(btn_Teaching);
            StoreButtonImage(btn_Setting);
            StoreButtonImage(btn_Alarm);
            StoreButtonImage(btn_Motion);
            StoreButtonImage(btn_Main);
            StoreButtonImage(btn_Status);
        }
        private void StoreButtonImage(Button btn)
        {
            if (btn.Image != null && !originalButtonImages.ContainsKey(btn))
            {
                originalButtonImages[btn] = btn.Image;
                //ResizeButtonImage(btn, 0.5f); // 초기 크기를 50%로 줄임
            }
        }
        private void ResizeButtonImage(Button btn, float ratio)
        {
            try
            {
                if (originalButtonImages.ContainsKey(btn))
                {
                    Image originalImage = originalButtonImages[btn];
                    int newWidth = Math.Max((int)(originalImage.Width * ratio*1.5f), 1); // 최소 너비를 1로 설정
                    int newHeight = Math.Max((int)(originalImage.Height * ratio*1.5f), 1); // 최소 높이를 1로 설정
                    btn.Image = new Bitmap(originalImage, new Size(newWidth, newHeight));
                }
            }
            catch (Exception ex)
            {
                // 예외 발생 시 로그 작성 또는 메시지 박스 출력
                Console.WriteLine($"이미지 사이즈 변경 중 에러: {ex.Message}");
               
            }
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
                float widthRatio = (float)tableLayoutPanel1.Width / initialPanelWidth;
                float heightRatio = (float)tableLayoutPanel1.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                AdjustFontSize(lbl_Login, initialFontSize_lbl_Login, ratio);
                AdjustFontSize(lblMaker, initialFontSize_lblMaker, ratio);

                // 라벨 크기 및 위치 조절
                AdjustLabel(lbl_Login, initialSize_lbl_Login, initialLocation_lbl_Login, widthRatio, heightRatio);
                AdjustLabel(lblMaker, initialSize_lblMaker, initialLocation_lblMaker, widthRatio, heightRatio);
                AdjustPictureBox(PictureLogin, initialSize_PictureLogin, initialLocation_PictureLogin, widthRatio, heightRatio);

                // 버튼 크기 및 위치 조절

                AdjustButton(btn_Exit, iniFontSize_Menubtn, iniLocation_Menubtn, iniSize_____Menubtn, ratio);
                //AdjustButton(btn_Teaching, iniFontSize_Menubtn, iniLocation_Menubtn, iniSize_____Menubtn, ratio);
                AdjustButton(btn_Setting, iniFontSize_Menubtn, iniLocation_Menubtn, iniSize_____Menubtn, ratio);
                AdjustButton(btn_Alarm, iniFontSize_Menubtn, iniLocation_Menubtn, iniSize_____Menubtn, ratio);
                AdjustButton(btn_Motion, iniFontSize_Menubtn, iniLocation_Menubtn, iniSize_____Menubtn, ratio);
                AdjustButton(btn_Main, iniFontSize_Menubtn, iniLocation_Menubtn, iniSize_____Menubtn, ratio);
                AdjustButton(btn_Status, iniFontSize_Menubtn, iniLocation_Menubtn, iniSize_____Menubtn, ratio);

                ResizeButtonImage(btn_Exit, ratio);
                //ResizeButtonImage(btn_Teaching, ratio);
                ResizeButtonImage(btn_Setting, ratio);
                ResizeButtonImage(btn_Alarm, ratio);
                ResizeButtonImage(btn_Motion, ratio);
                ResizeButtonImage(btn_Main, ratio);
                ResizeButtonImage(btn_Status, ratio);


            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }
        private void AdjustButton(Button btn, float initialFontSize, Point initialLocation, Size initialSize, float ratio)
        {
            btn.Font = new Font(btn.Font.FontFamily, initialFontSize * ratio*2, btn.Font.Style);
            btn.Width = (int)(initialSize.Width * ratio);
            btn.Height = (int)(initialSize.Height * ratio);
            btn.Location = new Point((int)(initialLocation.X * ratio), (int)(initialLocation.Y * ratio));
        }
        private void Label_Resize(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            AdjustLabelFontSize(lbl);
        }
        private void AdjustLabelFontSize(Label lbl)
        {
            if (originalLabelFontSizes.ContainsKey(lbl))
            {
                float newFontSize = Math.Max(8, originalLabelFontSizes[lbl] * (lbl.Height / lbl.PreferredHeight)); 
                lbl.Font = new Font(lbl.Font.FontFamily, newFontSize, lbl.Font.Style);
            }
        }
        /// <summary>
        /// Label/Image Size 변경에 대한 로직.
        /// </summary>
        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = initialFontSize * ratio * 2;
            float maxFontSize = newFontSize * 3;
            //newFontSize = Math.Max(8, Math.Min(maxFontSize, newFontSize));
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
        private void AdjustPictureBox(System.Windows.Forms.PictureBox pictureBox, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                pictureBox.Width = (int)(initialSize.Width * widthRatio);
                pictureBox.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                pictureBox.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        /// <summary>
        /// 버튼 숨기기
        /// </summary>
        public void SetButtonVisibility(string name, bool visible)
        {
            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (ctrl is Button button && button.Name == name)
                {
                    button.Visible = visible;
                }
            }
        }
        public void IntButtonVisibility()
        {
            // 보이고 싶은 버튼들 설정
            //SetButtonVisibility("btn_Motion", true); // 보이기
            //SetButtonVisibility("btn_Setting", true); // 보이기
            SetButtonVisibility("btn_Teaching", false); // 숨기기
        }
        /// <summary>
        /// ID 교체
        /// </summary>
        public void SetLabelText(string text)
        {
            lblMaker.Text = text;
        }
        /// <summary>
        /// Nav Bar Pop-Up 함수
        /// </summary>
        private void SetPanelNavPosition(Button btn)
        {
            PanelNav.Height = btn.Height;
            PanelNav.Top = btn.Top;
            tableLayoutPanel1.SetCellPosition(PanelNav, new TableLayoutPanelCellPosition(0, tableLayoutPanel1.GetRow(btn)));
            PanelNav.BringToFront();
            btn.BackColor = Color.FromArgb(46, 51, 73);
        }
        /// <summary>
        /// 실제 조작부
        /// </summary>
        private void btn_Main_Click_1(object sender, EventArgs e)
        {
            if (!btn_MainClick)
            {
                Button btn = sender as Button;
 
                ResetButtonStates();
                SetPanelNavPosition(btn);

                ELogSender(sender, enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"{btn.Text}Button Click");
                btn_Main.BackColor = Color.FromArgb(46, 51, 73);
                PanelMain?.Invoke();
                btn_MainClick = true;
            }
        }
        private void btn_Status_Click_1(object sender, EventArgs e)
        {
            if (!btn_StatusClick)
            {
                Subreset_Status?.Invoke();
                Button btn = sender as Button;

                ResetButtonStates();
                SetPanelNavPosition(btn);
               
                ELogSender(sender, enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_Status.BackColor = Color.FromArgb(46, 51, 73);
                PanelStatus?.Invoke();
                btn_StatusClick = true;
            }
        }
        private void btn_Exit_Click_1(object sender, EventArgs e)
        {
            if (!btn_ExitClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();
                SetPanelNavPosition(btn);

                ELogSender(sender, enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_Exit.BackColor = Color.FromArgb(46, 51, 73);
                btn_ExitClick = true;
                
                Application.Exit();
            }
        }
        private void btn_Motion_Click_1(object sender, EventArgs e)
        {
            if (!btn_MotionClick)
            {
                Subreset_Motion?.Invoke();
                Button btn = sender as Button;

                ResetButtonStates();
                SetPanelNavPosition(btn);

                ELogSender(sender, enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_Motion.BackColor = Color.FromArgb(46, 51, 73);
                PanelMotion?.Invoke();
                btn_MotionClick = true;
            }
        }
        private void btn_Alarm_Click_1(object sender, EventArgs e)
        {
            if (!btn_AlarmClick)
            {
                Button btn = sender as Button;

                ResetButtonStates();
                SetPanelNavPosition(btn);

                ELogSender(sender, enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_Alarm.BackColor = Color.FromArgb(46, 51, 73);
                PanelAlarm?.Invoke();
                btn_AlarmClick = true;
            }
        }
        private void btn_Setting_Click_1(object sender, EventArgs e)
        {
            if (!btn_SettingClick)
            {
                Subreset_Setting?.Invoke();
                Button btn = sender as Button;

                SetPanelNavPosition(btn);
                ResetButtonStates();

                ELogSender(sender, enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                btn_Setting.BackColor = Color.FromArgb(46, 51, 73);
                PanelSetting?.Invoke();
                btn_SettingClick = true;
            }
        }
        private void btn_Teaching_Click_1(object sender, EventArgs e)
        {
            if (!btn_TeachingClick)
            {
                Button btn = sender as Button;

                SetPanelNavPosition(btn);
                ResetButtonStates();

                ELogSender(sender, enLogType.Application, enLogLevel.Normal, enLogTitle.ButtonClick, $"{btn.Text} Button Click");
                //btn_Teaching.BackColor = Color.FromArgb(46, 51, 73);
                btn_TeachingClick = true;
            }
        }
        private void btn_Main_Leave_1(object sender, EventArgs e)
        {
            btn_Main.BackColor = Color.FromArgb(23, 30, 54);
        }
        private void btn_Status_Leave_1(object sender, EventArgs e)
        {
            btn_Status.BackColor = Color.FromArgb(23, 30, 54);
        }
        private void btn_Motion_Leave_1(object sender, EventArgs e)
        {
            btn_Motion.BackColor = Color.FromArgb(23, 30, 54);
        }
        private void btn_Alarm_Leave_1(object sender, EventArgs e)
        {
            btn_Alarm.BackColor = Color.FromArgb(23, 30, 54);
        }
        private void btn_Setting_Leave_1(object sender, EventArgs e)
        {
            btn_Setting.BackColor = Color.FromArgb(23, 30, 54);
        }
        //private void btn_Teaching_Leave_1(object sender, EventArgs e)
        //{
        //    btn_Teaching.BackColor = Color.FromArgb(23, 30, 54);
        //}
        private void btn_Exit_Leave_1(object sender, EventArgs e)
        {
            btn_Exit.BackColor = Color.FromArgb(23, 30, 54);
        }

        private void PictureLogin_MouseDown(object sender, MouseEventArgs e)
        {
            delMenuBtnVisible buttonVisible = new delMenuBtnVisible(SetButtonVisibility);
            delLoginlblIdChanged lblIdChange = new delLoginlblIdChanged(SetLabelText);
           
            Form login = new Login(buttonVisible, lblIdChange);
            login.ShowDialog();
        }
    }
}
