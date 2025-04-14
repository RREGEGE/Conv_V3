using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.TCP;
using MovenCore;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.PART {
    public enum FullClosedStatus_Command {
        TargetDistance,
        Torque,
        Velocity,
        Pos,
    }

    public enum FullClosedStatus_Feedback {
        OpState,
        TargetDistance,
        CommandDistance,
        ActualDistance,
        RawDistance,
        Torque,
        Velocity,
        Pos,
        AlarmCode,
    }

    public enum AxisSensorType {
        Home = 0,
        Negative_Limit,
        Positive_Limit,
        SW_Negative_Limit,
        SW_Positive_Limit
    }

    public enum AxisStatusType {
        pos_cmd = 0,
        pos_act,
        vel_cmd,
        vel_act,
        trq_cmd,
        trq_act,
        Profile_Traget_Pisition,
    }

    public enum AxisFlagType {
        Servo_On = 0,
        Run,
        HomeDone,
        Alarm,
        Waiting_Trigger,
        Inposition,
        Poset,
        Homing,
        Joging,
    }

    public enum ForkPositionState {
        HomePosition,
        FWDPosition,
        BWDPosition,
    }

    public partial class RackMasterMain {
        /// <summary>
        /// 현재 오토 상태인지 반환
        /// </summary>
        /// <returns></returns>
        public bool IsAutoState() {
            return GetSendBit(SendBitMap.Auto_State);
        }
        /// <summary>
        /// Cassette가 Fork 위에 안착되었는지 판단
        /// 대각 감지, 안착 감지 센서 2개로 확인
        /// </summary>
        /// <returns></returns>
        public bool IsCassetteOn() {
            if(m_param.GetMotionParam().presensType == PresenseSensorType.AllTurn) {
                if (IsInputEnabled(InputList.Presense_Detection_1) && !GetInputBit(InputList.Presense_Detection_1))
                    return false;
                if (IsInputEnabled(InputList.Presense_Detection_2) && !GetInputBit(InputList.Presense_Detection_2))
                    return false;
            }else if(m_param.GetMotionParam().presensType == PresenseSensorType.LeftOrRight) {
                if (GetSendBit(SendBitMap.Turn_Left_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_1) && !GetInputBit(InputList.Presense_Detection_1))
                        return false;
                }else if (GetSendBit(SendBitMap.Turn_Right_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_2) && !GetInputBit(InputList.Presense_Detection_2))
                        return false;
                }
            }


            if(m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Normal) {
                if (IsInputEnabled(InputList.Fork_In_Place_1) && !GetInputBit(InputList.Fork_In_Place_1))
                    return false;
                if (IsInputEnabled(InputList.Fork_In_Place_2) && !GetInputBit(InputList.Fork_In_Place_2))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// Casstte가 Fork 위에 존재하지 않는지 판단
        /// 대각 감지, 안착 감지 센서 2개로 판단
        /// </summary>
        /// <returns></returns>
        public bool IsCassetteEmpty() {
            if (m_param.GetMotionParam().presensType == PresenseSensorType.AllTurn) {
                if (IsInputEnabled(InputList.Presense_Detection_1) && GetInputBit(InputList.Presense_Detection_1))
                    return false;
                if (IsInputEnabled(InputList.Presense_Detection_2) && GetInputBit(InputList.Presense_Detection_2))
                    return false;
            }else if(m_param.GetMotionParam().presensType == PresenseSensorType.LeftOrRight) {
                if (GetSendBit(SendBitMap.Turn_Left_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_1) && GetInputBit(InputList.Presense_Detection_1))
                        return false;
                }else if (GetSendBit(SendBitMap.Turn_Right_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_2) && GetInputBit(InputList.Presense_Detection_2))
                        return false;
                }
            }

            if(m_param.GetMotionParam().inPlaceType == InPlaceSensorType.Normal) {
                if (IsInputEnabled(InputList.Fork_In_Place_1) && GetInputBit(InputList.Fork_In_Place_1))
                    return false;
                if (IsInputEnabled(InputList.Fork_In_Place_2) && GetInputBit(InputList.Fork_In_Place_2))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 만약 IsCassettOn()과 IsCassetteEmpty() 반환 값이 같을 경우 Abnormal 상황이며 Stick Sensor 1개라도 꺼질 경우 Abnormal 상황으로 판단
        /// </summary>
        /// <returns></returns>
        public bool IsCassetteAbnormal() {
            if (IsCassetteOn() == IsCassetteEmpty())
                return true;

            if (IsStickDetecSensorOff())
                return true;

            return false;
        }
        /// <summary>
        /// 안착 감지 센서 2개 다 On 상태인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsInPlace_On() {
            if (IsInputEnabled(InputList.Fork_In_Place_1) && !GetInputBit(InputList.Fork_In_Place_1))
                return false;
            if (IsInputEnabled(InputList.Fork_In_Place_2) && !GetInputBit(InputList.Fork_In_Place_2))
                return false;

            return true;
        }
        /// <summary>
        /// 안착 감지 센서 2개 다 Off 상태인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsInPlace_Off() {
            if (IsInputEnabled(InputList.Fork_In_Place_1) && GetInputBit(InputList.Fork_In_Place_1))
                return false;
            if (IsInputEnabled(InputList.Fork_In_Place_2) && GetInputBit(InputList.Fork_In_Place_2))
                return false;

            return true;
        }
        /// <summary>
        /// 안착 감지 센서가 전부 Disable 상태인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsInplaceAllDisabled() {
            if (!IsInputEnabled(InputList.Fork_In_Place_1) && !IsInputEnabled(InputList.Fork_In_Place_2))
                return true;

            return false;
        }
        /// <summary>
        /// 대각 감지 센서 2개가 전부 On 상태인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsPresense_On() {
            if(m_param.GetMotionParam().presensType == PresenseSensorType.AllTurn) {
                if (IsInputEnabled(InputList.Presense_Detection_1) && !GetInputBit(InputList.Presense_Detection_1))
                    return false;
                if (IsInputEnabled(InputList.Presense_Detection_2) && !GetInputBit(InputList.Presense_Detection_2))
                    return false;
            }else if(m_param.GetMotionParam().presensType == PresenseSensorType.LeftOrRight) {
                if (GetSendBit(SendBitMap.Turn_Left_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_1) && !GetInputBit(InputList.Presense_Detection_1))
                        return false;
                }else if (GetSendBit(SendBitMap.Turn_Right_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_2) && !GetInputBit(InputList.Presense_Detection_2))
                        return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 대각 감지 센서 2개가 전부 Off 상태인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsPresense_Off() {
            if(m_param.GetMotionParam().presensType == PresenseSensorType.AllTurn) {
                if (IsInputEnabled(InputList.Presense_Detection_1) && GetInputBit(InputList.Presense_Detection_1))
                    return false;
                if (IsInputEnabled(InputList.Presense_Detection_2) && GetInputBit(InputList.Presense_Detection_2))
                    return false;
            }else if(m_param.GetMotionParam().presensType == PresenseSensorType.LeftOrRight) {
                if (GetSendBit(SendBitMap.Turn_Left_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_1) && GetInputBit(InputList.Presense_Detection_1))
                        return false;
                }else if (GetSendBit(SendBitMap.Turn_Right_Position_State)) {
                    if (IsInputEnabled(InputList.Presense_Detection_2) && GetInputBit(InputList.Presense_Detection_2))
                        return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 대각 감지 센서가 전부 Disable 상태인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsPresenseAllDisabled() {
            if (!IsInputEnabled(InputList.Presense_Detection_1) && !IsInputEnabled(InputList.Presense_Detection_2))
                return true;

            return false;
        }
        /// <summary>
        /// Stick 센서가 1개라도 꺼졌는지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsStickDetecSensorOff() {
            if (IsInputEnabled(InputList.StickDetectSensor_1) && !GetInputBit(InputList.StickDetectSensor_1))
                return true;
            if (IsInputEnabled(InputList.StickDetectSensor_2) && !GetInputBit(InputList.StickDetectSensor_2))
                return true;
            if (IsInputEnabled(InputList.StickDetectSensor_3) && !GetInputBit(InputList.StickDetectSensor_3))
                return true;
            if (IsInputEnabled(InputList.StickDetectSensor_4) && !GetInputBit(InputList.StickDetectSensor_4))
                return true;

            return false;
        }
        /// <summary>
        /// Double Storage 센서 1이 Off인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsDoubleStorage1_Off() {
            if (IsInputEnabled(InputList.Double_Storage_Sensor_1) && !GetInputBit(InputList.Double_Storage_Sensor_1))
                return false;

            return true;
        }
        /// <summary>
        /// Double Storage 센서 2가 Off인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsDoubleStorage2_Off() {
            if (IsInputEnabled(InputList.Double_Storage_Sensor_2) && !GetInputBit(InputList.Double_Storage_Sensor_2))
                return false;

            return true;
        }
        /// <summary>
        /// Double Storage 센서 1이 On인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsDoubleStorage1_On() {
            if (IsInputEnabled(InputList.Double_Storage_Sensor_1) && GetInputBit(InputList.Double_Storage_Sensor_1))
                return false;

            return true;
        }
        /// <summary>
        /// Double Storage 센서 2가 On인지 판단
        /// </summary>
        /// <returns></returns>
        public bool IsDoubleStorage2_On() {
            if (IsInputEnabled(InputList.Double_Storage_Sensor_2) && GetInputBit(InputList.Double_Storage_Sensor_2))
                return false;

            return true;
        }
        /// <summary>
        /// targetID에 맞춰 ForkType이 Slide-NoTurn인 경우를 고려하여 Double Storage Sensor가 On 상태인지 반환
        /// </summary>
        /// <param name="targetID"></param>
        /// <returns></returns>
        public bool IsDoubleStorage_On(int targetID) {
            if(m_teaching.GetTeachingData(targetID) != null && m_teaching.IsExistPortOrShelf(targetID) && m_teaching.IsExistTeachingData(targetID)) {
                if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                    if (m_teaching.GetTeachingData(targetID).direction == (int)PortDirection_HP.Left) {
                        return IsDoubleStorage1_On();
                    }
                    else if (m_teaching.GetTeachingData(targetID).direction == (int)PortDirection_HP.Right) {
                        return IsDoubleStorage2_On();
                    }
                }
                else {
                    return IsDoubleStorage1_On();
                }
            }

            return false;
        }
        /// <summary>
        /// 위 함수와 동일하며 해당 함수는 Off 상태인지 반환
        /// </summary>
        /// <param name="targetID"></param>
        /// <returns></returns>
        public bool IsDoubleStorage_Off(int targetID) {
            if (m_teaching.GetTeachingData(targetID) != null && m_teaching.IsExistPortOrShelf(targetID) && m_teaching.IsExistTeachingData(targetID)) {
                if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                    if (m_teaching.GetTeachingData(targetID).direction == (int)PortDirection_HP.Left) {
                        return IsDoubleStorage1_Off();
                    }
                    else if (m_teaching.GetTeachingData(targetID).direction == (int)PortDirection_HP.Right) {
                        return IsDoubleStorage2_Off();
                    }
                }
                else {
                    return IsDoubleStorage1_Off();
                }
            }

            return false;
        }
        /// <summary>
        /// 특정 Input 값을 반환
        /// 만약 ignoreBInvert 값이 true인 경우 raw 데이터를 반환
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ignoreBInvert"></param>
        /// <returns></returns>
        public bool GetInputBit(InputList input, bool ignoreBInvert = false) {
            if (!IsInputEnabled(input))
                return false;

            int addr = (m_param.GetInputParameter(input).byteAddr * 8) + m_param.GetInputParameter(input).bitAddr;
            if (ignoreBInvert) {
                return Io.GetInputBit(addr);
            }
            else {
                if (m_param.GetInputParameter(input).isBInvert) {
                    return !Io.GetInputBit(addr);
                }
                else {
                    return Io.GetInputBit(addr);
                }
            }
        }
        /// <summary>
        /// 특정 Output 값을 반환
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool GetOutputBit(OutputList output) {
            if (!IsOutputEnabled(output))
                return false;

            int addr = (m_param.GetOutputParameter(output).byteAddr * 8) + m_param.GetOutputParameter(output).bitAddr;
            return Io.GetOutputBit(addr);
        }
        /// <summary>
        /// 특정 Input이 Enable 상태인지 판단
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsInputEnabled(InputList input) {
            return m_param.IsInputParameterEnabed(input);
        }
        /// <summary>
        /// 특정 Output이 Enable 상태인지 판단
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool IsOutputEnabled(OutputList output) {
            return m_param.GetOutputParameter(output).isEnabled;
        }
        /// <summary>
        /// Maser와의 통신상태 반환
        /// </summary>
        /// <returns></returns>
        public bool IsConnected_Master() {
            return m_Socket.IsConnected();
        }
        /// <summary>
        /// EtherCAT 통신상태 반환
        /// </summary>
        /// <returns></returns>
        public bool IsConnected_EtherCAT() {
            return WMX3.IsEngineCommunicating() && IsAllSlaveOPState();
        }
        /// <summary>
        /// Regulator와의 통신상태 반환
        /// </summary>
        /// <returns></returns>
        public bool IsConnected_Regulator() {
            return m_regSocket.IsConnected();
        }
        /// <summary>
        /// HP 위치의 GOT가 감지되었는지
        /// </summary>
        /// <returns></returns>
        public bool IsGOTDetected_HP() {
            if (GetInputBit(InputList.HP_DTP_Mode_Select_SW_1) || GetInputBit(InputList.HP_DTP_Mode_Select_SW_2))
                return true;

            return false;
        }
        /// <summary>
        /// OP 위치의 GOT가 감지되었는지
        /// </summary>
        /// <returns></returns>
        public bool IsGOTDetected_OP() {
            if (GetInputBit(InputList.OP_DTP_Mode_Select_SW_1) || GetInputBit(InputList.OP_DTP_Mode_Select_SW_2))
                return true;

            return false;
        }
        /// <summary>
        /// GOT를 사용할지 여부 판단
        /// 만약 GOT 관련 센서 중 1개라도 Disable이 되어있는 경우 사용하지 않는다고 판단
        /// </summary>
        /// <returns></returns>
        public bool IsUseGOT() {
            if (!IsInputEnabled(InputList.HP_DTP_Mode_Select_SW_1) || !IsInputEnabled(InputList.HP_DTP_Mode_Select_SW_2) ||
                !IsInputEnabled(InputList.HP_DTP_DeadMan_SW) || !IsInputEnabled(InputList.HP_DTP_EMS_SW))
                return false;

            if (!IsInputEnabled(InputList.OP_DTP_Mode_Select_SW_1) || !IsInputEnabled(InputList.OP_DTP_Mode_Select_SW_2) ||
                !IsInputEnabled(InputList.OP_DTP_DeadMan_SW) || !IsInputEnabled(InputList.OP_DTP_EMS_SW))
                return false;

            return true;
        }

        public partial class RackMasterMotion {
            /// <summary>
            /// 모든 축이 Servo On 상태인지
            /// </summary>
            /// <returns></returns>
            public bool IsAllServoOn() {
                foreach (AxisList item in Enum.GetValues(typeof(AxisList))) {
                    if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                        if (item == AxisList.T_Axis)
                            continue;
                    }
                    if (!GetAxisFlag(AxisFlagType.Servo_On, item))
                        return false;
                }
                return true;
            }
            /// <summary>
            /// 모든 축이 Home Done 상태인지
            /// </summary>
            /// <returns></returns>
            public bool IsAllHomeDone() {
                foreach (AxisList item in Enum.GetValues(typeof(AxisList))) {
                    if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                        if (item == AxisList.T_Axis)
                            continue;
                    }
                    if (!GetAxisFlag(AxisFlagType.HomeDone, item))
                        return false;
                }
                return true;
            }
            /// <summary>
            /// Amp Alarm Code를 토대로 해당 Alarm Code가 Absolute 관련 알람인지
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool IsAbsoluteAlarm(AxisList axis) {
                return m_axis.IsAbsoluteAlarm(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// 축 관련 Sensor 값 반환
            /// </summary>
            /// <param name="sensor"></param>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool GetAxisSensor(AxisSensorType sensor, AxisList axis) {
                switch (sensor) {
                    case AxisSensorType.Home:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_origin;

                    case AxisSensorType.Negative_Limit:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_nLimit;

                    case AxisSensorType.Positive_Limit:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_pLimit;

                    case AxisSensorType.SW_Negative_Limit:
                        if(axis == AxisList.X_Axis && m_param.GetMotionParam().useFullClosed) {
                            if(GetFullClosedStatus().m_fbStatus.m_actualDistance < m_param.GetAxisParameter(axis).swLimitNegative ||
                                GetDetectSensor_PositionlValue() < m_param.GetAxisParameter(axis).swLimitNegative) {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        else {
                            return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_nLimitSoft;
                        }

                    case AxisSensorType.SW_Positive_Limit:
                        if(axis == AxisList.X_Axis && m_param.GetMotionParam().useFullClosed) {
                            if (GetFullClosedStatus().m_fbStatus.m_actualDistance > m_param.GetAxisParameter(axis).swLimitPositive ||
                                GetDetectSensor_PositionlValue() > m_param.GetAxisParameter(axis).swLimitPositive) {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
                        else {
                            return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_pLimitSoft;
                        }
                }
                return false;
            }
            /// <summary>
            /// 축 관련 Flag 값 반환
            /// </summary>
            /// <param name="flag"></param>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool GetAxisFlag(AxisFlagType flag, AxisList axis) {
                switch (flag) {
                    case AxisFlagType.Servo_On:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_servoOn;

                    case AxisFlagType.Run:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_servoRun;

                    case AxisFlagType.HomeDone:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_homeDone;

                    case AxisFlagType.Alarm:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_alarm;

                    case AxisFlagType.Waiting_Trigger:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_waitingTrigger;

                    case AxisFlagType.Inposition:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_Inpos;

                    case AxisFlagType.Poset:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_posSet;

                    case AxisFlagType.Homing:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_homing;

                    case AxisFlagType.Joging:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_Joging;
                }
                return false;
            }
            /// <summary>
            /// 축 상태 반환
            /// </summary>
            /// <param name="status"></param>
            /// <param name="axis"></param>
            /// <returns></returns>
            public double GetAxisStatus(AxisStatusType status, AxisList axis) {
                switch (status) {
                    case AxisStatusType.pos_cmd:
                        if (m_param.GetMotionParam().useFullClosed && axis == AxisList.X_Axis) {
                            return m_axis.GetFullClosedStatus().m_cmdStatus.m_targetDistance;
                        }
                        else {
                            return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_cmdPos;
                        }

                    case AxisStatusType.pos_act:
                        if (m_param.GetMotionParam().useFullClosed && axis == AxisList.X_Axis) {
                            return m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance;
                        }
                        else {
                            return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_actualPos;
                        }

                    case AxisStatusType.vel_cmd:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_cmdVelocity;

                    case AxisStatusType.vel_act:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_actualVelocity;

                    case AxisStatusType.trq_cmd:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_cmdTorque;

                    case AxisStatusType.trq_act:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_actualTorque;

                    case AxisStatusType.Profile_Traget_Pisition:
                        return m_axis.GetAxisStatus(m_param.GetAxisNumber(axis)).m_profileTargetPos;
                }
                return 0;
            }
            /// <summary>
            /// Amp 온도 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public int GetAmpTemperature(AxisList axis) {
                return m_axis.GetAmpTemperature(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// 모든 축이 알람 상태가 아닌지 판단
            /// </summary>
            /// <returns></returns>
            public bool IsAllAxisIsNotAlarmState() {
                foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    if(GetAxisFlag(AxisFlagType.Alarm, axis)) {
                        return false;
                    }
                }

                return true;
            }
            /// <summary>
            /// 지름값과 회전각도를 통해 거리로 환산
            /// </summary>
            /// <param name="dia"></param>
            /// <param name="radian"></param>
            /// <returns></returns>
            public static double RadianToDistance(int dia, double radian) {
                double dist = (dia * Math.PI) * (radian / 360.0);

                return dist;
            }
            /// <summary>
            /// 현재 축의 회전량을 토대로 홈 위치부터 현재 위치까지의 총 이동거리 환산
            /// </summary>
            /// <param name="radian"></param>
            /// <returns></returns>
            public double RadianToCalculateDistance(double radian) {
                double calcPos = 0;
                int dia = m_param.GetMotionParam().ZAxisBeltFirstDia;
                for (int i = 0; i <= RadianToCaluclateRotationCount(radian); i++) {
                    if (i == RadianToCaluclateRotationCount(radian)) {
                        calcPos += RadianToDistance(dia, radian % 360000);
                    }
                    else {
                        calcPos += RadianToDistance(dia, 360000);
                    }
                    dia += m_param.GetMotionParam().ZAxisBeltDia;
                }
                return calcPos;
            }
            /// <summary>
            /// 회전 각도 양을 토대로 몇 회전 돌았는지 판단
            /// </summary>
            /// <param name="radian"></param>
            /// <returns></returns>
            public int RadianToCaluclateRotationCount(double radian) {
                return (int)(radian / 360000);
            }
            /// <summary>
            /// Z축에 사용되는 드럼벨트 타입의 경우 회전을 하면 할 수록 벨트가 안쪽 벨트를 덮으며 감겨 지름이 변경됨
            /// 현재까지 회전된 양과 설정한 Parameter 값과 매개변수로 받은 회전양을 통해 지름값을 반환
            /// </summary>
            /// <param name="radian"></param>
            /// <returns></returns>
            public int RadianToCalculateDia(double radian) {
                int dia = m_param.GetMotionParam().ZAxisBeltFirstDia;
                for(int i = 0; i < RadianToCaluclateRotationCount(radian); i++) {
                    dia += m_param.GetMotionParam().ZAxisBeltDia;
                }

                return dia;
            }
            /// <summary>
            /// 위 함수와 계산 방식은 동일하나 회전양의 경우 Z축 Position Data를 토대로 계산되어 값을 반환
            /// </summary>
            /// <returns></returns>
            public int GetCurrentZAxisDia() {
                int dia = m_param.GetMotionParam().ZAxisBeltFirstDia;
                for(int i = 0; i < GetCurrentZAxisRotationCount(); i++) {
                    dia += m_param.GetMotionParam().ZAxisBeltDia;
                }

                return dia;
            }
            /// <summary>
            /// 현재 Z축의 Position을 토대로 몇 회전 하였는지 계산
            /// </summary>
            /// <returns></returns>
            public int GetCurrentZAxisRotationCount() {
                double currentPos = m_axis.GetAxisStatus(m_param.GetAxisNumber(AxisList.Z_Axis)).m_actualPos;

                return (int)(currentPos + (m_param.GetMotionParam().ZAxisBeltHomeOffset * 1000)) / 360000;
            }
            /// <summary>
            /// 거리값과 지름값을 입력했을 때 각도를 계산
            /// </summary>
            /// <param name="dia"></param>
            /// <param name="dist"></param>
            /// <returns></returns>
            public static double DistanceToRadian(int dia, double dist) {
                double rad = (360.0 * dist) / (dia * Math.PI);

                return rad;
            }
            /// <summary>
            /// 거리값이 주어졌을 때 설정된 지름값을 토대로 현재 각도를 반환
            /// </summary>
            /// <param name="dist"></param>
            /// <returns></returns>
            public double DistanceToCalculatedRadian(double dist) {
                double rad = 0;
                int dia = m_param.GetMotionParam().ZAxisBeltFirstDia;
                while (true) {
                    double maxDistance = RadianToDistance(dia, 360000);
                    if(dist <= maxDistance) {
                        rad += DistanceToRadian(dia, dist);
                        break;
                    }
                    else {
                        rad += 360000;
                        dia += m_param.GetMotionParam().ZAxisBeltDia;
                        dist -= maxDistance;
                    }
                }

                return rad;
            }
            /// <summary>
            /// Drum Belt 타입의 Z축인 경우 이동거리로 계산된 위치 값을 반환(Actual)
            /// </summary>
            /// <returns></returns>
            public double GetDrumBeltZAxisActualPosition() {
                return RadianToCalculateDistance(GetAxisStatus(AxisStatusType.pos_act, AxisList.Z_Axis));
            }
            /// <summary>
            /// Drum Belt 타입의 Z축인 경우 이동거리로 계산된 위치 값을 반환(Command)
            /// </summary>
            /// <returns></returns>
            public double GetDrumBeltZAxisCommandPosition() {
                return RadianToCalculateDistance(GetAxisStatus(AxisStatusType.pos_cmd, AxisList.Z_Axis));
            }
            /// <summary>
            /// Drum Belt 타입의 Z축인 경우 이동거리로 계산된 속도 값을 반환(Actual)
            /// </summary>
            /// <returns></returns>
            public double GetDrumBeltZAxisActualVelocity() {
                return RadianToDistance(GetCurrentZAxisDia(), GetAxisStatus(AxisStatusType.vel_act, AxisList.Z_Axis));
            }
            /// <summary>
            /// Drum Belt 타입의 Z축인 경우 이동거리로 계산된 속도 값을 반환(Command)
            /// </summary>
            /// <returns></returns>
            public double GetDrumBeltZAxisCommandVelocity() {
                return RadianToDistance(GetCurrentZAxisDia(), GetAxisStatus(AxisStatusType.vel_cmd, AxisList.Z_Axis));
            }
            /// <summary>
            /// Drum Belt 타입의 Z축인 경우 타겟 위치를 이동 거리로 계산하여 반환
            /// </summary>
            /// <returns></returns>
            public double GetDrumBeltZAxisTargetPosition() {
                return RadianToDistance(GetCurrentZAxisDia(), GetAxisStatus(AxisStatusType.Profile_Traget_Pisition, AxisList.Z_Axis));
            }
            /// <summary>
            /// 특정 축의 Position Sensor 상태 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool GetPosSensor(AxisList axis) {
                if (!m_param.GetAxisParameter(axis).posSensorEnabled)
                    return true;

                int byteAddr = m_param.GetAxisParameter(axis).posSensorByteAddr;
                int bitAddr = m_param.GetAxisParameter(axis).posSensorBitAddr;

                return Io.GetInputBit(byteAddr, bitAddr);
            }
            /// <summary>
            /// 특정 축의 Position Sensor 2 상태 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool GetPosSensor2(AxisList axis) {
                if (!m_param.GetAxisParameter(axis).posSensor2Enabled)
                    return true;

                int byteAddr = m_param.GetAxisParameter(axis).posSensor2ByteAddr;
                int bitAddr = m_param.GetAxisParameter(axis).posSensor2BitAddr;

                return Io.GetInputBit(byteAddr, bitAddr);
            }
            /// <summary>
            /// 거리감지센서의 실제 Position Value를 int형으로 반환
            /// </summary>
            /// <returns></returns>
            public int GetDetectSensor_PositionlValue() {
                return Io.GetInputData_Int(m_param.GetMotionParam().distDetectSensor);
            }
            /// <summary>
            /// Full Closed User Rtdll에서 제공하는 상태 값
            /// </summary>
            /// <returns></returns>
            public BarcodeStatus GetFullClosedStatus() {
                return m_axis.GetFullClosedStatus();
            }
            /// <summary>
            /// 거리감지센서의 실제 Velocity Value를 int형으로 반환
            /// </summary>
            /// <returns></returns>
            public int GetDetectSensor_VelocityValue() {
                int byteAddr = m_param.GetMotionParam().distDetectSensor + (int)DistanceDetectSensorByteList.VelocityValue;

                return Io.GetInputData_Int(byteAddr);
            }
            /// <summary>
            /// 거리감지 센서에서 제공하는 Position의 상태 값들을 반환(Error 등)
            /// </summary>
            /// <param name="status"></param>
            /// <returns></returns>
            public bool GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus status) {
                int byteAddr = (int)status / 8;
                int bitAddr = (int)status % 8;

                byteAddr += m_param.GetMotionParam().distDetectSensor + (int)DistanceDetectSensorByteList.PositionStatus;

                return Io.GetInputBit(byteAddr, bitAddr);
            }
            /// <summary>
            /// 거리감지 센서에서 제공하는 Velocity의 상태 값들을 반환(Error 등)
            /// </summary>
            /// <param name="status"></param>
            /// <returns></returns>
            public bool GetDetectSensor_VelocityStatus(DistanceDetectSensorVelocityStatus status) {
                int byteAddr = (int)status / 8;
                int bitAddr = (int)status % 8;

                byteAddr += m_param.GetMotionParam().distDetectSensor + (int)DistanceDetectSensorByteList.VelocityStatus;

                return Io.GetInputBit(byteAddr, bitAddr);
            }
            /// <summary>
            /// Amp Alarm Code 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public int GetAxisAlarmCode(AxisList axis) {
                return m_axis.GetAlarmCode(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// Fork 축이 Home Done 상태인지 체크
            /// </summary>
            /// <returns></returns>
            public bool IsForkHomeCheck() {
                switch (m_param.GetMotionParam().forkType) {
                    case ForkType.SCARA:
                        return (GetAxisFlag(AxisFlagType.HomeDone, AxisList.A_Axis) && GetAxisFlag(AxisFlagType.HomeDone, AxisList.T_Axis) &&
                            IsHomePositionWithinErrorRange(AxisList.A_Axis));

                    case ForkType.Slide:
                    case ForkType.Slide_NoTurn:
                        return (GetAxisFlag(AxisFlagType.HomeDone, AxisList.A_Axis) && IsHomePositionWithinErrorRange(AxisList.A_Axis));
                }

                return false;
            }
            /// <summary>
            /// Fork축이 Home Position인지 체크
            /// </summary>
            /// <returns></returns>
            public bool ForkHomeCheck() {
                switch (m_param.GetMotionParam().forkType) {
                    case ForkType.SCARA:
                        return (GetAxisFlag(AxisFlagType.HomeDone, AxisList.A_Axis) && GetAxisFlag(AxisFlagType.HomeDone, AxisList.T_Axis) &&
                            (int)GetAxisStatus(AxisStatusType.pos_cmd, AxisList.A_Axis) == 0);

                    case ForkType.Slide:
                    case ForkType.Slide_NoTurn:
                        return (GetAxisFlag(AxisFlagType.HomeDone, AxisList.A_Axis) && (int)GetAxisStatus(AxisStatusType.pos_cmd, AxisList.A_Axis) == 0);
                }

                return false;
            }
            /// <summary>
            /// 각 축의 Home Position이 설정한 homePositionRange 값 이내로 들어오는지 체크
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool IsHomePositionWithinErrorRange(AxisList axis) {
                return (GetAxisStatus(AxisStatusType.pos_act, axis) < m_param.GetAxisParameter(axis).homePositionRange * 1000);
            }
            /// <summary>
            /// 현재 From/To 동작 중인 경우의 Target ID
            /// </summary>
            /// <param name="mode"></param>
            /// <returns></returns>
            public int GetCurrentTargetID(MotionMode mode) {
                switch (mode) {
                    case MotionMode.From:
                        return GetCurrentTargetFromID();

                    case MotionMode.To:
                        return GetCurrentTargetToID();
                }
                return 0;
            }
            /// <summary>
            /// 현재 Event 상태 반환
            /// </summary>
            /// <returns></returns>
            public EventList GetCurrentEvent() {
                switch (GetCurrentAutoStep()) {
                    case AutoStep.Step820_Double_Storage:
                        return EventList.DoubleStorage;

                    case AutoStep.Step801_Resume_Request:
                        return EventList.ResumeRequest;

                    case AutoStep.Step810_Source_Empty:
                        return EventList.SourceEmpty;

                    case AutoStep.Step800_Store_Alt:
                        return EventList.StoreAlt;
                }
                return EventList.None;
            }
            /// <summary>
            /// 현재 From 동작 중인지
            /// </summary>
            /// <returns></returns>
            public bool IsFromRun() {
                if (!IsAutoMotionRun() || IsAutoTeachingRun())
                    return false;

                return (GetCurrentAutoStep() >= AutoStep.Step101_From_CST_And_Fork_Home_Check && GetCurrentAutoStep() <= AutoStep.Step145_From_Complete);
            }
            /// <summary>
            /// From 동작이 완료 되었는지
            /// </summary>
            /// <returns></returns>
            public bool IsFromComplete() {
                return (GetCurrentAutoStep() == AutoStep.Step145_From_Complete);
            }
            /// <summary>
            /// 현재 To 동작 중인지
            /// </summary>
            /// <returns></returns>
            public bool IsToRun() {
                if (!IsAutoMotionRun() || IsAutoTeachingRun())
                    return false;

                return (GetCurrentAutoStep() >= AutoStep.Step201_To_CST_And_Fork_Home_Check && GetCurrentAutoStep() <= AutoStep.Step220_To_Complete);
            }
            /// <summary>
            /// To 동작이 완료 되었는지
            /// </summary>
            /// <returns></returns>
            public bool IsToComplete() {
                return (GetCurrentAutoStep() == AutoStep.Step220_To_Complete);
            }
            /// <summary>
            /// 현재 Maint 동작 중인지
            /// </summary>
            /// <returns></returns>
            public bool IsMaintRun() {
                return (GetCurrentAutoStep() >= AutoStep.Step400_Maint_Move_Fork_Home_Check && GetCurrentAutoStep() <= AutoStep.Step405_Maint_Complete);
            }
            /// <summary>
            /// 현재 From/To 동작 중 XZT 동작 스텝인지
            /// </summary>
            /// <returns></returns>
            public bool IsXZTGoing() {
                return (GetCurrentAutoStep() >= AutoStep.Step112_From_XZT_From_Move && GetCurrentAutoStep() <= AutoStep.Step113_From_XZT_From_Complete &&
                    GetCurrentAutoStep() >= AutoStep.Step204_To_XZT_To_Move && GetCurrentAutoStep() <= AutoStep.Step205_To_XZT_To_Complete);
            }
            /// <summary>
            /// 현재의 Auto Motion Step 반환
            /// </summary>
            /// <returns></returns>
            public AutoStep GetCurrentAutoStep() {
                return m_autoMotionStep;
            }
            /// <summary>
            /// CIM에게 보고 될 From 동작의 스텝 반환
            /// </summary>
            /// <param name="step"></param>
            /// <returns></returns>
            public bool IsCompleteFromStep(FromToStepNumber step) {
                switch (step) {
                    case FromToStepNumber.Zero:
                        return (GetCurrentAutoStep() >= AutoStep.Step112_From_XZT_From_Move && GetCurrentAutoStep() <= AutoStep.Step145_From_Complete);

                    case FromToStepNumber.One:
                        return (GetCurrentAutoStep() >= AutoStep.Step114_From_Shelf_Port_Check && GetCurrentAutoStep() <= AutoStep.Step145_From_Complete);

                    case FromToStepNumber.Two:
                        return (GetCurrentAutoStep() >= AutoStep.Step133_From_Z_Up && GetCurrentAutoStep() <= AutoStep.Step145_From_Complete);

                    case FromToStepNumber.Three:
                        return (GetCurrentAutoStep() >= AutoStep.Step142_From_Fork_BWD_Move && GetCurrentAutoStep() <= AutoStep.Step145_From_Complete);

                    case FromToStepNumber.Four:
                        return (GetCurrentAutoStep() >= AutoStep.Step144_From_Port_Ready_Off_Check && GetCurrentAutoStep() <= AutoStep.Step145_From_Complete);
                }
                return false;
            }
            /// <summary>
            /// CIM에게 보고 될 To 동작의 스텝 반환
            /// </summary>
            /// <param name="step"></param>
            /// <returns></returns>
            public bool IsCompleteToStep(FromToStepNumber step) {
                switch (step) {
                    case FromToStepNumber.Zero:
                        return (GetCurrentAutoStep() >= AutoStep.Step204_To_XZT_To_Move && GetCurrentAutoStep() <= AutoStep.Step220_To_Complete);

                    case FromToStepNumber.One:
                        return (GetCurrentAutoStep() >= AutoStep.Step207_To_Shelf_Port_Check && GetCurrentAutoStep() <= AutoStep.Step220_To_Complete);

                    case FromToStepNumber.Two:
                        return (GetCurrentAutoStep() >= AutoStep.Step212_To_Z_Down && GetCurrentAutoStep() <= AutoStep.Step220_To_Complete);

                    case FromToStepNumber.Three:
                        return (GetCurrentAutoStep() >= AutoStep.Step217_To_Fork_BWD_Move && GetCurrentAutoStep() <= AutoStep.Step220_To_Complete);

                    case FromToStepNumber.Four:
                        return (GetCurrentAutoStep() >= AutoStep.Step219_To_Port_Ready_Off_Check && GetCurrentAutoStep() <= AutoStep.Step220_To_Complete);
                }
                return false;
            }
            /// <summary>
            /// 현재 진행 중인 Auto Teaching Step 반환
            /// </summary>
            /// <returns></returns>
            public AutoTeachingStep GetCurrentAutoTeachingStep() {
                return m_autoTeachingStep;
            }
            /// <summary>
            /// 현재 진행 중인 Auto Teaching 중 Sensor Find Step 반환
            /// </summary>
            /// <returns></returns>
            public SensorFindStep GetCurrentSensorFindStep() {
                return m_sensorCheckStep;
            }
            /// <summary>
            /// 현재 Auto Teaching의 Sensor 데이터 반환
            /// </summary>
            /// <returns></returns>
            public AutoTeachingData GetCurrentSensorData() {
                return m_autoTeachingSensorData;
            }
            /// <summary>
            /// 매개변수의 ID에 해당하는 Port가 Pick/Place Sensor를 사용하는지 여부
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool IsPortSensorEnabled(int id) {
                return m_param.IsPortSensorEnabled(id);
            }
            /// <summary>
            /// 매개변수의 ID에 해당하는 Port Parameter 반환
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public RackMasterParam.PortParameter GetPortParameter(int id) {
                return m_param.GetPortParameter(id);
            }
            /// <summary>
            /// 랙마스터의 현재 Cycle 데이터 반환
            /// </summary>
            /// <returns></returns>
            public RackMasterCycleData GetRackMasterCycleData() {
                return m_data.GetRackMasterCycleData();
            }
        }
    }
}