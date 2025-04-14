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
using RackMaster.SEQ.COMMON;

namespace RackMaster.SUBFORM.SettingPage {
    public partial class SettingPage_PortShelf : Form {
        private enum DgvPortSettingsColumnIndex {
            PortType = 0,
            PortID,
            RowIndex,
            ColumnIndex,
            Direction,
            UseSensor,
            FromUpPosition,
            FromDownPosition,
            ToUpPosition,
            ToDownPosition,
            ForkPositionSensorType,
            UserForkPositionSensor,
        }

        private enum DgvShelfColumnIndex {
            RowCount = 0,
            ColumnCount,
        }

        private enum CboxEnalbed {
            Enabled,
            Disabled,
        }

        private enum CboxPortType {
            MGV_AGV_PORT = 0,
            OHT_MGV_PORT,
            AUTO_PORT,
            OVEN_PORT,
            SORTER_PORT
        }

        private enum CboxPortDirection {
            Left = PortDirection_HP.Left,
            Right = PortDirection_HP.Right,
        }

        private enum CboxPositionSensorType {
            PosSensor_1 = PositionSensorType.PositionSensor_1,
            PosSensor_2 = PositionSensorType.PositionSensor_2,
        }

        private UICtrl.DataGridViewCtrl m_dgvCtrl;
        private RackMasterMain.TeachingData m_teaching;
        private RackMasterMain.RackMasterParam m_param;
        private List<int> m_deletedId;

        private SEQ.CLS.Timer m_saveStopwatch;

        public SettingPage_PortShelf(RackMasterMain rackMaster) {
            m_teaching = rackMaster.m_teaching;
            m_param = rackMaster.m_param;
            m_deletedId = new List<int>();

            InitializeComponent();

            m_dgvCtrl = new UICtrl.DataGridViewCtrl();
            m_saveStopwatch = new SEQ.CLS.Timer();
            m_saveStopwatch.Stop();

            InitDataGridView_PortShelf();

            btnPortSave.MouseDown       += SaveMouseDown;
            btnPortSave.MouseUp         += SaveMouseUp;

            dgvShelf.CellValueChanged   += dgvShelf_CellValueChanged;
            dgvPort.CellValueChanged    += dgvPort_CellValueChanged;
        }

        private void InitDataGridView_PortShelf() {
            int dgvShelfRowNumber = dgvShelf.Rows.Add();

            if (!m_teaching.IsFileExist())
                return;

            dgvShelf.Rows[dgvShelfRowNumber].Cells[(int)DgvShelfColumnIndex.RowCount].Value = m_teaching.GetTeachingRow();
            dgvShelf.Rows[dgvShelfRowNumber].Cells[(int)DgvShelfColumnIndex.ColumnCount].Value = m_teaching.GetTeachingCol();
            foreach(RackMasterMain.RackMasterParam.PortParameter parameters in m_param.GetPortParameterList()) {
                int dgvPortRowNumber = dgvPort.Rows.Add();

                foreach (DgvPortSettingsColumnIndex col in Enum.GetValues(typeof(DgvPortSettingsColumnIndex))) {
                    switch (col) {
                        case DgvPortSettingsColumnIndex.PortType:
                            DataGridViewComboBoxCell cboxPortType = new DataGridViewComboBoxCell();
                            cboxPortType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxPortType portType in Enum.GetValues(typeof(CboxPortType))) {
                                cboxPortType.Items.Add($"{portType}");
                            }
                            if (parameters.type == PortType.MGV_AGV_PORT)
                                cboxPortType.Value = $"{CboxPortType.MGV_AGV_PORT}";
                            else if (parameters.type == PortType.OHT_MGV_PORT)
                                cboxPortType.Value = $"{CboxPortType.OHT_MGV_PORT}";
                            else if (parameters.type == PortType.AUTO_PORT)
                                cboxPortType.Value = $"{CboxPortType.AUTO_PORT}";
                            else if (parameters.type == PortType.OVEN_PORT)
                                cboxPortType.Value = $"{CboxPortType.OVEN_PORT}";
                            else if (parameters.type == PortType.SORTER_PORT)
                                cboxPortType.Value = $"{CboxPortType.SORTER_PORT}";

                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col] = cboxPortType;
                            break;

                        case DgvPortSettingsColumnIndex.PortID:
                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = parameters.id;
                            break;

                        case DgvPortSettingsColumnIndex.RowIndex:
                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = parameters.rowIndex = +1;
                            break;

                        case DgvPortSettingsColumnIndex.ColumnIndex:
                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = parameters.columnIndex + 1;
                            break;

                        case DgvPortSettingsColumnIndex.Direction:
                            DataGridViewComboBoxCell cboxDirection = new DataGridViewComboBoxCell();
                            cboxDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxPortDirection dir in Enum.GetValues(typeof(CboxPortDirection))) {
                                cboxDirection.Items.Add($"{dir}");
                            }
                            if (parameters.direction == PortDirection_HP.Left)
                                cboxDirection.Value = $"{CboxPortDirection.Left}";
                            else if (parameters.direction == PortDirection_HP.Right)
                                cboxDirection.Value = $"{CboxPortDirection.Right}";

                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col] = cboxDirection;
                            break;

