using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Master.Interface.Alarm;
using Master.ManagedFile;

namespace Master.Equipment.Port
{
    /// <summary>
    /// PortUI.cs 는 Form에서 사용되는 UI 아이템의 색상, 변경 등 UI Item 반응에 대한 작성 영역
    /// </summary>
    /// 
    public partial class Port
    {
        /// <summary>
        /// Port IO TabPage의 종류, 순서 정의
        /// </summary>
        public enum Port_IO_TabPage
        {
            TwoBuffer,
            OneBuffer,
            Conveyor,
            Shuttle_X,
            Shuttle_Z,
            Shuttle_T,
            LP_Buffer_Z,
            OP_Buffer_Z,
            OP_Buffer_Y,
            OHT,
            AGV,
            EQ,
            OMRON
        }

        /// <summary>
        /// Port Monitoring 영역에 표시되는 Error Info 행
        /// </summary>
        enum DGV_ErrorInfoRow
        {
            ErrorCode0,
            ErrorCode1,
            ErrorCode2,
            ErrorCode3,
            ErrorCode4
        }

        /// <summary>
        /// Port Monitoring 영역에 표시되는 Error Info 열
        /// </summary>
        enum DGV_ErrorInfoColumn
        {
            ErrorGenTime,
            ErrorIndex,
            ErrorCode,
            ClearState,
            ErrorClearTime
        }

        /// <summary>
        /// Port Monitoring 영역에 표시되는 Status Info 행
        /// </summary>
        enum DGV_PortStatusInfoRowList
        {
            TagReaderType,
            TagValue,
            BufferType,
            ControlDirection,
            AutoRunStatus,
            CycleRunStatus,
            OPStep,
            BPStep,
            LPStep,
            PortBusy,
            Shuttle_X_Axis,
            Shuttle_Z_Axis,
            Shuttle_T_Axis,
            Buffer_LP_X_Axis,
            Buffer_LP_Z_Axis,
            Buffer_LP_T_Axis,
            Buffer_OP_X_Axis,
            Buffer_OP_Z_Axis,
            Buffer_OP_T_Axis,
            Buffer_LP_CV,
            Buffer_OP_CV,
            Buffer_BP1_CV,
            Buffer_BP2_CV,
            Buffer_BP3_CV,
            Buffer_BP4_CV,
            EQ_PIO_Load_REQ,
            EQ_PIO_Unload_REQ,
            EQ_PIO_Ready
        }

        /// <summary>
        /// Port Monitoring 영역에 표시되는 Interlock Info 행
        /// </summary>
        enum DGV_InterlockInfoRowList
        {
            AutoRunEnable,
            TagReaderConnection,
            PortPower,
            Shuttle_X_AxisHomeDone,
            Shuttle_Z_AxisHomeDone,
            Shuttle_T_AxisHomeDone,
            Buffer_LP_X_AxisHomeDone,
            Buffer_LP_Z_AxisHomeDone,
            Buffer_LP_T_AxisHomeDone,
            Buffer_OP_X_AxisHomeDone,
            Buffer_OP_Z_AxisHomeDone,
            Buffer_OP_T_AxisHomeDone,
            OP_Carrier_Status,
            Shuttle_Carrier_Status,
            CV_BP1_Carrier_Status,
            CV_BP2_Carrier_Status,
            CV_BP3_Carrier_Status,
            CV_BP4_Carrier_Status,
            LP_Carrier_Status,
            OP_Carrier_ID,
            Shuttle_Carrier_ID,
            CV_BP1_Carrier_ID,
            CV_BP2_Carrier_ID,
            CV_BP3_Carrier_ID,
            CV_BP4_Carrier_ID,
            LP_Carrier_ID,
            RM_Carrier_ID,
            LightCurtain,
            ForkDetect,
            HoistDetect
        }

        /// <summary>
        /// Port Main Form 영역에 표시되는 Auto Run 상태 정보 행
        /// </summary>
        enum DGV_AutoRunStatusRowList
        {
            AutoRunType,
            RunningStatus,
            CycleRunProgress,
            RunningTime,
            ErrorName,
            OP_Step,
            BP_Step,
            LP_Step,
            OP_Carrier_Status,
            Shuttle_Carrier_Status,
            CV_BP1_Carrier_Status,
            CV_BP2_Carrier_Status,
            CV_BP3_Carrier_Status,
            CV_BP4_Carrier_Status,
            LP_Carrier_Status,
            OP_Carrier_ID,
            Shuttle_Carrier_ID,
            CV_BP1_Carrier_ID,
            CV_BP2_Carrier_ID,
            CV_BP3_Carrier_ID,
            CV_BP4_Carrier_ID,
            LP_Carrier_ID,
            RM_Carrier_ID,
            Port_To_OMRON_Carrier_ID,
            OMRON_To_Port_Carrier_ID,
            OP_Step_Timer,
            BP_Step_Timer,
            LP_Step_Timer,
            EQ_Step_Timer,
            RM_PIO_Timer,
            Equip_PIO_Timer,
            RM_Fork_Detect_Timer,
            OHT_Hoist_Detect_Timer,
            Port_Area_Timer,
            Port_Area_Release_Timer,
            Port_Area_And_ShuttleMoveTimer
        }

        /// <summary>
        /// Port 상태 Label에 표시되는 Text Type
        /// </summary>
        public enum PortInfoType
        {
            Simple,
            Normal,
            Detail,
            Map
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 서보 축 관련 정보 열 
        /// </summary>
        public enum DGV_MotionServoStatusColumn
        {
            Axis,
            Busy,
            Servo,
            Home,
            PosCommand,
            ActualPos,
            ActualVel,
            ActualTorque,
            AlarmCode
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 인버터 축 관련 정보 열 
        /// </summary>
        public enum DGV_MotionInverterStatusColumn
        {
            Axis,
            Busy,
            HighSpeedBWD,
            LowSpeedBWD,
            LowSpeedFWD,
            HighSpeedFWD,
            HighSpeed,
            LowSpeed,
            FWD,
            BWD
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 실린더 축 관련 정보 열 
        /// </summary>
        public enum DGV_MotionCylinderStatusColumn
        {
            Axis,
            Busy,
            BWDStatus,
            FWDStatus,
            BWD1,
            FWD1
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 컨베이어 관련 정보 열 
        /// </summary>
        public enum DGV_MotionConveyorStatusColumn
        {
            Axis,
            Busy,
            HighSpeedBWD,
            LowSpeedBWD,
            LowSpeedFWD,
            HighSpeedFWD,
            HighSpeed,
            LowSpeed,
            FWD,
            BWD
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 컨베이어 옵션 관련 정보 열 
        /// </summary>
        public enum DGV_MotionConveyorOptionCylinderStatusColumn
        {
            Axis,
            Busy,
            BWD,
            FWD
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류
        /// </summary>
        public enum DGV_PortSensorStatusRow
        {
            Shuttle_X_Axis,
            Shuttle_Z_Axis,
            Shuttle_T_Axis,
            Buffer1_OP_Status1,
            Buffer2_LP_Status1,
            Shuttle_Status,
            Buffer1_OP_Status2,
            Buffer2_LP_Status2,
            CVType_BP_CST_Status
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 셔틀 X축 센서 정보 열
        /// </summary>
        enum DGV_Shuttle_X_Axis_SensorStatusColumn
        {
            Type,
            NOT,
            POT,
            HOME,
            Pos,
            Busy,
            OriginOK,
            WaitPos
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 셔틀 Z축 센서 정보 열
        /// </summary>
        enum DGV_Shuttle_Z_Axis_SensorStatusColumn
        {
            Type,
            NOT,
            POT,
            HOME,
            Pos,
            Busy,
            OriginOK,
            BWD,
            FWD
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 셔틀 T축 센서 정보 열
        /// </summary>
        enum DGV_Shuttle_T_Axis_SensorStatusColumn
        {
            Type,
            NOT,
            POT,
            HOME,
            Pos,
            Busy,
            OriginOK,
            Deg_0,
            Deg_180
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 LP 영역 감지 관련 센서 정보 열
        /// </summary>
        enum DGV_Buffer1_LP_SensorStatus1Column
        {
            Type,
            CST_Detect1,
            CST_Detect2,
            CST_Presence,
            Hoist_Detect,
            Cart_Detect1,
            Cart_Detect2,
            LightCurtain,
            OHTDoorOpen,
            LED_Green,
            LED_Red
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 LP 영역 컨베이어 관련 센서 정보 열
        /// </summary>
        enum DGV_Buffer2_LP_SensorStatus2Column
        {
            Type,
            Buffer_CV_IN,
            Buffer_CV_OUT,
            Buffer_CV_Error,

            Buffer_Z_Axis_NOT,
            Buffer_Z_Axis_POS1,
            Buffer_Z_Axis_POS2,
            Buffer_Z_Axis_POT,
            Buffer_Z_Axis_Error,

            Buffer_CV_Forwarding,
            Buffer_CV_Backwarding,
            Buffer_CV_CST_Opposite,
            Buffer_CV_CST_Inplace1,
            Buffer_CV_CST_Inplace2
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 OP 영역 감지 관련 센서 정보 열
        /// </summary>
        enum DGV_Buffer2_OP_SensorStatus1Column
        {
            Type,
            CST_Detect1,
            CST_Detect2,
            CST_Presence,
            Fork_Detect
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 OP 영역 컨베이어 관련 센서 정보 열
        /// </summary>
        enum DGV_Buffer1_OP_SensorStatus2Column
        {
            Type,
            Buffer_CV_IN,
            Buffer_CV_OUT,
            Buffer_CV_Error,

            Buffer_Z_Axis_NOT,
            Buffer_Z_Axis_POS1,
            Buffer_Z_Axis_POS2,
            Buffer_Z_Axis_POT,
            Buffer_Z_Axis_Error,

            Buffer_CV_Forwarding,
            Buffer_CV_Backwarding,
            Buffer_CV_CST_Opposite,
            Buffer_CV_CST_Inplace1,
            Buffer_CV_CST_Inplace2,
            Buffer_CV_RM_ForkDetect
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 센서 상태 정보 종류 중 셔틀 영역 관련 센서 정보 열
        /// </summary>
        enum DGV_Shuttle_CST_SensorStatusColumn
        {
            Type,
            CST_Detect1,
            CST_Detect2
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 AGV, OHT 등 장비와의 PIO 정보 열
        /// </summary>
        enum DGV_Buffer1PIOStatusColumn
        {
            Equip_To_Port,
            Port_To_Equip
        }

        /// <summary>
        /// Port Status Form 영역에 표시되는 STK와의 PIO 정보 열
        /// </summary>
        enum DGV_Buffer2PIOStatusColumn
        {
            RM_To_Port,
            Port_To_RM
        }

        /// <summary>
        /// Port Teaching Form 영역에 표시되는 Teaching 값 저장 정보 열
        /// </summary>
        public enum DGV_TeachingParamColumn
        {
            Name,
            AppliedValue,
            SetValue,
            Btn
        }

        /// <summary>
        /// Port Teaching Form 영역에 표시되는 X축 Teaching 관련 값 행
        /// </summary>
        enum DGV_TeachingParam_X_AxisRow
        {
            X_OP_Position,
            X_Wait_Position,
            X_MGV_LP_Position,
            X_Equip_LP_Position,
            X_Move_Manual_Speed,
            X_Move_Manual_Acc,
            X_Move_Manual_Dec,
            X_Move_AutoRun_Speed,
            X_Move_AutoRun_Acc,
            X_Move_AutoRun_Dec
        }

        /// <summary>
        /// Port Teaching Form 영역에 표시되는 Y축 Teaching 관련 값 행
        /// </summary>
        enum DGV_TeachingParam_Y_AxisRow
        {
            Y_FWD_Position,
            Y_BWD_Position,
            Y_Move_Manual_Speed,
            Y_Move_Manual_Acc,
            Y_Move_Manual_Dec,
            Y_Move_AutoRun_Speed,
            Y_Move_AutoRun_Acc,
            Y_Move_AutoRun_Dec
        }

        /// <summary>
        /// Port Teaching Form 영역에 표시되는 Z축 Teaching 관련 값 행
        /// </summary>
        enum DGV_TeachingParam_Z_AxisRow
        {
            Z_Up_Position,
            Z_Down_Position,
            Z_OverrideDistance,
            Z_OverrideDecPercent,
            Z_Move_Manual_Speed,
            Z_Move_Manual_Acc,
            Z_Move_Manual_Dec,
            Z_Move_AutoRun_Speed,
            Z_Move_AutoRun_Acc,
            Z_Move_AutoRun_Dec
        }

        /// <summary>
        /// Port Teaching Form 영역에 표시되는 T축 Teaching 관련 값 행
        /// </summary>
        enum DGV_TeachingParam_T_AxisRow
        {
            T_0_Degree_Position,
            T_180_Degree_Position,
            T_Move_Manual_Speed,
            T_Move_Manual_Acc,
            T_Move_Manual_Dec,
            T_Move_AutoRun_Speed,
            T_Move_AutoRun_Acc,
            T_Move_AutoRun_Dec
        }

        /// <summary>
        /// Port Setting Form 영역에 표시되는 Watchdog 정보 열
        /// </summary>
        public enum DGV_WatchdogSettingsColumn
        {
            Name,
            AppliedValue,
            SetValue,
            Btn,
            ProgressTime
        }

        /// <summary>
        /// Port PIO에 Port -> OHT PIO 정보 행
        /// </summary>
        public enum DGV_PortToOHTPIORow
        {
            Load_REQ,
            Unload_REQ,
            Ready,
            ES,
            HO_AVBL
        }

        /// <summary>
        /// Port PIO에 Port -> OMRON PIO 정보 행
        /// </summary>
        public enum DGV_PortToOMRONPIORow
        {
            TR_REQ,
            Busy,
            Complete,
            Auto,
            Error
        }

        /// <summary>
        /// Port PIO에 OHT -> Port PIO 정보 행
        /// </summary>
        public enum DGV_OHTToPortPIORow
        {
            Valid,
            CS0,
            TR_REQ,
            Busy,
            Complete
        }

        /// <summary>
        /// Port PIO에 OMRON -> Port PIO 정보 행
        /// </summary>
        public enum DGV_OMRONToPortPIORow
        {
            Load_REQ,
            Unload_REQ,
            Ready,
            Auto,
            Error
        }

        /// <summary>
        /// Port PIO에 Port -> STK PIO 정보 행
        /// </summary>
        public enum DGV_PortToRMPIORow
        {
            Load_REQ,
            Unload_REQ,
            Ready
        }

        /// <summary>
        /// Port PIO에 STK -> Port PIO 정보 행
        /// </summary>
        public enum DGV_RMToPortPIORow
        {
            TR_REQ,
            Busy,
            Complete,
            STK_Error
        }

        /// <summary>
        /// Port Setting 중 Servo 축 관련 모션 변수 행
        /// </summary>
        public enum ServoParamRow
        {
            Servo_AxisNum,
            Servo_WaitPosEnable,
            Servo_TeachingPos0,
            Servo_TeachingPos0_Check,
            Servo_TeachingPos1,
            Servo_TeachingPos1_Check,
            Servo_TeachingPos2,
            Servo_TeachingPos2_Check,
            Servo_TeachingPos3,
            Servo_TeachingPos3_Check,
            Servo_OverrideDistance,
            Servo_OverrideDecPercent,
            Servo_Manual_Speed,
            Servo_Manual_Acc,
            Servo_Manual_Dec,
            Servo_AutoRun_Speed,
            Servo_AutoRun_Acc,
            Servo_AutoRun_Dec,
            Servo_MaxLoad,
            Servo_CrashCheck_ID,
            Servo_ManualPath,
        }

        /// <summary>
        /// Port Setting 중 인버터 축 관련 모션 변수 행
        /// </summary>
        public enum InverterParamRow
        {
            Inv_CtrlType,
            Inv_HighSpeed,
            Inv_LowSpeed,
            Inv_FWD,
            Inv_BWD,
            Inv_HzStartAddr,
            Inv_HzTarget
        }

        /// <summary>
        /// Port Setting 중 실린더 축 관련 모션 변수 행
        /// </summary>
        public enum CylinderParamRow
        {
            Cyl_FWD1_Ctrl,
            Cyl_FWD1_PosSensor,
            Cyl_BWD1_Ctrl,
            Cyl_BWD1_PosSensor,
        }

        /// <summary>
        /// Port Setting 중 컨베이어 관련 모션 변수 행
        /// </summary>
        public enum ConveyorParamRow
        {
            CV_SlowSensor_Enable,
            CV_Stopper_Enable,
            CV_Stopper_FWD,
            CV_Stopper_BWD,
            CV_Centering_Enable,
            CV_Centering_FWD,
            CV_Centering_BWD,
            CV_CtrlType,
            CV_HighSpeed,
            CV_LowSpeed,
            CV_FWD,
            CV_BWD,
            CV_HzStartAddr,
            CV_HzTarget,
            CV_CST_Detect_Enable,
            CV_CST_Detect,
        }

        /// <summary>
        /// Port IO Form 중 버퍼 Sensor 정보 출력 행
        /// </summary>
        public enum DGV_BufferSensorRow
        {
            //32개 제한
            LP_CST_Detect1,
            LP_CST_Detect2,
            LP_CST_Presence,
            LP_Hoist_Detect,
            LP_Cart_Detect1,
            LP_Cart_Detect2,
            LP_LED_Green,
            LP_LED_Red,
            OP_CST_Detect1,
            OP_CST_Detect2,
            OP_CST_Presence,
            OP_Fork_Detect,
            Shuttle_CST_Detect1,
            Shuttle_CST_Detect2,
            LP_CV_In,
            LP_CV_Out,
            LP_CV_Forwording,
            LP_CV_Backwording,
            OP_CV_In,
            OP_CV_Out,
            OP_CV_Forwording,
            OP_CV_Backwording,
            BP1_CST_Detect,
            BP2_CST_Detect,
            BP3_CST_Detect,
            BP4_CST_Detect
        }

        /// <summary>
        /// Port IO Form 중 X축 센서 정보 출력 행
        /// </summary>
        public enum DGV_ShuttleXAxisSensorRow
        {
            NOT,
            POT,
            HOME,
            Pos,
            Busy,
            OriginOK,
            WaitPosSensor
        }

        /// <summary>
        /// Port IO Form 중 Z축 센서 정보 출력 행
        /// </summary>
        public enum DGV_ShuttleZAxisSensorRow
        {
            NOT,
            POT,
            HOME,
            Pos,
            Busy,
            OriginOK,
            Cylinder_BWD_Pos,
            Cylinder_FWD_Pos,
            BWD_Command,
            FWD_Command
        }

        /// <summary>
        /// Port IO Form 중 T축 센서 정보 출력 행
        /// </summary>
        public enum DGV_ShuttleTAxisSensorRow
        {
            NOT,
            POT,
            HOME,
            Pos,
            Busy,
            OriginOK,
            Degree_0_Position,
            Degree_180_Position
        }

        /// <summary>
        /// Port IO Form 중 LP Buffer Z축 센서 정보 출력 행
        /// </summary>
        public enum DGV_BufferLP_ZAxisSensorRow
        {
            NOT,
            Pos1,
            Pos2,
            POT,
            Busy,
            HighSpeed,
            LowSpeed,
            FWD,
            BWD,
            HighSpeedFWDFlag,
            LowSpeedFWDFlag,
            HighSpeedBWDFlag,
            LowSpeedBWDFlag,
            Cylinder_BWD_Pos,
            Cylinder_FWD_Pos,
            BWD_Command,
            FWD_Command
        }

        /// <summary>
        /// Port IO Form 중 OP Buffer Z축 센서 정보 출력 행
        /// </summary>
        public enum DGV_BufferOP_ZAxisSensorRow
        {
            NOT,
            Pos1,
            Pos2,
            POT,
            Busy,
            HighSpeed,
            LowSpeed,
            FWD,
            BWD,
            HighSpeedFWDFlag,
            LowSpeedFWDFlag,
            HighSpeedBWDFlag,
            LowSpeedBWDFlag,
            Cylinder_BWD_Pos,
            Cylinder_FWD_Pos,
            BWD_Command,
            FWD_Command
        }

        /// <summary>
        /// Port IO Form 중 OP Buffer Y축 센서 정보 출력 행
        /// </summary>
        public enum DGV_BufferOP_YAxisSensorRow
        {
            Busy,
            Cylinder_BWD_Pos,
            Cylinder_FWD_Pos,
            BWD_Command,
            FWD_Command
        }

        /// <summary>
        /// Port IO Form 중 센서 정보 열
        /// </summary>
        public enum DGV_IOPageSensorStatusColumn
        {
            Number,
            SensorName,
            SensorStatus
        }

        /// <summary>
        /// Port Alarm Info Form 중 알람 리스트 정보 열
        /// </summary>
        public enum DGV_AlarmListColumn
        {
            Index,
            Hex,
            AlarmName,
            State
        }

        public void Update_Lbl_PortInfoLabel(ref Label lbl, PortInfoType ePortInfoType)
        {
            if (!IsEQPort())
            {
                if (ePortInfoType == PortInfoType.Simple)
                {
                    string InfoText = $"Port [ID : {GetParam().ID}] ({(m_eControlMode == ControlMode.CIMMode ? "CIM Mode" : "Master Mode")})\n" +
                                            $"Type : {GetParam().ePortType}\n" +
                                            $"Direction : {GetOperationDirection()}";
                    LabelFunc.SetText(lbl, InfoText);
                }
                else if (ePortInfoType == PortInfoType.Normal)
                {
                    string InfoText = $"Port [ID : {GetParam().ID}]\n" +
                                            $"Type : {GetParam().ePortType}\n" +
                                            $"Direction : {GetOperationDirection()}";
                    LabelFunc.SetText(lbl, InfoText);
                }
                else if (ePortInfoType == PortInfoType.Detail)
                {
                    string InfoText = $"Equipment : Port [ID : {GetParam().ID}]\n" +
                                    $"Type : {GetParam().ePortType}\n" +
                                    $"Direction : {GetOperationDirection()}\n" +
                                    $"Power : {(IsPortPowerOn() ? "On" : "Off")}\n" +
                                    $"Busy : {(IsPortBusy() ? "Busy" : "Idle")}\n" +
                                    $"Error : {GetRecentErrorCodeStr()}\n" +
                                    $"Auto Control : {(IsAutoControlRun() ? "Running" : "Idle")}\n" +
                                    $"Tag Reader : {(m_TagReader_Interface.IsConnected() ? "Connection" : "Disconnection")}";

                    LabelFunc.SetText(lbl, InfoText);
                }
                else if (ePortInfoType == PortInfoType.Map)
                {
                    string InfoText = $"Port[ID:{GetParam().ID}] ({(m_eControlMode == ControlMode.CIMMode ? "C Mode" : "M Mode")})\n" +
                                    $"Type: {GetParam().ePortType} ({GetOperationDirection()})\n" +
                                    $"Power: {(IsPortPowerOn() ? "On" : "Off")} ({(IsPortBusy() ? "Busy" : "Idle")})\n" +
                                    $"Control: {(IsAutoControlRun() ? "Auto Running" : IsAutoManualCycleRun() ? "Cycle Running" : "Idle")}\n" +
                                    $"Running Time: {StopWatchFunc.GetRunningTime(m_AutoRunProgressTime)}\n" +
                                    $"Error: {GetRecentErrorCodeStr()}\n";
                    InfoText = InfoText + GetAutoStepInfo();
                    LabelFunc.SetText(lbl, InfoText);
                }

                LabelFunc.SetBackColor(lbl, GetAlarmLevel() == AlarmLevel.Error ? Master.ErrorIntervalColor : IsAutoControlRun() ? Color.Lime : IsPortPowerOn() ? Color.Orange : Color.White);
                LabelFunc.SetVisible(lbl, true);
            }
            else
            {
                if (ePortInfoType == PortInfoType.Simple)
                {
                    string InfoText = $"Port[ID:{GetParam().ID}]\n" +
                                    $"Type: {GetParam().ePortType} ({GetOperationDirection()})";
                    LabelFunc.SetText(lbl, InfoText);
                    LabelFunc.SetBackColor(lbl, Color.Lime);
                    LabelFunc.SetVisible(lbl, true);
                }
                else
                {
                    string InfoText = $"Port[ID:{GetParam().ID}]\n" +
                                    $"Type: {GetParam().ePortType} ({GetOperationDirection()})\n" +
                                    $"Load_REQ: {(PIOStatus_EQToRM_Load_Req ? "On" : "Off")}\n" +
                                    $"Unload_REQ: {(PIOStatus_EQToRM_Unload_Req ? "On" : "Off")}\n" +
                                    $"Ready: {(PIOStatus_EQToRM_Ready ? "On" : "Off")}";
                    LabelFunc.SetText(lbl, InfoText);
                    LabelFunc.SetBackColor(lbl, Color.Lime);
                    LabelFunc.SetVisible(lbl, true);
                }
            }
        }
        public string GetFocusButtonStr()
        {
            string InfoText = $"Port[ID:{GetParam().ID}]({(m_eControlMode == ControlMode.CIMMode ? "CIM Mode" : "Master Mode")})\n" +
                                $"Type:{GetPortTypeToStr(GetParam().ePortType)}({(GetOperationDirection() == PortDirection.Input ? "IN" : "OUT")})";
            return InfoText;
        }
        private string GetAutoStepInfo()
        {
            string LP = $"LP Step: {Get_LP_AutoControlStepToStr()}";
            string BP = $"BP Step: {Get_BP_AutoControlStepToStr()}";
            string OP = $"OP Step: {Get_OP_AutoControlStepToStr()}";

            if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
            {
                return $"{LP}\n{BP}\n{OP}";
            }
            else if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
            {
                return $"{BP}\n{OP}";
            }
            else if (IsBufferControlPort() && GetMotionParam().IsBPCVUsed())
            {
                return $"{LP}\n{BP}\n{OP}";
            }
            else if (IsBufferControlPort() && !GetMotionParam().IsBPCVUsed())
            {
                return $"{LP}\n{OP}";
            }
            else
                return string.Empty;
        }
        public void Update_TeachingLabel(ref Label label_1_title, ref Label label_1_value, ref Label label_1_Unit, ref Button btn_1_MoveCMD,
                                        ref Label label_2_title, ref Label label_2_value, ref Label label_2_Unit, ref Button btn_2_MoveCMD,
                                        ref Label label_3_title, ref Label label_3_value, ref Label label_3_Unit, ref Button btn_3_MoveCMD,
                                        PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);

            if (IsValidServo)
            {
                if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                {
                    LabelFunc.SetText(label_1_title, $"{SynusLangPack.GetLanguage("Label_OPPos")} :");
                    LabelFunc.SetVisible(label_1_title, true);
                    LabelFunc.SetText(label_2_title, $"{SynusLangPack.GetLanguage("Label_WaitPos")} :");
                    LabelFunc.SetVisible(label_2_title, GetMotionParam().IsWaitPosEnable(ePortAxis) ? true : false);
                    LabelFunc.SetText(label_3_title, $"{SynusLangPack.GetLanguage("Label_LPPos")} :");
                    LabelFunc.SetVisible(label_3_title, true);

                    LabelFunc.SetText(label_1_value, GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.OP_Pos).ToString("0.0"));
                    LabelFunc.SetVisible(label_1_value, true);
                    LabelFunc.SetText(label_2_value, GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.Wait_Pos).ToString("0.0"));
                    LabelFunc.SetVisible(label_2_value, GetMotionParam().IsWaitPosEnable(ePortAxis) ? true : false);
                    LabelFunc.SetText(label_3_value, GetMotionParam().GetTeachingPos(ePortAxis, (int)(IsMGV() ? Teaching_X_Pos.MGV_LP_Pos : Teaching_X_Pos.Equip_LP_Pos)).ToString("0.0"));
                    LabelFunc.SetVisible(label_3_value, true);

                    LabelFunc.SetText(label_1_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_1_Unit, true);
                    LabelFunc.SetText(label_2_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_2_Unit, GetMotionParam().IsWaitPosEnable(ePortAxis) ? true : false);
                    LabelFunc.SetText(label_3_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_3_Unit, true);

                    ButtonFunc.SetVisible(btn_1_MoveCMD, true);
                    ButtonFunc.SetVisible(btn_2_MoveCMD, GetMotionParam().IsWaitPosEnable(ePortAxis) ? true : false);
                    ButtonFunc.SetVisible(btn_3_MoveCMD, true);
                }
                else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                {
                    LabelFunc.SetText(label_1_title, $"{SynusLangPack.GetLanguage("Label_UpPos")} :");
                    LabelFunc.SetVisible(label_1_title, true);
                    LabelFunc.SetText(label_2_title, $"{SynusLangPack.GetLanguage("Label_DownPos")} :");
                    LabelFunc.SetVisible(label_2_title, true);
                    LabelFunc.SetText(label_3_title, string.Empty);
                    LabelFunc.SetVisible(label_3_title, false);

                    LabelFunc.SetText(label_1_value, GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Up_Pos).ToString("0.0"));
                    LabelFunc.SetVisible(label_1_value, true);
                    LabelFunc.SetText(label_2_value, GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Down_Pos).ToString("0.0"));
                    LabelFunc.SetVisible(label_2_value, true);
                    LabelFunc.SetText(label_3_value, string.Empty);
                    LabelFunc.SetVisible(label_3_value, false);

                    LabelFunc.SetText(label_1_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_1_Unit, true);
                    LabelFunc.SetText(label_2_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_2_Unit, true);
                    LabelFunc.SetText(label_3_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_3_Unit, false);

                    ButtonFunc.SetVisible(btn_1_MoveCMD, true);
                    ButtonFunc.SetVisible(btn_2_MoveCMD, true);
                    ButtonFunc.SetVisible(btn_3_MoveCMD, false);
                }
                else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                {
                    LabelFunc.SetText(label_1_title, $"{SynusLangPack.GetLanguage("Label_0DegPos")} :");
                    LabelFunc.SetVisible(label_1_title, true);
                    LabelFunc.SetText(label_2_title, $"{SynusLangPack.GetLanguage("Label_180DegPos")} :");
                    LabelFunc.SetVisible(label_2_title, true);
                    LabelFunc.SetText(label_3_title, string.Empty);
                    LabelFunc.SetVisible(label_3_title, false);

                    LabelFunc.SetText(label_1_value, GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_T_Pos.Degree0_Pos).ToString("0.0"));
                    LabelFunc.SetVisible(label_1_value, true);
                    LabelFunc.SetText(label_2_value, GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_T_Pos.Degree180_Pos).ToString("0.0"));
                    LabelFunc.SetVisible(label_2_value, true);
                    LabelFunc.SetText(label_3_value, string.Empty);
                    LabelFunc.SetVisible(label_3_value, false);

                    LabelFunc.SetText(label_1_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_1_Unit, true);
                    LabelFunc.SetText(label_2_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_2_Unit, true);
                    LabelFunc.SetText(label_3_Unit, ePortAxis != PortAxis.Shuttle_T ? $"mm" : $"°");
                    LabelFunc.SetVisible(label_3_Unit, false);

                    ButtonFunc.SetVisible(btn_1_MoveCMD, true);
                    ButtonFunc.SetVisible(btn_2_MoveCMD, true);
                    ButtonFunc.SetVisible(btn_3_MoveCMD, false);
                }
            }
            //else if(GetMotionParam().IsCylinderType(ePortAxis))
            //{
            //    if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
            //    {
            //        LabelFunc.SetVisible(label_1_title, false);
            //        LabelFunc.SetVisible(label_2_title, false);
            //        LabelFunc.SetVisible(label_3_title, false);

            //        LabelFunc.SetVisible(label_1_value, false);
            //        LabelFunc.SetVisible(label_2_value, false);
            //        LabelFunc.SetVisible(label_3_value, false);

            //        LabelFunc.SetVisible(label_1_Unit, false);
            //        LabelFunc.SetVisible(label_2_Unit, false);
            //        LabelFunc.SetVisible(label_3_Unit, false);

            //        ButtonFunc.SetVisible(btn_1_MoveCMD, false);
            //        ButtonFunc.SetVisible(btn_2_MoveCMD, false);
            //        ButtonFunc.SetVisible(btn_3_MoveCMD, false);
            //    }
            //    else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
            //    {
            //        LabelFunc.SetText(label_1_title, $"{SynusLangPack.GetLanguage("Label_UpPos")} :");
            //        LabelFunc.SetVisible(label_1_title, true);
            //        LabelFunc.SetText(label_2_title, $"{SynusLangPack.GetLanguage("Label_DownPos")} :");
            //        LabelFunc.SetVisible(label_2_title, true);
            //        LabelFunc.SetText(label_3_title, string.Empty);
            //        LabelFunc.SetVisible(label_3_title, false);

            //        LabelFunc.SetVisible(label_1_value, false);
            //        LabelFunc.SetVisible(label_2_value, false);
            //        LabelFunc.SetVisible(label_3_value, false);

            //        LabelFunc.SetVisible(label_1_Unit, false);
            //        LabelFunc.SetVisible(label_2_Unit, false);
            //        LabelFunc.SetVisible(label_3_Unit, false);

            //        ButtonFunc.SetVisible(btn_1_MoveCMD, true);
            //        ButtonFunc.SetVisible(btn_2_MoveCMD, true);
            //        ButtonFunc.SetVisible(btn_3_MoveCMD, false);
            //    }
            //    else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
            //    {
            //        LabelFunc.SetVisible(label_1_title, false);
            //        LabelFunc.SetVisible(label_2_title, false);
            //        LabelFunc.SetVisible(label_3_title, false);

            //        LabelFunc.SetVisible(label_1_value, false);
            //        LabelFunc.SetVisible(label_2_value, false);
            //        LabelFunc.SetVisible(label_3_value, false);

            //        LabelFunc.SetVisible(label_1_Unit, false);
            //        LabelFunc.SetVisible(label_2_Unit, false);
            //        LabelFunc.SetVisible(label_3_Unit, false);

            //        ButtonFunc.SetVisible(btn_1_MoveCMD, false);
            //        ButtonFunc.SetVisible(btn_2_MoveCMD, false);
            //        ButtonFunc.SetVisible(btn_3_MoveCMD, false);
            //    }
            //}
            else
            {
                label_1_title.Text = string.Empty;
                label_2_title.Text = string.Empty;
                label_3_title.Text = string.Empty;

                label_1_value.Text = string.Empty;
                label_2_value.Text = string.Empty;
                label_3_value.Text = string.Empty;

                label_1_Unit.Text = string.Empty;
                label_2_Unit.Text = string.Empty;
                label_3_Unit.Text = string.Empty;

                btn_1_MoveCMD.Visible = false;
                btn_2_MoveCMD.Visible = false;
                btn_3_MoveCMD.Visible = false;
            }
        }

        public void UpdateWatchdogSettings(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_WatchdogSettingsColumn.Name:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_WatchdogList"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_WatchdogList");
                        break;
                    case (int)DGV_WatchdogSettingsColumn.AppliedValue:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_AppliedDetectTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_AppliedDetectTime");
                        break;
                    case (int)DGV_WatchdogSettingsColumn.ProgressTime:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ProgressTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ProgressTime");
                        break;
                    case (int)DGV_WatchdogSettingsColumn.SetValue:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SetDetectTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SetDetectTime");
                        break;
                }
            }

            string Key = $"{GetParam().ID}";
            if ((string)DGV.Tag != Key)
            {
                DGV.Rows.Clear();
                DGV.Tag = Key;
            }

