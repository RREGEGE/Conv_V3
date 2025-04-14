using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using RackMaster.SEQ.COMMON;
using MovenCore;

namespace RackMaster.SEQ.PART {
    public partial class RackMasterMain {
        private class RackMasterParameterInterlock {
            public int autoSpeedPercent;
            public int maxSpeed_X;
            public int maxSpeed_Z;
            public int maxSpeed_A;
            public int maxSpeed_T;
            public int accDec;
            public int manualHighSpeed_X;
            public int manualHighSpeed_Z;
            public int manualHighSpeed_A;
            public int manualHighSpeed_T;
            public int manualLowSpeed_X;
            public int manualLowSpeed_Z;
            public int manualLowSpeed_A;
            public int manualLowSpeed_T;
            public int jogSpeed_X;
            public int jogSpeed_Z;
            public int jogSpeed_A;
            public int jogSpeed_T;
            public int jerkRatio;
            
            public RackMasterParameterInterlock(RackMasterParam _RackMasterParam) {
                autoSpeedPercent    = 100;
                // 신시누 STK spec에 맞춰 x 및 z 최대 속도 130으로 수정
                maxSpeed_X          = 130;
                maxSpeed_Z          = 130;
                // Fork 타입이 scara일 때와 slide일 때 구분하여 최대값 수정 필요
                //if (_RackMasterParam.GetMotionParam().forkType == ForkType.SCARA)
                maxSpeed_A          = 80;
                maxSpeed_T          = 8000;
                accDec              = 5;
                manualHighSpeed_X   = 100;
                manualHighSpeed_Z   = 80;
                manualHighSpeed_A   = 80;
                manualHighSpeed_T   = 8000;
                manualLowSpeed_X    = 60;
                manualLowSpeed_Z    = 40;
                manualLowSpeed_A    = 40;
                manualLowSpeed_T    = 4000;
                jogSpeed_X          = 20;
                jogSpeed_Z          = 20;
                jogSpeed_A          = 20;
                jogSpeed_T          = 3000;
                jerkRatio           = 100;
            }
        }

