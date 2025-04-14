using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ.CLS
{
    public class FSM
    {
        public const int C_IDLE = 0;
        public const int C_ERROR = (-1);
        public const int DEFAULT_MSG = (-1);
        //-----------------------------------------------------------
        // 변수 선언..
        private Timer m_tmDelay = new Timer();
        private Timer m_tmOver = new Timer();

        private bool m_bRstDelay = true;
        private bool m_bRstTmLimit = true;
        private bool m_bOnce = true;

        public int m_nErr = 0;
        public int m_nCmd = C_IDLE;
        public int m_nMsg = DEFAULT_MSG;
        public int m_nStep = 0;

        public bool m_bStop = false;
        //-----------------------------------------------------------
        public void Set(int cmd, int msg = DEFAULT_MSG)
        {
            if (DEFAULT_MSG != msg)
            {
                m_nMsg = msg;
            }

            if (C_ERROR == cmd)
            {
                if (DEFAULT_MSG != msg)
                    m_nErr = msg;
                else
                    m_nErr = 1;
            }
            else if (C_IDLE < cmd)
            {
                m_nErr = 0;
            }

            if (C_IDLE == cmd)
            {
                m_bStop = false;
                m_nMsg = DEFAULT_MSG;
            }

            if (m_nCmd != cmd)
            {
                m_bRstDelay = true;
                m_bRstTmLimit = true;
                m_bOnce = true;
                m_nStep = 0;
            }
            m_nCmd = cmd;
        }

        //-----------------------------------------------------------
        public int Get()
        {
            return (m_nCmd);
        }

        //-----------------------------------------------------------
        public bool IsRun()
        {
            if (C_IDLE == m_nCmd)
                return (false);
            return (true);
        }

        //-----------------------------------------------------------
        public bool Between(int min, int max)
        {
            if (min > Get())
                return (false);
            if (max < Get())
                return (false);

            return (true);
        }

        //-----------------------------------------------------------
        public int GetErr()
        {
            return (m_nErr);
        }

        //-----------------------------------------------------------
        public bool Once()
        {
            bool bRet = m_bOnce;

            m_bOnce = false;
            return (bRet);
        }

        //-----------------------------------------------------------
        public void RstErr()
        {
            m_nErr = 0;
        }

        //-----------------------------------------------------------
        public void RstDelay()
        {
            m_bRstDelay = true;
            m_tmDelay.Restart();
        }

        //-----------------------------------------------------------
        public bool Delay(long timeMs)
        {
            if (m_bRstDelay)
            {
                m_bRstDelay = false;
                m_tmDelay.Restart();
            }

            return (m_tmDelay.Delay(timeMs));
        }

        //-----------------------------------------------------------
        public long Elapsed()
        {
            if (m_bRstDelay)
            {
                m_bRstDelay = false;
                m_tmDelay.Restart();
            }

            return (m_tmDelay.ElapsedMilliseconds());
        }

        //-----------------------------------------------------------
        public int GetMsg()
        {
            return (m_nMsg);
        }
    }
}
