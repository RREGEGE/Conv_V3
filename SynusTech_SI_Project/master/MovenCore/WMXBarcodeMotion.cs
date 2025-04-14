using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR.BarcodeControls;
using WMX3ApiCLR;
using System.Threading;

namespace MovenCore {
    public enum BarcodeSpecInType {
        SpecIn                              = BarcodeReaderControlSpecInType.SPECIN,
        SpecIn_With_ZeroSpeed               = BarcodeReaderControlSpecInType.SPECIN_WITH_ZEROSPEED,
        SpecIn_With_Time                    = BarcodeReaderControlSpecInType.SPECIN_WITH_TIME,
        SpecIn_With_Time_And_ZeroSpeed      = BarcodeReaderControlSpecInType.SPECIN_WITH_TIME_AND_ZEROSPEED,
    }

    public class BarcodeParameter {
        public int m_axis;
        public int m_startAddr;
        public int m_size;
        public double m_barcodeScale;
        public double m_homeOffset;
        public BarcodeSpecInType m_specInType;
        public double m_specInRange;
        public double m_specInTimeSec;

        public BarcodeParameter() {
            m_axis              = 0;
            m_startAddr         = 0;
            m_size              = 0;
            m_barcodeScale      = 0;
            m_homeOffset        = 0;
            m_specInType        = BarcodeSpecInType.SpecIn;
            m_specInRange       = 0;
            m_specInTimeSec     = 0;
        }
    }

    public class BarcodeSafetyParameter {
        public int m_error1_StartAddr;
        public int m_error1_Bit;
        public int m_error2_StartAddr;
        public int m_error2_Bit;
        public int m_error3_StartAddr;
        public int m_error3_Bit;
        public int m_error4_StartAddr;
        public int m_error4_Bit;
        public int m_EMO1_StartAddr;
        public int m_EMO1_Bit;
        public int m_EMO2_StartAddr;
        public int m_EMO2_Bit;
        public int m_EMO3_StartAddr;
        public int m_EMO3_Bit;
        public int m_EMO4_StartAddr;
        public int m_EMO4_Bit;
        public double m_alarmStopDecTimeSec;
        public double m_stopDecTimeSec;
        public double m_followingError;
        public double m_velocityLimit;
        public double m_accLimit;
        public double m_decLimit;
        public bool m_useBarcodePositiveLimit;
        public double m_barcodePositiveLimit;
        public bool m_useBarcodeNegativeLimit;
        public double m_barcodeNegativeLimit;

        public BarcodeSafetyParameter() {
            m_error1_StartAddr              = 0;
            m_error1_Bit                    = 0;
            m_error2_StartAddr              = 0;
            m_error2_Bit                    = 0;
            m_error3_StartAddr              = 0;
            m_error3_Bit                    = 0;
            m_error4_StartAddr              = 0;
            m_error4_Bit                    = 0;
            m_EMO1_StartAddr                = 0;
            m_EMO1_Bit                      = 0;
            m_EMO2_StartAddr                = 0;
            m_EMO2_Bit                      = 0;
            m_EMO3_StartAddr                = 0;
            m_EMO3_Bit                      = 0;
            m_EMO4_StartAddr                = 0;
            m_EMO4_Bit                      = 0;
            m_alarmStopDecTimeSec           = 0;
            m_stopDecTimeSec                = 0;
            m_followingError                = 0;
            m_velocityLimit                 = 0;
            m_accLimit                      = 0;
            m_decLimit                      = 0;
            m_useBarcodePositiveLimit       = false;
            m_barcodePositiveLimit          = 0;
            m_useBarcodeNegativeLimit       = false;
            m_barcodeNegativeLimit          = 0;
        }
    }

    public class BarcodeClosedLoopCommand {
        public double m_targetDistance;
        public double m_velocity;
        public double m_acc;
        public double m_dec;
        public double m_pGain;
        public double m_iGain;

