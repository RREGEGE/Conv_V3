using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.PART;
using MovenCore;

namespace RackMaster.SUBFORM {
    public partial class FrmMotor : Form {
        private Axis m_axis;
        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterMotion m_motion;
        private RackMasterMain.RackMasterParam m_param;
        private UICtrl.LabelCtrl m_lblCtrl;
        private UICtrl.ButtonCtrl m_btnCtrl;
        private AxisList m_curAxis = AxisList.X_Axis;

        private FrmLoading frmLoading;

        private double m_actPos = 0;
        private double m_cmdPos = 0;
        private double m_actVel = 0;
        private double m_cmdVel = 0;
        private double m_actTrq = 0;
        private int m_alarmCode = 0;

        private bool m_ignoreFork   = false;
        private bool m_isHoming     = false;
        private bool m_homingMode   = false;

        public FrmMotor(RackMasterMain rackMaster) {
            InitializeComponent();

            m_axis = Axis.Instance;
            m_rackMaster = rackMaster;
            m_motion = m_rackMaster.m_motion;
            m_param = m_rackMaster.m_param;

            m_lblCtrl = new UICtrl.LabelCtrl();
            m_btnCtrl = new UICtrl.ButtonCtrl();

            btnJogNegHigh.MouseDown     += JogControl;
            btnJogNegLow.MouseDown      += JogControl;
            btnJogPosHigh.MouseDown     += JogControl;
            btnJogPosLow.MouseDown      += JogControl;

            btnJogNegHigh.MouseUp       += JogStop;
            btnJogNegLow.MouseUp        += JogStop;
            btnJogPosHigh.MouseUp       += JogStop;
            btnJogPosLow.MouseUp        += JogStop;

            btnInchNeg.MouseDown        += InchingControl;
            btnInchPos.MouseDown        += InchingControl;

            btnInchNeg.MouseUp          += InchingStop;
            btnInchPos.MouseUp          += InchingStop;

            btnJogNegHigh.MouseLeave    += JogButtonMouseLeaveEvent;
            btnJogNegLow.MouseLeave     += JogButtonMouseLeaveEvent;
            btnJogPosHigh.MouseLeave    += JogButtonMouseLeaveEvent;
            btnJogPosLow.MouseLeave     += JogButtonMouseLeaveEvent;

            btnInchNeg.MouseLeave       += InchingButtonMouseLeaveEvent;
            btnInchPos.MouseLeave       += InchingButtonMouseLeaveEvent;

            btnAxisX.Click              += SetAxisNumber;
            btnAxisZ.Click              += SetAxisNumber;
            btnAxisA.Click              += SetAxisNumber;
            btnAxisT.Click              += SetAxisNumber;

            LanguageChanged();
        }

        public void UpdateFormUI() {
            if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                if (btnAxisT.Enabled) {
                    m_btnCtrl.EnabledButton(ref btnAxisT, false);
                }
            }
            else {
                if (!btnAxisT.Enabled) {
                    m_btnCtrl.EnabledButton(ref btnAxisT, true);
                }
            }

            if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum && m_curAxis == AxisList.Z_Axis) {
                m_actPos = m_motion.GetDrumBeltZAxisActualPosition() / 1000;
                m_cmdPos = m_motion.GetDrumBeltZAxisCommandPosition() / 1000;
                m_actVel = m_motion.GetDrumBeltZAxisActualVelocity() / 1000000 * 60;
                m_cmdVel = m_motion.GetDrumBeltZAxisCommandVelocity() / 1000000 * 60;
            }
            else {
                m_actPos = m_motion.GetAxisStatus(AxisStatusType.pos_act, m_curAxis) / 1000;
                m_cmdPos = m_motion.GetAxisStatus(AxisStatusType.pos_cmd, m_curAxis) / 1000;
                if (m_param.GetMotionParam().forkType == ForkType.SCARA && (m_curAxis == AxisList.A_Axis || m_curAxis == AxisList.T_Axis)) {
                    m_actVel = m_motion.GetAxisStatus(AxisStatusType.vel_act, m_curAxis) / 1000 * 60;
                    m_cmdVel = m_motion.GetAxisStatus(AxisStatusType.vel_cmd, m_curAxis) / 1000 * 60;
                }
                else {
                    m_actVel = m_motion.GetAxisStatus(AxisStatusType.vel_act, m_curAxis) / 1000000 * 60;
                    m_cmdVel = m_motion.GetAxisStatus(AxisStatusType.vel_cmd, m_curAxis) / 1000000 * 60;
                }
            }
            m_actTrq = m_motion.GetAxisStatus(AxisStatusType.trq_act, m_curAxis);
            m_alarmCode = m_motion.GetAxisAlarmCode(m_curAxis);

