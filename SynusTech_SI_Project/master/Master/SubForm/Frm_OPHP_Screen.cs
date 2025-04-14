using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Master.Equipment.RackMaster;
using Master.Equipment.Port;
using Master.ManagedFile;
using System.Threading;

namespace Master.SubForm
{
    public partial class Frm_OPHP_Screen : Form
    {
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
        enum DGV_RackMasterSensorStatusColumn
        {
            ArmBaseSensor,
            TurnLeftSideSensor,
            TurnRightSideSensor
        }
        enum DGV_RackMasterPIOStatusColumn
        {
            CIM_To_RackMaster,
            RackMaster_To_CIM
        }

        Label[] PortInfoLabel = null;

        RackMaster m_CurrentRackMaster;
        public Frm_OPHP_Screen(RackMaster rackMaster)
        {
            InitializeComponent();
            m_CurrentRackMaster = rackMaster;

            ControlItemInit();

            this.VisibleChanged += (object sender, EventArgs e) =>
            {
                if (this.Visible)
                {
                    UIUpdateTimer.Enabled = true;
                }
                else
                {
                    UIUpdateTimer.Enabled = false;
                }
            };

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;
            };

            this.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                this.Dispose();
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);
            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_RackMasterList, new object[] { true });
            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_RackMasterSensorList, new object[] { true });
            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_RackMasterPIO, new object[] { true });

            lbl_MasterVer.Text = Frm_Main.MasterVersion;

            PortInfoLabel = new Label[] {label_EquipInfo1, label_EquipInfo2, label_EquipInfo3, label_EquipInfo4,
                                                        label_EquipInfo5, label_EquipInfo6 };

            for (int nCount = 0; nCount < PortInfoLabel.Length; nCount++)
                PortInfoLabel[nCount].Visible = false;

            DGV_RackMasterList.Rows.Clear();

            if (m_CurrentRackMaster != null)
            {
                for (int nCount = 0; nCount < Enum.GetNames(typeof(Equipment.RackMaster.RackMaster.AxisType)).Length; nCount++)
                {
                    string AxisType = $"{(Equipment.RackMaster.RackMaster.AxisType)nCount}";
                    string ServoOn = string.Empty;
                    string HomeDone = string.Empty;
                    string Op = string.Empty;
                    string PosCommand = string.Empty;
                    string ActualPos = string.Empty;
                    string ActualVel = string.Empty;
                    string ActualTorque = string.Empty;

                    DGV_RackMasterList.Rows.Add(new string[] { $"{AxisType}",
                                                                $"{ServoOn}",
                                                                $"{HomeDone}",
                                                                $"{Op}",
                                                                $"{PosCommand}",
                                                                $"{ActualPos}",
                                                                $"{ActualVel}",
                                                                $"{ActualTorque}"});
                }
            }

            DGV_RackMasterSensorList.Rows.Clear();

            if (m_CurrentRackMaster != null)
            {
                DGV_RackMasterSensorList.Rows.Add(new string[] { $"{string.Empty}",
                                                                $"{string.Empty}",
                                                                $"{string.Empty}"});
            }

            DGV_RackMasterPIO.Rows.Clear();
            if (m_CurrentRackMaster != null)
            {
                DGV_RackMasterPIO.Rows.Add(new string[] { $"L-REQ", $"TR-REQ" });
                DGV_RackMasterPIO.Rows.Add(new string[] { $"UL-REQ", $"BUSY" });
                DGV_RackMasterPIO.Rows.Add(new string[] { $"READY", $"COMPLETE" });
                DGV_RackMasterPIO.Rows.Add(new string[] { $"PORT-ERROR", $"STK-ERROR" });
            }
            foreach (var port in Master.m_Ports.Select((value, index) => (value, index)))
            {
                if (port.index >= PortInfoLabel.Length)
                    continue;

                PortInfoLabel[port.index].Text = GetBasicInfoStr(port.value.Value);
                PortInfoLabel[port.index].Visible = true;
            }
        }
        private string GetBasicInfoStr(Equipment.Port.Port port)
        {
            string InfoText = $"Equipment : Port [ID : {port.GetParam().ID}]\n" +
                    $"Type : {port.GetParam().ePortType}\n" +
                    $"Mode : {port.GetPortOperationMode()}\n" +
                    $"Power : {(port.IsPortServoOn() ? "On" : "Off")}\n" +
                    $"Busy : {(port.IsPortBusy() ? "Busy" : "Idle")}\n" +
                    $"Error : {port.GetRecentErrorCodeStr()}\n" +
                    $"Auto Control : {(port.IsAutoControlRun() ? "Running" : "Idle")}\n";

            //string InfoText = $"Equipment : Port [ID : {port.GetParam().ID}]\n" +
            //                    $"Type : {port.PortTypeToStr(port.GetParam().ePortType)}\n" +
            //                    $"Mode : {port.GetPortOperationMode()}\n" +
            //                    $"ErrorCode : {port.Get_Port_2_CIM_Word_Data(Port.SendWordMapIndex.ErrorCode_0)}\n";

            for (int nCount = 0; nCount < Enum.GetNames(typeof(Port.PortAxis)).Length; nCount++)
            {
                Port.PortAxis ePortAxis = (Port.PortAxis)nCount;

                if (port.GetMotionParam().eZAxisType == Port.ZAxisType.Cylinder && ePortAxis == Port.PortAxis.Z_Axis)
                {
                    InfoText += $"{ePortAxis} [Cylinder]\n";
                }
                else
                {
                    if (port.GetWMXAxisNumber(ePortAxis) == -1)
                        continue;

                    InfoText += $"{ePortAxis} [{port.GetWMXAxisNumber(ePortAxis)}] Servo : {(port.GetAxisServo(ePortAxis) ? "On" : "Off")}\n";
                    InfoText += $"{ePortAxis} [{port.GetWMXAxisNumber(ePortAxis)}] Busy : {(port.GetAxisBusy(ePortAxis) ? "Busy" : "Idle")}\n";
                }
            }
            return InfoText;
        }

        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState != FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Maximized;

                NowTimeUpdate();
                LogOnTimeCheck();
                LaguageCheck();
                UpdateEquipmentItem();
                LogMessageUpdate();
            }
            catch (Exception ex) { }
        }
        private void LaguageCheck()
        {
            //FormFunc.SetText(this, SynusLangPack.GetLanguage("Login"));
            //LabelFunc.SetText(lbl_IDTitle, SynusLangPack.GetLanguage("ID"));
            //LabelFunc.SetText(lbl_PasswordTitle, SynusLangPack.GetLanguage("Password"));
            //LabelFunc.SetText(lbl_DurationTimeTitle, SynusLangPack.GetLanguage("DurationTime"));
            //ButtonFunc.SetText(btn_LogIn, SynusLangPack.GetLanguage("Login"));
        }
        private void LogMessageUpdate()
        {
            lock (Log.LogLock)
            {
                if (DGV_Log.Rows.Count != Log.LogItems.Count)
                {
                    DGV_Log.SuspendLayout();
                    DGV_Log.Rows.Clear();
                    for (int nCount = 0; nCount < Log.LogItems.Count; nCount++)
                    {
                        DGV_Log.Rows.Insert(nCount, Log.ItemToLogArray(Log.LogItems[nCount]));
                    }
                    DGV_Log.ResumeLayout();
                    if (DGV_Log.Rows.Count > 0)
                    {
                        DGV_Log.CurrentCell = DGV_Log.Rows[DGV_Log.Rows.Count - 1].Cells[0];
                        DGV_Log.CurrentCell = null;
                    }
                }

                for (int nRowCount = 0; nRowCount < DGV_Log.Rows.Count; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < DGV_Log.Columns.Count; nColumnCount++)
                    {
                        Log.ColumnIndex eLogColumnIndex = (Log.ColumnIndex)nColumnCount;
                        DataGridViewCell DGV_Cell = DGV_Log.Rows[nRowCount].Cells[nColumnCount];

                        switch (eLogColumnIndex)
                        {
                            case Log.ColumnIndex.LogLevel:
                                if ((string)DGV_Cell.Value == Log.LogLevel.Error.ToString() ||
                                    (string)DGV_Cell.Value == Log.LogLevel.Exception.ToString())
                                    DGV_Cell.Style.ForeColor = Color.Red;
                                else if ((string)DGV_Cell.Value == Log.LogLevel.Warning.ToString())
                                    DGV_Cell.Style.ForeColor = Color.Orange;
                                else
                                    DGV_Cell.Style.ForeColor = Color.Black;
                                break;
                        }
                    }
                }
            }
        }
        private void UpdateEquipmentItem()
        {
            if (m_CurrentRackMaster != null)
            {
                TopPanelStatusUpdate();
                RMMotionStatusUpdate();
                RMSensorStatusUpdate();
                RMPIOStatusUpdate();
                ButtonStatusUpdate();
                EStopButtonUpdate();
                PortInfoUpdate();
                CycleInfoUpdate();
            }
        }
        private void NowTimeUpdate()
        {
            LabelFunc.SetText(lbl_NowTime, DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
        }
        private void LogOnTimeCheck()
        {
            TimeSpan RemainingTime = TimeSpan.FromSeconds(ApplicationParam.m_ApplicationParam.LoginDuration - Frm_Main.LogOnTime.Elapsed.TotalSeconds);

            LabelFunc.SetText(lbl_RemainingTime, Frm_Main.m_bLogOn ? $"{RemainingTime.Minutes.ToString("00")}:{RemainingTime.Seconds.ToString("00")}" : $"-");
        }
        private void RMMotionStatusUpdate()
        {
            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(RackMaster.AxisType)).Length; nRowCount++)
            {
                RackMaster.AxisType eAxisType = (RackMaster.AxisType)nRowCount;

                for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_RackMasterStatusColumn)).Length; nColumnCount++)
                {
                    DGV_RackMasterStatusColumn eDGV_RackMasterStatusColumn = (DGV_RackMasterStatusColumn)nColumnCount;
                    DataGridViewCell DGV_Cell = DGV_RackMasterList.Rows[nRowCount].Cells[nColumnCount];
                    string Value = string.Empty;

                    switch (eDGV_RackMasterStatusColumn)
                    {
                        case DGV_RackMasterStatusColumn.AxisType:
                            Value = $"{eAxisType}";
                            break;
                        case DGV_RackMasterStatusColumn.Servo:
                            if (eAxisType == RackMaster.AxisType.X_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_X_Axis_ServoOn)}";
                            else if (eAxisType == RackMaster.AxisType.Z_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_Z_Axis_ServoOn)}";
                            else if (eAxisType == RackMaster.AxisType.A_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_A_Axis_ServoOn)}";
                            else if (eAxisType == RackMaster.AxisType.T_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_T_Axis_ServoOn)}";

                            Value = Value == "True" ? "On" : "Off";
                            DGV_Cell.Style.BackColor = Value == "On" ? Color.Lime : Color.White;
                            break;
                        case DGV_RackMasterStatusColumn.Home:
                            if (eAxisType == RackMaster.AxisType.X_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_X_Axis_HomeDone)}";
                            else if (eAxisType == RackMaster.AxisType.Z_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_Z_Axis_HomeDone)}";
                            else if (eAxisType == RackMaster.AxisType.A_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_A_Axis_HomeDone)}";
                            else if (eAxisType == RackMaster.AxisType.T_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_T_Axis_HomeDone)}";

                            Value = Value == "True" ? "Done" : "Not Home";
                            DGV_Cell.Style.BackColor = Value == "Done" ? Color.Lime : Color.White;
                            break;
                        case DGV_RackMasterStatusColumn.Busy:
                            if (eAxisType == RackMaster.AxisType.X_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_X_Axis_Busy)}";
                            else if (eAxisType == RackMaster.AxisType.Z_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_Z_Axis_Busy)}";
                            else if (eAxisType == RackMaster.AxisType.A_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_A_Axis_Busy)}";
                            else if (eAxisType == RackMaster.AxisType.T_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_T_Axis_Busy)}";
                            Value = Value == "True" ? "Busy" : "Idle";
                            DGV_Cell.Style.BackColor = Value == "Busy" ? Color.Lime : Color.White;
                            break;
                        case DGV_RackMasterStatusColumn.PosCommand:
                            if (eAxisType == RackMaster.AxisType.X_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_TargetPosition_0)).ToString("0.0")} mm";
                            else if (eAxisType == RackMaster.AxisType.Z_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_TargetPosition_0)).ToString("0.0")} mm";
                            else if (eAxisType == RackMaster.AxisType.A_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.A_Axis_TargetPosition_0)).ToString("0.0")} mm";
                            else if (eAxisType == RackMaster.AxisType.T_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.T_Axis_TargetPosition_0)).ToString("0.0")} °";
                            break;
                        case DGV_RackMasterStatusColumn.ActualPos:
                            if (eAxisType == RackMaster.AxisType.X_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                            else if (eAxisType == RackMaster.AxisType.Z_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                            else if (eAxisType == RackMaster.AxisType.A_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.A_Axis_CurrentPosition_0)).ToString("0.0")} mm";
                            else if (eAxisType == RackMaster.AxisType.T_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.T_Axis_CurrentPosition_0)).ToString("0.0")} °";
                            break;
                        case DGV_RackMasterStatusColumn.ActualVel:
                            if (eAxisType == RackMaster.AxisType.X_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                            else if (eAxisType == RackMaster.AxisType.Z_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                            else if (eAxisType == RackMaster.AxisType.A_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.A_Axis_CurrentSpeed_0)).ToString("0")} m/min";
                            else if (eAxisType == RackMaster.AxisType.T_Axis)
                                Value = $"{((float)m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.T_Axis_CurrentSpeed_0)).ToString("0")} °/min";
                            break;
                        case DGV_RackMasterStatusColumn.ActualTorque:
                            if (eAxisType == RackMaster.AxisType.X_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_CurrentTorque)} %";
                            else if (eAxisType == RackMaster.AxisType.Z_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_CurrentTorque)} %";
                            else if (eAxisType == RackMaster.AxisType.A_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.A_Axis_CurrentTorque)} %";
                            else if (eAxisType == RackMaster.AxisType.T_Axis)
                                Value = $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.T_Axis_CurrentTorque)} %";
                            break;
                    }

                    if ((string)DGV_Cell.Value != Value)
                        DGV_Cell.Value = Value;
                }
            }

            if (DGV_RackMasterList.CurrentCell != null)
                DGV_RackMasterList.CurrentCell = null;
        }
        private void RMSensorStatusUpdate()
        {
            for (int nRowCount = 0; nRowCount < DGV_RackMasterSensorList.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_RackMasterSensorStatusColumn)).Length; nColumnCount++)
                {
                    DGV_RackMasterSensorStatusColumn eDGV_RackMasterIOStatusColumn = (DGV_RackMasterSensorStatusColumn)nColumnCount;
                    DataGridViewCell DGV_Cell = DGV_RackMasterSensorList.Rows[nRowCount].Cells[nColumnCount];
                    bool bEnable = false;

                    switch (eDGV_RackMasterIOStatusColumn)
                    {
                        case DGV_RackMasterSensorStatusColumn.ArmBaseSensor:
                            bEnable = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_Arm_Axis_Home);
                            break;
                        case DGV_RackMasterSensorStatusColumn.TurnLeftSideSensor:
                            bEnable = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_Turn_Axis_LeftPos);
                            break;
                        case DGV_RackMasterSensorStatusColumn.TurnRightSideSensor:
                            bEnable = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_Turn_Axis_RightPos);
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

            if(DGV_RackMasterSensorList.CurrentCell != null)
                DGV_RackMasterSensorList.CurrentCell = null;
        }
        private void RMPIOStatusUpdate()
        {
            for (int nRowCount = 0; nRowCount < DGV_RackMasterPIO.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < Enum.GetNames(typeof(DGV_RackMasterPIOStatusColumn)).Length; nColumnCount++)
                {
                    DGV_RackMasterPIOStatusColumn eDGV_RackMasterPIOStatusColumn = (DGV_RackMasterPIOStatusColumn)nColumnCount;
                    DataGridViewCell DGV_Cell = DGV_RackMasterPIO.Rows[nRowCount].Cells[nColumnCount];
                    bool bEnable = false;

                    switch (eDGV_RackMasterPIOStatusColumn)
                    {
                        case DGV_RackMasterPIOStatusColumn.CIM_To_RackMaster:
                            if (nRowCount == 0)
                                bEnable = m_CurrentRackMaster.Get_CIM_2_RackMaster_Bit_Data(RackMaster.ReceiveBitMapIndex.CMD_PIO_L_REQ);
                            else if (nRowCount == 1)
                                bEnable = m_CurrentRackMaster.Get_CIM_2_RackMaster_Bit_Data(RackMaster.ReceiveBitMapIndex.CMD_PIO_UL_REQ);
                            else if (nRowCount == 2)
                                bEnable = m_CurrentRackMaster.Get_CIM_2_RackMaster_Bit_Data(RackMaster.ReceiveBitMapIndex.CMD_PIO_Ready);
                            else if (nRowCount == 3)
                                bEnable = m_CurrentRackMaster.Get_CIM_2_RackMaster_Bit_Data(RackMaster.ReceiveBitMapIndex.CMD_PIO_PortError);
                            break;
                        case DGV_RackMasterPIOStatusColumn.RackMaster_To_CIM:
                            if (nRowCount == 0)
                                bEnable = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_PIO_TR_REQ);
                            else if (nRowCount == 1)
                                bEnable = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_PIO_Busy);
                            else if (nRowCount == 2)
                                bEnable = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_PIO_Complete);
                            else if (nRowCount == 3)
                                bEnable = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_PIO_STK_Error);
                            break;
                    }

                    if (bEnable && nRowCount != 3)
                        DGV_Cell.Style.BackColor = Color.Lime;
                    else if (bEnable && nRowCount == 3)
                        DGV_Cell.Style.BackColor = Color.Red;
                    else
                        DGV_Cell.Style.BackColor = Color.White;
                }
            }

            if (DGV_RackMasterPIO.CurrentCell != null)
                DGV_RackMasterPIO.CurrentCell = null;
        }
        private void TopPanelStatusUpdate()
        {
            LabelFunc.SetText(lbl_RackMasterID, $"{(m_CurrentRackMaster.GetParam().ID)}");

            if (Master.m_CIM == null)
            {
                LabelFunc.SetText(lbl_CIMOnline, $"None");
                LabelFunc.SetBackColor(lbl_CIMOnline, Color.Yellow);
            }
            else
            {
                bool bCIMConnection = Master.m_CIM.IsConnected();
                LabelFunc.SetText(lbl_CIMOnline, $"{(bCIMConnection ? "Online" : "Offline")}");
                LabelFunc.SetBackColor(lbl_CIMOnline, bCIMConnection ? Color.Lime : Color.Red);
            }

            bool bRackMasterConnection = m_CurrentRackMaster.IsConnected();
            LabelFunc.SetText(lbl_RackMasterOnline, $"{(bRackMasterConnection ? "Online" : "Offline")}");
            LabelFunc.SetBackColor(lbl_RackMasterOnline, bRackMasterConnection ? Color.Lime : Color.Red);
            LabelFunc.SetText(lbl_AutoStatus, $"{(m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_AutoMode) ? "Running" : "Idle")}");
            LabelFunc.SetText(lbl_AutoStep, $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.AutoStep_Number)}");
            LabelFunc.SetText(lbl_FromID, $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.From_Shelf_ID_0)}");
            LabelFunc.SetText(lbl_ToID, $"{m_CurrentRackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.To_Shelf_ID_0)}");
        }
        private void ButtonStatusUpdate()
        {
            bool bAutoModeRunningStatus = m_CurrentRackMaster.Get_RackMaster_2_CIM_Bit_Data(RackMaster.SendBitMapIndex.Status_AutoMode);
            ButtonFunc.SetBackColor(btn_AutoRun, bAutoModeRunningStatus ? Color.Lime : Color.AliceBlue);
            ButtonFunc.SetBackColor(btn_AutoStop, !bAutoModeRunningStatus ? Color.Red : Color.AliceBlue);

            ButtonFunc.SetBackColor(btn_CIMMode, m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.CIMMode ? Color.Lime : Color.AliceBlue);
            ButtonFunc.SetBackColor(btn_MasterMode, m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.MasterMode ? Color.Lime : Color.AliceBlue);

            ButtonFunc.SetVisible(btn_CycleRun, m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.MasterMode ? true : false);
            ButtonFunc.SetVisible(btn_CycleStop, m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.MasterMode ? true : false);
            GroupBoxFunc.SetVisible(groupBox_CycleSettings, m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.MasterMode ? true : false);

            ButtonFunc.SetBackColor(btn_CycleRun, m_CurrentRackMaster.m_bAutoCycleRunning ? Color.Lime : Color.AliceBlue);
        }

        private void PortInfoUpdate()
        {
            foreach (var port in Master.m_Ports.Select((value, index) => (value, index)))
            {
                if (port.index >= PortInfoLabel.Length)
                    continue;

                PortInfoLabel[port.index].Text = GetBasicInfoStr(port.value.Value);
            }
        }
        private void CycleInfoUpdate()
        {
            string CurrentStep = m_CurrentRackMaster.m_eCycleControlStep.ToString();
            LabelFunc.SetText(lbl_CycleStep, $"Cycle Step : {CurrentStep}");

            string Error = m_CurrentRackMaster.m_eCycleControlErrorCode.ToString();
            LabelFunc.SetText(lbl_CycleError, $"Error : {Error}");

            double ProgressPercent = ((double)m_CurrentRackMaster.m_CycleProgress / (double)m_CurrentRackMaster.m_CycleCount) * 100.0;
            if (double.IsNaN(ProgressPercent))
                ProgressPercent = 0;

            LabelFunc.SetText(lbl_CycleProgressCount, $"Progress : {m_CurrentRackMaster.m_CycleProgress} / {m_CurrentRackMaster.m_CycleCount} ({ProgressPercent.ToString("0.0")}%)");
            
            string time = m_CurrentRackMaster.m_CycleRunningTime.Elapsed.ToString("hh\\:mm\\:ss\\.ff");
            LabelFunc.SetText(lbl_CycleProgressTime, $"Time : {time}");

        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_AutoRun_Click(object sender, EventArgs e)
        {
            if (m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.CIMMode)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("RejectCommandInCIMMode"), SynusLangPack.GetLanguage("Warning Message"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_CurrentRackMaster.SetAutoModeEnable();
        }

        private void btn_AutoStop_Click(object sender, EventArgs e)
        {
            if (m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.CIMMode)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("RejectCommandInCIMMode"), SynusLangPack.GetLanguage("Warning Message"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("AutoModeStopMessage"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
                m_CurrentRackMaster.SetAutoModeDisable();
        }

        private void btn_CycleRun_Click(object sender, EventArgs e)
        {
            if (m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.CIMMode)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("RejectCommandInCIMMode"), SynusLangPack.GetLanguage("Warning Message"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!m_CurrentRackMaster.IsAutoMode())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("AutoModeWarningMessage"), SynusLangPack.GetLanguage("Warning Message"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                //Thread Loop
                if (!m_CurrentRackMaster.m_bAutoCycleRunning)
                {
                    int CycleCount = Convert.ToInt32(tbx_CycleCount.Text);
                    int FromID = Convert.ToInt32(tbx_FromID.Text);
                    int ToID = Convert.ToInt32(tbx_ToID.Text);
                    m_CurrentRackMaster.AutoCycleRun(CycleCount, FromID, ToID);
                }
            }
            catch(Exception ex)
            {
                ManagedFile.ExceptionLog.Add($"Cycle Run", "ErrorMessage", ex);
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.Application, Log.Equipment.RackMaster, $"ID = {m_CurrentRackMaster.GetParam().ID} Cycle Command Error {ex.Message}"));
            }
        }

        private void btn_CycleStop_Click(object sender, EventArgs e)
        {
            m_CurrentRackMaster.AutoCycleStop();
        }

        private void btn_CIMMode_Click(object sender, EventArgs e)
        {
            if (m_CurrentRackMaster.IsAutoMode())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("AutoModeWarningMessage"), SynusLangPack.GetLanguage("Warning Message"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (m_CurrentRackMaster.IsAutoCycleRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("CycleModeWarningMessage"), SynusLangPack.GetLanguage("Warning Message"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_CurrentRackMaster.SetControlMode(RackMaster.ControlMode.CIMMode);
        }

        private void btn_MasterMode_Click(object sender, EventArgs e)
        {
            if (m_CurrentRackMaster.IsAutoMode() && m_CurrentRackMaster.m_eControlMode == RackMaster.ControlMode.CIMMode)
            {
                Master.InsertMasterAlarm(Master.MasterAlarm.Master_Mode_Change_Error);
            }

            if (m_CurrentRackMaster.IsAutoMode())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("AutoModeWarningMessage"), SynusLangPack.GetLanguage("Warning Message"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            m_CurrentRackMaster.SetControlMode(RackMaster.ControlMode.MasterMode);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        private void btn_EMO_Click(object sender, EventArgs e)
        {
            if (!Master.IsEStopState())
                Master.EStop();
            else
                Master.EStopRelease();
        }
        private void EStopButtonUpdate()
        {
            if (Master.IsEStopState())
            {
                ButtonFunc.SetBackColor(btn_EMO, Color.Red);
                ButtonFunc.SetText(btn_EMO, SynusLangPack.GetLanguage("EmergencyStopRelease"));
            }
            else
            {
                ButtonFunc.SetBackColor(btn_EMO, Color.AliceBlue);
                ButtonFunc.SetText(btn_EMO, SynusLangPack.GetLanguage("EmergencyStop"));
            }
        }

        private void btn_BuzzerStop_Click(object sender, EventArgs e)
        {
            Master.m_CIM.Set_CIM_2_Master_Bit_Data(Equipment.CIM.CIM.ReceiveBitMapIndex.TowerLamp_BUZZER_MUTE, false);
        }
    }
}
