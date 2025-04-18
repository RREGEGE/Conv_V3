using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace Synustech.Interface.Watchdog
{
    class Watchdog
    {
        Stopwatch st = new Stopwatch();
        private int m_DetectTime = 0;
        public Watchdog()
        {

        }

        /// <summary>
        /// WatchDog을 통해 검출 할 시간을 지정합니다. 단위 msec
        /// </summary>
        /// <param name="DetectTime"></param>
        public Watchdog(int DetectTime)
        {
            m_DetectTime = DetectTime;
        }

        /// <summary>
        /// Watchdog의 동작 상태를 판단합니다.
        /// </summary>
        public void StartWatchdog()
        {
            if (!st.IsRunning)
            {
                st.Reset();
                st.Start();
            }
        }

        /// <summary>
        /// Watchdog 측정을 재시작 합니다.
        /// </summary>
        public void ReStartWatchdog()
        {
            st.Restart();
        }

        /// <summary>
        /// Watchdog 측정을 중지합니다.
        /// Reset 하는 경우 측정 시간도 초기화 진행.
        /// </summary>
        /// <param name="bReset"></param>
        public void StopWatchdog(bool bReset)
        {
            if (st.IsRunning)
            {
                st.Stop();

                if (bReset)
                    st.Reset();
            }
        }

        /// <summary>
        /// Watchdog 측정 초기화, 중지
        /// </summary>
        public void ResetWatchdog()
        {
            st.Reset();
        }

        /// <summary>
        /// Watchdog이 지정 시간 이상 경과했는지 판단합니다.
        /// </summary>
        /// <returns></returns>
        public bool IsDetecting()
        {
            if (m_DetectTime <= 0)
                return true;

            if (st.ElapsedMilliseconds > m_DetectTime)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Watchdog 경과에 따른 색상을 출력합니다.
        /// </summary>
        /// <returns></returns>
        public Color GetNegativeColor()
        {
            if (st.ElapsedMilliseconds > (double)m_DetectTime)
            {
                return Color.Red;
            }
            else if (st.ElapsedMilliseconds > (double)m_DetectTime * 0.75)
            {
                return Color.Orange;
            }
            else if (st.ElapsedMilliseconds > (double)m_DetectTime * 0.5)
            {
                return Color.DarkOrange;
            }
            else
                return Color.White;
        }

        public Color GetPositiveColor()
        {
            if (st.ElapsedMilliseconds >= (double)m_DetectTime)
            {
                return Color.Lime;
            }
            else if (st.ElapsedMilliseconds > (double)m_DetectTime * 0.75)
            {
                return Color.Orange;
            }
            else if (st.ElapsedMilliseconds > (double)m_DetectTime * 0.5)
            {
                return Color.DarkOrange;
            }
            else
                return Color.Red;
        }

        /// <summary>
        /// Watchdog의 현재 상태를 텍스트로 출력합니다.
        /// </summary>
        /// <returns></returns>
        public string GetProgressTime()
        {
            return StopWatchFunc.GetRunningTime(st);
            //st.Elapsed.ToString("hh\\:mm\\:ss\\.fff") + $" [{st.ElapsedMilliseconds} msec]";
        }

        /// <summary>
        /// Watchdog에서 검출할 시간을 지정합니다.
        /// </summary>
        /// <param name="ms"></param>
        public void SetDetectTime(int ms)
        {
            m_DetectTime = ms;
        }

        /// <summary>
        /// Watchdog에 설정된 검출 시간을 얻어옵니다.
        /// </summary>
        /// <returns></returns>
        public int GetDetectTime()
        {
            return m_DetectTime;
        }
    }
}