            if (DGV.Rows.Count != Enum.GetValues(typeof(WatchdogList)).Length)
            {
                DGV.Rows.Clear();
                for(int nRowCount = 0; nRowCount < Enum.GetValues(typeof(WatchdogList)).Length; nRowCount++)
                {
                    WatchdogList eWatchdog = (WatchdogList)nRowCount;
                    DGV.Rows.Add(new string[5] { $"{eWatchdog}",
                                                    Watchdog_GetParam_DetectTime(eWatchdog).ToString(),
                                                    Watchdog_GetParam_DetectTime(eWatchdog).ToString(),
                                                    "Set" ,
                                                    string.Empty});
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    for(int nColumnCount =0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        WatchdogList eWatchdog = (WatchdogList)nRowCount;
                        DGV_WatchdogSettingsColumn eDGV_WatchdogSettingsColumn = (DGV_WatchdogSettingsColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;

                        if (eDGV_WatchdogSettingsColumn == DGV_WatchdogSettingsColumn.Name ||
                            eDGV_WatchdogSettingsColumn == DGV_WatchdogSettingsColumn.SetValue ||
                            eDGV_WatchdogSettingsColumn == DGV_WatchdogSettingsColumn.Btn)
                            continue;

                        switch (eDGV_WatchdogSettingsColumn)
                        {
                            case DGV_WatchdogSettingsColumn.AppliedValue:
                                Data = Watchdog_GetParam_DetectTime(eWatchdog).ToString();
                                break;
                            case DGV_WatchdogSettingsColumn.ProgressTime:
                                Data = Watchdog_GetProgressTime(eWatchdog);
                                if(eWatchdog == WatchdogList.PortArea_Release_Timer)
                                    DGV_Cell.Style.BackColor = Watchdog_GetColor(eWatchdog, true);
                                else
                                    DGV_Cell.Style.BackColor = Watchdog_GetColor(eWatchdog, false);
                                break;

                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }
                }
            }
        }
        public bool ApplyWatchdogSettings(ref DataGridView DGV)
        {
            for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
            {
                WatchdogList eWatchdog = (WatchdogList)nRowCount;
                DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[(int)DGV_WatchdogSettingsColumn.SetValue];
                DGV.CurrentCell = DGV_Cell;

                try
                {
                    string Text = (string)DGV_Cell.Value;
                    if (!string.IsNullOrEmpty(Text))
                    {
                        int DetectTime = Convert.ToInt32(Text);
                        Watchdog_SetParam_DetectTime(eWatchdog, DetectTime);
                    }
                    else
                    {
                        LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidParameter, $"{eWatchdog} Value: {Text}");
                        return false;
                    }
                }
                catch(Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{GetParam().ID}] {eWatchdog} Value");
                    return false;
                }
            }

            return true;
        }

