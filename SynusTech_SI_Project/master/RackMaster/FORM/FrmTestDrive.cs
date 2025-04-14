using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ;
using RackMaster.SEQ.PART;
using RackMaster.SEQ.COMMON;

namespace RackMaster.FORM {
    public partial class FrmTestDrive : Form {
        private enum ManualStep {
            Idle = 0,

            From_XZT_Going,
            From_Fork_FWD,
            From_Z_Up,
            From_Fork_BWD,

            To_XZT_Going,
            To_Fork_FWD,
            To_Z_Down,
            To_Fork_BWD,
        }

        private RackMaster.SEQ.CLS.Timer m_fromIdErrorblinkingTimer;
        private RackMaster.SEQ.CLS.Timer m_toIdErrorBlinkingTimer;
        private RackMaster.SEQ.CLS.Timer m_stepErrorBlinkingTimer;
        private int m_blinkingTimeOut = 500;
        private bool m_fromIdErrorBlinking = false;
        private bool m_toIdErrorBlinking = false;
        private bool m_stepErrorBlinking = false;

        private ManualStep m_step;

        private int m_fromId = 0;
        private int m_toId = 0;

        public FrmTestDrive() {
            InitializeComponent();

            m_fromIdErrorblinkingTimer = new SEQ.CLS.Timer();
            m_toIdErrorBlinkingTimer = new SEQ.CLS.Timer();
            m_stepErrorBlinkingTimer = new SEQ.CLS.Timer();

            m_step = ManualStep.Idle;

            btnFromXZTGoing.Click += SetMode;
            btnFromForkFWD.Click += SetMode;
            btnFromZUp.Click += SetMode;
            btnFromForkBWD.Click += SetMode;
            btnToXZTGoing.Click += SetMode;
            btnToForkFWD.Click += SetMode;
            btnToZDown.Click += SetMode;
            btnToForkBWD.Click += SetMode;

            btnFromRun.Enabled = false;
            btnToRun.Enabled = false;

            btnFromRun.MouseDown += RunButtonMouseDown;
            btnToRun.MouseDown += RunButtonMouseDown;

            btnFromRun.MouseUp += Stop;
            btnToRun.MouseUp += Stop;
        }

        public void UpdateFormUI() {
            ManualRun();

            txtCurrentStep.Text = RackMasterSEQ.GetCurrentStep().ToString();
            if(RackMasterSEQ.GetCurrentStep() == RackMasterSEQ.RM_STEP.Error) {
                BlinkStepError();
            }
            else {
                txtCurrentStep.BackColor = Color.White;
            }

            if (Global.CST_ON) {
                txtCSTOn.Text = "On";
                txtCSTOn.BackColor = Color.LightGreen;
            }
            else {
                txtCSTOn.Text = "Off";
                txtCSTOn.BackColor = Color.White;
            }

            if(!int.TryParse(txtFromID.Text, out m_fromId)) {
                BlinkFromIDError();
            }
            else {
                if (!Port.IsExistPort(m_fromId))
                    BlinkFromIDError();
                else
                    lblFromIDError.BackColor = Color.AliceBlue;
            }

            if(!int.TryParse(txtToID.Text, out m_toId)) {
                BlinkToIDError();
            }
            else {
                if (!Port.IsExistPort(m_toId))
                    BlinkToIDError();
                else
                    lblToIDError.BackColor = Color.AliceBlue;
            }
        }

        private void ManualRun() {
            switch (m_step) {
                case ManualStep.Idle:
                    break;

                case ManualStep.From_XZT_Going:
                    break;

                case ManualStep.From_Fork_FWD:
                    break;

                case ManualStep.From_Z_Up:
                    break;

                case ManualStep.From_Fork_BWD:
                    break;

                case ManualStep.To_XZT_Going:
                    break;

                case ManualStep.To_Fork_FWD:
                    break;

                case ManualStep.To_Z_Down:
                    break;

                case ManualStep.To_Fork_BWD:
                    break;
            }
        }

        private void BlinkFromIDError() {
            if (m_fromIdErrorblinkingTimer.Delay(m_blinkingTimeOut)) {
                if (m_fromIdErrorBlinking) {
                    lblFromIDError.BackColor = Color.LightGreen;
                    m_fromIdErrorBlinking = false;
                    m_fromIdErrorblinkingTimer.Restart();
                }
                else {
                    lblFromIDError.BackColor = Color.OrangeRed;
                    m_fromIdErrorBlinking = true;
                    m_fromIdErrorblinkingTimer.Restart();
                }
            }
        }

