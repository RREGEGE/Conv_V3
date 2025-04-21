using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Watchdog_Test
{
    partial class Port
    {
        /// <summary>
        /// 사용하고자 하는 Watchdog List
        /// 추가하고자 하는 경우 해당 Enum에 항목 추가
        /// </summary>
        public enum WatchdogList
        {
            T_Axis_HomingTimer,
            T_Axis_Move_To_LoadTimer,
            T_Axis_Move_To_UnLoadTimer,
            OHT_HoistDetectTimer,
            AGVorOHT_PIO_Timer,
            Init_Step_Timer,
            IN_Step_Timer,
            OUT_Step_Timer,
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

                //m_Watchdog[watdogIndex].SetDetectTime(Watchdog_GetParam_DetectTime(watchdog));
            }
        }


        /// <summary>
        /// Watchdog Parameter에 Detect Time을 적용
        /// </summary>
        /// <param name="eWatchdogList"></param>
        /// <param name="nWatchdogTime"></param>
        //public void Watchdog_SetParam_DetectTime(WatchdogList eWatchdogList, int nWatchdogTime)
        //{
        //    switch (eWatchdogList)
        //    {
        //        case WatchdogList.T_Axis_HomingTimer:
        //            GetMotionParam().WatchdogDetectParam.T_Axis_HomingTimer = nWatchdogTime;
        //            break;
        //        case WatchdogList.T_Axis_Move_To_LoadTimer:
        //            GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_0DegTimer = nWatchdogTime;
        //            break;
        //        case WatchdogList.T_Axis_Move_To_UnLoadTimer:
        //            GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_180DegTimer = nWatchdogTime;
        //            break;
        //        case WatchdogList.OHT_HoistDetectTimer:
        //            GetMotionParam().WatchdogDetectParam.OHT_HoistDetectTimer = nWatchdogTime;
        //            break;
        //        case WatchdogList.AGVorOHT_PIO_Timer:
        //            GetMotionParam().WatchdogDetectParam.LP_PIO_Timer = nWatchdogTime;
        //            break;
        //        case WatchdogList.Init_Step_Timer:
        //            GetMotionParam().WatchdogDetectParam.LP_Step_Timer = nWatchdogTime;
        //            break;
        //        case WatchdogList.OUT_Step_Timer:
        //            GetMotionParam().WatchdogDetectParam.BP_Step_Timer = nWatchdogTime;
        //            break;
        //        case WatchdogList.IN_Step_Timer:
        //            GetMotionParam().WatchdogDetectParam.OP_Step_Timer = nWatchdogTime;
        //            break;
        //    }
        //}


        /// <summary>
        /// Watchdog Parameter에 있는 값을 얻어옴
        /// </summary>
        /// <param name="eWatchdogList"></param>
        /// <returns></returns>
        //public int Watchdog_GetParam_DetectTime(WatchdogList eWatchdogList)
        //{
        //    switch (eWatchdogList)
        //    {
        //        case WatchdogList.T_Axis_HomingTimer:
        //            return GetMotionParam().WatchdogDetectParam.T_Axis_HomingTimer;
        //        case WatchdogList.T_Axis_Move_To_LoadTimer:
        //            return GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_0DegTimer;
        //        case WatchdogList.T_Axis_Move_To_UnLoadTimer:
        //            return GetMotionParam().WatchdogDetectParam.T_Axis_Move_To_180DegTimer;
        //        case WatchdogList.OHT_HoistDetectTimer:
        //            return GetMotionParam().WatchdogDetectParam.OHT_HoistDetectTimer;
        //        case WatchdogList.AGVorOHT_PIO_Timer:
        //            return GetMotionParam().WatchdogDetectParam.LP_PIO_Timer;
        //        case WatchdogList.Init_Step_Timer:
        //            return GetMotionParam().WatchdogDetectParam.LP_Step_Timer;
        //        case WatchdogList.OUT_Step_Timer:
        //            return GetMotionParam().WatchdogDetectParam.BP_Step_Timer;
        //        case WatchdogList.IN_Step_Timer:
        //            return GetMotionParam().WatchdogDetectParam.OP_Step_Timer;
        //    }
        //    return 0;
        //}


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

            if (bPositiveColor)
                return m_Watchdog[watdogIndex]?.GetPositiveColor() ?? Color.Black;
            else
                return m_Watchdog[watdogIndex]?.GetNegativeColor() ?? Color.Black;
        }
    }
}
