using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Interface.Watchdog;
using System.Drawing;

namespace Master.Equipment.Port
{
    partial class Port
    {
        /// <summary>
        /// 사용하고자 하는 Watchdog List
        /// 추가하고자 하는 경우 해당 Enum에 항목 추가
        /// </summary>
        public enum WatchdogList
        {
            X_Axis_HomingTimer,
            X_Axis_Move_To_LPTimer,
            X_Axis_Move_To_WaitTimer,
            X_Axis_Move_To_OPTimer,
            Z_Axis_HomingTimer,
            Z_Axis_Move_To_UpTimer,
            Z_Axis_Move_To_DownTimer,
            T_Axis_HomingTimer,
            T_Axis_Move_To_0DegTimer,
            T_Axis_Move_To_180DegTimer,
            RM_ForkDetectTimer,
            OHT_HoistDetectTimer,
            OP_Placement_ErrorTimer,
            BP_Placement_ErrorTimer,
            LP_Placement_ErrorTimer,
            PortArea_Timer,
            PortArea_Release_Timer,
            PortArea_And_ShuttleMovingErrorTimer,
            AGVorOHT_PIO_Timer,
            RackMaster_PIO_Timer,
            LP_Step_Timer,
            OP_Step_Timer,
            BP_Step_Timer,
            EQ_Step_Timer,
            Buffer1_CV_Moving_Timer,
            Buffer2_CV_Moving_Timer
        }
        Watchdog[] m_Watchdog;        
        
