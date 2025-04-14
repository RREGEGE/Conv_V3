using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using RackMaster.SEQ.PART;

namespace RackMaster.SUBFORM {
    public partial class FrmMain : Form {
        public enum Dgv_State_Column {
            Name,
            State
        }

        public enum Dgv_Main_AutoCondition_Row {
            Error,
            EtherCAT,
            TCP,
            AllServoOn,
            AllHoming,
            HPDoor,
            OPDoor,
            KeyAuto,
            LoadParameterSuccess,
            ParameterValueCorrect,
            GOT_On,
            Z_Axis_MaintStopper,
            EnableSensor,
            MST_EMO_Request,
            MST_SoftError,
        }

        public enum Dgv_Main_RMStatus_Row {
            ServoOnReady,
            ServoOn,
            ServoOffReady,
            ServoOff,
            AutoCondition,
            AutoState,
            ManualCondition,
            ManualState,
            ErrorState,
            ArmHomeState,
            CSTOn,
            TurnLeft,
            TurnRight,
        }

        public enum Dgv_Main_ServoStatus_Column {
            AxisName,
            ServoOn,
            Position,
            Velocity,
            Torque,
            HomeState,
            HomeSensor,
            NLimit,
            PLimit,
            Alarm,
            Temperature,
        }

        public enum Dgv_Main_ServoStatus_Row {
            X_Axis,
            Z_Axis,
            A_Axis,
            T_Axis 
        }

        public enum Dgv_Main_PIOMaster_Row {
            LoadRequest,
            UnloadRequest,
            Ready,
            PortError
        }

        public enum Dgv_Main_PIORackMaster_Row {
            TRRequest,
            Busy,
            Complete,
            STKError
        }

        public enum Dgv_Main_Motion_Row {
            FromReady,
            FromRun,
            FromComplete,
            ToReady,
            ToRun,
            ToComplete,
            MaintRun,
            MaintComplete,
            Step,
            FromID,
            ToID,
        }

        public enum Dgv_Main_IOMaster_Row {
            HPDoor,
            OPDoor,
            HPEMO,
            OPEMO,
            HPEscape,
            OPEscape,
            KeyAuto
        }

        public enum Dgv_Main_IORackMaster_Row {
            PresenceSensor_1,
            PresenceSensor_2,
            DoubleStorageSensor_1,
            DoubleStorageSensor_2,
            PickSensor_Left,
            PlaceSensor_Left,
            PickSensor_Right,
            PlaceSensor_Right,
            PlacementSensor_1,
            PlacementSensor_2,
            StickSensor_1,
            StickSensor_2,
            StickSensor_3,
            StickSensor_4,
        }

        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterMotion m_motion;
        private RackMasterMain.RackMasterAlarm m_alarm;

        public FrmMain(RackMasterMain rackMaster) {
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_rackMaster = rackMaster;
            m_motion = m_rackMaster.m_motion;
            m_alarm = m_rackMaster.m_alarm;

            InitializeComponent();

            InitDataGridView_ServoStatus();
            InitDataGridView_AutoCondition();
            InitDataGridView_RMStatus();
            InitDataGirdView_MotionStatus();
            InitDataGridView_AlarmList();
            InitDataGridView_MasterIO();
            InitDataGridView_MasterPIO();
            InitDataGridView_RackMasterIO();
            InitDataGridView_RackMasterPIO();

            LanguageChanged();
        }

