using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Master.Interface.Alarm;
using Master.Equipment.CPS;

namespace Master
{
    /// <summary>
    /// MasterUI.cs 는 Form에서 사용되는 UI 아이템의 색상, 변경 등 UI Item 반응에 대한 작성 영역
    /// </summary>
    /// 
    static partial class Master
    {
        /// <summary>
        /// Master Alarm Info or CPS Alarm 영역의 DataGridView Column에 활용
        /// </summary>
        enum DGV_ErrorInfoColumn
        {
            ErrorGenTime,
            ErrorIndex,
            ErrorCode,
            ErrorClearTime
        }

        /// <summary>
        /// Mainform or MasterControlForm Main 메뉴의 안전 관련 Sensor DataGridView의 Row에 활용
        /// </summary>
        public enum DGV_SaftyIOStatusRow
        {
            RM_GOT_EStop,
            RM_HP_EMO,
            RM_OP_EMO,

            Master_HP_DoorOpen,
            Master_OP_DoorOpen,
            Master_HP_EMO_Pushing,
            Master_HP_EMO_Escape_Status,
            Master_OP_EMO_Pushing,
            Master_OP_EMO_Escape_Status,
            Master_HP_MasterKey_AutoMode_Status,
            Bypass_Relay,
            Door_Open_Relay,
            Master_Maint_Door_Open,
            Master_Port_Handy_Touch_EMO,
            Master_HP_Handy_Touch_EMO,
            Master_OP_Handy_Touch_EMO,
            Master_CPS_Run,
            Master_CPS_Fault,
            Master_MCUL_Fault,

            Master_Inner_EMO1,
            Master_Inner_EMO2,
            Master_Inner_EMO3,
            Master_Inner_EMO4
        }

        /// <summary>
        /// Mainform or MasterControlForm Main 메뉴의 안전 관련 Sensor DataGridView의 Column에 활용
        /// </summary>
        enum DGV_SaftyIOStatusColumn
        {
            Number,
            StatusName,
            Status
        }

        /// <summary>
        /// Mainform or MasterControlForm Main 메뉴의 안전 관련 Sensor DataGridView의 Column에 활용
        /// </summary>
        public enum DGV_EquipmentStatusColumn
        {
            Equip,
            Auto,
            ControlMode,
            Power,
            Busy,
            Error,
            Control
        }

        /// <summary>
        /// Master -> CPS 메뉴의 CPS 상태 출력과 관련된 DataGridView의 Row에 활용
        /// </summary>
        enum DGV_CPSStatusInfoRow
        {
            TCPIPInfo,
            TCPIPState,
            CPSStatus,
            Voltage,
            DC_Current,
            IGBT_Current ,
            Track_Current ,
            OutputFreq ,
            Converter_Heatsink_Temp ,
            Covnerter_Inner_Temp ,
            Converter_Error_Code ,
            RequestCount,
            ReadCount
        }

        /// <summary>
        /// Master Alarm Info DataGridView의 Column에 활용
        /// </summary>
        public enum DGV_AlarmListColumn
        {
            Index,
            Hex,
            AlarmName,
            State
        }

        /// <summary>
        /// Master Alarm Info DataGridView의 Row에 활용
        /// </summary>
        enum DGV_ErrorWordRow
        {
            ErrorWord0,
            ErrorWord1,
            ErrorWord2,
            ErrorWord3,
            ErrorWord4,
            ErrorWord5,
            ErrorWord6
        }

        static public string GetMasterSafetyStr(DGV_SaftyIOStatusRow eDGV_SaftyIOStatusRow)
        {
            switch(eDGV_SaftyIOStatusRow)
            {
                case DGV_SaftyIOStatusRow.RM_GOT_EStop:
                    return "STK_Handy_Touch_EMO";
            case DGV_SaftyIOStatusRow.RM_HP_EMO:
                    return "STK_HP_EMO";
                case DGV_SaftyIOStatusRow.RM_OP_EMO:
                    return "STK_OP_EMO";
                case DGV_SaftyIOStatusRow.Master_HP_DoorOpen:
                    return "HP_Door_Open";
                case DGV_SaftyIOStatusRow.Master_OP_DoorOpen:
                    return "OP_Door_Open";
                case DGV_SaftyIOStatusRow.Master_HP_EMO_Pushing:
                    return "HP_Outside_EMO";
                case DGV_SaftyIOStatusRow.Master_HP_EMO_Escape_Status:
                    return "HP_Inside_EMO";
                case DGV_SaftyIOStatusRow.Master_OP_EMO_Pushing:
                    return "OP_Outside_EMO";
                case DGV_SaftyIOStatusRow.Master_OP_EMO_Escape_Status:
                    return "OP_Inside_EMO";
                case DGV_SaftyIOStatusRow.Master_HP_MasterKey_AutoMode_Status:
                    return "HP_Auto_Key";
                case DGV_SaftyIOStatusRow.Bypass_Relay:
                    return "Relay_Bypass";
                case DGV_SaftyIOStatusRow.Door_Open_Relay:
                    return "Relay_Door_Open";
                case DGV_SaftyIOStatusRow.Master_Maint_Door_Open:
                    return "Maint_Door_Open";
                case DGV_SaftyIOStatusRow.Master_Port_Handy_Touch_EMO:
                    return "Port_Handy_Touch_EMO";
                case DGV_SaftyIOStatusRow.Master_HP_Handy_Touch_EMO:
                    return "HP_Handy_Touch_EMO";
                case DGV_SaftyIOStatusRow.Master_OP_Handy_Touch_EMO:
                    return "OP_Handy_Touch_EMO";
                case DGV_SaftyIOStatusRow.Master_CPS_Run:
                    return "CPS Running";
                case DGV_SaftyIOStatusRow.Master_CPS_Fault:
                    return "CPS Error";
                case DGV_SaftyIOStatusRow.Master_MCUL_Fault:
                    return "MCUL Error";
                case DGV_SaftyIOStatusRow.Master_Inner_EMO1:
                    return "Inside_EMO_1";
                case DGV_SaftyIOStatusRow.Master_Inner_EMO2:
                    return "Inside_EMO_2";
                case DGV_SaftyIOStatusRow.Master_Inner_EMO3:
                    return "Inside_EMO_3";
                case DGV_SaftyIOStatusRow.Master_Inner_EMO4:
                    return "Inside_EMO_4";
                default:
                    return "None";
            }
        }

