using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovenCore;

namespace Master.Equipment.Port
{
    partial class Port
    {
        public bool InitWMXParam()
        {
            if (GetParam().ePortType >= PortType.MGV_WithAGV && GetParam().ePortType <= PortType.OHT)
            {
                var eAxisGroup = Enum.GetValues(typeof(PortAxis));
                foreach (PortAxis ePortAxis in eAxisGroup)
                {
                    if (GetMotionParam().IsValidServo(ePortAxis))
                    {
                        bool bSuccess = WMXParam_Apply_ProgToWMXEngine(GetMotionParam().GetAxisNum(ePortAxis), GetMotionParam().GetAxisWMXParam(ePortAxis));

                        if (!bSuccess)
                            return false;
                    }
                }
            }
            else if (GetParam().ePortType == PortType.Conveyor)
            {
                var eAxisGroup = Enum.GetValues(typeof(BufferAxis));
                foreach (BufferAxis eBufferAxis in eAxisGroup)
                {
                    if (GetMotionParam().IsValidServo(eBufferAxis))
                    {
                        bool bSuccess = WMXParam_Apply_ProgToWMXEngine(GetMotionParam().GetAxisNum(eBufferAxis), GetMotionParam().GetAxisWMXParam(eBufferAxis));

                        if (!bSuccess)
                            return false;
                    }
                }
            }

            return true;
        }
        public void RefreshWMXParam()
        {
            if (GetParam().ePortType >= PortType.MGV_WithAGV && GetParam().ePortType <= PortType.OHT)
            {
                var eAxisGroup = Enum.GetValues(typeof(PortAxis));
                foreach (PortAxis ePortAxis in eAxisGroup)
                {
                    if (GetMotionParam().IsValidServo(ePortAxis))
                    {
                        WMXParam_Load_EngineToProg(GetMotionParam().GetAxisNum(ePortAxis), GetMotionParam().GetAxisWMXParam(ePortAxis));
                    }
                }
            }
            else if (GetParam().ePortType == PortType.Conveyor)
            {
                var eAxisGroup = Enum.GetValues(typeof(BufferAxis));
                foreach (BufferAxis eBufferAxis in eAxisGroup)
                {
                    if (GetMotionParam().IsValidServo(eBufferAxis))
                    {
                        WMXParam_Load_EngineToProg(GetMotionParam().GetAxisNum(eBufferAxis), GetMotionParam().GetAxisWMXParam(eBufferAxis));
                    }
                }
            }
        }
        public bool WMXParam_Apply_ProgToWMXEngine(int nAxis, WMXMotion.AxisParameter _AxisParam)
        {
            m_WMXMotion.m_axisParameter[nAxis].SetParam(_AxisParam);
            int err = m_WMXMotion.AxisParameterApplyToEngine(nAxis, m_WMXMotion.m_axisParameter[nAxis]);

            if (err != WMXParam.ErrorCode_None)
            {
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                return false;
            }
            else
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.WMX, $"Port [{GetParam().ID}] X-Axis Param Write Success => WMX Axis[{nAxis}]"));

