using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMX3ApiCLR;
using static Synustech.G_Var;
using static Synustech.WMXMotion.AxisParameter;

namespace Synustech
{
    public class WaitCondition
    {
        public int m_axisCount;
        public int[] m_axisArray;
        public WMXParam.m_waitType m_waitType;

        public WaitCondition()
        {
            m_axisCount = 1;
            m_axisArray = new int[Constants.MaxAxes];
            m_waitType = WMXParam.m_waitType.AxisIdle;
        }
    }

    public class TriggerCondition
    {
        public double m_triggerValue;
        public int m_triggerAxis;
        public WMXParam.m_triggerType m_triggerType;

        public TriggerCondition()
        {
            m_triggerValue = 0;
            m_triggerAxis = 0;
            m_triggerType = WMXParam.m_triggerType.RemainingTime;
        }
    }

    public class AxisProfile
    {
        public double m_velocity;
        public double m_endvelocity;
        public double m_acc;
        public double m_dec;
        public double m_jerkRatio;
        public double m_dest;
        public int m_axis;
        public WMXParam.m_profileType m_profileType;

        public AxisProfile()
        {
            m_velocity = 10000;
            m_endvelocity = 0;
            m_acc = 100000;
            m_dec = 100000;
            m_jerkRatio = 0.75;
            m_axis = 0;
            m_profileType = WMXParam.m_profileType.Trapezoidal;
        }
    }

    public class AxesProfile
    {
        public uint m_axisCount;
        public int[] m_axisArray;
        public double m_velocity;
        public double m_acc;
        public double m_dec;
        public double m_jerkRatio;
        public double[] m_dest;
        public double[] m_maxVel;
        public double[] m_maxAcc;
        public double[] m_maxDec;
        public WMXParam.m_profileType m_profileType;

        public AxesProfile()
        {
            m_axisCount = 0;
            m_axisArray = new int[Constants.MaxAxes];
            m_dest = new double[Constants.MaxAxes];
            m_maxVel = new double[Constants.MaxAxes];
            m_maxAcc = new double[Constants.MaxAxes];
            m_maxDec = new double[Constants.MaxAxes];
            m_velocity = 10000;
            m_acc = 100000;
            m_dec = 100000;
            m_jerkRatio = 0.75;
            m_profileType = WMXParam.m_profileType.Trapezoidal;
        }
    }
    public class WMXMotion
    {
        public static int nErr;
        public static uint uiwaitTimeMilisec = 5000;
        public static string strErrString = "";
        private WMX3Api m_wmx3Api;
        private CoreMotion m_coreMotion;
        public AxisStatus[] m_axisStatus;
        public AxisParameter[] m_axisParameter;
        public AxisProfile[] m_AxisProfile;


        public class AxisStatus
        {
            public bool m_servoOffline;
            public bool m_servoOn;
            public bool m_alarm;
            public bool m_servoRun;
            public bool m_nLimit;
            public bool m_pLimit;
            public bool m_nLimitSoft;
            public bool m_pLimitSoft;
            public bool m_origin;
            public bool m_waitingTrigger;
            public bool m_homeDone;
            public bool m_Inpos;
            public bool m_posSet;
            public bool m_homing;
            public bool m_Joging;

            public double m_cmdPos;
            public double m_actualPos;
            public double m_cmdVelocity;
            public double m_actualVelocity;
            public double m_cmdTorque;
            public double m_actualTorque;
            public double m_profileTargetPos;
            public double m_cmdEncoder;
            public double m_actualEncoder;
            public int m_alarmCode;

            public AxisStatus()
            {
                m_servoOffline = true;
                m_servoOn = false;
                m_alarm = false;
                m_servoRun = false;
                m_nLimit = false;
                m_pLimit = false;
                m_nLimitSoft = false;
                m_pLimitSoft = false;
                m_origin = false;
                m_waitingTrigger = false;
                m_homeDone = false;
                m_Inpos = false;
                m_posSet = false;
                m_homing = false;
                m_Joging = false;

                m_cmdPos = 0;
                m_actualPos = 0;
                m_cmdVelocity = 0;
                m_actualVelocity = 0;
                m_cmdTorque = 0;
                m_actualTorque = 0;
                m_alarmCode = 0;
                m_profileTargetPos = 0;
                m_cmdEncoder = 0;
                m_actualEncoder = 0;
                m_alarmCode = 0;
            }

            public AxisStatus Copy()
            {
                return (AxisStatus)this.MemberwiseClone();
            }
        }
        public class AxisParameter
        {
            public double m_gearRatioNum;
            public double m_gearRatioDen;
            public WMXParam.m_motorDirection m_motorDirection;
            public bool m_absEncoderMode;
            public double m_absEncoderHomeOffset;

            public double m_inposWidth;
            public double m_posSetWidth;

            public WMXParam.m_homeType m_homeType;
            public WMXParam.m_homeDirection m_homeDirection;
            public double m_homeSlowVelocity;
            public double m_homeSlowAcc;
            public double m_homeSlowDec;
            public double m_homeFastVelocity;
            public double m_homeFastAcc;
            public double m_homeFastDec;
            public double m_homeShiftVelocity;
            public double m_homeShiftAcc;
            public double m_homeShiftDec;
            public double m_homeShiftDistance;

            public WMXParam.m_limitSwitchType m_limitSwitchType;
            public WMXParam.m_limitDirection m_limitDirection;
            public WMXParam.m_limitSwitchType m_softLimitSwitchType;
            public double m_softLimitPosValue;
            public double m_softLimitNegValue;
            public double m_limitDec;
            public double m_limitSlowDec;
            public bool m_invertPosLimit;
            public bool m_invertNegLimit;

            public WMXParam.m_linIntplCalcMode m_linintplCalcMode;
            public double m_quickStopDecel;

            public AxisParameter()
            {
                m_gearRatioNum = 1;
                m_gearRatioDen = 1;
                m_motorDirection = WMXParam.m_motorDirection.Positive;
                m_absEncoderMode = false;
                m_absEncoderHomeOffset = 0;

                m_inposWidth = 1000;
                m_posSetWidth = 1000;

                m_homeType = WMXParam.m_homeType.CurPos;
                m_homeDirection = WMXParam.m_homeDirection.Positive;
                m_homeSlowVelocity = 10000;
                m_homeSlowAcc = 10000;
                m_homeSlowDec = 10000;
                m_homeFastVelocity = 10000;
                m_homeFastAcc = 10000;
                m_homeFastDec = 10000;
                m_homeShiftVelocity = 10000;
                m_homeShiftAcc = 10000;
                m_homeShiftDec = 10000;
                m_homeShiftDistance = 0;

                m_limitSwitchType = WMXParam.m_limitSwitchType.None;
                m_limitDirection = WMXParam.m_limitDirection.Normal;
                m_softLimitSwitchType = WMXParam.m_limitSwitchType.None;
                m_softLimitPosValue = 0;
                m_softLimitNegValue = 0;
                m_limitDec = 10000;
                m_limitSlowDec = 10000;
                m_invertPosLimit = false;
                m_invertNegLimit = false;

                m_linintplCalcMode = WMXParam.m_linIntplCalcMode.AxisLimit;
                m_quickStopDecel = 100000;
            }