        static public void Update_Btn_BuzzerStop(ref Button btn)
        {
            ButtonFunc.SetText(btn, CMD_Buzzer_Mute_REQ ? SynusLangPack.GetLanguage("Btn_BuzzerOn") : SynusLangPack.GetLanguage("Btn_BuzzerStop"));
            ButtonFunc.SetBackColor(btn, CMD_Buzzer_Mute_REQ ? Color.LightGray : Color.White);
            ButtonFunc.SetImage(btn, CMD_Buzzer_Mute_REQ ? Properties.Resources.icons8_audio_48 : Properties.Resources.icons8_mute_48);
        }
        static public void Update_Btn_NormalMasterAlarmClear(ref Button btn)
        {
            var eAlarmLevel = GetAlarmLevel();
            ButtonFunc.SetBackColor(btn, eAlarmLevel == AlarmLevel.Error ? Master.ErrorIntervalColor : eAlarmLevel == AlarmLevel.Warning ? Color.Orange : Color.White);
            ButtonFunc.SetEnable(btn, eAlarmLevel == AlarmLevel.Error || eAlarmLevel == AlarmLevel.Warning ? true : false);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AlarmClear"));
        }
        static public void Update_Btn_MasterAlarmClear(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_MasterAlarmClear"));
            ButtonFunc.SetBackColor(btn, GetAlarmLevel() == AlarmLevel.Error ? Master.ErrorIntervalColor : Color.White);
            ButtonFunc.SetEnable(btn, GetAlarmLevel() == AlarmLevel.Error ? true : false);
        }
        static public void Update_Btn_AllAlarmClear(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AllAlarmClear"));

            bool bAlarmState = false;

            bAlarmState = GetAlarmLevel() == AlarmLevel.Error ? true : bAlarmState;

            if (m_RackMasters != null)
            {
                foreach (var rackmaster in m_RackMasters)
                {
                    bAlarmState = rackmaster.Value.IsAlarmState() ? true : bAlarmState;
                }
            }
            if (m_Ports != null)
            {
                foreach (var port in m_Ports)
                {
                    bAlarmState = port.Value.GetAlarmLevel() == AlarmLevel.Error ? true : bAlarmState;
                }
            }

            ButtonFunc.SetBackColor(btn, bAlarmState ? Master.ErrorIntervalColor : Color.White);
            ButtonFunc.SetEnable(btn, bAlarmState ? true : false);
        }

        static public void Update_Btn_AllEquipPowerOn(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AllEquipPowerOn"));

            if (!btn.Enabled)
            {
                ButtonFunc.SetBackColor(btn, Color.LightGray);
                return;
            }

            bool bAllPowerOnState = true;

            foreach (var rackmaster in m_RackMasters)
            {
                if (!bAllPowerOnState)
                    break;

                if (!rackmaster.Value.Status_ServoOn)
                    bAllPowerOnState = false;
            }
            foreach (var port in m_Ports)
            {
                if (!bAllPowerOnState)
                    break;

                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                if (!port.Value.IsPortPowerOn())
                    bAllPowerOnState = false;
            }

            ButtonFunc.SetBackColor(btn, bAllPowerOnState ? Color.Lime : Color.White);
        }

        static public void Update_Btn_AllEquipPowerOff(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AllEquipPowerOff"));

            if (!btn.Enabled)
            {
                ButtonFunc.SetBackColor(btn, Color.LightGray);
                return;
            }

            bool bAllPowerOffState = true;

            foreach (var rackmaster in m_RackMasters)
            {
                if (!bAllPowerOffState)
                    break;

                if (rackmaster.Value.Status_ServoOn)
                    bAllPowerOffState = false;
            }
            foreach (var port in m_Ports)
            {
                if (!bAllPowerOffState)
                    break;

                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                if (port.Value.IsPortPowerOn())
                    bAllPowerOffState = false;
            }

            ButtonFunc.SetBackColor(btn, bAllPowerOffState ? Color.Orange : Color.White);
        }

