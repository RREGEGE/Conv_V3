using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ.PART;

namespace RackMaster.SUBFORM.StatusPage {
    public partial class StatusPage_IO : Form {
        private enum DgvStateColumn {
            Name,
            State,
        }

        private enum DgvMasterPIORow {
            L_Request,
            UL_Request,
            Ready,
            PortError
        }

        private enum DgvRackMasterPIORow {
            TR_Req,
            Busy,
            Complete,
            STKError,
        }

        private enum DgvRegulatorRow {
            STX = 0,
            Status,
            BoostVoltage,
            OutputVoltage,
            BoostCurrent,
            OutputCurrent,
            HitSinkTemperature,
            InsideNTC,
            PickupNTC,
            ErrorCode,
            CheckSum,
            End,
        }

        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private RackMasterMain m_rackMaster;
        private RackMasterMain.RackMasterParam m_param;

        private bool m_isModifyMode = false;

        public StatusPage_IO(RackMasterMain rackMaster) {
            m_rackMaster = rackMaster;
            m_param = m_rackMaster.m_param;
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();

            InitializeComponent();

            InitDataGridView_IO();
            InitDataGridView_PIO();
            InitDataGridView_Regulator();
        }

        public void SetModifyMode(bool isModifyModeOn) {
            m_isModifyMode = isModifyModeOn;

            if (!m_isModifyMode) {
                foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                    m_rackMaster.SetOutputBit(output, false);
                }
            }
        }

        public bool GetModifyMode() {
            return m_isModifyMode;
        }

