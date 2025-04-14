using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using MovenCore;

namespace RackMaster.SEQ.PART {
    public enum AutoTeachingStep {
        Step0_Idle = 0,
        Step1_Init,                                     // Initialize
        Step2_ForkHomePositionCheck,                    // Fork 축이 Home 위치인지 확인
        Step3_ForkHoming,                               // Fork 축이 Home 위치가 아닌 경우 Home 위치로 이동
        Step4_ForkHomingInpositionCheck,                // Fork 축 Homing 완료 체크
        Step5_XZTMove,                                  // 지령 받은 Target 위치로 X,Z,T 이동
        Step6_XZTMoveInpositionCheck,                   // X,Z,T 도착 확인
        Step7_SensorCheck,                              // Pick or Place Sensor가 On 인지 확인
        Step8_XNegativeMove,                            // X축 Negative 방향으로 이동
        Step9_XNegativeMoveSensorOffCheck,              // Sensor Off 된 경우 X 축 정지
        Step10_XPositiveMove,                           // X축 Positive 방향으로 이동
        Step11_XPositiveMoveSensorOnCheck,              // Sensor On 되었는 지 확인(이유는 Sensor Off에서 X축이 Positive 방향으로 이동하기 때문)
        Step12_XPositiveMoveSensorOffCheck,             // Sensor Off 된 경우 X 축 정지
        Step13_XCenterDataSetting,                      // Negative, Positive 방향의 Sensor Off 위치를 기반으로 Center 위치 저장
        Step14_XLengthCheck,                            // Sensor 길이 체크
        Step15_XCenterMove,                             // X축 저장된 Center 위치로 이동
        Step16_XCenterMoveInpositionCheck,              // X축 Center 위치 도착 확인
        Step17_ZNegativeMove,                           // Z축 Negative 방향으로 이동
        Step18_ZNegativeMoveSensorOffCheck,             // Sensor Off 된 경우 Z 축 정지
        Step19_ZPositiveMove,                           // Z축 Positive 방향으로 이동
        Step20_ZPositiveMoveSensorOnCheck,              // Sensor On 되었는지 확인(이유는 Sensor Off에서 Z축이 Positive 방향으로 이동하기 때문)
        Step21_ZPositiveMoveSensorOffCheck,             // Sensor Off 된 경우 Z 축 정지
        Step22_ZCenterDataSetting,                      // Negative, Positive 방향의 Sensor Off 위치를 기반으로 Center 위치 저장
        Step23_ZLengthCheck,                            // Sensor 길이 체크
        Step24_CenterMove,                              // Z축 저장된 Center 위치로 이동
        Step25_CenterMoveInpositionCheckAndDataSave,    // Z축 Center 위치 도착 확인 및 티칭 데이터 저장
        Step26_AutoTeachingComplete,                    // Auto Teaching Complete

        Step400_AutoTeachingFailed = 400,

        Step401_EMO,
        Step402_Stop,
    }

    public enum SensorFindStep {
        Step0_Idle = 0,
        Step1_XNegativeMove,                // X축 Find Sensor Range X 값 만큼 Negative 방향으로 이동
        Step2_XNegativeMoveSensorCheck,     // X축 도착확인(Sensor 검출된 경우 Step100으로 이동)
        Step3_ZPositiveMove,                // Z축 Find Sensor Range Z 값 만큼 Positive 방향으로 이동
        Step4_ZPositiveMoveSensorCheck,     // Z축 도착확인(Sensor 검출된 경우 Step100으로 이동)
        Step5_XPositiveMove,                // X축 Find Sensor Range X * 2 값 만큼 Positive 방향으로 이동
        Step6_XPositiveMoveSensorCheck,     // X축 도착확인(Sensor 검출된 경우 Step100으로 이동)
        Step7_ZNegativeMove,                // Z축 Find Sensor Range Z * 2 값 만큼 Negative 방향으로 이동
        Step8_ZNegativeMoveSensorCheck,     // Z축 도착확인(Sensor 검출된 경우 Step100으로 이동)
        Step9_XNegativeMove,                // X축 Find Sensor Range X * 2 값 만큼 Negative 방향으로 이동
        Step10_XNegativeMoveSensorCheck,    // X축 도착확인(Sensor 검출된 경우 Step100으로 이동)

        Step100_SensorCheckComplete,        // Sensor 검출된 경우
        Step400_SensorCheckFailed,          // Sensor 검출에 실패한 경우
    }

    public enum AutoTeachingAlarm
    {
        ShelfValueNull,
        SensorCheckFailed,
        XMoveSensorFailed,
        ZMoveSensorFailed,
        ReflectorLengthFailed,
        EMO,
        DoubleStorageSensorFailed,
        StepTimeOver,
    }

    public enum AutoTeachingAxis {
        Axis_X = 0,
        Axis_Z,
    }

    public enum AutoTeachingTargetData {
        Target_X = 0,
        Target_Z
    }

    public enum AutoTeachingMoveDirection {
        Positive = 0,
        Negative,
        Center,
    }

    public enum AutoTeachingReflectorRange {
        Width = 0,
        Height,
    }

    public partial class RackMasterMain {
        public partial class RackMasterMotion {
            public class AutoTeachingData {
                public float x_negativePos;
                public float x_positivePos;
                public float z_negativePos;
                public float z_positivePos;

                public float x_centerPos;
                public float z_centerPos;

                public float width;
                public float height;

                public int id;

