using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.Safty
{
    
    public enum EStopState
    {
        Idle,
        EStop
    }

    /// <summary>
    /// SW, HW EStop 관련 조작위한 클래스
    /// </summary>
    class EStop
    {
        private EStopState eEStopState = EStopState.Idle;

        /// <summary>
        /// 현재 EStop 상태
        /// </summary>
        /// <returns></returns>
        public EStopState GetEStopState()
        {
            return eEStopState;
        }

        /// <summary>
        /// Estop 상태에 따른 String
        /// </summary>
        /// <returns></returns>
        public string GetEStopStateToStr()
        {
            return eEStopState == EStopState.EStop ? "On" : "Off";
        }

        /// <summary>
        /// 현재 EStop 상태
        /// </summary>
        /// <returns></returns>
        public bool IsEStop()
        {
            return eEStopState == EStopState.EStop;
        }

        /// <summary>
        /// EStop 누름
        /// </summary>
        public void PushEStop()
        {
            eEStopState = EStopState.EStop;
        }

        /// <summary>
        /// Estop 해제
        /// </summary>
        public void ReleaseEStop()
        {
            eEStopState = EStopState.Idle;
        }
    }
}