        static public void Update_Btn_AllEquipCIMMode(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AllEquipCIMMode"));

            if (!btn.Enabled)
            {
                ButtonFunc.SetBackColor(btn, Color.LightGray);
                return;
            }

            bool bAllCIMModeState = true;

            foreach (var rackmaster in m_RackMasters)
            {
                if (!bAllCIMModeState)
                    break;

                if (rackmaster.Value.m_eControlMode != Equipment.RackMaster.RackMaster.ControlMode.CIMMode)
                    bAllCIMModeState = false;
            }
            foreach (var port in m_Ports)
            {
                if (!bAllCIMModeState)
                    break;

                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                if (port.Value.m_eControlMode != Equipment.Port.Port.ControlMode.CIMMode)
                    bAllCIMModeState = false;
            }

            ButtonFunc.SetBackColor(btn, bAllCIMModeState ? Color.Lime : Color.White);
        }
        static public void Update_Btn_AllEquipMasterMode(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AllEquipMasterMode"));

            if (!btn.Enabled)
            {
                ButtonFunc.SetBackColor(btn, Color.LightGray);
                return;
            }

            bool bAllMasterModeState = true;

            foreach (var rackmaster in m_RackMasters)
            {
                if (!bAllMasterModeState)
                    break;

                if (rackmaster.Value.m_eControlMode != Equipment.RackMaster.RackMaster.ControlMode.MasterMode)
                    bAllMasterModeState = false;
            }
            foreach (var port in m_Ports)
            {
                if (!bAllMasterModeState)
                    break;

                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                if (port.Value.m_eControlMode != Equipment.Port.Port.ControlMode.MasterMode)
                    bAllMasterModeState = false;
            }

            ButtonFunc.SetBackColor(btn, bAllMasterModeState ? Color.Lime : Color.White);
        }
        static public void Update_Btn_AllEquipAutoRun(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AllEquipAutoRun"));

            if (!btn.Enabled)
            {
                ButtonFunc.SetBackColor(btn, Color.LightGray);
                return;
            }

            bool bAllAutoRunState = true;

            foreach (var rackmaster in m_RackMasters)
            {
                if (!bAllAutoRunState)
                    break;

                if (!rackmaster.Value.Status_AutoMode)
                    bAllAutoRunState = false;
            }
            foreach (var port in m_Ports)
            {
                if (!bAllAutoRunState)
                    break;

                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                if (!port.Value.IsAutoControlRun())
                    bAllAutoRunState = false;
            }

            ButtonFunc.SetBackColor(btn, bAllAutoRunState ? Color.Lime : Color.White);
        }
        static public void Update_Btn_AllEquipAutoStop(ref Button btn)
        {
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AllEquipAutoStop"));

            if (!btn.Enabled)
            {
                ButtonFunc.SetBackColor(btn, Color.LightGray);
                return;
            }

            bool bAllAutoStopState = true;

            foreach (var rackmaster in m_RackMasters)
            {
                if (!bAllAutoStopState)
                    break;

                if (rackmaster.Value.Status_AutoMode)
                    bAllAutoStopState = false;
            }
            foreach (var port in m_Ports)
            {
                if (!bAllAutoStopState)
                    break;

                if (port.Value.GetParam().ePortType == Equipment.Port.Port.PortType.EQ)
                    continue;

                if (port.Value.IsAutoControlRun())
                    bAllAutoStopState = false;
            }

            ButtonFunc.SetBackColor(btn, bAllAutoStopState ? Color.Orange : Color.White);
        }
        static public void Update_Lbl_MasterAlarm(ref Label lbl)
        {
            GetRecentErrorInfo(out int WordIndex, out short AlarmWord, out string AlarmText);
            LabelFunc.SetText(lbl, GetAlarmLevel() == AlarmLevel.None ? string.Empty : $"WordMap[{WordIndex}] : 0x{AlarmWord.ToString("x4")}, Level : {GetAlarmLevel()}");
            LabelFunc.SetForeColor(lbl, GetAlarmLevel() == AlarmLevel.Error ? Color.Red : GetAlarmLevel() == AlarmLevel.Warning ? Color.Orange : Color.White);
        }
        static public void Update_Lbl_MasterAlarmText(ref Label lbl)
        {
            GetRecentErrorInfo(out int WordIndex, out short AlarmWord, out string AlarmText);
            LabelFunc.SetText(lbl, AlarmText);
            LabelFunc.SetForeColor(lbl, GetAlarmLevel() == AlarmLevel.Error ? Color.Red : GetAlarmLevel() == AlarmLevel.Warning ? Color.Orange : Color.Black);
        }
        static public void Update_Lbl_WMXEngineStatus(ref Label lbl)
        {
            LabelFunc.SetText(lbl, MovenCore.WMX3.IsEngineCommunicating() ? "Running" : "Stop");
            LabelFunc.SetForeColor(lbl, MovenCore.WMX3.IsEngineCommunicating() ? Color.Green : Color.Red);
        }

        static public void Update_Lbl_CIMOnline(ref Label lbl)
        {
            LabelFunc.SetText(lbl, m_CIM == null ? $"None" : m_CIM.IsConnected() ? "Online" : "Offline");
            LabelFunc.SetBackColor(lbl, m_CIM == null ? Color.Yellow : m_CIM.IsConnected() ? Color.Lime : Color.White);
        }
        static public void Update_Lbl_CPSOnline(ref Label lbl)
        {
            LabelFunc.SetText(lbl, m_CPS == null ? $"None" : m_CPS.IsConnected() ? "Online" : "Offline");
            LabelFunc.SetBackColor(lbl, m_CPS == null ? Color.Yellow : m_CPS.IsConnected() ? Color.Lime : Master.ErrorIntervalColor);
        }

        static public void Update_Lbl_OMRONOnline(ref Label lbl)
        {
            bool bConnection = m_Omron.IsConnected();
            bool bValidState = m_Omron.m_OmronValid;

            LabelFunc.SetText(lbl, bConnection ? $"Online" : $"Offline");
            LabelFunc.SetBackColor(lbl, bConnection && bValidState ? Color.Lime : Master.ErrorIntervalColor);
        }

        static public void Update_Lbl_CPSPowerOnEnableLamp(ref Label lbl)
        {
            LabelFunc.SetBackColor(lbl, Sensor_CPSPowerOnEnableLamp ? Color.Lime : Color.White);
        }
        static public void Update_Lbl_CPSPowerOnReqLamp(ref Label lbl)
        {
            LabelFunc.SetBackColor(lbl, Sensor_CPSPowerOnReqLamp ? Color.Lime : Color.White);
        }
        static public void Update_Lbl_CPSErrorResetReqLamp(ref Label lbl)
        {
            LabelFunc.SetBackColor(lbl, Sensor_CPSErrorResetLamp ? Color.Lime : Color.White);
        }
        static public void Update_Lbl_CPSErrorLamp(ref Label lbl)
        {
            LabelFunc.SetBackColor(lbl, Sensor_CPSErrorLamp ? Master.ErrorIntervalColor : Color.White);
        }