        private void Load_X_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_X_AxisRow eDGV_TeachingParam_X_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_X_AxisRow)))
            {
                switch (eDGV_TeachingParam_X_AxisRow)
                {
                    case DGV_TeachingParam_X_AxisRow.X_OP_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.OP_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Wait_Position:
                        {
                            if (!GetMotionParam().IsWaitPosEnable(ePortAxis))
                                break;

                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.Wait_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_MGV_LP_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.MGV_LP_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Equip_LP_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_X_Pos.Equip_LP_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_Manual_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [m/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_Manual_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_Manual_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_AutoRun_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [m/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_AutoRun_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_AutoRun_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_X_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                }
            }
        }
        private void Load_Y_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_Y_AxisRow eDGV_TeachingParam_Y_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_Y_AxisRow)))
            {
                switch (eDGV_TeachingParam_Y_AxisRow)
                {
                    case DGV_TeachingParam_Y_AxisRow.Y_FWD_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Y_Pos.FWD_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;

                    case DGV_TeachingParam_Y_AxisRow.Y_BWD_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Y_Pos.BWD_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_Manual_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [m/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_Manual_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_Manual_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_AutoRun_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [m/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_AutoRun_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_AutoRun_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Y_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                }
            }
        }
        private void Load_Z_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_Z_AxisRow eDGV_TeachingParam_Z_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_Z_AxisRow)))
            {
                switch (eDGV_TeachingParam_Z_AxisRow)
                {
                    case DGV_TeachingParam_Z_AxisRow.Z_Up_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Up_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;

                    case DGV_TeachingParam_Z_AxisRow.Z_Down_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_Z_Pos.Down_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [mm]",
                                                                    value.ToString("0.0"),
                                                                    value.ToString("0.0"),
                                                                    "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_OverrideDistance:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDistance;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [mm]",
                                                                        value.ToString("0.0"),
                                                                        value.ToString("0.0"),
                                                                        "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_OverrideDecPercent:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDecPercent;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [%]",
                                                                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDecPercent.ToString("0"),
                                                                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDecPercent.ToString("0"),
                                                                            string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_Manual_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [m/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_Manual_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_Manual_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_AutoRun_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [m/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_AutoRun_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_AutoRun_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_Z_AxisRow} [m/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                }
            }
        }
        private void Load_T_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_T_AxisRow eDGV_TeachingParam_T_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_T_AxisRow)))
            {
                switch (eDGV_TeachingParam_T_AxisRow)
                {
                    case DGV_TeachingParam_T_AxisRow.T_0_Degree_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_T_Pos.Degree0_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°]",
                                                                        value.ToString("0.0"),
                                                                        value.ToString("0.0"),
                                                                        "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_180_Degree_Position:
                        {
                            float value = GetMotionParam().GetTeachingPos(ePortAxis, (int)Teaching_T_Pos.Degree180_Pos);
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°]",
                                                                        value.ToString("0.0"),
                                                                        value.ToString("0.0"),
                                                                        "Set Cur" });
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_Manual_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_Manual_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_Manual_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_AutoRun_Speed:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°/min]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_AutoRun_Acc:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_AutoRun_Dec:
                        {
                            float value = GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec;
                            DGV.Rows.Add(new string[4] { $"{eDGV_TeachingParam_T_AxisRow} [°/min^2]",
                                                                        value.ToString("0"),
                                                                        value.ToString("0"),
                                                                        string.Empty });
                        }
                        break;
                }
            }
        }
        public void LoadTeachingParamDataGridView(ref DataGridView DGV, ref GroupBox Gbx, PortAxis ePortAxis)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_TeachingParamColumn.Name:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ParameterName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ParameterName");
                        break;
                    case (int)DGV_TeachingParamColumn.AppliedValue:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_AppliedValue"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_AppliedValue");
                        break;
                    case (int)DGV_TeachingParamColumn.SetValue:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SetValue"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SetValue");
                        break;
                }
            }

            if (ePortAxis == PortAxis.Shuttle_X ||
                ePortAxis == PortAxis.Buffer_LP_X ||
                ePortAxis == PortAxis.Buffer_OP_X)
            {
                GroupBoxFunc.SetText(Gbx, $"{SynusLangPack.GetLanguage("X_Axis")} " + SynusLangPack.GetLanguage("GroupBox_TeachingParameter"));
                Load_X_AxisTeachingParam(ref DGV, ePortAxis);
            }
            else if(ePortAxis == PortAxis.Buffer_LP_Y ||
                ePortAxis == PortAxis.Buffer_OP_Y)
            {
                GroupBoxFunc.SetText(Gbx, $"{SynusLangPack.GetLanguage("Y_Axis")} " + SynusLangPack.GetLanguage("GroupBox_TeachingParameter"));
                Load_Y_AxisTeachingParam(ref DGV, ePortAxis);
            }
            else if (ePortAxis == PortAxis.Shuttle_Z ||
                ePortAxis == PortAxis.Buffer_LP_Z ||
                ePortAxis == PortAxis.Buffer_OP_Z)
            {
                GroupBoxFunc.SetText(Gbx, $"{SynusLangPack.GetLanguage("Z_Axis")} " + SynusLangPack.GetLanguage("GroupBox_TeachingParameter"));
                Load_Z_AxisTeachingParam(ref DGV, ePortAxis);
            }
            else if (ePortAxis == PortAxis.Shuttle_T ||
                ePortAxis == PortAxis.Buffer_LP_T ||
                ePortAxis == PortAxis.Buffer_OP_T)
            {
                GroupBoxFunc.SetText(Gbx, $"{SynusLangPack.GetLanguage("T_Axis")} " + SynusLangPack.GetLanguage("GroupBox_TeachingParameter"));
                Load_T_AxisTeachingParam(ref DGV, ePortAxis);
            }
            else
            {
                GroupBoxFunc.SetText(Gbx, SynusLangPack.GetLanguage("GroupBox_TeachingParameter"));
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();
            }

            DataGridViewFunc.AutoRowSize(DGV, 25, 23, 30);
        }

        private float FindCellValue(ref DataGridView DGV, string ObjectName)
        {
            for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
            {
                DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)DGV_TeachingParamColumn.Name];
                string Name = DGV_NameCell.Value.ToString();

                if (Name.Contains(ObjectName))
                {
                    DataGridViewCell DGV_ValueCell = DGV.Rows[nRowCount].Cells[(int)DGV_TeachingParamColumn.SetValue];
                    DGV.CurrentCell = DGV_ValueCell;
                    try
                    {
                        return Convert.ToSingle(DGV_ValueCell.Value);
                    }
                    catch
                    {
                        return 0.0f;
                    }
                }
            }

            return 0.0f;
        }
        public bool Apply_X_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_X_AxisRow eDGV_TeachingParam_X_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_X_AxisRow)))
            {
                switch (eDGV_TeachingParam_X_AxisRow)
                {
                    case DGV_TeachingParam_X_AxisRow.X_OP_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_X_Pos.OP_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Wait_Position:
                        {
                            if (!GetMotionParam().IsWaitPosEnable(ePortAxis))
                                break;

                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_X_Pos.Wait_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_MGV_LP_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_X_Pos.MGV_LP_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Equip_LP_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_X_Pos.Equip_LP_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_Manual_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_Manual_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_Manual_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_AutoRun_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_AutoRun_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_X_AxisRow.X_Move_AutoRun_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_X_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec = fValue;
                        }
                        break;
                }
            }

            return true;
        }
        public bool Apply_Y_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_Y_AxisRow eDGV_TeachingParam_Y_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_Y_AxisRow)))
            {
                switch (eDGV_TeachingParam_Y_AxisRow)
                {
                    case DGV_TeachingParam_Y_AxisRow.Y_FWD_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_Y_Pos.FWD_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_BWD_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_Y_Pos.BWD_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_Manual_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_Manual_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_Manual_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_AutoRun_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_AutoRun_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Y_AxisRow.Y_Move_AutoRun_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Y_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec = fValue;
                        }
                        break;
                }
            }

            return true;
        }
        public bool Apply_Z_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_Z_AxisRow eDGV_TeachingParam_Z_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_Z_AxisRow)))
            {
                switch (eDGV_TeachingParam_Z_AxisRow)
                {
                    case DGV_TeachingParam_Z_AxisRow.Z_Up_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_Z_Pos.Up_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Down_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_Z_Pos.Down_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_OverrideDistance:
                        {
                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDistance = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_OverrideDecPercent:
                        {
                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDecPercent = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_Manual_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_Manual_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_Manual_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_AutoRun_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_AutoRun_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_Z_AxisRow.Z_Move_AutoRun_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_Z_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec = fValue;
                        }
                        break;
                }
            }

            return true;
        }
        public bool Apply_T_AxisTeachingParam(ref DataGridView DGV, PortAxis ePortAxis)
        {
            foreach (DGV_TeachingParam_T_AxisRow eDGV_TeachingParam_T_AxisRow in Enum.GetValues(typeof(DGV_TeachingParam_T_AxisRow)))
            {
                switch (eDGV_TeachingParam_T_AxisRow)
                {
                    case DGV_TeachingParam_T_AxisRow.T_0_Degree_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_T_Pos.Degree0_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_180_Degree_Position:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).TeachingPos[(int)Teaching_T_Pos.Degree180_Pos] = fValue;
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_Manual_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_Manual_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_Manual_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec = fValue;
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_AutoRun_Speed:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidSpeed(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed = fValue;
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_AutoRun_Acc:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidAcc(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc = fValue;
                        }
                        break;
                    case DGV_TeachingParam_T_AxisRow.T_Move_AutoRun_Dec:
                        {
                            float fValue = FindCellValue(ref DGV, eDGV_TeachingParam_T_AxisRow.ToString());
                            string Value = fValue.ToString();

                            if (!EquipPortMotionParam.ServoParam.IsValidDec(Value, ePortAxis))
                                return false;

                            GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec = fValue;
                        }
                        break;
                }
            }

            return true;
        }

        public void Update_Btn_AutoRun(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AutoRun"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true);
                ButtonFunc.SetBackColor(btn, IsAutoControlRun() ? Color.Lime : Color.White);
            }
        }
        public void Update_Btn_AutoStop(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AutoStop"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true);
                ButtonFunc.SetBackColor(btn, !IsAutoControlRun() ? Color.Orange : Color.White);
            }
        }
        public void Update_Btn_PowerOn(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_PowerOn"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
                ButtonFunc.SetBackColor(btn, LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : IsPortPowerOn() ? Color.Lime : Color.White);
            }
        }
        public void Update_Btn_PowerOff(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_PowerOff"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
                ButtonFunc.SetBackColor(btn, LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : !IsPortPowerOn() ? Color.Orange : Color.White);
            }
        }
        public void Update_Btn_CIMMode(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_CIMMode"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
                ButtonFunc.SetBackColor(btn, LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : m_eControlMode == ControlMode.CIMMode ? Color.Lime : Color.White);
            }
        }
        public void Update_Btn_MasterMode(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_MasterMode"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
                ButtonFunc.SetBackColor(btn, LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : m_eControlMode == ControlMode.MasterMode ? Color.Lime : Color.White);
            }
        }
        public void Update_Btn_ModeChange(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_ModeChange"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetVisible(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
                ButtonFunc.SetVisible(btn, GetParam().ePortType == PortType.MGV_AGV || GetParam().ePortType == PortType.MGV_OHT ? true : false);
                ButtonFunc.SetBackColor(btn, Color.White);
            }
        }
        public void Update_Btn_DirectionChange(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_DirectionChange"));

            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
                ButtonFunc.SetBackColor(btn, Color.White);
            }
        }

        public void Update_Btn_PortEStop(ref Button btn)
        {
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetVisible(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                ButtonFunc.SetEnable(btn, true);
                ButtonFunc.SetVisible(btn, true);
                ButtonFunc.SetBackColor(btn, GetSWEStopState() != Interface.Safty.EStopState.Idle ? Master.ErrorIntervalColor : Color.White);
                ButtonFunc.SetText(btn, GetSWEStopState() != Interface.Safty.EStopState.Idle ? SynusLangPack.GetLanguage("Btn_EStopRelease") : SynusLangPack.GetLanguage("Btn_PortEStop"));
            }
        }
        public void Update_Btn_PortAlarmClear(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AlarmClear"));
            if (IsEQPort())
            {
                ButtonFunc.SetEnable(btn, false);
                ButtonFunc.SetVisible(btn, false);
                ButtonFunc.SetBackColor(btn, Color.DarkGray);
            }
            else
            {
                var eAlarmLevel = GetAlarmLevel();
                ButtonFunc.SetVisible(btn, true);
                ButtonFunc.SetBackColor(btn, eAlarmLevel == AlarmLevel.Error ? Master.ErrorIntervalColor : eAlarmLevel == AlarmLevel.Warning ? Color.Orange : Color.White);
                ButtonFunc.SetEnable(btn, (eAlarmLevel == AlarmLevel.Error || eAlarmLevel == AlarmLevel.Warning) ? true : false);
            }
        }

        public void Update_Btn_ServoOn(ref Button btn, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);
            bool bServoOn = ServoCtrl_GetServoOn(ePortAxis);
            ButtonFunc.SetEnable(btn, IsValidServo ? true : false);
            ButtonFunc.SetText(btn, !IsValidServo ? string.Empty : SynusLangPack.GetLanguage("Btn_ServoOn"));
            ButtonFunc.SetBackColor(btn, !IsValidServo ? Color.DarkGray : bServoOn ? Color.Lime : Master.FocusIntervalColor);
        }
        public void Update_Btn_ServoOff(ref Button btn, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);
            bool bServoOn = ServoCtrl_GetServoOn(ePortAxis);
            ButtonFunc.SetEnable(btn, IsValidServo ? true : false);
            ButtonFunc.SetText(btn, !IsValidServo ? string.Empty : SynusLangPack.GetLanguage("Btn_ServoOff"));
            ButtonFunc.SetBackColor(btn, !IsValidServo ? Color.DarkGray : bServoOn ? Color.White : Color.Yellow);
        }
        public void Update_Btn_Homing(ref Button btn, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);
            bool bServoOn = ServoCtrl_GetServoOn(ePortAxis);
            bool bHoming = ServoCtrl_GetHomingStatus(ePortAxis);
            ButtonFunc.SetEnable(btn, IsValidServo && bServoOn ? true : false);
            ButtonFunc.SetText(btn, !IsValidServo ? string.Empty : ServoCtrl_GetHomeDone(ePortAxis) ? SynusLangPack.GetLanguage("Btn_HomeDone") : SynusLangPack.GetLanguage("Btn_Homing"));
            ButtonFunc.SetBackColor(btn, !IsValidServo || !bServoOn ? Color.DarkGray : ServoCtrl_GetHomeDone(ePortAxis) || bHoming ? Color.Lime : Master.FocusIntervalColor);
        }
        public void Update_Btn_AmpAlarmClear(ref Button btn, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);
            ButtonFunc.SetEnable(btn, IsValidServo ? true : false);
            ButtonFunc.SetText(btn, !IsValidServo ? string.Empty : SynusLangPack.GetLanguage("Btn_AmpAlarmClear"));
            ButtonFunc.SetBackColor(btn, !IsValidServo ? Color.DarkGray : ServoCtrl_GetAlarmStatus(ePortAxis) ? Master.ErrorIntervalColor : Color.White);

            //ButtonFunc.SetBackColor(btn, GetAxisAlarm(ePortAxis) ? Master.ErrorIntervalColor : Color.White);
            //ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AmpAlarmClear"));
        }
        public void Update_Btn_MotionStop(ref Button btn, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);
            bool bServoOn = ServoCtrl_GetServoOn(ePortAxis);
            ButtonFunc.SetEnable(btn, IsValidServo && bServoOn ? true : false);
            ButtonFunc.SetText(btn, !IsValidServo ? string.Empty : SynusLangPack.GetLanguage("Btn_MotionStop"));
            ButtonFunc.SetBackColor(btn, !IsValidServo || !bServoOn ? Color.DarkGray : IsPortAxisBusy(ePortAxis) ? Color.Yellow : Color.White);

            //ButtonFunc.SetBackColor(btn, GetAxisBusy(ePortAxis) ? Color.Yellow : Color.White);
            //ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_MotionStop"));
        }

        public void Update_Btn_CycleRun(ref Button btn)
        {
            ButtonFunc.SetVisible(btn, m_eControlMode == ControlMode.MasterMode && !IsEQPort() ? true : false);
            ButtonFunc.SetBackColor(btn, IsAutoManualCycleRun() ? Color.Lime : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_CycleRun"));
        }
        public void Update_Btn_CycleStop(ref Button btn)
        {
            ButtonFunc.SetVisible(btn, m_eControlMode == ControlMode.MasterMode && !IsEQPort() ? true : false);
            ButtonFunc.SetBackColor(btn, IsAutoManualCycleRun() ? Color.Orange : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_CycleStop"));
        }

        public void Update_GBx_CycleGroup(ref GroupBox gbx)
        {
            GroupBoxFunc.SetVisible(gbx, m_eControlMode == ControlMode.MasterMode && !IsEQPort() ? true : false);
            GroupBoxFunc.SetText(gbx, SynusLangPack.GetLanguage("GorupBox_AutoCycleTest"));
        }

        public void Update_Lbl_OperationMode(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{m_ePortOperationMode}");
        }
        public void Update_Lbl_PortDirection(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{GetOperationDirection()}");
        }
        public void Update_Lbl_AutoControlStatus(ref Label lbl)
        {
            LabelFunc.SetText(lbl, IsAutoControlRun() ? "Running" : "Idle");
            LabelFunc.SetBackColor(lbl, IsAutoControlRun() ? Color.Lime : Color.White);
        }
        public void Update_Lbl_AlarmStatus(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{(GetAlarmLevel() == AlarmLevel.None ? string.Empty : GetAlarmLevel().ToString())}");
            LabelFunc.SetBackColor(lbl, GetAlarmLevel() == AlarmLevel.Error ? Master.ErrorIntervalColor : GetAlarmLevel() == AlarmLevel.Warning ? Color.Orange : Color.White);
        }
        public void Update_Lbl_AlarmText(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{(GetRecentErrorCodeStr() == "None" ? string.Empty : GetRecentErrorCodeStr())}");
        }
        public void Update_Lbl_JogLowSpeed(ref Label lbl, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);

            if (IsValidServo)
                LabelFunc.SetText(lbl, ePortAxis != PortAxis.Shuttle_T ? $"{SynusLangPack.GetLanguage("Label_LowSpeed")} (m/min) :" : $"{SynusLangPack.GetLanguage("Label_LowSpeed")} (°/min) :");
            else
                LabelFunc.SetText(lbl, string.Empty);
        }
        public void Update_Lbl_JogHighSpeed(ref Label lbl, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);

            if (IsValidServo)
                LabelFunc.SetText(lbl, ePortAxis != PortAxis.Shuttle_T ? $"{SynusLangPack.GetLanguage("Label_HighSpeed")} (m/min) :" : $"{SynusLangPack.GetLanguage("Label_HighSpeed")} (°/min) :");
            else
                LabelFunc.SetText(lbl, string.Empty);
        }
        public void Update_Lbl_RelMove(ref Label lbl, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);

            if (IsValidServo)
                LabelFunc.SetText(lbl, ePortAxis != PortAxis.Shuttle_T ? $"{SynusLangPack.GetLanguage("Label_InchingPos")} (mm) :" : $"{SynusLangPack.GetLanguage("Label_InchingPos")} (°) :");
            else
                LabelFunc.SetText(lbl, string.Empty);
        }
        public void Update_Lbl_AbsMove(ref Label lbl, PortAxis ePortAxis)
        {
            bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);

            if (IsValidServo)
                LabelFunc.SetText(lbl, ePortAxis != PortAxis.Shuttle_T ? $"{SynusLangPack.GetLanguage("Label_AbsolutePos")} (mm) :" : $"{SynusLangPack.GetLanguage("Label_AbsolutePos")} (°) :");
            else
                LabelFunc.SetText(lbl, string.Empty);
        }

        public void Update_Lbl_AutoRunSpeed(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AppliedValue")} : {GetMotionParam().AutoRun_Ratio} %");
        }

        public void Update_DGV_PortStatusInfo(ref DataGridView DGV)
        {
            for (int nCount =0; nCount < DGV.Columns.Count; nCount++)
            {
                switch(nCount)
                {
                    case 0:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_List"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_List");
                        break;
                    case 1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Status"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Status");
                        break;
                }
            }

            List<DGV_PortStatusInfoRowList> UseRowList = new List<DGV_PortStatusInfoRowList>();

            if (!IsEQPort())
            {
                UseRowList.Add(DGV_PortStatusInfoRowList.TagReaderType);

                if (GetParam().eTagReaderType != TagReader.TagReaderType.None)
                    UseRowList.Add(DGV_PortStatusInfoRowList.TagValue);

                if (IsShuttleControlPort())
                    UseRowList.Add(DGV_PortStatusInfoRowList.BufferType);

                UseRowList.Add(DGV_PortStatusInfoRowList.ControlDirection);
                UseRowList.Add(DGV_PortStatusInfoRowList.AutoRunStatus);

                if (!IsEQPort())
                    UseRowList.Add(DGV_PortStatusInfoRowList.CycleRunStatus);

                if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                {
                    UseRowList.Add(DGV_PortStatusInfoRowList.OPStep);
                    UseRowList.Add(DGV_PortStatusInfoRowList.BPStep);
                    UseRowList.Add(DGV_PortStatusInfoRowList.LPStep);
                }
                else if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                {
                    UseRowList.Add(DGV_PortStatusInfoRowList.OPStep);
                    UseRowList.Add(DGV_PortStatusInfoRowList.BPStep);
                }
                else if (IsBufferControlPort() && GetMotionParam().IsBPCVUsed())
                {
                    UseRowList.Add(DGV_PortStatusInfoRowList.OPStep);
                    UseRowList.Add(DGV_PortStatusInfoRowList.BPStep);
                    UseRowList.Add(DGV_PortStatusInfoRowList.LPStep);
                }
                else if (IsBufferControlPort() && !GetMotionParam().IsBPCVUsed())
                {
                    UseRowList.Add(DGV_PortStatusInfoRowList.OPStep);
                    UseRowList.Add(DGV_PortStatusInfoRowList.LPStep);
                }

                UseRowList.Add(DGV_PortStatusInfoRowList.PortBusy);

                foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
                {
                    bool bAxisUsed = !GetMotionParam().IsAxisUnUsed(ePortAxis);

                    if (bAxisUsed && ePortAxis == PortAxis.Shuttle_X)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Shuttle_X_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Shuttle_Z)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Shuttle_Z_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Shuttle_T)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Shuttle_T_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Buffer_LP_X)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_LP_X_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Buffer_LP_Z)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_LP_Z_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Buffer_LP_T)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_LP_T_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Buffer_OP_X)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_OP_X_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Buffer_OP_Z)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_OP_Z_Axis);
                    else if (bAxisUsed && ePortAxis == PortAxis.Buffer_OP_T)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_OP_T_Axis);
                }

                foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
                {
                    bool bCVUsed = GetMotionParam().IsCVUsed(eBufferCV);

                    if (bCVUsed && eBufferCV == BufferCV.Buffer_LP)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_LP_CV);
                    if (bCVUsed && eBufferCV == BufferCV.Buffer_BP1)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_BP1_CV);
                    else if (bCVUsed && eBufferCV == BufferCV.Buffer_BP2)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_BP2_CV);
                    else if (bCVUsed && eBufferCV == BufferCV.Buffer_BP3)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_BP3_CV);
                    else if (bCVUsed && eBufferCV == BufferCV.Buffer_BP4)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_BP4_CV);
                    else if (bCVUsed && eBufferCV == BufferCV.Buffer_OP)
                        UseRowList.Add(DGV_PortStatusInfoRowList.Buffer_OP_CV);
                }
            }
            else
            {
                UseRowList.Add(DGV_PortStatusInfoRowList.EQ_PIO_Load_REQ);
                UseRowList.Add(DGV_PortStatusInfoRowList.EQ_PIO_Unload_REQ);
                UseRowList.Add(DGV_PortStatusInfoRowList.EQ_PIO_Ready);
            }

            if (UseRowList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();
            }
            else
            {
                if (DGV.Rows.Count != UseRowList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < UseRowList.Count; nCount++)
                        DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 23, 40);

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_PortStatusInfoRowList eDGV_PortStatusInfoRow = UseRowList[nRowCount];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_ValueCell = DGV.Rows[nRowCount].Cells[1];

                    switch (eDGV_PortStatusInfoRow)
                    {
                        case DGV_PortStatusInfoRowList.TagReaderType:
                            {
                                DGV_NameCell.Value = "Tag Reader Type";
                                DGV_ValueCell.Value = m_TagReader_Interface.GetCurrentTagReader();;
                            }
                            break;
                        case DGV_PortStatusInfoRowList.TagValue:
                            {
                                DGV_NameCell.Value = "Tag Value(CST ID)";
                                DGV_ValueCell.Value = m_TagReader_Interface.GetTag();
                            }
                            break;
                        case DGV_PortStatusInfoRowList.BufferType:
                            {
                                DGV_NameCell.Value = "Buffer Type";
                                DGV_ValueCell.Value = GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer ? "2 Buffer" : "1 Buffer";
                            }
                            break;
                        case DGV_PortStatusInfoRowList.ControlDirection:
                            {
                                DGV_NameCell.Value = "Port Direction";
                                DGV_ValueCell.Value = GetOperationDirection() == PortDirection.Input ? "Input" : "Output";
                            }
                            break;
                        case DGV_PortStatusInfoRowList.AutoRunStatus:
                            {
                                bool bEnable = IsAutoControlRun();
                                DGV_NameCell.Value = "Auto Run Status";
                                DGV_ValueCell.Value = bEnable ? "Running" : "Idle";
                                DGV_ValueCell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_PortStatusInfoRowList.CycleRunStatus:
                            {
                                bool bEnable = IsAutoManualCycleRun();
                                DGV_NameCell.Value = "Manual Cycle Status";
                                DGV_ValueCell.Value = bEnable ? "Running" : "Idle";
                                DGV_ValueCell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_PortStatusInfoRowList.OPStep:
                            {
                                string StepStr  = Get_OP_AutoControlStepToStr();
                                int StepIndex   = Get_OP_AutoControlStep();
                                DGV_NameCell.Value = "OP Auto Step";
                                DGV_ValueCell.Value = $"{StepStr}[{StepIndex}]";
                            }
                            break;
                        case DGV_PortStatusInfoRowList.BPStep:
                            {
                                string StepStr = Get_BP_AutoControlStepToStr();
                                int StepIndex = Get_BP_AutoControlStep();
                                DGV_NameCell.Value = "BP Auto Step";
                                DGV_ValueCell.Value = $"{StepStr}[{StepIndex}]";
                            }
                            break;
                        case DGV_PortStatusInfoRowList.LPStep:
                            {
                                string StepStr = Get_LP_AutoControlStepToStr();
                                int StepIndex = Get_LP_AutoControlStep();
                                DGV_NameCell.Value = "LP Auto Step";
                                DGV_ValueCell.Value = $"{StepStr}[{StepIndex}]";
                            }
                            break;
                        case DGV_PortStatusInfoRowList.PortBusy:
                            {
                                bool bEnable = IsPortBusy();
                                DGV_NameCell.Value = "Port Busy";
                                DGV_ValueCell.Value = bEnable ? "Busy" : "Idle";
                                DGV_ValueCell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_PortStatusInfoRowList.Shuttle_X_Axis:
                        case DGV_PortStatusInfoRowList.Shuttle_Z_Axis:
                        case DGV_PortStatusInfoRowList.Shuttle_T_Axis:
                        case DGV_PortStatusInfoRowList.Buffer_LP_X_Axis:
                        case DGV_PortStatusInfoRowList.Buffer_LP_Z_Axis:
                        case DGV_PortStatusInfoRowList.Buffer_LP_T_Axis:
                        case DGV_PortStatusInfoRowList.Buffer_OP_X_Axis:
                        case DGV_PortStatusInfoRowList.Buffer_OP_Z_Axis:
                        case DGV_PortStatusInfoRowList.Buffer_OP_T_Axis:
                            {
                                PortAxis ePortAxis = PortAxis.Shuttle_X;
                                string AxisTitle = string.Empty;

                                if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Shuttle_X_Axis)
                                { AxisTitle = "Shuttle X-Axis"; ePortAxis = PortAxis.Shuttle_X; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Shuttle_Z_Axis)
                                { AxisTitle = "Shuttle Z-Axis"; ePortAxis = PortAxis.Shuttle_Z; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Shuttle_T_Axis)
                                { AxisTitle = "Shuttle T-Axis"; ePortAxis = PortAxis.Shuttle_T; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_LP_X_Axis)
                                { AxisTitle = "Buffer LP X-Axis"; ePortAxis = PortAxis.Buffer_LP_X; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_LP_Z_Axis)
                                { AxisTitle = "Buffer LP Z-Axis"; ePortAxis = PortAxis.Buffer_LP_Z; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_LP_T_Axis)
                                { AxisTitle = "Buffer LP T-Axis"; ePortAxis = PortAxis.Buffer_LP_T; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_OP_X_Axis)
                                { AxisTitle = "Buffer OP X-Axis"; ePortAxis = PortAxis.Buffer_OP_X; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_OP_Z_Axis)
                                { AxisTitle = "Buffer OP Z-Axis"; ePortAxis = PortAxis.Buffer_OP_Z; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_OP_T_Axis)
                                { AxisTitle = "Buffer OP T-Axis"; ePortAxis = PortAxis.Buffer_OP_T; }

                                if(GetMotionParam().IsServoType(ePortAxis))
                                {
                                    string ActualPos = ServoCtrl_GetCurrentPosition(ePortAxis).ToString("0.0") + (GetMotionParam().IsRotaryAxis(ePortAxis) ? " °" : " mm");
                                    string ActualTorque = ServoCtrl_GetCurrentTorque(ePortAxis).ToString("0") + " %";

                                    DGV_NameCell.Value = $"{AxisTitle}[Servo]";

                                    DGV_ValueCell.Style.BackColor = (string)DGV_ValueCell.Value != $"Pos {ActualPos}, Trq {ActualTorque}" ? Color.Lime : Color.White;
                                    DGV_ValueCell.Value = $"Pos {ActualPos}, Trq {ActualTorque}";
                                }
                                else if (GetMotionParam().IsCylinderType(ePortAxis))
                                {
                                    bool bFWD = CylinderCtrl_GetRunStatus(ePortAxis, CylCtrlList.FWD);
                                    bool bBWD = CylinderCtrl_GetRunStatus(ePortAxis, CylCtrlList.BWD);
                                    DGV_NameCell.Value = $"{AxisTitle}[Cylinder]";
                                    DGV_ValueCell.Value = $"FWD [{(bFWD ? "On" : "Off")}], BWD [{(bBWD ? "On" : "Off")}]";
                                    DGV_ValueCell.Style.BackColor = bFWD || bBWD ? Color.Lime : Color.White;
                                }
                                else if (GetMotionParam().IsInverterType(ePortAxis))
                                {
                                    if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
                                    {
                                        bool bHFWD = InverterCtrl_GetRunStatus(ePortAxis, InvCtrlType.HighSpeedFWD);
                                        bool bLFWD = InverterCtrl_GetRunStatus(ePortAxis, InvCtrlType.LowSpeedFWD);
                                        bool bLBWD = InverterCtrl_GetRunStatus(ePortAxis, InvCtrlType.LowSpeedBWD);
                                        bool bHBWD = InverterCtrl_GetRunStatus(ePortAxis, InvCtrlType.HighSpeedBWD);
                                        DGV_NameCell.Value = $"{AxisTitle}[Inverter]";
                                        DGV_ValueCell.Value = $"FWD [{(bHFWD ? "On" : "Off")}, {(bLFWD ? "On" : "Off")}], BWD [{(bHBWD ? "On" : "Off")}, {(bLBWD ? "On" : "Off")}]";
                                        DGV_ValueCell.Style.BackColor = bHFWD || bLFWD || bLBWD || bHBWD ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        bool bFWD = InverterCtrl_GetRunStatus(ePortAxis, InvCtrlType.FreqFWD);
                                        bool bBWD = InverterCtrl_GetRunStatus(ePortAxis, InvCtrlType.FreqBWD);
                                        DGV_NameCell.Value = $"{AxisTitle}[Inverter]";
                                        DGV_ValueCell.Value = $"FWD [{(bFWD ? "On" : "Off")}], BWD [{(bBWD ? "On" : "Off")}]";
                                        DGV_ValueCell.Style.BackColor = bFWD || bBWD  ? Color.Lime : Color.White;
                                    }
                                }
                            }
                            break;
                        case DGV_PortStatusInfoRowList.Buffer_LP_CV:
                        case DGV_PortStatusInfoRowList.Buffer_OP_CV:
                        case DGV_PortStatusInfoRowList.Buffer_BP1_CV:
                        case DGV_PortStatusInfoRowList.Buffer_BP2_CV:
                        case DGV_PortStatusInfoRowList.Buffer_BP3_CV:
                        case DGV_PortStatusInfoRowList.Buffer_BP4_CV:
                            {
                                BufferCV eBufferCV = BufferCV.Buffer_LP;
                                string AxisTitle = string.Empty;

                                if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_LP_CV)
                                { AxisTitle = "LP Conveyor"; eBufferCV = BufferCV.Buffer_LP; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_OP_CV)
                                { AxisTitle = "OP Conveyor"; eBufferCV = BufferCV.Buffer_OP; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_BP1_CV)
                                { AxisTitle = "BP1 Conveyor"; eBufferCV = BufferCV.Buffer_BP1; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_BP2_CV)
                                { AxisTitle = "BP2 Conveyor"; eBufferCV = BufferCV.Buffer_BP2; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_BP3_CV)
                                { AxisTitle = "BP3 Conveyor"; eBufferCV = BufferCV.Buffer_BP3; }
                                else if (eDGV_PortStatusInfoRow == DGV_PortStatusInfoRowList.Buffer_BP4_CV)
                                { AxisTitle = "BP4 Conveyor"; eBufferCV = BufferCV.Buffer_BP4; }

                                if (GetMotionParam().IsCVUsed(eBufferCV))
                                {
                                    if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
                                    {
                                        bool bHFWD = BufferCtrl_CV_GetRunStatus(eBufferCV, InvCtrlType.HighSpeedFWD);
                                        bool bLFWD = BufferCtrl_CV_GetRunStatus(eBufferCV, InvCtrlType.LowSpeedFWD);
                                        bool bLBWD = BufferCtrl_CV_GetRunStatus(eBufferCV, InvCtrlType.LowSpeedBWD);
                                        bool bHBWD = BufferCtrl_CV_GetRunStatus(eBufferCV, InvCtrlType.HighSpeedBWD);
                                        DGV_NameCell.Value = $"{AxisTitle}[Inverter]";
                                        DGV_ValueCell.Value = $"FWD [{(bHFWD ? "On" : "Off")}, {(bLFWD ? "On" : "Off")}], BWD [{(bHBWD ? "On" : "Off")}, {(bLBWD ? "On" : "Off")}]";
                                        DGV_ValueCell.Style.BackColor = bHFWD || bLFWD || bLBWD || bHBWD ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        bool bFWD = BufferCtrl_CV_GetRunStatus(eBufferCV, InvCtrlType.FreqFWD);
                                        bool bBWD = BufferCtrl_CV_GetRunStatus(eBufferCV, InvCtrlType.FreqBWD);
                                        DGV_NameCell.Value = $"{AxisTitle}[Inverter]";
                                        DGV_ValueCell.Value = $"FWD [{(bFWD ? "On" : "Off")}], BWD [{(bBWD ? "On" : "Off")}]";
                                        DGV_ValueCell.Style.BackColor = bFWD || bBWD ? Color.Lime : Color.White;
                                    }
                                }

                            }
                            break;
                        case DGV_PortStatusInfoRowList.EQ_PIO_Load_REQ:
                            {
                                bool bLoad_REQ = PIOStatus_EQToRM_Load_Req;
                                DGV_NameCell.Value = $"PIO - Load REQ";
                                DGV_ValueCell.Value = bLoad_REQ ? "On" : "Off";
                                DGV_ValueCell.Style.BackColor = bLoad_REQ ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_PortStatusInfoRowList.EQ_PIO_Unload_REQ:
                            {
                                bool bUnload_REQ = PIOStatus_EQToRM_Unload_Req;
                                DGV_NameCell.Value = $"PIO - Unload REQ";
                                DGV_ValueCell.Value = bUnload_REQ ? "On" : "Off";
                                DGV_ValueCell.Style.BackColor = bUnload_REQ ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_PortStatusInfoRowList.EQ_PIO_Ready:
                            {
                                bool bReady = PIOStatus_EQToRM_Ready;
                                DGV_NameCell.Value = $"PIO - Ready";
                                DGV_ValueCell.Value = bReady ? "On" : "Off";
                                DGV_ValueCell.Style.BackColor = bReady ? Color.Lime : Color.White;
                            }
                            break;
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;

            }
        }
        public void Update_DGV_ErrorInfo(ref DataGridView DGV)
        {
            if (IsEQPort())
            {
                DGV.Visible = false;
                return;
            }
            else
                DGV.Visible = true;

            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case 0:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage($"DGV_GenerateTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_GenerateTime");
                        break;
                    case 1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_List"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_List");
                        break;
                    case 2:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ErrorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ErrorName");
                        break;
                    case 3:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Clear"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Clear");
                        break;
                    case 4:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ClearTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ClearTime");
                        break;
                }
            }

            DataGridViewFunc.AutoRowSize(DGV, 25, 23, 40);

            if (DGV.Rows.Count != Enum.GetNames(typeof(DGV_ErrorInfoRow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_ErrorInfoRow)).Length; nCount++)
                {
                    DGV_ErrorInfoRow eDGV_ErrorInfoRow = (DGV_ErrorInfoRow)nCount;
                    switch (eDGV_ErrorInfoRow)
                    {
                        case DGV_ErrorInfoRow.ErrorCode0:
                            DGV.Rows.Add(new string[] { string.Empty, "Error 0", string.Empty, string.Empty, string.Empty });
                            break;
                        case DGV_ErrorInfoRow.ErrorCode1:
                            DGV.Rows.Add(new string[] { string.Empty, "Error 1", string.Empty, string.Empty, string.Empty });
                            break;
                        case DGV_ErrorInfoRow.ErrorCode2:
                            DGV.Rows.Add(new string[] { string.Empty, "Error 2", string.Empty, string.Empty, string.Empty });
                            break;
                        case DGV_ErrorInfoRow.ErrorCode3:
                            DGV.Rows.Add(new string[] { string.Empty, "Error 3", string.Empty, string.Empty, string.Empty });
                            break;
                        case DGV_ErrorInfoRow.ErrorCode4:
                            DGV.Rows.Add(new string[] { string.Empty, "Error 4", string.Empty, string.Empty, string.Empty });
                            break;
                    }

                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(DGV_ErrorInfoRow)).Length; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_ErrorInfoColumn)).Length; nColumnCount++)
                    {
                        if (nColumnCount >= DGV.Columns.Count)
                            continue;

                        DGV_ErrorInfoRow eDGV_ErrorInfoRow = (DGV_ErrorInfoRow)nRowCount;
                        DGV_ErrorInfoColumn eDGV_ErrorInfoColumn = (DGV_ErrorInfoColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string value = string.Empty;
                        var AlarmList = GetAlarmList();

                        if(AlarmList == null)
                            continue;


                        switch (eDGV_ErrorInfoColumn)
                        {
                            case DGV_ErrorInfoColumn.ErrorGenTime:
                                value = AlarmList.Count > nRowCount ? AlarmList[nRowCount].GenerateTime : string.Empty;
                                break;

                            case DGV_ErrorInfoColumn.ErrorIndex:
                                value = $"Error {nRowCount}";
                                break;

                            case DGV_ErrorInfoColumn.ErrorCode:
                                value = AlarmList.Count > nRowCount ? AlarmList[nRowCount].AlarmComment : string.Empty;
                                bool bClear = AlarmList.Count > nRowCount ? AlarmList[nRowCount].bClear : false;
                                AlarmLevel eAlarmLevel = AlarmList.Count > nRowCount ? AlarmList[nRowCount].eAlarmLevel : AlarmLevel.None;
                                DGV_Cell.Style.BackColor = (value == "None" || value == string.Empty || bClear) ? Color.White : (eAlarmLevel == AlarmLevel.Warning) ? Color.Orange :Master.ErrorIntervalColor;
                                break;
                            case DGV_ErrorInfoColumn.ClearState:
                                value = AlarmList.Count > nRowCount ? (AlarmList[nRowCount].bClear ? "Clear" : string.Empty) : string.Empty;
                                DGV_Cell.Style.BackColor = value == "Clear" ? Color.Lime : DGV.Rows[nRowCount].DefaultCellStyle.BackColor;
                                break;

                            case DGV_ErrorInfoColumn.ErrorClearTime:
                                value = AlarmList.Count > nRowCount ? AlarmList[nRowCount].ClearTime : string.Empty;
                                break;
                        }

                        if ((string)DGV_Cell.Value != value)
                            DGV_Cell.Value = value;
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_InterlockInfo(ref DataGridView DGV)
        {
            if (IsEQPort())
            {
                DGV.Visible = false;
                return;
            }
            else
                DGV.Visible = true;

            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case 0:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_List"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_List");
                        break;
                    case 1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Status"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Status");
                        break;
                }
            }

            List<DGV_InterlockInfoRowList> UseRowList = new List<DGV_InterlockInfoRowList>();

            UseRowList.Add(DGV_InterlockInfoRowList.AutoRunEnable);

            if(GetParam().eTagReaderType != TagReader.TagReaderType.None)
                UseRowList.Add(DGV_InterlockInfoRowList.TagReaderConnection);

            UseRowList.Add(DGV_InterlockInfoRowList.PortPower);

            foreach(PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if(GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Shuttle_X)
                    UseRowList.Add(DGV_InterlockInfoRowList.Shuttle_X_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Shuttle_Z)
                    UseRowList.Add(DGV_InterlockInfoRowList.Shuttle_Z_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Shuttle_T)
                    UseRowList.Add(DGV_InterlockInfoRowList.Shuttle_T_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Buffer_LP_X)
                    UseRowList.Add(DGV_InterlockInfoRowList.Buffer_LP_X_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Buffer_LP_Z)
                    UseRowList.Add(DGV_InterlockInfoRowList.Buffer_LP_Z_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Buffer_LP_T)
                    UseRowList.Add(DGV_InterlockInfoRowList.Buffer_LP_T_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Buffer_OP_X)
                    UseRowList.Add(DGV_InterlockInfoRowList.Buffer_OP_X_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Buffer_OP_Z)
                    UseRowList.Add(DGV_InterlockInfoRowList.Buffer_OP_Z_AxisHomeDone);
                else if (GetMotionParam().IsServoType(ePortAxis) && ePortAxis == PortAxis.Buffer_OP_T)
                    UseRowList.Add(DGV_InterlockInfoRowList.Buffer_OP_T_AxisHomeDone);
            }

            if(IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
            {
                UseRowList.Add(DGV_InterlockInfoRowList.OP_Carrier_Status);
                UseRowList.Add(DGV_InterlockInfoRowList.OP_Carrier_ID);
                UseRowList.Add(DGV_InterlockInfoRowList.Shuttle_Carrier_Status);
                UseRowList.Add(DGV_InterlockInfoRowList.Shuttle_Carrier_ID);
                UseRowList.Add(DGV_InterlockInfoRowList.LP_Carrier_Status);
                UseRowList.Add(DGV_InterlockInfoRowList.LP_Carrier_ID);
            }
            else if(IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
            {
                UseRowList.Add(DGV_InterlockInfoRowList.OP_Carrier_Status);
                UseRowList.Add(DGV_InterlockInfoRowList.OP_Carrier_ID);
                UseRowList.Add(DGV_InterlockInfoRowList.Shuttle_Carrier_Status);
                UseRowList.Add(DGV_InterlockInfoRowList.Shuttle_Carrier_ID);
            }
            else if(IsBufferControlPort() && GetPortOperationMode() == PortOperationMode.Conveyor)
            {
                UseRowList.Add(DGV_InterlockInfoRowList.OP_Carrier_Status);
                UseRowList.Add(DGV_InterlockInfoRowList.OP_Carrier_ID);

                foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
                {
                    if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP1)
                    {
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP1_Carrier_Status);
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP1_Carrier_ID);
                    }
                    else if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP2)
                    {
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP2_Carrier_Status);
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP2_Carrier_ID);
                    }
                    else if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP3)
                    {
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP3_Carrier_Status);
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP3_Carrier_ID);
                    }
                    else if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP4)
                    {
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP4_Carrier_Status);
                        UseRowList.Add(DGV_InterlockInfoRowList.CV_BP4_Carrier_ID);
                    }
                }
                UseRowList.Add(DGV_InterlockInfoRowList.LP_Carrier_Status);
                UseRowList.Add(DGV_InterlockInfoRowList.LP_Carrier_ID);
            }

            UseRowList.Add(DGV_InterlockInfoRowList.RM_Carrier_ID);
            UseRowList.Add(DGV_InterlockInfoRowList.LightCurtain);
            UseRowList.Add(DGV_InterlockInfoRowList.ForkDetect);

            if(GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.OHT)
                UseRowList.Add(DGV_InterlockInfoRowList.HoistDetect);

            if (UseRowList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();
            }
            else
            {
                if (DGV.Rows.Count != UseRowList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < UseRowList.Count; nCount++)
                        DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 23, 40);

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_InterlockInfoRowList eDGV_InterlockInfoRow = UseRowList[nRowCount];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_ValueCell = DGV.Rows[nRowCount].Cells[1];


                    switch (eDGV_InterlockInfoRow)
                    {
                        case DGV_InterlockInfoRowList.AutoRunEnable:
                            {
                                bool bAutoRunEnable = Status_RunEnable;
                                bool bAutoRun = IsAutoControlRun();
                                bool bCycleRun = IsAutoManualCycleRun();
                                DGV_NameCell.Value = "Auto Run Enable";
                                DGV_ValueCell.Value = bAutoRunEnable ? "Enable" : "Disable";
                                DGV_ValueCell.Style.BackColor = bAutoRunEnable ? Color.Lime : (bAutoRun || bCycleRun) ? Color.Orange : Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.TagReaderConnection:
                            {
                                DGV_NameCell.Value = "Tag Reader Connection";
                                DGV_ValueCell.Value = m_TagReader_Interface.IsConnected() ? "Connection" : "Disconnection";
                                DGV_ValueCell.Style.BackColor = m_TagReader_Interface.IsConnected() ? Color.Lime : Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.PortPower:
                            {
                                DGV_NameCell.Value = "Port Power";
                                DGV_ValueCell.Value = IsPortPowerOn() ? "On" : "Off";
                                DGV_ValueCell.Style.BackColor = IsPortPowerOn() ? Color.Lime : Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.Shuttle_X_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Shuttle_Z_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Shuttle_T_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Buffer_LP_X_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Buffer_LP_Z_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Buffer_LP_T_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Buffer_OP_X_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Buffer_OP_Z_AxisHomeDone:
                        case DGV_InterlockInfoRowList.Buffer_OP_T_AxisHomeDone:
                            {
                                PortAxis ePortAxis = PortAxis.Shuttle_X;
                                if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Shuttle_X_AxisHomeDone)
                                { DGV_NameCell.Value = "Shuttle X-Axis Home"; ePortAxis = PortAxis.Shuttle_X; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Shuttle_Z_AxisHomeDone)
                                { DGV_NameCell.Value = "Shuttle Z-Axis Home"; ePortAxis = PortAxis.Shuttle_Z; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Shuttle_T_AxisHomeDone)
                                { DGV_NameCell.Value = "Shuttle T-Axis Home"; ePortAxis = PortAxis.Shuttle_T; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Buffer_LP_X_AxisHomeDone)
                                { DGV_NameCell.Value = "Buffer LP X-Axis Home"; ePortAxis = PortAxis.Buffer_LP_X; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Buffer_LP_Z_AxisHomeDone)
                                { DGV_NameCell.Value = "Buffer LP Z-Axis Home"; ePortAxis = PortAxis.Buffer_LP_Z; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Buffer_LP_T_AxisHomeDone)
                                { DGV_NameCell.Value = "Buffer LP T-Axis Home"; ePortAxis = PortAxis.Buffer_LP_T; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Buffer_OP_X_AxisHomeDone)
                                { DGV_NameCell.Value = "Buffer OP X-Axis Home"; ePortAxis = PortAxis.Buffer_OP_X; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Buffer_OP_Z_AxisHomeDone)
                                { DGV_NameCell.Value = "Buffer OP Z-Axis Home"; ePortAxis = PortAxis.Buffer_OP_Z; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.Buffer_OP_T_AxisHomeDone)
                                { DGV_NameCell.Value = "Buffer OP T-Axis Home"; ePortAxis = PortAxis.Buffer_OP_T; }


                                DGV_ValueCell.Value = ServoCtrl_GetHomeDone(ePortAxis) ? "Done" : "Not Homed";
                                DGV_ValueCell.Style.BackColor = ServoCtrl_GetHomeDone(ePortAxis) ? Color.Lime : Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.OP_Carrier_Status:
                            {
                                DGV_NameCell.Value = "OP Carrier Status";

                                bool bEnable = Carrier_CheckOP_ExistProduct(true, true);
                                string value = OP_CarrierID;

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.OP_Carrier_ID:
                            {
                                DGV_NameCell.Value = "OP Carrier ID";

                                bool bEnable = Carrier_CheckOP_ExistProduct(true, true);
                                string value = OP_CarrierID;

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.Shuttle_Carrier_Status:
                            {
                                DGV_NameCell.Value = "Shuttle Carrier Status";

                                bool bEnable = Carrier_CheckShuttle_ExistProduct(true);
                                string value = Carrier_GetBP_CarrierID(0);

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.Shuttle_Carrier_ID:
                            {
                                DGV_NameCell.Value = "Shuttle Carrier ID";

                                bool bEnable = Carrier_CheckShuttle_ExistProduct(true);
                                string value = Carrier_GetBP_CarrierID(0);

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.LP_Carrier_Status:
                            {
                                DGV_NameCell.Value = "LP Carrier Status";

                                bool bEnable = Carrier_CheckLP_ExistProduct(true, true);
                                string value = LP_CarrierID;

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.LP_Carrier_ID:
                            {
                                DGV_NameCell.Value = "LP Carrier ID";

                                bool bEnable = Carrier_CheckLP_ExistProduct(true, true);
                                string value = LP_CarrierID;

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.CV_BP1_Carrier_Status:
                        case DGV_InterlockInfoRowList.CV_BP2_Carrier_Status:
                        case DGV_InterlockInfoRowList.CV_BP3_Carrier_Status:
                        case DGV_InterlockInfoRowList.CV_BP4_Carrier_Status:
                            {
                                BufferCV eBufferCV = BufferCV.Buffer_BP1;
                                if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP1_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP1 Carrier Status"; eBufferCV = BufferCV.Buffer_BP1; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP2_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP2 Carrier Status"; eBufferCV = BufferCV.Buffer_BP2; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP3_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP3 Carrier Status"; eBufferCV = BufferCV.Buffer_BP3; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP4_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP4 Carrier Status"; eBufferCV = BufferCV.Buffer_BP4; }

                                bool bEnable = Carrier_CheckCVBP_ExistProduct((int)eBufferCV - 2, true);
                                string value = Carrier_GetBP_CarrierID((int)eBufferCV - 2);

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.CV_BP1_Carrier_ID:
                        case DGV_InterlockInfoRowList.CV_BP2_Carrier_ID:
                        case DGV_InterlockInfoRowList.CV_BP3_Carrier_ID:
                        case DGV_InterlockInfoRowList.CV_BP4_Carrier_ID:
                            {
                                BufferCV eBufferCV = BufferCV.Buffer_BP1;
                                if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP1_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP1 Carrier ID"; eBufferCV = BufferCV.Buffer_BP1; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP2_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP2 Carrier ID"; eBufferCV = BufferCV.Buffer_BP2; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP3_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP3 Carrier ID"; eBufferCV = BufferCV.Buffer_BP3; }
                                else if (eDGV_InterlockInfoRow == DGV_InterlockInfoRowList.CV_BP4_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP4 Carrier ID"; eBufferCV = BufferCV.Buffer_BP4; }

                                bool bEnable = Carrier_CheckCVBP_ExistProduct((int)eBufferCV - 2, true);
                                string value = Carrier_GetBP_CarrierID((int)eBufferCV - 2);

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_InterlockInfoRowList.RM_Carrier_ID:
                            {
                                DGV_NameCell.Value = "RM Carrier ID";
                                DGV_ValueCell.Value = Carrier_GetRMToPort_RecvMapCarrierID();
                                DGV_ValueCell.Style.BackColor = Carrier_GetRMToPort_RecvMapCarrierID() != string.Empty ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_InterlockInfoRowList.LightCurtain:
                            {
                                DGV_NameCell.Value = "Light Curtain";
                                DGV_ValueCell.Value = Sensor_LightCurtain ? "On" : "Off";
                                DGV_ValueCell.Style.BackColor = Sensor_LightCurtain ? Color.Orange : Color.Lime;
                            }
                            break;
                        case DGV_InterlockInfoRowList.ForkDetect:
                            {
                                DGV_NameCell.Value = "Fork Detect";
                                DGV_ValueCell.Value = Sensor_OP_Fork_Detect ? "On" : "Off";
                                DGV_ValueCell.Style.BackColor = Sensor_OP_Fork_Detect ? Color.Orange : Color.Lime;
                            }
                            break;
                        case DGV_InterlockInfoRowList.HoistDetect:
                            {
                                DGV_NameCell.Value = "Hoist Detect";
                                DGV_ValueCell.Value = Sensor_LP_Hoist_Detect ? "On" : "Off";
                                DGV_ValueCell.Style.BackColor = Sensor_LP_Hoist_Detect ? Color.Orange : Color.Lime;
                            }
                            break;
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }

        public void Update_DGV_AutoRunStatus(ref DataGridView DGV)
        {
            if (IsEQPort())
            {
                DGV.Visible = false;
                return;
            }
            else
                DGV.Visible = true;

            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case 0:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_List"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_List");
                        break;
                    case 1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Status"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Status");
                        break;
                }
            }

            List<DGV_AutoRunStatusRowList> UseRowList = new List<DGV_AutoRunStatusRowList>();

            UseRowList.Add(DGV_AutoRunStatusRowList.AutoRunType);
            UseRowList.Add(DGV_AutoRunStatusRowList.RunningStatus);

            if(IsShuttleControlPort() || IsBufferControlPort())
                UseRowList.Add(DGV_AutoRunStatusRowList.CycleRunProgress);

            UseRowList.Add(DGV_AutoRunStatusRowList.RunningTime);
            UseRowList.Add(DGV_AutoRunStatusRowList.ErrorName);

            if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
            {
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Step);
                UseRowList.Add(DGV_AutoRunStatusRowList.BP_Step);
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Step);
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Carrier_Status);
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.Shuttle_Carrier_Status);
                UseRowList.Add(DGV_AutoRunStatusRowList.Shuttle_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Carrier_Status);
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.RM_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Step_Timer);
                UseRowList.Add(DGV_AutoRunStatusRowList.BP_Step_Timer);
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Step_Timer);
            }
            else if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
            {
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Step);
                UseRowList.Add(DGV_AutoRunStatusRowList.BP_Step);
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Carrier_Status);
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.Shuttle_Carrier_Status);
                UseRowList.Add(DGV_AutoRunStatusRowList.Shuttle_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.RM_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Step_Timer);
                UseRowList.Add(DGV_AutoRunStatusRowList.BP_Step_Timer);
            }
            else if (IsBufferControlPort() && GetPortOperationMode() == PortOperationMode.Conveyor)
            {
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Step);
                if(GetMotionParam().IsBPCVUsed())
                    UseRowList.Add(DGV_AutoRunStatusRowList.BP_Step);
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Step);

                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Carrier_Status);
                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Carrier_ID);

                foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
                {
                    if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP1)
                    {
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP1_Carrier_Status);
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP1_Carrier_ID);
                    }
                    else if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP2)
                    {
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP2_Carrier_Status);
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP2_Carrier_ID);
                    }
                    else if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP3)
                    {
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP3_Carrier_Status);
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP3_Carrier_ID);
                    }
                    else if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV == BufferCV.Buffer_BP4)
                    {
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP4_Carrier_Status);
                        UseRowList.Add(DGV_AutoRunStatusRowList.CV_BP4_Carrier_ID);
                    }
                }
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Carrier_Status);
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Carrier_ID);
                UseRowList.Add(DGV_AutoRunStatusRowList.RM_Carrier_ID);

                if (GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    UseRowList.Add(DGV_AutoRunStatusRowList.Port_To_OMRON_Carrier_ID);
                    UseRowList.Add(DGV_AutoRunStatusRowList.OMRON_To_Port_Carrier_ID);
                }

                UseRowList.Add(DGV_AutoRunStatusRowList.OP_Step_Timer);
                if (GetMotionParam().IsBPCVUsed())
                    UseRowList.Add(DGV_AutoRunStatusRowList.BP_Step_Timer);
                UseRowList.Add(DGV_AutoRunStatusRowList.LP_Step_Timer);
            }
            else if (IsEQPort())
            {
                UseRowList.Add(DGV_AutoRunStatusRowList.RM_Carrier_ID);
            }

            if (!IsEQPort())
            {
                UseRowList.Add(DGV_AutoRunStatusRowList.RM_PIO_Timer);
                if (GetPortOperationMode() == PortOperationMode.AGV || GetPortOperationMode() == PortOperationMode.OHT || GetPortOperationMode() == PortOperationMode.Conveyor)
                    UseRowList.Add(DGV_AutoRunStatusRowList.Equip_PIO_Timer);

                UseRowList.Add(DGV_AutoRunStatusRowList.RM_Fork_Detect_Timer);

                if (GetPortOperationMode() == PortOperationMode.OHT)
                    UseRowList.Add(DGV_AutoRunStatusRowList.OHT_Hoist_Detect_Timer);

                if (GetPortOperationMode() == PortOperationMode.MGV || GetPortOperationMode() == PortOperationMode.AGV || GetPortOperationMode() == PortOperationMode.Conveyor)
                {
                    if (GetParam().ePortType != PortType.Conveyor_OMRON)
                    {
                        UseRowList.Add(DGV_AutoRunStatusRowList.Port_Area_Timer);
                        UseRowList.Add(DGV_AutoRunStatusRowList.Port_Area_Release_Timer);
                    }
                }

                if (GetPortOperationMode() == PortOperationMode.MGV || GetPortOperationMode() == PortOperationMode.AGV)
                {
                    UseRowList.Add(DGV_AutoRunStatusRowList.Port_Area_And_ShuttleMoveTimer);
                }
            }


            if (UseRowList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();
            }
            else
            {
                if (DGV.Rows.Count != UseRowList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < UseRowList.Count; nCount++)
                        DGV.Rows.Add();
                }
                DataGridViewFunc.AutoRowSize(DGV, 25, 20, 40);

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_AutoRunStatusRowList eDGV_AutoRunStatusRowList = UseRowList[nRowCount];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_ValueCell = DGV.Rows[nRowCount].Cells[1];


                    switch (eDGV_AutoRunStatusRowList)
                    {
                        case DGV_AutoRunStatusRowList.AutoRunType:
                            {
                                DGV_NameCell.Value = "Auto Run Type";

                                if(IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                                    DGV_ValueCell.Value = "Shuttle And 2 Buffer Control";
                                else if (IsShuttleControlPort() && GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                                    DGV_ValueCell.Value = "Shuttle And 1 Buffer Control";
                                else if (IsBufferControlPort() && GetMotionParam().IsBPCVUsed() && GetParam().ePortType == PortType.Conveyor_AGV)
                                    DGV_ValueCell.Value = "LP, OP, BP Conveyor Control";
                                else if (IsBufferControlPort() && !GetMotionParam().IsBPCVUsed() && GetParam().ePortType == PortType.Conveyor_AGV)
                                    DGV_ValueCell.Value = "LP, OP Conveyor Control";
                                else if(IsBufferControlPort() && GetParam().ePortType == PortType.Conveyor_OMRON)
                                    DGV_ValueCell.Value = "LP, OP Conveyor Control(DIEBANK)";
                                else if (IsEQPort())
                                    DGV_ValueCell.Value = "EQ Type Control";
                            }
                            break;
                        case DGV_AutoRunStatusRowList.RunningStatus:
                            {
                                DGV_NameCell.Value = "Running Status";

                                bool bAutoRun = IsAutoControlRun();
                                bool bCycleRun = IsAutoManualCycleRun();

                                if (bAutoRun)
                                    DGV_ValueCell.Value = "Auto Running";
                                else if (bCycleRun)
                                    DGV_ValueCell.Value = "Cycle Running";
                                else 
                                    DGV_ValueCell.Value = "Idle";

                                DGV_ValueCell.Style.BackColor = (bAutoRun || bCycleRun) ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.CycleRunProgress:
                            {
                                DGV_NameCell.Value = "Cycle Progress";

                                bool bAutoRun = IsAutoControlRun();
                                bool bCycleRun = IsAutoManualCycleRun();

                                if (bAutoRun)
                                    DGV_ValueCell.Value = "Infinity";
                                else if (bCycleRun)
                                {
                                    double ProgressPercent = ((double)(m_CycleControlProgressCount + 1) / (double)m_CycleControlSetCount) * 100.0;
                                    if (double.IsNaN(ProgressPercent))
                                        ProgressPercent = 0;

                                    string Text = $"{ m_CycleControlProgressCount + 1 } / { m_CycleControlSetCount} ({ ProgressPercent.ToString("0.0")}%)";
                                    DGV_ValueCell.Value = Text;
                                }
                            }
                            break;
                        case DGV_AutoRunStatusRowList.RunningTime:
                            {
                                DGV_NameCell.Value = "Running Time";
                                DGV_ValueCell.Value = $"{StopWatchFunc.GetRunningTime(m_AutoRunProgressTime)}";
                            }
                            break;
                        case DGV_AutoRunStatusRowList.ErrorName:
                            {
                                DGV_NameCell.Value = "Recent Error";

                                string ErrorStr = GetRecentErrorCodeStr();
                                DGV_ValueCell.Value = ErrorStr == "None" ? string.Empty : ErrorStr;
                                DGV_ValueCell.Style.BackColor = ErrorStr == "None" ? Color.White : Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.OP_Step:
                            {
                                string StepStr = Get_OP_AutoControlStepToStr();
                                int StepIndex = Get_OP_AutoControlStep();
                                DGV_NameCell.Value = "OP Auto Step";
                                DGV_ValueCell.Value = $"{StepStr}[{StepIndex}]";
                            }
                            break;
                        case DGV_AutoRunStatusRowList.BP_Step:
                            {
                                string StepStr = Get_BP_AutoControlStepToStr();
                                int StepIndex = Get_BP_AutoControlStep();
                                DGV_NameCell.Value = "BP Auto Step";
                                DGV_ValueCell.Value = $"{StepStr}[{StepIndex}]";
                            }
                            break;
                        case DGV_AutoRunStatusRowList.LP_Step:
                            {
                                string StepStr = Get_LP_AutoControlStepToStr();
                                int StepIndex = Get_LP_AutoControlStep();
                                DGV_NameCell.Value = "LP Auto Step";
                                DGV_ValueCell.Value = $"{StepStr}[{StepIndex}]";
                            }
                            break;
                        case DGV_AutoRunStatusRowList.OP_Carrier_Status:
                            {
                                DGV_NameCell.Value = "OP Carrier Status";
                                bool bEnable = false;
                                if(GetParam().ePortType != PortType.Conveyor_OMRON)
                                    bEnable = Carrier_CheckOP_ExistProduct(true, true);
                                else
                                {
                                    if(IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                                    {
                                        bEnable = Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2;
                                    }
                                    else if(IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                                    {
                                        bEnable = Carrier_CheckOP_ExistProduct(true, true);
                                    }
                                }
                                string value = OP_CarrierID;

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.OP_Carrier_ID:
                            {
                                DGV_NameCell.Value = "OP Carrier ID";

                                bool bEnable = false;
                                if (GetParam().ePortType != PortType.Conveyor_OMRON)
                                    bEnable = Carrier_CheckOP_ExistProduct(true, true);
                                else
                                {
                                    if (IsZAxisPos_UP(PortAxis.Buffer_OP_Z))
                                    {
                                        bEnable = Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2;
                                    }
                                    else if (IsZAxisPos_DOWN(PortAxis.Buffer_OP_Z))
                                    {
                                        bEnable = Carrier_CheckOP_ExistProduct(true, true);
                                    }
                                }
                                string value = OP_CarrierID;

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.Shuttle_Carrier_Status:
                            {
                                DGV_NameCell.Value = "Shuttle Carrier Status";

                                bool bEnable = Carrier_CheckShuttle_ExistProduct(true);
                                string value = Carrier_GetBP_CarrierID(0);

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.Shuttle_Carrier_ID:
                            {
                                DGV_NameCell.Value = "Shuttle Carrier ID";

                                bool bEnable = Carrier_CheckShuttle_ExistProduct(true);
                                string value = Carrier_GetBP_CarrierID(0);

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.LP_Carrier_Status:
                            {
                                DGV_NameCell.Value = "LP Carrier Status";

                                bool bEnable = Carrier_CheckLP_ExistProduct(true, true);
                                string value = LP_CarrierID;

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.LP_Carrier_ID:
                            {
                                DGV_NameCell.Value = "LP Carrier ID";

                                bool bEnable = Carrier_CheckLP_ExistProduct(true, true);
                                string value = LP_CarrierID;

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.CV_BP1_Carrier_Status:
                        case DGV_AutoRunStatusRowList.CV_BP2_Carrier_Status:
                        case DGV_AutoRunStatusRowList.CV_BP3_Carrier_Status:
                        case DGV_AutoRunStatusRowList.CV_BP4_Carrier_Status:
                            {
                                BufferCV eBufferCV = BufferCV.Buffer_BP1;
                                if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP1_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP1 Carrier Status"; eBufferCV = BufferCV.Buffer_BP1; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP2_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP2 Carrier Status"; eBufferCV = BufferCV.Buffer_BP2; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP3_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP3 Carrier Status"; eBufferCV = BufferCV.Buffer_BP3; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP4_Carrier_Status)
                                { DGV_NameCell.Value = "CV BP4 Carrier Status"; eBufferCV = BufferCV.Buffer_BP4; }

                                bool bEnable = Carrier_CheckCVBP_ExistProduct((int)eBufferCV - 2, true);
                                string value = Carrier_GetBP_CarrierID((int)eBufferCV - 2);

                                DGV_ValueCell.Value = bEnable ? "Exist" : "Not exist";
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.CV_BP1_Carrier_ID:
                        case DGV_AutoRunStatusRowList.CV_BP2_Carrier_ID:
                        case DGV_AutoRunStatusRowList.CV_BP3_Carrier_ID:
                        case DGV_AutoRunStatusRowList.CV_BP4_Carrier_ID:
                            {
                                BufferCV eBufferCV = BufferCV.Buffer_BP1;
                                if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP1_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP1 Carrier ID"; eBufferCV = BufferCV.Buffer_BP1; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP2_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP2 Carrier ID"; eBufferCV = BufferCV.Buffer_BP2; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP3_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP3 Carrier ID"; eBufferCV = BufferCV.Buffer_BP3; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.CV_BP4_Carrier_ID)
                                { DGV_NameCell.Value = "CV BP4 Carrier ID"; eBufferCV = BufferCV.Buffer_BP4; }

                                bool bEnable = Carrier_CheckCVBP_ExistProduct((int)eBufferCV - 2, true);
                                string value = Carrier_GetBP_CarrierID((int)eBufferCV - 2);

                                DGV_ValueCell.Value = value;
                                if (value != string.Empty && bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.Lime;
                                else if (value == string.Empty && !bEnable)
                                    DGV_ValueCell.Style.BackColor = Color.White;
                                else
                                    DGV_ValueCell.Style.BackColor = Master.ErrorIntervalColor;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.RM_Carrier_ID:
                            {
                                DGV_NameCell.Value = "RM Carrier ID";
                                DGV_ValueCell.Value = Carrier_GetRMToPort_RecvMapCarrierID();
                                DGV_ValueCell.Style.BackColor = Carrier_GetRMToPort_RecvMapCarrierID() != string.Empty ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.Port_To_OMRON_Carrier_ID:
                            {
                                DGV_NameCell.Value = "Port -> OMRON Carrier ID";
                                DGV_ValueCell.Value = Port_To_OMRON_CarrierID;
                                DGV_ValueCell.Style.BackColor = Port_To_OMRON_CarrierID != string.Empty ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.OMRON_To_Port_Carrier_ID:
                            {
                                DGV_NameCell.Value = "OMRON -> Port Carrier ID";
                                DGV_ValueCell.Value = OMRON_To_Port_CarrierID;
                                DGV_ValueCell.Style.BackColor = OMRON_To_Port_CarrierID != string.Empty ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_AutoRunStatusRowList.OP_Step_Timer:
                        case DGV_AutoRunStatusRowList.BP_Step_Timer:
                        case DGV_AutoRunStatusRowList.LP_Step_Timer:
                        case DGV_AutoRunStatusRowList.EQ_Step_Timer:
                        case DGV_AutoRunStatusRowList.RM_PIO_Timer:
                        case DGV_AutoRunStatusRowList.Equip_PIO_Timer:
                        case DGV_AutoRunStatusRowList.RM_Fork_Detect_Timer:
                        case DGV_AutoRunStatusRowList.OHT_Hoist_Detect_Timer:
                        case DGV_AutoRunStatusRowList.Port_Area_Timer:
                        case DGV_AutoRunStatusRowList.Port_Area_Release_Timer:
                        case DGV_AutoRunStatusRowList.Port_Area_And_ShuttleMoveTimer:
                            {
                                WatchdogList eWatchdogList = WatchdogList.OP_Step_Timer;

                                bool bOHT = GetPortOperationMode() == PortOperationMode.OHT;
                                bool bAGV = GetPortOperationMode() == PortOperationMode.AGV;
                                bool bConveyor = GetPortOperationMode() == PortOperationMode.Conveyor;
                                string EquipName = bOHT ? "OHT" : (bAGV || bConveyor) ? "AGV" : string.Empty;

                                if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.OP_Step_Timer)
                                { DGV_NameCell.Value = "OP Step Timer"; eWatchdogList = WatchdogList.OP_Step_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.BP_Step_Timer)
                                { DGV_NameCell.Value = "BP Step Timer"; eWatchdogList = WatchdogList.BP_Step_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.LP_Step_Timer)
                                { DGV_NameCell.Value = "LP Step Timer"; eWatchdogList = WatchdogList.LP_Step_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.EQ_Step_Timer)
                                { DGV_NameCell.Value = "EQ Step Timer"; eWatchdogList = WatchdogList.EQ_Step_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.RM_PIO_Timer)
                                { DGV_NameCell.Value = "RackMaster PIO Timer"; eWatchdogList = WatchdogList.RackMaster_PIO_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.Equip_PIO_Timer)
                                { DGV_NameCell.Value = $"{EquipName} PIO Timer"; eWatchdogList = WatchdogList.AGVorOHT_PIO_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.RM_Fork_Detect_Timer)
                                { DGV_NameCell.Value = $"Fork Detect Timer"; eWatchdogList = WatchdogList.RM_ForkDetectTimer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.OHT_Hoist_Detect_Timer)
                                { DGV_NameCell.Value = $"Hoist Detect Timer"; eWatchdogList = WatchdogList.OHT_HoistDetectTimer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.Port_Area_Timer)
                                { DGV_NameCell.Value = $"Light Curtain Detect Timer"; eWatchdogList = WatchdogList.PortArea_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.Port_Area_Release_Timer)
                                { DGV_NameCell.Value = $"Light Curtain Release Timer"; eWatchdogList = WatchdogList.PortArea_Release_Timer; }
                                else if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.Port_Area_And_ShuttleMoveTimer)
                                { DGV_NameCell.Value = $"Port Moving Detect Timer"; eWatchdogList = WatchdogList.PortArea_And_ShuttleMovingErrorTimer; }

                                DGV_ValueCell.Value = $"{Watchdog_GetProgressTime(eWatchdogList)} / Detect Time : {Watchdog_Get_DetectTime(eWatchdogList)} msec";

                                if (eDGV_AutoRunStatusRowList == DGV_AutoRunStatusRowList.Port_Area_Release_Timer)
                                    DGV_ValueCell.Style.BackColor = Watchdog_GetColor(eWatchdogList, true);
                                else
                                    DGV_ValueCell.Style.BackColor = Watchdog_GetColor(eWatchdogList, false);
                            }
                            break;
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Event_DGV_ShuttleAxisStatus_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex >= 0 &&
                senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (e.ColumnIndex == (int)DGV_MotionServoStatusColumn.AlarmCode)
                {
                    DataGridViewCell DGV_AxisCell = senderGrid.Rows[e.RowIndex].Cells[(int)DGV_MotionServoStatusColumn.Axis];

                    if (!string.IsNullOrEmpty((string)DGV_AxisCell.Value))
                    {
                        string AxisType = (string)DGV_AxisCell.Value;

                        for(int nCount = (int)PortAxis.Shuttle_X; nCount <= (int)PortAxis.Buffer_OP_T; nCount++)
                        {
                            PortAxis ePortAxis = (PortAxis)nCount;
                            if (AxisType.Contains($"{ePortAxis}"))
                            {
                                ManualOpen(ePortAxis);
                            }
                        }
                    }
                }
            }
            senderGrid.CurrentCell = null;
        }

        public void Event_DGV_WatchdogSettings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataGridViewCell DGV_Cell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

                string value = (string)DGV_Cell.Value;

                if (value.Contains("Set"))
                {
                    WatchdogList eWatchdog = (WatchdogList)e.RowIndex;
                    DataGridViewCell DGV_SetValueCell = senderGrid.Rows[e.RowIndex].Cells[(int)DGV_WatchdogSettingsColumn.SetValue];

                    try
                    {
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{GetParam().ID}] Watchdog Parameter {eWatchdog} Set Click");
                        string Text = (string)DGV_SetValueCell.Value;
                        if (!string.IsNullOrEmpty(Text))
                        {
                            int DetectTime = Convert.ToInt32(Text);
                            Watchdog_SetParam_DetectTime(eWatchdog, DetectTime);
                            Watchdog_Refresh_DetectTime();
                        }
                        else
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Watchdog_SetParam_DetectTime(eWatchdog, Watchdog_GetParam_DetectTime(eWatchdog));
                            DGV_SetValueCell.Value = Watchdog_GetParam_DetectTime(eWatchdog).ToString();
                        }
                    }
                    catch
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Watchdog_SetParam_DetectTime(eWatchdog, Watchdog_GetParam_DetectTime(eWatchdog));
                        DGV_SetValueCell.Value = Watchdog_GetParam_DetectTime(eWatchdog).ToString();
                    }
                }
            }
        }
        public void Update_DGV_Buffer1PIOStatus(ref DataGridView DGV)
        {
            if ((string)DGV.Tag != GetParam().ID)
            {
                DGV.Rows.Clear();
                DGV.Tag = GetParam().ID;
            }

            if (GetPortOperationMode() == PortOperationMode.MGV)
            {
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Equip_To_Port].HeaderText = $"{GetPortOperationMode()} -> {SynusLangPack.GetLanguage("DGV_Port")}";
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Port_To_Equip].HeaderText = $"{SynusLangPack.GetLanguage("DGV_Port")} -> {GetPortOperationMode()}";

                if (DGV.Rows.Count != 1)
                {
                    if (DGV.Rows.Count > 0)
                        DGV.Rows.Clear();

                    DGV.Rows.Add();
                    DataGridViewFunc.SetSize(DGV, 25, 23, 0);
                }
                else
                {
                    for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                    {
                        for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_Buffer1PIOStatusColumn)).Length; nColumnCount++)
                        {
                            DGV_Buffer1PIOStatusColumn eDGV_Buffer2PIOStatusColumn = (DGV_Buffer1PIOStatusColumn)nColumnCount;
                            DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                            bool bEnable = false;

                            switch (eDGV_Buffer2PIOStatusColumn)
                            {
                                case DGV_Buffer1PIOStatusColumn.Equip_To_Port:
                                    if (nRowCount == 0)
                                    {
                                        bEnable = Sensor_LightCurtain;
                                        DGV_Cell.Value = $"Light Curtain";
                                    }
                                    break;
                            }

                            DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                        }
                    }
                }
            }
            else if (GetPortOperationMode() == PortOperationMode.Conveyor && GetParam().ePortType == PortType.Conveyor_AGV)
            {
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Equip_To_Port].HeaderText = $"AGV -> {SynusLangPack.GetLanguage("DGV_Port")}";
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Port_To_Equip].HeaderText = $"{SynusLangPack.GetLanguage("DGV_Port")} -> AGV";

                if (DGV.Rows.Count != 6)
                {
                    if (DGV.Rows.Count > 0)
                        DGV.Rows.Clear();

                    for(int nCount = 0; nCount < 6; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 0);
                }
                else
                {
                    for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                    {
                        for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_Buffer1PIOStatusColumn)).Length; nColumnCount++)
                        {
                            DGV_Buffer1PIOStatusColumn eDGV_Buffer2PIOStatusColumn = (DGV_Buffer1PIOStatusColumn)nColumnCount;
                            DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                            bool bEnable = false;

                            switch (eDGV_Buffer2PIOStatusColumn)
                            {
                                case DGV_Buffer1PIOStatusColumn.Equip_To_Port:
                                    if (nRowCount == 0) //VALID
                                    {
                                        bEnable = PIOStatus_AGVToPort_Valid;
                                        DGV_Cell.Value = $"VALID";
                                    }
                                    else if (nRowCount == 1) //CS0
                                    {
                                        bEnable = PIOStatus_AGVToPort_CS0;
                                        DGV_Cell.Value = $"CS0";
                                    }
                                    else if (nRowCount == 2) //TR_REQ
                                    {
                                        bEnable = PIOStatus_AGVToPort_TR_Req;
                                        DGV_Cell.Value = $"TR_REQ";
                                    }
                                    else if (nRowCount == 3) //BUSY
                                    {
                                        bEnable = PIOStatus_AGVToPort_Busy;
                                        DGV_Cell.Value = $"BUSY";
                                    }
                                    else if (nRowCount == 4) //COMPLETE
                                    {
                                        bEnable = PIOStatus_AGVToPort_Complete;
                                        DGV_Cell.Value = $"COMPLETE";
                                    }
                                    else if (nRowCount == 5) //Light Curtain
                                    {
                                        bEnable = Sensor_LightCurtain;
                                        DGV_Cell.Value = $"Light Curtain";
                                    }
                                    break;
                                case DGV_Buffer1PIOStatusColumn.Port_To_Equip:
                                    if (nRowCount == 0) //L-REQ
                                    {
                                        bEnable = PIOStatus_PortToAGV_Load_Req;
                                        DGV_Cell.Value = $"L-REQ";
                                    }
                                    else if (nRowCount == 1) //UL-REQ
                                    {
                                        bEnable = PIOStatus_PortToAGV_Unload_Req;
                                        DGV_Cell.Value = $"UL-REQ";
                                    }
                                    else if (nRowCount == 2) //READY
                                    {
                                        bEnable = PIOStatus_PortToAGV_Ready;
                                        DGV_Cell.Value = $"READY";
                                    }
                                    else if (nRowCount == 3) //ES
                                    {
                                        bEnable = PIOStatus_PortToAGV_ES;
                                        DGV_Cell.Value = $"ES";
                                    }
                                    break;
                            }

                            DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                        }
                    }
                }
            }
            else if (GetPortOperationMode() == PortOperationMode.Conveyor && GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Equip_To_Port].HeaderText = $"Omron -> {SynusLangPack.GetLanguage("DGV_Port")}";
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Port_To_Equip].HeaderText = $"{SynusLangPack.GetLanguage("DGV_Port")} -> Omron";

                if (DGV.Rows.Count != 5)
                {
                    if (DGV.Rows.Count > 0)
                        DGV.Rows.Clear();

                    for (int nCount = 0; nCount < 5; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 0);
                }
                else
                {
                    for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                    {
                        for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_Buffer1PIOStatusColumn)).Length; nColumnCount++)
                        {
                            DGV_Buffer1PIOStatusColumn eDGV_Buffer2PIOStatusColumn = (DGV_Buffer1PIOStatusColumn)nColumnCount;
                            DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                            bool bEnable = false;

                            switch (eDGV_Buffer2PIOStatusColumn)
                            {
                                case DGV_Buffer1PIOStatusColumn.Equip_To_Port:
                                    if (nRowCount == 0) //TR_REQ
                                    {
                                        bEnable = PIOStatus_OMRONToPort_Load_REQ;
                                        DGV_Cell.Value = $"L-REQ";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 1) //BUSY
                                    {
                                        bEnable = PIOStatus_OMRONToPort_Unload_REQ;
                                        DGV_Cell.Value = $"UL_REQ";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 2) //COMPLETE
                                    {
                                        bEnable = PIOStatus_OMRONToPort_Ready;
                                        DGV_Cell.Value = $"READY";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 3) //COMPLETE
                                    {
                                        bEnable = PIOStatus_OMRONToPort_Auto;
                                        DGV_Cell.Value = $"AUTO";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 4) //Light Curtain
                                    {
                                        bEnable = PIOStatus_OMRONToPort_Error;
                                        DGV_Cell.Value = $"Error";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Red : Color.White;
                                    }
                                    break;
                                case DGV_Buffer1PIOStatusColumn.Port_To_Equip:
                                    if (nRowCount == 0) //L-REQ
                                    {
                                        bEnable = PIOStatus_PortToOMRON_TR_REQ;
                                        DGV_Cell.Value = $"TR-REQ";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 1) //UL-REQ
                                    {
                                        bEnable = PIOStatus_PortToOMRON_Busy_REQ;
                                        DGV_Cell.Value = $"BUSY";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 2) //READY
                                    {
                                        bEnable = PIOStatus_PortToOMRON_Complete;
                                        DGV_Cell.Value = $"COMPLETE";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 3) //READY
                                    {
                                        bEnable = PIOStatus_PortToOMRON_Auto;
                                        DGV_Cell.Value = $"AUTO";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                    }
                                    else if (nRowCount == 4) //ES
                                    {
                                        bEnable = PIOStatus_PortToOMRON_Error;
                                        DGV_Cell.Value = $"Port Error";
                                        DGV_Cell.Style.BackColor = bEnable ? Color.Red : Color.White;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Equip_To_Port].HeaderText = $"{GetPortOperationMode()} -> {SynusLangPack.GetLanguage("DGV_Port")}";
                DGV.Columns[(int)DGV_Buffer1PIOStatusColumn.Port_To_Equip].HeaderText = $"{SynusLangPack.GetLanguage("DGV_Port")} -> {GetPortOperationMode()}";

                if (DGV.Rows.Count != 5)
                {
                    if (DGV.Rows.Count > 0)
                        DGV.Rows.Clear();

                    for (int nCount = 0; nCount < 5; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 0);
                }
                else
                {
                    for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                    {
                        for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_Buffer1PIOStatusColumn)).Length; nColumnCount++)
                        {
                            DGV_Buffer1PIOStatusColumn eDGV_Buffer2PIOStatusColumn = (DGV_Buffer1PIOStatusColumn)nColumnCount;
                            DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                            bool bEnable = false;

                            switch (eDGV_Buffer2PIOStatusColumn)
                            {
                                case DGV_Buffer1PIOStatusColumn.Equip_To_Port:
                                    if (nRowCount == 0) //VALID
                                    {
                                        DGV_Cell.Value = $"VALID";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_OHTToPort_Valid;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_AGVToPort_Valid;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 1) //CS0
                                    {
                                        DGV_Cell.Value = $"CS0";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_OHTToPort_CS0;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_AGVToPort_CS0;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 2) //TR_REQ
                                    {
                                        DGV_Cell.Value = $"TR_REQ";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_OHTToPort_TR_Req;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_AGVToPort_TR_Req;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 3) //BUSY
                                    {
                                        DGV_Cell.Value = $"BUSY";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_OHTToPort_Busy;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_AGVToPort_Busy;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 4) //COMPLETE
                                    {
                                        DGV_Cell.Value = $"COMPLETE";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_OHTToPort_Complete;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_AGVToPort_Complete;
                                        else
                                            bEnable = false;
                                    }
                                    break;
                                case DGV_Buffer1PIOStatusColumn.Port_To_Equip:
                                    if (nRowCount == 0) //L-REQ
                                    {
                                        DGV_Cell.Value = $"L-REQ";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_PortToOHT_Load_Req;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_PortToAGV_Load_Req;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 1) //UL-REQ
                                    {
                                        DGV_Cell.Value = $"UL-REQ";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_PortToOHT_Unload_Req;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_PortToAGV_Unload_Req;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 2) //READY
                                    {
                                        DGV_Cell.Value = $"READY";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_PortToOHT_Ready;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_PortToAGV_Ready;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 3) //ES
                                    {
                                        DGV_Cell.Value = $"ES";

                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                            bEnable = PIOStatus_PortToOHT_ES;
                                        else if (GetPortOperationMode() == PortOperationMode.AGV)
                                            bEnable = PIOStatus_PortToAGV_ES;
                                        else
                                            bEnable = false;
                                    }
                                    else if (nRowCount == 4) //HO_AVBL
                                    {
                                        if (GetPortOperationMode() == PortOperationMode.OHT)
                                        {
                                            DGV_Cell.Value = $"HO_AVBL";
                                            bEnable = PIOStatus_PortToOHT_HO_AVBL;
                                        }
                                        else
                                        {
                                            DGV_Cell.Value = string.Empty;
                                            bEnable = false;
                                        }
                                    }
                                    break;
                            }

                            DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                        }
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }       
        public void Update_DGV_Buffer2PIOStatus(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_Buffer2PIOStatusColumn.RM_To_Port:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_RMToPort"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_RMToPort");
                        break;
                    case (int)DGV_Buffer2PIOStatusColumn.Port_To_RM:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_PortToRM"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_PortToRM");
                        break;
                }
            }

            if ((string)DGV.Tag != GetParam().ID)
            {
                DGV.Rows.Clear();
                DGV.Tag = GetParam().ID;

                DGV.Rows.Add(new string[] { $"TR-REQ", $"L-REQ" });
                DGV.Rows.Add(new string[] { $"BUSY", $"UL-REQ" });
                DGV.Rows.Add(new string[] { $"COMPLETE", $"READY" });
                DGV.Rows.Add(new string[] { $"STK-ERROR", $"PORT-ERROR" });

                DataGridViewFunc.SetSize(DGV, 25, 23, 0);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_Buffer2PIOStatusColumn)).Length; nColumnCount++)
                    {
                        DGV_Buffer2PIOStatusColumn eDGV_Buffer1PIOStatusColumn = (DGV_Buffer2PIOStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        bool bEnable;

                        switch (eDGV_Buffer1PIOStatusColumn)
                        {
                            case DGV_Buffer2PIOStatusColumn.RM_To_Port:
                                if (nRowCount == 0)
                                {
                                    bEnable = PIOStatus_STKToPort_TR_REQ;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 1)
                                {
                                    bEnable = PIOStatus_STKToPort_Busy;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 2)
                                {
                                    bEnable = PIOStatus_STKToPort_Complete;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 3)
                                {
                                    bEnable = PIOStatus_STKToPort_STKError;
                                    DGV_Cell.Style.BackColor = bEnable ? Master.ErrorIntervalColor : Color.White;
                                }
                                break;
                            case DGV_Buffer2PIOStatusColumn.Port_To_RM:
                                if (nRowCount == 0)
                                {
                                    bEnable = PIOStatus_PortToSTK_Load_Req;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 1)
                                {
                                    bEnable = PIOStatus_PortToSTK_Unload_Req;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 2)
                                {
                                    bEnable = PIOStatus_PortToSTK_Ready;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 3)
                                {
                                    if (!IsEQPort())
                                    {
                                        bEnable = PIOStatus_PortToSTK_Error;
                                        DGV_Cell.Style.BackColor = bEnable ? Master.ErrorIntervalColor : Color.White;
                                    }
                                    else
                                    {
                                        DGV_Cell.Value = string.Empty;
                                        DGV_Cell.Style.BackColor = Color.DarkGray;
                                    }
                                }
                                break;
                        }
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_PortToOHT_PIOStatus(ref DataGridView DGV)
        {
            if(DGV.Rows.Count != Enum.GetValues(typeof(DGV_PortToOHTPIORow)).Length)
            {
                DGV.Rows.Clear();

                for(int nCount =0; nCount < Enum.GetValues(typeof(DGV_PortToOHTPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
            }
            else
            {
                for(int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_PortToOHTPIORow eDGV_PortToOHTPIORow = (DGV_PortToOHTPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_PortToOHTPIORow)
                    {
                        case DGV_PortToOHTPIORow.Load_REQ:
                            DGV_Cell.Value = $"L-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOHT_Load_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOHTPIORow.Unload_REQ:
                            DGV_Cell.Value = $"UL-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOHT_Unload_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOHTPIORow.Ready:
                            DGV_Cell.Value = $"READY";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOHT_Ready ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOHTPIORow.ES:
                            DGV_Cell.Value = $"ES";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOHT_ES ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOHTPIORow.HO_AVBL:
                            DGV_Cell.Value = $"HO_AVBL";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOHT_HO_AVBL ? Color.Lime : Color.White;
                            break;
                    }
                }
            }

            if(DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_PortToAGV_PIOStatus(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(DGV_PortToOHTPIORow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetValues(typeof(DGV_PortToOHTPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_PortToOHTPIORow eDGV_PortToOHTPIORow = (DGV_PortToOHTPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_PortToOHTPIORow)
                    {
                        case DGV_PortToOHTPIORow.Load_REQ:
                            DGV_Cell.Value = $"L-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToAGV_Load_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOHTPIORow.Unload_REQ:
                            DGV_Cell.Value = $"UL-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToAGV_Unload_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOHTPIORow.Ready:
                            DGV_Cell.Value = $"READY";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToAGV_Ready ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOHTPIORow.ES:
                            DGV_Cell.Value = $"ES";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToAGV_ES ? Color.Lime : Color.White;
                            break;
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_PortToOMRON_PIOStatus(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(DGV_PortToOMRONPIORow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetValues(typeof(DGV_PortToOMRONPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_PortToOMRONPIORow eDGV_PortToOMRONPIORow = (DGV_PortToOMRONPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_PortToOMRONPIORow)
                    {
                        case DGV_PortToOMRONPIORow.TR_REQ:
                            DGV_Cell.Value = $"TR-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOMRON_TR_REQ ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOMRONPIORow.Busy:
                            DGV_Cell.Value = $"BUSY";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOMRON_Busy_REQ ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOMRONPIORow.Complete:
                            DGV_Cell.Value = $"COMPLETE";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOMRON_Complete ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOMRONPIORow.Auto:
                            DGV_Cell.Value = $"AUTO";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOMRON_Auto ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToOMRONPIORow.Error:
                            DGV_Cell.Value = $"Port Error";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToOMRON_Error ? Color.Red : Color.White;
                            break;
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_PortToRM_PIOStatus(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(DGV_PortToRMPIORow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetValues(typeof(DGV_PortToRMPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 50);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_PortToRMPIORow eDGV_PortToRMPIORow = (DGV_PortToRMPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_PortToRMPIORow)
                    {
                        case DGV_PortToRMPIORow.Load_REQ:
                            DGV_Cell.Value = $"L-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToSTK_Load_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToRMPIORow.Unload_REQ:
                            DGV_Cell.Value = $"UL-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToSTK_Unload_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_PortToRMPIORow.Ready:
                            DGV_Cell.Value = $"READY";
                            DGV_Cell.Style.BackColor = PIOStatus_PortToSTK_Ready ? Color.Lime : Color.White;
                            break;
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_OHTToPort_PIOStatus(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(DGV_OHTToPortPIORow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetValues(typeof(DGV_OHTToPortPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_OHTToPortPIORow eDGV_OHTToPortPIORow = (DGV_OHTToPortPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_OHTToPortPIORow)
                    {
                        case DGV_OHTToPortPIORow.Valid:
                            DGV_Cell.Value = $"VALID";
                            DGV_Cell.Style.BackColor = PIOStatus_OHTToPort_Valid ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.CS0:
                            DGV_Cell.Value = $"CS0";
                            DGV_Cell.Style.BackColor = PIOStatus_OHTToPort_CS0 ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.TR_REQ:
                            DGV_Cell.Value = $"TR-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_OHTToPort_TR_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.Busy:
                            DGV_Cell.Value = $"BUSY";
                            DGV_Cell.Style.BackColor = PIOStatus_OHTToPort_Busy ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.Complete:
                            DGV_Cell.Value = $"COMPLETE";
                            DGV_Cell.Style.BackColor = PIOStatus_OHTToPort_Complete ? Color.Lime : Color.White;
                            break;
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_AGVToPort_PIOStatus(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(DGV_OHTToPortPIORow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetValues(typeof(DGV_OHTToPortPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_OHTToPortPIORow eDGV_OHTToPortPIORow = (DGV_OHTToPortPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_OHTToPortPIORow)
                    {
                        case DGV_OHTToPortPIORow.Valid:
                            DGV_Cell.Value = $"VALID";
                            DGV_Cell.Style.BackColor = PIOStatus_AGVToPort_Valid ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.CS0:
                            DGV_Cell.Value = $"CS0";
                            DGV_Cell.Style.BackColor = PIOStatus_AGVToPort_CS0 ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.TR_REQ:
                            DGV_Cell.Value = $"TR-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_AGVToPort_TR_Req ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.Busy:
                            DGV_Cell.Value = $"BUSY";
                            DGV_Cell.Style.BackColor = PIOStatus_AGVToPort_Busy ? Color.Lime : Color.White;
                            break;
                        case DGV_OHTToPortPIORow.Complete:
                            DGV_Cell.Value = $"COMPLETE";
                            DGV_Cell.Style.BackColor = PIOStatus_AGVToPort_Complete ? Color.Lime : Color.White;
                            break;
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_OMRONToPort_PIOStatus(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(DGV_OMRONToPortPIORow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetValues(typeof(DGV_OMRONToPortPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_OMRONToPortPIORow eDGV_OMRONToPortPIORow = (DGV_OMRONToPortPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_OMRONToPortPIORow)
                    {
                        case DGV_OMRONToPortPIORow.Load_REQ:
                            DGV_Cell.Value = $"L-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_OMRONToPort_Load_REQ ? Color.Lime : Color.White;
                            break;
                        case DGV_OMRONToPortPIORow.Unload_REQ:
                            DGV_Cell.Value = $"UL-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_OMRONToPort_Unload_REQ ? Color.Lime : Color.White;
                            break;
                        case DGV_OMRONToPortPIORow.Ready:
                            DGV_Cell.Value = $"READY";
                            DGV_Cell.Style.BackColor = PIOStatus_OMRONToPort_Ready ? Color.Lime : Color.White;
                            break;
                        case DGV_OMRONToPortPIORow.Auto:
                            DGV_Cell.Value = $"AUTO";
                            DGV_Cell.Style.BackColor = PIOStatus_OMRONToPort_Auto ? Color.Lime : Color.White;
                            break;
                        case DGV_OMRONToPortPIORow.Error:
                            DGV_Cell.Value = $"Error";
                            DGV_Cell.Style.BackColor = PIOStatus_OMRONToPort_Error ? Color.Red : Color.White;
                            break;
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_RMToPort_PIOStatus(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(DGV_RMToPortPIORow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetValues(typeof(DGV_RMToPortPIORow)).Length; nCount++)
                {
                    DGV.Rows.Add();
                }

                DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_RMToPortPIORow eDGV_RMToPortPIORow = (DGV_RMToPortPIORow)nRowCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[0];
                    switch (eDGV_RMToPortPIORow)
                    {
                        case DGV_RMToPortPIORow.TR_REQ:
                            DGV_Cell.Value = $"TR-REQ";
                            DGV_Cell.Style.BackColor = PIOStatus_STKToPort_TR_REQ ? Color.Lime : Color.White;
                            break;
                        case DGV_RMToPortPIORow.Busy:
                            DGV_Cell.Value = $"BUSY";
                            DGV_Cell.Style.BackColor = PIOStatus_STKToPort_Busy ? Color.Lime : Color.White;
                            break;
                        case DGV_RMToPortPIORow.Complete:
                            DGV_Cell.Value = $"COMPLETE";
                            DGV_Cell.Style.BackColor = PIOStatus_STKToPort_Complete ? Color.Lime : Color.White;
                            break;
                        case DGV_RMToPortPIORow.STK_Error:
                            DGV_Cell.Value = $"STK-ERROR";
                            DGV_Cell.Style.BackColor = PIOStatus_STKToPort_STKError ? Color.Red : Color.White;
                            break;
                    }
                }
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_MotionServoStatus(ref DataGridView DGV, object CurrentTag = null)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_MotionServoStatusColumn.Axis:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ServoAxis"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ServoAxis");
                        break;
                    case (int)DGV_MotionServoStatusColumn.Servo:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Servo"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Servo");
                        break;
                    case (int)DGV_MotionServoStatusColumn.Home:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Home"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Home");
                        break;
                    case (int)DGV_MotionServoStatusColumn.Busy:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Busy"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Busy");
                        break;
                    case (int)DGV_MotionServoStatusColumn.PosCommand:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_PosCommand"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_PosCommand");
                        break;
                    case (int)DGV_MotionServoStatusColumn.ActualPos:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ActualPos"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ActualPos");
                        break;
                    case (int)DGV_MotionServoStatusColumn.ActualVel:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ActualVel"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ActualVel");
                        break;
                    case (int)DGV_MotionServoStatusColumn.ActualTorque:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ActualTorque"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ActualTorque");
                        break;
                    case (int)DGV_MotionServoStatusColumn.AlarmCode:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_AlarmCode"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_AlarmCode");
                        break;
                }
            }

            List<PortAxis> ServoList = new List<PortAxis>();
            foreach(PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if (GetMotionParam().IsServoType(ePortAxis))
                    ServoList.Add(ePortAxis);
            }

            if (ServoList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if(DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != ServoList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < ServoList.Count; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 2);
                }

                for(int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    PortAxis ePortAxis = ServoList[nRowCount];
                    bool IsValidServo = GetMotionParam().IsValidServo(ePortAxis);

                    for (int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_MotionServoStatusColumn eDGV_MotionServoStatusColumn = (DGV_MotionServoStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;

                        if(eDGV_MotionServoStatusColumn != DGV_MotionServoStatusColumn.Axis && !IsValidServo)
                        {
                            DGV_Cell.Value = string.Empty;
                            DGV_Cell.Style.ForeColor = Color.DarkGray;
                            DGV_Cell.Style.BackColor = Color.DarkGray;
                            continue;
                        }

                        switch (eDGV_MotionServoStatusColumn)
                        {
                            case DGV_MotionServoStatusColumn.Axis:
                                {
                                    int nAxisNum = GetMotionParam().GetServoAxisNum(ePortAxis);
                                    Data = $"{ePortAxis} [{nAxisNum}]";

                                    if (nAxisNum == -1)
                                    {
                                        DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                    else
                                    {
                                        if (CurrentTag != null)
                                        {
                                            if (CurrentTag.GetType() == typeof(Port.PortAxis))
                                            {
                                                if ((Port.PortAxis)CurrentTag == ePortAxis)
                                                    DGV_Cell.Style.BackColor = Color.Lime;
                                                else
                                                    DGV_Cell.Style.BackColor = Color.White;
                                            }
                                            else
                                                DGV_Cell.Style.BackColor = Color.White;
                                        }
                                        else
                                            DGV_Cell.Style.BackColor = Color.White;
                                    }
                                }
                                break;
                            case DGV_MotionServoStatusColumn.Servo:
                                {
                                    bool bServoOn = ServoCtrl_GetServoOn(ePortAxis);
                                    Data = bServoOn ? $"On" : $"Off";
                                    DGV_Cell.Style.ForeColor = bServoOn ? Color.Lime : Color.Blue;
                                    DGV_Cell.Style.BackColor = bServoOn ? Color.White : Master.ErrorIntervalColor;
                                }
                                break;
                            case DGV_MotionServoStatusColumn.Home:
                                {
                                    bool bHomeDone = ServoCtrl_GetHomeDone(ePortAxis);
                                    Data = bHomeDone ? $"Done" : $"Not Homed";
                                    DGV_Cell.Style.ForeColor = bHomeDone ? Color.Lime : Color.Blue;
                                    DGV_Cell.Style.BackColor = bHomeDone ? Color.White : Master.ErrorIntervalColor;
                                }
                                break;
                            case DGV_MotionServoStatusColumn.Busy:
                                {
                                    bool bBusy = ServoCtrl_GetBusy(ePortAxis);
                                    Data = bBusy ? $"Busy" : $"Idle";
                                    DGV_Cell.Style.ForeColor = bBusy ? Color.Blue : Color.Black;
                                    DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_MotionServoStatusColumn.PosCommand:
                                {
                                    string PosCommand = ServoCtrl_GetTargetPosition(ePortAxis).ToString("0.0") + (GetMotionParam().IsRotaryAxis(ePortAxis) ? " °" : " mm");

                                    Data = PosCommand;
                                    DGV_Cell.Style.ForeColor = Color.Black;
                                    if ((string)DGV_Cell.Value != PosCommand)
                                        DGV_Cell.Style.BackColor = Color.Lime;
                                    else
                                    {
                                        DGV_Cell.Style.BackColor = Color.White;
                                    }
                                }
                                break;
                            case DGV_MotionServoStatusColumn.ActualPos:
                                {
                                    string ActualPos = ServoCtrl_GetCurrentPosition(ePortAxis).ToString("0.0") + (GetMotionParam().IsRotaryAxis(ePortAxis) ? " °" : " mm");

                                    Data = ActualPos;
                                    DGV_Cell.Style.ForeColor = Color.Black;
                                    if ((string)DGV_Cell.Value != ActualPos)
                                        DGV_Cell.Style.BackColor = Color.Lime;
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionServoStatusColumn.ActualVel:
                                {
                                    string CurrentSpeed = ServoCtrl_GetCurrentSpeed(ePortAxis).ToString("0") + (GetMotionParam().IsRotaryAxis(ePortAxis) ? " °/min" : " m/min");

                                    Data = CurrentSpeed;
                                    DGV_Cell.Style.ForeColor = Color.Black;
                                    if ((string)DGV_Cell.Value != CurrentSpeed)
                                        DGV_Cell.Style.BackColor = Color.Lime;
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionServoStatusColumn.ActualTorque:
                                {
                                    string ActualTorque = ServoCtrl_GetCurrentTorque(ePortAxis).ToString("0") + " %";

                                    Data = ActualTorque;
                                    DGV_Cell.Style.ForeColor = Color.Black;

                                    if (ServoCtrl_GetCurrentTorque(ePortAxis) > GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).MaxLoad)
                                    {
                                        DGV_Cell.Style.BackColor = Color.Red;
                                    }
                                    else if (ServoCtrl_GetCurrentTorque(ePortAxis) > GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).MaxLoad * 0.8)
                                    {
                                        DGV_Cell.Style.BackColor = Color.OrangeRed;
                                    }
                                    else if (ServoCtrl_GetCurrentTorque(ePortAxis) > GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).MaxLoad * 0.5)
                                    {
                                        DGV_Cell.Style.BackColor = Color.Orange;
                                    }
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionServoStatusColumn.AlarmCode:
                                {
                                    string AlarmCode = $"0x{ServoCtrl_GetAxisAlarmCode(ePortAxis).ToString("x4")}";
                                    Data = AlarmCode;
                                    DGV_Cell.Style.ForeColor = Color.Black;
                                    DGV_Cell.Style.BackColor = ServoCtrl_GetAlarmStatus(ePortAxis) ? Master.ErrorIntervalColor : Color.White; //
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }
                }

                if (DGV.CurrentCell != null)
                {
                    if (DGV.CurrentCell.ColumnIndex != (int)DGV_MotionServoStatusColumn.AlarmCode)
                        DGV.CurrentCell = null;
                }
            }
        }
        public void Update_DGV_MotionInverterStatus(ref DataGridView DGV, object CurrentTag = null)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_MotionInverterStatusColumn.Axis:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_InverterAxis"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_InverterAxis");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.Busy:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Busy"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Busy");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.HighSpeedBWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_HighSpeedBWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_HighSpeedBWD");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.LowSpeedBWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LowSpeedBWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LowSpeedBWD");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.LowSpeedFWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LowSpeedFWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LowSpeedFWD");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.HighSpeedFWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_HighSpeedFWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_HighSpeedFWD");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.HighSpeed:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_HighSpeed"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_HighSpeed");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.LowSpeed:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LowSpeed"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LowSpeed");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.FWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_FWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_FWD");
                        break;
                    case (int)DGV_MotionInverterStatusColumn.BWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_BWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_BWD");
                        break;
                }
            }

            List<PortAxis> InverterList = new List<PortAxis>();
            foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if (GetMotionParam().IsInverterType(ePortAxis))
                    InverterList.Add(ePortAxis);
            }

            if (InverterList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != InverterList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < InverterList.Count; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 2);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    PortAxis ePortAxis = InverterList[nRowCount];

                    for (int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_MotionInverterStatusColumn eDGV_MotionInverterStatusColumn = (DGV_MotionInverterStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;
                        var InverterParam = GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis);


                        switch (eDGV_MotionInverterStatusColumn)
                        {
                            case DGV_MotionInverterStatusColumn.Axis:
                                {
                                    Data = $"{ePortAxis}";

                                    if (CurrentTag != null)
                                    {
                                        if (CurrentTag.GetType() == typeof(Port.PortAxis))
                                        {
                                            if ((Port.PortAxis)CurrentTag == ePortAxis)
                                                DGV_Cell.Style.BackColor = Color.Lime;
                                            else
                                                DGV_Cell.Style.BackColor = Color.White;
                                        }
                                        else
                                            DGV_Cell.Style.BackColor = Color.White;
                                    }
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionInverterStatusColumn.Busy:
                                {
                                    bool bBusy = InverterCtrl_Is_Busy(ePortAxis);
                                    Data = bBusy ? $"Busy" : $"Idle";
                                    DGV_Cell.Style.ForeColor = bBusy ? Color.Blue : Color.Black;
                                    DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_MotionInverterStatusColumn.HighSpeedFWD:
                            case DGV_MotionInverterStatusColumn.LowSpeedFWD:
                            case DGV_MotionInverterStatusColumn.LowSpeedBWD:
                            case DGV_MotionInverterStatusColumn.HighSpeedBWD:
                                {
                                    if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
                                    {
                                        InvCtrlType eInvCtrlType = InvCtrlType.HighSpeedFWD;

                                        if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.HighSpeedFWD)
                                            eInvCtrlType = InvCtrlType.HighSpeedFWD;
                                        else if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.LowSpeedFWD)
                                            eInvCtrlType = InvCtrlType.LowSpeedFWD;
                                        else if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.LowSpeedBWD)
                                            eInvCtrlType = InvCtrlType.LowSpeedBWD;
                                        else if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.HighSpeedBWD)
                                            eInvCtrlType = InvCtrlType.HighSpeedBWD;

                                        bool bHighSpeedOn = InverterCtrl_GetRunStatus(ePortAxis, eInvCtrlType);

                                        Data = bHighSpeedOn ? $"On" : $"Off";
                                        DGV_Cell.Style.ForeColor = bHighSpeedOn ? Color.Blue : Color.Black;
                                        DGV_Cell.Style.BackColor = bHighSpeedOn ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = string.Empty;
                                        DGV_Cell.Style.ForeColor = Color.DarkGray;
                                        DGV_Cell.Style.BackColor = Color.DarkGray;
                                    }
                                }
                                break;
                            case DGV_MotionInverterStatusColumn.HighSpeed:
                            case DGV_MotionInverterStatusColumn.LowSpeed:
                                {
                                    if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
                                    {
                                        InvIOCtrlFlag eInvCtrlFlag = InvIOCtrlFlag.HighSpeed;

                                        if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.HighSpeed)
                                            eInvCtrlFlag = InvIOCtrlFlag.HighSpeed;
                                        else if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.LowSpeed)
                                            eInvCtrlFlag = InvIOCtrlFlag.LowSpeed;

                                        var IOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(eInvCtrlFlag);
                                        bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                        if (GetMotionParam().IsValidIO(IOParam))
                                        {
                                            int StartAddr = IOParam.StartAddr;
                                            int Bit = IOParam.Bit;
                                            Data = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                            DGV_Cell.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                            DGV_Cell.Style.BackColor = Status ? Color.Lime : Color.White;
                                        }
                                        else
                                        {
                                            Data = "Not Define";
                                            DGV_Cell.Style.ForeColor = Color.Blue;
                                            DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                        }
                                    }
                                    else
                                    {
                                        Data = string.Empty;
                                        DGV_Cell.Style.ForeColor = Color.DarkGray;
                                        DGV_Cell.Style.BackColor = Color.DarkGray;
                                    }
                                }
                                break;
                            case DGV_MotionInverterStatusColumn.FWD:
                            case DGV_MotionInverterStatusColumn.BWD:
                                {
                                    //if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
                                    //{
                                        InvIOCtrlFlag eInvCtrlFlag = InvIOCtrlFlag.HighSpeed;

                                        if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.FWD)
                                            eInvCtrlFlag = InvIOCtrlFlag.FWD;
                                        else if (eDGV_MotionInverterStatusColumn == DGV_MotionInverterStatusColumn.BWD)
                                            eInvCtrlFlag = InvIOCtrlFlag.BWD;

                                        var IOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(eInvCtrlFlag);
                                        bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                        if (GetMotionParam().IsValidIO(IOParam))
                                        {
                                            int StartAddr = IOParam.StartAddr;
                                            int Bit = IOParam.Bit;
                                            Data = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                            DGV_Cell.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                            DGV_Cell.Style.BackColor = Status ? Color.Lime : Color.White;
                                        }
                                        else
                                        {
                                            Data = "Not Define";
                                            DGV_Cell.Style.ForeColor = Color.Blue;
                                            DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                        }
                                    //}
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_MotionCylinderStatus(ref DataGridView DGV, object CurrentTag = null)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_MotionCylinderStatusColumn.Axis:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_CylinderAxis"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_CylinderAxis");
                        break;
                    case (int)DGV_MotionCylinderStatusColumn.Busy:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Busy"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Busy");
                        break;
                    case (int)DGV_MotionCylinderStatusColumn.BWDStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_BWDStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_BWDStatus");
                        break;
                    case (int)DGV_MotionCylinderStatusColumn.FWDStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_FWDStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_FWDStatus");
                        break;
                    case (int)DGV_MotionCylinderStatusColumn.BWD1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_BWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_BWD");
                        break;
                    case (int)DGV_MotionCylinderStatusColumn.FWD1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_FWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_FWD");
                        break;
                }
            }

            List<PortAxis> CylinderList = new List<PortAxis>();

            foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if (GetMotionParam().IsCylinderType(ePortAxis))
                    CylinderList.Add(ePortAxis);
            }

            int CylinderAllCount = CylinderList.Count;

            if (CylinderAllCount == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != CylinderAllCount)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < CylinderAllCount; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 2);
                }

                int nRowCount = 0;

                for (int nCount = 0; nCount < CylinderList.Count; nCount++)
                {
                    PortAxis ePortAxis = CylinderList[nCount];

                    for (int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_MotionCylinderStatusColumn eDGV_MotionCylinderStatusColumn = (DGV_MotionCylinderStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;
                        var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis);


                        switch (eDGV_MotionCylinderStatusColumn)
                        {
                            case DGV_MotionCylinderStatusColumn.Axis:
                                {
                                    Data = $"{ePortAxis}";

                                    if (CurrentTag != null)
                                    {
                                        if (CurrentTag.GetType() == typeof(Port.PortAxis))
                                        {
                                            if ((Port.PortAxis)CurrentTag == ePortAxis)
                                                DGV_Cell.Style.BackColor = Color.Lime;
                                            else
                                                DGV_Cell.Style.BackColor = Color.White;
                                        }
                                        else
                                            DGV_Cell.Style.BackColor = Color.White;
                                    }
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionCylinderStatusColumn.Busy:
                                {
                                    bool bBusy = CylinderCtrl_Is_Busy(ePortAxis);
                                    Data = bBusy ? $"Busy" : $"Idle";
                                    DGV_Cell.Style.ForeColor = bBusy ? Color.Blue : Color.Black;
                                    DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_MotionCylinderStatusColumn.BWDStatus:
                            case DGV_MotionCylinderStatusColumn.FWDStatus:
                                {
                                    CylCtrlList eCylCtrlList = CylCtrlList.FWD;

                                    if (eDGV_MotionCylinderStatusColumn == DGV_MotionCylinderStatusColumn.FWDStatus)
                                        eCylCtrlList = CylCtrlList.FWD;
                                    else if (eDGV_MotionCylinderStatusColumn == DGV_MotionCylinderStatusColumn.BWDStatus)
                                        eCylCtrlList = CylCtrlList.BWD;

                                    bool bFWDOn = CylinderCtrl_GetPosSensorOn(ePortAxis, eCylCtrlList);

                                    if (GetMotionParam().IsValidIO(CylinderParam.GetPosSensorIOParam(eCylCtrlList)))
                                    {
                                        int StartAddr = CylinderParam.GetPosSensorIOParam(eCylCtrlList).StartAddr;
                                        int Bit = CylinderParam.GetPosSensorIOParam(eCylCtrlList).Bit;
                                        Data = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Cell.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                        DGV_Cell.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = "Not Define";
                                        DGV_Cell.Style.ForeColor = Color.Blue;
                                        DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                break;
                            case DGV_MotionCylinderStatusColumn.FWD1:
                            case DGV_MotionCylinderStatusColumn.BWD1:
                                {
                                    CylCtrlList eCylCtrlList = CylCtrlList.FWD;

                                    if (eDGV_MotionCylinderStatusColumn == DGV_MotionCylinderStatusColumn.FWD1)
                                        eCylCtrlList = CylCtrlList.FWD;
                                    else if (eDGV_MotionCylinderStatusColumn == DGV_MotionCylinderStatusColumn.BWD1)
                                        eCylCtrlList = CylCtrlList.BWD;

                                    bool bFWDOn = CylinderCtrl_GetRunStatus(ePortAxis, eCylCtrlList);

                                    if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                    {
                                        int StartAddr   = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                        int Bit         = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                        Data            = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Cell.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                        DGV_Cell.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = "Not Define";
                                        DGV_Cell.Style.ForeColor = Color.Blue;
                                        DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }

                    nRowCount++;
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_MotionConveyorStatus(ref DataGridView DGV, object CurrentTag = null)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_MotionConveyorStatusColumn.Axis:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_CVBuffer"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_CVBuffer");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.Busy:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Busy"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Busy");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.HighSpeedBWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_HighSpeedBWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_HighSpeedBWD");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.LowSpeedBWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LowSpeedBWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LowSpeedBWD");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.LowSpeedFWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LowSpeedFWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LowSpeedFWD");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.HighSpeedFWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_HighSpeedFWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_HighSpeedFWD");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.HighSpeed:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_HighSpeed"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_HighSpeed");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.LowSpeed:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_LowSpeed"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_LowSpeed");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.FWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_FWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_FWD");
                        break;
                    case (int)DGV_MotionConveyorStatusColumn.BWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_BWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_BWD");
                        break;
                }
            }

            List<BufferCV> BufferCVList = new List<BufferCV>();
            foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
            {
                if (GetMotionParam().IsCVUsed(eBufferCV))
                    BufferCVList.Add(eBufferCV);
            }

            if (BufferCVList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != BufferCVList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < BufferCVList.Count; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 2);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    BufferCV eBufferCV = BufferCVList[nRowCount];

                    for (int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_MotionConveyorStatusColumn eDGV_MotionConveyorStatusColumn = (DGV_MotionConveyorStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;
                        var InverterParam = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV);


                        switch (eDGV_MotionConveyorStatusColumn)
                        {
                            case DGV_MotionConveyorStatusColumn.Axis:
                                {
                                    Data = $"{eBufferCV}";

                                    if (CurrentTag != null)
                                    {
                                        if (CurrentTag.GetType() == typeof(Port.BufferCV))
                                        {
                                            if ((Port.BufferCV)CurrentTag == eBufferCV)
                                                DGV_Cell.Style.BackColor = Color.Lime;
                                            else
                                                DGV_Cell.Style.BackColor = Color.White;
                                        }
                                        else
                                            DGV_Cell.Style.BackColor = Color.White;
                                    }
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionConveyorStatusColumn.Busy:
                                {
                                    bool bBusy = BufferCtrl_CV_Is_Busy(eBufferCV);
                                    Data = bBusy ? $"Busy" : $"Idle";
                                    DGV_Cell.Style.ForeColor = bBusy ? Color.Blue : Color.Black;
                                    DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_MotionConveyorStatusColumn.HighSpeedFWD:
                            case DGV_MotionConveyorStatusColumn.LowSpeedFWD:
                            case DGV_MotionConveyorStatusColumn.LowSpeedBWD:
                            case DGV_MotionConveyorStatusColumn.HighSpeedBWD:
                                {
                                    if (GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).InvCtrlMode == InvCtrlMode.IOControl)
                                    {
                                        InvCtrlType eInvCtrlType = InvCtrlType.HighSpeedFWD;

                                        if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.HighSpeedFWD)
                                            eInvCtrlType = InvCtrlType.HighSpeedFWD;
                                        else if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.LowSpeedFWD)
                                            eInvCtrlType = InvCtrlType.LowSpeedFWD;
                                        else if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.LowSpeedBWD)
                                            eInvCtrlType = InvCtrlType.LowSpeedBWD;
                                        else if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.HighSpeedBWD)
                                            eInvCtrlType = InvCtrlType.HighSpeedBWD;

                                        bool bHighSpeedOn = BufferCtrl_CV_GetRunStatus(eBufferCV, eInvCtrlType); //BufferCtrl_Is_FWDHighSpeedMove_Flag(eBufferCV);

                                        Data = bHighSpeedOn ? $"On" : $"Off";
                                        DGV_Cell.Style.ForeColor = bHighSpeedOn ? Color.Blue : Color.Black;
                                        DGV_Cell.Style.BackColor = bHighSpeedOn ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = string.Empty;
                                        DGV_Cell.Style.ForeColor = Color.DarkGray;
                                        DGV_Cell.Style.BackColor = Color.DarkGray;
                                    }
                                }
                                break;
                            case DGV_MotionConveyorStatusColumn.HighSpeed:
                            case DGV_MotionConveyorStatusColumn.LowSpeed:
                                {
                                    if (GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).InvCtrlMode == InvCtrlMode.IOControl)
                                    {
                                        InvIOCtrlFlag eInvCtrlFlag = InvIOCtrlFlag.HighSpeed;

                                        if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.HighSpeed)
                                            eInvCtrlFlag = InvIOCtrlFlag.HighSpeed;
                                        else if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.LowSpeed)
                                            eInvCtrlFlag = InvIOCtrlFlag.LowSpeed;

                                        var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(eInvCtrlFlag);
                                        bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                        if (GetMotionParam().IsValidIO(IOParam))
                                        {
                                            int StartAddr = IOParam.StartAddr;
                                            int Bit = IOParam.Bit;
                                            Data = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                            DGV_Cell.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                            DGV_Cell.Style.BackColor = Status ? Color.Lime : Color.White;
                                        }
                                        else
                                        {
                                            Data = "Not Define";
                                            DGV_Cell.Style.ForeColor = Color.Blue;
                                            DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                        }
                                    }
                                    else
                                    {
                                        Data = string.Empty;
                                        DGV_Cell.Style.ForeColor = Color.DarkGray;
                                        DGV_Cell.Style.BackColor = Color.DarkGray;
                                    }
                                }
                                break;
                            case DGV_MotionConveyorStatusColumn.FWD:
                            case DGV_MotionConveyorStatusColumn.BWD:
                                {
                                    InvIOCtrlFlag eInvCtrlFlag = InvIOCtrlFlag.HighSpeed;

                                    if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.FWD)
                                        eInvCtrlFlag = InvIOCtrlFlag.FWD;
                                    else if (eDGV_MotionConveyorStatusColumn == DGV_MotionConveyorStatusColumn.BWD)
                                        eInvCtrlFlag = InvIOCtrlFlag.BWD;

                                    var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(eInvCtrlFlag);
                                    bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                    if (GetMotionParam().IsValidIO(IOParam))
                                    {
                                        int StartAddr = IOParam.StartAddr;
                                        int Bit = IOParam.Bit;
                                        Data = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Cell.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                        DGV_Cell.Style.BackColor = Status ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = "Not Define";
                                        DGV_Cell.Style.ForeColor = Color.Blue;
                                        DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_MotionCVOptionStatus(ref DataGridView DGV, object CurrentTag = null)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_MotionConveyorOptionCylinderStatusColumn.Axis:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_CVOption"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_CVOption");
                        break;
                    case (int)DGV_MotionConveyorOptionCylinderStatusColumn.Busy:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Busy"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Busy");
                        break;
                    case (int)DGV_MotionConveyorOptionCylinderStatusColumn.BWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_BWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_BWD");
                        break;
                    case (int)DGV_MotionConveyorOptionCylinderStatusColumn.FWD:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_FWD"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_FWD");
                        break;
                }
            }

            List<BufferCV> CVStopperList = new List<BufferCV>();
            List<BufferCV> CVCenteringList = new List<BufferCV>();

            foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
            {
                if (GetMotionParam().IsStopperEnable(eBufferCV))
                {
                    CVStopperList.Add(eBufferCV);
                }
                if (GetMotionParam().IsCenteringEnable(eBufferCV))
                {
                    CVCenteringList.Add(eBufferCV);
                }
            }

            int CylinderAllCount = CVStopperList.Count + CVCenteringList.Count;

            if (CylinderAllCount == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != CylinderAllCount)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < CylinderAllCount; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 25, 23, 2);
                }

                int nRowCount = 0;

                for (int nCount = 0; nCount < CVStopperList.Count; nCount++)
                {
                    BufferCV eBufferCV = CVStopperList[nCount];

                    for (int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_MotionConveyorOptionCylinderStatusColumn eDGV_MotionConveyorOptionCylinderStatusColumn = (DGV_MotionConveyorOptionCylinderStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;
                        var CylinderParam = GetMotionParam().GetBufferCtrl_StopperParam(eBufferCV);


                        switch (eDGV_MotionConveyorOptionCylinderStatusColumn)
                        {
                            case DGV_MotionConveyorOptionCylinderStatusColumn.Axis:
                                {
                                    Data = $"{eBufferCV} [Stopper]";

                                    if (CurrentTag != null)
                                    {
                                        if (CurrentTag.GetType() == typeof(Port.BufferCV))
                                        {
                                            if ((Port.BufferCV)CurrentTag == eBufferCV)
                                                DGV_Cell.Style.BackColor = Color.Lime;
                                            else
                                                DGV_Cell.Style.BackColor = Color.White;
                                        }
                                        else
                                            DGV_Cell.Style.BackColor = Color.White;
                                    }
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionConveyorOptionCylinderStatusColumn.Busy:
                                {
                                    bool bBusy = BufferCtrl_Stopper_Is_Busy(eBufferCV);
                                    Data = bBusy ? $"Busy" : $"Idle";
                                    DGV_Cell.Style.ForeColor = bBusy ? Color.Blue : Color.Black;
                                    DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_MotionConveyorOptionCylinderStatusColumn.FWD:
                            case DGV_MotionConveyorOptionCylinderStatusColumn.BWD:
                                {
                                    CylCtrlList eCylCtrlList = CylCtrlList.FWD;

                                    if (eDGV_MotionConveyorOptionCylinderStatusColumn == DGV_MotionConveyorOptionCylinderStatusColumn.FWD)
                                        eCylCtrlList = CylCtrlList.FWD;
                                    else if (eDGV_MotionConveyorOptionCylinderStatusColumn == DGV_MotionConveyorOptionCylinderStatusColumn.BWD)
                                        eCylCtrlList = CylCtrlList.BWD;

                                    bool bFWDOn = BufferCtrl_Stopper_GetRunStatus(eBufferCV, eCylCtrlList);

                                    if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                    {
                                        int StartAddr = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                        int Bit = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                        Data = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Cell.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                        DGV_Cell.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = "Not Define";
                                        DGV_Cell.Style.ForeColor = Color.Blue;
                                        DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }

                    nRowCount++;
                }
                for (int nCount = 0; nCount < CVCenteringList.Count; nCount++)
                {
                    BufferCV eBufferCV = CVCenteringList[nCount];

                    for (int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_MotionConveyorOptionCylinderStatusColumn eDGV_MotionConveyorOptionCylinderStatusColumn = (DGV_MotionConveyorOptionCylinderStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;
                        var CylinderParam = GetMotionParam().GetBufferCtrl_CenteringParam(eBufferCV);


                        switch (eDGV_MotionConveyorOptionCylinderStatusColumn)
                        {
                            case DGV_MotionConveyorOptionCylinderStatusColumn.Axis:
                                {
                                    Data = $"{eBufferCV} [Centering]";

                                    if (CurrentTag != null)
                                    {
                                        if (CurrentTag.GetType() == typeof(Port.BufferCV))
                                        {
                                            if ((Port.BufferCV)CurrentTag == eBufferCV)
                                                DGV_Cell.Style.BackColor = Color.Lime;
                                            else
                                                DGV_Cell.Style.BackColor = Color.White;
                                        }
                                        else
                                            DGV_Cell.Style.BackColor = Color.White;
                                    }
                                    else
                                        DGV_Cell.Style.BackColor = Color.White;
                                }
                                break;
                            case DGV_MotionConveyorOptionCylinderStatusColumn.Busy:
                                {
                                    bool bBusy = BufferCtrl_Centering_Is_Busy(eBufferCV);
                                    Data = bBusy ? $"Busy" : $"Idle";
                                    DGV_Cell.Style.ForeColor = bBusy ? Color.Blue : Color.Black;
                                    DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_MotionConveyorOptionCylinderStatusColumn.FWD:
                            case DGV_MotionConveyorOptionCylinderStatusColumn.BWD:
                                {
                                    CylCtrlList eCylCtrlList = CylCtrlList.FWD;

                                    if (eDGV_MotionConveyorOptionCylinderStatusColumn == DGV_MotionConveyorOptionCylinderStatusColumn.FWD)
                                        eCylCtrlList = CylCtrlList.FWD;
                                    else if (eDGV_MotionConveyorOptionCylinderStatusColumn == DGV_MotionConveyorOptionCylinderStatusColumn.BWD)
                                        eCylCtrlList = CylCtrlList.BWD;

                                    bool bFWDOn = BufferCtrl_Centering_GetRunStatus(eBufferCV, eCylCtrlList);

                                    if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                    {
                                        int StartAddr = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                        int Bit = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                        Data = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Cell.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                        DGV_Cell.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = "Not Define";
                                        DGV_Cell.Style.ForeColor = Color.Blue;
                                        DGV_Cell.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }

                    nRowCount++;
                }


                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_PortSensorStatus(ref DataGridView DGV)
        {
            List<DGV_PortSensorStatusRow> UseRowList = new List<DGV_PortSensorStatusRow>();
            
            foreach (PortAxis ePortAxis in Enum.GetValues(typeof(PortAxis)))
            {
                if (ePortAxis == PortAxis.Shuttle_X && GetMotionParam().IsServoType(ePortAxis))
                    UseRowList.Add(DGV_PortSensorStatusRow.Shuttle_X_Axis);
                else if (ePortAxis == PortAxis.Shuttle_Z && 
                        (GetMotionParam().IsServoType(ePortAxis) || GetMotionParam().IsCylinderType(ePortAxis)))
                    UseRowList.Add(DGV_PortSensorStatusRow.Shuttle_Z_Axis);
                else if(ePortAxis == PortAxis.Shuttle_T && GetMotionParam().IsServoType(ePortAxis))
                    UseRowList.Add(DGV_PortSensorStatusRow.Shuttle_T_Axis);
            }

            if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer && !IsBufferControlPort() && !IsEQPort())
            {
                UseRowList.Add(DGV_PortSensorStatusRow.Buffer1_OP_Status1);
                UseRowList.Add(DGV_PortSensorStatusRow.Buffer2_LP_Status1);
                UseRowList.Add(DGV_PortSensorStatusRow.Shuttle_Status);
            }
            else if(GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer && !IsBufferControlPort() && !IsEQPort())
            {
                UseRowList.Add(DGV_PortSensorStatusRow.Buffer1_OP_Status1);
                UseRowList.Add(DGV_PortSensorStatusRow.Buffer2_LP_Status1);
                UseRowList.Add(DGV_PortSensorStatusRow.Shuttle_Status);
            }

            if (GetParam().ePortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
            {
                UseRowList.Add(DGV_PortSensorStatusRow.Buffer1_OP_Status2);
                UseRowList.Add(DGV_PortSensorStatusRow.Buffer2_LP_Status2);

                List<BufferCV> BPList = new List<BufferCV>();

                foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
                {
                    if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV != BufferCV.Buffer_LP && eBufferCV != BufferCV.Buffer_OP)
                    {
                        if(GetMotionParam().IsCSTDetectSensorEnable(eBufferCV))
                            BPList.Add(eBufferCV);
                    }
                }

                if(BPList.Count != 0)
                    UseRowList.Add(DGV_PortSensorStatusRow.CVType_BP_CST_Status);
            }

            if (UseRowList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != UseRowList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nCount = 0; nCount < UseRowList.Count; nCount++)
                        DGV.Rows.Add();

                    DataGridViewFunc.SetSize(DGV, 0, 23, 2);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_PortSensorStatusRow eDGV_PortSensorStatusRow = UseRowList[nRowCount];

                    switch(eDGV_PortSensorStatusRow)
                    {
                        case DGV_PortSensorStatusRow.Shuttle_X_Axis:
                            Update_DGVRow_Shuttle_X_Axis_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.Shuttle_Z_Axis:
                            Update_DGVRow_Shuttle_Z_Axis_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.Shuttle_T_Axis:
                            Update_DGVRow_Shuttle_T_Axis_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.Buffer1_OP_Status1:
                            Update_DGVRow_Buffer_OP_Status1_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.Buffer2_LP_Status1:
                            Update_DGVRow_Buffer_LP_Status1_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.Shuttle_Status:
                            Update_DGVRow_Shuttle_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.Buffer1_OP_Status2:
                            Update_DGVRow_Buffer_OP_Status2_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.Buffer2_LP_Status2:
                            Update_DGVRow_Buffer_LP_Status2_Sensor(ref DGV, nRowCount);
                            break;
                        case DGV_PortSensorStatusRow.CVType_BP_CST_Status:
                            Update_DGVRow_CV_Buffer_BP_CST_Status_Sensor(ref DGV, nRowCount);
                            break;
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        private void Update_DGVRow_Shuttle_X_Axis_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            List<DGV_Shuttle_X_Axis_SensorStatusColumn> SensorList = new List<DGV_Shuttle_X_Axis_SensorStatusColumn>();
            SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.Type);
            if (GetMotionParam().IsServoType(PortAxis.Shuttle_X))
            {
                SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.NOT);
                SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.POT);
                SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.HOME);
                SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.Pos);
                if(GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                    SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.WaitPos);
                SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.OriginOK);
            }
            SensorList.Add(DGV_Shuttle_X_Axis_SensorStatusColumn.Busy);

            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for(int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                if (nCount >= SensorList.Count)
                {
                    DGV_Cell.Value = string.Empty;
                    DGV_Cell.Style.BackColor = Color.DarkGray;
                    continue;
                }
                DGV_Shuttle_X_Axis_SensorStatusColumn eDGV_Shuttle_X_Axis_SensorStatusColumn = SensorList[nCount];
                string Data = string.Empty;

                switch(eDGV_Shuttle_X_Axis_SensorStatusColumn)
                {
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Shuttle_X_Axis")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.NOT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_NOT");
                            bool bNOT = Sensor_X_Axis_NOT;
                            Data = Infotxt;// bNOT ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bNOT ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.POT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_POT");
                            bool bPOT = Sensor_X_Axis_POT;
                            Data = Infotxt;// bPOT ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bPOT ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.HOME:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_HOMESwitch");
                            bool bHome = Sensor_X_Axis_HOME;
                            Data = Infotxt;//bHome ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bHome ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.Pos:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_PosSensor");
                            bool bPos = Sensor_X_Axis_POS;
                            Data = Infotxt;//bPos ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bPos ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.WaitPos:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_WaitSensor");
                            bool bWaitPos = Sensor_X_Axis_WaitPosSensor;
                            Data = Infotxt;//bWaitPos ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bWaitPos ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.OriginOK:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_HomeDone");
                            bool bOriginOK = Sensor_X_Axis_OriginOK;
                            Data = Infotxt;//bOriginOK ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bOriginOK ? Color.Lime : Master.ErrorIntervalColor;
                        }
                        break;
                    case DGV_Shuttle_X_Axis_SensorStatusColumn.Busy:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Busy");
                            bool bBusy = Sensor_X_Axis_Busy;
                            Data = Infotxt;//bBusy ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_Shuttle_Z_Axis_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            List<DGV_Shuttle_Z_Axis_SensorStatusColumn> SensorList = new List<DGV_Shuttle_Z_Axis_SensorStatusColumn>();
            SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.Type);
            if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z))
            {
                SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.NOT);
                SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.POT);
                SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.HOME);
                SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.Pos);
                SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.OriginOK);
            }
            else if(GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z))
            {
                SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.BWD);
                SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.FWD);
            }
            SensorList.Add(DGV_Shuttle_Z_Axis_SensorStatusColumn.Busy);

            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                if (nCount >= SensorList.Count)
                {
                    DGV_Cell.Value = string.Empty;
                    DGV_Cell.Style.BackColor = Color.DarkGray;
                    continue;
                }

                DGV_Shuttle_Z_Axis_SensorStatusColumn eDGV_Shuttle_Z_Axis_SensorStatusColumn = SensorList[nCount];
                string Data = string.Empty;

                switch (eDGV_Shuttle_Z_Axis_SensorStatusColumn)
                {
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Shuttle_Z_Axis")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.NOT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_NOT");
                            bool bNOT = Sensor_Z_Axis_NOT;
                            Data = Infotxt;//bNOT ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bNOT ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.POT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_POT");
                            bool bPOT = Sensor_Z_Axis_POT;
                            Data = Infotxt;//bPOT ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bPOT ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.HOME:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_HOMESwitch");
                            bool bHome = Sensor_Z_Axis_HOME;
                            Data = Infotxt;//bHome ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bHome ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.Pos:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_PosSensor");
                            bool bPos = Sensor_Z_Axis_POS;
                            Data = Infotxt;//bPos ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bPos ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.Busy:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Busy");
                            bool bBusy = Sensor_Z_Axis_Busy;
                            Data = Infotxt;//bBusy ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.OriginOK:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_HomeDone");
                            bool bOriginOK = Sensor_Z_Axis_OriginOK;
                            Data = Infotxt;//bOriginOK ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bOriginOK ? Color.Lime : Master.ErrorIntervalColor;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.BWD:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_BWD");
                            bool bBWD = Sensor_Z_Axis_BWDSensor;
                            Data = Infotxt;//bBWD ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bBWD ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_Z_Axis_SensorStatusColumn.FWD:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_FWD");
                            bool bFWD = Sensor_Z_Axis_FWDSensor;
                            Data = Infotxt;//bFWD ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bFWD ? Color.Lime : Color.White;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_Shuttle_T_Axis_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            List<DGV_Shuttle_T_Axis_SensorStatusColumn> SensorList = new List<DGV_Shuttle_T_Axis_SensorStatusColumn>();
            SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.Type);
            if (GetMotionParam().IsServoType(PortAxis.Shuttle_T))
            {
                SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.NOT);
                SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.POT);
                SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.HOME);
                SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.Pos);
                SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.OriginOK);
                SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.Deg_0);
                SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.Deg_180);
            }
            SensorList.Add(DGV_Shuttle_T_Axis_SensorStatusColumn.Busy);

            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                if (nCount >= SensorList.Count)
                {
                    DGV_Cell.Value = string.Empty;
                    DGV_Cell.Style.BackColor = Color.DarkGray;
                    continue;
                }

                DGV_Shuttle_T_Axis_SensorStatusColumn eDGV_Shuttle_T_Axis_SensorStatusColumn = SensorList[nCount];
                string Data = string.Empty;

                switch (eDGV_Shuttle_T_Axis_SensorStatusColumn)
                {
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Shuttle_T_Axis")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.NOT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_NOT");
                            bool bNOT = Sensor_T_Axis_NOT;
                            Data = Infotxt;//bNOT ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bNOT ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.POT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_POT");
                            bool bPOT = Sensor_T_Axis_POT;
                            Data = Infotxt;//bPOT ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bPOT ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.HOME:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_HOMESwitch");
                            bool bHome = Sensor_T_Axis_HOME;
                            Data = Infotxt;//bHome ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bHome ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.Pos:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_PosSensor");
                            bool bPos = Sensor_T_Axis_POS;
                            Data = Infotxt;//bPos ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bPos ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.Busy:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Busy");
                            bool bBusy = Sensor_T_Axis_Busy;
                            Data = Infotxt;//bBusy ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bBusy ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.OriginOK:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_HomeDone");
                            bool bOriginOK = Sensor_T_Axis_OriginOK;
                            Data = Infotxt;//bOriginOK ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bOriginOK ? Color.Lime : Master.ErrorIntervalColor;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.Deg_0:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_0DegSensor");
                            bool bDeg = Sensor_T_Axis_0DegSensor;
                            Data = Infotxt;//bDeg ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bDeg ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_T_Axis_SensorStatusColumn.Deg_180:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_180DegSensor");
                            bool bDeg = Sensor_T_Axis_180DegSensor;
                            Data = Infotxt;//bDeg ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bDeg ? Color.Lime : Color.White;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_Buffer_LP_Status1_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            List<DGV_Buffer1_LP_SensorStatus1Column> SensorList = new List<DGV_Buffer1_LP_SensorStatus1Column>();
            
            SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.Type);

            if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
            {
                SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.CST_Detect1);

                if (GetPortOperationMode() != PortOperationMode.OHT)
                    SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.CST_Detect2);
                else
                {
                    if(IsValidInputItemMapping(OHT_InputItem.LP_Placement_Detect_2.ToString()))
                        SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.CST_Detect2);
                }
            }

            SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.CST_Presence);
            if (GetParam().ePortType == PortType.OHT || GetParam().ePortType == PortType.MGV_OHT)
                SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.Hoist_Detect);

            if (GetParam().ePortType == PortType.AGV || GetParam().ePortType == PortType.MGV_AGV)
            {
                if(IsValidInputItemMapping(AGV_MGV_InputItem.Cart_Detect1.ToString()))
                    SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.Cart_Detect1);

                if (IsValidInputItemMapping(AGV_MGV_InputItem.Cart_Detect2.ToString()))
                    SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.Cart_Detect2);
            }
            else if (GetParam().ePortType == PortType.OHT || GetParam().ePortType == PortType.MGV_OHT)
            {
                if (IsValidInputItemMapping(OHT_InputItem.Cart_Detect1.ToString()))
                    SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.Cart_Detect1);

                if (IsValidInputItemMapping(OHT_InputItem.Cart_Detect2.ToString()))
                    SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.Cart_Detect2);

                if (IsValidInputItemMapping(OHT_InputItem.Door_Close_Status.ToString()))
                    SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.OHTDoorOpen);
            }

            if (GetParam().ePortType == PortType.MGV || GetParam().ePortType == PortType.MGV_AGV)
            {
                SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.LightCurtain);
            }

            SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.LED_Green);
            SensorList.Add(DGV_Buffer1_LP_SensorStatus1Column.LED_Red);

            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                if (nCount >= SensorList.Count)
                {
                    DGV_Cell.Value = string.Empty;
                    DGV_Cell.Style.BackColor = Color.DarkGray;
                    continue;
                }

                DGV_Buffer1_LP_SensorStatus1Column eDGV_Buffer1_LP_SensorStatus1Column = SensorList[nCount];
                string Data = string.Empty;

                switch (eDGV_Buffer1_LP_SensorStatus1Column)
                {
                    case DGV_Buffer1_LP_SensorStatus1Column.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Buffer2")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.CST_Detect1:
                        {
                            string Infotxt  = SynusLangPack.GetLanguage("DGV_CST1");
                            bool bSensor = Sensor_LP_CST_Detect1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.CST_Detect2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CST2");
                            bool bSensor = Sensor_LP_CST_Detect2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.CST_Presence:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Presence");
                            bool bSensor = Sensor_LP_CST_Presence;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.Hoist_Detect:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Hoist");
                            bool bSensor = Sensor_LP_Hoist_Detect;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Red : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.Cart_Detect1:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Cart1");
                            bool bSensor = Sensor_LP_Cart_Detect1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Red : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.Cart_Detect2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Cart2");
                            bool bSensor = Sensor_LP_Cart_Detect2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Red : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.LightCurtain:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_LightCurtain");
                            bool bSensor = Sensor_LightCurtain;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Red : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.OHTDoorOpen:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_OHTDoorOpen");
                            bool bSensor = !Status_OHT_Door_Close;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Red : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.LED_Green:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Green");
                            bool bSensor = Sensor_LP_LEDBar_Green;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_LP_SensorStatus1Column.LED_Red:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Red");
                            bool bSensor = Sensor_LP_LEDBar_Red;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_Buffer_OP_Status1_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DGV_Buffer2_OP_SensorStatus1Column eDGV_Buffer2_OP_SensorStatus1Column = (DGV_Buffer2_OP_SensorStatus1Column)nCount;
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                string Data = string.Empty;

                switch (eDGV_Buffer2_OP_SensorStatus1Column)
                {
                    case DGV_Buffer2_OP_SensorStatus1Column.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Buffer1")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Buffer2_OP_SensorStatus1Column.CST_Detect1:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CST1");
                            bool bSensor = Sensor_OP_CST_Detect1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_OP_SensorStatus1Column.CST_Detect2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CST2");
                            bool bSensor = Sensor_OP_CST_Detect2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_OP_SensorStatus1Column.CST_Presence:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Presence");
                            bool bSensor = Sensor_OP_CST_Presence;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_OP_SensorStatus1Column.Fork_Detect:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_ForkDetect");
                            bool bSensor = Sensor_OP_Fork_Detect;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    default:
                        {
                            DGV_Cell.Value = string.Empty;
                            DGV_Cell.Style.BackColor = Color.DarkGray;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_Shuttle_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DGV_Shuttle_CST_SensorStatusColumn eDGV_Shuttle_CST_SensorStatusColumn = (DGV_Shuttle_CST_SensorStatusColumn)nCount;
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                string Data = string.Empty;

                switch (eDGV_Shuttle_CST_SensorStatusColumn)
                {
                    case DGV_Shuttle_CST_SensorStatusColumn.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Shuttle")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Shuttle_CST_SensorStatusColumn.CST_Detect1:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CST1");
                            bool bSensor = Sensor_Shuttle_CSTDetect1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Shuttle_CST_SensorStatusColumn.CST_Detect2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CST2");
                            bool bSensor = Sensor_Shuttle_CSTDetect2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    default:
                        {
                            DGV_Cell.Value = string.Empty;
                            DGV_Cell.Style.BackColor = Color.DarkGray;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_Buffer_OP_Status2_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            List<DGV_Buffer1_OP_SensorStatus2Column> SensorList = new List<DGV_Buffer1_OP_SensorStatus2Column>();
            SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Type);
            if (GetMotionParam().IsCVUsed(BufferCV.Buffer_OP))
            {
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_IN);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_OUT);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_Error);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_Forwarding);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_Backwarding);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_CST_Inplace1);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_CST_Inplace2);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_RM_ForkDetect);

                //SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_CST_Opposite);
            }

            if (GetMotionParam().IsInverterType(PortAxis.Buffer_OP_Z))
            {
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_NOT);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_POS1);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_POS2);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_POT);
                SensorList.Add(DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_Error);
            }

            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                if (nCount >= SensorList.Count)
                {
                    DGV_Cell.Value = string.Empty;
                    DGV_Cell.Style.BackColor = Color.DarkGray;
                    continue;
                }
                DGV_Buffer1_OP_SensorStatus2Column eDGV_Buffer1_LP_SensorStatus2Column = SensorList[nCount];

                string Data = string.Empty;

                switch (eDGV_Buffer1_LP_SensorStatus2Column)
                {
                    case DGV_Buffer1_OP_SensorStatus2Column.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Buffer1")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_IN:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_BufferCVIn");
                            bool bSensor = Sensor_OP_CV_IN;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_OUT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_BufferCVOut");
                            bool bSensor = Sensor_OP_CV_STOP;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_Error:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_BufferCVError");
                            bool bSensor = Sensor_LP_CV_Error;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_Forwarding:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CV_Forwarding");
                            bool bSensor = Sensor_OP_CV_FWD_Status;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_Backwarding:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CV_Backwarding");
                            bool bSensor = Sensor_OP_CV_BWD_Status;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_NOT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_NOT");
                            bool bSensor = Sensor_OP_Z_NOT;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_POS1:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_Pos1");
                            bool bSensor = Sensor_OP_Z_POS1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_POS2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_Pos2");
                            bool bSensor = Sensor_OP_Z_POS2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_POT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_POT");
                            bool bSensor = Sensor_OP_Z_POT;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_Z_Axis_Error:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_Error");
                            bool bSensor = Sensor_OP_Z_Error;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_CST_Inplace1:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CST1");
                            bool bSensor = Sensor_OP_CST_Detect1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_CST_Inplace2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CST2");
                            bool bSensor = Sensor_OP_CST_Detect2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_CST_Opposite:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Presence");
                            bool bSensor = Sensor_OP_CST_Presence;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer1_OP_SensorStatus2Column.Buffer_CV_RM_ForkDetect:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_ForkDetect");
                            bool bSensor = Sensor_OP_Fork_Detect;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Red : Color.White;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_Buffer_LP_Status2_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            List<DGV_Buffer2_LP_SensorStatus2Column> SensorList = new List<DGV_Buffer2_LP_SensorStatus2Column>();
            SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Type);
            if (GetMotionParam().IsCVUsed(BufferCV.Buffer_LP))
            {
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_IN);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_OUT);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_Error);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_Forwarding);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_Backwarding);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_CST_Inplace1);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_CST_Inplace2);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_CST_Opposite);
            }

            if (GetMotionParam().IsInverterType(PortAxis.Buffer_LP_Z))
            {
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_NOT);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_POS1);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_POS2);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_POT);
                SensorList.Add(DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_Error);
            }

            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                if (nCount >= SensorList.Count)
                {
                    DGV_Cell.Value = string.Empty;
                    DGV_Cell.Style.BackColor = Color.DarkGray;
                    continue;
                }

                DGV_Buffer2_LP_SensorStatus2Column eDGV_Buffer2_OP_SensorStatus2Column = SensorList[nCount];
                string Data = string.Empty;

                switch (eDGV_Buffer2_OP_SensorStatus2Column)
                {
                    case DGV_Buffer2_LP_SensorStatus2Column.Type:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_Buffer2")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_IN:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_BufferCVIn");
                            bool bSensor = Sensor_LP_CV_IN;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_OUT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_BufferCVOut");
                            bool bSensor = Sensor_LP_CV_STOP;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_Error:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_BufferCVError");
                            bool bSensor = Sensor_LP_CV_Error;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_Forwarding:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CV_Forwarding");
                            bool bSensor = Sensor_LP_CV_FWD_Status;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_Backwarding:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_CV_Backwarding");
                            bool bSensor = Sensor_LP_CV_BWD_Status;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_NOT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_NOT");
                            bool bSensor = Sensor_LP_Z_NOT;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_POS1:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_Pos1");
                            bool bSensor = Sensor_LP_Z_POS1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_POS2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_Pos2");
                            bool bSensor = Sensor_LP_Z_POS2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_POT:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_POT");
                            bool bSensor = Sensor_LP_Z_POT;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_Z_Axis_Error:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Z_Axis_Error");
                            bool bSensor = Sensor_LP_Z_Error;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Master.ErrorIntervalColor : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_CST_Inplace1:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Inplace1");
                            bool bSensor = Sensor_LP_CST_Detect1;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_CST_Inplace2:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Buffer_Inplace2");
                            bool bSensor = Sensor_LP_CST_Detect2;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                    case DGV_Buffer2_LP_SensorStatus2Column.Buffer_CV_CST_Opposite:
                        {
                            string Infotxt = SynusLangPack.GetLanguage("DGV_Presence");
                            bool bSensor = Sensor_LP_CST_Presence;
                            Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                            DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        private void Update_DGVRow_CV_Buffer_BP_CST_Status_Sensor(ref DataGridView DGV, int nRowIndex)
        {
            List<BufferCV> BPList = new List<BufferCV>();

            foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
            {
                if (GetMotionParam().IsCVUsed(eBufferCV) && eBufferCV != BufferCV.Buffer_LP && eBufferCV != BufferCV.Buffer_OP)
                {
                    if (GetMotionParam().IsCSTDetectSensorEnable(eBufferCV))
                        BPList.Add(eBufferCV);
                }
            }

            DataGridViewRow DGVRow = DGV.Rows[nRowIndex];

            for (int nCount = 0; nCount < DGVRow.Cells.Count; nCount++)
            {
                DataGridViewCell DGV_Cell = DGVRow.Cells[nCount];
                string Data = string.Empty;

                switch (nCount)
                {
                    case 0:
                        {
                            Data = $"{SynusLangPack.GetLanguage("DGV_CVCST")}";
                            DGV_Cell.Style.BackColor = Color.Gainsboro;
                        }
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        {
                            if(nCount-1 < BPList.Count)
                            {
                                BufferCV eBufferCV = BPList[nCount - 1];

                                string BP = string.Empty;

                                if (eBufferCV == BufferCV.Buffer_BP1)
                                    BP = "BP1 CST";
                                else if (eBufferCV == BufferCV.Buffer_BP2)
                                    BP = "BP2 CST";
                                else if (eBufferCV == BufferCV.Buffer_BP3)
                                    BP = "BP3 CST";
                                else if (eBufferCV == BufferCV.Buffer_BP4)
                                    BP = "BP4 CST";

                                string Infotxt = BP;
                                bool bSensor = BufferCtrl_BP_CSTDetect_Status(eBufferCV);
                                Data = Infotxt;//bSensor ? Infotxt + $" On" : Infotxt + $" Off";
                                DGV_Cell.Style.BackColor = bSensor ? Color.Lime : Color.White;
                            }
                        }
                        break;
                }
                if ((string)DGV_Cell.Value != Data)
                    DGV_Cell.Value = Data;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }
        public void Update_DGV_CIMToPortBitMap(ref DataGridView DGV)
        {
            if(DGV.Rows.Count != Enum.GetValues(typeof(ReceiveBitMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(ReceiveBitMapIndex)).Length; nCount++)
                {
                    if (nCount >= GetParam().RecvBitMapSize)
                        continue;

                    DGV.Rows.Add();
                }
            }
            else
            {
                int nRowIndex = 0;
                foreach(ReceiveBitMapIndex receiveMap in Enum.GetValues(typeof(ReceiveBitMapIndex)))
                {
                    if (nRowIndex >= GetParam().RecvBitMapSize)
                        continue;

                    DataGridViewCell StartAddr_Cell     = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell         = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell          = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell         = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value    = $"0x{(GetParam().RecvBitMapStartAddr + (int)receiveMap).ToString("x4")}";
                    Index_Cell.Value        = $"{(int)receiveMap} [0x{((int)receiveMap).ToString("x2")}]";
                    Name_Cell.Value         = receiveMap.ToString();

                    bool value = Master.m_Port_RecvBitMap[(int)receiveMap + GetParam().RecvBitMapStartAddr];
                    Value_Cell.Value = $"{value}";
                    Value_Cell.Style.BackColor = value ? Color.Lime : Color.White;

                    nRowIndex++;
                }
            }
        }
        public void Update_DGV_PortToCIMBitMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(SendBitMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(SendBitMapIndex)).Length; nCount++)
                {
                    if (nCount >= GetParam().SendBitMapSize)
                        continue;

                    DGV.Rows.Add();
                }
            }
            else
            {
                int nRowIndex = 0;
                foreach (SendBitMapIndex sendMap in Enum.GetValues(typeof(SendBitMapIndex)))
                {
                    if (nRowIndex >= GetParam().SendBitMapSize)
                        continue;

                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(GetParam().SendBitMapStartAddr + (int)sendMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)sendMap} [0x{((int)sendMap).ToString("x2")}]";
                    Name_Cell.Value = sendMap.ToString();

                    bool value = Master.m_Port_SendBitMap[(int)sendMap + GetParam().SendBitMapStartAddr];
                    Value_Cell.Value = $"{value}";
                    Value_Cell.Style.BackColor = value ? Color.Lime : Color.White;

                    nRowIndex++;
                }
            }
        }
        public void Update_DGV_CIMToPortWordMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(ReceiveWordMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(ReceiveWordMapIndex)).Length; nCount++)
                {
                    if (nCount >= GetParam().RecvWordMapSize)
                        continue;

                    DGV.Rows.Add();
                }
            }
            else
            {
                int nRowIndex = 0;
                foreach (ReceiveWordMapIndex receiveMap in Enum.GetValues(typeof(ReceiveWordMapIndex)))
                {
                    if (nRowIndex >= GetParam().RecvWordMapSize)
                        continue;

                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(GetParam().RecvWordMapStartAddr + (int)receiveMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)receiveMap} [0x{((int)receiveMap).ToString("x2")}]";
                    Name_Cell.Value = receiveMap.ToString();

                    short value = Master.m_Port_RecvWordMap[(int)receiveMap + GetParam().RecvWordMapStartAddr];
                    string valueText = $"{value} [0x{value.ToString("x4")}]";

                    if ((string)Value_Cell.Value != valueText)
                    {
                        Value_Cell.Style.BackColor = Color.LightYellow;
                        Value_Cell.Tag = DateTime.Now;
                    }
                    else
                    {
                        if (Value_Cell.Tag != null && Value_Cell.Style.BackColor == Color.LightYellow)
                        {
                            DateTime dt = (DateTime)Value_Cell.Tag;

                            TimeSpan dtSpan = DateTime.Now - dt;

                            if (dtSpan.TotalSeconds > 5.0)
                                Value_Cell.Style.BackColor = Color.White;
                        }
                        else
                            Value_Cell.Style.BackColor = Color.White;
                    }

                    Value_Cell.Value = valueText;

                    nRowIndex++;
                }
            }
        }
        public void Update_DGV_PortToCIMWordMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(SendWordMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(SendWordMapIndex)).Length; nCount++)
                {
                    if (nCount >= GetParam().SendWordMapSize)
                        continue;

                    DGV.Rows.Add();
                }
            }
            else
            {
                int nRowIndex = 0;
                foreach (SendWordMapIndex sendMap in Enum.GetValues(typeof(SendWordMapIndex)))
                {
                    if (nRowIndex >= GetParam().SendWordMapSize)
                        continue;

                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(GetParam().SendWordMapStartAddr + (int)sendMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)sendMap} [0x{((int)sendMap).ToString("x2")}]";
                    Name_Cell.Value = sendMap.ToString();

                    short value = Master.m_Port_SendWordMap[(int)sendMap + GetParam().SendWordMapStartAddr];
                    string valueText = $"{value} [0x{value.ToString("x4")}]";

                    if ((string)Value_Cell.Value != valueText)
                    {
                        Value_Cell.Style.BackColor = Color.LightYellow;
                        Value_Cell.Tag = DateTime.Now;
                    }
                    else
                    {
                        if (Value_Cell.Tag != null && Value_Cell.Style.BackColor == Color.LightYellow)
                        {
                            DateTime dt = (DateTime)Value_Cell.Tag;

                            TimeSpan dtSpan = DateTime.Now - dt;

                            if (dtSpan.TotalSeconds > 5.0)
                                Value_Cell.Style.BackColor = Color.White;
                        }
                        else
                            Value_Cell.Style.BackColor = Color.White;
                    }

                    Value_Cell.Value = valueText;

                    nRowIndex++;
                }
            }
        }
        public List<DGV_BufferSensorRow> Get_DGV_BufferSensorList()
        {
            List<DGV_BufferSensorRow> BufferSensorList = new List<DGV_BufferSensorRow>();

            if (IsShuttleControlPort())
            {
                foreach (DGV_BufferSensorRow eBufferSensorRow in Enum.GetValues(typeof(DGV_BufferSensorRow)))
                {
                    if (eBufferSensorRow >= DGV_BufferSensorRow.LP_CST_Detect1 && eBufferSensorRow <= DGV_BufferSensorRow.Shuttle_CST_Detect2)
                    {
                        if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.Two_Buffer)
                        {
                            if (eBufferSensorRow == DGV_BufferSensorRow.LP_CST_Detect2 && GetPortOperationMode() == PortOperationMode.OHT && !IsValidInputItemMapping(OHT_InputItem.LP_Placement_Detect_2.ToString()))
                                continue;

                            if (eBufferSensorRow == DGV_BufferSensorRow.LP_Hoist_Detect && GetPortOperationMode() != PortOperationMode.OHT)
                                continue;

                            bool bCartDetectEnable = GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.MGV_AGV || GetParam().ePortType == PortType.MGV;

                            if (eBufferSensorRow == DGV_BufferSensorRow.LP_Cart_Detect1 && bCartDetectEnable)
                                continue;

                            if (eBufferSensorRow == DGV_BufferSensorRow.LP_Cart_Detect2 && bCartDetectEnable)
                                continue;

                            BufferSensorList.Add(eBufferSensorRow);
                        }
                        else if (GetMotionParam().eBufferType == ShuttleCtrlBufferType.One_Buffer)
                        {
                            if (eBufferSensorRow >= DGV_BufferSensorRow.LP_CST_Detect1 && eBufferSensorRow <= DGV_BufferSensorRow.LP_CST_Detect2)
                                continue;

                            if (eBufferSensorRow == DGV_BufferSensorRow.LP_Hoist_Detect && GetPortOperationMode() != PortOperationMode.OHT)
                                continue;

                            bool bCartDetectEnable = GetParam().ePortType == PortType.MGV_OHT || GetParam().ePortType == PortType.MGV_AGV || GetParam().ePortType == PortType.MGV;

                            if (eBufferSensorRow == DGV_BufferSensorRow.LP_Cart_Detect1 && bCartDetectEnable)
                                continue;

                            if (eBufferSensorRow == DGV_BufferSensorRow.LP_Cart_Detect2 && bCartDetectEnable)
                                continue;

                            BufferSensorList.Add(eBufferSensorRow);
                        }

                    }
                }
            }
            else if (IsBufferControlPort())
            {
                BufferSensorList.Add(DGV_BufferSensorRow.LP_CV_In);
                BufferSensorList.Add(DGV_BufferSensorRow.LP_CV_Out);
                BufferSensorList.Add(DGV_BufferSensorRow.LP_CST_Presence);
                BufferSensorList.Add(DGV_BufferSensorRow.LP_CV_Forwording);
                BufferSensorList.Add(DGV_BufferSensorRow.LP_CV_Backwording);
                BufferSensorList.Add(DGV_BufferSensorRow.OP_CV_In);
                BufferSensorList.Add(DGV_BufferSensorRow.OP_CV_Out);
                BufferSensorList.Add(DGV_BufferSensorRow.OP_CV_Forwording);
                BufferSensorList.Add(DGV_BufferSensorRow.OP_CV_Backwording);
                BufferSensorList.Add(DGV_BufferSensorRow.OP_CST_Detect1);
                BufferSensorList.Add(DGV_BufferSensorRow.OP_CST_Detect2);
                BufferSensorList.Add(DGV_BufferSensorRow.OP_Fork_Detect);

                if ((GetMotionParam().IsCVUsed(BufferCV.Buffer_BP1) && GetMotionParam().IsCSTDetectSensorEnable(BufferCV.Buffer_BP1)))
                    BufferSensorList.Add(DGV_BufferSensorRow.BP1_CST_Detect);

                if ((GetMotionParam().IsCVUsed(BufferCV.Buffer_BP2) && GetMotionParam().IsCSTDetectSensorEnable(BufferCV.Buffer_BP2)))
                    BufferSensorList.Add(DGV_BufferSensorRow.BP2_CST_Detect);

                if ((GetMotionParam().IsCVUsed(BufferCV.Buffer_BP3) && GetMotionParam().IsCSTDetectSensorEnable(BufferCV.Buffer_BP3)))
                    BufferSensorList.Add(DGV_BufferSensorRow.BP3_CST_Detect);

                if ((GetMotionParam().IsCVUsed(BufferCV.Buffer_BP4) && GetMotionParam().IsCSTDetectSensorEnable(BufferCV.Buffer_BP4)))
                    BufferSensorList.Add(DGV_BufferSensorRow.BP4_CST_Detect);

            }
            else if (IsEQPort())
            {

            }

            return BufferSensorList;
        }
        public void Update_DGV_BufferSensorList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_IOPageSensorStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Number"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Number");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorName");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorStatus");
                        break;
                }
            }

            List<DGV_BufferSensorRow> BufferSensorList = Get_DGV_BufferSensorList();

            if (BufferSensorList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != BufferSensorList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < BufferSensorList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                    }

                    DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_BufferSensorRow eBufferSensorRow = BufferSensorList[nRowCount];

                    DataGridViewCell DGV_NumberCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[2];

                    switch (eBufferSensorRow)
                    {
                        case DGV_BufferSensorRow.LP_CST_Detect1:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_CST_Detect1;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_CST_Detect2:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_CST_Detect2;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_CST_Presence:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_CST_Presence;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_Hoist_Detect:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_Hoist_Detect;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Red : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_Cart_Detect1:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_Cart_Detect1;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_Cart_Detect2:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_Cart_Detect2;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_LED_Green:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_LEDBar_Green;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_LED_Red:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_LEDBar_Red;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_CST_Detect1:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_CST_Detect1;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_CST_Detect2:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_CST_Detect2;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_CST_Presence:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_CST_Presence;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_Fork_Detect:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_Fork_Detect;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Red : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.Shuttle_CST_Detect1:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_Shuttle_CSTDetect1;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.Shuttle_CST_Detect2:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_Shuttle_CSTDetect2;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_CV_In:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_CV_IN;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_CV_Out:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_CV_STOP;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_CV_Forwording:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_CV_FWD_Status;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.LP_CV_Backwording:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_LP_CV_BWD_Status;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_CV_In:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_CV_IN;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_CV_Out:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_CV_STOP;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_CV_Forwording:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_CV_FWD_Status;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.OP_CV_Backwording:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = Sensor_OP_CV_BWD_Status;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferSensorRow.BP1_CST_Detect:
                        case DGV_BufferSensorRow.BP2_CST_Detect:
                        case DGV_BufferSensorRow.BP3_CST_Detect:
                        case DGV_BufferSensorRow.BP4_CST_Detect:
                            {
                                string BPName = string.Empty;
                                BufferCV eBufferCV = BufferCV.Buffer_BP1;

                                if (eBufferSensorRow == DGV_BufferSensorRow.BP1_CST_Detect)
                                {
                                    BPName = "BP1";
                                    eBufferCV = BufferCV.Buffer_BP1;
                                }
                                else if (eBufferSensorRow == DGV_BufferSensorRow.BP2_CST_Detect)
                                {
                                    BPName = "BP2";
                                    eBufferCV = BufferCV.Buffer_BP1;
                                }
                                else if (eBufferSensorRow == DGV_BufferSensorRow.BP3_CST_Detect)
                                {
                                    BPName = "BP3";
                                    eBufferCV = BufferCV.Buffer_BP1;
                                }
                                else if (eBufferSensorRow == DGV_BufferSensorRow.BP4_CST_Detect)
                                {
                                    BPName = "BP4";
                                    eBufferCV = BufferCV.Buffer_BP1;
                                }
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eBufferSensorRow}";
                                bool bStatus = BufferCtrl_BP_CSTDetect_Status(eBufferCV);
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }

        public List<DGV_ShuttleXAxisSensorRow> Get_DGV_ShuttleX_SensorList()
        {
            List<DGV_ShuttleXAxisSensorRow> ShuttleX_SensorList = new List<DGV_ShuttleXAxisSensorRow>();

            if (IsShuttleControlPort())
            {
                if (GetMotionParam().IsServoType(PortAxis.Shuttle_X))
                {
                    foreach (DGV_ShuttleXAxisSensorRow eDGV_ShuttleXAxisSensorRow in Enum.GetValues(typeof(DGV_ShuttleXAxisSensorRow)))
                    {
                        if (eDGV_ShuttleXAxisSensorRow == DGV_ShuttleXAxisSensorRow.WaitPosSensor && !GetMotionParam().IsWaitPosEnable(PortAxis.Shuttle_X))
                            continue;

                        ShuttleX_SensorList.Add(eDGV_ShuttleXAxisSensorRow);
                    }
                }
            }

            return ShuttleX_SensorList;
        }
        public void Update_DGV_ShuttleX_SensorList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_IOPageSensorStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Number"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Number");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorName");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorStatus");
                        break;
                }
            }

            List<DGV_ShuttleXAxisSensorRow> ShuttleX_SensorList = Get_DGV_ShuttleX_SensorList();


            if (ShuttleX_SensorList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != ShuttleX_SensorList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < ShuttleX_SensorList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                    }

                    DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_ShuttleXAxisSensorRow eDGV_ShuttleXAxisSensorRow = ShuttleX_SensorList[nRowCount];

                    DataGridViewCell DGV_NumberCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[2];

                    switch (eDGV_ShuttleXAxisSensorRow)
                    {
                        case DGV_ShuttleXAxisSensorRow.NOT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleXAxisSensorRow}";
                                bool bStatus = Sensor_X_Axis_NOT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleXAxisSensorRow.POT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleXAxisSensorRow}";
                                bool bStatus = Sensor_X_Axis_POT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleXAxisSensorRow.HOME:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleXAxisSensorRow}";
                                bool bStatus = Sensor_X_Axis_HOME;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleXAxisSensorRow.Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleXAxisSensorRow}";
                                bool bStatus = Sensor_X_Axis_POS;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleXAxisSensorRow.Busy:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleXAxisSensorRow}";
                                bool bStatus = Sensor_X_Axis_Busy;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleXAxisSensorRow.OriginOK:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleXAxisSensorRow}";
                                bool bStatus = Sensor_X_Axis_OriginOK;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleXAxisSensorRow.WaitPosSensor:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleXAxisSensorRow}";
                                bool bStatus = Sensor_X_Axis_WaitPosSensor;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }

        public List<DGV_ShuttleZAxisSensorRow> Get_DGV_ShuttleZ_SensorList()
        {
            List<DGV_ShuttleZAxisSensorRow> ShuttleZ_SensorList = new List<DGV_ShuttleZAxisSensorRow>();

            if (IsShuttleControlPort())
            {
                if (GetMotionParam().IsServoType(PortAxis.Shuttle_Z))
                {
                    foreach (DGV_ShuttleZAxisSensorRow eDGV_ShuttleZAxisSensorRow in Enum.GetValues(typeof(DGV_ShuttleZAxisSensorRow)))
                    {
                        if (eDGV_ShuttleZAxisSensorRow == DGV_ShuttleZAxisSensorRow.Cylinder_BWD_Pos ||
                            eDGV_ShuttleZAxisSensorRow == DGV_ShuttleZAxisSensorRow.Cylinder_FWD_Pos ||
                            eDGV_ShuttleZAxisSensorRow == DGV_ShuttleZAxisSensorRow.FWD_Command ||
                            eDGV_ShuttleZAxisSensorRow == DGV_ShuttleZAxisSensorRow.BWD_Command)
                            continue;

                        ShuttleZ_SensorList.Add(eDGV_ShuttleZAxisSensorRow);
                    }
                }
                else if (GetMotionParam().IsCylinderType(PortAxis.Shuttle_Z))
                {
                    foreach (DGV_ShuttleZAxisSensorRow eDGV_ShuttleZAxisSensorRow in Enum.GetValues(typeof(DGV_ShuttleZAxisSensorRow)))
                    {
                        if (eDGV_ShuttleZAxisSensorRow != DGV_ShuttleZAxisSensorRow.Cylinder_BWD_Pos &&
                            eDGV_ShuttleZAxisSensorRow != DGV_ShuttleZAxisSensorRow.Cylinder_FWD_Pos &&
                            eDGV_ShuttleZAxisSensorRow != DGV_ShuttleZAxisSensorRow.BWD_Command &&
                            eDGV_ShuttleZAxisSensorRow != DGV_ShuttleZAxisSensorRow.FWD_Command &&
                            eDGV_ShuttleZAxisSensorRow != DGV_ShuttleZAxisSensorRow.Busy)
                            continue;

                        ShuttleZ_SensorList.Add(eDGV_ShuttleZAxisSensorRow);
                    }
                }
            }
            return ShuttleZ_SensorList;
        }
        public void Update_DGV_ShuttleZ_SensorList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_IOPageSensorStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Number"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Number");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorName");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorStatus");
                        break;
                }
            }

            List<DGV_ShuttleZAxisSensorRow> ShuttleZ_SensorList = Get_DGV_ShuttleZ_SensorList();


            if (ShuttleZ_SensorList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != ShuttleZ_SensorList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < ShuttleZ_SensorList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                    }

                    DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_ShuttleZAxisSensorRow eDGV_ShuttleZAxisSensorRow = ShuttleZ_SensorList[nRowCount];

                    DataGridViewCell DGV_NumberCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[2];

                    switch (eDGV_ShuttleZAxisSensorRow)
                    {
                        case DGV_ShuttleZAxisSensorRow.NOT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_NOT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.POT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_POT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.HOME:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_HOME;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_POS;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.Busy:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_Busy;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.OriginOK:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_OriginOK;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.Cylinder_BWD_Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_BWDSensor;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.Cylinder_FWD_Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";
                                bool bStatus = Sensor_Z_Axis_FWDSensor;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.BWD_Command:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";

                                CylCtrlList eCylCtrlList = CylCtrlList.BWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Shuttle_Z);
                                bool bBWDOn = CylinderCtrl_GetRunStatus(PortAxis.Shuttle_Z, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bBWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bBWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bBWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_ShuttleZAxisSensorRow.FWD_Command:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleZAxisSensorRow}";

                                CylCtrlList eCylCtrlList = CylCtrlList.FWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Shuttle_Z);
                                bool bFWDOn = CylinderCtrl_GetRunStatus(PortAxis.Shuttle_Z, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }

        public List<DGV_ShuttleTAxisSensorRow> Get_DGV_ShuttleT_SensorList()
        {
            List<DGV_ShuttleTAxisSensorRow> ShuttleT_SensorList = new List<DGV_ShuttleTAxisSensorRow>();

            if (IsShuttleControlPort())
            {
                if (GetMotionParam().IsServoType(PortAxis.Shuttle_T))
                {
                    foreach (DGV_ShuttleTAxisSensorRow eDGV_ShuttleTAxisSensorRow in Enum.GetValues(typeof(DGV_ShuttleTAxisSensorRow)))
                    {
                        ShuttleT_SensorList.Add(eDGV_ShuttleTAxisSensorRow);
                    }
                }
            }

            return ShuttleT_SensorList;
        }
        public void Update_DGV_ShuttleT_SensorList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_IOPageSensorStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Number"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Number");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorName");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorStatus");
                        break;
                }
            }

            List<DGV_ShuttleTAxisSensorRow> ShuttleT_SensorList = Get_DGV_ShuttleT_SensorList();


            if (ShuttleT_SensorList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != ShuttleT_SensorList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < ShuttleT_SensorList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                    }


                    DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_ShuttleTAxisSensorRow eDGV_ShuttleTAxisSensorRow = ShuttleT_SensorList[nRowCount];

                    DataGridViewCell DGV_NumberCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[2];

                    switch (eDGV_ShuttleTAxisSensorRow)
                    {
                        case DGV_ShuttleTAxisSensorRow.NOT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_NOT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleTAxisSensorRow.POT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_POT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleTAxisSensorRow.HOME:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_HOME;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleTAxisSensorRow.Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_POS;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleTAxisSensorRow.Busy:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_Busy;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleTAxisSensorRow.OriginOK:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_OriginOK;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleTAxisSensorRow.Degree_0_Position:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_0DegSensor;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_ShuttleTAxisSensorRow.Degree_180_Position:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_ShuttleTAxisSensorRow}";
                                bool bStatus = Sensor_T_Axis_180DegSensor;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }

        public List<DGV_BufferLP_ZAxisSensorRow> Get_DGV_BufferLP_Z_SensorList()
        {
            List<DGV_BufferLP_ZAxisSensorRow> DGV_BufferLP_ZAxisSensorList = new List<DGV_BufferLP_ZAxisSensorRow>();

            if (IsBufferControlPort())
            {
                //if (GetMotionParam().IsInverterType(PortAxis.Buffer_LP_Z))
                //{
                foreach (DGV_BufferLP_ZAxisSensorRow eDGV_BufferLP_ZAxisSensorRow in Enum.GetValues(typeof(DGV_BufferLP_ZAxisSensorRow)))
                {
                    if (GetMotionParam().IsInverterType(PortAxis.Buffer_LP_Z))
                    {
                        if (eDGV_BufferLP_ZAxisSensorRow >= DGV_BufferLP_ZAxisSensorRow.NOT && eDGV_BufferLP_ZAxisSensorRow <= DGV_BufferLP_ZAxisSensorRow.LowSpeedBWDFlag)
                            DGV_BufferLP_ZAxisSensorList.Add(eDGV_BufferLP_ZAxisSensorRow);
                    }
                    else if (GetMotionParam().IsCylinderType(PortAxis.Buffer_LP_Z))
                    {
                        if (eDGV_BufferLP_ZAxisSensorRow >= DGV_BufferLP_ZAxisSensorRow.Cylinder_BWD_Pos && eDGV_BufferLP_ZAxisSensorRow <= DGV_BufferLP_ZAxisSensorRow.FWD_Command)
                            DGV_BufferLP_ZAxisSensorList.Add(eDGV_BufferLP_ZAxisSensorRow);
                        else if (eDGV_BufferLP_ZAxisSensorRow == DGV_BufferLP_ZAxisSensorRow.Busy)
                            DGV_BufferLP_ZAxisSensorList.Add(eDGV_BufferLP_ZAxisSensorRow);
                    }
                }
                //}

            }

            return DGV_BufferLP_ZAxisSensorList;
        }
        public void Update_DGV_BufferLP_Z_SensorList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_IOPageSensorStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Number"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Number");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorName");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorStatus");
                        break;
                }
            }

            List<DGV_BufferLP_ZAxisSensorRow> DGV_BufferLP_ZAxisSensorList = Get_DGV_BufferLP_Z_SensorList();


            if (DGV_BufferLP_ZAxisSensorList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != DGV_BufferLP_ZAxisSensorList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < DGV_BufferLP_ZAxisSensorList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                    }

                    DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_BufferLP_ZAxisSensorRow eDGV_BufferLP_ZAxisSensorRow = DGV_BufferLP_ZAxisSensorList[nRowCount];

                    DataGridViewCell DGV_NumberCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[2];

                    switch (eDGV_BufferLP_ZAxisSensorRow)
                    {
                        case DGV_BufferLP_ZAxisSensorRow.NOT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                bool bStatus = Sensor_LP_Z_NOT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Red : Color.White;
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.POT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                bool bStatus = Sensor_LP_Z_POT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Red : Color.White;
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.Pos1:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                bool bStatus = Sensor_LP_Z_POS1;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.Pos2:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                bool bStatus = Sensor_LP_Z_POS2;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.Busy:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                bool bStatus = IsPortAxisBusy(PortAxis.Buffer_LP_Z);
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.HighSpeed:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_LP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                    var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_LP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.HighSpeed);
                                    bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                    if (GetMotionParam().IsValidIO(IOParam))
                                    {
                                        int StartAddr = IOParam.StartAddr;
                                        int Bit = IOParam.Bit;
                                        DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                        DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        DGV_Value.Value = "Not Define";
                                        DGV_Value.Style.ForeColor = Color.Blue;
                                        DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.LowSpeed:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_LP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                    var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_LP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.LowSpeed);
                                    bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                    if (GetMotionParam().IsValidIO(IOParam))
                                    {
                                        int StartAddr = IOParam.StartAddr;
                                        int Bit = IOParam.Bit;
                                        DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                        DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        DGV_Value.Value = "Not Define";
                                        DGV_Value.Style.ForeColor = Color.Blue;
                                        DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.FWD:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_LP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.FWD);
                                bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                if (GetMotionParam().IsValidIO(IOParam))
                                {
                                    int StartAddr = IOParam.StartAddr;
                                    int Bit = IOParam.Bit;
                                    DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.BWD:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_LP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.BWD);
                                bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                if (GetMotionParam().IsValidIO(IOParam))
                                {
                                    int StartAddr = IOParam.StartAddr;
                                    int Bit = IOParam.Bit;
                                    DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.HighSpeedFWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_LP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_LP_Z, InvCtrlType.HighSpeedFWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.LowSpeedFWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_LP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_LP_Z, InvCtrlType.LowSpeedFWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.HighSpeedBWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_LP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_LP_Z, InvCtrlType.HighSpeedBWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.LowSpeedBWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_LP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_LP_Z, InvCtrlType.LowSpeedBWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.Cylinder_FWD_Pos:
                        case DGV_BufferLP_ZAxisSensorRow.Cylinder_BWD_Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                

                                CylCtrlList eCylCtrlList = CylCtrlList.FWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Buffer_LP_Z);

                                if (eDGV_BufferLP_ZAxisSensorRow == DGV_BufferLP_ZAxisSensorRow.Cylinder_FWD_Pos)
                                {
                                    eCylCtrlList = CylCtrlList.FWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                }
                                else if (eDGV_BufferLP_ZAxisSensorRow == DGV_BufferLP_ZAxisSensorRow.Cylinder_BWD_Pos)
                                {
                                    eCylCtrlList = CylCtrlList.BWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                }

                                bool bFWDOn = CylinderCtrl_GetPosSensorOn(PortAxis.Buffer_LP_Z, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetPosSensorIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetPosSensorIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetPosSensorIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_BufferLP_ZAxisSensorRow.FWD_Command:
                        case DGV_BufferLP_ZAxisSensorRow.BWD_Command:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";

                                CylCtrlList eCylCtrlList = CylCtrlList.FWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Buffer_LP_Z);

                                if (eDGV_BufferLP_ZAxisSensorRow == DGV_BufferLP_ZAxisSensorRow.FWD_Command)
                                {
                                    eCylCtrlList = CylCtrlList.FWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                }
                                else if (eDGV_BufferLP_ZAxisSensorRow == DGV_BufferLP_ZAxisSensorRow.BWD_Command)
                                {
                                    eCylCtrlList = CylCtrlList.BWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferLP_ZAxisSensorRow}";
                                }

                                bool bFWDOn = CylinderCtrl_GetRunStatus(PortAxis.Buffer_LP_Z, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }

        public List<DGV_BufferOP_ZAxisSensorRow> Get_DGV_BufferOP_Z_SensorList()
        {
            List<DGV_BufferOP_ZAxisSensorRow> DGV_BufferOP_ZAxisSensorList = new List<DGV_BufferOP_ZAxisSensorRow>();

            if (IsBufferControlPort())
            {
                //if (GetMotionParam().IsInverterType(PortAxis.Buffer_OP_Z))
                //{
                //    foreach (DGV_BufferOP_ZAxisSensorRow eDGV_BufferOP_ZAxisSensorRow in Enum.GetValues(typeof(DGV_BufferOP_ZAxisSensorRow)))
                //    {
                //        DGV_BufferOP_ZAxisSensorList.Add(eDGV_BufferOP_ZAxisSensorRow);
                //    }
                //}
                foreach (DGV_BufferOP_ZAxisSensorRow eDGV_BufferOP_ZAxisSensorRow in Enum.GetValues(typeof(DGV_BufferOP_ZAxisSensorRow)))
                {
                    if (GetMotionParam().IsInverterType(PortAxis.Buffer_OP_Z))
                    {
                        if (eDGV_BufferOP_ZAxisSensorRow >= DGV_BufferOP_ZAxisSensorRow.NOT && eDGV_BufferOP_ZAxisSensorRow <= DGV_BufferOP_ZAxisSensorRow.LowSpeedBWDFlag)
                            DGV_BufferOP_ZAxisSensorList.Add(eDGV_BufferOP_ZAxisSensorRow);
                    }
                    else if (GetMotionParam().IsCylinderType(PortAxis.Buffer_OP_Z))
                    {
                        if (eDGV_BufferOP_ZAxisSensorRow >= DGV_BufferOP_ZAxisSensorRow.Cylinder_BWD_Pos && eDGV_BufferOP_ZAxisSensorRow <= DGV_BufferOP_ZAxisSensorRow.FWD_Command)
                            DGV_BufferOP_ZAxisSensorList.Add(eDGV_BufferOP_ZAxisSensorRow);
                        else if (eDGV_BufferOP_ZAxisSensorRow == DGV_BufferOP_ZAxisSensorRow.Busy)
                            DGV_BufferOP_ZAxisSensorList.Add(eDGV_BufferOP_ZAxisSensorRow);
                    }
                }
            }

            return DGV_BufferOP_ZAxisSensorList;
        }
        public void Update_DGV_BufferOP_Z_SensorList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_IOPageSensorStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Number"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Number");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorName");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorStatus");
                        break;
                }
            }

            List<DGV_BufferOP_ZAxisSensorRow> DGV_BufferOP_ZAxisSensorList = Get_DGV_BufferOP_Z_SensorList();


            if (DGV_BufferOP_ZAxisSensorList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != DGV_BufferOP_ZAxisSensorList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < DGV_BufferOP_ZAxisSensorList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                    }

                    DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_BufferOP_ZAxisSensorRow eDGV_BufferOP_ZAxisSensorRow = DGV_BufferOP_ZAxisSensorList[nRowCount];

                    DataGridViewCell DGV_NumberCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[2];

                    switch (eDGV_BufferOP_ZAxisSensorRow)
                    {
                        case DGV_BufferOP_ZAxisSensorRow.NOT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                bool bStatus = Sensor_OP_Z_NOT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Red : Color.White;
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.POT:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                bool bStatus = Sensor_OP_Z_POT;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Red : Color.White;
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.Pos1:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                bool bStatus = Sensor_OP_Z_POS1;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.Pos2:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                bool bStatus = Sensor_OP_Z_POS2;
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.Busy:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                bool bStatus = IsPortAxisBusy(PortAxis.Buffer_OP_Z);
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.HighSpeed:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_OP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                    var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_OP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.HighSpeed);
                                    bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                    if (GetMotionParam().IsValidIO(IOParam))
                                    {
                                        int StartAddr = IOParam.StartAddr;
                                        int Bit = IOParam.Bit;
                                        DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                        DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        DGV_Value.Value = "Not Define";
                                        DGV_Value.Style.ForeColor = Color.Blue;
                                        DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.LowSpeed:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_OP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                    var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_OP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.LowSpeed);
                                    bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                    if (GetMotionParam().IsValidIO(IOParam))
                                    {
                                        int StartAddr = IOParam.StartAddr;
                                        int Bit = IOParam.Bit;
                                        DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                        DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                        DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        DGV_Value.Value = "Not Define";
                                        DGV_Value.Style.ForeColor = Color.Blue;
                                        DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                    }
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.FWD:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_OP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.FWD);
                                bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                if (GetMotionParam().IsValidIO(IOParam))
                                {
                                    int StartAddr = IOParam.StartAddr;
                                    int Bit = IOParam.Bit;
                                    DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.BWD:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                var IOParam = GetMotionParam().Ctrl_Axis[(int)PortAxis.Buffer_OP_Z].inverterParam.GetIOParam(InvIOCtrlFlag.BWD);
                                bool Status = GetOutBit(IOParam.StartAddr, IOParam.Bit);

                                if (GetMotionParam().IsValidIO(IOParam))
                                {
                                    int StartAddr = IOParam.StartAddr;
                                    int Bit = IOParam.Bit;
                                    DGV_Value.Value = Status ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.HighSpeedFWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_OP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_OP_Z, InvCtrlType.HighSpeedFWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.LowSpeedFWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_OP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_OP_Z, InvCtrlType.LowSpeedFWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.HighSpeedBWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_OP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_OP_Z, InvCtrlType.HighSpeedBWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.LowSpeedBWDFlag:
                            {
                                if (GetMotionParam().GetShuttleCtrl_InvParam(PortAxis.Buffer_OP_Z).InvCtrlMode == InvCtrlMode.IOControl)
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";

                                    bool Status = InverterCtrl_GetRunStatus(PortAxis.Buffer_OP_Z, InvCtrlType.LowSpeedBWD);
                                    DGV_Value.Value = Status ? $"On" : $"Off";
                                    DGV_Value.Style.ForeColor = Status ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = Status ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_NumberCell.Value = $"{nRowCount}";
                                    DGV_NameCell.Value = string.Empty;
                                    DGV_Value.Value = string.Empty;
                                    DGV_Value.Style.ForeColor = Color.DarkGray;
                                    DGV_Value.Style.BackColor = Color.DarkGray;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.Cylinder_FWD_Pos:
                        case DGV_BufferOP_ZAxisSensorRow.Cylinder_BWD_Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";

                                CylCtrlList eCylCtrlList = CylCtrlList.FWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Buffer_OP_Z);

                                if (eDGV_BufferOP_ZAxisSensorRow == DGV_BufferOP_ZAxisSensorRow.Cylinder_FWD_Pos)
                                {
                                    eCylCtrlList = CylCtrlList.FWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                }
                                else if (eDGV_BufferOP_ZAxisSensorRow == DGV_BufferOP_ZAxisSensorRow.Cylinder_BWD_Pos)
                                {
                                    eCylCtrlList = CylCtrlList.BWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                }

                                bool bFWDOn = CylinderCtrl_GetPosSensorOn(PortAxis.Buffer_OP_Z, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetPosSensorIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetPosSensorIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetPosSensorIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_BufferOP_ZAxisSensorRow.FWD_Command:
                        case DGV_BufferOP_ZAxisSensorRow.BWD_Command:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";

                                CylCtrlList eCylCtrlList = CylCtrlList.FWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Buffer_OP_Z);

                                if (eDGV_BufferOP_ZAxisSensorRow == DGV_BufferOP_ZAxisSensorRow.FWD_Command)
                                {
                                    eCylCtrlList = CylCtrlList.FWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                }
                                else if (eDGV_BufferOP_ZAxisSensorRow == DGV_BufferOP_ZAxisSensorRow.BWD_Command)
                                {
                                    eCylCtrlList = CylCtrlList.BWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_ZAxisSensorRow}";
                                }

                                bool bFWDOn = CylinderCtrl_GetRunStatus(PortAxis.Buffer_OP_Z, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }


        public List<DGV_BufferOP_YAxisSensorRow> Get_DGV_BufferOP_Y_SensorList()
        {
            List<DGV_BufferOP_YAxisSensorRow> DGV_BufferOP_YAxisSensorList = new List<DGV_BufferOP_YAxisSensorRow>();

            if (IsBufferControlPort())
            {
                foreach (DGV_BufferOP_YAxisSensorRow eDGV_BufferOP_YAxisSensorRow in Enum.GetValues(typeof(DGV_BufferOP_YAxisSensorRow)))
                {
                    if (GetMotionParam().IsCylinderType(PortAxis.Buffer_OP_Y))
                    {
                        DGV_BufferOP_YAxisSensorList.Add(eDGV_BufferOP_YAxisSensorRow);
                    }
                }
            }

            return DGV_BufferOP_YAxisSensorList;
        }
        public void Update_DGV_BufferOP_Y_SensorList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_IOPageSensorStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Number"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Number");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorName");
                        break;
                    case (int)DGV_IOPageSensorStatusColumn.SensorStatus:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SensorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SensorStatus");
                        break;
                }
            }

            List<DGV_BufferOP_YAxisSensorRow> DGV_BufferOP_YAxisSensorList = Get_DGV_BufferOP_Y_SensorList();


            if (DGV_BufferOP_YAxisSensorList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != DGV_BufferOP_YAxisSensorList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < DGV_BufferOP_YAxisSensorList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                    }

                    DataGridViewFunc.AutoRowSize(DGV, 25, 25, 40);
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DGV_BufferOP_YAxisSensorRow eDGV_BufferOP_YAxisSensorRow = DGV_BufferOP_YAxisSensorList[nRowCount];

                    DataGridViewCell DGV_NumberCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[2];

                    switch (eDGV_BufferOP_YAxisSensorRow)
                    {
                        case DGV_BufferOP_YAxisSensorRow.Busy:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";
                                DGV_NameCell.Value = $"{eDGV_BufferOP_YAxisSensorRow}";
                                bool bStatus = IsPortAxisBusy(PortAxis.Buffer_OP_Y);
                                DGV_Value.Value = bStatus ? "On" : "Off";
                                DGV_Value.Style.BackColor = bStatus ? Color.Lime : Color.White;
                            }
                            break;
                        case DGV_BufferOP_YAxisSensorRow.Cylinder_FWD_Pos:
                        case DGV_BufferOP_YAxisSensorRow.Cylinder_BWD_Pos:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";

                                CylCtrlList eCylCtrlList = CylCtrlList.FWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Buffer_OP_Y);

                                if (eDGV_BufferOP_YAxisSensorRow == DGV_BufferOP_YAxisSensorRow.Cylinder_FWD_Pos)
                                {
                                    eCylCtrlList = CylCtrlList.FWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_YAxisSensorRow}";
                                }
                                else if (eDGV_BufferOP_YAxisSensorRow == DGV_BufferOP_YAxisSensorRow.Cylinder_BWD_Pos)
                                {
                                    eCylCtrlList = CylCtrlList.BWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_YAxisSensorRow}";
                                }

                                bool bFWDOn = CylinderCtrl_GetPosSensorOn(PortAxis.Buffer_OP_Y, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetPosSensorIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetPosSensorIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetPosSensorIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                        case DGV_BufferOP_YAxisSensorRow.FWD_Command:
                        case DGV_BufferOP_YAxisSensorRow.BWD_Command:
                            {
                                DGV_NumberCell.Value = $"{nRowCount}";

                                CylCtrlList eCylCtrlList = CylCtrlList.FWD;
                                var CylinderParam = GetMotionParam().GetShuttleCtrl_CylParam(PortAxis.Buffer_OP_Y);

                                if (eDGV_BufferOP_YAxisSensorRow == DGV_BufferOP_YAxisSensorRow.FWD_Command)
                                {
                                    eCylCtrlList = CylCtrlList.FWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_YAxisSensorRow}";
                                }
                                else if (eDGV_BufferOP_YAxisSensorRow == DGV_BufferOP_YAxisSensorRow.BWD_Command)
                                {
                                    eCylCtrlList = CylCtrlList.BWD;
                                    DGV_NameCell.Value = $"{eDGV_BufferOP_YAxisSensorRow}";
                                }

                                bool bFWDOn = CylinderCtrl_GetRunStatus(PortAxis.Buffer_OP_Y, eCylCtrlList);

                                if (GetMotionParam().IsValidIO(CylinderParam.GetCtrlIOParam(eCylCtrlList)))
                                {
                                    int StartAddr = CylinderParam.GetCtrlIOParam(eCylCtrlList).StartAddr;
                                    int Bit = CylinderParam.GetCtrlIOParam(eCylCtrlList).Bit;
                                    DGV_Value.Value = bFWDOn ? $"On [{StartAddr},{Bit}]" : $"Off [{StartAddr},{Bit}]";
                                    DGV_Value.Style.ForeColor = bFWDOn ? Color.Blue : Color.Black;
                                    DGV_Value.Style.BackColor = bFWDOn ? Color.Lime : Color.White;
                                }
                                else
                                {
                                    DGV_Value.Value = "Not Define";
                                    DGV_Value.Style.ForeColor = Color.Blue;
                                    DGV_Value.Style.BackColor = Master.ErrorIntervalColor;
                                }
                            }
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }
        public void Update_DGV_AlarmList(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_AlarmListColumn.Index:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Index"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Index");
                        break;
                    case (int)DGV_AlarmListColumn.Hex:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Hex"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Hex");
                        break;
                    case (int)DGV_AlarmListColumn.AlarmName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_AlarmName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_AlarmName");
                        break;
                    case (int)DGV_AlarmListColumn.State:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_AlarmState"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_AlarmState");
                        break;
                }
            }

            if (DGV.Rows.Count != Enum.GetValues(typeof(PortAlarm)).Length)
            {
                DGV.Rows.Clear();
                foreach (PortAlarm ePortAlarm in Enum.GetValues(typeof(PortAlarm)))
                {
                    int Index = (int)ePortAlarm;
                    string Hex = $"0x{Index.ToString("x2")}";
                    string Name = GetPortAlarmComment((short)Index);
                    string State = AlarmContains(ePortAlarm) ? "On" : "Off";
                    DGV.Rows.Add(Index.ToString(), Hex, Name, State);
                }
            }
            else
            {
                for(int nRowCount = 0; nRowCount < Enum.GetValues(typeof(PortAlarm)).Length; nRowCount++)
                {
                    DataGridViewCell DGV_IndexCell = DGV.Rows[nRowCount].Cells[(int)DGV_AlarmListColumn.Index];
                    DataGridViewCell DGV_StateCell = DGV.Rows[nRowCount].Cells[(int)DGV_AlarmListColumn.State];
                    short Index = Convert.ToInt16(DGV_IndexCell.Value);

                    PortAlarm ePortAlarm = IndexToAlarmEnum(Index);
                    string State = AlarmContains(ePortAlarm) ? "On" : "Off";
                    DGV_StateCell.Value = State;
                    DGV_StateCell.Style.BackColor = State == "On" ? Color.Red : Color.LightGray;
                }
            }
        }
        
        private void MotionParamGridViewInit(ref DataGridView DGV, string Type)
        {
            DGV = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(DGV)).BeginInit();

            DGV.AllowUserToAddRows = false;
            DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DGV.BackgroundColor = System.Drawing.Color.AliceBlue;
            DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            DGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;

            // 
            // Column6
            // 
            DataGridViewCellStyle NameColumnCellStyle = new DataGridViewCellStyle();
            DataGridViewTextBoxColumn DGVNameColumn = new DataGridViewTextBoxColumn();
            DGVNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            NameColumnCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DGVNameColumn.FillWeight = Type.Contains("Servo") ? 100 : 200;
            DGVNameColumn.DefaultCellStyle = NameColumnCellStyle;
            DGVNameColumn.HeaderText = $"{Type}";
            DGVNameColumn.Name = "Column6";
            DGVNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGVNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            DGVNameColumn.ReadOnly = true;
            // 
            // Column5
            // 
            DataGridViewCellStyle ValueColumnCellStyle = new DataGridViewCellStyle();
            DataGridViewTextBoxColumn DGVValueColumn = new DataGridViewTextBoxColumn();
            DGVValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            ValueColumnCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            DGVValueColumn.FillWeight = 70;
            DGVValueColumn.DefaultCellStyle = ValueColumnCellStyle;
            DGVValueColumn.HeaderText = "Value";
            DGVValueColumn.Name = "Column5";
            DGVValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGVValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            DataGridViewCellStyle ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle();
            ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkGray;
            ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            ColumnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            DGV.ColumnHeadersDefaultCellStyle = ColumnHeadersDefaultCellStyle;
            DGV.ColumnHeadersHeight = 40;
            DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { DGVNameColumn, DGVValueColumn });

            DataGridViewCellStyle DefaultCellStyle = new DataGridViewCellStyle();
            DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DefaultCellStyle.BackColor = System.Drawing.Color.White;
            DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            DGV.DefaultCellStyle = DefaultCellStyle;
            DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            DGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            DGV.EnableHeadersVisualStyles = false;
            DGV.GridColor = System.Drawing.Color.LightGray;
            DGV.Location = new System.Drawing.Point(0, 0);
            DGV.Margin = new System.Windows.Forms.Padding(0);
            DGV.MultiSelect = false;
            DGV.Name = "DGV_Item";
            DGV.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            DataGridViewCellStyle RowHeadersDefaultCellStyle = new DataGridViewCellStyle();
            RowHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            RowHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            RowHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            RowHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            DGV.RowHeadersDefaultCellStyle = RowHeadersDefaultCellStyle;
            DGV.RowHeadersVisible = false;
            DGV.RowTemplate.Height = 23;
            DGV.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            DGV.Size = new System.Drawing.Size(275, 634);
            DGV.TabIndex = 34;

            DGV.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DGV.Margin = new Padding(2, 2, 2, 2);

            ((System.ComponentModel.ISupportInitialize)(DGV)).EndInit();
        }
        private void IOParamGridViewInit(ref DataGridView DGV, string Type)
        {
            DGV = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(DGV)).BeginInit();

            DGV.AllowUserToAddRows = false;
            DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DGV.BackgroundColor = System.Drawing.Color.AliceBlue;
            DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            DGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Raised;

            // 
            // Column6
            // 
            DataGridViewCellStyle NameColumnCellStyle = new DataGridViewCellStyle();
            DataGridViewTextBoxColumn DGVNameColumn = new DataGridViewTextBoxColumn();
            DGVNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            NameColumnCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DGVNameColumn.FillWeight = Type.Contains("Servo") ? 100 : 200;
            DGVNameColumn.DefaultCellStyle = NameColumnCellStyle;
            DGVNameColumn.HeaderText = $"{Type}";
            DGVNameColumn.Name = "Column6";
            DGVNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGVNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            DGVNameColumn.ReadOnly = true;
            // 
            // Column5
            // 
            DataGridViewCellStyle ValueColumnCellStyle = new DataGridViewCellStyle();
            DataGridViewTextBoxColumn DGVValueColumn = new DataGridViewTextBoxColumn();
            DGVValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            ValueColumnCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            DGVValueColumn.FillWeight = 70;
            DGVValueColumn.DefaultCellStyle = ValueColumnCellStyle;
            DGVValueColumn.HeaderText = "";
            DGVValueColumn.Name = "Column5";
            DGVValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGVValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            DataGridViewCellStyle ValueColumn2CellStyle = new DataGridViewCellStyle();
            DataGridViewTextBoxColumn DGVValue2Column = new DataGridViewTextBoxColumn();
            DGVValue2Column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            ValueColumn2CellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            DGVValue2Column.FillWeight = 70;
            DGVValue2Column.DefaultCellStyle = ValueColumn2CellStyle;
            DGVValue2Column.HeaderText = "";
            DGVValue2Column.Name = "Column5";
            DGVValue2Column.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGVValue2Column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            DataGridViewCellStyle ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle();
            ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkGray;
            ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            ColumnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            ColumnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            ColumnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            DGV.ColumnHeadersDefaultCellStyle = ColumnHeadersDefaultCellStyle;
            DGV.ColumnHeadersHeight = 40;
            DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { DGVNameColumn, DGVValueColumn, DGVValue2Column });

            DataGridViewCellStyle DefaultCellStyle = new DataGridViewCellStyle();
            DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DefaultCellStyle.BackColor = System.Drawing.Color.White;
            DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            DGV.DefaultCellStyle = DefaultCellStyle;
            DGV.Dock = System.Windows.Forms.DockStyle.Fill;
            DGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            DGV.EnableHeadersVisualStyles = false;
            DGV.GridColor = System.Drawing.Color.LightGray;
            DGV.Location = new System.Drawing.Point(0, 0);
            DGV.Margin = new System.Windows.Forms.Padding(0);
            DGV.MultiSelect = false;
            DGV.Name = "DGV_Item";
            DGV.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;

            DataGridViewCellStyle RowHeadersDefaultCellStyle = new DataGridViewCellStyle();
            RowHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            RowHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            RowHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            RowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            RowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            RowHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            DGV.RowHeadersDefaultCellStyle = RowHeadersDefaultCellStyle;
            DGV.RowHeadersVisible = false;
            DGV.RowTemplate.Height = 23;
            DGV.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            DGV.Size = new System.Drawing.Size(275, 634);
            DGV.TabIndex = 34;

            DGV.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DGV.Margin = new Padding(2, 2, 2, 2);

            ((System.ComponentModel.ISupportInitialize)(DGV)).EndInit();
        }

        public void Load_DGV_ServoGridView(ref DataGridView DGV, PortAxis ePortAxis)
        {
            MotionParamGridViewInit(ref DGV, $"{ePortAxis}\n(Servo Type)");

            List<ServoParamRow> ServoParamList = new List<ServoParamRow>();

            if(ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
            {
                foreach (ServoParamRow ServoRow in Enum.GetValues(typeof(ServoParamRow)))
                {
                    if (ServoRow != ServoParamRow.Servo_OverrideDistance &&
                            ServoRow != ServoParamRow.Servo_OverrideDecPercent &&
                            ServoRow != ServoParamRow.Servo_CrashCheck_ID)
                        ServoParamList.Add(ServoRow);
                }
            }
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
            {
                foreach (ServoParamRow ServoRow in Enum.GetValues(typeof(ServoParamRow)))
                {
                    if (ServoRow != ServoParamRow.Servo_TeachingPos2 &&
                        ServoRow != ServoParamRow.Servo_TeachingPos2_Check &&
                        ServoRow != ServoParamRow.Servo_TeachingPos3 &&
                        ServoRow != ServoParamRow.Servo_TeachingPos3_Check &&
                        ServoRow != ServoParamRow.Servo_WaitPosEnable &&
                        ServoRow != ServoParamRow.Servo_CrashCheck_ID)
                        ServoParamList.Add(ServoRow);
                }
            }
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
            {
                foreach (ServoParamRow ServoRow in Enum.GetValues(typeof(ServoParamRow)))
                {
                    if (ServoRow != ServoParamRow.Servo_TeachingPos2 &&
                        ServoRow != ServoParamRow.Servo_TeachingPos2_Check &&
                        ServoRow != ServoParamRow.Servo_TeachingPos3 &&
                        ServoRow != ServoParamRow.Servo_TeachingPos3_Check &&
                        ServoRow != ServoParamRow.Servo_WaitPosEnable && 
                        ServoRow != ServoParamRow.Servo_OverrideDistance && 
                        ServoRow != ServoParamRow.Servo_OverrideDecPercent)
                        ServoParamList.Add(ServoRow);
                }
            }

            if (ServoParamList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != ServoParamList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < ServoParamList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                        ServoParamRow eServoParamRow = ServoParamList[nListCount];

                        if (eServoParamRow == ServoParamRow.Servo_WaitPosEnable)
                        {
                            DataGridViewComboBoxCell cbxCell_WaitPosEnable = new DataGridViewComboBoxCell();
                            cbxCell_WaitPosEnable.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.WaitPosEnable)).Length; nCount++)
                                cbxCell_WaitPosEnable.Items.Add(((Port.WaitPosEnable)nCount).ToString());

                            DGV.Rows[nListCount].Cells[1] = cbxCell_WaitPosEnable;
                        }
                        else if (eServoParamRow == ServoParamRow.Servo_TeachingPos0_Check ||
                                    eServoParamRow == ServoParamRow.Servo_TeachingPos1_Check ||
                                    eServoParamRow == ServoParamRow.Servo_TeachingPos2_Check ||
                                    eServoParamRow == ServoParamRow.Servo_TeachingPos3_Check)
                        {
                            DataGridViewComboBoxCell cbxCell_TeachingPosCheck = new DataGridViewComboBoxCell();
                            cbxCell_TeachingPosCheck.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.PositionCheckType)).Length; nCount++)
                                cbxCell_TeachingPosCheck.Items.Add(((Port.PositionCheckType)nCount).ToString());

                            DGV.Rows[nListCount].Cells[1] = cbxCell_TeachingPosCheck;
                        }
                    }
                }

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    ServoParamRow eServoParamRow = ServoParamList[nRowCount];

                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];

                    switch (eServoParamRow)
                    {
                        case ServoParamRow.Servo_AxisNum:
                            {
                                int nAxis = GetMotionParam().GetServoAxisNum(ePortAxis);
                                DGV_NameCell.Value = "Axis Num";
                                DGV_Value.Value = nAxis == -1 ? string.Empty : $"{nAxis}";
                            }
                            break;
                        case ServoParamRow.Servo_WaitPosEnable:
                            {
                                bool bEnable = GetMotionParam().IsWaitPosEnable(ePortAxis);

                                DGV_NameCell.Value = "Wait Pos Enable";
                                DGV_Value.Value = bEnable ? Port.WaitPosEnable.Enable.ToString() : Port.WaitPosEnable.Disable.ToString();
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos0:
                            {
                                if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X) //|| ePortAxis == Port.PortAxis.Buffer1_X || ePortAxis == Port.PortAxis.Buffer2_X
                                    DGV_NameCell.Value = $"{Port.Teaching_X_Pos.OP_Pos} [mm]";
                                else if (ePortAxis == Port.PortAxis.Shuttle_Z || ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z) //|| ePortAxis == Port.PortAxis.Buffer1_Z || ePortAxis == Port.PortAxis.Buffer2_Z
                                    DGV_NameCell.Value = $"{Port.Teaching_Z_Pos.Up_Pos} [mm]";
                                else if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T) //|| ePortAxis == Port.PortAxis.Buffer1_T || ePortAxis == Port.PortAxis.Buffer2_T
                                    DGV_NameCell.Value = $"{Port.Teaching_T_Pos.Degree0_Pos} [deg]";

                                DGV_Value.Value = $"{GetMotionParam().GetTeachingPos(ePortAxis, 0).ToString("0.00")}";
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos0_Check:
                            {
                                if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X) //|| ePortAxis == Port.PortAxis.Buffer1_X || ePortAxis == Port.PortAxis.Buffer2_X
                                    DGV_NameCell.Value = $"{Port.Teaching_X_Pos.OP_Pos} Check Type";
                                else if (ePortAxis == Port.PortAxis.Shuttle_Z || ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z) //|| ePortAxis == Port.PortAxis.Buffer1_Z || ePortAxis == Port.PortAxis.Buffer2_Z
                                    DGV_NameCell.Value = $"{Port.Teaching_Z_Pos.Up_Pos} Check Type";
                                else if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T) //|| ePortAxis == Port.PortAxis.Buffer1_T || ePortAxis == Port.PortAxis.Buffer2_T
                                    DGV_NameCell.Value = $"{Port.Teaching_T_Pos.Degree0_Pos} Check Type";

                                DGV_Value.Value = $"{GetMotionParam().GetPositionCheckType(ePortAxis, 0).ToString()}";
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos1:
                            {
                                if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X) //|| ePortAxis == Port.PortAxis.Buffer1_X || ePortAxis == Port.PortAxis.Buffer2_X
                                    DGV_NameCell.Value = $"{Port.Teaching_X_Pos.MGV_LP_Pos} [mm]";
                                else if (ePortAxis == Port.PortAxis.Shuttle_Z || ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z) //|| ePortAxis == Port.PortAxis.Buffer1_Z || ePortAxis == Port.PortAxis.Buffer2_Z
                                    DGV_NameCell.Value = $"{Port.Teaching_Z_Pos.Down_Pos} [mm]";
                                else if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T) //|| ePortAxis == Port.PortAxis.Buffer1_T || ePortAxis == Port.PortAxis.Buffer2_T
                                    DGV_NameCell.Value = $"{Port.Teaching_T_Pos.Degree180_Pos} [deg]";

                                DGV_Value.Value =$"{GetMotionParam().GetTeachingPos(ePortAxis, 1).ToString("0.00")}";
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos1_Check:
                            {
                                if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X) //|| ePortAxis == Port.PortAxis.Buffer1_X || ePortAxis == Port.PortAxis.Buffer2_X
                                    DGV_NameCell.Value = $"{Port.Teaching_X_Pos.MGV_LP_Pos} Check Type";
                                else if (ePortAxis == Port.PortAxis.Shuttle_Z || ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z) //|| ePortAxis == Port.PortAxis.Buffer1_Z || ePortAxis == Port.PortAxis.Buffer2_Z
                                    DGV_NameCell.Value = $"{Port.Teaching_Z_Pos.Down_Pos} Check Type";
                                else if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T) //|| ePortAxis == Port.PortAxis.Buffer1_T || ePortAxis == Port.PortAxis.Buffer2_T
                                    DGV_NameCell.Value = $"{Port.Teaching_T_Pos.Degree180_Pos} Check Type";

                                DGV_Value.Value = $"{GetMotionParam().GetPositionCheckType(ePortAxis, 1).ToString()}";
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos2:
                            DGV_NameCell.Value = $"{Port.Teaching_X_Pos.Wait_Pos} [mm]";
                            DGV_Value.Value = $"{GetMotionParam().GetTeachingPos(ePortAxis, 2).ToString("0.00")}";
                            break;
                        case ServoParamRow.Servo_TeachingPos2_Check:
                            {
                                DGV_NameCell.Value = $"{Port.Teaching_X_Pos.Wait_Pos} Check Type";
                                DGV_Value.Value = $"{GetMotionParam().GetPositionCheckType(ePortAxis, 2).ToString()}";
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos3:
                            {
                                if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X) //|| ePortAxis == Port.PortAxis.Buffer1_X || ePortAxis == Port.PortAxis.Buffer2_X
                                    DGV_NameCell.Value = $"{Port.Teaching_X_Pos.Equip_LP_Pos} [mm]";

                                DGV_Value.Value = $"{GetMotionParam().GetTeachingPos(ePortAxis, 3).ToString("0.00")}";
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos3_Check:
                            {
                                if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X) //|| ePortAxis == Port.PortAxis.Buffer1_X || ePortAxis == Port.PortAxis.Buffer2_X
                                    DGV_NameCell.Value = $"{Port.Teaching_X_Pos.Equip_LP_Pos} Check Type";

                                DGV_Value.Value = $"{GetMotionParam().GetPositionCheckType(ePortAxis, 3).ToString()}";
                            }
                            break;
                        case ServoParamRow.Servo_OverrideDistance:
                            DGV_NameCell.Value = $"Override Distance [mm]";
                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDistance}";
                            break;
                        case ServoParamRow.Servo_OverrideDecPercent:
                            DGV_NameCell.Value = $"Override Dec Percent [%]";
                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).OverrideDecPercent}";
                            break;
                        case ServoParamRow.Servo_Manual_Speed:
                            if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Shuttle_Z ||
                                ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X ||
                                ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z)
                                DGV_NameCell.Value = $"Manual Speed [m/min]";
                            else
                                DGV_NameCell.Value = $"Manual Speed [°/min]";

                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Speed}";
                            break;
                        case ServoParamRow.Servo_Manual_Acc:
                            if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Shuttle_Z ||
                                ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X ||
                                ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z)
                                DGV_NameCell.Value = $"Manual Acc [m/min^2]";
                            else
                                DGV_NameCell.Value = $"Manual Acc [°/min^2]";

                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Acc}";
                            break;
                        case ServoParamRow.Servo_Manual_Dec:
                            if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Shuttle_Z ||
                                ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X ||
                                ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z)
                                DGV_NameCell.Value = $"Manual Dec [m/min^2]";
                            else
                                DGV_NameCell.Value = $"Manual Dec [°/min^2]";

                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec}";
                            break;
                        case ServoParamRow.Servo_AutoRun_Speed:
                            if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Shuttle_Z ||
                                ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X ||
                                ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z)
                                DGV_NameCell.Value = $"Auto Run Speed [m/min]";
                            else
                                DGV_NameCell.Value = $"Auto Run Speed [°/min]";

                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Speed}";
                            break;
                        case ServoParamRow.Servo_AutoRun_Acc:
                            if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Shuttle_Z ||
                                ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X ||
                                ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z)
                                DGV_NameCell.Value = $"Auto Run Acc [m/min^2]";
                            else
                                DGV_NameCell.Value = $"Auto Run Acc [°/min^2]";

                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Acc}";
                            break;
                        case ServoParamRow.Servo_AutoRun_Dec:
                            if (ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Shuttle_Z ||
                                ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X ||
                                ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z)
                                DGV_NameCell.Value = $"Auto Run Dec [m/min^2]";
                            else
                                DGV_NameCell.Value = $"Auto Run Dec [°/min^2]";

                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec}";
                            break;
                        case ServoParamRow.Servo_MaxLoad:
                            DGV_NameCell.Value = $"Max Load [%]";
                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).MaxLoad}";
                            break;
                        case ServoParamRow.Servo_CrashCheck_ID:
                            DGV_NameCell.Value = $"Crash Check ID";
                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).CrashCheckID}";
                            break;
                        case ServoParamRow.Servo_ManualPath:
                            DGV_NameCell.Value = $"Manual file [Path]";
                            DGV_Value.Value = $"{GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis).ManualPath}";
                            break;
                    }
                }

            }
            DGV.CurrentCell = null;
        }
        public bool Apply_ServoGridView(ref DataGridView DGV, PortAxis ePortAxis)
        {
            try
            {
                List<ServoParamRow> ServoParamList = new List<ServoParamRow>();

                if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                {
                    foreach (ServoParamRow ServoRow in Enum.GetValues(typeof(ServoParamRow)))
                    {
                        if (ServoRow != ServoParamRow.Servo_OverrideDistance &&
                                ServoRow != ServoParamRow.Servo_OverrideDecPercent &&
                                ServoRow != ServoParamRow.Servo_CrashCheck_ID)
                            ServoParamList.Add(ServoRow);
                    }
                }
                else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                {
                    foreach (ServoParamRow ServoRow in Enum.GetValues(typeof(ServoParamRow)))
                    {
                        if (ServoRow != ServoParamRow.Servo_TeachingPos2 &&
                            ServoRow != ServoParamRow.Servo_TeachingPos2_Check &&
                            ServoRow != ServoParamRow.Servo_TeachingPos3 &&
                            ServoRow != ServoParamRow.Servo_TeachingPos3_Check &&
                            ServoRow != ServoParamRow.Servo_WaitPosEnable &&
                            ServoRow != ServoParamRow.Servo_CrashCheck_ID)
                            ServoParamList.Add(ServoRow);
                    }
                }
                else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                {
                    foreach (ServoParamRow ServoRow in Enum.GetValues(typeof(ServoParamRow)))
                    {
                        if (ServoRow != ServoParamRow.Servo_TeachingPos2 &&
                            ServoRow != ServoParamRow.Servo_TeachingPos2_Check &&
                            ServoRow != ServoParamRow.Servo_TeachingPos3 &&
                            ServoRow != ServoParamRow.Servo_TeachingPos3_Check &&
                            ServoRow != ServoParamRow.Servo_WaitPosEnable &&
                            ServoRow != ServoParamRow.Servo_OverrideDistance &&
                            ServoRow != ServoParamRow.Servo_OverrideDecPercent)
                            ServoParamList.Add(ServoRow);
                    }
                }

                EquipPortMotionParam.ServoParam SetServoParam = new EquipPortMotionParam.ServoParam();
                SetServoParam.WMXParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].servoParam.WMXParam;

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    ServoParamRow eServoParamRow = ServoParamList[nRowCount];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];
                    DGV.CurrentCell = DGV_Value;

                    switch (eServoParamRow)
                    {
                        case ServoParamRow.Servo_AxisNum:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidAxisNum((string)DGV_Value.Value))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.AxisNum = Convert.ToInt32(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_WaitPosEnable:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                {
                                    SetServoParam.WaitPosEnable = (Port.WaitPosEnable)Enum.Parse(typeof(Port.WaitPosEnable), Convert.ToString(DGV_Value.Value));
                                }
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos0:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.TeachingPos[0] = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos0_Check:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.PositionCheckType[0] = (Port.PositionCheckType)Enum.Parse(typeof(Port.PositionCheckType), Convert.ToString(DGV_Value.Value));
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos1:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.TeachingPos[1] = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos1_Check:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.PositionCheckType[1] = (Port.PositionCheckType)Enum.Parse(typeof(Port.PositionCheckType), Convert.ToString(DGV_Value.Value));
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos2:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.TeachingPos[2] = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos2_Check:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.PositionCheckType[2] = (Port.PositionCheckType)Enum.Parse(typeof(Port.PositionCheckType), Convert.ToString(DGV_Value.Value));
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos3:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidTeachingPos((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.TeachingPos[3] = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_TeachingPos3_Check:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.PositionCheckType[3] = (Port.PositionCheckType)Enum.Parse(typeof(Port.PositionCheckType), Convert.ToString(DGV_Value.Value));
                            }
                            break;
                        case ServoParamRow.Servo_OverrideDistance:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.OverrideDistance = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_OverrideDecPercent:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.OverrideDecPercent = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_Manual_Speed:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidSpeed((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.Manual_Speed = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_Manual_Acc:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidAcc((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.Manual_Acc = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_Manual_Dec:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidDec((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.Manual_Dec = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_AutoRun_Speed:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidSpeed((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.AutoRun_Speed = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_AutoRun_Acc:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidAcc((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.AutoRun_Acc = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_AutoRun_Dec:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidDec((string)DGV_Value.Value, ePortAxis))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.AutoRun_Dec = Convert.ToSingle(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_MaxLoad:
                            {
                                if (!EquipPortMotionParam.ServoParam.IsValidMaxLoad((string)DGV_Value.Value))
                                    return false;

                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.MaxLoad = Convert.ToInt16(DGV_Value.Value);
                            }
                            break;
                        case ServoParamRow.Servo_CrashCheck_ID:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.CrashCheckID = Convert.ToString(DGV_Value.Value);
                                else
                                    SetServoParam.CrashCheckID = string.Empty;
                            }
                            break;
                        case ServoParamRow.Servo_ManualPath:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetServoParam.ManualPath = Convert.ToString(DGV_Value.Value);
                            }
                            break;
                    }
                }

                DGV.CurrentCell = null;
                GetMotionParam().SetShuttleCtrl_ServoParam(ePortAxis, SetServoParam);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Load_DGV_InverterGridView(ref DataGridView DGV, PortAxis ePortAxis)
        {
            IOParamGridViewInit(ref DGV, $"{ePortAxis}\n(Inverter Type)");

            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(InverterParamRow)).Length; nRowCount++)
            {
                InverterParamRow eInverterParamRow = (InverterParamRow)nRowCount;
                DGV.Rows.Add();


                if (eInverterParamRow == InverterParamRow.Inv_CtrlType)
                {
                    DataGridViewComboBoxCell cbxCell_InvCtrlType = new DataGridViewComboBoxCell();
                    cbxCell_InvCtrlType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.InvCtrlMode)).Length; nCount++)
                        cbxCell_InvCtrlType.Items.Add(((Port.InvCtrlMode)nCount).ToString());

                    DGV.Rows[nRowCount].Cells[1] = cbxCell_InvCtrlType;
                }

                DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];
                DataGridViewCell DGV_BitCell = DGV.Rows[nRowCount].Cells[2];

                var InverterParam = GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis);

                switch (eInverterParamRow)
                {
                    case InverterParamRow.Inv_CtrlType:
                        {
                            DGV_NameCell.Value = "Inv Control Type";
                            DGV_Value.Value = InverterParam.InvCtrlMode.ToString();
                            DGV_BitCell.Value = string.Empty;
                            DGV_BitCell.ReadOnly = true;
                            DGV_BitCell.Style.BackColor = Color.DarkGray;
                        }
                        break;
                    case InverterParamRow.Inv_HighSpeed:
                        {
                            int nStartAddr = InverterParam.GetIOParam(Port.InvIOCtrlFlag.HighSpeed).StartAddr;
                            int nBit = InverterParam.GetIOParam(Port.InvIOCtrlFlag.HighSpeed).Bit;
                            DGV_NameCell.Value = "High Spd Output Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                    case InverterParamRow.Inv_LowSpeed:
                        {
                            int nStartAddr = InverterParam.GetIOParam(Port.InvIOCtrlFlag.LowSpeed).StartAddr;
                            int nBit = InverterParam.GetIOParam(Port.InvIOCtrlFlag.LowSpeed).Bit;
                            DGV_NameCell.Value = "Low Spd Output Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                    case InverterParamRow.Inv_FWD:
                        {
                            int nStartAddr = InverterParam.GetIOParam(Port.InvIOCtrlFlag.FWD).StartAddr;
                            int nBit = InverterParam.GetIOParam(Port.InvIOCtrlFlag.FWD).Bit;
                            DGV_NameCell.Value = "FWD Output Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                    case InverterParamRow.Inv_BWD:
                        {
                            int nStartAddr = InverterParam.GetIOParam(Port.InvIOCtrlFlag.BWD).StartAddr;
                            int nBit = InverterParam.GetIOParam(Port.InvIOCtrlFlag.BWD).Bit;
                            DGV_NameCell.Value = "BWD Output Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                    case InverterParamRow.Inv_HzStartAddr:
                        {
                            int nStartAddr = InverterParam.HzStartAddr;
                            DGV_NameCell.Value = "Hz Ctrl Start Addr [byte]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = string.Empty;
                            DGV_BitCell.ReadOnly = true;
                            DGV_BitCell.Style.BackColor = Color.DarkGray;
                        }
                        break;
                    case InverterParamRow.Inv_HzTarget:
                        {
                            int nHzTarget = InverterParam.HzTarget;
                            DGV_NameCell.Value = "Target Hz [Freq]";
                            DGV_Value.Value = nHzTarget != -1 ? $"{nHzTarget}" : string.Empty;
                            DGV_BitCell.Value = string.Empty;
                            DGV_BitCell.ReadOnly = true;
                            DGV_BitCell.Style.BackColor = Color.DarkGray;
                        }
                        break;
                }
            }
            DGV.CurrentCell = null;
        }
        public bool Apply_InverterGridView(ref DataGridView DGV, PortAxis ePortAxis)
        {
            try
            {
                EquipPortMotionParam.InverterParam SetInvParam = new EquipPortMotionParam.InverterParam();

                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(InverterParamRow)).Length; nRowCount++)
                {
                    InverterParamRow eInverterParamRow = (InverterParamRow)nRowCount;
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_BitCell = DGV.Rows[nRowCount].Cells[2];
                    DGV.CurrentCell = DGV_Value;

                    //var InverterParam = GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis);

                    switch (eInverterParamRow)
                    {
                        case InverterParamRow.Inv_CtrlType:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                {
                                    SetInvParam.InvCtrlMode = (Port.InvCtrlMode)Enum.Parse(typeof(Port.InvCtrlMode), Convert.ToString(DGV_Value.Value));
                                }
                            }
                            break;
                        case InverterParamRow.Inv_HighSpeed:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.HighSpeed).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.HighSpeed).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case InverterParamRow.Inv_LowSpeed:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.LowSpeed).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.LowSpeed).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case InverterParamRow.Inv_FWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.FWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.FWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case InverterParamRow.Inv_BWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.BWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetInvParam.GetIOParam(Port.InvIOCtrlFlag.BWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case InverterParamRow.Inv_HzStartAddr:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetInvParam.HzStartAddr = Convert.ToInt32(DGV_Value.Value);
                            }
                            break;
                        case InverterParamRow.Inv_HzTarget:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetInvParam.HzTarget = Convert.ToInt16(DGV_Value.Value);
                            }
                            break;
                    }
                }

                DGV.CurrentCell = null;
                GetMotionParam().SetShuttleCtrl_InvParam(ePortAxis, SetInvParam);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Load_DGV_CylinderGridView(ref DataGridView DGV, PortAxis ePortAxis)
        {
            IOParamGridViewInit(ref DGV, $"{ePortAxis}\n(Cylinder Type)");

            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(CylinderParamRow)).Length; nRowCount++)
            {
                CylinderParamRow eCylinderParamRow = (CylinderParamRow)nRowCount;
                DGV.Rows.Add();

                DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];
                DataGridViewCell DGV_BitCell = DGV.Rows[nRowCount].Cells[2];

                switch (eCylinderParamRow)
                {
                    case CylinderParamRow.Cyl_FWD1_Ctrl:
                        {
                            int nStartAddr = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetCtrlIOParam(Port.CylCtrlList.FWD).StartAddr;
                            int nBit = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetCtrlIOParam(Port.CylCtrlList.FWD).Bit;
                            DGV_NameCell.Value = "FWD1 Ctrl Output Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                    case CylinderParamRow.Cyl_FWD1_PosSensor:
                        {
                            int nStartAddr = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetPosSensorIOParam(Port.CylCtrlList.FWD).StartAddr;
                            int nBit = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetPosSensorIOParam(Port.CylCtrlList.FWD).Bit;
                            DGV_NameCell.Value = "FWD1 Pos Sensor Input Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                    case CylinderParamRow.Cyl_BWD1_Ctrl:
                        {
                            int nStartAddr = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetCtrlIOParam(Port.CylCtrlList.BWD).StartAddr;
                            int nBit = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetCtrlIOParam(Port.CylCtrlList.BWD).Bit;
                            DGV_NameCell.Value = "BWD1 Ctrl Output Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                    case CylinderParamRow.Cyl_BWD1_PosSensor:
                        {
                            int nStartAddr = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetPosSensorIOParam(Port.CylCtrlList.BWD).StartAddr;
                            int nBit = GetMotionParam().GetShuttleCtrl_CylParam(ePortAxis).GetPosSensorIOParam(Port.CylCtrlList.BWD).Bit;
                            DGV_NameCell.Value = "BWD1 Pos Sensor Input Addr [byte, bit]";
                            DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                            DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                        }
                        break;
                }
            }

            DGV.CurrentCell = null;
        }
        public bool Apply_CylinderGridView(ref DataGridView DGV, PortAxis ePortAxis)
        {
            try
            {
                EquipPortMotionParam.CylinderParam SetCylParam = new EquipPortMotionParam.CylinderParam();

                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(CylinderParamRow)).Length; nRowCount++)
                {
                    CylinderParamRow eCylinderParamRow = (CylinderParamRow)nRowCount;
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_BitCell = DGV.Rows[nRowCount].Cells[2];
                    DGV.CurrentCell = DGV_Value;

                    switch (eCylinderParamRow)
                    {
                        case CylinderParamRow.Cyl_FWD1_Ctrl:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCylParam.GetCtrlIOParam(Port.CylCtrlList.FWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCylParam.GetCtrlIOParam(Port.CylCtrlList.FWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case CylinderParamRow.Cyl_FWD1_PosSensor:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCylParam.GetPosSensorIOParam(Port.CylCtrlList.FWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCylParam.GetPosSensorIOParam(Port.CylCtrlList.FWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case CylinderParamRow.Cyl_BWD1_Ctrl:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCylParam.GetCtrlIOParam(Port.CylCtrlList.BWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCylParam.GetCtrlIOParam(Port.CylCtrlList.BWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case CylinderParamRow.Cyl_BWD1_PosSensor:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCylParam.GetPosSensorIOParam(Port.CylCtrlList.BWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCylParam.GetPosSensorIOParam(Port.CylCtrlList.BWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                    }
                }

                DGV.CurrentCell = null;
                GetMotionParam().SetShuttleCtrl_CylParam(ePortAxis, SetCylParam); ;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Load_DGV_CVGridView(ref DataGridView DGV, BufferCV eBufferCV)
        {
            IOParamGridViewInit(ref DGV, $"{eBufferCV}\n(Inverter Type)");

            List<ConveyorParamRow> CVParamList = new List<ConveyorParamRow>();

            if (eBufferCV == Port.BufferCV.Buffer_LP || eBufferCV == Port.BufferCV.Buffer_OP)
            {
                foreach (ConveyorParamRow CVRow in Enum.GetValues(typeof(ConveyorParamRow)))
                {
                    if (CVRow <= ConveyorParamRow.CV_HzTarget)
                        CVParamList.Add(CVRow);
                }
            }
            else
            {
                foreach (ConveyorParamRow CVRow in Enum.GetValues(typeof(ConveyorParamRow)))
                {
                    if (CVRow >= ConveyorParamRow.CV_CtrlType)
                        CVParamList.Add(CVRow);
                }
            }

            if (CVParamList.Count == 0)
            {
                if (DGV.Rows.Count > 0)
                    DGV.Rows.Clear();

                if (DGV.Visible)
                    DGV.Visible = false;

                DGV.Height = 0;
            }
            else
            {
                if (!DGV.Visible)
                    DGV.Visible = true;

                if (DGV.Rows.Count != CVParamList.Count)
                {
                    DGV.Rows.Clear();
                    for (int nListCount = 0; nListCount < CVParamList.Count; nListCount++)
                    {
                        DGV.Rows.Add();
                        ConveyorParamRow eConveyorParamRow = CVParamList[nListCount];

                        if (eConveyorParamRow == ConveyorParamRow.CV_SlowSensor_Enable)
                        {
                            DataGridViewComboBoxCell cbx_Cell = new DataGridViewComboBoxCell();
                            cbx_Cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.CVSlowSensorEnable)).Length; nCount++)
                                cbx_Cell.Items.Add(((Port.CVSlowSensorEnable)nCount).ToString());

                            DGV.Rows[nListCount].Cells[1] = cbx_Cell;
                        }
                        if (eConveyorParamRow == ConveyorParamRow.CV_Stopper_Enable)
                        {
                            DataGridViewComboBoxCell cbx_Cell = new DataGridViewComboBoxCell();
                            cbx_Cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.CVStopperEnable)).Length; nCount++)
                                cbx_Cell.Items.Add(((Port.CVStopperEnable)nCount).ToString());

                            DGV.Rows[nListCount].Cells[1] = cbx_Cell;
                        }
                        if (eConveyorParamRow == ConveyorParamRow.CV_Centering_Enable)
                        {
                            DataGridViewComboBoxCell cbx_Cell = new DataGridViewComboBoxCell();
                            cbx_Cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.CVCenteringEnable)).Length; nCount++)
                                cbx_Cell.Items.Add(((Port.CVCenteringEnable)nCount).ToString());

                            DGV.Rows[nListCount].Cells[1] = cbx_Cell;
                        }
                        if (eConveyorParamRow == ConveyorParamRow.CV_CST_Detect_Enable)
                        {
                            DataGridViewComboBoxCell cbx_Cell = new DataGridViewComboBoxCell();
                            cbx_Cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.CVCenteringEnable)).Length; nCount++)
                                cbx_Cell.Items.Add(((Port.CVCenteringEnable)nCount).ToString());

                            DGV.Rows[nListCount].Cells[1] = cbx_Cell;
                        }
                        if(eConveyorParamRow == ConveyorParamRow.CV_CtrlType)
                        {
                            DataGridViewComboBoxCell cbx_Cell = new DataGridViewComboBoxCell();
                            cbx_Cell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.InvCtrlMode)).Length; nCount++)
                                cbx_Cell.Items.Add(((Port.InvCtrlMode)nCount).ToString());

                            DGV.Rows[nListCount].Cells[1] = cbx_Cell;
                        }
                    }

                    //DataGridViewFunc.AutoRowSize(DGV, 40, 16, 30);
                    //DataGridViewFunc.SetSize(DGV, 35, 20, 2);
                }

                //DataGridViewFunc.AutoRowSize(DGV, 40, 16, 30);

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    ConveyorParamRow eConveyorParamRow = CVParamList[nRowCount];
                    DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_BitCell = DGV.Rows[nRowCount].Cells[2];

                    switch (eConveyorParamRow)
                    {
                        case ConveyorParamRow.CV_SlowSensor_Enable:
                            {
                                bool bEnable = GetMotionParam().IsSlowSensorEnable(eBufferCV);

                                DGV_NameCell.Value = "Slow Sensor";
                                DGV_Value.Value = bEnable ? Port.CVSlowSensorEnable.Enable.ToString() : Port.CVSlowSensorEnable.Disable.ToString();
                                DGV_BitCell.ReadOnly = true;
                                DGV_BitCell.Style.BackColor = Color.LightGray;
                            }
                            break;
                        case ConveyorParamRow.CV_Stopper_Enable:
                            {
                                bool bEnable = GetMotionParam().IsStopperEnable(eBufferCV);

                                DGV_NameCell.Value = "Stopper Ctrl";
                                DGV_Value.Value = bEnable ? Port.CVStopperEnable.Enable.ToString() : Port.CVStopperEnable.Disable.ToString();
                                DGV_BitCell.ReadOnly = true;
                                DGV_BitCell.Style.BackColor = Color.LightGray;
                            }
                            break;
                        case ConveyorParamRow.CV_Stopper_FWD:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_StopperParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.FWD).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_StopperParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.FWD).Bit;
                                DGV_NameCell.Value = "Stopper FWD Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_Stopper_BWD:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_StopperParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.BWD).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_StopperParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.BWD).Bit;
                                DGV_NameCell.Value = "Stopper BWD Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_Centering_Enable:
                            {
                                bool bEnable = GetMotionParam().IsCenteringEnable(eBufferCV);

                                DGV_NameCell.Value = "Centering Ctrl";
                                DGV_Value.Value = bEnable ? Port.CVCenteringEnable.Enable.ToString() : Port.CVCenteringEnable.Disable.ToString();
                                DGV_BitCell.ReadOnly = true;
                                DGV_BitCell.Style.BackColor = Color.LightGray;
                            }
                            break;
                        case ConveyorParamRow.CV_Centering_FWD:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CenteringParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.FWD).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_CenteringParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.FWD).Bit;
                                DGV_NameCell.Value = "Centering FWD Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_Centering_BWD:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CenteringParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.BWD).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_CenteringParam(eBufferCV).GetCtrlIOParam(Port.CylCtrlList.BWD).Bit;
                                DGV_NameCell.Value = "Centering BWD Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_CtrlType:
                            {
                                DGV_NameCell.Value = "CV Control Type";
                                DGV_Value.Value = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).InvCtrlMode.ToString();
                                DGV_BitCell.Value = string.Empty;
                                DGV_BitCell.ReadOnly = true;
                                DGV_BitCell.Style.BackColor = Color.DarkGray;
                            }
                            break;
                        case ConveyorParamRow.CV_HighSpeed:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.HighSpeed).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.HighSpeed).Bit;
                                DGV_NameCell.Value = "CV High Spd Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_LowSpeed:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.LowSpeed).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.LowSpeed).Bit;
                                DGV_NameCell.Value = "CV Low Spd Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_FWD:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.FWD).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.FWD).Bit;
                                DGV_NameCell.Value = "CV FWD Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_BWD:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.BWD).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).GetIOParam(Port.InvIOCtrlFlag.BWD).Bit;
                                DGV_NameCell.Value = "CV BWD Output Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                        case ConveyorParamRow.CV_HzStartAddr:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).HzStartAddr;
                                DGV_NameCell.Value = "Hz Ctrl Start Addr [byte]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = string.Empty;
                                DGV_BitCell.ReadOnly = true;
                                DGV_BitCell.Style.BackColor = Color.DarkGray;
                            }
                            break;
                        case ConveyorParamRow.CV_HzTarget:
                            {
                                int nHzTarget = GetMotionParam().GetBufferCtrl_CVParam(eBufferCV).HzTarget;
                                DGV_NameCell.Value = "Target Hz [Freq]";
                                DGV_Value.Value = nHzTarget != -1 ? $"{nHzTarget}" : string.Empty;
                                DGV_BitCell.Value = string.Empty;
                                DGV_BitCell.ReadOnly = true;
                                DGV_BitCell.Style.BackColor = Color.DarkGray;
                            }
                            break;
                        case ConveyorParamRow.CV_CST_Detect_Enable:
                            {
                                bool bEnable = GetMotionParam().IsCSTDetectSensorEnable(eBufferCV);

                                DGV_NameCell.Value = "CST Detect Sensor";
                                DGV_Value.Value = bEnable ? Port.CVCSTDetectSensorEnable.Enable.ToString() : Port.CVCSTDetectSensorEnable.Disable.ToString();
                                DGV_BitCell.ReadOnly = true;
                                DGV_BitCell.Style.BackColor = Color.LightGray;
                            }
                            break;
                        case ConveyorParamRow.CV_CST_Detect:
                            {
                                int nStartAddr = GetMotionParam().GetBufferCtrl_CSTDetectParam(eBufferCV).StartAddr;
                                int nBit = GetMotionParam().GetBufferCtrl_CSTDetectParam(eBufferCV).Bit;
                                DGV_NameCell.Value = "CST Detect Input Map [byte, bit]";
                                DGV_Value.Value = nStartAddr != -1 ? $"{nStartAddr}" : string.Empty;
                                DGV_BitCell.Value = nBit != -1 ? $"{nBit}" : string.Empty;
                            }
                            break;
                    }
                }
            }

            DGV.CurrentCell = null;
        }
        public bool Apply_CVGridView(ref DataGridView DGV, BufferCV eBufferCV)
        {
            try
            {
                List<ConveyorParamRow> CVParamList = new List<ConveyorParamRow>();

                if (eBufferCV == Port.BufferCV.Buffer_LP || eBufferCV == Port.BufferCV.Buffer_OP)
                {
                    foreach (ConveyorParamRow CVRow in Enum.GetValues(typeof(ConveyorParamRow)))
                    {
                        if (CVRow <= ConveyorParamRow.CV_HzTarget)
                            CVParamList.Add(CVRow);
                    }
                }
                else
                {
                    foreach (ConveyorParamRow CVRow in Enum.GetValues(typeof(ConveyorParamRow)))
                    {
                        if (CVRow >= ConveyorParamRow.CV_HighSpeed)
                            CVParamList.Add(CVRow);
                    }
                }

                EquipPortMotionParam.BufferCVParam SetCVParam = new EquipPortMotionParam.BufferCVParam();
                SetCVParam.eCVCtrlEnable = GetMotionParam().GetBufferCVParam(eBufferCV).eCVCtrlEnable;

                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    ConveyorParamRow eConveyorParamRow = CVParamList[nRowCount];
                    DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_BitCell = DGV.Rows[nRowCount].Cells[2];

                    DGV.CurrentCell = DGV_Value;

                    switch (eConveyorParamRow)
                    {
                        case ConveyorParamRow.CV_SlowSensor_Enable:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                {
                                    SetCVParam.eCVSlowSensorEnable = (Port.CVSlowSensorEnable)Enum.Parse(typeof(Port.CVSlowSensorEnable), Convert.ToString(DGV_Value.Value));
                                }
                                else
                                    SetCVParam.eCVSlowSensorEnable = Port.CVSlowSensorEnable.Disable;
                            }
                            break;
                        case ConveyorParamRow.CV_Stopper_Enable:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                {
                                    SetCVParam.eCVStopperEnable = (Port.CVStopperEnable)Enum.Parse(typeof(Port.CVStopperEnable), Convert.ToString(DGV_Value.Value));
                                }
                                else
                                    SetCVParam.eCVStopperEnable = Port.CVStopperEnable.Disable;
                            }
                            break;
                        case ConveyorParamRow.CV_Stopper_FWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.StopperParam.GetCtrlIOParam(Port.CylCtrlList.FWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.StopperParam.GetCtrlIOParam(Port.CylCtrlList.FWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_Stopper_BWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.StopperParam.GetCtrlIOParam(Port.CylCtrlList.BWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.StopperParam.GetCtrlIOParam(Port.CylCtrlList.BWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_Centering_Enable:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                {
                                    SetCVParam.eCVCenteringEnable = (Port.CVCenteringEnable)Enum.Parse(typeof(Port.CVCenteringEnable), Convert.ToString(DGV_Value.Value));
                                }
                                else
                                    SetCVParam.eCVCenteringEnable = Port.CVCenteringEnable.Disable;
                            }
                            break;
                        case ConveyorParamRow.CV_Centering_FWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CenteringParam.GetCtrlIOParam(Port.CylCtrlList.FWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.CenteringParam.GetCtrlIOParam(Port.CylCtrlList.FWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_Centering_BWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CenteringParam.GetCtrlIOParam(Port.CylCtrlList.BWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.CenteringParam.GetCtrlIOParam(Port.CylCtrlList.BWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_CtrlType:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                {
                                    SetCVParam.CVParam.InvCtrlMode = (Port.InvCtrlMode)Enum.Parse(typeof(Port.InvCtrlMode), Convert.ToString(DGV_Value.Value));
                                }
                                else
                                    SetCVParam.CVParam.InvCtrlMode = Port.InvCtrlMode.IOControl;
                            }
                            break;
                        case ConveyorParamRow.CV_HighSpeed:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.HighSpeed).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.HighSpeed).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_LowSpeed:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.LowSpeed).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.LowSpeed).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_FWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.FWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.FWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_BWD:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.BWD).StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.CVParam.GetIOParam(Port.InvIOCtrlFlag.BWD).Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_HzStartAddr:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CVParam.HzStartAddr = Convert.ToInt32(DGV_Value.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_HzTarget:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CVParam.HzTarget = Convert.ToInt16(DGV_Value.Value);
                            }
                            break;
                        case ConveyorParamRow.CV_CST_Detect_Enable:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                {
                                    SetCVParam.eCSTDetectSensorEnable = (Port.CVCSTDetectSensorEnable)Enum.Parse(typeof(Port.CVCSTDetectSensorEnable), Convert.ToString(DGV_Value.Value));
                                }
                                else
                                    SetCVParam.eCSTDetectSensorEnable = Port.CVCSTDetectSensorEnable.Disable;
                            }
                            break;
                        case ConveyorParamRow.CV_CST_Detect:
                            {
                                if ((string)DGV_Value.Value != string.Empty)
                                    SetCVParam.CST_DetectParam.StartAddr = Convert.ToInt32(DGV_Value.Value);

                                if ((string)DGV_BitCell.Value != string.Empty)
                                    SetCVParam.CST_DetectParam.Bit = Convert.ToInt32(DGV_BitCell.Value);
                            }
                            break;
                    }
                }

                DGV.CurrentCell = null;
                GetMotionParam().SetBufferCVParam(eBufferCV, SetCVParam);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public int GetUIManualSpeedRatio()
        {
            return GetUIParam()?.PortManualSpeedRatio ?? 50;
        }
        public void SetUIManualSpeedRatio(int value)
        {
            GetUIParam().PortManualSpeedRatio = value;
            GetUIParam().Save(GetParam().ID, GetUIParam());
        }
        public string GetUIJogLowSpeed(PortAxis ePortAxis)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                return (GetUIParam()?.X_Axis_JogLowSpeed ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                return (GetUIParam()?.Z_Axis_JogLowSpeed ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                return (GetUIParam()?.T_Axis_JogLowSpeed ?? 0.0f).ToString();

            return string.Empty;
        }
        public string GetUIJogHighSpeed(PortAxis ePortAxis)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                return (GetUIParam()?.X_Axis_JogHighSpeed ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                return (GetUIParam()?.Z_Axis_JogHighSpeed ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                return (GetUIParam()?.T_Axis_JogHighSpeed ?? 0.0f).ToString();

            return string.Empty;
        }
        public string GetUIInchingValue(PortAxis ePortAxis)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                return (GetUIParam()?.X_Axis_Inching ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                return (GetUIParam()?.Z_Axis_Inching ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                return (GetUIParam()?.T_Axis_Inching ?? 0.0f).ToString();

            return string.Empty;
        }
        public string GetUITargetValue(PortAxis ePortAxis)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                return (GetUIParam()?.X_Axis_Target ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                return (GetUIParam()?.Z_Axis_Target ?? 0.0f).ToString();
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                return (GetUIParam()?.T_Axis_Target ?? 0.0f).ToString();

            return string.Empty;
        }
        public void SetUIJogLowSpeed(PortAxis ePortAxis, float value)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                GetUIParam().X_Axis_JogLowSpeed = value;
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                GetUIParam().Z_Axis_JogLowSpeed = value;
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                GetUIParam().T_Axis_JogLowSpeed = value;

            GetUIParam().Save(GetParam().ID, GetUIParam());
        }
        public void SetUIJogHighSpeed(PortAxis ePortAxis, float value)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                GetUIParam().X_Axis_JogHighSpeed = value;
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                GetUIParam().Z_Axis_JogHighSpeed = value;
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                GetUIParam().T_Axis_JogHighSpeed = value;

            GetUIParam().Save(GetParam().ID, GetUIParam());
        }
        public void SetUIInchingValue(PortAxis ePortAxis, float value)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                GetUIParam().X_Axis_Inching = value;
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                GetUIParam().Z_Axis_Inching = value;
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                GetUIParam().T_Axis_Inching = value;

            GetUIParam().Save(GetParam().ID, GetUIParam());
        }
        public void SetUITargetValue(PortAxis ePortAxis, float value)
        {
            if (ePortAxis == PortAxis.Shuttle_X || ePortAxis == PortAxis.Buffer_LP_X || ePortAxis == PortAxis.Buffer_OP_X)
                GetUIParam().X_Axis_Target = value;
            else if (ePortAxis == PortAxis.Shuttle_Z || ePortAxis == PortAxis.Buffer_LP_Z || ePortAxis == PortAxis.Buffer_OP_Z)
                GetUIParam().Z_Axis_Target = value;
            else if (ePortAxis == PortAxis.Shuttle_T || ePortAxis == PortAxis.Buffer_LP_T || ePortAxis == PortAxis.Buffer_OP_T)
                GetUIParam().T_Axis_Target = value;

            GetUIParam().Save(GetParam().ID, GetUIParam());
        }
    }
}
