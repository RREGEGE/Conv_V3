using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Master.Interface.Alarm;


namespace Master.Equipment.Port
{
    /// <summary>
    /// PortAlarm.cs는 Port Alarm 정의 및 처리 관련 동작 작성
    /// </summary>
    public partial class Port
    {
        /// <summary>
        /// Port Code Alarm 표
        /// 발생 최신 순서대로 WordMap Array에 맵핑
        /// </summary>
        public enum PortAlarm : int
        {
            None = 0x00,
            Port_E_Stop = 0x01,
            GOT_E_Stop,
            Power_Off_Error,

            Key_Switch_Error = 5,
            Door_Open_Error,
            WMX_E_Stop,
            Software_E_Stop,
            Master_E_Stop,
            Maint_Door_Open_Error,




            X_Axis_ServoPack_Battery_Low_Alarm = 0x10,
            Z_Axis_ServoPack_Battery_Low_Alarm,
            T_Axis_ServoPack_Battery_Low_Alarm,

            X_Axis_ServoPack_Error = 0x20,
            X_Axis_POT,
            X_Axis_NOT,
            X_Axis_Origin_Search_Fail,
            X_Axis_Software_Limit_Error,
            X_Axis_Dest_Position_Error_Encorder,
            X_Axis_Absolute_Origin_Loss,
            X_Axis_Dest_Positioning_Complete_Error,
            X_Axis_OverLoad_Error,
            X_Axis_HomeSensor_Always_On_Error,
            X_Axis_HomeMove_TimeOutError,
            X_Axis_Dest_Position_Error_Sensor,
            X_Axis_Move_To_LP_Timeout_Error,
            X_Axis_Move_To_Wait_Timeout_Error,
            X_Axis_Move_To_OP_Timeout_Error,

            Z_Axis_ServoPack_Error = 0x30,
            Z_Axis_POT,
            Z_Axis_NOT,
            Z_Axis_Origin_Search_Fail,
            Z_Axis_Software_Limit_Error,
            Z_Axis_Dest_Position_Error_Encorder,
            Z_Axis_Absolute_Origin_Loss,
            Z_Axis_Dest_Positioning_Complete_Error,
            Z_Axis_OverLoad_Error,
            Z_Axis_HomeSensor_Always_On_Error,
            Z_Axis_HomeMove_TimeOutError,
            Z_Axis_Move_To_Up_Timeout_Error,
            Z_Axis_Move_To_Down_Timeout_Error,

            T_Axis_ServoPack_Error = 0x40,
            T_Axis_POT,
            T_Axis_NOT,
            T_Axis_Origin_Search_Fail,
            T_Axis_Software_Limit_Error,
            T_Axis_Dest_Position_Error_Encorder,
            T_Axis_Absolute_Origin_Loss,
            T_Axis_Dest_Positioning_Complete_Error,
            T_Axis_OverLoad_Error,
            T_Axis_HomeSensor_Always_On_Error,
            T_Axis_HomeMove_TimeOutError,
            T_Axis_Move_To_0Deg_Timeout_Error,
            T_Axis_Move_To_180Deg_Timeout_Error,

            OHT_Detect_Error = 0x50,
            Fork_Detect_Error,
            OP_Placement_Detect_Error,
            Shuttle_Placement_Detect_Error,
            LP_Placement_Detect_Error,
            Port_Area_Sensor_Error,
            X_Axis_Crash_Detect_Error,
            Z_Axis_Crash_Detect_Error,
            T_Axis_Crash_Detect_Error,
            OP_No_Cassette_ID_Error,
            LP_No_Cassette_ID_Error,
            Shuttle_No_Cassette_ID_Error,
            LP_Diagonal_Sensor_Error,
            OP_CST_Detect_Sensor_Group_Error,

            CIM_IF_TimeOut_RF_Read_Req_Error = 0x60,
            CIM_IF_TimeOut_ID_Remove_Error,
            RM_PIO_IF_TimeOut_Error,
            Port_PIO_IF_TimeOut_Error,
            //Port_Stop_From_CIM,
            Step_TimeOver_Error = 0x65,

            Conveyor_Moving_TimeOut_Error = 0x70,
            Tag_Read_Fail,
            Tag_Disconnection,
            //OMRON_PIO_Comm_Disconnection,
            //OMRON_PIO_Invalid_State,

            Buffer_LP_CV_Error = 0x80,
            Buffer_LP_Z_Error,
            Buffer_LP_Z_POT,
            Buffer_LP_Z_NOT,

            Buffer_OP_CV_Error = 0x90,
            Buffer_OP_Z_Error,
            Buffer_OP_Z_POT,
            Buffer_OP_Z_NOT

        }

        /// <summary>
        /// Alarm 발생 시 Port 정지 축 지정
        /// </summary>
        enum PortAlarmStopCase
        {
            All,
            X_Axis,
            Z_Axis,
            T_Axis,
            None
        }

        /// <summary>
        /// Port Alarm Code 변수이며 Port 객체 할당 시 생성
        /// </summary>
        CodeAlarm m_PortAlarm;

        /// <summary>
        /// Home Sensor가 위치할 Range 지정
        /// </summary>
        private double X_Axis_HomeSensor_Range = 40.0; //mm
        private double Z_Axis_HomeSensor_Range = 15.0; //mm
        private double T_Axis_HomeSensor_Range = 10.0; //Degree

