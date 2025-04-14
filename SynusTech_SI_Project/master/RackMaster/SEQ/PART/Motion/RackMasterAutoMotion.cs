using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using System.Threading;

namespace RackMaster.SEQ.PART {
    public enum ManualSpeedType {
        High = 0,
        Low
    }

    public enum EventList {
        None,
        StoreAlt,
        SourceEmpty,
        DoubleStorage,
        ResumeRequest,
    }

    public enum FromToStepNumber {
        Zero,
        One,
        Two,
        Three,
        Four
    }

    public enum MotionMode {
        From,
        To
    }

    public enum AutoStep {
        Step0_Idle                                      = 0,

        //From Step
        Step100_From_ID_Check                           = 100,  // From ID가 정상적인지 체크
        Step101_From_CST_And_Fork_Home_Check            = 101,  // CST가 없으며 Fork가 Home 위치인지 체크
        Step102_From_Fork_Home_Move                     = 102,  // Fork Homing
        Step103_From_Fork_Home_Check                    = 103,  // Fork Homing 완료 체크
        Step112_From_XZT_From_Move                      = 112,  // X,Z,T 축 타겟 위치로 이동(Slide-No Turn인 경우 X, Z축만)
        Step113_From_XZT_From_Complete                  = 113,  // X,Z,T 축 타겟 위치 정상 도착 확인
        Step114_From_Shelf_Port_Check                   = 114,  // Target ID가 Shelf인지 Port인지 체크
        Step122_From_Port_Ready_Check                   = 122,  // Target ID가 Port인 경우 PIO 시작 및 PIO 비트 체크
        Step123_From_Pick_Sensor_Cehck                  = 123,  // Pick Sensor 체크
        Step130_From_Fork_FWD_Move                      = 130,  // Fork축 전진
        Step132_From_Fork_FWD_Check                     = 132,  // Fork축 전진 도착 확인
        Step133_From_Z_Up                               = 133,  // Z축 Fast Velocity로 상승
        Step134_From_Z_Up_Override_Slow                 = 134,  // Z축 Slow Override 위치에서 Slow Velocity로 속도 Override 후 상승
        Step135_From_Z_Up_Override_Fast                 = 135,  // Z축 Fast Override 위치에서 Fast Velocity로 속도 Override 후 상승
        Step136_From_Z_Inposition_Check                 = 136,  // Z축 타겟 위치 도착 확인
        Step137_From_CST_Check_Sensor                   = 137,  // CST가 정상적으로 안착 되었는지 확인
        Step138_From_Source_Empty_Z_From_Pos_Move       = 138,  // (Source Empty)CST가 존재하지 않을 경우 From 위치로 Z축 하강
        Step139_From_Source_Empty_Z_Inposition_Check    = 139,  // (Source Empty)Z축 하강 도착 확인
        Step140_From_Source_Empty_Fork_Home_Move        = 140,  // (Source Empty)Fork Home Position으로 이동
        Step141_From_Source_Empty_Fork_Home_Check       = 141,  // (Source Empty)Fork Home 위치 도착 확인
        Step142_From_Fork_BWD_Move                      = 142,  // Fork 후진
        Step143_From_Fork_BWD_Check                     = 143,  // Fork 후진 확인
        Step144_From_Port_Ready_Off_Check               = 144,  // Target ID가 Port인 경우 PIO 종료 및 PIO 비트 체크
        Step145_From_Complete                           = 145,  // From Complete

        //To Step
        Step200_To_ID_Check                             = 200,  // To ID가 정상적인지 체크
        Step201_To_CST_And_Fork_Home_Check              = 201,  // CST가 존재하며 Fork가 Home 위치인지 체크
        Step202_To_Fork_Home_Move                       = 202,  // Fork Homing
        Step203_To_Fork_Home_Check                      = 203,  // Fork Homing 완료 체크
        Step204_To_XZT_To_Move                          = 204,  // X,Z,T 축 타겟 위치로 이동(Slide-No Turn인 경우 X, Z축만)
        Step205_To_XZT_To_Complete                      = 205,  // X,Z,T 축 타겟 위치 정상 도착 확인
        Step206_To_Double_Storage_Check                 = 206,  // Double Storage Sensor 체크(Shelf인 경우에만)
        Step207_To_Shelf_Port_Check                     = 207,  // Target ID가 Shelf인지 Port인지 체크
        Step208_To_Port_Ready_Check                     = 208,  // Target ID가 Port인 경우 PIO 시작 및 PIO 비트 체크
        Step209_To_Place_Sensor_Check                   = 209,  // Place Sensor 체크
        Step210_To_Fork_FWD_Move                        = 210,  // Fork축 전진
        Step211_To_Fork_FWD_Check                       = 211,  // Fork축 전진 도착 확인
        Step212_To_Z_Down                               = 212,  // Z축 Fast Velocity로 하강
        Step213_To_Z_Down_Override_Slow                 = 213,  // Z축 Slow Override 위치에서 Slow Velocity로 속도 Override 후 하강
        Step214_To_Z_Down_Override_Fast                 = 214,  // Z축 Fast Override 위치에서 Fast Velocity로 속도 Override 후 하강
        Step215_To_Z_Inposition_Check                   = 215,  // Z축 타겟 위치 도착 확인
        Step216_To_CST_Fork_Placement_Check             = 216,  // CST가 정상적으로 Shelf에 올려졌는지 확인
        Step217_To_Fork_BWD_Move                        = 217,  // Fork 후진
        Step218_To_Fork_BWD_Check                       = 218,  // Fork 후진 확인
        Step219_To_Port_Ready_Off_Check                 = 219,  // Target ID가 Port인 경우 PIO 종료 및 PIO 비트 체크
        Step220_To_Complete                             = 220,  // To Complete

        // Auto Homing 기능
        Step300_Z_Axis_AutoHomingZMove                  = 300,  // Auto Homing을 하기 위해 Z축을 Homing Position 근처로 이동
        Step301_Z_Axis_AutoHomingZMoveCheck             = 301,  // Z축 이동 확인
        Step302_Z_Axis_AutoHomingStart                  = 302,  // Z축 Homing 시작
        Step303_Z_Axis_AutoHomingCheck                  = 303,  // Z축 Homing 완료 체크
        Step304_Z_Axis_AutoHomingComplete               = 304,  // Auto Homing Complete

        Step400_Maint_Move_Fork_Home_Check              = 400,  // Fork가 Home 위치인지 확인
        Step401_Maint_Move_Fork_Home_Move               = 401,  // Fork가 Home 위치가 아닌 경우 Home 위치로 이동
        Step402_Maint_Move_Fork_Home_Move_Check         = 402,  // Fork가 Home 위치 도착하였는지 확인
        Step403_Maint_Move                              = 403,  // 설정된 Maint 위치로 이동
        Step404_Maint_Move_Check                        = 404,  // Maint 위치 도착 확인
        Step405_Maint_Complete                          = 405,  // Maint Complete

        Step500_Error                                   = 500,
        Step501_Stop                                    = 501,

        Step800_Store_Alt                               = 800,
        Step801_Resume_Request                          = 801,
        Step810_Source_Empty                            = 810,
        Step820_Double_Storage                          = 820,
    }

    public enum ManualStep {
        Idle,

        From_XZTGoing,
        From_ForkFWD,
        From_ZUp,
        From_ForkBWD,

        To_XZTGoing,
        To_ForkFWD,
        To_ZDown,
        To_ForkBWD,

        SourceEmpty,
    }

    public enum ManualAutoCylceStep {
        Idle,
        Run,
        FromMotionStart,
        FromMotionCompleteWait,
        ToMotionStart,
        ToMotionCompleteWait,
        Stop,
    }

    public partial class RackMasterMain {
        public partial class RackMasterMotion {
            public enum MotionStepType {
                From,
                To,
            }