            txtActPos.Text = $"{m_actPos:F3}";
            txtCmdPos.Text = $"{m_cmdPos:F3}";
            txtActVel.Text = $"{m_actVel:F3}";
            txtCmdVel.Text = $"{m_cmdVel:F3}";
            txtActTrq.Text = $"{m_actTrq:F3}";
            txtAlarmCode.Text = $"{m_alarmCode}";

            m_lblCtrl.SetOnOffLabelStyle(ref lblNLS, m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, m_curAxis));
            m_lblCtrl.SetOnOffLabelStyle(ref lblPLS, m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, m_curAxis));
            m_lblCtrl.SetOnOffLabelStyle(ref lblORG, m_motion.GetAxisSensor(AxisSensorType.Home, m_curAxis));
            m_lblCtrl.SetOnOffLabelStyle(ref lblHomeDone, m_motion.GetAxisFlag(AxisFlagType.HomeDone, m_curAxis));

            if (m_param.GetAxisParameter(m_curAxis).posSensorEnabled) {
                m_lblCtrl.SetOnOffLabelStyle(ref lblPosSensor, m_motion.GetPosSensor(m_curAxis));
            }
            else {
                m_lblCtrl.SetEnableLabelStyle(ref lblPosSensor, false);
            }
            if (m_param.GetAxisParameter(m_curAxis).posSensor2Enabled) {
                m_lblCtrl.SetOnOffLabelStyle(ref lblPosSensor2, m_motion.GetPosSensor2(m_curAxis));
            }
            else {
                m_lblCtrl.SetEnableLabelStyle(ref lblPosSensor2, false);
            }

            m_lblCtrl.SetOnOffLabelStyle(ref lblPickLeft, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Left));
            m_lblCtrl.SetOnOffLabelStyle(ref lblPickRight, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Right));
            m_lblCtrl.SetOnOffLabelStyle(ref lblPlaceLeft, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Left));
            m_lblCtrl.SetOnOffLabelStyle(ref lblPlaceRight, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Right));

            m_lblCtrl.SetErrorLabelStyle(ref lblAlarm, m_motion.GetAxisFlag(AxisFlagType.Alarm, m_curAxis));
            m_lblCtrl.BlinkingLabel(ref lblAlarm, m_motion.GetAxisFlag(AxisFlagType.Alarm, m_curAxis));
            m_btnCtrl.BlinkingButton(ref btnAlarmClear, m_motion.GetAxisFlag(AxisFlagType.Alarm, m_curAxis));

            m_btnCtrl.SetOnOffButtonStyle(ref btnServoOn, m_motion.GetAxisFlag(AxisFlagType.Servo_On, m_curAxis));
            m_btnCtrl.SetOnOffButtonStyle(ref btnIgnoreFork, m_ignoreFork);

            if (m_rackMaster.m_alarm.IsAlarmState()) {
                if(m_rackMaster.m_alarm.IsCurrentAlarmContainAt(AlarmList.GOT_EStop) || m_rackMaster.m_alarm.IsCurrentAlarmContainAt(AlarmList.HP_EStop) ||
                    m_rackMaster.m_alarm.IsCurrentAlarmContainAt(AlarmList.HP_Key_EStop) || m_rackMaster.m_alarm.IsCurrentAlarmContainAt(AlarmList.MST_HP_EStop) ||
                    m_rackMaster.m_alarm.IsCurrentAlarmContainAt(AlarmList.MST_OP_EStop) || m_rackMaster.m_alarm.IsCurrentAlarmContainAt(AlarmList.OP_EStop) ||
                    m_rackMaster.m_alarm.IsCurrentAlarmContainAt(AlarmList.OP_Key_EStop)) {
                    ControlButtonEnable(false);
                }
                else {
                    DTP_Control();
                }
            }
            else {
                DTP_Control();
            }
        }

        private void DTP_Control() {
            if (m_rackMaster.IsAutoState())
                return;

            if (!m_rackMaster.IsUseGOT()) {
                ControlButtonEnable(true);
                return;
            }

            if (m_rackMaster.GetInputBit(InputList.HP_DTP_Mode_Select_SW_1) || m_rackMaster.GetInputBit(InputList.HP_DTP_Mode_Select_SW_2)) {
                if (m_rackMaster.GetInputBit(InputList.HP_DTP_DeadMan_SW)) {
                    ControlButtonEnable(true);
                }
                else {
                    ControlButtonEnable(false);
                    if (m_motion.GetAxisFlag(AxisFlagType.Joging, m_curAxis)) {
                        DeadManStopControl();
                    }
                }
            }
            else if (m_rackMaster.GetInputBit(InputList.OP_DTP_Mode_Select_SW_1) || m_rackMaster.GetInputBit(InputList.OP_DTP_Mode_Select_SW_2)) {
                if (m_rackMaster.GetInputBit(InputList.OP_DTP_DeadMan_SW)) {
                    ControlButtonEnable(true);
                }
                else {
                    ControlButtonEnable(false);
                    if (m_motion.GetAxisFlag(AxisFlagType.Joging, m_curAxis)) {
                        DeadManStopControl();
                    }
                }
            }
            else {
                ControlButtonEnable(false);
            }
        }

        private void ControlButtonEnable(bool enable) {
            m_btnCtrl.EnabledButton(ref btnJogNegHigh, enable);
            m_btnCtrl.EnabledButton(ref btnJogNegLow, enable);
            m_btnCtrl.EnabledButton(ref btnJogPosHigh, enable);
            m_btnCtrl.EnabledButton(ref btnJogPosLow, enable);
            m_btnCtrl.EnabledButton(ref btnInchNeg, enable);
            m_btnCtrl.EnabledButton(ref btnInchPos, enable);
            if (enable) {
                if (m_homingMode) {
                    m_btnCtrl.SetOnOffButtonStyle(ref btnHoming, enable);
                }
                else {
                    m_btnCtrl.EnabledButton(ref btnHoming, enable);
                }
            }
            else {
                m_btnCtrl.EnabledButton(ref btnHoming, enable);
            }
        }

        private void DeadManStopControl() {
            //m_motion.JogStop(m_curAxis);
            //m_motion.InchingStop(m_curAxis);
            //m_motion.HomeStop(m_curAxis);

            if (m_motion.GetAxisFlag(AxisFlagType.HomeDone, m_curAxis) && m_isHoming) {
                m_param.SaveAbsoluteHomeDone(m_curAxis);
                m_isHoming = false;
            }

            if (m_motion.GetAxisFlag(AxisFlagType.Joging, m_curAxis))
                m_motion.JogStop(m_curAxis);
            else if (m_motion.GetAxisFlag(AxisFlagType.Homing, m_curAxis))
                m_motion.HomeStop(m_curAxis);
            else
                m_motion.InchingStop(m_curAxis);

            m_ignoreFork = false;
        }

        private void btnServoOn_Click(object sender, EventArgs e) {
            if (m_motion.GetAxisFlag(AxisFlagType.Servo_On, m_curAxis)) {
                m_motion.ServoOff(m_curAxis);
            }
            else {
                m_motion.ServoOn(m_curAxis);
            }
        }

        private void SetAxisNumber(object sender, EventArgs e) {
            Button btn = sender as Button;

            if(btn == btnAxisT) {
                lblCmdPos.Text = "deg";
                lblActPos.Text = "deg";
                lblCmdVel.Text = "deg/min";
                lblActVel.Text = "deg/min";
                lblJogLowSpd.Text = "LOW SPD(deg/min)";
                lblJogHighSpd.Text = "HIGH SPD(deg/min)";
                lblInch.Text = "INCHING(deg)";
            }else if(btn == btnAxisX || btn == btnAxisZ) {
                lblCmdPos.Text = "mm";
                lblActPos.Text = "mm";
                lblCmdVel.Text = "m/min";
                lblActVel.Text = "m/min";
                lblJogLowSpd.Text = "LOW SPD(m/min)";
                lblJogHighSpd.Text = "HIGH SPD(m/min)";
                lblInch.Text = "INCHING(mm)";
            }else if(btn == btnAxisA) {
                if(m_param.GetMotionParam().forkType == ForkType.SCARA) {
                    lblCmdPos.Text = "deg";
                    lblActPos.Text = "deg";
                    lblCmdVel.Text = "deg/min";
                    lblActVel.Text = "deg/min";
                    lblJogLowSpd.Text = "LOW SPD(deg/min)";
                    lblJogHighSpd.Text = "HIGH SPD(deg/min)";
                    lblInch.Text = "INCHING(deg)";
                }
                else {
                    lblCmdPos.Text = "mm";
                    lblActPos.Text = "mm";
                    lblCmdVel.Text = "m/min";
                    lblActVel.Text = "m/min";
                    lblJogLowSpd.Text = "LOW SPD(m/min)";
                    lblJogHighSpd.Text = "HIGH SPD(m/min)";
                    lblInch.Text = "INCHING(mm)";
                }
            }

            if(btn == btnAxisX) {
                m_curAxis = AxisList.X_Axis;
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisX, true);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisZ, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisA, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisT, false);
                txtHighSpeed.Text = "10";
                txtLowSpeed.Text = "1";
            }
            else if(btn == btnAxisZ) {
                m_curAxis = AxisList.Z_Axis;
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisX, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisZ, true);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisA, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisT, false);
                txtHighSpeed.Text = "10";
                txtLowSpeed.Text = "1";
            }
            else if(btn == btnAxisA) {
                m_curAxis = AxisList.A_Axis;
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisX, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisZ, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisA, true);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisT, false);
                if(m_param.GetMotionParam().forkType == ForkType.SCARA) {
                    txtHighSpeed.Text = "500";
                    txtLowSpeed.Text = "100";
                }
                else {
                    txtHighSpeed.Text = "10";
                    txtLowSpeed.Text = "1";
                }
            }
            else if(btn == btnAxisT) {
                m_curAxis = AxisList.T_Axis;
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisX, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisZ, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisA, false);
                m_btnCtrl.SetOnOffButtonStyle(ref btnAxisT, true);
                txtHighSpeed.Text = "500";
                txtLowSpeed.Text = "100";
            }
        }

        private void btnAlarmClear_Click(object sender, EventArgs e) {
            if (m_motion.GetAxisFlag(AxisFlagType.Alarm, m_curAxis)) {
                if (m_motion.IsAbsoluteAlarm(m_curAxis)) {
                    Thread loadingThread = new Thread(new ThreadStart(ShowLoadingScreen));
                    loadingThread.Start();

                    m_motion.AlarmClear(m_curAxis);

                    if (loadingThread != null)
                        frmLoading.Invoke(new Action(frmLoading.Close));
                }
                else {
                    m_motion.AlarmClear(m_curAxis);
                }
            }
        }

        private void JogControl(object sender, MouseEventArgs e) {
            if(!m_motion.GetAxisFlag(AxisFlagType.Servo_On, m_curAxis)) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.NoServoOn}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            if (m_curAxis != AxisList.A_Axis) {
                if (m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis) > (m_param.GetAxisParameter(AxisList.A_Axis).homePositionRange * 1000)) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ForkHomePositionCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }
            }

            if (m_curAxis == AxisList.Z_Axis && !m_rackMaster.Interlock_ZAxisMaintStopper()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ZAxisMaintStopperCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            double jogVelHigh = 0;
            double jogVelLow = 0;

            if(!(double.TryParse(txtHighSpeed.Text, out jogVelHigh)) || !(double.TryParse(txtLowSpeed.Text, out jogVelLow))){
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ValidValueCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }
            Button btn = sender as Button;
            if (m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, m_curAxis)) {
                if (btn == btnJogNegHigh || btn == btnJogNegLow) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.NegativeLimitSensorOn}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }
            }else if(m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, m_curAxis)) {
                if(btn == btnJogPosHigh || btn == btnJogPosLow) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.PositiveLimitSensorOn}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }
            }

            AxisProfile profile = new AxisProfile();

            profile.m_axis = m_param.GetAxisNumber(m_curAxis);
            profile.m_profileType = MovenCore.WMXParam.m_profileType.JerkRatio;
            if(btn == btnJogNegHigh || btn == btnJogPosHigh) {
                jogVelHigh = (jogVelHigh * 1000000) / 60;

                profile.m_acc = jogVelHigh;
                profile.m_dec = jogVelHigh;

                if(btn == btnJogNegHigh) {
                    jogVelHigh *= (-1);
                }

                profile.m_velocity = jogVelHigh;

                if(m_param.GetMotionParam().forkType == ForkType.SCARA && m_curAxis == AxisList.A_Axis) {
                    AxisProfile[] profiles = new AxisProfile[2];
                    profiles[0] = new AxisProfile();
                    profiles[1] = new AxisProfile();

                    profiles[0].m_axis = m_param.GetAxisNumber(AxisList.A_Axis);
                    profiles[1].m_axis = m_param.GetAxisNumber(AxisList.T_Axis);

                    profiles[0].m_velocity = profiles[1].m_velocity = profile.m_velocity / 1000;
                    profiles[0].m_acc = profiles[1].m_acc = profile.m_acc;
                    profiles[0].m_dec = profiles[1].m_dec = profile.m_dec;

                    m_axis.Jog(profiles.Length, profiles);
                }
                else if(m_curAxis == AxisList.T_Axis) {
                    profile.m_velocity /= 1000;

                    if(btn == btnJogNegHigh)
                    {
                        profile.m_acc = profile.m_dec = profile.m_velocity * (-1);
                    }
                    else
                    {
                        profile.m_acc = profile.m_dec = profile.m_velocity;
                    }
                    m_axis.Jog(profile);
                }
                else if(m_curAxis == AxisList.Z_Axis && m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                    double calcVel = RackMasterMain.RackMasterMotion.DistanceToRadian(m_motion.GetCurrentZAxisDia(), jogVelHigh);

                    profile.m_velocity = calcVel;
                    if(btn == btnJogNegHigh) {
                        profile.m_acc = profile.m_dec = profile.m_velocity * (-1);
                    }
                    else {
                        profile.m_acc = profile.m_dec = profile.m_velocity;
                    }

                    m_axis.Jog(profile);
                }
                else {
                    m_axis.Jog(profile);
                }
            }else if(btn == btnJogNegLow || btn == btnJogPosLow) {
                jogVelLow = (jogVelLow * 1000000) / 60;

                profile.m_acc = jogVelLow;
                profile.m_dec = jogVelLow;

                if(btn == btnJogNegLow) {
                    jogVelLow *= (-1);
                }

                profile.m_velocity = jogVelLow;

                if (m_param.GetMotionParam().forkType == ForkType.SCARA && m_curAxis == AxisList.A_Axis) {
                    AxisProfile[] profiles = new AxisProfile[2];
                    profiles[0] = new AxisProfile();
                    profiles[1] = new AxisProfile();

                    profiles[0].m_axis = m_param.GetAxisNumber(AxisList.A_Axis);
                    profiles[1].m_axis = m_param.GetAxisNumber(AxisList.T_Axis);

                    profiles[0].m_velocity = profiles[1].m_velocity = profile.m_velocity / 1000;
                    profiles[0].m_acc = profiles[1].m_acc = profile.m_acc;
                    profiles[0].m_dec = profiles[1].m_dec = profile.m_dec;

                    m_axis.Jog(profiles.Length, profiles);
                }
                else if (m_curAxis == AxisList.T_Axis) {
                    profile.m_velocity /= 1000;

                    if(btn == btnJogNegLow)
                    {
                        profile.m_acc = profile.m_dec = profile.m_velocity * (-1);
                    }
                    else
                    {
                        profile.m_acc = profile.m_dec = profile.m_velocity;
                    }
                    m_axis.Jog(profile);
                }
                else if (m_curAxis == AxisList.Z_Axis && m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                    double calcVel = RackMasterMain.RackMasterMotion.DistanceToRadian(m_motion.GetCurrentZAxisDia(), jogVelLow);

                    profile.m_velocity = calcVel;
                    if (btn == btnJogNegLow) {
                        profile.m_acc = profile.m_dec = profile.m_velocity * (-1);
                    }
                    else {
                        profile.m_acc = profile.m_dec = profile.m_velocity;
                    }

                    m_axis.Jog(profile);
                }
                else {
                    m_axis.Jog(profile);
                }
            }
        }

        private void JogStop(object sender, MouseEventArgs e) {
            foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                m_motion.JogStop(axis);
            }
            m_ignoreFork = false;
        }

        private void InchingControl(object sender, MouseEventArgs e) {
            if(!m_motion.GetAxisFlag(AxisFlagType.Servo_On, m_curAxis)) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.NoServoOn}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            if (!m_ignoreFork) {
                if (m_curAxis != AxisList.A_Axis) {
                    if (m_motion.GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis) > (m_param.GetAxisParameter(AxisList.A_Axis).homePositionRange * 1000)) {
                        MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ForkHomePositionCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                        return;
                    }
                }
            }

            if (m_curAxis == AxisList.Z_Axis && !m_rackMaster.Interlock_ZAxisMaintStopper()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ZAxisMaintStopperCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            double inchingVel = 0;
            double inchingTarget = 0;
            
            if(!(double.TryParse(txtLowSpeed.Text, out inchingVel)) || !(double.TryParse(txtInchDist.Text, out inchingTarget))) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ValidValueCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            Button btn = sender as Button;

            if(m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, m_curAxis)) {
                if(btn == btnInchNeg) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.NegativeLimitSensorOn}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }
            }else if(m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, m_curAxis)) {
                if(btn == btnInchPos) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.PositiveLimitSensorOn}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }
            }

            if(btn == btnInchNeg) {
                inchingTarget *= (-1);
            }

            AxisProfile profile = new AxisProfile();
            profile.m_axis = m_param.GetAxisNumber(m_curAxis);
            profile.m_dest = inchingTarget * 1000;
            inchingVel = (inchingVel * 1000000) / 60;
            profile.m_velocity = inchingVel;
            profile.m_acc = inchingVel;
            profile.m_dec = inchingVel;

            if(m_param.GetMotionParam().forkType == ForkType.SCARA && m_curAxis == AxisList.A_Axis) {
                profile.m_axis = m_param.GetAxisNumber(AxisList.A_Axis);
                profile.m_velocity /= 1000;
                m_axis.RelativeMove(profile);
                profile.m_axis = m_param.GetAxisNumber(AxisList.T_Axis);
                m_axis.RelativeMove(profile);
            }else if(m_curAxis == AxisList.T_Axis) {
                profile.m_velocity /= 1000;
                profile.m_acc = profile.m_velocity;
                profile.m_dec = profile.m_velocity;
                m_axis.RelativeMove(profile);
            }else if(m_curAxis == AxisList.Z_Axis && m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum) {
                profile.m_dest = RackMasterMain.RackMasterMotion.DistanceToRadian(m_motion.GetCurrentZAxisDia(), (inchingTarget * 1000));
                profile.m_velocity = RackMasterMain.RackMasterMotion.DistanceToRadian(m_motion.GetCurrentZAxisDia(), inchingVel);
                profile.m_acc = profile.m_dec = profile.m_velocity;
                m_axis.RelativeMove(profile);
            }
            else {
                m_axis.RelativeMove(profile);
            }
        }

        private void InchingStop(object sender, MouseEventArgs e) {
            foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                m_motion.InchingStop(axis);
            }

            m_ignoreFork = false;
        }

        private void txtLowSpeed_TextChanged(object sender, EventArgs e) {
            double txtSpeed = 0;
            if(!double.TryParse(txtLowSpeed.Text, out txtSpeed)) {
                txtLowSpeed.Text = "";
                return;
            }

            if(txtSpeed > m_param.GetAxisParameter(m_curAxis).jogLowSpeedLimit) {
                txtLowSpeed.Text = $"{m_param.GetAxisParameter(m_curAxis).jogLowSpeedLimit:F0}";
            }
        }

        private void txtHighSpeed_TextChanged(object sender, EventArgs e) {
            double txtSpeed = 0;
            if(!double.TryParse(txtHighSpeed.Text, out txtSpeed)) {
                txtHighSpeed.Text = "";
                return;
            }

            if(txtSpeed > m_param.GetAxisParameter(m_curAxis).jogHighSpeedLimit) {
                txtHighSpeed.Text = $"{m_param.GetAxisParameter(m_curAxis).jogHighSpeedLimit:F0}";
            }
        }

        private void txtInchDist_TextChanged(object sender, EventArgs e) {
            double txtInching = 0;
            if(!double.TryParse(txtInchDist.Text, out txtInching)) {
                txtInchDist.Text = "";
                return;
            }

            if(txtInching > m_param.GetAxisParameter(m_curAxis).inchingLimit) {
                txtInchDist.Text = $"{m_param.GetAxisParameter(m_curAxis).inchingLimit:F0}";
            }
        }

        private void btnHoming_MouseDown(object sender, MouseEventArgs e) {
            if(!m_motion.GetAxisFlag(AxisFlagType.Servo_On, m_curAxis)) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.NoServoOn}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            if (m_motion.GetAxisFlag(AxisFlagType.Run, m_curAxis)) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.MotorIsRunning}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            if(m_curAxis == AxisList.Z_Axis && !m_rackMaster.Interlock_ZAxisMaintStopper()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ZAxisMaintStopperCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            if (!m_homingMode) {
                if(MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureHomingMode}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    m_homingMode = true;
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.HomingModeEnable}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                }
            }
            else {
                m_motion.AxisHomeStart(m_curAxis);
                m_isHoming = true;
            }
        }

        private void btnHoming_MouseUp(object sender, MouseEventArgs e) {
            if(m_motion.GetAxisFlag(AxisFlagType.HomeDone, m_curAxis)) {
                m_param.SaveAbsoluteHomeDone(m_curAxis);
            }

            foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                m_motion.HomeStop(axis);
            }
            m_isHoming      = false;
            m_homingMode    = false;
        }

        private void btnIgnoreFork_Click(object sender, EventArgs e) {
            if (m_ignoreFork) {
                m_ignoreFork = false;
            }
            else {
                m_ignoreFork = true;
            }
        }

        public void LanguageChanged() {
            //gboxAxisControl.Text        = SynusLangPack.GetLanguage(gboxAxisControl.Name);
            //gboxJog.Text                = SynusLangPack.GetLanguage(gboxJog.Name);
            //gboxInching.Text            = SynusLangPack.GetLanguage(gboxInching.Name);
            //gboxMotorStatus.Text        = SynusLangPack.GetLanguage(gboxMotorStatus.Name);
        }

        private void JogButtonMouseLeaveEvent(object sender, EventArgs e) {
            m_motion.JogStop(m_curAxis);
        }

        private void InchingButtonMouseLeaveEvent(object sender, EventArgs e) {
            m_motion.InchingStop(m_curAxis);
        }

        private void ShowLoadingScreen() {
            frmLoading = new FrmLoading(LoadingType.AbsoluteAlarmClearLoading);
            Application.Run(frmLoading);
        }
    }
}