            public void SetParam(AxisParameter axisParameter)
            {
                m_gearRatioNum = axisParameter.m_gearRatioNum;
                m_gearRatioDen = axisParameter.m_gearRatioDen;
                m_motorDirection = axisParameter.m_motorDirection;
                m_absEncoderMode = axisParameter.m_absEncoderMode;
                m_absEncoderHomeOffset = axisParameter.m_absEncoderHomeOffset;

                m_inposWidth = axisParameter.m_inposWidth;
                m_posSetWidth = axisParameter.m_posSetWidth;

                m_homeType = axisParameter.m_homeType;
                m_homeDirection = axisParameter.m_homeDirection;
                m_homeSlowVelocity = axisParameter.m_homeSlowVelocity;
                m_homeSlowAcc = axisParameter.m_homeSlowAcc;
                m_homeSlowDec = axisParameter.m_homeSlowDec;
                m_homeFastVelocity = axisParameter.m_homeFastVelocity;
                m_homeFastAcc = axisParameter.m_homeFastAcc;
                m_homeFastDec = axisParameter.m_homeFastDec;
                m_homeShiftVelocity = axisParameter.m_homeShiftVelocity;
                m_homeShiftAcc = axisParameter.m_homeShiftAcc;
                m_homeShiftDec = axisParameter.m_homeShiftDec;
                m_homeShiftDistance = axisParameter.m_homeShiftDistance;

                m_limitSwitchType = axisParameter.m_limitSwitchType;
                m_limitDirection = axisParameter.m_limitDirection;
                m_softLimitSwitchType = axisParameter.m_softLimitSwitchType;
                m_softLimitPosValue = axisParameter.m_softLimitPosValue;
                m_softLimitNegValue = axisParameter.m_softLimitNegValue;
                m_limitDec = axisParameter.m_limitDec;
                m_limitSlowDec = axisParameter.m_limitSlowDec;
                m_invertPosLimit = axisParameter.m_invertPosLimit;
                m_invertNegLimit = axisParameter.m_invertNegLimit;

                m_linintplCalcMode = axisParameter.m_linintplCalcMode;
                m_quickStopDecel = axisParameter.m_quickStopDecel;
            }
            public void GetParam(ref AxisParameter axisParameter)
            {
                axisParameter.m_gearRatioNum = m_gearRatioNum;
                axisParameter.m_gearRatioDen = m_gearRatioDen;
                axisParameter.m_motorDirection = m_motorDirection;
                axisParameter.m_absEncoderMode = m_absEncoderMode;
                axisParameter.m_absEncoderHomeOffset = m_absEncoderHomeOffset;

                axisParameter.m_inposWidth = m_inposWidth;
                axisParameter.m_posSetWidth = m_posSetWidth;

                axisParameter.m_homeType = m_homeType;
                axisParameter.m_homeDirection = m_homeDirection;
                axisParameter.m_homeSlowVelocity = m_homeSlowVelocity;
                axisParameter.m_homeSlowAcc = m_homeSlowAcc;
                axisParameter.m_homeSlowDec = m_homeSlowDec;
                axisParameter.m_homeFastVelocity = m_homeFastVelocity;
                axisParameter.m_homeFastAcc = m_homeFastAcc;
                axisParameter.m_homeFastDec = m_homeFastDec;
                axisParameter.m_homeShiftVelocity = m_homeShiftVelocity;
                axisParameter.m_homeShiftAcc = m_homeShiftAcc;
                axisParameter.m_homeShiftDec = m_homeShiftDec;
                axisParameter.m_homeShiftDistance = m_homeShiftDistance;

                axisParameter.m_limitSwitchType = m_limitSwitchType;
                axisParameter.m_limitDirection = m_limitDirection;
                axisParameter.m_softLimitSwitchType = m_softLimitSwitchType;
                axisParameter.m_softLimitPosValue = m_softLimitPosValue;
                axisParameter.m_softLimitNegValue = m_softLimitNegValue;
                axisParameter.m_limitDec = m_limitDec;
                axisParameter.m_limitSlowDec = m_limitSlowDec;
                axisParameter.m_invertPosLimit = m_invertPosLimit;
                axisParameter.m_invertNegLimit = m_invertNegLimit;

                axisParameter.m_linintplCalcMode = m_linintplCalcMode;
                axisParameter.m_quickStopDecel = m_quickStopDecel;
            }
        }
        public WMXMotion(string deviceName, bool AllAxisAutoUpdate = true)
        {
            m_wmx3Api = new WMX3Api();

            m_wmx3Api.CreateDevice(@"C:\Program Files\SoftServo\WMX3", WMX3ApiCLR.DeviceType.DeviceTypeNormal);
            m_wmx3Api.SetDeviceName(deviceName);

            m_coreMotion = new CoreMotion(m_wmx3Api);
            //m_coreMotionStatus = new CoreMotionStatus();
            //m_systemParam = new Config.SystemParam();
            //m_axisParam = new Config.AxisParam();

            m_axisStatus = new AxisStatus[Constants.MaxAxes];
            for (int i = 0; i < Constants.MaxAxes; i++)
                m_axisStatus[i] = new AxisStatus();
            m_axisParameter = new AxisParameter[Constants.MaxAxes];
            m_AxisProfile = new AxisProfile[Constants.MaxAxes];

            for (int i = 0; i < Constants.MaxAxes; i++)
            {
                m_axisParameter[i] = new AxisParameter();
                m_AxisProfile[i] = new AxisProfile();
            }



            if (AllAxisAutoUpdate)
            {
                Thread LocalThread = new Thread(delegate ()
                {
                    while (WMX3.IsDeviceValid() && IsDeviceValid())
                    {
                        UpdateMotionStatus();
                        Thread.Sleep(5);
                    }
                });
                LocalThread.IsBackground = true;
                LocalThread.Name = $"MovenCore_WMXMotion [{deviceName}]";
                LocalThread.Start();
            }
        }

        public void CloseWMXMotion()
        {
            if (m_wmx3Api != null)
            {
                if (m_wmx3Api.IsDeviceValid())
                {
                    Console.WriteLine("Motion:" + m_wmx3Api.IsDeviceValid());
                    m_wmx3Api.CloseDevice();
                }
                //    m_wmx3Api.CloseDevice();
                //Console.WriteLine("Motion:"+m_wmx3Api.IsDeviceValid());
            }
        }

