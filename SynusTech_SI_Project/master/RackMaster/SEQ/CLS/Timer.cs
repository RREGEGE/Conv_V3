using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RackMaster.SEQ.CLS
{
    public class Timer
    {
        private Stopwatch m_tm = new Stopwatch();

        public Timer()
        {
            m_tm.Start();
        }
        /// <summary>
        /// 스탑워치 타이머 재시작 함수
        /// </summary>
        public void Restart()
        {
            m_tm.Restart();
        }
        /// <summary>
        /// 스탑워치 타이머 리셋 함수
        /// </summary>
        public void Reset()
        {
            m_tm.Reset();
        }
        /// <summary>
        /// 스탑워치 타이머 종료 함수
        /// </summary>
        public void Stop() {
            m_tm.Stop();
        }
        /// <summary>
        /// 스탑워치 타이머 시작 함수
        /// </summary>
        public void Start() {
            m_tm.Start();
        }
        /// <summary>
        /// 현재 스탑워치 타이머가 돌고 있는지 판단하는 함수
        /// </summary>
        /// <returns></returns>
        public bool IsTimerStarted() {
            return m_tm.IsRunning;
        }
        /// <summary>
        /// 현재까지 진행된 스탑워치 타이머의 총 Milliseconds를 반환하는 함수
        /// </summary>
        /// <returns></returns>
        public long ElapsedMilliseconds()
        {
            long elapsed = m_tm.ElapsedMilliseconds;
            return elapsed;
        }
        /// <summary>
        /// 현재까지 진행된 스탑워치 타이머의 총 Seconds를 반환하는 함수
        /// </summary>
        /// <returns></returns>
        public double ElapsedSeconds()
        {
            return m_tm.Elapsed.TotalSeconds;
        }
        /// <summary>
        /// 매개변수로 입력된 값까지의 남은 시간을 반환하는 함수
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public TimeSpan GetRemainingTimeSpan(long milliseconds) {
            TimeSpan ts = new TimeSpan(milliseconds * 10000);
            TimeSpan remainTs = ts - m_tm.Elapsed;
            return remainTs;
        }

        /// <summary>
        /// True면 Delay 끝
        /// False면 Delay 중
        /// </summary>
        /// <param name="timeMs"></param>
        /// <returns></returns>
        public bool Delay(long timeMs)
        {
            if(timeMs < m_tm.ElapsedMilliseconds)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
