using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Interface.Alarm;

namespace Master.Equipment.Port
{
    /// <summary>
    /// PortInterlock.cs는 STK 명령 수행 시 인터락 처리 작성
    /// 함수 명 : 동작 기능
    /// Check : 인터락 체크 항목 나열
    /// </summary>
    public partial class Port
    {
        /// <summary>
        /// 명령 전송 위치를 나타냄
        /// </summary>
        public enum InterlockFrom
        {
            UI_Event,
            ApplicationLoop,
            TCPIP
        }


        public List<PortAxis> DeadManControlAxis = new List<PortAxis>();    //DeadMan Switch를 눌러 제어하는 경우 기록되며 누름 해제에 따라 정지 시키기 위한 리스트
        public List<BufferCV> DeadManControlBuffer = new List<BufferCV>();  //DeadMan Switch를 눌러 제어하는 경우 기록되며 누름 해제에 따라 정지 시키기 위한 리스트

        /// <summary>
        /// Port의 Control Mode를 지정함
        /// Master Mode : Master의 명령을 받아서 제어 (UI 조작 버튼 활성화)
        /// CIM Mode : CIM의 명령을 받아서 제어 (UI 에서 제어 불가)
        /// </summary>
        /// <param name="eControlMode"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetControlMode(ControlMode eControlMode, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Set Control Mode Interlock Error";

            //if (Check_CurrentMode(eControlMode, ErrorTitleMsg, _InterlockFrom))
            //    return;

            if (IsEQPort())
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_PortSetControlMode(eControlMode);
        }

