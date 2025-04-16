using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Synustech.G_Var;

namespace Synustech.Conveyor
{
    partial class Conveyor
    {
        public void WMX_Motion_Axis_ServoOn(int nAxis)
        {
            int err = m_WMXMotion.ServoOn(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-ServoOn", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_ServoOff(int nAxis)
        {
            int err = m_WMXMotion.ServoOff(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-ServoOff", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_StartHoming(int nAxis)
        {
            int err = m_WMXMotion.HomeStart(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-StartHome", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_StartJog_POS(int nAxis, double velocity, double acc, double dec)
        {
            AxisProfile axisprofile = new AxisProfile();
            axisprofile.m_axis = nAxis;
            axisprofile.m_velocity = velocity;
            axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
            axisprofile.m_acc = acc;
            axisprofile.m_dec = dec;
            axisprofile.m_jerkRatio = 1.0;

            int err = m_WMXMotion.JogMove(axisprofile);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-JogMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_StartJog_NEG(int nAxis, double velocity, double acc, double dec)
        {
            AxisProfile axisprofile = new AxisProfile();
            axisprofile.m_axis = nAxis;
            axisprofile.m_velocity = -velocity;
            axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
            axisprofile.m_acc = acc;
            axisprofile.m_dec = dec;
            axisprofile.m_jerkRatio = 1.0;

            int err = m_WMXMotion.JogMove(axisprofile);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-JogMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_InchingMove(int nAxis, double inching, double velocity, double acc, double dec)
        {
            AxisProfile axisprofile = new AxisProfile();
            axisprofile.m_axis = nAxis;
            axisprofile.m_dest = inching;
            axisprofile.m_velocity = velocity;
            axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
            axisprofile.m_acc = acc;
            axisprofile.m_dec = dec;
            axisprofile.m_jerkRatio = 1.0;

            int err = m_WMXMotion.RelativeMove(axisprofile);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-InchingMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_TargetMove(int nAxis, double dest, double velocity, double acc, double dec)
        {
            AxisProfile axisprofile = new AxisProfile();
            axisprofile.m_axis = nAxis;
            axisprofile.m_dest = dest;
            axisprofile.m_velocity = velocity;
            axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
            axisprofile.m_acc = acc;
            axisprofile.m_dec = dec;
            axisprofile.m_jerkRatio = 1.0;

            int err = m_WMXMotion.AbsoluteMove(axisprofile);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AbsoluteMove", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_Stop(int nAxis)
        {
            int err = m_WMXMotion.StopServo(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-StopServo", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_AlarmClear(int nAxis)
        {
            int err = m_WMXMotion.AlarmClear(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-AlarmClear", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_EStop(int nAxis)
        {
            int err = m_WMXMotion.EmergencyStop(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-EmergencyStop", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_HomeDoneClear(int nAxis)
        {
            int err = m_WMXMotion.HomeDoneClear(nAxis);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-HomeDoneClear", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
        public void WMX_Motion_Axis_SetHomeDone(int nAxis)
        {
            int err = m_WMXMotion.SetHomeDoneFlag(nAxis, true);

            if (err != WMXParam.ErrorCode_None)
                LogMsg.AddWMXLog(LogMsg.LogLevel.Error, "WMX3CoreMotion-SetHomeDone", $"Axis:{nAxis} {WMX3.ErrorCodeToString(err)}");
        }
    }
}