        private RackMaster.SEQ.CLS.Timer m_GOTTimer;
        /// <summary>
        /// 오토 조건 Interlock
        /// </summary>
        /// <returns></returns>
        public bool Interlock_AutoCondition() {
            if(IsConnected_EtherCAT() && IsConnected_Master() && m_motion.IsAllServoOn() &&
                    !m_alarm.IsAlarmState() && m_motion.GetCurerntManualStep() == ManualStep.Idle && GetReceiveBit(ReceiveBitMap.RM_Key_Auto) && !m_motion.IsManualAutoTeachingRun() && 
                    Interlock_IsSettingSuccessParamter() && Interlock_IsParameterValueCorrect() && !Interlock_GOTDetected() && Interlock_ZAxisMaintStopper() && Interlock_EnableSensor() &&
                    !GetReceiveBit(ReceiveBitMap.MST_Emo_Request) && !GetReceiveBit(ReceiveBitMap.MST_Soft_Error_State)) {
                if (!m_motion.IsAutoMotionRun()) {
                    if (!m_motion.IsAllHomeDone())
                        return false;
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// Servo On Ready Interlock
        /// </summary>
        /// <returns></returns>
        public bool Interlock_ServoOnReady() {
            return IsConnected_EtherCAT() && IsConnected_Master() && !m_alarm.IsAlarmState();
        }
        /// <summary>
        /// From Ready Interlock
        /// </summary>
        /// <returns></returns>
        public bool Interlock_FromReady() {
            return !IsCassetteOn() && !IsCassetteAbnormal() && IsAutoState() && !m_motion.IsAutoMotionRun() && !m_motion.IsAutoTeachingRun();
        }
        /// <summary>
        /// To Ready Interlock
        /// </summary>
        /// <returns></returns>
        public bool Interlock_ToReady() {
            return IsCassetteOn() && !IsCassetteAbnormal() && IsAutoState() && !m_motion.IsAutoMotionRun() && !m_motion.IsAutoTeachingRun();
        }
        /// <summary>
        /// Maint Ready Interlock
        /// </summary>
        /// <returns></returns>
        public bool Interlock_MaintReady() {
            return !IsCassetteAbnormal() && IsAutoState() && !m_motion.IsAutoMotionRun() && !m_motion.IsAutoTeachingRun() && !m_alarm.IsAlarmState() &&
                m_motion.IsAllServoOn() && m_motion.IsAllHomeDone();
        }
        /// <summary>
        /// Teaching Read/Write Ready Interlock
        /// </summary>
        /// <returns></returns>
        public bool Interlock_TeachingRWReady() {
            return IsAutoState() && !GetReceiveBit(ReceiveBitMap.RM_Teaching_Read_Write_Start);
        }
        /// <summary>
        /// Auto Teaching Start Ready Interlock
        /// </summary>
        /// <returns></returns>
        public bool Interlock_AutoTeachingStartReady() {
            return (m_motion.IsAutoMotionRun() || m_motion.IsAutoTeachingRun() || m_alarm.IsAlarmState() || !IsAutoState()) ? false : true;
        }
        /// <summary>
        /// Port나 Shelf가 준비 되었는 지에 대한 Interlock
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Interlock_PortOrShelfReady(int id) {
            if (m_teaching.IsEnablePort(id) && m_teaching.IsExistPortOrShelf(id) && m_teaching.IsExistTeachingData(id))
                return true;
            return false;
        }
        /// <summary>
        /// 현재 발생된 알람이 오직 Master와의 통신 알람인지 반환
        /// </summary>
        /// <returns></returns>
        public bool Interlock_OnlyMSTDisconnectedAlarmOccurred() {
            return m_alarm.GetCurrentAlarmCount() == 1 && m_alarm.IsCurrentAlarmContainAt(AlarmList.MST_DisConnected);
        }
        /// <summary>
        /// Full Closed가 Abnormal 상태인지
        /// </summary>
        /// <returns></returns>
        public bool Interlock_FullClosedAbnormal() {
            return m_fullClosedAbnormal;
        }
        /// <summary>
        /// Full Closed Abnormal 상태 감지를 위한 업데이트
        /// </summary>
        private void Interlock_FullClosedAbnormalUpdate() {
            Interlock_DistanceDetectSensorError();
            Interlock_DistanceDetectSensorPositionError();
            Interlock_DistanceDetectSensorVelocityError();
            Interlock_FullClosedStatusError();
            Interlock_FullClosedSettingValueError();
        }
        /// <summary>
        /// 거리 감지 센서에 에러가 발생했는지
        /// </summary>
        private void Interlock_DistanceDetectSensorError() {
            if(m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.HardwareError) ||
                m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Intensity) ||
                m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Laser) ||
                m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.LaserStatus) ||
                m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Plausibility) ||
                m_motion.GetDetecSensor_PositionStastus(DistanceDetectSensorPositionStatus.Temperature)) {
                if (!m_fullClosedAbnormal) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Distance Detect Sensor Position Status Error!"));
                }
                m_fullClosedAbnormal = true;
            }

            if (m_motion.GetDetectSensor_VelocityStatus(DistanceDetectSensorVelocityStatus.MeasurementError)) {
                if(!m_fullClosedAbnormal) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Distance Detect Sensor Velocity Status Error!"));
                }
                m_fullClosedAbnormal = true;
            }
        }
        /// <summary>
        /// 거리 감지 센서에서 Position 데이터가 너무 작을 경우
        /// </summary>
        private void Interlock_DistanceDetectSensorPositionError() {
            if(m_motion.GetDetectSensor_PositionlValue() < 5000) {
                if (!m_fullClosedAbnormal) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "Distance Detect Sensor Position Value Error! 5000"));
                }
                m_fullClosedAbnormal = true;
            }
        }
        /// <summary>
        /// 거리 감지 센서가 제공하는 Velocity 값이 너무 클 경우 에러
        /// </summary>
        private void Interlock_DistanceDetectSensorVelocityError() {
            //double speedLimit = (m_param.GetAxisParameter(AxisList.X_Axis).maxSpeed / 60) + 100;

            //if (m_motion.GetDetectSensor_VelocityValue() >= speedLimit) {
            //    m_fullClosedAbnormal = true;
            //}

            //if(m_motion.GetAxisStatus(AxisStatusType.vel_cmd, AxisList.X_Axis) > (speedLimit * 1000)) {
            //    m_fullClosedAbnormal = true;
            //}
        }
        /// <summary>
        /// User Rtdll에서 제공하는 Alarm 상태 확인
        /// </summary>
        private void Interlock_FullClosedStatusError() {
            if (m_motion.GetFullClosedStatus().m_fbStatus.m_op == MovenCore.BarcodeOpState.ALARM) {
                if (!m_fullClosedAbnormal) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "FullClosed Alarm!"));
                }
                m_fullClosedAbnormal = true;
            }

            if (m_motion.GetFullClosedStatus().m_fbStatus.m_actualDistance < 1000) {
                if (!m_fullClosedAbnormal) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, "FullClosed Actual Distance Error!"));
                }
                m_fullClosedAbnormal = true;
            }
        }
        /// <summary>
        /// Full Closed 세팅 파라미터가 잘못되었는지 확인
        /// </summary>
        private void Interlock_FullClosedSettingValueError() {
            if (!m_fullClosedTimer.IsTimerStarted()) {
                m_fullClosedTimer.Reset();
                m_fullClosedTimer.Start();
            }

            if (m_fullClosedTimer.Delay(10000)) {
                byte[] settingData = new byte[1];
                uint actSize = 0;
                uint errorCode = 0;

                if (!m_fullClosedAbnormal) {
                    for (int i = 0; i < WMX3.GetSlaveCount(); i++) {
                        if (WMX3.GetSlaveInformation(i) == null) {
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, $"Get Slave Information Fail"));
                        }
                        else {
                            if (WMX3.GetSlaveInformation(i).vendorId == m_fullClosedVendorID &&
                            WMX3.GetSlaveInformation(i).productCode == m_fullClosedProductCode) {
                                uint errCode = 0;

                                int ret = WMX3.SDOUploadExpedited(WMX3.GetSlaveInformation(i).id, m_fullClosedIndex, m_fullClosedSubIndex, ref settingData, ref actSize, ref errCode);

                                if (ret == WMXParam.ErrorCode_None) {
                                    if (settingData[0] != m_fullClosedValue[0]) {
                                        m_fullClosedAbnormal = true;
                                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, $"Full Closed Setting Value Error"));
                                        FullClosedSetting();
                                        break;
                                    }
                                }
                                else {
                                    Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.WMX, $"{WMX3.ErrorCodeToString(ret)}"));
                                }
                            }
                        }
                    }
                    m_fullClosedTimer.Stop();
                    m_fullClosedTimer.Reset();
                }
            }
        }
        /// <summary>
        /// True = 성공, False = 실패
        /// 현재 설정된 Parameter가 정상적인지 판단
        /// </summary>
        /// <returns></returns>
        public bool Interlock_IsParameterValueCorrect() {
            if (!Interlock_ParameterAxisNumber())
                return false;

            foreach(AxisList axis in Enum.GetValues(typeof(AxisList))) {
                if (!Interlock_AutoSpeedPercentLimit(axis))
                    return false;
                if (!Interlock_MaxSpeedLimit(axis))
                    return false;
                if (!Interlock_ManualHighSpeedLimit(axis))
                    return false;
                if (!Interlock_ManualLowSpeedLimit(axis))
                    return false;
                if (!Interlock_JogHighSpeedLimit(axis))
                    return false;
                if (!Interlock_JogLowSpeedLimit(axis))
                    return false;
                if (!Interlock_JogHighSpeedLimit(axis))
                    return false;
                if (!Interlock_MaxAccDecLimit(axis))
                    return false;
                if (!Interlock_ManualHighAccDecLimit(axis))
                    return false;
                if (!Interlock_ManualLowAccDecLimit(axis))
                    return false;
                if (!Interlock_QuickStopDecelLimit(axis))
                    return false;
                if (!Interlock_NormalStopDecelLimit(axis))
                    return false;
                if (!Interlock_SlowStopDecelLimit(axis))
                    return false;
                if (!Interlock_JerkRatioLimit(axis))
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 파라미터 Load or Save가 성공했는지 판단
        /// </summary>
        /// <returns></returns>
        public bool Interlock_IsSettingSuccessParamter() {
            if (!m_param.IsSettingSuccess_Axis())
                return false;
            if (!m_param.IsSettingSuccess_WMX())
                return false;
            if (!m_param.IsSettingSuccess_RM())
                return false;
            if (!m_param.IsSettingSuccess_IO())
                return false;
            if (!m_param.IsSettingSuccess_Port())
                return false;

            return true;
        }
        /// <summary>
        /// 만약 X,Z,A,T 축 번호 설정이 같은 경우 에러
        /// </summary>
        /// <returns></returns>
        public bool Interlock_ParameterAxisNumber() {
            if (m_param.GetAxisParameter(AxisList.X_Axis).axisNumber == m_param.GetAxisParameter(AxisList.Z_Axis).axisNumber)
                return false;

            if (m_param.GetAxisParameter(AxisList.X_Axis).axisNumber == m_param.GetAxisParameter(AxisList.A_Axis).axisNumber)
                return false;

            if (m_param.GetAxisParameter(AxisList.X_Axis).axisNumber == m_param.GetAxisParameter(AxisList.T_Axis).axisNumber)
                return false;

            if (m_param.GetAxisParameter(AxisList.Z_Axis).axisNumber == m_param.GetAxisParameter(AxisList.A_Axis).axisNumber)
                return false;

            if (m_param.GetAxisParameter(AxisList.Z_Axis).axisNumber == m_param.GetAxisParameter(AxisList.T_Axis).axisNumber)
                return false;

            if (m_param.GetAxisParameter(AxisList.A_Axis).axisNumber == m_param.GetAxisParameter(AxisList.T_Axis).axisNumber)
                return false;

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Max Speed 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_MaxSpeedLimit(AxisList axis) {
            if(axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).maxSpeed > m_interlockParam.maxSpeed_X)
                    return false;
            }else if(axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).maxSpeed > m_interlockParam.maxSpeed_Z)
                    return false;
            }else if(axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).maxSpeed > (m_param.GetMotionParam().forkType == ForkType.SCARA ? m_interlockParam.maxSpeed_T : m_interlockParam.maxSpeed_A))
                    return false;
            }else if(axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).maxSpeed > m_interlockParam.maxSpeed_T)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Manual High Speed 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_ManualHighSpeedLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighSpeed > m_interlockParam.manualHighSpeed_X)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighSpeed > m_interlockParam.manualHighSpeed_Z)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighSpeed > (m_param.GetMotionParam().forkType == ForkType.SCARA ? m_interlockParam.manualHighSpeed_T : m_interlockParam.manualHighSpeed_A))
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighSpeed > m_interlockParam.manualHighSpeed_T)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Manual Low Speed 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_ManualLowSpeedLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowSpeed > m_interlockParam.manualLowSpeed_X)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowSpeed > m_interlockParam.manualLowSpeed_Z)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowSpeed > (m_param.GetMotionParam().forkType == ForkType.SCARA ? m_interlockParam.manualLowSpeed_T : m_interlockParam.manualLowSpeed_A))
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowSpeed > m_interlockParam.manualLowSpeed_T)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Jog High Speed Limit 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_JogHighSpeedLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).jogHighSpeedLimit > m_interlockParam.jogSpeed_X)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).jogHighSpeedLimit > m_interlockParam.jogSpeed_Z)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).jogHighSpeedLimit > (m_param.GetMotionParam().forkType == ForkType.SCARA ? m_interlockParam.jogSpeed_T : m_interlockParam.jogSpeed_A))
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).jogHighSpeedLimit > m_interlockParam.jogSpeed_T)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Jog Low Speed Limit 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_JogLowSpeedLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).jogLowSpeedLimit > m_interlockParam.jogSpeed_X)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).jogLowSpeedLimit > m_interlockParam.jogSpeed_Z)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).jogLowSpeedLimit > (m_param.GetMotionParam().forkType == ForkType.SCARA ? m_interlockParam.jogSpeed_T : m_interlockParam.jogSpeed_A))
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).jogLowSpeedLimit > m_interlockParam.jogSpeed_T)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Max Acc/Dec 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_MaxAccDecLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).maxAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).maxAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).maxAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).maxAccDec > m_interlockParam.accDec)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Manual High Acc Dec 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_ManualHighAccDecLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).manualHighAccDec > m_interlockParam.accDec)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Manual Low Acc/Dec 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_ManualLowAccDecLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowAccDec > m_interlockParam.accDec)
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).manualLowAccDec > m_interlockParam.accDec)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Auto Speed Percent 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_AutoSpeedPercentLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).autoSpeedPercent > m_interlockParam.autoSpeedPercent)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).autoSpeedPercent > m_interlockParam.autoSpeedPercent)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).autoSpeedPercent > m_interlockParam.autoSpeedPercent)
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).autoSpeedPercent > m_interlockParam.autoSpeedPercent)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Quick Stop Dec 설정이 Max Acc/Dec 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_QuickStopDecelLimit(AxisList axis) {
            if (m_param.GetAxisParameter(axis).quickStop > m_param.GetAxisParameter(axis).maxAccDec)
                return false;

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Normal Stop Dec 설정이 Max Acc/Dec 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_NormalStopDecelLimit(AxisList axis) {
            if (m_param.GetAxisParameter(axis).normalStop > m_param.GetAxisParameter(axis).maxAccDec)
                return false;

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Slow Stop Dec 설정이 Max Acc/Dec 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_SlowStopDecelLimit(AxisList axis) {
            if (m_param.GetAxisParameter(axis).slowStop > m_param.GetAxisParameter(axis).maxAccDec)
                return false;

            return true;
        }
        /// <summary>
        /// 각 축에 대한 Jerk Ratio 설정이 Interlock 클래스 내의 값보다 클 경우 에러
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public bool Interlock_JerkRatioLimit(AxisList axis) {
            if (axis == AxisList.X_Axis) {
                if (m_param.GetAxisParameter(axis).jerkRatio > m_interlockParam.jerkRatio)
                    return false;
            }
            else if (axis == AxisList.Z_Axis) {
                if (m_param.GetAxisParameter(axis).jerkRatio > m_interlockParam.jerkRatio)
                    return false;
            }
            else if (axis == AxisList.A_Axis) {
                if (m_param.GetAxisParameter(axis).jerkRatio > m_interlockParam.jerkRatio)
                    return false;
            }
            else if (axis == AxisList.T_Axis) {
                if (m_param.GetAxisParameter(axis).jerkRatio > m_interlockParam.jerkRatio)
                    return false;
            }

            return true;
        }
        /// <summary>
        /// GOT Emo 신호가 들어왔는지 판단
        /// </summary>
        /// <returns></returns>
        public bool Interlock_GOTEmoOn() {
            if (IsUseGOT()) {
                if (IsGOTDetected_HP() || IsGOTDetected_OP()) {
                    if (!m_GOTTimer.IsTimerStarted())
                        m_GOTTimer.Restart();

                    if (m_GOTTimer.Delay(500)) {
                        if (IsGOTDetected_HP()) {
                            if (GetInputBit(InputList.HP_DTP_EMS_SW))
                                return true;
                        }

                        if (IsGOTDetected_OP()) {
                            if (GetInputBit(InputList.OP_DTP_EMS_SW))
                                return true;
                        }
                    }
                }
                else {
                    m_GOTTimer.Stop();
                }
            }
            else {
                m_GOTTimer.Stop();
            }

            return false;
        }
        /// <summary>
        /// GOT 신호가 감지되었는지
        /// </summary>
        /// <returns></returns>
        public bool Interlock_GOTDetected() {
            if (IsUseGOT()) {
                if (IsGOTDetected_HP() || IsGOTDetected_OP())
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Z축 Maint Stopper 설정이 되었는지
        /// </summary>
        /// <returns></returns>
        public bool Interlock_ZAxisMaintStopper() {
            if (IsInputEnabled(InputList.Z_Axis_Maint_Stopper_Sensor_1) && !GetInputBit(InputList.Z_Axis_Maint_Stopper_Sensor_1))
                return false;
            if (IsInputEnabled(InputList.Z_Axis_Maint_Stopper_Sensor_2) && !GetInputBit(InputList.Z_Axis_Maint_Stopper_Sensor_2))
                return false;

            return true;
        }
        /// <summary>
        /// 필수 Sensor에 한해서 설정이 되었는지
        /// </summary>
        /// <returns></returns>
        public bool Interlock_EnableSensor() {
            if (GetCurrentSettingMode() == SettingMode.Demo)
                return true;

            if (!Interlock_EnableSensor_EMO())
                return false;
            //if (!Interlock_EnableSensor_ZAxisMaintStopper())
            //    return false;
            if (!Interlock_EnableSensor_Presense())
                return false;
            if (!Interlock_EnableSensor_DoubleStorage())
                return false;
            if (!Interlock_EnableSensor_Pick())
                return false;
            if (!Interlock_EnableSensor_Place())
                return false;
            if (!Interlock_EnableSensor_Placement())
                return false;
            //if (!Interlock_EnableSensor_Stick())
            //    return false;

            return true;
        }
        /// <summary>
        /// EMO Sensor Enable 상태 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_EMO() {
            return IsInputEnabled(InputList.EMO_HP) && IsInputEnabled(InputList.EMO_OP);
        }
        /// <summary>
        /// Z Axis Maint Stopper Sensor Enable 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_ZAxisMaintStopper() {
            return IsInputEnabled(InputList.Z_Axis_Maint_Stopper_Sensor_1) || IsInputEnabled(InputList.Z_Axis_Maint_Stopper_Sensor_2);
        }
        /// <summary>
        /// 대각 감지 센서 Enable 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_Presense() {
            return IsInputEnabled(InputList.Presense_Detection_1) || IsInputEnabled(InputList.Presense_Detection_2);
        }
        /// <summary>
        /// Double Storage Sensor Enable 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_DoubleStorage() {
            return IsInputEnabled(InputList.Double_Storage_Sensor_1) || IsInputEnabled(InputList.Double_Storage_Sensor_2);
        }
        /// <summary>
        /// Pick Sensor Enable 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_Pick() {
            return IsInputEnabled(InputList.Fork_Pick_Sensor_Left) || IsInputEnabled(InputList.Fork_Pick_Sensor_Right);
        }
        /// <summary>
        /// Place Sensor Enable 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_Place() {
            return IsInputEnabled(InputList.Fork_Place_Sensor_Left) || IsInputEnabled(InputList.Fork_Place_Sensor_Right);
        }
        /// <summary>
        /// 안착 감지 센서 Enable 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_Placement() {
            return IsInputEnabled(InputList.Fork_In_Place_1) || IsInputEnabled(InputList.Fork_In_Place_2);
        }
        /// <summary>
        /// Stick Sensor Enable 확인
        /// </summary>
        /// <returns></returns>
        private bool Interlock_EnableSensor_Stick() {
            return IsInputEnabled(InputList.StickDetectSensor_1) || IsInputEnabled(InputList.StickDetectSensor_2) || IsInputEnabled(InputList.StickDetectSensor_3)
                || IsInputEnabled(InputList.StickDetectSensor_4);
        }
    }
}