            public string GetRackMasterAutoMotionStepString(AutoStep step) {
                switch (step) {
                    case AutoStep.Step0_Idle:
                        return $"{(int)step}_Idle";

                    case AutoStep.Step100_From_ID_Check:
                        return $"{(int)step}_From ID Check";

                    case AutoStep.Step101_From_CST_And_Fork_Home_Check:
                        return $"{(int)step}_From CST And Fork Home Check";

                    case AutoStep.Step102_From_Fork_Home_Move:
                        return $"{(int)step}_From Fork Home Move";

                    case AutoStep.Step103_From_Fork_Home_Check:
                        return $"{(int)step}_From Fork Home Check";

                    case AutoStep.Step112_From_XZT_From_Move:
                        return $"{(int)step}_From XZT Move";

                    case AutoStep.Step113_From_XZT_From_Complete:
                        return $"{(int)step}_From XZT Inpos Check";

                    case AutoStep.Step114_From_Shelf_Port_Check:
                        return $"{(int)step}_From Shelf or Port Check";

                    case AutoStep.Step122_From_Port_Ready_Check:
                        return $"{(int)step}_From Port Ready Check";

                    case AutoStep.Step123_From_Pick_Sensor_Cehck:
                        return $"{(int)step}_From Pick Sensor Check";

                    case AutoStep.Step130_From_Fork_FWD_Move:
                        return $"{(int)step}_From Fork FWD Move";

                    case AutoStep.Step132_From_Fork_FWD_Check:
                        return $"{(int)step}_From Fork FWD Inpos Check";

                    case AutoStep.Step133_From_Z_Up:
                        return $"{(int)step}_From Z Up";

                    case AutoStep.Step134_From_Z_Up_Override_Slow:
                        return $"{(int)step}_From Z Override Slow";

                    case AutoStep.Step135_From_Z_Up_Override_Fast:
                        return $"{(int)step}_From Z Override Fast";

                    case AutoStep.Step136_From_Z_Inposition_Check:
                        return $"{(int)step}_From Z Inpos Check";

                    case AutoStep.Step137_From_CST_Check_Sensor:
                        return $"{(int)step}_From CST Sensor Check";

                    case AutoStep.Step138_From_Source_Empty_Z_From_Pos_Move:
                        return $"{(int)step}_Source Empty Z From Pos Move";

                    case AutoStep.Step139_From_Source_Empty_Z_Inposition_Check:
                        return $"{(int)step}_Source Empty Z Inpos Check";

                    case AutoStep.Step140_From_Source_Empty_Fork_Home_Move:
                        return $"{(int)step}_Source EmptyFork Home Move";

                    case AutoStep.Step141_From_Source_Empty_Fork_Home_Check:
                        return $"{(int)step}_Source Empty Fork Inpos Check";

                    case AutoStep.Step142_From_Fork_BWD_Move:
                        return $"{(int)step}_From Fork BWD Move";

                    case AutoStep.Step143_From_Fork_BWD_Check:
                        return $"{(int)step}_From Fork BWD Inpos Check";

                    case AutoStep.Step144_From_Port_Ready_Off_Check:
                        return $"{(int)step}_From Port Ready Off Check";

                    case AutoStep.Step145_From_Complete:
                        return $"{(int)step}_From Complete";

                    case AutoStep.Step200_To_ID_Check:
                        return $"{(int)step}_To ID Check";

                    case AutoStep.Step201_To_CST_And_Fork_Home_Check:
                        return $"{(int)step}_To CST And Fork Home Check";

                    case AutoStep.Step202_To_Fork_Home_Move:
                        return $"{(int)step}_To Fork Home Move";

                    case AutoStep.Step203_To_Fork_Home_Check:
                        return $"{(int)step}_To Fork Home Check";

                    case AutoStep.Step204_To_XZT_To_Move:
                        return $"{(int)step}_To XZT Move";

                    case AutoStep.Step205_To_XZT_To_Complete:
                        return $"{(int)step}_To XZT Inpos Check";

                    case AutoStep.Step206_To_Double_Storage_Check:
                        return $"{(int)step}_To Double Storage Sensor Check";

                    case AutoStep.Step207_To_Shelf_Port_Check:
                        return $"{(int)step}_To Shelf or Port Check";

                    case AutoStep.Step208_To_Port_Ready_Check:
                        return $"{(int)step}_To Port Ready Check";

                    case AutoStep.Step209_To_Place_Sensor_Check:
                        return $"{(int)step}_To Place Sensor Check";

                    case AutoStep.Step210_To_Fork_FWD_Move:
                        return $"{(int)step}_To Fork FWD Move";

                    case AutoStep.Step211_To_Fork_FWD_Check:
                        return $"{(int)step}_To Fork FWD Inpos Check";

                    case AutoStep.Step212_To_Z_Down:
                        return $"{(int)step}_To Z Down";

                    case AutoStep.Step213_To_Z_Down_Override_Slow:
                        return $"{(int)step}_To Z Override Slow";

                    case AutoStep.Step214_To_Z_Down_Override_Fast:
                        return $"{(int)step}_To Z Override Fast";

                    case AutoStep.Step215_To_Z_Inposition_Check:
                        return $"{(int)step}_To Z Inpos Check";

                    case AutoStep.Step216_To_CST_Fork_Placement_Check:
                        return $"{(int)step}_To CST Fork Placement Check";

                    case AutoStep.Step217_To_Fork_BWD_Move:
                        return $"{(int)step}_To Fork BWD Move";

                    case AutoStep.Step218_To_Fork_BWD_Check:
                        return $"{(int)step}_To Fork BWD Inpos Check";

                    case AutoStep.Step219_To_Port_Ready_Off_Check:
                        return $"{(int)step}_To Port Ready Off Check";

                    case AutoStep.Step220_To_Complete:
                        return $"{(int)step}_To Complete";

                    case AutoStep.Step300_Z_Axis_AutoHomingZMove:
                        return $"{(int)step}_Z Axis Auto Homing Z Move";

                    case AutoStep.Step301_Z_Axis_AutoHomingZMoveCheck:
                        return $"{(int)step}_Z Axis Auto Homing Z Move Check";

                    case AutoStep.Step302_Z_Axis_AutoHomingStart:
                        return $"{(int)step}_Z Axis Auto Homing Start";

                    case AutoStep.Step303_Z_Axis_AutoHomingCheck:
                        return $"{(int)step}_Z Axis Auto Homing Check";

                    case AutoStep.Step304_Z_Axis_AutoHomingComplete:
                        return $"{(int)step}_Z Axis Auto Homing Complete";

                    case AutoStep.Step400_Maint_Move_Fork_Home_Check:
                        return $"{(int)step}_Maint Fork Home Check";

                    case AutoStep.Step401_Maint_Move_Fork_Home_Move:
                        return $"{(int)step}_Maint Fork Home Move";

                    case AutoStep.Step402_Maint_Move_Fork_Home_Move_Check:
                        return $"{(int)step}_Maint Fork Home Move Check";

                    case AutoStep.Step403_Maint_Move:
                        return $"{(int)step}_Maint Move";

                    case AutoStep.Step404_Maint_Move_Check:
                        return $"{(int)step}_Maint Move Inposition Check";

                    case AutoStep.Step405_Maint_Complete:
                        return $"{(int)step}_Maint Complete";

                    case AutoStep.Step500_Error:
                        return $"{(int)step}_Error";

                    case AutoStep.Step501_Stop:
                        return $"{(int)step}_Stop";

                    case AutoStep.Step800_Store_Alt:
                        return $"{(int)step}_Store Alt";

                    case AutoStep.Step801_Resume_Request:
                        return $"{(int)step}_Resume Request";

                    case AutoStep.Step810_Source_Empty:
                        return $"{(int)step}_Source Empty";

                    case AutoStep.Step820_Double_Storage:
                        return $"{(int)step}_Double Storage";
                }

                return "";
            }

            private AutoStep m_autoMotionStep;
            private AutoStep m_logPrevAutoStep;
            private ManualStep m_manualMotionStep;
            private SEQ.CLS.Timer m_autoMotionTimer;
            private SEQ.CLS.Timer[] m_cycleTimer;
            private Axis m_axis;
            private RackMasterMain m_main;
            private RackMasterParam m_param;
            private TeachingData m_teaching;
            private RackMasterData m_data;
            private ManualSpeedType m_speedType;
            private int m_fromID;
            private int m_toID;

            private AxisStep[] m_motionStep;
            private double[] m_currentTarget;

            private int m_inposCheckTime = 150;
            private bool m_PIOState = false;

            private bool m_isManualAutoCylceMode = false;
            private bool m_isManualAutoCylceRun = false;
            private int m_manualAutoCylceCount = 0;
            private ManualAutoCylceStep m_manualAutoCylceStep = ManualAutoCylceStep.Idle;
            private Thread m_manualAutoCylce;

            private Average<double> m_avrTorqueX = new Average<double>();
            private Average<double> m_avrTorqueZ = new Average<double>();
            private Average<double> m_avrTorqueA = new Average<double>();
            private Average<double> m_avrTorqueT = new Average<double>();

            private Max<double> m_maxTorqueX = new Max<double>();
            private Max<double> m_maxTorqueZ = new Max<double>();
            private Max<double> m_maxTorqueA = new Max<double>();
            private Max<double> m_maxTorqueT = new Max<double>();

            public RackMasterMotion(RackMasterMain main) {
                m_logPrevAutoStep           = AutoStep.Step0_Idle;
                m_autoMotionStep            = AutoStep.Step0_Idle;
                m_manualMotionStep          = ManualStep.Idle;
                m_axis                      = Axis.Instance;
                m_autoMotionTimer           = new SEQ.CLS.Timer();
                m_cycleTimer                = new CLS.Timer[(int)CycleTime.MAX_COUNT];
                for (int i = 0; i < m_cycleTimer.Length; i++)
                    m_cycleTimer[i] = new CLS.Timer();
                m_main                      = main;
                m_param                     = m_main.m_param;
                m_teaching                  = m_main.m_teaching;
                m_data                      = m_main.m_data;

                m_autoTeachingSensorData    = new AutoTeachingData();
                m_autoTeachingStep          = AutoTeachingStep.Step0_Idle;
                m_logPrevTeachingStep       = AutoTeachingStep.Step0_Idle;
                m_autoTeachingPrevStep      = AutoTeachingStep.Step0_Idle;
                m_autoTeachingTimer         = new CLS.Timer();
                m_autoTeachingTarget        = new TargetData();
                m_autoTeachingStepTimer     = new CLS.Timer();
                m_autoTeachingStepTimer.Stop();

                m_speedType                 = ManualSpeedType.Low;

                m_motionStep = new AxisStep[m_param.GetAxisCount()];
                for (int i = 0; i < m_param.GetAxisCount(); i++) {
                    m_motionStep[i] = AxisStep.Idle;
                }
                m_currentTarget = new double[m_param.GetAxisCount()];
                SetAutoMotionStep(AutoStep.Step0_Idle);

                m_manualAutoCylce = null;
                m_isManualAutoTeachingRun = false;

                m_avrTorqueX = new Average<double>();
                m_avrTorqueZ = new Average<double>();
                m_avrTorqueA = new Average<double>();
                m_avrTorqueT = new Average<double>();

                m_maxTorqueX = new Max<double>();
                m_maxTorqueZ = new Max<double>();
                m_maxTorqueA = new Max<double>();
                m_maxTorqueT = new Max<double>();
            }
            /// <summary>
            /// 현재의 Auto Motion Step 변경
            /// </summary>
            /// <param name="step"></param>
            public void SetAutoMotionStep(AutoStep step) {
                m_autoMotionTimer.Restart();
                m_autoMotionStep = step;
            }
            /// <summary>
            /// 현재 Auto Motion Running 중인지
            /// </summary>
            /// <returns></returns>
            public bool IsAutoMotionRun() {
                return (GetCurrentAutoStep() != AutoStep.Step0_Idle);
            }

            // False면 아직 Delay중 , True면 설정한 시간이 지남
            private bool AutoMotionDelay(long timeMilliseconds) {
                return m_autoMotionTimer.Delay(timeMilliseconds);
            }
            /// <summary>
            /// 각 구간(혹은 Total)별로 걸린 시간 측정 타이머 시작
            /// </summary>
            /// <param name="type"></param>
            private void CycleTimerStart(CycleTime type) {
                m_cycleTimer[(int)type].Stop();
                m_cycleTimer[(int)type].Reset();
                m_cycleTimer[(int)type].Start();
            }
            /// <summary>
            /// 각 구간(혹은 Total)별로 걸린 시간 측정 후 데이터 저장
            /// </summary>
            /// <param name="type"></param>
            private void CycleTimerStopAndSaveData(CycleTime type) {
                if(m_cycleTimer[(int)type].IsTimerStarted())
                    m_cycleTimer[(int)type].Stop();
                m_data.SetRackMasterCycleTime(type, (int)m_cycleTimer[(int)type].ElapsedMilliseconds());
            }
            /// <summary>
            /// From ID 설정
            /// </summary>
            /// <param name="id"></param>
            public void SetAutoMotionFromID(int id) {
                if (GetCurrentAutoStep() != AutoStep.Step0_Idle)
                    return;

                if(m_fromID != id) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Set From ID={id}"));
                }

                m_fromID = id;
            }
            /// <summary>
            /// To ID 설정
            /// </summary>
            /// <param name="id"></param>
            public void SetAutoMotionToID(int id) {
                if (GetCurrentAutoStep() != AutoStep.Step0_Idle)
                    return;

                if(m_toID != id) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Set to ID={id}"));
                }