        /// <summary>
        /// Parameter의 값을 Watchdog Detectime에 적용
        /// </summary>
        public void Watchdog_Refresh_DetectTime()
        {
            var eWatchDogGroup = Enum.GetValues(typeof(WatchdogList));

            foreach (WatchdogList watchdog in eWatchDogGroup)
            {
                int watdogIndex = (int)watchdog;

                m_Watchdog[watdogIndex].SetDetectTime(Watchdog_GetParam_DetectTime(watchdog));
            }
        }
        
        
        /// <summary>
        /// Watchdog Parameter에 Detect Time을 적용
        /// </summary>
        /// <param name="eWatchdogList"></param>
        /// <param name="nWatchdogTime"></param>
        public void Watchdog_SetParam_DetectTime(WatchdogList eWatchdogList, int nWatchdogTime)
        {
            switch (eWatchdogList)
            {
                case WatchdogList.X_Axis_HomingTimer:
                    GetMotionParam().WatchdogDetectParam.X_Axis_HomingTimer = nWatchdogTime;
                    break;
                case WatchdogList.X_Axis_Move_To_LPTimer:
                    GetMotionParam().WatchdogDetectParam.X_Axis_Move_To_LPTimer = nWatchdogTime;
                    break;
                case WatchdogList.X_Axis_Move_To_WaitTimer:
                    GetMotionParam().WatchdogDetectParam.X_Axis_Move_To_WaitTimer = nWatchdogTime;
                    break;
                case WatchdogList.X_Axis_Move_To_OPTimer:
                    GetMotionParam().WatchdogDetectParam.X_Axis_Move_To_OPTimer = nWatchdogTime;
                    break;
                case WatchdogList.Z_Axis_HomingTimer:
                    GetMotionParam().WatchdogDetectParam.Z_Axis_HomingTimer = nWatchdogTime;
                    break;
                case WatchdogList.Z_Axis_Move_To_UpTimer:
                    GetMotionParam().WatchdogDetectParam.Z_Axis_Move_To_UpTimer = nWatchdogTime;
                    break;
                case WatchdogList.Z_Axis_Move_To_DownTimer:
                    GetMotionParam().WatchdogDetectParam.Z_Axis_Move_To_DownTimer = nWatchdogTime;
                    break;
                case WatchdogList.T_Axis_HomingTimer:
                    GetMotionParam().WatchdogDetectParam.T_Axis_HomingTimer = nWatchdogTime;
                    break;
                case WatchdogList.T_Axis_Move_To_0DegTimer:
                    GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_0DegTimer = nWatchdogTime;
                    break;
                case WatchdogList.T_Axis_Move_To_180DegTimer:
                    GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_180DegTimer = nWatchdogTime;
                    break;
                case WatchdogList.RM_ForkDetectTimer:
                    GetMotionParam().WatchdogDetectParam.RM_ForkDetectTimer = nWatchdogTime;
                    break;
                case WatchdogList.OHT_HoistDetectTimer:
                    GetMotionParam().WatchdogDetectParam.OHT_HoistDetectTimer = nWatchdogTime;
                    break;
                case WatchdogList.OP_Placement_ErrorTimer:
                    GetMotionParam().WatchdogDetectParam.OP_Placement_ErrorTimer = nWatchdogTime;
                    break;
                case WatchdogList.BP_Placement_ErrorTimer:
                    GetMotionParam().WatchdogDetectParam.BP_Placement_ErrorTimer = nWatchdogTime;
                    break;
                case WatchdogList.LP_Placement_ErrorTimer:
                    GetMotionParam().WatchdogDetectParam.LP_Placement_ErrorTimer = nWatchdogTime;
                    break;
                case WatchdogList.PortArea_Timer:
                    GetMotionParam().WatchdogDetectParam.PortArea_Timer = nWatchdogTime;
                    break;
                case WatchdogList.PortArea_Release_Timer:
                    GetMotionParam().WatchdogDetectParam.PortArea_ReleaseTimer = nWatchdogTime;
                    break;
                case WatchdogList.PortArea_And_ShuttleMovingErrorTimer:
                    GetMotionParam().WatchdogDetectParam.PortArea_And_ShuttleMovingTimer = nWatchdogTime;
                    break;
                case WatchdogList.AGVorOHT_PIO_Timer:
                    GetMotionParam().WatchdogDetectParam.LP_PIO_Timer = nWatchdogTime;
                    break;
                case WatchdogList.RackMaster_PIO_Timer:
                    GetMotionParam().WatchdogDetectParam.OP_PIO_Timer = nWatchdogTime;
                    break;
                case WatchdogList.LP_Step_Timer:
                    GetMotionParam().WatchdogDetectParam.LP_Step_Timer = nWatchdogTime;
                    break;
                case WatchdogList.BP_Step_Timer:
                    GetMotionParam().WatchdogDetectParam.BP_Step_Timer = nWatchdogTime;
                    break;
                case WatchdogList.OP_Step_Timer:
                    GetMotionParam().WatchdogDetectParam.OP_Step_Timer = nWatchdogTime;
                    break;
                case WatchdogList.EQ_Step_Timer:
                    GetMotionParam().WatchdogDetectParam.EQ_Step_Timer = nWatchdogTime;
                    break;
                case WatchdogList.Buffer1_CV_Moving_Timer:
                    GetMotionParam().WatchdogDetectParam.Buffer1_CV_Moving_Timer = nWatchdogTime;
                    break;
                case WatchdogList.Buffer2_CV_Moving_Timer:
                    GetMotionParam().WatchdogDetectParam.Buffer2_CV_Moving_Timer = nWatchdogTime;
                    break;
            }
        }
       
        
        /// <summary>
       /// Watchdog Parameter에 있는 값을 얻어옴
       /// </summary>
       /// <param name="eWatchdogList"></param>
       /// <returns></returns>
        public int Watchdog_GetParam_DetectTime(WatchdogList eWatchdogList)
        {
            switch (eWatchdogList)
            {
                case WatchdogList.X_Axis_HomingTimer:
                    return GetMotionParam().WatchdogDetectParam.X_Axis_HomingTimer;
                case WatchdogList.X_Axis_Move_To_LPTimer:
                    return GetMotionParam().WatchdogDetectParam.X_Axis_Move_To_LPTimer;
                case WatchdogList.X_Axis_Move_To_WaitTimer:
                    return GetMotionParam().WatchdogDetectParam.X_Axis_Move_To_WaitTimer;
                case WatchdogList.X_Axis_Move_To_OPTimer:
                    return GetMotionParam().WatchdogDetectParam.X_Axis_Move_To_OPTimer;
                case WatchdogList.Z_Axis_HomingTimer:
                    return GetMotionParam().WatchdogDetectParam.Z_Axis_HomingTimer;
                case WatchdogList.Z_Axis_Move_To_UpTimer:
                    return GetMotionParam().WatchdogDetectParam.Z_Axis_Move_To_UpTimer;
                case WatchdogList.Z_Axis_Move_To_DownTimer:
                    return GetMotionParam().WatchdogDetectParam.Z_Axis_Move_To_DownTimer;
                case WatchdogList.T_Axis_HomingTimer:
                    return GetMotionParam().WatchdogDetectParam.T_Axis_HomingTimer;
                case WatchdogList.T_Axis_Move_To_0DegTimer:
                    return GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_0DegTimer;
                case WatchdogList.T_Axis_Move_To_180DegTimer:
                    return GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_180DegTimer;
                case WatchdogList.RM_ForkDetectTimer:
                    return GetMotionParam().WatchdogDetectParam.RM_ForkDetectTimer;
                case WatchdogList.OHT_HoistDetectTimer:
                    return GetMotionParam().WatchdogDetectParam.OHT_HoistDetectTimer;
                case WatchdogList.OP_Placement_ErrorTimer:
                    return GetMotionParam().WatchdogDetectParam.OP_Placement_ErrorTimer;
                case WatchdogList.BP_Placement_ErrorTimer:
                    return GetMotionParam().WatchdogDetectParam.BP_Placement_ErrorTimer;
                case WatchdogList.LP_Placement_ErrorTimer:
                    return GetMotionParam().WatchdogDetectParam.LP_Placement_ErrorTimer;
                case WatchdogList.PortArea_Timer:
                    return GetMotionParam().WatchdogDetectParam.PortArea_Timer;
                case WatchdogList.PortArea_Release_Timer:
                    return GetMotionParam().WatchdogDetectParam.PortArea_ReleaseTimer;
                case WatchdogList.PortArea_And_ShuttleMovingErrorTimer:
                    return GetMotionParam().WatchdogDetectParam.PortArea_And_ShuttleMovingTimer;
                case WatchdogList.AGVorOHT_PIO_Timer:
                    return GetMotionParam().WatchdogDetectParam.LP_PIO_Timer;
                case WatchdogList.RackMaster_PIO_Timer:
                    return GetMotionParam().WatchdogDetectParam.OP_PIO_Timer;
                case WatchdogList.LP_Step_Timer:
                    return GetMotionParam().WatchdogDetectParam.LP_Step_Timer;
                case WatchdogList.BP_Step_Timer:
                    return GetMotionParam().WatchdogDetectParam.BP_Step_Timer;
                case WatchdogList.OP_Step_Timer:
                    return GetMotionParam().WatchdogDetectParam.OP_Step_Timer;
                case WatchdogList.EQ_Step_Timer:
                    return GetMotionParam().WatchdogDetectParam.EQ_Step_Timer;
                case WatchdogList.Buffer1_CV_Moving_Timer:
                    return GetMotionParam().WatchdogDetectParam.Buffer1_CV_Moving_Timer;
                case WatchdogList.Buffer2_CV_Moving_Timer:
                    return GetMotionParam().WatchdogDetectParam.Buffer2_CV_Moving_Timer;
            }
            return 0;
        }
        
        
        /// <summary>
        /// 지정한 Watchdog에 대해 시간 측정 시작
        /// </summary>
        /// <param name="eWatchdogList"></param>
        private void Watchdog_Start(WatchdogList eWatchdogList)
        {
            int watdogIndex = (int)eWatchdogList;

            if (watdogIndex < m_Watchdog.Length)
                m_Watchdog[watdogIndex].StartWatchdog();
        }
        