        private void InitDataGridView_AutoCondition() {
            foreach(Dgv_Main_AutoCondition_Row item in Enum.GetValues(typeof(Dgv_Main_AutoCondition_Row))) {
                m_dgvCtrl.AddRow(ref dgvRMAutoCondition, GetDgvAutoCondtionHeaderText(item), (int)Dgv_State_Column.Name);
            }
            m_dgvCtrl.SetReadonly(ref dgvRMAutoCondition);
            m_dgvCtrl.DisableUserControl(ref dgvRMAutoCondition);
            foreach (DataGridViewColumn column in dgvRMAutoCondition.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        
        private string GetDgvAutoCondtionHeaderText(Dgv_Main_AutoCondition_Row item) {
            switch (item) {
                case Dgv_Main_AutoCondition_Row.Error:
                    return "Error";

                case Dgv_Main_AutoCondition_Row.EtherCAT:
                    return "EtherCAT";

                case Dgv_Main_AutoCondition_Row.TCP:
                    return "TCP Connection";

                case Dgv_Main_AutoCondition_Row.AllServoOn:
                    return "All Servo On";

                case Dgv_Main_AutoCondition_Row.AllHoming:
                    return "All Homing";

                case Dgv_Main_AutoCondition_Row.HPDoor:
                    return "HP Door Open";

                case Dgv_Main_AutoCondition_Row.OPDoor:
                    return "OP Door Open";

                case Dgv_Main_AutoCondition_Row.KeyAuto:
                    return "Key Auto";

                case Dgv_Main_AutoCondition_Row.LoadParameterSuccess:
                    return "Load Parameter";

                case Dgv_Main_AutoCondition_Row.ParameterValueCorrect:
                    return "Parameter Value";

                case Dgv_Main_AutoCondition_Row.GOT_On:
                    return "GOT On";

                case Dgv_Main_AutoCondition_Row.Z_Axis_MaintStopper:
                    return "Z Axis Maint Stopper";

                case Dgv_Main_AutoCondition_Row.EnableSensor:
                    return "Sensor Enable";

                case Dgv_Main_AutoCondition_Row.MST_EMO_Request:
                    return "Master EMO";

                case Dgv_Main_AutoCondition_Row.MST_SoftError:
                    return "Master Error";
            }

            return "";
        }

        private void InitDataGridView_RMStatus() {
            foreach(Dgv_Main_RMStatus_Row item in Enum.GetValues(typeof(Dgv_Main_RMStatus_Row))) {
                m_dgvCtrl.AddRow(ref dgvRMStatus, GetDgvRMStatusHeaderText(item), (int)Dgv_State_Column.Name);
            }
            m_dgvCtrl.SetReadonly(ref dgvRMStatus);
            m_dgvCtrl.DisableUserControl(ref dgvRMStatus);
            foreach (DataGridViewColumn column in dgvRMStatus.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string GetDgvRMStatusHeaderText(Dgv_Main_RMStatus_Row item) {
            switch (item) {
                case Dgv_Main_RMStatus_Row.ArmHomeState:
                    return "Arm Home State";

                case Dgv_Main_RMStatus_Row.AutoCondition:
                    return "Auto Condition";

                case Dgv_Main_RMStatus_Row.AutoState:
                    return "Auto State";

                case Dgv_Main_RMStatus_Row.CSTOn:
                    return "Cassette On";

                case Dgv_Main_RMStatus_Row.ErrorState:
                    return "Error";

                case Dgv_Main_RMStatus_Row.ManualCondition:
                    return "Manual Condition";

                case Dgv_Main_RMStatus_Row.ManualState:
                    return "Manual State";

                case Dgv_Main_RMStatus_Row.ServoOff:
                    return "All Servo Off";

                case Dgv_Main_RMStatus_Row.ServoOffReady:
                    return "Servo Off Ready";

                case Dgv_Main_RMStatus_Row.ServoOn:
                    return "All Servo On";

                case Dgv_Main_RMStatus_Row.ServoOnReady:
                    return "Servo On Ready";

                case Dgv_Main_RMStatus_Row.TurnLeft:
                    return "Turn Left";

                case Dgv_Main_RMStatus_Row.TurnRight:
                    return "Turn Right";
            }

            return "";
        }

        private void InitDataGirdView_MotionStatus() {
            foreach(Dgv_Main_Motion_Row item in Enum.GetValues(typeof(Dgv_Main_Motion_Row))) {
                m_dgvCtrl.AddRow(ref dgvMotionStatus, GetDgvMotionHeaderText(item), (int)Dgv_State_Column.Name);
            }
            m_dgvCtrl.SetReadonly(ref dgvMotionStatus);
            m_dgvCtrl.DisableUserControl(ref dgvMotionStatus);
            foreach (DataGridViewColumn column in dgvMotionStatus.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string GetDgvMotionHeaderText(Dgv_Main_Motion_Row item) {
            switch (item) {
                case Dgv_Main_Motion_Row.FromComplete:
                    return "From Complete";

                case Dgv_Main_Motion_Row.FromReady:
                    return "From Ready";

                case Dgv_Main_Motion_Row.FromRun:
                    return "From Run";

                case Dgv_Main_Motion_Row.MaintComplete:
                    return "Maint Complete";

                case Dgv_Main_Motion_Row.MaintRun:
                    return "Maint Run";

                case Dgv_Main_Motion_Row.ToComplete:
                    return "To Complete";

                case Dgv_Main_Motion_Row.ToReady:
                    return "To Ready";

                case Dgv_Main_Motion_Row.ToRun:
                    return "To Run";

                case Dgv_Main_Motion_Row.Step:
                    return "Step";

                case Dgv_Main_Motion_Row.FromID:
                    return "From ID";

                case Dgv_Main_Motion_Row.ToID:
                    return "To ID";
            }

            return "";
        }

        private void InitDataGridView_AlarmList() {
            for(int i = 0; i <= (int)AlarmList.MAX; i++) {
                m_dgvCtrl.AddRow(ref dgvAlarmList, $"{i}", (int)Dgv_State_Column.Name);
                m_dgvCtrl.SetCellText(ref dgvAlarmList, i, (int)Dgv_State_Column.State, m_alarm.GetAlarmString((AlarmList)i));
            }
            m_dgvCtrl.SetReadonly(ref dgvAlarmList);
            m_dgvCtrl.DisableUserControl(ref dgvAlarmList);
            foreach (DataGridViewColumn column in dgvAlarmList.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitDataGridView_MasterIO() {
            foreach(Dgv_Main_IOMaster_Row item in Enum.GetValues(typeof(Dgv_Main_IOMaster_Row))) {
                m_dgvCtrl.AddRow(ref dgvMasterIO, GetDgvMasterIOHeaderText(item), (int)Dgv_State_Column.Name);
            }
            m_dgvCtrl.SetReadonly(ref dgvMasterIO);
            m_dgvCtrl.DisableUserControl(ref dgvMasterIO);
            foreach (DataGridViewColumn column in dgvMasterIO.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string GetDgvMasterIOHeaderText(Dgv_Main_IOMaster_Row item) {
            switch (item) {
                case Dgv_Main_IOMaster_Row.HPDoor:
                    return "HP Door";

                case Dgv_Main_IOMaster_Row.HPEMO:
                    return "HP EMO";

                case Dgv_Main_IOMaster_Row.HPEscape:
                    return "HP Escape";

                case Dgv_Main_IOMaster_Row.KeyAuto:
                    return "Key";

                case Dgv_Main_IOMaster_Row.OPDoor:
                    return "OP Door";

                case Dgv_Main_IOMaster_Row.OPEMO:
                    return "OP EMO";

                case Dgv_Main_IOMaster_Row.OPEscape:
                    return "OP Escape";
            }

            return "";
        }

        private void InitDataGridView_RackMasterIO() {
            foreach(Dgv_Main_IORackMaster_Row item in Enum.GetValues(typeof(Dgv_Main_IORackMaster_Row))) {
                m_dgvCtrl.AddRow(ref dgvRMIO, GetDgvRackMasterIOHeaderText(item), (int)Dgv_State_Column.Name);
            }
            m_dgvCtrl.SetReadonly(ref dgvRMIO);
            m_dgvCtrl.DisableUserControl(ref dgvRMIO);
            foreach (DataGridViewColumn column in dgvRMIO.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string GetDgvRackMasterIOHeaderText(Dgv_Main_IORackMaster_Row item) {
            switch (item) {
                case Dgv_Main_IORackMaster_Row.DoubleStorageSensor_1:
                    return "Double Storage Sensor 1";

                case Dgv_Main_IORackMaster_Row.DoubleStorageSensor_2:
                    return "Double Storage Sensor 2";

                case Dgv_Main_IORackMaster_Row.PickSensor_Left:
                    return "Pick Sensor Left";

                case Dgv_Main_IORackMaster_Row.PlacementSensor_1:
                    return "Placement Sensor 1";

                case Dgv_Main_IORackMaster_Row.PlacementSensor_2:
                    return "Placement Sensor 2";

                case Dgv_Main_IORackMaster_Row.PlaceSensor_Left:
                    return "Place Sensor Left";

                case Dgv_Main_IORackMaster_Row.PresenceSensor_1:
                    return "Presence Sensor 1";

                case Dgv_Main_IORackMaster_Row.PresenceSensor_2:
                    return "Presence Sensor 2";

                case Dgv_Main_IORackMaster_Row.StickSensor_1:
                    return "Stick Sensor 1";

                case Dgv_Main_IORackMaster_Row.StickSensor_2:
                    return "Stick Sensor 2";

                case Dgv_Main_IORackMaster_Row.StickSensor_3:
                    return "Stick Sensor 3";

                case Dgv_Main_IORackMaster_Row.StickSensor_4:
                    return "Stick Sensor 4";

                case Dgv_Main_IORackMaster_Row.PickSensor_Right:
                    return "Pick Sensor Right";

                case Dgv_Main_IORackMaster_Row.PlaceSensor_Right:
                    return "Place Sensor Right";
            }

            return "";
        }

        private void InitDataGridView_MasterPIO() {
            foreach(Dgv_Main_PIOMaster_Row item in Enum.GetValues(typeof(Dgv_Main_PIOMaster_Row))) {
                m_dgvCtrl.AddRow(ref dgvPIOMaster, GetDgvMasterPIOHeaderText(item), (int)Dgv_State_Column.Name);
            }
            m_dgvCtrl.SetReadonly(ref dgvPIOMaster);
            m_dgvCtrl.DisableUserControl(ref dgvPIOMaster);
            foreach (DataGridViewColumn column in dgvPIOMaster.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string GetDgvMasterPIOHeaderText(Dgv_Main_PIOMaster_Row item) {
            switch (item) {
                case Dgv_Main_PIOMaster_Row.LoadRequest:
                    return "L-REQ";

                case Dgv_Main_PIOMaster_Row.PortError:
                    return "Port Error";

                case Dgv_Main_PIOMaster_Row.Ready:
                    return "Ready";

                case Dgv_Main_PIOMaster_Row.UnloadRequest:
                    return "UL-REQ";
            }

            return "";
        }

        private void InitDataGridView_RackMasterPIO() {
            foreach(Dgv_Main_PIORackMaster_Row item in Enum.GetValues(typeof(Dgv_Main_PIORackMaster_Row))) {
                m_dgvCtrl.AddRow(ref dgvPIORM, GetDgvRackMasterPIOHeaderText(item), (int)Dgv_State_Column.Name);
            }
            m_dgvCtrl.SetReadonly(ref dgvPIORM);
            m_dgvCtrl.DisableUserControl(ref dgvPIORM);
            foreach (DataGridViewColumn column in dgvPIORM.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string GetDgvRackMasterPIOHeaderText(Dgv_Main_PIORackMaster_Row item) {
            switch (item) {
                case Dgv_Main_PIORackMaster_Row.Busy:
                    return "Busy";

                case Dgv_Main_PIORackMaster_Row.Complete:
                    return "Complete";

                case Dgv_Main_PIORackMaster_Row.STKError:
                    return "STK Error";

                case Dgv_Main_PIORackMaster_Row.TRRequest:
                    return "TR-REQ";
            }

            return "";
        }

        private void InitDataGridView_ServoStatus() {
            foreach(Dgv_Main_ServoStatus_Row item in Enum.GetValues(typeof(Dgv_Main_ServoStatus_Row))) {
                m_dgvCtrl.AddRow(ref dgvServoStatus, GetDgvServoStatusHeaderText(item), (int)Dgv_State_Column.Name);
            }
            colTemp.HeaderText = "Temperature";
            m_dgvCtrl.SetReadonly(ref dgvServoStatus);
            m_dgvCtrl.DisableUserControl(ref dgvServoStatus);
            foreach (DataGridViewColumn column in dgvServoStatus.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private string GetDgvServoStatusHeaderText(Dgv_Main_ServoStatus_Row item) {
            switch (item) {
                case Dgv_Main_ServoStatus_Row.X_Axis:
                    return "X-Axis";

                case Dgv_Main_ServoStatus_Row.Z_Axis:
                    return "Z-Axis";

                case Dgv_Main_ServoStatus_Row.A_Axis:
                    return "A-Axis";

                case Dgv_Main_ServoStatus_Row.T_Axis:
                    return "T-Axis";
            }

            return "";
        }

        public void UpdateFormUI() {
            dgvServoStatus.ClearSelection();
            dgvRMAutoCondition.ClearSelection();
            dgvRMStatus.ClearSelection();
            dgvMotionStatus.ClearSelection();
            dgvAlarmList.ClearSelection();
            dgvMasterIO.ClearSelection();
            dgvPIOMaster.ClearSelection();
            dgvRMIO.ClearSelection();
            dgvPIORM.ClearSelection();

            UpdateDataGridView_AutoCondition();
            UpdateDataGridView_AlarmList();
            UpdateDataGridView_MasterIO();
            UpdateDataGridView_MotionStatus();
            UpdateDataGridView_PIOMaster();
            UpdateDataGridView_PIORackMaster();
            UpdateDataGridView_RackMasterIO();
            UpdateDataGridView_RackMasterStatus();
            UpdateDataGridView_ServoStatus();
        }

        private void UpdateDataGridView_AutoCondition() {
            foreach(Dgv_Main_AutoCondition_Row item in Enum.GetValues(typeof(Dgv_Main_AutoCondition_Row))) {
                int row = (int)item;
                int col = (int)Dgv_State_Column.State;
                switch (item) {
                    case Dgv_Main_AutoCondition_Row.AllHoming:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_motion.IsAllHomeDone());
                        break;

                    case Dgv_Main_AutoCondition_Row.AllServoOn:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_motion.IsAllServoOn());
                        break;

                    case Dgv_Main_AutoCondition_Row.Error:
                        m_dgvCtrl.SetErrorCell(ref dgvRMAutoCondition, row, col, m_alarm.IsAlarmState());
                        break;

                    case Dgv_Main_AutoCondition_Row.EtherCAT:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_rackMaster.IsConnected_EtherCAT());
                        break;

                    case Dgv_Main_AutoCondition_Row.HPDoor:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_HP_Door_Open));
                        break;

                    case Dgv_Main_AutoCondition_Row.KeyAuto:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_Key_Auto));
                        break;

                    case Dgv_Main_AutoCondition_Row.OPDoor:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_OP_Door_Open));
                        break;

                    case Dgv_Main_AutoCondition_Row.TCP:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_rackMaster.IsConnected_Master());
                        break;

                    case Dgv_Main_AutoCondition_Row.LoadParameterSuccess:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_rackMaster.Interlock_IsSettingSuccessParamter());
                        break;

                    case Dgv_Main_AutoCondition_Row.ParameterValueCorrect:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_rackMaster.Interlock_IsParameterValueCorrect());
                        break;

                    case Dgv_Main_AutoCondition_Row.GOT_On:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, m_rackMaster.Interlock_GOTDetected());
                        break;

                    case Dgv_Main_AutoCondition_Row.Z_Axis_MaintStopper:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_rackMaster.Interlock_ZAxisMaintStopper());
                        break;

                    case Dgv_Main_AutoCondition_Row.EnableSensor:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, !m_rackMaster.Interlock_EnableSensor());
                        break;

                    case Dgv_Main_AutoCondition_Row.MST_EMO_Request:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.MST_Emo_Request));
                        break;

                    case Dgv_Main_AutoCondition_Row.MST_SoftError:
                        m_dgvCtrl.SetWarningCellStyle(ref dgvRMAutoCondition, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.MST_Soft_Error_State));
                        break;
                }
            }
        }

