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
using System.Collections;
using Master.ManagedFile;

namespace Master.SubForm.MasterForm.MasterSubForm
{
    public partial class Frm_MasterSettings : Form
    {
        public enum WMXIOGridViewColumn
        {
            ItemName,
            StartAddr,
            BitNum,
            MapStatus,
            WMXStatus,
            Invert
        }

        public Frm_MasterSettings()
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

                foreach (Control item in tabPage_IOMap.Controls)
                    ControlFunc.Dispose(item);

                tabPage_IOMap.Dispose();
                tabcontrol_Settings.Dispose();

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);

            ClearIOMap();
            IOGridViewLoad();
        }
        private void ClearIOMap()
        {
            if (DGV_OutputMapSettings.Rows.Count > 0)
                DGV_OutputMapSettings.Rows.Clear();

            if (DGV_InputMapSettings.Rows.Count > 0)
                DGV_InputMapSettings.Rows.Clear();
        }

        private void IOOutputValueUpdate()
        {
            for (int nRowCount = 0; nRowCount < DGV_OutputMapSettings.Rows.Count; nRowCount++)
            {
                DataGridViewCell DGV_Name = DGV_OutputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
                DataGridViewCell DGV_MapStatus = DGV_OutputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.MapStatus];
                DataGridViewCell DGV_WMXIOStatus = DGV_OutputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.WMXStatus];

                var OutputMap = Master.GetMotionParam().Ctrl_IO.OutputMap;

                Master.MasterOutputItem eMasterOutputItem = (Master.MasterOutputItem)Enum.Parse(typeof(Master.MasterOutputItem), DGV_Name.Value.ToString());

                object MapData = Master.WMX_IO_ItemToMapData(eMasterOutputItem);
                DGV_MapStatus.Value = MapData == null ? "Not Define" : (bool)MapData ? "On" : "Off";
                DGV_MapStatus.Style.BackColor = MapData == null ? Color.DarkGray : (bool)MapData ? Color.Lime : Color.Orange;

                bool bMapFindAndValid = false;

                foreach (var IOMap in OutputMap)
                {
                    //Name이 같을 때 까지 찾음
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

                        bool bEnable = IOMap.bInvert ? !Master.GetOutBit(StartAddr, Bit) : Master.GetOutBit(StartAddr, Bit);

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
            for (int nRowCount = 0; nRowCount < DGV_InputMapSettings.Rows.Count; nRowCount++)
            {
                DataGridViewCell DGV_Name = DGV_InputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.ItemName];
                DataGridViewCell DGV_MapStatus = DGV_InputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.MapStatus];
                DataGridViewCell DGV_WMXIOStatus = DGV_InputMapSettings.Rows[nRowCount].Cells[(int)WMXIOGridViewColumn.WMXStatus];

                var InputMap = Master.GetMotionParam().Ctrl_IO.InputMap;

                Master.MasterInputItem eMasterInputItem = (Master.MasterInputItem)Enum.Parse(typeof(Master.MasterInputItem), DGV_Name.Value.ToString());

                object MapData = Master.WMX_IO_ItemToMapData(eMasterInputItem);
                DGV_MapStatus.Value = MapData == null ? "Not Define" : (bool)MapData ? "On" : "Off";
                DGV_MapStatus.Style.BackColor = MapData == null ? Color.DarkGray : (bool)MapData ? Color.Lime : Color.Orange;

                bool bMapFindAndValid = false;

                foreach (var IOMap in InputMap)
                {
                    //Name이 같을 때 까지 찾음
                    if(IOMap.Name == DGV_Name.Value.ToString())
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

                        bool bEnable = IOMap.bInvert ? !Master.GetInputBit(StartAddr, Bit) : Master.GetInputBit(StartAddr, Bit);

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
        private void IOValueUpdate()
        {
            IOOutputValueUpdate();
            IOInputValueUpdate();
        }
        private void IOGridViewLoad()
        {
            LoadOutputGridView(ref DGV_OutputMapSettings);
            LoadInputGridView(ref DGV_InputMapSettings);
        }

        private void LoadOutputGridView(ref DataGridView DGV)
        {
            foreach (Master.MasterOutputItem item in Enum.GetValues(typeof(Master.MasterOutputItem)))
            {
                string Name = item.ToString();
                string StartAddr = string.Empty;
                string Bit = string.Empty;
                string MapStatus = Master.WMX_IO_ItemToMapData(item)?.ToString() ?? "None";
                string WMXStatus = string.Empty;
                string Invert = false.ToString();


                foreach (var IOParam in Master.GetMotionParam().Ctrl_IO.OutputMap)
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
        private bool ApplyOutputGridView(ref DataGridView DGV, ref Exception _Exception)
        {
            try
            {
                var OutputMap = Master.GetMotionParam().Ctrl_IO;

                for (int nRowCount = 0; nRowCount < OutputMap.OutputMap.Length; nRowCount++)
                {
                    if (nRowCount >= DGV.Rows.Count)
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

                        OutputMap.OutputMap[nRowCount].Name = Convert.ToString(DGV_NameCell.Value);

                        if (!string.IsNullOrEmpty((string)DGV_StartAddrCell.Value))
                            OutputMap.OutputMap[nRowCount].StartAddr = Convert.ToInt32(DGV_StartAddrCell.Value);
                        else
                            OutputMap.OutputMap[nRowCount].StartAddr = -1;

                        if (!string.IsNullOrEmpty((string)DGV_BitNumCell.Value))
                            OutputMap.OutputMap[nRowCount].Bit = Convert.ToInt32(DGV_BitNumCell.Value);
                        else
                            OutputMap.OutputMap[nRowCount].Bit = -1;

                        OutputMap.OutputMap[nRowCount].bInvert = Convert.ToBoolean(DGV_InvertCell.Value);

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
            catch (Exception ex)
            {
                _Exception = ex;
                return false;
            }
        }
        private void LoadInputGridView(ref DataGridView DGV)
        {
            foreach(Master.MasterInputItem item in Enum.GetValues(typeof(Master.MasterInputItem)))
            {
                string Name = item.ToString();
                string StartAddr = string.Empty;
                string Bit = string.Empty;
                string MapStatus = Master.WMX_IO_ItemToMapData(item)?.ToString() ?? "None";;
                string WMXStatus = string.Empty;
                string Invert = false.ToString();


                foreach(var IOParam in Master.GetMotionParam().Ctrl_IO.InputMap)
                {
                    if(IOParam.Name == Name)
                    {
                        StartAddr   = IOParam.StartAddr != -1 ? IOParam.StartAddr.ToString() : StartAddr;
                        Bit         = IOParam.Bit != -1 ? IOParam.Bit.ToString() : Bit;
                        Invert      = IOParam.bInvert.ToString();
                        break;
                    }
                }

                DGV.Rows.Add(new string[] { Name, StartAddr, Bit, MapStatus, WMXStatus, Invert });
            }
        }
        private bool ApplyInputGridView(ref DataGridView DGV, ref Exception _Exception)
        {
            try
            {
                var InputMap = Master.GetMotionParam().Ctrl_IO;

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
            catch (Exception ex)
            {
                _Exception = ex;
                return false;
            }
        }

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
                GroupBoxFunc.SetText(groupBox_WMXIOParameterSettings, SynusLangPack.GetLanguage("GroupBox_WMXIOParameterSettings"));
                ButtonFunc.SetText(btn_IOMapApplyAndSave, SynusLangPack.GetLanguage("Btn_ParamSave"));

                IOValueUpdate();
            }
            catch{ }
            finally
            {

            }
        }
        private void btn_IOMapRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-IO Settings Refresh Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (rackMaster.Value.Status_AutoMode ||
                        rackMaster.Value.IsAutoCycleRun() ||
                        rackMaster.Value.IsAutoTeachingRun())
                    {
                        MessageBox.Show($"RackMaster ID = [{rackMaster.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"RackMaster[{rackMaster.Value.GetParam().ID}] is busy");
                        return;
                    }
                }

                foreach (var port in Master.m_Ports)
                {
                    if (port.Value.IsAutoControlRun() ||
                        port.Value.IsAutoManualCycleRun() ||
                        port.Value.IsPortBusy())
                    {
                        MessageBox.Show($"Port ID = [{port.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{port.Value.GetParam().ID}] is busy");
                        return;
                    }
                }

                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Master_RefreshResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (result != DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-IO Settings Refresh Cancel");
                    return;
                }

                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;

                EquipMasterIOParam.MasterIOParameter m_LoadedMasterIOParameter = new EquipMasterIOParam.MasterIOParameter();
                EquipMasterIOParam.LoadMasterIOParam(ref m_LoadedMasterIOParameter);

                for (int nCount = 0; nCount < m_LoadedMasterIOParameter.Ctrl_IO.InputMap.Length; nCount++)
                    Master.GetMotionParam().Ctrl_IO.InputMap[nCount] = m_LoadedMasterIOParameter.Ctrl_IO.InputMap[nCount];

                for (int nCount = 0; nCount < m_LoadedMasterIOParameter.Ctrl_IO.OutputMap.Length; nCount++)
                    Master.GetMotionParam().Ctrl_IO.OutputMap[nCount] = m_LoadedMasterIOParameter.Ctrl_IO.OutputMap[nCount];

                MessageBox.Show(SynusLangPack.GetLanguage("Message_RefreshSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Master Control Form-IO Settings Refresh Exception");
            }
            finally
            {
                DGV_OutputMapSettings.Tag = null;
                DGV_InputMapSettings.Tag = null;
                DGV_OutputMapSettings.Rows.Clear();
                DGV_InputMapSettings.Rows.Clear();
                IOGridViewLoad();
            }
        }

        private void btn_IOMapApply_Click(object sender, EventArgs e)
        {
            try
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-IO Settings Apply Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Maint)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_MaintPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Master_ApplyResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (result != DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"IO Settings Save Cancel");
                    return;
                }

                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (rackMaster.Value.Status_AutoMode ||
                        rackMaster.Value.IsAutoCycleRun() ||
                        rackMaster.Value.IsAutoTeachingRun())
                    {
                        MessageBox.Show($"RackMaster ID = [{rackMaster.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"RackMaster[{rackMaster.Value.GetParam().ID}] is busy");
                        return;
                    }
                }

                foreach (var port in Master.m_Ports)
                {
                    if (port.Value.IsAutoControlRun() ||
                        port.Value.IsAutoManualCycleRun() ||
                        port.Value.IsPortBusy())
                    {
                        MessageBox.Show($"Port ID = [{port.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{port.Value.GetParam().ID}] is busy");
                        return;
                    }
                }

                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;

                bool bOutputApplyfail = false;
                Exception Outputex = new Exception();

                bool bInputApplyfail = false;
                Exception Inputex = new Exception();

                lock (Master.GetIOUpdateLock())
                {
                    if (!ApplyOutputGridView(ref DGV_OutputMapSettings, ref Outputex))
                    {
                        bOutputApplyfail = true;
                    }

                    if (!ApplyInputGridView(ref DGV_InputMapSettings, ref Inputex))
                    {
                        bInputApplyfail = true;
                    }
                }

                if (bOutputApplyfail)
                {
                    LogMsg.AddExceptionLog(Outputex, $"Master Output Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Output Gird Apply Fail");
                }

                if (bInputApplyfail)
                {
                    LogMsg.AddExceptionLog(Inputex, $"Master Input Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Input Gird Apply Fail");
                }

                if (bOutputApplyfail || bInputApplyfail)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplySuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Master IO Param Apply Exception");
            }
            finally
            {
                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;
            }
        }

        private void btn_IOMapApplyAndSave_Click(object sender, EventArgs e)
        {
            try
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Master Control Form-IO Settings Save Click");

                if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                GlobalForm.Frm_AcceptSave frm_AcceptSave = new GlobalForm.Frm_AcceptSave(GlobalForm.Frm_AcceptSave.SaveSection.Master_IO, string.Empty);
                frm_AcceptSave.Location = this.Location;
                frm_AcceptSave.StartPosition = FormStartPosition.CenterScreen;
                DialogResult AcceptResult = frm_AcceptSave.ShowDialog();

                if (AcceptResult != DialogResult.OK || this.IsDisposed)
                    return;

                DialogResult result = MessageBox.Show(SynusLangPack.GetLanguage("Message_Master_ApplyAndSaveResult"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                if (result != DialogResult.OK)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"IO Settings Save Cancel");
                    return;
                }

                foreach (var rackMaster in Master.m_RackMasters)
                {
                    if (rackMaster.Value.Status_AutoMode ||
                        rackMaster.Value.IsAutoCycleRun() ||
                        rackMaster.Value.IsAutoTeachingRun())
                    {
                        MessageBox.Show($"RackMaster ID = [{rackMaster.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"RackMaster[{rackMaster.Value.GetParam().ID}] is busy");
                        return;
                    }
                }

                foreach (var port in Master.m_Ports)
                {
                    if (port.Value.IsAutoControlRun() ||
                        port.Value.IsAutoManualCycleRun() ||
                        port.Value.IsPortBusy())
                    {
                        MessageBox.Show($"Port ID = [{port.Value.GetParam().ID}]" + SynusLangPack.GetLanguage("Message_RejectCommandInWorking"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Port[{port.Value.GetParam().ID}] is busy");
                        return;
                    }
                }

                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;

                bool bOutputApplyfail = false;
                Exception Outputex = new Exception();

                bool bInputApplyfail = false;
                Exception Inputex = new Exception();

                lock (Master.GetIOUpdateLock())
                {
                    if (!ApplyOutputGridView(ref DGV_OutputMapSettings, ref Outputex))
                    {
                        bOutputApplyfail = true;
                    }

                    if (!ApplyInputGridView(ref DGV_InputMapSettings, ref Inputex))
                    {
                        bInputApplyfail = true;
                    }
                }

                if (bOutputApplyfail)
                {
                    LogMsg.AddExceptionLog(Outputex, $"Master Output Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Output Gird Apply Fail");
                }

                if (bInputApplyfail)
                {
                    LogMsg.AddExceptionLog(Inputex, $"Master Input Map Apply");
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.Process, $"Input Gird Apply Fail");
                }

                if (bOutputApplyfail || bInputApplyfail)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_ApplyFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (EquipMasterIOParam.SaveFile(Master.GetMotionParam()))
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"Master IO Param Save Exception");
            }
            finally
            {
                DGV_OutputMapSettings.CurrentCell = null;
                DGV_InputMapSettings.CurrentCell = null;
            }
        }
    }
}