        private void BlinkToIDError() {
            if (m_toIdErrorBlinkingTimer.Delay(m_blinkingTimeOut)) {
                if (m_toIdErrorBlinking) {
                    lblToIDError.BackColor = Color.LightGreen;
                    m_toIdErrorBlinking = false;
                    m_toIdErrorBlinkingTimer.Restart();
                }
                else {
                    lblToIDError.BackColor = Color.OrangeRed;
                    m_toIdErrorBlinking = true;
                    m_toIdErrorBlinkingTimer.Restart();
                }
            }
        }

        private void BlinkStepError() {
            if (m_stepErrorBlinkingTimer.Delay(m_blinkingTimeOut)) {
                if (m_stepErrorBlinking) {
                    txtCurrentStep.BackColor = Color.LightGreen;
                    m_stepErrorBlinking = false;
                    m_stepErrorBlinkingTimer.Restart();
                }
                else {
                    txtCurrentStep.BackColor = Color.OrangeRed;
                    m_stepErrorBlinking = false;
                    m_stepErrorBlinkingTimer.Restart();
                }
            }
        }

        public void SetMode(object sender, EventArgs e) {
            ButtonColorClear();
            btnToRun.Enabled = true;
            btnFromRun.Enabled = true;

            Button btn = sender as Button;

            if(btn == btnFromXZTGoing) {
                btnFromXZTGoing.BackColor = Color.LightGreen;
                btnToRun.Enabled = false;
            }
            else if(btn == btnFromForkFWD) {
                btnFromForkFWD.BackColor = Color.LightGreen;
                btnToRun.Enabled = false;
            }
            else if(btn == btnFromZUp) {
                btnFromZUp.BackColor = Color.LightGreen;
                btnToRun.Enabled = false;
            }
            else if(btn == btnFromForkBWD) {
                btnFromForkBWD.BackColor = Color.LightGreen;
                btnToRun.Enabled = false;
            }else if(btn == btnToXZTGoing) {
                btnToXZTGoing.BackColor = Color.LightGreen;
                btnFromRun.Enabled = false;
            }else if(btn == btnToForkFWD) {
                btnToForkFWD.BackColor = Color.LightGreen;
                btnFromRun.Enabled = false;
            }else if(btn == btnToZDown) {
                btnToZDown.BackColor = Color.LightGreen;
                btnFromRun.Enabled = false;
            }else if(btn == btnToForkBWD) {
                btnToForkBWD.BackColor = Color.LightGreen;
                btnFromRun.Enabled = false;
            }
        }

        private void ButtonColorClear() {
            btnFromXZTGoing.BackColor = Color.AliceBlue;
            btnFromForkFWD.BackColor = Color.AliceBlue;
            btnFromZUp.BackColor = Color.AliceBlue;
            btnFromForkBWD.BackColor = Color.AliceBlue;

            btnToXZTGoing.BackColor = Color.AliceBlue;
            btnToForkFWD.BackColor = Color.AliceBlue;
            btnToZDown.BackColor = Color.AliceBlue;
            btnToForkBWD.BackColor = Color.AliceBlue;
        }

        private void btnErrorReset_Click(object sender, EventArgs e) {
            RackMasterSEQ.SetStep((int)RackMasterSEQ.RM_STEP.Idle);
        }

        private void RunButtonMouseDown(object sender, MouseEventArgs e) {
            Button btn = sender as Button;

            if(btn == btnFromRun) {
                int id = 0;
                if(!int.TryParse(txtFromID.Text, out id)) {
                    MessageBox.Show("올바른 ID를 입력하세요");
                    return;
                }

                btnFromRun.BackColor = Color.LightGreen;
                RackMasterSEQ.SetFromID(id);
                RackMasterSEQ.SetStep((int)RackMasterSEQ.RM_STEP.From_ID_Check);
            }else if(btn == btnToRun) {
                int id = 0;
                if (!int.TryParse(txtToID.Text, out id)) {
                    MessageBox.Show("올바른 ID를 입력하세요");
                    return;
                }

                btnToRun.BackColor = Color.LightGreen;
                RackMasterSEQ.SetToID(id);
                RackMasterSEQ.SetStep((int)RackMasterSEQ.RM_STEP.To_ID_Check);
            }
        }

        private void Stop(object sender, MouseEventArgs e) {
            RackMasterSEQ.SetStep((int)RackMasterSEQ.RM_STEP.Stop);
        }
    }
}