                m_toID = id;
            }
            /// <summary>
            /// 현재 설정된 From ID 반환
            /// </summary>
            /// <returns></returns>
            public int GetCurrentTargetFromID() {
                return m_fromID;
            }
            /// <summary>
            /// 현재 설정된 To ID 반환
            /// </summary>
            /// <returns></returns>
            public int GetCurrentTargetToID() {
                return m_toID;
            }
            /// <summary>
            /// 현재 설정된 From/To ID 클리어
            /// </summary>
            public void ClearFromToID() {
                if(GetCurrentTargetFromID() != 0 || GetCurrentTargetToID() != 0) {
                    m_fromID = 0;
                    m_toID = 0;
                    //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"From/To ID Clear"));
                }
            }
            /// <summary>
            /// 현재 Port와 PIO 중인지 판단
            /// </summary>
            /// <returns></returns>
            public bool IsPIOState() {
                return m_PIOState;
            }
            /// <summary>
            /// From 개시
            /// </summary>
            public void AutoMotionFromStart() {
                if (IsPIOState())
                    return;

                if (IsAutoTeachingRun())
                    return;

                if (GetCurrentAutoStep() == AutoStep.Step0_Idle)
                    SetAutoMotionStep(AutoStep.Step100_From_ID_Check);
            }
            /// <summary>
            /// To 개시
            /// </summary>
            public void AutoMotionToStart() {
                if (IsPIOState())
                    return;

                if (IsAutoTeachingRun())
                    return;

                if (GetCurrentAutoStep() == AutoStep.Step0_Idle)
                    SetAutoMotionStep(AutoStep.Step200_To_ID_Check);
            }
            /// <summary>
            /// Maint Move 시작
            /// </summary>
            public void MaintMoveStart() {
                if (IsPIOState() || IsAutoMotionRun() || IsAutoTeachingRun())
                    return;

                if (GetCurrentAutoStep() == AutoStep.Step0_Idle)
                    SetAutoMotionStep(AutoStep.Step400_Maint_Move_Fork_Home_Check);
            }
            /// <summary>
            /// Manual Auto Cycle Step 반환(현재 사용 X)
            /// </summary>
            /// <returns></returns>
            public ManualAutoCylceStep GetManualAutoCylceStep() {
                return m_manualAutoCylceStep;
            }
            /// <summary>
            /// 현재까지 진행된 Manual Auto Cycle Count 반환(현재 사용 X)
            /// </summary>
            /// <returns></returns>
            public int GetManualAutoCycleCount() {
                return m_manualAutoCylceCount;
            }
            /// <summary>
            /// Manual Auto Cycle 정지(현재 사용 X)
            /// </summary>
            public void StopManualAutoCycle() {
                if (m_isManualAutoCylceRun) {
                    m_manualAutoCylceStep = ManualAutoCylceStep.Stop;
                }
            }
            /// <summary>
            /// Manual Auto Cycle 중인지(현재 사용 X)
            /// Manual Auto Cycle이란 Master처럼 From/To 동작을 반복하는 것을 RackMaster 자체에서도 할 수 있는 기능
            /// </summary>
            /// <returns></returns>
            public bool IsManualAutoCycleRun() {
                return m_isManualAutoCylceRun;
            }

            // Manual로 돌리는 Auto Cycle
            public void StartManualAutoCycle(int count, int fromId, int toId) {
                if (GetCurrentAutoStep() != AutoStep.Step0_Idle) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Warning, AutoStep is Not Idle"));
                    return;
                }