            return true;
        }
        public void WMXParam_Load_EngineToProg(int nAxis, WMXMotion.AxisParameter _AxisParam)
        {
            int err = m_WMXMotion.GetAxisParameterFromEngine(nAxis, ref _AxisParam);

            if (err != WMXParam.ErrorCode_None)
                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
        }

        public WMXMotion.AxisStatus GetAxisStatus(int nAxis)
        {
            if (m_WMXMotion != null && nAxis != -1)
                return m_WMXMotion.m_axisStatus[nAxis];

            return null;
        }

        public int GetWMXAxisNumber(PortAxis ePortAxis)
        {
            return GetMotionParam().GetAxisNum(ePortAxis);
        }
        public int GetWMXAxisNumber(BufferAxis eBufferAxis)
        {
            return GetMotionParam().GetAxisNum(eBufferAxis);
        }

        public bool IsServoType(PortAxis ePortAxis)
        {
            return GetMotionParam().IsServoType(ePortAxis);
        }

        public bool IsValidServo(PortAxis ePortAxis)
        {
            return GetMotionParam().IsValidServo(ePortAxis);
        }

        public float GetAxisTargetPosition(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.X_Axis:
                    return (float)Get_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_TargetPosition_0);
                case PortAxis.Z_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eZAxisType == ZAxisType.Cylinder)
                        return 0;
                    else
                        return (float)Get_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_TargetPosition_0);
                case PortAxis.T_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eTAxisType != TAxisType.Servo)
                        return 0;
                    else
                        return (float)Get_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_TargetPosition_0);
                default:
                    return 0;

            }
        }
        public float GetAxisCurrentPosition(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.X_Axis:
                    return (float)Get_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentPosition_0);
                case PortAxis.Z_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eZAxisType == ZAxisType.Cylinder)
                        return 0;
                    else
                        return (float)Get_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentPosition_0);
                case PortAxis.T_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eTAxisType != TAxisType.Servo)
                        return 0;
                    else
                        return (float)Get_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentPosition_0);
                default:
                    return 0;

            }
        }

        public float GetAxisTargetVelocity(PortAxis ePortAxis)
        {
            if (GetWMXAxisNumber(ePortAxis) != -1)
            {
                var AxisStatus = m_WMXAxisStatus[(int)ePortAxis];

                return (float)Port.WMXVelToProgramUnit(ePortAxis == PortAxis.T_Axis ? AxisType.Rotary : AxisType.Linear, AxisStatus.m_cmdVelocity);
            }

            return 0;
        }
        public float GetAxisProfileTargetPosition(PortAxis ePortAxis)
        {
            if (GetWMXAxisNumber(ePortAxis) != -1)
            {
                var AxisStatus = m_WMXAxisStatus[(int)ePortAxis];

                return (float)Port.WMXPosToProgramUnit(ePortAxis == PortAxis.T_Axis ? AxisType.Rotary : AxisType.Linear, AxisStatus.m_profileTargetPos);
            }

            return 0;
        }
        public int GetAxisAlarmCode(PortAxis ePortAxis)
        {
            if (GetWMXAxisNumber(ePortAxis) != -1)
            {
                var AxisStatus = m_WMXAxisStatus[(int)ePortAxis];

                return AxisStatus.m_alarmCode;
            }

            return 0;
        }
        public float GetAxisCurrentVelocity(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.X_Axis:
                    return (short)Get_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentSpeed);
                case PortAxis.Z_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eZAxisType == ZAxisType.Cylinder)
                        return 0;
                    else
                        return (short)Get_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentSpeed);
                case PortAxis.T_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eTAxisType != TAxisType.Servo)
                        return 0;
                    else
                        return (short)Get_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentSpeed);
                default:
                    return 0;

            }
        }
        public float GetAxisCurrentTorque(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.X_Axis:
                    return (short)Get_Port_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentTorque);
                case PortAxis.Z_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eZAxisType == ZAxisType.Cylinder)
                        return 0;
                    else
                        return (short)Get_Port_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentTorque);
                case PortAxis.T_Axis:
                    if (GetWMXAxisNumber(ePortAxis) == -1 || m_PortMotionParam.eTAxisType != TAxisType.Servo)
                        return 0;
                    else
                        return (short)Get_Port_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentTorque);
                default:
                    return 0;

            }
        }
        public bool GetAxisServoOn(int nAxis)
        {
            return GetAxisStatus(nAxis)?.m_servoOn ?? false;
        }

        public bool GetAxisAlarm(int nAxis)
        {
            return GetAxisStatus(nAxis)?.m_alarm ?? false;
        }
        public bool GetAxisHomeDone(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.X_Axis:
                    return Sensor_X_Axis_Is_OriginOK();
                case PortAxis.Z_Axis:
                    return Sensor_Z_Axis_Is_OriginOK();
                case PortAxis.T_Axis:
                    return Sensor_T_Axis_Is_OriginOK();
                default:
                    return false;

            }
        }
        public bool GetAxisBusy(PortAxis ePortAxis)
        {
            switch (ePortAxis)
            {
                case PortAxis.X_Axis:
                    return Sensor_X_Axis_Is_Busy();
                case PortAxis.Z_Axis:
                    return Sensor_Z_Axis_Is_Busy();
                case PortAxis.T_Axis:
                    return Sensor_T_Axis_Is_Busy();
                default:
                    return false;

            }
        }
        public bool GetAxisHomeMotionState(int nAxis)
        {
            return GetAxisStatus(nAxis)?.m_homing ?? false;
        }
        public bool GetAxisJogMotionState(int nAxis)
        {
            return GetAxisStatus(nAxis)?.m_Joging ?? false;
        }
        public bool GetAxisMoveEnable(int nAxis)
        {
            var AxisStatus = GetAxisStatus(nAxis);
            bool bServoOn = AxisStatus?.m_servoOn ?? false;
            bool bAlarm = AxisStatus?.m_alarm ?? true;
            bool bHomeDone = AxisStatus?.m_homeDone ?? false;
            bool bBusy = AxisStatus?.m_servoRun ?? true;

            return bServoOn && !bAlarm && bHomeDone && !bBusy;
        }

        private void CMD_AxisServoOn(int nAxis)
        {
            if (nAxis != -1)
            {
                int err = m_WMXMotion.ServoOn(nAxis);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }
        private void CMD_AxisServoOff(int nAxis)
        {
            if (nAxis != -1)
            {
                int err = m_WMXMotion.ServoOff(nAxis);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }
        private void CMD_AxisStartHoming(int nAxis)
        {
            if (nAxis != -1)
            {
                int err = m_WMXMotion.HomeStart(nAxis);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }
        private void CMD_AxisStartJog(int nAxis, float velocity, bool bDirection, AxisType eAxisType)
        {
            if (nAxis != -1)
            {
                AxisProfile axisprofile = new AxisProfile();
                axisprofile.m_axis = nAxis;
                axisprofile.m_velocity = ProgramUnitToWMXVel(eAxisType, (double)velocity) * (bDirection ? 1 : -1);
                axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                axisprofile.m_acc = Math.Abs((double)axisprofile.m_velocity * 10.0);
                axisprofile.m_dec = Math.Abs((double)axisprofile.m_velocity * 10.0);
                axisprofile.m_jerkRatio = 0.75;


                int err = m_WMXMotion.JogMove(axisprofile);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }
        private void CMD_AxisInchingMove(int nAxis, float Velocity, float Acc, float Dec, double inching, bool bDirection, AxisType eAxisType)
        {
            var AxisStatus = GetAxisStatus(nAxis);

            if (nAxis != -1 && AxisStatus != null)
            {
                AxisProfile axisprofile         = new AxisProfile();
                axisprofile.m_axis              = nAxis;
                axisprofile.m_dest              = ProgramUnitToWMXPos(eAxisType, inching) * (bDirection ? 1 : -1);
                axisprofile.m_velocity          = ProgramUnitToWMXVel(eAxisType, Velocity);
                axisprofile.m_profileType       = WMXParam.m_profileType.JerkRatio;
                axisprofile.m_acc               = Math.Abs(ProgramUnitToWMXVel(eAxisType, Acc));
                axisprofile.m_dec               = Math.Abs(ProgramUnitToWMXVel(eAxisType, Dec));
                axisprofile.m_jerkRatio         = 0.75;

                if (!AxisStatus.m_servoRun)
                {
                    int err = m_WMXMotion.RelativeMove(axisprofile);

                    if (err != WMXParam.ErrorCode_None)
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                }
            }
        }
        private void CMD_AxisTargetMove(int nAxis, float Velocity, float Acc, float Dec, double target, AxisType eAxisType)
        {
            var AxisStatus = GetAxisStatus(nAxis);

            if (nAxis != -1 && AxisStatus != null)
            {
                AxisProfile axisprofile         = new AxisProfile();
                axisprofile.m_axis              = nAxis;
                axisprofile.m_dest              = ProgramUnitToWMXPos(eAxisType, target);
                axisprofile.m_velocity          = ProgramUnitToWMXVel(eAxisType, Velocity);
                axisprofile.m_profileType       = WMXParam.m_profileType.JerkRatio;
                axisprofile.m_acc               = Math.Abs(ProgramUnitToWMXVel(eAxisType, Acc));
                axisprofile.m_dec               = Math.Abs(ProgramUnitToWMXVel(eAxisType, Dec));
                axisprofile.m_jerkRatio         = 0.75;

                if (!AxisStatus.m_servoRun)
                {
                    int err = m_WMXMotion.AbsoluteMove(axisprofile);

                    if (err != WMXParam.ErrorCode_None)
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                }
            }
        }

        private void CMD_AxisMotionStop(int nAxis)
        {
            if (nAxis != -1 && m_WMXMotion.IsServoRun(nAxis))
            {
                int err = m_WMXMotion.StopServo(nAxis);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }
        private void CMD_AxisAlarmClear(int nAxis)
        {
            if (nAxis != -1)
            {
                int err = m_WMXMotion.AlarmClear(nAxis);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }
        private void CMD_AxisMotionEStop(int nAxis)
        {
            if (nAxis != -1)
            {
                int err = m_WMXMotion.EmergencyStop(nAxis);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }

        private void CMD_X_Axis_Move_To_TeachingPos(Teaching_X_Pos eTeaching_X_Pos, bool IsAutoRun, int SpeedPercent)
        {
            int nAxis = GetWMXAxisNumber(PortAxis.X_Axis);
            var AxisStatus = GetAxisStatus(nAxis);


            if (nAxis != -1 && AxisStatus != null)
            {
                double Target;
                if (eTeaching_X_Pos == Teaching_X_Pos.OP_Pos)
                    Target = GetMotionParam().X_AxisParam.X_OP_Position;
                else if (eTeaching_X_Pos == Teaching_X_Pos.Wait_Pos)
                    Target = GetMotionParam().X_AxisParam.X_Wait_Position;
                else if (eTeaching_X_Pos == Teaching_X_Pos.LP_Pos)
                    Target = GetMotionParam().X_AxisParam.X_LP_Position;
                else
                    return;

                AxisProfile axisprofile = new AxisProfile();
                axisprofile.m_axis = nAxis;
                axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, Target);
                axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().X_AxisParam.X_Move_AutoRun_Speed : GetMotionParam().X_AxisParam.X_Move_Manual_Speed) * ((double)SpeedPercent / 100.0);
                axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().X_AxisParam.X_Move_AutoRun_Acc : GetMotionParam().X_AxisParam.X_Move_Manual_Acc) * ((double)SpeedPercent / 100.0));
                axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().X_AxisParam.X_Move_AutoRun_Dec : GetMotionParam().X_AxisParam.X_Move_Manual_Dec) * ((double)SpeedPercent / 100.0));
                axisprofile.m_jerkRatio = 0.75;

                if (!AxisStatus.m_servoRun)
                {
                    int err = m_WMXMotion.AbsoluteMove(axisprofile);

                    if (err != WMXParam.ErrorCode_None)
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                }
            }
        }
        private void CMD_Z_Axis_Move_To_TeachingPos(Teaching_Z_Pos eTeaching_Z_Pos, bool IsAutoRun, int SpeedPercent)
        {
            int nAxis = GetWMXAxisNumber(PortAxis.Z_Axis);
            var AxisStatus = GetAxisStatus(PortAxis.Z_Axis);

            if (nAxis != -1 && AxisStatus != null)
            {
                if (eTeaching_Z_Pos == Teaching_Z_Pos.Up_Pos)
                {
                    if (GetAxisCurrentPosition(PortAxis.Z_Axis) < GetMotionParam().Z_AxisParam.Z_OverrideDistance)
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().Z_AxisParam.Z_OverrideDistance);
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Speed : GetMotionParam().Z_AxisParam.Z_Move_Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Acc : GetMotionParam().Z_AxisParam.Z_Move_Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Dec : GetMotionParam().Z_AxisParam.Z_Move_Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_endvelocity = axisprofile.m_velocity * (GetMotionParam().Z_AxisParam.Z_OverrideDecPercent > 100.0 ? 1.0 : GetMotionParam().Z_AxisParam.Z_OverrideDecPercent / 100.0);
                        axisprofile.m_jerkRatio = 0.75;

                        TriggerCondition axisTrigger = new TriggerCondition();
                        axisTrigger.m_triggerType = WMXParam.m_triggerType.RemainingTime;
                        axisTrigger.m_triggerAxis = axisprofile.m_axis;
                        axisTrigger.m_triggerValue = 0;

                        if (!AxisStatus.m_servoRun)
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));

                            axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().Z_AxisParam.Z_Up_Position);

                            err = m_WMXMotion.AbsoluteMove(axisprofile, axisTrigger);

                            if (err != WMXParam.ErrorCode_None)
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                        }
                    }
                    else
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().Z_AxisParam.Z_Up_Position);
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Speed : GetMotionParam().Z_AxisParam.Z_Move_Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Acc : GetMotionParam().Z_AxisParam.Z_Move_Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Dec : GetMotionParam().Z_AxisParam.Z_Move_Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_jerkRatio = 0.75;

                        if (!AxisStatus.m_servoRun)
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                        }
                    }
                }
                else if (eTeaching_Z_Pos == Teaching_Z_Pos.Down_Pos)
                {
                    if (GetAxisCurrentPosition(PortAxis.Z_Axis) > GetMotionParam().Z_AxisParam.Z_OverrideDistance)
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().Z_AxisParam.Z_OverrideDistance);
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Speed : GetMotionParam().Z_AxisParam.Z_Move_Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Acc : GetMotionParam().Z_AxisParam.Z_Move_Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Dec : GetMotionParam().Z_AxisParam.Z_Move_Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_endvelocity = axisprofile.m_velocity * (GetMotionParam().Z_AxisParam.Z_OverrideDecPercent > 100.0 ? 1.0 : GetMotionParam().Z_AxisParam.Z_OverrideDecPercent / 100.0);
                        axisprofile.m_jerkRatio = 0.75;

                        TriggerCondition axisTrigger = new TriggerCondition();
                        axisTrigger.m_triggerType = WMXParam.m_triggerType.RemainingTime;
                        axisTrigger.m_triggerAxis = axisprofile.m_axis;
                        axisTrigger.m_triggerValue = 0;

                        if (!AxisStatus.m_servoRun)
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));

                            axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().Z_AxisParam.Z_Down_Position);

                            err = m_WMXMotion.AbsoluteMove(axisprofile, axisTrigger);

                            if (err != WMXParam.ErrorCode_None)
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                        }
                    }
                    else
                    {
                        AxisProfile axisprofile = new AxisProfile();
                        axisprofile.m_axis = nAxis;
                        axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Linear, GetMotionParam().Z_AxisParam.Z_Down_Position);
                        axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Speed : GetMotionParam().Z_AxisParam.Z_Move_Manual_Speed) * ((double)SpeedPercent / 100.0);
                        axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Acc : GetMotionParam().Z_AxisParam.Z_Move_Manual_Acc) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Linear, IsAutoRun ? GetMotionParam().Z_AxisParam.Z_Move_AutoRun_Dec : GetMotionParam().Z_AxisParam.Z_Move_Manual_Dec) * ((double)SpeedPercent / 100.0));
                        axisprofile.m_jerkRatio = 0.75;

                        if (!AxisStatus.m_servoRun)
                        {
                            int err = m_WMXMotion.AbsoluteMove(axisprofile);

                            if (err != WMXParam.ErrorCode_None)
                                Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                        }
                    }
                }
                else
                    return;
            }
        }
        private void CMD_T_Axis_Move_To_TeachingPos(Teaching_T_Pos eTeaching_T_Pos, bool IsAutoRun, int SpeedPercent)
        {
            int nAxis = GetWMXAxisNumber(PortAxis.T_Axis);
            var AxisStatus = GetAxisStatus(PortAxis.T_Axis);

            if (nAxis != -1 && AxisStatus != null)
            {
                double Target;
                if (eTeaching_T_Pos == Teaching_T_Pos.Degree0_Pos)
                    Target = GetMotionParam().T_AxisParam.T_0_Degree_Position;
                else if (eTeaching_T_Pos == Teaching_T_Pos.Degree180_Pos)
                    Target = GetMotionParam().T_AxisParam.T_180_Degree_Position;
                else
                    return;

                AxisProfile axisprofile = new AxisProfile();
                axisprofile.m_axis = nAxis;
                axisprofile.m_dest = ProgramUnitToWMXPos(AxisType.Rotary, Target);
                axisprofile.m_velocity = ProgramUnitToWMXVel(AxisType.Rotary, IsAutoRun ? GetMotionParam().T_AxisParam.T_Move_AutoRun_Speed : GetMotionParam().T_AxisParam.T_Move_Manual_Speed) * ((double)SpeedPercent / 100.0);
                axisprofile.m_profileType = WMXParam.m_profileType.JerkRatio;
                axisprofile.m_acc = Math.Abs(ProgramUnitToWMXVel(AxisType.Rotary, IsAutoRun ? GetMotionParam().T_AxisParam.T_Move_AutoRun_Acc : GetMotionParam().T_AxisParam.T_Move_Manual_Acc) * ((double)SpeedPercent / 100.0));
                axisprofile.m_dec = Math.Abs(ProgramUnitToWMXVel(AxisType.Rotary, IsAutoRun ? GetMotionParam().T_AxisParam.T_Move_AutoRun_Dec : GetMotionParam().T_AxisParam.T_Move_Manual_Dec) * ((double)SpeedPercent / 100.0));
                axisprofile.m_jerkRatio = 0.75;

                if (!AxisStatus.m_servoRun)
                {
                    int err = m_WMXMotion.AbsoluteMove(axisprofile);

                    if (err != WMXParam.ErrorCode_None)
                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
                }
            }
        }

        private void CMD_AxisHomeDoneClear(PortAxis ePortAxis)
        {
            int nAxis = GetWMXAxisNumber(ePortAxis);

            if (nAxis != -1)
            {
                int err = m_WMXMotion.HomeDoneClear(nAxis);

                if (err != WMXParam.ErrorCode_None)
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(err)}"));
            }
        }
    }
}
