using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Master.Interface.Alarm;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// RackMasterAutoCycleMotion.cs 파일은 STK Auto Cycle 공정과 관련 내용 작성
    /// </summary>
    public partial class RackMaster
    {
        /// <summary>
        /// Auto Cycle 공정 Step 정의
        /// </summary>
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
            User_Stop,
            Cycle_End,
            Error
        }

        /// <summary>
        /// Auto Teaching 공정 중 에러 코드 정의
        /// </summary>
        public enum CycleControlErrorCode
        {
            None,
            TCPIP_Disconnection,
            Status_Error,
            Emergency_Stop,
            PIO_Error,
            Flag_Error,
            Port_Error,
            Port_RestartError,
            PortStatus_Error,
            Port_Step_TimeOut,
            Ack_TimeOut
        }

        public CycleControlStep m_eCycleControlStep             = CycleControlStep.Idle;
        public CycleControlStep m_PreCycleControlStep           = CycleControlStep.Idle;
        public CycleControlErrorCode m_eCycleControlErrorCode   = CycleControlErrorCode.None;
        public Stopwatch m_CycleRunningTime                     = new Stopwatch();
        private bool m_bAutoCycleRunning                        = false;
        public bool m_bCycleStop                                = false;
        public int m_CycleCount                                 = 0;
        public int m_CycleProgress                              = 0;
        public long m_CycleModeAckStart                         = 0;
        public int m_CycleModeAckDelay                          = 10000; //임시
        private bool bFromSwapEnd                               = false;
        private bool bToSwapEnd                                 = false;

        private bool bFromMotionEnd                             = false;
        private bool bToMotionEnd                               = false;

        public int m_nFromID                                    = 0;
        public int m_nToID                                      = 0;


        /// <summary>
        /// STK Auto Cycle 동작 중 여부 리턴
        /// </summary>
        /// <returns></returns>
        public bool IsAutoCycleRun()
        {
            return m_bAutoCycleRunning;
        }

        /// <summary>
        /// STK Auto Cycle 동작 정지 명령 
        /// </summary>
        public void AutoCycleStop()
        {
            m_bCycleStop = true;
        }

        /// <summary>
        /// STK Auto Cycle 동작 
        /// </summary>
        /// <param name="_CycleCount"></param>
        /// <param name="_FromID"></param>
        /// <param name="_ToID"></param>
        public void CMD_AutoCycleRun(int _CycleCount, int _FromID, int _ToID)
        {
            //1. 스레드 진입 전 초기화, 이니셜 과정
            m_CycleCount            = _CycleCount;  //진행 할 Cycle Count 이니셜
            m_CycleProgress         = 0;            //진행률 초기화

            m_nFromID               = _FromID;      //From Shelf ID 이니셜
            m_nToID                 = _ToID;        //To Shelf ID 이니셜

            m_eCycleControlStep         = CycleControlStep.Initialize;  //스텝 초기화
            m_PreCycleControlStep       = CycleControlStep.Initialize;  //스텝 초기화
            m_eCycleControlErrorCode    = CycleControlErrorCode.None;   //에러 코드 초기화
            m_CycleRunningTime.Reset();                                 //진행 시간 초기화
            m_bAutoCycleRunning         = true;                         //스레드 동작 플래그 초기화
            m_bCycleStop                = false;                        //스레드 정지 플래그 초기화

            CMD_FromRun_REQ             = false;    //From 동작 요청 비트 초기화
            CMD_FromCompleteACK_REQ     = false;    //From 완료 아크 비트 초기화
            CMD_ToRun_REQ               = false;    //To 동작 요청 비트 초기화
            CMD_ToCompleteACK_REQ       = false;    //To 완료 아크 비트 초기화

            Thread LocalThread = new Thread(delegate ()
            {
                m_CycleRunningTime.Start(); //진행 시간 측정 시작

                while (m_bAutoCycleRunning)
                {
                    try
                    {
                        //1. 사이클 공정 유효 상태 체크(통신, 에러 등 체크)
                        if (!IsCycleValidStatus())
                            m_eCycleControlStep = CycleControlStep.Error;

                        //2. 스텝 변한 경우 로그 작성
                        if (m_PreCycleControlStep != m_eCycleControlStep)
                        {
                            LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterCycleStepInfo, $"RM Cycle Step : {m_PreCycleControlStep} => {m_eCycleControlStep}");
                            m_PreCycleControlStep = m_eCycleControlStep;
                        }

                        //3. 스텝 수행 진행
                        switch (m_eCycleControlStep)
                        {
                            case CycleControlStep.Idle:
                                break;
                            case CycleControlStep.Initialize:
                                {
                                    //Initialize Step은 동작 전 준비 과정 
                                    CMD_CIM_To_STK_From_Shelf_ID = m_nFromID;    //STK에 From ID 전송
                                    CMD_CIM_To_STK_To_Shelf_ID = m_nToID;      //STK에 To ID 전송
                                    CMD_FromRun_REQ = false;        //From 동작 요청 비트 초기화
                                    CMD_FromCompleteACK_REQ = false;        //From 완료 아크 비트 초기화
                                    CMD_ToRun_REQ = false;        //To 동작 요청 비트 초기화
                                    CMD_ToCompleteACK_REQ = false;        //To 완료 아크 비트 초기화

                                    m_eCycleControlStep = CycleControlStep.InitializeAckWait;       //다음 수행할 스텝 정보로 변경
                                    m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds;   //스텝 변경 전 시간 측정
                                }
                                break;
                            case CycleControlStep.InitializeAckWait:
                            case CycleControlStep.FromToIDSwapInitializeWait:
                                {
                                    //InitializeAckWait, FromToIDSwapInitializeWait Step은 동작 전 준비 과정에서 보낸 정보를 STK에서 잘 받았는 지 확인하는 Step
                                    int GetFromID = Status_STK_To_CIM_FromID; //STK에서 전송한 From ID 확인
                                    int GetToID = Status_STK_To_CIM_ToID;   //STK에서 전송한 To ID 확인

                                    if (GetFromID == m_nFromID && GetToID == m_nToID)
                                    {
                                        //모두 이상 없는 경우 상태에 따라 스텝 진행
                                        if (Status_FromMoveWait && !Status_CassetteOn) //From 동작 대기 중이면서 카세트가 없는 경우
                                            m_eCycleControlStep = CycleControlStep.FromMotionStartRequest; //From 동작 진행
                                        else if (Status_ToMoveWait && Status_CassetteOn) //To 동작 대기 중이면서 카세트가 있는 경우
                                            m_eCycleControlStep = CycleControlStep.ToMotionStartRequest; //To 동작 진행
                                        else
                                        {
                                            //둘다 해당되지 않는 경우 플래그 에러(비정상)
                                            m_eCycleControlErrorCode = CycleControlErrorCode.Flag_Error;
                                            m_eCycleControlStep = CycleControlStep.Error;
                                        }
                                    }
                                    else
                                    {
                                        //2개중 하나라도 일치하지 않는 다면 Timeout에 의한 에러 발생
                                        //현재 시간 - 이전 스텝 마지막 측정 시간 > 10초 넘어가는 경우 발생
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
                                    //FromMotionStartRequest Step은 From 동작 명령을 위한 Step
                                    CMD_FromRun_REQ = true; //From 동작 개시 요청

                                    m_eCycleControlStep = CycleControlStep.FromMotionStartRequestAckWait; //다음 수행할 스텝 정보로 변경
                                    m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds; //스텝 변경 전 시간 측정
                                }
                                break;
                            case CycleControlStep.FromMotionStartRequestAckWait:
                                {
                                    //FromMotionStartRequestAckWait Step은 From 동작 ACK 확인을 위한 Step

                                    if (Status_FromRunACK)
                                    {
                                        //동작 명령을 잘 수신한 경우 동작 명령 비트를 초기화 시키고 완료 확인 스텝으로 변경
                                        CMD_FromRun_REQ = false;
                                        m_eCycleControlStep = CycleControlStep.FromMotionCompleteWait;
                                    }
                                    else
                                    {
                                        //From Run ACK의 반응이 없는 경우 Timeout에 의한 에러 발생
                                        //현재 시간 - 이전 스텝 마지막 측정 시간 > 10초 넘어가는 경우 발생
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
                                    //FromMotionCompleteWait Step은 From 동작 완료 확인을 위한 Step

                                    if (Status_FromRunComplete)
                                    {
                                        //동작 완료 수신한 경우 ACK 전송
                                        CMD_FromCompleteACK_REQ = true;
                                        m_eCycleControlStep = CycleControlStep.FromMotionCompleteEnd;
                                    }
                                    else
                                    {
                                        //완료 비트가 안오는 경우 무한 대기
                                        //통신이 끊어지던 특정 상황에 의해 종료 되도록
                                    }
                                }
                                break;
                            case CycleControlStep.FromMotionCompleteEnd:
                                {
                                    //FromMotionCompleteEnd Step은 STK에서 FromRunComplete Bit 초기화 하는 것을 대기, 확인하는 Step

                                    //User가 Stop 버튼을 누른 경우
                                    if (m_bCycleStop)
                                    {
                                        m_eCycleControlStep = CycleControlStep.User_Stop;
                                        break;
                                    }

                                    if (!Status_FromRunComplete)
                                    {
                                        //STK에서 From Run Complete Bit를 초기화 완료한 경우 ACK Bit 초기화 후 To Motion 진행
                                        CMD_FromCompleteACK_REQ = false;
                                        m_eCycleControlStep = CycleControlStep.ToMotionStartRequest;
                                    }
                                }
                                break;
                            case CycleControlStep.ToMotionStartRequest:
                                {
                                    //ToMotionStartRequest Step은 To 동작 명령을 위한 Step
                                    CMD_ToRun_REQ = true;
                                    m_eCycleControlStep = CycleControlStep.ToMotionStartRequestAckWait;
                                    m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds;
                                }
                                break;
                            case CycleControlStep.ToMotionStartRequestAckWait:
                                {
                                    //ToMotionStartRequestAckWait Step은 From 동작 ACK 확인을 위한 Step

                                    if (Status_ToRunACK)
                                    {
                                        //동작 명령을 잘 수신한 경우 동작 명령 비트를 초기화 시키고 완료 확인 스텝으로 변경
                                        CMD_ToRun_REQ = false;
                                        m_eCycleControlStep = CycleControlStep.ToMotionCompleteWait;
                                    }
                                    else
                                    {
                                        //To Run ACK의 반응이 없는 경우 Timeout에 의한 에러 발생
                                        //현재 시간 - 이전 스텝 마지막 측정 시간 > 10초 넘어가는 경우 발생
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
                                    //ToMotionCompleteWait Step은 To 동작 완료 확인을 위한 Step

                                    if (Status_ToRunComplete)
                                    {
                                        //Port 모션 판단을 위한 변수 초기화
                                        bFromMotionEnd = false;
                                        bToMotionEnd = false;
                                        CMD_ToCompleteACK_REQ = true; //동작 완료 수신한 경우 ACK 전송
                                        m_eCycleControlStep = CycleControlStep.ToMotionCompleteEnd;
                                    }
                                    else
                                    {
                                        //완료 비트가 안오는 경우 무한 대기
                                        //통신이 끊어지던 특정 상황에 의해 종료 되도록
                                    }
                                }
                                break;
                            case CycleControlStep.ToMotionCompleteEnd:
                                {
                                    //ToMotionCompleteEnd Step은 STK에서 ToRunComplete Bit 초기화 하는 것을 대기, 확인하는 Step

                                    if (!Status_ToRunComplete)
                                    {
                                        //1. STK에서 To Run Complete Bit를 초기화 완료한 경우
                                        //2. 연속 수행 여부 판단

                                        bool bContinue = (m_CycleProgress + 1) >= m_CycleCount ? false : true;
                                        string FromPortKey = Convert.ToString(m_nFromID);
                                        string ToPortKey = Convert.ToString(m_nToID);

                                        if (!m_bCycleStop && bContinue)
                                        {
                                            //Cycle이 정지가 아니며 연속 수행해야 하는 상황인 경우
                                            bFromSwapEnd = false;
                                            bToSwapEnd = false;
                                            m_CycleProgress++;
                                            CMD_ToCompleteACK_REQ = false;
                                            m_eCycleControlStep = CycleControlStep.FromToIDSwapInitialize;
                                        }
                                        else
                                        {
                                            //From Shelf ID가 Master에 할당된 포트인 경우 공정 종료 및 종료 확인 
                                            if (Master.m_Ports.ContainsKey(FromPortKey) && !bFromMotionEnd)
                                            {
                                                var port = Master.m_Ports[FromPortKey];
                                                if (port.IsAutoControlRun())
                                                {
                                                    if (port.IsShuttleControlPort() &&
                                                        port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.Two_Buffer &&
                                                        port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req)
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                    else if (port.IsShuttleControlPort() &&
                                                        port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.One_Buffer &&
                                                        (port.Get_BP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS : (int)Port.Port.Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid) ||
                                                        port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load))
                                                    {
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                    }
                                                    else if (port.IsBufferControlPort() &&
                                                             port.GetParam().ePortType == Port.Port.PortType.Conveyor_AGV &&
                                                            (port.Get_LP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.LP_CV_AutoStep.Step200_InMode_Await_PIO_CS : (int)Port.Port.LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid) ||
                                                            port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load))
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                    else if (port.IsBufferControlPort() &&
                                                             port.GetParam().ePortType == Port.Port.PortType.Conveyor_OMRON &&
                                                        (port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step200_InMode_Await_Check_Unload_REQ ||
                                                        port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step300_InMode_Await_MGV_CST_Load))
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                }
                                                else
                                                {
                                                    bFromMotionEnd = true;
                                                }
                                            }
                                            else
                                                bFromMotionEnd = true;

                                            //To Shelf ID가 Master에 할당된 포트인 경우 공정 종료 및 종료 확인 
                                            if (Master.m_Ports.ContainsKey(ToPortKey) && !bToMotionEnd)
                                            {
                                                var port = Master.m_Ports[ToPortKey];
                                                if (port.IsAutoControlRun())
                                                {
                                                    if (port.IsShuttleControlPort() &&
                                                        port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.Two_Buffer &&
                                                        port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_2BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req)
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                    else if (port.IsShuttleControlPort() &&
                                                        port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.One_Buffer &&
                                                        (port.Get_BP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS : (int)Port.Port.Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid) ||
                                                        port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload))
                                                    {
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                    }
                                                    else if (port.IsBufferControlPort() &&
                                                    port.GetParam().ePortType == Port.Port.PortType.Conveyor_AGV &&
                                                    (port.Get_LP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS : (int)Port.Port.LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid) ||
                                                    port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload))
                                                    {
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                    }
                                                    else if (port.IsBufferControlPort() &&
                                                    port.GetParam().ePortType == Port.Port.PortType.Conveyor_OMRON &&
                                                    (port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ ||
                                                    port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_MGV_CST_Unload))
                                                    {
                                                        port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                    }
                                                }
                                                else
                                                {
                                                    bToMotionEnd = true;
                                                }
                                            }
                                            else
                                                bToMotionEnd = true;

                                            //From, To 포트가 공정 종료된 경우 
                                            if (bFromMotionEnd && bToMotionEnd)
                                            {
                                                m_CycleProgress++;

                                                if (m_bCycleStop)
                                                    m_eCycleControlStep = CycleControlStep.User_Stop;
                                                else
                                                    m_eCycleControlStep = CycleControlStep.Cycle_End;
                                            }
                                        }
                                    }
                                }
                                break;
                            case CycleControlStep.FromToIDSwapInitialize:
                                {
                                    //FromToIDSwapInitialize Step은 Cycle 연속 수행 시 From, To 위치를 변경하기 위한 Step

                                    if (!m_bCycleStop)
                                    {
                                        int GetToID = m_nToID;
                                        int GetFromID = m_nFromID;

                                        //From Shelf ID가 Master에 할당된 포트인 경우 공정 종료 -> 방향 전환 -> 공정 시작
                                        string FromPortKey = Convert.ToString(m_nFromID);
                                        if (Master.m_Ports.ContainsKey(FromPortKey) && !bFromSwapEnd)
                                        {
                                            var port = Master.m_Ports[FromPortKey];
                                            if (port.IsAutoControlRun())
                                            {
                                                if (port.IsShuttleControlPort() &&
                                                    port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.Two_Buffer &&
                                                    port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_2BP_AutoStep.Step200_InMode_Await_LP_Unload_Req)
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                else if (port.IsShuttleControlPort() &&
                                                    port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.One_Buffer &&
                                                    (port.Get_BP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.Shuttle_1BP_AutoStep.Step200_InMode_Await_PIO_CS : (int)Port.Port.Shuttle_1BP_AutoStep.Step210_InMode_Check_PIO_Valid) ||
                                                    port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_1BP_AutoStep.Step300_InMode_Await_MGV_CST_Load))
                                                {
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                }
                                                else if (port.IsBufferControlPort() &&
                                                        port.GetParam().ePortType == Port.Port.PortType.Conveyor_AGV &&
                                                        (port.Get_LP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.LP_CV_AutoStep.Step200_InMode_Await_PIO_CS : (int)Port.Port.LP_CV_AutoStep.Step210_InMode_Check_PIO_Valid) ||
                                                        port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_AutoStep.Step300_InMode_Await_MGV_CST_Load))
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                else if (port.IsBufferControlPort() &&
                                                        port.GetParam().ePortType == Port.Port.PortType.Conveyor_OMRON &&
                                                        (port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step200_InMode_Await_Check_Unload_REQ ||
                                                        port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step300_InMode_Await_MGV_CST_Load))
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                            }
                                            else
                                            {
                                                if (port.GetParam().ePortType != Port.Port.PortType.EQ && port.GetOperationDirection() == Port.Port.PortDirection.Input)
                                                    port.Interlock_AutoControlDirectionChange(Port.Port.PortDirection.Output, Port.Port.InterlockFrom.ApplicationLoop);
                                                else if (port.GetParam().ePortType == Port.Port.PortType.EQ && port.GetOperationDirection() == Port.Port.PortDirection.Output)
                                                    port.Interlock_AutoControlDirectionChange(Port.Port.PortDirection.Input, Port.Port.InterlockFrom.ApplicationLoop);
                                                else
                                                {
                                                    if (port.GetAlarmLevel() == AlarmLevel.Error)
                                                    {
                                                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CycleControlAlarmStop, $"RackMaster Cycle Control Error : Port Alarm State");
                                                        m_eCycleControlErrorCode = CycleControlErrorCode.Port_Error;
                                                        m_eCycleControlStep = CycleControlStep.Error;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        port.Interlock_StartAutoControl(Port.Port.InterlockFrom.ApplicationLoop);

                                                        if (port.IsAutoControlRun())
                                                        {
                                                            bFromSwapEnd = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            bFromSwapEnd = true;

                                        //To Shelf ID가 Master에 할당된 포트인 경우 공정 종료 -> 방향 전환 -> 공정 시작
                                        string ToPortKey = Convert.ToString(m_nToID);
                                        if (Master.m_Ports.ContainsKey(ToPortKey) && !bToSwapEnd)
                                        {
                                            var port = Master.m_Ports[ToPortKey];
                                            if (port.IsAutoControlRun())
                                            {
                                                if (port.IsShuttleControlPort() &&
                                                    port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.Two_Buffer &&
                                                    port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_2BP_AutoStep.Step700_OutMode_Await_OP_Unload_Req)
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                else if (port.IsShuttleControlPort() &&
                                                    port.GetMotionParam().eBufferType == Port.Port.ShuttleCtrlBufferType.One_Buffer &&
                                                    (port.Get_BP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.Shuttle_1BP_AutoStep.Step800_OutMode_Await_PIO_CS : (int)Port.Port.Shuttle_1BP_AutoStep.Step810_OutMode_Check_PIO_Valid) ||
                                                    port.Get_BP_AutoControlStep() == (int)Port.Port.Shuttle_1BP_AutoStep.Step900_OutMode_Await_MGV_CST_Unload))
                                                {
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                }
                                                else if (port.IsBufferControlPort() &&
                                                        port.GetParam().ePortType == Port.Port.PortType.Conveyor_AGV &&
                                                        (port.Get_LP_AutoControlStep() == (port.GetAGVPIOFlagOption() ? (int)Port.Port.LP_CV_AutoStep.Step900_OutMode_Await_PIO_CS : (int)Port.Port.LP_CV_AutoStep.Step910_OutMode_Check_PIO_Valid) ||
                                                        port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_AutoStep.Step990_OutMode_Await_MGV_CST_Unload))
                                                {
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                }
                                                else if (port.IsBufferControlPort() &&
                                                        port.GetParam().ePortType == Port.Port.PortType.Conveyor_OMRON &&
                                                        (port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step800_OutMode_Await_Check_PIO_Load_REQ ||
                                                        port.Get_LP_AutoControlStep() == (int)Port.Port.LP_CV_DIEBANK_AutoStep.Step900_OutMode_Await_MGV_CST_Unload))
                                                {
                                                    port.Interlock_StopAutoControl(Port.Port.InterlockFrom.ApplicationLoop);
                                                }
                                            }
                                            else
                                            {
                                                if (port.GetParam().ePortType != Port.Port.PortType.EQ && port.GetOperationDirection() == Port.Port.PortDirection.Output)
                                                    port.Interlock_AutoControlDirectionChange(Port.Port.PortDirection.Input, Port.Port.InterlockFrom.ApplicationLoop);
                                                else if (port.GetParam().ePortType == Port.Port.PortType.EQ && port.GetOperationDirection() == Port.Port.PortDirection.Input)
                                                    port.Interlock_AutoControlDirectionChange(Port.Port.PortDirection.Output, Port.Port.InterlockFrom.ApplicationLoop);
                                                else
                                                {
                                                    if (port.GetAlarmLevel() == AlarmLevel.Error)
                                                    {
                                                        LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CycleControlAlarmStop, $"RackMaster Cycle Control Error : Port Alarm State");
                                                        m_eCycleControlErrorCode = CycleControlErrorCode.Port_Error;
                                                        m_eCycleControlStep = CycleControlStep.Error;
                                                    }
                                                    else
                                                    {
                                                        port.Interlock_StartAutoControl(Port.Port.InterlockFrom.ApplicationLoop);

                                                        if (port.IsAutoControlRun())
                                                        {
                                                            bToSwapEnd = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                            bToSwapEnd = true;

                                        if (bFromSwapEnd && bToSwapEnd)
                                        {
                                            //Port의 방향 전환 및 공정 시작이 완료 된 경우
                                            m_nToID = GetFromID;            //기존 To 영역에 From ID 삽입
                                            m_nFromID = GetToID;              //기존 From 영역에 To ID 삽입
                                            CMD_CIM_To_STK_From_Shelf_ID = m_nFromID;  //신규 From ID 전송
                                            CMD_CIM_To_STK_To_Shelf_ID = m_nToID;    //신규 To ID 전송
                                            m_eCycleControlStep = CycleControlStep.FromToIDSwapInitializeWait; //다음 수행할 스텝 정보로 변경
                                            m_CycleModeAckStart = m_CycleRunningTime.ElapsedMilliseconds; //스텝 변경 전 시간 측정
                                        }
                                    }
                                    else
                                    {
                                        m_eCycleControlStep = CycleControlStep.User_Stop;
                                    }
                                }
                                break;
                            case CycleControlStep.Error:
                            case CycleControlStep.User_Stop:
                            case CycleControlStep.Cycle_End:
                                {
                                    //Error, User Stop, Cycle End Step은 공정이 종료되는 스텝으로 사용 비트 초기화 진행
                                    m_bAutoCycleRunning = false;        //스레드 종료 플래그
                                    CMD_FromCompleteACK_REQ = false;    //From Complete ACK Bit 초기화, 전송
                                    CMD_ToCompleteACK_REQ = false;      //To complete Bit 초기화, 전송
                                    CMD_ToRun_REQ = false;              //To Run Bit 초기화, 전송
                                    CMD_FromRun_REQ = false;            //From Run Bit 초기화, 전송
                                }
                                break;
                        }

                        Thread.Sleep(Master.StepUpdateTime);
                    }
                    catch(Exception ex)
                    {
                        //Exception 발생 시 플래그 초기화
                        m_bAutoCycleRunning = false;        //스레드 종료 플래그
                        CMD_FromCompleteACK_REQ = false;    //From Complete ACK Bit 초기화, 전송
                        CMD_ToCompleteACK_REQ = false;      //To complete Bit 초기화, 전송
                        CMD_ToRun_REQ = false;              //To Run Bit 초기화, 전송
                        CMD_FromRun_REQ = false;            //From Run Bit 초기화, 전송
                        LogMsg.AddExceptionLog(ex, $"RackMaster[{GetParam().ID}] Auto Cycle Control Seq Exception");
                    }
                }

                m_CycleRunningTime.Stop();
            });
            LocalThread.Start();
        }


        /// <summary>
        /// Auto Cycle 동작 유효 상태 점검
        /// </summary>
        /// <returns></returns>
        public bool IsCycleValidStatus()
        {
            if (!IsConnected())
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CycleControlAlarmStop, $"RackMaster Cycle Control Error : TCP/IP Disconnect");
                m_eCycleControlErrorCode = CycleControlErrorCode.TCPIP_Disconnection;
                return false;
            }

            if (Status_Error)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CycleControlAlarmStop, $"RackMaster Cycle Control Error : RackMaster Error Status");
                m_eCycleControlErrorCode = CycleControlErrorCode.Status_Error;
                return false;
            }

            if (PIOStatus_Port_Error)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CycleControlAlarmStop, $"RackMaster Cycle Control Error : Port Error Status");
                m_eCycleControlErrorCode = CycleControlErrorCode.PortStatus_Error;
                return false;
            }

            if (CMD_EmergencyStop_REQ)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CycleControlAlarmStop, $"RackMaster Cycle Control Error : RackMaster EMO Status");
                m_eCycleControlErrorCode = CycleControlErrorCode.Emergency_Stop;
                return false;
            }

            if (PIOStatus_STK_Error)
            {
                LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.CycleControlAlarmStop, $"RackMaster Cycle Control Error : RackMaster PIO Error Flag");
                m_eCycleControlErrorCode = CycleControlErrorCode.PIO_Error;
                return false;
            }

            if (m_eCycleControlErrorCode != CycleControlErrorCode.None)
            {
                return false;
            }

            return true;
        }
    }
}