                if (m_main.IsAutoState()) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Warning, Auto State is On"));
                    return;
                }

                if (GetCurerntManualStep() != ManualStep.Idle) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Warning, ManualStep is Not Idle"));
                    return;
                }

                if (!m_teaching.IsEnablePort(fromId) || !m_teaching.IsExistPortOrShelf(fromId) || !m_teaching.IsExistTeachingData(fromId)) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Warning, From ID Error"));
                    return;
                }

                if (!m_teaching.IsEnablePort(toId) || !m_teaching.IsExistPortOrShelf(toId) || !m_teaching.IsExistTeachingData(toId)) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Warning, To ID Error"));
                    return;
                }

                foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    if(GetAxisFlag(AxisFlagType.Alarm, axis)) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Manual Auto Cycle Warning, {axis} Axis is Alarm"));
                        return;
                    }
                }

                if (!IsAllServoOn()) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Warning, All Axis is Not Servo On!"));
                    return;
                }

                if (!IsAllHomeDone()) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Warning, Axis in Not Home Done"));
                    return;
                }

                if(count <= 0) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Manual Auto Cycle Warning, Count Error, Count={count}"));
                    return;
                }

                if(GetManualAutoCylceStep() != ManualAutoCylceStep.Idle) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Manual Auto Cycle Warning, Curret Step is Not Idle, Step={GetManualAutoCylceStep()}"));
                    return;
                }

                if (m_manualAutoCylce == null) {
                    m_manualAutoCylce = new Thread(delegate () {
                        SetAutoMotionFromID(fromId);
                        SetAutoMotionToID(toId);

                        m_isManualAutoCylceRun = true;
                        m_manualAutoCylceStep = ManualAutoCylceStep.Run;
                        int currentCount = 0;
                        m_manualAutoCylceCount = currentCount;
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cycle Start"));
                        while (m_isManualAutoCylceRun) {
                            if (m_main.m_alarm.IsAlarmState() && !m_main.Interlock_OnlyMSTDisconnectedAlarmOccurred()) {
                                m_isManualAutoCylceRun = false;
                                m_isManualAutoCylceMode = false;
                                m_manualAutoCylceStep = ManualAutoCylceStep.Idle;
                                break;
                            }
                            switch (m_manualAutoCylceStep) {
                                case ManualAutoCylceStep.Idle:
                                    if(currentCount == count && GetCurrentAutoStep() == AutoStep.Step0_Idle) {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cylce Finish"));
                                        m_isManualAutoCylceRun = false;
                                    }
                                    break;

                                case ManualAutoCylceStep.Run:
                                    if(GetCurrentAutoStep() == AutoStep.Step0_Idle) {
                                        if (m_main.IsCassetteOn()) {
                                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cycle To Motion Start"));
                                            m_manualAutoCylceStep = ManualAutoCylceStep.ToMotionStart;
                                        }
                                        else {
                                            Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cycle From Motion Start"));
                                            m_manualAutoCylceStep = ManualAutoCylceStep.FromMotionStart;
                                        }
                                    }
                                    break;

                                case ManualAutoCylceStep.FromMotionStart:
                                    SetAutoMotionStep(AutoStep.Step100_From_ID_Check);
                                    m_manualAutoCylceStep = ManualAutoCylceStep.FromMotionCompleteWait;
                                    break;

                                case ManualAutoCylceStep.FromMotionCompleteWait:
                                    if(GetCurrentAutoStep() == AutoStep.Step145_From_Complete) {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cycle From Motion Finish"));
                                        SetAutoMotionStep(AutoStep.Step0_Idle);
                                        m_manualAutoCylceStep = ManualAutoCylceStep.ToMotionStart;
                                        Thread.Sleep(500);
                                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cycle To Motion Start"));
                                    }
                                    break;

                                case ManualAutoCylceStep.ToMotionStart:
                                    SetAutoMotionStep(AutoStep.Step200_To_ID_Check);
                                    m_manualAutoCylceStep = ManualAutoCylceStep.ToMotionCompleteWait;
                                    break;

                                case ManualAutoCylceStep.ToMotionCompleteWait:
                                    if(GetCurrentAutoStep() == AutoStep.Step220_To_Complete) {
                                        SetAutoMotionStep(AutoStep.Step0_Idle);
                                        m_manualAutoCylceStep = ManualAutoCylceStep.Idle;
                                        currentCount++;
                                        m_manualAutoCylceCount = currentCount;
                                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Manual Auto Cycle To Motion Finish, Current Count={currentCount}"));
                                        Thread.Sleep(500);
                                    }
                                    break;

                                case ManualAutoCylceStep.Stop:
                                    if(GetCurrentAutoStep() == AutoStep.Step145_From_Complete || GetCurrentAutoStep() == AutoStep.Step220_To_Complete) {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cylce Stop"));
                                        m_isManualAutoCylceRun = false;
                                        m_manualAutoCylceStep = ManualAutoCylceStep.Idle;
                                    }
                                    break;
                            }
                        }

                        m_manualAutoCylce = null;
                    });
                    m_manualAutoCylce.IsBackground = true;
                    m_manualAutoCylce.Name = $"Manual Auto Cycle Thread";
                    m_manualAutoCylce.Start();
                }
            }
            /// <summary>
            /// Manual(Test Drive) 속도 타입 설정(High or Low)
            /// </summary>
            /// <param name="speedType"></param>
            public void SetManualSpeedType(ManualSpeedType speedType) {
                m_speedType = speedType;
            }
            /// <summary>
            /// 현재 설정된 Manual(Test Drive) 속도 타입 반환
            /// </summary>
            /// <returns></returns>
            public ManualSpeedType GetCurrentManualSpeedType() {
                return m_speedType;
            }
            /// <summary>
            /// Manual(Test Drive) 시작
            /// </summary>
            /// <param name="id"></param>
            /// <param name="mode"></param>
            /// <returns></returns>
            public bool StartManualRun(int id, MotionMode mode) {
                if (IsManualAutoCylceMode())
                    return false;

                if (!m_main.Interlock_PortOrShelfReady(id)) return false;

                if (m_main.m_alarm.IsAlarmState() && !m_main.Interlock_OnlyMSTDisconnectedAlarmOccurred()) return false;

                if (m_manualMotionStep == ManualStep.Idle) {
                    if(mode == MotionMode.From) {
                        m_manualMotionStep = ManualStep.From_XZTGoing;
                    }else if(mode == MotionMode.To) {
                        m_manualMotionStep = ManualStep.To_XZTGoing;
                    }
                }

                switch (m_manualMotionStep) {
                    case ManualStep.From_XZTGoing:
                        SetAutoMotionFromID(id);
                        SetAutoMotionStep(AutoStep.Step100_From_ID_Check);
                        break;

                    case ManualStep.From_ForkFWD:
                        SetAutoMotionFromID(id);
                        if(m_teaching.GetPortType(id) != PortType.SHELF) {
                            if (IsPortSensorEnabled(id)) {
                                SetAutoMotionStep(AutoStep.Step123_From_Pick_Sensor_Cehck);
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step130_From_Fork_FWD_Move);
                            }
                        }
                        else {
                            SetAutoMotionStep(AutoStep.Step123_From_Pick_Sensor_Cehck);
                        }
                        break;

                    case ManualStep.From_ZUp:
                        SetAutoMotionFromID(id);
                        SetAutoMotionStep(AutoStep.Step133_From_Z_Up);
                        break;

                    case ManualStep.From_ForkBWD:
                        SetAutoMotionFromID(id);
                        SetAutoMotionStep(AutoStep.Step142_From_Fork_BWD_Move);
                        break;

                    case ManualStep.To_XZTGoing:
                        SetAutoMotionToID(id);
                        SetAutoMotionStep(AutoStep.Step200_To_ID_Check);
                        break;

                    case ManualStep.To_ForkFWD:
                        SetAutoMotionToID(id);
                        SetAutoMotionStep(AutoStep.Step206_To_Double_Storage_Check);
                        break;

                    case ManualStep.To_ZDown:
                        SetAutoMotionToID(id);
                        SetAutoMotionStep(AutoStep.Step212_To_Z_Down);
                        break;

                    case ManualStep.To_ForkBWD:
                        SetAutoMotionToID(id);
                        SetAutoMotionStep(AutoStep.Step216_To_CST_Fork_Placement_Check);
                        break;
                }

                return true;
            }
            /// <summary>
            /// 현재 진행된 Manual(Test Drive) 스텝 반환
            /// </summary>
            /// <returns></returns>
            public ManualStep GetCurerntManualStep() {
                return m_manualMotionStep;
            }
            /// <summary>
            /// 정상적으로 Manual(Test Drive) 마무리 시 해당 함수 호출
            /// </summary>
            public void CompleteManualRun() {
                if (m_main.IsAutoState())
                    return;

                if (IsManualAutoCylceMode())
                    return;

                SetAutoMotionStep(AutoStep.Step0_Idle);
                SetAutoMotionFromID(0);
                SetAutoMotionToID(0);

                if (m_manualMotionStep == ManualStep.To_ForkBWD || m_manualMotionStep == ManualStep.From_ForkBWD || GetCurrentAutoStep() == AutoStep.Step141_From_Source_Empty_Fork_Home_Check)
                    m_manualMotionStep = ManualStep.Idle;
                else
                    m_manualMotionStep++;
            }
            /// <summary>
            /// Manual(Test Drive) 종료
            /// </summary>
            public void StopManualRun() {
                if (m_main.IsAutoState())
                    return;

                if (IsManualAutoCylceMode())
                    return;

                SetAutoMotionFromID(0);
                SetAutoMotionToID(0);

                if (GetCurrentAutoStep() == AutoStep.Step0_Idle)
                    return;

                SetAutoMotionStep(AutoStep.Step501_Stop);
            }
            /// <summary>
            /// 현재까지 진행된 Manual(Test Drive) 스텝 클리어(Idle로 전환)
            /// </summary>
            public void ResetManualRun() {
                if (m_main.IsAutoState())
                    return;

                if (IsManualAutoCylceMode())
                    return;

                StopManualRun();
                m_manualMotionStep = ManualStep.Idle;
            }
            /// <summary>
            /// Manual Auto Cycle Mode 설정(현재 사용 X)
            /// </summary>
            /// <param name="isAutoCycle"></param>
            public void SetManualAutoCycleMode(bool isAutoCycle) {
                if (GetCurrentAutoStep() != AutoStep.Step0_Idle || GetCurrentAutoStep() != AutoStep.Step500_Error)
                    return;

                if (m_main.IsAutoState())
                    return;

                m_isManualAutoCylceMode = isAutoCycle;
                if (m_isManualAutoCylceMode) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Maual Auto Cycle Mode On"));
                }
                else {
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, "Manual Auto Cycle Mode Off"));
                }
            }
            /// <summary>
            /// 현재 Manual Auto Cycle 모드인지(현재 사용 X)
            /// </summary>
            /// <returns></returns>
            public bool IsManualAutoCylceMode() {
                return m_isManualAutoCylceMode;
            }
            /// <summary>
            /// PIO Bit 클리어
            /// </summary>
            public void SetPIOInitial() {
                SetSendBit(SendBitMap.PIO_TR_Request, false);
                SetSendBit(SendBitMap.PIO_Busy, false);
                SetSendBit(SendBitMap.PIO_Complete, false);
                SetSendBit(SendBitMap.PIO_STK_Error, false);
                m_PIOState = false;
            }
            /// <summary>
            /// 각 축 별로 Average, Max Torque 설정
            /// </summary>
            private void SetAxisData() {
                switch (GetCurrentAutoStep()) {
                    case AutoStep.Step0_Idle:
                        m_avrTorqueX.Clear();
                        m_avrTorqueZ.Clear();
                        m_avrTorqueA.Clear();
                        m_avrTorqueT.Clear();

                        m_maxTorqueX.Clear();
                        m_maxTorqueZ.Clear();
                        m_maxTorqueA.Clear();
                        m_maxTorqueT.Clear();
                        break;

                    case AutoStep.Step112_From_XZT_From_Move:
                    case AutoStep.Step113_From_XZT_From_Complete:
                    case AutoStep.Step204_To_XZT_To_Move:
                    case AutoStep.Step205_To_XZT_To_Complete:
                        m_avrTorqueX.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.X_Axis));
                        m_avrTorqueZ.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.Z_Axis));
                        m_maxTorqueX.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.X_Axis));
                        m_maxTorqueZ.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.Z_Axis));

                        if (GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move ||
                            GetCurrentAutoStep() == AutoStep.Step113_From_XZT_From_Complete) {
                            m_data.SetRackMasterAxisData(AxisDataList.From_XZT_AverageTorque_X, m_avrTorqueX.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.From_XZT_AverageTorque_Z, m_avrTorqueZ.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.From_XZT_MaxTorque_X, m_maxTorqueX.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.From_XZT_MaxTorque_Z, m_maxTorqueZ.Result());
                        }
                        else if (GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                           GetCurrentAutoStep() == AutoStep.Step205_To_XZT_To_Complete) {
                            m_data.SetRackMasterAxisData(AxisDataList.To_XZT_AverageTorque_X, m_avrTorqueX.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.To_XZT_AverageTorque_Z, m_avrTorqueZ.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.To_XZT_MaxTorque_X, m_maxTorqueX.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.To_XZT_MaxTorque_Z, m_maxTorqueZ.Result());
                        }

                        if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                            m_avrTorqueT.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.T_Axis));
                            m_maxTorqueT.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.T_Axis));

                            if (GetCurrentAutoStep() == AutoStep.Step112_From_XZT_From_Move ||
                                GetCurrentAutoStep() == AutoStep.Step113_From_XZT_From_Complete) {
                                m_data.SetRackMasterAxisData(AxisDataList.From_XZT_AverageTorque_T, m_avrTorqueT.Result());
                                m_data.SetRackMasterAxisData(AxisDataList.From_XZT_MaxTorque_T, m_maxTorqueT.Result());
                            }
                            else if (GetCurrentAutoStep() == AutoStep.Step204_To_XZT_To_Move ||
                               GetCurrentAutoStep() == AutoStep.Step205_To_XZT_To_Complete) {
                                m_data.SetRackMasterAxisData(AxisDataList.To_XZT_AverageTorque_T, m_avrTorqueT.Result());
                                m_data.SetRackMasterAxisData(AxisDataList.To_XZT_MaxTorque_T, m_maxTorqueT.Result());
                            }
                        }
                        break;

                    case AutoStep.Step130_From_Fork_FWD_Move:
                    case AutoStep.Step132_From_Fork_FWD_Check:
                    case AutoStep.Step210_To_Fork_FWD_Move:
                    case AutoStep.Step211_To_Fork_FWD_Check:
                        m_avrTorqueZ.Clear();
                        m_maxTorqueZ.Clear();

                        m_avrTorqueA.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.A_Axis));
                        m_maxTorqueA.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.A_Axis));

                        if (GetCurrentAutoStep() == AutoStep.Step130_From_Fork_FWD_Move || GetCurrentAutoStep() == AutoStep.Step132_From_Fork_FWD_Check) {
                            m_data.SetRackMasterAxisData(AxisDataList.From_ForkFWD_AverageTorque_A, m_avrTorqueA.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.From_ForkFWD_MaxTorque_A, m_maxTorqueA.Result());
                        }
                        else if (GetCurrentAutoStep() == AutoStep.Step210_To_Fork_FWD_Move ||
                           GetCurrentAutoStep() == AutoStep.Step211_To_Fork_FWD_Check) {
                            m_data.SetRackMasterAxisData(AxisDataList.To_ForkFWD_AverageTorque_A, m_avrTorqueA.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.To_ForkFWD_MaxTorque_A, m_maxTorqueA.Result());
                        }
                        break;

                    case AutoStep.Step133_From_Z_Up:
                    case AutoStep.Step134_From_Z_Up_Override_Slow:
                    case AutoStep.Step135_From_Z_Up_Override_Fast:
                    case AutoStep.Step136_From_Z_Inposition_Check:
                    case AutoStep.Step212_To_Z_Down:
                    case AutoStep.Step213_To_Z_Down_Override_Slow:
                    case AutoStep.Step214_To_Z_Down_Override_Fast:
                    case AutoStep.Step215_To_Z_Inposition_Check:
                        m_avrTorqueA.Clear();
                        m_maxTorqueA.Clear();

                        m_avrTorqueZ.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.Z_Axis));
                        m_maxTorqueZ.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.Z_Axis));

                        if(GetCurrentAutoStep() == AutoStep.Step133_From_Z_Up || GetCurrentAutoStep() == AutoStep.Step134_From_Z_Up_Override_Slow ||
                            GetCurrentAutoStep() == AutoStep.Step135_From_Z_Up_Override_Fast || GetCurrentAutoStep() == AutoStep.Step136_From_Z_Inposition_Check) {
                            m_data.SetRackMasterAxisData(AxisDataList.From_Z_Up_AverageTorque_Z, m_avrTorqueZ.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.From_Z_Up_MaxTorque_Z, m_maxTorqueZ.Result());
                        }else if(GetCurrentAutoStep() == AutoStep.Step212_To_Z_Down || GetCurrentAutoStep() == AutoStep.Step213_To_Z_Down_Override_Slow ||
                            GetCurrentAutoStep() == AutoStep.Step214_To_Z_Down_Override_Fast || GetCurrentAutoStep() == AutoStep.Step215_To_Z_Inposition_Check) {
                            m_data.SetRackMasterAxisData(AxisDataList.To_Z_Down_AverageTorque_Z, m_avrTorqueZ.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.To_Z_Down_MaxTorque_Z, m_maxTorqueZ.Result());
                        }
                        break;

                    case AutoStep.Step142_From_Fork_BWD_Move:
                    case AutoStep.Step143_From_Fork_BWD_Check:
                    case AutoStep.Step217_To_Fork_BWD_Move:
                    case AutoStep.Step218_To_Fork_BWD_Check:
                        m_avrTorqueA.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.A_Axis));
                        m_maxTorqueA.Insert(GetAxisStatus(AxisStatusType.trq_act, AxisList.A_Axis));

                        if(GetCurrentAutoStep() == AutoStep.Step142_From_Fork_BWD_Move || GetCurrentAutoStep() == AutoStep.Step143_From_Fork_BWD_Check) {
                            m_data.SetRackMasterAxisData(AxisDataList.From_ForkBWD_AverageTorque_A, m_avrTorqueA.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.From_ForkBWD_MaxTorque_A, m_maxTorqueA.Result());
                        }else if(GetCurrentAutoStep() == AutoStep.Step217_To_Fork_BWD_Move || GetCurrentAutoStep() == AutoStep.Step218_To_Fork_BWD_Check) {
                            m_data.SetRackMasterAxisData(AxisDataList.To_ForkBWD_AverageToruqe_A, m_avrTorqueA.Result());
                            m_data.SetRackMasterAxisData(AxisDataList.To_ForkBWD_MaxTorque_A, m_maxTorqueA.Result());
                        }
                        break;
                }
            }

            // Auto Motion Run
            public void AutoMotionRun() {
                if (m_logPrevAutoStep != m_autoMotionStep) {
                    m_logPrevAutoStep = m_autoMotionStep;
                    if(m_autoMotionStep == AutoStep.Step800_Store_Alt || m_autoMotionStep == AutoStep.Step801_Resume_Request ||
                        m_autoMotionStep == AutoStep.Step810_Source_Empty || m_autoMotionStep == AutoStep.Step820_Double_Storage) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterEvent, $"{m_autoMotionStep}"));
                    }
                    else {
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAutoStep, $"{m_autoMotionStep}"));
                    }
                }

                UpdateAxisStep();

                if (IsAutoTeachingRun())
                    return;

                if (m_main.m_alarm.IsAlarmState()) {
                    if(GetCurrentAutoStep() != AutoStep.Step500_Error) {
                        if (m_main.IsAutoState()) {
                            m_manualMotionStep = ManualStep.Idle;
                            EmergencyStop();
                            SetAutoMotionStep(AutoStep.Step500_Error);
                            SetPIOInitial();
                        }
                        else {
                            if (!m_main.Interlock_OnlyMSTDisconnectedAlarmOccurred()) {
                                m_manualMotionStep = ManualStep.Idle;
                                EmergencyStop();
                                SetAutoMotionStep(AutoStep.Step500_Error);
                                SetPIOInitial();
                            }
                        }
                    }
                }
                else if (m_main.GetReceiveBit(ReceiveBitMap.MST_Emo_Request) || m_main.GetReceiveBit(ReceiveBitMap.MST_Soft_Error_State) || !m_main.IsConnected_EtherCAT()) {
                    if (GetCurrentAutoStep() != AutoStep.Step500_Error && GetCurrentAutoStep() != AutoStep.Step0_Idle) {
                        EmergencyStop();
                        m_manualMotionStep = ManualStep.Idle;
                        SetAutoMotionStep(AutoStep.Step500_Error);
                        SetPIOInitial();
                    }
                }

                if (GetCurrentAutoStep() == AutoStep.Step500_Error) {
                    if (!m_main.m_alarm.IsAlarmState()) {
                        SetAutoMotionStep(AutoStep.Step0_Idle);
                    }
                    return;
                }

                SetAxisData();

                switch (m_autoMotionStep) {
                    case AutoStep.Step0_Idle:
                        //SetPIOInitial();
                        break;

                    // 전달 받은 ID에 대한 Error Check
                    case AutoStep.Step100_From_ID_Check:
                        m_data.SetRackMasterCycleMode(MotionMode.From);
                        CycleTimerStart(CycleTime.From_TotalCycle); // 시간 측정
                        SetAutoMotionStep(AutoStep.Step101_From_CST_And_Fork_Home_Check);

                        break;

                    // CST 안착 체크랑 Fork Home 위치 체크
                    case AutoStep.Step101_From_CST_And_Fork_Home_Check:
                        if (ForkHomeCheck()) {
                            SetAutoMotionStep(AutoStep.Step112_From_XZT_From_Move);
                        }
                        else {
                            SetAutoMotionStep(AutoStep.Step102_From_Fork_Home_Move);
                        }

                        break;

                    // Fork Home으로
                    case AutoStep.Step102_From_Fork_Home_Move:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkHoming();
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step103_From_Fork_Home_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkHoming();
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step103_From_Fork_Home_Check);
                            }
                        }

                        break;

                    // Fork Home 도착 했는지 확인
                    case AutoStep.Step103_From_Fork_Home_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                SetAutoMotionStep(AutoStep.Step112_From_XZT_From_Move);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAutoMotionStep(AutoStep.Step112_From_XZT_From_Move);
                            }
                        }

                        break;

                    // XZT 목적지 이동
                    case AutoStep.Step112_From_XZT_From_Move:
                        if (m_teaching.GetPortType(GetCurrentTargetFromID()) == PortType.OVEN_PORT) {
                            SetSendBit(SendBitMap.PIO_TR_Request, true);
                        }

                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                                XZTMove(GetCurrentTargetFromID(), MotionStepType.From);
                                CycleTimerStart(CycleTime.From_XZT_Move); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.X_Axis) != AxisStep.Idle &&
                               GetAxisStep(AxisList.Z_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step113_From_XZT_From_Complete);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle &&
                            GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                XZTMove(GetCurrentTargetFromID(), MotionStepType.From);
                                CycleTimerStart(CycleTime.From_XZT_Move); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.X_Axis) != AxisStep.Idle &&
                               GetAxisStep(AxisList.Z_Axis) != AxisStep.Idle &&
                               GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step113_From_XZT_From_Complete);
                            }
                        }

                        break;

                    // XZT 목적지 도착 확인
                    case AutoStep.Step113_From_XZT_From_Complete:
                        if(GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                        }

                        if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);

                                CycleTimerStopAndSaveData(CycleTime.From_XZT_Move); // Tiemr Stop

                                if (m_main.IsAutoState()) {
                                    SetAutoMotionStep(AutoStep.Step114_From_Shelf_Port_Check);
                                }
                                else {
                                    if (IsManualAutoCylceMode()) {
                                        SetAutoMotionStep(AutoStep.Step114_From_Shelf_Port_Check);
                                    }
                                    else {
                                        CompleteManualRun();
                                    }
                                }
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished &&
                            GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);

                                CycleTimerStopAndSaveData(CycleTime.From_XZT_Move); // Tiemr Stop

                                if (m_main.IsAutoState()) {
                                    SetAutoMotionStep(AutoStep.Step114_From_Shelf_Port_Check);
                                }
                                else {
                                    if (IsManualAutoCylceMode()) {
                                        SetAutoMotionStep(AutoStep.Step114_From_Shelf_Port_Check);
                                    }
                                    else {
                                        CompleteManualRun();
                                    }
                                }
                            }
                        }

                        break;

                    // Port 인지 Shelf인지 구분
                    case AutoStep.Step114_From_Shelf_Port_Check:
                        if (m_teaching.GetPortType(GetCurrentTargetFromID()) == PortType.SHELF) {
                            SetAutoMotionStep(AutoStep.Step123_From_Pick_Sensor_Cehck);
                        }
                        else {
                            if (IsManualAutoCylceMode()) {
                                SetAutoMotionStep(AutoStep.Step0_Idle);
                                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Manual Auto Cycle Port ID Error!"));
                            }

                            if (!m_main.IsAutoState()) {
                                SetAutoMotionStep(AutoStep.Step130_From_Fork_FWD_Move);
                            }
                            else {
                                if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                                    SetAutoMotionStep(AutoStep.Step501_Stop);
                                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                                }
                                else {
                                    m_PIOState = true;
                                    //m_main.ClearCassetteID();
                                    //if (TeachingData.GetPortType(m_fromID) == TeachingData.PortType.OVEN_PORT || TeachingData.GetPortType(m_fromID) == TeachingData.PortType.SORTER_PORT) {
                                    //    if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Unload_Request)) {
                                    //        SetSendBit(SendBitMap.PIO_TR_Request, true);
                                    //        SetAutoMotionStep(AutoStep.Step122_From_Port_Ready_Check);
                                    //    }
                                    //}
                                    //else {
                                    //    SetSendBit(SendBitMap.PIO_TR_Request, true);

                                    //    if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Unload_Request)) {
                                    //        SetSendBit(SendBitMap.PIO_Busy, true);
                                    //        SetAutoMotionStep(AutoStep.Step122_From_Port_Ready_Check);
                                    //    }
                                    //}
                                    if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Unload_Request)) {
                                        SetSendBit(SendBitMap.PIO_TR_Request, true);

                                        if(m_teaching.GetPortType(GetCurrentTargetFromID()) == PortType.OVEN_PORT || m_teaching.GetPortType(GetCurrentTargetFromID()) == PortType.SORTER_PORT) {

                                        }
                                        else {
                                            SetSendBit(SendBitMap.PIO_Busy, true);
                                        }

                                        SetAutoMotionStep(AutoStep.Step122_From_Port_Ready_Check);
                                    }
                                }
                            }
                        }
                        break;

                    // PIO Ready 체크
                    case AutoStep.Step122_From_Port_Ready_Check:
                        if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                            SetAutoMotionStep(AutoStep.Step501_Stop);
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                        }
                        else {
                            if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Ready)) {
                                SetSendBit(SendBitMap.PIO_Busy, true);
                                if (IsPortSensorEnabled(GetCurrentTargetFromID())) {
                                    SetAutoMotionStep(AutoStep.Step123_From_Pick_Sensor_Cehck);
                                }
                                else {
                                    SetAutoMotionStep(AutoStep.Step130_From_Fork_FWD_Move);
                                }
                            }
                        }
                        break;

                    case AutoStep.Step123_From_Pick_Sensor_Cehck:
                        if (!AutoMotionDelay(m_inposCheckTime))
                            break;
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAutoStep, $"{m_autoMotionStep}"));
                        SetAutoMotionStep(AutoStep.Step130_From_Fork_FWD_Move);
                        break;

                    // Fork 전진
                    case AutoStep.Step130_From_Fork_FWD_Move:
                        if (IsManualAutoCylceMode()) {
                            m_manualMotionStep = ManualStep.From_ForkFWD;
                        }
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetFromID(), ForkMoveType.Forward);
                                CycleTimerStart(CycleTime.From_Fork_FWD); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                //if(m_main.GetForkPosition() == ForkPositionState.HomePosition) {
                                //    ForkMove(GetCurrentTargetFromID(), ForkMoveType.Forward);
                                //    break;
                                //}

                                SetAutoMotionStep(AutoStep.Step132_From_Fork_FWD_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetFromID(), ForkMoveType.Forward);
                                CycleTimerStart(CycleTime.From_Fork_FWD); // Timer Start
                            }
                            else {
                                //if (m_main.GetForkPosition() == ForkPositionState.HomePosition) {
                                //    ForkMove(GetCurrentTargetFromID(), ForkMoveType.Forward);
                                //    break;
                                //}

                                SetAutoMotionStep(AutoStep.Step132_From_Fork_FWD_Check);
                            }
                        }
                        break;

                    // Fork 전진 도착 확인
                    case AutoStep.Step132_From_Fork_FWD_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                CycleTimerStopAndSaveData(CycleTime.From_Fork_FWD); // Timer Stop
                                if (m_main.IsAutoState() || IsManualAutoCylceMode()) {
                                    SetAutoMotionStep(AutoStep.Step133_From_Z_Up);
                                }
                                else {
                                    CompleteManualRun();
                                }
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                CycleTimerStopAndSaveData(CycleTime.From_Fork_FWD); // Timer Stop
                                if (m_main.IsAutoState() || IsManualAutoCylceMode()) {
                                    SetAutoMotionStep(AutoStep.Step133_From_Z_Up);
                                }
                                else
                                    CompleteManualRun();
                            }
                        }
                        break;

                    // Z축 상승
                    case AutoStep.Step133_From_Z_Up:
                        if (IsManualAutoCylceMode()) {
                            m_manualMotionStep = ManualStep.From_ZUp;
                        }

                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            ZMove(GetCurrentTargetFromID(), MotionStepType.From);
                            CycleTimerStart(CycleTime.From_Z_Up); // Timer Start
                        }
                        else {
                            if (m_param.GetAxisParameter(AxisList.Z_Axis).autoSpeedPercent <= 30) {
                                SetAutoMotionStep(AutoStep.Step136_From_Z_Inposition_Check);
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step134_From_Z_Up_Override_Slow);
                            }
                        }

                        break;

                    // Center 위치에서 -5mm 일 때 속도 감속
                    case AutoStep.Step134_From_Z_Up_Override_Slow:
                        ZMoveOverride(GetCurrentTargetFromID(), MotionStepType.From, ZOverrideType.Slow);
                        SetAutoMotionStep(AutoStep.Step135_From_Z_Up_Override_Fast);

                        break;

                    // Center 위치에서 +5mm 일 때 속도 다시 상승
                    case AutoStep.Step135_From_Z_Up_Override_Fast:
                        if (!AutoMotionDelay(m_inposCheckTime)) {
                            break;
                        }

                        if (!GetAxisFlag(AxisFlagType.Waiting_Trigger, AxisList.Z_Axis)) {
                            ZMoveOverride(GetCurrentTargetFromID(), MotionStepType.From, ZOverrideType.Fast);
                            SetAutoMotionStep(AutoStep.Step136_From_Z_Inposition_Check);
                        }

                        break;

                    // Z축 도착 확인
                    case AutoStep.Step136_From_Z_Inposition_Check:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                            CycleTimerStopAndSaveData(CycleTime.From_Z_Up); // Timer Stop
                            SetAutoMotionStep(AutoStep.Step137_From_CST_Check_Sensor);
                        }

                        break;

                    // CST 체크
                    case AutoStep.Step137_From_CST_Check_Sensor:
                        if(m_param.GetMotionParam().inPlaceType == InPlaceSensorType.DieBank) {
                            if (m_main.IsAutoState() || IsManualAutoCylceMode())
                                SetAutoMotionStep(AutoStep.Step142_From_Fork_BWD_Move);
                            else
                                CompleteManualRun();
                        }
                        else {
                            if (m_main.IsInPlace_On()) {
                                if (m_main.IsAutoState() || IsManualAutoCylceMode())
                                    SetAutoMotionStep(AutoStep.Step142_From_Fork_BWD_Move);
                                else
                                    CompleteManualRun();
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step138_From_Source_Empty_Z_From_Pos_Move);
                            }
                        }
                        break;

                    // Source Empty일 때 Z축 다시 하강
                    case AutoStep.Step138_From_Source_Empty_Z_From_Pos_Move:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            ZDown(GetCurrentTargetFromID());
                        }
                        else {
                            SetAutoMotionStep(AutoStep.Step139_From_Source_Empty_Z_Inposition_Check);
                        }
                        break;

                    // Z축 하강 확인
                    case AutoStep.Step139_From_Source_Empty_Z_Inposition_Check:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            if (GetAxisFlag(AxisFlagType.Poset, AxisList.Z_Axis)) {
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                SetAutoMotionStep(AutoStep.Step140_From_Source_Empty_Fork_Home_Move);
                            }
                        }

                        break;

                    // Fork 후진
                    case AutoStep.Step140_From_Source_Empty_Fork_Home_Move:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetFromID(), ForkMoveType.Backward);
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step141_From_Source_Empty_Fork_Home_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetFromID(), ForkMoveType.Backward);
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step141_From_Source_Empty_Fork_Home_Check);
                            }
                        }

                        break;

                    // Fork 후진 확인
                    case AutoStep.Step141_From_Source_Empty_Fork_Home_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                if (m_main.IsAutoState())
                                    SetAutoMotionStep(AutoStep.Step810_Source_Empty);
                                else {
                                    ClearFromToID();
                                    SetAutoMotionStep(AutoStep.Step0_Idle);
                                    m_manualMotionStep = ManualStep.SourceEmpty;
                                    if (IsManualAutoCylceMode()) {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Mode Source Empty!"));
                                    }
                                    else {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Source Empty!"));
                                    }
                                }
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                if (m_main.IsAutoState())
                                    SetAutoMotionStep(AutoStep.Step810_Source_Empty);
                                else {
                                    ClearFromToID();
                                    SetAutoMotionStep(AutoStep.Step0_Idle);
                                    m_manualMotionStep = ManualStep.SourceEmpty;
                                    if (IsManualAutoCylceMode()) {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Mode Source Empty!"));
                                    }
                                    else {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Source Empty!"));
                                    }
                                }
                            }
                        }

                        break;

                    // 정상적으로 CST가 올라왔을 때 Fork 후진
                    case AutoStep.Step142_From_Fork_BWD_Move:
                        if (IsManualAutoCylceMode()) {
                            m_manualMotionStep = ManualStep.From_ForkBWD;
                        }

                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetFromID(), ForkMoveType.Backward);
                                CycleTimerStart(CycleTime.From_Fork_BWD); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step143_From_Fork_BWD_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetFromID(), ForkMoveType.Backward);
                                CycleTimerStart(CycleTime.From_Fork_BWD); // Timer Start
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step143_From_Fork_BWD_Check);
                            }
                        }
                        break;

                    // Fork 후진 도착 확인
                    case AutoStep.Step143_From_Fork_BWD_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                CycleTimerStopAndSaveData(CycleTime.From_Fork_BWD); // Timer Stop
                                if (m_PIOState) {
                                    SetAutoMotionStep(AutoStep.Step144_From_Port_Ready_Off_Check);
                                }
                                else {
                                    //SetAutoMotionStep(AutoStep.Step145_From_Complete);
                                    if (GetRackMasterCycleData().autoHomingCount > m_param.GetMotionParam().Z_AutoHomingCount && m_param.GetMotionParam().Z_AutoHomingCount != 0) {
                                        SetAutoMotionStep(AutoStep.Step300_Z_Axis_AutoHomingZMove);
                                    }
                                    else {
                                        SetAutoMotionStep(AutoStep.Step145_From_Complete);
                                    }
                                }
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                CycleTimerStopAndSaveData(CycleTime.From_Fork_BWD); // Timer Stop
                                if (m_PIOState) {
                                    SetAutoMotionStep(AutoStep.Step144_From_Port_Ready_Off_Check);
                                }
                                else {
                                    //SetAutoMotionStep(AutoStep.Step145_From_Complete);
                                    if (GetRackMasterCycleData().autoHomingCount > m_param.GetMotionParam().Z_AutoHomingCount && m_param.GetMotionParam().Z_AutoHomingCount != 0) {
                                        SetAutoMotionStep(AutoStep.Step300_Z_Axis_AutoHomingZMove);
                                    }
                                    else {
                                        SetAutoMotionStep(AutoStep.Step145_From_Complete);
                                    }
                                }
                            }
                        }

                        break;

                    // PIO UL-REQ Off 체크(Ready Off 상태 확인 X)
                    case AutoStep.Step144_From_Port_Ready_Off_Check:
                        if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                            SetAutoMotionStep(AutoStep.Step501_Stop);
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                        }
                        else {
                            if (m_teaching.GetPortType(m_fromID) == PortType.OVEN_PORT || m_teaching.GetPortType(m_fromID) == PortType.SORTER_PORT) {
                                SetSendBit(SendBitMap.PIO_Busy, false);
                            }

                            if (!m_main.GetReceiveBit(ReceiveBitMap.PIO_Unload_Request)) {
                                SetPIOInitial();
                                if (!m_main.GetReceiveBit(ReceiveBitMap.PIO_Ready)) {
                                    if (GetRackMasterCycleData().autoHomingCount > m_param.GetMotionParam().Z_AutoHomingCount && m_param.GetMotionParam().Z_AutoHomingCount != 0) {
                                        SetAutoMotionStep(AutoStep.Step300_Z_Axis_AutoHomingZMove);
                                    }
                                    else {
                                        SetAutoMotionStep(AutoStep.Step145_From_Complete);
                                    }
                                    m_PIOState = false;
                                }
                            }
                            else {
                                SetSendBit(SendBitMap.PIO_Complete, true);
                            }
                        }

                        break;

                    // From Complete
                    case AutoStep.Step145_From_Complete:
                        CycleTimerStopAndSaveData(CycleTime.From_TotalCycle); // Timer Stop

                        // From Complete 신호 보내기
                        if (m_main.IsAutoState()) {
                            if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                                m_data.IncreaseAutoHomingCount();
                                m_data.IncreaseCycleCount();
                                //m_data.SaveRackMasterData();
                                SetAutoMotionStep(AutoStep.Step501_Stop);
                                SetSendBit(SendBitMap.From_Complete, false);
                                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                            }
                            else {
                                SetSendBit(SendBitMap.From_Complete, true);
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_From_Complete_ACK)) {
                                    m_data.IncreaseAutoHomingCount();
                                    m_data.IncreaseCycleCount();
                                    //m_data.SaveRackMasterData();
                                    SetAutoMotionStep(AutoStep.Step0_Idle);
                                    SetAutoMotionFromID(0);
                                    SetSendBit(SendBitMap.Accessing_RM_Down_From, false);
                                }
                            }
                        }
                        else {
                            if (IsManualAutoCylceMode()) {
                                //SetAutoMotionStep(AutoStep.Step0_Idle);
                                //SetAutoMotionFromID(0);
                                //SetAutoMotionToID(0);

                                //m_manualMotionStep = ManualStep.Idle;
                            }
                            else {
                                CompleteManualRun();
                            }
                        }
                        break;


                    // To ID 체크
                    case AutoStep.Step200_To_ID_Check:
                        m_data.SetRackMasterCycleMode(MotionMode.To);
                        CycleTimerStart(CycleTime.To_TotalCycle); // Timer Start
                        SetAutoMotionStep(AutoStep.Step201_To_CST_And_Fork_Home_Check);
                        break;

                    case AutoStep.Step201_To_CST_And_Fork_Home_Check:
                        if (ForkHomeCheck()) {
                            SetAutoMotionStep(AutoStep.Step204_To_XZT_To_Move);
                        }
                        else {
                            SetAutoMotionStep(AutoStep.Step202_To_Fork_Home_Move);
                        }

                        break;

                    // Fork 홈 위치로 이동
                    case AutoStep.Step202_To_Fork_Home_Move:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkHoming();
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step203_To_Fork_Home_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkHoming();
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step203_To_Fork_Home_Check);
                            }
                        }

                        break;

                    // Fork Home 위치 도착 확인
                    case AutoStep.Step203_To_Fork_Home_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                SetAutoMotionStep(AutoStep.Step204_To_XZT_To_Move);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                if (GetAxisFlag(AxisFlagType.Poset, AxisList.A_Axis)) {
                                    SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                    SetAutoMotionStep(AutoStep.Step204_To_XZT_To_Move);
                                }
                            }
                        }

                        break;

                    // XZT 목적지 이동
                    case AutoStep.Step204_To_XZT_To_Move:
                        if (m_teaching.GetPortType(GetCurrentTargetToID()) == PortType.OVEN_PORT) {
                            SetSendBit(SendBitMap.PIO_TR_Request, true);
                        }

                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                                XZTMove(GetCurrentTargetToID(), MotionStepType.To);
                                CycleTimerStart(CycleTime.To_XZT_Move); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.X_Axis) != AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step205_To_XZT_To_Complete);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle &&
                            GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                XZTMove(GetCurrentTargetToID(), MotionStepType.To);
                                CycleTimerStart(CycleTime.To_XZT_Move); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.X_Axis) != AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) != AxisStep.Idle &&
                               GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step205_To_XZT_To_Complete);
                            }
                        }

                        break;

                    case AutoStep.Step205_To_XZT_To_Complete:
                        if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                        }

                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                CycleTimerStopAndSaveData(CycleTime.To_XZT_Move); // Timer Stop
                                if (m_main.IsAutoState()) {
                                    if (m_teaching.GetTeachingData(GetCurrentTargetToID()).portType != (int)PortType.SHELF) {
                                        SetAutoMotionStep(AutoStep.Step207_To_Shelf_Port_Check);
                                    }
                                    else {
                                        SetAutoMotionStep(AutoStep.Step206_To_Double_Storage_Check);
                                    }
                                }
                                else {
                                    if (IsManualAutoCylceMode()) {
                                        SetAutoMotionStep(AutoStep.Step206_To_Double_Storage_Check);
                                    }
                                    else {
                                        CompleteManualRun();
                                    }
                                }
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished &&
                            GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                CycleTimerStopAndSaveData(CycleTime.To_XZT_Move); // Timer Stop
                                if (m_main.IsAutoState()) {
                                    if (m_teaching.GetTeachingData(GetCurrentTargetToID()).portType != (int)PortType.SHELF) {
                                        SetAutoMotionStep(AutoStep.Step207_To_Shelf_Port_Check);
                                    }
                                    else {
                                        SetAutoMotionStep(AutoStep.Step206_To_Double_Storage_Check);
                                    }
                                }
                                else {
                                    if (IsManualAutoCylceMode()) {
                                        SetAutoMotionStep(AutoStep.Step206_To_Double_Storage_Check);
                                    }
                                    else {
                                        CompleteManualRun();
                                    }
                                }
                            }
                        }

                        break;

                    // Double Storage 센서 체크
                    case AutoStep.Step206_To_Double_Storage_Check:
                        if (IsManualAutoCylceMode()) {
                            m_manualMotionStep = ManualStep.To_ForkFWD;
                        }

                        if (!m_autoMotionTimer.Delay(m_param.GetTimerParam().IO_TIMER)) {
                            break;
                        }

                        if (m_main.IsDoubleStorage_Off(GetCurrentTargetToID())) {
                            if (m_main.IsAutoState()) {
                                SetAutoMotionStep(AutoStep.Step820_Double_Storage);
                            }
                        }else if (m_main.IsDoubleStorage_On(GetCurrentTargetToID())) {
                            SetAutoMotionStep(AutoStep.Step209_To_Place_Sensor_Check);
                            m_autoMotionTimer.Restart();
                        }

                        //if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                        //    if(m_teaching.GetTeachingData(GetCurrentTargetToID()).direction == (int)PortDirection_HP.Left) {
                        //        if (m_main.IsDoubleStorage1_Off()) {
                        //            if (m_main.IsAutoState()) {
                        //                SetAutoMotionStep(AutoStep.Step820_Double_Storage);
                        //            }
                        //            else {
                        //                SetAutoMotionStep(AutoStep.Step0_Idle);
                        //                ClearFromToID();
                        //                if (IsManualAutoCylceMode()) {
                        //                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Double Storage"));
                        //                }
                        //                else {
                        //                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Double Storage"));
                        //                }
                        //            }
                        //        }
                        //        else {
                        //            if (m_autoMotionTimer.Delay(m_param.GetTimerParam().IO_TIMER)) {
                        //                SetAutoMotionStep(AutoStep.Step209_To_Place_Sensor_Check);
                        //            }
                        //        }
                        //    }else if(m_teaching.GetTeachingData(GetCurrentTargetToID()).direction == (int)PortDirection_HP.Right) {
                        //        if (m_main.IsDoubleStorage2_Off()) {
                        //            if (m_main.IsAutoState()) {
                        //                SetAutoMotionStep(AutoStep.Step820_Double_Storage);
                        //            }
                        //            else {
                        //                SetAutoMotionStep(AutoStep.Step0_Idle);
                        //                ClearFromToID();
                        //                if (IsManualAutoCylceMode()) {
                        //                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Double Storage"));
                        //                }
                        //                else {
                        //                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Double Storage"));
                        //                }
                        //            }
                        //        }
                        //        else {
                        //            if (m_autoMotionTimer.Delay(m_param.GetTimerParam().IO_TIMER)) {
                        //                SetAutoMotionStep(AutoStep.Step209_To_Place_Sensor_Check);
                        //            }
                        //        }
                        //    }
                        //}
                        //else {
                        //    if (m_main.IsDoubleStorage1_Off()) {
                        //        if (m_main.IsAutoState()) {
                        //            SetAutoMotionStep(AutoStep.Step820_Double_Storage);
                        //        }
                        //        else {
                        //            SetAutoMotionStep(AutoStep.Step0_Idle);
                        //            ClearFromToID();
                        //            if (IsManualAutoCylceMode()) {
                        //                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Auto Cycle Double Storage"));
                        //            }
                        //            else {
                        //                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Manual Double Storage"));
                        //            }
                        //        }
                        //    }
                        //    else {
                        //        if (m_autoMotionTimer.Delay(300)) {
                        //            SetAutoMotionStep(AutoStep.Step209_To_Place_Sensor_Check);
                        //        }
                        //    }
                        //}
                        break;

                    // Shelf인지 Port인지 구분
                    case AutoStep.Step207_To_Shelf_Port_Check:
                        if (m_teaching.GetPortType(GetCurrentTargetToID()) == PortType.SHELF) {
                            SetAutoMotionStep(AutoStep.Step209_To_Place_Sensor_Check);
                        }
                        else {
                            if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                                SetAutoMotionStep(AutoStep.Step501_Stop);
                                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                            }
                            else {
                                m_PIOState = true;
                                //m_main.ClearCassetteID();
                                //if (TeachingData.GetPortType(m_toID) == TeachingData.PortType.OVEN_PORT || TeachingData.GetPortType(m_toID) == TeachingData.PortType.SORTER_PORT) {
                                //    if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Load_Request)) {
                                //        SetSendBit(SendBitMap.PIO_TR_Request, true);
                                //        SetAutoMotionStep(AutoStep.Step206_To_Port_Ready_Check);
                                //    }
                                //}
                                //else {
                                //    SetSendBit(SendBitMap.PIO_TR_Request, true);

                                //    if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Load_Request)) {
                                //        SetSendBit(SendBitMap.PIO_Busy, true);
                                //        SetAutoMotionStep(AutoStep.Step206_To_Port_Ready_Check);
                                //    }
                                //}
                                if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Load_Request)) {
                                    SetSendBit(SendBitMap.PIO_TR_Request, true);
                                    if (m_teaching.GetPortType(GetCurrentTargetToID()) == PortType.OVEN_PORT || m_teaching.GetPortType(GetCurrentTargetToID()) == PortType.SORTER_PORT) {

                                    }
                                    else {
                                        SetSendBit(SendBitMap.PIO_Busy, true);
                                    }

                                    SetAutoMotionStep(AutoStep.Step208_To_Port_Ready_Check);
                                }
                            }
                        }
                        break;

                    // PIO Ready 체크
                    case AutoStep.Step208_To_Port_Ready_Check:
                        if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                            SetAutoMotionStep(AutoStep.Step501_Stop);
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                        }
                        else {
                            if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Ready)) {
                                SetSendBit(SendBitMap.PIO_Busy, true);
                                if (IsPortSensorEnabled(GetCurrentTargetToID())) {
                                    SetAutoMotionStep(AutoStep.Step209_To_Place_Sensor_Check);
                                }
                                else {
                                    SetAutoMotionStep(AutoStep.Step210_To_Fork_FWD_Move);
                                }
                            }
                        }
                        break;

                    case AutoStep.Step209_To_Place_Sensor_Check:
                        SetAutoMotionStep(AutoStep.Step210_To_Fork_FWD_Move);
                        break;

                    // Fork 전진
                    case AutoStep.Step210_To_Fork_FWD_Move:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetToID(), ForkMoveType.Forward);
                                CycleTimerStart(CycleTime.To_Fork_FWD); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                //if (m_main.GetForkPosition() == ForkPositionState.HomePosition) {
                                //    ForkMove(GetCurrentTargetToID(), ForkMoveType.Forward);
                                //    break;
                                //}

                                SetAutoMotionStep(AutoStep.Step211_To_Fork_FWD_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetToID(), ForkMoveType.Forward);
                                CycleTimerStart(CycleTime.To_Fork_FWD); // Timer Start
                            }
                            else {
                                //if (m_main.GetForkPosition() == ForkPositionState.HomePosition) {
                                //    ForkMove(GetCurrentTargetToID(), ForkMoveType.Forward);
                                //    break;
                                //}

                                SetAutoMotionStep(AutoStep.Step211_To_Fork_FWD_Check);
                            }
                        }
                        break;

                    // Fork 전진 도착 확인
                    case AutoStep.Step211_To_Fork_FWD_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                CycleTimerStopAndSaveData(CycleTime.To_Fork_FWD); // Timer Stop
                                if (m_main.IsAutoState() || IsManualAutoCylceMode())
                                    SetAutoMotionStep(AutoStep.Step212_To_Z_Down);
                                else
                                    CompleteManualRun();
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                CycleTimerStopAndSaveData(CycleTime.To_Fork_FWD); // Timer Stop
                                if (m_main.IsAutoState() || IsManualAutoCylceMode())
                                    SetAutoMotionStep(AutoStep.Step212_To_Z_Down);
                                else
                                    CompleteManualRun();
                            }
                        }

                        break;

                    // Z축 하강
                    case AutoStep.Step212_To_Z_Down:
                        if (IsManualAutoCylceMode()) {
                            m_manualMotionStep = ManualStep.To_ZDown;
                        }

                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            ZMove(GetCurrentTargetToID(), MotionStepType.To);
                            CycleTimerStart(CycleTime.To_Z_Down); // Timer Start
                        }
                        else {
                            if(m_param.GetAxisParameter(AxisList.Z_Axis).autoSpeedPercent <= 30) {
                                SetAutoMotionStep(AutoStep.Step215_To_Z_Inposition_Check);
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step213_To_Z_Down_Override_Slow);
                            }
                        }

                        break;

                    // Center 위치부터 +5mm 시점에서 속도 감속
                    case AutoStep.Step213_To_Z_Down_Override_Slow:
                        ZMoveOverride(GetCurrentTargetToID(), MotionStepType.To, ZOverrideType.Slow);
                        SetAutoMotionStep(AutoStep.Step214_To_Z_Down_Override_Fast);
                        break;

                    // Center 위치부터 -5mm 시점에서 속도 가속
                    case AutoStep.Step214_To_Z_Down_Override_Fast:
                        if (!AutoMotionDelay(m_inposCheckTime))
                            break;

                        if (!GetAxisFlag(AxisFlagType.Waiting_Trigger, AxisList.Z_Axis)) {
                            ZMoveOverride(GetCurrentTargetToID(), MotionStepType.To, ZOverrideType.Fast);
                            SetAutoMotionStep(AutoStep.Step215_To_Z_Inposition_Check);
                        }

                        break;

                    // Z축 도착 확인
                    case AutoStep.Step215_To_Z_Inposition_Check:
                        if(m_main.IsInputEnabled(InputList.Fork_In_Place_1) && m_main.IsInputEnabled(InputList.Fork_In_Place_2)) {
                            if(!m_main.GetInputBit(InputList.Fork_In_Place_1) && !m_main.GetInputBit(InputList.Fork_In_Place_2)) {
                                m_main.ClearCassetteID();
                            }
                        }else if (m_main.IsInputEnabled(InputList.Fork_In_Place_1)) {
                            if (!m_main.GetInputBit(InputList.Fork_In_Place_1)) {
                                m_main.ClearCassetteID();
                            }
                        }else if (m_main.IsInputEnabled(InputList.Fork_In_Place_2)) {
                            if (!m_main.GetInputBit(InputList.Fork_In_Place_2)) {
                                m_main.ClearCassetteID();
                            }
                        }

                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                            CycleTimerStopAndSaveData(CycleTime.To_Z_Down); // Timer Stop
                            if (m_main.IsAutoState() || IsManualAutoCylceMode())
                                SetAutoMotionStep(AutoStep.Step216_To_CST_Fork_Placement_Check);
                            else
                                CompleteManualRun();
                        }

                        break;

                    // CST 센서 확인
                    case AutoStep.Step216_To_CST_Fork_Placement_Check:
                        if (IsManualAutoCylceMode()) {
                            m_manualMotionStep = ManualStep.To_ForkBWD;
                        }

                        if(m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Normal) {
                            if (!m_main.IsInPlace_On()) {
                                SetAutoMotionStep(AutoStep.Step217_To_Fork_BWD_Move);
                            }
                        }else if(m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Oven || m_param.GetMotionParam().inPlaceType == InPlaceSensorType.DieBank) {
                            SetAutoMotionStep(AutoStep.Step217_To_Fork_BWD_Move);
                        }
                        m_main.ClearCassetteID();

                        break;

                    // Fork 후진
                    case AutoStep.Step217_To_Fork_BWD_Move:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetToID(), ForkMoveType.Backward);
                                CycleTimerStart(CycleTime.To_Fork_BWD); // Timer Start
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step218_To_Fork_BWD_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkMove(GetCurrentTargetToID(), ForkMoveType.Backward);
                                CycleTimerStart(CycleTime.To_Fork_BWD); // Timer Start
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step218_To_Fork_BWD_Check);
                            }
                        }

                        break;

                    // Fork 후진 도착 확인
                    case AutoStep.Step218_To_Fork_BWD_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                CycleTimerStopAndSaveData(CycleTime.To_Fork_BWD); // Timer Stop
                                if (IsPIOState()) {
                                    SetAutoMotionStep(AutoStep.Step219_To_Port_Ready_Off_Check);
                                }
                                else {
                                    SetAutoMotionStep(AutoStep.Step220_To_Complete);
                                }
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                CycleTimerStopAndSaveData(CycleTime.To_Fork_BWD); // Timer Stop
                                if (IsPIOState()) {
                                    SetAutoMotionStep(AutoStep.Step219_To_Port_Ready_Off_Check);
                                }
                                else {
                                    SetAutoMotionStep(AutoStep.Step220_To_Complete);
                                }
                            }
                        }

                        break;

                    // PIO L-REQ Off 체크(Ready 체크 X)
                    case AutoStep.Step219_To_Port_Ready_Off_Check:
                        if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                            SetAutoMotionStep(AutoStep.Step501_Stop);
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                        }
                        else {
                            if (m_teaching.GetPortType(GetCurrentTargetToID()) == PortType.OVEN_PORT || m_teaching.GetPortType(GetCurrentTargetToID()) == PortType.SORTER_PORT) {
                                SetSendBit(SendBitMap.PIO_Busy, false);
                            }

                            if (!m_main.GetReceiveBit(ReceiveBitMap.PIO_Load_Request)) {
                                if (m_main.CompareCassetteID()) {
                                    SetPIOInitial();
                                    if (!m_main.GetReceiveBit(ReceiveBitMap.PIO_Ready)) {
                                        SetAutoMotionStep(AutoStep.Step220_To_Complete);
                                        m_PIOState = false;
                                    }
                                }
                            }
                            else {
                                m_main.SetCassetteID();
                                SetSendBit(SendBitMap.PIO_Complete, true);
                            }
                        }
                        break;

                    case AutoStep.Step220_To_Complete:
                        CycleTimerStopAndSaveData(CycleTime.To_TotalCycle); // Timer Stop

                        if (m_main.IsAutoState()) {
                            if (m_main.GetReceiveBit(ReceiveBitMap.PIO_Port_Error)) {
                                m_data.IncreaseAutoHomingCount();
                                m_data.IncreaseCycleCount();
                                //m_data.SaveRackMasterData();
                                SetAutoMotionStep(AutoStep.Step501_Stop);
                                SetSendBit(SendBitMap.To_Complete, false);
                                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Error!"));
                            }
                            else {
                                SetSendBit(SendBitMap.To_Complete, true);
                                if (m_main.GetReceiveBit(ReceiveBitMap.RM_To_Complete_ACK)) {
                                    m_data.IncreaseAutoHomingCount();
                                    m_data.IncreaseCycleCount();
                                    //m_data.SaveRackMasterData();
                                    SetAutoMotionStep(AutoStep.Step0_Idle);
                                    SetAutoMotionToID(0);
                                    SetSendBit(SendBitMap.Accessing_RM_Down_To, false);
                                }
                            }
                        }
                        else {
                            if (!m_main.IsAutoState()) {
                                if (IsManualAutoCylceMode()) {
                                    //SetAutoMotionStep(AutoStep.Step0_Idle);
                                    //SetAutoMotionFromID(0);
                                    //SetAutoMotionToID(0);

                                    //m_manualMotionStep = ManualStep.Idle;
                                }
                                else {
                                    CompleteManualRun();
                                }
                                break;
                            }
                        }

                        break;

                    // Z Axis Auto Homing
                    case AutoStep.Step300_Z_Axis_AutoHomingZMove:
                        ZAxisAutoHomingMove();
                        SetAutoMotionStep(AutoStep.Step301_Z_Axis_AutoHomingZMoveCheck);
                        break;

                    case AutoStep.Step301_Z_Axis_AutoHomingZMoveCheck:
                        if(GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                            SetAutoMotionStep(AutoStep.Step302_Z_Axis_AutoHomingStart);
                        }

                        break;

                    case AutoStep.Step302_Z_Axis_AutoHomingStart:
                        if(GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            if (ZAxisAutoHoming()) {
                                SetAutoMotionStep(AutoStep.Step303_Z_Axis_AutoHomingCheck);
                            }
                        }
                        break;

                    case AutoStep.Step303_Z_Axis_AutoHomingCheck:
                        if(GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                            SetAutoMotionStep(AutoStep.Step304_Z_Axis_AutoHomingComplete);
                        }
                        break;

                    case AutoStep.Step304_Z_Axis_AutoHomingComplete:
                        m_data.ClearAutoHomingCount();
                        //m_data.SaveRackMasterData();
                        SetAutoMotionStep(AutoStep.Step145_From_Complete);
                        break;


                    case AutoStep.Step400_Maint_Move_Fork_Home_Check:
                        if (ForkHomeCheck()) {
                            SetAutoMotionStep(AutoStep.Step403_Maint_Move);
                        }
                        else {
                            SetAutoMotionStep(AutoStep.Step401_Maint_Move_Fork_Home_Move);
                        }
                        break;

                    case AutoStep.Step401_Maint_Move_Fork_Home_Move:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkHoming();
                            }
                            else if (GetAxisStep(AxisList.A_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step402_Maint_Move_Fork_Home_Move_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkHoming();
                            }
                            else {
                                SetAutoMotionStep(AutoStep.Step402_Maint_Move_Fork_Home_Move_Check);
                            }
                        }

                        break;

                    case AutoStep.Step402_Maint_Move_Fork_Home_Move_Check:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                SetAutoMotionStep(AutoStep.Step403_Maint_Move);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                if (GetAxisFlag(AxisFlagType.Poset, AxisList.A_Axis)) {
                                    SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                    SetAutoMotionStep(AutoStep.Step403_Maint_Move);
                                }
                            }
                        }

                        break;

                    case AutoStep.Step403_Maint_Move:
                        if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            if(GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                                MaintMove();
                            }else if(GetAxisStep(AxisList.X_Axis) != AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step404_Maint_Move_Check);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                MaintMove();
                            }else if(GetAxisStep(AxisList.X_Axis) != AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) != AxisStep.Idle && GetAxisStep(AxisList.T_Axis) != AxisStep.Idle) {
                                SetAutoMotionStep(AutoStep.Step404_Maint_Move_Check);
                            }
                        }
                        break;

                    case AutoStep.Step404_Maint_Move_Check:
                        if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            if(GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                SetAutoMotionStep(AutoStep.Step405_Maint_Complete);
                            }
                        }
                        else {
                            if(GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                SetAutoMotionStep(AutoStep.Step405_Maint_Complete);
                            }
                        }
                        break;

                    case AutoStep.Step405_Maint_Complete:
                        SetSendBit(SendBitMap.Maint_Move_Complete, true);

                        if (m_main.GetReceiveBit(ReceiveBitMap.RM_Maint_Complete_ACK)) {
                            SetSendBit(SendBitMap.Maint_Move_Complete, false);
                            SetAutoMotionStep(AutoStep.Step0_Idle);
                        }
                        break;


                    case AutoStep.Step500_Error:
                        SetPIOInitial();
                        ClearFromToID();
                        break;

                    case AutoStep.Step501_Stop:
                        AllStop();
                        if (m_PIOState) {
                            SetPIOInitial();
                        }
                        SetAutoMotionStep(AutoStep.Step0_Idle);

                        break;

                    case AutoStep.Step800_Store_Alt:
                        SetSendBit(SendBitMap.Store_Alt_Request, true);

                        if (m_main.GetReceiveBit(ReceiveBitMap.RM_Store_Alt_ACK)) {
                            SetSendBit(SendBitMap.Store_Alt_Request, false);
                            SetAutoMotionStep(AutoStep.Step0_Idle);
                        }

                        break;

                    case AutoStep.Step801_Resume_Request:
                        SetSendBit(SendBitMap.Resume_Request_Request, true);

                        if (m_main.GetReceiveBit(ReceiveBitMap.RM_Resume_Request_ACK)) {
                            SetSendBit(SendBitMap.Resume_Request_Request, false);
                            SetAutoMotionStep(AutoStep.Step0_Idle);
                        }

                        break;

                    case AutoStep.Step810_Source_Empty:
                        SetSendBit(SendBitMap.Source_Empty_Request, true);

                        if (m_main.GetReceiveBit(ReceiveBitMap.RM_Source_Empty_ACK)) {
                            SetSendBit(SendBitMap.Source_Empty_Request, false);
                            SetAutoMotionStep(AutoStep.Step0_Idle);
                        }

                        break;

                    case AutoStep.Step820_Double_Storage:
                        SetSendBit(SendBitMap.Double_Storage_Request, true);

                        if (m_main.GetReceiveBit(ReceiveBitMap.RM_Double_Storage_ACK)) {
                            SetSendBit(SendBitMap.Double_Storage_Request, false);
                            SetAutoMotionStep(AutoStep.Step0_Idle);
                        }

                        break;
                }
            }
        }
    }
}