        public void UpdateMotionStatus()
        {
            var m_coreMotionStatus = WMX3.m_coreMotionStatus;


            for (int i = 0; i < Constants.MaxAxes; i++)
            {
                m_axisStatus[i].m_actualPos = m_coreMotionStatus.AxesStatus[i].ActualPos;
                m_axisStatus[i].m_cmdPos = m_coreMotionStatus.AxesStatus[i].PosCmd;
                m_axisStatus[i].m_actualVelocity = m_coreMotionStatus.AxesStatus[i].ActualVelocity;
                m_axisStatus[i].m_cmdVelocity = m_coreMotionStatus.AxesStatus[i].VelocityCmd;
                m_axisStatus[i].m_actualTorque = m_coreMotionStatus.AxesStatus[i].ActualTorque;
                m_axisStatus[i].m_cmdTorque = m_coreMotionStatus.AxesStatus[i].TorqueCmd;
                m_axisStatus[i].m_profileTargetPos = m_coreMotionStatus.AxesStatus[i].ProfileTargetPos;
                m_axisStatus[i].m_cmdEncoder = m_coreMotionStatus.AxesStatus[i].EncoderCommand;
                m_axisStatus[i].m_actualEncoder = m_coreMotionStatus.AxesStatus[i].EncoderFeedback;
                m_axisStatus[i].m_alarmCode = m_coreMotionStatus.AxesStatus[i].AmpAlarmCode;

                m_axisStatus[i].m_servoOffline = m_coreMotionStatus.AxesStatus[i].ServoOffline;
                m_axisStatus[i].m_servoOn = m_coreMotionStatus.AxesStatus[i].ServoOn;
                m_axisStatus[i].m_nLimit = m_coreMotionStatus.AxesStatus[i].NegativeLS;
                m_axisStatus[i].m_pLimit = m_coreMotionStatus.AxesStatus[i].PositiveLS;
                m_axisStatus[i].m_nLimitSoft = m_coreMotionStatus.AxesStatus[i].NegativeSoftLimit;
                m_axisStatus[i].m_pLimitSoft = m_coreMotionStatus.AxesStatus[i].PositiveSoftLimit;
                m_axisStatus[i].m_origin = m_coreMotionStatus.AxesStatus[i].HomeSwitch;
                m_axisStatus[i].m_alarm = m_coreMotionStatus.AxesStatus[i].AmpAlarm;
                m_axisStatus[i].m_waitingTrigger = m_coreMotionStatus.AxesStatus[i].WaitingForTrigger;
                m_axisStatus[i].m_homeDone = m_coreMotionStatus.AxesStatus[i].HomeDone;

                m_axisStatus[i].m_Inpos = m_coreMotionStatus.AxesStatus[i].InPos;
                m_axisStatus[i].m_posSet = m_coreMotionStatus.AxesStatus[i].PosSet;

                if (m_coreMotionStatus.AxesStatus[i].OpState == OperationState.Idle)
                    m_axisStatus[i].m_servoRun = false;
                else
                    m_axisStatus[i].m_servoRun = true;

                if (m_coreMotionStatus.AxesStatus[i].OpState == OperationState.Home)
                    m_axisStatus[i].m_homing = true;
                else
                    m_axisStatus[i].m_homing = false;

                if (m_coreMotionStatus.AxesStatus[i].OpState == OperationState.Jog)
                    m_axisStatus[i].m_Joging = true;
                else
                    m_axisStatus[i].m_Joging = false;
            }
            Thread.Sleep(1);
        }
        public void UpdateMotionStatus(int nAxis)
        {
            var m_coreMotionStatus = WMX3.m_coreMotionStatus;

            m_axisStatus[nAxis].m_actualPos = m_coreMotionStatus.AxesStatus[nAxis].ActualPos;
            m_axisStatus[nAxis].m_cmdPos = m_coreMotionStatus.AxesStatus[nAxis].PosCmd;
            m_axisStatus[nAxis].m_actualEncoder = m_coreMotionStatus.AxesStatus[nAxis].EncoderFeedback;
            m_axisStatus[nAxis].m_cmdEncoder = m_coreMotionStatus.AxesStatus[nAxis].EncoderCommand;
            m_axisStatus[nAxis].m_actualVelocity = m_coreMotionStatus.AxesStatus[nAxis].ActualVelocity;
            m_axisStatus[nAxis].m_cmdVelocity = m_coreMotionStatus.AxesStatus[nAxis].VelocityCmd;
            m_axisStatus[nAxis].m_actualTorque = m_coreMotionStatus.AxesStatus[nAxis].ActualTorque;
            m_axisStatus[nAxis].m_cmdTorque = m_coreMotionStatus.AxesStatus[nAxis].TorqueCmd;
            m_axisStatus[nAxis].m_profileTargetPos = m_coreMotionStatus.AxesStatus[nAxis].ProfileTargetPos;
            m_axisStatus[nAxis].m_alarmCode = m_coreMotionStatus.AxesStatus[nAxis].AmpAlarmCode;

            m_axisStatus[nAxis].m_servoOffline = m_coreMotionStatus.AxesStatus[nAxis].ServoOffline;
            m_axisStatus[nAxis].m_servoOn = m_coreMotionStatus.AxesStatus[nAxis].ServoOn;
            m_axisStatus[nAxis].m_nLimit = m_coreMotionStatus.AxesStatus[nAxis].NegativeLS;
            m_axisStatus[nAxis].m_pLimit = m_coreMotionStatus.AxesStatus[nAxis].PositiveLS;
            m_axisStatus[nAxis].m_nLimitSoft = m_coreMotionStatus.AxesStatus[nAxis].NegativeSoftLimit;
            m_axisStatus[nAxis].m_pLimitSoft = m_coreMotionStatus.AxesStatus[nAxis].PositiveSoftLimit;
            m_axisStatus[nAxis].m_origin = m_coreMotionStatus.AxesStatus[nAxis].HomeSwitch;
            m_axisStatus[nAxis].m_alarm = m_coreMotionStatus.AxesStatus[nAxis].AmpAlarm;
            m_axisStatus[nAxis].m_waitingTrigger = m_coreMotionStatus.AxesStatus[nAxis].WaitingForTrigger;
            m_axisStatus[nAxis].m_homeDone = m_coreMotionStatus.AxesStatus[nAxis].HomeDone;

            m_axisStatus[nAxis].m_Inpos = m_coreMotionStatus.AxesStatus[nAxis].InPos;
            m_axisStatus[nAxis].m_posSet = m_coreMotionStatus.AxesStatus[nAxis].PosSet;

            if (m_coreMotionStatus.AxesStatus[nAxis].OpState == OperationState.Idle)
                m_axisStatus[nAxis].m_servoRun = false;
            else
                m_axisStatus[nAxis].m_servoRun = true;

            if (m_coreMotionStatus.AxesStatus[nAxis].OpState == OperationState.Home)
                m_axisStatus[nAxis].m_homing = true;
            else
                m_axisStatus[nAxis].m_homing = false;

            if (m_coreMotionStatus.AxesStatus[nAxis].OpState == OperationState.Jog)
                m_axisStatus[nAxis].m_Joging = true;
            else
                m_axisStatus[nAxis].m_Joging = false;

            Thread.Sleep(1);
        }