        /// <summary>
        /// Sensor 상황에 따른 Watch dog 시작 명령이며 검출 시 Alarm 및 정지 명령 제어
        /// </summary>
        /// <param name="eWatchdog"></param>
        /// <param name="ePortAlarm"></param>
        /// <param name="ePortAlarmStopCase"></param>
        private void Alarm_WatchdogProcess(WatchdogList eWatchdog, PortAlarm ePortAlarm, PortAlarmStopCase ePortAlarmStopCase)
        {
            Watchdog_Start(eWatchdog);
            if (Watchdog_IsDetect(eWatchdog))
            {
                AlarmInsert((short)ePortAlarm, AlarmLevel.Error);

                if (ePortAlarmStopCase == PortAlarmStopCase.All)
                    CMD_PortStop();
                else if (ePortAlarmStopCase == PortAlarmStopCase.X_Axis)
                    ServoCtrl_MotionStop(PortAxis.Shuttle_X);
                else if (ePortAlarmStopCase == PortAlarmStopCase.Z_Axis)
                {
                    if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z))
                        ServoCtrl_MotionStop(PortAxis.Shuttle_Z);
                    else if (GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z))
                        CylinderCtrl_MotionStop(PortAxis.Shuttle_Z);
                }
                else if (ePortAlarmStopCase == PortAlarmStopCase.T_Axis)
                    ServoCtrl_MotionStop(PortAxis.Shuttle_T);
            }
        }

        /// <summary>
        /// Port의 모든 Alarm을 Clear
        /// </summary>
        private void CMD_PortAlarmClear()
        {
            var ClearAlarms = m_PortAlarm.Clear();

            foreach (var ClearAlarm in ClearAlarms)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AlarmClear, $"Alarm Code: {ClearAlarm.AlarmCode} / Alarm Name: {ClearAlarm.AlarmComment} Clear.");
            }
        }

        /// <summary>
        /// Port Alarm 검출 시 Alarm Insert 및 Alarm에 따른 로그를 작성
        /// </summary>
        /// <param name="AlarmCode"></param>
        /// <param name="eAlarmLevel"></param>
        public void AlarmInsert(short AlarmCode, AlarmLevel eAlarmLevel)
        {
            //1. 포함되어 있지 않음 알람인 경우
            if (!m_PortAlarm.Contains(AlarmCode))
            {
                //2. 알람 Comment (Title) 얻기
                string AlarmComment = GetPortAlarmComment(AlarmCode);

                //3. 알람 Insert 진행
                if (m_PortAlarm.Insert(AlarmCode, AlarmComment, eAlarmLevel))
                {
                    //4. Insert 성공 시 로그 작성
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmCreate, $"Alarm Code: {AlarmCode} / Alarm Name: {AlarmComment} Create.");

                    //5. 알람에 따른 상세 Log 추가 출력
                    PrintDetailLog((PortAlarm)AlarmCode);
                }
            }
        }

        /// <summary>
        /// Port Alarm 출력 시 상세 로그를 작성하고 싶은 경우 추가
        /// </summary>
        /// <param name="ePortAlarm"></param>
        private void PrintDetailLog(PortAlarm ePortAlarm)
        {
            switch (ePortAlarm)
            {
                case PortAlarm.None:
                    break;
                case PortAlarm.Port_E_Stop:
                    break;
                case PortAlarm.GOT_E_Stop:
                    break;
                case PortAlarm.Power_Off_Error:
                    break;
                case PortAlarm.Key_Switch_Error:
                    break;
                case PortAlarm.Door_Open_Error:
                    break;
                case PortAlarm.WMX_E_Stop:
                    break;
                case PortAlarm.Software_E_Stop:
                    break;
                case PortAlarm.Master_E_Stop:
                    break;
                case PortAlarm.Maint_Door_Open_Error:
                    break;
                case PortAlarm.X_Axis_ServoPack_Battery_Low_Alarm:
                    break;
                case PortAlarm.Z_Axis_ServoPack_Battery_Low_Alarm:
                    break;
                case PortAlarm.T_Axis_ServoPack_Battery_Low_Alarm:
                    break;
                case PortAlarm.X_Axis_ServoPack_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"X Axis Servo Pack Error Code : 0x{ServoCtrl_GetAxisAlarmCode(PortAxis.Shuttle_X).ToString("x4")}");
                    }
                    break;
                case PortAlarm.X_Axis_POT:
                    break;
                case PortAlarm.X_Axis_NOT:
                    break;
                case PortAlarm.X_Axis_Origin_Search_Fail:
                    break;
                case PortAlarm.X_Axis_Software_Limit_Error:
                    break;
                case PortAlarm.X_Axis_Dest_Position_Error_Encorder:
                    break;
                case PortAlarm.X_Axis_Absolute_Origin_Loss:
                    break;
                case PortAlarm.X_Axis_Dest_Positioning_Complete_Error:
                    break;
                case PortAlarm.X_Axis_OverLoad_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"X Axis OverLoad Detail => Set Value : {Motion_X_Axis_OverloadSettingTorque}%, Detected Value : {Motion_X_Axis_OverloadDetectTorque}%");
                    }
                    break;
                case PortAlarm.X_Axis_HomeSensor_Always_On_Error:
                    {
                        float TeachingDownPos = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.MGV_LP_Pos);
                        float TeachingDownPos2 = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Equip_LP_Pos);
                        double Range = X_Axis_HomeSensor_Range;
                        bool bHomeSensor = Sensor_X_Axis_HOME;
                        bool bHomeDone = Sensor_X_Axis_OriginOK;

                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"X Axis HomeSensor Always On Detail => X Act Pos : {Motion_X_Axis_CurrentPosition}, Teaching LP Pos : MGV({TeachingDownPos}) Equip({TeachingDownPos2}), Range : {Range}, HomeSensor : {bHomeSensor}, HomeDone : {bHomeDone}");
                    }
                    break;
                case PortAlarm.X_Axis_HomeMove_TimeOutError:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"X Axis Homing Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.X_Axis_HomingTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.X_Axis_HomingTimer)}");
                    }
                    break;
                case PortAlarm.X_Axis_Dest_Position_Error_Sensor:
                    break;
                case PortAlarm.X_Axis_Move_To_LP_Timeout_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"X Axis LP Motion Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.X_Axis_Move_To_LPTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.X_Axis_Move_To_LPTimer)}");
                    }
                    break;
                case PortAlarm.X_Axis_Move_To_Wait_Timeout_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"X Axis Wait Motion Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.X_Axis_Move_To_WaitTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.X_Axis_Move_To_WaitTimer)}");
                    }
                    break;
                case PortAlarm.X_Axis_Move_To_OP_Timeout_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"X Axis OP Motion Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.X_Axis_Move_To_OPTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.X_Axis_Move_To_OPTimer)}");
                    }
                    break;
                case PortAlarm.Z_Axis_ServoPack_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"Z Axis Servo Pack Error Code : 0x{ServoCtrl_GetAxisAlarmCode(PortAxis.Shuttle_Z).ToString("x4")}");
                    }
                    break;
                case PortAlarm.Z_Axis_POT:
                    break;
                case PortAlarm.Z_Axis_NOT:
                    break;
                case PortAlarm.Z_Axis_Origin_Search_Fail:
                    break;
                case PortAlarm.Z_Axis_Software_Limit_Error:
                    break;
                case PortAlarm.Z_Axis_Dest_Position_Error_Encorder:
                    break;
                case PortAlarm.Z_Axis_Absolute_Origin_Loss:
                    break;
                case PortAlarm.Z_Axis_Dest_Positioning_Complete_Error:
                    break;
                case PortAlarm.Z_Axis_OverLoad_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"Z Axis OverLoad Detail => Set Value : {Motion_Z_Axis_OverloadSettingTorque}%, Detected Value : {Motion_Z_Axis_OverloadDetectTorque}%");
                    }
                    break;
                case PortAlarm.Z_Axis_HomeSensor_Always_On_Error:
                    {
                        float TeachingDownPos = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Down_Pos);
                        double Range = Z_Axis_HomeSensor_Range;
                        bool bHomeSensor = Sensor_Z_Axis_HOME;
                        bool bHomeDone = Sensor_Z_Axis_OriginOK;

                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"Z Axis HomeSensor Always On Detail => Z Act Pos : {Motion_Z_Axis_CurrentPosition}, Teaching Down Pos : {TeachingDownPos}, Range : {Range}, HomeSensor : {bHomeSensor}, HomeDone : {bHomeDone}");
                    }
                    break;
                case PortAlarm.Z_Axis_HomeMove_TimeOutError:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"Z Axis Homing Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.Z_Axis_HomingTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.Z_Axis_HomingTimer)}");
                    }
                    break;
                case PortAlarm.Z_Axis_Move_To_Up_Timeout_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"Z Axis Up Motion Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.Z_Axis_Move_To_UpTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.Z_Axis_Move_To_UpTimer)}");
                    }
                    break;
                case PortAlarm.Z_Axis_Move_To_Down_Timeout_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"Z Axis Down Motion Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.Z_Axis_Move_To_DownTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.Z_Axis_Move_To_DownTimer)}");
                    }
                    break;
                case PortAlarm.T_Axis_ServoPack_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"T Axis Servo Pack Error Code : 0x{ServoCtrl_GetAxisAlarmCode(PortAxis.Shuttle_T).ToString("x4")}");
                    }
                    break;
                case PortAlarm.T_Axis_POT:
                    break;
                case PortAlarm.T_Axis_NOT:
                    break;
                case PortAlarm.T_Axis_Origin_Search_Fail:
                    break;
                case PortAlarm.T_Axis_Software_Limit_Error:
                    break;
                case PortAlarm.T_Axis_Dest_Position_Error_Encorder:
                    break;
                case PortAlarm.T_Axis_Absolute_Origin_Loss:
                    break;
                case PortAlarm.T_Axis_Dest_Positioning_Complete_Error:
                    break;
                case PortAlarm.T_Axis_OverLoad_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"T Axis OverLoad Detail => Set Value : {Motion_T_Axis_OverloadSettingTorque}%, Detected Value : {Motion_T_Axis_OverloadDetectTorque}%");
                    }
                    break;
                case PortAlarm.T_Axis_HomeSensor_Always_On_Error:
                    {
                        float TeachingDownPos = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos);
                        double Range = T_Axis_HomeSensor_Range;
                        bool bHomeSensor = Sensor_T_Axis_HOME;
                        bool bHomeDone = Sensor_T_Axis_OriginOK;

                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"T Axis HomeSensor Always On Detail => T Act Pos : {Motion_T_Axis_CurrentPosition}, Teaching 0 Deg Pos : {TeachingDownPos}, Range : {Range}, HomeSensor : {bHomeSensor}, HomeDone : {bHomeDone}");
                    }
                    break;
                case PortAlarm.T_Axis_HomeMove_TimeOutError:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"T Axis Homing Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.T_Axis_HomingTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.T_Axis_HomingTimer)}");
                    }
                    break;
                case PortAlarm.T_Axis_Move_To_0Deg_Timeout_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"T Axis 0 Deg Motion Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.T_Axis_Move_To_0DegTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.T_Axis_Move_To_0DegTimer)}");
                    }
                    break;
                case PortAlarm.T_Axis_Move_To_180Deg_Timeout_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"T Axis 180 Deg Motion Timeout Detail => Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.T_Axis_Move_To_180DegTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.T_Axis_Move_To_180DegTimer)}");
                    }
                    break;
                case PortAlarm.OHT_Detect_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"OHT Detect Error Detail => Hoist Sensor : {Sensor_LP_Hoist_Detect}, Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.OHT_HoistDetectTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.OHT_HoistDetectTimer)}");
                    }
                    break;
                case PortAlarm.Fork_Detect_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, 
                            $"Fork Detect Error Detail => Fork Sensor : {Sensor_OP_Fork_Detect}, Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.RM_ForkDetectTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.RM_ForkDetectTimer)}");
                    }
                    break;
                case PortAlarm.OP_Placement_Detect_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"OP Placement Detect Error Detail => ");
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"Port Type: {GetParam().ePortType}, Operation Mode: {GetPortOperationMode()}, Buffer Type: {GetMotionParam().eBufferType}, Direction: {GetOperationDirection()}");
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"OP CST Detect 1 : {Sensor_OP_CST_Detect1}, OP CST Detect 2 : {Sensor_OP_CST_Detect2}, OP CST Presence : {Sensor_OP_CST_Presence}");

                        if (IsBufferControlPort())
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"Z Axis Pos1 : {Sensor_OP_Z_POS1}, Z Axis Pos2 : {Sensor_OP_Z_POS2}, OP CV IN : {Sensor_OP_CV_IN}, OP CV STOP : {Sensor_OP_CV_STOP}");
                    }
                    break;
                case PortAlarm.Shuttle_Placement_Detect_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"Shuttle Placement Detect Error Detail => ");
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"Port Type: {GetParam().ePortType}, Operation Mode: {GetPortOperationMode()}, Buffer Type: {GetMotionParam().eBufferType}, Direction: {GetOperationDirection()}");
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"Shuttle CST Detect 1 : {Sensor_Shuttle_CSTDetect1}, Shuttle CST Detect 2 : {Sensor_Shuttle_CSTDetect2}");
                    }
                    break;
                case PortAlarm.LP_Placement_Detect_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"LP Placement Detect Error Detail => ");
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"Port Type: {GetParam().ePortType}, Operation Mode: {GetPortOperationMode()}, Buffer Type: {GetMotionParam().eBufferType}, Direction: {GetOperationDirection()}");
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"LP CST Detect 1 : {Sensor_LP_CST_Detect1}, LP CST Detect 2 : {Sensor_LP_CST_Detect2}, LP CST Presence : {Sensor_LP_CST_Presence}");

                        if (IsBufferControlPort())
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation, $"Z Axis Pos1 : {Sensor_LP_Z_POS1}, Z Axis Pos2 : {Sensor_LP_Z_POS2}, LP CV IN : {Sensor_LP_CV_IN}, LP CV STOP : {Sensor_LP_CV_STOP}");
                    }
                    break;
                case PortAlarm.Port_Area_Sensor_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"L.C Sensor State : {Sensor_LightCurtain}, Area Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.PortArea_Timer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.PortArea_Timer)}");

                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.AlarmDetailInformation,
                            $"Port Busy : {IsPortBusy()}, Area Moving Watchdog Set Time : {Watchdog_Get_DetectTime(WatchdogList.PortArea_And_ShuttleMovingErrorTimer)}, Detected Time : {Watchdog_GetProgressTime(WatchdogList.PortArea_And_ShuttleMovingErrorTimer)}");
                    }
                    break;
                case PortAlarm.X_Axis_Crash_Detect_Error:
                    break;
                case PortAlarm.Z_Axis_Crash_Detect_Error:
                    break;
                case PortAlarm.T_Axis_Crash_Detect_Error:
                    break;
                case PortAlarm.OP_No_Cassette_ID_Error:
                    break;
                case PortAlarm.LP_No_Cassette_ID_Error:
                    break;
                case PortAlarm.Shuttle_No_Cassette_ID_Error:
                    break;
                case PortAlarm.LP_Diagonal_Sensor_Error:
                    break;
                case PortAlarm.OP_CST_Detect_Sensor_Group_Error:
                    break;
                case PortAlarm.CIM_IF_TimeOut_RF_Read_Req_Error:
                    break;
                case PortAlarm.CIM_IF_TimeOut_ID_Remove_Error:
                    break;
                case PortAlarm.RM_PIO_IF_TimeOut_Error:
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PIOInformation,
                                   $"Port -> RM: Load[{PIOStatus_PortToSTK_Load_Req}], Unload[{PIOStatus_PortToSTK_Unload_Req}], Ready[{PIOStatus_PortToSTK_Ready}], Error[{PIOStatus_PortToSTK_Error}]");
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PIOInformation,
                            $"RM -> Port: TR[{PIOStatus_STKToPort_TR_REQ}], Busy[{PIOStatus_STKToPort_Busy}], Complete[{PIOStatus_STKToPort_Complete}], Error[{PIOStatus_STKToPort_STKError}]");
                    }
                    break;
                case PortAlarm.Port_PIO_IF_TimeOut_Error:
                    {
                        if (GetPortOperationMode() == PortOperationMode.AGV || GetPortOperationMode() == PortOperationMode.Conveyor)
                        {
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PIOInformation,
                                $"Port -> AGV: ES[{PIOStatus_PortToAGV_ES}], Load[{PIOStatus_PortToAGV_Load_Req}], Unload[{PIOStatus_PortToAGV_Unload_Req}], Ready[{PIOStatus_PortToAGV_Ready}]");
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PIOInformation,
                                $"AGV -> Port: CS[{PIOStatus_AGVToPort_CS0}], Valid[{PIOStatus_AGVToPort_Valid}], TR[{PIOStatus_AGVToPort_TR_Req}], Busy[{PIOStatus_AGVToPort_Busy}], Complete[{PIOStatus_AGVToPort_Complete}]");
                        }
                        else if (GetPortOperationMode() == PortOperationMode.OHT)
                        {
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PIOInformation,
                                $"Port -> OHT: ES[{PIOStatus_PortToOHT_ES}], Load[{PIOStatus_PortToOHT_Load_Req}], Unload[{PIOStatus_PortToOHT_Unload_Req}], Ready[{PIOStatus_PortToOHT_Ready}], HO_AVBL[{PIOStatus_PortToOHT_HO_AVBL}]");
                            LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.PIOInformation,
                                $"OHT -> Port: CS[{PIOStatus_OHTToPort_CS0}], Valid[{PIOStatus_OHTToPort_Valid}], TR[{PIOStatus_OHTToPort_TR_Req}], Busy[{PIOStatus_OHTToPort_Busy}], Complete[{PIOStatus_OHTToPort_Complete}]");
                        }
                    }
                    break;
                case PortAlarm.Step_TimeOver_Error:
                    break;
                case PortAlarm.Conveyor_Moving_TimeOut_Error:
                    break;
                case PortAlarm.Tag_Read_Fail:
                    break;
                case PortAlarm.Tag_Disconnection:
                    break;
                case PortAlarm.Buffer_LP_CV_Error:
                    break;
                case PortAlarm.Buffer_LP_Z_Error:
                    break;
                case PortAlarm.Buffer_LP_Z_POT:
                    break;
                case PortAlarm.Buffer_LP_Z_NOT:
                    break;
                case PortAlarm.Buffer_OP_CV_Error:
                    break;
                case PortAlarm.Buffer_OP_Z_Error:
                    break;
                case PortAlarm.Buffer_OP_Z_POT:
                    break;
                case PortAlarm.Buffer_OP_Z_NOT:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Port Alarm Check thread
        /// PortStatusUpdate() 함수에서 반복 호출 (상시 체크)
        /// </summary>
        private void AlarmCheck()
        {
            List<PortAlarm> CheckList = new List<PortAlarm>();
            CheckList.Add(PortAlarm.Port_E_Stop);
            CheckList.Add(PortAlarm.Power_Off_Error);
            CheckList.Add(PortAlarm.Maint_Door_Open_Error);

            if (GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.OHT)
            {
                CheckList.Add(PortAlarm.Key_Switch_Error);
                CheckList.Add(PortAlarm.Door_Open_Error);
            }


            CheckList.Add(PortAlarm.WMX_E_Stop);
            CheckList.Add(PortAlarm.Software_E_Stop);
            CheckList.Add(PortAlarm.Master_E_Stop);

            if (GetMotionParam().IsValidServo(PortAxis.Shuttle_X))
            {
                CheckList.Add(PortAlarm.X_Axis_ServoPack_Battery_Low_Alarm);
                CheckList.Add(PortAlarm.X_Axis_HomeSensor_Always_On_Error);
                CheckList.Add(PortAlarm.X_Axis_HomeMove_TimeOutError);
                CheckList.Add(PortAlarm.X_Axis_Move_To_LP_Timeout_Error);
                CheckList.Add(PortAlarm.X_Axis_Move_To_Wait_Timeout_Error);
                CheckList.Add(PortAlarm.X_Axis_Move_To_OP_Timeout_Error);
                CheckList.Add(PortAlarm.X_Axis_ServoPack_Error);
                CheckList.Add(PortAlarm.X_Axis_POT);
                CheckList.Add(PortAlarm.X_Axis_NOT);
                CheckList.Add(PortAlarm.X_Axis_Origin_Search_Fail);
                CheckList.Add(PortAlarm.X_Axis_Software_Limit_Error);
                CheckList.Add(PortAlarm.X_Axis_OverLoad_Error);
                CheckList.Add(PortAlarm.X_Axis_Crash_Detect_Error);
            }
            if (GetMotionParam().IsValidServo(PortAxis.Shuttle_Z))
            {
                CheckList.Add(PortAlarm.Z_Axis_ServoPack_Battery_Low_Alarm);
                CheckList.Add(PortAlarm.Z_Axis_HomeSensor_Always_On_Error);
                CheckList.Add(PortAlarm.Z_Axis_HomeMove_TimeOutError);
                CheckList.Add(PortAlarm.Z_Axis_Move_To_Up_Timeout_Error);
                CheckList.Add(PortAlarm.Z_Axis_Move_To_Down_Timeout_Error);
                CheckList.Add(PortAlarm.Z_Axis_ServoPack_Error);
                CheckList.Add(PortAlarm.Z_Axis_POT);
                CheckList.Add(PortAlarm.Z_Axis_NOT);
                CheckList.Add(PortAlarm.Z_Axis_Origin_Search_Fail);
                CheckList.Add(PortAlarm.Z_Axis_Software_Limit_Error);
                CheckList.Add(PortAlarm.Z_Axis_OverLoad_Error);
                CheckList.Add(PortAlarm.Z_Axis_Crash_Detect_Error);
            }
            else if(GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z))
            {
                CheckList.Add(PortAlarm.Z_Axis_Move_To_Up_Timeout_Error);
                CheckList.Add(PortAlarm.Z_Axis_Move_To_Down_Timeout_Error);
            }
            if (GetMotionParam().IsValidServo(PortAxis.Shuttle_T))
            {
                CheckList.Add(PortAlarm.T_Axis_ServoPack_Battery_Low_Alarm);
                CheckList.Add(PortAlarm.T_Axis_HomeSensor_Always_On_Error);
                CheckList.Add(PortAlarm.T_Axis_HomeMove_TimeOutError);
                CheckList.Add(PortAlarm.T_Axis_Move_To_0Deg_Timeout_Error);
                CheckList.Add(PortAlarm.T_Axis_Move_To_180Deg_Timeout_Error);
                CheckList.Add(PortAlarm.T_Axis_ServoPack_Error);
                CheckList.Add(PortAlarm.T_Axis_POT);
                CheckList.Add(PortAlarm.T_Axis_NOT);
                CheckList.Add(PortAlarm.T_Axis_Origin_Search_Fail);
                CheckList.Add(PortAlarm.T_Axis_Software_Limit_Error);
                CheckList.Add(PortAlarm.T_Axis_OverLoad_Error);
                CheckList.Add(PortAlarm.T_Axis_Crash_Detect_Error);
            }

            if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                CheckList.Add(PortAlarm.LP_Diagonal_Sensor_Error);

            //if (IsShuttleControlPort())
            //    CheckList.Add(PortAlarm.OP_CST_Detect_Sensor_Group_Error); //direct Update

            if (GetPortOperationMode() == PortOperationMode.OHT)
                CheckList.Add(PortAlarm.OHT_Detect_Error);

            if (GetPortOperationMode() != PortOperationMode.EQ)
                CheckList.Add(PortAlarm.Fork_Detect_Error);

            if (GetParam().ePortType <= PortType.AGV)
                CheckList.Add(PortAlarm.Port_Area_Sensor_Error);

            if (GetPortOperationMode() == PortOperationMode.Conveyor)
                CheckList.Add(PortAlarm.Conveyor_Moving_TimeOut_Error);

            if(GetParam().eTagReaderType != TagReader.TagReaderType.None)
                CheckList.Add(PortAlarm.Tag_Disconnection);

            if (GetPortOperationMode() == PortOperationMode.Conveyor)
            {
                if(GetMotionParam().IsCVUsed(BufferCV.Buffer_LP))
                    CheckList.Add(PortAlarm.Buffer_LP_CV_Error);

                if (GetMotionParam().IsCVUsed(BufferCV.Buffer_OP))
                    CheckList.Add(PortAlarm.Buffer_OP_CV_Error);

                if (GetMotionParam().IsInverterType(PortAxis.Buffer_LP_Z))
                {
                    CheckList.Add(PortAlarm.Buffer_LP_Z_Error);
                    CheckList.Add(PortAlarm.Buffer_LP_Z_POT);
                    CheckList.Add(PortAlarm.Buffer_LP_Z_NOT);
                }

                if (GetMotionParam().IsInverterType(PortAxis.Buffer_OP_Z))
                {
                    CheckList.Add(PortAlarm.Buffer_OP_Z_Error);
                    CheckList.Add(PortAlarm.Buffer_OP_Z_POT);
                    CheckList.Add(PortAlarm.Buffer_OP_Z_NOT);
                }
            }

            //if (GetParam().ePortType == PortType.Conveyor_OMRON)
            //{
            //    CheckList.Add(PortAlarm.OMRON_PIO_Comm_Disconnection);
            //    CheckList.Add(PortAlarm.OMRON_PIO_Invalid_State);
            //}


            foreach (PortAlarm eAlarm in CheckList)
            {
                switch (eAlarm)
                {
                    case PortAlarm.None:
                        break;
                    case PortAlarm.Port_E_Stop:
                        if (mHW_EStop.GetEStopState() == Interface.Safty.EStopState.EStop)
                        {
                            AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.GOT_E_Stop:
                        if(Master.mPortHandyTouch_EStop.IsEStop())
                        {
                            AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.Power_Off_Error:
                        {
                            if (!IsPortPowerOn() && (IsAutoControlRun() || IsAutoManualCycleRun()))
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            }
                        }
                        break;
                    case PortAlarm.Key_Switch_Error:
                        if ((IsAutoControlRun() || IsAutoManualCycleRun()) &&
                            (GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.OHT) &&
                            IsValidInputItemMapping(OHT_InputItem.Door_Open_Key_Status.ToString()))
                        {
                            if (Status_OHT_Key_Open_Status && GetPortOperationMode() == PortOperationMode.OHT)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            else if (Status_OHT_Key_Open_Status && GetPortOperationMode() == PortOperationMode.MGV)
                            {
                                if (!Get_LP_AutoControlStepToStr().ToLower().Contains("await mgv") &&
                                    !Get_BP_AutoControlStepToStr().ToLower().Contains("await mgv"))
                                {
                                    //LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AlarmCreate, $"OHT Key Alarm LP Step = {Get_LP_AutoControlStepToStr().ToLower()}");
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                }
                            }
                        }
                        break;
                    case PortAlarm.Door_Open_Error:
                        if ((IsAutoControlRun() || IsAutoManualCycleRun()) &&
                            (GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.OHT) &&
                            IsValidInputItemMapping(OHT_InputItem.Door_Close_Status.ToString()))
                        {
                            if (!Status_OHT_Door_Close && GetPortOperationMode() == PortOperationMode.OHT)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            else if (!Status_OHT_Door_Close && GetPortOperationMode() == PortOperationMode.MGV)
                            {
                                if (!Get_LP_AutoControlStepToStr().ToLower().Contains("await mgv") &&
                                    !Get_BP_AutoControlStepToStr().ToLower().Contains("await mgv"))
                                {
                                    //LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.AlarmCreate, $"OHT Door Open Alarm LP Step = {Get_LP_AutoControlStepToStr().ToLower()}");
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                }
                            }
                        }
                        break;
                    case PortAlarm.Maint_Door_Open_Error:
                        {
                            if ((IsAutoControlRun() || IsAutoManualCycleRun()))
                            {
                                if (IsValidInputItemMapping(OHT_InputItem.Maint_Door_Open.ToString()) && Status_Maint_Door_Open_Status)
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                else if (IsValidInputItemMapping(OHT_InputItem.Maint_Door_Open2.ToString()) && Status_Maint_Door_Open_Status2)
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            }
                        }
                        break;
                    case PortAlarm.WMX_E_Stop:
                        if(Master.IsWMXEStopState())
                        {
                            AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.Software_E_Stop:
                        if (mSW_EStop.GetEStopState() == Interface.Safty.EStopState.EStop)
                        {
                            AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.Master_E_Stop:
                        if(Master.mPort_EStop.GetEStopState() == Interface.Safty.EStopState.EStop)
                        {
                            AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.X_Axis_ServoPack_Battery_Low_Alarm:
                        {
                            if(ServoCtrl_GetAxisAlarmCode(PortAxis.Shuttle_X) == 0xffa2)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.Z_Axis_ServoPack_Battery_Low_Alarm:
                        {
                            if(ServoCtrl_GetAxisAlarmCode(PortAxis.Shuttle_Z) == 0xffa2)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.T_Axis_ServoPack_Battery_Low_Alarm:
                        {
                            if(ServoCtrl_GetAxisAlarmCode(PortAxis.Shuttle_T) == 0xffa2)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.X_Axis_Dest_Position_Error_Encorder:
                        break;
                    case PortAlarm.X_Axis_Absolute_Origin_Loss:
                        break;
                    case PortAlarm.X_Axis_Dest_Positioning_Complete_Error:
                        break;
                    case PortAlarm.X_Axis_HomeSensor_Always_On_Error:
                        {
                            var CheckType = GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos));

                            if (CheckType == PositionCheckType.Servo)
                                break;

                            if (IsAutoControlRun() && IsAutoManualCycleRun() &&
                                IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)), (float)X_Axis_HomeSensor_Range) &&
                                Sensor_X_Axis_HOME &&
                                Sensor_X_Axis_OriginOK)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.X_Axis_HomeMove_TimeOutError:
                    case PortAlarm.Z_Axis_HomeMove_TimeOutError:
                    case PortAlarm.T_Axis_HomeMove_TimeOutError:
                        {
                            PortAxis ePortAxis = PortAxis.Shuttle_X;
                            WatchdogList eWatchdogList = WatchdogList.X_Axis_HomingTimer;
                            PortAlarmStopCase ePortAlarmStopCase = PortAlarmStopCase.X_Axis;

                            if (eAlarm == PortAlarm.X_Axis_HomeMove_TimeOutError)
                            {
                                ePortAxis = PortAxis.Shuttle_X;
                                eWatchdogList = WatchdogList.X_Axis_HomingTimer;
                                ePortAlarmStopCase = PortAlarmStopCase.X_Axis;
                            }
                            else if (eAlarm == PortAlarm.Z_Axis_HomeMove_TimeOutError)
                            {
                                ePortAxis = PortAxis.Shuttle_Z;
                                eWatchdogList = WatchdogList.Z_Axis_HomingTimer;
                                ePortAlarmStopCase = PortAlarmStopCase.Z_Axis;
                            }
                            else if (eAlarm == PortAlarm.T_Axis_HomeMove_TimeOutError)
                            {
                                ePortAxis = PortAxis.Shuttle_T;
                                eWatchdogList = WatchdogList.T_Axis_HomingTimer;
                                ePortAlarmStopCase = PortAlarmStopCase.T_Axis;
                            }

                            if (ServoCtrl_GetHomingStatus(ePortAxis))
                            {
                                Alarm_WatchdogProcess(eWatchdogList, eAlarm, ePortAlarmStopCase);
                            }
                            else
                                Watchdog_Stop(eWatchdogList, false);
                        }
                        break;
                    case PortAlarm.X_Axis_Dest_Position_Error_Sensor:
                        break;

                    case PortAlarm.X_Axis_Move_To_LP_Timeout_Error:
                        {
                                WatchdogList eWatchdogList = WatchdogList.X_Axis_Move_To_LPTimer;
                                if (IsPortAxisBusy(PortAxis.Shuttle_X) && ServoCtrl_GetProfileTargetPosition(PortAxis.Shuttle_X) == GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)))
                                {
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.X_Axis);
                                }
                                else
                                    Watchdog_Stop(eWatchdogList, false);
                        }
                        break;
                    case PortAlarm.X_Axis_Move_To_Wait_Timeout_Error:
                        {
                                WatchdogList eWatchdogList = WatchdogList.X_Axis_Move_To_WaitTimer;
                                if (IsPortAxisBusy(PortAxis.Shuttle_X) && ServoCtrl_GetProfileTargetPosition(PortAxis.Shuttle_X) == GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Wait_Pos))
                                {
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.X_Axis);
                                }
                                else
                                    Watchdog_Stop(eWatchdogList, false);
                        }
                        break;
                    case PortAlarm.X_Axis_Move_To_OP_Timeout_Error:
                        {
                                WatchdogList eWatchdogList = WatchdogList.X_Axis_Move_To_OPTimer;
                                if (IsPortAxisBusy(PortAxis.Shuttle_X) && ServoCtrl_GetProfileTargetPosition(PortAxis.Shuttle_X) == GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos))
                                {
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.X_Axis);
                                }
                                else
                                    Watchdog_Stop(eWatchdogList, false);
                        }
                        break;
                    case PortAlarm.Z_Axis_Dest_Position_Error_Encorder:
                        break;
                    case PortAlarm.Z_Axis_Absolute_Origin_Loss:
                        break;
                    case PortAlarm.Z_Axis_Dest_Positioning_Complete_Error:
                        break;
                    case PortAlarm.Z_Axis_HomeSensor_Always_On_Error:
                        {
                            var CheckType = GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Down_Pos);

                            if (CheckType == PositionCheckType.Servo)
                                break;

                            if (IsAutoControlRun() && 
                                IsAxisPositionOutside(PortAxis.Shuttle_Z, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Down_Pos), (float)Z_Axis_HomeSensor_Range) &&
                                Sensor_Z_Axis_HOME &&
                                Sensor_Z_Axis_OriginOK)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.Z_Axis_Move_To_Up_Timeout_Error:
                        {
                            WatchdogList eWatchdogList = WatchdogList.Z_Axis_Move_To_UpTimer;
                            if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z))
                            {
                                if (IsPortAxisBusy(PortAxis.Shuttle_Z) &&
                                ServoCtrl_GetProfileTargetPosition(PortAxis.Shuttle_Z) == GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos))
                                {
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.Z_Axis);
                                }
                                else
                                    Watchdog_Stop(eWatchdogList, false);
                            }
                            else if(GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z))
                            {
                                if (IsPortAxisBusy(PortAxis.Shuttle_Z) && CylinderCtrl_GetRunStatus(PortAxis.Shuttle_Z, CylCtrlList.FWD))
                                {
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.Z_Axis);
                                }
                                else
                                    Watchdog_Stop(eWatchdogList, false);
                            }
                        }
                        break;
                    case PortAlarm.Z_Axis_Move_To_Down_Timeout_Error:
                        {
                            WatchdogList eWatchdogList = WatchdogList.Z_Axis_Move_To_DownTimer;
                            if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z))
                            {
                                if (IsPortAxisBusy(PortAxis.Shuttle_Z) && ServoCtrl_GetProfileTargetPosition(PortAxis.Shuttle_Z) == GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Down_Pos))
                                {
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.Z_Axis);
                                }
                                else
                                    Watchdog_Stop(eWatchdogList, false);
                            }
                            else if (GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z))
                            {
                                if (IsPortAxisBusy(PortAxis.Shuttle_Z) && CylinderCtrl_GetRunStatus(PortAxis.Shuttle_Z, CylCtrlList.BWD))
                                {
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.Z_Axis);
                                }
                                else
                                    Watchdog_Stop(eWatchdogList, false);
                            }
                        }
                        break;
                    case PortAlarm.X_Axis_ServoPack_Error:
                    case PortAlarm.Z_Axis_ServoPack_Error:
                    case PortAlarm.T_Axis_ServoPack_Error:
                        {
                            PortAxis ePortAxis = PortAxis.Shuttle_X;

                            if (eAlarm == PortAlarm.X_Axis_ServoPack_Error)
                                ePortAxis = PortAxis.Shuttle_X;
                            else if (eAlarm == PortAlarm.Z_Axis_ServoPack_Error)
                                ePortAxis = PortAxis.Shuttle_Z;
                            else if (eAlarm == PortAlarm.T_Axis_ServoPack_Error)
                                ePortAxis = PortAxis.Shuttle_T;

                            if (ServoCtrl_GetAlarmStatus(ePortAxis))
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            }
                        }
                        break;
                    case PortAlarm.X_Axis_POT:
                    case PortAlarm.Z_Axis_POT:
                    case PortAlarm.T_Axis_POT:
                        {
                            PortAxis ePortAxis = PortAxis.Shuttle_X;

                            if (IsShuttleControlPort())
                            {
                                if (eAlarm == PortAlarm.X_Axis_POT)
                                    ePortAxis = PortAxis.Shuttle_X;
                                else if (eAlarm == PortAlarm.Z_Axis_POT)
                                    ePortAxis = PortAxis.Shuttle_Z;
                                else if (eAlarm == PortAlarm.T_Axis_POT)
                                    ePortAxis = PortAxis.Shuttle_T;

                                bool bPOT = ePortAxis == PortAxis.Shuttle_X ? Sensor_X_Axis_POT : ePortAxis == PortAxis.Shuttle_Z ? Sensor_Z_Axis_POT : Sensor_T_Axis_POT;


                                if (bPOT)
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);

                                    if (!ServoCtrl_GetJoggingStatus(ePortAxis) && IsPortAxisBusy(ePortAxis))
                                    {
                                        if (!ServoCtrl_GetHomingStatus(ePortAxis))
                                            ServoCtrl_MotionStop(ePortAxis);
                                    }
                                    else if (ServoCtrl_GetJoggingStatus(ePortAxis))
                                    {
                                        if (ServoCtrl_GetTargetSpeed(ePortAxis) > 0)
                                            ServoCtrl_MotionStop(ePortAxis);
                                    }
                                }
                            }
                        }
                        break;
                    case PortAlarm.X_Axis_NOT:
                    case PortAlarm.Z_Axis_NOT:
                    case PortAlarm.T_Axis_NOT:
                        {
                            PortAxis ePortAxis = PortAxis.Shuttle_X;

                            if (IsShuttleControlPort())
                            {
                                if (eAlarm == PortAlarm.X_Axis_NOT)
                                    ePortAxis = PortAxis.Shuttle_X;
                                else if (eAlarm == PortAlarm.Z_Axis_NOT)
                                    ePortAxis = PortAxis.Shuttle_Z;
                                else if (eAlarm == PortAlarm.T_Axis_NOT)
                                    ePortAxis = PortAxis.Shuttle_T;

                                bool bNOT = ePortAxis == PortAxis.Shuttle_X ? Sensor_X_Axis_NOT : ePortAxis == PortAxis.Shuttle_Z ? Sensor_Z_Axis_NOT : Sensor_T_Axis_NOT;

                                if (bNOT)
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);

                                    if (!ServoCtrl_GetJoggingStatus(ePortAxis) && IsPortAxisBusy(ePortAxis))
                                    {
                                        if (!ServoCtrl_GetHomingStatus(ePortAxis))
                                            ServoCtrl_MotionStop(ePortAxis);
                                    }
                                    else if (ServoCtrl_GetJoggingStatus(ePortAxis))
                                    {
                                        if (ServoCtrl_GetTargetSpeed(ePortAxis) < 0)
                                            ServoCtrl_MotionStop(ePortAxis);
                                    }
                                }
                            }
                        }
                        break;
                    case PortAlarm.X_Axis_Origin_Search_Fail:
                    case PortAlarm.Z_Axis_Origin_Search_Fail:
                    case PortAlarm.T_Axis_Origin_Search_Fail:
                        {
                            PortAxis ePortAxis = PortAxis.Shuttle_X;

                            if (eAlarm == PortAlarm.X_Axis_Origin_Search_Fail)
                                ePortAxis = PortAxis.Shuttle_X;
                            else if (eAlarm == PortAlarm.Z_Axis_Origin_Search_Fail)
                                ePortAxis = PortAxis.Shuttle_Z;
                            else if (eAlarm == PortAlarm.T_Axis_Origin_Search_Fail)
                                ePortAxis = PortAxis.Shuttle_T;

                            bool bPOT = ePortAxis == PortAxis.Shuttle_X ? Sensor_X_Axis_POT : ePortAxis == PortAxis.Shuttle_Z ? Sensor_Z_Axis_POT : Sensor_T_Axis_POT;
                            bool bNOT = ePortAxis == PortAxis.Shuttle_X ? Sensor_X_Axis_NOT : ePortAxis == PortAxis.Shuttle_Z ? Sensor_Z_Axis_NOT : Sensor_T_Axis_NOT;

                            if ((bPOT || bNOT) &&
                                ServoCtrl_GetHomingStatus(ePortAxis))
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                ServoCtrl_MotionStop(ePortAxis);
                            }
                        }
                        break;
                    case PortAlarm.X_Axis_Software_Limit_Error:
                    case PortAlarm.Z_Axis_Software_Limit_Error:
                    case PortAlarm.T_Axis_Software_Limit_Error:
                        {
                            PortAxis ePortAxis = PortAxis.Shuttle_X;

                            if (eAlarm == PortAlarm.X_Axis_Software_Limit_Error)
                                ePortAxis = PortAxis.Shuttle_X;
                            else if (eAlarm == PortAlarm.Z_Axis_Software_Limit_Error)
                                ePortAxis = PortAxis.Shuttle_Z;
                            else if (eAlarm == PortAlarm.T_Axis_Software_Limit_Error)
                                ePortAxis = PortAxis.Shuttle_T;

                            if (ServoCtrl_GetSoftLimitStatus(ePortAxis))
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            }
                        }
                        break;
                    case PortAlarm.T_Axis_Dest_Position_Error_Encorder:
                        break;
                    case PortAlarm.T_Axis_Absolute_Origin_Loss:
                        break;
                    case PortAlarm.T_Axis_Dest_Positioning_Complete_Error:
                        break;
                    case PortAlarm.X_Axis_OverLoad_Error:
                    case PortAlarm.Z_Axis_OverLoad_Error:
                    case PortAlarm.T_Axis_OverLoad_Error:
                        {
                            PortAxis ePortAxis = PortAxis.Shuttle_X;

                            if (eAlarm == PortAlarm.X_Axis_OverLoad_Error)
                                ePortAxis = PortAxis.Shuttle_X;
                            else if (eAlarm == PortAlarm.Z_Axis_OverLoad_Error)
                                ePortAxis = PortAxis.Shuttle_Z;
                            else if (eAlarm == PortAlarm.T_Axis_OverLoad_Error)
                                ePortAxis = PortAxis.Shuttle_T;

                            double CurrentTorque = Motion_CurrentTorque(ePortAxis);
                            double SetMaxTorque = Motion_OverloadSettingTorque(ePortAxis);
                            if (Math.Abs(CurrentTorque) > SetMaxTorque)
                            {
                                Motion_OverloadDetectTorque(ePortAxis, Math.Abs(CurrentTorque));
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                ServoCtrl_MotionStop(ePortAxis);
                            }
                        }
                        break;
                    case PortAlarm.T_Axis_HomeSensor_Always_On_Error:
                        {
                            var CheckType = GetMotionParam().GetPositionCheckType(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos);

                            if (CheckType == PositionCheckType.Servo)
                                break;

                            if (IsAutoControlRun() && 
                                IsAxisPositionOutside(PortAxis.Shuttle_T, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos), (float)T_Axis_HomeSensor_Range) &&
                                Sensor_T_Axis_HOME &&
                                Sensor_T_Axis_OriginOK)
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    case PortAlarm.T_Axis_Move_To_0Deg_Timeout_Error:
                        {
                            WatchdogList eWatchdogList = WatchdogList.T_Axis_Move_To_0DegTimer;
                            if (IsPortAxisBusy(PortAxis.Shuttle_T) &&
                                ServoCtrl_GetProfileTargetPosition(PortAxis.Shuttle_T) == GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos))
                            {
                                Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.T_Axis);
                            }
                            else
                                Watchdog_Stop(eWatchdogList, false);
                        }
                        break;
                    case PortAlarm.T_Axis_Move_To_180Deg_Timeout_Error:
                        {
                            WatchdogList eWatchdogList = WatchdogList.T_Axis_Move_To_180DegTimer;
                            if (IsPortAxisBusy(PortAxis.Shuttle_T) &&
                                ServoCtrl_GetProfileTargetPosition(PortAxis.Shuttle_T) == GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree180_Pos))
                            {
                                Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.T_Axis);
                            }
                            else
                                Watchdog_Stop(eWatchdogList, false);
                        }
                        break;
                    case PortAlarm.OHT_Detect_Error:
                        {
                            WatchdogList eWatchdogList = WatchdogList.OHT_HoistDetectTimer;
                            if (!PIOStatus_PortToOHT_Ready && Sensor_LP_Hoist_Detect)
                            {
                                Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.None);
                            }
                            else
                                Watchdog_Stop(eWatchdogList, true);
                        }
                        break;
                    case PortAlarm.Fork_Detect_Error:
                        {
                            WatchdogList eWatchdogList = WatchdogList.RM_ForkDetectTimer;


                            if (!PIOStatus_PortToSTK_Ready && Sensor_OP_Fork_Detect)
                            {
                                if (GetMotionParam().IsCylinderType(PortAxis.Buffer_OP_Y) && IsYAxisPos_FWD(PortAxis.Buffer_OP_Y))
                                    Watchdog_Stop(eWatchdogList, true);
                                else
                                    Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.None);
                            }
                            else
                                Watchdog_Stop(eWatchdogList, true);
                        }
                        break;
                    //case PortAlarm.OP_Placement_Detect_Error:
                    //    {
                    //        WatchdogList eWatchdogList = WatchdogList.OP_Placement_ErrorTimer;

                    //        if (IsAutoControlRun())
                    //        {
                    //            if (GetOperationDirection() == PortDirection.Input)
                    //            {
                    //                if ((OPBufferAutoControlInModeStep)Get_OP_AutoControlStep() == OPBufferAutoControlInModeStep.Step07_Handshake_End_Check)
                    //                {
                    //                    if (Carrier_CheckOP_ExistProduct(true, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //                else if ((OPBufferAutoControlInModeStep)Get_OP_AutoControlStep() <= OPBufferAutoControlInModeStep.Step05_RM_To_Motion_Check &&
                    //                        (OPBufferAutoControlInModeStep)Get_OP_AutoControlStep() >= OPBufferAutoControlInModeStep.Step04_RM_TR_REQ_Check)
                    //                {
                    //                    if (Carrier_CheckOP_ExistProduct(false, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if ((OPBufferAutoControlOutModeStep)Get_OP_AutoControlStep() == OPBufferAutoControlOutModeStep.Step07_RM_TR_REQ_Off_Wait)
                    //                {
                    //                    if (Carrier_CheckOP_ExistProduct(false, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //                else if ((OPBufferAutoControlOutModeStep)Get_OP_AutoControlStep() <= OPBufferAutoControlOutModeStep.Step05_RM_To_Motion_Check &&
                    //                        (OPBufferAutoControlOutModeStep)Get_OP_AutoControlStep() >= OPBufferAutoControlOutModeStep.Step04_RM_TR_REQ_Check)
                    //                {
                    //                    if (Carrier_CheckOP_ExistProduct(true, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    break;
                    //case PortAlarm.Shuttle_Placement_Detect_Error:
                    //    {
                    //        WatchdogList eWatchdogList = WatchdogList.BP_Placement_ErrorTimer;
                    //        if (IsAutoControlRun())
                    //        {
                    //            if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z))
                    //            {
                    //                if (GetAxisPositionInsideCheck(PortAxis.Z_Axis, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos), 5.0f))
                    //                {
                    //                    if (Carrier_CheckBP_ExistProduct(false) &&
                    //                        ((ShuttleAutoControlOutModeStep)Get_BP_AutoControlStep() > ShuttleAutoControlOutModeStep.Step03_Z_Axis_Move_To_Down_Pos ||
                    //                        (ShuttleAutoControlInModeStep)Get_BP_AutoControlStep() > ShuttleAutoControlInModeStep.Step03_Z_Axis_Move_To_Down_Pos))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //                else if (GetAxisPositionInsideCheck(PortAxis.Z_Axis, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Down_Pos), 5.0f))
                    //                {
                    //                    if (Carrier_CheckBP_ExistProduct(true))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    break;
                    //case PortAlarm.LP_Placement_Detect_Error:
                    //    {
                    //        WatchdogList eWatchdogList = WatchdogList.LP_Placement_ErrorTimer;

                    //        if (IsAutoControlRun())
                    //        {
                    //            if (GetOperationDirection() == PortDirection.Input)
                    //            {
                    //                if ((LPBufferAutoControlInModeStep)Get_LP_AutoControlStep() >= LPBufferAutoControlInModeStep.Step09_Handshake_End_Check &&
                    //                    (LPBufferAutoControlInModeStep)Get_LP_AutoControlStep() <= LPBufferAutoControlInModeStep.Step11_RFID_Reading)
                    //                {
                    //                    if (Carrier_CheckLP_ExistProduct(false, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //                else if ((LPBufferAutoControlInModeStep)Get_LP_AutoControlStep() <= LPBufferAutoControlInModeStep.Step07_TR_Busy_Check &&
                    //                        (LPBufferAutoControlInModeStep)Get_LP_AutoControlStep() >= LPBufferAutoControlInModeStep.Step05_Valid_Check)
                    //                {
                    //                    if (Carrier_CheckLP_ExistProduct(true, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if ((LPBufferAutoControlOutModeStep)Get_LP_AutoControlStep() == LPBufferAutoControlOutModeStep.Step09_Handshake_End_Check)
                    //                {
                    //                    if (Carrier_CheckLP_ExistProduct(true, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //                else if ((LPBufferAutoControlOutModeStep)Get_LP_AutoControlStep() <= LPBufferAutoControlOutModeStep.Step07_TR_Busy_Check &&
                    //                        (LPBufferAutoControlOutModeStep)Get_LP_AutoControlStep() >= LPBufferAutoControlOutModeStep.Step05_Valid_Check)
                    //                {
                    //                    if (Carrier_CheckLP_ExistProduct(false, false))
                    //                    {
                    //                        Alarm_WatchdogProcess(eWatchdogList, eAlarm, PortAlarmStopCase.All);
                    //                    }
                    //                    else
                    //                        Watchdog_Stop(eWatchdogList);
                    //                }
                    //            }
                    //        }
                    //    }
                    //break;
                    case PortAlarm.LP_Diagonal_Sensor_Error:
                        {
                            //One buffer Type 한정 체크
                            //자재를 들고 OP에 갔는데 LP 대각이 켜져 있는 경우 알람
                            if(IsAutoControlRun() ||
                                IsAutoManualCycleRun())
                            {
                                float Target = GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos);

                                if (IsAxisPositionInside(PortAxis.Shuttle_X, Target, 0.1f) &&
                                    IsLPOppositeAngle())
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                }
                            }
                        }
                        break;
                    case PortAlarm.Port_Area_Sensor_Error:
                        {
                            WatchdogList eWatchdogPortArea_Timer = WatchdogList.PortArea_Timer;
                            WatchdogList eWatchdogPortArea_ReleaseTimer = WatchdogList.PortArea_Release_Timer;
                            WatchdogList eWatchdogPortArea_And_ShuttleMovingTimer = WatchdogList.PortArea_And_ShuttleMovingErrorTimer;
                            if (Sensor_LightCurtain)
                            {
                                Watchdog_ResetAndStop(eWatchdogPortArea_ReleaseTimer);
                                Alarm_WatchdogProcess(eWatchdogPortArea_Timer, eAlarm, PortAlarmStopCase.None);

                                if (Carrier_CheckShuttle_ExistProduct(true) && IsPortBusy())
                                {
                                    Alarm_WatchdogProcess(eWatchdogPortArea_And_ShuttleMovingTimer, eAlarm, PortAlarmStopCase.None);
                                }
                                else
                                    Watchdog_Stop(eWatchdogPortArea_And_ShuttleMovingTimer, true);
                            }
                            else
                            {
                                Watchdog_Stop(eWatchdogPortArea_Timer, true);
                                Watchdog_Stop(eWatchdogPortArea_And_ShuttleMovingTimer, true);

                                if(!Watchdog_IsDetect(eWatchdogPortArea_ReleaseTimer))
                                    Watchdog_Start(eWatchdogPortArea_ReleaseTimer);
                                else
                                    Watchdog_Stop(eWatchdogPortArea_ReleaseTimer, false);
                            }
                        }
                        break;
                    case PortAlarm.X_Axis_Crash_Detect_Error:
                        {
                            //셔틀에 제품이 있고 OP에 제품이 있는 상태로 OP 방향으로 이동하는 경우 알람
                            float OPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos));
                            float LPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)));
                            float MiddleLocation = (OPLocation - LPLocation) / 2.0f;
                            if (Motion_TargetPosition(PortAxis.Shuttle_X) >= MiddleLocation)
                            {
                                //Target Position이 중간 이상 위치인 경우 Shuttle에 자재가 있고 OP에 자재가 있으면 에러
                                if (ServoCtrl_GetHomeDone(PortAxis.Shuttle_X) &&
                                    IsPortAxisBusy(PortAxis.Shuttle_X) &&
                                    (ServoCtrl_GetTargetSpeed(PortAxis.Shuttle_X) > 0.0f) &&
                                    Carrier_CheckShuttle_ExistProduct(true) &&
                                    Carrier_CheckOP_ExistProduct(true, false))
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                    CMD_PortStop();
                                }
                            }
                            //셔틀에 제품이 있고 LP에 제품이 있는 상태로 LP 방향으로 이동하는 경우 알람
                            else if (Motion_TargetPosition(PortAxis.Shuttle_X) <= MiddleLocation &&
                                    GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                            {
                                //Target Position이 중간 이하 위치인 경우 Shuttle에 자재가 있고 LP에 자재가 있으면 에러
                                if (ServoCtrl_GetHomeDone(PortAxis.Shuttle_X) &&
                                    IsPortAxisBusy(PortAxis.Shuttle_X) &&
                                    (ServoCtrl_GetTargetSpeed(PortAxis.Shuttle_X) < 0.0f) &&
                                    Carrier_CheckShuttle_ExistProduct(true) &&
                                    Carrier_CheckLP_ExistProduct(true, false))
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                    CMD_PortStop();
                                }
                            }
                        }
                        break;
                    case PortAlarm.Z_Axis_Crash_Detect_Error:
                        {
                            //셔틀에 제품이 있고 OP, LP 위치에서 벗어나 있는 상태로 Z를 하강 시키려 하면 Alarm
                            if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z) &&
                                IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos), 2.0f) &&
                                IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)), 2.0f))
                            {
                                if (ServoCtrl_GetHomeDone(PortAxis.Shuttle_Z) &&
                                    IsPortAxisBusy(PortAxis.Shuttle_Z) &&
                                    (Motion_CurrentPosition(PortAxis.Shuttle_Z) <= GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos) - 10.0f) &&
                                    (ServoCtrl_GetTargetSpeed(PortAxis.Shuttle_Z) < 0.0f) &&
                                    Carrier_CheckShuttle_ExistProduct(true))
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                    CMD_PortStop();
                                }
                            }
                            else if(GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z) &&
                                IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos), 2.0f) &&
                                IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)), 2.0f) &&
                                Carrier_CheckShuttle_ExistProduct(true))
                            {
                                if (CylinderCtrl_GetRunStatus(PortAxis.Shuttle_Z, CylCtrlList.BWD))
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                    CMD_PortStop();
                                }
                            }
                        }
                        break;
                    case PortAlarm.T_Axis_Crash_Detect_Error:
                        {
                            //Wait Pos 위치에서 벗어나 있는 상태로 T축을 회전 시도하면 Alarm
                            if (GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) &&
                                IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Wait_Pos), 10.0f))
                            {
                                if (ServoCtrl_GetHomeDone(PortAxis.Shuttle_T) &&
                                    IsPortAxisBusy(PortAxis.Shuttle_T))
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                    CMD_PortStop();
                                }
                            }
                            else if (!IsZAxisPos_UP(PortAxis.Shuttle_Z) &&
                                    Carrier_CheckShuttle_ExistProduct(true))
                            {
                                if (ServoCtrl_GetHomeDone(PortAxis.Shuttle_T) &&
                                    IsPortAxisBusy(PortAxis.Shuttle_T))
                                {
                                    AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                    CMD_PortStop();
                                }
                            }
                        }
                        break;

                    case PortAlarm.CIM_IF_TimeOut_RF_Read_Req_Error:
                        break;
                    case PortAlarm.CIM_IF_TimeOut_ID_Remove_Error:
                        break;
                    case PortAlarm.RM_PIO_IF_TimeOut_Error:
                        //InsertType Alarm
                        break;
                    case PortAlarm.Port_PIO_IF_TimeOut_Error:
                        //InsertType Alarm
                        break;
                    //case PortAlarm.Port_Stop_From_CIM:
                    //    break;
                    case PortAlarm.Step_TimeOver_Error:
                        //InsertType Alarm
                        break;

                    case PortAlarm.Conveyor_Moving_TimeOut_Error:
                        foreach(BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
                        {
                            if (eBufferCV == BufferCV.Buffer_BP1 || eBufferCV == BufferCV.Buffer_BP2 || eBufferCV == BufferCV.Buffer_BP3 || eBufferCV == BufferCV.Buffer_BP4)
                                continue;

                            if(GetMotionParam().IsCVUsed(eBufferCV))
                            {
                                if(BufferCtrl_CV_Is_Busy(eBufferCV))
                                {
                                    Watchdog_Start(eBufferCV == BufferCV.Buffer_LP ? WatchdogList.Buffer1_CV_Moving_Timer : WatchdogList.Buffer2_CV_Moving_Timer);
                                    if (Watchdog_IsDetect(eBufferCV == BufferCV.Buffer_LP ? WatchdogList.Buffer1_CV_Moving_Timer : WatchdogList.Buffer2_CV_Moving_Timer))
                                    {
                                        AlarmInsert((short)eAlarm, AlarmLevel.Error);
                                        CMD_PortStop();
                                    }
                                }
                                else
                                    Watchdog_Stop(eBufferCV == BufferCV.Buffer_LP ? WatchdogList.Buffer1_CV_Moving_Timer : WatchdogList.Buffer2_CV_Moving_Timer, true);
                            }
                        }
                        break;

                    case PortAlarm.Tag_Read_Fail:
                        //InsertType Alarm
                        break;
                    case PortAlarm.Tag_Disconnection:
                        if (IsAutoControlRun() &&
                            !m_TagReader_Interface.IsConnected())
                        {
                            AlarmInsert((short)eAlarm, AlarmLevel.Error);
                        }
                        break;
                    //case PortAlarm.OMRON_PIO_Comm_Disconnection:
                    //    if (IsAutoControlRun() && !(Master.m_Omron?.IsConnected() ?? false))
                    //        AlarmInsert((short)eAlarm, AlarmLevel.Error);
                    //    break;
                    //case PortAlarm.OMRON_PIO_Invalid_State:
                    //    if(IsAutoControlRun() && !(Master.m_Omron?.m_OmronValid ?? false))
                    //        AlarmInsert((short)eAlarm, AlarmLevel.Error);
                    //    break;

                    case PortAlarm.Buffer_LP_CV_Error:
                    case PortAlarm.Buffer_OP_CV_Error:
                        {
                            BufferCV eBufferCV = BufferCV.Buffer_LP;
                            if (eAlarm == PortAlarm.Buffer_LP_CV_Error)
                                eBufferCV = BufferCV.Buffer_LP;
                            else if (eAlarm == PortAlarm.Buffer_OP_CV_Error)
                                eBufferCV = BufferCV.Buffer_OP;
                            else
                                continue;

                            bool bError = eBufferCV == BufferCV.Buffer_LP ? Sensor_LP_CV_Error : Sensor_OP_CV_Error;


                            if (bError)
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);

                                BufferCtrl_CV_MotionStop(eBufferCV);
                            }
                        }
                        break;
                    case PortAlarm.Buffer_LP_Z_Error:
                    case PortAlarm.Buffer_OP_Z_Error:
                        {
                            PortAxis ePortAxis = PortAxis.Buffer_LP_Z;
                            if (eAlarm == PortAlarm.Buffer_LP_Z_Error)
                                ePortAxis = PortAxis.Buffer_LP_Z;
                            else if (eAlarm == PortAlarm.Buffer_OP_Z_Error)
                                ePortAxis = PortAxis.Buffer_OP_Z;
                            else
                                continue;

                            bool bError = ePortAxis == PortAxis.Buffer_LP_Z ? Sensor_LP_Z_Error : Sensor_OP_Z_Error;


                            if (bError)
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);

                                InverterCtrl_MotionStop(ePortAxis);
                            }
                        }
                        break;

                    case PortAlarm.Buffer_LP_Z_NOT:
                    case PortAlarm.Buffer_OP_Z_NOT:
                        {
                            PortAxis ePortAxis = PortAxis.Buffer_LP_Z;
                            if (eAlarm == PortAlarm.Buffer_LP_Z_NOT)
                                ePortAxis = PortAxis.Buffer_LP_Z;
                            else if (eAlarm == PortAlarm.Buffer_OP_Z_NOT)
                                ePortAxis = PortAxis.Buffer_OP_Z;
                            else
                                continue;

                            bool bNOT = ePortAxis == PortAxis.Buffer_LP_Z ? Sensor_LP_Z_NOT : Sensor_OP_Z_NOT;


                            if (bNOT)
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            }
                        }
                        break;

                    case PortAlarm.Buffer_LP_Z_POT:
                    case PortAlarm.Buffer_OP_Z_POT:
                        {
                            PortAxis ePortAxis = PortAxis.Buffer_LP_Z;
                            if (eAlarm == PortAlarm.Buffer_LP_Z_POT)
                                ePortAxis = PortAxis.Buffer_LP_Z;
                            else if (eAlarm == PortAlarm.Buffer_OP_Z_POT)
                                ePortAxis = PortAxis.Buffer_OP_Z;
                            else
                                continue;

                            bool bPOT = ePortAxis == PortAxis.Buffer_LP_Z ? Sensor_LP_Z_POT : Sensor_OP_Z_POT;


                            if (bPOT)
                            {
                                AlarmInsert((short)eAlarm, AlarmLevel.Error);
                            }
                        }
                        break;
                }
            }
        }
        
        /// <summary>
        /// Port에 발생된 Alarm Code를 Word Map에 맵핑용
        /// PortErrorCode Set시 적용
        /// </summary>
        private void AlarmListWordMapUpdate()
        {
            var m_AlarmList = m_PortAlarm.GetAlarmList();
            short[] ErrorCode = new short[5] { 0, 0, 0, 0, 0};
            for (int nCount = 0; nCount < (m_AlarmList.Count < 5 ? m_AlarmList.Count : 5); nCount++)
                ErrorCode[nCount] = (m_AlarmList[nCount].bClear || m_AlarmList[nCount].eAlarmLevel != AlarmLevel.Error) ? (short)0 : m_AlarmList[nCount].AlarmCode;

            PortErrorCode = ErrorCode;
        }

        /// <summary>
        /// Port의 현재 Alarm List를 가져옴
        /// </summary>
        /// <returns></returns>
        public List<CodeAlarm.CodeAlarmParam> GetAlarmList()
        {
            return m_PortAlarm.GetAlarmList();
        }

        /// <summary>
        /// 가장 최근 발생한 Alarm Code를 가져옴
        /// </summary>
        /// <returns></returns>
        public short GetRecentErrorCode()
        {
            return m_PortAlarm.GetRecentAlarmCode();
        }

        /// <summary>
        /// 가장 최근 발생한 Alarm Level을 가져옴
        /// </summary>
        /// <returns></returns>
        public AlarmLevel GetAlarmLevel()
        {
            return m_PortAlarm.GetRecentAlarmLevel();
        }

        /// <summary>
        /// 특정 알람이 현재 발생된 상태인지 확인
        /// </summary>
        /// <param name="ePortAlarm"></param>
        /// <returns></returns>
        public bool AlarmContains(PortAlarm ePortAlarm)
        {
            return m_PortAlarm.Contains((short)ePortAlarm);
        }

        /// <summary>
        /// 코드의 알람이 Define 되어있는지 확인
        /// </summary>
        /// <param name="AlarmCode"></param>
        /// <returns></returns>
        public PortAlarm IndexToAlarmEnum(short AlarmCode)
        {
            try
            {
                if (Enum.IsDefined(typeof(PortAlarm), (int)AlarmCode))
                    return (PortAlarm)AlarmCode;
                else
                    return (PortAlarm)(-1);
            }
            catch
            {
                return (PortAlarm)(-1);
            }
        }

        /// <summary>
        /// Lang Pack에서 특정 Alarm의 트러블 슈팅 텍스트를 가져옴
        /// </summary>
        /// <param name="ePortAlarm"></param>
        /// <returns></returns>
        public string GetAlarmSolutionText(PortAlarm ePortAlarm)
        {
            return SynusLangPack.GetLanguage($"PortAlarm_{ePortAlarm}_Solution");
        }

        /// <summary>
        /// 최근 발생한 알람의 코멘트를 가져옴
        /// </summary>
        /// <returns></returns>
        public string GetRecentErrorCodeStr()
        {
            short AlarmCode = m_PortAlarm.GetRecentAlarmCode();
            return GetPortAlarmComment(AlarmCode);
        }

        /// <summary>
        /// 알람마다 출력이 정의된 코멘트
        /// </summary>
        /// <param name="AlarmCode"></param>
        /// <returns></returns>
        private string GetPortAlarmComment(short AlarmCode)
        {
            PortAlarm ePortAlarm = (PortAlarm)AlarmCode;

            switch (ePortAlarm)
            {
                case PortAlarm.None:
                    return "None";
                case PortAlarm.Port_E_Stop:
                    return "HW E-Stop";
                case PortAlarm.GOT_E_Stop:
                    return "GOT E-Stop";

                case PortAlarm.Key_Switch_Error:
                    return "Auto Key Switch Error";
                case PortAlarm.Door_Open_Error:
                    return "Door Open Error";
                case PortAlarm.WMX_E_Stop:
                    return "WMX E-Stop";
                case PortAlarm.Software_E_Stop:
                    return "SW E-Stop";
                case PortAlarm.Master_E_Stop:
                    return "Master E-Stop";

                case PortAlarm.Maint_Door_Open_Error:
                    return "Maint Door Open Error";

                case PortAlarm.Power_Off_Error:
                    return "Port Power Off Error";
                case PortAlarm.X_Axis_ServoPack_Battery_Low_Alarm:
                    return "X-Axis Servo Pack Battery Low Alarm";
                case PortAlarm.Z_Axis_ServoPack_Battery_Low_Alarm:
                    return "Z-Axis Servo Pack Battery Low Alarm";
                case PortAlarm.T_Axis_ServoPack_Battery_Low_Alarm:
                    return "T-Axis Servo Pack Battery Low Alarm";

                case PortAlarm.X_Axis_ServoPack_Error:
                    return "X-Axis Servo Pack Error";
                case PortAlarm.X_Axis_POT:
                    return "X-Axis POT";
                case PortAlarm.X_Axis_NOT:
                    return "X-Axis NOT";
                case PortAlarm.X_Axis_Origin_Search_Fail:
                    return "X-Axis Origin Search Fail(Home Sensor)";
                case PortAlarm.X_Axis_Software_Limit_Error:
                    return "X-Axis Software Limit Error";
                case PortAlarm.X_Axis_Dest_Position_Error_Encorder:
                    return "X-Axis Dest Position Error(Encorder)";
                case PortAlarm.X_Axis_Absolute_Origin_Loss:
                    return "X-Axis Absolute Origin Loss";
                case PortAlarm.X_Axis_Dest_Positioning_Complete_Error:
                    return "X-Axis Dest Positioning Complete Error";
                case PortAlarm.X_Axis_OverLoad_Error:
                    return "X-Axis OVER LOAD Error";
                case PortAlarm.X_Axis_HomeSensor_Always_On_Error:
                    return "X-Axis Home Sensor Always on Error";
                case PortAlarm.X_Axis_HomeMove_TimeOutError:
                    return "X-Home Move Time Out Error";
                case PortAlarm.X_Axis_Dest_Position_Error_Sensor:
                    return "X-Axis Dest Position Error(Sensor)";
                case PortAlarm.X_Axis_Move_To_LP_Timeout_Error:
                    return "X-Axis Move To LP Position Timeout Error";
                case PortAlarm.X_Axis_Move_To_Wait_Timeout_Error:
                    return "X-Axis Move To Wait Position Timeout Error";
                case PortAlarm.X_Axis_Move_To_OP_Timeout_Error:
                    return "X-Axis Move To OP Position Timeout Error";

                case PortAlarm.Z_Axis_ServoPack_Error:
                    return "Z-Axis Servo Pack Error";
                case PortAlarm.Z_Axis_POT:
                    return "Z-Axis POT";
                case PortAlarm.Z_Axis_NOT:
                    return "Z-Axis NOT";
                case PortAlarm.Z_Axis_Origin_Search_Fail:
                    return "Z-Axis Origin Search Fail(Home Sensor)";
                case PortAlarm.Z_Axis_Software_Limit_Error:
                    return "Z-Axis Software Limit Error";
                case PortAlarm.Z_Axis_Dest_Position_Error_Encorder:
                    return "Z-Axis Dest Position Error(Encorder)";
                case PortAlarm.Z_Axis_Absolute_Origin_Loss:
                    return "Z-Axis Absolute Origin Loss";
                case PortAlarm.Z_Axis_Dest_Positioning_Complete_Error:
                    return "Z-Axis Dest Positioning Complete Error";
                case PortAlarm.Z_Axis_OverLoad_Error:
                    return "Z-Axis OVER LOAD Error";
                case PortAlarm.Z_Axis_HomeSensor_Always_On_Error:
                    return "Z-Axis Home Sensor Always on Error";
                case PortAlarm.Z_Axis_HomeMove_TimeOutError:
                    return "Z-Home Move Time Out Error";
                case PortAlarm.Z_Axis_Move_To_Up_Timeout_Error:
                    return "Z-Axis Move To Up Position Timeout Error";
                case PortAlarm.Z_Axis_Move_To_Down_Timeout_Error:
                    return "Z-Axis Move To Down Position Timeout Error";

                case PortAlarm.T_Axis_ServoPack_Error:
                    return "T-Axis Servo Pack Error";
                case PortAlarm.T_Axis_POT:
                    return "T-Axis POT";
                case PortAlarm.T_Axis_NOT:
                    return "T-Axis NOT";
                case PortAlarm.T_Axis_Origin_Search_Fail:
                    return "T-Axis Origin Search Fail(Home Sensor)";
                case PortAlarm.T_Axis_Software_Limit_Error:
                    return "T-Axis Software Limit Error";
                case PortAlarm.T_Axis_Dest_Position_Error_Encorder:
                    return "T-Axis Dest Position Error(Encorder)";
                case PortAlarm.T_Axis_Absolute_Origin_Loss:
                    return "T-Axis Absolute Origin Loss";
                case PortAlarm.T_Axis_Dest_Positioning_Complete_Error:
                    return "T-Axis Dest Positioning Complete Error";
                case PortAlarm.T_Axis_OverLoad_Error:
                    return "T-Axis OVER LOAD Error";
                case PortAlarm.T_Axis_HomeSensor_Always_On_Error:
                    return "T-Axis Home Sensor Always on Error";
                case PortAlarm.T_Axis_HomeMove_TimeOutError:
                    return "T-Home Move Time Out Error";
                case PortAlarm.T_Axis_Move_To_0Deg_Timeout_Error:
                    return "T-Axis Move To 0 Deg Position Timeout Error";
                case PortAlarm.T_Axis_Move_To_180Deg_Timeout_Error:
                    return "T-Axis Move To 180 Deg Position Timeout Error";

                case PortAlarm.OHT_Detect_Error:
                    return "OHT Detect Error";
                case PortAlarm.Fork_Detect_Error:
                    return "Fork Detect Error";
                case PortAlarm.OP_Placement_Detect_Error:
                    return "OP Placement Detect Error";
                case PortAlarm.Shuttle_Placement_Detect_Error:
                    return "Shuttle Placement Detect Error";
                case PortAlarm.LP_Placement_Detect_Error:
                    return "LP Placement Detect Error";
                case PortAlarm.Port_Area_Sensor_Error:
                    return "Port Area Sensor Error";
                case PortAlarm.X_Axis_Crash_Detect_Error:
                    return "X-Axis Crash Detect Error";
                case PortAlarm.Z_Axis_Crash_Detect_Error:
                    return "Z-Axis Crash Detect Error";
                case PortAlarm.T_Axis_Crash_Detect_Error:
                    return "T-Axis Crash Detect Error";

                case PortAlarm.OP_No_Cassette_ID_Error:
                    return "OP No Cassette ID Error";
                case PortAlarm.LP_No_Cassette_ID_Error:
                    return "LP No Cassette ID Error";
                case PortAlarm.Shuttle_No_Cassette_ID_Error:
                    return "Shuttle No Cassette ID Error";


                case PortAlarm.LP_Diagonal_Sensor_Error:
                    return "LP Diagonal Sensor Error";
                case PortAlarm.OP_CST_Detect_Sensor_Group_Error:
                    return "OP CST Detect Sensor Group Error";

                case PortAlarm.CIM_IF_TimeOut_RF_Read_Req_Error:
                    return "CIM IF Timeout_RF Read Req Error";
                case PortAlarm.CIM_IF_TimeOut_ID_Remove_Error:
                    return "CIM IF Timeout_ID Remove Error";
                case PortAlarm.RM_PIO_IF_TimeOut_Error:
                    return "RM PIO I/F Timeout Error";
                case PortAlarm.Port_PIO_IF_TimeOut_Error:
                    return "Port PIO I/F Timeout Error";
                //case PortAlarm.Port_Stop_From_CIM:
                //    return "Port Stop From CIM";
                case PortAlarm.Step_TimeOver_Error:
                    return "Step Time Over Error";

                case PortAlarm.Conveyor_Moving_TimeOut_Error:
                    return "Conveyor Move Time Out Error";
                case PortAlarm.Tag_Read_Fail:
                    return "Tag(RFID or BCR) Read Fail";
                case PortAlarm.Tag_Disconnection:
                    return "Tag(RFID or BCR) TCP/IP Disconnection";
                //case PortAlarm.OMRON_PIO_Comm_Disconnection:
                //    return "OMRON PIO COMM Disconnection";
                //case PortAlarm.OMRON_PIO_Invalid_State:
                //    return "OMRON PIO Data Invalid Error";

                case PortAlarm.Buffer_LP_CV_Error:
                    return "LP Buffer Conveyor Error";
                case PortAlarm.Buffer_LP_Z_Error:
                    return "LP Buffer Z Axis Error";
                case PortAlarm.Buffer_LP_Z_POT:
                    return "LP Buffer Z Axis POT";
                case PortAlarm.Buffer_LP_Z_NOT:
                    return "LP Buffer Z Axis NOT";

                case PortAlarm.Buffer_OP_CV_Error:
                    return "OP Buffer Conveyor Error";
                case PortAlarm.Buffer_OP_Z_Error:
                    return "OP Buffer Z Axis Error";
                case PortAlarm.Buffer_OP_Z_POT:
                    return "OP Buffer Z Axis POT";
                case PortAlarm.Buffer_OP_Z_NOT:
                    return "OP Buffer Z Axis NOT";

                default:
                    return $"Reserve Name : Code: {(int)ePortAlarm}, Enum: {ePortAlarm}";
            }
        }
    }
}
