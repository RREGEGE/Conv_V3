using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.ManagedFile;
using System.IO;
using Master.Equipment.RackMaster;
using System.Diagnostics;

namespace Master.SubForm.RMTPForm.RMTPSubForm
{
    public partial class Frm_RackMasterTPTeaching : Form
    {
        enum DGV_TeachingListColumnIndex
        {
            Check,
            ShelfID,
            XPos,
            ZPos,
            State
        }
        enum CSVColumnType
        {
            ShelfID,
            XPos,
            ZPos,
            State
        }


        public Frm_RackMasterTPTeaching()
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

                foreach (Control item in tabPage_TableGenerator.Controls)
                    ControlFunc.Dispose(item);

                foreach (Control item in tabPage_XZOffsetCalculator.Controls)
                    ControlFunc.Dispose(item);

                if(!tabPage_TableGenerator.IsDisposed)
                    tabPage_TableGenerator.Dispose();

                if(!tabPage_XZOffsetCalculator.IsDisposed)
                    tabPage_XZOffsetCalculator.Dispose();

                if(!tabControl1.IsDisposed)
                    tabControl1.Dispose();

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);
        }
        private void DGV_TeachingList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex == -1)
            {
                //CheckBox가 이미 있다면 추가로 그리지 않음.
                for (int nCount = 0; nCount < DGV_TeachingList.Controls.Count; nCount++)
                {
                    var Control = DGV_TeachingList.Controls[nCount];
                    if (Control.GetType() == typeof(CheckBox))
                    {
                        return;
                    }
                }

                e.PaintBackground(e.ClipBounds, false);
                Point pt = e.CellBounds.Location;
                int nChkBoxWidth = 15;
                int nChkBoxHeight = 15;
                int offsetx = (e.CellBounds.Width - nChkBoxWidth) / 2;
                int offsety = (e.CellBounds.Height - nChkBoxHeight) / 2;
                pt.X += offsetx;
                pt.Y += offsety + 1;
                CheckBox cb = new CheckBox();
                cb.CheckAlign = ContentAlignment.MiddleCenter;
                cb.Size = new Size(nChkBoxWidth, nChkBoxHeight);
                cb.Location = pt;
                cb.CheckedChanged += new EventHandler(chx_SegOptionAll_CheckedChanged);
                ((DataGridView)sender).Controls.Add(cb);
                e.Handled = true;
            }
        }
        private void chx_SegOptionAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                bool bChecked = ((CheckBox)sender).Checked;

                for (int nRowIndex = 0; nRowIndex < DGV_TeachingList.Rows.Count; nRowIndex++)
                {
                    DataGridViewCell DGV_Cell = DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.Check];

                    if (DGV_Cell is DataGridViewCheckBoxCell)
                    {
                        if(!DGV_Cell.ReadOnly)
                            DGV_Cell.Value = bChecked;
                    }
                }

                DGV_TeachingList.CommitEdit(DataGridViewDataErrorContexts.Commit);
                DGV_TeachingList.RefreshEdit();
                DGV_TeachingList.EndEdit();
            }
            catch
            {

            }

        }

        private void DGV_TeachingList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderGrid = (DataGridView)sender;

                if (senderGrid == null || e.RowIndex < 0)
                    return;

                if (!(senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewCheckBoxCell))
                    return;

                senderGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                senderGrid.RefreshEdit();
                senderGrid.EndEdit();
            }
            catch
            {

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

                GroupBoxFunc.SetText(groupBox_TeachingFile, SynusLangPack.GetLanguage("GorupBox_TeachingFileInfo"));
                GroupBoxFunc.SetText(groupBox_CycleSettings, SynusLangPack.GetLanguage("GorupBox_TeachingStatus"));
                GroupBoxFunc.SetText(groupBox_SingleTeachingParam, SynusLangPack.GetLanguage("GorupBox_SingleTeachingParam"));
                GroupBoxFunc.SetText(groupBox_ContinuousTeachingParam, SynusLangPack.GetLanguage("GorupBox_ContinuousTeachingParam"));

                ButtonFunc.SetText(btn_NewFile, SynusLangPack.GetLanguage("Btn_NewFile"));
                ButtonFunc.SetText(btn_FileLoad, SynusLangPack.GetLanguage("Btn_FileLoad"));
                ButtonFunc.SetText(btn_FileSave, SynusLangPack.GetLanguage("Btn_FileSave"));
                ButtonFunc.SetText(btn_FileSaveAs, SynusLangPack.GetLanguage("Btn_FileSaveAs"));
                ButtonFunc.SetText(btn_Up, SynusLangPack.GetLanguage("Btn_RowUp"));
                ButtonFunc.SetText(btn_Down, SynusLangPack.GetLanguage("Btn_RowDown"));
                ButtonFunc.SetText(btn_Add, SynusLangPack.GetLanguage("Btn_RowAdd"));
                ButtonFunc.SetText(btn_Insert, SynusLangPack.GetLanguage("Btn_RowInsert"));
                ButtonFunc.SetText(btn_Remove, SynusLangPack.GetLanguage("Btn_RowRemove"));
                ButtonFunc.SetText(btn_SingleMode, SynusLangPack.GetLanguage("Btn_SingleTeachingMode"));
                ButtonFunc.SetText(btn_ContinuousMode, SynusLangPack.GetLanguage("Btn_ContinuousTeachingMode"));
                ButtonFunc.SetText(btn_TeachingRun, SynusLangPack.GetLanguage("Btn_TeachingStart"));
                
                ButtonFunc.SetText(btn_TeachingStop, SynusLangPack.GetLanguage("Btn_TeachingStop"));
                ButtonFunc.SetText(btn_GetCurrentShelfID, SynusLangPack.GetLanguage("Btn_LoadSelectedCellInfo"));

                ButtonFunc.SetText(btn_10000ShelfAllCheck, "1xxxx " + SynusLangPack.GetLanguage("Btn_NRangeShelfAllCheck"));
                ButtonFunc.SetText(btn_20000ShelfAllCheck, "2xxxx " + SynusLangPack.GetLanguage("Btn_NRangeShelfAllCheck"));
                ButtonFunc.SetText(btn_30000ShelfAllCheck, "3xxxx " + SynusLangPack.GetLanguage("Btn_NRangeShelfAllCheck"));
                ButtonFunc.SetText(btn_ShelfAllCheck, SynusLangPack.GetLanguage("Btn_ShelfAllCheck"));

                ButtonFunc.SetText(btn_10000ShelfFailShelfAllCheck, "1xxxx " + SynusLangPack.GetLanguage("Btn_NRangeFailShelfAllCheck"));
                ButtonFunc.SetText(btn_20000ShelfFailShelfAllCheck, "2xxxx " + SynusLangPack.GetLanguage("Btn_NRangeFailShelfAllCheck"));
                ButtonFunc.SetText(btn_30000ShelfFailShelfAllCheck, "3xxxx " + SynusLangPack.GetLanguage("Btn_NRangeFailShelfAllCheck"));
                ButtonFunc.SetText(btn_FailShelfAllCheck, SynusLangPack.GetLanguage("Btn_ShelfFailAllCheck"));

                ButtonFunc.SetText(btn_10000ShelfAllReleaseCheck, "1xxxx " + SynusLangPack.GetLanguage("Btn_NRangeShelfUncheck"));
                ButtonFunc.SetText(btn_20000ShelfAllReleaseCheck, "2xxxx " + SynusLangPack.GetLanguage("Btn_NRangeShelfUncheck"));
                ButtonFunc.SetText(btn_30000ShelfAllReleaseCheck, "3xxxx " + SynusLangPack.GetLanguage("Btn_NRangeShelfUncheck"));
                ButtonFunc.SetText(btn_ShelfAllReleaseCheck, SynusLangPack.GetLanguage("Btn_ShelfAllUncheck"));

                LabelFunc.SetText(label_SingleParamShelfID, SynusLangPack.GetLanguage("Label_ShelfID") + " :");
                LabelFunc.SetText(label_SingleParamStartXPos, SynusLangPack.GetLanguage("Label_TeachingStart_XPos") + " :");
                LabelFunc.SetText(label_SingleParamStartZPos, SynusLangPack.GetLanguage("Label_TeachingStart_ZPos") + " :");
                LabelFunc.SetText(label_SingleParamShelfState, SynusLangPack.GetLanguage("Label_ShelfSearchState") + " :");

                TabPageFunc.SetText(tabPage_TableGenerator, "1." + SynusLangPack.GetLanguage("TabPage_TableGenerator"));
                TabPageFunc.SetText(tabPage_XZOffsetCalculator, "2." + SynusLangPack.GetLanguage("TabPage_XZOffsetCalculator"));

                LabelFunc.SetText(lbl_BaseShelfID, SynusLangPack.GetLanguage("Label_BaseShelfID") + " :");
                LabelFunc.SetText(lbl_BaseShelfXPos, SynusLangPack.GetLanguage("Label_BaseShelfXPos")+" [mm] :");
                LabelFunc.SetText(lbl_BaseShelfZPos, SynusLangPack.GetLanguage("Label_BaseShelfZPos") + " [mm] :");
                LabelFunc.SetText(lbl_IntervalX, SynusLangPack.GetLanguage("Label_IntervalX") + " [mm] :");
                LabelFunc.SetText(lbl_IntervalZ, SynusLangPack.GetLanguage("Label_IntervalZ") + " [mm] :");
                LabelFunc.SetText(lbl_ColumnCount, SynusLangPack.GetLanguage("Label_ColumnCount") + " :");
                LabelFunc.SetText(lbl_RowCount, SynusLangPack.GetLanguage("Label_RowCount") + " :");

                ButtonFunc.SetText(btn_GetBaseShelfData, SynusLangPack.GetLanguage("Btn_GetBaseShelfData"));
                ButtonFunc.SetText(btn_CreateAutoTeachingTable, SynusLangPack.GetLanguage("Btn_CreateAutoTeachingTable"));

                LabelFunc.SetText(lbl_StartShelfID, SynusLangPack.GetLanguage("Label_StartShelfID") + " :");
                LabelFunc.SetText(lbl_EndShelfID, SynusLangPack.GetLanguage("Label_EndShelfID") + " :");
                LabelFunc.SetText(lbl_OffsetX, SynusLangPack.GetLanguage("Label_OffsetX") + " [mm] :");
                LabelFunc.SetText(lbl_OffsetZ, SynusLangPack.GetLanguage("Label_OffsetZ") + " [mm] :");

                ButtonFunc.SetText(btn_ApplyOffset, SynusLangPack.GetLanguage("Btn_OffsetApply"));


                RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

                if (rackMaster == null)
                    return;

                LabelFunc.SetText(lbl_LoadedFilePath, $"{SynusLangPack.GetLanguage("Label_LoadFilePath")} : {rackMaster.m_AutoTeachingFilePath}");

                bool bAutoTeachingRun = rackMaster.IsAutoTeachingRun();
                ButtonFunc.SetEnable(btn_SingleMode, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_ContinuousMode, !bAutoTeachingRun);
                ButtonFunc.SetBackColor(btn_SingleMode, rackMaster.GetTeachingMode() == RackMaster.TeachingMode.Single ? Color.Lime : Color.White);
                ButtonFunc.SetBackColor(btn_ContinuousMode, rackMaster.GetTeachingMode() == RackMaster.TeachingMode.Continuous ? Color.Lime : Color.White);

                ButtonFunc.SetEnable(btn_TeachingRun, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_TeachingStop, bAutoTeachingRun);
                ButtonFunc.SetBackColor(btn_TeachingRun, btn_TeachingRun.Enabled ? Color.White : Color.DarkGray);
                ButtonFunc.SetBackColor(btn_TeachingStop, btn_TeachingStop.Enabled ? Color.Orange : Color.DarkGray);

                if ((string)DGV_TeachingList.Tag != rackMaster.m_AutoTeachingFilePath)
                {
                    DGV_TeachingList.Tag = rackMaster.m_AutoTeachingFilePath;
                    Sync_FileAndGridView();
                }

                //GroupBoxFunc.SetEnable(groupBox_TeachingFile, rackMaster.IsAutoTeachingRun() ? false : true);
                ButtonFunc.SetEnable(btn_NewFile, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_FileLoad, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_FileSave, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_FileSaveAs, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_Up, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_Down, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_Add, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_Insert, !bAutoTeachingRun);
                ButtonFunc.SetEnable(btn_Remove, !bAutoTeachingRun);

                for (int nColumnCount = 0; nColumnCount < DGV_TeachingList.Columns.Count; nColumnCount++)
                {
                    if (nColumnCount != (int)DGV_TeachingListColumnIndex.State)
                    {
                        if (DGV_TeachingList.Columns[nColumnCount].ReadOnly != bAutoTeachingRun)
                            DGV_TeachingList.Columns[nColumnCount].ReadOnly = bAutoTeachingRun;
                    }
                }

                tabControl1.Enabled = !bAutoTeachingRun;

                GroupBoxFunc.SetEnable(groupBox_SingleTeachingParam, bAutoTeachingRun ? false : true);
                GroupBoxFunc.SetEnable(groupBox_ContinuousTeachingParam, bAutoTeachingRun ? false : true);

                DupleCheck();
                ShelfCheck();
                CycleInfoUpdate();
                TeachingProgressToDGV();

                if(rackMaster.Status_TeachingRWComplete)
                {
                    tbox_BaseShelfXPos.Text = Convert.ToString(rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_Teaching_Data_0));
                    tbox_BaseShelfZPos.Text = Convert.ToString(rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_Teaching_Data_0));

                    if(rackMaster.IsRunTeachingRWStopwatch())
                        rackMaster.StopTeachingRWStopwatch();

                    LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterTeachingRWSuccess,
                        $"X Data: {Convert.ToString(rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.X_Axis_Teaching_Data_0))}, " +
                        $"Z Data: {Convert.ToString(rackMaster.Get_RackMaster_2_CIM_Word_Data(RackMaster.SendWordMapIndex.Z_Axis_Teaching_Data_0))}");
                }
                else if(!rackMaster.Status_TeachingRWComplete && rackMaster.GetTeachingRWStopwatchTime() > 5)
                {
                    rackMaster.StopTeachingRWStopwatch();
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Failed_to_Get_Shelf_Data"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                ButtonFunc.SetBackColor(btn_GetBaseShelfData, rackMaster.IsRunTeachingRWStopwatch() ? Color.Lime : Color.White);
            }
            catch{ }
            finally
            {

            }
        }
        private void CycleInfoUpdate()
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            rackMaster.Update_Lbl_AutoTeachingStep(ref lbl_AutoTeachingStep);
            rackMaster.Update_Lbl_AutoTeachingError(ref lbl_AutoTeachingError);
            rackMaster.Update_Lbl_AutoTeachingProgressPercent(ref lbl_AutoTeachingProgressCount);
            rackMaster.Update_Lbl_AutoTeachingProgressTime(ref lbl_AutoTeachingProgressTime);
        }

        private void TeachingProgressToDGV()
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            if (rackMaster.TeachingList == null)
                return;

            var TeachingList = rackMaster.TeachingList;

            if (TeachingList.Count == 0)
            {
                for (int nCount = 0; nCount < DGV_TeachingList.Rows.Count; nCount++)
                {
                    string state = (string)DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value;

                    if (state == $"{RackMaster.ShelfTeachingState.Running}")
                    {
                        DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State];
                        DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value = $"{RackMaster.ShelfTeachingState.Running}";
                        DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State].Style.BackColor = Color.Orange;
                    }
                    else if (state == $"{RackMaster.ShelfTeachingState.Success}")
                    {
                        DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value = $"{RackMaster.ShelfTeachingState.Success}";
                        DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State].Style.BackColor = Color.Lime;
                    }
                    else if (state == $"{RackMaster.ShelfTeachingState.Fail}" ||
                            state == $"{RackMaster.ShelfTeachingState.UserStop}" ||
                            state == $"{RackMaster.ShelfTeachingState.ErrorStop}" ||
                            state == $"{RackMaster.ShelfTeachingState.DupleError}")
                    {
                        DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value = state;
                        DGV_TeachingList.Rows[nCount].Cells[(int)DGV_TeachingListColumnIndex.State].Style.BackColor = Color.Red;
                        }
                }
            }
            else
            {
                for (int nCount = 0; nCount < TeachingList.Count; nCount++)
                {
                    int nRowIndex = TeachingList[nCount].nRowIndex;
                    string state = TeachingList[nCount].State;

                    if (nRowIndex <= DGV_TeachingList.Rows.Count)
                    {
                        if (state == $"{RackMaster.ShelfTeachingState.Running}")
                        {
                            DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.State];
                            DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.State].Value = $"{RackMaster.ShelfTeachingState.Running}";
                            DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.State].Style.BackColor = Color.Orange;
                        }
                        else if (state == $"{RackMaster.ShelfTeachingState.Success}")
                        {
                            DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.State].Value = $"{RackMaster.ShelfTeachingState.Success}";
                            DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.State].Style.BackColor = Color.Lime;
                        }
                        else if (state == $"{RackMaster.ShelfTeachingState.Fail}" ||
                                state == $"{RackMaster.ShelfTeachingState.UserStop}" ||
                                state == $"{RackMaster.ShelfTeachingState.ErrorStop}" ||
                                state == $"{RackMaster.ShelfTeachingState.DupleError}")
                        {
                            DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.State].Value = state;
                            DGV_TeachingList.Rows[nRowIndex].Cells[(int)DGV_TeachingListColumnIndex.State].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        private void DupleCheck()
        {
            Dictionary<string, int> DupleShelfID = new Dictionary<string, int>();
            Dictionary<string, int> DupleXZPos = new Dictionary<string, int>();

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                string XPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                string ZPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);

                if (!DupleShelfID.ContainsKey(ShelfID))
                    DupleShelfID.Add(ShelfID, 1);
                else
                    DupleShelfID[ShelfID]++;

                if (!DupleXZPos.ContainsKey($"{XPos}_{ZPos}"))
                    DupleXZPos.Add($"{XPos}_{ZPos}", 1);
                else
                    DupleXZPos[$"{XPos}_{ZPos}"]++;
            }

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                bool bError = false;
                string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                string XPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                string ZPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);

                if (DupleShelfID.ContainsKey(ShelfID))
                {
                    if (DupleShelfID[ShelfID] >= 2)
                    {
                        bError = true;
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Style.BackColor = Color.White;
                    }
                }

                if (DupleXZPos.ContainsKey($"{XPos}_{ZPos}"))
                {
                    if (DupleXZPos[$"{XPos}_{ZPos}"] >= 3)
                    {
                        bError = true;
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Style.BackColor = Color.Red;
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Style.BackColor = Color.Red;
                    }
                    else
                    {
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Style.BackColor = Color.White;
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Style.BackColor = Color.White;
                    }
                }

                if(bError)
                    DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value = $"{RackMaster.ShelfTeachingState.DupleError}";
                else
                {
                    if (Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value) == $"{RackMaster.ShelfTeachingState.DupleError}")
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value = string.Empty;
                }
            }
        }

        private void ShelfCheck()
        {
            int nCount = 0;

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                if ((bool)DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value)
                    nCount++;
            }

            LabelFunc.SetText(lbl_AllShelf, $"{SynusLangPack.GetLanguage("Label_AllShelfCount")} : {DGV_TeachingList.Rows.Count.ToString()}");
            LabelFunc.SetText(lbl_CheckedShelf, $"{SynusLangPack.GetLanguage("Label_CheckedShelfCount")} : {nCount.ToString()}");
        }
        private void btn_NewFile_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Teaching New File Click");

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "csv";
            saveFile.Title = "New Teaching File Create";
            saveFile.Filter = ".CSV File(*.csv)|*.csv";
            saveFile.FileName = $"RM[{rackMaster.GetParam().ID ?? "None"}]_{ManagedFileInfo.TeachingFileName}";
            saveFile.InitialDirectory = ManagedFileInfo.TeachingFileDirectory;


            if (!System.IO.Directory.Exists(saveFile.InitialDirectory))           //폴더가 없다면
            {
                System.IO.Directory.CreateDirectory(saveFile.InitialDirectory);   //폴더 생성
            }

            DialogResult pathresult = saveFile.ShowDialog();

            if (pathresult != DialogResult.OK || saveFile.FileNames.Length <= 0)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Teaching New File Cancel");
                return;
            }

            rackMaster.m_AutoTeachingFilePath = saveFile.FileNames[0];

            StreamWriter sw = new StreamWriter(rackMaster.m_AutoTeachingFilePath);
            sw.WriteLine($",,");
            sw.Close();
            sw.Dispose();

            if ((string)DGV_TeachingList.Tag != rackMaster.m_AutoTeachingFilePath)
            {
                DGV_TeachingList.Tag = rackMaster.m_AutoTeachingFilePath;
                Sync_FileAndGridView();
            }
        }

        private void Sync_FileAndGridView()
        {
            try
            {
                for (int nCount = 0; nCount < DGV_TeachingList.Columns.Count; nCount++)
                {
                    switch (nCount)
                    {
                        case (int)DGV_TeachingListColumnIndex.ShelfID:
                            if (DGV_TeachingList.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_ShelfID"))
                                DGV_TeachingList.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_ShelfID");
                            break;
                        case (int)DGV_TeachingListColumnIndex.XPos:
                            if (DGV_TeachingList.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_TeachingXPos"))
                                DGV_TeachingList.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_TeachingXPos");
                            break;
                        case (int)DGV_TeachingListColumnIndex.ZPos:
                            if (DGV_TeachingList.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_TeachingZPos"))
                                DGV_TeachingList.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_TeachingZPos");
                            break;
                        case (int)DGV_TeachingListColumnIndex.State:
                            if (DGV_TeachingList.Columns[nCount].HeaderText != SynusLangPack.GetLanguage("DGV_TeachingResult"))
                                DGV_TeachingList.Columns[nCount].HeaderText = SynusLangPack.GetLanguage("DGV_TeachingResult");
                            break;
                    }
                }

                RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

                if (rackMaster == null)
                    return;

                if (string.IsNullOrEmpty(rackMaster.m_AutoTeachingFilePath))
                {
                    if (DGV_TeachingList.Rows.Count > 0)
                        DGV_TeachingList.Rows.Clear();
                }
                else
                {
                    if (!File.Exists(rackMaster.m_AutoTeachingFilePath))
                        return;

                    string[] strArray = File.ReadAllLines(rackMaster.m_AutoTeachingFilePath);

                    if (strArray.Length == 0)
                    {
                        LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.FileLineIsEmpty, $"Teaching File Not have Info");
                        return;
                    }
                    else
                    {
                        if (DGV_TeachingList.Rows.Count > 0)
                            DGV_TeachingList.Rows.Clear();
                    }


                    for(int nCount = 0; nCount < strArray.Length; nCount++)
                    {
                        string[] CSVColumnData = strArray[nCount].Split(',');

                        if (CSVColumnData.Length > Enum.GetValues(typeof(CSVColumnType)).Length)
                        {
                            LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.FileLineIsInvalid, $"Line: {nCount} / Value: {strArray[nCount]}");
                            continue;
                        }
                        else
                        {
                            if (CSVColumnData.Length != 4)
                                Array.Resize(ref CSVColumnData, 4);

                            string ShelfID = CSVColumnData[(int)CSVColumnType.ShelfID];
                            string XPos = CSVColumnData[(int)CSVColumnType.XPos];
                            string ZPos = CSVColumnData[(int)CSVColumnType.ZPos];
                            string State = CSVColumnData[(int)CSVColumnType.State];

                            DGV_TeachingList.Rows.Add(new object[] { true, ShelfID, XPos, ZPos, State });
                        }
                    }

                    //foreach (var str in strArray.Select((value, index) => (value, index)))
                    //{
                    //    string[] CSVColumnData = str.value.Split(',');

                    //    if (CSVColumnData.Length > Enum.GetValues(typeof(CSVColumnType)).Length)
                    //    {
                    //        LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Warning, LogMsg.MsgList.FileLineIsInvalid, $"Line: {str.index} / Value: {str.value}");
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        if (CSVColumnData.Length != 4)
                    //            Array.Resize(ref CSVColumnData, 4);

                    //        string ShelfID = CSVColumnData[(int)CSVColumnType.ShelfID];
                    //        string XPos = CSVColumnData[(int)CSVColumnType.XPos];
                    //        string ZPos = CSVColumnData[(int)CSVColumnType.ZPos];
                    //        string State = CSVColumnData[(int)CSVColumnType.State];

                    //        DGV_TeachingList.Rows.Add(new object[] { true, ShelfID, XPos, ZPos, State });
                    //    }
                    //}
                }
            }
            catch
            {

            }
        }
        private void btn_FileLoad_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Teaching File Load Click");

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.DefaultExt = "csv";
            openFile.Title = "Load Teaching File Create";
            openFile.Filter = ".CSV File(*.csv)|*.csv";
            openFile.InitialDirectory = ManagedFileInfo.TeachingFileDirectory;

            if (!System.IO.Directory.Exists(openFile.InitialDirectory))           //폴더가 없다면
            {
                System.IO.Directory.CreateDirectory(openFile.InitialDirectory);   //폴더 생성
            }

            DialogResult pathresult = openFile.ShowDialog();

            if (DialogResult.OK == pathresult && openFile.FileNames.Length > 0)
            {
                if (openFile.FileNames[0] != null)
                {
                    if (openFile.FileNames[0].Length < 255)
                    {
                        rackMaster.m_AutoTeachingFilePath = openFile.FileNames[0];
                        if ((string)DGV_TeachingList.Tag != rackMaster.m_AutoTeachingFilePath)
                        {
                            DGV_TeachingList.Tag = rackMaster.m_AutoTeachingFilePath;
                            Sync_FileAndGridView();
                        }
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_FilePathLengthOver"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Teaching File Load Cancel Click");
        }

        private void btn_FileSave_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Teaching File Save Click");

            if (string.IsNullOrEmpty(rackMaster.m_AutoTeachingFilePath))
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_NotLoadedFile"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.FilePathIsEmpty, $"RackMaster Teaching File");
                return;
            }

            if (TeachingFileSave(rackMaster.m_AutoTeachingFilePath))
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.FileSaveSuccess, $"RackMaster Teaching File");
            }
            else
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.FileSaveFail, $"RackMaster Teaching File");
            }
        }

        private void btn_FileSaveAs_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Teaching File Save as Click");

            if (string.IsNullOrEmpty(rackMaster.m_AutoTeachingFilePath))
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_NotLoadedFile"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.FilePathIsEmpty, $"RackMaster Teaching File");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.DefaultExt = "csv";
            saveFile.Title = "New Teaching File Create";
            saveFile.Filter = ".CSV File(*.csv)|*.csv";
            saveFile.FileName = $"RM[{rackMaster.GetParam().ID ?? "None"}]_{ManagedFileInfo.TeachingFileName}";
            saveFile.InitialDirectory = ManagedFileInfo.TeachingFileDirectory;


            if (!System.IO.Directory.Exists(saveFile.InitialDirectory))           //폴더가 없다면
            {
                System.IO.Directory.CreateDirectory(saveFile.InitialDirectory);   //폴더 생성
            }

            DialogResult pathresult = saveFile.ShowDialog();

            if (pathresult != DialogResult.OK || saveFile.FileNames.Length <= 0)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Teaching File Save as Cancel Click");
                return;
            }

            rackMaster.m_AutoTeachingFilePath = saveFile.FileNames[0];

            if (TeachingFileSave(rackMaster.m_AutoTeachingFilePath))
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.FileSaveSuccess, $"RackMaster Teaching File");
            }
            else
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveFail"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.FileSaveFail, $"RackMaster Teaching File");
            }
        }

        private bool TeachingFileSave(string path)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path);

                for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
                {
                    string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                    string XPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                    string ZPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);

                    sw.WriteLine($"{ShelfID},{XPos},{ZPos}");
                }

                sw.Close();
                sw.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void btn_Up_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Teaching Grid Row Up Click");

            DataGridViewRow DGV_Row = DGV_TeachingList.CurrentRow;

            if(DGV_Row == null)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidCurrentCell"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int RowIndex = DGV_Row.Index;

                if(RowIndex > 0)
                {
                    DGV_TeachingList.Rows.RemoveAt(RowIndex);
                    DGV_TeachingList.Rows.Insert(RowIndex - 1, DGV_Row);
                    DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[RowIndex - 1].Cells[0];
                }
                else
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidTopCell"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btn_Down_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Teaching Grid Row Down Click");

            DataGridViewRow DGV_Row = DGV_TeachingList.CurrentRow;

            if (DGV_Row == null)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidCurrentCell"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int RowIndex = DGV_Row.Index;

                if (RowIndex + 1 < DGV_TeachingList.Rows.Count)
                {
                    DGV_TeachingList.Rows.RemoveAt(RowIndex);
                    DGV_TeachingList.Rows.Insert(RowIndex + 1, DGV_Row);
                    DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[RowIndex + 1].Cells[0];
                }
                else
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidDownCell"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Teaching Grid Row Add Click");

            DGV_TeachingList.Rows.Add(new object[] { true, string.Empty, string.Empty, string.Empty, string.Empty });
            DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[DGV_TeachingList.Rows.Count - 1].Cells[0];
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Teaching Grid Row Insert Click");

            DataGridViewRow DGV_Row = DGV_TeachingList.CurrentRow;

            if (DGV_Row == null)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidCurrentCell"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int RowIndex = DGV_Row.Index;
                DGV_TeachingList.Rows.Insert(RowIndex, new object[] { true, string.Empty, string.Empty, string.Empty, string.Empty });
                DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[RowIndex].Cells[0];
            }
        }

        private void btn_Remove_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Teaching Grid Row Remove Click");

            DataGridViewRow DGV_Row = DGV_TeachingList.CurrentRow;

            if (DGV_Row == null)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidCurrentCell"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int RowIndex = DGV_Row.Index;
                DGV_TeachingList.Rows.RemoveAt(RowIndex);

                if(RowIndex < DGV_TeachingList.Rows.Count)
                    DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[RowIndex].Cells[0];
                else if(DGV_TeachingList.Rows.Count > 0)
                    DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[DGV_TeachingList.Rows.Count - 1].Cells[0];
            }
        }

        private void btn_SingleMode_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Teaching Single Mode Click");
            rackMaster.SetTeachingMode(Equipment.RackMaster.RackMaster.TeachingMode.Single);
        }

        private void btn_ContinuousMode_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Teaching Continuous Mode Click");
            rackMaster.SetTeachingMode(Equipment.RackMaster.RackMaster.TeachingMode.Continuous);
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

        private void tbx_ShelfID_TextChanged(object sender, EventArgs e)
        {
            try
            {


                bool bMatching = false;
                string UserInsertShelfID = tbx_ShelfID.Text;

                if (string.IsNullOrEmpty(UserInsertShelfID))
                {
                    LabelFunc.SetText(lbl_Shelf_State, "Not Find Shelf ID");
                    LabelFunc.SetText(lbl_Shelf_XPos, $"0 mm");
                    LabelFunc.SetText(lbl_Shelf_ZPos, $"0 mm");
                    LabelFunc.SetBackColor(lbl_Shelf_State, Color.Red);
                }

                for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
                {
                    string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                    string XPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                    string ZPos = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);

                    if (UserInsertShelfID == ShelfID)
                    {
                        DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[nRowCount].Cells[0];
                        LabelFunc.SetText(lbl_Shelf_XPos, $"{XPos} mm");
                        LabelFunc.SetText(lbl_Shelf_ZPos, $"{ZPos} mm");
                        bMatching = true;
                        break;
                    }
                }

                if (!bMatching)
                {
                    LabelFunc.SetText(lbl_Shelf_State, "Not Find Shelf ID");
                    LabelFunc.SetText(lbl_Shelf_XPos, $"0 mm");
                    LabelFunc.SetText(lbl_Shelf_ZPos, $"0 mm");
                    LabelFunc.SetBackColor(lbl_Shelf_State, Color.Red);
                }
                else
                {
                    LabelFunc.SetText(lbl_Shelf_State, "Find Success");
                    LabelFunc.SetBackColor(lbl_Shelf_State, Color.Lime);
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void btn_GetCurrentShelfID_Click(object sender, EventArgs e)
        {
            try
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Get Shelf ID Click");

                DataGridViewRow DGV_Row = DGV_TeachingList.CurrentRow;

                if (DGV_Row == null)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidCurrentCell"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    int RowIndex = DGV_Row.Index;

                    string ShelfID = Convert.ToString(DGV_TeachingList.Rows[RowIndex].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                    string XPos = Convert.ToString(DGV_TeachingList.Rows[RowIndex].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                    string ZPos = Convert.ToString(DGV_TeachingList.Rows[RowIndex].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);

                    tbx_ShelfID.Text = ShelfID;
                    LabelFunc.SetText(lbl_Shelf_XPos, $"{XPos} mm");
                    LabelFunc.SetText(lbl_Shelf_ZPos, $"{ZPos} mm");
                }
            }
            catch { }
        }

        private void btn_xxxxxShelfAllCheck_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn == null)
                return;

            if(btn == btn_10000ShelfAllCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-10000 Number Shelf All Check Click");
            else if (btn == btn_20000ShelfAllCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-20000 Number Shelf All Check Click");
            else if (btn == btn_30000ShelfAllCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-30000 Number Shelf All Check Click");

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                
                try
                {
                    int nShelfID = Convert.ToInt32(ShelfID);

                    if (nShelfID >= 10000 && nShelfID < 20000 && btn == btn_10000ShelfAllCheck ||
                        nShelfID >= 20000 && nShelfID < 30000 && btn == btn_20000ShelfAllCheck ||
                        nShelfID >= 30000 && nShelfID < 40000 && btn == btn_30000ShelfAllCheck)
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = true;
                }
                catch
                {
                    DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = false;
                }
            }
        }

        private void btn_xxxxxShelfFailShelfAllCheck_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn == null)
                return;

            if (btn == btn_10000ShelfFailShelfAllCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-10000 Number Fail Shelf All Check Click");
            else if (btn == btn_20000ShelfFailShelfAllCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-20000 Number Fail Shelf All Check Click");
            else if (btn == btn_30000ShelfFailShelfAllCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-30000 Number Fail Shelf All Check Click");

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                string State = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value);

                try
                {
                    int nShelfID = Convert.ToInt32(ShelfID);

                    bool bFailState = (State == $"{RackMaster.ShelfTeachingState.Fail}") || 
                                        string.IsNullOrEmpty(State) || 
                                        State == $"{RackMaster.ShelfTeachingState.ErrorStop}" || 
                                        State == $"{RackMaster.ShelfTeachingState.UserStop}";

                    if (nShelfID >= 10000 && nShelfID < 20000 && btn == btn_10000ShelfFailShelfAllCheck && (bFailState) ||
                        nShelfID >= 20000 && nShelfID < 30000 && btn == btn_20000ShelfFailShelfAllCheck && (bFailState) ||
                        nShelfID >= 30000 && nShelfID < 40000 && btn == btn_30000ShelfFailShelfAllCheck && (bFailState))
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = true;
                }
                catch
                {
                    DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = false;
                }
            }

            DGV_TeachingList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DGV_TeachingList.RefreshEdit();
            DGV_TeachingList.EndEdit();
        }

        private void btn_xxxxxShelfAllReleaseCheck_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn == null)
                return;

            if (btn == btn_10000ShelfAllReleaseCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-10000 Number Shelf All Check Release Click");
            else if (btn == btn_20000ShelfAllReleaseCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-20000 Number Shelf All Check Release Click");
            else if (btn == btn_30000ShelfAllReleaseCheck)
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-30000 Number Shelf All Check Release Click");

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);

                try
                {
                    int nShelfID = Convert.ToInt32(ShelfID);

                    if (nShelfID >= 10000 && nShelfID < 20000 && btn == btn_10000ShelfAllReleaseCheck ||
                        nShelfID >= 20000 && nShelfID < 30000 && btn == btn_20000ShelfAllReleaseCheck ||
                        nShelfID >= 30000 && nShelfID < 40000 && btn == btn_30000ShelfAllReleaseCheck)
                        DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = false;
                }
                catch
                {
                    DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = false;
                }
            }

            DGV_TeachingList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DGV_TeachingList.RefreshEdit();
            DGV_TeachingList.EndEdit();
        }

        private void btn_FailShelfAllCheck_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Fail Shelf All Check Click");

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                string State = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value);

                bool bFailState = (State == $"{RackMaster.ShelfTeachingState.Fail}") ||
                                        string.IsNullOrEmpty(State) ||
                                        State == $"{RackMaster.ShelfTeachingState.ErrorStop}" ||
                                        State == $"{RackMaster.ShelfTeachingState.UserStop}";

                if (bFailState)
                    DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = true;
            }

            DGV_TeachingList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DGV_TeachingList.RefreshEdit();
            DGV_TeachingList.EndEdit();
        }

        private void btn_ShelfAllCheck_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Shelf All Check Click");

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = true;
            }

            DGV_TeachingList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DGV_TeachingList.RefreshEdit();
            DGV_TeachingList.EndEdit();
        }

        private void btn_ShelfAllReleaseCheck_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-Shelf All Check Release Click");

            for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
            {
                DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value = false;
            }

            DGV_TeachingList.CommitEdit(DataGridViewDataErrorContexts.Commit);
            DGV_TeachingList.RefreshEdit();
            DGV_TeachingList.EndEdit();
        }

        private void btn_TeachingRun_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Auto Teaching Run Click");

            try
            {
                if(rackMaster.GetTeachingMode() == RackMaster.TeachingMode.Single)
                {
                    List<RackMaster.TeachingParam> teachingList = new List<RackMaster.TeachingParam>();
                    RackMaster.TeachingParam teachingParam = new RackMaster.TeachingParam();
                    teachingParam.ShelfID = tbx_ShelfID.Text;

                    for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
                    {
                        string ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);

                        if (teachingParam.ShelfID == ShelfID)
                        {
                            teachingParam.X_Pos = Convert.ToInt16(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                            teachingParam.Z_Pos = Convert.ToInt16(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);
                            teachingParam.State = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value);
                            teachingParam.nRowIndex = nRowCount;

                            if (!teachingParam.IsValid())
                            {
                                DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check];
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidTeachingParamError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                                teachingList.Add(teachingParam);

                            break;
                        }
                    }

                    rackMaster.Interlock_StartAutoTeachingControl(rackMaster.GetTeachingMode(), teachingList, RackMaster.InterlockFrom.UI_Event);
                }
                else if(rackMaster.GetTeachingMode() == RackMaster.TeachingMode.Continuous)
                {
                    List<RackMaster.TeachingParam> teachingList = new List<RackMaster.TeachingParam>();

                    for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
                    {
                        if((bool)DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check].Value)
                        {
                            RackMaster.TeachingParam teachingParam = new RackMaster.TeachingParam();
                            teachingParam.ShelfID = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                            teachingParam.X_Pos = Convert.ToInt16(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                            teachingParam.Z_Pos = Convert.ToInt16(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);
                            teachingParam.State = Convert.ToString(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.State].Value);
                            teachingParam.nRowIndex = nRowCount;

                            if (!teachingParam.IsValid())
                            {
                                DGV_TeachingList.CurrentCell = DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.Check];
                                MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidTeachingParamError"), SynusLangPack.GetLanguage("WarningMessage"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            else
                                teachingList.Add(teachingParam);
                        }
                    }


                    rackMaster.Interlock_StartAutoTeachingControl(rackMaster.GetTeachingMode(), teachingList, RackMaster.InterlockFrom.UI_Event);
                }
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"RackMaster[{rackMaster.GetParam().ID}] Auto Teching Run");
            }
        }

        private void btn_TeachingStop_Click(object sender, EventArgs e)
        {
            RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

            if (rackMaster == null)
                return;

            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Auto Teaching Stop Click");
            rackMaster.AutoTeachingStop();
        }

        private void btn_CreateAutoTeachingTable_Click(object sender, EventArgs e)
        {
            try
            {
                RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

                if (rackMaster == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Create Auto Teaching Table Click");

                if (string.IsNullOrEmpty(rackMaster.m_AutoTeachingFilePath))
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_NotLoadedFile"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.FilePathIsEmpty, $"RackMaster Teaching File");
                    return;
                }

                double dBaseShelfXPos = Convert.ToDouble(tbox_BaseShelfXPos.Text);
                double dBaseShelfZPos = Convert.ToDouble(tbox_BaseShelfZPos.Text);
                double dXInterval = Convert.ToDouble(tbox_XInterval.Text);
                double dZInterval = Convert.ToDouble(tbox_ZInterval.Text);
                int nRowCount = Convert.ToInt32(tbox_RowCount.Text);
                int nColumncount = Convert.ToInt32(tbox_ColCount.Text);
                int nBaseShelfID = Convert.ToInt32(tbox_BaseShelfID.Text);

                if(dBaseShelfXPos < 0)
                {
                    MessageBox.Show("Base Shelf X Position " + SynusLangPack.GetLanguage("Message_Invalid_Data_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dBaseShelfZPos < 0)
                {
                    MessageBox.Show("Base Shelf Z Position " + SynusLangPack.GetLanguage("Message_Invalid_Data_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dXInterval < 0)
                {
                    MessageBox.Show("X Interval " + SynusLangPack.GetLanguage("Message_Invalid_Data_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dZInterval < 0)
                {
                    MessageBox.Show("Z Interval " + SynusLangPack.GetLanguage("Message_Invalid_Data_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (nRowCount < 0)
                {
                    MessageBox.Show("Row Count " + SynusLangPack.GetLanguage("Message_Invalid_Data_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (nColumncount < 0)
                {
                    MessageBox.Show("Column Count " + SynusLangPack.GetLanguage("Message_Invalid_Data_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (nBaseShelfID >= 30000 || nBaseShelfID < 10101)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Invalid_ShelfID_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int colCnt = 0; colCnt < nColumncount; colCnt++)
                {
                    int StartShelfID = nBaseShelfID + (colCnt * 100);
                    double XPos = Math.Ceiling(dBaseShelfXPos + (dXInterval * colCnt));
                    for (int rowCnt = 0; rowCnt < nRowCount; rowCnt++)
                    {
                        int CalShelfID = StartShelfID + rowCnt;
                        double ZPos = Math.Ceiling(dBaseShelfZPos + (dZInterval * rowCnt));
                        DGV_TeachingList.Rows.Add(new object[] { true, CalShelfID, XPos, ZPos, string.Empty });
                    }
                }
            }
            catch(Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"RackMaster Control Form-Create Teaching Table Exception");
            }

        }

        private void btn_GetBaseShelfData_Click(object sender, EventArgs e)
        {
            try
            {
                RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;
                if (rackMaster == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Get Base Shelf Info Click");

                int nBaseShelfID = Convert.ToInt32(tbox_BaseShelfID.Text);
                
                if (nBaseShelfID >= 30000 || nBaseShelfID < 10101)
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Invalid_ShelfID_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (rackMaster.IsRunTeachingRWStopwatch())
                {
                    rackMaster.StopTeachingRWStopwatch();
                    return;
                }
                rackMaster.Interlock_GetAutoTeachingShelfInfo(nBaseShelfID, RackMaster.InterlockFrom.UI_Event);
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"RackMaster Control Form-Get BaseShelf Data Exception, Shelf ID : {tbox_BaseShelfID.Text}");
            }
        }

        private void btn_ApplyOffset_Click(object sender, EventArgs e)
        {
            try
            {
                RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;
                if (rackMaster == null)
                    return;

                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"RackMaster Control Form-RackMaster[{rackMaster.GetParam().ID}] Offset Apply Click");

                int StartShelfID = Convert.ToInt32(tbx_OffsetStartShelfID.Text);
                int EndShelfID = Convert.ToInt32(tbx_OffsetEndShelfID.Text);
                double Offset_X = Convert.ToDouble(tbx_OffsetX.Text);
                double Offset_Z = Convert.ToDouble(tbx_OffsetZ.Text);

                int ApplyCount = 0;
                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterTeachingOffsetApply, $"Offset Info X : {Offset_X}[mm], Offset Z : {Offset_Z}[mm]");

                for (int nRowCount = 0; nRowCount < DGV_TeachingList.Rows.Count; nRowCount++)
                {
                    try
                    {
                        int GetShelfID = Convert.ToInt32(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ShelfID].Value);
                        
                        if (GetShelfID >= StartShelfID && GetShelfID <= EndShelfID)
                        {
                            double X_Pos = Convert.ToDouble(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value);
                            double Z_Pos = Convert.ToDouble(DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value);

                            double Cal_X_Pos = Math.Ceiling(X_Pos + Offset_X);
                            double Cal_Z_Pos = Math.Ceiling(Z_Pos + Offset_Z);

                            DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.XPos].Value = Cal_X_Pos;
                            DGV_TeachingList.Rows[nRowCount].Cells[(int)DGV_TeachingListColumnIndex.ZPos].Value = Cal_Z_Pos;

                            LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterTeachingOffsetApply, $"Shelf[{GetShelfID}] X Pos : {X_Pos}[mm] -> {Cal_X_Pos}[mm], Z Pos : {Z_Pos}[mm] -> {Cal_Z_Pos}[mm]");
                            ApplyCount++;
                        }
                    }
                    catch
                    {

                    }
                }

                LogMsg.AddRackMasterLog(rackMaster.GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.RackMasterTeachingOffsetApply, $"Applyed Shelf Count : {ApplyCount}");
            }
            catch (Exception ex)
            {
                LogMsg.AddExceptionLog(ex, $"RackMaster Control Form-Offset Apply Exception");
            }
        }
    }
}
