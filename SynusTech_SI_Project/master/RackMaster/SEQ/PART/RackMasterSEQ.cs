using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using MovenCore;

namespace RackMaster.SEQ.PART {
    public class RackMasterSEQ {
        public enum RM_STEP {
            Idle = 0,

            //From Step
            From_ID_Check,
            From_CST_And_Fork_Home_Check = 100,
            From_Fork_Home_Move = 102,
            From_Fork_Home_Check = 103,
            From_XZT_From_Move = 112,
            From_XZT_From_Complete,
            From_Shelf_Port_Check,
            From_Port_Ready_Check = 122,
            From_Pick_Sensor_Cehck, // Shelf 일 때만 체크
            From_Fork_FWD_Move = 130,
            From_Fork_FWD_Check = 132,
            From_Z_Up,
            From_Z_Up_Override_Slow,
            From_Z_Up_Override_Fast,
            From_Z_Inposition_Check,
            From_CST_Check_Sensor,
            From_Source_Empty_Z_From_Pos_Move,
            From_Source_Empty_Z_Inposition_Check,
            From_Source_Empty_Fork_Home_Move,
            From_Source_Empty_Fork_Home_Check,
            From_Fork_BWD_Move,
            From_Fork_BWD_Check,
            From_Port_Ready_Off_Check,
            From_Complete,
            
            //To Step
            To_ID_Check = 198,
            To_CST_And_Fork_Home_Check = 199,
            To_Fork_Home_Move,
            To_Fork_Home_Check,
            To_XZT_To_Move,
            To_XZT_To_Complete,
            To_Double_Storage_Check,
            To_Shelf_Port_Check,
            To_Port_Ready_Check,
            To_Place_Sensor_Check, // Shelf일때만 To Sensor 체크
            To_Fork_FWD_Move,
            To_Fork_FWD_Check,
            To_Z_Down,
            To_Z_Down_Override_Slow,
            To_Z_Down_Override_Fast,
            To_Z_Inposition_Check,
            To_CST_Fork_Placement_Check,
            To_Fork_BWD_Move,
            To_Fork_BWD_Check,
            To_Port_Ready_Off_Check,
            To_Complete,

            Maint_Move = 400,
            Maint_Move_Check,
            Maint_Complete,

            Push_Move = 300,
            Push_Move_Check,
            Push_Complete,

            Inventory_Move,
            Inventory_Move_Check,
            Inventory_Complete,

            Error = 500,
            Warning,
            Event,
            WMX3_Call_Error,
            Stop,
            EMO,

            Store_Alt = 800,
            Resume_Request,
            Source_Empty = 810,
            Double_Storage = 820,

            Auto_Teaching_Start,
            Auto_Teaching_Compelte,
        }

        private static FSM m_mainFsm;
        private Motor m_motor;
        private static int m_fromID;
        private static int m_toID;

        private double m_forkHomePos = 0;

        private static double m_maintTargetX = 0;
        private static double m_maintTargetZ = 0;

        public RackMasterSEQ() {
            m_mainFsm = new FSM();
            m_motor = Motor.Instance;

            m_mainFsm.Set((int)RM_STEP.Idle);
        }

        public bool IsIdle() {
            if (m_mainFsm.Get() == (int)RM_STEP.Idle)
                return true;
            else
                return false;
        }

        public static RM_STEP GetCurrentStep() {
            return (RM_STEP)m_mainFsm.Get();
        }

        public static void SetStep(int stepIdx) {
            m_mainFsm.Set(stepIdx);
        }

        public static void SetFromID(int id) {
            m_fromID = id;
        }

        public static void SetToID(int id) {
            m_toID = id;
        }

        public static int GetFromID() {
            return m_fromID;
        }

        public static int GetToID() {
            return m_toID;
        }

        public static void SetMaintMoveTargetX(double target) {
            m_maintTargetX = target;
        }

        public static void SetMaintMoveTargetZ(double target) {
            m_maintTargetZ = target;
        }

        public static void FromStart() {
            if (Global.PIO_STATE)
                return;

            if(m_mainFsm.Get() == (int)RM_STEP.Idle)
                m_mainFsm.Set((int)RM_STEP.From_ID_Check);
        }

        public static void ToStart() {
            if (Global.PIO_STATE)
                return;

            if(m_mainFsm.Get() == (int)RM_STEP.Idle)
                m_mainFsm.Set((int)RM_STEP.To_ID_Check);
        }

        public static void MaintMoveStart() {
            if (Global.PIO_STATE)
                return;

            if (m_mainFsm.Get() == (int)RM_STEP.Idle)
                m_mainFsm.Set((int)RM_STEP.Maint_Move);
        }

        public static void EMO() {
            m_mainFsm.Set((int)RM_STEP.EMO);
        }

