using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Equipment.Port;
using Master.Equipment.Port.TagReader.ReaderEquip;
using Master.ManagedFile;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using MovenCore;

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortTPSettings : Form
    {
        public enum WMXParameterGridViewRow
        {
            GearRatioNum,
            GearRatioDen,
            MotorDirection,
            InposWidth,
            PosSetWidth,
            HomeType,
            HomeDirection,
            HomeFastVelocity,
            HomeFastAcc,
            HomeFastDec,
            HomeSlowVelocity,
            HomeSlowAcc,
            HomeSlowDec,
            HomeShiftVelocity,
            HomeShiftAcc,
            HomeShiftDec,
            HomeShiftDistance,
            AbsoluteEncorderMode,
            AbsoluteEncorderOffset,
            SoftLimitSwitchType,
            SoftLimitPosValue,
            SoftLimitNegValue,
            InvertPosLimitSwitch,
            InvertNegLimitSwitch,
            QuickStopDecel
        }
        public enum WMXIOGridViewColumn
        {
            ItemName,
            StartAddr,
            BitNum,
            MapStatus,
            WMXStatus,
            Invert
        }
        Stopwatch m_PushSt = new Stopwatch();
        DataGridView[] DGV_MotionParam = new DataGridView[Enum.GetValues(typeof(Port.PortAxis)).Length];
        DataGridView[] DGV_BufferCVParam = new DataGridView[Enum.GetValues(typeof(Port.BufferCV)).Length];
        DataGridView DGV_WMXParam = new DataGridView();
        public Frm_PortTPSettings()
        {
            InitializeComponent();
            ControlItemInit();

            this.VisibleChanged += (object sender, EventArgs e) =>
            {
                if (this.Visible)
                {
                    UIUpdateTimer.Enabled = true;
                }
                else
                {
                    tableLayoutPanel_MotionParam.Tag = null;
                    tableLayoutPanel_BufferParam.Tag = null;
                    pnl_WMXParamGrid.Tag = null;
                    DGV_OutputMapSettings.Tag = null;
                    DGV_InputMapSettings.Tag = null;
                    UIUpdateTimer.Enabled = false;
                }
            };

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                for (int nCount = 0; nCount < DGV_MotionParam.Length; nCount++)
                    DGV_MotionParam[nCount].Dispose();

                for (int nCount = 0; nCount < DGV_BufferCVParam.Length; nCount++)
                    DGV_BufferCVParam[nCount].Dispose();

                DGV_WMXParam.Dispose();

                foreach (Control item in tabPage_BufferCVControlParam.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_IDReader.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_IOParam.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_PortControlParam.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_PortSafty.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_ServoParam.Controls)
                    ControlFunc.Dispose(item);

                tabPage_BufferCVControlParam.Dispose();
                tabPage_IDReader.Dispose();
                tabPage_IOParam.Dispose();
                tabPage_PortControlParam.Dispose();
                tabPage_PortSafty.Dispose();
                tabPage_ServoParam.Dispose();

                tabcontrol_Settings.Dispose();

                foreach (Control item in this.Controls)
                    ControlFunc.Dispose(item);
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);

            for (int nCount = 0; nCount < DGV_MotionParam.Length; nCount++)
                DGV_MotionParam[nCount] = new DataGridView();

            for (int nCount = 0; nCount < DGV_BufferCVParam.Length; nCount++)
                DGV_BufferCVParam[nCount] = new DataGridView();
        }
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);

            tableLayoutPanel15.RowStyles[1].Height = 90.0f;
            tableLayoutPanel20.RowStyles[0].Height = 90.0f;
            tableLayoutPanel20.RowStyles[1].Height = 90.0f;
            tableLayoutPanel20.RowStyles[2].Height = 90.0f;
            tableLayoutPanel21.RowStyles[0].Height = 90.0f;
            tableLayoutPanel21.RowStyles[1].Height = 90.0f;
            tableLayoutPanel21.RowStyles[2].Height = 90.0f;
            tableLayoutPanel14.RowStyles[0].Height = 90.0f;
            tableLayoutPanel14.RowStyles[1].Height = 90.0f;
            tableLayoutPanel14.RowStyles[2].Height = 90.0f;
            tableLayoutPanel14.RowStyles[3].Height = 90.0f;
            tableLayoutPanel22.RowStyles[0].Height = 90.0f;
            tableLayoutPanel5.RowStyles[1].Height = 60.0f;
        }
        private void ClearMotionDGV()
        {
            if (tableLayoutPanel_MotionParam.Controls.Count > 0)
                tableLayoutPanel_MotionParam.Controls.Clear();

            for (int nCount = 0; nCount < DGV_MotionParam.Length; nCount++)
                DGV_MotionParam[nCount].Rows.Clear();
        }
        private void ClearBufferCVDGV()
        {
            if (tableLayoutPanel_BufferParam.Controls.Count > 0)
                tableLayoutPanel_BufferParam.Controls.Clear();

            for (int nCount = 0; nCount < DGV_BufferCVParam.Length; nCount++)
                DGV_BufferCVParam[nCount].Rows.Clear();
        }
        private void ClearWMXParamDGV()
        {
            if (pnl_WMXParamGrid.Controls.Count > 0)
                pnl_WMXParamGrid.Controls.Clear();

            DGV_WMXParam.Rows.Clear();
        }
        private void ClearIODGV()
        {
            if (DGV_OutputMapSettings.Rows.Count > 0)
                DGV_OutputMapSettings.Rows.Clear();

            if (DGV_InputMapSettings.Rows.Count > 0)
                DGV_InputMapSettings.Rows.Clear();
        }

        private void AllPanelAndGridReload()
        {
            tableLayoutPanel_MotionParam.Tag = null;
            tableLayoutPanel_BufferParam.Tag = null;
            pnl_WMXParamGrid.Tag = null;
            DGV_OutputMapSettings.Tag = null;
            DGV_InputMapSettings.Tag = null;
        }
        
        private void IOParamUpdate()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
            {
                if (DGV_OutputMapSettings.Rows.Count > 0)
                    DGV_OutputMapSettings.Rows.Clear();

                if (DGV_InputMapSettings.Rows.Count > 0)
                    DGV_InputMapSettings.Rows.Clear();
            }
            else
            {
                if ((string)DGV_OutputMapSettings.Tag != CurrentPort.GetParam().ID &&
                    (string)DGV_InputMapSettings.Tag != CurrentPort.GetParam().ID)
                {
                    ClearIODGV();
                    LoadOutputGridView(ref DGV_OutputMapSettings, CurrentPort);
                    LoadInputGridView(ref DGV_InputMapSettings, CurrentPort);
                    DGV_OutputMapSettings.Tag = CurrentPort.GetParam().ID;
                    DGV_InputMapSettings.Tag = CurrentPort.GetParam().ID;
                }
            }
        }

        private void LoadOutputGridView(ref DataGridView DGV, Port port)
        {
            if (port.GetParam().ePortType == Port.PortType.MGV_AGV ||
                    port.GetParam().ePortType == Port.PortType.MGV ||
                    port.GetParam().ePortType == Port.PortType.AGV)
            {
                foreach (Port.AGV_MGV_OutputItem item in Enum.GetValues(typeof(Port.AGV_MGV_OutputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.OutputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }
            else if (port.GetParam().ePortType == Port.PortType.OHT || port.GetParam().ePortType == Port.PortType.MGV_OHT)
            {
                foreach (Port.OHT_OutputItem item in Enum.GetValues(typeof(Port.OHT_OutputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.OutputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }
            else if (port.GetParam().ePortType == Port.PortType.Conveyor_AGV || port.GetParam().ePortType == Port.PortType.Conveyor_OMRON)
            {
                foreach (Port.Conveyor_OutputItem item in Enum.GetValues(typeof(Port.Conveyor_OutputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.OutputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }
            else if (port.GetParam().ePortType == Port.PortType.EQ)
            {
                foreach (Port.EQ_OutputItem item in Enum.GetValues(typeof(Port.EQ_OutputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.OutputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }

            //var OutputMap = port.GetMotionParam().Ctrl_IO;

            //for (int nRowCount = 0; nRowCount < OutputMap.OutputMap.Length; nRowCount++)
            //{
            //    bool bRowAdd = false;
            //    if (port.GetParam().ePortType == Port.PortType.MGV_AGV ||
            //        port.GetParam().ePortType == Port.PortType.MGV ||
            //        port.GetParam().ePortType == Port.PortType.AGV)
            //    {
            //        if(Enum.IsDefined(typeof(Port.AGV_MGV_OutputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.AGV_MGV_OutputItem eAGV_MGV_OutputItem = (Port.AGV_MGV_OutputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eAGV_MGV_OutputItem}";
            //            bRowAdd = true;
            //        }
            //    }
            //    else if(port.GetParam().ePortType == Port.PortType.OHT || port.GetParam().ePortType == Port.PortType.MGV_OHT)
            //    {
            //        if (Enum.IsDefined(typeof(Port.OHT_OutputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.OHT_OutputItem eOHT_OutputItem = (Port.OHT_OutputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eOHT_OutputItem}";
            //            bRowAdd = true;
            //        }
            //    }
            //    else if(port.GetParam().ePortType == Port.PortType.Conveyor)
            //    {
            //        if (Enum.IsDefined(typeof(Port.Conveyor_OutputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.Conveyor_OutputItem eConveyor_OutputItem = (Port.Conveyor_OutputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eConveyor_OutputItem}";
            //            bRowAdd = true;
            //        }
            //    }
            //    else if (port.GetParam().ePortType == Port.PortType.EQ)
            //    {
            //        if (Enum.IsDefined(typeof(Port.EQ_OutputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.EQ_OutputItem eEQ_OutputItem = (Port.EQ_OutputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eEQ_OutputItem}";
            //            bRowAdd = true;
            //        }
            //    }

            //    if (bRowAdd)
            //    {
            //        DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //        DataGridViewCell DGV_StartAddrCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.StartAddr];
            //        DataGridViewCell DGV_BitNumCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.BitNum];
            //        DataGridViewCell DGV_WMXStatusCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.WMXStatus];
            //        DataGridViewCell DGV_InvertCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.Invert];

            //        for (int nCount = 0; nCount < OutputMap.OutputMap.Length; nCount++)
            //        {
            //            var IOParam = OutputMap.OutputMap[nCount];
                        
            //            try
            //            {
            //                if (string.IsNullOrEmpty((string)DGV_NameCell.Value))
            //                    continue;

            //                if (Convert.ToString(DGV_NameCell.Value) == IOParam.Name && !string.IsNullOrEmpty(IOParam.Name))
            //                {
            //                    DGV_StartAddrCell.Value = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : string.Empty;
            //                    DGV_BitNumCell.Value = IOParam.Bit != -1 ? IOParam.Bit.ToString() : string.Empty;
            //                    DGV_WMXStatusCell.Value = $"Off";
            //                    DGV_InvertCell.Value = IOParam.bInvert.ToString();
            //                    break;
            //                }
            //            }
            //            catch
            //            {

            //            }
            //        }
            //    }
            //}
        }
        private bool ApplyOutputGridView(ref DataGridView DGV, Port port, ref Exception _Exception)
        {
            try
            {
                var OutputMap = port.GetMotionParam().Ctrl_IO;

                for (int nRowCount = 0; nRowCount < OutputMap.OutputMap.Length; nRowCount++)
                {
                    if(nRowCount >= DGV.Rows.Count)
                    {
                        //Default
                        OutputMap.OutputMap[nRowCount].Name = string.Empty;
                        OutputMap.OutputMap[nRowCount].StartAddr = -1;
                        OutputMap.OutputMap[nRowCount].Bit = -1;
                        OutputMap.OutputMap[nRowCount].bInvert = false;
                    }
                    else
                    {
                        DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
                        DataGridViewCell DGV_StartAddrCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.StartAddr];
                        DataGridViewCell DGV_BitNumCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.BitNum];
                        DataGridViewCell DGV_InvertCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.Invert];

                        DGV.CurrentCell = DGV_BitNumCell;

                        if (!string.IsNullOrEmpty((string)DGV_StartAddrCell.Value))
                            OutputMap.OutputMap[nRowCount].StartAddr = Convert.ToInt32(DGV_StartAddrCell.Value);
                        else
                            OutputMap.OutputMap[nRowCount].StartAddr = -1;

                        if (!string.IsNullOrEmpty((string)DGV_BitNumCell.Value))
                            OutputMap.OutputMap[nRowCount].Bit = Convert.ToInt32(DGV_BitNumCell.Value);
                        else
                            OutputMap.OutputMap[nRowCount].Bit = -1;

                        OutputMap.OutputMap[nRowCount].bInvert = Convert.ToBoolean(DGV_InvertCell.Value);

                        OutputMap.OutputMap[nRowCount].Name = Convert.ToString(DGV_NameCell.Value);

                        if (!OutputMap.OutputMap[nRowCount].IsValidStartAddrRange() && OutputMap.OutputMap[nRowCount].StartAddr != -1)
                        {
                            OutputMap.OutputMap[nRowCount].StartAddr = -1;
                            throw new Exception($"StartAddr Out of Range");
                        }
                        if (!OutputMap.OutputMap[nRowCount].IsValidBitRange() && OutputMap.OutputMap[nRowCount].Bit != -1)
                        {
                            OutputMap.OutputMap[nRowCount].Bit = -1;
                            throw new Exception($"Bit Out of Range");
                        }
                    }
                }

                DGV.CurrentCell = null;
                return true;
            }
            catch(Exception ex)
            {
                _Exception = ex;
                return false;
            }
        }
        

        private void LoadInputGridView(ref DataGridView DGV, Port port)
        {
            if (port.GetParam().ePortType == Port.PortType.MGV_AGV ||
                    port.GetParam().ePortType == Port.PortType.MGV ||
                    port.GetParam().ePortType == Port.PortType.AGV)
            {
                foreach (Port.AGV_MGV_InputItem item in Enum.GetValues(typeof(Port.AGV_MGV_InputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.InputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }
            else if (port.GetParam().ePortType == Port.PortType.OHT || port.GetParam().ePortType == Port.PortType.MGV_OHT)
            {
                foreach (Port.OHT_InputItem item in Enum.GetValues(typeof(Port.OHT_InputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.InputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }
            else if (port.GetParam().ePortType == Port.PortType.Conveyor_AGV || port.GetParam().ePortType == Port.PortType.Conveyor_OMRON)
            {
                foreach (Port.Conveyor_InputItem item in Enum.GetValues(typeof(Port.Conveyor_InputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.InputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }
            else if (port.GetParam().ePortType == Port.PortType.EQ)
            {
                foreach (Port.EQ_InputItem item in Enum.GetValues(typeof(Port.EQ_InputItem)))
                {
                    string Name = item.ToString();
                    string StartAddr = string.Empty;
                    string Bit = string.Empty;
                    string MapStatus = port.WMX_IO_ItemToMapData(item) == null ? "None" : port.WMX_IO_ItemToMapData(item).ToString();
                    string WMXStatus = string.Empty;
                    string Invert = false.ToString();


                    foreach (var IOParam in port.GetMotionParam().Ctrl_IO.InputMap)
                    {
                        if (IOParam.Name == Name)
                        {
                            StartAddr = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                            Bit = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                            Invert = IOParam.bInvert.ToString();
                            break;
                        }
                    }

                    DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
                }
            }
            //var InputMap = port.GetMotionParam().Ctrl_IO;

            //for (int nRowCount = 0; nRowCount < InputMap.InputMap.Length; nRowCount++)
            //{
            //    bool bRowAdd = false;

            //    if (port.GetParam().ePortType == Port.PortType.MGV_AGV ||
            //        port.GetParam().ePortType == Port.PortType.MGV ||
            //        port.GetParam().ePortType == Port.PortType.AGV)
            //    {
            //        if (Enum.IsDefined(typeof(Port.AGV_MGV_InputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.AGV_MGV_InputItem eAGV_MGV_InputItem = (Port.AGV_MGV_InputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eAGV_MGV_InputItem}";
            //            bRowAdd = true;
            //        }
            //    }
            //    else if (port.GetParam().ePortType == Port.PortType.OHT || port.GetParam().ePortType == Port.PortType.MGV_OHT)
            //    {
            //        if (Enum.IsDefined(typeof(Port.OHT_InputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.OHT_InputItem eOHT_InputItem = (Port.OHT_InputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eOHT_InputItem}";
            //            bRowAdd = true;
            //        }
            //    }
            //    else if (port.GetParam().ePortType == Port.PortType.Conveyor)
            //    {
            //        if (Enum.IsDefined(typeof(Port.Conveyor_InputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.Conveyor_InputItem eConveyor_InputItem = (Port.Conveyor_InputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eConveyor_InputItem}";
            //            bRowAdd = true;
            //        }
            //    }
            //    else if (port.GetParam().ePortType == Port.PortType.EQ)
            //    {
            //        if (Enum.IsDefined(typeof(Port.EQ_InputItem), nRowCount))
            //        {
            //            DGV.Rows.Add();
            //            Port.EQ_InputItem eEQ_InputItem = (Port.EQ_InputItem)nRowCount;
            //            DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //            DGV_NameCell.Value = $"{eEQ_InputItem}";
            //            bRowAdd = true;
            //        }
            //    }


            //    if (bRowAdd)
            //    {
            //        DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
            //        DataGridViewCell DGV_StartAddrCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.StartAddr];
            //        DataGridViewCell DGV_BitNumCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.BitNum];
            //        DataGridViewCell DGV_WMXStatusCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.WMXStatus];
            //        DataGridViewCell DGV_InvertCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.Invert];

            //        for (int nCount = 0; nCount < InputMap.OutputMap.Length; nCount++)
            //        {
            //            var IOParam = InputMap.InputMap[nCount];

            //            try
            //            {
            //                if (string.IsNullOrEmpty((string)DGV_NameCell.Value))
            //                    continue;

            //                if (Convert.ToString(DGV_NameCell.Value) == IOParam.Name && !string.IsNullOrEmpty(IOParam.Name))
            //                {
            //                    DGV_StartAddrCell.Value = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : string.Empty;
            //                    DGV_BitNumCell.Value = IOParam.Bit != -1 ? IOParam.Bit.ToString() : string.Empty;
            //                    DGV_WMXStatusCell.Value = $"Off";
            //                    DGV_InvertCell.Value = IOParam.bInvert.ToString();
            //                    break;
            //                }
            //            }
            //            catch
            //            {

            //            }
            //        }
            //    }
            //}
        }
        private bool ApplyInputGridView(ref DataGridView DGV, Port port, ref Exception _Exception)
        {
            try
            {
                var InputMap = port.GetMotionParam().Ctrl_IO;

                for (int nRowCount = 0; nRowCount < InputMap.InputMap.Length; nRowCount++)
                {
                    if (nRowCount >= DGV.Rows.Count)
                    {
                        //Default
                        InputMap.InputMap[nRowCount].Name = string.Empty;
                        InputMap.InputMap[nRowCount].StartAddr = -1;
                        InputMap.InputMap[nRowCount].Bit = -1;
                        InputMap.InputMap[nRowCount].bInvert = false;
                    }
                    else
                    {
                        DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
                        DataGridViewCell DGV_StartAddrCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.StartAddr];
                        DataGridViewCell DGV_BitNumCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.BitNum];
                        DataGridViewCell DGV_InvertCell = DGV.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.Invert];
                        DGV.CurrentCell = DGV_BitNumCell;

                        InputMap.InputMap[nRowCount].Name = Convert.ToString(DGV_NameCell.Value);

                        if (!string.IsNullOrEmpty((string)DGV_StartAddrCell.Value))
                            InputMap.InputMap[nRowCount].StartAddr = Convert.ToInt32(DGV_StartAddrCell.Value);
                        else
                            InputMap.InputMap[nRowCount].StartAddr = -1;

                        if (!string.IsNullOrEmpty((string)DGV_BitNumCell.Value))
                            InputMap.InputMap[nRowCount].Bit = Convert.ToInt32(DGV_BitNumCell.Value);
                        else
                            InputMap.InputMap[nRowCount].Bit = -1;

                        InputMap.InputMap[nRowCount].bInvert = Convert.ToBoolean(DGV_InvertCell.Value);

                        if (!InputMap.InputMap[nRowCount].IsValidStartAddrRange() && InputMap.InputMap[nRowCount].StartAddr != -1)
                        {
                            InputMap.InputMap[nRowCount].StartAddr = -1;
                            throw new Exception($"StartAddr Out of Range");
                        }
                        if (!InputMap.InputMap[nRowCount].IsValidBitRange() && InputMap.InputMap[nRowCount].Bit != -1)
                        {
                            InputMap.InputMap[nRowCount].Bit = -1;
                            throw new Exception($"Bit Out of Range");
                        }
                    }
                }

                DGV.CurrentCell = null;
                return true;
            }
            catch(Exception ex)
            {
                _Exception = ex;
                return false;
            }
        }

        /// <summary>
        /// Motion Parmaeter, CV Parameter
        /// </summary>
        private void MotionTypeToParamUpdate()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
            {
                if (tableLayoutPanel_MotionParam.Controls.Count > 0)
                    tableLayoutPanel_MotionParam.Controls.Clear();
            }
            else
            {
                if ((string)tableLayoutPanel_MotionParam.Tag != CurrentPort.GetParam().ID)
                {
                    ClearMotionDGV();
                    MotionTypeGridViewLoad(CurrentPort);
                    tableLayoutPanel_MotionParam.Tag = CurrentPort.GetParam().ID;
                }
            }
        }
        private void CVTypeToParamUpdate()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
            {
                if (tableLayoutPanel_BufferParam.Controls.Count > 0)
                    tableLayoutPanel_BufferParam.Controls.Clear();
            }
            else
            {
                if ((string)tableLayoutPanel_BufferParam.Tag != CurrentPort.GetParam().ID)
                {
                    ClearBufferCVDGV();
                    CVTypeGridViewLoad(CurrentPort);
                    tableLayoutPanel_BufferParam.Tag = CurrentPort.GetParam().ID;
                }
            }
        }
        private void MotionTypeGridViewLoad(Port port)
        {
            int nColumnCount = 0;

            int nCount = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_X : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_LP_X : 0;
            int nMaxCount = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_T : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_OP_T : 0;

                //3축 짜리
            for(; nCount <= nMaxCount; nCount++)
            {
                Port.PortAxis ePortAxis = (Port.PortAxis)nCount;
                if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo)
                {
                    port.Load_DGV_ServoGridView(ref DGV_MotionParam[nCount], ePortAxis);
                }
                else if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Inverter)
                {
                    port.Load_DGV_InverterGridView(ref DGV_MotionParam[nCount], ePortAxis);
                }
                else if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Cylinder)
                {
                    port.Load_DGV_CylinderGridView(ref DGV_MotionParam[nCount], ePortAxis);
                }

                if(!port.GetMotionParam().IsAxisUnUsed(ePortAxis))
                {
                    tableLayoutPanel_MotionParam.Controls.Add(DGV_MotionParam[nCount], (nColumnCount >= 3) ? nColumnCount-3 : nColumnCount, (nColumnCount >= 3) ? 1 : 0);
                    DGV_MotionParam[nCount].Dock = DockStyle.Fill;
                    nColumnCount++;
                }
            }
        }
        private void CVTypeGridViewLoad(Port port)
        {
            int LPorOPCount = 0;
            int BPCount = 0;
            if (port.IsBufferControlPort())
            {
                foreach(Port.BufferCV eBufferCV in Enum.GetValues(typeof(Port.BufferCV)))
                {
                    if (port.GetMotionParam().GetBufferControlEnable(eBufferCV) == Port.CVCtrlEnable.Enable)
                    {
                        port.Load_DGV_CVGridView(ref DGV_BufferCVParam[(int)eBufferCV], eBufferCV);
                    }

                    if (port.GetMotionParam().IsCVUsed(eBufferCV))
                    {
                        if(eBufferCV == Port.BufferCV.Buffer_LP)
                            tableLayoutPanel_BufferParam.Controls.Add(DGV_BufferCVParam[(int)eBufferCV], LPorOPCount++, 0);
                        else if (eBufferCV == Port.BufferCV.Buffer_OP)
                            tableLayoutPanel_BufferParam.Controls.Add(DGV_BufferCVParam[(int)eBufferCV], LPorOPCount, 0);
                        else if (eBufferCV == Port.BufferCV.Buffer_BP1)
                            tableLayoutPanel_BufferParam.Controls.Add(DGV_BufferCVParam[(int)eBufferCV], BPCount++, 1);
                        else if (eBufferCV == Port.BufferCV.Buffer_BP2)
                            tableLayoutPanel_BufferParam.Controls.Add(DGV_BufferCVParam[(int)eBufferCV], BPCount++, 1);
                        else if (eBufferCV == Port.BufferCV.Buffer_BP3)
                            tableLayoutPanel_BufferParam.Controls.Add(DGV_BufferCVParam[(int)eBufferCV], BPCount++, 1);
                        else if (eBufferCV == Port.BufferCV.Buffer_BP4)
                            tableLayoutPanel_BufferParam.Controls.Add(DGV_BufferCVParam[(int)eBufferCV], BPCount++, 1);
                        DGV_BufferCVParam[(int)eBufferCV].Dock = DockStyle.Fill;
                    }
                }
            }
        }


        /// <summary>
        /// WMX Parameter
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="port"></param>
        private void WMXParamGridViewInit(ref DataGridView DGV, Port port)
        {
            DGV = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(DGV)).BeginInit();

            DGV.AllowUserToAddRows = false;
            DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            DGV.BackgroundColor = System.Drawing.Color.AliceBlue;
            DGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            DGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;

            // 
            // Column6
            // 
            DataGridViewCellStyle NameColumnCellStyle = new DataGridViewCellStyle();
            DataGridViewTextBoxColumn DGVNameColumn = new DataGridViewTextBoxColumn();
            DGVNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            NameColumnCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DGVNameColumn.FillWeight = 100;
            DGVNameColumn.DefaultCellStyle = NameColumnCellStyle;
            DGVNameColumn.HeaderText = $"Parameters Name";
            DGVNameColumn.Name = "Column6";
            DGVNameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            DGVNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            DGVNameColumn.ReadOnly = true;
            DGV.Columns.Add(DGVNameColumn);
            // 
            // Column5
            // 


            int nPortAxis = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_X : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_LP_X : 0;
            int nMaxPortAxis = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_T : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_OP_T : 0;

            for (; nPortAxis <= nMaxPortAxis; nPortAxis++)
            {
                Port.PortAxis ePortAxis = (Port.PortAxis)nPortAxis;
                if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo &&
                    port.GetMotionParam().IsValidServo(ePortAxis))
                {
                    DataGridViewCellStyle ValueColumnCellStyle = new DataGridViewCellStyle();
                    DataGridViewTextBoxColumn DGVValueColumn = new DataGridViewTextBoxColumn();
                    DGVValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                    ValueColumnCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                    DGVValueColumn.FillWeight = 70;
                    DGVValueColumn.DefaultCellStyle = ValueColumnCellStyle;
                    DGVValueColumn.HeaderText = $"{ePortAxis} [{port.GetMotionParam().GetServoAxisNum(ePortAxis)}]";
                    DGVValueColumn.Name = "Column5";
                    DGVValueColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                    DGVValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

                    DGV.Columns.Add(DGVValueColumn);
                }
            }
            


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
            //DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { DGVNameColumn, DGVValueColumn });

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
            DGV.Margin = new Padding(5, 5, 5, 5);

            ((System.ComponentModel.ISupportInitialize)(DGV)).EndInit();

            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(WMXParameterGridViewRow)).Length; nRowCount++)
            {
                DGV.Rows.Add();
            }

            for (int nColumnCount = 1; nColumnCount < DGV.Columns.Count; nColumnCount++)
            {
                try
                {
                    DataGridViewComboBoxCell cbxCell_MotorDirection = new DataGridViewComboBoxCell();
                    cbxCell_MotorDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    for (int nCount = 0; nCount < Enum.GetNames(typeof(MovenCore.WMXParam.m_motorDirection)).Length; nCount++)
                        cbxCell_MotorDirection.Items.Add(((MovenCore.WMXParam.m_motorDirection)nCount).ToString());

                    DGV.Rows[(int)WMXParameterGridViewRow.MotorDirection].Cells[nColumnCount] = cbxCell_MotorDirection;

                    DataGridViewComboBoxCell cbxCell_HomeType = new DataGridViewComboBoxCell();
                    cbxCell_HomeType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    for (int nCount = 0; nCount < Enum.GetNames(typeof(MovenCore.WMXParam.m_homeType)).Length; nCount++)
                        cbxCell_HomeType.Items.Add(((MovenCore.WMXParam.m_homeType)nCount).ToString());

                    DGV.Rows[(int)WMXParameterGridViewRow.HomeType].Cells[nColumnCount] = cbxCell_HomeType;

                    DataGridViewComboBoxCell cbxCell_HomeDirection = new DataGridViewComboBoxCell();
                    cbxCell_HomeDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    for (int nCount = 0; nCount < Enum.GetNames(typeof(MovenCore.WMXParam.m_homeDirection)).Length; nCount++)
                        cbxCell_HomeDirection.Items.Add(((MovenCore.WMXParam.m_homeDirection)nCount).ToString());

                    DGV.Rows[(int)WMXParameterGridViewRow.HomeDirection].Cells[nColumnCount] = cbxCell_HomeDirection;


                    DataGridViewComboBoxCell cbxCell_UseAbsEncorderModeSelect = new DataGridViewComboBoxCell();
                    cbxCell_UseAbsEncorderModeSelect.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cbxCell_UseAbsEncorderModeSelect.Items.Add("false");
                    cbxCell_UseAbsEncorderModeSelect.Items.Add("true");

                    DGV.Rows[(int)WMXParameterGridViewRow.AbsoluteEncorderMode].Cells[nColumnCount] = cbxCell_UseAbsEncorderModeSelect;


                    DataGridViewComboBoxCell cbxCell_SoftLimitSwitchType = new DataGridViewComboBoxCell();
                    cbxCell_SoftLimitSwitchType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    for (int nCount = 0; nCount < Enum.GetNames(typeof(MovenCore.WMXParam.m_limitSwitchType)).Length; nCount++)
                        cbxCell_SoftLimitSwitchType.Items.Add(((MovenCore.WMXParam.m_limitSwitchType)nCount).ToString());

                    DGV.Rows[(int)WMXParameterGridViewRow.SoftLimitSwitchType].Cells[nColumnCount] = cbxCell_SoftLimitSwitchType;



                    DataGridViewComboBoxCell cbxCell_NegLimitSwitchBoolSelect = new DataGridViewComboBoxCell();
                    cbxCell_NegLimitSwitchBoolSelect.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cbxCell_NegLimitSwitchBoolSelect.Items.Add("false");
                    cbxCell_NegLimitSwitchBoolSelect.Items.Add("true");

                    DGV.Rows[(int)WMXParameterGridViewRow.InvertNegLimitSwitch].Cells[nColumnCount] = cbxCell_NegLimitSwitchBoolSelect;



                    DataGridViewComboBoxCell cbxCell_PosLimitSwitchBoolSelect = new DataGridViewComboBoxCell();
                    cbxCell_PosLimitSwitchBoolSelect.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cbxCell_PosLimitSwitchBoolSelect.Items.Add("false");
                    cbxCell_PosLimitSwitchBoolSelect.Items.Add("true");

                    DGV.Rows[(int)WMXParameterGridViewRow.InvertPosLimitSwitch].Cells[nColumnCount] = cbxCell_PosLimitSwitchBoolSelect;
                }
                catch
                {

                }
            }
        }
        private void WMXParamDataInit(ref DataGridView DGV, Port port)
        {
            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(WMXParameterGridViewRow)).Length; nRowCount++)
            {
                WMXParameterGridViewRow eWMXParameterGridViewRow = (WMXParameterGridViewRow)nRowCount;

                int nCount = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_X : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_LP_X : 0;
                int nMaxCount = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_T : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_OP_T : 0;

                int nColumnCount = 1;


                for (; nCount <= nMaxCount; nCount++)
                {
                    Port.PortAxis ePortAxis = (Port.PortAxis)nCount;
                    if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo &&
                        port.GetMotionParam().IsValidServo(ePortAxis))
                    {
                        DataGridViewCell DGV_NameCell = DGV.Rows[nRowCount].Cells[0];
                        DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[nColumnCount];
                        var MotionParam = port.GetMotionParam();
                        var ServoParam = port.GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);

                        switch (eWMXParameterGridViewRow)
                        {
                            case WMXParameterGridViewRow.GearRatioNum:
                                DGV_NameCell.Value = $"Gear Ratio Numerator";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_gearRatioNum}";
                                break;
                            case WMXParameterGridViewRow.GearRatioDen:
                                DGV_NameCell.Value = $"Gear Ratio Denominator";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_gearRatioDen}";
                                break;
                            case WMXParameterGridViewRow.MotorDirection:
                                DGV_NameCell.Value = $"Motor Direction";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_motorDirection}";
                                break;
                            case WMXParameterGridViewRow.InposWidth:
                                DGV_NameCell.Value = $"Inpos Width [mm or °]";
                                DGV_Value.Value = $"{Port.WMXPosToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_inposWidth)}";
                                break;
                            case WMXParameterGridViewRow.PosSetWidth:
                                DGV_NameCell.Value = $"Pos Set Width [mm or °]";
                                DGV_Value.Value = $"{Port.WMXPosToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_posSetWidth)}";
                                break;
                            case WMXParameterGridViewRow.HomeType:
                                DGV_NameCell.Value = $"Home Type";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_homeType}";
                                break;
                            case WMXParameterGridViewRow.HomeDirection:
                                DGV_NameCell.Value = $"Home Direction";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_homeDirection}";
                                break;
                            case WMXParameterGridViewRow.HomeFastVelocity:
                                DGV_NameCell.Value = $"Home Fast Velocity [m/min or °/min]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeFastVelocity)}";
                                break;
                            case WMXParameterGridViewRow.HomeFastAcc:
                                DGV_NameCell.Value = $"Home Fast Acc [m/min^2 or °/min^2]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeFastAcc)}";
                                break;
                            case WMXParameterGridViewRow.HomeFastDec:
                                DGV_NameCell.Value = $"Home Fast Dec [m/min^2 or °/min^2]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeFastDec)}";
                                break;
                            case WMXParameterGridViewRow.HomeSlowVelocity:
                                DGV_NameCell.Value = $"Home Slow Velocity [m/min or °/min]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeSlowVelocity)}";
                                break;
                            case WMXParameterGridViewRow.HomeSlowAcc:
                                DGV_NameCell.Value = $"Home Slow Acc [m/min^2 or °/min^2]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeSlowAcc)}";
                                break;
                            case WMXParameterGridViewRow.HomeSlowDec:
                                DGV_NameCell.Value = $"Home Slow Dec [m/min^2 or °/min^2]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeSlowDec)}";
                                break;
                            case WMXParameterGridViewRow.HomeShiftVelocity:
                                DGV_NameCell.Value = $"Home Shift Velocity [m/min or °/min]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeShiftVelocity)}";
                                break;
                            case WMXParameterGridViewRow.HomeShiftAcc:
                                DGV_NameCell.Value = $"Home Shift Acc [m/min^2 or °/min^2]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeShiftAcc)}";
                                break;
                            case WMXParameterGridViewRow.HomeShiftDec:
                                DGV_NameCell.Value = $"Home Shift Dec [m/min^2 or °/min^2]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeShiftDec)}";
                                break;
                            case WMXParameterGridViewRow.HomeShiftDistance:
                                DGV_NameCell.Value = $"Home Shift Distance [mm]";
                                DGV_Value.Value = $"{Port.WMXPosToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_homeShiftDistance)}";
                                break;
                            case WMXParameterGridViewRow.AbsoluteEncorderMode:
                                DGV_NameCell.Value = $"Absolute Encorder Mode";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_absEncoderMode.ToString().ToLower()}";
                                break;
                            case WMXParameterGridViewRow.AbsoluteEncorderOffset:
                                {
                                    int nAxis = port.GetMotionParam().GetServoAxisNum(ePortAxis);
                                    WMXMotion.AxisParameter EngineAxisParameter = new WMXMotion.AxisParameter();
                                    port.WMXParam_Load_EngineToProg(nAxis, EngineAxisParameter);

                                    double EncorderHomeOffset = Port.WMXPosToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_absEncoderHomeOffset);
                                    EncorderHomeOffset = EncorderHomeOffset * EngineAxisParameter.m_gearRatioDen / EngineAxisParameter.m_gearRatioNum;

                                    DGV_NameCell.Value = $"Absolute Encorder Offset [mm or °]";
                                    DGV_Value.Value = $"{EncorderHomeOffset}";
                                    break;
                                }
                            case WMXParameterGridViewRow.SoftLimitSwitchType:
                                DGV_NameCell.Value = $"Soft Limit Switch Type";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_softLimitSwitchType}";
                                break;
                            case WMXParameterGridViewRow.SoftLimitPosValue:
                                DGV_NameCell.Value = $"Soft Limit Positive Value [mm or °]";
                                DGV_Value.Value = $"{Port.WMXPosToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_softLimitPosValue)}";
                                break;
                            case WMXParameterGridViewRow.SoftLimitNegValue:
                                DGV_NameCell.Value = $"Soft Limit Negative Value [mm or °]";
                                DGV_Value.Value = $"{Port.WMXPosToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_softLimitNegValue)}";
                                break;
                            case WMXParameterGridViewRow.InvertPosLimitSwitch:
                                DGV_NameCell.Value = $"Invert Pos Limit Switch";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_invertPosLimit.ToString().ToLower()}";
                                break;
                            case WMXParameterGridViewRow.InvertNegLimitSwitch:
                                DGV_NameCell.Value = $"Invert Neg Limit Switch";
                                DGV_Value.Value = $"{ServoParam.WMXParam.m_invertNegLimit.ToString().ToLower()}";
                                break;
                            case WMXParameterGridViewRow.QuickStopDecel:
                                DGV_NameCell.Value = $"Quick Stop Decel [m/min^2 or °/min^2]";
                                DGV_Value.Value = $"{Port.WMXVelToProgramUnit(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, ServoParam.WMXParam.m_quickStopDecel)}";
                                break;
                        }

                        nColumnCount++;
                    }
                }
            }

            DGV.CurrentCell = null;
        }
        private void LoadWMXParameterGridView(ref DataGridView DGV, Port port)
        {
            //Shuttle 제어인 경우 셔틀 X ~ T
            //Buffer 제어인 경우 Buffer LP X ~ Buffer OP T
            int nPortAxis           = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_X : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_LP_X : 0;
            int nMaxPortAxis        = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_T : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_OP_T : 0;
            int UsedCount = 0;

            for (; nPortAxis <= nMaxPortAxis; nPortAxis++)
            {
                Port.PortAxis ePortAxis = (Port.PortAxis)nPortAxis;
                if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo &&
                    port.GetMotionParam().IsValidServo(ePortAxis))
                {
                    UsedCount++;
                }
            }

            if (UsedCount > 0)
            {
                WMXParamGridViewInit(ref DGV, port);
                WMXParamDataInit(ref DGV, port);
                pnl_WMXParamGrid.Controls.Add(DGV);
                DGV.Dock = DockStyle.Fill;

                ButtonFunc.SetVisible(btn_EngineParameterLoad, true);
                ButtonFunc.SetVisible(btn_WMXParamSave, true);
            }
            else
            {
                ButtonFunc.SetVisible(btn_EngineParameterLoad, false);
                ButtonFunc.SetVisible(btn_WMXParamSave, false);
            }
        }
        private bool ApplyWMXParameterGridView(ref DataGridView DGV, Port port)
        {
            try
            {
                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(WMXParameterGridViewRow)).Length; nRowCount++)
                {
                    WMXParameterGridViewRow eWMXParameterGridViewRow = (WMXParameterGridViewRow)nRowCount;

                    int nCount = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_X : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_LP_X : 0;
                    int nMaxCount = port.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_T : port.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_OP_T : 0;

                    int nColumnCount = 1;


                    for (; nCount <= nMaxCount; nCount++)
                    {
                        Port.PortAxis ePortAxis = (Port.PortAxis)nCount;
                        if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo &&
                            port.GetMotionParam().IsValidServo(ePortAxis))
                        {
                            DataGridViewCell DGV_Value = DGV.Rows[nRowCount].Cells[nColumnCount];
                            DGV.CurrentCell = DGV_Value;
                            var MotionParam = port.GetMotionParam();
                            var ServoParam = port.GetMotionParam().GetShuttleCtrl_ServoParam(ePortAxis);

                            switch (eWMXParameterGridViewRow)
                            {
                                case WMXParameterGridViewRow.GearRatioNum:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_gearRatioNum = Convert.ToDouble(DGV_Value.Value);
                                    break;
                                case WMXParameterGridViewRow.GearRatioDen:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_gearRatioDen = Convert.ToDouble(DGV_Value.Value);
                                    break;
                                case WMXParameterGridViewRow.MotorDirection:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_motorDirection = (MovenCore.WMXParam.m_motorDirection)Enum.Parse(typeof(MovenCore.WMXParam.m_motorDirection), Convert.ToString(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.InposWidth:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_inposWidth = Port.ProgramUnitToWMXPos(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.PosSetWidth:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_posSetWidth = Port.ProgramUnitToWMXPos(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeType:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeType = (MovenCore.WMXParam.m_homeType)Enum.Parse(typeof(MovenCore.WMXParam.m_homeType), Convert.ToString(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeDirection:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeDirection = (MovenCore.WMXParam.m_homeDirection)Enum.Parse(typeof(MovenCore.WMXParam.m_homeDirection), Convert.ToString(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeFastVelocity:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeFastVelocity = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeFastAcc:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeFastAcc = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeFastDec:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeFastDec = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeSlowVelocity:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeSlowVelocity = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeSlowAcc:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeSlowAcc = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeSlowDec:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeSlowDec = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeShiftVelocity:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeShiftVelocity = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeShiftAcc:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeShiftAcc = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeShiftDec:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeShiftDec = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.HomeShiftDistance:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_homeShiftDistance = Port.ProgramUnitToWMXPos(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.AbsoluteEncorderMode:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_absEncoderMode = Convert.ToBoolean(Convert.ToString(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.AbsoluteEncorderOffset:
                                    if ((string)DGV_Value.Value != string.Empty)
                                    {
                                        int nAxis = port.GetMotionParam().GetServoAxisNum(ePortAxis);
                                        WMXMotion.AxisParameter EngineAxisParameter = new WMXMotion.AxisParameter();
                                        port.WMXParam_Load_EngineToProg(nAxis, EngineAxisParameter);

                                        double EncorderHomeOffset = Port.ProgramUnitToWMXPos(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                        EncorderHomeOffset = EncorderHomeOffset * EngineAxisParameter.m_gearRatioNum / EngineAxisParameter.m_gearRatioDen;
                                        ServoParam.WMXParam.m_absEncoderHomeOffset = EncorderHomeOffset;
                                    }
                                    break;
                                case WMXParameterGridViewRow.SoftLimitSwitchType:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_softLimitSwitchType = (MovenCore.WMXParam.m_limitSwitchType)Enum.Parse(typeof(MovenCore.WMXParam.m_limitSwitchType), Convert.ToString(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.SoftLimitPosValue:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_softLimitPosValue = Port.ProgramUnitToWMXPos(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.SoftLimitNegValue:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_softLimitNegValue = Port.ProgramUnitToWMXPos(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.InvertPosLimitSwitch:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_invertPosLimit = Convert.ToBoolean(Convert.ToString(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.InvertNegLimitSwitch:
                                    if ((string)DGV_Value.Value != string.Empty)
                                        ServoParam.WMXParam.m_invertNegLimit = Convert.ToBoolean(Convert.ToString(DGV_Value.Value));
                                    break;
                                case WMXParameterGridViewRow.QuickStopDecel:
                                    if ((string)DGV_Value.Value != string.Empty)
                                    {
                                        double InsertValue = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, Convert.ToDouble(DGV_Value.Value));
                                        double AutoRunDec = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, MotionParam.GetShuttleCtrl_ServoParam(ePortAxis).AutoRun_Dec);
                                        double ManualDec = Port.ProgramUnitToWMXVel(MotionParam.IsRotaryAxis(ePortAxis) ? Port.AxisType.Rotary : Port.AxisType.Linear, MotionParam.GetShuttleCtrl_ServoParam(ePortAxis).Manual_Dec);

                                        if(InsertValue <= AutoRunDec || InsertValue <= ManualDec)
                                        {
                                            double Bigger = AutoRunDec > ManualDec ? AutoRunDec : ManualDec;
                                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InvalidQuickStopDec"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LogMsg.AddPortLog(port.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidParameter, $"Insert Value: {InsertValue} <= Minimum Value: {Bigger}");
                                            return false;
                                        }

                                        ServoParam.WMXParam.m_quickStopDecel = InsertValue;
                                    }
                                    break;
                            }

                            nColumnCount++;
                        }
                    }
                }

                DGV.CurrentCell = null;
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Update Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                UIUpdateTimer.Enabled = false;
                return;
            }

            try
            {
                UIUpdateTimer.Interval = Master.UIUpdateIntervalTime;
                GroupBoxFunc.SetText(groupBox_WMXServoParameterSettings, SynusLangPack.GetLanguage("GroupBox_WMXServoParameterSettings"));
                GroupBoxFunc.SetText(groupBox_WMXIOParameterSettings, SynusLangPack.GetLanguage("GroupBox_WMXIOParameterSettings"));
                GroupBoxFunc.SetText(groupBox_PortErrorTimeSettings, SynusLangPack.GetLanguage("GroupBox_PortErrorTimeSettings"));
                GroupBoxFunc.SetText(groupBox_PortOverLoadSettings, SynusLangPack.GetLanguage("GorupBox_Port_OverloadSetting"));
                GroupBoxFunc.SetText(groupBox_PortOverLoadClear, SynusLangPack.GetLanguage("GorupBox_Port_OverloadClear"));
                GroupBoxFunc.SetText(groupBox_AutoRunMoveSpeed, SynusLangPack.GetLanguage("GroupBox_AutoRunMoveSpeed"));
                GroupBoxFunc.SetText(groupBox_PortControlParameterSettings, SynusLangPack.GetLanguage("GorupBox_PortControlParameterSettings"));
                GroupBoxFunc.SetText(groupBox_ConveyorControlSettings, SynusLangPack.GetLanguage("GorupBox_ConveyorControlParameterSettings"));
                GroupBoxFunc.SetText(groupBox_TagReadFailOptionSettings, SynusLangPack.GetLanguage("GorupBox_TagReadFailOptionSettings"));

                ButtonFunc.SetText(btn_X_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_X_Axis_OverLoadSettings_Send, btn_X_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_Z_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_Z_Axis_OverLoadSettings_Send, btn_Z_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_T_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_Send"));
                ButtonFunc.SetBackColor(btn_T_Axis_OverLoadSettings_Send, btn_T_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_All_Axis_OverLoadSettings_Send, SynusLangPack.GetLanguage("Btn_AllSend"));
                ButtonFunc.SetBackColor(btn_All_Axis_OverLoadSettings_Send, btn_All_Axis_OverLoadSettings_Send.Tag != null ? Color.Lime : Color.White);

                ButtonFunc.SetText(btn_X_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_Clear"));
                ButtonFunc.SetBackColor(btn_X_Axis_Detected_OverLoad_Clear, btn_X_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_Z_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_Clear"));
                ButtonFunc.SetBackColor(btn_Z_Axis_Detected_OverLoad_Clear, btn_Z_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_T_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_Clear"));
                ButtonFunc.SetBackColor(btn_T_Axis_Detected_OverLoad_Clear, btn_T_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetText(btn_All_Axis_Detected_OverLoad_Clear, SynusLangPack.GetLanguage("Btn_AllClear"));
                ButtonFunc.SetBackColor(btn_All_Axis_Detected_OverLoad_Clear, btn_All_Axis_Detected_OverLoad_Clear.Tag != null ? Color.Lime : Color.White);

                ButtonFunc.SetBackColor(btn_AutoSpeed1, btn_AutoSpeed1.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetBackColor(btn_AutoSpeed2, btn_AutoSpeed2.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetBackColor(btn_AutoSpeed3, btn_AutoSpeed3.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetBackColor(btn_AutoSpeed4, btn_AutoSpeed4.Tag != null ? Color.Lime : Color.White);
                ButtonFunc.SetBackColor(btn_AutoSpeed5, btn_AutoSpeed5.Tag != null ? Color.Lime : Color.White);


                ButtonFunc.SetText(btn_DGVMotionParamApplyAndSave, SynusLangPack.GetLanguage("Btn_ParamSave"));
                ButtonFunc.SetText(btn_DGVBufferParamApplyAndSave, SynusLangPack.GetLanguage("Btn_ParamSave"));
                ButtonFunc.SetText(btn_IOMapApplyAndSave, SynusLangPack.GetLanguage("Btn_ParamSave"));

                ButtonFunc.SetText(btn_EngineParameterLoad, SynusLangPack.GetLanguage("Btn_Refresh"));
                
                ButtonFunc.SetText(btn_WMXParamSave, SynusLangPack.GetLanguage("Btn_ParamSave"));

                ButtonFunc.SetText(btn_WatchdogParamSave, SynusLangPack.GetLanguage("Btn_SavetoPort"));
                ButtonFunc.SetText(btn_WatchdogParamAllSave, SynusLangPack.GetLanguage("Btn_SavetoAllPort"));

                ButtonFunc.SetText(btn_WorkingErrorStop, SynusLangPack.GetLanguage("Btn_WorkStopAndError"));
                ButtonFunc.SetText(btn_WorkingContinuous, SynusLangPack.GetLanguage("Btn_WorkContinuousAndWarning"));

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    CurrentPort.UpdateWatchdogSettings(ref DGV_WatchdogSettings);
                    CurrentPort.Update_Lbl_AutoRunSpeed(ref lbl_AutoRunSpeedSetValue);

                    if (tabcontrol_Settings.Tag != CurrentPort)
                    {
                        TabPageVisibleUpdate(CurrentPort);
                        tabcontrol_Settings.Tag = CurrentPort;
                    }

                    if (CurrentPort.GetParam().eTagReaderType == Equipment.Port.TagReader.TagReaderType.RFID)
                    {
                        GroupBoxFunc.SetEnable(groupBox_RFID, true);
                        GroupBoxFunc.SetEnable(groupBox_BCR, false);
                    }
                    else if (CurrentPort.GetParam().eTagReaderType == Equipment.Port.TagReader.TagReaderType.BCR)
                    {
                        GroupBoxFunc.SetEnable(groupBox_RFID, false);
                        GroupBoxFunc.SetEnable(groupBox_BCR, true);

                        //BCR Update
                    }
                    else
                    {
                        GroupBoxFunc.SetEnable(groupBox_RFID, false);
                        GroupBoxFunc.SetEnable(groupBox_BCR, false);
                    }

                    btn_RFIDTagRead.BackColor       = btn_RFIDTagRead.Enabled ? Color.White : Color.DarkGray;
                    btn_SetRTA.BackColor            = btn_SetRTA.Enabled ? Color.White : Color.DarkGray;
                    btn_SetRTB.BackColor            = btn_SetRTB.Enabled ? Color.White : Color.DarkGray;
                    btn_SetVerboseReadTimeout.BackColor = btn_SetVerboseReadTimeout.Enabled ? Color.White : Color.DarkGray;
                    btn_ANT1Enable.BackColor        = btn_ANT1Enable.Enabled ? Color.White : Color.DarkGray;
                    btn_ANT2Enable.BackColor        = btn_ANT2Enable.Enabled ? Color.White : Color.DarkGray;
                    btn_ANT1ANT2Enable.BackColor    = btn_ANT1ANT2Enable.Enabled ? Color.White : Color.DarkGray;
                    btn_SetVerboseMode.BackColor    = btn_SetVerboseMode.Enabled ? Color.White : Color.DarkGray;
                    btn_SetAutoReadMode.BackColor   = btn_SetAutoReadMode.Enabled ? Color.White : Color.DarkGray;
                    btn_GetOperationMode.BackColor  = btn_GetOperationMode.Enabled ? Color.White : Color.DarkGray;
                    btn_RFIDSave.BackColor          = btn_RFIDSave.Enabled ? Color.White : Color.DarkGray;

                    btn_BCRAutoSettings.BackColor   = btn_BCRAutoSettings.Enabled ? Color.White : Color.DarkGray;
                    btn_BCRTagRead.BackColor        = btn_BCRTagRead.Enabled ? Color.White : Color.DarkGray;
                    btn_BCRAlignModeOn.BackColor    = btn_BCRAlignModeOn.Enabled ? Color.White : Color.DarkGray;
                    btn_BCRAlignModeOff.BackColor   = btn_BCRAlignModeOff.Enabled ? Color.White : Color.DarkGray;
                    btn_BCRTeachIn.BackColor        = btn_BCRTeachIn.Enabled ? Color.White : Color.DarkGray;
                    btn_BCRGetStatus.BackColor      = btn_BCRGetStatus.Enabled ? Color.White : Color.DarkGray;

                    ButtonFunc.SetBackColor(btn_WorkingErrorStop, CurrentPort.GetMotionParam().TagReadFailError ? Color.Lime : Color.White);
                    ButtonFunc.SetBackColor(btn_WorkingContinuous, !CurrentPort.GetMotionParam().TagReadFailError ? Color.Lime : Color.White);
                }
                else
                {
                    if (DGV_WatchdogSettings.Rows.Count > 0)
                        DGV_WatchdogSettings.Rows.Clear();
                }

                UpdateSetMaxLoadValue();
                UpdateDetectedMaxLoadValue();
                UpdateTagInfo();
                MotionTypeToParamUpdate();
                CVTypeToParamUpdate();
                UpdateWMXParamGridView();
                IOParamUpdate();

                IOOutputValueUpdate();
                IOInputValueUpdate();


                for (int nCount = 0; nCount < DGV_MotionParam.Length; nCount++)
                    DataGridViewFunc.AutoRowSize(DGV_MotionParam[nCount], 40, 16, 30);

                for (int nCount = 0; nCount < DGV_BufferCVParam.Length; nCount++)
                    DataGridViewFunc.AutoRowSize(DGV_BufferCVParam[nCount], 40, 16, 30);

                TabPageFunc.SetText(tabPage_PortControlParam, SynusLangPack.GetLanguage("TabPage_PortControlParam"));
                TabPageFunc.SetText(tabPage_BufferCVControlParam, SynusLangPack.GetLanguage("TabPage_ConveyorControlParam"));
                TabPageFunc.SetText(tabPage_ServoParam, SynusLangPack.GetLanguage("TabPage_ServoParam"));
                TabPageFunc.SetText(tabPage_IOParam, SynusLangPack.GetLanguage("TabPage_IOParam"));
                TabPageFunc.SetText(tabPage_PortSafty, SynusLangPack.GetLanguage("TabPage_PortSafty"));
                TabPageFunc.SetText(tabPage_IDReader, SynusLangPack.GetLanguage("TabPage_IDReader"));
            }
            catch { }
            finally
            {

            }
        }
        private void TabPageVisibleUpdate(Port port)
        {
            TabPage tabpage = tabcontrol_Settings.SelectedTab;

            bool bInsertForm = false;
            foreach(Port.PortAxis ePortAxis in Enum.GetValues(typeof(Port.PortAxis)))
            {
                if (port.GetMotionParam().GetAxisControlType(ePortAxis) != Port.AxisCtrlType.None)
                {
                    bInsertForm = true;
                    break;
                }
            }

            if(bInsertForm)
            {
                if (!tabcontrol_Settings.TabPages.Contains(tabPage_PortControlParam))
                    tabcontrol_Settings.TabPages.Insert(0 <= tabcontrol_Settings.TabPages.Count ? 0 : tabcontrol_Settings.TabPages.Count, tabPage_PortControlParam);
            }
            else
            {
                if (tabcontrol_Settings.TabPages.Contains(tabPage_PortControlParam))
                    tabcontrol_Settings.TabPages.Remove(tabPage_PortControlParam);
            }


            bInsertForm = false;

            foreach (Port.BufferCV eBufferCV in Enum.GetValues(typeof(Port.BufferCV)))
            {
                if (port.GetMotionParam().IsCVUsed(eBufferCV) && port.IsBufferControlPort())
                {
                    bInsertForm = true;
                    //PortTabPageList.Add(tabPage_BufferCVControlParam);
                    break;
                }
            }

            if (bInsertForm)
            {
                if (!tabcontrol_Settings.TabPages.Contains(tabPage_BufferCVControlParam))
                    tabcontrol_Settings.TabPages.Insert(1 <= tabcontrol_Settings.TabPages.Count ? 1 : tabcontrol_Settings.TabPages.Count, tabPage_BufferCVControlParam);
            }
            else
            {
                if (tabcontrol_Settings.TabPages.Contains(tabPage_BufferCVControlParam))
                    tabcontrol_Settings.TabPages.Remove(tabPage_BufferCVControlParam);
            }

            bInsertForm = false;

            foreach (Port.PortAxis ePortAxis in Enum.GetValues(typeof(Port.PortAxis)))
            {
                if (port.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo)
                {
                    bInsertForm = true;
                    //PortTabPageList.Add(tabPage_ServoParam);
                    break;
                }
            }

            if (bInsertForm)
            {
                if (!tabcontrol_Settings.TabPages.Contains(tabPage_ServoParam))
                    tabcontrol_Settings.TabPages.Insert(2 <= tabcontrol_Settings.TabPages.Count ? 2 : tabcontrol_Settings.TabPages.Count, tabPage_ServoParam);
            }
            else
            {
                if (tabcontrol_Settings.TabPages.Contains(tabPage_ServoParam))
                    tabcontrol_Settings.TabPages.Remove(tabPage_ServoParam);
            }

            bInsertForm = false;

            if (port.GetPortOperationMode() == Port.PortOperationMode.MGV ||
                port.GetPortOperationMode() == Port.PortOperationMode.AGV ||
                port.GetPortOperationMode() == Port.PortOperationMode.OHT ||
                port.GetPortOperationMode() == Port.PortOperationMode.Conveyor ||
                port.GetPortOperationMode() == Port.PortOperationMode.EQ)
            {
                bInsertForm = true;
                //PortTabPageList.Add(tabPage_IOParam);
            }

            if (bInsertForm)
            {
                if (!tabcontrol_Settings.TabPages.Contains(tabPage_IOParam))
                    tabcontrol_Settings.TabPages.Insert(3 <= tabcontrol_Settings.TabPages.Count ? 3 : tabcontrol_Settings.TabPages.Count, tabPage_IOParam);
            }
            else
            {
                if (tabcontrol_Settings.TabPages.Contains(tabPage_IOParam))
                    tabcontrol_Settings.TabPages.Remove(tabPage_IOParam);
            }

            //if (port.GetPortOperationMode() != Port.PortOperationMode.EQ)
            //{
                if(!tabcontrol_Settings.TabPages.Contains(tabPage_PortSafty))
                    tabcontrol_Settings.TabPages.Insert(4 <= tabcontrol_Settings.TabPages.Count ? 4 : tabcontrol_Settings.TabPages.Count, tabPage_PortSafty);
            //}
            //else
            //{
              //  if (tabcontrol_Settings.TabPages.Contains(tabPage_PortSafty))
                //    tabcontrol_Settings.TabPages.Remove(tabPage_PortSafty);
            //}

            if (port.GetPortOperationMode() != Port.PortOperationMode.EQ)
            {
                if (!tabcontrol_Settings.TabPages.Contains(tabPage_IDReader))
                    tabcontrol_Settings.TabPages.Insert(5 <= tabcontrol_Settings.TabPages.Count ? 4 : tabcontrol_Settings.TabPages.Count, tabPage_IDReader);
            }
            else
            {
                if (tabcontrol_Settings.TabPages.Contains(tabPage_IDReader))
                    tabcontrol_Settings.TabPages.Remove(tabPage_IDReader);
            }

            //PortTabPageList.Add(tabPage_PortSafty);
            //PortTabPageList.Add(tabPage_IDReader);


            //bool bMatch = true;

            //if (tabcontrol_Settings.TabPages.Count == PortTabPageList.Count)
            //{
            //    for(int nCount = 0; nCount < tabcontrol_Settings.TabPages.Count; nCount++)
            //    {
            //        if (tabcontrol_Settings.TabPages[nCount] != PortTabPageList[nCount])
            //            bMatch = false;
            //    }
            //}
            //else
            //    bMatch = false;

            //if(!bMatch)
            //{
            //    tabcontrol_Settings.TabPages.Clear();
            //    for (int nCount = 0; nCount < PortTabPageList.Count; nCount++)
            //        tabcontrol_Settings.TabPages.Add(PortTabPageList[nCount]);
            //}

            if (!tabcontrol_Settings.TabPages.Contains(tabpage))
                tabcontrol_Settings.SelectedTab = tabcontrol_Settings.TabPages.Count > 0 ? tabcontrol_Settings.TabPages[0] : null;
            else
                tabcontrol_Settings.SelectedTab = tabpage;
        }
        
        private void UpdateSetMaxLoadValue()
        {
            LabelFunc.SetText(lbl_XAxisMaxLoadTitle, SynusLangPack.GetLanguage("Label_XAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_ZAxisMaxLoadTitle, SynusLangPack.GetLanguage("Label_ZAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_TAxisMaxLoadTitle, SynusLangPack.GetLanguage("Label_TAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_AppliedValue, SynusLangPack.GetLanguage("Label_AppliedValue"));
            LabelFunc.SetText(lbl_SetValue, SynusLangPack.GetLanguage("Label_SetValue"));

            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LabelFunc.SetText(lbl_X_Axis_Read_SetMaxLoad, $"{CurrentPort.Motion_X_Axis_OverloadSettingTorque} %");
            LabelFunc.SetText(lbl_Z_Axis_Read_SetMaxLoad, $"{CurrentPort.Motion_Z_Axis_OverloadSettingTorque} %");
            LabelFunc.SetText(lbl_T_Axis_Read_SetMaxLoad, $"{CurrentPort.Motion_T_Axis_OverloadSettingTorque} %");
        }
        private void UpdateDetectedMaxLoadValue()
        {
            LabelFunc.SetText(lbl_XAxisMaxLoadTitle2, SynusLangPack.GetLanguage("Label_XAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_ZAxisMaxLoadTitle2, SynusLangPack.GetLanguage("Label_ZAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_TAxisMaxLoadTitle2, SynusLangPack.GetLanguage("Label_TAxisLoad") + " [%]");
            LabelFunc.SetText(lbl_DetectedValue, SynusLangPack.GetLanguage("Label_DetectValue"));

            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LabelFunc.SetText(lbl_X_Axis_Detected_OverLoad, $"{CurrentPort.Motion_X_Axis_OverloadDetectTorque} %");
            LabelFunc.SetText(lbl_Z_Axis_Detected_OverLoad, $"{CurrentPort.Motion_Z_Axis_OverloadDetectTorque} %");
            LabelFunc.SetText(lbl_T_Axis_Detected_OverLoad, $"{CurrentPort.Motion_T_Axis_OverloadDetectTorque} %");
        }
        private void UpdateTagInfo()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();
                string RFIDIP = CurrentPort.GetParam().TagEquipServerIP;
                int RFIDPort = CurrentPort.GetParam().TagEquipServerPort;

                LabelFunc.SetText(lbl_RFIDTCPIP, $"TCP/IP : IP {(string.IsNullOrEmpty(RFIDIP) ? "None" : RFIDIP)}, Port {(RFIDPort == -1 ? $"None" : $"{RFIDPort}")}, {(Reader.IsConnected() ? "Connection" : "Disconnection")}");
                LabelFunc.SetText(lbl_RFID_Tag, $"Tag : {Reader.GetTag()}");
                LabelFunc.SetText(lbl_RFID_Error, $"Error : {Reader.GetErrorStr()}");
                LabelFunc.SetText(lbl_RFID_OperationMode, $"Operation Mode : {Reader.GetOperationMode()}");
                LabelFunc.SetText(lbl_RFID_Request, $"Request : {Reader.GetRequestResult()}");
                LabelFunc.SetText(lbl_RWState, $"R/W State : {(Reader.IsBusy() ? "Busy" : "Idle")}");
            }
            else if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetBCRReader();
                string BCRIP = CurrentPort.GetParam().TagEquipServerIP;
                int BCRPort = CurrentPort.GetParam().TagEquipServerPort;

                LabelFunc.SetText(lbl_BCRTCPIP, $"TCP/IP : IP {(string.IsNullOrEmpty(BCRIP) ? "None" : BCRIP)}, Port {(BCRPort == -1 ? $"None" : $"{BCRPort}")}, {(Reader.IsConnected() ? "Connection" : "Disconnection")}");
                LabelFunc.SetText(lbl_BCR_Tag, $"Tag : {Reader.GetCodeInfo()}");
                LabelFunc.SetText(lbl_BCR_ReadingQuality, $"Reading Quality : {Reader.GetReadingQuality()}");
                LabelFunc.SetText(lbl_BCR_CodeType, $"Code Type : {Reader.GetCodeType()}");
                LabelFunc.SetText(lbl_BCR_CodeLength, $"Code Length : {Reader.GetCodeLength()}");
                LabelFunc.SetText(lbl_BCR_CA_State, $"CA Response : {Reader.GetCAResponse()}");
                LabelFunc.SetForeColor(lbl_BCR_CA_State, Reader.GetCAResponse() != BCRReader.CACommandResponse.Valid ? Color.Red : Color.Lime);
                LabelFunc.SetText(lbl_BCR_RS_State, $"RS Response : {Reader.GetRSResponse()}");
                LabelFunc.SetForeColor(lbl_BCR_RS_State, Reader.GetRSResponse() != BCRReader.RSResponse.Valid ? Color.Red : Color.Lime);
                LabelFunc.SetText(lbl_BCR_TestReadyState, $"Test Ready State : {Reader.GetTestReadyState()}");
                LabelFunc.SetText(lbl_BCR_RunningMode, $"Running Mode : {Reader.GetRunningMode()}");
                LabelFunc.SetText(lbl_BCR_EquipmentErrorState, $"Equipment Error State : {Reader.GetEquipmentState()}");
            }
        }
        private void UpdateWMXParamGridView()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
            {
                if (pnl_WMXParamGrid.Controls.Count > 0)
                    pnl_WMXParamGrid.Controls.Clear();
            }
            else
            {
                if ((string)pnl_WMXParamGrid.Tag != CurrentPort.GetParam().ID)
                {
                    ClearWMXParamDGV();
                    LoadWMXParameterGridView(ref DGV_WMXParam, CurrentPort);
                    pnl_WMXParamGrid.Tag = CurrentPort.GetParam().ID;
                }
            }
        }
        private void IOOutputValueUpdate()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            for (int nRowCount = 0; nRowCount < DGV_OutputMapSettings.Rows.Count; nRowCount++)
            {
                DataGridViewCell DGV_Name = DGV_OutputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
                DataGridViewCell DGV_MapStatus = DGV_OutputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.MapStatus];
                DataGridViewCell DGV_WMXIOStatus = DGV_OutputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.WMXStatus];

                var OutputMap = CurrentPort.GetMotionParam().Ctrl_IO.OutputMap;
                object MapData = null;
                if (CurrentPort.GetParam().ePortType == Port.PortType.MGV_AGV ||
                    CurrentPort.GetParam().ePortType == Port.PortType.MGV ||
                    CurrentPort.GetParam().ePortType == Port.PortType.AGV)
                {
                    Port.AGV_MGV_OutputItem eAGV_MGV_OutputItem = (Port.AGV_MGV_OutputItem)Enum.Parse(typeof(Port.AGV_MGV_OutputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eAGV_MGV_OutputItem);
                }
                else if (CurrentPort.GetParam().ePortType == Port.PortType.OHT ||
                        CurrentPort.GetParam().ePortType == Port.PortType.MGV_OHT)
                {
                    Port.OHT_OutputItem eOHT_OutputItem = (Port.OHT_OutputItem)Enum.Parse(typeof(Port.OHT_OutputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eOHT_OutputItem);
                }
                else if (CurrentPort.GetParam().ePortType == Port.PortType.Conveyor_AGV || CurrentPort.GetParam().ePortType == Port.PortType.Conveyor_OMRON)
                {
                    Port.Conveyor_OutputItem eConveyor_OutputItem = (Port.Conveyor_OutputItem)Enum.Parse(typeof(Port.Conveyor_OutputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eConveyor_OutputItem);
                }
                else if (CurrentPort.GetParam().ePortType == Port.PortType.EQ)
                {
                    Port.EQ_OutputItem eEQ_OutputItem = (Port.EQ_OutputItem)Enum.Parse(typeof(Port.EQ_OutputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eEQ_OutputItem);
                }

                DGV_MapStatus.Value = MapData == null ? "Not Define" : (bool)MapData ? "On" : "Off";
                DGV_MapStatus.Style.BackColor = MapData == null ? Color.DarkGray : (bool)MapData ? Color.Lime : Color.Orange;

                bool bMapFindAndValid = false;

                foreach (var IOMap in OutputMap)
                {
                    //Name을 같을 때 까지 찾음
                    if (IOMap.Name == DGV_Name.Value.ToString())
                    {
                        int StartAddr = IOMap.StartAddr;
                        int Bit = IOMap.Bit;

                        if (StartAddr < 0 || Bit < 0)
                        {
                            DGV_MapStatus.Value = string.Empty;
                            DGV_MapStatus.Style.BackColor = Color.DarkGray;
                            DGV_WMXIOStatus.Value = string.Empty;
                            DGV_WMXIOStatus.Style.BackColor = Color.DarkGray;
                            break;
                        }

                        bool bEnable = IOMap.bInvert ? !CurrentPort.GetOutBit(StartAddr, Bit) : CurrentPort.GetOutBit(StartAddr, Bit);

                        DGV_WMXIOStatus.Value = bEnable ? "On" : "Off";
                        DGV_WMXIOStatus.Style.BackColor = bEnable ? Color.Lime : Color.Orange;
                        bMapFindAndValid = true;
                    }
                }

                if(!bMapFindAndValid)
                {
                    DGV_MapStatus.Value = string.Empty;
                    DGV_MapStatus.Style.BackColor = Color.DarkGray;
                    DGV_WMXIOStatus.Value = string.Empty;
                    DGV_WMXIOStatus.Style.BackColor = Color.DarkGray;
                }

            }
        }
        private void IOInputValueUpdate()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            for (int nRowCount = 0; nRowCount < DGV_InputMapSettings.Rows.Count; nRowCount++)
            {
                DataGridViewCell DGV_Name = DGV_InputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
                DataGridViewCell DGV_MapStatus = DGV_InputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.MapStatus];
                DataGridViewCell DGV_WMXIOStatus = DGV_InputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.WMXStatus];

                var InputMap = CurrentPort.GetMotionParam().Ctrl_IO.InputMap;
                object MapData = null;
                if (CurrentPort.GetParam().ePortType == Port.PortType.MGV_AGV ||
                    CurrentPort.GetParam().ePortType == Port.PortType.MGV ||
                    CurrentPort.GetParam().ePortType == Port.PortType.AGV)
                {
                    Port.AGV_MGV_InputItem eAGV_MGV_InputItem = (Port.AGV_MGV_InputItem)Enum.Parse(typeof(Port.AGV_MGV_InputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eAGV_MGV_InputItem);
                }
                else if (CurrentPort.GetParam().ePortType == Port.PortType.OHT ||
                        CurrentPort.GetParam().ePortType == Port.PortType.MGV_OHT)
                {
                    Port.OHT_InputItem eOHT_InputItem = (Port.OHT_InputItem)Enum.Parse(typeof(Port.OHT_InputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eOHT_InputItem);
                }
                else if (CurrentPort.GetParam().ePortType == Port.PortType.Conveyor_AGV || CurrentPort.GetParam().ePortType == Port.PortType.Conveyor_OMRON)
                {
                    Port.Conveyor_InputItem eConveyor_InputItem = (Port.Conveyor_InputItem)Enum.Parse(typeof(Port.Conveyor_InputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eConveyor_InputItem);
                }
                else if (CurrentPort.GetParam().ePortType == Port.PortType.EQ)
                {
                    Port.EQ_InputItem eEQ_InputItem = (Port.EQ_InputItem)Enum.Parse(typeof(Port.EQ_InputItem), DGV_Name.Value.ToString());
                    MapData = CurrentPort.WMX_IO_ItemToMapData(eEQ_InputItem);
                }

                DGV_MapStatus.Value = MapData == null ? "Not Define" : (bool)MapData ? "On" : "Off";
                DGV_MapStatus.Style.BackColor = MapData == null ? Color.DarkGray : (bool)MapData ? Color.Lime : Color.Orange;

                bool bMapFindAndValid = false;

                foreach (var IOMap in InputMap)
                {
                    //Name 같을 때 까지 찾음
                    if (IOMap.Name == DGV_Name.Value.ToString())
                    {
                        int StartAddr = IOMap.StartAddr;
                        int Bit = IOMap.Bit;

                        if (StartAddr < 0 || Bit < 0)
                        {
                            DGV_MapStatus.Value = string.Empty;
                            DGV_MapStatus.Style.BackColor = Color.DarkGray;
                            DGV_WMXIOStatus.Value = string.Empty;
                            DGV_WMXIOStatus.Style.BackColor = Color.DarkGray;
                            break;
                        }

                        bool bEnable = IOMap.bInvert ? !CurrentPort.GetInputBit(StartAddr, Bit) : CurrentPort.GetInputBit(StartAddr, Bit);

                        DGV_WMXIOStatus.Value = bEnable ? "On" : "Off";
                        DGV_WMXIOStatus.Style.BackColor = bEnable ? Color.Lime : Color.Orange;
                        bMapFindAndValid = true;
                    }
                }

                if(!bMapFindAndValid)
                {
                    DGV_MapStatus.Value = string.Empty;
                    DGV_MapStatus.Style.BackColor = Color.DarkGray;
                    DGV_WMXIOStatus.Value = string.Empty;
                    DGV_WMXIOStatus.Style.BackColor = Color.DarkGray;
                }
            }
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

        private void btn_RFIDTagRead_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                    byte readsize = Convert.ToByte(tbx_TagReadSize.Text);
                    Reader.TagClear();

                    if (rdb_ANT1.Checked)
                        Reader.TagRead(RFIDReader.ANT.ANT1, readsize);
                    else if (rdb_ANT2.Checked)
                        Reader.TagRead(RFIDReader.ANT.ANT2, readsize);
                    else if (rdb_ANT3.Checked)
                        Reader.TagRead(RFIDReader.ANT.ANT3, readsize);
                    else if (rdb_ANT4.Checked)
                        Reader.TagRead(RFIDReader.ANT.ANT4, readsize);
                }
            }
            catch
            {

            }
        }

        private void btn_SetVerboseMode_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                Reader.VerboseMode();
            }
        }

        private void btn_SetAutoReadMode_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                Reader.AutoRead();
            }
        }

        private void btn_GetOperationMode_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                Reader.SystemRegisterRead();
            }
        }

        private void btn_ANT1Enable_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                Reader.OnlyANT1Enable();
            }
        }

        private void btn_ANT2Enable_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                Reader.OnlyANT2Enable();
            }
        }

        private void btn_ANT1ANT2Enable_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                Reader.ANT1ANT2Enable();
            }
        }

        private void btn_SetRTA_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                    byte startaddr = Convert.ToByte(tbx_AutoReadTagStartAddr.Text);
                    Reader.SetRTA(startaddr);
                }
            }
            catch
            {

            }
        }

        private void btn_SetRTB_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                    byte size = Convert.ToByte(tbx_AutoReadTagSize.Text);
                    Reader.SetRTB(size);
                }
            }
            catch
            {

            }
        }

        private void btn_RFID_Save_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
            {
                var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                Reader.Save();
            }
        }

        private void btn_SetVerboseReadTimeout_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if(CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.RFID)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetRFIDReader();

                    byte time = (byte)(Convert.ToByte(tbx_VerboseTimeout.Text) * 10);
                    Reader.VerboseTimeOutSet(time);
                }
            }
            catch
            {

            }
        }

        
        private void btn_FileParamRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] WMX Parameter File Refresh Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterRefresh($"WMX Parameter", Port.InterlockFrom.UI_Event))
                    return;

                DGV_WMXParam.CurrentCell = null;

                EquipPortMotionParam.PortMotionParameter m_LoadedPortMotionParameter = new EquipPortMotionParam.PortMotionParameter();
                m_LoadedPortMotionParameter.Load(CurrentPort.GetParam().ID, ref m_LoadedPortMotionParameter);

                for (int nCount = 0; nCount < m_LoadedPortMotionParameter.Ctrl_Axis.Length; nCount++)
                    CurrentPort.GetMotionParam().Ctrl_Axis[nCount].servoParam.WMXParam = m_LoadedPortMotionParameter.Ctrl_Axis[nCount].servoParam.WMXParam;

                MessageBox.Show(SynusLangPack.GetLanguage("Message_RefreshSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter File Refresh Exception");
            }
            finally
            {
                pnl_WMXParamGrid.Tag = null;
            }
        }
        private void btn_EngineParameterLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;


                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] WMX Parameter Engine Param Load Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CurrentPort.RefreshWMXParam();

                MessageBox.Show(SynusLangPack.GetLanguage("Message_RefreshSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter Engine Param Load Exception");
            }
            finally
            {
                pnl_WMXParamGrid.Tag = null;
            }
        }
        private void btn_WMXParamSave_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;


                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] WMX Parameter Save Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GlobalForm.Frm_AcceptSave frm_AcceptSave = new GlobalForm.Frm_AcceptSave(GlobalForm.Frm_AcceptSave.SaveSection.Port_WMXParam, $"Port[{CurrentPort.GetParam().ID}]");
                frm_AcceptSave.Location = this.Location;
                frm_AcceptSave.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = frm_AcceptSave.ShowDialog();

                if (result != DialogResult.OK || this.IsDisposed)
                    return;

                if (!CurrentPort.Interlock_ParameterSave($"WMX Parameter", Port.InterlockFrom.UI_Event))
                    return;

                DGV_WMXParam.CurrentCell = null;

                if (!ApplyWMXParameterGridView(ref DGV_WMXParam, CurrentPort))
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter Gird Apply Fail");
                    return;
                }

                if (!CurrentPort.InitWMXParam())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("WMXFailMessage"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter Engine Write Fail");
                    return;
                }

                FrmSettings_PortParamSave(CurrentPort);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter Save Exception");
            }
            finally
            {
                pnl_WMXParamGrid.Tag = null;
            }
        }
        private void btn_WMXParamApply_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] WMX Parameter Apply Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterApply($"WMX Parameter", Port.InterlockFrom.UI_Event))
                    return;

                DGV_WMXParam.CurrentCell = null;

                if (!ApplyWMXParameterGridView(ref DGV_WMXParam, CurrentPort))
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter Gird Apply Fail");
                    return;
                }

                if (!CurrentPort.InitWMXParam())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("WMXFailMessage"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter Engine Write Fail");
                    return;
                }

                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplySuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] WMX Parameter Apply Exception");
            }
            finally
            {
                pnl_WMXParamGrid.Tag = null;
            }
        }






        private void btn_WatchdogParamSave_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Watchdog Parameter Save Click");

            if (!CurrentPort.Interlock_ParameterSave($"WatchDog Parameter", Port.InterlockFrom.UI_Event))
                return;

            if (CurrentPort.ApplyWatchdogSettings(ref DGV_WatchdogSettings))
            {
                if (CurrentPort.GetMotionParam().Save(CurrentPort.GetParam().ID, CurrentPort.GetMotionParam()))
                {
                    CurrentPort.Watchdog_Refresh_DetectTime();
                }
                else
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] Watchdog Gird Apply Fail");
            }
        }
        
        private void DGV_WatchdogSettings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort != null)
                CurrentPort.Event_DGV_WatchdogSettings_CellContentClick(sender, e);
        }
        
        private void btn_WatchdogParamAllSave_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Watchdog Parameter All Save Click");

            foreach (var port in Master.m_Ports)
            {
                if (!CurrentPort.Interlock_ParameterSave($"Watchdog Parameter", Port.InterlockFrom.ApplicationLoop))
                    continue;

                if (port.Value.ApplyWatchdogSettings(ref DGV_WatchdogSettings))
                {
                    if (port.Value.GetMotionParam().Save(port.Value.GetParam().ID, port.Value.GetMotionParam()))
                    {
                        port.Value.Watchdog_Refresh_DetectTime();
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Pushing Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AutoSpeed_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CurrentPort.Interlock_ParameterSave($"Auto Speed Parameter", Port.InterlockFrom.UI_Event))
                return;

            Button btn = (Button)sender;

            if (btn == btn_AutoSpeed1)
                CurrentPort.Interlock_SetAutoRunSpeed(10, Port.InterlockFrom.UI_Event);
            else if (btn == btn_AutoSpeed2)
                CurrentPort.Interlock_SetAutoRunSpeed(30, Port.InterlockFrom.UI_Event);
            else if (btn == btn_AutoSpeed3)
                CurrentPort.Interlock_SetAutoRunSpeed(50, Port.InterlockFrom.UI_Event);
            else if (btn == btn_AutoSpeed4)
                CurrentPort.Interlock_SetAutoRunSpeed(70, Port.InterlockFrom.UI_Event);
            else if (btn == btn_AutoSpeed5)
                CurrentPort.Interlock_SetAutoRunSpeed(100, Port.InterlockFrom.UI_Event);
            else
                CurrentPort.Interlock_SetAutoRunSpeed(50, Port.InterlockFrom.UI_Event);

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Auto Speed({CurrentPort.GetMotionParam().AutoRun_Ratio}) Save Click");
            FrmSettings_PortParamSave(CurrentPort, false);
        }
        private void btn_OverLoadSettings_Send_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!CurrentPort.Interlock_ParameterSave($"Over Load Parameter", Port.InterlockFrom.UI_Event))
                return;

            Button btn = (Button)sender;

            try
            {
                if (btn == btn_X_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] X Axis Over Load Settings Click");
                    short value = Convert.ToInt16(tbx_X_Axis_OverLoadSettings.Text);
                    CurrentPort.Interlock_SetOverLoadValue(Port.PortAxis.Shuttle_X, value, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_Z_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Z Axis Over Load Settings Click");
                    short value = Convert.ToInt16(tbx_Z_Axis_OverLoadSettings.Text);
                    CurrentPort.Interlock_SetOverLoadValue(Port.PortAxis.Shuttle_Z, value, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_T_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] T Axis Over Load Settings Click");
                    short value = Convert.ToInt16(tbx_T_Axis_OverLoadSettings.Text);
                    CurrentPort.Interlock_SetOverLoadValue(Port.PortAxis.Shuttle_T, value, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_All_Axis_OverLoadSettings_Send)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] All Axis Over Load Settings Click");
                    short value = Convert.ToInt16(tbx_X_Axis_OverLoadSettings.Text);
                    CurrentPort.Interlock_SetOverLoadValue(Port.PortAxis.Shuttle_X, value, Port.InterlockFrom.UI_Event);

                    value = Convert.ToInt16(tbx_Z_Axis_OverLoadSettings.Text);
                    CurrentPort.Interlock_SetOverLoadValue(Port.PortAxis.Shuttle_Z, value, Port.InterlockFrom.UI_Event);

                    value = Convert.ToInt16(tbx_T_Axis_OverLoadSettings.Text);
                    CurrentPort.Interlock_SetOverLoadValue(Port.PortAxis.Shuttle_T, value, Port.InterlockFrom.UI_Event);
                }

                FrmSettings_PortParamSave(CurrentPort, false);
            }
            catch
            {

            }
        }
        private void btn_Detected_OverLoad_Clear_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (!CurrentPort.Interlock_ParameterClear($"Over Load Clear", Port.InterlockFrom.UI_Event))
                return;

            Button btn = (Button)sender;

            try
            {
                if (btn == btn_X_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] X Axis Over Load Clear Click");
                    CurrentPort.Interlock_SetOverClear(Port.PortAxis.Shuttle_X, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_Z_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Z Axis Over Load Clear Click");
                    CurrentPort.Interlock_SetOverClear(Port.PortAxis.Shuttle_Z, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_T_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] T Axis Over Load Clear Click");
                    CurrentPort.Interlock_SetOverClear(Port.PortAxis.Shuttle_T, Port.InterlockFrom.UI_Event);
                }
                else if (btn == btn_All_Axis_Detected_OverLoad_Clear)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] All Axis Over Load Clear Click");
                    CurrentPort.Interlock_SetOverClear(Port.PortAxis.Shuttle_X, Port.InterlockFrom.UI_Event);
                    CurrentPort.Interlock_SetOverClear(Port.PortAxis.Shuttle_Z, Port.InterlockFrom.UI_Event);
                    CurrentPort.Interlock_SetOverClear(Port.PortAxis.Shuttle_T, Port.InterlockFrom.UI_Event);
                }
            }
            catch
            {

            }
        }
        private void btn_AutoSpeed_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_AutoSpeed_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_AutoSpeed_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }
        private void btn_OverLoadSettings_Send_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_OverLoadSettings_Send_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_OverLoadSettings_Send_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }
        private void btn_Detected_OverLoad_Clear_MouseDown(object sender, MouseEventArgs e)
        {
            Thread LocalThread = new Thread(delegate ()
            {
                Button btn = (Button)sender;
                m_PushSt.Restart();
                btn.Tag = m_PushSt;
                while (m_PushSt.ElapsedMilliseconds < 1000 && m_PushSt.IsRunning)
                {
                    Thread.Sleep(1);
                }
                m_PushSt.Stop();
                btn.Tag = null;

                if (m_PushSt.ElapsedMilliseconds >= 1000 && !m_PushSt.IsRunning)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            btn_Detected_OverLoad_Clear_Click(sender, e);
                        }));
                    }
                    else
                    {
                        btn_Detected_OverLoad_Clear_Click(sender, e);
                    }
                }
            });
            LocalThread.IsBackground = true;
            LocalThread.Start();
        }
        private void btn_Send_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            m_PushSt.Stop();
            btn.Tag = null;
        }



        private void btn_DGVMotionParamApply_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Motion Parameter Apply Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterApply($"Motion Parameter", Port.InterlockFrom.UI_Event))
                    return;

                for (int nCount = 0; nCount < DGV_MotionParam.Length; nCount++)
                    DGV_MotionParam[nCount].CurrentCell = null;

                int nPortAxis = CurrentPort.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_X : CurrentPort.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_LP_X : 0;
                int nMaxPortAxis = CurrentPort.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_T : CurrentPort.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_OP_T : 0;

                for (int nCount = nPortAxis; nCount <= nMaxPortAxis; nCount++)
                {
                    Port.PortAxis ePortAxis = (Port.PortAxis)nCount;
                    if (CurrentPort.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo)
                    {
                        if (!CurrentPort.Apply_ServoGridView(ref DGV_MotionParam[nCount], ePortAxis))
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {ePortAxis} Servo Parameter Apply Fail");
                            return;
                        }
                    }
                    else if (CurrentPort.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Inverter)
                    {
                        if (!CurrentPort.Apply_InverterGridView(ref DGV_MotionParam[nCount], ePortAxis))
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {ePortAxis} Inverter Parameter Apply Fail");
                            return;
                        }
                    }
                    else if (CurrentPort.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Cylinder)
                    {
                        if (!CurrentPort.Apply_CylinderGridView(ref DGV_MotionParam[nCount], ePortAxis))
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {ePortAxis} Cylinder Parameter Apply Fail");
                            return;
                        }
                    }
                }

                if (Port.IsIOParamDupleCheck())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_DupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] IO Parameter Duple Fail");
                    return;
                }

                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplySuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Port Motion Param Apply Exception");
            }
            finally
            {
                tableLayoutPanel_MotionParam.Tag = null;
            }
        }
        private void btn_DGVMotionParamApplyAndSave_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Motion Parameter Save Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GlobalForm.Frm_AcceptSave frm_AcceptSave = new GlobalForm.Frm_AcceptSave(GlobalForm.Frm_AcceptSave.SaveSection.Port_MotionParam, $"Port[{CurrentPort.GetParam().ID}]");
                frm_AcceptSave.Location = this.Location;
                frm_AcceptSave.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = frm_AcceptSave.ShowDialog();

                if (result != DialogResult.OK || this.IsDisposed)
                    return;

                if (!CurrentPort.Interlock_ParameterSave($"Motion Parameter", Port.InterlockFrom.UI_Event))
                    return;

                for (int nCount = 0; nCount < DGV_MotionParam.Length; nCount++)
                    DGV_MotionParam[nCount].CurrentCell = null;

                int nPortAxis = CurrentPort.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_X : CurrentPort.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_LP_X : 0;
                int nMaxPortAxis = CurrentPort.IsShuttleControlPort() ? (int)Port.PortAxis.Shuttle_T : CurrentPort.IsBufferControlPort() ? (int)Port.PortAxis.Buffer_OP_T : 0;

                for (int nCount = nPortAxis; nCount <= nMaxPortAxis; nCount++)
                {
                    Port.PortAxis ePortAxis = (Port.PortAxis)nCount;
                    if (CurrentPort.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Servo)
                    {
                        if (!CurrentPort.Apply_ServoGridView(ref DGV_MotionParam[nCount], ePortAxis))
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {ePortAxis} Servo Parameter Apply Fail");
                            return;
                        }
                    }
                    else if (CurrentPort.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Inverter)
                    {
                        if (!CurrentPort.Apply_InverterGridView(ref DGV_MotionParam[nCount], ePortAxis))
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {ePortAxis} Inverter Parameter Apply Fail");
                            return;
                        }
                    }
                    else if (CurrentPort.GetMotionParam().GetAxisControlType(ePortAxis) == Port.AxisCtrlType.Cylinder)
                    {
                        if (!CurrentPort.Apply_CylinderGridView(ref DGV_MotionParam[nCount], ePortAxis))
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {ePortAxis} Cylinder Parameter Apply Fail");
                            return;
                        }
                    }
                }

                if (Port.IsIOParamDupleCheck())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_DupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] IO Parameter Duple Fail");
                    return;
                }

                FrmSettings_PortParamSave(CurrentPort);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Port Motion Param Save Exception");
            }
            finally
            {
                tableLayoutPanel_MotionParam.Tag = null;
            }
        }
        private void btn_DGVMotionParamRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Motion Parameter Refresh Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterRefresh($"Motion Parameter", Port.InterlockFrom.UI_Event))
                    return;

                for (int nCount = 0; nCount < DGV_MotionParam.Length; nCount++)
                    DGV_MotionParam[nCount].CurrentCell = null;

                EquipPortMotionParam.PortMotionParameter m_LoadedPortMotionParameter = new EquipPortMotionParam.PortMotionParameter();
                m_LoadedPortMotionParameter.Load(CurrentPort.GetParam().ID, ref m_LoadedPortMotionParameter);

                for (int nCount = 0; nCount < m_LoadedPortMotionParameter.Ctrl_Axis.Length; nCount++)
                    CurrentPort.GetMotionParam().Ctrl_Axis[nCount] = m_LoadedPortMotionParameter.Ctrl_Axis[nCount];

                MessageBox.Show(SynusLangPack.GetLanguage("Message_RefreshSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Port Motion Param Refresh Exception");
            }
            finally
            {
                tableLayoutPanel_MotionParam.Tag = null;
            }
        }



        private void btn_DGVBufferParamApply_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Conveyor Parameter Apply Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterApply($"Conveyor Parameter", Port.InterlockFrom.UI_Event))
                    return;

                for (int nCount = 0; nCount < DGV_BufferCVParam.Length; nCount++)
                    DGV_BufferCVParam[nCount].CurrentCell = null;

                if (CurrentPort.IsBufferControlPort())
                {
                    //6축 + 2CV
                    for (int nCount = (int)Port.BufferCV.Buffer_LP; nCount <= (int)Port.BufferCV.Buffer_BP4; nCount++)
                    {
                        Port.BufferCV eBufferCV = (Port.BufferCV)nCount;
                        if (CurrentPort.GetMotionParam().GetBufferControlEnable(eBufferCV) == Port.CVCtrlEnable.Enable)
                        {
                            if (!CurrentPort.Apply_CVGridView(ref DGV_BufferCVParam[nCount], eBufferCV))
                            {
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {eBufferCV} Conveyor Parameter Apply Fail");
                                return;
                            }
                        }
                    }
                }

                if (Port.IsIOParamDupleCheck())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_DupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] IO Parameter Duple Fail");
                    return;
                }

                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplySuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Port Buffer Param Apply Exception");
            }
            finally
            {
                tableLayoutPanel_BufferParam.Tag = null;
            }
        }
        private void btn_DGVBufferParamApplyAndSave_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Conveyor Parameter Save Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GlobalForm.Frm_AcceptSave frm_AcceptSave = new GlobalForm.Frm_AcceptSave(GlobalForm.Frm_AcceptSave.SaveSection.Port_ConveryorParam, $"Port[{CurrentPort.GetParam().ID}]");
                frm_AcceptSave.Location = this.Location;
                frm_AcceptSave.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = frm_AcceptSave.ShowDialog();

                if (result != DialogResult.OK || this.IsDisposed)
                    return;

                if (!CurrentPort.Interlock_ParameterSave($"Conveyor Parameter", Port.InterlockFrom.UI_Event))
                    return;

                for (int nCount = 0; nCount < DGV_BufferCVParam.Length; nCount++)
                    DGV_BufferCVParam[nCount].CurrentCell = null;

                if (CurrentPort.IsBufferControlPort())
                {
                    //6축 + 2CV
                    for (int nCount = (int)Port.BufferCV.Buffer_LP; nCount <= (int)Port.BufferCV.Buffer_BP4; nCount++)
                    {
                        Port.BufferCV eBufferCV = (Port.BufferCV)nCount;
                        if (CurrentPort.GetMotionParam().GetBufferControlEnable(eBufferCV) == Port.CVCtrlEnable.Enable)
                        {
                            if (!CurrentPort.Apply_CVGridView(ref DGV_BufferCVParam[nCount], eBufferCV))
                            {
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] {eBufferCV} Conveyor Parameter Apply Fail");
                                return;
                            }
                        }
                    }
                }

                if (Port.IsIOParamDupleCheck())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_DupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] IO Parameter Duple Fail");
                    return;
                }

                FrmSettings_PortParamSave(CurrentPort);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Port Buffer Param Save Exception");
            }
            finally
            {
                tableLayoutPanel_BufferParam.Tag = null;
            }
        }
        private void btn_DGVBufferParamRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] Conveyor Parameter Refresh Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterRefresh($"Conveyor Parameter", Port.InterlockFrom.UI_Event))
                    return;

                for (int nCount = 0; nCount < DGV_BufferCVParam.Length; nCount++)
                    DGV_BufferCVParam[nCount].CurrentCell = null;

                EquipPortMotionParam.PortMotionParameter m_LoadedPortMotionParameter = new EquipPortMotionParam.PortMotionParameter();
                m_LoadedPortMotionParameter.Load(CurrentPort.GetParam().ID, ref m_LoadedPortMotionParameter);

                for (int nCount = 0; nCount < m_LoadedPortMotionParameter.Ctrl_CV.Length; nCount++)
                    CurrentPort.GetMotionParam().Ctrl_CV[nCount] = m_LoadedPortMotionParameter.Ctrl_CV[nCount];

                MessageBox.Show(SynusLangPack.GetLanguage("Message_RefreshSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] Port Buffer Param Refresh Exception");
            }
            finally
            {
                tableLayoutPanel_BufferParam.Tag = null;
            }
        }



        private void btn_IOMapApply_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] IO Parameter Apply Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterApply($"IO Parameter", Port.InterlockFrom.UI_Event))
                    return;

                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;

                bool bOutputApplyfail = false;
                Exception Outputex = new Exception();

                bool bInputApplyfail = false;
                Exception Inputex = new Exception();

                lock (CurrentPort.GetIOUpdateLock())
                {
                    if (!ApplyOutputGridView(ref DGV_OutputMapSettings, CurrentPort, ref Outputex))
                    {
                        bOutputApplyfail = true;
                    }

                    if (!ApplyInputGridView(ref DGV_InputMapSettings, CurrentPort, ref Inputex))
                    {
                        bInputApplyfail = true;
                    }
                }

                if (bOutputApplyfail)
                {
                    LogMsg.AddExceptionLog(Outputex, $"Port[{CurrentPort.GetParam().ID}] Output Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] Output Gird Apply Fail");
                }

                if (bInputApplyfail)
                {
                    LogMsg.AddExceptionLog(Inputex, $"Port[{CurrentPort.GetParam().ID}] Input Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] Input Gird Apply Fail");
                }

                if (bOutputApplyfail || bInputApplyfail)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Port.IsIOParamDupleCheck())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_DupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] IO Parameter Duple Fail");
                    return;
                }

                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplySuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] IO Param Apply Exception");
            }
            finally
            {
                DGV_OutputMapSettings.Tag = null;
                DGV_InputMapSettings.Tag = null;
            }
        }
        private void btn_IOMapApplyAndSave_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] IO Parameter Save Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GlobalForm.Frm_AcceptSave frm_AcceptSave = new GlobalForm.Frm_AcceptSave(GlobalForm.Frm_AcceptSave.SaveSection.Port_IO, $"Port[{CurrentPort.GetParam().ID}]");
                frm_AcceptSave.Location = this.Location;
                frm_AcceptSave.StartPosition = FormStartPosition.CenterScreen;
                DialogResult result = frm_AcceptSave.ShowDialog();

                if (result != DialogResult.OK || this.IsDisposed)
                    return;

                if (!CurrentPort.Interlock_ParameterSave($"IO Parameter", Port.InterlockFrom.UI_Event))
                    return;

                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;

                bool bOutputApplyfail = false;
                Exception Outputex = new Exception();

                bool bInputApplyfail = false;
                Exception Inputex = new Exception();

                lock (CurrentPort.GetIOUpdateLock())
                {
                    if (!ApplyOutputGridView(ref DGV_OutputMapSettings, CurrentPort, ref Outputex))
                    {
                        bOutputApplyfail = true;
                    }

                    if (!ApplyInputGridView(ref DGV_InputMapSettings, CurrentPort, ref Inputex))
                    {
                        bInputApplyfail = true;
                    }
                }

                if (bOutputApplyfail)
                {
                    LogMsg.AddExceptionLog(Outputex, $"Port[{CurrentPort.GetParam().ID}] Output Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] Output Gird Apply Fail");
                }

                if (bInputApplyfail)
                {
                    LogMsg.AddExceptionLog(Inputex, $"Port[{CurrentPort.GetParam().ID}] Input Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] Input Gird Apply Fail");
                }

                if (bOutputApplyfail || bInputApplyfail)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Port.IsIOParamDupleCheck())
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_DupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{CurrentPort.GetParam().ID}] IO Parameter Duple Fail");
                    return;
                }

                FrmSettings_PortParamSave(CurrentPort);
            }
            catch(Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] IO Param Save Exception");
            }
            finally
            {
                DGV_OutputMapSettings.Tag = null;
                DGV_InputMapSettings.Tag = null;
            }
        }
        private void btn_IOMapRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] IO Parameter Refresh Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!CurrentPort.Interlock_ParameterRefresh($"IO Parameter", Port.InterlockFrom.UI_Event))
                    return;

                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;

                EquipPortMotionParam.PortMotionParameter m_LoadedPortMotionParameter = new EquipPortMotionParam.PortMotionParameter();
                m_LoadedPortMotionParameter.Load(CurrentPort.GetParam().ID, ref m_LoadedPortMotionParameter);

                for (int nCount = 0; nCount < m_LoadedPortMotionParameter.Ctrl_IO.InputMap.Length; nCount++)
                    CurrentPort.GetMotionParam().Ctrl_IO.InputMap[nCount] = m_LoadedPortMotionParameter.Ctrl_IO.InputMap[nCount];

                for (int nCount = 0; nCount < m_LoadedPortMotionParameter.Ctrl_IO.OutputMap.Length; nCount++)
                    CurrentPort.GetMotionParam().Ctrl_IO.OutputMap[nCount] = m_LoadedPortMotionParameter.Ctrl_IO.OutputMap[nCount];

                MessageBox.Show(SynusLangPack.GetLanguage("Message_RefreshSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                LogMsg.AddExceptionLog(ex, $"Port[{CurrentPort.GetParam().ID}] IO Param Refresh Exception");
            }
            finally
            {
                DGV_OutputMapSettings.Tag = null;
                DGV_InputMapSettings.Tag = null;
                DGV_OutputMapSettings.Rows.Clear();
                DGV_InputMapSettings.Rows.Clear();
                IOParamUpdate();
            }
        }

        private void FrmSettings_PortParamSave(Port port, bool PanelReload = true)
        {
            if (port.GetMotionParam().Save(port.GetParam().ID, port.GetMotionParam()))
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                if(PanelReload)
                    AllPanelAndGridReload();
            }
            else
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }










        private void btn_BCRTagRead_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetBCRReader();

                    Reader.TagRead();
                }
            }
            catch
            {

            }
        }

        private void btn_BCRAutoSettings_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetBCRReader();

                    Reader.AutoSettings();
                }
            }
            catch
            {

            }
        }

        private void btn_BCRAlignModeOn_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetBCRReader();

                    Reader.AlignOn();
                }
            }
            catch
            {

            }
        }

        private void btn_BCRAlignModeOff_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetBCRReader();

                    Reader.AlignOff();
                }
            }
            catch
            {

            }
        }

        private void btn_BCRTeachIn_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetBCRReader();

                    Reader.TeachIn();
                }
            }
            catch
            {

            }
        }

        private void btn_BCRGetStatus_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            try
            {
                if (CurrentPort.m_TagReader_Interface.GetReaderEquipType() == Equipment.Port.TagReader.TagReaderType.BCR)
                {
                    var Reader = CurrentPort.m_TagReader_Interface.GetBCRReader();

                    Reader.GetStatus();
                }
            }
            catch
            {

            }
        }

        private void btn_WorkingErrorStop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            CurrentPort.Interlock_TagReadFailErrorOptionChange(true, Port.InterlockFrom.UI_Event);
        }

        private void btn_WorkingContinuous_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            CurrentPort.Interlock_TagReadFailErrorOptionChange(false, Port.InterlockFrom.UI_Event);
        }
    }
}