                        case DgvPortSettingsColumnIndex.UseSensor:
                            DataGridViewComboBoxCell cboxUseSensor = new DataGridViewComboBoxCell();
                            cboxUseSensor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed enabled in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxUseSensor.Items.Add($"{enabled}");
                            }
                            if (parameters.useSensor)
                                cboxUseSensor.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxUseSensor.Value = $"{CboxEnalbed.Disabled}";

                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col] = cboxUseSensor;
                            break;

                        case DgvPortSettingsColumnIndex.FromUpPosition:
                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = parameters.fromUpPosition;
                            break;

                        case DgvPortSettingsColumnIndex.FromDownPosition:
                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = parameters.fromDownPosition;
                            break;

                        case DgvPortSettingsColumnIndex.ToUpPosition:
                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = parameters.toUpPosition;
                            break;

                        case DgvPortSettingsColumnIndex.ToDownPosition:
                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = parameters.toDownPosition;
                            break;

                        case DgvPortSettingsColumnIndex.ForkPositionSensorType:
                            DataGridViewComboBoxCell cboxPosSensor = new DataGridViewComboBoxCell();
                            cboxPosSensor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach(CboxPositionSensorType posSensor in Enum.GetValues(typeof(CboxPositionSensorType))) {
                                cboxPosSensor.Items.Add($"{posSensor}");
                            }
                            cboxPosSensor.Value = $"{(CboxPositionSensorType)parameters.forkPosSensorType}";

                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col] = cboxPosSensor;
                            break;

                        case DgvPortSettingsColumnIndex.UserForkPositionSensor:
                            DataGridViewComboBoxCell cboxUseForkPosSensor = new DataGridViewComboBoxCell();
                            cboxUseForkPosSensor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            foreach (CboxEnalbed enabled in Enum.GetValues(typeof(CboxEnalbed))) {
                                cboxUseForkPosSensor.Items.Add($"{enabled}");
                            }
                            if (parameters.useForkPosSensor)
                                cboxUseForkPosSensor.Value = $"{CboxEnalbed.Enabled}";
                            else
                                cboxUseForkPosSensor.Value = $"{CboxEnalbed.Disabled}";

                            dgvPort.Rows[dgvPortRowNumber].Cells[(int)col] = cboxUseForkPosSensor;
                            break;
                    }
                }
            }

            //for (int i = 0; i < m_teaching.GetTeachingDataArrayCount(); i++) {
            //    if (m_teaching.GetTeachingDataList()[i].portType != (int)PortType.SHELF) {
            //        int dgvPortRowNumber = dgvPort.Rows.Add();

            //        foreach (DgvPortSettingsColumnIndex col in Enum.GetValues(typeof(DgvPortSettingsColumnIndex))) {
            //            switch (col) {
            //                case DgvPortSettingsColumnIndex.PortType:
            //                    DataGridViewComboBoxCell cboxPortType = new DataGridViewComboBoxCell();
            //                    cboxPortType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            //                    foreach (CboxPortType portType in Enum.GetValues(typeof(CboxPortType))) {
            //                        cboxPortType.Items.Add($"{portType}");
            //                    }
            //                    if (m_teaching.GetTeachingDataList()[i].portType == (int)PortType.MGV_AGV_PORT)
            //                        cboxPortType.Value = $"{CboxPortType.MGV_AGV_PORT}";
            //                    else if (m_teaching.GetTeachingDataList()[i].portType == (int)PortType.OHT_MGV_PORT)
            //                        cboxPortType.Value = $"{CboxPortType.OHT_MGV_PORT}";
            //                    else if (m_teaching.GetTeachingDataList()[i].portType == (int)PortType.AUTO_PORT)
            //                        cboxPortType.Value = $"{CboxPortType.AUTO_PORT}";
            //                    else if (m_teaching.GetTeachingDataList()[i].portType == (int)PortType.OVEN_PORT)
            //                        cboxPortType.Value = $"{CboxPortType.OVEN_PORT}";
            //                    else if (m_teaching.GetTeachingDataList()[i].portType == (int)PortType.SORTER_PORT)
            //                        cboxPortType.Value = $"{CboxPortType.SORTER_PORT}";
            //                    dgvPort.Rows[dgvPortRowNumber].Cells[(int)col] = cboxPortType;
            //                    break;

            //                case DgvPortSettingsColumnIndex.PortID:
            //                    dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = m_teaching.GetTeachingDataList()[i].id;
            //                    break;

            //                case DgvPortSettingsColumnIndex.RowIndex:
            //                    dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = m_teaching.GetTeachingDataList()[i].row;
            //                    break;

            //                case DgvPortSettingsColumnIndex.ColumnIndex:
            //                    dgvPort.Rows[dgvPortRowNumber].Cells[(int)col].Value = m_teaching.GetTeachingDataList()[i].col;
            //                    break;

            //                case DgvPortSettingsColumnIndex.Direction:
            //                    DataGridViewComboBoxCell cboxDirection = new DataGridViewComboBoxCell();
            //                    cboxDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            //                    foreach (CboxPortDirection dir in Enum.GetValues(typeof(CboxPortDirection))) {
            //                        cboxDirection.Items.Add($"{dir}");
            //                    }
            //                    if (m_teaching.GetTeachingDataList()[i].direction == (int)PortDirection_HP.Left)
            //                        cboxDirection.Value = $"{CboxPortDirection.Left}";
            //                    else if (m_teaching.GetTeachingDataList()[i].direction == (int)PortDirection_HP.Right)
            //                        cboxDirection.Value = $"{CboxPortDirection.Right}";
            //                    dgvPort.Rows[dgvPortRowNumber].Cells[(int)col] = cboxDirection;
            //                    break;
            //            }
            //        }
            //    }
            //}

            m_dgvCtrl.DisableUserControl(ref dgvShelf);
            m_dgvCtrl.DisableUserControl(ref dgvPort);

            foreach(DataGridViewColumn col in dgvPort.Columns) {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void UpdateFormUI() {
            // Shelf에 대한 정보가 없으면 Port 세팅하는 부분 전부 Disable
            if (!CheckDataGridView_Shelf()) {
                dgvPort.Enabled = false;
                btnPortSave.Enabled = false;
                btnAddPort.Enabled = false;
                btnDeletePort.Enabled = false;
            }
            else {
                dgvPort.Enabled = true;
                btnPortSave.Enabled = true;
                btnAddPort.Enabled = true;
                btnDeletePort.Enabled = true;
            }
        }

        private bool CheckDataGridView_Port() {
            if (dgvPort.Rows.Count <= 0)
                return true;

            foreach (DgvPortSettingsColumnIndex col in Enum.GetValues(typeof(DgvPortSettingsColumnIndex))) {
                if (col == DgvPortSettingsColumnIndex.PortType || col == DgvPortSettingsColumnIndex.Direction ||
                    col == DgvPortSettingsColumnIndex.UseSensor || col == DgvPortSettingsColumnIndex.ForkPositionSensorType ||
                    col == DgvPortSettingsColumnIndex.UserForkPositionSensor)
                    continue;

                for (int row = 0; row < dgvPort.Rows.Count; row++) {
                    int tempData = 0;
                    if (!int.TryParse($"{dgvPort[(int)col, row].Value}", out tempData))
                        return false;
                }
            }
            return true;
        }

        private bool CheckDataGridView_Shelf() {
            int tempData = 0;
            int row = 0;

            foreach (DgvShelfColumnIndex col in Enum.GetValues(typeof(DgvShelfColumnIndex))) {
                if (!int.TryParse($"{dgvShelf[(int)col, row].Value}", out tempData))
                    return false;
            }
            return true;
        }

        private void SaveMouseDown(object sender, MouseEventArgs e) {
            if (!CheckDataGridView_Port() || !CheckDataGridView_Shelf()) {
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.DataGridViewInvalidValue}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                return;
            }

            saveTimer.Start();
        }

        private void SaveMouseUp(object sender, MouseEventArgs e) {
            saveTimer.Stop();
            m_saveStopwatch.Stop();
        }

        private void btnAddPort_Click(object sender, EventArgs e) {
            if (DialogResult.No == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureAddPort}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo))
                return;

            try {
                DataGridViewComboBoxCell cboxPortType = new DataGridViewComboBoxCell();
                cboxPortType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                foreach (CboxPortType type in Enum.GetValues(typeof(CboxPortType))) {
                    cboxPortType.Items.Add($"{type}");
                }

                DataGridViewComboBoxCell cboxDirection = new DataGridViewComboBoxCell();
                cboxDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                foreach (CboxPortDirection dir in Enum.GetValues(typeof(CboxPortDirection))) {
                    cboxDirection.Items.Add($"{dir}");
                }

                DataGridViewComboBoxCell cboxUseSensor = new DataGridViewComboBoxCell();
                cboxUseSensor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                foreach (CboxEnalbed useSensor in Enum.GetValues(typeof(CboxEnalbed))) {
                    cboxUseSensor.Items.Add($"{useSensor}");
                }

                DataGridViewComboBoxCell cboxPosSensor = new DataGridViewComboBoxCell();
                cboxPosSensor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                foreach (CboxPositionSensorType posSensor in Enum.GetValues(typeof(CboxPositionSensorType))) {
                    cboxPosSensor.Items.Add($"{posSensor}");
                }

                DataGridViewComboBoxCell cboxUseForkPosSensor = new DataGridViewComboBoxCell();
                cboxUseForkPosSensor.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                foreach (CboxEnalbed usePosSensor in Enum.GetValues(typeof(CboxEnalbed))) {
                    cboxUseForkPosSensor.Items.Add($"{usePosSensor}");
                }

                int numberRow = dgvPort.Rows.Add();
                dgvPort.Rows[numberRow].Cells[(int)DgvPortSettingsColumnIndex.PortType] = cboxPortType;
                dgvPort.Rows[numberRow].Cells[(int)DgvPortSettingsColumnIndex.Direction] = cboxDirection;
                dgvPort.Rows[numberRow].Cells[(int)DgvPortSettingsColumnIndex.UseSensor] = cboxUseSensor;
                dgvPort.Rows[numberRow].Cells[(int)DgvPortSettingsColumnIndex.ForkPositionSensorType] = cboxPosSensor;
                dgvPort.Rows[numberRow].Cells[(int)DgvPortSettingsColumnIndex.UserForkPositionSensor] = cboxUseForkPosSensor;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeletePort_Click(object sender, EventArgs e) {
            if (DialogResult.No == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureDeletePort}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo))
                return;

            try {
                int rowIndex = dgvPort.SelectedCells[0].RowIndex;
                int id = Convert.ToInt32(dgvPort.Rows[rowIndex].Cells[(int)DgvPortSettingsColumnIndex.PortID].Value);

                dgvPort.Rows.RemoveAt(rowIndex);
                m_deletedId.Add(id);
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, Log.LogMessage_Main.UI_DataGridViewExceptionErrorOccurred, ex));
            }
        }

        private void dgvShelf_CellValueChanged(object sender, DataGridViewCellEventArgs e) {

        }

        private void dgvPort_CellValueChanged(object sender, DataGridViewCellEventArgs e) {

        }

        private void AlertMessageBox() {
            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ValidValueCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
            return;
        }

        private void SavePort() {
            int shelfRowCount = 0;
            int shelfColumnCount = 0;

            if (!int.TryParse($"{dgvShelf[(int)DgvShelfColumnIndex.RowCount, 0].Value}", out shelfRowCount)) {
                AlertMessageBox();
                return;
            }

            if (!int.TryParse($"{dgvShelf[(int)DgvShelfColumnIndex.ColumnCount, 0].Value}", out shelfColumnCount)) {
                AlertMessageBox();
                return;
            }

            if (!m_teaching.IsFileExist()) {
                m_teaching.CreatePortData(shelfRowCount, shelfColumnCount);
            }

            //if(shelfRowCount != m_teaching.GetTeachingRow() || shelfColumnCount != m_teaching.GetTeachingCol()) {
            //    MessageBox.Show("Row값이나 Column값이 변경되었습니다. 그래도 진행하시겠습니까?");
            //}

            int rowCount = dgvPort.Rows.Count;
            if (rowCount == 0)
                return;

            foreach(int deleteId in m_deletedId) {
                m_param.DeletePortParameter(deleteId);
            }

            m_deletedId.Clear();

            try {
                for (int i = 0; i < rowCount; i++) {
                    RackMasterMain.RackMasterParam.PortParameter addParam = new RackMasterMain.RackMasterParam.PortParameter();

                    foreach (DgvPortSettingsColumnIndex col in Enum.GetValues(typeof(DgvPortSettingsColumnIndex))) {
                        switch (col) {
                            case DgvPortSettingsColumnIndex.PortType:
                                DataGridViewComboBoxCell cboxPortType = (DataGridViewComboBoxCell)dgvPort.Rows[i].Cells[(int)col];
                                switch ((CboxPortType)cboxPortType.Items.IndexOf(cboxPortType.Value)) {
                                    case CboxPortType.MGV_AGV_PORT:
                                        addParam.type = PortType.MGV_AGV_PORT; break;

                                    case CboxPortType.OHT_MGV_PORT:
                                        addParam.type = PortType.OHT_MGV_PORT; break;

                                    case CboxPortType.OVEN_PORT:
                                        addParam.type = PortType.OVEN_PORT; break;

                                    case CboxPortType.SORTER_PORT:
                                        addParam.type = PortType.SORTER_PORT; break;

                                    case CboxPortType.AUTO_PORT:
                                        addParam.type = PortType.AUTO_PORT; break;
                                }
                                break;

                            case DgvPortSettingsColumnIndex.PortID:
                                addParam.id = Convert.ToInt32(dgvPort.Rows[i].Cells[(int)col].Value);
                                break;

                            case DgvPortSettingsColumnIndex.RowIndex:
                                addParam.rowIndex = (Convert.ToInt32(dgvPort.Rows[i].Cells[(int)col].Value)) - 1;
                                break;

                            case DgvPortSettingsColumnIndex.ColumnIndex:
                                addParam.columnIndex = (Convert.ToInt32(dgvPort.Rows[i].Cells[(int)col].Value)) - 1;
                                break;

                            case DgvPortSettingsColumnIndex.Direction:
                                DataGridViewComboBoxCell cboxDirection = (DataGridViewComboBoxCell)dgvPort.Rows[i].Cells[(int)col];
                                if ((CboxPortDirection)cboxDirection.Items.IndexOf(cboxDirection.Value) == CboxPortDirection.Left)
                                    addParam.direction = PortDirection_HP.Left;
                                else if ((CboxPortDirection)cboxDirection.Items.IndexOf(cboxDirection.Value) == CboxPortDirection.Right)
                                    addParam.direction = PortDirection_HP.Right;
                                break;

                            case DgvPortSettingsColumnIndex.UseSensor:
                                DataGridViewComboBoxCell cboxUseSensor = (DataGridViewComboBoxCell)dgvPort.Rows[i].Cells[(int)col];
                                if ((CboxEnalbed)cboxUseSensor.Items.IndexOf(cboxUseSensor.Value) == CboxEnalbed.Enabled)
                                    addParam.useSensor = true;
                                else if ((CboxEnalbed)cboxUseSensor.Items.IndexOf(cboxUseSensor.Value) == CboxEnalbed.Disabled)
                                    addParam.useSensor = false;
                                break;

                            case DgvPortSettingsColumnIndex.FromUpPosition:
                                addParam.fromUpPosition = Convert.ToInt32(dgvPort.Rows[i].Cells[(int)col].Value);
                                break;

                            case DgvPortSettingsColumnIndex.FromDownPosition:
                                addParam.fromDownPosition = Convert.ToInt32(dgvPort.Rows[i].Cells[(int)col].Value);
                                break;

                            case DgvPortSettingsColumnIndex.ToUpPosition:
                                addParam.toUpPosition = Convert.ToInt32(dgvPort.Rows[i].Cells[(int)col].Value);
                                break;

                            case DgvPortSettingsColumnIndex.ToDownPosition:
                                addParam.toDownPosition = Convert.ToInt32(dgvPort.Rows[i].Cells[(int)col].Value);
                                break;

                            case DgvPortSettingsColumnIndex.ForkPositionSensorType:
                                DataGridViewComboBoxCell cboxPosSensor = (DataGridViewComboBoxCell)dgvPort.Rows[i].Cells[(int)col];
                                addParam.forkPosSensorType = (PositionSensorType)cboxPosSensor.Items.IndexOf(cboxPosSensor.Value);
                                break;

                            case DgvPortSettingsColumnIndex.UserForkPositionSensor:
                                DataGridViewComboBoxCell cboxUseForkSensor = (DataGridViewComboBoxCell)dgvPort.Rows[i].Cells[(int)col];
                                if ((CboxEnalbed)cboxUseForkSensor.Items.IndexOf(cboxUseForkSensor.Value) == CboxEnalbed.Enabled)
                                    addParam.useForkPosSensor = true;
                                else if ((CboxEnalbed)cboxUseForkSensor.Items.IndexOf(cboxUseForkSensor.Value) == CboxEnalbed.Disabled)
                                    addParam.useForkPosSensor = false;
                                break;
                        }
                    }

                    m_param.SetPortParameter(addParam);

                    //DataGridViewComboBoxCell cboxPortType = (DataGridViewComboBoxCell)dgvPort.Rows[i].Cells[(int)DgvPortSettingsColumnIndex.PortType];
                    //int selectedPortType = cboxPortType.Items.IndexOf(cboxPortType.Value);
                    //int portType = 0;

                    //switch ((CboxPortType)selectedPortType) {
                    //    case CboxPortType.MGV_AGV_PORT:
                    //        portType = (int)PortType.MGV_AGV_PORT; break;

                    //    case CboxPortType.OHT_MGV_PORT:
                    //        portType = (int)PortType.OHT_MGV_PORT; break;

                    //    case CboxPortType.OVEN_PORT:
                    //        portType = (int)PortType.OVEN_PORT; break;

                    //    case CboxPortType.SORTER_PORT:
                    //        portType = (int)PortType.SORTER_PORT; break;

                    //    case CboxPortType.AUTO_PORT:
                    //        portType = (int)PortType.AUTO_PORT; break;
                    //}

                    //int portId = 0;
                    //int rowIndex = 0;
                    //int columnIndex = 0;
                    //DataGridViewComboBoxCell cboxDirection = (DataGridViewComboBoxCell)dgvPort.Rows[i].Cells[(int)DgvPortSettingsColumnIndex.Direction];
                    //int direction = cboxDirection.Items.IndexOf(cboxDirection.Value);

                    //if (!int.TryParse(dgvPort[(int)DgvPortSettingsColumnIndex.PortID, i].Value.ToString(), out portId)) {
                    //    AlertMessageBox();
                    //    return;
                    //}

                    //if (!int.TryParse(dgvPort[(int)DgvPortSettingsColumnIndex.RowIndex, i].Value.ToString(), out rowIndex)) {
                    //    AlertMessageBox();
                    //    return;
                    //}

                    //if (!int.TryParse(dgvPort[(int)DgvPortSettingsColumnIndex.ColumnIndex, i].Value.ToString(), out columnIndex)) {
                    //    AlertMessageBox();
                    //    return;
                    //}

                    //if (m_teaching.IsFileExist()) {
                    //    m_teaching.AddPortData(rowIndex, columnIndex, direction, portType, portId);
                    //    m_teaching.SaveDataFile();
                    //}
                }

                m_param.SavePortParameter();
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, $"Port Parameter Save Fail", ex));
                MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveFailed}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
                return;
            }

            Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.UIControl, $"Port Parameter Save Success"));
            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.ParameterSaveSuccess}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
            return;
        }

        private void saveTimer_Tick(object sender, EventArgs e) {
            if (!m_saveStopwatch.IsTimerStarted())
                m_saveStopwatch.Restart();

            if (m_saveStopwatch.Delay(UICtrl.m_saveDelayTime)) {
                saveTimer.Stop();
                m_saveStopwatch.Stop();
                if (DialogResult.No == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureParameterSave}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                    return;
                }

                SavePort();
            }
        }
    }
}