        public void GetAxisParam()
        {
            Config.SystemParam systemParam = new Config.SystemParam();
            Config.AxisParam axisParam = new Config.AxisParam();

            m_coreMotion.Config.GetParam(ref systemParam);
            m_coreMotion.Config.GetAxisParam(ref axisParam);

            for (int i = 0; i < Constants.MaxAxes; i++)
            {
                m_axisParameter[i].m_gearRatioNum = axisParam.GearRatioNumerator[i];
                m_axisParameter[i].m_gearRatioDen = axisParam.GearRatioDenominator[i];
                m_axisParameter[i].m_motorDirection = axisParam.AxisPolarity[i] == 1 ? WMXParam.m_motorDirection.Positive : WMXParam.m_motorDirection.Negative;
                m_axisParameter[i].m_absEncoderMode = axisParam.AbsoluteEncoderMode[i];
                m_axisParameter[i].m_absEncoderHomeOffset = axisParam.AbsoluteEncoderHomeOffset[i];

                m_axisParameter[i].m_inposWidth = systemParam.FeedbackParam[i].InPosWidth;
                m_axisParameter[i].m_posSetWidth = systemParam.FeedbackParam[i].PosSetWidth;

                m_axisParameter[i].m_homeType = (WMXParam.m_homeType)systemParam.HomeParam[i].HomeType;
                m_axisParameter[i].m_homeDirection = (WMXParam.m_homeDirection)systemParam.HomeParam[i].HomeDirection;

                m_axisParameter[i].m_homeFastVelocity = systemParam.HomeParam[i].HomingVelocityFast;
                m_axisParameter[i].m_homeSlowVelocity = systemParam.HomeParam[i].HomingVelocitySlow;
                m_axisParameter[i].m_homeShiftVelocity = systemParam.HomeParam[i].HomeShiftVelocity;

                m_axisParameter[i].m_homeFastAcc = systemParam.HomeParam[i].HomingVelocityFastAcc;
                m_axisParameter[i].m_homeSlowAcc = systemParam.HomeParam[i].HomingVelocitySlowAcc;
                m_axisParameter[i].m_homeShiftAcc = systemParam.HomeParam[i].HomeShiftAcc;

                m_axisParameter[i].m_homeFastDec = systemParam.HomeParam[i].HomingVelocityFastDec;
                m_axisParameter[i].m_homeSlowDec = systemParam.HomeParam[i].HomingVelocitySlowDec;
                m_axisParameter[i].m_homeShiftDec = systemParam.HomeParam[i].HomeShiftDec;

                m_axisParameter[i].m_homeShiftDistance = systemParam.HomeParam[i].HomeShiftDistance;

                m_axisParameter[i].m_linintplCalcMode = (WMXParam.m_linIntplCalcMode)systemParam.MotionParam[i].LinearIntplProfileCalcMode;
                m_axisParameter[i].m_quickStopDecel = systemParam.MotionParam[i].QuickStopDec;

                m_axisParameter[i].m_limitSwitchType = (WMXParam.m_limitSwitchType)systemParam.LimitParam[i].LSType;
                m_axisParameter[i].m_limitDirection = (WMXParam.m_limitDirection)systemParam.LimitParam[i].LSDirection;
                m_axisParameter[i].m_softLimitSwitchType = (WMXParam.m_limitSwitchType)systemParam.LimitParam[i].SoftLimitType;

                m_axisParameter[i].m_softLimitPosValue = systemParam.LimitParam[i].SoftLimitPositivePos;
                m_axisParameter[i].m_softLimitNegValue = systemParam.LimitParam[i].SoftLimitNegativePos;

                m_axisParameter[i].m_invertNegLimit = systemParam.LimitParam[i].InvertNegativeLSPolarity;
                m_axisParameter[i].m_invertPosLimit = systemParam.LimitParam[i].InvertPositiveLSPolarity;
            }
        }

        public int SetAxisParam()
        {
            int ret = -1;

            Config.SystemParam systemParam = new Config.SystemParam();
            Config.AxisParam axisParam = new Config.AxisParam();

            ret = m_coreMotion.Config.GetParam(ref systemParam);

            if (ret != WMXParam.ErrorCode_None)
                return ret;

            ret = m_coreMotion.Config.GetAxisParam(ref axisParam);

            if (ret != WMXParam.ErrorCode_None)
                return ret;

            for (int i = 0; i < Constants.MaxAxes; i++)
            {
                axisParam.GearRatioNumerator[i] = m_axisParameter[i].m_gearRatioNum;
                axisParam.GearRatioDenominator[i] = m_axisParameter[i].m_gearRatioDen;
                axisParam.AxisPolarity[i] = m_axisParameter[i].m_motorDirection == WMXParam.m_motorDirection.Positive ? (sbyte)1 : (sbyte)-1;
                axisParam.AbsoluteEncoderMode[i] = m_axisParameter[i].m_absEncoderMode;
                axisParam.AbsoluteEncoderHomeOffset[i] = m_axisParameter[i].m_absEncoderHomeOffset;

                systemParam.FeedbackParam[i].InPosWidth = m_axisParameter[i].m_inposWidth;
                systemParam.FeedbackParam[i].PosSetWidth = m_axisParameter[i].m_posSetWidth;

                systemParam.HomeParam[i].HomeType = (Config.HomeType)m_axisParameter[i].m_homeType;
                systemParam.HomeParam[i].HomeDirection = (Config.HomeDirection)m_axisParameter[i].m_homeDirection;
                systemParam.HomeParam[i].HomingVelocityFast = m_axisParameter[i].m_homeFastVelocity;
                systemParam.HomeParam[i].HomingVelocitySlow = m_axisParameter[i].m_homeSlowVelocity;
                systemParam.HomeParam[i].HomeShiftVelocity = m_axisParameter[i].m_homeShiftVelocity;

                systemParam.HomeParam[i].HomingVelocityFastAcc = m_axisParameter[i].m_homeFastAcc;
                systemParam.HomeParam[i].HomingVelocitySlowAcc = m_axisParameter[i].m_homeSlowAcc;
                systemParam.HomeParam[i].HomeShiftAcc = m_axisParameter[i].m_homeShiftAcc;

                systemParam.HomeParam[i].HomingVelocityFastDec = m_axisParameter[i].m_homeFastDec;
                systemParam.HomeParam[i].HomingVelocitySlowDec = m_axisParameter[i].m_homeSlowDec;
                systemParam.HomeParam[i].HomeShiftDec = m_axisParameter[i].m_homeShiftDec;

                systemParam.HomeParam[i].HomeShiftDistance = m_axisParameter[i].m_homeShiftDistance;

                systemParam.MotionParam[i].LinearIntplProfileCalcMode = (Config.LinearIntplProfileCalcMode)m_axisParameter[i].m_linintplCalcMode;
                systemParam.MotionParam[i].QuickStopDec = m_axisParameter[i].m_quickStopDecel;

                systemParam.LimitParam[i].LSType = (Config.LimitSwitchType)m_axisParameter[i].m_limitSwitchType;
                systemParam.LimitParam[i].LSDirection = (Config.LimitSwitchDirection)m_axisParameter[i].m_limitDirection;
                systemParam.LimitParam[i].SoftLimitType = (Config.LimitSwitchType)m_axisParameter[i].m_softLimitSwitchType;

                systemParam.LimitParam[i].SoftLimitPositivePos = m_axisParameter[i].m_softLimitPosValue;
                systemParam.LimitParam[i].SoftLimitNegativePos = m_axisParameter[i].m_softLimitNegValue;

                systemParam.LimitParam[i].LSDec = m_axisParameter[i].m_limitDec;
                systemParam.LimitParam[i].LSSlowDec = m_axisParameter[i].m_limitSlowDec;

                systemParam.LimitParam[i].InvertNegativeLSPolarity = m_axisParameter[i].m_invertNegLimit;
                systemParam.LimitParam[i].InvertPositiveLSPolarity = m_axisParameter[i].m_invertPosLimit;
            }
            ret = m_coreMotion.Config.SetParam(systemParam);
            if (ret != WMXParam.ErrorCode_None)
                return ret;
            ret = m_coreMotion.Config.SetAxisParam(axisParam);
            if (ret != WMXParam.ErrorCode_None)
                return ret;
            return ret;
        }

