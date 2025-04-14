using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Master.Equipment.RackMaster
{
    public public partial class RackMaster
    {
        public enum CycleControlStep
        {
            Idle,
            Initialize,
            InitializeAckWait,
            FromMotionStartRequest,
            FromMotionStartRequestAckWait,
            FromMotionCompleteWait,
            FromMotionCompleteEnd,
            ToMotionStartRequest,
            ToMotionStartRequestAckWait,
            ToMotionCompleteWait,
            ToMotionCompleteEnd,
            FromToIDSwapInitialize,
            FromToIDSwapInitializeWait,
            Error
        }
        public enum CycleControlErrorCode
        {
            None,
            TCPIP_Disconnection,
            Status_Error,
            PIO_Error,
            Flag_Error,
            Ack_TimeOut
        }

        public CycleControlStep m_eCycleControlStep = CycleControlStep.Idle;
        public CycleControlErrorCode m_eCycleControlErrorCode = CycleControlErrorCode.None;
        public Stopwatch m_CycleRunningTime = new Stopwatch();
        public bool m_bAutoCycleRunning = false;
        public bool m_bCycleStop = false;
        public int m_CycleCount = 0;
        public int m_CycleProgress = 0;
        public long m_CycleModeAckStart = 0;
        public int m_CycleModeAckDelay = 10000; //임시

        public void AutoCycleRun(int _CycleCount, int _FromID, int _ToID)
        {
            m_CycleCount = _CycleCount;
            m_CycleProgress = 0;

            int FromID = _FromID;
            int ToID = _ToID;

            m_eCycleControlStep = CycleControlStep.Initialize;
            m_eCycleControlErrorCode = CycleControlErrorCode.None;
            m_CycleRunningTime.Reset();
            m_bAutoCycleRunning = true;
            m_bCycleStop = false;

            Thread LocalThread = new Thread(delegate ()
            {
                m_CycleRunningTime.Start();

                while (m_bAutoCycleRunning)
                {
                    if (!IsCycleValidStatus())
                        m_eCycleControlStep = CycleControlStep.Error;

                    switch (m_eCycleControlStep)
                    {
                        case CycleControlStep.Idle:
                            break;
                        case CycleControlStep.Initialize:
                            {
                                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.From_Shelf_ID_0, FromID);
                                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.From_Shelf_ID_0, 2);
                                Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.To_Shelf_ID_0, ToID);
                                Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.To_Shelf_ID_0, 2);

                                m_eCycleControlStep = CycleControlStep.InitializeAckWait;
                                m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds;
                            }
                            break;
                        case CycleControlStep.InitializeAckWait:
                            {
                                int GetFromID = (int)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.From_Shelf_ID_0);
                                int GetToID = (int)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.To_Shelf_ID_0);

                                if (GetFromID == FromID && GetToID == ToID)
                                {
                                    if (Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_FromMove_Wait) &&
                                    Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Cassette_Not_Exist))
                                        m_eCycleControlStep = CycleControlStep.FromMotionStartRequest;
                                    else if (Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ToMove_Wait) &&
                                    !Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Cassette_Not_Exist))
                                        m_eCycleControlStep = CycleControlStep.ToMotionStartRequest;
                                    else
                                    {
                                        m_eCycleControlErrorCode = CycleControlErrorCode.Flag_Error;
                                        m_eCycleControlStep = CycleControlStep.Error;
                                    }
                                }
                                else
                                {
                                    if (m_CycleRunningTime.ElapsedMilliseconds - m_CycleModeAckStart > m_CycleModeAckDelay)
                                    {
                                        m_eCycleControlErrorCode = CycleControlErrorCode.Ack_TimeOut;
                                        m_eCycleControlStep = CycleControlStep.Error;
                                    }
                                }
                            }
                            break;
                        case CycleControlStep.FromMotionStartRequest:
                            {
                                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Run, true);
                                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Run);

                                m_eCycleControlStep = CycleControlStep.FromMotionStartRequestAckWait;
                                m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds;
                            }
                            break;
                        case CycleControlStep.FromMotionStartRequestAckWait:
                            {
                                bool bFromMotionRunAck = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_From_Run_ACK);

                                if (bFromMotionRunAck)
                                {
                                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Run, false);
                                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Run);
                                    m_eCycleControlStep = CycleControlStep.FromMotionCompleteWait;
                                }
                                else
                                {
                                    if (m_CycleRunningTime.ElapsedMilliseconds - m_CycleModeAckStart > m_CycleModeAckDelay)
                                    {
                                        m_eCycleControlErrorCode = CycleControlErrorCode.Ack_TimeOut;
                                        m_eCycleControlStep = CycleControlStep.Error;
                                    }
                                }
                            }
                            break;
                        case CycleControlStep.FromMotionCompleteWait:
                            {
                                bool bFromMotionComplete = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_From_Complete);

                                if (bFromMotionComplete)
                                {
                                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Complete_ACK, true);
                                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Complete_ACK);
                                    m_eCycleControlStep = CycleControlStep.FromMotionCompleteEnd;
                                }
                                else
                                {
                                    //Wait ??
                                }
                            }
                            break;
                        case CycleControlStep.FromMotionCompleteEnd:
                            {
                                bool bFromMotionComplete = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_From_Complete);

                                if (!bFromMotionComplete)
                                {
                                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Complete_ACK, false);
                                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_From_Complete_ACK);
                                    m_eCycleControlStep = CycleControlStep.ToMotionStartRequest;
                                }
                                else
                                {
                                    if (m_bCycleStop)
                                    {
                                        m_bAutoCycleRunning = false;
                                        m_eCycleControlStep = CycleControlStep.Idle;
                                    }
                                }
                            }
                            break;
                        case CycleControlStep.ToMotionStartRequest:
                            {
                                Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Run, true);
                                Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Run);

                                m_eCycleControlStep = CycleControlStep.ToMotionStartRequestAckWait;
                                m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds;
                            }
                            break;
                        case CycleControlStep.ToMotionStartRequestAckWait:
                            {
                                bool bToMotionRunAck = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_To_Run_ACK);

                                if (bToMotionRunAck)
                                {
                                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Run, false);
                                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Run);
                                    m_eCycleControlStep = CycleControlStep.ToMotionCompleteWait;
                                }
                                else
                                {
                                    if (m_CycleRunningTime.ElapsedMilliseconds - m_CycleModeAckStart > m_CycleModeAckDelay)
                                    {
                                        m_eCycleControlErrorCode = CycleControlErrorCode.Ack_TimeOut;
                                        m_eCycleControlStep = CycleControlStep.Error;
                                    }
                                }
                            }
                            break;
                        case CycleControlStep.ToMotionCompleteWait:
                            {
                                bool bToMotionComplete = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_To_Run_Complete);

                                if (bToMotionComplete)
                                {
                                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Complete_ACK, true);
                                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Complete_ACK);
                                    m_eCycleControlStep = CycleControlStep.ToMotionCompleteEnd;
                                }
                                else
                                {
                                    //Wait ??
                                }
                            }
                            break;
                        case CycleControlStep.ToMotionCompleteEnd:
                            {
                                bool bToMotionComplete = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_To_Run_Complete);
                                if (!bToMotionComplete)
                                {
                                    Set_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Complete_ACK, false);
                                    Send_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_RackMaster_To_Complete_ACK);

                                    m_CycleProgress++;

                                    if (m_CycleProgress >= m_CycleCount)
                                    {
                                        m_eCycleControlStep = CycleControlStep.Idle;
                                        m_bAutoCycleRunning = false;
                                    }
                                    else
                                    {
                                        if (!m_bCycleStop)
                                        {
                                            m_eCycleControlStep = CycleControlStep.FromToIDSwapInitialize;
                                        }
                                        else
                                        {
                                            m_eCycleControlStep = CycleControlStep.Idle;
                                            m_bAutoCycleRunning = false;
                                        }
                                    }
                                }
                            }
                            break;
                        case CycleControlStep.FromToIDSwapInitialize:
                            {
                                if (!m_bCycleStop)
                                {
                                    int GetToID = ToID;
                                    int GetFromID = FromID;
                                    ToID = GetFromID;
                                    FromID = GetToID;
                                    //ToID = (int)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.From_Shelf_ID_0);
                                    //FromID = (int)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.To_Shelf_ID_0);

                                    Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.From_Shelf_ID_0, FromID);
                                    Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.From_Shelf_ID_0, 2);
                                    Set_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.To_Shelf_ID_0, ToID);
                                    Send_CIM_2_RackMaster_Word_Data(ReceiveWordMapIndex.To_Shelf_ID_0, 2);

                                    m_eCycleControlStep = CycleControlStep.FromToIDSwapInitializeWait;
                                    m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds;
                                }
                                else
                                {
                                    m_eCycleControlStep = CycleControlStep.Idle;
                                    m_bAutoCycleRunning = false;
                                }
                            }
                            break;
                        case CycleControlStep.FromToIDSwapInitializeWait:
                            {
                                if (!m_bCycleStop)
                                {
                                    int GetFromID = (int)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.From_Shelf_ID_0);
                                    int GetToID = (int)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.To_Shelf_ID_0);

                                    if (GetFromID == FromID && GetToID == ToID)
                                    {
                                        if (Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_FromMove_Wait) &&
                                        Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Cassette_Not_Exist))
                                            m_eCycleControlStep = CycleControlStep.FromMotionStartRequest;
                                        else if (Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_ToMove_Wait) &&
                                        !Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Cassette_Not_Exist))
                                            m_eCycleControlStep = CycleControlStep.ToMotionStartRequest;
                                        else
                                        {
                                            m_eCycleControlErrorCode = CycleControlErrorCode.Flag_Error;
                                            m_eCycleControlStep = CycleControlStep.Error;
                                        }
                                    }
                                    else
                                    {
                                        if (m_CycleRunningTime.ElapsedMilliseconds - m_CycleModeAckStart > m_CycleModeAckDelay)
                                        {
                                            m_eCycleControlErrorCode = CycleControlErrorCode.Ack_TimeOut;
                                            m_eCycleControlStep = CycleControlStep.Error;
                                        }
                                    }
                                }
                                else
                                {
                                    m_eCycleControlStep = CycleControlStep.Idle;
                                    m_bAutoCycleRunning = false;
                                }
                            }
                            break;
                        case CycleControlStep.Error:
                            m_bAutoCycleRunning = false;
                            break;
                    }

                    Thread.Sleep(1);
                }

                m_CycleRunningTime.Stop();
            });
            LocalThread.Start();
        }
        public void AutoCycleStop()
        {
            m_bCycleStop = true;
        }
        public bool IsAutoCycleRun()
        {
            return m_bAutoCycleRunning;
        }
        public bool IsCycleValidStatus()
        {
            if (!IsConnected())
            {
                m_eCycleControlErrorCode = CycleControlErrorCode.TCPIP_Disconnection;
                return false;
            }

            if (Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Error))
            {
                m_eCycleControlErrorCode = CycleControlErrorCode.Status_Error;
                return false;
            }

            if (Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_PIO_STK_Error))
            {
                m_eCycleControlErrorCode = CycleControlErrorCode.PIO_Error;
                return false;
            }

            return true;
        }
    }
}
