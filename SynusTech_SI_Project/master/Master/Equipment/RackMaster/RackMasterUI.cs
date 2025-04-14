using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Master.Equipment.RackMaster
{
    /// <summary>
    /// RackMasterUI.cs 는 Form에서 사용되는 UI 아이템의 색상, 변경 등 UI Item 반응에 대한 작성 영역
    /// </summary>
    /// 
    public partial class RackMaster
    {
        /// <summary>
        /// Frm_Monitoring 영역에 표시되는 에러 GridView의 Row List
        /// </summary>
        enum DGV_ErrorInfoRow
        {
            ErrorCode0,
            ErrorCode1,
            ErrorCode2,
            ErrorCode3,
            ErrorCode4,
            ErrorCode5,
            ErrorCode6,
            ErrorCode7,
            ErrorCode8,
            ErrorCode9,
            ErrorCode10,
            ErrorCode11,
            ErrorCode12
        }

        /// <summary>
        /// Frm_Monitoring 영역에 표시되는 에러 GridView의 Row List
        /// </summary>
        enum DGV_ErrorInfoColumn
        {
            ErrorGenTime,
            ErrorIndex,
            ErrorCode,
            ErrorClearTime
        }

        /// <summary>
        /// Frm_Monitoring 영역에 표시되는 상태 GridView의 Row List
        /// </summary>
        enum DGV_RMStatusInfoRow
        {
            AutoCycleRunningStatus,
            AutoTeachingStatus,
            Idle,
            Active,
            CassetteOn,
            CassettePos,
            X_AxisBusy,
            X_AxisPos,
            X_AxisPeakTorque,
            X_AxisAvrTorque,
            Z_AxisBusy,
            Z_AxisPos,
            Z_AxisPeakTorque,
            Z_AxisAvrTorque,
            A_AxisBusy,
            A_AxisPos,
            A_AxisPeakTorque,
            A_AxisAvrTorque,
            T_AxisBusy,
            T_AxisPos,
            T_AxisPeakTorque,
            T_AxisAvrTorque
        }

        /// <summary>
        /// Frm_Monitoring 영역에 표시되는 인터락 GridView의 Row List
        /// </summary>
        enum DGV_InterlockInfoRow
        {
            AutoModeStatus,
            AutoRunEnable,
            ManualStatus,
            ManualEnable,
            MaintMove,
            ServoOnStatus,
            ServoOnEnable,
            ServoOffStatus,
            ServoOffEnable,
            ErrorStatus,
            HomeDoneStatus,
            EmergencyStopStatus,
            RM_EStop,
            RM_GOT_EStop,
            RM_HP_EMO,
            RM_OP_EMO,
            RM_GOTMode,

            Master_HP_DoorOpen,
            Master_OP_DoorOpen,
            Master_HP_EMO_Pushing,
            Master_HP_EMO_Escape_Status,
            Master_OP_EMO_Pushing,
            Master_OP_EMO_Escape_Status,
            Master_HP_MasterKey_AutoMode_Status
        }

        /// <summary>
        /// STK 제어 화면의 Status GridView의 열
        /// </summary>
        enum DGV_RackMasterStatusColumn
        {
            AxisType,
            Servo,
            Home,
            Busy,
            PosCommand,
            ActualPos,
            ActualVel,
            ActualTorque
        }

        /// <summary>
        /// STK 제어 화면의 Sensor GridView의 열
        /// </summary>
        enum DGV_RackMasterSensorStatusColumn
        {
            ArmBaseSensor,
            TurnLeftSideSensor,
            TurnRightSideSensor
        }

        /// <summary>
        /// STK 제어 화면의 Regulator GridView의 열
        /// </summary>
        enum DGV_RackMasterRegulatorStatusColumn
        {
            InputVoltage,
            OutputVoltage,
            OutputCurrent,
            PickupTemp,
            HeatsinkTemp,
            InnerTemp
        }

        /// <summary>
        /// STK 제어 화면의 PIO GridView의 열
        /// </summary>
        enum DGV_RackMasterPIOStatusColumn
        {
            CIM_To_RackMaster,
            RackMaster_To_CIM
        }

        /// <summary>
        /// STK 상태 표시 라벨의 출력 항목
        /// </summary>
        public enum RackMasterInfoType
        {
            Simple,
            Normal,
            Detail,
            Map
        }
        public void Update_Lbl_RackMasterInfoLabel(ref Label lbl, RackMasterInfoType eRackMasterInfoType)
        {
            bool bConnection = IsConnected();

            if (eRackMasterInfoType == RackMasterInfoType.Simple)
            {
                string InfoText = string.Empty;
                LabelFunc.SetText(lbl, InfoText);
            }
            else if (eRackMasterInfoType == RackMasterInfoType.Normal)
            {
                string InfoText = string.Empty;
                LabelFunc.SetText(lbl, InfoText);
            }
            else if (eRackMasterInfoType == RackMasterInfoType.Detail)
            {
                string InfoText = $"RackMaster[ID:{GetParam().ID}] ({(m_eControlMode == ControlMode.CIMMode ? "C Mode" : "M Mode")})\n" +
                                $"TCP/IP State : {(bConnection ? "Connection" : "Disconnection")}\n" +
                                $"Power : {(Status_ServoOn ? "On" : "Off")} ({(IsBusy() ? "Busy" : "Idle")})\n" +
                                $"Mode : {(Status_AutoMode ? "Auto" : "Manual")}\n" +
                                $"Access/From/To : {Status_STK_To_CIM_AccessID}/{Status_STK_To_CIM_FromID}/{Status_STK_To_CIM_ToID}\n" +
                                $"Error : {(Status_Error ? "Error" : "None")}";

                LabelFunc.SetText(lbl, InfoText);
            }
            else if (eRackMasterInfoType == RackMasterInfoType.Map)
            {
                double ProgressPercent = ((double)m_CycleProgress / (double)m_CycleCount) * 100.0;
                if (double.IsNaN(ProgressPercent))
                    ProgressPercent = 0;

                string InfoText = $"RackMaster[ID:{GetParam().ID}] ({(m_eControlMode == ControlMode.CIMMode ? "C Mode" : "M Mode")})\n" +
                                $"TCP/IP Info : {GetParam().ServerIP}, {GetParam().ServerPort}\n" +
                                $"TCP/IP State : {(bConnection ? "Connection" : "Disconnection")}\n" +
                                $"Power : {(Status_ServoOn ? "On" : "Off")} ({(IsBusy() ? "Busy" : "Idle")})\n" +
                                $"Mode : {(Status_AutoMode ? "Auto" : "Manual")}\n" +
                                $"Access/From/To : {Status_STK_To_CIM_AccessID}/{Status_STK_To_CIM_FromID}/{Status_STK_To_CIM_ToID}\n" +
                                $"Cycle Time :{StopWatchFunc.GetRunningTime(m_CycleRunningTime)}\n" +
                                $"Cycle Step : {m_eCycleControlStep}\n" +
                                $"Cycle Progress : {m_CycleProgress} / {m_CycleCount} ({ProgressPercent.ToString("0.0")}%)\n" +
                                $"Cycle Error : {m_eCycleControlErrorCode}\n" +
                                $"Error : {(Status_Error ? "Error" : "None")}";

                LabelFunc.SetText(lbl, InfoText);
            }

            LabelFunc.SetBackColor(lbl, !bConnection ? Color.DarkGray : IsAlarmState() == true ? Master.ErrorIntervalColor : Status_AutoMode ? Color.Lime : Status_ServoOn ? Color.Orange : Color.White);
            LabelFunc.SetVisible(lbl, true);
        }
        public string GetFocusButtonStr()
        {
            string InfoText = $"RackMaster [ID : {GetParam().ID}] ({(m_eControlMode == ControlMode.CIMMode ? "CIM Mode" : "Master Mode")})";
            return InfoText;
        }
        public void Update_Btn_AutoRun(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection);
            ButtonFunc.SetBackColor(btn, !bConnection ? Color.DarkGray : Status_AutoMode ? Color.Lime : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AutoRun"));
        }
        public void Update_Btn_AutoStop(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection);
            ButtonFunc.SetBackColor(btn, !bConnection ? Color.DarkGray : !Status_AutoMode ? Color.Orange : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AutoStop"));
        }
        public void Update_Btn_PowerOn(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
            ButtonFunc.SetBackColor(btn, !bConnection || LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : Status_ServoOn ? Color.Lime : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_PowerOn"));
        }
        public void Update_Btn_PowerOff(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
            ButtonFunc.SetBackColor(btn, !bConnection || LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : Status_ServoOff ? Color.Orange : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_PowerOff"));
        }
        public void Update_Btn_CIMMode(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
            ButtonFunc.SetBackColor(btn, !bConnection || LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : m_eControlMode == ControlMode.CIMMode ? Color.Lime : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_CIMMode"));
        }
        public void Update_Btn_MasterMode(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection && LogIn.GetLogInLevel() >= LogIn.LogInLevel.Maint);
            ButtonFunc.SetBackColor(btn, !bConnection || LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint ? Color.DarkGray : m_eControlMode == ControlMode.MasterMode ? Color.Lime : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_MasterMode"));
        }
        public void Update_Btn_RMEStop(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection);
            ButtonFunc.SetBackColor(btn, !bConnection ? Color.DarkGray : CMD_EmergencyStop_REQ ? Master.ErrorIntervalColor : Color.White);
            ButtonFunc.SetText(btn, CMD_EmergencyStop_REQ ? SynusLangPack.GetLanguage("Btn_EStopRelease") : SynusLangPack.GetLanguage("Btn_RMEStop"));
        }
        public void Update_Btn_RMAlarmClear(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetBackColor(btn, !bConnection ? Color.DarkGray : IsAlarmState() == true ? Master.ErrorIntervalColor : Color.White);
            ButtonFunc.SetEnable(btn, !bConnection ? false : IsAlarmState() == true ? true : false);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_AlarmClear"));
        }
        public void Update_Btn_CycleRun(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection && !IsAutoCycleRun());
            ButtonFunc.SetVisible(btn, m_eControlMode == ControlMode.MasterMode ? true : false);
            ButtonFunc.SetBackColor(btn, !bConnection || IsAutoCycleRun() ? Color.DarkGray : Color.White);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_CycleRun"));
        }
        public void Update_Btn_CycleStop(ref Button btn)
        {
            bool bConnection = IsConnected();
            ButtonFunc.SetEnable(btn, bConnection && IsAutoCycleRun());
            ButtonFunc.SetVisible(btn, m_eControlMode == ControlMode.MasterMode ? true : false);
            ButtonFunc.SetBackColor(btn, !bConnection || !IsAutoCycleRun() ? Color.DarkGray : Color.Orange);
            ButtonFunc.SetText(btn, SynusLangPack.GetLanguage("Btn_CycleStop"));
        }

        public void Update_GBx_CycleGroup(ref GroupBox gbx)
        {
            GroupBoxFunc.SetVisible(gbx, m_eControlMode == ControlMode.MasterMode ? true : false);
            GroupBoxFunc.SetText(gbx, SynusLangPack.GetLanguage("GorupBox_AutoCycleTest"));
        }
        public void Update_Lbl_RackMasterTitleID(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"RM {GetParam().ID}");
        }
        public void Update_Lbl_RackMasterID(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{GetParam().ID}");
        }
        public void Update_Lbl_RackMasterOnline(ref Label lbl)
        {
            LabelFunc.SetText(lbl, IsConnected() ? $"Online" : "Offline");
            LabelFunc.SetBackColor(lbl, IsConnected() ? Color.Lime : Master.ErrorIntervalColor);
        }
        public void Update_Lbl_AutoStatus(ref Label lbl)
        {
            LabelFunc.SetText(lbl, Status_AutoMode ? $"Running" : $"Idle");
        }
        public void Update_Lbl_AutoStep(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.AutoStep_Number)}");
        }
        public void Update_Lbl_FromID(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"From ID : {Status_STK_To_CIM_FromID}");
        }
        public void Update_Lbl_ToID(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"To ID : {Status_STK_To_CIM_ToID}");
        }
        public void Update_Lbl_AccessID(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"Access ID : {Status_STK_To_CIM_AccessID}");
        }
        public void Update_Lbl_CarrierID(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CassetteID_PIO_Word_0)}");
        }
        public void Update_Lbl_Alarm(ref Label lbl)
        {
            LabelFunc.SetText(lbl, Status_Error ? "Error" : string.Empty);
            LabelFunc.SetBackColor(lbl, Status_Error ? Master.ErrorIntervalColor : Color.White);
        }
        public void Update_Lbl_AlarmText(ref Label lbl)
        {
            LabelFunc.SetText(lbl, GetAlarmCount() == "None" ? string.Empty : $"{GetAlarmCount()}");
        }
        public void Update_Lbl_AutoCycleStep(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoCycleStep")} : {m_eCycleControlStep}");
        }
        public void Update_Lbl_AutoCycleError(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoCycleError")} : {m_eCycleControlErrorCode}");
        }
        public void Update_Lbl_AutoCycleProgressPercent(ref Label lbl)
        {
            double ProgressPercent = ((double)m_CycleProgress / (double)m_CycleCount) * 100.0;
            if (double.IsNaN(ProgressPercent))
                ProgressPercent = 0;

            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoCycleProgressPercent")} : {m_CycleProgress} / {m_CycleCount} ({ProgressPercent.ToString("0.0")}%)");
        }
        public void Update_Lbl_AutoCycleProgressTime(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoCycleTime")} : {StopWatchFunc.GetRunningTime(m_CycleRunningTime)}");
        }
        public void Update_Lbl_AutoCyclePIOProgressTime(ref Label lbl)
        {
            string AccessID = Convert.ToString(Status_STK_To_CIM_AccessID);

            if (Master.m_Ports.ContainsKey(AccessID))
            {
                LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_PIOProgressTime")} : {Master.m_Ports[AccessID].Watchdog_GetProgressTime(Port.Port.WatchdogList.RackMaster_PIO_Timer)}");
            }
            else
                LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_PIOProgressTime")} : None");
        }

        public void Update_Lbl_AutoTeachingStep(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoTeachingStep")} : {m_eAutoTeachingStep}");
        }
        public void Update_Lbl_AutoTeachingError(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoTeachingError")} : {m_eAutoTeachingErrorCode}");
        }
        public void Update_Lbl_AutoTeachingProgressPercent(ref Label lbl)
        {
            double ProgressPercent = ((double)m_AutoTeachingProgress / (double)m_AutoTeachingCount) * 100.0;
            if (double.IsNaN(ProgressPercent))
                ProgressPercent = 0;

            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoTeachingProgressPercent")} : {m_AutoTeachingProgress} / {m_AutoTeachingCount} ({ProgressPercent.ToString("0.0")}%)");
        }
        public void Update_Lbl_AutoTeachingProgressTime(ref Label lbl)
        {
            LabelFunc.SetText(lbl, $"{SynusLangPack.GetLanguage("Label_AutoTeachingTime")} : {StopWatchFunc.GetRunningTime(m_TeachingRunningTime)}");
        }


        public void Update_DGV_RMStatusInfo(ref DataGridView DGV)
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

            if (DGV.Rows.Count != Enum.GetNames(typeof(DGV_RMStatusInfoRow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_RMStatusInfoRow)).Length; nCount++)
                {
                    DGV_RMStatusInfoRow eDGV_RMStatusInfoRow = (DGV_RMStatusInfoRow)nCount;
                    switch (eDGV_RMStatusInfoRow)
                    {
                        case DGV_RMStatusInfoRow.AutoCycleRunningStatus:
                            DGV.Rows.Add(new string[] { "Auto Cycle", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.AutoTeachingStatus:
                            DGV.Rows.Add(new string[] { "Auto Teaching", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.Idle:
                            DGV.Rows.Add(new string[] { "Idle", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.Active:
                            DGV.Rows.Add(new string[] { "Active", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.CassetteOn:
                            DGV.Rows.Add(new string[] { "Cassette On", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.CassettePos:
                            DGV.Rows.Add(new string[] { "Cassette Valid Pos", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.X_AxisBusy:
                            DGV.Rows.Add(new string[] { "X Axis Busy", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.X_AxisPos:
                            DGV.Rows.Add(new string[] { "X Axis Pos[mm]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.X_AxisPeakTorque:
                            DGV.Rows.Add(new string[] { "X Axis Peak Torque[%]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.X_AxisAvrTorque:
                            DGV.Rows.Add(new string[] { "X Axis Avr Torque[%]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisBusy:
                            DGV.Rows.Add(new string[] { "Z Axis Busy", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisPos:
                            DGV.Rows.Add(new string[] { "Z Axis Pos[mm]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisPeakTorque:
                            DGV.Rows.Add(new string[] { "Z Axis Peak Torque[%]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisAvrTorque:
                            DGV.Rows.Add(new string[] { "Z Axis Avr Torque[%]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.A_AxisBusy:
                            DGV.Rows.Add(new string[] { "A Axis Busy", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.A_AxisPos:
                            DGV.Rows.Add(new string[] { "A Axis Pos[mm]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.A_AxisPeakTorque:
                            DGV.Rows.Add(new string[] { "A Axis Peak Torque[%]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.A_AxisAvrTorque:
                            DGV.Rows.Add(new string[] { "A Axis Avr Torque[%]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.T_AxisBusy:
                            DGV.Rows.Add(new string[] { "T Axis Busy", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.T_AxisPos:
                            DGV.Rows.Add(new string[] { "T Axis Pos[°]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.T_AxisPeakTorque:
                            DGV.Rows.Add(new string[] { "T Axis Peak Torque[%]", string.Empty });
                            break;
                        case DGV_RMStatusInfoRow.T_AxisAvrTorque:
                            DGV.Rows.Add(new string[] { "T Axis Avr Torque[%]", string.Empty });
                            break;
                    }

                }
            }
            else
            {
                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_RMStatusInfoRow)).Length; nCount++)
                {
                    const int DataColumnIndex = 1;
                    if (DataColumnIndex >= DGV.Columns.Count)
                        continue;

                    DGV_RMStatusInfoRow eDGV_RMStatusInfoRow = (DGV_RMStatusInfoRow)nCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nCount].Cells[DataColumnIndex];
                    string value = string.Empty;

                    switch (eDGV_RMStatusInfoRow)
                    {
                        case DGV_RMStatusInfoRow.AutoCycleRunningStatus:
                            value = IsAutoCycleRun() ? "Running" : "Idle";
                            DGV_Cell.Style.BackColor = Status_AutoMode ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.AutoTeachingStatus:
                            value = IsAutoTeachingRun() ? "Running" : "Idle";
                            DGV_Cell.Style.BackColor = Status_AutoMode ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.Idle:
                            value = Status_IDLE ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_IDLE ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.Active:
                            value = Status_Active ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_Active ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.CassetteOn:
                            value = Status_CassetteOn ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_CassetteOn ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.CassettePos:
                            value = Status_CassettePos ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_CassettePos ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.X_AxisBusy:
                            value = IsAxisBusy(AxisType.X_Axis) ? "Busy" : "Idle";
                            DGV_Cell.Style.BackColor = IsAxisBusy(AxisType.X_Axis) ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.X_AxisPos:
                            value = ((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentPosition_0)).ToString("0.0");
                            break;
                        case DGV_RMStatusInfoRow.X_AxisPeakTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_Peak_Torque)).ToString();
                            break;
                        case DGV_RMStatusInfoRow.X_AxisAvrTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_Average_Torque)).ToString();
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisBusy:
                            value = IsAxisBusy(AxisType.Z_Axis) ? "Busy" : "Idle";
                            DGV_Cell.Style.BackColor = IsAxisBusy(AxisType.Z_Axis) ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisPos:
                            value = ((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentPosition_0)).ToString("0.0");
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisPeakTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_Peak_Torque)).ToString();
                            break;
                        case DGV_RMStatusInfoRow.Z_AxisAvrTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_Average_Torque)).ToString();
                            break;
                        case DGV_RMStatusInfoRow.A_AxisBusy:
                            value = IsAxisBusy(AxisType.A_Axis) ? "Busy" : "Idle";
                            DGV_Cell.Style.BackColor = IsAxisBusy(AxisType.A_Axis) ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.A_AxisPos:
                            value = ((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_CurrentPosition_0)).ToString("0.0");
                            break;
                        case DGV_RMStatusInfoRow.A_AxisPeakTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_Peak_Torque)).ToString();
                            break;
                        case DGV_RMStatusInfoRow.A_AxisAvrTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_Average_Torque)).ToString();
                            break;
                        case DGV_RMStatusInfoRow.T_AxisBusy:
                            value = IsAxisBusy(AxisType.T_Axis) ? "Busy" : "Idle";
                            DGV_Cell.Style.BackColor = IsAxisBusy(AxisType.T_Axis) ? Color.Lime : Color.White;
                            break;
                        case DGV_RMStatusInfoRow.T_AxisPos:
                            value = ((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentPosition_0)).ToString("0.0");
                            break;
                        case DGV_RMStatusInfoRow.T_AxisPeakTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_Peak_Torque)).ToString();
                            break;
                        case DGV_RMStatusInfoRow.T_AxisAvrTorque:
                            value = ((short)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_Average_Torque)).ToString();
                            break;
                    }


                    if ((string)DGV_Cell.Value != value)
                        DGV_Cell.Value = value;
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_ErrorInfo(ref DataGridView DGV)
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

            if (DGV.Rows.Count != Enum.GetNames(typeof(DGV_ErrorInfoRow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_ErrorInfoRow)).Length; nCount++)
                {
                    DGV_ErrorInfoRow eDGV_ErrorInfoRow = (DGV_ErrorInfoRow)nCount;
                    DGV.Rows.Add(new string[] { string.Empty, $"{eDGV_ErrorInfoRow}", string.Empty, string.Empty });
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


                        switch (eDGV_ErrorInfoColumn)
                        {
                            case DGV_ErrorInfoColumn.ErrorGenTime:
                                value = GetAlarmAt(nRowCount) != null ? GetAlarmAt(nRowCount).GenerateTime : string.Empty;
                                break;

                            case DGV_ErrorInfoColumn.ErrorIndex:
                                value = $"{eDGV_ErrorInfoRow}";
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
        public void Update_DGV_ErrorDetailInfo(ref DataGridView DGV, int SelectedErrorGroup)
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

            for(int nCount = 0; nCount < 0x10; nCount ++)
            {
                if (Enum.IsDefined(typeof(RackMasterAlarmList), (StartErrorGroup + nCount)))
                    DetailList.Add((StartErrorGroup + nCount));
            }

            if (DGV.Rows.Count != DetailList.Count)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < DetailList.Count; nCount++)
                {
                    DGV.Rows.Add();
                    //RackMasterAlarmList eRackMasterAlarmList = (RackMasterAlarmList)DetailList[nCount];
                    //DGV.Rows.Add(new string[] { $"{eRackMasterAlarmList}", $"{DetailList[nCount] - StartErrorGroup}", string.Empty });
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    DataGridViewCell DGV_ErrorNameCell = DGV.Rows[nRowCount].Cells[0];
                    DataGridViewCell DGV_ErrorBitCell = DGV.Rows[nRowCount].Cells[1];
                    DataGridViewCell DGV_ErrorStatusCell = DGV.Rows[nRowCount].Cells[2];

                    int ErrorWordIndex = (int)SendWordMapIndex.ErrorWord_0 + SelectedErrorGroup;
                    short ErrorCode = (short)Get_RackMaster_2_CIM_Word_Data((SendWordMapIndex)ErrorWordIndex);
                    RackMasterAlarmList eRackMasterAlarmList = (RackMasterAlarmList)DetailList[nRowCount];
                    int BitPos = DetailList[nRowCount] - StartErrorGroup;
                    bool bEnable = (ErrorCode & (0x1 << BitPos)) == (0x1 << BitPos);

                    if ((string)DGV_ErrorNameCell.Value != $"{eRackMasterAlarmList}")
                        DGV_ErrorNameCell.Value = $"{eRackMasterAlarmList}";

                    if ((string)DGV_ErrorBitCell.Value != $"{BitPos}")
                        DGV_ErrorBitCell.Value = $"{BitPos}";

                    if ((string)DGV_ErrorStatusCell.Value != (bEnable ? "On" : "Off"))
                        DGV_ErrorStatusCell.Value = (bEnable ? "On" : "Off");

                    DGV_ErrorStatusCell.Style.BackColor = bEnable ? Master.ErrorIntervalColor : Color.White;
                }
            }
        }
        public void Update_DGV_InterlockInfo(ref DataGridView DGV)
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

            if (DGV.Rows.Count != Enum.GetNames(typeof(DGV_InterlockInfoRow)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_InterlockInfoRow)).Length; nCount++)
                {
                    DGV_InterlockInfoRow eDGV_ErrorInfoRow = (DGV_InterlockInfoRow)nCount;
                    switch (eDGV_ErrorInfoRow)
                    {
                        case DGV_InterlockInfoRow.AutoModeStatus:
                            DGV.Rows.Add(new string[] { "Auto Mode Status", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.AutoRunEnable:
                            DGV.Rows.Add(new string[] { "Auto Mode Enable", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.ManualStatus:
                            DGV.Rows.Add(new string[] { "Manual Mode Status", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.ManualEnable:
                            DGV.Rows.Add(new string[] { "Manual Mode Enable", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.MaintMove:
                            DGV.Rows.Add(new string[] { "Maint Move Status", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.ServoOnStatus:
                            DGV.Rows.Add(new string[] { "Servo On Status", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.ServoOnEnable:
                            DGV.Rows.Add(new string[] { "Servo On Enable", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.ServoOffStatus:
                            DGV.Rows.Add(new string[] { "Servo Off Status", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.ServoOffEnable:
                            DGV.Rows.Add(new string[] { "Servo Off Enable", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.ErrorStatus:
                            DGV.Rows.Add(new string[] { "Error Status", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.HomeDoneStatus:
                            DGV.Rows.Add(new string[] { "Home Done Status", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.EmergencyStopStatus:
                            DGV.Rows.Add(new string[] { "Emergency Stop", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.RM_EStop:
                            DGV.Rows.Add(new string[] { "RM E-Stop", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.RM_GOT_EStop:
                            DGV.Rows.Add(new string[] { "RM GOT-Stop", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.RM_HP_EMO:
                            DGV.Rows.Add(new string[] { "RM HP EMO", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.RM_OP_EMO:
                            DGV.Rows.Add(new string[] { "RM OP EMO", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.RM_GOTMode:
                            DGV.Rows.Add(new string[] { "RM GOT Mode", string.Empty });
                            break;

                        case DGV_InterlockInfoRow.Master_HP_DoorOpen:
                            DGV.Rows.Add(new string[] { "Master HP DoorOpen", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.Master_OP_DoorOpen:
                            DGV.Rows.Add(new string[] { "Master OP DoorOpen", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.Master_HP_EMO_Pushing:
                            DGV.Rows.Add(new string[] { "Master HP EMO", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.Master_HP_EMO_Escape_Status:
                            DGV.Rows.Add(new string[] { "Master HP Escape", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.Master_OP_EMO_Pushing:
                            DGV.Rows.Add(new string[] { "Master OP EMO", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.Master_OP_EMO_Escape_Status:
                            DGV.Rows.Add(new string[] { "Master OP Escape", string.Empty });
                            break;
                        case DGV_InterlockInfoRow.Master_HP_MasterKey_AutoMode_Status:
                            DGV.Rows.Add(new string[] { "Master HP Key", string.Empty });
                            break;

                    }

                }
            }
            else
            {
                for (int nCount = 0; nCount < Enum.GetNames(typeof(DGV_InterlockInfoRow)).Length; nCount++)
                {
                    const int DataColumnIndex = 1;
                    if (DataColumnIndex >= DGV.Columns.Count)
                        continue;

                    DGV_InterlockInfoRow eDGV_InterlockInfoRow = (DGV_InterlockInfoRow)nCount;
                    DataGridViewCell DGV_Cell = DGV.Rows[nCount].Cells[DataColumnIndex];
                    string value = string.Empty;

                    switch (eDGV_InterlockInfoRow)
                    {
                        case DGV_InterlockInfoRow.AutoModeStatus:
                            value = Status_AutoMode ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_AutoMode ? Color.Lime : Color.White;
                            break;
                        case DGV_InterlockInfoRow.AutoRunEnable:
                            value = Status_AutoModeReady ? "Enable" : "Disable";
                            DGV_Cell.Style.BackColor = Status_AutoModeReady ? Color.Lime : Color.Orange;
                            break;
                        
                        case DGV_InterlockInfoRow.ManualStatus:
                            value = Status_ManualMode ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_ManualMode ? Color.Orange : Color.White;
                            break;
                        case DGV_InterlockInfoRow.ManualEnable:
                            value = Status_ManualModeEnable ? "Enable" : "Disable";
                            DGV_Cell.Style.BackColor = Status_ManualModeEnable ? Color.Lime : Color.Orange;
                            break;
                        case DGV_InterlockInfoRow.MaintMove:
                            value = Status_MaintMove ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_MaintMove ? Color.Lime : Color.White;
                            break;
                        case DGV_InterlockInfoRow.ServoOnStatus:
                            value = Status_ServoOn ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_ServoOn ? Color.Lime : Color.White;
                            break;
                        
                        case DGV_InterlockInfoRow.ServoOnEnable:
                            value = Status_ServoOnEnable ? "Enable" : "Disable";
                            DGV_Cell.Style.BackColor = Status_ServoOnEnable ? Color.Lime : Color.Orange;
                            break;
                        case DGV_InterlockInfoRow.ServoOffStatus:
                            value = Status_ServoOff ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_ServoOff ? Color.Orange : Color.White;
                            break;
                        case DGV_InterlockInfoRow.ServoOffEnable:
                            value = Status_ServoOffEnable ? "Enable" : "Disable";
                            DGV_Cell.Style.BackColor = Status_ServoOffEnable ? Color.Lime : Color.Orange;
                            break;
                        case DGV_InterlockInfoRow.ErrorStatus:
                            value = Status_Error ? "Error" : "None";
                            DGV_Cell.Style.BackColor = Status_Error ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.HomeDoneStatus:
                            value = Status_HomeDone ? "Done" : "Not Homed";
                            DGV_Cell.Style.BackColor = Status_HomeDone ? Color.Lime : Master.ErrorIntervalColor;
                            break;
                        case DGV_InterlockInfoRow.EmergencyStopStatus:
                            value = CMD_EmergencyStop_REQ ? "Emergency" : "None";
                            DGV_Cell.Style.BackColor = CMD_EmergencyStop_REQ ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.RM_EStop:
                            value = Status_HWEStopSwitch ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_HWEStopSwitch ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.RM_GOT_EStop:
                            value = Status_GOTEStopSwitch ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_GOTEStopSwitch ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.RM_HP_EMO:
                            value = Status_HPEStopSwitch ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_HPEStopSwitch ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.RM_OP_EMO:
                            value = Status_OPEStopSwitch ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_OPEStopSwitch ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.RM_GOTMode:
                            value = Status_GOTMode ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Status_GOTMode ? Color.Orange : Color.Lime;
                            break;

                        case DGV_InterlockInfoRow.Master_HP_DoorOpen:
                            value = Master.Sensor_HPDoorOpen ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Master.Sensor_HPDoorOpen ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.Master_OP_DoorOpen:
                            value = Master.Sensor_OPDoorOpen ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Master.Sensor_OPDoorOpen ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.Master_HP_EMO_Pushing:
                            value = Master.mHPOutSide_EStop.GetEStopStateToStr();
                            DGV_Cell.Style.BackColor = Master.mHPOutSide_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.Master_HP_EMO_Escape_Status:
                            value = Master.mHPInnerEscape_EStop.GetEStopStateToStr();
                            DGV_Cell.Style.BackColor = Master.mHPInnerEscape_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.Master_OP_EMO_Pushing:
                            value = Master.mOPOutSide_EStop.GetEStopStateToStr();
                            DGV_Cell.Style.BackColor = Master.mOPOutSide_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.Master_OP_EMO_Escape_Status:
                            value = Master.mOPInnerEscape_EStop.GetEStopStateToStr();
                            DGV_Cell.Style.BackColor = Master.mOPInnerEscape_EStop.IsEStop() ? Master.ErrorIntervalColor : Color.Lime;
                            break;
                        case DGV_InterlockInfoRow.Master_HP_MasterKey_AutoMode_Status:
                            value = Master.Sensor_HPAutoKey ? "Auto" : "Manual";
                            DGV_Cell.Style.BackColor = Master.Sensor_HPAutoKey ? Color.Lime : Color.Orange;
                            break;
                    }

                    if ((string)DGV_Cell.Value != value)
                        DGV_Cell.Value = value;
                }

                if (DGV.CurrentRow != null || DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }

        public void Update_DGV_RMMotionStatus(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_RackMasterStatusColumn.AxisType:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Axis"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Axis");
                        break;
                    case (int)DGV_RackMasterStatusColumn.Servo:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Servo"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Servo");
                        break;
                    case (int)DGV_RackMasterStatusColumn.Home:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Home"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Home");
                        break;
                    case (int)DGV_RackMasterStatusColumn.Busy:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_Busy"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_Busy");
                        break;
                    case (int)DGV_RackMasterStatusColumn.PosCommand:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_PosCommand"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_PosCommand");
                        break;
                    case (int)DGV_RackMasterStatusColumn.ActualPos:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ActualPos"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ActualPos");
                        break;
                    case (int)DGV_RackMasterStatusColumn.ActualVel:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ActualVel"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ActualVel");
                        break;
                    case (int)DGV_RackMasterStatusColumn.ActualTorque:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ActualTorque"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ActualTorque");
                        break;
                }
            }

            if (DGV.Rows.Count != Enum.GetNames(typeof(AxisType)).Length)
            {
                DGV.Rows.Clear();

                for (int nCount = 0; nCount < Enum.GetNames(typeof(AxisType)).Length; nCount++)
                {
                    string AxisType = $"{(AxisType)nCount}";
                    string ServoOn = string.Empty;
                    string HomeDone = string.Empty;
                    string Op = string.Empty;
                    string PosCommand = string.Empty;
                    string ActualPos = string.Empty;
                    string ActualVel = string.Empty;
                    string ActualTorque = string.Empty;

                    DGV.Rows.Add(new string[] { $"{AxisType}",
                                                        $"{ServoOn}",
                                                        $"{HomeDone}",
                                                        $"{Op}",
                                                        $"{PosCommand}",
                                                        $"{ActualPos}",
                                                        $"{ActualVel}",
                                                        $"{ActualTorque}"});
                }
            }
            else
            {
                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(AxisType)).Length; nRowCount++)
                {
                    AxisType eAxisType = (AxisType)nRowCount;

                    for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_RackMasterStatusColumn)).Length; nColumnCount++)
                    {
                        if (nColumnCount >= DGV.Columns.Count)
                            continue;

                        DGV_RackMasterStatusColumn eDGV_RackMasterStatusColumn = (DGV_RackMasterStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Value = string.Empty;

                        switch (eDGV_RackMasterStatusColumn)
                        {
                            case DGV_RackMasterStatusColumn.AxisType:
                                Value = $"{eAxisType}";
                                break;
                            case DGV_RackMasterStatusColumn.Servo:
                                if (eAxisType == AxisType.X_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_X_Axis_ServoOn)}";
                                else if (eAxisType == AxisType.Z_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Z_Axis_ServoOn)}";
                                else if (eAxisType == AxisType.A_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_A_Axis_ServoOn)}";
                                else if (eAxisType == AxisType.T_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_T_Axis_ServoOn)}";

                                Value = Value == "True" ? "On" : "Off";
                                DGV_Cell.Style.BackColor = Value == "On" ? Color.Lime : Color.White;
                                break;
                            case DGV_RackMasterStatusColumn.Home:
                                if (eAxisType == AxisType.X_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_X_Axis_HomeDone)}";
                                else if (eAxisType == AxisType.Z_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Z_Axis_HomeDone)}";
                                else if (eAxisType == AxisType.A_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_A_Axis_HomeDone)}";
                                else if (eAxisType == AxisType.T_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_T_Axis_HomeDone)}";

                                Value = Value == "True" ? "Done" : "Not Home";
                                DGV_Cell.Style.BackColor = Value == "Done" ? Color.Lime : Color.White;
                                break;
                            case DGV_RackMasterStatusColumn.Busy:
                                if (eAxisType == AxisType.X_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_X_Axis_Busy)}";
                                else if (eAxisType == AxisType.Z_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Z_Axis_Busy)}";
                                else if (eAxisType == AxisType.A_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_A_Axis_Busy)}";
                                else if (eAxisType == AxisType.T_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_T_Axis_Busy)}";
                                Value = Value == "True" ? "Busy" : "Idle";
                                DGV_Cell.Style.BackColor = Value == "Busy" ? Color.Lime : Color.White;
                                break;
                            case DGV_RackMasterStatusColumn.PosCommand:
                                if (eAxisType == AxisType.X_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_TargetPosition_0)).ToString("0.0")} mm";
                                else if (eAxisType == AxisType.Z_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_TargetPosition_0)).ToString("0.0")} mm";
                                else if (eAxisType == AxisType.A_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_TargetPosition_0)).ToString("0.0")} mm";
                                else if (eAxisType == AxisType.T_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_TargetPosition_0)).ToString("0.0")} °";
                                break;
                            case DGV_RackMasterStatusColumn.ActualPos:
                                if (eAxisType == AxisType.X_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                                else if (eAxisType == AxisType.Z_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                                else if (eAxisType == AxisType.A_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                                else if (eAxisType == AxisType.T_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentPosition_0)).ToString("0.0")} °";
                                break;
                            case DGV_RackMasterStatusColumn.ActualVel:
                                if (eAxisType == AxisType.X_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                                else if (eAxisType == AxisType.Z_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                                else if (eAxisType == AxisType.A_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                                else if (eAxisType == AxisType.T_Axis)
                                    Value = $"{((float)Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentSpeed_0)).ToString("0")} °/min";
                                break;
                            case DGV_RackMasterStatusColumn.ActualTorque:
                                if (eAxisType == AxisType.X_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.X_Axis_CurrentTorque)} %";
                                else if (eAxisType == AxisType.Z_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.Z_Axis_CurrentTorque)} %";
                                else if (eAxisType == AxisType.A_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.A_Axis_CurrentTorque)} %";
                                else if (eAxisType == AxisType.T_Axis)
                                    Value = $"{Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.T_Axis_CurrentTorque)} %";
                                break;
                        }

                        if ((string)DGV_Cell.Value != Value)
                            DGV_Cell.Value = Value;
                    }
                }

                if (DGV.CurrentRow != null || DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_RMSensorStatus(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_RackMasterSensorStatusColumn.ArmBaseSensor:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_RM_AAxis_OriginPos"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_RM_AAxis_OriginPos");
                        break;
                    case (int)DGV_RackMasterSensorStatusColumn.TurnLeftSideSensor:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_RM_TAxis_LeftOriginPos"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_RM_TAxis_LeftOriginPos");
                        break;
                    case (int)DGV_RackMasterSensorStatusColumn.TurnRightSideSensor:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_RM_TAxis_RightOriginPos"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_RM_TAxis_RightOriginPos");
                        break;
                }
            }

            if (DGV.Rows.Count != 1)
            {
                DGV.Rows.Clear();

                DGV.Rows.Add(new string[] { $"{string.Empty}",
                                                            $"{string.Empty}",
                                                            $"{string.Empty}"});
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_RackMasterSensorStatusColumn)).Length; nColumnCount++)
                    {
                        DGV_RackMasterSensorStatusColumn eDGV_RackMasterIOStatusColumn = (DGV_RackMasterSensorStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        bool bEnable = false;

                        switch (eDGV_RackMasterIOStatusColumn)
                        {
                            case DGV_RackMasterSensorStatusColumn.ArmBaseSensor:
                                bEnable = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Arm_Axis_Home);
                                break;
                            case DGV_RackMasterSensorStatusColumn.TurnLeftSideSensor:
                                bEnable = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Turn_Axis_LeftPos);
                                break;
                            case DGV_RackMasterSensorStatusColumn.TurnRightSideSensor:
                                bEnable = Get_RackMaster_2_CIM_Bit_Data(SendBitMapIndex.Status_Turn_Axis_RightPos);
                                break;

                        }

                        if (bEnable)
                        {
                            DGV_Cell.Value = "On";
                            DGV_Cell.Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            DGV_Cell.Value = "Off";
                            DGV_Cell.Style.BackColor = Color.White;
                        }
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_RegulatorStatus(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_RackMasterRegulatorStatusColumn.InputVoltage:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_InputVoltage"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_InputVoltage");
                        break;
                    case (int)DGV_RackMasterRegulatorStatusColumn.OutputVoltage:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_OutputVoltage"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_OutputVoltage");
                        break;
                    case (int)DGV_RackMasterRegulatorStatusColumn.OutputCurrent:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_InputVoltage"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_InputVoltage");
                        break;
                    case (int)DGV_RackMasterRegulatorStatusColumn.PickupTemp:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_PickupTemp"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_PickupTemp");
                        break;
                    case (int)DGV_RackMasterRegulatorStatusColumn.HeatsinkTemp:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_HeatsinkTemp"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_HeatsinkTemp");
                        break;
                    case (int)DGV_RackMasterRegulatorStatusColumn.InnerTemp:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_InnerTemp"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_InnerTemp");
                        break;
                }
            }

            if (DGV.Rows.Count != 1)
            {
                DGV.Rows.Clear();

                DGV.Rows.Add(new string[] { $"{string.Empty}",
                                            $"{string.Empty}",
                                            $"{string.Empty}",
                                            $"{string.Empty}",
                                            $"{string.Empty}",
                                            $"{string.Empty}"});
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < DGV.ColumnCount; nColumnCount++)
                    {
                        DGV_RackMasterRegulatorStatusColumn eDGV_RackMasterIOStatusColumn = (DGV_RackMasterRegulatorStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        string Value = string.Empty;

                        switch (eDGV_RackMasterIOStatusColumn)
                        {
                            case DGV_RackMasterRegulatorStatusColumn.InputVoltage:
                                Value = $"{Convert.ToString(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CPS_2_Regulator_InputVoltage))} V";
                                break;
                            case DGV_RackMasterRegulatorStatusColumn.OutputVoltage:
                                Value = $"{Convert.ToString(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CPS_2_Regulator_OutputVoltage))} V";
                                break;
                            case DGV_RackMasterRegulatorStatusColumn.OutputCurrent:
                                Value = $"{Convert.ToString(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CPS_2_Regulator_OutputCurrent))} A";
                                break;
                            case DGV_RackMasterRegulatorStatusColumn.PickupTemp:
                                Value = $"{Convert.ToString(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CPS_2_Regulator_Pickup_Temp))} °C";
                                break;
                            case DGV_RackMasterRegulatorStatusColumn.HeatsinkTemp:
                                Value = $"{Convert.ToString(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CPS_2_Regulator_HeatSink_Temp))} °C";
                                break;
                            case DGV_RackMasterRegulatorStatusColumn.InnerTemp:
                                Value = $"{Convert.ToString(Get_RackMaster_2_CIM_Word_Data(SendWordMapIndex.CPS_2_Regulator_Inner_Temp))} °C";
                                break;

                        }

                        if ((string)DGV_Cell.Value != Value)
                            DGV_Cell.Value = Value;
                    }
                }

                if (DGV.CurrentRow != null || DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }
        public void Update_DGV_RMPIOStatus(ref DataGridView DGV)
        {
            for (int nCount = 0; nCount < DGV.Columns.Count; nCount++)
            {
                switch (nCount)
                {
                    case (int)DGV_RackMasterPIOStatusColumn.CIM_To_RackMaster:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_PortToRM"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_PortToRM");
                        break;
                    case (int)DGV_RackMasterPIOStatusColumn.RackMaster_To_CIM:
                        if (DGV.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_RMToPort"))
                            DGV.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_RMToPort");
                        break;
                }
            }

            DataGridViewFunc.AutoRowSize(DGV, 25, 23, 30);

            if (DGV.Rows.Count != 4)
            {
                DGV.Rows.Clear();

                DGV.Rows.Add(new string[] { $"L-REQ", $"TR-REQ" });
                DGV.Rows.Add(new string[] { $"UL-REQ", $"BUSY" });
                DGV.Rows.Add(new string[] { $"READY", $"COMPLETE" });
                DGV.Rows.Add(new string[] { $"PORT-ERROR", $"STK-ERROR" });
            }
            else
            {
                for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_RackMasterPIOStatusColumn)).Length; nColumnCount++)
                    {
                        if (nColumnCount >= DGV.Columns.Count)
                            continue;

                        DGV_RackMasterPIOStatusColumn eDGV_RackMasterPIOStatusColumn = (DGV_RackMasterPIOStatusColumn)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[nColumnCount];
                        bool bEnable;

                        switch (eDGV_RackMasterPIOStatusColumn)
                        {
                            case DGV_RackMasterPIOStatusColumn.CIM_To_RackMaster:
                                if (nRowCount == 0)
                                {
                                    bEnable = Get_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_L_REQ);
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 1)
                                {
                                    bEnable = Get_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_UL_REQ);
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 2)
                                {
                                    bEnable = Get_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_Ready);
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 3)
                                {
                                    bEnable = Get_CIM_2_RackMaster_Bit_Data(ReceiveBitMapIndex.CMD_PIO_PortError);
                                    DGV_Cell.Style.BackColor = bEnable ? Master.ErrorIntervalColor : Color.White;
                                }
                                break;
                            case DGV_RackMasterPIOStatusColumn.RackMaster_To_CIM:
                                if (nRowCount == 0)
                                {
                                    bEnable = PIOStatus_STK_TR_REQ;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 1)
                                {
                                    bEnable = PIOStatus_STK_Busy;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 2)
                                {
                                    bEnable = PIOStatus_STK_Complete;
                                    DGV_Cell.Style.BackColor = bEnable ? Color.Lime : Color.White;
                                }
                                else if (nRowCount == 3)
                                {
                                    bEnable = PIOStatus_STK_Error;
                                    DGV_Cell.Style.BackColor = bEnable ? Master.ErrorIntervalColor : Color.White;
                                }
                                break;
                        }
                    }
                }

                if (DGV.CurrentCell != null)
                    DGV.CurrentCell = null;
            }
        }

        public void Update_DGV_CIMToRMBitMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(ReceiveBitMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(ReceiveBitMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (ReceiveBitMapIndex receiveMap in Enum.GetValues(typeof(ReceiveBitMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(GetParam().RecvBitMapStartAddr + (int)receiveMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)receiveMap} [0x{((int)receiveMap).ToString("x2")}]";
                    Name_Cell.Value = receiveMap.ToString();

                    bool value = Master.m_RackMaster_RecvBitMap[(int)receiveMap + GetParam().RecvBitMapStartAddr];
                    Value_Cell.Value = $"{value}";
                    Value_Cell.Style.BackColor = value ? Color.Lime : Color.White;

                    nRowIndex++;
                }
            }
        }
        public void Update_DGV_RMToCIMBitMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(SendBitMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(SendBitMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (SendBitMapIndex sendMap in Enum.GetValues(typeof(SendBitMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(GetParam().SendBitMapStartAddr + (int)sendMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)sendMap} [0x{((int)sendMap).ToString("x2")}]";
                    Name_Cell.Value = sendMap.ToString();

                    bool value = Master.m_RackMaster_SendBitMap[(int)sendMap + GetParam().SendBitMapStartAddr];
                    Value_Cell.Value = $"{value}";
                    Value_Cell.Style.BackColor = value ? Color.Lime : Color.White;

                    nRowIndex++;
                }
            }
        }
        public void Update_DGV_CIMToRMWordMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(ReceiveWordMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(ReceiveWordMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (ReceiveWordMapIndex receiveMap in Enum.GetValues(typeof(ReceiveWordMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(GetParam().RecvWordMapStartAddr + (int)receiveMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)receiveMap} [0x{((int)receiveMap).ToString("x2")}]";
                    Name_Cell.Value = receiveMap.ToString();

                    short value = Master.m_RackMaster_RecvWordMap[(int)receiveMap + GetParam().RecvWordMapStartAddr];

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
        public void Update_DGV_RMToCIMWordMap(ref DataGridView DGV)
        {
            if (DGV.Rows.Count != Enum.GetValues(typeof(SendWordMapIndex)).Length)
            {
                DGV.Rows.Clear();
                for (int nCount = 0; nCount < Enum.GetValues(typeof(SendWordMapIndex)).Length; nCount++)
                    DGV.Rows.Add();
            }
            else
            {
                int nRowIndex = 0;
                foreach (SendWordMapIndex sendMap in Enum.GetValues(typeof(SendWordMapIndex)))
                {
                    DataGridViewCell StartAddr_Cell = DGV.Rows[nRowIndex].Cells[0];
                    DataGridViewCell Index_Cell = DGV.Rows[nRowIndex].Cells[1];
                    DataGridViewCell Name_Cell = DGV.Rows[nRowIndex].Cells[2];
                    DataGridViewCell Value_Cell = DGV.Rows[nRowIndex].Cells[3];

                    StartAddr_Cell.Value = $"0x{(GetParam().SendWordMapStartAddr + (int)sendMap).ToString("x4")}";
                    Index_Cell.Value = $"{(int)sendMap} [0x{((int)sendMap).ToString("x2")}]";
                    Name_Cell.Value = sendMap.ToString();

                    short value = Master.m_RackMaster_SendWordMap[(int)sendMap + GetParam().SendWordMapStartAddr];
                    string valueText = $"{value} [0x{value.ToString("x4")}]";

                    if((string)Value_Cell.Value != valueText)
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
    }
}