                public AutoTeachingData() {
                    x_negativePos = 0;
                    x_positivePos = 0;
                    z_negativePos = 0;
                    z_positivePos = 0;
                    x_centerPos = 0;
                    z_centerPos = 0;
                    width = 0;
                    height = 0;
                    id = 0;
                }

                public void ClearData() {
                    x_negativePos = 0;
                    x_positivePos = 0;
                    z_negativePos = 0;
                    z_positivePos = 0;
                    x_centerPos = 0;
                    z_centerPos = 0;
                    id = 0;
                }
            }

            private class TargetData {
                public int id;
                public double targetX;
                public double targetZ;

                public TargetData() {
                    id = 0;
                    targetX = 0;
                    targetZ = 0;
                }

                public void ClearData() {
                    id = 0;
                    targetX = 0;
                    targetZ = 0;
                }
            }

            private AutoTeachingData m_autoTeachingSensorData;
            private TargetData m_autoTeachingTarget;

            private SEQ.CLS.Timer m_autoTeachingStepTimer;

            private AutoTeachingStep m_autoTeachingStep;
            private AutoTeachingStep m_logPrevTeachingStep;
            private AutoTeachingStep m_autoTeachingPrevStep;
            private CLS.Timer m_autoTeachingTimer;

            private SensorFindStep m_sensorCheckStep;