        private void UpdateDataGridView_RackMasterStatus() {
            foreach(Dgv_Main_RMStatus_Row item in Enum.GetValues(typeof(Dgv_Main_RMStatus_Row))) {
                int row = (int)item;
                int col = (int)Dgv_State_Column.State;
                switch (item) {
                    case Dgv_Main_RMStatus_Row.ArmHomeState:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Arm_Home_Position_State));
                        break;

                    case Dgv_Main_RMStatus_Row.AutoCondition:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Auto_Ready));
                        break;

                    case Dgv_Main_RMStatus_Row.AutoState:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.IsAutoState());
                        break;

                    case Dgv_Main_RMStatus_Row.CSTOn:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.CST_ON));
                        break;

                    case Dgv_Main_RMStatus_Row.ErrorState:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Error_State));
                        break;

                    case Dgv_Main_RMStatus_Row.ManualCondition:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Manual_Ready));
                        break;

                    case Dgv_Main_RMStatus_Row.ManualState:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Manual_State));
                        break;

                    case Dgv_Main_RMStatus_Row.ServoOff:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Servo_Off_State));
                        break;

                    case Dgv_Main_RMStatus_Row.ServoOffReady:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Servo_Off_Ready));
                        break;

                    case Dgv_Main_RMStatus_Row.ServoOn:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Servo_On_State));
                        break;

                    case Dgv_Main_RMStatus_Row.ServoOnReady:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Servo_On_Ready));
                        break;

                    case Dgv_Main_RMStatus_Row.TurnLeft:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Turn_Left_Position_State));
                        break;

                    case Dgv_Main_RMStatus_Row.TurnRight:
                        m_dgvCtrl.SetOnOffCell(ref dgvRMStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Turn_Right_Position_State));
                        break;
                }
            }
        }

        private void UpdateDataGridView_MotionStatus() {
            foreach(Dgv_Main_Motion_Row item in Enum.GetValues(typeof(Dgv_Main_Motion_Row))) {
                int row = (int)item;
                int col = (int)Dgv_State_Column.State;
                switch (item) {
                    case Dgv_Main_Motion_Row.FromComplete:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.From_Complete));
                        break;

                    case Dgv_Main_Motion_Row.FromReady:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.From_Ready));
                        break;

                    case Dgv_Main_Motion_Row.FromRun:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.From_State));
                        break;

                    case Dgv_Main_Motion_Row.MaintComplete:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Maint_Move_Complete));
                        break;

                    case Dgv_Main_Motion_Row.MaintRun:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.Maint_Move_State));
                        break;

                    case Dgv_Main_Motion_Row.ToComplete:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.To_Complete));
                        break;

                    case Dgv_Main_Motion_Row.ToReady:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.To_Ready));
                        break;

                    case Dgv_Main_Motion_Row.ToRun:
                        m_dgvCtrl.SetOnOffCell(ref dgvMotionStatus, row, col, m_rackMaster.GetSendBit(SendBitMap.To_State));
                        break;

                    case Dgv_Main_Motion_Row.Step:
                        m_dgvCtrl.SetCellText(ref dgvMotionStatus, row, col, m_rackMaster.m_motion.GetRackMasterAutoMotionStepString(m_rackMaster.m_motion.GetCurrentAutoStep()));
                        break;

                    case Dgv_Main_Motion_Row.FromID:
                        m_dgvCtrl.SetCellText(ref dgvMotionStatus, row, col, $"{m_rackMaster.m_motion.GetCurrentTargetFromID()}");
                        break;

                    case Dgv_Main_Motion_Row.ToID:
                        m_dgvCtrl.SetCellText(ref dgvMotionStatus, row, col, $"{m_rackMaster.m_motion.GetCurrentTargetToID()}");
                        break;
                }
            }
        }

        private void UpdateDataGridView_AlarmList() {
            foreach(AlarmList alarm in Enum.GetValues(typeof(AlarmList))) {
                m_dgvCtrl.SetErrorCell(ref dgvAlarmList, (int)alarm, (int)Dgv_State_Column.Name, m_alarm.IsCurrentAlarmContainAt(alarm), false);
                m_dgvCtrl.SetErrorCell(ref dgvAlarmList, (int)alarm, (int)Dgv_State_Column.State, m_alarm.IsCurrentAlarmContainAt(alarm), false);
            }
        }

        private void UpdateDataGridView_PIOMaster() {
            foreach(Dgv_Main_PIOMaster_Row item in Enum.GetValues(typeof(Dgv_Main_PIOMaster_Row))) {
                int row = (int)item;
                int col = (int)Dgv_State_Column.State;
                switch (item) {
                    case Dgv_Main_PIOMaster_Row.LoadRequest:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIOMaster, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Load_Request));
                        break;

                    case Dgv_Main_PIOMaster_Row.PortError:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIOMaster, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Port_Error));
                        break;

                    case Dgv_Main_PIOMaster_Row.Ready:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIOMaster, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Ready));
                        break;

                    case Dgv_Main_PIOMaster_Row.UnloadRequest:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIOMaster, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Unload_Request));
                        break;
                }
            }
        }

        private void UpdateDataGridView_PIORackMaster() {
            foreach(Dgv_Main_PIORackMaster_Row item in Enum.GetValues(typeof(Dgv_Main_PIORackMaster_Row))) {
                int row = (int)item;
                int col = (int)Dgv_State_Column.State;
                switch (item) {
                    case Dgv_Main_PIORackMaster_Row.Busy:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIORM, row, col, m_rackMaster.GetSendBit(SendBitMap.PIO_Busy));
                        break;

                    case Dgv_Main_PIORackMaster_Row.Complete:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIORM, row, col, m_rackMaster.GetSendBit(SendBitMap.PIO_Complete));
                        break;

                    case Dgv_Main_PIORackMaster_Row.STKError:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIORM, row, col, m_rackMaster.GetSendBit(SendBitMap.PIO_STK_Error));
                        break;

                    case Dgv_Main_PIORackMaster_Row.TRRequest:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIORM, row, col, m_rackMaster.GetSendBit(SendBitMap.PIO_TR_Request));
                        break;
                }
            }
        }

        private void UpdateDataGridView_MasterIO() {
            foreach(Dgv_Main_IOMaster_Row item in Enum.GetValues(typeof(Dgv_Main_IOMaster_Row))) {
                int row = (int)item;
                int col = (int)Dgv_State_Column.State;
                switch (item) {
                    case Dgv_Main_IOMaster_Row.HPDoor:
                        m_dgvCtrl.SetOnOffCell(ref dgvMasterIO, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_HP_Door_Open));
                        break;

                    case Dgv_Main_IOMaster_Row.HPEMO:
                        m_dgvCtrl.SetOnOffCell(ref dgvMasterIO, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_HP_EMO));
                        break;

                    case Dgv_Main_IOMaster_Row.HPEscape:
                        m_dgvCtrl.SetOnOffCell(ref dgvMasterIO, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_HP_Escape));
                        break;

                    case Dgv_Main_IOMaster_Row.KeyAuto:
                        m_dgvCtrl.SetOnOffCell(ref dgvMasterIO, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_Key_Auto));
                        break;

                    case Dgv_Main_IOMaster_Row.OPDoor:
                        m_dgvCtrl.SetOnOffCell(ref dgvMasterIO, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_OP_Door_Open));
                        break;

                    case Dgv_Main_IOMaster_Row.OPEMO:
                        m_dgvCtrl.SetOnOffCell(ref dgvMasterIO, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_OP_EMO));
                        break;

                    case Dgv_Main_IOMaster_Row.OPEscape:
                        m_dgvCtrl.SetOnOffCell(ref dgvMasterIO, row, col, m_rackMaster.GetReceiveBit(ReceiveBitMap.RM_OP_Escape));
                        break;
                }
            }
        }

        private void UpdateDataGridView_RackMasterIO() {
            foreach(Dgv_Main_IORackMaster_Row item in Enum.GetValues(typeof(Dgv_Main_IORackMaster_Row))) {
                int row = (int)item;
                int col = (int)Dgv_State_Column.State;
                switch (item) {
                    case Dgv_Main_IORackMaster_Row.DoubleStorageSensor_1:
                        if (m_rackMaster.IsInputEnabled(InputList.Double_Storage_Sensor_1)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Double_Storage_Sensor_1, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.DoubleStorageSensor_2:
                        if (m_rackMaster.IsInputEnabled(InputList.Double_Storage_Sensor_2)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Double_Storage_Sensor_2, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PickSensor_Left:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Pick_Sensor_Left)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Left, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PlacementSensor_1:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_In_Place_1)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Fork_In_Place_1, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PlacementSensor_2:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_In_Place_2)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Fork_In_Place_2, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PlaceSensor_Left:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Place_Sensor_Left)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Left, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PresenceSensor_1:
                        if (m_rackMaster.IsInputEnabled(InputList.Presense_Detection_1)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Presense_Detection_1, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PresenceSensor_2:
                        if (m_rackMaster.IsInputEnabled(InputList.Presense_Detection_2)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Presense_Detection_2, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.StickSensor_1:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_1)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.StickDetectSensor_1, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.StickSensor_2:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_2)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.StickDetectSensor_2, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.StickSensor_3:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_3)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.StickDetectSensor_3, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.StickSensor_4:
                        if (m_rackMaster.IsInputEnabled(InputList.StickDetectSensor_4)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.StickDetectSensor_4, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PickSensor_Right:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Pick_Sensor_Right)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Fork_Pick_Sensor_Right, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;

                    case Dgv_Main_IORackMaster_Row.PlaceSensor_Right:
                        if (m_rackMaster.IsInputEnabled(InputList.Fork_Place_Sensor_Right)) {
                            m_dgvCtrl.SetOnOffCell(ref dgvRMIO, row, col, m_rackMaster.GetInputBit(InputList.Fork_Place_Sensor_Right, UICtrl.UI_IsIORawData()));
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvRMIO, row, col);
                        }
                        break;
                }
            }
        }

        private void UpdateDataGridView_ServoStatus() {
            //if (m_tempTimer.Delay(m_tempTimerCount)) {
            //    m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.X_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{m_motion.GetAmpTemperature(AxisList.X_Axis)}");
            //    m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.Z_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{m_motion.GetAmpTemperature(AxisList.Z_Axis)}");
            //    m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.A_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{m_motion.GetAmpTemperature(AxisList.A_Axis)}");
            //    if (m_rackMaster.m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
            //        m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.T_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{m_motion.GetAmpTemperature(AxisList.X_Axis)}");
            //    }

            //    m_tempTimer.Stop();
            //    m_tempTimer.Reset();
            //    m_tempTimer.Start();
            //}

            foreach (Dgv_Main_ServoStatus_Row row in Enum.GetValues(typeof(Dgv_Main_ServoStatus_Row))) {
                foreach(Dgv_Main_ServoStatus_Column col in Enum.GetValues(typeof(Dgv_Main_ServoStatus_Column))) {
                    int rowIdx = (int)row;
                    int colIdx = (int)col;
                    AxisList axis = AxisList.X_Axis;

                    switch (row) {
                        case Dgv_Main_ServoStatus_Row.X_Axis:
                            axis = AxisList.X_Axis;
                            break;

                        case Dgv_Main_ServoStatus_Row.Z_Axis:
                            axis = AxisList.Z_Axis;
                            break;

                        case Dgv_Main_ServoStatus_Row.A_Axis:
                            axis = AxisList.A_Axis;
                            break;

                        case Dgv_Main_ServoStatus_Row.T_Axis:
                            axis = AxisList.T_Axis;
                            break;
                    }

                    if(m_rackMaster.m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && axis == AxisList.T_Axis) {
                        if(col == Dgv_Main_ServoStatus_Column.AxisName) {
                            m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, UICtrl.m_disableColor);
                        }
                        else {
                            m_dgvCtrl.SetCellDisableStyle(ref dgvServoStatus, rowIdx, colIdx);
                        }
                        continue;
                    }
                    else {
                        if(col == Dgv_Main_ServoStatus_Column.AxisName) {
                            m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, UICtrl.m_idleColor);
                        }
                    }

                    switch (col) {
                        case Dgv_Main_ServoStatus_Column.Alarm:
                            int alarmCode = m_motion.GetAxisAlarmCode(axis);
                            if (m_motion.GetAxisFlag(AxisFlagType.Alarm, axis)) {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{alarmCode:X}", UICtrl.m_errorColor);
                            }
                            else {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"None", UICtrl.m_idleColor);
                            }
                            break;

                        case Dgv_Main_ServoStatus_Column.HomeSensor:
                            m_dgvCtrl.SetOnOffCell(ref dgvServoStatus, rowIdx, colIdx, m_motion.GetAxisSensor(AxisSensorType.Home, axis));
                            break;

                        case Dgv_Main_ServoStatus_Column.HomeState:
                            if(m_motion.GetAxisFlag(AxisFlagType.HomeDone, axis)) {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, "Done", UICtrl.m_onColor);
                            }
                            else {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, "Not Homed", UICtrl.m_idleColor);
                            }
                            break;

                        case Dgv_Main_ServoStatus_Column.NLimit:
                            m_dgvCtrl.SetOnOffCell(ref dgvServoStatus, rowIdx, colIdx, m_motion.GetAxisSensor(AxisSensorType.Negative_Limit, axis));
                            break;

                        case Dgv_Main_ServoStatus_Column.PLimit:
                            m_dgvCtrl.SetOnOffCell(ref dgvServoStatus, rowIdx, colIdx, m_motion.GetAxisSensor(AxisSensorType.Positive_Limit, axis));
                            break;

                        case Dgv_Main_ServoStatus_Column.Position:
                            if(m_rackMaster.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Drum && axis == AxisList.Z_Axis) {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{(m_motion.GetDrumBeltZAxisActualPosition() / 1000):F3}", UICtrl.m_idleColor);
                            }
                            else {
                                m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{(m_motion.GetAxisStatus(AxisStatusType.pos_act, axis) / 1000):F3}", UICtrl.m_idleColor);
                            }
                            break;

                        case Dgv_Main_ServoStatus_Column.ServoOn:
                            m_dgvCtrl.SetOnOffCell(ref dgvServoStatus, rowIdx, colIdx, m_motion.GetAxisFlag(AxisFlagType.Servo_On, axis));
                            break;

                        case Dgv_Main_ServoStatus_Column.Torque:
                            m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{m_motion.GetAxisStatus(AxisStatusType.trq_act, axis):F3}", UICtrl.m_idleColor);
                            break;

                        case Dgv_Main_ServoStatus_Column.Velocity:
                            switch (axis) {
                                case AxisList.X_Axis:
                                    m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{(m_motion.GetAxisStatus(AxisStatusType.vel_act, axis) / 1000000 * 60):F3}", UICtrl.m_idleColor);
                                    break;
                                case AxisList.Z_Axis:
                                    if(m_rackMaster.m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                        m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{(m_motion.GetAxisStatus(AxisStatusType.vel_act, axis) / 1000000 * 60):F3}", UICtrl.m_idleColor);
                                    }
                                    else {
                                        m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{(m_motion.GetDrumBeltZAxisActualVelocity() / 1000000 * 60):F3}", UICtrl.m_idleColor);
                                    }
                                    break;

                                case AxisList.A_Axis:
                                case AxisList.T_Axis:
                                    m_dgvCtrl.SetCellStyle(ref dgvServoStatus, rowIdx, colIdx, $"{(m_motion.GetAxisStatus(AxisStatusType.vel_act, axis) / 1000 * 60):F3}", UICtrl.m_idleColor);
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        public void UpdateAmpTemperature(AxisList axis, int temp) {
            switch (axis) {
                case AxisList.X_Axis:
                    m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.X_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{temp}");
                    break;

                case AxisList.Z_Axis:
                    m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.Z_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{temp}");
                    break;

                case AxisList.A_Axis:
                    m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.A_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{temp}");
                    break;

                case AxisList.T_Axis:
                    m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)Dgv_Main_ServoStatus_Row.T_Axis, (int)Dgv_Main_ServoStatus_Column.Temperature, $"{temp}");
                    break;
            }
        }

        public void LanguageChanged() {
            //gboxRackMasterAutoCondition.Text    = SynusLangPack.GetLanguage(gboxRackMasterAutoCondition.Name);
            //gboxRackMasterStatus.Text           = SynusLangPack.GetLanguage(gboxRackMasterStatus.Name);
            //gboxMotionStatus.Text               = SynusLangPack.GetLanguage(gboxMotionStatus.Name);
            ////gboxErrorList.Text         = SynusLangPack.GetLanguage(gboxErrorList.Name);
            //gboxMasterIOStatus.Text             = SynusLangPack.GetLanguage(gboxMasterIOStatus.Name);
            //gboxRackMasterIOStatus.Text         = SynusLangPack.GetLanguage(gboxRackMasterIOStatus.Name);
            //gboxPIOStatus.Text                  = SynusLangPack.GetLanguage(gboxPIOStatus.Name);
            //gboxMaster.Text                     = SynusLangPack.GetLanguage(gboxMaster.Name);
            //gboxRackMaster.Text                 = SynusLangPack.GetLanguage(gboxRackMaster.Name);
            //gboxAxisStatus.Text                 = SynusLangPack.GetLanguage(gboxAxisStatus.Name);

            ////foreach(Dgv_Main_ServoStatus_Column column in Enum.GetValues(typeof(Dgv_Main_ServoStatus_Column))) {
            ////    m_dgvCtrl.SetColumnHeaderText(ref dgvServoStatus, (int)column, SynusLangPack.GetLanguage($"{column}"));
            ////    if(column == Dgv_Main_ServoStatus_Column.AxisName) {
            ////        foreach(Dgv_Main_ServoStatus_Row row in Enum.GetValues(typeof(Dgv_Main_ServoStatus_Row))) {
            ////            m_dgvCtrl.SetCellText(ref dgvServoStatus, (int)row, (int)column, SynusLangPack.GetLanguage($"{row}"));
            ////        }
            ////    }
            ////}
        }
    }
}
