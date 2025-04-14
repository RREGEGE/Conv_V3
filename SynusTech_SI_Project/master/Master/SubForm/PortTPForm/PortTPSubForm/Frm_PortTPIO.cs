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
using System.Threading;

using Master.Equipment.Port;

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortTPIO : Form
    {
        bool bTPScreenMode = false;
        float bScaleXFactor = 1.0f;
        float bScaleYFactor = 1.0f;

        public Frm_PortTPIO()
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
                    foreach (var port in Master.m_Ports)
                    {
                        port.Value.PIOStatus_ManualReleasePortToOHTPIO();
                        port.Value.PIOStatus_ManualReleasePortToAGVPIO();
                        port.Value.PIOStatus_ManualReleasePortToRMPIO();
                        port.Value.PIOStatus_ManualReleasePortToOMRONPIO();
                        port.Value.PIOStatus_ManualReleaseSTKToEQPIO();
                    }

                    UIUpdateTimer.Enabled = false;
                    tabControl.Tag = null;
                }
            };

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                foreach (Control item in tabPage_TwoBufferIO.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_OneBufferIO.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_CVBufferIO.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_OHT.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_AGV.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_EQ.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_OMRON.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_Shuttle_T.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_Shuttle_X.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_Shuttle_Z.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_BufferLP_ZAxis.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_BufferOP_ZAxis.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_BufferOP_YAxis.Controls)
                    ControlFunc.Dispose(item);

                tabPage_TwoBufferIO.Dispose();
                tabPage_OneBufferIO.Dispose();
                tabPage_CVBufferIO.Dispose();
                tabPage_AGV.Dispose();
                tabPage_OHT.Dispose();
                tabPage_EQ.Dispose();
                tabPage_OMRON.Dispose();
                tabPage_Shuttle_T.Dispose();
                tabPage_Shuttle_X.Dispose();
                tabPage_Shuttle_Z.Dispose();
                tabPage_BufferLP_ZAxis.Dispose();
                tabPage_BufferOP_ZAxis.Dispose();
                tabPage_BufferOP_YAxis.Dispose();

                tabControl.Dispose();

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }

        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);
        }

        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);

            bTPScreenMode = true;
            bScaleXFactor = FactorX;
            bScaleYFactor = FactorY;
        }

        private void UpdateLoginLevelInterlock()
        {
            bool bVisible = LogIn.GetLogInLevel() == LogIn.LogInLevel.Admin;

            btn_Settings_AGVIOMap.Visible           = bVisible && !bTPScreenMode;
            btn_Settings_CVBufferIOMap.Visible      = bVisible && !bTPScreenMode;
            btn_Settings_EQIOMap.Visible            = bVisible && !bTPScreenMode;
            btn_Settings_LPBufferZIOMap.Visible     = bVisible && !bTPScreenMode;
            btn_Settings_OHTIOMap.Visible           = bVisible && !bTPScreenMode;
            btn_Settings_OmronIOMap.Visible         = bVisible && !bTPScreenMode;
            btn_Settings_OneBufferIOMap.Visible     = bVisible && !bTPScreenMode;
            btn_Settings_OPBufferYIOMap.Visible     = bVisible && !bTPScreenMode;
            btn_Settings_OPBufferZIOMap.Visible     = bVisible && !bTPScreenMode;
            btn_Settings_ShuttleTIOMap.Visible      = bVisible && !bTPScreenMode;
            btn_Settings_ShuttleXIOMap.Visible      = bVisible && !bTPScreenMode;
            btn_Settings_ShuttleZIOMap.Visible      = bVisible && !bTPScreenMode;
            btn_Settings_TwoBufferIOMap.Visible     = bVisible && !bTPScreenMode;

            btn_Save_AGVIOMap.Visible               = bVisible && !bTPScreenMode;
            btn_Save_CVBufferIOMap.Visible          = bVisible && !bTPScreenMode;
            btn_Save_EQIOMap.Visible                = bVisible && !bTPScreenMode;
            btn_Save_LPBufferZIOMap.Visible         = bVisible && !bTPScreenMode;
            btn_Save_OHTIOMap.Visible               = bVisible && !bTPScreenMode;
            btn_Save_OmronIOMap.Visible             = bVisible && !bTPScreenMode;
            btn_Save_OneBufferIOMap.Visible         = bVisible && !bTPScreenMode;
            btn_Save_OPBufferYIOMap.Visible         = bVisible && !bTPScreenMode;
            btn_Save_OPBufferZIOMap.Visible         = bVisible && !bTPScreenMode;
            btn_Save_ShuttleTIOMap.Visible          = bVisible && !bTPScreenMode;
            btn_Save_ShuttleXIOMap.Visible          = bVisible && !bTPScreenMode;
            btn_Save_ShuttleZIOMap.Visible          = bVisible && !bTPScreenMode;
            btn_Save_TwoBufferIOMap.Visible         = bVisible && !bTPScreenMode;
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
                
                UpdateLoginLevelInterlock();

                GroupBoxFunc.SetText(groupBox_PIOStatus, SynusLangPack.GetLanguage("GorupBox_PIOStatus"));
                GroupBoxFunc.SetText(groupBox_BufferSensorStatus, SynusLangPack.GetLanguage("GroupBox_BufferSensorStatus"));
                GroupBoxFunc.SetText(groupBox_TwoBufferSensorMap, SynusLangPack.GetLanguage("GroupBox_TwoBufferSensorMap"));
                GroupBoxFunc.SetText(groupBox_OneBufferSensorMap, SynusLangPack.GetLanguage("GroupBox_OneBufferSensorMap"));
                GroupBoxFunc.SetText(groupBox_CVBufferSensorMap, SynusLangPack.GetLanguage("GroupBox_CVBufferSensorMap"));

                GroupBoxFunc.SetText(groupBox_ShuttleXSensorMap, SynusLangPack.GetLanguage("GroupBox_ShuttleXSensorMap"));
                GroupBoxFunc.SetText(groupBox_ShuttleXSensorStatus, SynusLangPack.GetLanguage("GroupBox_ShuttleXSensorStatus"));
                GroupBoxFunc.SetText(groupBox_ShuttleXPosStatus, SynusLangPack.GetLanguage("GroupBox_ShuttleXPosStatus"));

                GroupBoxFunc.SetText(groupBox_ShuttleZSensorMap, SynusLangPack.GetLanguage("GroupBox_ShuttleZSensorMap"));
                GroupBoxFunc.SetText(groupBox_ShuttleZSensorStatus, SynusLangPack.GetLanguage("GroupBox_ShuttleZSensorStatus"));
                GroupBoxFunc.SetText(groupBox_ShuttleZPosStatus, SynusLangPack.GetLanguage("GroupBox_ShuttleZPosStatus"));

                GroupBoxFunc.SetText(groupBox_ShuttleTSensorMap, SynusLangPack.GetLanguage("GroupBox_ShuttleTSensorMap"));
                GroupBoxFunc.SetText(groupBox_ShuttleTSensorStatus, SynusLangPack.GetLanguage("GroupBox_ShuttleTSensorStatus"));
                GroupBoxFunc.SetText(groupBox_ShuttleTPosStatus, SynusLangPack.GetLanguage("GroupBox_ShuttleTPosStatus"));

                GroupBoxFunc.SetText(groupBox_BufferLPZSensorMap, SynusLangPack.GetLanguage("GroupBox_BufferLPZSensorMap"));
                GroupBoxFunc.SetText(groupBox_BufferLPZSensorStatus, SynusLangPack.GetLanguage("GroupBox_BufferLPZSensorStatus"));
                GroupBoxFunc.SetText(groupBox_BufferLPZPosStatus, SynusLangPack.GetLanguage("GroupBox_BufferLPZPosStatus"));

                GroupBoxFunc.SetText(groupBox_BufferOPZSensorMap, SynusLangPack.GetLanguage("GroupBox_BufferOPZSensorMap"));
                GroupBoxFunc.SetText(groupBox_BufferOPZSensorStatus, SynusLangPack.GetLanguage("GroupBox_BufferOPZSensorStatus"));
                GroupBoxFunc.SetText(groupBox_BufferOPZPosStatus, SynusLangPack.GetLanguage("GroupBox_BufferOPZPosStatus"));

                GroupBoxFunc.SetText(groupBox_BufferOPYSensorMap, SynusLangPack.GetLanguage("GroupBox_BufferOPYSensorMap"));
                GroupBoxFunc.SetText(groupBox_BufferOPYSensorStatus, SynusLangPack.GetLanguage("GroupBox_BufferOPYSensorStatus"));
                GroupBoxFunc.SetText(groupBox_BufferOPYPosStatus, SynusLangPack.GetLanguage("GroupBox_BufferOPYPosStatus"));

                GroupBoxFunc.SetText(groupBox_OHTPIOStatus, SynusLangPack.GetLanguage("GroupBox_OHTPIOStatus"));
                GroupBoxFunc.SetText(groupBox_PortToOHT_ManualPIO, SynusLangPack.GetLanguage("GroupBox_PortToOHT_ManualPIO"));

                GroupBoxFunc.SetText(groupBox_AGVPIOStatus, SynusLangPack.GetLanguage("GroupBox_AGVPIOStatus"));
                GroupBoxFunc.SetText(groupBox_PortToAGV_ManualPIO, SynusLangPack.GetLanguage("GroupBox_PortToAGV_ManualPIO"));

                GroupBoxFunc.SetText(groupBox_EQPIOStatus, SynusLangPack.GetLanguage("GroupBox_EQPIOStatus"));
                GroupBoxFunc.SetText(groupBox_PortToRM_ManualPIO, SynusLangPack.GetLanguage("GroupBox_PortToRM_ManualPIO"));
                GroupBoxFunc.SetText(groupBox_RMToEQ_ManualPIO, SynusLangPack.GetLanguage("GroupBox_RMToEQ_ManualPIO"));
                GroupBoxFunc.SetText(groupBox_OMRONPIOStatus, SynusLangPack.GetLanguage("GroupBox_OMRONPIOStatus"));
                GroupBoxFunc.SetText(groupBox_PortToOMRON_ManualPIO, SynusLangPack.GetLanguage("GroupBox_PortToOMRON_ManualPIO"));

                LabelFunc.SetText(lbl_Shuttle_X_AxisTargetPosition, SynusLangPack.GetLanguage("Label_TargetPosition"));
                LabelFunc.SetText(lbl_Shuttle_X_AxisActualPosition, SynusLangPack.GetLanguage("Label_ActualPosition"));
                LabelFunc.SetText(lbl_Shuttle_Z_AxisTargetPosition, SynusLangPack.GetLanguage("Label_TargetPosition"));
                LabelFunc.SetText(lbl_Shuttle_Z_AxisActualPosition, SynusLangPack.GetLanguage("Label_ActualPosition"));
                LabelFunc.SetText(lbl_Shuttle_T_AxisTargetPosition, SynusLangPack.GetLanguage("Label_TargetPosition"));
                LabelFunc.SetText(lbl_Shuttle_T_AxisActualPosition, SynusLangPack.GetLanguage("Label_ActualPosition"));

                TabPageFunc.SetText(tabPage_TwoBufferIO, SynusLangPack.GetLanguage("TabPage_BufferIO"));
                TabPageFunc.SetText(tabPage_OneBufferIO, SynusLangPack.GetLanguage("TabPage_BufferIO"));
                TabPageFunc.SetText(tabPage_CVBufferIO, SynusLangPack.GetLanguage("TabPage_CVBufferIO"));
                TabPageFunc.SetText(tabPage_AGV, SynusLangPack.GetLanguage("TabPage_AGV"));
                TabPageFunc.SetText(tabPage_OHT, SynusLangPack.GetLanguage("TabPage_OHT"));
                TabPageFunc.SetText(tabPage_EQ, SynusLangPack.GetLanguage("TabPage_EQ"));
                TabPageFunc.SetText(tabPage_OMRON, SynusLangPack.GetLanguage("TabPage_OMRON"));
                TabPageFunc.SetText(tabPage_Shuttle_T, SynusLangPack.GetLanguage("TabPage_Shuttle_T"));
                TabPageFunc.SetText(tabPage_Shuttle_X, SynusLangPack.GetLanguage("TabPage_Shuttle_X"));
                TabPageFunc.SetText(tabPage_Shuttle_Z, SynusLangPack.GetLanguage("TabPage_Shuttle_Z"));
                TabPageFunc.SetText(tabPage_BufferLP_ZAxis, SynusLangPack.GetLanguage("TabPage_BufferLP_ZAxis"));
                TabPageFunc.SetText(tabPage_BufferOP_ZAxis, SynusLangPack.GetLanguage("TabPage_BufferOP_ZAxis"));
                TabPageFunc.SetText(tabPage_BufferOP_YAxis, SynusLangPack.GetLanguage("TabPage_BufferOP_YAxis"));


                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (tabControl.Tag != CurrentPort)
                {
                    Update_TabPage(CurrentPort);
                    SyncSafetyInfo();
                }

                if (CurrentPort == null)
                    return;

                if (tabControl.Controls.Contains(tabPage_TwoBufferIO))
                    TwoBufferTabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_OneBufferIO))
                    OneBufferTabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_CVBufferIO))
                    CVBufferTabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_Shuttle_X))
                    Shuttle_X_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_Shuttle_Z))
                    Shuttle_Z_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_Shuttle_T))
                    Shuttle_T_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_BufferLP_ZAxis))
                    BufferLP_Z_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_BufferOP_ZAxis))
                    BufferOP_Z_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_BufferOP_YAxis))
                    BufferOP_Y_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_AGV))
                    AGV_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_OHT))
                    OHT_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_EQ))
                    EQ_TabPageUpdate(CurrentPort);

                if (tabControl.Controls.Contains(tabPage_OMRON))
                    OMRON_TabPageUpdate(CurrentPort);
            }
            catch{ }
            finally
            {

            }
        }
        private void Update_TabPage(Port port)
        {
            if (port == null)
            {
                if (tabControl.TabPages.Count > 0)
                    tabControl.TabPages.Clear();

                tabControl.Tag = null;
            }
            else
            {

                TabPage tabpage = tabControl.SelectedTab;

                if (port.IsShuttleControlPort() && port.GetMotionParam().eBufferType == Port.ShuttleCtrlBufferType.Two_Buffer)
                {
                    if (tabControl.TabPages.Contains(tabPage_OneBufferIO))
                        tabControl.TabPages.Remove(tabPage_OneBufferIO);

                    if (tabControl.TabPages.Contains(tabPage_CVBufferIO))
                        tabControl.TabPages.Remove(tabPage_CVBufferIO);

                    if (!tabControl.TabPages.Contains(tabPage_TwoBufferIO))
                        tabControl.TabPages.Insert(0 <= tabControl.TabPages.Count ? 0 : tabControl.TabPages.Count, tabPage_TwoBufferIO);
                }
                else if (port.IsShuttleControlPort() && port.GetMotionParam().eBufferType == Port.ShuttleCtrlBufferType.One_Buffer)
                {
                    if (tabControl.TabPages.Contains(tabPage_TwoBufferIO))
                        tabControl.TabPages.Remove(tabPage_TwoBufferIO);

                    if (tabControl.TabPages.Contains(tabPage_CVBufferIO))
                        tabControl.TabPages.Remove(tabPage_CVBufferIO);

                    if (!tabControl.TabPages.Contains(tabPage_OneBufferIO))
                        tabControl.TabPages.Insert(0 <= tabControl.TabPages.Count ? 0 : tabControl.TabPages.Count, tabPage_OneBufferIO);
                }
                else if (port.IsBufferControlPort())
                {
                    if (tabControl.TabPages.Contains(tabPage_TwoBufferIO))
                        tabControl.TabPages.Remove(tabPage_TwoBufferIO);

                    if (tabControl.TabPages.Contains(tabPage_OneBufferIO))
                        tabControl.TabPages.Remove(tabPage_OneBufferIO);

                    if (!tabControl.TabPages.Contains(tabPage_CVBufferIO))
                        tabControl.TabPages.Insert(0 <= tabControl.TabPages.Count ? 0 : tabControl.TabPages.Count, tabPage_CVBufferIO);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_TwoBufferIO))
                        tabControl.TabPages.Remove(tabPage_TwoBufferIO);

                    if (tabControl.TabPages.Contains(tabPage_OneBufferIO))
                        tabControl.TabPages.Remove(tabPage_OneBufferIO);

                    if (tabControl.TabPages.Contains(tabPage_CVBufferIO))
                        tabControl.TabPages.Remove(tabPage_CVBufferIO);
                }


                if (port.GetMotionParam().IsServoType(Port.PortAxis.Shuttle_X))
                {
                    if (!tabControl.TabPages.Contains(tabPage_Shuttle_X))
                        tabControl.TabPages.Insert(1 <= tabControl.TabPages.Count ? 1 : tabControl.TabPages.Count, tabPage_Shuttle_X);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_Shuttle_X))
                        tabControl.TabPages.Remove(tabPage_Shuttle_X);
                }

                if (port.GetMotionParam().IsInverterType(Port.PortAxis.Buffer_LP_Z) || port.GetMotionParam().IsCylinderType(Port.PortAxis.Buffer_LP_Z))
                {
                    if (!tabControl.TabPages.Contains(tabPage_BufferLP_ZAxis))
                        tabControl.TabPages.Insert(1 <= tabControl.TabPages.Count ? 1 : tabControl.TabPages.Count, tabPage_BufferLP_ZAxis);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_BufferLP_ZAxis))
                        tabControl.TabPages.Remove(tabPage_BufferLP_ZAxis);
                }

                if (port.GetMotionParam().IsServoType(Port.PortAxis.Shuttle_Z) || port.GetMotionParam().IsCylinderType(Port.PortAxis.Shuttle_Z))
                {
                    if (!tabControl.TabPages.Contains(tabPage_Shuttle_Z))
                        tabControl.TabPages.Insert(2 <= tabControl.TabPages.Count ? 2 : tabControl.TabPages.Count, tabPage_Shuttle_Z);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_Shuttle_Z))
                        tabControl.TabPages.Remove(tabPage_Shuttle_Z);
                }

                if (port.GetMotionParam().IsInverterType(Port.PortAxis.Buffer_OP_Z) || port.GetMotionParam().IsCylinderType(Port.PortAxis.Buffer_OP_Z))
                {
                    if (!tabControl.TabPages.Contains(tabPage_BufferOP_ZAxis))
                        tabControl.TabPages.Insert(1 <= tabControl.TabPages.Count ? 1 : tabControl.TabPages.Count, tabPage_BufferOP_ZAxis);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_BufferOP_ZAxis))
                        tabControl.TabPages.Remove(tabPage_BufferOP_ZAxis);
                }

                if (port.GetMotionParam().IsCylinderType(Port.PortAxis.Buffer_OP_Y))
                {
                    if (!tabControl.TabPages.Contains(tabPage_BufferOP_YAxis))
                        tabControl.TabPages.Insert(2 <= tabControl.TabPages.Count ? 2 : tabControl.TabPages.Count, tabPage_BufferOP_YAxis);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_BufferOP_YAxis))
                        tabControl.TabPages.Remove(tabPage_BufferOP_YAxis);
                }

                if (port.GetMotionParam().IsServoType(Port.PortAxis.Shuttle_T))
                {
                    if (!tabControl.TabPages.Contains(tabPage_Shuttle_T))
                        tabControl.TabPages.Insert(3 <= tabControl.TabPages.Count ? 3 : tabControl.TabPages.Count, tabPage_Shuttle_T);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_Shuttle_T))
                        tabControl.TabPages.Remove(tabPage_Shuttle_T);
                }

                if ((port.GetPortOperationMode() == Port.PortOperationMode.AGV || port.GetParam().ePortType == Port.PortType.Conveyor_AGV))
                {
                    if (!tabControl.TabPages.Contains(tabPage_AGV))
                        tabControl.TabPages.Insert(4 <= tabControl.TabPages.Count ? 4 : tabControl.TabPages.Count, tabPage_AGV);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_AGV))
                        tabControl.TabPages.Remove(tabPage_AGV);
                }

                if (port.GetParam().ePortType == Port.PortType.Conveyor_OMRON)
                {
                    if (!tabControl.TabPages.Contains(tabPage_OMRON))
                        tabControl.TabPages.Insert(4 <= tabControl.TabPages.Count ? 4 : tabControl.TabPages.Count, tabPage_OMRON);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_OMRON))
                        tabControl.TabPages.Remove(tabPage_OMRON);
                }

                if (port.GetPortOperationMode() == Port.PortOperationMode.OHT)
                {
                    if (!tabControl.TabPages.Contains(tabPage_OHT))
                        tabControl.TabPages.Insert(4 <= tabControl.TabPages.Count ? 4 : tabControl.TabPages.Count, tabPage_OHT);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_OHT))
                        tabControl.TabPages.Remove(tabPage_OHT);
                }

                if (port.GetPortOperationMode() == Port.PortOperationMode.EQ) //port.GetPortOperationMode() == Port.PortOperationMode.EQ
                {
                    if (!tabControl.TabPages.Contains(tabPage_EQ))
                        tabControl.TabPages.Insert(4 <= tabControl.TabPages.Count ? 4 : tabControl.TabPages.Count, tabPage_EQ);
                }
                else
                {
                    if (tabControl.TabPages.Contains(tabPage_EQ))
                        tabControl.TabPages.Remove(tabPage_EQ);
                }

                if (!tabControl.TabPages.Contains(tabpage))
                    tabControl.SelectedTab = tabControl.TabPages.Count > 0 ? tabControl.TabPages[0] : null;
                else
                    tabControl.SelectedTab = tabpage;


                tabControl.Tag = port;

                ///PIO GridView Move
                if (tabControl.Controls.Contains(tabPage_TwoBufferIO))
                {
                    if (!tableLayoutPanel4.Controls.Contains(groupBox_PIOStatus))
                        tableLayoutPanel4.Controls.Add(groupBox_PIOStatus, 0, 1);

                    if(!tableLayoutPanel3.Controls.Contains(groupBox_BufferSensorStatus))
                        tableLayoutPanel3.Controls.Add(groupBox_BufferSensorStatus, 1, 0);
                }
                if (tabControl.Controls.Contains(tabPage_OneBufferIO))
                {
                    if (!tableLayoutPanel21.Controls.Contains(groupBox_PIOStatus))
                        tableLayoutPanel21.Controls.Add(groupBox_PIOStatus, 0, 1);

                    if (!tableLayoutPanel5.Controls.Contains(groupBox_BufferSensorStatus))
                        tableLayoutPanel5.Controls.Add(groupBox_BufferSensorStatus, 1, 0);
                }
                if (tabControl.Controls.Contains(tabPage_CVBufferIO))
                {
                    if (!tableLayoutPanel42.Controls.Contains(groupBox_PIOStatus))
                        tableLayoutPanel42.Controls.Add(groupBox_PIOStatus, 0, 1);

                    if (!tableLayoutPanel41.Controls.Contains(groupBox_BufferSensorStatus))
                        tableLayoutPanel41.Controls.Add(groupBox_BufferSensorStatus, 1, 0);
                }
            }
        }
        
        private string FindTextIndex(DataGridView DGV, string sector, string text)
        {
            for(int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
            {
                try
                {
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[1];
                    string Data = Convert.ToString(DGV_Cell.Value);

                    if (Data.Contains(text) && Data.Contains(sector))
                    {
                        return Convert.ToString(DGV.Rows[nRowCount].Cells[0].Value);
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }
        private string FindTextIndex(DataGridView DGV, string text)
        {
            for (int nRowCount = 0; nRowCount < DGV.Rows.Count; nRowCount++)
            {
                try
                {
                    DataGridViewCell DGV_Cell = DGV.Rows[nRowCount].Cells[1];
                    string Data = Convert.ToString(DGV_Cell.Value);

                    if (Data.Contains(text))
                    {
                        return Convert.ToString(DGV.Rows[nRowCount].Cells[0].Value);
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }

            return string.Empty;
        }


        private void TwoBufferTabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_TwoBuffer_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_TwoBuffer_PictureBackPanel.Width;

            panel_TwoBuffer_Picture.Location = new Point((nSaftyPanelWidth - panel_TwoBuffer_Picture.Width) / 2, (nSaftyPanelHeight - panel_TwoBuffer_Picture.Height) / 2);

            port.Update_DGV_BufferSensorList(ref DGV_BufferSensorList);

            if (!DGV_Buffer1PIOStatus.Visible)
                DGV_Buffer1PIOStatus.Visible = true;

            port.Update_DGV_Buffer1PIOStatus(ref DGV_Buffer1PIOStatus);
            port.Update_DGV_Buffer2PIOStatus(ref DGV_Buffer2PIOStatus);


            tpnl_TwoBuf_LP_CSTDetect2.Visible   = (port.GetPortOperationMode() == Port.PortOperationMode.OHT && !port.IsValidInputItemMapping(Port.OHT_InputItem.LP_Placement_Detect_2.ToString()))? false : true;
            tpnl_TwoBuf_LP_HoistDetect.Visible  = port.GetPortOperationMode() == Port.PortOperationMode.OHT ? true : false;
            tpnl_TwoBuf_LP_CartDetect1.Visible  = port.GetPortOperationMode() == Port.PortOperationMode.AGV ? true : false;
            tpnl_TwoBuf_LP_CartDetect2.Visible  = port.GetPortOperationMode() == Port.PortOperationMode.AGV ? true : false;

            LabelFunc.SetText(lbl_Number_TwoBuf_LP_CSTDetect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CST_Detect1}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_LP_CSTDetect1, port.Sensor_LP_CST_Detect1 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_LP_CSTDetect1, port.Sensor_LP_CST_Detect1 ? "On" : "Off");

            if (port.GetPortOperationMode() != Port.PortOperationMode.OHT)
            {
                LabelFunc.SetText(lbl_Number_TwoBuf_LP_CSTDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CST_Detect2}"));
                LabelFunc.SetBackColor(lbl_Status_TwoBuf_LP_CSTDetect2, port.Sensor_LP_CST_Detect2 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_TwoBuf_LP_CSTDetect2, port.Sensor_LP_CST_Detect2 ? "On" : "Off");
            }
            else
            {
                if(port.IsValidInputItemMapping(Port.OHT_InputItem.LP_Placement_Detect_2.ToString()))
                {
                    LabelFunc.SetText(lbl_Number_TwoBuf_LP_CSTDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CST_Detect2}"));
                    LabelFunc.SetBackColor(lbl_Status_TwoBuf_LP_CSTDetect2, port.Sensor_LP_CST_Detect2 ? Color.Lime : Color.White);
                    LabelFunc.SetText(lbl_Status_TwoBuf_LP_CSTDetect2, port.Sensor_LP_CST_Detect2 ? "On" : "Off");
                }
            }

            LabelFunc.SetText(lbl_Number_TwoBuf_LP_CSTPresence, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CST_Presence}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_LP_CSTPresence, port.Sensor_LP_CST_Presence ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_LP_CSTPresence, port.Sensor_LP_CST_Presence ? "On" : "Off");

            if (port.GetPortOperationMode() == Port.PortOperationMode.OHT)
            {
                LabelFunc.SetText(lbl_Number_TwoBuf_LP_HoistDetect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_Hoist_Detect}"));
                LabelFunc.SetBackColor(lbl_Status_TwoBuf_LP_HoistDetect, port.Sensor_LP_Hoist_Detect ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_TwoBuf_LP_HoistDetect, port.Sensor_LP_Hoist_Detect ? "On" : "Off");
            }

            if(port.GetPortOperationMode() == Port.PortOperationMode.AGV)
            {
                LabelFunc.SetText(lbl_Number_TwoBuf_LP_CartDetect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_Cart_Detect1}"));
                LabelFunc.SetBackColor(lbl_Status_TwoBuf_LP_CartDetect1, port.Sensor_LP_Cart_Detect1 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_TwoBuf_LP_CartDetect1, port.Sensor_LP_Cart_Detect1 ? "On" : "Off");
                LabelFunc.SetText(lbl_Number_TwoBuf_LP_CartDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_Cart_Detect2}"));
                LabelFunc.SetBackColor(lbl_Status_TwoBuf_LP_CartDetect2, port.Sensor_LP_Cart_Detect2 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_TwoBuf_LP_CartDetect2, port.Sensor_LP_Cart_Detect2 ? "On" : "Off");
            }

            LabelFunc.SetText(lbl_Number_TwoBuf_OP_CSTDetect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Detect1}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_OP_CSTDetect1, port.Sensor_OP_CST_Detect1 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_OP_CSTDetect1, port.Sensor_OP_CST_Detect1 ? "On" : "Off");
            LabelFunc.SetText(lbl_Number_TwoBuf_OP_CSTDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Detect2}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_OP_CSTDetect2, port.Sensor_OP_CST_Detect2 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_OP_CSTDetect2, port.Sensor_OP_CST_Detect2 ? "On" : "Off");
            LabelFunc.SetText(lbl_Number_TwoBuf_OP_CSTPresence, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Presence}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_OP_CSTPresence, port.Sensor_OP_CST_Presence ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_OP_CSTPresence, port.Sensor_OP_CST_Presence ? "On" : "Off");
            LabelFunc.SetText(lbl_Number_TwoBuf_OP_ForkDetect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_Fork_Detect}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_OP_ForkDetect, port.Sensor_OP_Fork_Detect ? Color.Red : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_OP_ForkDetect, port.Sensor_OP_Fork_Detect ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_TwoBuf_Shuttle_CSTDetect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.Shuttle_CST_Detect1}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_Shuttle_CSTDetect1, port.Sensor_Shuttle_CSTDetect1 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_Shuttle_CSTDetect1, port.Sensor_Shuttle_CSTDetect1 ? "On" : "Off");
            LabelFunc.SetText(lbl_Number_TwoBuf_Shuttle_CSTDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.Shuttle_CST_Detect2}"));
            LabelFunc.SetBackColor(lbl_Status_TwoBuf_Shuttle_CSTDetect2, port.Sensor_Shuttle_CSTDetect2 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_TwoBuf_Shuttle_CSTDetect2, port.Sensor_Shuttle_CSTDetect2 ? "On" : "Off");
        }

        private void OneBufferTabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_OneBuffer_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_OneBuffer_PictureBackPanel.Width;

            panel_OneBuffer_Picture.Location = new Point((nSaftyPanelWidth - panel_OneBuffer_Picture.Width) / 2, (nSaftyPanelHeight - panel_OneBuffer_Picture.Height) / 2);

            port.Update_DGV_BufferSensorList(ref DGV_BufferSensorList);

            if (!DGV_Buffer1PIOStatus.Visible)
                DGV_Buffer1PIOStatus.Visible = true;

            port.Update_DGV_Buffer1PIOStatus(ref DGV_Buffer1PIOStatus);
            port.Update_DGV_Buffer2PIOStatus(ref DGV_Buffer2PIOStatus);

            tpnl_OneBuf_LP_HoistDetect.Visible = port.GetPortOperationMode() == Port.PortOperationMode.OHT ? true : false;
            tpnl_OneBuf_LP_CartDetect1.Visible = port.GetPortOperationMode() == Port.PortOperationMode.AGV ? true : false;
            tpnl_OneBuf_LP_CartDetect2.Visible = port.GetPortOperationMode() == Port.PortOperationMode.AGV ? true : false;

            if (port.GetPortOperationMode() == Port.PortOperationMode.OHT)
            {
                LabelFunc.SetText(lbl_Number_OneBuf_LP_HoistDetect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_Hoist_Detect}"));
                LabelFunc.SetBackColor(lbl_Status_OneBuf_LP_HoistDetect, port.Sensor_LP_Hoist_Detect ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_OneBuf_LP_HoistDetect, port.Sensor_LP_Hoist_Detect ? "On" : "Off");
            }

            if (port.GetPortOperationMode() == Port.PortOperationMode.AGV)
            {
                LabelFunc.SetText(lbl_Number_OneBuf_LP_CartDetect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_Cart_Detect1}"));
                LabelFunc.SetBackColor(lbl_Status_OneBuf_LP_CartDetect1, port.Sensor_LP_Cart_Detect1 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_OneBuf_LP_CartDetect1, port.Sensor_LP_Cart_Detect1 ? "On" : "Off");
                LabelFunc.SetText(lbl_Number_OneBuf_LP_CartDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_Cart_Detect2}"));
                LabelFunc.SetBackColor(lbl_Status_OneBuf_LP_CartDetect2, port.Sensor_LP_Cart_Detect2 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_OneBuf_LP_CartDetect2, port.Sensor_LP_Cart_Detect2 ? "On" : "Off");
            }

            LabelFunc.SetText(lbl_Number_OneBuf_OP_CSTDetect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Detect1}"));
            LabelFunc.SetBackColor(lbl_Status_OneBuf_OP_CSTDetect1, port.Sensor_OP_CST_Detect1 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_OneBuf_OP_CSTDetect1, port.Sensor_OP_CST_Detect1 ? "On" : "Off");
            LabelFunc.SetText(lbl_Number_OneBuf_OP_CSTDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Detect2}"));
            LabelFunc.SetBackColor(lbl_Status_OneBuf_OP_CSTDetect2, port.Sensor_OP_CST_Detect2 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_OneBuf_OP_CSTDetect2, port.Sensor_OP_CST_Detect2 ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_OneBuf_LP_CSTPresence, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CST_Presence}"));
            LabelFunc.SetBackColor(lbl_Status_OneBuf_LP_CSTPresence, port.Sensor_LP_CST_Presence ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_OneBuf_LP_CSTPresence, port.Sensor_LP_CST_Presence ? "On" : "Off");
            LabelFunc.SetText(lbl_Number_OneBuf_OP_CSTPresence, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Presence}"));
            LabelFunc.SetBackColor(lbl_Status_OneBuf_OP_CSTPresence, port.Sensor_OP_CST_Presence ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_OneBuf_OP_CSTPresence, port.Sensor_OP_CST_Presence ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_OneBuf_OP_ForkDetect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_Fork_Detect}"));
            LabelFunc.SetBackColor(lbl_Status_OneBuf_OP_ForkDetect, port.Sensor_OP_Fork_Detect ? Color.Red : Color.White);
            LabelFunc.SetText(lbl_Status_OneBuf_OP_ForkDetect, port.Sensor_OP_Fork_Detect ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_OneBuf_Shuttle_CSTDetect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.Shuttle_CST_Detect1}"));
            LabelFunc.SetBackColor(lbl_Status_OneBuf_Shuttle_CSTDetect1, port.Sensor_Shuttle_CSTDetect1 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_OneBuf_Shuttle_CSTDetect1, port.Sensor_Shuttle_CSTDetect1 ? "On" : "Off");
            LabelFunc.SetText(lbl_Number_OneBuf_Shuttle_CSTDetect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.Shuttle_CST_Detect2}"));
            LabelFunc.SetBackColor(lbl_Status_OneBuf_Shuttle_CSTDetect2, port.Sensor_Shuttle_CSTDetect2 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_OneBuf_Shuttle_CSTDetect2, port.Sensor_Shuttle_CSTDetect2 ? "On" : "Off");
        }

        private void CVBufferTabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_CVBuffer_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_CVBuffer_PictureBackPanel.Width;

            panel_CVBuffer_Picture.Location = new Point((nSaftyPanelWidth - panel_CVBuffer_Picture.Width) / 2, (nSaftyPanelHeight - panel_CVBuffer_Picture.Height) / 2);

            port.Update_DGV_BufferSensorList(ref DGV_BufferSensorList);

            if (!DGV_Buffer1PIOStatus.Visible)
                DGV_Buffer1PIOStatus.Visible = true;

            port.Update_DGV_Buffer1PIOStatus(ref DGV_Buffer1PIOStatus);
            port.Update_DGV_Buffer2PIOStatus(ref DGV_Buffer2PIOStatus);

            bool bPortDirectionInput = port.GetOperationDirection() == Port.PortDirection.Input;

            tpnl_CVBuf_BP1_CST_Detect.Visible = (port.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP1) && port.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP1)) ? true : false;
            tpnl_CVBuf_BP2_CST_Detect.Visible = (port.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP2) && port.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP2)) ? true : false;
            tpnl_CVBuf_BP3_CST_Detect.Visible = (port.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP3) && port.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP3)) ? true : false;
            tpnl_CVBuf_BP4_CST_Detect.Visible = (port.GetMotionParam().IsCVUsed(Port.BufferCV.Buffer_BP4) && port.GetMotionParam().IsCSTDetectSensorEnable(Port.BufferCV.Buffer_BP4)) ? true : false;

            LabelFunc.SetText(bPortDirectionInput ? lbl_Number_CVBuf_LP_CV_In : lbl_Number_CVBuf_LP_CV_Out, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CV_In}"));
            LabelFunc.SetBackColor(bPortDirectionInput ? lbl_Status_CVBuf_LP_CV_In : lbl_Status_CVBuf_LP_CV_Out, port.Sensor_LP_CV_IN ? Color.Lime : Color.White);
            LabelFunc.SetText(bPortDirectionInput ? lbl_Status_CVBuf_LP_CV_In : lbl_Status_CVBuf_LP_CV_Out, port.Sensor_LP_CV_IN ? "On" : "Off");

            LabelFunc.SetText(bPortDirectionInput ? lbl_Number_CVBuf_LP_CV_Out : lbl_Number_CVBuf_LP_CV_In, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CV_Out}"));
            LabelFunc.SetBackColor(bPortDirectionInput ? lbl_Status_CVBuf_LP_CV_Out : lbl_Status_CVBuf_LP_CV_In, port.Sensor_LP_CV_STOP ? Color.Lime : Color.White);
            LabelFunc.SetText(bPortDirectionInput ? lbl_Status_CVBuf_LP_CV_Out : lbl_Status_CVBuf_LP_CV_In, port.Sensor_LP_CV_STOP ? "On" : "Off");

            LabelFunc.SetText(bPortDirectionInput ? lbl_Number_CVBuf_OP_CV_In : lbl_Number_CVBuf_OP_CV_Out, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CV_In}"));
            LabelFunc.SetBackColor(bPortDirectionInput ? lbl_Status_CVBuf_OP_CV_In : lbl_Status_CVBuf_OP_CV_Out, port.Sensor_OP_CV_IN ? Color.Lime : Color.White);
            LabelFunc.SetText(bPortDirectionInput ? lbl_Status_CVBuf_OP_CV_In : lbl_Status_CVBuf_OP_CV_Out, port.Sensor_OP_CV_IN ? "On" : "Off");

            LabelFunc.SetText(bPortDirectionInput ? lbl_Number_CVBuf_OP_CV_Out : lbl_Number_CVBuf_OP_CV_In, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CV_Out}"));
            LabelFunc.SetBackColor(bPortDirectionInput ? lbl_Status_CVBuf_OP_CV_Out : lbl_Status_CVBuf_OP_CV_In, port.Sensor_OP_CV_STOP ? Color.Lime : Color.White);
            LabelFunc.SetText(bPortDirectionInput ? lbl_Status_CVBuf_OP_CV_Out : lbl_Status_CVBuf_OP_CV_In, port.Sensor_OP_CV_STOP ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_CVBuf_LP_Precense_Detect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.LP_CST_Presence}"));
            LabelFunc.SetBackColor(lbl_Status_CVBuf_LP_Presence_Detect, port.Sensor_LP_CST_Presence ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_CVBuf_LP_Presence_Detect, port.Sensor_LP_CST_Presence ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_CVBuf_OP_CST_Detect1, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Detect1}"));
            LabelFunc.SetBackColor(lbl_Status_CVBuf_OP_CST_Detect1, port.Sensor_OP_CST_Detect1 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_CVBuf_OP_CST_Detect1, port.Sensor_OP_CST_Detect1 ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_CVBuf_OP_CST_Detect2, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_CST_Detect2}"));
            LabelFunc.SetBackColor(lbl_Status_CVBuf_OP_CST_Detect2, port.Sensor_OP_CST_Detect2 ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_CVBuf_OP_CST_Detect2, port.Sensor_OP_CST_Detect2 ? "On" : "Off");

            if (tpnl_CVBuf_BP1_CST_Detect.Visible)
            {
                LabelFunc.SetText(lbl_Number_CVBuf_BP1_CST_Detect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.BP1_CST_Detect}"));
                LabelFunc.SetBackColor(lbl_Status_CVBuf_BP1_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP1) ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_CVBuf_BP1_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP1) ? "On" : "Off");
            }

            if (tpnl_CVBuf_BP2_CST_Detect.Visible)
            {
                LabelFunc.SetText(lbl_Number_CVBuf_BP2_CST_Detect, FindTextIndex(DGV_BufferSensorList,$"{Port.DGV_BufferSensorRow.BP2_CST_Detect}"));
                LabelFunc.SetBackColor(lbl_Status_CVBuf_BP2_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP2) ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_CVBuf_BP2_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP2) ? "On" : "Off");
            }

            if (tpnl_CVBuf_BP3_CST_Detect.Visible)
            {
                LabelFunc.SetText(lbl_Number_CVBuf_BP3_CST_Detect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.BP3_CST_Detect}"));
                LabelFunc.SetBackColor(lbl_Status_CVBuf_BP3_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP3) ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_CVBuf_BP3_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP3) ? "On" : "Off");
            }

            if (tpnl_CVBuf_BP4_CST_Detect.Visible)
            {
                LabelFunc.SetText(lbl_Number_CVBuf_BP4_CST_Detect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.BP4_CST_Detect}"));
                LabelFunc.SetBackColor(lbl_Status_CVBuf_BP4_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP4) ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_CVBuf_BP4_CST_Detect, port.BufferCtrl_BP_CSTDetect_Status(Port.BufferCV.Buffer_BP4) ? "On" : "Off");
            }

            LabelFunc.SetText(lbl_Number_CVBuf_OP_ForkDetect, FindTextIndex(DGV_BufferSensorList, $"{Port.DGV_BufferSensorRow.OP_Fork_Detect}"));
            LabelFunc.SetBackColor(lbl_Status_CVBuf_OP_ForkDetect, port.Sensor_OP_Fork_Detect ? Color.Red : Color.White);
            LabelFunc.SetText(lbl_Status_CVBuf_OP_ForkDetect, port.Sensor_OP_Fork_Detect ? "On" : "Off");
        }

        private void Shuttle_X_TabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_Shuttle_X_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_Shuttle_X_PictureBackPanel.Width;

            panel_Shuttle_X_Picture.Location = new Point((nSaftyPanelWidth - panel_Shuttle_X_Picture.Width) / 2, (nSaftyPanelHeight - panel_Shuttle_X_Picture.Height) / 2);

            port.Update_DGV_ShuttleX_SensorList(ref DGV_ShuttleXAxisSensorStatus);

            if (port.GetMotionParam().IsServoType(Port.PortAxis.Shuttle_X))
            {
                if (!groupBox_ShuttleXPosStatus.Visible)
                    groupBox_ShuttleXPosStatus.Visible = true;

                lbl_X_Axis_TargetPos.Text = port.ServoCtrl_GetTargetPosition(Port.PortAxis.Shuttle_X).ToString("0.0") + (port.GetMotionParam().IsRotaryAxis(Port.PortAxis.Shuttle_X) ? " °" : " mm");
                lbl_X_Axis_ActPos.Text = port.ServoCtrl_GetCurrentPosition(Port.PortAxis.Shuttle_X).ToString("0.0") + (port.GetMotionParam().IsRotaryAxis(Port.PortAxis.Shuttle_X) ? " °" : " mm");

                X_Axis_ImageUpdate(port);
            }
            else
                groupBox_ShuttleXPosStatus.Visible = false;

            tpnl_Shuttle_X_Wait.Visible = port.GetMotionParam().IsWaitPosEnable(Port.PortAxis.Shuttle_X) ? true : false;

            LabelFunc.SetText(lbl_Number_Shuttle_X_NOT, FindTextIndex(DGV_ShuttleXAxisSensorStatus, $"{Port.DGV_ShuttleXAxisSensorRow.NOT}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_X_NOT, port.Sensor_X_Axis_NOT ? Color.Red : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_X_NOT, port.Sensor_X_Axis_NOT ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_Shuttle_X_Home, FindTextIndex(DGV_ShuttleXAxisSensorStatus, $"{Port.DGV_ShuttleXAxisSensorRow.HOME}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_X_Home, port.Sensor_X_Axis_HOME ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_X_Home, port.Sensor_X_Axis_HOME ? "On" : "Off");

            if(tpnl_Shuttle_X_Wait.Visible)
            {
                LabelFunc.SetText(lbl_Number_Shuttle_X_Wait, FindTextIndex(DGV_ShuttleXAxisSensorStatus, $"{Port.DGV_ShuttleXAxisSensorRow.WaitPosSensor}"));
                LabelFunc.SetBackColor(lbl_Status_Shuttle_X_Wait, port.Sensor_X_Axis_WaitPosSensor ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_Shuttle_X_Wait, port.Sensor_X_Axis_WaitPosSensor ? "On" : "Off");
            }

            LabelFunc.SetText(lbl_Number_Shuttle_X_Pos, FindTextIndex(DGV_ShuttleXAxisSensorStatus, $"{Port.DGV_ShuttleXAxisSensorRow.Pos}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_X_Pos, port.Sensor_X_Axis_POS ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_X_Pos, port.Sensor_X_Axis_POS ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_Shuttle_X_POT, FindTextIndex(DGV_ShuttleXAxisSensorStatus, $"{Port.DGV_ShuttleXAxisSensorRow.POT}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_X_POT, port.Sensor_X_Axis_POT ? Color.Red : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_X_POT, port.Sensor_X_Axis_POT ? "On" : "Off");
        }

        private void Shuttle_Z_TabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_Shuttle_Z_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_Shuttle_Z_PictureBackPanel.Width;

            panel_Shuttle_Z_Picture.Location = new Point((nSaftyPanelWidth - panel_Shuttle_Z_Picture.Width) / 2, (nSaftyPanelHeight - panel_Shuttle_Z_Picture.Height) / 2);

            port.Update_DGV_ShuttleZ_SensorList(ref DGV_ShuttleZAxisSensorStatus);

            if (port.GetMotionParam().IsServoType(Port.PortAxis.Shuttle_Z))
            {
                if (!groupBox_ShuttleZPosStatus.Visible)
                    groupBox_ShuttleZPosStatus.Visible = true;

                tableLayoutPanel25.Visible = true;
                panel_ZAxis_Image.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                lbl_ZUpPos.Visible = true;
                lbl_ZDownPos.Visible = true;
                Z_Axis_ImageUpdate(port);

                tpnl_Shuttle_Z_Home.Visible = true;
                tpnl_Shuttle_Z_Pos.Visible = true;
                tpnl_Shuttle_Z_NOT.Visible = true;
                tpnl_Shuttle_Z_POT.Visible = true;
                tpnl_Shuttle_Z_BWD.Visible = false;
                tpnl_Shuttle_Z_FWD.Visible = false;

                lbl_Z_Axis_TargetPos.Text = port.ServoCtrl_GetTargetPosition(Port.PortAxis.Shuttle_Z).ToString("0.0") + (port.GetMotionParam().IsRotaryAxis(Port.PortAxis.Shuttle_Z) ? " °" : " mm");
                lbl_Z_Axis_ActPos.Text = port.ServoCtrl_GetCurrentPosition(Port.PortAxis.Shuttle_Z).ToString("0.0") + (port.GetMotionParam().IsRotaryAxis(Port.PortAxis.Shuttle_Z) ? " °" : " mm");

                LabelFunc.SetText(lbl_Number_Shuttle_Z_NOT, FindTextIndex(DGV_ShuttleZAxisSensorStatus, $"{Port.DGV_ShuttleZAxisSensorRow.NOT}"));
                LabelFunc.SetBackColor(lbl_Status_Shuttle_Z_NOT, port.Sensor_Z_Axis_NOT ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_Shuttle_Z_NOT, port.Sensor_Z_Axis_NOT ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_Shuttle_Z_POT, FindTextIndex(DGV_ShuttleZAxisSensorStatus, $"{Port.DGV_ShuttleZAxisSensorRow.POT}"));
                LabelFunc.SetBackColor(lbl_Status_Shuttle_Z_POT, port.Sensor_Z_Axis_POT ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_Shuttle_Z_POT, port.Sensor_Z_Axis_POT ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_Shuttle_Z_Home, FindTextIndex(DGV_ShuttleZAxisSensorStatus, $"{Port.DGV_ShuttleZAxisSensorRow.HOME}"));
                LabelFunc.SetBackColor(lbl_Status_Shuttle_Z_Home, port.Sensor_Z_Axis_HOME ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_Shuttle_Z_Home, port.Sensor_Z_Axis_HOME ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_Shuttle_Z_Pos, FindTextIndex(DGV_ShuttleZAxisSensorStatus, $"{Port.DGV_ShuttleZAxisSensorRow.Pos}"));
                LabelFunc.SetBackColor(lbl_Status_Shuttle_Z_Pos, port.Sensor_Z_Axis_POS ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_Shuttle_Z_Pos, port.Sensor_Z_Axis_POS ? "On" : "Off");
            }
            else if (port.GetMotionParam().IsCylinderType(Port.PortAxis.Shuttle_Z))
            {
                if (!groupBox_ShuttleZPosStatus.Visible)
                    groupBox_ShuttleZPosStatus.Visible = true;

                tableLayoutPanel25.Visible = false;
                panel_ZAxis_Image.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                lbl_ZUpPos.Visible = false;
                lbl_ZDownPos.Visible = false;

                tpnl_Shuttle_Z_Home.Visible = false;
                tpnl_Shuttle_Z_Pos.Visible = false;
                tpnl_Shuttle_Z_NOT.Visible = false;
                tpnl_Shuttle_Z_POT.Visible = false;
                tpnl_Shuttle_Z_BWD.Visible = true;
                tpnl_Shuttle_Z_FWD.Visible = true;

                LabelFunc.SetText(lbl_Number_Shuttle_Z_BWD, FindTextIndex(DGV_ShuttleZAxisSensorStatus, $"{Port.DGV_ShuttleZAxisSensorRow.Cylinder_BWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_Shuttle_Z_BWD, port.Sensor_Z_Axis_BWDSensor ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_Shuttle_Z_BWD, port.Sensor_Z_Axis_BWDSensor ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_Shuttle_Z_FWD, FindTextIndex(DGV_ShuttleZAxisSensorStatus, $"{Port.DGV_ShuttleZAxisSensorRow.Cylinder_FWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_Shuttle_Z_FWD, port.Sensor_Z_Axis_FWDSensor ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_Shuttle_Z_FWD, port.Sensor_Z_Axis_FWDSensor ? "On" : "Off");
            }
            else
                groupBox_ShuttleZPosStatus.Visible = false;
        }

        private void Shuttle_T_TabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_Shuttle_T_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_Shuttle_T_PictureBackPanel.Width;

            panel_Shuttle_T_Picture.Location = new Point((nSaftyPanelWidth - panel_Shuttle_T_Picture.Width) / 2, (nSaftyPanelHeight - panel_Shuttle_T_Picture.Height) / 2);
            
            port.Update_DGV_ShuttleT_SensorList(ref DGV_ShuttleTAxisSensorStatus);
            T_Axis_ImageUpdate(port);

            lbl_T_Axis_TargetPos.Text = port.ServoCtrl_GetTargetPosition(Port.PortAxis.Shuttle_T).ToString("0.0") + (port.GetMotionParam().IsRotaryAxis(Port.PortAxis.Shuttle_T) ? " °" : " mm");
            lbl_T_Axis_ActPos.Text = port.ServoCtrl_GetCurrentPosition(Port.PortAxis.Shuttle_T).ToString("0.0") + (port.GetMotionParam().IsRotaryAxis(Port.PortAxis.Shuttle_T) ? " °" : " mm");

            LabelFunc.SetText(lbl_Number_Shuttle_T_NOT, FindTextIndex(DGV_ShuttleTAxisSensorStatus, $"{Port.DGV_ShuttleTAxisSensorRow.NOT}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_T_NOT, port.Sensor_T_Axis_NOT ? Color.Red : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_T_NOT, port.Sensor_T_Axis_NOT ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_Shuttle_T_POT, FindTextIndex(DGV_ShuttleTAxisSensorStatus, $"{Port.DGV_ShuttleTAxisSensorRow.POT}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_T_POT, port.Sensor_T_Axis_POT ? Color.Red : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_T_POT, port.Sensor_T_Axis_POT ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_Shuttle_T_Home, FindTextIndex(DGV_ShuttleTAxisSensorStatus, $"{Port.DGV_ShuttleTAxisSensorRow.HOME}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_T_Home, port.Sensor_T_Axis_HOME ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_T_Home, port.Sensor_T_Axis_HOME ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_Shuttle_T_Pos, FindTextIndex(DGV_ShuttleTAxisSensorStatus, $"{Port.DGV_ShuttleTAxisSensorRow.Pos}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_T_Pos, port.Sensor_T_Axis_POS ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_T_Pos, port.Sensor_T_Axis_POS ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_Shuttle_T_0Deg, FindTextIndex(DGV_ShuttleTAxisSensorStatus, $"{Port.DGV_ShuttleTAxisSensorRow.Degree_0_Position}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_T_0Deg, port.Sensor_T_Axis_0DegSensor ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_T_0Deg, port.Sensor_T_Axis_0DegSensor ? "On" : "Off");

            LabelFunc.SetText(lbl_Number_Shuttle_T_180Deg, FindTextIndex(DGV_ShuttleTAxisSensorStatus, $"{Port.DGV_ShuttleTAxisSensorRow.Degree_180_Position}"));
            LabelFunc.SetBackColor(lbl_Status_Shuttle_T_180Deg, port.Sensor_T_Axis_180DegSensor ? Color.Lime : Color.White);
            LabelFunc.SetText(lbl_Status_Shuttle_T_180Deg, port.Sensor_T_Axis_180DegSensor ? "On" : "Off");
        }

        private void BufferLP_Z_TabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_BufferLP_Z_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_BufferLP_Z_PictureBackPanel.Width;

            panel_BufferLP_Z_Picture.Location = new Point((nSaftyPanelWidth - panel_BufferLP_Z_Picture.Width) / 2, (nSaftyPanelHeight - panel_BufferLP_Z_Picture.Height) / 2);

            port.Update_DGV_BufferLP_Z_SensorList(ref DGV_BufferLPZAxisSensorStatus);

            if (port.GetMotionParam().IsInverterType(Port.PortAxis.Buffer_LP_Z))
            {
                tpnl_BufferLP_Z_Pos1.Visible = true;
                tpnl_BufferLP_Z_Pos2.Visible = true;
                tpnl_BufferLP_Z_NOT.Visible = true;
                tpnl_BufferLP_Z_POT.Visible = true;
                tpnl_BufferLP_Z_BWD.Visible = false;
                tpnl_BufferLP_Z_FWD.Visible = false;

                LabelFunc.SetText(lbl_Number_BufferLP_Z_NOT, FindTextIndex(DGV_BufferLPZAxisSensorStatus, $"{Port.DGV_BufferLP_ZAxisSensorRow.NOT}"));
                LabelFunc.SetBackColor(lbl_Status_BufferLP_Z_NOT, port.Sensor_LP_Z_NOT ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_BufferLP_Z_NOT, port.Sensor_LP_Z_NOT ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferLP_Z_POT, FindTextIndex(DGV_BufferLPZAxisSensorStatus, $"{Port.DGV_BufferLP_ZAxisSensorRow.POT}"));
                LabelFunc.SetBackColor(lbl_Status_BufferLP_Z_POT, port.Sensor_LP_Z_POT ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_BufferLP_Z_POT, port.Sensor_LP_Z_POT ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferLP_Z_Pos1, FindTextIndex(DGV_BufferLPZAxisSensorStatus, $"{Port.DGV_BufferLP_ZAxisSensorRow.Pos1}"));
                LabelFunc.SetBackColor(lbl_Status_BufferLP_Z_Pos1, port.Sensor_LP_Z_POS1 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferLP_Z_Pos1, port.Sensor_LP_Z_POS1 ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferLP_Z_Pos2, FindTextIndex(DGV_BufferLPZAxisSensorStatus, $"{Port.DGV_BufferLP_ZAxisSensorRow.Pos2}"));
                LabelFunc.SetBackColor(lbl_Status_BufferLP_Z_Pos2, port.Sensor_LP_Z_POS2 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferLP_Z_Pos2, port.Sensor_LP_Z_POS2 ? "On" : "Off");
            }
            else if(port.GetMotionParam().IsCylinderType(Port.PortAxis.Buffer_LP_Z))
            {
                tpnl_BufferLP_Z_Pos1.Visible = false;
                tpnl_BufferLP_Z_Pos2.Visible = false;
                tpnl_BufferLP_Z_NOT.Visible = false;
                tpnl_BufferLP_Z_POT.Visible = false;
                tpnl_BufferLP_Z_BWD.Visible = true;
                tpnl_BufferLP_Z_FWD.Visible = true;

                bool bFWDStatus = port.CylinderCtrl_GetPosSensorOn(Port.PortAxis.Buffer_LP_Z, Port.CylCtrlList.FWD);
                bool bBWDStatus = port.CylinderCtrl_GetPosSensorOn(Port.PortAxis.Buffer_LP_Z, Port.CylCtrlList.BWD);
                LabelFunc.SetText(lbl_Number_BufferLP_Z_FWD, FindTextIndex(DGV_BufferLPZAxisSensorStatus, $"{Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_FWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_BufferLP_Z_FWD, bFWDStatus ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferLP_Z_FWD, bFWDStatus ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferLP_Z_BWD, FindTextIndex(DGV_BufferLPZAxisSensorStatus, $"{Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_BWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_BufferLP_Z_BWD, bBWDStatus ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferLP_Z_BWD, bBWDStatus ? "On" : "Off");                
            }
        }

        private void BufferOP_Z_TabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_BufferOP_Z_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_BufferOP_Z_PictureBackPanel.Width;

            panel_BufferOP_Z_Picture.Location = new Point((nSaftyPanelWidth - panel_BufferOP_Z_Picture.Width) / 2, (nSaftyPanelHeight - panel_BufferOP_Z_Picture.Height) / 2);

            port.Update_DGV_BufferOP_Z_SensorList(ref DGV_BufferOPZAxisSensorStatus);

            if (port.GetMotionParam().IsInverterType(Port.PortAxis.Buffer_OP_Z))
            {
                tpnl_BufferOP_Z_Pos1.Visible = true;
                tpnl_BufferOP_Z_Pos2.Visible = true;
                tpnl_BufferOP_Z_NOT.Visible = true;
                tpnl_BufferOP_Z_POT.Visible = true;
                tpnl_BufferOP_Z_BWD.Visible = false;
                tpnl_BufferOP_Z_FWD.Visible = false;

                LabelFunc.SetText(lbl_Number_BufferOP_Z_NOT, FindTextIndex(DGV_BufferOPZAxisSensorStatus, $"{Port.DGV_BufferOP_ZAxisSensorRow.NOT}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Z_NOT, port.Sensor_OP_Z_NOT ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Z_NOT, port.Sensor_OP_Z_NOT ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferOP_Z_POT, FindTextIndex(DGV_BufferOPZAxisSensorStatus, $"{Port.DGV_BufferOP_ZAxisSensorRow.POT}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Z_POT, port.Sensor_OP_Z_POT ? Color.Red : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Z_POT, port.Sensor_OP_Z_POT ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferOP_Z_Pos1, FindTextIndex(DGV_BufferOPZAxisSensorStatus, $"{Port.DGV_BufferOP_ZAxisSensorRow.Pos1}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Z_Pos1, port.Sensor_OP_Z_POS1 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Z_Pos1, port.Sensor_OP_Z_POS1 ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferOP_Z_Pos2, FindTextIndex(DGV_BufferOPZAxisSensorStatus, $"{Port.DGV_BufferOP_ZAxisSensorRow.Pos2}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Z_Pos2, port.Sensor_OP_Z_POS2 ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Z_Pos2, port.Sensor_OP_Z_POS2 ? "On" : "Off");
            }
            else if (port.GetMotionParam().IsCylinderType(Port.PortAxis.Buffer_OP_Z))
            {
                tpnl_BufferOP_Z_Pos1.Visible = false;
                tpnl_BufferOP_Z_Pos2.Visible = false;
                tpnl_BufferOP_Z_NOT.Visible = false;
                tpnl_BufferOP_Z_POT.Visible = false;
                tpnl_BufferOP_Z_BWD.Visible = true;
                tpnl_BufferOP_Z_FWD.Visible = true;

                bool bFWDStatus = port.CylinderCtrl_GetPosSensorOn(Port.PortAxis.Buffer_OP_Z, Port.CylCtrlList.FWD);
                bool bBWDStatus = port.CylinderCtrl_GetPosSensorOn(Port.PortAxis.Buffer_OP_Z, Port.CylCtrlList.BWD);
                LabelFunc.SetText(lbl_Number_BufferOP_Z_FWD, FindTextIndex(DGV_BufferOPZAxisSensorStatus, $"{Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_FWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Z_FWD, bFWDStatus ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Z_FWD, bFWDStatus ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferOP_Z_BWD, FindTextIndex(DGV_BufferOPZAxisSensorStatus, $"{Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_BWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Z_BWD, bBWDStatus ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Z_BWD, bBWDStatus ? "On" : "Off");
            }
        }
        
        private void BufferOP_Y_TabPageUpdate(Port port)
        {
            int nSaftyPanelHeight = panel_BufferOP_Y_PictureBackPanel.Height;
            int nSaftyPanelWidth = panel_BufferOP_Y_PictureBackPanel.Width;

            panel_BufferOP_Y_Picture.Location = new Point((nSaftyPanelWidth - panel_BufferOP_Y_Picture.Width) / 2, (nSaftyPanelHeight - panel_BufferOP_Y_Picture.Height) / 2);

            port.Update_DGV_BufferOP_Y_SensorList(ref DGV_BufferOPYAxisSensorStatus);

            if (port.GetMotionParam().IsCylinderType(Port.PortAxis.Buffer_OP_Z))
            {
                bool bFWDStatus = port.CylinderCtrl_GetPosSensorOn(Port.PortAxis.Buffer_OP_Y, Port.CylCtrlList.FWD);
                bool bBWDStatus = port.CylinderCtrl_GetPosSensorOn(Port.PortAxis.Buffer_OP_Y, Port.CylCtrlList.BWD);
                LabelFunc.SetText(lbl_Number_BufferOP_Y_FWD, FindTextIndex(DGV_BufferOPYAxisSensorStatus, $"{Port.DGV_BufferOP_YAxisSensorRow.Cylinder_FWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Y_FWD, bFWDStatus ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Y_FWD, bFWDStatus ? "On" : "Off");

                LabelFunc.SetText(lbl_Number_BufferOP_Y_BWD, FindTextIndex(DGV_BufferOPYAxisSensorStatus, $"{Port.DGV_BufferOP_YAxisSensorRow.Cylinder_BWD_Pos}"));
                LabelFunc.SetBackColor(lbl_Status_BufferOP_Y_BWD, bBWDStatus ? Color.Lime : Color.White);
                LabelFunc.SetText(lbl_Status_BufferOP_Y_BWD, bBWDStatus ? "On" : "Off");
            }
        }
        
        
        private void AGV_TabPageUpdate(Port port)
        {
            //int nSaftyPanelHeight = panel_TwoBuffer_PictureBackPanel.Height;
            //int nSaftyPanelWidth = panel_TwoBuffer_PictureBackPanel.Width;

            //panel_TwoBuffer_Picture.Location = new Point((nSaftyPanelWidth - panel_TwoBuffer_Picture.Width) / 2, (nSaftyPanelHeight - panel_TwoBuffer_Picture.Height) / 2);

            port.Update_DGV_AGVToPort_PIOStatus(ref DGV_AGVToPort_PIO);
            port.Update_DGV_PortToAGV_PIOStatus(ref DGV_PortToAGV_PIO);

            ButtonFunc.SetBackColor(btn_Port_AGV_Load_REQ, port.PIOStatus_PortToAGV_Load_Req ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_AGV_Unload_REQ, port.PIOStatus_PortToAGV_Unload_Req ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_AGV_Ready, port.PIOStatus_PortToAGV_Ready ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_AGV_ES, port.PIOStatus_PortToAGV_ES ? Color.Lime : Color.White);
        }
        private void OMRON_TabPageUpdate(Port port)
        {
            port.Update_DGV_OMRONToPort_PIOStatus(ref DGV_OMRONToPort_PIO);
            port.Update_DGV_PortToOMRON_PIOStatus(ref DGV_PortToOMRON_PIO);

            ButtonFunc.SetBackColor(btn_Port_OMRON_TR_REQ, port.PIOStatus_PortToOMRON_TR_REQ ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_OMRON_Busy_REQ, port.PIOStatus_PortToOMRON_Busy_REQ ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_OMRON_Complete_REQ, port.PIOStatus_PortToOMRON_Complete ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_OMRON_Error_REQ, port.PIOStatus_PortToOMRON_Error ? Color.Red : Color.White);
        }
        private void OHT_TabPageUpdate(Port port)
        {
            //int nSaftyPanelHeight = panel_TwoBuffer_PictureBackPanel.Height;
            //int nSaftyPanelWidth = panel_TwoBuffer_PictureBackPanel.Width;

            //panel_TwoBuffer_Picture.Location = new Point((nSaftyPanelWidth - panel_TwoBuffer_Picture.Width) / 2, (nSaftyPanelHeight - panel_TwoBuffer_Picture.Height) / 2);

            if (port.GetOperationDirection() == Port.PortDirection.Input)
            {
                if (pictureBox_OHT_TestInfo.BackgroundImage != Properties.Resources.Load)
                    pictureBox_OHT_TestInfo.BackgroundImage = Properties.Resources.Load;
            }
            else if (port.GetOperationDirection() == Port.PortDirection.Output)
            {
                if (pictureBox_OHT_TestInfo.BackgroundImage != Properties.Resources.Unload)
                    pictureBox_OHT_TestInfo.BackgroundImage = Properties.Resources.Unload;
            }

            port.Update_DGV_OHTToPort_PIOStatus(ref DGV_OHTToPort_PIO);
            port.Update_DGV_PortToOHT_PIOStatus(ref DGV_PortToOHT_PIO);

            ButtonFunc.SetBackColor(btn_Port_OHT_Load_REQ, port.PIOStatus_PortToOHT_Load_Req ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_OHT_Unload_REQ, port.PIOStatus_PortToOHT_Unload_Req ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_OHT_Ready, port.PIOStatus_PortToOHT_Ready ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_OHT_ES, port.PIOStatus_PortToOHT_ES ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_Port_OHT_HO_AVBL, port.PIOStatus_PortToOHT_HO_AVBL ? Color.Lime : Color.White);
        }
        private void EQ_TabPageUpdate(Port port)
        {
            groupBox_PortToRM_ManualPIO.Visible = false;
            //ButtonFunc.SetBackColor(btn_Port_EQ_Load_REQ, port.PIOStatus_PortToSTK_Load_Req ? Color.Lime : Color.White);
            //ButtonFunc.SetBackColor(btn_Port_EQ_Unload_REQ, port.PIOStatus_PortToSTK_Unload_Req ? Color.Lime : Color.White);
            //ButtonFunc.SetBackColor(btn_Port_EQ_Ready, port.PIOStatus_PortToSTK_Ready ? Color.Lime : Color.White);


            foreach (var rackMaster in Master.m_RackMasters)
            {
                if (rackMaster.Value.Status_AutoMode)
                {
                    port.PIOStatus_ManualReleaseSTKToEQPIO();
                }
            }

            port.Update_DGV_RMToPort_PIOStatus(ref DGV_RMToPort_PIO);
            port.Update_DGV_PortToRM_PIOStatus(ref DGV_PortToRM_PIO);

            ButtonFunc.SetBackColor(btn_STK_EQ_TR_REQ, port.PIOStatus_STKToPort_TR_REQ ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_STK_EQ_BUSY, port.PIOStatus_STKToPort_Busy ? Color.Lime : Color.White);
            ButtonFunc.SetBackColor(btn_STK_EQ_Complete, port.PIOStatus_STKToPort_Complete ? Color.Lime : Color.White);
        }
        
        
        /// <summary>
        /// Motion Command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Stop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (CurrentPort.IsAutoControlRun())
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_InAutoControl"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Button btn = (Button)sender;

            btn.Tag = null;

            CurrentPort.CMD_PortStop();
        }
        private void btn_TeachingPos_MouseUp(object sender, MouseEventArgs e)
        {
            btn_Stop_Click(sender, e);
        }

        private void btn_X_Axis_LP_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Shuttle_X} LP Motion Click");
            CurrentPort.X_Axis_MotionAndDone(Port.PortAxis.Shuttle_X, CurrentPort.IsMGV() ? Port.Teaching_X_Pos.MGV_LP_Pos : Port.Teaching_X_Pos.Equip_LP_Pos, false);
        }
        private void btn_X_Axis_Wait_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Shuttle_X} Wait Motion Click");
            CurrentPort.X_Axis_MotionAndDone(Port.PortAxis.Shuttle_X, Port.Teaching_X_Pos.Wait_Pos, false);
        }
        private void btn_X_Axis_OP_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Shuttle_X} OP Motion Click");
            CurrentPort.X_Axis_MotionAndDone(Port.PortAxis.Shuttle_X, Port.Teaching_X_Pos.OP_Pos, false);
        }

        private void btn_Z_Axis_UP_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_Z_Axis_UP_PosMove)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Shuttle_Z} Up Motion Click");
                CurrentPort.Z_Axis_MotionAndDone(Port.PortAxis.Shuttle_Z, Port.Teaching_Z_Pos.Up_Pos, false);
            }
            else if (btn == btn_BufferLP_Z_Axis_UP_Move)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Buffer_LP_Z} Up Motion Click");
                CurrentPort.Z_Axis_MotionAndDone(Port.PortAxis.Buffer_LP_Z, Port.Teaching_Z_Pos.Up_Pos, false);
            }
            else if (btn == btn_BufferOP_Z_Axis_UP_Move)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Buffer_OP_Z} Up Motion Click");
                CurrentPort.Z_Axis_MotionAndDone(Port.PortAxis.Buffer_OP_Z, Port.Teaching_Z_Pos.Up_Pos, false);
            }
        }
        private void btn_Y_Axis_FWD_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_BufferOP_Y_Axis_FWD_Move)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Buffer_OP_Y} FWD Motion Click");
                CurrentPort.Y_Axis_MotionAndDone(Port.PortAxis.Buffer_OP_Y, Port.Teaching_Y_Pos.FWD_Pos, false);
            }
        }
        private void btn_Y_Axis_BWD_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_BufferOP_Y_Axis_BWD_Move)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Buffer_OP_Y} BWD Motion Click");
                CurrentPort.Y_Axis_MotionAndDone(Port.PortAxis.Buffer_OP_Y, Port.Teaching_Y_Pos.BWD_Pos, false);
            }
        }
        private void btn_Z_Axis_Down_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_Z_Axis_Down_PosMove)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Shuttle_Z} Down Motion Click");
                CurrentPort.Z_Axis_MotionAndDone(Port.PortAxis.Shuttle_Z, Port.Teaching_Z_Pos.Down_Pos, false);
            }
            else if (btn == btn_BufferLP_Z_Axis_Down_Move)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Buffer_LP_Z} Down Motion Click");
                CurrentPort.Z_Axis_MotionAndDone(Port.PortAxis.Buffer_LP_Z, Port.Teaching_Z_Pos.Down_Pos, false);
            }
            else if (btn == btn_BufferOP_Z_Axis_Down_Move)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Buffer_OP_Z} Down Motion Click");
                CurrentPort.Z_Axis_MotionAndDone(Port.PortAxis.Buffer_OP_Z, Port.Teaching_Z_Pos.Down_Pos, false);
            }
        }

        private void btn_T_Axis_0Deg_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null || CurrentPort.GetMotionParam().GetServoAxisNum(Port.PortAxis.Shuttle_T) == -1)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Shuttle_T} 0 Deg Motion Click");
            CurrentPort.T_Axis_MotionAndDone(Port.PortAxis.Shuttle_T, Port.Teaching_T_Pos.Degree0_Pos, false);
        }
        private void btn_T_Axis_180Deg_PosMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null || CurrentPort.GetMotionParam().GetServoAxisNum(Port.PortAxis.Shuttle_T) == -1)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {Port.PortAxis.Shuttle_T} 180 Deg Motion Click");
            CurrentPort.T_Axis_MotionAndDone(Port.PortAxis.Shuttle_T, Port.Teaching_T_Pos.Degree180_Pos, false);
        }


        /// <summary>
        /// Motion Image Update
        /// </summary>
        /// <param name="port"></param>
        private void X_Axis_ImageUpdate(Port port)
        {
            if (port == null)
            {
                return;
            }

            int Width = panel_XAxis_Image.Width - lbl_X_Axis_Image.Width;
            int Height = panel_XAxis_Image.Height;

            float LPPos = port.GetMotionParam().GetTeachingPos(Port.PortAxis.Shuttle_X, (int)(port.IsMGV() ? Port.Teaching_X_Pos.MGV_LP_Pos : Port.Teaching_X_Pos.Equip_LP_Pos));
            float WaitPos = port.GetMotionParam().GetTeachingPos(Port.PortAxis.Shuttle_X, (int)Port.Teaching_X_Pos.Wait_Pos);
            float OPPos = port.GetMotionParam().GetTeachingPos(Port.PortAxis.Shuttle_X, (int)Port.Teaching_X_Pos.OP_Pos);

            float CurrentPos = (float)port.Motion_CurrentPosition(Port.PortAxis.Shuttle_X);

            LabelFunc.SetText(lbl_OPPos, $"{OPPos.ToString("0.0")} mm");
            LabelFunc.SetText(lbl_WaitPos, $"{WaitPos.ToString("0.0")} mm");
            LabelFunc.SetText(lbl_LPPos, $"{LPPos.ToString("0.0")} mm");

            if (port.GetMotionParam().IsWaitPosEnable(Port.PortAxis.Shuttle_X))
            {
                LabelFunc.SetVisible(lbl_WaitPos, true);
                LabelFunc.SetVisible(label_WaitPosImage, true);
                ButtonFunc.SetVisible(btn_X_Axis_Wait_PosMove, true);

                if (CurrentPos <= WaitPos)
                {
                    float XPosPercent = (CurrentPos - LPPos) / (WaitPos - LPPos);
                    LabelFunc.SetLocation(lbl_X_Axis_Image, new Point((int)((Width / 2) * XPosPercent), (Height - lbl_X_Axis_Image.Height) / 2));
                }
                else
                {
                    float XPosPercent = (CurrentPos - WaitPos) / (OPPos - WaitPos);
                    LabelFunc.SetLocation(lbl_X_Axis_Image, new Point((int)((Width / 2) * XPosPercent) + (Width / 2), (Height - lbl_X_Axis_Image.Height) / 2));
                }
            }
            else
            {
                LabelFunc.SetVisible(lbl_WaitPos, false);
                LabelFunc.SetVisible(label_WaitPosImage, false);
                ButtonFunc.SetVisible(btn_X_Axis_Wait_PosMove, false);

                float XPosPercent = (CurrentPos - LPPos) / (OPPos - LPPos);
                LabelFunc.SetLocation(lbl_X_Axis_Image, new Point((int)((Width) * XPosPercent), (Height - lbl_X_Axis_Image.Height) / 2));
            }
        }
        private void Z_Axis_ImageUpdate(Port port)
        {
            if (port == null)
            {
                return;
            }

            int Width = panel_ZAxis_Image.Width;
            int Height = panel_ZAxis_Image.Height - lbl_Z_Axis_Image.Height;

            float DownPos = port.GetMotionParam().GetTeachingPos(Port.PortAxis.Shuttle_Z, (int)Port.Teaching_Z_Pos.Down_Pos);
            float UpPos = port.GetMotionParam().GetTeachingPos(Port.PortAxis.Shuttle_Z, (int)Port.Teaching_Z_Pos.Up_Pos);

            float CurrentPos = (float)port.Motion_CurrentPosition(Port.PortAxis.Shuttle_Z);

            LabelFunc.SetText(lbl_ZDownPos, $"{DownPos.ToString("0.0")} mm");
            LabelFunc.SetText(lbl_ZUpPos, $"{UpPos.ToString("0.0")} mm");

            float ZPosPercent = (CurrentPos - DownPos) / (UpPos - DownPos);
            LabelFunc.SetLocation(lbl_Z_Axis_Image, new Point(Width - lbl_Z_Axis_Image.Width, (Height - (int)(Height * ZPosPercent))));
        }
        private void T_Axis_ImageUpdate(Port port)
        {
            if (port == null)
            {
                return;
            }

            float Deg0Pos = port.GetMotionParam().GetTeachingPos(Port.PortAxis.Shuttle_T, (int)Port.Teaching_T_Pos.Degree0_Pos);
            float Deg180Pos = port.GetMotionParam().GetTeachingPos(Port.PortAxis.Shuttle_T, (int)Port.Teaching_T_Pos.Degree180_Pos);
            float CurrentPos = (float)port.Motion_CurrentPosition(Port.PortAxis.Shuttle_T);

            LabelFunc.SetText(lbl_TDeg0Pos, $"{Deg0Pos.ToString("0.0")} mm");
            LabelFunc.SetText(lbl_TDeg180Pos, $"{Deg180Pos.ToString("0.0")} mm");

            float TPosPercent = (CurrentPos - Deg0Pos) / (Deg180Pos - Deg0Pos);

            float RotateDegree = 180.0f - (180.0f * TPosPercent);

            if (float.IsNaN(RotateDegree))
                RotateDegree = 0.0f;

            LabelFunc.SetImage(lbl_T_Axis_Image, RotateImage(Properties.Resources.icons8_plane_48, RotateDegree));
        }
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }


        /// <summary>
        /// Port -> AGV Manual PIO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Port_OHT_Load_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OHT Manual PIO - Load REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToOHTPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOHT_LoadREQ(!CurrentPort.PIOStatus_PortToOHT_Load_Req, Port.InterlockFrom.UI_Event);
        }
        private void btn_Port_OHT_Unload_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OHT Manual PIO - Unload REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToOHTPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOHT_UnloadREQ(!CurrentPort.PIOStatus_PortToOHT_Unload_Req, Port.InterlockFrom.UI_Event);
        }
        private void btn_Port_OHT_Ready_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OHT Manual PIO - Ready Click");
            CurrentPort.PIOStatus_ManualSavePortToOHTPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOHT_Ready(!CurrentPort.PIOStatus_PortToOHT_Ready, Port.InterlockFrom.UI_Event);
        }
        private void btn_Port_OHT_ES_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OHT Manual PIO - ES Click");
            CurrentPort.PIOStatus_ManualSavePortToOHTPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOHT_ES(!CurrentPort.PIOStatus_PortToOHT_ES, Port.InterlockFrom.UI_Event);
        }

        private void btn_Port_OHT_HO_AVBL_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OHT Manual PIO - HO_AVBL Click");
            CurrentPort.PIOStatus_ManualSavePortToOHTPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOHT_HO_AVBL(!CurrentPort.PIOStatus_PortToOHT_HO_AVBL, Port.InterlockFrom.UI_Event);
        }


        /// <summary>
        /// Port -> AGV Manual PIO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Port_AGV_Load_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] AGV Manual PIO - Load REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToAGVPIO();
            CurrentPort.Interlock_Manual_PIO_PortToAGV_LoadREQ(!CurrentPort.PIOStatus_PortToAGV_Load_Req, Port.InterlockFrom.UI_Event);
        }
        private void btn_Port_AGV_Unload_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] AGV Manual PIO - Unload REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToAGVPIO();
            CurrentPort.Interlock_Manual_PIO_PortToAGV_UnloadREQ(!CurrentPort.PIOStatus_PortToAGV_Unload_Req, Port.InterlockFrom.UI_Event);
        }
        private void btn_Port_AGV_Ready_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] AGV Manual PIO - Ready Click");
            CurrentPort.PIOStatus_ManualSavePortToAGVPIO();
            CurrentPort.Interlock_Manual_PIO_PortToAGV_Ready(!CurrentPort.PIOStatus_PortToAGV_Ready, Port.InterlockFrom.UI_Event);
        }
        private void btn_Port_AGV_ES_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] AGV Manual PIO - ES Click");
            CurrentPort.PIOStatus_ManualSavePortToAGVPIO();
            CurrentPort.Interlock_Manual_PIO_PortToAGV_ES(!CurrentPort.PIOStatus_PortToAGV_ES, Port.InterlockFrom.UI_Event);
        }

        private void btn_Port_EQ_Load_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] EQ Manual PIO - Load REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToRMPIO();
            CurrentPort.Interlock_Manual_PIO_PortToRM_LoadREQ(CurrentPort.PIOStatus_PortToSTK_Load_Req ? false : true, Port.InterlockFrom.UI_Event);
        }

        private void btn_Port_EQ_Unload_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] EQ Manual PIO - Unload REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToRMPIO();
            CurrentPort.Interlock_Manual_PIO_PortToRM_UnloadREQ(!CurrentPort.PIOStatus_PortToSTK_Unload_Req, Port.InterlockFrom.UI_Event);
        }

        private void btn_Port_EQ_Ready_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] EQ Manual PIO - Ready Click");
            CurrentPort.PIOStatus_ManualSavePortToRMPIO();
            CurrentPort.Interlock_Manual_PIO_PortToRM_Ready(!CurrentPort.PIOStatus_PortToSTK_Ready, Port.InterlockFrom.UI_Event);
        }

        private void btn_STK_EQ_TR_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            foreach(var rackMaster in Master.m_RackMasters)
            {
                if(rackMaster.Value.Status_AutoMode)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InAutoMode"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] EQ Manual PIO - TR REQ Click");
            CurrentPort.PIOStatus_ManualSaveSTKToEQPIO();
            CurrentPort.Interlock_Manual_PIO_STKToEQ_TR_REQ(CurrentPort.PIOStatus_STKToPort_TR_REQ ? false : true, Port.InterlockFrom.UI_Event);
        }

        private void btn_STK_EQ_BUSY_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            foreach (var rackMaster in Master.m_RackMasters)
            {
                if (rackMaster.Value.Status_AutoMode)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InAutoMode"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] EQ Manual PIO - BUSY Click");
            CurrentPort.PIOStatus_ManualSaveSTKToEQPIO();
            CurrentPort.Interlock_Manual_PIO_STKToEQ_BUSY(CurrentPort.PIOStatus_STKToPort_Busy ? false : true, Port.InterlockFrom.UI_Event);
        }

        private void btn_STK_EQ_Complete_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            foreach (var rackMaster in Master.m_RackMasters)
            {
                if (rackMaster.Value.Status_AutoMode)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_InAutoMode"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] EQ Manual PIO - COMPLETE Click");
            CurrentPort.PIOStatus_ManualSaveSTKToEQPIO();
            CurrentPort.Interlock_Manual_PIO_STKToEQ_Complete(CurrentPort.PIOStatus_STKToPort_Complete ? false : true, Port.InterlockFrom.UI_Event);
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if ((string)btn.Tag == "Push")
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                btn.Tag = null;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] All Motion Stop (Mouse Leave)");

                btn_Stop_Click(sender, e);
            }
        }

        private void btn_Port_OMRON_TR_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OMRON Manual PIO - TR REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToOMRONPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOMRON_TR_REQ(!CurrentPort.PIOStatus_PortToOMRON_TR_REQ, Port.InterlockFrom.UI_Event);
        }

        private void btn_Port_OMRON_Busy_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OMRON Manual PIO - Busy REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToOMRONPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOMRON_Busy_REQ(!CurrentPort.PIOStatus_PortToOMRON_Busy_REQ, Port.InterlockFrom.UI_Event);
        }

        private void btn_Port_OMRON_Complete_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OMRON Manual PIO - Complete REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToOMRONPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOMRON_Complete(!CurrentPort.PIOStatus_PortToOMRON_Complete, Port.InterlockFrom.UI_Event);
        }

        private void btn_Port_OMRON_Error_REQ_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] OMRON Manual PIO - Error REQ Click");
            CurrentPort.PIOStatus_ManualSavePortToOMRONPIO();
            CurrentPort.Interlock_Manual_PIO_PortToOMRON_Error(!CurrentPort.PIOStatus_PortToOMRON_Error, Port.InterlockFrom.UI_Event);
        }

        private void ShowIOSettingForm(Port port, Port.Port_IO_TabPage TabPage)
        {
            SyncRefreshSafetyInfo(TabPage);
            GlobalForm.Frm_PortSafetyMapSettings frm_PortSafetyMapSettings = new GlobalForm.Frm_PortSafetyMapSettings(port, TabPage, port.GetUIParam());
            frm_PortSafetyMapSettings.StartPosition = FormStartPosition.CenterParent;
            frm_PortSafetyMapSettings.ApplyEvent += Frm_PortSafetyMapSettings_ApplyEvent;
            frm_PortSafetyMapSettings.ShowDialog();
            frm_PortSafetyMapSettings.ApplyEvent -= Frm_PortSafetyMapSettings_ApplyEvent;
            frm_PortSafetyMapSettings.Dispose();
        }
        private void SyncSafetyInfo()
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            var portSafetyInfos = CurrentPort.GetUIParam().port_SafetyImageInfos;

            for(int nCount =0; nCount < Enum.GetNames(typeof(Port.Port_IO_TabPage)).Length; nCount++)
            {
                Port.Port_IO_TabPage eTabPage = (Port.Port_IO_TabPage)nCount;
                var _PortSafetyImageInfo = portSafetyInfos[nCount];

                switch (eTabPage)
                {
                    case Port.Port_IO_TabPage.TwoBuffer:

                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_TwoBuffer);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect1], tpnl_TwoBuf_LP_CartDetect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect2], tpnl_TwoBuf_LP_CartDetect2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Detect1], tpnl_TwoBuf_LP_CSTDetect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Detect2], tpnl_TwoBuf_LP_CSTDetect2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_TwoBuf_LP_CSTPresence);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Hoist_Detect], tpnl_TwoBuf_LP_HoistDetect);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_TwoBuf_OP_CSTDetect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_TwoBuf_OP_CSTDetect2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Presence], tpnl_TwoBuf_OP_CSTPresence);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_TwoBuf_OP_ForkDetect);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect1], tpnl_TwoBuf_Shuttle_CSTDetect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect2], tpnl_TwoBuf_Shuttle_CSTDetect2);
                        break;
                    case Port.Port_IO_TabPage.OneBuffer:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_OneBuffer);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect1], tpnl_OneBuf_LP_CartDetect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect2], tpnl_OneBuf_LP_CartDetect2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_OneBuf_LP_CSTPresence);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Hoist_Detect], tpnl_OneBuf_LP_HoistDetect);
                                       
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_OneBuf_OP_CSTDetect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_OneBuf_OP_CSTDetect2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Presence], tpnl_OneBuf_OP_CSTPresence);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_OneBuf_OP_ForkDetect);
                                       
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect1], tpnl_OneBuf_Shuttle_CSTDetect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect2], tpnl_OneBuf_Shuttle_CSTDetect2);

                        break;
                    case Port.Port_IO_TabPage.Conveyor:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ConveyorBuffer);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CV_In], tpnl_CVBuf_LP_CV_In);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CV_Out], tpnl_CVBuf_LP_CV_Out);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_CVBuf_LP_Presence_Detect);
                                        
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CV_In], tpnl_CVBuf_OP_CV_In);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CV_Out], tpnl_CVBuf_OP_CV_Out);
                                         
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP1_CST_Detect], tpnl_CVBuf_BP1_CST_Detect);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP2_CST_Detect], tpnl_CVBuf_BP2_CST_Detect);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP3_CST_Detect], tpnl_CVBuf_BP3_CST_Detect);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP4_CST_Detect], tpnl_CVBuf_BP4_CST_Detect);
                                         
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_CVBuf_OP_CST_Detect1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_CVBuf_OP_CST_Detect2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_CVBuf_OP_ForkDetect);
                        break;
                    case Port.Port_IO_TabPage.Shuttle_X:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ShuttleXAxis);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.NOT], tpnl_Shuttle_X_NOT);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.HOME], tpnl_Shuttle_X_Home);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.WaitPosSensor], tpnl_Shuttle_X_Wait);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.Pos], tpnl_Shuttle_X_Pos);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.POT], tpnl_Shuttle_X_POT);
                        break;
                    case Port.Port_IO_TabPage.Shuttle_Z:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ShuttleZAxis);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.NOT], tpnl_Shuttle_Z_NOT);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.HOME], tpnl_Shuttle_Z_Home);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Pos], tpnl_Shuttle_Z_Pos);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.POT], tpnl_Shuttle_Z_POT);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Cylinder_BWD_Pos], tpnl_Shuttle_Z_BWD);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Cylinder_FWD_Pos], tpnl_Shuttle_Z_FWD);
                        break;
                    case Port.Port_IO_TabPage.Shuttle_T:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ShuttleTAxis);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.NOT], tpnl_Shuttle_T_NOT);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.HOME], tpnl_Shuttle_T_Home);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Pos], tpnl_Shuttle_T_Pos);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.POT], tpnl_Shuttle_T_POT);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Degree_0_Position], tpnl_Shuttle_T_0Deg);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Degree_180_Position], tpnl_Shuttle_T_180Deg);
                        break;
                    case Port.Port_IO_TabPage.LP_Buffer_Z:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_LPBufferZAxis);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.NOT], tpnl_BufferLP_Z_NOT);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Pos1], tpnl_BufferLP_Z_Pos1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Pos2], tpnl_BufferLP_Z_Pos2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.POT], tpnl_BufferLP_Z_POT);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferLP_Z_BWD);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferLP_Z_FWD);
                        break;
                    case Port.Port_IO_TabPage.OP_Buffer_Z:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_OPBufferZAxis);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.NOT], tpnl_BufferOP_Z_NOT);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Pos1], tpnl_BufferOP_Z_Pos1);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Pos2], tpnl_BufferOP_Z_Pos2);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.POT], tpnl_BufferOP_Z_POT);

                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferOP_Z_BWD);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferOP_Z_FWD);
                        break;
                    case Port.Port_IO_TabPage.OP_Buffer_Y:
                        SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_OPBufferYAxis);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_YAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferOP_Y_BWD);
                        SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_YAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferOP_Y_FWD);
                        break;
                    case Port.Port_IO_TabPage.OHT:
                        {
                            SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_OHTPIOBackground, pictureBox_OHT);

                            //nCount == 0 ? "Port PIO" : "Equip PIO";
                            var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                            DGV_PortToOHT_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                            var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                            DGV_OHTToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
                        }
                        break;
                    case Port.Port_IO_TabPage.AGV:
                        {
                            SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_AGVPIOBackground, pictureBox_AGV);

                            //nCount == 0 ? "Port PIO" : "Equip PIO";
                            var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                            DGV_PortToAGV_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                            var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                            DGV_AGVToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
                        }
                        break;
                    case Port.Port_IO_TabPage.EQ:
                        {
                            SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_EQPIOBackground, pictureBox_EQ);

                            //nCount == 0 ? "Port PIO" : "Equip PIO";
                            var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                            DGV_PortToRM_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                            var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                            DGV_RMToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
                        }
                        break;
                    case Port.Port_IO_TabPage.OMRON:
                        {
                            SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_OmronPIOBackground, pictureBox_Omron);

                            //nCount == 0 ? "Port PIO" : "Equip PIO";
                            var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                            DGV_PortToOMRON_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                            var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                            DGV_OMRONToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void SyncRefreshSafetyInfo(Port.Port_IO_TabPage eTabPage)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            if (bTPScreenMode)
                return;

            switch (eTabPage)
            {
                case Port.Port_IO_TabPage.TwoBuffer:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect1], tpnl_TwoBuf_LP_CartDetect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect2], tpnl_TwoBuf_LP_CartDetect2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Detect1], tpnl_TwoBuf_LP_CSTDetect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Detect2], tpnl_TwoBuf_LP_CSTDetect2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_TwoBuf_LP_CSTPresence);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Hoist_Detect], tpnl_TwoBuf_LP_HoistDetect);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_TwoBuf_OP_CSTDetect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_TwoBuf_OP_CSTDetect2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Presence], tpnl_TwoBuf_OP_CSTPresence);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_TwoBuf_OP_ForkDetect);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect1], tpnl_TwoBuf_Shuttle_CSTDetect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect2], tpnl_TwoBuf_Shuttle_CSTDetect2);
                    break;
                case Port.Port_IO_TabPage.OneBuffer:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect1], tpnl_OneBuf_LP_CartDetect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect2], tpnl_OneBuf_LP_CartDetect2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_OneBuf_LP_CSTPresence);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Hoist_Detect], tpnl_OneBuf_LP_HoistDetect);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_OneBuf_OP_CSTDetect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_OneBuf_OP_CSTDetect2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Presence], tpnl_OneBuf_OP_CSTPresence);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_OneBuf_OP_ForkDetect);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect1], tpnl_OneBuf_Shuttle_CSTDetect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect2], tpnl_OneBuf_Shuttle_CSTDetect2);

                    break;
                case Port.Port_IO_TabPage.Conveyor:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CV_In], tpnl_CVBuf_LP_CV_In);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CV_Out], tpnl_CVBuf_LP_CV_Out);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_CVBuf_LP_Presence_Detect);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CV_In], tpnl_CVBuf_OP_CV_In);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CV_Out], tpnl_CVBuf_OP_CV_Out);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.BP1_CST_Detect], tpnl_CVBuf_BP1_CST_Detect);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.BP2_CST_Detect], tpnl_CVBuf_BP2_CST_Detect);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.BP3_CST_Detect], tpnl_CVBuf_BP3_CST_Detect);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.BP4_CST_Detect], tpnl_CVBuf_BP4_CST_Detect);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_CVBuf_OP_CST_Detect1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_CVBuf_OP_CST_Detect2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_CVBuf_OP_ForkDetect);
                    break;
                case Port.Port_IO_TabPage.Shuttle_X:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.NOT], tpnl_Shuttle_X_NOT);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.HOME], tpnl_Shuttle_X_Home);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.WaitPosSensor], tpnl_Shuttle_X_Wait);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.Pos], tpnl_Shuttle_X_Pos);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.POT], tpnl_Shuttle_X_POT);
                    break;                          
                case Port.Port_IO_TabPage.Shuttle_Z:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.NOT], tpnl_Shuttle_Z_NOT);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.HOME], tpnl_Shuttle_Z_Home);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Pos], tpnl_Shuttle_Z_Pos);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.POT], tpnl_Shuttle_Z_POT);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Cylinder_BWD_Pos], tpnl_Shuttle_Z_BWD);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Cylinder_FWD_Pos], tpnl_Shuttle_Z_FWD);
                    break;                          
                case Port.Port_IO_TabPage.Shuttle_T:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.NOT], tpnl_Shuttle_T_NOT);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.HOME], tpnl_Shuttle_T_Home);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Pos], tpnl_Shuttle_T_Pos);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.POT], tpnl_Shuttle_T_POT);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Degree_0_Position], tpnl_Shuttle_T_0Deg);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Degree_180_Position], tpnl_Shuttle_T_180Deg);
                    break;
                case Port.Port_IO_TabPage.LP_Buffer_Z:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.NOT], tpnl_BufferLP_Z_NOT);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Pos1], tpnl_BufferLP_Z_Pos1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Pos2], tpnl_BufferLP_Z_Pos2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.POT], tpnl_BufferLP_Z_POT);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferLP_Z_BWD);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferLP_Z_FWD);
                    break;
                case Port.Port_IO_TabPage.OP_Buffer_Z:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.NOT], tpnl_BufferOP_Z_NOT);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Pos1], tpnl_BufferOP_Z_Pos1);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Pos2], tpnl_BufferOP_Z_Pos2);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.POT], tpnl_BufferOP_Z_POT);

                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferOP_Z_BWD);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferOP_Z_FWD);
                    break;
                case Port.Port_IO_TabPage.OP_Buffer_Y:
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_YAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferOP_Y_BWD);
                    SetRefreshItemPanelLocation(ref CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[(int)Port.DGV_BufferOP_YAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferOP_Y_FWD);
                    break;
                case Port.Port_IO_TabPage.OHT:
                    {
                        //nCount == 0 ? "Port PIO" : "Equip PIO";
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_X = pictureBox_OHT.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_Y = pictureBox_OHT.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].X = DGV_PortToOHT_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].Y = DGV_PortToOHT_PIO.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].X = DGV_OHTToPort_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].Y = DGV_OHTToPort_PIO.Location.Y;
                    }
                    break;
                case Port.Port_IO_TabPage.AGV:
                    {
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_X = pictureBox_AGV.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_Y = pictureBox_AGV.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].X = DGV_PortToAGV_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].Y = DGV_PortToAGV_PIO.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].X = DGV_AGVToPort_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].Y = DGV_AGVToPort_PIO.Location.Y;
                        //nCount == 0 ? "Port PIO" : "Equip PIO";
                    }
                    break;
                case Port.Port_IO_TabPage.EQ:
                    {
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_X = pictureBox_EQ.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_Y = pictureBox_EQ.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].X = DGV_PortToRM_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].Y = DGV_PortToRM_PIO.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].X = DGV_RMToPort_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].Y = DGV_RMToPort_PIO.Location.Y;
                        //nCount == 0 ? "Port PIO" : "Equip PIO";
                    }
                    break;
                case Port.Port_IO_TabPage.OMRON:
                    {
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_X = pictureBox_Omron.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].EquipmentImageLocation_Y = pictureBox_Omron.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].X = DGV_PortToOMRON_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[0].Y = DGV_PortToOMRON_PIO.Location.Y;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].X = DGV_OMRONToPort_PIO.Location.X;
                        CurrentPort.GetUIParam().port_SafetyImageInfos[(int)eTabPage].SafetyItems[1].Y = DGV_OMRONToPort_PIO.Location.Y;
                        //nCount == 0 ? "Port PIO" : "Equip PIO";
                    }
                    break;
                default:
                    break;
            }
        }

        private void Frm_PortSafetyMapSettings_ApplyEvent(Port.Port_IO_TabPage TabPage, ManagedFile.EquipPortMotionParam.Port_SafetyImageInfo _PortSafetyImageInfo)
        {
            //if (bTPScreenMode)
            //    return;

            if(TabPage == Port.Port_IO_TabPage.TwoBuffer)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_TwoBuffer);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect1], tpnl_TwoBuf_LP_CartDetect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect2], tpnl_TwoBuf_LP_CartDetect2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Detect1], tpnl_TwoBuf_LP_CSTDetect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Detect2], tpnl_TwoBuf_LP_CSTDetect2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_TwoBuf_LP_CSTPresence);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Hoist_Detect], tpnl_TwoBuf_LP_HoistDetect);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_TwoBuf_OP_CSTDetect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_TwoBuf_OP_CSTDetect2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Presence], tpnl_TwoBuf_OP_CSTPresence);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_TwoBuf_OP_ForkDetect);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect1], tpnl_TwoBuf_Shuttle_CSTDetect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect2], tpnl_TwoBuf_Shuttle_CSTDetect2);
            }
            else if (TabPage == Port.Port_IO_TabPage.OneBuffer)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_OneBuffer);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect1], tpnl_OneBuf_LP_CartDetect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Cart_Detect2], tpnl_OneBuf_LP_CartDetect2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_OneBuf_LP_CSTPresence);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_Hoist_Detect], tpnl_OneBuf_LP_HoistDetect);
                                 
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_OneBuf_OP_CSTDetect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_OneBuf_OP_CSTDetect2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Presence], tpnl_OneBuf_OP_CSTPresence);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_OneBuf_OP_ForkDetect);
                             
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect1], tpnl_OneBuf_Shuttle_CSTDetect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.Shuttle_CST_Detect2], tpnl_OneBuf_Shuttle_CSTDetect2);

            }
            else if (TabPage == Port.Port_IO_TabPage.Conveyor)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ConveyorBuffer);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CV_In], tpnl_CVBuf_LP_CV_In);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CV_Out], tpnl_CVBuf_LP_CV_Out);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.LP_CST_Presence], tpnl_CVBuf_LP_Presence_Detect);
                               
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CV_In], tpnl_CVBuf_OP_CV_In);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CV_Out], tpnl_CVBuf_OP_CV_Out);
                               
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP1_CST_Detect], tpnl_CVBuf_BP1_CST_Detect);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP2_CST_Detect], tpnl_CVBuf_BP2_CST_Detect);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP3_CST_Detect], tpnl_CVBuf_BP3_CST_Detect);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.BP4_CST_Detect], tpnl_CVBuf_BP4_CST_Detect);
                            
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect1], tpnl_CVBuf_OP_CST_Detect1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_CST_Detect2], tpnl_CVBuf_OP_CST_Detect2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferSensorRow.OP_Fork_Detect], tpnl_CVBuf_OP_ForkDetect);
            }
            else if(TabPage == Port.Port_IO_TabPage.Shuttle_X)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ShuttleXAxis);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.NOT], tpnl_Shuttle_X_NOT);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.HOME], tpnl_Shuttle_X_Home);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.WaitPosSensor], tpnl_Shuttle_X_Wait);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.Pos], tpnl_Shuttle_X_Pos);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleXAxisSensorRow.POT], tpnl_Shuttle_X_POT);
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_Z)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ShuttleZAxis);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.NOT], tpnl_Shuttle_Z_NOT);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.HOME], tpnl_Shuttle_Z_Home);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Pos], tpnl_Shuttle_Z_Pos);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.POT], tpnl_Shuttle_Z_POT);
                               
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Cylinder_BWD_Pos], tpnl_Shuttle_Z_BWD);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleZAxisSensorRow.Cylinder_FWD_Pos], tpnl_Shuttle_Z_FWD);
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_T)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_ShuttleTAxis);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.NOT], tpnl_Shuttle_T_NOT);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.HOME], tpnl_Shuttle_T_Home);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Pos], tpnl_Shuttle_T_Pos);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.POT], tpnl_Shuttle_T_POT);
                                
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Degree_0_Position], tpnl_Shuttle_T_0Deg);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_ShuttleTAxisSensorRow.Degree_180_Position], tpnl_Shuttle_T_180Deg);
            }
            else if (TabPage == Port.Port_IO_TabPage.LP_Buffer_Z)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_LPBufferZAxis);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.NOT], tpnl_BufferLP_Z_NOT);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Pos1], tpnl_BufferLP_Z_Pos1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Pos2], tpnl_BufferLP_Z_Pos2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.POT], tpnl_BufferLP_Z_POT);
                                 
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferLP_Z_BWD);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferLP_ZAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferLP_Z_FWD);
            }
            else if (TabPage == Port.Port_IO_TabPage.OP_Buffer_Z)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_OPBufferZAxis);

                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.NOT], tpnl_BufferOP_Z_NOT);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Pos1], tpnl_BufferOP_Z_Pos1);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Pos2], tpnl_BufferOP_Z_Pos2);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.POT], tpnl_BufferOP_Z_POT);
                                                                                  
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferOP_Z_BWD);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_ZAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferOP_Z_FWD);
            }
            else if (TabPage == Port.Port_IO_TabPage.OP_Buffer_Y)
            {
                SetPictureBoxImage(_PortSafetyImageInfo, pictureBox_OPBufferYAxis);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_YAxisSensorRow.Cylinder_BWD_Pos], tpnl_BufferOP_Y_BWD);
                SetPanelLocation(ref _PortSafetyImageInfo.SafetyItems[(int)Port.DGV_BufferOP_YAxisSensorRow.Cylinder_FWD_Pos], tpnl_BufferOP_Y_FWD);
            }
            else if (TabPage == Port.Port_IO_TabPage.OHT)
            {
                SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_OHTPIOBackground, pictureBox_OHT);

                //nCount == 0 ? "Port PIO" : "Equip PIO";
                var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                DGV_PortToOHT_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                DGV_OHTToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
            }
            else if (TabPage == Port.Port_IO_TabPage.AGV)
            {
                SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_AGVPIOBackground, pictureBox_AGV);

                //nCount == 0 ? "Port PIO" : "Equip PIO";
                var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                DGV_PortToAGV_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                DGV_AGVToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
            }
            else if (TabPage == Port.Port_IO_TabPage.EQ)
            {
                SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_EQPIOBackground, pictureBox_EQ);

                //nCount == 0 ? "Port PIO" : "Equip PIO";
                var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                DGV_PortToRM_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                DGV_RMToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
            }
            else if (TabPage == Port.Port_IO_TabPage.OMRON)
            {
                SetPanelAndPictureboxImage(_PortSafetyImageInfo, panel_OmronPIOBackground, pictureBox_Omron);

                //nCount == 0 ? "Port PIO" : "Equip PIO";
                var PortPIOItem = _PortSafetyImageInfo.SafetyItems[0];
                DGV_PortToOMRON_PIO.Location = new Point((int)((float)PortPIOItem.X * bScaleXFactor), (int)((float)PortPIOItem.Y * bScaleYFactor));

                var EquipPIOItem = _PortSafetyImageInfo.SafetyItems[1];
                DGV_OMRONToPort_PIO.Location = new Point((int)((float)EquipPIOItem.X * bScaleXFactor), (int)((float)EquipPIOItem.Y * bScaleYFactor));
            }
        }
        private void SetPanelAndPictureboxImage(ManagedFile.EquipPortMotionParam.Port_SafetyImageInfo safetyImageInfo, Panel pnl, PictureBox picBox)
        {
            try
            {
                string ImagePath = safetyImageInfo.MainImagePath;
                Image image = Image.FromFile(ImagePath);

                if (image != null)
                {
                    pnl.BackgroundImage = image;
                }
            }
            catch
            {
                pnl.BackgroundImage = safetyImageInfo.GetDefaultImage();
            }

            try
            {
                string ImagePath = safetyImageInfo.EquipmentImagePath;
                Image image = Image.FromFile(ImagePath);

                if (image != null)
                {
                    picBox.BackgroundImage = image;
                    if(safetyImageInfo.EquipmentImageLocation_X != 0 && safetyImageInfo.EquipmentImageLocation_Y != 0)
                        picBox.Location = new Point((int)((float)safetyImageInfo.EquipmentImageLocation_X * bScaleXFactor), (int)((float)safetyImageInfo.EquipmentImageLocation_Y * bScaleYFactor));
                    else
                    {
                        safetyImageInfo.EquipmentImageLocation_X = picBox.Location.X;
                        safetyImageInfo.EquipmentImageLocation_Y = picBox.Location.Y;
                    }
                }
            }
            catch
            {
                picBox.BackgroundImage = safetyImageInfo.GetDefaultImage();
                if (safetyImageInfo.EquipmentImageLocation_X != 0 && safetyImageInfo.EquipmentImageLocation_Y != 0)
                    picBox.Location = new Point((int)((float)safetyImageInfo.EquipmentImageLocation_X * bScaleXFactor), (int)((float)safetyImageInfo.EquipmentImageLocation_Y * bScaleYFactor));
                else
                {
                    safetyImageInfo.EquipmentImageLocation_X = picBox.Location.X;
                    safetyImageInfo.EquipmentImageLocation_Y = picBox.Location.Y;
                }
            }
        }
        private void SetPictureBoxImage(ManagedFile.EquipPortMotionParam.Port_SafetyImageInfo safetyImageInfo, PictureBox picBox)
        {
            try
            {
                string ImagePath = safetyImageInfo.MainImagePath;
                Image image = Image.FromFile(ImagePath);

                if (image != null)
                {
                    picBox.BackgroundImage = image;
                }
            }
            catch
            {
                picBox.BackgroundImage = safetyImageInfo.GetDefaultImage();
            }
        }

        private void SetPanelLocation(ref ManagedFile.EquipPortMotionParam.Port_SafetyImageInfo.SafetyItem safetyItem, TableLayoutPanel tpnl)
        {
            if (safetyItem.X != 0 && safetyItem.Y != 0)
                tpnl.Location = new Point((int)((float)safetyItem.X * bScaleXFactor), (int)((float)safetyItem.Y * bScaleYFactor));
            else
            {
                safetyItem.X = tpnl.Location.X;
                safetyItem.Y = tpnl.Location.Y;
            }
        }

        private void SetRefreshItemPanelLocation(ref ManagedFile.EquipPortMotionParam.Port_SafetyImageInfo.SafetyItem safetyItem, TableLayoutPanel tpnl)
        {
            safetyItem.X = tpnl.Location.X;
            safetyItem.Y = tpnl.Location.Y;
        }

        private void btn_Settings_IOMap_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bTPScreenMode)
                return;

            if (btn == btn_Settings_TwoBufferIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.TwoBuffer);
            else if (btn == btn_Settings_OneBufferIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.OneBuffer);
            else if (btn == btn_Settings_CVBufferIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.Conveyor);
            else if (btn == btn_Settings_ShuttleXIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.Shuttle_X);
            else if (btn == btn_Settings_ShuttleZIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.Shuttle_Z);
            else if (btn == btn_Settings_ShuttleTIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.Shuttle_T);
            else if (btn == btn_Settings_LPBufferZIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.LP_Buffer_Z);
            else if (btn == btn_Settings_OPBufferZIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.OP_Buffer_Z);
            else if (btn == btn_Settings_OPBufferYIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.OP_Buffer_Y);
            else if (btn == btn_Settings_OHTIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.OHT);
            else if (btn == btn_Settings_AGVIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.AGV);
            else if (btn == btn_Settings_EQIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.EQ);
            else if (btn == btn_Settings_OmronIOMap)
                ShowIOSettingForm(CurrentPort, Port.Port_IO_TabPage.OMRON);
        }

        private void lbl_StatusLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            Label lbl = (Label)sender;
            lbl.Parent.Tag = "On";
        }

        private void lbl_StatusLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            Label lbl = (Label)sender;

            if ((string)lbl.Parent.Tag == "On" && e.Button == MouseButtons.Left)
                lbl.Parent.Location = new Point(e.X + lbl.Parent.Location.X - (lbl.Size.Width / 2), e.Y + lbl.Parent.Location.Y - (lbl.Size.Height / 2));
        }

        private void lbl_StatusLabel_MouseUp(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            Label lbl = (Label)sender;
            lbl.Parent.Tag = "Off";
        }

        private void DGV_PIODGV_MouseUp(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            DataGridView DGV = (DataGridView)sender;
            DGV.Tag = "Off";
        }

        private void DGV_PIODGV_MouseMove(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            DataGridView DGV = (DataGridView)sender;

            if ((string)DGV.Tag == "On" && e.Button == MouseButtons.Left)
                DGV.Location = new Point(e.X + DGV.Location.X - (DGV.Size.Width / 2), e.Y + DGV.Location.Y - (DGV.Size.Height / 2));
        }

        private void DGV_PIODGV_MouseDown(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            DataGridView DGV = (DataGridView)sender;
            DGV.Tag = "On";
        }

        private void pictureBox_Equip_MouseUp(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            PictureBox picBox = (PictureBox)sender;
            picBox.Tag = "Off";
        }

        private void pictureBox_Equip_MouseMove(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            PictureBox picBox = (PictureBox)sender;

            if ((string)picBox.Tag == "On" && e.Button == MouseButtons.Left)
                picBox.Location = new Point(e.X + picBox.Location.X - (picBox.Size.Width / 2), e.Y + picBox.Location.Y - (picBox.Size.Height / 2));
        }

        private void pictureBox_Equip_MouseDown(object sender, MouseEventArgs e)
        {
            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
                return;

            if (bTPScreenMode)
                return;

            PictureBox picBox = (PictureBox)sender;
            picBox.Tag = "On";
        }

        private void btn_Save_IOMap_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;

            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;

            if (LogIn.GetLogInLevel() < LogIn.LogInLevel.Admin)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_AdminPermissionError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (bTPScreenMode)
                return;

            if (btn == btn_Save_TwoBufferIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.TwoBuffer);
            else if (btn == btn_Save_OneBufferIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.OneBuffer);
            else if (btn == btn_Save_CVBufferIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.Conveyor);
            else if (btn == btn_Save_ShuttleXIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.Shuttle_X);
            else if (btn == btn_Save_ShuttleZIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.Shuttle_Z);
            else if (btn == btn_Save_ShuttleTIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.Shuttle_T);
            else if (btn == btn_Save_LPBufferZIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.LP_Buffer_Z);
            else if (btn == btn_Save_OPBufferZIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.OP_Buffer_Z);
            else if (btn == btn_Save_OPBufferYIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.OP_Buffer_Y);
            else if (btn == btn_Save_OHTIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.OHT);
            else if (btn == btn_Save_AGVIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.AGV);
            else if (btn == btn_Save_EQIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.EQ);
            else if (btn == btn_Save_OmronIOMap)
                SyncRefreshSafetyInfo(Port.Port_IO_TabPage.OMRON);

            if(CurrentPort.GetUIParam().Save(CurrentPort.GetParam().ID, CurrentPort.GetUIParam()))
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