        public BarcodeClosedLoopCommand() {
            m_targetDistance    = 0;
            m_velocity          = 0;
            m_acc               = 0;
            m_dec               = 0;
            m_pGain             = 0;
            m_iGain             = 0;
        }
    }

    public enum BarcodeOpState {
        IDLE                    = BarcodeReaderControlState.IDLE,
        SERVO_OFF               = BarcodeReaderControlState.SERVO_OFF,
        POS_CLOSE_LOOP_MOVE     = BarcodeReaderControlState.POS_CLOSE_LOOP_MOVE,
        ALARM                   = BarcodeReaderControlState.ALARM,
        STOP                    = BarcodeReaderControlState.STOP
    }

    public class BarcodeStatus {
        public class BarcodeStatusCommand {
            public double m_targetDistance;
            public double m_torque;
            public double m_velocity;
            public double m_pos;

            public BarcodeStatusCommand() {
                m_targetDistance    = 0;
                m_torque            = 0;
                m_velocity          = 0;
                m_pos               = 0;
            }
        }

        public class BarcodeStatusFeedback {
            public BarcodeOpState m_op;
            public double m_targetDistance;
            public double m_commandDistance;
            public double m_actualDistance;
            public double m_rawDistance;
            public double m_torque;
            public double m_velocity;
            public double m_pos;
            public int m_alarmCode;

            public BarcodeStatusFeedback() {
                m_op                    = BarcodeOpState.IDLE;
                m_targetDistance        = 0;
                m_commandDistance       = 0;
                m_actualDistance        = 0;
                m_rawDistance           = 0;
                m_torque                = 0;
                m_velocity              = 0;
                m_pos                   = 0;
                m_alarmCode             = 0;
            }
        }

        public BarcodeStatusCommand m_cmdStatus;
        public BarcodeStatusFeedback m_fbStatus;

        public BarcodeStatus() {
            m_cmdStatus     = new BarcodeStatusCommand();
            m_fbStatus      = new BarcodeStatusFeedback();
        }
    }

    public enum BarcodeError_API_Call_Error {
        BARCODEREADERDLL_API_ERROR = 0x32000,
        BARCODEREADERDLL_API_ERROR_INVALID_PARAM,
        BARCODEREADERDLL_API_ERROR_NOT_IDLE_STATE,
        BARCODEREADERDLL_API_ERROR_PARAM_MEMORY_ERROR,
        BARCODEREADERDLL_API_ERROR_ANALOGINPUT_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_IO_STARTADDR_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_IO_BIT_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_AXIS_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_VELOCITY_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_ACC_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_DEC_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_ALARM_STOP_TIME_RANGE_ERROR,
        BARCODEREADERDLL_API_ERROR_EXIST_ALARMCODE,
        BARCODEREADERDLL_API_ERROR_SIZE
    }

    public enum BarcodeError_AlarmCode {
        NONE = 0x00,
        AXIS_ALARM,
        ERROR_BIT1_ALARM,
        ERROR_BIT2_ALARM,
        EMO_BIT_ALARM,
        VEL_LIMIT_ALARM,
        ACC_LIMIT_ALARM,
        DEC_LIMIT_ALARM,
        POSITIVE_POS_LIMIT_ALARM,
        NEGATIVE_POS_LIMIT_ALARM,
        FOLLOWING_ERROR_ALARM,
        NONE_BARCODE_DATA
    }

    public class WMXBarcodeMotion {
        private WMX3Api m_wmxlib;
        private BarcodeReaderCLRApi m_barcode;
        private BarcodeReaderParam m_param;
        private BarcodeReaderSafetyParam m_safetyParam;
        private BarcodeReaderStatus m_barcodeStatus;

        public BarcodeStatus m_status;

