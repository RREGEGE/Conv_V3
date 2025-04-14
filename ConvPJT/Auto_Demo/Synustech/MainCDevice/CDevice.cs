using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synustech
{
    abstract class CDevice
    {
        protected bool bIsTrReq;
        protected bool bIsBusy;
        protected bool bIsCompt;
        protected bool bIsUReq;
        protected bool bIsLReq;
        protected bool bIsReady;

        bool TrReq
        {
            get { return bIsTrReq; }
            set { bIsTrReq = value; }
        }

        bool Busy
        {
            get { return bIsBusy; }
            set { bIsBusy = value; }
        }

        bool Compt
        {
            get { return bIsCompt; }
            set { bIsCompt = value; }
        }

        bool UReq
        {
            get { return bIsUReq; }
        }

        bool LReq
        {
            get { return bIsLReq; }
        }

        bool Ready
        {
            get { return bIsReady; }
        }


        public abstract void Process();

        public void Cnv_PIO_Ini()
        {
            bIsLReq = false;   // 로딩 요청 초기화
            bIsTrReq = false;  // 자재 이송 요청 초기화
            bIsReady = false;  // 준비 상태 초기화
            bIsBusy = false;   // 작업 중 상태 초기화
            bIsCompt = false;  // 완료 상태 초기화

        }

    }
}
