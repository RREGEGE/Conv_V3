using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// RackMasterAutoTeaching.cs 파일은 STK Auto Teaching 공정과 관련 내용 작성
    /// </summary>
    public partial class RackMaster
    {
        /// <summary>
        /// Teaching 공정 진행 시 사용되는 Teaching Parameter
        /// </summary>
        public class TeachingParam
        {
            public string ShelfID;
            public short X_Pos;
            public short Z_Pos;
            public string State;
            public int nRowIndex;

            public bool IsValid()
            {
                try
                {
                    int ShelfID = Convert.ToInt32(this.ShelfID);

                    if (ShelfID < 10000 || ShelfID >= 40000)
                        return false;

                    if (State == $"{ShelfTeachingState.DupleError}")
                        return false;

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Auto Teaching 공정 Step 정의
        /// </summary>
        public enum TeachingControlStep
        {
            Idle,
            Initialize,
            InitializeAckWait,
            AutoTeachingStartRequest,
            AutoTeachingStartRequestAckWait,
            AutoTeachingRunningStateCheck,
            RackMasterHandShakeCheck,
            NextShelfCheck,
            TeachingComplete,
            Stop,
            Error
        }

        /// <summary>
        /// Auto Teaching 공정 중 에러 코드 정의
        /// </summary>
        public enum TeachingControlErrorCode
        {
            None,
            TCPIP_Disconnection,
            Status_Error,
            Emergency_Stop,
            AutoMode_Error,
            CIMMode_Error,
            Ack_TimeOut,
            RunningState_Error,
            CompleteStateDuple_Error
        }

        /// <summary>
        /// Auto Teaching 상태 정의
        /// </summary>
        public enum ShelfTeachingState
        {
            Running,
            Success,
            Fail,
            UserStop,
            ErrorStop,
            DupleError
        }

        /// <summary>
        /// Auto Teaching 모드 정의
        /// Single : 단일 쉘프 티칭
        /// Continuous : 복수 쉘프 연속 티칭
        /// </summary>
        public enum TeachingMode
        {
            Single,
            Continuous
        }

        private TeachingMode m_eTeachingMode                = TeachingMode.Single;
        public TeachingControlStep m_eAutoTeachingStep      = TeachingControlStep.Idle;
        TeachingControlStep m_ePreAutoTeachingStep          = TeachingControlStep.Idle;
        public TeachingControlErrorCode m_eAutoTeachingErrorCode    = TeachingControlErrorCode.None;
        public Stopwatch m_TeachingRunningTime              = new Stopwatch();
        public bool m_bAutoTeachingRunning                  = false;
        public bool m_bAutoTeachingStop                     = false;
        public int m_AutoTeachingCount                      = 0;
        public int m_AutoTeachingProgress                   = 0;
        public long m_AutoTeachingAckStart                  = 0;
        public int m_AutoTeachingAckDelay                   = 10000; //임시
        public string m_AutoTeachingFilePath                = string.Empty;
        public List<TeachingParam> TeachingList             = new List<TeachingParam>();
        Stopwatch TeachingRWRequest                         = new Stopwatch();

        /// <summary>
        /// 티칭 동작 중 여부 리턴
        /// </summary>
        /// <returns></returns>
        public bool IsAutoTeachingRun()
        {
            return m_bAutoTeachingRunning;
        }

        /// <summary>
        /// 티칭 동작 정지 명령
        /// </summary>
        public void AutoTeachingStop()
        {
            m_bAutoTeachingStop = true;
        }

        /// <summary>
        /// STK 오토 티칭 동작
        /// </summary>
        /// <param name="teachingList"></param>
        public void CMD_AutoTeachingRun(List<TeachingParam> teachingList)
        {
            //1. 스레드 진입 전 초기화, 이니셜 과정
            TeachingList.Clear();                               //기존 공정에 사용된 리스트 초기화
            m_AutoTeachingCount         = teachingList.Count;   //신규 리스트 Count 이니셜
            m_AutoTeachingProgress      = 0;                    //진행률 초기화

            for (int nCount = 0; nCount < teachingList.Count; nCount++) //신규 쉘프 리스트의 오토 티칭 상태 모두 초기화(쉘프 마다 공정 상태가 기록 됨, 성공, 실패)
                teachingList[nCount].State = string.Empty;

            m_eAutoTeachingStep         = TeachingControlStep.Initialize;   //스텝 초기화
            m_ePreAutoTeachingStep      = TeachingControlStep.Initialize;   //스텝 초기화
            m_eAutoTeachingErrorCode    = TeachingControlErrorCode.None;    //에러 코드 초기화
            m_TeachingRunningTime.Reset();                                  //진행 시간 초기화
            m_bAutoTeachingRunning      = true;                             //스레드 동작 플래그 초기화
            m_bAutoTeachingStop         = false;                            //스레드 정지 플래그 초기화

            Thread LocalThread = new Thread(delegate ()
            {
                m_TeachingRunningTime.Start(); //진행 시간 측정 시작

                while (m_bAutoTeachingRunning)
                {
                    try
                    {
                        //1. 티칭 유효 상태 체크(통신, 에러 등 체크)
                        if (!IsTeachingValidStatus())
                            m_eAutoTeachingStep = TeachingControlStep.Error;

                        //2. 스텝 변한 경우 로그 작성
                        if (m_ePreAutoTeachingStep != m_eAutoTeachingStep)
                        {
                            LogMsg.AddRackMasterLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterCycleStepInfo, $"RM AutoTeaching Step : {m_ePreAutoTeachingStep} => {m_eAutoTeachingStep}");
                            m_ePreAutoTeachingStep = m_eAutoTeachingStep;
                        }

                        //3. 스텝 수행 진행
                        switch (m_eAutoTeachingStep)
                        {
                            case TeachingControlStep.Idle:
                                break;
                            case TeachingControlStep.Initialize:
                                {
                                    //Initialize Step은 동작 전 준비 과정 
                                    TeachingList.Add(teachingList[m_AutoTeachingProgress]); //수행 할 티칭 Shelf 정보를 삽입
                                    CMD_AutoTeachingCompleteACK_REQ         = false;        //ACK Bit 초기화
                                    CMD_AutoTeachingErrorCompleteACK_REQ    = false;        //ACK Bit 초기화
                                    CMD_CIM_To_STK_Teaching_Shelf_ID        = Convert.ToInt32(teachingList[m_AutoTeachingProgress].ShelfID);    //Teaching 하려는 Shelf ID 전송
                                    CMD_CIM_To_STK_Teaching_X_Pos           = teachingList[m_AutoTeachingProgress].X_Pos;                       //X축 Teaching 기준 위치 전송
                                    CMD_CIM_To_STK_Teaching_Z_Pos           = teachingList[m_AutoTeachingProgress].Z_Pos;                       //Z축 Teaching 기준 위치 전송

                                    teachingList[m_AutoTeachingProgress].State = $"{ShelfTeachingState.Running}";   //Shelf의 티칭 진행 상태 변경
                                    m_eAutoTeachingStep     = TeachingControlStep.InitializeAckWait;                //다음 수행할 스텝 정보로 변경
                                    m_AutoTeachingAckStart  = m_TeachingRunningTime.ElapsedMilliseconds;            //스텝 변경 전 시간 측정
                                }
                                break;
                            case TeachingControlStep.InitializeAckWait:
                                {
                                    //InitializeAckWait Step은 동작 전 준비 과정에서 보낸 정보를 STK에서 잘 받았는 지 확인하는 Step
                                    string ShelfID  = Convert.ToString(Status_STK_To_CIM_TeachingID);   //STK에서 전송한 Teaching ID 확인
                                    short XPos      = Status_STK_To_CIM_Teaching_X_Pos;                 //STK에서 전송한 X축 Teaching 위치 확인
                                    short ZPos      = Status_STK_To_CIM_Teaching_Z_Pos;                 //STK에서 전송한 Z축 Teaching 위치 확인

                                    if (ShelfID == teachingList[m_AutoTeachingProgress].ShelfID &&
                                        XPos == teachingList[m_AutoTeachingProgress].X_Pos &&
                                        ZPos == teachingList[m_AutoTeachingProgress].Z_Pos)
                                    {
                                        //모두 이상 없는 경우 Auto Teaching Start 요청
                                        m_eAutoTeachingStep = TeachingControlStep.AutoTeachingStartRequest;
                                    }
                                    else
                                    {
                                        //3개중 하나라도 일치하지 않는 다면 Timeout에 의한 에러 발생
                                        //현재 시간 - 이전 스텝 마지막 측정 시간 > 10초 넘어가는 경우 발생
                                        if (m_TeachingRunningTime.ElapsedMilliseconds - m_AutoTeachingAckStart > m_AutoTeachingAckDelay)
                                        {
                                            m_eAutoTeachingErrorCode = TeachingControlErrorCode.Ack_TimeOut;
                                            m_eAutoTeachingStep = TeachingControlStep.Error;
                                        }
                                    }
                                }
                                break;
                            case TeachingControlStep.AutoTeachingStartRequest:
                                {
                                    //AutoTeachingStartRequest Step은 Initialize가 모두 정상인 경우 동작 명령을 위한 Step
                                    CMD_AutoTeachingStop_REQ    = false;    //Stop bit 초기화, 전송
                                    CMD_AutoTeachingRun_REQ     = true;     //Run Bit 초기화, 전송

                                    m_eAutoTeachingStep = TeachingControlStep.AutoTeachingStartRequestAckWait;  //다음 수행할 스텝 정보로 변경
                                    m_AutoTeachingAckStart = m_TeachingRunningTime.ElapsedMilliseconds;         //스텝 변경 전 시간 측정
                                }
                                break;
                            case TeachingControlStep.AutoTeachingStartRequestAckWait:
                                {
                                    //AutoTeachingStartRequestAckWait Step은 Auto Teaching Start 요청을 제대로 수신하였는 지 판단하기 위한 Step
                                    if (Status_AutoTeachingRunACK)
                                    {
                                        //Auto Teaching Start ACK를 수신한 경우 Running 상태 체크 스텝으로 변경
                                        m_eAutoTeachingStep = TeachingControlStep.AutoTeachingRunningStateCheck;
                                    }
                                    else
                                    {
                                        //Auto Teaching Start ACK의 반응이 없는 경우 Timeout에 의한 에러 발생
                                        //현재 시간 - 이전 스텝 마지막 측정 시간 > 10초 넘어가는 경우 발생
                                        if (m_TeachingRunningTime.ElapsedMilliseconds - m_AutoTeachingAckStart > m_AutoTeachingAckDelay)
                                        {
                                            m_eAutoTeachingErrorCode    = TeachingControlErrorCode.Ack_TimeOut;
                                            m_eAutoTeachingStep         = TeachingControlStep.Error;
                                        }
                                    }
                                }
                                break;
                            case TeachingControlStep.AutoTeachingRunningStateCheck:
                                {
                                    //AutoTeachingRunningStateCheck Step은 Auto Teaching 진행 상태를 판단하기 위한 Step
                                    
                                    //User가 Stop 버튼을 누른 경우
                                    if (m_bAutoTeachingStop)
                                    {
                                        m_eAutoTeachingStep = TeachingControlStep.Stop;
                                        break;
                                    }

                                    //Auto Teaching 동작 중인 경우
                                    if (Status_AutoTeachingRunningState)
                                    {
                                        if (Status_AutoTeachingComplete && Status_AutoTeachingErrorComplete)
                                        {
                                            //오토 티칭 컴플리트와 에러 컴플리트가 동시에 들어온 경우(문제)
                                            m_eAutoTeachingErrorCode    = TeachingControlErrorCode.CompleteStateDuple_Error;
                                            m_eAutoTeachingStep         = TeachingControlStep.Error;
                                        }
                                        else if (Status_AutoTeachingComplete)
                                        {
                                            //오토 티칭 컴플리트만 들어온 경우(정상 완료)
                                            teachingList[m_AutoTeachingProgress].State  = $"{ShelfTeachingState.Success}";              //Shelf Teaching 상태를 성공으로 변경
                                            CMD_AutoTeachingCompleteACK_REQ             = true;                                         //Complete ACK Bit 전송
                                            m_eAutoTeachingStep                         = TeachingControlStep.RackMasterHandShakeCheck;
                                            m_AutoTeachingAckStart                      = m_TeachingRunningTime.ElapsedMilliseconds;         //스텝 변경 전 시간 측정
                                        }
                                        else if (Status_AutoTeachingErrorComplete)
                                        {
                                            //오토 티칭 에러 컴플리트만 들어온 경우(비정상 완료)
                                            teachingList[m_AutoTeachingProgress].State  = $"{ShelfTeachingState.Fail}";                 //Shelf Teaching 상태를 실패로 변경
                                            CMD_AutoTeachingErrorCompleteACK_REQ        = true;                                         //Error Complete ACK Bit 전송
                                            m_eAutoTeachingStep                         = TeachingControlStep.RackMasterHandShakeCheck;
                                            m_AutoTeachingAckStart                      = m_TeachingRunningTime.ElapsedMilliseconds;         //스텝 변경 전 시간 측정
                                        }
                                    }
                                    else
                                    {
                                        //Auto Teaching 동작 비트가 꺼진 경우(문제)
                                        m_eAutoTeachingErrorCode    = TeachingControlErrorCode.RunningState_Error;
                                        m_eAutoTeachingStep         = TeachingControlStep.Error;
                                    }
                                }
                                break;
                            case TeachingControlStep.RackMasterHandShakeCheck:
                                {
                                    //RackMasterHandShakeCheck Step은 Auto Teaching 진행 관련 플래그를 초기화하기 위한 Step

                                    //User가 Stop 버튼을 누른 경우
                                    if (m_bAutoTeachingStop)
                                    {
                                        m_eAutoTeachingStep = TeachingControlStep.Stop;
                                        break;
                                    }

                                    //STK에서 AutoTeaching Running State가 꺼지는 경우
                                    if (!Status_AutoTeachingRunningState)
                                    {
                                        CMD_AutoTeachingRun_REQ                 = false; //Auto Teaching Run Bit 초기화
                                        CMD_AutoTeachingCompleteACK_REQ         = false; //Auto Teaching Complete ACK Bit 초기화
                                        CMD_AutoTeachingErrorCompleteACK_REQ    = false; //Auto Teaching Error Complete ACK Bit 초기화
                                        m_eAutoTeachingStep                     = TeachingControlStep.NextShelfCheck;
                                    }
                                    else
                                    {
                                        //Status_AutoTeachingRunningState가 꺼지지 않는 경우 Timeout에 의한 에러 발생
                                        //현재 시간 - 이전 스텝 마지막 측정 시간 > 10초 넘어가는 경우 발생
                                        if (m_TeachingRunningTime.ElapsedMilliseconds - m_AutoTeachingAckStart > m_AutoTeachingAckDelay)
                                        {
                                            m_eAutoTeachingErrorCode    = TeachingControlErrorCode.Ack_TimeOut;
                                            m_eAutoTeachingStep         = TeachingControlStep.Error;
                                        }
                                    }
                                }
                                break;
                            case TeachingControlStep.NextShelfCheck:
                                {
                                    //NextShelfCheck Step은 다음 Shelf 정보 체크 Step

                                    //진행 카운트 증가
                                    m_AutoTeachingProgress++;

                                    //User가 Stop 버튼을 누른 경우
                                    if (m_bAutoTeachingStop)
                                    {
                                        m_eAutoTeachingStep = TeachingControlStep.Stop;
                                    }
                                    else
                                    {
                                        if (m_AutoTeachingProgress < teachingList.Count)
                                        {
                                            //현재 진행 카운트가 리스트 카운트보다 적은 경우 Initialize Step 으로 변경 후 다시 진행
                                            m_eAutoTeachingStep = TeachingControlStep.Initialize;
                                        }
                                        else
                                        {
                                            //현재 진행 카운트가 Teaching List의 카운트와 같아 진 경우 완료
                                            m_eAutoTeachingStep = TeachingControlStep.TeachingComplete;
                                        }
                                    }
                                }
                                break;
                            case TeachingControlStep.TeachingComplete:
                                {
                                    //TeachingComplete Step은 Auto Teaching 완료 Step으로 스레드 종료 및 파일 저장

                                    m_bAutoTeachingRunning = false;         //스레드 종료 플래그
                                    SaveAutoTeachingResult(teachingList);   //진행된 티칭 리스트의 티칭 결과를 파일로 저장
                                }
                                break;
                            case TeachingControlStep.Stop:
                                {
                                    //Stop Step은 User의 Stop 버튼에 의한 정지로 스레드 종료 및 플래그 초기화, 파일 저장 진행

                                    m_bAutoTeachingRunning                          = false; //스레드 종료 플래그
                                    CMD_AutoTeachingRun_REQ                         = false; //Auto Teaching Run Bit 초기화, 전송
                                    CMD_AutoTeachingStop_REQ                        = true;  //Auto Teaching Stop Bit 활성화, 전송
                                    CMD_AutoTeachingCompleteACK_REQ                 = false; //Complete ACK Bit 초기화, 전송
                                    CMD_AutoTeachingErrorCompleteACK_REQ            = false; //Error Complete ACK Bit 초기화, 전송
                                    teachingList[m_AutoTeachingProgress].State      = $"{ShelfTeachingState.UserStop}"; //마지막 티칭 쉘프의 결과 정보를 유저 정지로 변경

                                    SaveAutoTeachingResult(teachingList); //진행된 티칭 리스트의 티칭 결과를 파일로 저장
                                }
                                break;
                            case TeachingControlStep.Error:
                                {
                                    //Error Step은 Timeout 또는 비 정상 상황에 대한 종료 스텝으로 스레드 종료 및 플래그 초기화, 파일 저장 진행

                                    m_bAutoTeachingRunning                          = false;
                                    CMD_AutoTeachingRun_REQ                         = false;
                                    CMD_AutoTeachingStop_REQ                        = true;
                                    CMD_AutoTeachingCompleteACK_REQ                 = false;
                                    CMD_AutoTeachingErrorCompleteACK_REQ            = false;
                                    teachingList[m_AutoTeachingProgress].State      = $"{ShelfTeachingState.ErrorStop}"; //마지막 티칭 쉘프의 결과 정보를 에러 정지로 변경

                                    SaveAutoTeachingResult(teachingList);
                                }
                                break;
                        }

                        Thread.Sleep(Master.StepUpdateTime);
                    }
                    catch(Exception ex)
                    {
                        //Exception 발생 시 플래그 초기화
                        m_bAutoTeachingRunning                  = false;
                        CMD_AutoTeachingRun_REQ                 = false;
                        CMD_AutoTeachingStop_REQ                = true;
                        CMD_AutoTeachingCompleteACK_REQ         = false;
                        CMD_AutoTeachingErrorCompleteACK_REQ    = false;
                        LogMsg.AddExceptionLog(ex, $"RackMaster[{GetParam().ID}] Auto Teaching Seq Exception");
                    }
                }

                m_TeachingRunningTime.Stop();
            });
            LocalThread.Start();
        }
        
        /// <summary>
        /// 티칭 모드 설정 - 단일 쉘프 티칭, 복수 쉘프 연속 티칭
        /// </summary>
        /// <param name="eTeachingMode"></param>
        public void SetTeachingMode(TeachingMode eTeachingMode)
        {
            if (IsAutoTeachingRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InAutoTeachingRun"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_eTeachingMode = eTeachingMode;
        }

        /// <summary>
        /// 현재 티칭 모드 리턴
        /// </summary>
        /// <returns></returns>
        public TeachingMode GetTeachingMode()
        {
            return m_eTeachingMode;
        }

        /// <summary>
        /// 티칭 완료 또는 종료 시 티칭 결과 자동 저장 기능
        /// </summary>
        /// <param name="_TeachingList"></param>
        public void SaveAutoTeachingResult(List<TeachingParam> _TeachingList)
        {
            try
            {
                string TeachingResultPath = ManagedFile.ManagedFileInfo.TeachingFileDirectory + "\\" + ManagedFile.ManagedFileInfo.TeachingResultFileName;

                if (!Directory.Exists(ManagedFile.ManagedFileInfo.TeachingFileDirectory))
                    Directory.CreateDirectory(ManagedFile.ManagedFileInfo.TeachingFileDirectory);

                StreamWriter sw = new StreamWriter(TeachingResultPath);

                for(int nCount = 0;nCount < _TeachingList.Count; nCount++)
                    sw.WriteLine($"{_TeachingList[nCount].ShelfID},{_TeachingList[nCount].X_Pos},{_TeachingList[nCount].Z_Pos},{_TeachingList[nCount].State}");

                sw.Close();
                sw.Dispose();
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"RackMaster[{GetParam().ID}] Auto Teaching Reault Save Exception");
            }
        }

        /// <summary>
        /// 티칭 동작 유효 상태 점검
        /// </summary>
        /// <returns></returns>
        public bool IsTeachingValidStatus()
        {
            if (!IsConnected())
            {
                m_eAutoTeachingErrorCode = TeachingControlErrorCode.TCPIP_Disconnection;
                return false;
            }

            if (Status_Error)
            {
                m_eAutoTeachingErrorCode = TeachingControlErrorCode.Status_Error;
                return false;
            }

            if (CMD_EmergencyStop_REQ)
            {
                m_eAutoTeachingErrorCode = TeachingControlErrorCode.Emergency_Stop;
                return false;
            }

            if (m_eControlMode == ControlMode.CIMMode)
            {
                m_eAutoTeachingErrorCode = TeachingControlErrorCode.CIMMode_Error;
                return false;
            }

            if (!Status_AutoMode)
            {
                m_eAutoTeachingErrorCode = TeachingControlErrorCode.AutoMode_Error;
                return false;
            }

            return true;
        }



        /// <summary>
        /// 티칭 쉘프 기준 정보 얻기 명령
        /// 5초 이내 회신 없는 경우 에러
        /// </summary>
        /// <param name="_ShelfID"></param>
        private void CMD_GetShelfInfo(int _ShelfID)
        {
            CMD_CIM_To_STK_Teaching_RW_ID = _ShelfID;
            CMD_TeachingRWRun_REQ = true;
            TeachingRWRequest.Reset();
            TeachingRWRequest.Start();
        }

        /// <summary>
        /// 티칭 쉘프 기준 정보 얻기 명령 이후 지난 시간 리턴[sec]
        /// </summary>
        /// <returns></returns>
        public double GetTeachingRWStopwatchTime()
        {
            return TeachingRWRequest.Elapsed.TotalSeconds;
        }

        /// <summary>
        /// 티칭 쉘프 기준 정보 얻기 명령 동작 중 여부 리턴
        /// </summary>
        /// <returns></returns>
        public bool IsRunTeachingRWStopwatch()
        {
            return TeachingRWRequest.IsRunning;
        }

        /// <summary>
        /// 티칭 쉘프 기준 정보 얻기 명령 초기화
        /// 1. 시간 측정 Stopwatch 초기화
        /// 2. 명령 플래그 초기화
        /// </summary>
        public void StopTeachingRWStopwatch()
        {
            TeachingRWRequest.Stop();
            TeachingRWRequest.Reset();
            CMD_TeachingRWRun_REQ = false;
        }
    }
}