        public WMXBarcodeMotion(string devName) {
            m_wmxlib = new WMX3Api();
            m_wmxlib.CreateDevice(@"C:\Program Files\SoftServo\WMX3", DeviceType.DeviceTypeNormal, 3000);
            m_wmxlib.SetDeviceName(devName);

            m_barcode = new BarcodeReaderCLRApi(m_wmxlib);
            m_param = new BarcodeReaderParam();
            m_safetyParam = new BarcodeReaderSafetyParam();
            m_barcodeStatus = new BarcodeReaderStatus();
            m_status = new BarcodeStatus();

            Thread LocalThread = new Thread(delegate ()
            {
                while (WMX3.IsDeviceValid() && IsDeviceValid()) {
                    UpdateBarcodeMotionStatus();
                    Thread.Sleep(5);
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Name = $"MovenCore_WMXMotion [{devName}]";
            LocalThread.Start();
        }

        private void UpdateBarcodeMotionStatus() {
            m_barcode.GetStatus(ref m_barcodeStatus);

            m_status.m_cmdStatus.m_targetDistance   = m_barcodeStatus.cmd.TargetDistance;
            m_status.m_cmdStatus.m_pos              = m_barcodeStatus.cmd.Pos;
            m_status.m_cmdStatus.m_velocity         = m_barcodeStatus.cmd.Velocity;
            m_status.m_cmdStatus.m_torque           = m_barcodeStatus.cmd.Torque;

            switch (m_barcodeStatus.fb.op) {
                case BarcodeReaderControlState.IDLE:
                    m_status.m_fbStatus.m_op = BarcodeOpState.IDLE;
                    break;

                case BarcodeReaderControlState.SERVO_OFF:
                    m_status.m_fbStatus.m_op = BarcodeOpState.SERVO_OFF;
                    break;

                case BarcodeReaderControlState.POS_CLOSE_LOOP_MOVE:
                    m_status.m_fbStatus.m_op = BarcodeOpState.POS_CLOSE_LOOP_MOVE;
                    break;

                case BarcodeReaderControlState.STOP:
                    m_status.m_fbStatus.m_op = BarcodeOpState.STOP;
                    break;

                case BarcodeReaderControlState.ALARM:
                    m_status.m_fbStatus.m_op = BarcodeOpState.ALARM;
                    break;
            }
            m_status.m_fbStatus.m_commandDistance   = m_barcodeStatus.fb.CommandDistance;
            m_status.m_fbStatus.m_actualDistance    = m_barcodeStatus.fb.ActualDistance;
            m_status.m_fbStatus.m_pos               = m_barcodeStatus.fb.Pos;
            m_status.m_fbStatus.m_rawDistance       = m_barcodeStatus.fb.RawDistance;
            m_status.m_fbStatus.m_targetDistance    = m_barcodeStatus.fb.TargetDistance;
            m_status.m_fbStatus.m_velocity          = m_barcodeStatus.fb.Velocity;
            m_status.m_fbStatus.m_torque            = m_barcodeStatus.fb.Torque;
            m_status.m_fbStatus.m_alarmCode         = m_barcodeStatus.fb.AlarmCode;
        }

        public bool IsDeviceValid() {
            return m_barcode.IsDeviceValid();
        }

        public int SetBarcodeParam(BarcodeParameter parameter) {
            m_param.nAxis           = parameter.m_axis;
            m_param.StartAddr       = parameter.m_startAddr;
            m_param.Size            = parameter.m_size;
            m_param.BarcodeScale    = parameter.m_barcodeScale;
            m_param.HomeOffset      = parameter.m_homeOffset;

            switch (parameter.m_specInType) {
                case BarcodeSpecInType.SpecIn:
                    m_param.SpecInType = BarcodeReaderControlSpecInType.SPECIN;
                    break;

                case BarcodeSpecInType.SpecIn_With_ZeroSpeed:
                    m_param.SpecInType = BarcodeReaderControlSpecInType.SPECIN_WITH_ZEROSPEED;
                    break;

                case BarcodeSpecInType.SpecIn_With_Time:
                    m_param.SpecInType = BarcodeReaderControlSpecInType.SPECIN_WITH_TIME;
                    break;

                case BarcodeSpecInType.SpecIn_With_Time_And_ZeroSpeed:
                    m_param.SpecInType = BarcodeReaderControlSpecInType.SPECIN_WITH_TIME_AND_ZEROSPEED;
                    break;
            }

            m_param.SpecInRange     = parameter.m_specInRange;
            m_param.SpecInTimeSec   = parameter.m_specInTimeSec;

            return m_barcode.SetParam(m_param);
        }

        public BarcodeParameter GetBarcodeParameter() {
            m_barcode.GetParam(ref m_param);

            BarcodeParameter param = new BarcodeParameter();

            param.m_axis = m_param.nAxis;
            param.m_startAddr = m_param.StartAddr;
            param.m_size = m_param.Size;
            param.m_barcodeScale = m_param.BarcodeScale;
            param.m_homeOffset = m_param.HomeOffset;
            switch (m_param.SpecInType) {
                case BarcodeReaderControlSpecInType.SPECIN:
                    param.m_specInType = BarcodeSpecInType.SpecIn;
                    break;

                case BarcodeReaderControlSpecInType.SPECIN_WITH_TIME:
                    param.m_specInType = BarcodeSpecInType.SpecIn_With_Time;
                    break;

                case BarcodeReaderControlSpecInType.SPECIN_WITH_TIME_AND_ZEROSPEED:
                    param.m_specInType = BarcodeSpecInType.SpecIn_With_Time_And_ZeroSpeed;
                    break;

                case BarcodeReaderControlSpecInType.SPECIN_WITH_ZEROSPEED:
                    param.m_specInType = BarcodeSpecInType.SpecIn_With_ZeroSpeed;
                    break;
            }
            param.m_specInRange = m_param.SpecInRange;
            param.m_specInTimeSec = m_param.SpecInTimeSec;

            return param;
        }

        public int SetBarcodeSafetyParam(BarcodeSafetyParameter parameter) {
            m_safetyParam.Error1_StartAddr              = parameter.m_error1_StartAddr;
            m_safetyParam.Error1_Bit                    = parameter.m_error1_Bit;
            m_safetyParam.Error2_StartAddr              = parameter.m_error2_StartAddr;
            m_safetyParam.Error2_Bit                    = parameter.m_error2_Bit;
            m_safetyParam.Error3_StartAddr              = parameter.m_error3_StartAddr;
            m_safetyParam.Error3_Bit                    = parameter.m_error3_Bit;
            m_safetyParam.Error4_StartAddr              = parameter.m_error4_StartAddr;
            m_safetyParam.Error4_Bit                    = parameter.m_error4_Bit;
            m_safetyParam.EMO1_StartAddr                = parameter.m_EMO1_StartAddr;
            m_safetyParam.EMO1_Bit                      = parameter.m_EMO1_Bit;
            m_safetyParam.EMO2_StartAddr                = parameter.m_EMO2_StartAddr;
            m_safetyParam.EMO2_Bit                      = parameter.m_EMO2_Bit;
            m_safetyParam.EMO3_StartAddr                = parameter.m_EMO3_StartAddr;
            m_safetyParam.EMO3_Bit                      = parameter.m_EMO3_Bit;
            m_safetyParam.EMO4_StartAddr                = parameter.m_EMO4_StartAddr;
            m_safetyParam.EMO4_Bit                      = parameter.m_EMO4_Bit;
            m_safetyParam.AlarmStopDecTimeSec           = parameter.m_alarmStopDecTimeSec;
            m_safetyParam.StopDecTimeSec                = parameter.m_stopDecTimeSec;
            m_safetyParam.FollowingError                = parameter.m_followingError;
            m_safetyParam.VelocityLimit                 = parameter.m_velocityLimit;
            m_safetyParam.AccLimit                      = parameter.m_accLimit;
            m_safetyParam.DecLimit                      = parameter.m_decLimit;
            m_safetyParam.UseBarcodePositiveLimit       = parameter.m_useBarcodePositiveLimit;
            m_safetyParam.BarcodePositiveLimit          = parameter.m_barcodePositiveLimit;
            m_safetyParam.UseBarcodeNegativeLimit       = parameter.m_useBarcodeNegativeLimit;
            m_safetyParam.BarcodeNegativeLimit          = parameter.m_barcodeNegativeLimit;

            return m_barcode.SetSafetyParam(m_safetyParam);
        }

        public BarcodeSafetyParameter GetBarcodeSafetyParameter() {
            m_barcode.GetSafetyParam(ref m_safetyParam);

            BarcodeSafetyParameter param = new BarcodeSafetyParameter();

            param.m_error1_StartAddr            = m_safetyParam.Error1_StartAddr;
            param.m_error1_Bit                  = m_safetyParam.Error1_Bit;
            param.m_error2_StartAddr            = m_safetyParam.Error2_StartAddr;
            param.m_error2_Bit                  = m_safetyParam.Error2_Bit;
            param.m_error3_StartAddr            = m_safetyParam.Error3_StartAddr;
            param.m_error3_Bit                  = m_safetyParam.Error3_Bit;
            param.m_error4_StartAddr            = m_safetyParam.Error4_StartAddr;
            param.m_error4_Bit                  = m_safetyParam.Error4_Bit;
            param.m_EMO1_StartAddr              = m_safetyParam.EMO1_StartAddr;
            param.m_EMO1_Bit                    = m_safetyParam.EMO1_Bit;
            param.m_EMO2_StartAddr              = m_safetyParam.EMO2_StartAddr;
            param.m_EMO2_Bit                    = m_safetyParam.EMO2_Bit;
            param.m_EMO3_StartAddr              = m_safetyParam.EMO3_StartAddr;
            param.m_EMO3_Bit                    = m_safetyParam.EMO3_Bit;
            param.m_EMO4_StartAddr              = m_safetyParam.EMO4_StartAddr;
            param.m_EMO4_Bit                    = m_safetyParam.EMO4_Bit;
            param.m_alarmStopDecTimeSec         = m_safetyParam.AlarmStopDecTimeSec;
            param.m_stopDecTimeSec              = m_safetyParam.StopDecTimeSec;
            param.m_followingError              = m_safetyParam.FollowingError;
            param.m_velocityLimit               = m_safetyParam.VelocityLimit;
            param.m_accLimit                    = m_safetyParam.AccLimit;
            param.m_decLimit                    = m_safetyParam.DecLimit;
            param.m_useBarcodePositiveLimit     = m_safetyParam.UseBarcodePositiveLimit;
            param.m_barcodePositiveLimit        = m_safetyParam.BarcodePositiveLimit;
            param.m_useBarcodeNegativeLimit     = m_safetyParam.UseBarcodeNegativeLimit;
            param.m_barcodeNegativeLimit        = m_safetyParam.BarcodeNegativeLimit;

            return param;
        }

        public int StartFullClosedMotion(BarcodeClosedLoopCommand command) {
            BarcodePosCloseLoopMotionCommand cmd = new BarcodePosCloseLoopMotionCommand();
            cmd.TargetDistance      = command.m_targetDistance;
            cmd.Velocity            = command.m_velocity;
            cmd.Acc                 = command.m_acc;
            cmd.Dec                 = command.m_dec;
            cmd.Pgain               = command.m_pGain;
            cmd.Igain               = command.m_iGain;

            return m_barcode.PosCloseLoopStart(cmd);
        }

        public void StopFullClosed() {
            m_barcode.Stop();
        }

        public void AlarmReset() {
            m_barcode.AlarmReset();
        }

        public string GetControlAlrmString(int alarmCode) {
            return BarcodeReaderCLRApi.ControlAlarmToString(alarmCode);
        }

        public string GetErrorToString(int errCode) {
            return BarcodeReaderCLRApi.ErrorToString(errCode);
        }
    }
}