        /// <summary>
        /// 지정한 Watchdog에 대해 시간 측정 중지
        /// </summary>
        /// <param name="eWatchdogList"></param>
        private void Watchdog_Stop(WatchdogList eWatchdogList, bool bReset)
        {
            int watdogIndex = (int)eWatchdogList;

            if (watdogIndex < m_Watchdog.Length)
                m_Watchdog[watdogIndex].StopWatchdog(bReset);
        }

        /// <summary>
        /// Watchdog 현재 시간 리셋, 중지
        /// </summary>
        /// <param name="eWatchdogList"></param>
        private void Watchdog_ResetAndStop(WatchdogList eWatchdogList)
        {
            int watdogIndex = (int)eWatchdogList;

            if (watdogIndex < m_Watchdog.Length)
                m_Watchdog[watdogIndex].ResetWatchdog();
        }

        /// <summary>
        /// 지정한 Watchdog에 대해 시간을 초기화 하고 재 시작
        /// </summary>
        /// <param name="eWatchdogList"></param>
        private void Watchdog_Restart(WatchdogList eWatchdogList)
        {
            int watdogIndex = (int)eWatchdogList;

            if (watdogIndex < m_Watchdog.Length)
                m_Watchdog[watdogIndex].ReStartWatchdog();
        }


        /// <summary>
        /// 지정한 Watchdog이 Detect 시간을 초과하는 경우 true
        /// </summary>
        /// <param name="eWatchdogList"></param>
        /// <returns></returns>
        private bool Watchdog_IsDetect(WatchdogList eWatchdogList)
        {
            int watdogIndex = (int)eWatchdogList;

            if (watdogIndex < m_Watchdog.Length)
                return m_Watchdog[watdogIndex].IsDetecting();
            else
                return false;
        }


        /// <summary>
        /// Watchdog의 경과 시간을 가져옴
        /// </summary>
        /// <param name="eWatchdogList"></param>
        /// <returns></returns>
        public string Watchdog_GetProgressTime(WatchdogList eWatchdogList)
        {
            int watdogIndex = (int)eWatchdogList;

            return m_Watchdog[watdogIndex]?.GetProgressTime() ?? string.Empty;
        }

        /// <summary>
        /// Watchdog의 설정 시간을 가져옴
        /// </summary>
        /// <param name="eWatchdogList"></param>
        /// <returns></returns>
        public int Watchdog_Get_DetectTime(WatchdogList eWatchdogList)
        {
            int watdogIndex = (int)eWatchdogList;

            return m_Watchdog[watdogIndex]?.GetDetectTime() ?? 0;
        }

        /// <summary>
        /// Watchdog의 경과 시간에 대한 표시 색상을 가져옴
        /// </summary>
        /// <param name="eWatchdogList"></param>
        /// <returns></returns>
        private Color Watchdog_GetColor(WatchdogList eWatchdogList, bool bPositiveColor)
        {
            int watdogIndex = (int)eWatchdogList;

            if(bPositiveColor)
                return m_Watchdog[watdogIndex]?.GetPositiveColor() ?? Color.Black;
            else
                return m_Watchdog[watdogIndex]?.GetNegativeColor() ?? Color.Black;
        }
    }
}