        private void InitDataGridView_IO() {
            foreach (InputList input in Enum.GetValues(typeof(InputList))) {
                switch (input) {
                    case InputList.RM_MC_On:
                        m_dgvCtrl.AddRow(ref dgvInput, "RM MC On", (int)DgvStateColumn.Name);
                        break;

                    case InputList.EMO_HP:
                        m_dgvCtrl.AddRow(ref dgvInput, "EMO HP", (int)DgvStateColumn.Name);
                        break;

                    case InputList.EMO_OP:
                        m_dgvCtrl.AddRow(ref dgvInput, "EMO OP", (int)DgvStateColumn.Name);
                        break;

                    case InputList.HP_DTP_EMS_SW:
                        m_dgvCtrl.AddRow(ref dgvInput, "HP DTP EMS SW", (int)DgvStateColumn.Name);
                        break;

                    case InputList.HP_DTP_DeadMan_SW:
                        m_dgvCtrl.AddRow(ref dgvInput, "HP DTP DeadMan SW", (int)DgvStateColumn.Name);
                        break;

                    case InputList.HP_DTP_Mode_Select_SW_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "HP DTP Mode Select SW 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.HP_DTP_Mode_Select_SW_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "HP DTP Mode Select SW 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.OP_DTP_EMS_SW:
                        m_dgvCtrl.AddRow(ref dgvInput, "OP DTP EMS SW", (int)DgvStateColumn.Name);
                        break;

                    case InputList.OP_DTP_DeadMan_SW:
                        m_dgvCtrl.AddRow(ref dgvInput, "OP DTP DeadMan SW", (int)DgvStateColumn.Name);
                        break;

                    case InputList.OP_DTP_Mode_Select_SW_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "OP DTP Mode Select SW 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.OP_DTP_Mode_Select_SW_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "OP DTP Mode Select SW 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Z_Axis_Maint_Stopper_Sensor_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "Z Axis Maint Stopper Sensor 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Z_Axis_Maint_Stopper_Sensor_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "Z Axis Maint Stopper Sensor 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Z_Axis_Wire_Cut_Sensor:
                        m_dgvCtrl.AddRow(ref dgvInput, "Z Axis Wire Cut Sensor", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Presense_Detection_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "Presense Detection 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Presense_Detection_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "Presense Detection 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Double_Storage_Sensor_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "Double Storage Sensor 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Double_Storage_Sensor_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "Double Storage Sensor 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Fork_Pick_Sensor_Left:
                        m_dgvCtrl.AddRow(ref dgvInput, "Fork Pick Sensor Left", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Fork_Place_Sensor_Left:
                        m_dgvCtrl.AddRow(ref dgvInput, "Fork Place Sensor Left", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Fork_Pick_Sensor_Right:
                        m_dgvCtrl.AddRow(ref dgvInput, "Fork Pick Sensor Right", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Fork_Place_Sensor_Right:
                        m_dgvCtrl.AddRow(ref dgvInput, "Fork Place Sensor Right", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Fork_In_Place_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "Placement Sensor 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.Fork_In_Place_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "Placement Sensor 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.StickDetectSensor_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "Stick Detect Sensor 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.StickDetectSensor_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "Stick Detect Sensor 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.StickDetectSensor_3:
                        m_dgvCtrl.AddRow(ref dgvInput, "Stick Detect Sensor 3", (int)DgvStateColumn.Name);
                        break;

                    case InputList.StickDetectSensor_4:
                        m_dgvCtrl.AddRow(ref dgvInput, "Stick Detect Sensor 4", (int)DgvStateColumn.Name);
                        break;

                    case InputList.X_Axis_Position_Sensor_1:
                        m_dgvCtrl.AddRow(ref dgvInput, "X Axis Position Sensor 1", (int)DgvStateColumn.Name);
                        break;

                    case InputList.X_Axis_Position_Sensor_2:
                        m_dgvCtrl.AddRow(ref dgvInput, "X Axis Position Sensor 2", (int)DgvStateColumn.Name);
                        break;

                    case InputList.X_Axis_HP_Moving_Speed_Cut_Neg:
                        m_dgvCtrl.AddRow(ref dgvInput, "X Axis HP Moving Speed Cut Neg", (int)DgvStateColumn.Name);
                        break;

                    case InputList.X_Axis_HP_Moving_Speed_Cut_Pos:
                        m_dgvCtrl.AddRow(ref dgvInput, "X Axis HP Moving Speed Cut Pos", (int)DgvStateColumn.Name);
                        break;

                    case InputList.CPS_Regulator_Run:
                        m_dgvCtrl.AddRow(ref dgvInput, "CPS Regulator Run", (int)DgvStateColumn.Name);
                        break;

                    case InputList.CPS_Regulator_Fault:
                        m_dgvCtrl.AddRow(ref dgvInput, "CPS Regulator Fault", (int)DgvStateColumn.Name);
                        break;
                }
            }

            foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                switch (output) {
                    case OutputList.Handy_Touch_EMO_Relay_On:
                        m_dgvCtrl.AddRow(ref dgvOutput, "Handy Touch EMO Relay On", (int)DgvStateColumn.Name);
                        break;

                    case OutputList.Voice_Buzzer_CH1:
                        m_dgvCtrl.AddRow(ref dgvOutput, "Voice Buzzer CH1", (int)DgvStateColumn.Name);
                        break;

                    case OutputList.Voice_Buzzer_CH2:
                        m_dgvCtrl.AddRow(ref dgvOutput, "Voice Buzzer CH2", (int)DgvStateColumn.Name);
                        break;

                    case OutputList.Voice_Buzzer_CH3:
                        m_dgvCtrl.AddRow(ref dgvOutput, "Voice Buzzer CH3", (int)DgvStateColumn.Name);
                        break;

                    case OutputList.Voice_Buzzer_CH4:
                        m_dgvCtrl.AddRow(ref dgvOutput, "Voice Buzzer CH4", (int)DgvStateColumn.Name);
                        break;
                }
            }

            foreach (DataGridViewColumn column in dgvInput.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            foreach (DataGridViewColumn column in dgvOutput.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void InitDataGridView_PIO() {
            dgvPIOMaster.ReadOnly = true;
            dgvRackMasterPIO.ReadOnly = true;
            foreach (DgvMasterPIORow row in Enum.GetValues(typeof(DgvMasterPIORow))) {
                switch (row) {
                    case DgvMasterPIORow.L_Request:
                        m_dgvCtrl.AddRow(ref dgvPIOMaster, "L-Req", (int)DgvStateColumn.Name);
                        break;

                    case DgvMasterPIORow.UL_Request:
                        m_dgvCtrl.AddRow(ref dgvPIOMaster, "UL-Req", (int)DgvStateColumn.Name);
                        break;

                    case DgvMasterPIORow.Ready:
                        m_dgvCtrl.AddRow(ref dgvPIOMaster, "Ready", (int)DgvStateColumn.Name);
                        break;

                    case DgvMasterPIORow.PortError:
                        m_dgvCtrl.AddRow(ref dgvPIOMaster, "Error", (int)DgvStateColumn.Name);
                        break;
                }
            }

            foreach (DgvRackMasterPIORow row in Enum.GetValues(typeof(DgvRackMasterPIORow))) {
                switch (row) {
                    case DgvRackMasterPIORow.TR_Req:
                        m_dgvCtrl.AddRow(ref dgvRackMasterPIO, "TR-Req", (int)DgvStateColumn.Name);
                        break;

                    case DgvRackMasterPIORow.Busy:
                        m_dgvCtrl.AddRow(ref dgvRackMasterPIO, "Busy", (int)DgvStateColumn.Name);
                        break;

                    case DgvRackMasterPIORow.Complete:
                        m_dgvCtrl.AddRow(ref dgvRackMasterPIO, "Complete", (int)DgvStateColumn.Name);
                        break;

                    case DgvRackMasterPIORow.STKError:
                        m_dgvCtrl.AddRow(ref dgvRackMasterPIO, "Error", (int)DgvStateColumn.Name);
                        break;
                }
            }

            foreach (DataGridViewColumn column in dgvPIOMaster.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn column in dgvRackMasterPIO.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void InitDataGridView_Regulator() {
            dgvRegulator.ReadOnly = true;
            foreach (DgvRegulatorRow row in Enum.GetValues(typeof(DgvRegulatorRow))) {
                switch (row) {
                    case DgvRegulatorRow.STX:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "STX", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.Status:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Status", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.BoostVoltage:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Boost Voltage", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.OutputVoltage:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Output Voltage", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.BoostCurrent:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Boost Current", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.OutputCurrent:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Output Current", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.HitSinkTemperature:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Hitsink Temperature", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.InsideNTC:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Inside NTC", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.PickupNTC:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Pickup NTC", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.ErrorCode:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Error Code", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.CheckSum:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "Check Sum", (int)DgvStateColumn.Name);
                        break;

                    case DgvRegulatorRow.End:
                        m_dgvCtrl.AddRow(ref dgvRegulator, "End", (int)DgvStateColumn.Name);
                        break;
                }
            }

            foreach (DataGridViewColumn column in dgvRegulator.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        public void UpdateFormUI() {
            UpdateDataGridView_IO();
            UpdateDataGridView_PIO();
            if (m_param.GetMotionParam().useRegulator) {
                gboxRegulator.Visible = true;
                UpdateDataGridView_Regulator();
            }
            else {
                gboxRegulator.Visible = false;
            }
        }

        private void UpdateDataGridView_IO() {
            foreach (InputList input in Enum.GetValues(typeof(InputList))) {
                if (m_rackMaster.IsInputEnabled(input)) {
                    m_dgvCtrl.SetOnOffCell(ref dgvInput, (int)input, (int)DgvStateColumn.State, m_rackMaster.GetInputBit(input, UICtrl.UI_IsIORawData()));
                }
                else {
                    m_dgvCtrl.SetCellDisableStyle(ref dgvInput, (int)input, (int)DgvStateColumn.State);
                }
            }

            foreach (OutputList output in Enum.GetValues(typeof(OutputList))) {
                if (m_rackMaster.IsOutputEnabled(output)) {
                    m_dgvCtrl.SetOnOffCell(ref dgvOutput, (int)output, (int)DgvStateColumn.State, m_rackMaster.GetOutputBit(output));
                }
                else {
                    m_dgvCtrl.SetCellDisableStyle(ref dgvOutput, (int)output, (int)DgvStateColumn.State);
                }
            }
        }

        private void UpdateDataGridView_PIO() {
            foreach (DgvMasterPIORow row in Enum.GetValues(typeof(DgvMasterPIORow))) {
                switch (row) {
                    case DgvMasterPIORow.L_Request:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIOMaster, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Load_Request));
                        break;

                    case DgvMasterPIORow.UL_Request:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIOMaster, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Unload_Request));
                        break;

                    case DgvMasterPIORow.Ready:
                        m_dgvCtrl.SetOnOffCell(ref dgvPIOMaster, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Ready));
                        break;

                    case DgvMasterPIORow.PortError:
                        m_dgvCtrl.SetErrorCell(ref dgvPIOMaster, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetReceiveBit(ReceiveBitMap.PIO_Port_Error));
                        break;
                }
            }

            foreach (DgvRackMasterPIORow row in Enum.GetValues(typeof(DgvRackMasterPIORow))) {
                switch (row) {
                    case DgvRackMasterPIORow.TR_Req:
                        m_dgvCtrl.SetOnOffCell(ref dgvRackMasterPIO, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetSendBit(SendBitMap.PIO_TR_Request));
                        break;

                    case DgvRackMasterPIORow.Busy:
                        m_dgvCtrl.SetOnOffCell(ref dgvRackMasterPIO, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetSendBit(SendBitMap.PIO_Busy));
                        break;

                    case DgvRackMasterPIORow.Complete:
                        m_dgvCtrl.SetOnOffCell(ref dgvRackMasterPIO, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetSendBit(SendBitMap.PIO_Complete));
                        break;

                    case DgvRackMasterPIORow.STKError:
                        m_dgvCtrl.SetErrorCell(ref dgvRackMasterPIO, (int)row, (int)DgvStateColumn.State, m_rackMaster.GetSendBit(SendBitMap.PIO_STK_Error));
                        break;
                }
            }
        }

        private void UpdateDataGridView_Regulator() {
            foreach (DgvRegulatorRow row in Enum.GetValues(typeof(DgvRegulatorRow))) {
                switch (row) {
                    case DgvRegulatorRow.STX:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.STX)}");
                        break;

                    case DgvRegulatorRow.Status:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.Status)}");
                        break;

                    case DgvRegulatorRow.BoostVoltage:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.BoostVoltage)}");
                        break;

                    case DgvRegulatorRow.OutputVoltage:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.OutputVoltage)}");
                        break;

                    case DgvRegulatorRow.BoostCurrent:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.BoostCurrent)}");
                        break;

                    case DgvRegulatorRow.OutputCurrent:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.OutputCurrent)}");
                        break;

                    case DgvRegulatorRow.HitSinkTemperature:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.HitSinkTemperature)}");
                        break;

                    case DgvRegulatorRow.InsideNTC:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.InsideNTC)}");
                        break;

                    case DgvRegulatorRow.PickupNTC:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.PickupNTC)}");
                        break;

                    case DgvRegulatorRow.ErrorCode:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.ErrorCode)}");
                        break;

                    case DgvRegulatorRow.CheckSum:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.CheckSum)}");
                        break;

                    case DgvRegulatorRow.End:
                        m_dgvCtrl.SetCellText(ref dgvRegulator, (int)row, (int)DgvStateColumn.State, $"{m_rackMaster.Regulator_GetData(SEQ.TCP.ProtocolRoles_Regulator.RegulatorToRackMaster.End)}");
                        break;
                }
            }
        }

        private void dgvOutput_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (!GetModifyMode())
                return;

            if (e.ColumnIndex != (int)DgvStateColumn.State)
                return;

            if (m_rackMaster.GetOutputBit((OutputList)e.RowIndex)) {
                m_rackMaster.SetOutputBit((OutputList)e.RowIndex, false);
            }
            else if (!m_rackMaster.GetOutputBit((OutputList)e.RowIndex)) {
                m_rackMaster.SetOutputBit((OutputList)e.RowIndex, true);
            }
        }

        private void dgvRackMasterPIO_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (!GetModifyMode())
                return;

            if (e.ColumnIndex != (int)DgvStateColumn.State)
                return;

            DgvRackMasterPIORow row = (DgvRackMasterPIORow)e.RowIndex;
            SendBitMap bit = SendBitMap.PIO_TR_Request;

            if (row == DgvRackMasterPIORow.TR_Req)
                bit = SendBitMap.PIO_TR_Request;
            else if (row == DgvRackMasterPIORow.Busy)
                bit = SendBitMap.PIO_Busy;
            else if (row == DgvRackMasterPIORow.Complete)
                bit = SendBitMap.PIO_Complete;
            else if (row == DgvRackMasterPIORow.STKError)
                bit = SendBitMap.PIO_STK_Error;

            if (m_rackMaster.GetSendBit(bit)) {
                m_rackMaster.SetSendBit(bit, false);
            }
            else {
                m_rackMaster.SetSendBit(bit, true);
            }
        }
    }
}
