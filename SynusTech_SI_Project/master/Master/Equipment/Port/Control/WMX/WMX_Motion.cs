using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovenCore;

namespace Master.Equipment.Port
{
    /// <summary>
    /// 인터락등을 거친 최종 WMX 함수 호출 영역
    /// </summary>
    partial class Port
    {
        public bool InitWMXParam()
        {
            var eAxisGroup = Enum.GetValues(typeof(PortAxis));
            foreach (PortAxis ePortAxis in eAxisGroup)
            {
                if (GetMotionParam().IsValidServo(ePortAxis))
                {
                    bool bSuccess = WMXParam_Apply_ProgToWMXEngine(GetMotionParam().GetServoAxisNum(ePortAxis), GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).WMXParam);

                    if (!bSuccess)
                        return false;
                    else
                    {
                        WMXMotion.AxisParameter FileWMXParam = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).WMXParam;

                        if (!FileWMXParam.m_absEncoderMode)
                        {
                            continue;
                        }

                        bool IsRotaryAxis = GetMotionParam().IsRotaryAxis(ePortAxis);
                        double CMDEncorderPos = ServoCtrl_GetCMDEncorderPos(ePortAxis);
                        double EncorderHomeOffset = WMXPosToProgramUnit(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, FileWMXParam.m_absEncoderHomeOffset * FileWMXParam.m_gearRatioDen / FileWMXParam.m_gearRatioNum);
                        double HomeShiftDistance = ServoCtrl_GetHomeShiftDistance(ePortAxis);
                        int WMXDirection = m_PortMotionParameter.GetShuttleCtrl_ServoParam(ePortAxis).WMXParam.m_motorDirection == WMXParam.m_motorDirection.Positive ? 1 : -1;
                        double WMXPos = ProgramUnitToWMXPos(IsRotaryAxis ? AxisType.Rotary : AxisType.Linear, ((CMDEncorderPos - EncorderHomeOffset) * WMXDirection) + HomeShiftDistance);


                        ServoCtrl_SetCommandPos(ePortAxis, WMXPos);
                        ServoCtrl_SetHomeDone(ePortAxis);
                    }
                }
            }

            return true;
        }
        public void RefreshWMXParam()
        {
            var eAxisGroup = Enum.GetValues(typeof(PortAxis));
            foreach (PortAxis ePortAxis in eAxisGroup)
            {
                if (GetMotionParam().IsValidServo(ePortAxis))
                {
                    WMXParam_Load_EngineToProg(GetMotionParam().GetServoAxisNum(ePortAxis), GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).WMXParam);
                }
            }
        }
        public bool WMXParam_Apply_ProgToWMXEngine(int nAxis, WMXMotion.AxisParameter _AxisParam)
        {
            m_WMXMotion.m_axisParameter[nAxis].SetParam(_AxisParam);
            int err = m_WMXMotion.AxisParameterApplyToEngine(nAxis, m_WMXMotion.m_axisParameter[nAxis]);

            if (err != WMXParam.ErrorCode_None)
            {
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AxisParameterApplyToEngine", $"Axis: {nAxis} / Error: {WMX3.ErrorCodeToString(err)}");
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.WMXParameterApplyFail, $"Axis: {nAxis} Parameter");
                return false;
            }
            else
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.WMXParameterApplySuccess, $"Axis: {nAxis} Parameter");
            }

            return true;
        }
        public void WMXParam_Load_EngineToProg(int nAxis, WMXMotion.AxisParameter _AxisParam)
        {
            int err = m_WMXMotion.GetAxisParameterFromEngine(nAxis, ref _AxisParam);

            if (err != WMXParam.ErrorCode_None)
            {
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-GetAxisParameterFromEngine", $"Axis: {nAxis} / Error: {WMX3.ErrorCodeToString(err)}");
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.WMXParameterGetFail, $"Axis: {nAxis} Parameter");
            }
            else
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.WMXParameterGetSuccess, $"Axis: {nAxis} Parameter");
        }

        private void WMX_Motion_Axis_ServoOn(int nAxis)
        {
            int err = m_WMXMotion.ServoOn(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-ServoOn", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_ServoOff(int nAxis)
        {
            int err = m_WMXMotion.ServoOff(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-ServoOff", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_StartHoming(int nAxis)
        {
            int err = m_WMXMotion.HomeStart(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-StartHome", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_StartJog(int nAxis, double velocity, double acc, double dec)
        {
            AxisProfile axisprofile     = new AxisProfile();
            axisprofile.m_axis          = nAxis;
            axisprofile.m_velocity      = velocity;
            axisprofile.m_profileType   = WMXParam.m_profileType.JerkRatio;
            axisprofile.m_acc           = acc;
            axisprofile.m_dec           = dec;
            axisprofile.m_jerkRatio     = 1.0;

            int err = m_WMXMotion.JogMove(axisprofile);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-JogMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_InchingMove(int nAxis, double inching, double velocity, double acc, double dec)
        {
            AxisProfile axisprofile     = new AxisProfile();
            axisprofile.m_axis          = nAxis;
            axisprofile.m_dest          = inching;
            axisprofile.m_velocity      = velocity;
            axisprofile.m_profileType   = WMXParam.m_profileType.JerkRatio;
            axisprofile.m_acc           = acc;
            axisprofile.m_dec           = dec;
            axisprofile.m_jerkRatio     = 1.0;

            int err = m_WMXMotion.RelativeMove(axisprofile);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-InchingMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_TargetMove(int nAxis, double dest, double velocity, double acc, double dec)
        {
            AxisProfile axisprofile     = new AxisProfile();
            axisprofile.m_axis          = nAxis;
            axisprofile.m_dest          = dest;
            axisprofile.m_velocity      = velocity;
            axisprofile.m_profileType   = WMXParam.m_profileType.JerkRatio;
            axisprofile.m_acc           = acc;
            axisprofile.m_dec           = dec;
            axisprofile.m_jerkRatio     = 1.0;

            int err = m_WMXMotion.AbsoluteMove(axisprofile);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_Stop(int nAxis)
        {
            int err = m_WMXMotion.StopServo(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-StopServo", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_AlarmClear(int nAxis)
        {
            int err = m_WMXMotion.AlarmClear(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AlarmClear", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_EStop(int nAxis)
        {
            int err = m_WMXMotion.EmergencyStop(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-EmergencyStop", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_HomeDoneClear(int nAxis)
        {
            int err = m_WMXMotion.HomeDoneClear(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-HomeDoneClear", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_SetCommandPos(int nAxis, double NewPos)
        {
            int err = m_WMXMotion.SetCommandPos(nAxis, NewPos);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-SetCommandPos", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        private void WMX_Motion_Axis_SetHomeDone(int nAxis)
        {
            int err = m_WMXMotion.SetHomeDoneFlag(nAxis, true);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-SetHomeDone", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
    }
}