        static public void Update_DGV_SaftyIOStatus(ref DataGridView DGV, ManagedFile.MasterSafetyImageInfo safetyImageInfo)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_SaftyIOStatusColumn.Number:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Index"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Index");
                        break;
                    case (int)DGV_SaftyIOStatusColumn.StatusName:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SaftyIOName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SaftyIOName");
                        break;
                    case (int)DGV_SaftyIOStatusColumn.Status:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_SaftyIOStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_SaftyIOStatus");
                        break;
                }
            }

            List<DGV_SaftyIOStatusRow> UseRowList = new List<DGV_SaftyIOStatusRow>();

            foreach (DGV_SaftyIOStatusRow eDGV_SaftyIOStatusRow in Enum.GetValues(typeof(DGV_SaftyIOStatusRow)))
            {
                if (safetyImageInfo == null)
                {
                    UseRowList.Add(eDGV_SaftyIOStatusRow);
                    continue;
                }

                int Index = (int)eDGV_SaftyIOStatusRow;
                if (safetyImageInfo.SafetyItems == null)
                    continue;

                if (safetyImageInfo.SafetyItems.Length <= Index)
                    continue;

                if (safetyImageInfo.SafetyItems[Index].GridVisible)
                    UseRowList.Add(eDGV_SaftyIOStatusRow);
            }

            if (DGV.Rows.Count != UseRowList.Count)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < UseRowList.Count; nCount++)
                    DGV.Rows.Add();

            }

            DataGridViewFunc.AutoRowSize(DGV, 30, 25, 40);

            for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
            {
                DGV_SaftyIOStatusRow eDGV_SaftyIOStatusRow = UseRowList[nRowCount];

                DataGridViewCell DGV_Cell_Index = DGV.Rows[nRowCount].Cells[(int)DGV_SaftyIOStatusColumn.Number];
                DataGridViewCell DGV_Cell_Name = DGV.Rows[nRowCount].Cells[(int)DGV_SaftyIOStatusColumn.StatusName];
                DataGridViewCell DGV_Cell_Status = DGV.Rows[nRowCount].Cells[(int)DGV_SaftyIOStatusColumn.Status];
                string value = string.Empty;

                DGV_Cell_Index.Value = $"{(int)eDGV_SaftyIOStatusRow}";
                DGV_Cell_Name.Value = GetMasterSafetyStr(eDGV_SaftyIOStatusRow);

                switch (eDGV_SaftyIOStatusRow)
                {
                    case DGV_SaftyIOStatusRow.RM_GOT_EStop:
                        {
                            if (m_RackMasters == null)
                                continue;

                            foreach (var rackMaster in m_RackMasters)
                            {
                                if (!string.IsNullOrEmpty(value))
                                    value += " / ";

                                string OnOff = rackMaster.Value.Status_GOTEStopSwitch ? "On" : "Off";
                                value += $"RM[ID = {rackMaster.Value.GetParam().ID}] = {OnOff}";
                            }

                            DGV_Cell_Status.Style.BackColor = value.Contains("On") ? Master.ErrorIntervalColor : Color.Lime;
                        }
                        break;
                    case DGV_SaftyIOStatusRow.RM_HP_EMO:
                        {
                            if (m_RackMasters == null)
                                continue;

                            foreach (var rackMaster in m_RackMasters)
                            {
                                if (!string.IsNullOrEmpty(value))
                                    value += " / ";

                                string OnOff = rackMaster.Value.Status_HPEStopSwitch ? "On" : "Off";
                                value += $"RM[ID = {rackMaster.Value.GetParam().ID}] = {OnOff}";
                            }

                            DGV_Cell_Status.Style.BackColor = value.Contains("On") ? Master.ErrorIntervalColor : Color.Lime;
                        }
                        break;
                    case DGV_SaftyIOStatusRow.RM_OP_EMO:
                        {
                            if (m_RackMasters == null)
                                continue;

                            foreach (var rackMaster in m_RackMasters)
                            {
                                if (!string.IsNullOrEmpty(value))
                                    value += " / ";

                                string OnOff = rackMaster.Value.Status_OPEStopSwitch ? "On" : "Off";
                                value += $"RM[ID = {rackMaster.Value.GetParam().ID}] = {OnOff}";
                            }

                            DGV_Cell_Status.Style.BackColor = value.Contains("On") ? Master.ErrorIntervalColor : Color.Lime;
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_HP_DoorOpen:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.HP_Door_Open);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                bool bHPDoorOpenStatus = Sensor_HPDoorOpen;
                                value = bHPDoorOpenStatus ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = bHPDoorOpenStatus ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_OP_DoorOpen:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.OP_Door_Open);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                bool bOPDoorOpenStatus = Sensor_OPDoorOpen;
                                value = bOPDoorOpenStatus ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = bOPDoorOpenStatus ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_HP_EMO_Pushing:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.HP_Inside_EMO);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mHPOutSide_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mHPOutSide_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_HP_EMO_Escape_Status:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.HP_Outside_EMO);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mHPInnerEscape_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mHPInnerEscape_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_OP_EMO_Pushing:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.OP_Outside_EMO);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mOPOutSide_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mOPOutSide_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_OP_EMO_Escape_Status:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.OP_Inside_EMO);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mOPInnerEscape_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mOPInnerEscape_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_HP_MasterKey_AutoMode_Status:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.HP_AutoManual_Select_Key);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                bool bHPMasterKeyStatus = Sensor_HPAutoKey;
                                value = bHPMasterKeyStatus ? "Auto" : "Manual";
                                DGV_Cell_Status.Style.BackColor = bHPMasterKeyStatus ? Color.Lime : Color.Orange;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Bypass_Relay:
                        {
                            bool bEnable = IsValidOutputItemMapping(MasterOutputItem.Door_Bypass_Relay_On);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                bool bDoorBypassRelayOnStatus = CMD_DoorOpen_Relay;
                                value = bDoorBypassRelayOnStatus ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = bDoorBypassRelayOnStatus ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Door_Open_Relay:
                        {
                            bool bEnable = IsValidOutputItemMapping(MasterOutputItem.Door_Open_Relay_On);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                bool bDoorOpenRaleyOnStatus = CMD_DoorBypass_Relay;
                                value = bDoorOpenRaleyOnStatus ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = bDoorOpenRaleyOnStatus ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_Maint_Door_Open:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.Maint_Door_Open) ||
                                            IsValidInputItemMapping(MasterInputItem.Maint_Door_Open2);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                bool bMaintDoorOpenStatus = Sensor_MaintDoorOpen || Sensor_MaintDoorOpen2;
                                value = bMaintDoorOpenStatus ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = bMaintDoorOpenStatus ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_Port_Handy_Touch_EMO:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.Port_DTP_EMO) && (IsValidInputItemMapping(MasterInputItem.Port_DTP_Mode1) || IsValidInputItemMapping(MasterInputItem.Port_DTP_Mode2));
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = mPortHandyTouch_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = mPortHandyTouch_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_HP_Handy_Touch_EMO:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.HP_DTP_EMO) && (IsValidInputItemMapping(MasterInputItem.HP_DTP_Mode1) || IsValidInputItemMapping(MasterInputItem.HP_DTP_Mode2));
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = mHPHandyTouch_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = mHPHandyTouch_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_OP_Handy_Touch_EMO:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.OP_DTP_EMO) && (IsValidInputItemMapping(MasterInputItem.OP_DTP_Mode1) || IsValidInputItemMapping(MasterInputItem.OP_DTP_Mode2));
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = mOPHandyTouch_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = mOPHandyTouch_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_CPS_Run:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.CPS_Run);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Sensor_CPSRun ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = Sensor_CPSRun ? Color.Lime : Master.ErrorIntervalColor;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_CPS_Fault:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.CPS_Fault);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Sensor_CPSError ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = Sensor_CPSError ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_MCUL_Fault:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.MCUL_Fault);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Sensor_MCULFault ? "On" : "Off";
                                DGV_Cell_Status.Style.BackColor = Sensor_MCULFault ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_Inner_EMO1:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.Inner_EMO_1);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mDieBankInnerEMO_EStop.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mDieBankInnerEMO_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_Inner_EMO2:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.Inner_EMO_2);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mDieBankInnerEMO_EStop2.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mDieBankInnerEMO_EStop2.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_Inner_EMO3:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.Inner_EMO_3);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mDieBankInnerEMO_EStop3.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mDieBankInnerEMO_EStop3.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;
                    case DGV_SaftyIOStatusRow.Master_Inner_EMO4:
                        {
                            bool bEnable = IsValidInputItemMapping(MasterInputItem.Inner_EMO_4);
                            if (!bEnable)
                            {
                                value = "Disable";
                                DGV_Cell_Status.Style.BackColor = Color.DarkGray;
                            }
                            else
                            {
                                value = Master.mDieBankInnerEMO_EStop4.GetEStopStateToStr();
                                DGV_Cell_Status.Style.BackColor = Master.mDieBankInnerEMO_EStop4.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            }
                        }
                        break;

                    
                }

                if ((string)DGV_Cell_Status.Value != value)
                    DGV_Cell_Status.Value = value;
            }

            if (DGV.CurrentCell != null)
                DGV.CurrentCell = null;
        }

        static public void Update_DGV_EquipmentAutoRunStatus(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_EquipmentStatusColumn.Equip:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Equipment"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Equipment");
                        break;
                    case (int)DGV_EquipmentStatusColumn.Auto:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Auto"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Auto");
                        break;
                    case (int)DGV_EquipmentStatusColumn.ControlMode:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ControlMode"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ControlMode");
                        break;
                    case (int)DGV_EquipmentStatusColumn.Power:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Power"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Power");
                        break;
                    case (int)DGV_EquipmentStatusColumn.Busy:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Busy"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Busy");
                        break;
                    case (int)DGV_EquipmentStatusColumn.Error:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ErrorStatus"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ErrorStatus");
                        break;
                    case (int)DGV_EquipmentStatusColumn.Control:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Control"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Control");
                        break;
                }
            }

            int EquipmentCount = (m_RackMasters?.Count ?? 0) + (m_Ports?.Count ?? 0);

            DataGridViewFunc.AutoRowSize(DGV, 30, 30, 100);

            if (DGV.Rows.Count != EquipmentCount)
            {
                DGV.Rows.Clear();

                for(int nCount = 0; nCount < EquipmentCount; nCount ++)
                {
                    DGV.Rows.Add();
                }
            }
            else
            {
                int nRowCount = 0;

                if (m_RackMasters == null)
                    return;

                foreach (var rackmaster in m_RackMasters)
                {
                    if (nRowCount >= DGV.Rows.Count)
                        break;

                    for(int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_EquipmentStatusColumn eDGV_EquipmentStatusColumn = (DGV_EquipmentStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;

                        switch(eDGV_EquipmentStatusColumn)
                        {
                            case DGV_EquipmentStatusColumn.Equip:
                                {
                                    Data = $"RackMaster [ID : {rackmaster.Value.GetParam().ID}]";
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Auto:
                                {
                                    Data = rackmaster.Value.Status_AutoMode ? "Auto" : "Manual";
                                    DGV_Cell.Style.BackColor = Data == "Auto" ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_EquipmentStatusColumn.ControlMode:
                                {
                                    Data = rackmaster.Value.m_eControlMode == Equipment.RackMaster.RackMaster.ControlMode.CIMMode ? "CIM" : "Master";
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Power:
                                {
                                    Data = rackmaster.Value.Status_ServoOn ? "On" : "Off";
                                    DGV_Cell.Style.BackColor = Data == "On" ? Color.Lime : Color.Yellow;
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Busy:
                                {
                                    Data = rackmaster.Value.IsBusy() ? "Busy" : "Idle";
                                    DGV_Cell.Style.BackColor = Data == "Busy" ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Error:
                                {
                                    Data = rackmaster.Value.IsAlarmState() ? "Error" : "None";
                                    DGV_Cell.Style.BackColor = Data == "Error" ? Master.ErrorIntervalColor : Color.White;
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Control:
                                {
                                    Data = "Go";
                                    DGV_Cell.Tag = rackmaster.Value;
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }

                    nRowCount++;
                }

                foreach (var port in m_Ports)
                {
                    if (nRowCount >= DGV.Rows.Count)
                        break;

                    for (int nColumnCount = 0; nColumnCount < DGV.Columns.Count; nColumnCount++)
                    {
                        DGV_EquipmentStatusColumn eDGV_EquipmentStatusColumn = (DGV_EquipmentStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Data = string.Empty;

                        switch (eDGV_EquipmentStatusColumn)
                        {
                            case DGV_EquipmentStatusColumn.Equip:
                                {
                                    if(!port.Value.IsEQPort())
                                        Data = $"Port [ID : {port.Value.GetParam().ID}] Type : {port.Value.GetPortTypeToStr(port.Value.GetParam().ePortType)} ({port.Value.GetOperationDirection()})";
                                    else
                                        Data = $"Port [ID : {port.Value.GetParam().ID}] Type : {port.Value.GetPortTypeToStr(port.Value.GetParam().ePortType)}";
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Auto:
                                {
                                    if (!port.Value.IsEQPort())
                                    {
                                        Data = port.Value.IsAutoControlRun() ? "Running" : "Idle";
                                        DGV_Cell.Style.BackColor = Data == "Running" ? Color.Lime : Color.White;
                                    }
                                    else
                                    {
                                        Data = "Running";
                                        DGV_Cell.Style.BackColor = Color.Lime;
                                    }
                                }
                                break;
                            case DGV_EquipmentStatusColumn.ControlMode:
                                {
                                    if (!port.Value.IsEQPort())
                                    {
                                        Data = port.Value.m_eControlMode == Equipment.Port.Port.ControlMode.CIMMode ? "CIM" : "Master";
                                    }
                                    else
                                    {
                                        Data = string.Empty;
                                    }
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Power:
                                {
                                    if (!port.Value.IsEQPort())
                                    {
                                        Data = port.Value.IsPortPowerOn() ? "On" : "Off";
                                        DGV_Cell.Style.BackColor = Data == "On" ? Color.Lime : Color.Yellow;
                                    }
                                    else
                                    {
                                        Data = string.Empty;
                                        DGV_Cell.Style.BackColor = Color.White;
                                    }
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Busy:
                                {
                                    Data = port.Value.IsPortBusy() ? "Busy" : "Idle";
                                    DGV_Cell.Style.BackColor = Data == "Busy" ? Color.Lime : Color.White;
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Error:
                                {
                                    if (!port.Value.IsEQPort())
                                    {
                                        Data = port.Value.GetAlarmLevel() == AlarmLevel.Error ? "Error" : "None";
                                        DGV_Cell.Style.BackColor = Data == "Error" ? Master.ErrorIntervalColor : Color.White;
                                    }
                                    else
                                    {
                                        Data = string.Empty;
                                        DGV_Cell.Style.BackColor = Color.White;
                                    }
                                }
                                break;
                            case DGV_EquipmentStatusColumn.Control:
                                {
                                    Data = "Go";
                                    DGV_Cell.Tag = port.Value;
                                }
                                break;
                        }

                        if ((string)DGV_Cell.Value != Data)
                            DGV_Cell.Value = Data;
                    }

                    nRowCount++;
                }
            }

            if (DGV.CurrentCell != null)
            {
                if (DGV.CurrentCell.ColumnIndex != (int)DGV_EquipmentStatusColumn.Control)
                    DGV.CurrentCell = null;
            }
        }

        static public void Update_DGV_CIMToMasterBitMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveBitMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveBitMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (Equipment.CIM.CIM.ReceiveBitMapIndex receiveMap in Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveBitMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value    = $"0x{(m_CIM.GetParam().RecvBitMapStartAddr + (int)receiveMap).ToString("x4")}";
                    Index_Cell.Value        = $"{(int)receiveMap} [0x{((int)receiveMap).ToString("x2")}]";
                    Name_Cell.Value         = receiveMap.ToString();

                    if (m_CIM.GetParam().RecvBitMapStartAddr == -1 ||
                        m_CIM_RecvBitMap.Length <= (int)receiveMap + m_CIM.GetParam().RecvBitMapStartAddr)
                    {
                        nRowIndex++;
                        continue;
                    }

                    bool value = m_CIM_RecvBitMap[(int)receiveMap + m_CIM.GetParam().RecvBitMapStartAddr];
                    Value_Cell.Value = $"{value}";
                    Value_Cell.Style.BackColor = value ? Color.Lime : Color.White;

                    nRowIndex++;
                }
            }
        }
        static public void Update_DGV_MasterToCIMBitMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(Equipment.CIM.CIM.SendBitMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(Equipment.CIM.CIM.SendBitMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (Equipment.CIM.CIM.SendBitMapIndex sendMap in Enum.GetValues(typeof(Equipment.CIM.CIM.SendBitMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(m_CIM.GetParam().SendBitMapStartAddr + (int)sendMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)sendMap} [0x{((int)sendMap).ToString("x2")}]";
                    Name_Cell.Value = sendMap.ToString();

                    if (m_CIM.GetParam().SendBitMapStartAddr == -1 ||
                        m_CIM_SendBitMap.Length <= (int)sendMap + m_CIM.GetParam().SendBitMapStartAddr)
                    {
                        nRowIndex++;
                        continue;
                    }

                    bool value = m_CIM_SendBitMap[(int)sendMap + m_CIM.GetParam().SendBitMapStartAddr];
                    Value_Cell.Value = $"{value}";
                    Value_Cell.Style.BackColor = value ? Color.Lime : Color.White;

                    nRowIndex++;
                }
            }
        }
        static public void Update_DGV_CIMToMasterWordMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveWordMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveWordMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (Equipment.CIM.CIM.ReceiveWordMapIndex receiveMap in Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveWordMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(m_CIM.GetParam().RecvWordMapStartAddr + (int)receiveMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)receiveMap} [0x{((int)receiveMap).ToString("x2")}]";
                    Name_Cell.Value = receiveMap.ToString();

                    if (m_CIM.GetParam().RecvWordMapStartAddr == -1 || 
                        m_CIM_RecvWordMap.Length <= (int)receiveMap + m_CIM.GetParam().RecvWordMapStartAddr)
                    {
                        nRowIndex++;
                        continue;
                    }

                    short value = Master.m_CIM_RecvWordMap[(int)receiveMap + m_CIM.GetParam().RecvWordMapStartAddr];

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
        static public void Update_DGV_MasterToCIMWordMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(Equipment.CIM.CIM.SendWordMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(Equipment.CIM.CIM.SendWordMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (Equipment.CIM.CIM.SendWordMapIndex sendMap in Enum.GetValues(typeof(Equipment.CIM.CIM.SendWordMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(m_CIM.GetParam().SendWordMapStartAddr + (int)sendMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)sendMap} [0x{((int)sendMap).ToString("x2")}]";
                    Name_Cell.Value = sendMap.ToString();

                    if (m_CIM.GetParam().SendWordMapStartAddr == -1 || 
                        m_CIM_SendWordMap.Length <= (int)sendMap + m_CIM.GetParam().SendWordMapStartAddr)
                    {
                        nRowIndex++;
                        continue;
                    }

                    short value = m_CIM_SendWordMap[(int)sendMap + m_CIM.GetParam().SendWordMapStartAddr];
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


        static public void Update_DGV_CPSStatus(ref DataGridView DGV)
        {
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

            if (DGV.Rows.Count != Enum.GetNames(typeof(DGV_CPSStatusInfoRow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_CPSStatusInfoRow)).Length; nCount++)
                {
                    DGV_CPSStatusInfoRow eDGV_CPSStatusInfoRow = (DGV_CPSStatusInfoRow)nCount;
                    switch (eDGV_CPSStatusInfoRow)
                    {
                        case DGV_CPSStatusInfoRow.TCPIPInfo:
                            DGV.Rows.Add(new string[] { "TCP/IP Info", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.TCPIPState:
                            DGV.Rows.Add(new string[] { "TCP/IP State", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.CPSStatus:
                            DGV.Rows.Add(new string[] { "CPS State", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.Voltage:
                            DGV.Rows.Add(new string[] { "Voltage", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.DC_Current:
                            DGV.Rows.Add(new string[] { "DC Current", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.IGBT_Current:
                            DGV.Rows.Add(new string[] { "IGBT Current", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.Track_Current:
                            DGV.Rows.Add(new string[] { "Track Current", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.OutputFreq:
                            DGV.Rows.Add(new string[] { "Output Frequency", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.Converter_Heatsink_Temp:
                            DGV.Rows.Add(new string[] { "Heatsink Temp", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.Covnerter_Inner_Temp:
                            DGV.Rows.Add(new string[] { "Inner Temp", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.Converter_Error_Code:
                            DGV.Rows.Add(new string[] { "Error Code", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.RequestCount:
                            DGV.Rows.Add(new string[] { "Request Count", string.Empty });
                            break;
                        case DGV_CPSStatusInfoRow.ReadCount:
                            DGV.Rows.Add(new string[] { "Read Count", string.Empty });
                            break;
                    }

                }
            }
            else
            {
                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_CPSStatusInfoRow)).Length; nCount++)
                {
                    const int DataColumnIndex = 1;
                    if (DataColumnIndex >= DGV.Columns.Count)
                        continue;

                    DGV_CPSStatusInfoRow eDGV_CPSStatusInfoRow = (DGV_CPSStatusInfoRow)nCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nCount].Cells[DataColumnIndex];
                    string value = string.Empty;

                    switch (eDGV_CPSStatusInfoRow)
                    {
                        case DGV_CPSStatusInfoRow.TCPIPInfo:
                            value = $"Port: {m_CPS.GetParam().ServerPort}";
                            break;
                        case DGV_CPSStatusInfoRow.TCPIPState:
                            value = m_CPS.IsConnected() ? "Connection" : "Disconnection";
                            DGV_Cell.Style.BackColor = m_CPS.IsConnected() ? Color.Lime : Color.White;
                            break;
                        case DGV_CPSStatusInfoRow.CPSStatus:
                            value = $"{m_CPS.eCPSStatus}";
                            DGV_Cell.Style.BackColor = m_CPS.eCPSStatus == CPS.CPSStatus.Warning ? Color.Orange : m_CPS.eCPSStatus == CPS.CPSStatus.Run ? Color.Lime : Master.ErrorIntervalColor;
                            break;
                        case DGV_CPSStatusInfoRow.Voltage:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.Voltage)} Vdc";
                            break;
                        case DGV_CPSStatusInfoRow.DC_Current:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.DC_Current)} Idc";
                            break;
                        case DGV_CPSStatusInfoRow.IGBT_Current:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.IGBT_Current)} Idc";
                            break;
                        case DGV_CPSStatusInfoRow.Track_Current:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.Track_Current)} Idc";
                            break;
                        case DGV_CPSStatusInfoRow.OutputFreq:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.OutputFreq)} kHz";
                            break;
                        case DGV_CPSStatusInfoRow.Converter_Heatsink_Temp:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.Converter_Heatsink_Temp)} °C";
                            break;
                        case DGV_CPSStatusInfoRow.Covnerter_Inner_Temp:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.Covnerter_Inner_Temp)} °C";
                            break;
                        case DGV_CPSStatusInfoRow.Converter_Error_Code:
                            value = $"{m_CPS.Get_CPS_Data(CPS.PacketStruct.Converter_Error_Code)}";
                            break;
                        case DGV_CPSStatusInfoRow.RequestCount:
                            value = $"{m_CPS.nRequestCount}";
                            break;
                        case DGV_CPSStatusInfoRow.ReadCount:
                            value = $"{m_CPS.nReceiveCount}";
                            break;
                    }


                    if ((string)DGV_Cell.Value != value)
                        DGV_Cell.Value = value;
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        static public void Update_DGV_CPSErrorInfo(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case 0:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_GenerateTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_GenerateTime");
                        break;
                    case 1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_List"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_List");
                        break;
                    case 2:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ErrorCode"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ErrorCode");
                        break;
                    case 3:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ClearTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ClearTime");
                        break;
                }
            }

            if (DGV.Rows.Count != Enum.GetNames(typeof(CPS.CPSAlarmWordMap)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(CPS.CPSAlarmWordMap)).Length; nCount++)
                {
                    CPS.CPSAlarmWordMap eCPSAlarmWordMap = (CPS.CPSAlarmWordMap)nCount;
                    DGV.Rows.Add(new string[] { string.Empty, $"{eCPSAlarmWordMap}", string.Empty, string.Empty });
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(CPS.CPSAlarmWordMap)).Length; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(CPS.CPSAlarmWordMap)).Length; nColumnCount++)
                    {
                        if (nColumnCount >= DGV.Columns.Count)
                            continue;

                        CPS.CPSAlarmWordMap eDGV_ErrorInfoRow = (CPS.CPSAlarmWordMap)nRowCount;
                        DGV_ErrorInfoColumn eDGV_ErrorInfoColumn = (DGV_ErrorInfoColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string value = string.Empty;


                        switch (eDGV_ErrorInfoColumn)
                        {
                            case DGV_ErrorInfoColumn.ErrorGenTime:
                                value = m_CPS.GetWordAlarm(eDGV_ErrorInfoRow).GenerateTime;
                                break;

                            case DGV_ErrorInfoColumn.ErrorIndex:
                                value = $"{eDGV_ErrorInfoRow}";
                                break;

                            case DGV_ErrorInfoColumn.ErrorCode:
                                value = $"0x{m_CPS.GetWordAlarm(eDGV_ErrorInfoRow).AlarmWord.ToString("x4")}";

                                DGV_Cell.Style.BackColor = value == "0x0000" ? Color.White : Master.ErrorIntervalColor;
                                break;

                            case DGV_ErrorInfoColumn.ErrorClearTime:
                                value = m_CPS.GetWordAlarm(eDGV_ErrorInfoRow).ClearTime;
                                break;
                        }

                        if ((string)DGV_Cell.Value != value)
                            DGV_Cell.Value = value;
                    }
                }
            }
        }
        static public void Update_DGV_CPSErrorDetailInfo(ref DataGridView DGV, int SelectedErrorGroup)
        {
            if (SelectedErrorGroup == -1)
                return;

            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case 0:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ErrorName"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ErrorName");
                        break;
                    case 1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Bit"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Bit");
                        break;
                    case 2:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Status"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Status");
                        break;
                }
            }

            List<int> DetailList = new List<int>();
            int StartErrorGroup = SelectedErrorGroup * 0x10;

            for (int nCount = 0; nCount < 0x10; nCount++)
            {
                if (Enum.IsDefined(typeof(CPS.CPSAlarmList), (StartErrorGroup + nCount)))
                    DetailList.Add((StartErrorGroup + nCount));
                else
                    DetailList.Add(-1);
            }

            if (DGV.Rows.Count != DetailList.Count)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < DetailList.Count; nCount++)
                {
                    DGV.Rows.Add();
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DataGridViewCell DGV_ErrorNameCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_ErrorBitCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_ErrorStatusCell = DGV.Rows[nRowCount].Cells[2];

                    short ErrorCode = m_CPS.GetWordAlarm(SelectedErrorGroup).AlarmWord;
                    string AlarmText = Enum.IsDefined(typeof(CPS.CPSAlarmList), DetailList[nRowCount]) ? $"{(CPS.CPSAlarmList)DetailList[nRowCount]}" : "Reserve";
                        //CPS.CPSAlarmList eCPSAlarm = (CPS.CPSAlarmList)DetailList[nRowCount];
                    //int BitPos = DetailList[nRowCount] - StartErrorGroup;
                    bool bEnable = (ErrorCode & (0x1 << nRowCount)) == (0x1 << nRowCount);

                    if ((string)DGV_ErrorNameCell.Value != $"{AlarmText}")
                        DGV_ErrorNameCell.Value = $"{AlarmText}";

                    if ((string)DGV_ErrorBitCell.Value != $"{nRowCount}")
                        DGV_ErrorBitCell.Value = $"{nRowCount}";

                    if ((string)DGV_ErrorStatusCell.Value != (bEnable ? "On" : "Off"))
                        DGV_ErrorStatusCell.Value = (bEnable ? "On" : "Off");

                    DGV_ErrorStatusCell.Style.BackColor = bEnable ? Master.ErrorIntervalColor : Color.White;
                }
            }
        }
        static public void Update_DGV_CPSByteMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(Equipment.CPS.CPS.PacketStruct)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(Equipment.CPS.CPS.PacketStruct)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (Equipment.CPS.CPS.PacketStruct packet in Enum.GetValues(typeof(Equipment.CPS.CPS.PacketStruct)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{((int)packet).ToString("x4")}";
                    Index_Cell.Value = $"{((int)packet)}";
                    Name_Cell.Value = packet.ToString();

                    object value = m_CPS.Get_CPS_Data(packet);
                    string valueText = $"{value} [{m_CPS.Get_CPS_Data_Array(packet)}]";

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
    
        static public void Update_DGV_AlarmList(ref DataGridView DGV)
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

            if (DGV.Rows.Count != Enum.GetValues(typeof(MasterAlarm)).Length)
            {
                DGV.Rows.Clear();
                foreach (MasterAlarm eMasterAlarm in Enum.GetValues(typeof(MasterAlarm)))
                {
                    int Index = (int)eMasterAlarm;
                    string Hex = $"0x{Index.ToString("x2")}";
                    string Name = GetMasterAlarmComment(eMasterAlarm);
                    string State = AlarmContains(eMasterAlarm) ? "On" : "Off";
                    DGV.Rows.Add(Index.ToString(), Hex, Name, State);
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < Enum.GetValues(typeof(MasterAlarm)).Length; nRowCount++)
                {
                    DataGridViewCell DGV_IndexCell = DGV.Rows[nRowCount].Cells[(int)DGV_AlarmListColumn.Index];
                    DataGridViewCell DGV_StateCell = DGV.Rows[nRowCount].Cells[(int)DGV_AlarmListColumn.State];
                    short Index = Convert.ToInt16(DGV_IndexCell.Value);

                    MasterAlarm eMasterAlarm = IndexToAlarmEnum(Index);
                    string State = AlarmContains(eMasterAlarm) ? "On" : "Off";
                    DGV_StateCell.Value = State;
                    DGV_StateCell.Style.BackColor = State == "On" ? Color.Red : Color.LightGray;
                }
            }
        }
    
        static public void Update_DGV_ErrorInfo(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case 0:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_GenerateTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_GenerateTime");
                        break;
                    case 1:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_List"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_List");
                        break;
                    case 2:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ErrorCode"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ErrorCode");
                        break;
                    case 3:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ClearTime"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ClearTime");
                        break;
                }
            }

            if (DGV.Rows.Count != Enum.GetNames(typeof(DGV_ErrorWordRow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_ErrorWordRow)).Length; nCount++)
                {
                    DGV_ErrorWordRow eDGV_ErrorWordRow = (DGV_ErrorWordRow)nCount;
                    DGV.Rows.Add(new string[] { string.Empty, $"{eDGV_ErrorWordRow}", string.Empty, string.Empty });
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(DGV_ErrorWordRow)).Length; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_ErrorInfoColumn)).Length; nColumnCount++)
                    {
                        if (nColumnCount >= DGV.Columns.Count)
                            continue;

                        DGV_ErrorWordRow eDGV_ErrorWordRow = (DGV_ErrorWordRow)nRowCount;
                        DGV_ErrorInfoColumn eDGV_ErrorInfoColumn = (DGV_ErrorInfoColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string value = string.Empty;


                        switch (eDGV_ErrorInfoColumn)
                        {
                            case DGV_ErrorInfoColumn.ErrorGenTime:
                                value = GetAlarmAt(nRowCount) != null ? GetAlarmAt(nRowCount).GenerateTime : string.Empty;
                                break;

                            case DGV_ErrorInfoColumn.ErrorIndex:
                                value = $"{eDGV_ErrorInfoColumn}";
                                break;

                            case DGV_ErrorInfoColumn.ErrorCode:
                                value = GetAlarmAt(nRowCount) != null ? $"0x{GetAlarmAt(nRowCount).AlarmWord.ToString("x4")}" : string.Empty;

                                DGV_Cell.Style.BackColor = value == "0x0000" ? Color.White : Master.ErrorIntervalColor;
                                break;

                            case DGV_ErrorInfoColumn.ErrorClearTime:
                                value = GetAlarmAt(nRowCount) != null ? GetAlarmAt(nRowCount).ClearTime : string.Empty;
                                break;
                        }

                        if ((string)DGV_Cell.Value != value)
                            DGV_Cell.Value = value;
                    }
                }
            }
        }
    }
}
