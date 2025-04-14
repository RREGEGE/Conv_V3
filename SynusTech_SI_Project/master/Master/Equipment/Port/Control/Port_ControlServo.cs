using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.Math;
using MovenCore;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_ControlServo.cs 는 Port의 축 타입 중 Servo 축 관련 제어 기능이 작성된 페이지
    /// </summary>
    partial class Port
    {
        /// <summary>
        /// 평균 토크량 산출 클래스
        /// </summary>
        Average<double> X_AverageTorque = new Average<double>();
        Average<double> Z_AverageTorque = new Average<double>();
        Average<double> T_AverageTorque = new Average<double>();

        /// <summary>
        /// 최대 토크량 산출 클래스
        /// </summary>
        Max<double> X_MaxTorque = new Max<double>();
        Max<double> Z_MaxTorque = new Max<double>();
        Max<double> T_MaxTorque = new Max<double>();

        /// <summary>
        /// 축이 Servo Type인지 체크
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="bInsertLog"></param>
        /// <returns></returns>
        private bool Check_ServoType(PortAxis ePortAxis, bool bInsertLog = false)
        {
            if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].eAxisCtrlType != AxisCtrlType.Servo)
            {
                if(bInsertLog)
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidControlType, $"Control Type: {GetMotionParam().Ctrl_Axis[(int)ePortAxis].eAxisCtrlType}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 축의 번호가 유효한 번호인지 체크 0~127
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="bInsertLog"></param>
        /// <returns></returns>
        private bool Check_ValidAxisNum(PortAxis ePortAxis, bool bInsertLog = false)
        {
            if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].servoParam.AxisNum < 0 || GetMotionParam().Ctrl_Axis[(int)ePortAxis].servoParam.AxisNum >= 128)
            {
                if (bInsertLog)
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidParameter, $"Axis Number: {GetMotionParam().Ctrl_Axis[(int)ePortAxis].servoParam.AxisNum}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 앰프가 오프라인 상태인지 체크 (서보 오프와는 다름)
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="bInsertLog"></param>
        /// <returns></returns>
        private bool Check_IsAmpOffline(PortAxis ePortAxis, bool bInsertLog = false)
        {
            int nAxis           = GetMotionParam().GetServoAxisNum(ePortAxis);
            bool IsAmpOffline   = m_WMXMotion.IsAmpOffline(nAxis);

            if (IsAmpOffline && bInsertLog)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortServoAmpOffline, $"Axis Type: {ePortAxis}, Axis Number: {IsAmpOffline}");

            return IsAmpOffline;
        }
        
        /// <summary>
        /// Axis Command
        /// </summary>
        /// <param name="ePortAxis"></param>
        public void ServoCtrl_ServoOn(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
            WMX_Motion_Axis_ServoOn(nAxis);
        }
        public void ServoCtrl_ServoOff(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
            WMX_Motion_Axis_ServoOff(nAxis);
        }
        public void ServoCtrl_StartHoming(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);

            if(!ServoStatus.m_servoRun)
                WMX_Motion_Axis_StartHoming(nAxis);
        }
        public void ServoCtrl_StartJog(PortAxis ePortAxis, float velocity, bool bDirection)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var ServoParam      = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);
            var ServoStatus     = m_WMXAxisStatus[(int)ePortAxis];
            bool IsRotaryAxis   = GetMotionParam().IsRotaryAxis(ePortAxis);
            
            int nAxis       = ServoParam.AxisNum;
            double Conv_Vel = ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)velocity) * (bDirection ? 1.0 : -1.0);
            double Conv_Acc = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)ServoParam.Manual_Acc));
            double Conv_Dec = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)ServoParam.Manual_Dec));

            //Conv_Vel *= (double)(GetUIManualSpeedRatio() / 100.0); //속도는 적용하지 않음
            Conv_Acc *= (double)(GetUIManualSpeedRatio() / 100.0);
            Conv_Dec *= (double)(GetUIManualSpeedRatio() / 100.0);

            if (!ServoStatus.m_servoRun)
                WMX_Motion_Axis_StartJog(nAxis, Conv_Vel, Conv_Acc, Conv_Dec);
        }
        public void ServoCtrl_StartInchingMove(PortAxis ePortAxis, double inching, bool bDirection)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var ServoParam      = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);
            var ServoStatus     = m_WMXAxisStatus[(int)ePortAxis];
            bool IsRotaryAxis   = GetMotionParam().IsRotaryAxis(ePortAxis);

            int nAxis           = ServoParam.AxisNum;
            double Conv_Dest    = ProgramUnitToWMXPos(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, inching) * (bDirection ? 1.0 : -1.0);
            double Conv_Vel     = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)ServoParam.Manual_Speed));
            double Conv_Acc     = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)ServoParam.Manual_Acc));
            double Conv_Dec     = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)ServoParam.Manual_Dec));

            Conv_Vel *= (double)(GetUIManualSpeedRatio() / 100.0);
            Conv_Acc *= (double)(GetUIManualSpeedRatio() / 100.0);
            Conv_Dec *= (double)(GetUIManualSpeedRatio() / 100.0);

            if (!ServoStatus.m_servoRun)
                WMX_Motion_Axis_InchingMove(nAxis, Conv_Dest, Conv_Vel, Conv_Acc, Conv_Dec);
        }
        public void ServoCtrl_StartTargetMove(PortAxis ePortAxis, double target)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var Param           = GetMotionParam();
            var ServoParam      = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);
            var ServoStatus     = m_WMXAxisStatus[(int)ePortAxis];
            bool IsRotaryAxis   = GetMotionParam().IsRotaryAxis(ePortAxis);
            bool IsAutoRun      = IsAutoControlRun() ? true : false;
            double SpeedRatio   = IsAutoRun ? (Param.AutoRun_Ratio / 100.0) : (GetUIManualSpeedRatio() / 100.0);

            int nAxis           = ServoParam.AxisNum;
            double Conv_Dest    = ProgramUnitToWMXPos(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, target);
            double Conv_Vel     = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)(IsAutoRun ? ServoParam.AutoRun_Speed :ServoParam.Manual_Speed)));
            double Conv_Acc     = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)(IsAutoRun ? ServoParam.AutoRun_Acc : ServoParam.Manual_Acc)));
            double Conv_Dec     = Math.Abs(ProgramUnitToWMXVel(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, (double)(IsAutoRun ? ServoParam.AutoRun_Dec : ServoParam.Manual_Dec)));

            Conv_Vel *= SpeedRatio;
            Conv_Acc *= SpeedRatio;
            Conv_Dec *= SpeedRatio;

            if (!ServoStatus.m_servoRun)
                WMX_Motion_Axis_TargetMove(nAxis, Conv_Dest, Conv_Vel, Conv_Acc, Conv_Dec);
        }
        public void ServoCtrl_MotionStop(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var ServoParam = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);
            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            int nAxis = ServoParam.AxisNum;

            //if (ServoStatus.m_servoRun)
                WMX_Motion_Axis_Stop(nAxis);
        }
        public void ServoCtrl_AlarmClear(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var ServoParam = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);

            int nAxis = ServoParam.AxisNum;

            WMX_Motion_Axis_AlarmClear(nAxis);
        }
        public void ServoCtrl_MotionEStop(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var ServoParam = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);

            int nAxis = ServoParam.AxisNum;

            WMX_Motion_Axis_EStop(nAxis);
        }
        public void ServoCtrl_HomeDoneClear(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis, true))
                return;

            if (!Check_ValidAxisNum(ePortAxis, true))
                return;

            if (Check_IsAmpOffline(ePortAxis, true))
                return;

            var ServoParam = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);

            int nAxis = ServoParam.AxisNum;

            WMX_Motion_Axis_HomeDoneClear(nAxis);
        }

        /// <summary>
        /// Set Command Pos는 사용 주의 (타겟 위치 값을 바꿔줌으로써 모터 발산 가능성 있음 , Moven Core에서는 Set CMD Pos와 Fd Pos를 같이 변경)
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="NewPos"></param>
        public void ServoCtrl_SetCommandPos(PortAxis ePortAxis, double NewPos)
        {
            if (!Check_ServoType(ePortAxis))
                return;

            if (!Check_ValidAxisNum(ePortAxis))
                return;

            if (Check_IsAmpOffline(ePortAxis))
                return;

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
            WMX_Motion_Axis_SetCommandPos(nAxis, NewPos);
        }

        public void ServoCtrl_SetHomeDone(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return;

            if (!Check_ValidAxisNum(ePortAxis))
                return;

            if (Check_IsAmpOffline(ePortAxis))
                return;

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
            WMX_Motion_Axis_SetHomeDone(nAxis);
        }

        /// <summary>
        /// Axis Status
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <returns></returns>
        public bool ServoCtrl_GetAxisMoveEnable(PortAxis ePortAxis, bool bFromAutoStep)
        {
            if (!Check_ServoType(ePortAxis, !bFromAutoStep))
                return false;

            if (!Check_ValidAxisNum(ePortAxis, !bFromAutoStep))
                return false;

            if (Check_IsAmpOffline(ePortAxis, !bFromAutoStep))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bServoOn   = ServoStatus.m_servoOn;
            bool bAlarm     = ServoStatus.m_alarm;
            bool bHomeDone  = ServoStatus.m_homeDone;
            bool bBusy      = ServoStatus.m_servoRun;

            bool bMoveEnable = bServoOn && !bAlarm && bHomeDone && !bBusy;
            if (!bMoveEnable && !bFromAutoStep)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PortStatusInfo,
                    $"Port Is Not Move Enable State -> Servo: {bServoOn}, Alarm: {bAlarm}, HomeDone: {bHomeDone}, Busy: {bBusy}");

            return bMoveEnable;
        }
        public bool ServoCtrl_GetJoggingStatus(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return false;

            if (!Check_ValidAxisNum(ePortAxis))
                return false;

            if (Check_IsAmpOffline(ePortAxis))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bJogging = ServoStatus.m_Joging;

            return bJogging;
        }
        public bool ServoCtrl_GetHomingStatus(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return false;

            if (!Check_ValidAxisNum(ePortAxis))
                return false;

            if (Check_IsAmpOffline(ePortAxis))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bHoming = ServoStatus.m_homing;

            return bHoming;
        }
        public bool ServoCtrl_GetBusy(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return false;

            if (!Check_ValidAxisNum(ePortAxis))
                return false;

            if (Check_IsAmpOffline(ePortAxis))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bBusy = ServoStatus.m_servoRun;

            return bBusy;
        }
        public bool ServoCtrl_GetServoOn(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return false;

            if (!Check_ValidAxisNum(ePortAxis))
                return false;

            if (Check_IsAmpOffline(ePortAxis))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bServoOn = ServoStatus.m_servoOn;

            return bServoOn;
        }
        public bool ServoCtrl_GetHomeDone(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return false;

            if (!Check_ValidAxisNum(ePortAxis))
                return false;

            if (Check_IsAmpOffline(ePortAxis))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bHomeDone = ServoStatus.m_homeDone;

            return bHomeDone;
        }
        public bool ServoCtrl_GetAlarmStatus(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return false;

            if (!Check_ValidAxisNum(ePortAxis))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bAlarm = ServoStatus.m_alarm;

            return bAlarm;
        }
        public bool ServoCtrl_GetSoftLimitStatus(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return false;

            if (!Check_ValidAxisNum(ePortAxis))
                return false;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool bPositiveLimit = ServoStatus.m_pLimitSoft;
            bool bNegativeLimit = ServoStatus.m_nLimitSoft;

            return bPositiveLimit || bNegativeLimit;
        }

        public float ServoCtrl_GetProfileTargetPosition(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0.0f;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0.0f;

            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            float ProfileTargetPosition = (float)WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_profileTargetPos);

            return ProfileTargetPosition;
        }
        public float ServoCtrl_GetTargetPosition(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0.0f;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0.0f;

            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            float TargetPosition = (float)WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_cmdPos);

            return TargetPosition;
        }
        public float ServoCtrl_GetCurrentPosition(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0.0f;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0.0f;

            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            float ActualPosition = (float)WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_actualPos);

            return ActualPosition;
        }
        public double ServoCtrl_GetCMDEncorderPos(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0.0f;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0.0f;

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            WMXMotion.AxisParameter EngineAxisParameter = new WMXMotion.AxisParameter();

            m_WMXMotion.GetAxisParameterFromEngine(nAxis, ref EngineAxisParameter);

            double EncorderCMDPosition = WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_cmdEncoder * EngineAxisParameter.m_gearRatioDen / EngineAxisParameter.m_gearRatioNum);

            return EncorderCMDPosition;
        }

        public double ServoCtrl_GetCMDEncorderOffset(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0.0f;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0.0f;

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);

            WMXMotion.AxisParameter EngineAxisParameter = new WMXMotion.AxisParameter();
            
            m_WMXMotion.GetAxisParameterFromEngine(nAxis, ref EngineAxisParameter);

            double EncorderHomeOffset = WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, EngineAxisParameter.m_absEncoderHomeOffset * EngineAxisParameter.m_gearRatioDen / EngineAxisParameter.m_gearRatioNum);

            return EncorderHomeOffset;
        }
        public double ServoCtrl_GetHomeShiftDistance(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0.0f;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0.0f;

            int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);

            WMXMotion.AxisParameter EngineAxisParameter = new WMXMotion.AxisParameter();

            m_WMXMotion.GetAxisParameterFromEngine(nAxis, ref EngineAxisParameter);

            double ShiftDistance = WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, EngineAxisParameter.m_homeShiftDistance);

            return ShiftDistance;
        }

        public float ServoCtrl_GetTargetSpeed(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0.0f;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0.0f;

            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            float TargetSpeed = (float)WMXVelToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_cmdVelocity);

            return TargetSpeed;
        }
        public short ServoCtrl_GetCurrentSpeed(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0;

            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            short ActualSpeed = (short)WMXVelToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_actualVelocity);

            return ActualSpeed;
        }
        public short ServoCtrl_GetCurrentTorque(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            short CurrentTorque = (short)Math.Abs(ServoStatus.m_actualTorque);

            return CurrentTorque;
        }
        public int ServoCtrl_GetAxisAlarmCode(PortAxis ePortAxis)
        {
            if (!Check_ServoType(ePortAxis))
                return 0;

            if (!Check_ValidAxisNum(ePortAxis))
                return 0;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];

            int AlarmCode = ServoStatus.m_alarmCode;

            return AlarmCode;
        }

        /// <summary>
        /// Servo Status to Bit, Word Map
        /// </summary>
        private void ServoCtrl_BitWordUpdate(PortAxis ePortAxis)
        {
            if (!GetMotionParam().IsValidServo(ePortAxis))
                return;

            var ServoStatus = m_WMXAxisStatus[(int)ePortAxis];
            bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);

            double CurrentPos = WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_actualPos);
            double TargetPos = WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_cmdPos);
            double CurrentSpeed = WMXVelToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ServoStatus.m_actualVelocity);
            double CurrentTorque = Math.Abs(ServoStatus.m_actualTorque);

            Motion_CurrentPosition(ePortAxis, CurrentPos);
            Motion_TargetPosition(ePortAxis, TargetPos);
            Motion_CurrentSpeed(ePortAxis, CurrentSpeed);
            Motion_CurrentTorque(ePortAxis, CurrentTorque);

            if (IsAutoControlRun() || IsAutoManualCycleRun())
            {
                ServoCtrl_InsertAxisTorque(ePortAxis, (short)CurrentTorque);
                Motion_PeakTorque(ePortAxis, X_MaxTorque.Result());
                Motion_AvrTorque(ePortAxis, X_AverageTorque.Result());
            }

            bool bBusy = ServoStatus.m_servoRun;
            bool bNOT = ServoStatus.m_nLimit;
            bool bPOT = ServoStatus.m_pLimit;
            bool bHome = ServoStatus.m_origin;
            bool bHomeDone = ServoStatus.m_homeDone;

            if (Status_EStop && bBusy)
                ServoCtrl_MotionEStop(ePortAxis);

            //개별
            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Sensor_X_Axis_NOT = bNOT;
                    Sensor_X_Axis_POT = bPOT;
                    Sensor_X_Axis_HOME = bHome;
                    Sensor_X_Axis_Busy = bBusy;
                    Sensor_X_Axis_OriginOK = bHomeDone;
                    break;
                case PortAxis.Shuttle_Z:
                    Sensor_Z_Axis_NOT = bNOT;
                    Sensor_Z_Axis_POT = bPOT;
                    Sensor_Z_Axis_HOME = bHome;
                    Sensor_Z_Axis_Busy = bBusy;
                    Sensor_Z_Axis_OriginOK = bHomeDone;
                    break;
                case PortAxis.Shuttle_T:
                    Sensor_T_Axis_NOT = bNOT;
                    Sensor_T_Axis_POT = bPOT;
                    Sensor_T_Axis_HOME = bHome;
                    Sensor_T_Axis_Busy = bBusy;
                    Sensor_T_Axis_OriginOK = bHomeDone;
                    Sensor_T_Axis_0DegSensor = bHome;
                    break;
                default:
                    bPortAxisBusy[(int)ePortAxis] = bBusy;
                    break;
            }
        }

        /// <summary>
        /// 토크 값 연산
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="value"></param>
        private void ServoCtrl_InsertAxisTorque(PortAxis ePortAxis, short value)
        {
            if (ePortAxis == PortAxis.Shuttle_X)
            {
                X_AverageTorque.Insert(value);
                X_MaxTorque.Insert(value);
            }
            else if (ePortAxis == PortAxis.Shuttle_X)
            {
                Z_AverageTorque.Insert(value);
                Z_MaxTorque.Insert(value);
            }
            else if (ePortAxis == PortAxis.Shuttle_X)
            {
                T_AverageTorque.Insert(value);
                T_MaxTorque.Insert(value);
            }
        }

        /// <summary>
        /// 측정 토크 값 초기화
        /// </summary>
        public void ServoCtrl_ClearAxisTorqueCal()
        {
            X_AverageTorque.Clear();
            Z_AverageTorque.Clear();
            T_AverageTorque.Clear();

            X_MaxTorque.Clear();
            Z_MaxTorque.Clear();
            T_MaxTorque.Clear();
        }

        /// <summary>
        /// X축 티칭 위치 제어 모션
        /// </summary>
        /// <param name="eTeaching_X_Pos"></param>
        /// <param name="IsAutoRun"></param>
        private void CMD_X_Axis_Move_To_TeachingPos(Teaching_X_Pos eTeaching_X_Pos, bool IsAutoRun)
        {
            int nAxis = GetMotionParam().GetServoAxisNum(PortAxis.Shuttle_X);
            int SpeedPercent = IsAutoRun ? GetMotionParam().AutoRun_Ratio : GetUIManualSpeedRatio();
            
            if (nAxis != -1)
            {
                double Target;
                if (eTeaching_X_Pos == Teaching_X_Pos.OP_Pos)
                    Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos);
                else if (eTeaching_X_Pos == Teaching_X_Pos.Wait_Pos)
                    Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Wait_Pos);
                else if (eTeaching_X_Pos == Teaching_X_Pos.MGV_LP_Pos || eTeaching_X_Pos == Teaching_X_Pos.Equip_LP_Pos)
                    Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos));
                else
                    return;

                AxisProfile axisprofile = new AxisProfile();
                axisprofile.m_axis = nAxis;
                axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, Target);
                axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_X).AutoRun_Speed : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_X).Manual_Speed) * ((double)SpeedPercent / 100.0);
                axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_X).AutoRun_Acc : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_X).Manual_Acc) * ((double)SpeedPercent / 100.0));
                axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_X).AutoRun_Dec : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_X).Manual_Dec) * ((double)SpeedPercent / 100.0));
                axisprofile.m_jerkRatio = 0.75;

                if (!ServoCtrl_GetBusy(PortAxis.Shuttle_X))
                {
                    int err = m_WMXMotion.AbsoluteMove(axisprofile);

                    if (err != WMXParam.ErrorCode_None)
                        LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
                }
            }
        }
        
        /// <summary>
        /// Z축 티칭 위치 제어 모션
        /// </summary>
        /// <param name="eTeaching_Z_Pos"></param>
        /// <param name="IsAutoRun"></param>
        private void CMD_Z_Axis_Move_To_TeachingPos(Teaching_Z_Pos eTeaching_Z_Pos, bool IsAutoRun)
        {
            int nAxis = GetMotionParam().GetServoAxisNum(PortAxis.Shuttle_Z);
            int SpeedPercent = IsAutoRun ? GetMotionParam().AutoRun_Ratio : GetUIManualSpeedRatio();

            if (nAxis != -1)
            {
                if (eTeaching_Z_Pos == Teaching_Z_Pos.Up_Pos)
                {
                    if (Motion_CurrentPosition(PortAxis.Shuttle_Z) < GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDistance)
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDistance);
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Speed : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Acc : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Dec : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_endvelocity = axisprofile.m_velocity * (GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDecPercent > 100.0 ? 1.0 : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDecPercent / 100.0);
                        axisprofile.m_jerkRatio = 0.75;

                        TriggerCondition axisTrigger = new TriggerCondition();
                        axisTrigger.m_triggerType = WMXParam.m_triggerType.RemainingTime;
                        axisTrigger.m_triggerAxis = axisprofile.m_axis;
                        axisTrigger.m_triggerValue = 0;

                        if (!ServoCtrl_GetBusy(PortAxis.Shuttle_Z))
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");

                            axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos));

                            err = m_WMXMotion.AbsoluteMove(axisprofile, axisTrigger);

                            if (err != WMXParam.ErrorCode_None)
                                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
                        }
                    }
                    else
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos));
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Speed : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Acc : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Dec : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_jerkRatio = 0.75;

                        if (!ServoCtrl_GetBusy(PortAxis.Shuttle_Z))
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
                        }
                    }
                }
                else if (eTeaching_Z_Pos == Teaching_Z_Pos.Down_Pos)
                {
                    if (Motion_CurrentPosition(PortAxis.Shuttle_Z) > GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDistance)
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDistance);
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Speed : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Acc : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Dec : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_endvelocity = axisprofile.m_velocity * (GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDecPercent > 100.0 ? 1.0 : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).OverrideDecPercent / 100.0);
                        axisprofile.m_jerkRatio = 0.75;

                        TriggerCondition axisTrigger = new TriggerCondition();
                        axisTrigger.m_triggerType = WMXParam.m_triggerType.RemainingTime;
                        axisTrigger.m_triggerAxis = axisprofile.m_axis;
                        axisTrigger.m_triggerValue = 0;

                        if (!ServoCtrl_GetBusy(PortAxis.Shuttle_Z))
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");

                            axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Down_Pos));

                            err = m_WMXMotion.AbsoluteMove(axisprofile, axisTrigger);

                            if (err != WMXParam.ErrorCode_None)
                                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
                        }
                    }
                    else
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Down_Pos));
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Speed : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Acc : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).AutoRun_Dec : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_Z).Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_jerkRatio = 0.75;

                        if (!ServoCtrl_GetBusy(PortAxis.Shuttle_Z))
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
                        }
                    }
                }
                else
                    return;
            }
        }
        
        /// <summary>
        /// T축 티칭 위치 제어 모션
        /// </summary>
        /// <param name="eTeaching_T_Pos"></param>
        /// <param name="IsAutoRun"></param>
        private void CMD_T_Axis_Move_To_TeachingPos(Teaching_T_Pos eTeaching_T_Pos, bool IsAutoRun)
        {
            int nAxis = GetMotionParam().GetServoAxisNum(PortAxis.Shuttle_T);
            int SpeedPercent = IsAutoRun ? GetMotionParam().AutoRun_Ratio : GetUIManualSpeedRatio();

            if (nAxis != -1)
            {
                double Target;
                if (eTeaching_T_Pos == Teaching_T_Pos.Degree0_Pos)
                    Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos);
                else if (eTeaching_T_Pos == Teaching_T_Pos.Degree180_Pos)
                    Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree180_Pos);
                else
                    return;

                AxisProfile axisprofile = new AxisProfile();
                axisprofile.m_axis = nAxis;
                axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Rotary, Target);
                axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Rotary, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_T).AutoRun_Speed : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_T).Manual_Speed) * ((double)SpeedPercent / 100.0);
                axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Rotary, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_T).AutoRun_Acc : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_T).Manual_Acc) * ((double)SpeedPercent / 100.0));
                axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Rotary, IsAutoRun ? GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_T).AutoRun_Dec : GetMotionParam().GetShuttleCtrl_ServoParam(PortAxis.Shuttle_T).Manual_Dec) * ((double)SpeedPercent / 100.0));
                axisprofile.m_jerkRatio = 0.75;

                if (!ServoCtrl_GetBusy(PortAxis.Shuttle_T))
                {
                    int err = m_WMXMotion.AbsoluteMove(axisprofile);

                    if (err != WMXParam.ErrorCode_None)
                        LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
                }
            }
        }
    }
}