        public static void SetPIOInitial() {
            RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_TR_Request, false);
            RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_Busy, false);
            RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_Complete, false);
            RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_STK_Error, false);
        }

        public void Run() {
            PortData port;
            double targetX;
            double targetZ;
            double targetT;
            double targetA;
            double triggerValue;

            // CST 유무에 따라 From 대기 상태인지 To 대기 상태인지 올려주기
            if (Io.GetInputBit((int)IoList.InputList.Fork_Presence_Sensor) &&
                !Io.GetInputBit((int)IoList.InputList.Fork_Placement_Sensor_1) &&
                !Io.GetInputBit((int)IoList.InputList.Fork_Placement_Sensor_2)) {
                Global.CST_ON = true;
            }
            else {
                Global.CST_ON = false;
            }

            Alarm.AlarmCheck();

            switch (m_mainFsm.Get()) {
                case (int)RM_STEP.Idle:
                    SetPIOInitial();

                    break;

                // 전달 받은 ID에 대한 Error Check
                case (int)RM_STEP.From_ID_Check:
                    m_mainFsm.Set((int)RM_STEP.From_CST_And_Fork_Home_Check);
                    m_mainFsm.RstDelay();
                    break;

                    // CST 안착 체크랑 Fork Home 위치 체크
                case (int)RM_STEP.From_CST_And_Fork_Home_Check:
                    if (!RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_From_Start_Request)) {

                        if (m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_cmdPos != 0 || 
                            !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_origin) {
                            // fork축이 Home Position이 아닐 떄 Home Position으로 이동한다
                            m_mainFsm.Set((int)RM_STEP.From_Fork_Home_Move);
                            m_mainFsm.RstDelay();
                        }
                        else if (m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_cmdPos == 0 &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_origin) {
                            // fork축이 Home Position일 때 다음 Step으로 넘어감
                            m_mainFsm.Set((int)RM_STEP.From_XZT_From_Move);
                            m_mainFsm.RstDelay();
                        }
                    }

                    break;

                    // Fork Home으로
                case (int)RM_STEP.From_Fork_Home_Move:
                    if (Global.FORK_TYPE == Global.ForkType.Slide && Global.FORK_TYPE == Global.ForkType.Slide_NoTurn) {
                        ForkMove(m_forkHomePos);
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        targetT = m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_cmdPos - m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_cmdPos;
                        ForkMove(m_forkHomePos, targetT);
                    }
                    m_mainFsm.Set((int)RM_STEP.From_Fork_Home_Check);
                    m_mainFsm.RstDelay();
                    break;

                    // Fork Home 도착 했는지 확인
                case (int)RM_STEP.From_Fork_Home_Check:
                    if (!m_mainFsm.Delay(100)) {
                        break;
                    }

                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_posSet) {
                        m_mainFsm.Set((int)RM_STEP.From_XZT_From_Move);
                        m_mainFsm.RstDelay();
                    }
                    break;

                    // XZT 목적지 이동
                case (int)RM_STEP.From_XZT_From_Move:
                    port = Port.GetPortData(m_fromID);
                    targetZ = (double)port.valZ - (double)port.valZDown;
                    XZTMove((double)port.valX, targetZ, (double)port.valT);
                    m_mainFsm.Set((int)RM_STEP.From_XZT_From_Complete);
                    m_mainFsm.RstDelay();

                    break;

                    // XZT 목적지 도착 확인
                case (int)RM_STEP.From_XZT_From_Complete:
                    if (!m_mainFsm.Delay(300)) {
                        break;
                    }
                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_X].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_servoRun &&
                        !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun) {
                        if (m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_X].m_posSet && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_posSet &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_posSet) {
                            m_mainFsm.Set((int)RM_STEP.From_Shelf_Port_Check);
                            m_mainFsm.RstDelay();

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }
                    break;

                    // Port 인지 Shelf인지 구분
                case (int)RM_STEP.From_Shelf_Port_Check:
                    if(Port.GetPortType(m_fromID) == (int)Port.PortType.SHELF) {
                        m_mainFsm.Set((int)RM_STEP.From_Pick_Sensor_Cehck);
                        m_mainFsm.RstDelay();
                    }
                    else {
                        Global.PIO_STATE = true;
                        RM.ClearCassetteID();
                        RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_TR_Request, true);

                        if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Unload_Request)) {
                            RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_Busy, true);
                            m_mainFsm.Set((int)RM_STEP.From_Port_Ready_Check);
                            m_mainFsm.RstDelay();
                        }
                    }
                    break;

                    // PIO Ready 체크
                case (int)RM_STEP.From_Port_Ready_Check:
                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Ready)) {
                        m_mainFsm.Set((int)RM_STEP.From_Pick_Sensor_Cehck);
                    }
                    
                    break;

                case (int)RM_STEP.From_Pick_Sensor_Cehck:
                    m_mainFsm.Set((int)RM_STEP.From_Fork_FWD_Move);

                    break;

                    // Fork 전진
                case (int)RM_STEP.From_Fork_FWD_Move:
                    port = Port.GetPortData(m_fromID);
                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        ForkMove((double)port.valFork_A);
                        m_mainFsm.Set((int)RM_STEP.From_Fork_FWD_Check);
                        m_mainFsm.RstDelay();
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        ForkMove((double)port.valFork_A, (double)port.valFork_T);
                        m_mainFsm.Set((int)RM_STEP.From_Fork_FWD_Check);
                        m_mainFsm.RstDelay();
                    }
                    break;

                    // Fork 전진 도착 확인
                case (int)RM_STEP.From_Fork_FWD_Check:
                    if (!m_mainFsm.Delay(100)) {
                        break;
                    }
                    if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_posSet &&
                            !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_posSet) {
                            m_mainFsm.Set((int)RM_STEP.From_Z_Up);
                            m_mainFsm.RstDelay();

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_posSet) {
                            m_mainFsm.Set((int)RM_STEP.From_Z_Up);
                            m_mainFsm.RstDelay();

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }
                    break;

                    // Z축 상승
                case (int)RM_STEP.From_Z_Up:
                    port = Port.GetPortData(m_fromID);
                    targetZ = (double)port.valZ;
                    ZMove(targetZ);
                    m_mainFsm.Set((int)RM_STEP.From_Z_Up_Override_Slow);
                    m_mainFsm.RstDelay();

                    break;

                    // Center 위치에서 -5mm 일 때 속도 감속
                case (int)RM_STEP.From_Z_Up_Override_Slow:
                    port = Port.GetPortData(m_fromID);
                    targetZ = (double)port.valZ + (double)port.valZUp;
                    ZOverride(targetZ, RMParameters.Servo.Z_OVERRIDE_DIST, true);
                    m_mainFsm.Set((int)RM_STEP.From_Z_Up_Override_Fast);
                    m_mainFsm.RstDelay();

                    break;

                    // Center 위치에서 +5mm 일 때 속도 다시 상승
                case (int)RM_STEP.From_Z_Up_Override_Fast:
                    if (!m_mainFsm.Delay(100)) {
                        break;
                    }

                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_waitingTrigger) {
                        port = Port.GetPortData(m_fromID);
                        targetZ = (double)port.valZ + (double)port.valZUp;
                        ZOverride(targetZ, RMParameters.Servo.Z_OVERRIDE_DIST * 2, false);
                        m_mainFsm.Set((int)RM_STEP.From_Z_Inposition_Check);
                        m_mainFsm.RstDelay();
                    }

                    break;

                    // Z축 도착 확인
                case (int)RM_STEP.From_Z_Inposition_Check:
                    if (!m_mainFsm.Delay(100)) {
                        break;
                    }

                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_posSet) {
                        m_mainFsm.Set((int)RM_STEP.From_CST_Check_Sensor);
                        m_mainFsm.RstDelay();

                        if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                            m_mainFsm.Set((int)RM_STEP.Idle);
                        }
                    }

                    break;

                    // CST 체크
                case (int)RM_STEP.From_CST_Check_Sensor:
                    if (Global.CST_ON)
                        m_mainFsm.Set((int)RM_STEP.From_Fork_BWD_Move);
                    else
                        m_mainFsm.Set((int)RM_STEP.From_Source_Empty_Z_From_Pos_Move);

                    m_mainFsm.RstDelay();
                    break;

                    // Source Empty일 때 Z축 다시 하강
                case (int)RM_STEP.From_Source_Empty_Z_From_Pos_Move:
                    port = Port.GetPortData(m_fromID);
                    targetZ = (double)port.valZ - (double)port.valZDown;
                    ZMove(targetZ);
                    m_mainFsm.Set((int)RM_STEP.From_Source_Empty_Z_Inposition_Check);

                    m_mainFsm.RstDelay();
                    break;

                    // Z축 하강 확인
                case (int)RM_STEP.From_Source_Empty_Z_Inposition_Check:
                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_Inpos) {
                        m_mainFsm.Set((int)RM_STEP.From_Source_Empty_Fork_Home_Move);
                        m_mainFsm.RstDelay();
                    }

                    break;

                    // Fork 후진
                case (int)RM_STEP.From_Source_Empty_Fork_Home_Move:
                    port = Port.GetPortData(m_fromID);
                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        ForkMove((double)port.valFork_A);
                        m_mainFsm.Set((int)RM_STEP.From_Source_Empty_Fork_Home_Check);
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        ForkMove((double)port.valFork_A, (double)port.valT);
                        m_mainFsm.Set((int)RM_STEP.From_Source_Empty_Fork_Home_Check);
                    }

                    m_mainFsm.RstDelay();

                    break;

                    // Fork 후진 확인
                case (int)RM_STEP.From_Source_Empty_Fork_Home_Check:
                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_posSet) {
                            m_mainFsm.Set((int)RM_STEP.Source_Empty);
                            m_mainFsm.RstDelay();
                        }
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_posSet && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_posSet) {
                            m_mainFsm.Set((int)RM_STEP.Source_Empty);
                            m_mainFsm.RstDelay();
                        }
                    }

                    break;

                    // 정상적으로 CST가 올라왔을 때 Fork 후진
                case (int)RM_STEP.From_Fork_BWD_Move:
                    port = Port.GetPortData(m_fromID);
                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        ForkMove(m_forkHomePos);
                        m_mainFsm.Set((int)RM_STEP.From_Fork_BWD_Check);
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        ForkMove(m_forkHomePos, (double)port.valT);
                        m_mainFsm.Set((int)RM_STEP.From_Fork_BWD_Check);
                    }

                    m_mainFsm.RstDelay();
                    break;

                    // Fork 후진 도착 확인
                case (int)RM_STEP.From_Fork_BWD_Check:
                    if (!m_mainFsm.Delay(100)) {
                        break;
                    }

                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_Inpos) {
                            if (Global.PIO_STATE) {
                                m_mainFsm.Set((int)RM_STEP.From_Port_Ready_Off_Check);
                            }
                            else {
                                m_mainFsm.Set((int)RM_STEP.From_Complete);
                            }

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                            m_mainFsm.RstDelay();
                        }
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_Inpos && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_Inpos) {
                            if (Global.PIO_STATE) {
                                m_mainFsm.Set((int)RM_STEP.From_Port_Ready_Off_Check);
                            }
                            else {
                                m_mainFsm.Set((int)RM_STEP.From_Complete);
                            }

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                            m_mainFsm.RstDelay();
                        }
                    }

                    break;

                    // PIO UL-REQ Off 체크(Ready Off 상태 확인 X)
                case (int)RM_STEP.From_Port_Ready_Off_Check:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_Complete, true);

                    if(!RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Unload_Request)) {
                        SetPIOInitial();
                        if (!RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Ready))
                        {
                            m_mainFsm.Set((int)RM_STEP.From_Complete);
                            Global.PIO_STATE = false;
                        }
                    }

                    break;

                    // From Complete
                case (int)RM_STEP.From_Complete:
                    // From Complete 신호 보내기
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.From_Complete, true);
                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_From_Complete_ACK))
                    {
                        m_mainFsm.Set((int)RM_STEP.Idle);
                        SetFromID(0);
                    }
                    break;


                    // To ID 체크
                case (int)RM_STEP.To_ID_Check:
                    m_mainFsm.Set((int)RM_STEP.To_CST_And_Fork_Home_Check);

                    m_mainFsm.RstDelay();
                    break;

                case (int)RM_STEP.To_CST_And_Fork_Home_Check:
                    // IoList 수정
                    if (m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_cmdPos != 0 ||
                        !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_origin) {
                        m_mainFsm.Set((int)RM_STEP.To_Fork_Home_Move);
                    }
                    else if (m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_cmdPos == 0 &&
                        m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_origin) {
                        m_mainFsm.Set((int)RM_STEP.To_XZT_To_Move);
                    }

                    m_mainFsm.RstDelay();

                    break;

                    // Fork 홈 위치로 이동
                case (int)RM_STEP.To_Fork_Home_Move:
                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        ForkMove(m_forkHomePos);
                        m_mainFsm.Set((int)RM_STEP.To_Fork_Home_Check);
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        port = Port.GetPortData(m_toID);
                        ForkMove(m_forkHomePos, (double)port.valT);
                        m_mainFsm.Set((int)RM_STEP.To_Fork_Home_Check);
                    }

                    m_mainFsm.RstDelay();
                    break;

                    // Fork Home 위치 도착 확인
                case (int)RM_STEP.To_Fork_Home_Check:
                    if (!m_mainFsm.Delay(100)) {
                        break;
                    }

                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_Inpos) {
                            m_mainFsm.Set((int)RM_STEP.To_XZT_To_Move);
                            m_mainFsm.RstDelay();
                        }
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_Inpos && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_Inpos) {
                            m_mainFsm.Set((int)RM_STEP.To_XZT_To_Move);
                            m_mainFsm.RstDelay();
                        }
                    }

                    break;

                    // XZT 목적지 이동
                case (int)RM_STEP.To_XZT_To_Move:
                    port = Port.GetPortData(m_toID);
                    targetZ = (double)port.valZ + (double)port.valZUp;
                    XZTMove((double)port.valX, targetZ, (double)port.valT);
                    m_mainFsm.Set((int)RM_STEP.To_XZT_To_Complete);

                    break;

                case (int)RM_STEP.To_XZT_To_Complete:
                    if (!m_mainFsm.Delay(300))
                        break;

                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_X].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_servoRun &&
                        !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun) {
                        if (m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_X].m_Inpos && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_Inpos &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_Inpos) {
                            m_mainFsm.Set((int)RM_STEP.To_Double_Storage_Check);
                            m_mainFsm.RstDelay();

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }

                    break;

                    // Double Storage 센서 체크
                case (int)RM_STEP.To_Double_Storage_Check:
                    //if (Io.GetInputBit((int)IoList.InputList.Fork_Double_Storage_Sensor)) {
                    //    m_mainFsm.Set((int)RM_STEP.Double_Storage);
                    //}
                    //else {
                    //    m_mainFsm.Set((int)RM_STEP.To_Shelf_Port_Check);
                    //}
                    m_mainFsm.Set((int)RM_STEP.To_Shelf_Port_Check);
                    m_mainFsm.RstDelay();
                    break;

                    // Shelf인지 Port인지 구분
                case (int)RM_STEP.To_Shelf_Port_Check:
                    if(Port.GetPortType(m_toID) == (int)Port.PortType.SHELF) {
                        m_mainFsm.Set((int)RM_STEP.To_Fork_FWD_Move);
                    }
                    else {
                        Global.PIO_STATE = true;
                        RM.ClearCassetteID();
                        RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_TR_Request, true);

                        if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Load_Request)) {
                            m_mainFsm.Set((int)RM_STEP.To_Port_Ready_Check);
                        }
                    }
                    m_mainFsm.RstDelay();
                    break;

                    // PIO Ready 체크
                case (int)RM_STEP.To_Port_Ready_Check:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_Busy, true);

                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Ready)) {
                        m_mainFsm.Set((int)RM_STEP.To_Place_Sensor_Check);
                    }
                    break;

                case (int)RM_STEP.To_Place_Sensor_Check:
                    m_mainFsm.Set((int)RM_STEP.To_Fork_FWD_Move);

                    break;

                    // Fork 전진
                case (int)RM_STEP.To_Fork_FWD_Move:
                    port = Port.GetPortData(m_toID);
                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        ForkMove((double)port.valFork_A);
                        m_mainFsm.Set((int)RM_STEP.To_Fork_FWD_Check);
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        ForkMove((double)port.valFork_A, (double)port.valFork_T);
                        m_mainFsm.Set((int)RM_STEP.To_Fork_FWD_Check);
                    }
                    m_mainFsm.RstDelay();
                    break;

                    // Fork 전진 도착 확인
                case (int)RM_STEP.To_Fork_FWD_Check:
                    if (!m_mainFsm.Delay(100))
                        break;

                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_Inpos) {
                            m_mainFsm.Set((int)RM_STEP.To_Z_Down);
                            m_mainFsm.RstDelay();

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_Inpos && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_Inpos) {
                            m_mainFsm.Set((int)RM_STEP.To_Z_Down);
                            m_mainFsm.RstDelay();

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }

                    break;

                    // Z축 하강
                case (int)RM_STEP.To_Z_Down:
                    port = Port.GetPortData(m_toID);
                    targetZ = (double)port.valZ;
                    ZMove(targetZ);
                    m_mainFsm.Set((int)RM_STEP.To_Z_Down_Override_Slow);

                    break;

                    // Center 위치부터 +5mm 시점에서 속도 감속
                case (int)RM_STEP.To_Z_Down_Override_Slow:
                    port = Port.GetPortData(m_toID);
                    targetZ = (double)port.valZ - (double)port.valZDown;
                    ZOverride(targetZ, RMParameters.Servo.Z_OVERRIDE_DIST, true);
                    m_mainFsm.Set((int)RM_STEP.To_Z_Down_Override_Fast);
                    m_mainFsm.RstDelay();
                    break;

                    // Center 위치부터 -5mm 시점에서 속도 가속
                case (int)RM_STEP.To_Z_Down_Override_Fast:
                    if (!m_mainFsm.Delay(100))
                        break;

                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_waitingTrigger) {
                        port = Port.GetPortData(m_toID);
                        targetZ = (double)port.valZ - (double)port.valZDown;
                        ZOverride(targetZ, RMParameters.Servo.Z_OVERRIDE_DIST * 2, false);
                        m_mainFsm.Set((int)RM_STEP.To_Z_Inposition_Check);
                        m_mainFsm.RstDelay();
                    }

                    break;

                    // Z축 도착 확인
                case (int)RM_STEP.To_Z_Inposition_Check:
                    if (!m_mainFsm.Delay(100))
                        break;

                    if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_Inpos) {
                        m_mainFsm.Set((int)RM_STEP.To_CST_Fork_Placement_Check);
                        m_mainFsm.RstDelay();

                        if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                            m_mainFsm.Set((int)RM_STEP.Idle);
                        }
                    }

                    break;

                    // CST 센서 확인
                case (int)RM_STEP.To_CST_Fork_Placement_Check:
                    // Io List 수정 확인
                    if (!Global.CST_ON) {
                        m_mainFsm.Set((int)RM_STEP.To_Fork_BWD_Move);
                    }

                    break;

                    // Fork 후진
                case (int)RM_STEP.To_Fork_BWD_Move:
                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        ForkMove(m_forkHomePos);
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        port = Port.GetPortData(m_toID);
                        ForkMove(m_forkHomePos, (double)port.valT);
                    }
                    m_mainFsm.Set((int)RM_STEP.To_Fork_BWD_Check);
                    m_mainFsm.RstDelay();

                    break;

                    // Fork 후진 도착 확인
                case (int)RM_STEP.To_Fork_BWD_Check:
                    if (!m_mainFsm.Delay(100))
                        break;

                    if (Global.FORK_TYPE == Global.ForkType.Slide) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_Inpos) {
                            if (Global.PIO_STATE) {
                                m_mainFsm.Set((int)RM_STEP.To_Port_Ready_Off_Check);
                            }
                            else {
                                m_mainFsm.Set((int)RM_STEP.To_Complete);
                            }

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }
                    else if (Global.FORK_TYPE == Global.ForkType.SCARA) {
                        if (!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_servoRun &&
                            m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_A].m_Inpos && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_T].m_Inpos) {
                            if (Global.PIO_STATE) {
                                m_mainFsm.Set((int)RM_STEP.To_Port_Ready_Off_Check);
                            }
                            else {
                                m_mainFsm.Set((int)RM_STEP.To_Complete);
                            }

                            if (Global.MANUAL_STATE && Global.MANUAL_RUN) {
                                m_mainFsm.Set((int)RM_STEP.Idle);
                            }
                        }
                    }
                    m_mainFsm.RstDelay();

                    break;

                    // PIO L-REQ Off 체크(Ready 체크 X)
                case (int)RM_STEP.To_Port_Ready_Off_Check:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.PIO_Complete, true);
                    RM.SetCassetteID();

                    if (!RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Load_Request))
                    {
                        if (RM.CompareCassetteID())
                        {
                            SetPIOInitial();
                            if (!RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.PIO_Ready))
                            {
                                m_mainFsm.Set((int)RM_STEP.To_Complete);
                                Global.PIO_STATE = false;
                            }
                        }
                    }
                    break;

                case (int)RM_STEP.To_Complete:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.To_Complete, true);
                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_To_Complete_ACK))
                    {
                        m_mainFsm.Set((int)RM_STEP.Idle);
                        SetToID(0);
                    }

                    break;


                case (int)RM_STEP.Maint_Move:
                    MaintMove(m_maintTargetX, m_maintTargetZ);
                    m_mainFsm.Set((int)RM_STEP.Maint_Move_Check);
                    break;

                case (int)RM_STEP.Maint_Move_Check:
                    if(!m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_X].m_servoRun && !m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_servoRun &&
                        m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_X].m_posSet && m_motor.m_status[RMParameters.Servo.AXIS_NUMBER_Z].m_posSet) {
                        m_mainFsm.Set((int)RM_STEP.Maint_Complete);
                    }

                    break;

                case (int)RM_STEP.Maint_Complete:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Maint_Move_Complete, true);

                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_Maint_Complete_ACK)) {
                        RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Maint_Move_Complete, false);
                        m_mainFsm.Set((int)RM_STEP.Idle);
                    }

                    break;


                case (int)RM_STEP.Push_Move:
                    break;

                case (int)RM_STEP.Push_Complete:
                    break;


                case (int)RM_STEP.Inventory_Move:
                    break;

                case (int)RM_STEP.Inventory_Complete:
                    break;

                case (int)RM_STEP.Error:
                    
                    break;

                case (int)RM_STEP.Stop:
                    Stop();
                    m_mainFsm.Set((int)RM_STEP.Idle);

                    break;

                case (int)RM_STEP.EMO:
                    Stop();
                    m_mainFsm.Set((int)RM_STEP.Idle);

                    break;

                case (int)RM_STEP.Store_Alt:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Store_Alt_Request, true);

                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_Store_Alt_ACK)) {
                        RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Store_Alt_Request, false);
                        m_mainFsm.Set((int)RM_STEP.Idle);
                    }

                    break;

                case (int)RM_STEP.Resume_Request:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Resume_Request_Request, true);

                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_Resume_Request_ACK)) {
                        RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Resume_Request_Request, false);
                        m_mainFsm.Set((int)RM_STEP.Idle);
                    }

                    break;

                case (int)RM_STEP.Source_Empty:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Source_Empty_Request, true);

                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_Source_Empty_ACK)) {
                        RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Source_Empty_Request, false);
                        m_mainFsm.Set((int)RM_STEP.Idle);
                    }

                    break;

                case (int)RM_STEP.Double_Storage:
                    RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Double_Storage_Request, true);

                    if (RM.GetReceiveBit(TCP.TcpDataDef.ReceiveBitMap.RM_Double_Storage_ACK)) {
                        RM.SetSendBit(TCP.TcpDataDef.SendBitMap.Double_Storage_Request, false);
                        m_mainFsm.Set((int)RM_STEP.Idle);
                    }

                    break;
            }
        }

        public bool XZTMove(double targetX, double targetZ, double targetT, bool useInterpolation = false) {
            if (useInterpolation) {
                AxesProfile profile = new AxesProfile();
                profile.m_axisCount = 3;
                profile.m_axisArray[0] = RMParameters.Servo.AXIS_NUMBER_X;
                profile.m_axisArray[1] = RMParameters.Servo.AXIS_NUMBER_Z;
                profile.m_axisArray[2] = RMParameters.Servo.AXIS_NUMBER_T;

                profile.m_dest[0] = targetX;
                profile.m_dest[1] = targetZ;
                profile.m_dest[2] = targetT;

                profile.m_maxVel[0] = RMParameters.Servo.MAX_SPEED_X * RMParameters.Servo.AUTO_SPEED_PERCENT;
                profile.m_maxVel[1] = RMParameters.Servo.MAX_SPEED_Z * RMParameters.Servo.AUTO_SPEED_PERCENT;
                profile.m_maxVel[2] = RMParameters.Servo.MAX_SPEED_T * RMParameters.Servo.AUTO_SPEED_PERCENT;

                profile.m_maxAcc[0] = profile.m_maxVel[0] / RMParameters.Servo.MAX_ACC_DEC_X;
                profile.m_maxAcc[1] = profile.m_maxVel[1] / RMParameters.Servo.MAX_ACC_DEC_Z;
                profile.m_maxAcc[2] = profile.m_maxVel[2] / RMParameters.Servo.MAX_ACC_DEC_T;

                profile.m_maxDec[0] = profile.m_maxAcc[0];
                profile.m_maxDec[1] = profile.m_maxAcc[1];
                profile.m_maxDec[2] = profile.m_maxAcc[2];

                return m_motor.InterpolationMove(profile);
            }
            else {
                AxisProfile[] profile = new AxisProfile[3];
                for(int i = 0; i < profile.Length; i++) {
                    profile[i] = new AxisProfile();
                }

                profile[0].m_axis = RMParameters.Servo.AXIS_NUMBER_X;
                profile[0].m_dest = targetX;
                profile[0].m_profileType = WMXParam.m_profileType.JerkRatio;
                profile[0].m_velocity = RMParameters.Servo.MAX_SPEED_X * RMParameters.Servo.AUTO_SPEED_PERCENT;
                profile[0].m_acc = profile[0].m_velocity / RMParameters.Servo.MAX_ACC_DEC_X;
                profile[0].m_dec = profile[0].m_acc;
                profile[0].m_jerkRatio = RMParameters.Servo.JERK_RATIO_X;

                profile[1].m_axis = RMParameters.Servo.AXIS_NUMBER_Z;
                profile[1].m_dest = targetZ;
                profile[1].m_profileType = WMXParam.m_profileType.JerkRatio;
                profile[1].m_velocity = RMParameters.Servo.MAX_SPEED_Z * RMParameters.Servo.AUTO_SPEED_PERCENT;
                profile[1].m_acc = profile[1].m_velocity / RMParameters.Servo.MAX_ACC_DEC_Z;
                profile[1].m_dec = profile[1].m_acc;
                profile[1].m_jerkRatio = RMParameters.Servo.JERK_RATIO_Z;

                profile[2].m_axis = RMParameters.Servo.AXIS_NUMBER_T;
                profile[2].m_dest = targetT;
                profile[2].m_profileType = WMXParam.m_profileType.JerkRatio;
                profile[2].m_velocity = RMParameters.Servo.MAX_SPEED_T * RMParameters.Servo.AUTO_SPEED_PERCENT;
                profile[2].m_acc = profile[2].m_velocity / RMParameters.Servo.MAX_ACC_DEC_T;
                profile[2].m_dec = profile[2].m_acc;
                profile[2].m_jerkRatio = RMParameters.Servo.JERK_RATIO_T;

                bool ret = false;
                for(int i = 0; i < profile.Length; i++) {
                    ret = m_motor.AbsoluteMove(profile[i]);
                    if (!ret) {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool ZMove(double targetZ) {
            AxisProfile profile = new AxisProfile();

            profile.m_axis = RMParameters.Servo.AXIS_NUMBER_Z;
            profile.m_dest = targetZ;
            profile.m_profileType = WMXParam.m_profileType.JerkRatio;
            profile.m_velocity = RMParameters.Servo.MAX_SPEED_Z * RMParameters.Servo.AUTO_SPEED_PERCENT;
            profile.m_acc = profile.m_velocity / RMParameters.Servo.MAX_ACC_DEC_Z;
            profile.m_dec = profile.m_acc;
            profile.m_jerkRatio = RMParameters.Servo.JERK_RATIO_Z;

            return m_motor.AbsoluteMove(profile);
        }

        public bool ZOverride(double targetZ, double triggerValue, bool isSlow) {
            AxisProfile profile = new AxisProfile();
            TriggerCondition trig = new TriggerCondition();

            profile.m_axis = RMParameters.Servo.AXIS_NUMBER_Z;
            profile.m_dest = targetZ;
            profile.m_profileType = WMXParam.m_profileType.JerkRatio;

            trig.m_triggerAxis = RMParameters.Servo.AXIS_NUMBER_Z;
            if (isSlow) {
                trig.m_triggerType = WMXParam.m_triggerType.RemainginDistance;
                trig.m_triggerValue = triggerValue;
                profile.m_velocity = RMParameters.Servo.MAX_SPEED_Z * RMParameters.Servo.AUTO_SPEED_PERCENT * RMParameters.Servo.Z_OVERRIDE_SPEED_PERCENT;
                profile.m_acc = RMParameters.Servo.MAX_SPEED_Z * RMParameters.Servo.AUTO_SPEED_PERCENT * RMParameters.Servo.MAX_ACC_DEC_Z;
                profile.m_dec = profile.m_acc;
            }
            else {
                trig.m_triggerType = WMXParam.m_triggerType.CompletedDistance;
                trig.m_triggerValue = triggerValue;
                profile.m_velocity = RMParameters.Servo.MAX_SPEED_Z * RMParameters.Servo.AUTO_SPEED_PERCENT;
                profile.m_acc = profile.m_velocity / RMParameters.Servo.MAX_ACC_DEC_Z;
                profile.m_dec = profile.m_acc;
            }
            profile.m_jerkRatio = RMParameters.Servo.JERK_RATIO_Z;

            return m_motor.AbsoluteMove(profile, trig);
        }

        public bool ForkMove(double targetA, double targetT) {
            AxesProfile profile = new AxesProfile();

            profile.m_axisCount = 2;
            profile.m_axisArray[0] = RMParameters.Servo.AXIS_NUMBER_A;
            profile.m_axisArray[1] = RMParameters.Servo.AXIS_NUMBER_T;
            profile.m_profileType = WMXParam.m_profileType.JerkRatio;

            profile.m_dest[0] = targetA;
            profile.m_dest[1] = targetT;

            profile.m_maxVel[1] = RMParameters.Servo.MAX_SPEED_T * RMParameters.Servo.AUTO_SPEED_PERCENT;
            profile.m_maxVel[0] = RMParameters.Servo.MAX_SPEED_A * RMParameters.Servo.AUTO_SPEED_PERCENT;

            profile.m_maxAcc[0] = profile.m_maxVel[0] / RMParameters.Servo.MAX_ACC_DEC_A;
            profile.m_maxAcc[1] = profile.m_maxVel[1] / RMParameters.Servo.MAX_ACC_DEC_T;

            profile.m_maxDec[0] = profile.m_maxAcc[0];
            profile.m_maxDec[1] = profile.m_maxAcc[1];

            return m_motor.InterpolationMove(profile);
        }

        public bool ForkMove(double targetA) {
            AxisProfile profile = new AxisProfile();
            profile.m_axis = RMParameters.Servo.AXIS_NUMBER_X;
            profile.m_profileType = WMXParam.m_profileType.JerkRatio;
            profile.m_velocity = RMParameters.Servo.MAX_SPEED_A * RMParameters.Servo.AUTO_SPEED_PERCENT;
            profile.m_acc = profile.m_velocity / RMParameters.Servo.MAX_ACC_DEC_A;
            profile.m_dec = profile.m_acc;
            profile.m_jerkRatio = RMParameters.Servo.JERK_RATIO_A;

            return m_motor.AbsoluteMove(profile);
        }

        public bool MaintMove(double targetX, double targetZ) {
            AxesProfile profile = new AxesProfile();

            profile.m_axisCount = 2;
            profile.m_axisArray[0] = RMParameters.Servo.AXIS_NUMBER_X;
            profile.m_axisArray[1] = RMParameters.Servo.AXIS_NUMBER_T;
            profile.m_profileType = WMXParam.m_profileType.JerkRatio;

            profile.m_dest[0] = targetX;
            profile.m_dest[1] = targetZ;

            profile.m_maxVel[0] = RMParameters.Servo.MAX_SPEED_X * RMParameters.Servo.AUTO_SPEED_PERCENT;
            profile.m_maxVel[1] = RMParameters.Servo.MAX_SPEED_Z * RMParameters.Servo.AUTO_SPEED_PERCENT;

            profile.m_maxAcc[0] = profile.m_maxVel[0] / RMParameters.Servo.MAX_ACC_DEC_X;
            profile.m_maxAcc[1] = profile.m_maxVel[1] / RMParameters.Servo.MAX_ACC_DEC_Z;

            profile.m_maxDec[0] = profile.m_maxAcc[0];
            profile.m_maxDec[1] = profile.m_maxDec[1];

            return m_motor.InterpolationMove(profile);
        }

        public void Stop() {
            for(int i = 0; i < RMParameters.Servo.AXIS_COUNT; i++) {
                m_motor.Stop(i);
            }
        }

        public bool AllServoOn() {
            for(int i = 0; i < RMParameters.Servo.AXIS_COUNT; i++) {
                bool ret = m_motor.ServoOn(i);
                if (!ret)
                    return false;
            }

            return true;
        }

        public void AllServoOff() {
            for (int i = 0; i < RMParameters.Servo.AXIS_COUNT; i++) {
                m_motor.ServoOff(i);
            }
        }
    }
}