        /// <summary>
        /// Port의 Auto 공정 시작 명령
        /// 공정 명령 시 안전 상황등을 추가하려면 아래에 Check 함수 추가
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_StartAutoControl(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Start Auto Control Interlock Error";

            if (IsEQPort())
                return;

            if (Check_PortAlarm(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortBusy(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortHomeDone(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRunEnable(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_OHT_Door_Key_Open(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_PortStartAutoControl();
        }

        /// <summary>
        /// Port의 Auto 공정 정지 명령
        /// UI Button으로 부터 온 명령인 경우 메세지 출력
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_StopAutoControl(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Stop Auto Control Interlock Error";

            if (IsEQPort())
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (!IsAutoControlRun())
                return;

            if (_InterlockFrom == InterlockFrom.UI_Event)
            {
                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AutoModeStop"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                    CMD_PortStopAutoControl();
            }
            else
                CMD_PortStopAutoControl();
        }

        /// <summary>
        /// Port의 Power On 명령
        /// 전체 서보앰프가 서보 온 상태로 진입
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_PortPowerOn(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Power On Interlock Error";

            if (IsEQPort())
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PowerOnEnable(ErrorTitleMsg, _InterlockFrom))
                return;

            foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if(GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Servo)
                    ServoCtrl_ServoOn(ePortAxis);
            }
        }

        /// <summary>
        /// Port의 Power Off 명령
        /// 전체 서보 앰프가 서보 오프 상태로 진입
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_PortPowerOff(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Power Off Interlock Error";

            if (IsEQPort())
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PowerOffEnable(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortBusy(ErrorTitleMsg, _InterlockFrom))
                return;

            foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if (GetMotionParam().GetAxisControlType(ePortAxis) == AxisCtrlType.Servo)
                    ServoCtrl_ServoOff(ePortAxis);
            }
        }
    
        /// <summary>
        /// Port의 알람 초기화
        /// 프로그램 상 알람
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_PortAlarmClear(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Alarm Clear Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_PortAlarmClear();
        }

        /// <summary>
        /// Port의 전체 축 알람 초기화
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_PortAmpAlarmClear(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Amp Alarm Clear Interlock Error";

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            foreach(PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if(GetMotionParam().IsValidServo(ePortAxis))
                    ServoCtrl_AlarmClear(ePortAxis);
            }
        }

        /// <summary>
        /// Port의 Cycle 공정 시작
        /// </summary>
        /// <param name="CycleCount"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_StartAutoManualCycleControl(int CycleCount, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Start Manual Auto Control Interlock Error";

            if (Check_PortAlarm(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortHomeDone(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortBusy(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_OHT_Door_Key_Open(ErrorTitleMsg, _InterlockFrom))
                return;

            // 231011 - Door Close 상태에서 Auto 진입 불가한 상황이 없으므로 주석 처리 - Louis
            //if ((GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.OHT) &&
            //    Check_OHT_Door_Close_Status(ErrorTitleMsg, _InterlockFrom) && IsValidInputItemMapping(OHT_InputItem.Door_Close_Status.ToString()))
            //    return;

            CMD_PortStartAutoManualCycleControl(CycleCount);
        }
    
        /// <summary>
        /// Port의 Cycle 공정 정지
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_StopAutoManualCycleControl(InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Stop Auto Manual Cycle Control Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (!IsAutoManualCycleRun())
                return;

            if (_InterlockFrom == InterlockFrom.UI_Event)
            {
                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AutoManualCycleModeStop"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                    CMD_PortStopAutoManualCycleControl();
            }
            else
                CMD_PortStopAutoManualCycleControl();
        }
        
        /// <summary>
        /// Port의 Auto 공정 속도 비율 설정
        /// </summary>
        /// <param name="SpeedPercent"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetAutoRunSpeed(int SpeedPercent, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Set Auto Run Speed Adjust Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetAutoRunSpeed(SpeedPercent);
        }
    
        /// <summary>
        /// Port의 공정 방향 변경
        /// </summary>
        /// <param name="ePortDirection"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AutoControlDirectionChange(PortDirection ePortDirection, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Auto Control Direction Change Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_DirectionChangeEnable(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_PortSetAutoControlDirection(ePortDirection);
        }

        /// <summary>
        /// Port의 Tag Reader Option 설정
        /// Tag Read 실패 시 알람 or 경고 후 계속 진행
        /// </summary>
        /// <param name="bOption"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_TagReadFailErrorOptionChange(bool bOption, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Tag Read Fail Option Change Interlock Error";

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;


            CMD_PortTagReadFailErrorOption(bOption);
        }

        /// <summary>
        /// Port의 Operation Mode를 설정 (듀얼 모드인 경우들)
        /// </summary>
        /// <param name="ePortOperationMode"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_OperationModeChange(PortOperationMode ePortOperationMode, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Operation Mode Change Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_ModeChangeEnable(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_PortSetOperationMode(ePortOperationMode);
        }
    
        /// <summary>
        /// Port의 특정 축 서보 온
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisServoOn(PortAxis ePortAxis, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Servo On Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            ServoCtrl_ServoOn(ePortAxis);
        }
        
        /// <summary>
        /// Port의 특정 축 서보 오프
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisServoOff(PortAxis ePortAxis, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Servo Off Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            ServoCtrl_ServoOff(ePortAxis);
        }
        
        /// <summary>
        /// Port의 특정 축 Homing 동작 진행
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisStartHoming(PortAxis ePortAxis, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Start Homing Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                return;

            bool AbsoluteEncorderMode = m_PortMotionParameter.GetShuttleCtrl_ServoParam(ePortAxis).WMXParam.m_absEncoderMode;
            

            if (ServoCtrl_GetHomeDone(ePortAxis) && !AbsoluteEncorderMode)
            {
                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisalreadyHomeDone"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{GetParam().ID}] Home Done Clear Click");
                    ServoCtrl_HomeDoneClear(ePortAxis);
                }
            }
            else
                ServoCtrl_StartHoming(ePortAxis);
        }
        
        /// <summary>
        /// Port의 특정 축 Jog 동작 진행
        /// </summary>
        /// <param name="tbx"></param>
        /// <param name="ePortAxis"></param>
        /// <param name="bLowSpeed"></param>
        /// <param name="velocity"></param>
        /// <param name="bDirection"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisStartJog(ref TextBox tbx, PortAxis ePortAxis, bool bLowSpeed, float velocity, bool bDirection, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Start Jog Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_Axis_Limit_WithMovingDirection(ePortAxis, bDirection, ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_ValidJogSpeedValue(ref tbx, ePortAxis, bLowSpeed, velocity, ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_X_Axis_WaitPos(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                return;

            if (!bDirection)
            {
                if (Check_Z_Axis_Down_Enable(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            if (Check_T_Axis_ParallelPos(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                return;

            if (ePortAxis == PortAxis.Shuttle_X)
            {
                float OPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos));
                float LPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)));
                float MiddleLocation = (OPLocation - LPLocation) / 2.0f;

                if (bDirection && 
                    AlarmContains(PortAlarm.X_Axis_Crash_Detect_Error) && 
                    (Motion_TargetPosition(ePortAxis) >= MiddleLocation))
                {
                    if (Check_X_Axis_MoveTo_OPLP_Enable(Teaching_X_Pos.OP_Pos, ErrorTitleMsg, _InterlockFrom))
                        return;
                }
                else if (!bDirection &&
                    AlarmContains(PortAlarm.X_Axis_Crash_Detect_Error) &&
                    (Motion_TargetPosition(ePortAxis) <= MiddleLocation))
                {
                    if (Check_X_Axis_MoveTo_OPLP_Enable(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos, ErrorTitleMsg, _InterlockFrom))
                        return;
                }

                if (bDirection &&
                    (Motion_TargetPosition(ePortAxis) >= MiddleLocation))
                {
                    if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                        return;
                }
            }

            if (ePortAxis == PortAxis.Shuttle_T)
            {
                if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            ServoCtrl_StartJog(ePortAxis, velocity, bDirection);

            if(bLowSpeed)
                SetUIJogLowSpeed(ePortAxis, velocity);
            else
                SetUIJogHighSpeed(ePortAxis, velocity);
        }
        
        /// <summary>
        /// Port의 특정 축 인칭 동작 진행(상대 위치 제어, 내 현재 위치에서 얼마나 갈 건지)
        /// </summary>
        /// <param name="tbx"></param>
        /// <param name="ePortAxis"></param>
        /// <param name="inching"></param>
        /// <param name="bDirection"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisInchingMove(ref TextBox tbx, PortAxis ePortAxis, double inching, bool bDirection, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Start Inching Move Interlock Error";

            if (Check_ValidInchingValue(ref tbx, inching, ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortHomeDone(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))

                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (!m_bInterlockOff)
            {

                if (Check_Axis_Limit_WithMovingDirection(ePortAxis, bDirection, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_X_Axis_WaitPos(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;


                if (!bDirection)
                {
                    if (Check_Z_Axis_Down_Enable(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                        return;
                }

                if (Check_T_Axis_ParallelPos(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (ePortAxis == PortAxis.Shuttle_T)
                {
                    if (Check_Crash_Interlock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                        return;

                    if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                        return;

                }
                if (ePortAxis == PortAxis.Shuttle_X)
                {
                    float OPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos));
                    float LPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)));
                    float MiddleLocation = (OPLocation - LPLocation) / 2.0f;

                    if (bDirection &&
                        ((Motion_TargetPosition(ePortAxis) + (inching * (bDirection ? 1.0f : -1.0f))) >= MiddleLocation))
                    {
                        if (Check_X_Axis_MoveTo_OPLP_Enable(Teaching_X_Pos.OP_Pos, ErrorTitleMsg, _InterlockFrom))
                            return;
                    }
                    else if (!bDirection &&
                        ((Motion_TargetPosition(ePortAxis) + (inching * (bDirection ? 1.0f : -1.0f))) <= MiddleLocation))
                    {
                        if (Check_X_Axis_MoveTo_OPLP_Enable(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos, ErrorTitleMsg, _InterlockFrom))
                            return;
                    }

                    if (bDirection &&
                        ((Motion_TargetPosition(ePortAxis) + (inching * (bDirection ? 1.0f : -1.0f))) >= MiddleLocation))
                    {
                        if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                            return;
                    }
                }
            }
            ServoCtrl_StartInchingMove(ePortAxis, inching, bDirection);
            SetUIInchingValue(ePortAxis, (float)inching);
        }
        
        /// <summary>
        /// Port의 특정 축 타겟 동작 진행(절대 위치 제어, 주어진 목표 위치로 가는 것)
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="Target"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisTargetMove(PortAxis ePortAxis, double Target, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Start Target Move Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortHomeDone(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (!m_bInterlockOff)
            {
                if (Check_Axis_Limit_WithOutMovingDirection(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_X_Axis_WaitPos(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Target - ServoCtrl_GetProfileTargetPosition(ePortAxis) < 0.0f)
                {
                    if (Check_Z_Axis_Down_Enable(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                        return;
                }

                if (Check_T_Axis_ParallelPos(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (ePortAxis == PortAxis.Shuttle_T)
                {
                    if (Check_Crash_Interlock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                        return;

                    if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                        return;
                }

                if (ePortAxis == PortAxis.Shuttle_X)
                {
                    float OPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos));
                    float LPLocation = (GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)));
                    float MiddleLocation = (OPLocation - LPLocation) / 2.0f;

                    if (Target >= MiddleLocation)
                    {
                        if (Check_X_Axis_MoveTo_OPLP_Enable(Teaching_X_Pos.OP_Pos, ErrorTitleMsg, _InterlockFrom))
                            return;
                    }
                    else if (Target <= MiddleLocation)
                    {
                        if (Check_X_Axis_MoveTo_OPLP_Enable(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos, ErrorTitleMsg, _InterlockFrom))
                            return;
                    }

                    if (Target >= MiddleLocation)
                    {
                        if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                            return;
                    }
                }
            }

            ServoCtrl_StartTargetMove(ePortAxis, Target);

            SetUITargetValue(ePortAxis, (float)Target);
        }
        
        /// <summary>
        /// Port의 특정 축 알람 클리어
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisAmpAlarmClear(PortAxis ePortAxis, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Amp Alarm Clear Interlock Error";

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            ServoCtrl_AlarmClear(ePortAxis);
        }

        /// <summary>
        /// Port의 특정 축 정지 명령
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AxisStop(PortAxis ePortAxis, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Stop Interlock Error";

            if (!IsAutoRun)
            {
                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            ServoCtrl_MotionStop(ePortAxis);
        }
        
        /// <summary>
        /// X 티칭 위치 이동 동작 명령
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eTeaching_X_Pos"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_X_Axis_Move_To_TeachingPos(PortAxis ePortAxis, Teaching_X_Pos eTeaching_X_Pos, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"X Axis Teaching Pos {eTeaching_X_Pos} Move Interlock Error";

            if(!IsAutoRun)//Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortHomeDone(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (!Check_ServoType(ePortAxis, true))
                    return;

                if (!Check_ValidAxisNum(ePortAxis, true))
                    return;

                if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            if (ePortAxis == PortAxis.Shuttle_X)
            {
                if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            if (Check_X_Axis_MoveTo_OPLP_Enable(eTeaching_X_Pos, ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_X_Axis_Move_To_TeachingPos(eTeaching_X_Pos, IsAutoRun);
        }
        
        /// <summary>
        /// Z 티칭 위치 이동 동작 명령
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eTeaching_Z_Pos"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_Z_Axis_Move_To_TeachingPos(PortAxis ePortAxis, Teaching_Z_Pos eTeaching_Z_Pos, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Z Axis Teaching Pos {eTeaching_Z_Pos} Move Interlock Error";

            if (!IsAutoRun)//Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortHomeDone(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (!Check_ServoType(ePortAxis, true))
                    return;

                if (!Check_ValidAxisNum(ePortAxis, true))
                    return;

                if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            if (Check_T_Axis_ParallelPos(PortAxis.Shuttle_Z, ErrorTitleMsg, _InterlockFrom))
                return;

            if (eTeaching_Z_Pos == Teaching_Z_Pos.Down_Pos)
            {
                if (Check_Z_Axis_Down_Enable(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            CMD_Z_Axis_Move_To_TeachingPos(eTeaching_Z_Pos, IsAutoRun);
        }
        
        /// <summary>
        /// T 티칭 위치 이동 동작 명령
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eTeaching_T_Pos"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_T_Axis_Move_To_TeachingPos(PortAxis ePortAxis, Teaching_T_Pos eTeaching_T_Pos, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"T Axis Teaching Pos {eTeaching_T_Pos} Move Interlock Error";

            if (!IsAutoRun)//Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (!Check_ServoType(ePortAxis, true))
                    return;

                if (Check_PortHomeDone(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (!Check_ValidAxisNum(ePortAxis, true))
                    return;

                if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_Crash_Interlock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_Z_Axis_Up_Status(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            if (Check_X_Axis_WaitPos(PortAxis.Shuttle_T, ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_T_Axis_Move_To_TeachingPos(eTeaching_T_Pos, IsAutoRun);
        }
        
        /// <summary>
        /// Cylinder 모션 동작
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eCylCtrlList"></param>
        /// <param name="bEnable"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetCylinderMove(PortAxis ePortAxis, CylCtrlList eCylCtrlList, bool bEnable, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Cylinder {eCylCtrlList} Move Interlock Error";

            if (!IsAutoRun)//Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_IOValid(ePortAxis, eCylCtrlList, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            if (Check_T_Axis_ParallelPos(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                return;

            if (eCylCtrlList == CylCtrlList.BWD)
            {
                if (Check_Z_Axis_Down_Enable(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            if(ePortAxis == PortAxis.Buffer_OP_Z && eCylCtrlList == CylCtrlList.BWD && bEnable == true)
            {
                if(GetMotionParam().IsCylinderType(PortAxis.Buffer_OP_Y))
                {
                    if (Check_Y_Axis_FWD_State(ErrorTitleMsg, _InterlockFrom))
                        return;
                }
            }

            if (ePortAxis == PortAxis.Buffer_OP_Y && eCylCtrlList == CylCtrlList.FWD && bEnable == true)
            {
                if (GetMotionParam().IsCylinderType(PortAxis.Buffer_OP_Z))
                {
                    if (Check_Z_Axis_FWD_State(ErrorTitleMsg, _InterlockFrom))
                        return;
                }
            }

            CylinderCtrl_SetRunFlag(ePortAxis, eCylCtrlList, bEnable);
        }
        
        /// <summary>
        /// Cylinder 정지 동작
        /// Cylinder는 사실상 정지가 안 됨 (Bit Off만 해주는 기능)
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_CylinderMotionStop(PortAxis ePortAxis, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Cylinder Stop Interlock Error";

            if (!IsAutoRun)
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            CylinderCtrl_MotionStop(ePortAxis);
        }

        /// <summary>
        /// Inverter Motion 동작
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eInvCtrlType"></param>
        /// <param name="bEnable"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetInverterMove(PortAxis ePortAxis, InvCtrlType eInvCtrlType, bool bEnable, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Cylinder {eInvCtrlType} Move Interlock Error";

            if (!IsAutoRun)//Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_DeadMan_ControlLock(ePortAxis, ErrorTitleMsg, _InterlockFrom))
                    return;
                //if (Check_IOValid(ePortAxis, eInvCtrlList, ErrorTitleMsg, _InterlockFrom))
                //    return;
            }

            if(ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
            {
                bool bPOT = ePortAxis == PortAxis.Buffer_LP_Z ? Sensor_LP_Z_POT : Sensor_OP_Z_POT;
                bool bNOT = ePortAxis == PortAxis.Buffer_LP_Z ? Sensor_LP_Z_NOT : Sensor_OP_Z_NOT;

                if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
                {
                    if (eInvCtrlType == InvCtrlType.HighSpeedFWD || eInvCtrlType == InvCtrlType.LowSpeedFWD)
                    {
                        if (bPOT)
                        {
                            string Axis = ePortAxis == PortAxis.Buffer_LP_Z ? "LP Z Axis" : "OP Z Axis";
                            string ErrorInfoMsg = $"{Axis} is HW Switch Limit(POT) Location";

                            if (_InterlockFrom == InterlockFrom.UI_Event)
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                            return;
                        }
                    }
                    else if (eInvCtrlType == InvCtrlType.HighSpeedBWD || eInvCtrlType == InvCtrlType.LowSpeedBWD)
                    {
                        if (bNOT)
                        {
                            string Axis = ePortAxis == PortAxis.Buffer_LP_Z ? "LP Z Axis" : "OP Z Axis";
                            string ErrorInfoMsg = $"{Axis} is HW Switch Limit(NOT) Location";

                            if (_InterlockFrom == InterlockFrom.UI_Event)
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                            return;
                        }
                    }
                }
                else
                {
                    if (eInvCtrlType == InvCtrlType.FreqFWD)
                    {
                        if (bPOT)
                        {
                            string Axis = ePortAxis == PortAxis.Buffer_LP_Z ? "LP Z Axis" : "OP Z Axis";
                            string ErrorInfoMsg = $"{Axis} is HW Switch Limit(POT) Location";

                            if (_InterlockFrom == InterlockFrom.UI_Event)
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                            return;
                        }
                    }
                    else if (eInvCtrlType == InvCtrlType.FreqBWD)
                    {
                        if (bNOT)
                        {
                            string Axis = ePortAxis == PortAxis.Buffer_LP_Z ? "LP Z Axis" : "OP Z Axis";
                            string ErrorInfoMsg = $"{Axis} is HW Switch Limit(NOT) Location";

                            if (_InterlockFrom == InterlockFrom.UI_Event)
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                            return;
                        }
                    }
                }
            }

            InverterCtrl_SetRunFlag(ePortAxis, eInvCtrlType, bEnable);
        }
        
        /// <summary>
        /// Inverter Motion 정지 동작
        /// 모든 비트를 Off
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_InverterMotionStop(PortAxis ePortAxis, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Inverter Stop Interlock Error";

            if (!IsAutoRun)
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            InverterCtrl_MotionStop(ePortAxis);
        }

        /// <summary>
        /// Inverter Error Reset Bit가 맵핑되어 있는 경우 Reset Bit On
        /// 맵핑만 되어 있고 실제 사용해본 적 없음
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="bEnable"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_InverterReset(PortAxis ePortAxis, bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Inverter Reset Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            InverterCtrl_SetResetFlag(ePortAxis, bEnable);
        }

        /// <summary>
        /// Stopper 제어 진행 (Cylinder 제어)
        /// 맵핑만 되어 있고 실제 사용해본 적 없음
        /// </summary>
        /// <param name="eBufferCV"></param>
        /// <param name="eCylCtrlList"></param>
        /// <param name="bEnable"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetStopperMove(BufferCV eBufferCV, CylCtrlList eCylCtrlList, bool bEnable, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eBufferCV} Stopper {eCylCtrlList} Move Interlock Error";

            if (!IsAutoRun) //Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_IOValid(eBufferCV, eCylCtrlList, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_DeadMan_ControlLock(eBufferCV, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            BufferCtrl_Stopper_SetRunFlag(eBufferCV, eCylCtrlList, bEnable);
        }

        /// <summary>
        /// Stopper Motion 정지 (Cylinder 제어)
        /// 맵핑만 되어 있고 실제 사용해본 적 없음
        /// </summary>
        /// <param name="eBufferCV"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_StopperMotionStop(BufferCV eBufferCV, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eBufferCV} Stopper Stop Interlock Error";

            if (!IsAutoRun)
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            BufferCtrl_Stopper_MotionStop(eBufferCV);
        }

        /// <summary>
        /// Centering 제어 진행 (Cylinder 제어)
        /// 맵핑만 되어 있고 실제 사용해본 적 없음
        /// </summary>
        /// <param name="eBufferCV"></param>
        /// <param name="eCylCtrlList"></param>
        /// <param name="bEnable"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetCenteringMove(BufferCV eBufferCV, CylCtrlList eCylCtrlList, bool bEnable, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eBufferCV} Centering {eCylCtrlList} Move Interlock Error";

            if (!IsAutoRun) //Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_IOValid(eBufferCV, eCylCtrlList, ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_DeadMan_ControlLock(eBufferCV, ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            BufferCtrl_Centering_SetRunFlag(eBufferCV, eCylCtrlList, bEnable);
        }

        /// <summary>
        /// Centering 제어 정지 (Cylinder 제어)
        /// 맵핑만 되어 있고 실제 사용해본 적 없음
        /// </summary>
        /// <param name="eBufferCV"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_CenteringMotionStop(BufferCV eBufferCV, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eBufferCV} Centering Stop Interlock Error";

            if (!IsAutoRun)
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            BufferCtrl_Centering_MotionStop(eBufferCV);
        }

        /// <summary>
        /// Conveyor Motion 동작 (Inverter 제어)
        /// </summary>
        /// <param name="eBufferCV"></param>
        /// <param name="eInvCtrlType"></param>
        /// <param name="bEnable"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetConveyorMove(BufferCV eBufferCV, InvCtrlType eInvCtrlType, bool bEnable, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eBufferCV} Conveyor {eInvCtrlType} Move Interlock Error";

            if (!IsAutoRun) //Auto Control Thread 에서 주어지는 명령인지 체크
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_PortEStopAlarm(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_DeadMan_ControlLock(eBufferCV, ErrorTitleMsg, _InterlockFrom))
                    return;

                //if (Check_IOValid(eBufferCV, eInvCtrlType, ErrorTitleMsg, _InterlockFrom))
                //    return;
            }

            BufferCtrl_CV_SetRunFlag(eBufferCV, eInvCtrlType, bEnable);
        }
        
        /// <summary>
        /// conveyor Motion 정지 (Inverter 제어)
        /// </summary>
        /// <param name="eBufferCV"></param>
        /// <param name="IsAutoRun"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_ConveyorMotionStop(BufferCV eBufferCV, bool IsAutoRun, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eBufferCV} Conveyor Stop Interlock Error";

            if (!IsAutoRun)
            {
                if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;

                if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                    return;
            }

            BufferCtrl_CV_MotionStop(eBufferCV);
        }
        
        /// <summary>
        /// Conveyor Inverter Reset Bit 맵핑되어 있는 경우 On
        /// </summary>
        /// <param name="eBufferCV"></param>
        /// <param name="bEnable"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_ConveyorReset(BufferCV eBufferCV, bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{eBufferCV} Conveyor Reset Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            BufferCtrl_CV_SetResetFlag(eBufferCV, bEnable);
        }

        /// <summary>
        /// Servo Amp 사용 시 과부하 검출 값을 지정
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="value"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetOverLoadValue(PortAxis ePortAxis, short value, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Set Over Load Value Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_PortOverLoadLimit(value, ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetOverLoadValue(ePortAxis, value);
        }
        
        /// <summary>
        /// 검출된 과부하 값을 초기화
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_SetOverClear(PortAxis ePortAxis, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ePortAxis} Set Over Load Clear Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            CMD_SetOverLoadClear(ePortAxis);
        }

        /// <summary>
        /// Port -> STK PIO 수동 조작
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_Manual_PIO_PortToRM_LoadREQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [RM] - Load-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToSTK_Load_Req = bEnable;
        }
        public void Interlock_Manual_PIO_PortToRM_UnloadREQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [RM] - Unload-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToSTK_Unload_Req = bEnable;
        }
        public void Interlock_Manual_PIO_PortToRM_Ready(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [RM] - Ready Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToSTK_Ready = bEnable;
        }
        public void Interlock_Manual_PIO_PortToRM_Error(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [RM] - Error Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToSTK_Error = bEnable;
        }

        /// <summary>
        /// STK -> Port -> EQ PIO 수동 조작
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_Manual_PIO_STKToEQ_TR_REQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [EQ] - TR-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_STKToPort_TR_REQ = bEnable;
        }
        public void Interlock_Manual_PIO_STKToEQ_BUSY(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [EQ] - Busy Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_STKToPort_Busy = bEnable;
        }
        public void Interlock_Manual_PIO_STKToEQ_Complete(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [EQ] - Complete Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_STKToPort_Complete = bEnable;
        }
        public void Interlock_Manual_PIO_STKToEQ_Error(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [EQ] - STK Error Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_STKToPort_STKError = bEnable;
        }

        /// <summary>
        /// Port -> OHT PIO 수동 조작
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_Manual_PIO_PortToOHT_LoadREQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OHT] - Load-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOHT_Load_Req = bEnable;
        }
        public void Interlock_Manual_PIO_PortToOHT_UnloadREQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OHT] - Unload-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOHT_Unload_Req = bEnable;
        }
        public void Interlock_Manual_PIO_PortToOHT_Ready(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OHT] - Ready Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOHT_Ready = bEnable;
        }
        public void Interlock_Manual_PIO_PortToOHT_ES(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OHT] - ES Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOHT_ES = bEnable;
        }
        public void Interlock_Manual_PIO_PortToOHT_HO_AVBL(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OHT] - HO_AVBL Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOHT_HO_AVBL = bEnable;
        }

        /// <summary>
        /// Port -> AGV PIO 수동 조작
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_Manual_PIO_PortToAGV_LoadREQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [AGV] - Load-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToAGV_Load_Req = bEnable;
        }
        public void Interlock_Manual_PIO_PortToAGV_UnloadREQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [AGV] - Unload-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToAGV_Unload_Req = bEnable;
        }
        public void Interlock_Manual_PIO_PortToAGV_Ready(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [AGV] - Ready Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToAGV_Ready = bEnable;
        }
        public void Interlock_Manual_PIO_PortToAGV_ES(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [AGV] - ES Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToAGV_ES = bEnable;
        }

        /// <summary>
        /// Port -> OMRON PIO 수동 조작
        /// </summary>
        /// <param name="bEnable"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_Manual_PIO_PortToOMRON_TR_REQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OMRON] - TR-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOMRON_TR_REQ = bEnable;
        }
        public void Interlock_Manual_PIO_PortToOMRON_Busy_REQ(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OMRON] - Busy-REQ Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOMRON_Busy_REQ = bEnable;
        }
        public void Interlock_Manual_PIO_PortToOMRON_Complete(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OMRON] - Complete Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOMRON_Complete = bEnable;
        }
        public void Interlock_Manual_PIO_PortToOMRON_Error(bool bEnable, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Manual PIO [OMRON] - Err Flag Interlock Error";

            if (Check_CIMMode(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            PIOStatus_PortToOMRON_Error = bEnable;
        }

        /// <summary>
        /// Port Parameter Save 명령
        /// </summary>
        /// <param name="SaveObjectName"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        public bool Interlock_ParameterSave(string SaveObjectName, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{SaveObjectName} Save or Apply Interlock Error";

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            if (_InterlockFrom == InterlockFrom.UI_Event)
            {
                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_ApplyAndSaveResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (result != DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port[{GetParam().ID}] {SaveObjectName} Save or Apply Cancel");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Port Parameter Apply 명령
        /// </summary>
        /// <param name="SaveObjectName"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        public bool Interlock_ParameterApply(string SaveObjectName, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{SaveObjectName} Apply Interlock Error";

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            if (_InterlockFrom == InterlockFrom.UI_Event)
            {
                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_ApplyResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (result != DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port[{GetParam().ID}] {SaveObjectName} Apply Cancel");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Port Parameter 새로고침 인터락 (공정 중 변하지 않도록)
        /// </summary>
        /// <param name="SaveObjectName"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        public bool Interlock_ParameterRefresh(string SaveObjectName, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{SaveObjectName} Refresh Interlock Error";

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            if (_InterlockFrom == InterlockFrom.UI_Event)
            {
                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_RefreshResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (result != DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port[{GetParam().ID}] {SaveObjectName} Refresh Cancel");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Port Parameter Clear 인터락 (공정 중 변하지 않도록)
        /// 프로세스 상 1회 우선 체크하도록 구성
        /// </summary>
        /// <param name="ObjectName"></param>
        /// <param name="_InterlockFrom"></param>
        /// <returns></returns>
        public bool Interlock_ParameterClear(string ObjectName, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"{ObjectName} Interlock Error";

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return false;

            return true;
        }

        /// <summary>
        /// 매뉴얼 알람 생성 (Port -> Error Info 메뉴에서 Cell 마우스 우클릭 시 기능)
        /// </summary>
        /// <param name="AlarmCode"></param>
        /// <param name="_InterlockFrom"></param>
        public void Interlock_AlarmCreate(short AlarmCode, InterlockFrom _InterlockFrom)
        {
            string ErrorTitleMsg = $"Alarm Manual Create Interlock Error";

            if (Check_AutoControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            if (Check_CycleControlRun(ErrorTitleMsg, _InterlockFrom))
                return;

            AlarmInsert(AlarmCode, AlarmLevel.Error);
        }

        //아래는 특정 상황을 체크하기 위해 만든 함수
        private bool Check_CIMMode(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (m_eControlMode == ControlMode.CIMMode && _InterlockFrom != InterlockFrom.TCPIP)
            {
                string ErrorInfoMsg = $"Port is CIM Mode";

                if(_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InCIMMode"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_CycleControlRun(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (IsAutoManualCycleRun())
            {
                string ErrorInfoMsg = $"Port is Cycle Running State";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InCycleControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_PortOverLoadLimit(short value, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!ManagedFile.EquipPortMotionParam.ServoParam.IsValidMaxLoad(value.ToString()))
            {
                string ErrorInfoMsg = $"Port Max Load is OverRange";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_MaxLoadOverRangeError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_PowerOffEnable(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if(!Status_PowerOffEnable)
            {
                string ErrorInfoMsg = $"Power Off Enable State Not Ready";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_PowerOnEnable(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_PowerOnEnable)
            {
                if (IsPortPowerOn())
                {
                    string ErrorInfoMsg = $"Already Power On State";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AlreadyPowerOnState"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
                else
                {
                    string ErrorInfoMsg = $"Power On Enable State Not Ready";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
            }

            return false;
        }
        private bool Check_AutoControlRunEnable(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_RunEnable)
            {
                string ErrorInfoMsg = $"Port is not Auto Control Run Enable";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_AutoControlRun(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (IsAutoControlRun())
            {
                string ErrorInfoMsg = $"Port is Auto Control Running State";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InAutoControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_PortBusy(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (IsPortBusy())
            {
                string ErrorInfoMsg = $"Port is Busy State";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InBusyState"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_PortHomeDone(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if(!IsPortHomeDone())
            {
                string ErrorInfoMsg = $"Port is Not Home Done State";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_IntNotHomeDone"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_PortAlarm(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (GetAlarmLevel() == AlarmLevel.Error)
            {
                string ErrorInfoMsg = $"Port is Alarm State";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AlarmStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_ModeChangeEnable(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_TypeChangeEnable)
            {
                string ErrorInfoMsg = $"Mode Change Flag Not Ready";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_DirectionChangeEnable(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_DirectionChangeEnable)
            {
                string ErrorInfoMsg = $"Direction Change Flag Not Ready";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_EnableStateError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_PortEStopAlarm(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Status_EStop)
            {
                bool bMasterEStop = Master.mPort_EStop.GetEStopState() == Interface.Safty.EStopState.EStop;
                bool bWMXEStop = Master.IsWMXEStopState();
                bool bPortSWEStop = mSW_EStop.GetEStopState() == Interface.Safty.EStopState.EStop;
                bool bPortHWEStop = mHW_EStop.GetEStopState() == Interface.Safty.EStopState.EStop;
                bool bDTPEStop = Master.mPortHandyTouch_EStop.IsEStop();

                string ErrorInfoMsg = $"Port is E-stop State, " +
                    $"Master EStop:{bMasterEStop}, " +
                    $"WMX EStop:{bWMXEStop}, " +
                    $"Port SW_EStop:{bPortSWEStop}, " +
                    $"Port HW_EStop:{bPortHWEStop}, " +
                    $"Port DTP_EStop:{bDTPEStop}";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InEStopState"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_ValidJogSpeedValue(ref TextBox tbx, PortAxis ePortAxis, bool bLowSpeed, float velocity, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            float AutoRunSpeed = 0.0f;
            float ManualSpeed = 0.0f;

            AutoRunSpeed = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed;
            ManualSpeed = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed;

            if ((velocity > AutoRunSpeed * (bLowSpeed ? 0.5 : 1.0)) && 
                (velocity > ManualSpeed * (bLowSpeed ? 0.5 : 1.0)))
            {
                string ErrorInfoMsg = $"Invalid speed has been set for the port. Auto Run Speed : {AutoRunSpeed}, Manual Speed : {ManualSpeed}, Tryed Speed Value : {velocity}";
                tbx.Text = AutoRunSpeed >= ManualSpeed ? (AutoRunSpeed * (bLowSpeed ? 0.5 : 1.0)).ToString("0.0") : (ManualSpeed * (bLowSpeed ? 0.5 : 1.0)).ToString("0.0");
                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidSpeedError") + "\nHigh Speed : Below auto or manual speed\nLow Speed : Below auto or manual speed(50%)", SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_IOValid(PortAxis ePortAxis, CylCtrlList eCylCtrlList, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            var IOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.GetCtrlIOParam(eCylCtrlList);
            if (!IOParam.IsValidBitRange() || !IOParam.IsValidStartAddrRange())
            {
                string ErrorInfoMsg = $"Port has an invalid IO value.";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidIOParameter"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_IOValid(PortAxis ePortAxis, InvIOCtrlFlag eInvCtrlList, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            var IOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(eInvCtrlList);
            if (!IOParam.IsValidBitRange() || !IOParam.IsValidStartAddrRange())
            {
                string ErrorInfoMsg = $"Port has an invalid IO value.";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidIOParameter"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_IOValid(BufferCV eBufferCV, CylCtrlList eCylCtrlList, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetCtrlIOParam(eCylCtrlList);
            if (!IOParam.IsValidBitRange() || !IOParam.IsValidStartAddrRange())
            {
                string ErrorInfoMsg = $"Port has an invalid IO value.";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidIOParameter"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_IOValid(BufferCV eBufferCV, InvIOCtrlFlag eInvCtrlList, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(eInvCtrlList);
            if (!IOParam.IsValidBitRange() || !IOParam.IsValidStartAddrRange())
            {
                string ErrorInfoMsg = $"Port has an invalid IO value.";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidIOParameter"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_ValidInchingValue(ref TextBox tbx, double Inching, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Inching <= 0)
            {
                string ErrorInfoMsg = $"Invalid Inching Value";
                tbx.Text = "1";
                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidInchingError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        //Status_OHT_Key_Auto_Status
        private bool Check_OHT_Door_Key_Close(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_OHT_Key_Open_Status)
            {
                string ErrorInfoMsg = $"OHT Door Key is in Close Location";
                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_OHTDoorKeyClose"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_OHT_Door_Key_Open(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            // 231011 수정 - Port Mode가 MGV인 경우 Status_OHT_Key_Open_Status가 반전인 것을 지워 Port Type에 관계없이 동일하게 Door Open 상태에서 Interlock 발생하도록 수정 - Louis
            if (Status_OHT_Key_Open_Status && 
                (GetPortOperationMode() == PortOperationMode.OHT || GetPortOperationMode() == PortOperationMode.MGV) &&
                IsValidInputItemMapping(OHT_InputItem.Door_Open_Key_Status.ToString()))
            {
                string ErrorInfoMsg = $"OHT Door Key is in Open Location";
                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_OHTDoorKeyOpen"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_OHT_Door_Close_Status(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!Status_OHT_Door_Close && (GetPortOperationMode() == PortOperationMode.OHT))
            {
                string ErrorInfoMsg = $"OHT Door is Closed";
                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_OHTDoorClose"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }
            else if (Status_OHT_Door_Close && (GetPortOperationMode() == PortOperationMode.MGV))
            {
                string ErrorInfoMsg = $"OHT Door is Closed";
                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_OHTDoorClose"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_Axis_Limit_WithMovingDirection(PortAxis ePortAxis, bool bDirection, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if ((AlarmContains(PortAlarm.X_Axis_POT) && ePortAxis == PortAxis.Shuttle_X && bDirection) ||
                (AlarmContains(PortAlarm.X_Axis_NOT) && ePortAxis == PortAxis.Shuttle_X && !bDirection))
            {
                string ErrorInfoMsg = $"X Axis is HW Switch Limit Location";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }
            else if ((AlarmContains(PortAlarm.Z_Axis_POT) && ePortAxis == PortAxis.Shuttle_Z && bDirection) ||
                (AlarmContains(PortAlarm.Z_Axis_NOT) && ePortAxis == PortAxis.Shuttle_Z && !bDirection))
            {
                string ErrorInfoMsg = $"Z Axis is HW Switch Limit Location";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }
            else if ((AlarmContains(PortAlarm.T_Axis_POT) && ePortAxis == PortAxis.Shuttle_T && bDirection) ||
                (AlarmContains(PortAlarm.T_Axis_NOT) && ePortAxis == PortAxis.Shuttle_T && !bDirection))
            {
                string ErrorInfoMsg = $"T Axis is HW Switch Limit Location";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_Axis_Limit_WithOutMovingDirection(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if ((AlarmContains(PortAlarm.X_Axis_POT) && ePortAxis == PortAxis.Shuttle_X) ||
                (AlarmContains(PortAlarm.X_Axis_NOT) && ePortAxis == PortAxis.Shuttle_X))
            {
                string ErrorInfoMsg = $"X Axis is HW Switch Limit Location";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }
            else if ((AlarmContains(PortAlarm.Z_Axis_POT) && ePortAxis == PortAxis.Shuttle_Z) ||
                (AlarmContains(PortAlarm.Z_Axis_NOT) && ePortAxis == PortAxis.Shuttle_Z))
            {
                string ErrorInfoMsg = $"Z Axis is HW Switch Limit Location";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }
            else if ((AlarmContains(PortAlarm.T_Axis_POT) && ePortAxis == PortAxis.Shuttle_T) ||
                (AlarmContains(PortAlarm.T_Axis_NOT) && ePortAxis == PortAxis.Shuttle_T))
            {
                string ErrorInfoMsg = $"T Axis is HW Switch Limit Location";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Limit_Error"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_Z_Axis_Up_Status(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Carrier_CheckShuttle_ExistProduct(true))
            {
                if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z) &&
                    ServoCtrl_GetHomeDone(PortAxis.Shuttle_Z) &&
                    IsAxisPositionOutside(PortAxis.Shuttle_Z, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos), 0.5f))
                {
                    string ErrorInfoMsg = $"Z Axis is Not Up Pos Location (BP Carrier Exist)";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_Not_Up_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
                else if (GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z) &&
                        !Sensor_Z_Axis_FWDSensor)
                {
                    string ErrorInfoMsg = $"Z Axis is Not Up Pos Location (BP Carrier Exist)";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_Not_Up_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
            }

            return false;
        }

        private bool Check_DeadMan_ControlLock(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if(Master.Is_DeadMan_Status_ControlLock && _InterlockFrom == InterlockFrom.UI_Event)
            {
                string ErrorInfoMsg = $"{ePortAxis} is Dead Man Status On";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_DeadMan_Status"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            if(Master.Is_DeadMan_Status_Control)
                DeadManControlAxis.Add(ePortAxis);

            return false;
        }

        private bool Check_Crash_Interlock(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            string CrashCheckPortID = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).CrashCheckID;
            bool bCrashInterlock = false;

            if (Master.m_Ports.ContainsKey(CrashCheckPortID))
            {
                var port = Master.m_Ports[CrashCheckPortID];


                //if (port.ServoCtrl_GetCurrentPosition(ePortAxis) != 0 || port.ServoCtrl_GetCurrentPosition(ePortAxis) != 180 || port.ServoCtrl_GetBusy(ePortAxis))
                if((port.IsAxisPositionOutside(ePortAxis, 0, 10) && port.IsAxisPositionOutside(ePortAxis, 180, 10)) || port.ServoCtrl_GetBusy(ePortAxis))
                    bCrashInterlock = true;
            }

            if(bCrashInterlock && _InterlockFrom == InterlockFrom.UI_Event)
            {
                string ErrorInfoMsg = $"{ePortAxis} is Crash Interlock [Port {CrashCheckPortID}]";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Crash_Interlock_Status"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }
        private bool Check_DeadMan_ControlLock(BufferCV eBufferCV, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (Master.Is_DeadMan_Status_ControlLock && _InterlockFrom == InterlockFrom.UI_Event)
            {
                string ErrorInfoMsg = $"{eBufferCV} is Dead Man Status On";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_DeadMan_Status"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            if (Master.Is_DeadMan_Status_Control)
                DeadManControlBuffer.Add(eBufferCV);

            return false;
        }
        private bool Check_Z_Axis_Down_Enable(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (ePortAxis == PortAxis.Shuttle_Z)
            {
                if(GetMotionParam().IsServoType(PortAxis.Shuttle_X) &&
                    ServoCtrl_GetHomeDone(PortAxis.Shuttle_X) &&
                    !IsAxisPositionInside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos), 2.0f) &&
                    !IsAxisPositionInside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)), 2.0f) &&
                    // ---------------------------------------------------------------------------230731 Updated-----------------------------------------
                    (!IsAxisPositionInside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Wait_Pos), 2.0f) && GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X)) &&
                    Carrier_CheckShuttle_ExistProduct(true))
                {
                    string ErrorInfoMsg = $"The Z axis cannot be moved to the Down position. (BP Carrier Exist)";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_DoNotMove_Down"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
            }

            return false;
        }

        private bool Check_Y_Axis_FWD_State(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if(!IsYAxisPos_BWD(PortAxis.Buffer_OP_Y))
            {
                string ErrorInfoMsg = $"The Y axis is not BWD Location.";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Y_Axis_Not_BWD_Location"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_Z_Axis_FWD_State(string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (!IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
            {
                string ErrorInfoMsg = $"The Z axis is not Up Location.";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_Not_Up_Location"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        //private bool Check_Z_Axis_DownPos(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        //{
        //    if(ePortAxis == PortAxis.Shuttle_Z)
        //    {
        //        if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer && 
        //            GetMotionParam().IsServoType(PortAxis.Shuttle_X))
        //        {
        //            if (GetMotionParam().IsServoType(PortAxis.Shuttle_X) &&
        //                (IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos), 2.0f) && IsAxisPositionInside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos), CasseteSize/2)) ||
        //                (IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.LP_Pos), 2.0f) && IsAxisPositionInside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.LP_Pos), CasseteSize/2)))
        //            {
        //                if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z) && 
        //                    ServoCtrl_GetHomeDone(PortAxis.Shuttle_Z) &&
        //                    (Motion_CurrentPosition(PortAxis.Shuttle_Z) <= GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos) - 10.0f) &&
        //                    Carrier_CheckShuttle_ExistProduct(true))
        //                {
        //                    string ErrorInfoMsg = $"The Z axis cannot be moved to the Down position. (BP Carrier Exist)";

        //                    if (_InterlockFrom == InterlockFrom.UI_Event)
        //                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_DoNotMove_Down"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
        //                    return true;
        //                }
        //                else if (GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z) &&
        //                        Carrier_CheckShuttle_ExistProduct(true))
        //                {
        //                    string ErrorInfoMsg = $"The Z axis cannot be moved to the Down position. (BP Carrier Exist)";

        //                    if (_InterlockFrom == InterlockFrom.UI_Event)
        //                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_DoNotMove_Down"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
        //                    return true;
        //                }
        //            }
        //        }
        //        else if(GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer && 
        //            GetMotionParam().IsServoType(PortAxis.Shuttle_X))
        //        {
        //            if (GetMotionParam().IsServoType(PortAxis.Shuttle_X) && 
        //                (IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos), 2.0f) && IsAxisPositionInside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.OP_Pos), CasseteSize / 2)))
        //            {
        //                if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z) && 
        //                    ServoCtrl_GetHomeDone(PortAxis.Shuttle_Z) &&
        //                    (Motion_CurrentPosition(PortAxis.Shuttle_Z) <= GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos) - 10.0f) &&
        //                    Carrier_CheckShuttle_ExistProduct(true))
        //                {
        //                    string ErrorInfoMsg = $"The Z axis cannot be moved to the Down position. (BP Carrier Exist)";

        //                    if (_InterlockFrom == InterlockFrom.UI_Event)
        //                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_DoNotMove_Down"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
        //                    return true;
        //                }
        //                else if (GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z) &&
        //                        Carrier_CheckShuttle_ExistProduct(true))
        //                {
        //                    string ErrorInfoMsg = $"The Z axis cannot be moved to the Down position. (BP Carrier Exist)";

        //                    if (_InterlockFrom == InterlockFrom.UI_Event)
        //                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_DoNotMove_Down"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}
        private bool Check_X_Axis_MoveTo_OPLP_Enable(Teaching_X_Pos eTeaching_X_Pos, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (GetMotionParam().IsServoType(PortAxis.Shuttle_X) &&
                ServoCtrl_GetHomeDone(PortAxis.Shuttle_X) &&
                Carrier_CheckShuttle_ExistProduct(true) &&
                eTeaching_X_Pos == Teaching_X_Pos.OP_Pos &&
                Carrier_CheckOP_ExistProduct(true, false))
            {
                string ErrorInfoMsg = $"The X axis cannot be moved to the OP position. (OP, BP Carrier Exist)";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_X_Axis_DoNotMove_OP"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }
            else if (GetMotionParam().IsServoType(PortAxis.Shuttle_X) &&
                ServoCtrl_GetHomeDone(PortAxis.Shuttle_X) &&
                Carrier_CheckShuttle_ExistProduct(true) &&
                eTeaching_X_Pos == (IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos) &&
                Carrier_CheckLP_ExistProduct(true, false) &&
                GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
            {
                string ErrorInfoMsg = $"The X axis cannot be moved to the LP position. (LP, BP Carrier Exist)";

                if (_InterlockFrom == InterlockFrom.UI_Event)
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_X_Axis_DoNotMove_LP"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                return true;
            }

            return false;
        }

        private bool Check_X_Axis_WaitPos(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (ePortAxis == PortAxis.Shuttle_T)
            {
                if (GetMotionParam().IsServoType(PortAxis.Shuttle_X) &&
                        GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) &&
                        ServoCtrl_GetHomeDone(PortAxis.Shuttle_X) &&
                        IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)Teaching_X_Pos.Wait_Pos), 0.5f))
                {
                    string ErrorInfoMsg = $"X Axis is Not Wait Pos Location";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_X_Axis_Not_Wait_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
                else if(GetMotionParam().IsServoType(PortAxis.Shuttle_X) &&
                        !GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X) &&
                        ServoCtrl_GetHomeDone(PortAxis.Shuttle_X) &&
                        IsAxisPositionOutside(PortAxis.Shuttle_X, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_X, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)), 0.5f))
                {
                    string ErrorInfoMsg = $"X Axis is Not LP Pos Location";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_X_Axis_Not_LP_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
            }

            return false;
        }

        //private bool Check_Z_Axis_Up(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        //{
        //    if (ePortAxis == PortAxis.Shuttle_T)
        //    {
        //        if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z) &&
        //            ServoCtrl_GetHomeDone(PortAxis.Shuttle_Z) &&
        //            Carrier_CheckShuttle_ExistProduct(true) &&
        //            IsAxisPositionOutside(PortAxis.Shuttle_Z, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos), 0.5f))
        //        {
        //            string ErrorInfoMsg = $"Z Axis is Not Up Pos Location (BP Carrier Exist)";

        //            if (_InterlockFrom == InterlockFrom.UI_Event)
        //                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_Not_Up_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //            Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
        //            return true;
        //        }
        //        else if(GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z) &&
        //                !Sensor_Z_Axis_FWDSensor &&
        //                Carrier_CheckShuttle_ExistProduct(true))
        //        {
        //            string ErrorInfoMsg = $"Z Axis is Not Up Pos Location (BP Carrier Exist)";

        //            if (_InterlockFrom == InterlockFrom.UI_Event)
        //                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_Not_Up_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //            Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
        //            return true;
        //        }
        //    }
        //    else if(ePortAxis == PortAxis.Shuttle_X)
        //    {
        //        if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z) &&
        //            ServoCtrl_GetHomeDone(PortAxis.Shuttle_Z) &&
        //            Carrier_CheckShuttle_ExistProduct(true) &&
        //            IsAxisPositionOutside(PortAxis.Shuttle_Z, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_Z, (int)Teaching_Z_Pos.Up_Pos), 0.5f))
        //        {
        //            string ErrorInfoMsg = $"Z Axis is Not Up Pos Location (BP Carrier Exist)";

        //            if (_InterlockFrom == InterlockFrom.UI_Event)
        //                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_Z_Axis_Not_Up_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //            Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        private bool Check_T_Axis_ParallelPos(PortAxis ePortAxis, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (ePortAxis == PortAxis.Shuttle_Z)
            {
                if (GetMotionParam().IsServoType(PortAxis.Shuttle_T) &&
                    ServoCtrl_GetHomeDone(PortAxis.Shuttle_T) &&
                    IsAxisPositionOutside(PortAxis.Shuttle_T, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos), 0.5f) &&
                    IsAxisPositionOutside(PortAxis.Shuttle_T, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree180_Pos), 0.5f))
                {
                    string ErrorInfoMsg = $"T Axis is Not Parallel Pos Location";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_T_Axis_Not_Parallel_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    
                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
            }

            return false;
        }
        private bool Check_T_Axis_ParallelPos(BufferCV eBufferCV, string ErrorTitleMsg, InterlockFrom _InterlockFrom)
        {
            if (eBufferCV == BufferCV.Buffer_LP)
            {
                if (GetMotionParam().IsServoType(PortAxis.Shuttle_T) &&
                    ServoCtrl_GetHomeDone(PortAxis.Shuttle_T) &&
                    IsAxisPositionOutside(PortAxis.Shuttle_T, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree0_Pos), 0.5f) &&
                    IsAxisPositionOutside(PortAxis.Shuttle_T, GetMotionParam().GetTeachingPos(PortAxis.Shuttle_T, (int)Teaching_T_Pos.Degree180_Pos), 0.5f))
                {
                    string ErrorInfoMsg = $"T Axis is Not Parallel Pos Location";

                    if (_InterlockFrom == InterlockFrom.UI_Event)
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_T_Axis_Not_Parallel_Pos"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    Interlock_LogMessage(_InterlockFrom, ErrorTitleMsg, ErrorInfoMsg);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 상위 명령의 경우 [CIM CMD를 추가하여 로그 기재]
        /// </summary>
        /// <param name="_InterlockFrom"></param>
        /// <param name="ErrorTitleMsg"></param>
        /// <param name="ErrorInfoMsg"></param>
        private void Interlock_LogMessage(InterlockFrom _InterlockFrom, string ErrorTitleMsg, string ErrorInfoMsg)
        {
            if (_InterlockFrom == InterlockFrom.TCPIP)
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.Interlock, $"[CIM CMD] {ErrorTitleMsg}: {ErrorInfoMsg}");
            else
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.Interlock, $"{ErrorTitleMsg}: {ErrorInfoMsg}");
        }
    }
}
