using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.PART;
using RackMaster.SUBFORM.StatusPage;

namespace RackMaster.SUBFORM {
    public partial class FrmStatus : Form {
        private enum DgvIOColumn {
            Name,
            Status
        }

        private enum DgvStateColumn {
            Name,
            State,
        }

        private enum DgvMasterPIORow {
            L_Request,
            UL_Request,
            Ready,
            PortError
        }

        private enum DgvRackMasterPIORow {
            TR_Req,
            Busy,
            Complete,
            STKError,
        }

        public enum DgvRegulatorRow {
            STX = 0,
            Status,
            BoostVoltage,
            OutputVoltage,
            BoostCurrent,
            OutputCurrent,
            HitSinkTemperature,
            InsideNTC,
            PickupNTC,
            ErrorCode,
            CheckSum,
            End,
        }

        public enum DgvBarcodeRow {
            PositionValue,
            VelocityValue,
            HardwareError,
            LaserStatus,
            Intensity,
            Temperature,
            Laser,
            Plausibility,
            VelocityMeasurementError,
        }

        private FrmLogin frmLogin;
        private StatusPage_IO page_Io;
        private StatusPage_FullClosed page_FullClosed;
        private StatusPage_Axis page_Axis;
        private StatusPage_Motion page_Motion;

        private UICtrl.ButtonCtrl m_btnCtrl;
        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterParam m_param;

        public FrmStatus(RackMasterMain rackMaster) {
            InitializeComponent();

            m_rackMaster = rackMaster;
            m_param = m_rackMaster.m_param;
            m_btnCtrl = new UICtrl.ButtonCtrl();

            page_Io = new StatusPage_IO(m_rackMaster);
            page_FullClosed = new StatusPage_FullClosed(m_rackMaster);
            page_Axis = new StatusPage_Axis(m_rackMaster);
            page_Motion = new StatusPage_Motion(m_rackMaster);

            page_Io.TopLevel            = false;
            page_FullClosed.TopLevel    = false;
            page_Axis.TopLevel          = false;
            page_Motion.TopLevel        = false;

            pnlCenter.Controls.Add(page_Io);
            pnlCenter.Controls.Add(page_FullClosed);
            pnlCenter.Controls.Add(page_Axis);
            pnlCenter.Controls.Add(page_Motion);

            HideAllPage();
            m_btnCtrl.SetOnOffButtonStyle(ref btnIOPage, true);
            page_Io.Visible = true;

            btnIOPage.Click                 += PageButtonClickEvent;
            btnFullClosedPage.Click         += PageButtonClickEvent;
            btnAxisPage.Click               += PageButtonClickEvent;
            btnMotionPage.Click             += PageButtonClickEvent;

            this.VisibleChanged             += ThisFormVisibleChangedEvent;
        }

        private void ThisFormVisibleChangedEvent(object sender, EventArgs e) {
            page_Io.SetModifyMode(false);

            foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                m_rackMaster.SetOutputBit(output, false);
            }
        }

        private void HideAllPage() {
            m_btnCtrl.SetOnOffButtonStyle(ref btnIOPage, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnFullClosedPage, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnAxisPage, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnMotionPage, false);

            page_Io.Visible         = false;
            page_FullClosed.Visible = false;
            page_Axis.Visible       = false;
            page_Motion.Visible     = false;
        }

        public void UpdateFormUI() {
            if (m_param.GetMotionParam().useFullClosed) {
                page_FullClosed.UpdateFormUI();
                if (!btnFullClosedPage.Visible) {
                    btnFullClosedPage.Visible = true;
                }
            }
            else {
                if (btnFullClosedPage.Visible) {
                    btnFullClosedPage.Visible = false;
                }
            }

            m_btnCtrl.SetOnOffButtonStyle(ref btnOutputMode, page_Io.GetModifyMode());

            page_Io.UpdateFormUI();
            page_Axis.UpdateFormUI();
            page_Motion.UpdateFormUI();
        }

        private void PageButtonClickEvent(object sender, EventArgs e) {
            Button btn = sender as Button;

            if(btn == btnIOPage) {
                HideAllPage();
                page_Io.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btnIOPage, true);
            }else if(btn == btnFullClosedPage) {
                HideAllPage();
                page_FullClosed.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btnFullClosedPage, true);
            }else if(btn == btnAxisPage) {
                HideAllPage();
                page_Axis.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisPage, true);
            }else if(btn == btnMotionPage) {
                HideAllPage();
                page_Motion.Visible = true;
                m_btnCtrl.SetOnOffButtonStyle(ref btnMotionPage, true);
            }
        }

        private void btnModifyMode_Click(object sender, EventArgs e) {
            if (page_Io.GetModifyMode()) {
                page_Io.SetModifyMode(false);
            }
            else {
                frmLogin = new FrmLogin(false);
                frmLogin.loginEvent += AccessModifyMode;
                frmLogin.ShowDialog();
            }
        }

        private void AccessModifyMode(Utility.PasswordType type) {
            page_Io.SetModifyMode(true);
        }

        public void UpdateAmpTemperature(AxisList axis, int temp) {
            page_Axis.UpdateAmpTemperature(axis, temp);
        }
    }
}