        public int AxisParameterApplyToEngine(int nAxis, AxisParameter axisParameter)
        {
            int ret = -1;

            Config.AxisParam axisParam = new Config.AxisParam();
            Config.SystemParam axisSystemParam = new Config.SystemParam();

            ret = m_coreMotion.Config.GetParam(ref axisSystemParam);

            if (ret != WMXParam.ErrorCode_None)
                return ret;

            ret = m_coreMotion.Config.GetAxisParam(ref axisParam);

            if (ret != WMXParam.ErrorCode_None)
                return ret;

            axisParam.GearRatioNumerator[nAxis] = axisParameter.m_gearRatioNum;
            axisParam.GearRatioDenominator[nAxis] = axisParameter.m_gearRatioDen;
            axisParam.AxisPolarity[nAxis] = axisParameter.m_motorDirection == WMXParam.m_motorDirection.Positive ? (sbyte)1 : (sbyte)-1;
            axisParam.AbsoluteEncoderMode[nAxis] = axisParameter.m_absEncoderMode;
            axisParam.AbsoluteEncoderHomeOffset[nAxis] = axisParameter.m_absEncoderHomeOffset;

            axisSystemParam.FeedbackParam[nAxis].InPosWidth = axisParameter.m_inposWidth;
            axisSystemParam.FeedbackParam[nAxis].PosSetWidth = axisParameter.m_posSetWidth;

            axisSystemParam.HomeParam[nAxis].HomeType = (Config.HomeType)axisParameter.m_homeType;
            axisSystemParam.HomeParam[nAxis].HomeDirection = (Config.HomeDirection)axisParameter.m_homeDirection;
            axisSystemParam.HomeParam[nAxis].HomingVelocityFast = axisParameter.m_homeFastVelocity;
            axisSystemParam.HomeParam[nAxis].HomingVelocitySlow = axisParameter.m_homeSlowVelocity;
            axisSystemParam.HomeParam[nAxis].HomeShiftVelocity = axisParameter.m_homeShiftVelocity;

            axisSystemParam.HomeParam[nAxis].HomingVelocityFastAcc = axisParameter.m_homeFastAcc;
            axisSystemParam.HomeParam[nAxis].HomingVelocitySlowAcc = axisParameter.m_homeSlowAcc;
            axisSystemParam.HomeParam[nAxis].HomeShiftAcc = axisParameter.m_homeShiftAcc;

            axisSystemParam.HomeParam[nAxis].HomingVelocityFastDec = axisParameter.m_homeFastDec;
            axisSystemParam.HomeParam[nAxis].HomingVelocitySlowDec = axisParameter.m_homeSlowDec;
            axisSystemParam.HomeParam[nAxis].HomeShiftDec = axisParameter.m_homeShiftDec;

            axisSystemParam.HomeParam[nAxis].HomeShiftDistance = axisParameter.m_homeShiftDistance;

            axisSystemParam.MotionParam[nAxis].LinearIntplProfileCalcMode = (Config.LinearIntplProfileCalcMode)axisParameter.m_linintplCalcMode;
            axisSystemParam.MotionParam[nAxis].QuickStopDec = axisParameter.m_quickStopDecel;

            axisSystemParam.LimitParam[nAxis].LSType = (Config.LimitSwitchType)axisParameter.m_limitSwitchType;
            axisSystemParam.LimitParam[nAxis].LSDirection = (Config.LimitSwitchDirection)axisParameter.m_limitDirection;
            axisSystemParam.LimitParam[nAxis].SoftLimitType = (Config.LimitSwitchType)axisParameter.m_softLimitSwitchType;

            axisSystemParam.LimitParam[nAxis].LSDec = axisParameter.m_limitDec;
            axisSystemParam.LimitParam[nAxis].LSSlowDec = axisParameter.m_limitSlowDec;
            axisSystemParam.LimitParam[nAxis].SoftLimitPositivePos = axisParameter.m_softLimitPosValue;
            axisSystemParam.LimitParam[nAxis].SoftLimitNegativePos = axisParameter.m_softLimitNegValue;

            axisSystemParam.LimitParam[nAxis].InvertNegativeLSPolarity = axisParameter.m_invertNegLimit;
            axisSystemParam.LimitParam[nAxis].InvertPositiveLSPolarity = axisParameter.m_invertPosLimit;

            ret = m_coreMotion.Config.SetParam(nAxis, axisSystemParam);
            if (ret != WMXParam.ErrorCode_None)
                return ret;

            Thread.Sleep(1);

            ret = m_coreMotion.Config.SetAxisParam(nAxis, axisParam);
            if (ret != WMXParam.ErrorCode_None)
                return ret;

            return ret;
        }
        public int GetAxisParameterFromEngine(int nAxis, ref AxisParameter axisParameter)
        {
            Config.AxisParam axisParam = new Config.AxisParam();
            Config.SystemParam axisSystemParam = new Config.SystemParam();

            int ret = m_coreMotion.Config.GetParam(nAxis, ref axisSystemParam);

            if (ret != WMXParam.ErrorCode_None)
                return ret;

            Thread.Sleep(1);

            ret = m_coreMotion.Config.GetAxisParam(nAxis, ref axisParam);

            if (ret != WMXParam.ErrorCode_None)
                return ret;

            axisParameter.m_gearRatioNum = axisParam.GearRatioNumerator[nAxis];
            axisParameter.m_gearRatioDen = axisParam.GearRatioDenominator[nAxis];
            axisParameter.m_motorDirection = axisParam.AxisPolarity[nAxis] == 1 ? WMXParam.m_motorDirection.Positive : WMXParam.m_motorDirection.Negative;
            axisParameter.m_absEncoderMode = axisParam.AbsoluteEncoderMode[nAxis];
            axisParameter.m_absEncoderHomeOffset = axisParam.AbsoluteEncoderHomeOffset[nAxis];

            axisParameter.m_inposWidth = axisSystemParam.FeedbackParam[nAxis].InPosWidth;
            axisParameter.m_posSetWidth = axisSystemParam.FeedbackParam[nAxis].PosSetWidth;

            axisParameter.m_homeType = (WMXParam.m_homeType)axisSystemParam.HomeParam[nAxis].HomeType;
            axisParameter.m_homeDirection = (WMXParam.m_homeDirection)axisSystemParam.HomeParam[nAxis].HomeDirection;

            axisParameter.m_homeFastVelocity = axisSystemParam.HomeParam[nAxis].HomingVelocityFast;
            axisParameter.m_homeSlowVelocity = axisSystemParam.HomeParam[nAxis].HomingVelocitySlow;
            axisParameter.m_homeShiftVelocity = axisSystemParam.HomeParam[nAxis].HomeShiftVelocity;

            axisParameter.m_homeFastAcc = axisSystemParam.HomeParam[nAxis].HomingVelocityFastAcc;
            axisParameter.m_homeSlowAcc = axisSystemParam.HomeParam[nAxis].HomingVelocitySlowAcc;
            axisParameter.m_homeShiftAcc = axisSystemParam.HomeParam[nAxis].HomeShiftAcc;

            axisParameter.m_homeFastDec = axisSystemParam.HomeParam[nAxis].HomingVelocityFastDec;
            axisParameter.m_homeSlowDec = axisSystemParam.HomeParam[nAxis].HomingVelocitySlowDec;
            axisParameter.m_homeShiftDec = axisSystemParam.HomeParam[nAxis].HomeShiftDec;

            axisParameter.m_homeShiftDistance = axisSystemParam.HomeParam[nAxis].HomeShiftDistance;

            axisParameter.m_linintplCalcMode = (WMXParam.m_linIntplCalcMode)axisSystemParam.MotionParam[nAxis].LinearIntplProfileCalcMode;
            axisParameter.m_quickStopDecel = axisSystemParam.MotionParam[nAxis].QuickStopDec;

            axisParameter.m_limitSwitchType = (WMXParam.m_limitSwitchType)axisSystemParam.LimitParam[nAxis].LSType;
            axisParameter.m_limitDirection = (WMXParam.m_limitDirection)axisSystemParam.LimitParam[nAxis].LSDirection;
            axisParameter.m_softLimitSwitchType = (WMXParam.m_limitSwitchType)axisSystemParam.LimitParam[nAxis].SoftLimitType;

            axisParameter.m_limitDec = axisSystemParam.LimitParam[nAxis].LSDec;
            axisParameter.m_limitSlowDec = axisSystemParam.LimitParam[nAxis].LSSlowDec;
            axisParameter.m_softLimitPosValue = axisSystemParam.LimitParam[nAxis].SoftLimitPositivePos;
            axisParameter.m_softLimitNegValue = axisSystemParam.LimitParam[nAxis].SoftLimitNegativePos;

            axisParameter.m_invertNegLimit = axisSystemParam.LimitParam[nAxis].InvertNegativeLSPolarity;
            axisParameter.m_invertPosLimit = axisSystemParam.LimitParam[nAxis].InvertPositiveLSPolarity;

            return ret;
        }