            private bool m_isManualAutoTeachingRun;
            /// <summary>
            /// Auto Teaching을 진행할 타겟 ID 설정
            /// </summary>
            /// <param name="id"></param>
            public void SetAutoTeachingTargetID(int id) {
                m_autoTeachingTarget.id = id;
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Set Auto Teaching ID={id}"));
            }
            /// <summary>
            /// Auto Teaching을 진행할 때의 타겟 X 위치 설정
            /// </summary>
            /// <param name="x"></param>
            public void SetAutoTeachingTargetX(double x) {
                m_autoTeachingTarget.targetX = x;
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Set Auto Teaching X Data={x}"));
            }
            /// <summary>
            /// Auto Teaching을 진행할 때의 타겟 Z 위치 설정
            /// </summary>
            /// <param name="z"></param>
            public void SetAutoTeachingTargetZ(double z) {
                m_autoTeachingTarget.targetZ = z;
                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Set Auto Teaching Z Data={z}"));
            }
            /// <summary>
            /// Auto Teaching 시작
            /// </summary>
            /// <returns></returns>
            public bool AutoTeachingStart() {
                if (IsAutoMotionRun() || IsAutoTeachingRun())
                    return false;

                SetAutoTeachingStep(AutoTeachingStep.Step1_Init);
                return true;
            }
            /// <summary>
            /// Master(or CIM)에서 받은 지령이 아닌 RackMaster에서 할 수 있는 단일 오토티칭 시작
            /// </summary>
            /// <param name="id"></param>
            /// <param name="targetX"></param>
            /// <param name="targetZ"></param>
            /// <returns></returns>
            public bool AutoTeachingManualStart(int id, double targetX, double targetZ) {
                if (IsAutoMotionRun() || IsAutoTeachingRun())
                    return false;

                SetAutoTeachingTargetID(id);
                SetAutoTeachingTargetX(targetX);
                SetAutoTeachingTargetZ(targetZ);

                SetAutoTeachingStep(AutoTeachingStep.Step1_Init);

                m_isManualAutoTeachingRun = true;

                return true;
            }
            /// <summary>
            /// RackMaster에서 진행된 단일 오토티칭 정지
            /// </summary>
            public void AutoTeachingManualStop() {
                m_isManualAutoTeachingRun = false;
            }
            /// <summary>
            /// 현재의 Auto Teaching Step 설정
            /// </summary>
            /// <param name="step"></param>
            private void SetAutoTeachingStep(AutoTeachingStep step) {
                m_autoTeachingTimer.Restart();
                m_autoTeachingStep = step;
            }
            /// <summary>
            /// 현재 Auto Teaching이 진행 중인지
            /// </summary>
            /// <returns></returns>
            public bool IsAutoTeachingRun() {
                return (GetCurrentAutoTeachingStep() != AutoTeachingStep.Step0_Idle);
            }
            /// <summary>
            /// 현재 RackMaster에서 진행하는 단일 오토티칭 중인지
            /// </summary>
            /// <returns></returns>
            public bool IsManualAutoTeachingRun() {
                return m_isManualAutoTeachingRun;
            }
            /// <summary>
            /// 현재 설정된 Auto Teaching Target ID 반환
            /// </summary>
            /// <returns></returns>
            public int GetAutoTeachingTargetID() {
                return m_autoTeachingTarget.id;
            }
            /// <summary>
            /// 현재 설정된 Auto Teaching Target X 값 반환
            /// </summary>
            /// <returns></returns>
            public double GetAutoTeachingTargetX() {
                return m_autoTeachingTarget.targetX;
            }
            /// <summary>
            /// 현재 설정된 Auto Teaching Target Z 값 반환
            /// </summary>
            /// <returns></returns>
            public double GetAutoTeachingTargetZ() {
                return m_autoTeachingTarget.targetZ;
            }
            /// <summary>
            /// Auto Teaching을 시작할 때 From/To(Pick/Place) Sensor 중 설정된 Sensor가 어떤 것인지 반환
            /// </summary>
            /// <returns></returns>
            public AutoTeachingSensorType GetAutoTeachingSensorType() {
                return m_param.GetAutoTeachingParam().sensorType;
            }
            /// <summary>
            /// Auto Teaching에 필요한 Timer Delay
            /// </summary>
            /// <param name="milliseconds"></param>
            /// <returns></returns>
            private bool AutoTeachingDelay(long milliseconds) {
                return m_autoTeachingTimer.Delay(milliseconds);
            }
            /// <summary>
            /// Auto Teaching할 때 실패할 경우 로그 생성
            /// </summary>
            /// <param name="alarm"></param>
            /// <param name="currentStep"></param>
            private void AutoTeachingAlarmLog(AutoTeachingAlarm alarm, AutoTeachingStep currentStep) {
                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterAutoTeachingAlarmOccurred, $"{alarm}, Step={currentStep}"));
            }
            /// <summary>
            /// Auto Teaching 시작
            /// </summary>
            public void AutoTeachingRun() {
                UpdateAxisStep();

                if (GetCurrentAutoTeachingStep() == AutoTeachingStep.Step0_Idle)
                    return;

                if (m_main.IsAutoState()) {
                    if (m_main.GetReceiveBit(ReceiveBitMap.RM_Auto_Teaching_Stop_Request)) {
                        AutoTeachingAxisStop();
                        SetAutoTeachingStep(AutoTeachingStep.Step402_Stop);
                    }

                    if (m_main.GetReceiveBit(ReceiveBitMap.MST_Soft_Error_State)) {
                        EmergencyStop();
                        SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                    }

                    if (m_main.m_alarm.IsAlarmState()) {
                        EmergencyStop();
                        SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                    }
                }
                else {
                    if (IsAutoTeachingRun()) {
                        if (m_main.m_alarm.IsAlarmState() && !m_main.Interlock_OnlyMSTDisconnectedAlarmOccurred()) {
                            EmergencyStop();
                            SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                        }
                    }
                    
                    if(!m_isManualAutoTeachingRun && IsAutoTeachingRun()) {
                        SetAutoTeachingStep(AutoTeachingStep.Step402_Stop);
                    }
                }

                if(m_logPrevTeachingStep != GetCurrentAutoTeachingStep()) {
                    m_logPrevTeachingStep = GetCurrentAutoTeachingStep();
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Auto Teaching Step={m_logPrevTeachingStep}"));
                }

                if(m_autoTeachingPrevStep == GetCurrentAutoTeachingStep() && GetCurrentAutoTeachingStep() != AutoTeachingStep.Step0_Idle) {
                    if (!m_autoTeachingStepTimer.IsTimerStarted()) {
                        m_autoTeachingStepTimer.Reset();
                        m_autoTeachingStepTimer.Start();
                    }

                    if (m_autoTeachingStepTimer.Delay(m_param.GetTimerParam().AUTO_TEACHING_STEP_TIMEOVER)) {
                        if(GetCurrentAutoTeachingStep() != AutoTeachingStep.Step400_AutoTeachingFailed) {
                            AutoTeachingAlarmLog(AutoTeachingAlarm.StepTimeOver, GetCurrentAutoTeachingStep());
                            SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                            m_autoTeachingStepTimer.Stop();
                        }
                    }
                }
                else {
                    if(m_autoTeachingPrevStep != GetCurrentAutoTeachingStep()) {
                        m_autoTeachingPrevStep = GetCurrentAutoTeachingStep();
                    }

                    m_autoTeachingStepTimer.Stop();
                }

                TeachingValueData port = m_teaching.GetTeachingData(m_autoTeachingTarget.id);
                if(port == null) {
                    if(GetCurrentAutoTeachingStep() != AutoTeachingStep.Step400_AutoTeachingFailed) {
                        Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Port Data is Null, ID={m_autoTeachingTarget.id}"));
                        SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                    }
                }

                switch (m_autoTeachingStep) {
                    case AutoTeachingStep.Step1_Init:
                        try {
                            if (m_main.IsAutoState()) {
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_Start_ACK, true);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_State, true);
                            }

                            if (!AutoTeachingIDCheck(m_autoTeachingTarget.id)) {
                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                            }
                            
                            m_autoTeachingTarget.targetX *= 1000;
                            m_autoTeachingTarget.targetZ *= 1000;

                            if (m_param.GetAutoTeachingParam().sensorType == AutoTeachingSensorType.Pick) {
                                m_autoTeachingTarget.targetZ -= (float)m_teaching.GetTeachingData(m_autoTeachingTarget.id).valZDown;
                            }
                            else {
                                m_autoTeachingTarget.targetZ += (float)m_teaching.GetTeachingData(m_autoTeachingTarget.id).valZUp;
                            }

                            m_autoTeachingSensorData.id = m_autoTeachingTarget.id;
                            m_sensorCheckStep = SensorFindStep.Step0_Idle;

                            SetAutoTeachingStep(AutoTeachingStep.Step2_ForkHomePositionCheck);
                        }catch(Exception ex) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "Auto Teaching Init Fail", ex));
                            SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                        }

                        break;

                    case AutoTeachingStep.Step2_ForkHomePositionCheck:
                        if (ForkHomeCheck()) {
                            SetAutoTeachingStep(AutoTeachingStep.Step5_XZTMove);
                        }
                        else {
                            SetAutoTeachingStep(AutoTeachingStep.Step3_ForkHoming);
                        }

                        break;

                    case AutoTeachingStep.Step3_ForkHoming:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle && GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                                ForkHoming();
                                SetAutoTeachingStep(AutoTeachingStep.Step4_ForkHomingInpositionCheck);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Idle) {
                                ForkHoming();
                                SetAutoTeachingStep(AutoTeachingStep.Step4_ForkHomingInpositionCheck);
                            }
                        }

                        break;

                    case AutoTeachingStep.Step4_ForkHomingInpositionCheck:
                        if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished && GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                SetAutoTeachingStep(AutoTeachingStep.Step5_XZTMove);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.A_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.A_Axis);
                                SetAutoTeachingStep(AutoTeachingStep.Step5_XZTMove);
                            }
                        }
                        break;

                    case AutoTeachingStep.Step5_XZTMove:
                        if (port == null) {
                            AutoTeachingAlarmLog(AutoTeachingAlarm.ShelfValueNull, GetCurrentAutoTeachingStep());
                            SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                            break;
                        }

                        if(m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                            if (port.valT == null) {
                                AutoTeachingAlarmLog(AutoTeachingAlarm.ShelfValueNull, GetCurrentAutoTeachingStep());
                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                                break;
                            }
                        }

                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle &&
                            GetAxisStep(AxisList.T_Axis) == AxisStep.Idle) {
                            if(m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                AutoTeachingTurnMove();
                            }
                            AutoTeachingXZMove();
                            SetAutoTeachingStep(AutoTeachingStep.Step6_XZTMoveInpositionCheck);
                        }

                        break;

                    case AutoTeachingStep.Step6_XZTMoveInpositionCheck:
                        if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                SetAutoTeachingStep(AutoTeachingStep.Step7_SensorCheck);
                            }
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished &&
                            GetAxisStep(AxisList.T_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                SetAxisStep(AxisStep.Idle, AxisList.T_Axis);
                                SetAutoTeachingStep(AutoTeachingStep.Step7_SensorCheck);
                            }
                        }
                        break;

                    case AutoTeachingStep.Step7_SensorCheck:
                        if (AutoTeachingSensorCheck() && m_sensorCheckStep == SensorFindStep.Step0_Idle) {
                            SetAutoTeachingStep(AutoTeachingStep.Step8_XNegativeMove);
                        }
                        else {
                            if(m_sensorCheckStep == SensorFindStep.Step0_Idle)
                            {
                                m_sensorCheckStep = SensorFindStep.Step1_XNegativeMove;
                            }

                            AutoTeachingSensorCheckCycle();

                            if (m_main.GetReceiveBit(ReceiveBitMap.RM_Auto_Teaching_Stop_Request))
                            {
                                break;
                            }

                            if (GetAxisFlag(AxisFlagType.Poset, AxisList.X_Axis) && GetAxisFlag(AxisFlagType.Poset, AxisList.Z_Axis))
                            {
                                if (m_sensorCheckStep == SensorFindStep.Step100_SensorCheckComplete)
                                {
                                    SetAutoTeachingTargetX(GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis));
                                    if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                        SetAutoTeachingTargetZ(GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis));
                                    }
                                    else {
                                        double tempZ = RadianToCalculateDistance(GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis));
                                        SetAutoTeachingTargetZ(tempZ);
                                    }
                                    SetAutoTeachingStep(AutoTeachingStep.Step8_XNegativeMove);
                                    m_sensorCheckStep = SensorFindStep.Step0_Idle;
                                    Thread.Sleep(m_inposCheckTime);
                                    break;
                                }
                                else if (m_sensorCheckStep == SensorFindStep.Step400_SensorCheckFailed)
                                {
                                    AutoTeachingAlarmLog(AutoTeachingAlarm.SensorCheckFailed, GetCurrentAutoTeachingStep());
                                    SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                                    m_sensorCheckStep = SensorFindStep.Step0_Idle;
                                    Thread.Sleep(m_inposCheckTime);
                                    break;
                                }
                            }
                        }

                        break;

                    case AutoTeachingStep.Step8_XNegativeMove:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle) {
                            AutoTeachingMove(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Negative);
                            SetAutoTeachingStep(AutoTeachingStep.Step9_XNegativeMoveSensorOffCheck);
                        }
                        break;

                    case AutoTeachingStep.Step9_XNegativeMoveSensorOffCheck:
                        if (AutoTeachingGetData(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Negative)) {
                            AutoTeachingAxisStop();
                            SetAutoTeachingStep(AutoTeachingStep.Step10_XPositiveMove);
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                AutoTeachingAlarmLog(AutoTeachingAlarm.XMoveSensorFailed, GetCurrentAutoTeachingStep());
                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                            }
                        }
                        break;

                    case AutoTeachingStep.Step10_XPositiveMove:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle) {
                            AutoTeachingMove(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Positive, true);
                            SetAutoTeachingStep(AutoTeachingStep.Step11_XPositiveMoveSensorOnCheck);
                        }else if(GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                        }
                        break;

                    case AutoTeachingStep.Step11_XPositiveMoveSensorOnCheck:
                        if (AutoTeachingSensorCheck()) {
                            SetAutoTeachingStep(AutoTeachingStep.Step12_XPositiveMoveSensorOffCheck);
                        }
                        break;

                    case AutoTeachingStep.Step12_XPositiveMoveSensorOffCheck:
                        if (AutoTeachingGetData(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Positive)) {
                            AutoTeachingAxisStop();
                            SetAutoTeachingStep(AutoTeachingStep.Step13_XCenterDataSetting);
                        }
                        else {
                            if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                                AutoTeachingAlarmLog(AutoTeachingAlarm.XMoveSensorFailed, GetCurrentAutoTeachingStep());
                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                            }
                        }

                        break;

                    case AutoTeachingStep.Step13_XCenterDataSetting:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle) {
                            SetAutoTeachingCenterData(AutoTeachingAxis.Axis_X);
                            SetAutoTeachingStep(AutoTeachingStep.Step14_XLengthCheck);
                        }else if(GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                        }
                        break;


                    case AutoTeachingStep.Step14_XLengthCheck:
                        if (AutoTeachingLengthCheck(AutoTeachingReflectorRange.Width)) {
                            SetAutoTeachingStep(AutoTeachingStep.Step15_XCenterMove);
                        }
                        else {
                            AutoTeachingAlarmLog(AutoTeachingAlarm.ReflectorLengthFailed, GetCurrentAutoTeachingStep());
                            SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                        }
                        break;

                    case AutoTeachingStep.Step15_XCenterMove:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle) {
                            AutoTeachingMove(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Center);
                            SetAutoTeachingStep(AutoTeachingStep.Step16_XCenterMoveInpositionCheck);
                        }else if(GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                        }

                        break;

                    case AutoTeachingStep.Step16_XCenterMoveInpositionCheck:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                            SetAutoTeachingStep(AutoTeachingStep.Step17_ZNegativeMove);
                        }

                        break;

                    case AutoTeachingStep.Step17_ZNegativeMove:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            AutoTeachingMove(AutoTeachingAxis.Axis_Z, AutoTeachingMoveDirection.Negative);
                            SetAutoTeachingStep(AutoTeachingStep.Step18_ZNegativeMoveSensorOffCheck);
                        }else if(GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                        }

                        break;

                    case AutoTeachingStep.Step18_ZNegativeMoveSensorOffCheck:
                        if (AutoTeachingGetData(AutoTeachingAxis.Axis_Z, AutoTeachingMoveDirection.Negative)) {
                            AutoTeachingAxisStop();
                            SetAutoTeachingStep(AutoTeachingStep.Step19_ZPositiveMove);
                        }
                        else {
                            if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                AutoTeachingAlarmLog(AutoTeachingAlarm.ZMoveSensorFailed, GetCurrentAutoTeachingStep());
                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                            }
                        }

                        break;

                    case AutoTeachingStep.Step19_ZPositiveMove:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            AutoTeachingMove(AutoTeachingAxis.Axis_Z, AutoTeachingMoveDirection.Positive, true);
                            SetAutoTeachingStep(AutoTeachingStep.Step20_ZPositiveMoveSensorOnCheck);
                        }else if(GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                        }
                        break;

                    case AutoTeachingStep.Step20_ZPositiveMoveSensorOnCheck:
                        if (AutoTeachingSensorCheck()) {
                            SetAutoTeachingStep(AutoTeachingStep.Step21_ZPositiveMoveSensorOffCheck);
                        }

                        break;

                    case AutoTeachingStep.Step21_ZPositiveMoveSensorOffCheck:
                        if (AutoTeachingGetData(AutoTeachingAxis.Axis_Z, AutoTeachingMoveDirection.Positive)) {
                            AutoTeachingAxisStop();
                            SetAutoTeachingStep(AutoTeachingStep.Step22_ZCenterDataSetting);
                        }
                        else {
                            if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                                SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                                AutoTeachingAlarmLog(AutoTeachingAlarm.ZMoveSensorFailed, GetCurrentAutoTeachingStep());
                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                            }
                        }

                        break;

                    case AutoTeachingStep.Step22_ZCenterDataSetting:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            SetAutoTeachingCenterData(AutoTeachingAxis.Axis_Z);
                            SetAutoTeachingStep(AutoTeachingStep.Step23_ZLengthCheck);
                        }else if(GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                        }
                        break;

                    case AutoTeachingStep.Step23_ZLengthCheck:
                        SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                        if (AutoTeachingLengthCheck(AutoTeachingReflectorRange.Height)) {
                            SetAutoTeachingStep(AutoTeachingStep.Step24_CenterMove);
                        }
                        else {
                            AutoTeachingAlarmLog(AutoTeachingAlarm.ReflectorLengthFailed, GetCurrentAutoTeachingStep());
                            SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                        }
                        break;

                    case AutoTeachingStep.Step24_CenterMove:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle && GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            AutoTeachingMove(AutoTeachingAxis.Axis_Z, AutoTeachingMoveDirection.Center);
                            AutoTeachingMove(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Center);
                            SetAutoTeachingStep(AutoTeachingStep.Step25_CenterMoveInpositionCheckAndDataSave);
                        }

                        break;

                    case AutoTeachingStep.Step25_CenterMoveInpositionCheckAndDataSave:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished && GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                            float teachingDataZ = (float)m_autoTeachingSensorData.z_centerPos;

                            if (port.valZDown == null || port.valZUp == null) {
                                AutoTeachingAlarmLog(AutoTeachingAlarm.ShelfValueNull, GetCurrentAutoTeachingStep());
                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                                break;
                            }

                            if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Pick) {
                                if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                    teachingDataZ += (float)port.valZDown;
                                }
                                else {
                                    teachingDataZ += (float)DistanceToRadian(GetCurrentZAxisDia(), (double)port.valZDown);
                                }
                            }
                            else if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Place) {
                                if (m_param.GetAutoTeachingParam().doubleStorageSensorCheck) {
                                    if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                                        if (m_teaching.GetTeachingData(m_autoTeachingTarget.id).direction == (int)PortDirection_HP.Left) {
                                            if (!m_main.IsDoubleStorage1_On()) {
                                                AutoTeachingAlarmLog(AutoTeachingAlarm.DoubleStorageSensorFailed, GetCurrentAutoTeachingStep());
                                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                                                break;
                                            }
                                        }else if(m_teaching.GetTeachingData(m_autoTeachingTarget.id).direction == (int)PortDirection_HP.Right) {
                                            if (!m_main.IsDoubleStorage2_On()) {
                                                AutoTeachingAlarmLog(AutoTeachingAlarm.DoubleStorageSensorFailed, GetCurrentAutoTeachingStep());
                                                SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                                                break;
                                            }
                                        }
                                    }
                                    else {
                                        if (!m_main.IsDoubleStorage1_On()) {
                                            AutoTeachingAlarmLog(AutoTeachingAlarm.DoubleStorageSensorFailed, GetCurrentAutoTeachingStep());
                                            SetAutoTeachingStep(AutoTeachingStep.Step400_AutoTeachingFailed);
                                            break;
                                        }
                                    }
                                }

                                if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                    teachingDataZ -= (float)port.valZUp;
                                }
                                else {
                                    teachingDataZ -= (float)DistanceToRadian(GetCurrentZAxisDia(), (double)port.valZUp);
                                }
                            }

                            m_teaching.SetValXZ_ID(m_autoTeachingSensorData.id, (float)m_autoTeachingSensorData.x_centerPos, teachingDataZ);
                            if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                                m_teaching.SetValTurn_ID(m_autoTeachingSensorData.id, 0);
                            }
                            m_teaching.SaveDataFile();
                            SetAutoTeachingStep(AutoTeachingStep.Step26_AutoTeachingComplete);
                        }
                        break;


                    case AutoTeachingStep.Step26_AutoTeachingComplete:
                        if (m_main.IsAutoState()) {
                            m_main.SetSendBit(SendBitMap.Auto_Teaching_Complete, true);
                            if (m_main.GetReceiveBit(ReceiveBitMap.RM_Auto_Teaching_Complete_ACK)) {
                                SetAutoTeachingStep(AutoTeachingStep.Step0_Idle);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_Start_ACK, false);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_Complete, false);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_State, false);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_Alarm, false);
                                m_autoTeachingSensorData.ClearData();
                                m_autoTeachingTarget.ClearData();
                            }
                        }
                        else {
                            SetAutoTeachingStep(AutoTeachingStep.Step0_Idle);
                            m_isManualAutoTeachingRun = false;
                            m_autoTeachingSensorData.ClearData();
                            m_autoTeachingTarget.ClearData();
                        }

                        break;

                    case AutoTeachingStep.Step400_AutoTeachingFailed:
                        if (!AutoTeachingDelay(m_inposCheckTime))
                            break;

                        if (m_main.IsAutoState()) {
                            m_main.SetSendBit(SendBitMap.Auto_Teaching_Alarm, true);
                            if (m_main.GetReceiveBit(ReceiveBitMap.RM_Auto_Teaching_Complete_Alarm_ACK)) {
                                SetAutoTeachingStep(AutoTeachingStep.Step0_Idle);
                                AutoTeachingAxisStop();
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_Start_ACK, false);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_Complete, false);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_State, false);
                                m_main.SetSendBit(SendBitMap.Auto_Teaching_Alarm, false);

                                m_autoTeachingSensorData.ClearData();
                                m_autoTeachingTarget.ClearData();
                            }
                        }
                        else {
                            SetAutoTeachingStep(AutoTeachingStep.Step0_Idle);
                            AutoTeachingAxisStop();
                            m_isManualAutoTeachingRun = false;
                            m_autoTeachingSensorData.ClearData();
                            m_autoTeachingTarget.ClearData();
                        }

                        break;

                    case AutoTeachingStep.Step401_EMO:
                        //AutoTeachingAlarmLog(AutoTeachingAlarm.EMO, GetCurrentAutoTeachingStep());
                        //AutoTeachingAxisStop();
                        //m_main.SetSendBit(SendBitMap.Auto_Teaching_Start_ACK, false);
                        //m_main.SetSendBit(SendBitMap.Auto_Teaching_Complete, false);
                        //m_main.SetSendBit(SendBitMap.Auto_Teaching_State, false);
                        //m_main.SetSendBit(SendBitMap.Auto_Teaching_Alarm, false);

                        //m_autoTeachingSensorData.ClearData();
                        //m_autoTeachingTarget.ClearData();
                        break;

                    case AutoTeachingStep.Step402_Stop:
                        AutoTeachingAxisStop();
                        m_main.SetSendBit(SendBitMap.Auto_Teaching_Start_ACK, false);
                        m_main.SetSendBit(SendBitMap.Auto_Teaching_Complete, false);
                        m_main.SetSendBit(SendBitMap.Auto_Teaching_State, false);
                        m_main.SetSendBit(SendBitMap.Auto_Teaching_Alarm, false);
                        m_isManualAutoTeachingRun = false;
                        m_autoTeachingSensorData.ClearData();
                        m_autoTeachingTarget.ClearData();
                        SetAutoTeachingStep(AutoTeachingStep.Step0_Idle);
                        break;

                }
            }
            /// <summary>
            /// X,Z,T 가 타겟 위치 도착 후 Sensor가 검출되지 않았을 때 검출 Cycle 시작
            /// </summary>
            public void AutoTeachingSensorCheckCycle() {
                switch (m_sensorCheckStep) {
                    case SensorFindStep.Step1_XNegativeMove:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle) {
                            AutoTeachingSensorCheckMove(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Negative, false);
                            m_sensorCheckStep = SensorFindStep.Step2_XNegativeMoveSensorCheck;
                        }
                        break;

                    case SensorFindStep.Step2_XNegativeMoveSensorCheck:
                        if (AutoTeachingSensorCheck()) {
                            m_sensorCheckStep = SensorFindStep.Step100_SensorCheckComplete;
                            AutoTeachingAxisStop();
                            break;
                        }

                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                            m_sensorCheckStep = SensorFindStep.Step3_ZPositiveMove;
                        }
                        break;

                    case SensorFindStep.Step3_ZPositiveMove:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            AutoTeachingSensorCheckMove(AutoTeachingAxis.Axis_Z, AutoTeachingMoveDirection.Positive, false);
                            m_sensorCheckStep = SensorFindStep.Step4_ZPositiveMoveSensorCheck;
                        }
                        break;

                    case SensorFindStep.Step4_ZPositiveMoveSensorCheck:
                        if (AutoTeachingSensorCheck()) {
                            m_sensorCheckStep = SensorFindStep.Step100_SensorCheckComplete;
                            AutoTeachingAxisStop();
                            break;
                        }

                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                            m_sensorCheckStep = SensorFindStep.Step5_XPositiveMove;
                        }
                        break;

                    case SensorFindStep.Step5_XPositiveMove:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle) {
                            AutoTeachingSensorCheckMove(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Positive, true);
                            m_sensorCheckStep = SensorFindStep.Step6_XPositiveMoveSensorCheck;
                        }
                        break;

                    case SensorFindStep.Step6_XPositiveMoveSensorCheck:
                        if (AutoTeachingSensorCheck()) {
                            m_sensorCheckStep = SensorFindStep.Step100_SensorCheckComplete;
                            AutoTeachingAxisStop();
                            break;
                        }

                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                            m_sensorCheckStep = SensorFindStep.Step7_ZNegativeMove;
                        }
                        break;

                    case SensorFindStep.Step7_ZNegativeMove:
                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Idle) {
                            AutoTeachingSensorCheckMove(AutoTeachingAxis.Axis_Z, AutoTeachingMoveDirection.Negative, true);
                            m_sensorCheckStep = SensorFindStep.Step8_ZNegativeMoveSensorCheck;
                        }
                        break;

                    case SensorFindStep.Step8_ZNegativeMoveSensorCheck:
                        if (AutoTeachingSensorCheck()) {
                            m_sensorCheckStep = SensorFindStep.Step100_SensorCheckComplete;
                            AutoTeachingAxisStop();
                            break;
                        }

                        if (GetAxisStep(AxisList.Z_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.Z_Axis);
                            m_sensorCheckStep = SensorFindStep.Step9_XNegativeMove;
                        }
                        break;

                    case SensorFindStep.Step9_XNegativeMove:
                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Idle) {
                            AutoTeachingSensorCheckMove(AutoTeachingAxis.Axis_X, AutoTeachingMoveDirection.Negative, true);
                            m_sensorCheckStep = SensorFindStep.Step10_XNegativeMoveSensorCheck;
                        }
                        break;

                    case SensorFindStep.Step10_XNegativeMoveSensorCheck:
                        if (AutoTeachingSensorCheck()) {
                            m_sensorCheckStep = SensorFindStep.Step100_SensorCheckComplete;
                            AutoTeachingAxisStop();
                            break;
                        }

                        if (GetAxisStep(AxisList.X_Axis) == AxisStep.Finished) {
                            SetAxisStep(AxisStep.Idle, AxisList.X_Axis);
                            m_sensorCheckStep = SensorFindStep.Step400_SensorCheckFailed;
                        }
                        break;
                }
            }
            /// <summary>
            /// Sensor가 Off된 경우 해당 위치를 저장
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="dir"></param>
            /// <returns></returns>
            private bool AutoTeachingGetData(AutoTeachingAxis axis, AutoTeachingMoveDirection dir) {
                if (!AutoTeachingSensorCheck()) {
                    if (axis == AutoTeachingAxis.Axis_X) {
                        if (dir == AutoTeachingMoveDirection.Positive)
                            m_autoTeachingSensorData.x_positivePos = (float)GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis);
                        else if (dir == AutoTeachingMoveDirection.Negative)
                            m_autoTeachingSensorData.x_negativePos = (float)GetAxisStatus(AxisStatusType.pos_act, AxisList.X_Axis);
                    }
                    else if (axis == AutoTeachingAxis.Axis_Z) {
                        if (dir == AutoTeachingMoveDirection.Positive)
                            m_autoTeachingSensorData.z_positivePos = (float)GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis);
                        else if (dir == AutoTeachingMoveDirection.Negative)
                            m_autoTeachingSensorData.z_negativePos = (float)GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis);
                    }

                    return true;
                }
                else {
                    return false;
                }
            }
            /// <summary>
            /// X,Z 축의 Negative, Positive 위치를 기반으로 Center 데이터 저장
            /// </summary>
            /// <param name="axis"></param>
            private void SetAutoTeachingCenterData(AutoTeachingAxis axis) {
                if (axis == AutoTeachingAxis.Axis_X) {
                    float temp = m_autoTeachingSensorData.x_positivePos - m_autoTeachingSensorData.x_negativePos;
                    m_autoTeachingSensorData.width = temp;
                    temp /= 2;
                    m_autoTeachingSensorData.x_centerPos = m_autoTeachingSensorData.x_negativePos + temp + (m_param.GetAutoTeachingParam().autoTeachingCompensationX * 1000);
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"{(m_autoTeachingSensorData.x_positivePos * 1000):F0}"));
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"{(m_autoTeachingSensorData.x_negativePos * 1000):F0}"));
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"{(m_autoTeachingSensorData.width * 1000):F0}"));
                }
                else if (axis == AutoTeachingAxis.Axis_Z) {
                    float temp = m_autoTeachingSensorData.z_positivePos - m_autoTeachingSensorData.z_negativePos;
                    // 231115 추가
                    if (m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum)
                        m_autoTeachingSensorData.height = (float)(RadianToCalculateDistance(m_autoTeachingSensorData.z_positivePos) - RadianToCalculateDistance(m_autoTeachingSensorData.z_negativePos));
                    else
                        m_autoTeachingSensorData.height = (float)(m_autoTeachingSensorData.z_positivePos - m_autoTeachingSensorData.z_negativePos);

                    //m_autoTeachingSensorData.height = (float)temp;
                    temp /= 2;
                    m_autoTeachingSensorData.z_centerPos = m_autoTeachingSensorData.z_negativePos + temp + (m_param.GetAutoTeachingParam().autoTeachingCompensationZ * 1000);
                }
            }
            /// <summary>
            /// X,Z 축의 Negatvie, Positive 위치를 기반으로 길이 체크
            /// 설정된 길이보다 길거나 짧은 경우 실패
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            private bool AutoTeachingLengthCheck(AutoTeachingReflectorRange type) {
                switch (type) {
                    case AutoTeachingReflectorRange.Width:
                        if (m_autoTeachingSensorData.width > (m_param.GetAutoTeachingParam().autoTeachingReflectorWidth * 1000) + (m_param.GetAutoTeachingParam().autoTeachingReflectorErrorRangeWidth * 1000) ||
                            m_autoTeachingSensorData.width < (m_param.GetAutoTeachingParam().autoTeachingReflectorWidth * 1000) - (m_param.GetAutoTeachingParam().autoTeachingReflectorErrorRangeWidth * 1000)) {
                            return false;
                        }
                        else
                            return true;

                    case AutoTeachingReflectorRange.Height:
                        if (m_autoTeachingSensorData.height > (m_param.GetAutoTeachingParam().autoTeachingReflectorHeight * 1000) + (m_param.GetAutoTeachingParam().autoTeachingReflectorErrorRangeHeight * 1000) ||
                            m_autoTeachingSensorData.height < (m_param.GetAutoTeachingParam().autoTeachingReflectorHeight * 1000) - (m_param.GetAutoTeachingParam().autoTeachingReflectorErrorRangeHeight * 1000)) {
                            return false;
                        }
                        else {
                            return true;
                        }
                }

                return false;
            }
            /// <summary>
            /// Auto Teaching으로 지령 받은 ID가 정상적인지 확인
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            private bool AutoTeachingIDCheck(int id) {
                if (!m_teaching.IsExistPortOrShelf(id))
                    return false;

                if (m_teaching.GetTeachingData(id).row == null || m_teaching.GetTeachingData(id).col == null)
                    return false;

                if (m_teaching.GetTeachingData(id).valZUp == null || m_teaching.GetTeachingData(id).valZDown == null)
                    return false;

                return true;
            }
            /// <summary>
            /// Auto Teaching에서 사용되는 센서 상태 반환
            /// </summary>
            /// <returns></returns>
            private bool AutoTeachingSensorCheck() {
                if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                    if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Pick) {
                        return m_main.GetInputBit(InputList.Fork_Pick_Sensor_Left);
                    }
                    else if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Place) {
                        return m_main.GetInputBit(InputList.Fork_Place_Sensor_Left);
                    }
                }
                else if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                    if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Pick) {
                        if (m_teaching.GetTeachingData(m_autoTeachingTarget.id).direction == (int)PortDirection_HP.Left) {
                            return m_main.GetInputBit(InputList.Fork_Pick_Sensor_Left);
                        }
                        else if (m_teaching.GetTeachingData(m_autoTeachingTarget.id).direction == (int)PortDirection_HP.Right) {
                            return m_main.GetInputBit(InputList.Fork_Pick_Sensor_Right);
                        }
                    }
                    else if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Place) {
                        if (m_teaching.GetTeachingData(m_autoTeachingTarget.id).direction == (int)PortDirection_HP.Left) {
                            return m_main.GetInputBit(InputList.Fork_Place_Sensor_Left);
                        }
                        else if (m_teaching.GetTeachingData(m_autoTeachingTarget.id).direction == (int)PortDirection_HP.Right) {
                            return m_main.GetInputBit(InputList.Fork_Place_Sensor_Right);
                        }
                    }
                }
                else {
                    if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Pick) {
                        if (GetAxisSensor(AxisSensorType.Home, AxisList.T_Axis)) {
                            return m_main.GetInputBit(InputList.Fork_Pick_Sensor_Left);
                        }
                        else if (GetPosSensor(AxisList.T_Axis)) {
                            return m_main.GetInputBit(InputList.Fork_Pick_Sensor_Right);
                        }
                    }
                    else if (GetAutoTeachingSensorType() == AutoTeachingSensorType.Place) {
                        if (GetAxisSensor(AxisSensorType.Home, AxisList.T_Axis)) {
                            return m_main.GetInputBit(InputList.Fork_Place_Sensor_Left);
                        }
                        else if (GetPosSensor(AxisList.T_Axis)) {
                            return m_main.GetInputBit(InputList.Fork_Place_Sensor_Right);
                        }
                    }
                }

                return false;
            }
        }
    }
}