        public int LoadAndSetAxisParameter(string path)
        {
            int ret = -1;
            ret = m_coreMotion.Config.ImportAndSetAll(path);
            //ret = m_coreMotion.Config.Import(path, ref m_systemParam, ref m_axisParam);
            //if (ret != WMXParam.ErrorCode_None)
            //    return ret;
            //ret = m_coreMotion.Config.SetParam(m_systemParam);
            //if (ret != WMXParam.ErrorCode_None)
            //    return ret;
            //ret = m_coreMotion.Config.SetAxisParam(m_axisParam);
            //if (ret != WMXParam.ErrorCode_None)
            //    return ret;
            GetAxisParam();
            return ret;
        }

        public int GetAndSaveAxisParameter(string path)
        {
            int ret = -1;
            SetAxisParam();
            //ret = m_coreMotion.Config.Export(path, m_systemParam, m_axisParam);
            ret = m_coreMotion.Config.GetAndExportAll(path);
            return ret;
        }
        public void ServoOn(int axis)
        {
            int ret = -1;
            ret = m_coreMotion.AxisControl.SetServoOn(axis, 1);

            if (ret != ErrorCode.None)
            {
                strErrString = WMX3Api.ErrorToString(ret);
                MessageBox.Show($"Faild to set servo on \n: {strErrString}");
                return;
            }
        }
        public void ServoOff(int axis)
        {
            int ret = -1;
            ret = m_coreMotion.AxisControl.SetServoOn(axis, 0);

            if (ret != ErrorCode.None)
            {
                strErrString = WMX3Api.ErrorToString(nErr);
                MessageBox.Show($"Faild to set servo on \n: {strErrString}");
                return;
            }
        }
        public bool IsAmpOffline(int axisIndex)
        {
            if (WMX3.IsEngineCommunicating())
            {
                return m_axisStatus[axisIndex]?.m_servoOffline ?? true;
            }
            else
                return true;
        }
        public bool IsServoOn(int axisIndex)
        {
            if (WMX3.IsEngineCommunicating())
            {
                //GetWMX3MotionStatus();
                if (m_axisStatus[axisIndex].m_servoOn)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public bool IsServoRun(int axisIndex)
        {
            if (IsServoOn(axisIndex))
            {
                if (m_axisStatus[axisIndex].m_servoRun)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        public int AbsoluteMove(AxisProfile posProfile, double Location)
        {
            int ret = -1;
            Motion.PosCommand pos = new Motion.PosCommand();

            pos.Axis = posProfile.m_axis;
            pos.Profile.Type = (ProfileType)posProfile.m_profileType;
            pos.Profile.Velocity = posProfile.m_velocity;
            pos.Profile.Acc = posProfile.m_acc;
            pos.Profile.Dec = posProfile.m_dec;
            pos.Target = Location;
            pos.Profile.EndVelocity = posProfile.m_endvelocity;
            pos.Profile.JerkAccRatio = pos.Profile.JerkDecRatio = posProfile.m_jerkRatio;

            ret = m_coreMotion.Motion.StartPos(pos);
            return ret;
        }

        public int AbsoluteMove(AxisProfile posProfile, TriggerCondition trigger)
        {
            int ret = -1;
            Motion.TriggerPosCommand pos = new Motion.TriggerPosCommand();

            pos.Axis = posProfile.m_axis;
            pos.Profile.Type = (ProfileType)posProfile.m_profileType;
            pos.Profile.Velocity = posProfile.m_velocity;
            pos.Profile.Acc = posProfile.m_acc;
            pos.Profile.Dec = posProfile.m_dec;
            pos.Target = posProfile.m_dest;
            pos.Profile.JerkAccRatio = pos.Profile.JerkDecRatio = posProfile.m_jerkRatio;

            pos.Trigger.TriggerAxis = trigger.m_triggerAxis;
            pos.Trigger.TriggerValue = trigger.m_triggerValue;
            pos.Trigger.TriggerType = (TriggerType)trigger.m_triggerType;

            ret = m_coreMotion.Motion.StartPos(pos);
            return ret;
        }

        public int RelativeMove(AxisProfile movProfile, double distance, double velocity)
        {
            int ret = -1;
            Motion.PosCommand pos = new Motion.PosCommand();

            pos.Axis = movProfile.m_axis;
            pos.Profile.Type = (ProfileType)movProfile.m_profileType;
            pos.Profile.Velocity = velocity;
            pos.Profile.Acc = movProfile.m_acc;
            pos.Profile.Dec = movProfile.m_dec;
            pos.Target = distance;
            pos.Profile.JerkAccRatio = pos.Profile.JerkDecRatio = movProfile.m_jerkRatio;

            ret = m_coreMotion.Motion.StartMov(pos);
            return ret;

        }

        public int RelativeMove(AxisProfile posProfile, TriggerCondition trigger)
        {
            int ret = -1;
            Motion.TriggerPosCommand pos = new Motion.TriggerPosCommand();

            pos.Axis = posProfile.m_axis;
            pos.Profile.Type = (ProfileType)posProfile.m_profileType;
            pos.Profile.Velocity = posProfile.m_velocity;
            pos.Profile.Acc = posProfile.m_acc;
            pos.Profile.Dec = posProfile.m_dec;
            pos.Target = posProfile.m_dest;
            pos.Profile.JerkAccRatio = pos.Profile.JerkDecRatio = posProfile.m_jerkRatio;

            pos.Trigger.TriggerAxis = trigger.m_triggerAxis;
            pos.Trigger.TriggerValue = trigger.m_triggerValue;
            pos.Trigger.TriggerType = (TriggerType)trigger.m_triggerType;

            ret = m_coreMotion.Motion.StartPos(pos);
            return ret;
        }
        public void AllServoOn()
        {
            foreach (var conveyor in conveyors)
            {
                if ((conveyor.servo == ServoOnOff.Off) && (conveyor.axis != -1))
                {
                    if (conveyor.type == "Turn")
                    {
                        ServoOn(conveyor.axis);
                        ServoOn(conveyor.turnAxis);
                    }
                    else
                    {
                        ServoOn(conveyor.axis);
                    }
                }
            }
        }
        public void AllServoOff()
        {
            foreach (var conveyor in conveyors)
            {
                if ((conveyor.servo == ServoOnOff.On) && (conveyor.axis != -1))
                {
                    if (conveyor.type == "Turn")
                    {
                        ServoOff(conveyor.axis);
                        ServoOff(conveyor.turnAxis);
                    }
                    else
                    {
                        ServoOff(conveyor.axis);
                    }
                }
            }
        }
        public void StartJogPos(int axis, double speed)
        {
            if (G_Var.bMouse == true)
            {
                Motion.JogCommand jog = new Motion.JogCommand();
                jog.Axis = axis;
                jog.Profile.Velocity = speed * 10;
                jog.Profile.Acc = 500;
                jog.Profile.Dec = 500;

                int ret = m_coreMotion.Motion.StartJog(jog);
                if (ret != ErrorCode.None)
                {
                    ret = m_coreMotion.Motion.Stop(axis);
                    MessageBox.Show("Failed to start positive jog. Error code: " + ret.ToString());
                    return;
                }
            }
        }
        public void StartJogNeg(int axis, double speed)
        {
            if (G_Var.bMouse == true)
            {
                Motion.JogCommand jog = new Motion.JogCommand();
                jog.Axis = axis;
                jog.Profile.Velocity = -speed * 10;
                jog.Profile.Acc = 500;
                jog.Profile.Dec = 500;

                int ret = m_coreMotion.Motion.StartJog(jog);
                if (ret != ErrorCode.None)
                {
                    ret = m_coreMotion.Motion.Stop(axis);
                    MessageBox.Show("Failed to start negative jog. Error code: " + ret.ToString());
                    return;
                }
            }
        }
        public void StartJogPos_Auto(int axis, double speed, double acc, double dec)
        {

            Motion.JogCommand jog = new Motion.JogCommand();
            jog.Axis = axis;
            jog.Profile.Velocity = speed * 10;
            jog.Profile.Acc = acc * 10;
            jog.Profile.Dec = dec * 10;

            int ret = m_coreMotion.Motion.StartJog(jog);
            //if (ret != ErrorCode.None)
            //{
            //    ret = m_coreMotion.Motion.Stop(axis);
            //    MessageBox.Show("Failed to start positive jog. Error code: " + ret.ToString());
            //    return;
            //}

        }
        public void StartJogNeg_Auto(int axis, double speed, double acc, double dec)
        {

            Motion.JogCommand jog = new Motion.JogCommand();
            jog.Axis = axis;
            jog.Profile.Velocity = -speed * 10;
            jog.Profile.Acc = acc * 10;
            jog.Profile.Dec = dec * 10;

            int ret = m_coreMotion.Motion.StartJog(jog);
            //if (ret != ErrorCode.None)
            //{
            //    ret = m_coreMotion.Motion.Stop(axis);
            //    MessageBox.Show("Failed to start negative jog. Error code: " + ret.ToString());
            //    return;
            //}

        }
        // JOG 공통 메서드.
        public void StopJog(int axis)
        {
            int ret = m_coreMotion.Motion.Stop(axis);
            if (ret != ErrorCode.None)
            {
                MessageBox.Show("Failed to stop jog. Error code: " + ret.ToString());
            }
        }
        public void AutoModeChange()
        {
            if (!isAutoRun && isAutoEnable)
            {
                isAutoRun = true;
            }
            else
            {
                isAutoRun = false;
            }
        }
        public int HomeDoneClear(int axisIndex)
        {
            int ret = m_coreMotion.Home.SetHomeDone(axisIndex, 0);
            return ret;
        }
        public int SetHomeDoneFlag(int axisIndex, bool homeDone)
        {
            int ret = 0;
            if (homeDone)
                ret = m_coreMotion.Home.SetHomeDone(axisIndex, 1);
            else
                ret = m_coreMotion.Home.SetHomeDone(axisIndex, 0);

            return ret;
        }
        public int EmergencyStop(int axisIndex)
        {
            int ret = -1;
            ret = m_coreMotion.Motion.ExecQuickStop(axisIndex);
            return ret;
        }
        public int AbsLinearInterpolation(AxesProfile axesProfile)
        {
            int ret = -1;
            Motion.LinearIntplCommand lin = new Motion.LinearIntplCommand();

            lin.AxisCount = axesProfile.m_axisCount;
            for (int i = 0; i < axesProfile.m_axisCount; i++)
            {
                lin.Target[i] = axesProfile.m_dest[i];
                lin.Axis[i] = axesProfile.m_axisArray[i];
                lin.MaxVelocity[i] = axesProfile.m_maxVel[i];
                lin.MaxAcc[i] = axesProfile.m_maxAcc[i];
                lin.MaxDec[i] = axesProfile.m_maxDec[i];
                lin.MaxJerkAcc[i] = lin.MaxJerkDec[i] = axesProfile.m_jerkRatio;
            }

            lin.Profile.Type = (ProfileType)axesProfile.m_profileType;
            lin.Profile.Velocity = axesProfile.m_velocity;
            lin.Profile.Acc = axesProfile.m_acc;
            lin.Profile.Dec = axesProfile.m_dec;
            lin.Profile.JerkAccRatio = lin.Profile.JerkDecRatio = axesProfile.m_jerkRatio;

            ret = m_coreMotion.Motion.StartLinearIntplPos(lin);
            return ret;
        }

        public int AbsLinearInterpolation(AxesProfile axesProfile, TriggerCondition trigger)
        {
            int ret = -1;
            Motion.LinearIntplCommand lin = new Motion.LinearIntplCommand();
            Trigger trig = new Trigger();

            lin.AxisCount = axesProfile.m_axisCount;
            for (int i = 0; i < axesProfile.m_axisCount; i++)
            {
                lin.Target[i] = axesProfile.m_dest[i];
                lin.Axis[i] = axesProfile.m_axisArray[i];
                lin.MaxVelocity[i] = axesProfile.m_maxVel[i];
                lin.MaxAcc[i] = axesProfile.m_maxAcc[i];
                lin.MaxDec[i] = axesProfile.m_maxDec[i];
                lin.MaxJerkAcc[i] = lin.MaxJerkDec[i] = axesProfile.m_jerkRatio;
            }

            lin.Profile.Type = (ProfileType)axesProfile.m_profileType;
            lin.Profile.Velocity = axesProfile.m_velocity;
            lin.Profile.Acc = axesProfile.m_acc;
            lin.Profile.Dec = axesProfile.m_dec;
            lin.Profile.JerkAccRatio = lin.Profile.JerkDecRatio = axesProfile.m_jerkRatio;

            trig.TriggerAxis = trigger.m_triggerAxis;
            trig.TriggerValue = trigger.m_triggerValue;
            trig.TriggerType = (TriggerType)trigger.m_triggerType;

            ret = m_coreMotion.Motion.StartLinearIntplPos(lin, trig);
            return ret;
        }

        public int RelLinearInterpolation(AxesProfile axesProfile)
        {
            int ret = -1;
            Motion.LinearIntplCommand lin = new Motion.LinearIntplCommand();

            lin.AxisCount = axesProfile.m_axisCount;
            for (int i = 0; i < axesProfile.m_axisCount; i++)
            {
                lin.Target[i] = axesProfile.m_dest[i];
                lin.Axis[i] = axesProfile.m_axisArray[i];
                lin.MaxVelocity[i] = axesProfile.m_maxVel[i];
                lin.MaxAcc[i] = axesProfile.m_maxAcc[i];
                lin.MaxDec[i] = axesProfile.m_maxDec[i];
                lin.MaxJerkAcc[i] = lin.MaxJerkDec[i] = axesProfile.m_jerkRatio;
            }

            lin.Profile.Type = (ProfileType)axesProfile.m_profileType;
            lin.Profile.Velocity = axesProfile.m_velocity;
            lin.Profile.Acc = axesProfile.m_acc;
            lin.Profile.Dec = axesProfile.m_dec;
            lin.Profile.JerkAccRatio = lin.Profile.JerkDecRatio = axesProfile.m_jerkRatio;

            ret = m_coreMotion.Motion.StartLinearIntplMov(lin);
            return ret;
        }

        public int RelLinearInterpolation(AxesProfile axesProfile, TriggerCondition trigger)
        {
            int ret = -1;
            Motion.LinearIntplCommand lin = new Motion.LinearIntplCommand();
            Trigger trig = new Trigger();

            lin.AxisCount = axesProfile.m_axisCount;
            for (int i = 0; i < axesProfile.m_axisCount; i++)
            {
                lin.Target[i] = axesProfile.m_dest[i];
                lin.Axis[i] = axesProfile.m_axisArray[i];
                lin.MaxVelocity[i] = axesProfile.m_maxVel[i];
                lin.MaxAcc[i] = axesProfile.m_maxAcc[i];
                lin.MaxDec[i] = axesProfile.m_maxDec[i];
                lin.MaxJerkAcc[i] = lin.MaxJerkDec[i] = axesProfile.m_jerkRatio;
            }

            lin.Profile.Type = (ProfileType)axesProfile.m_profileType;
            lin.Profile.Velocity = axesProfile.m_velocity;
            lin.Profile.Acc = axesProfile.m_acc;
            lin.Profile.Dec = axesProfile.m_dec;
            lin.Profile.JerkAccRatio = lin.Profile.JerkDecRatio = axesProfile.m_jerkRatio;

            trig.TriggerAxis = trigger.m_triggerAxis;
            trig.TriggerValue = trigger.m_triggerValue;
            trig.TriggerType = (TriggerType)trigger.m_triggerType;

            ret = m_coreMotion.Motion.StartLinearIntplMov(lin, trig);
            return ret;
        }

        public int AlarmClear(int axisIndex)
        {
            int ret;
            ret = m_coreMotion.AxisControl.ClearAmpAlarm(axisIndex);

            if (ret != ErrorCode.None)
                return ret;

            ret = m_coreMotion.AxisControl.ClearAxisAlarm(axisIndex);

            return ret;
        }

        public int HomeStart(int axisIndex)
        {
            int ret = -1;
            ret = m_coreMotion.Home.StartHome(axisIndex);
            return ret;
        }

        public int Wait(int axisIndex)
        {
            int ret = -1;
            ret = m_coreMotion.Motion.Wait(axisIndex);
            return ret;
        }

        public int Wait(WaitCondition waitCondition)
        {
            int ret = -1;

            Motion.WaitCondition wait = new Motion.WaitCondition();

            wait.AxisCount = waitCondition.m_axisCount;
            for (int i = 0; i < waitCondition.m_axisCount; i++)
            {
                wait.Axis[i] = waitCondition.m_axisArray[i];
            }
            wait.WaitConditionType = (Motion.WaitConditionType)waitCondition.m_waitType;

            ret = m_coreMotion.Motion.Wait(wait);
            return ret;
        }

        public int SetCommandPos(int axis, double position)
        {
            int ret = -1;

            ret = m_coreMotion.Home.SetCommandPos(axis, position);

            m_coreMotion.Home.SetFeedbackPos(axis, position);

            return ret;
        }

        public CoreMotion GetMotionLib()
        {
            return m_coreMotion;
        }

        public int CloseMotionDevice()
        {
            int ret = -1;

            ret = m_wmx3Api.CloseDevice();
            return ret;
        }

        public bool IsDeviceValid()
        {
            try
            {
                if (m_wmx3Api == null)
                    return false;

                if (m_wmx3Api.IsDeviceValid())
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
