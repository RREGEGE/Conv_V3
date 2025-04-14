using Master.Equipment.Port;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Master.SubForm
{
    public partial class Frm_EquipMotionSettings : Form
    {
        enum MotionParamRowIndex
        {
            BufferType,
            PortDirection,
            Shuttle_X_Type,
            Shuttle_Z_Type,
            Shuttle_T_Type,

            Buffer_LP_X_Type,
            Buffer_LP_Y_Type,
            Buffer_LP_Z_Type,
            Buffer_LP_T_Type,
            Buffer_OP_X_Type,
            Buffer_OP_Y_Type,
            Buffer_OP_Z_Type,
            Buffer_OP_T_Type,
            Buffer_LP_CV_Enable,
            Buffer_OP_CV_Enable,
            Buffer_BP1_CV_Enable,
            Buffer_BP2_CV_Enable,
            Buffer_BP3_CV_Enable,
            Buffer_BP4_CV_Enable
        }
        public Frm_EquipMotionSettings()
        {
            InitializeComponent();
            ControlItemInit();

            this.Disposed += (object sender, EventArgs e) =>
            {
                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }

        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);

            LoadDataGridView();

            LaguageCheck();

            ButtonFunc.SetText(btn_Save, SynusLangPack.GetLanguage("Btn_Save"));
            ButtonFunc.SetText(btn_Close, SynusLangPack.GetLanguage("Btn_Cancel"));
        }
        private void LoadDataGridView()
        {
            InitDataGridView();
            InitMotionParam();
        }

        private void InitDataGridView()
        {
            DGV_PortMotionParam.Rows.Clear();
            DGV_PortMotionParam.Columns.Clear();

            DGV_PortMotionParam.TopLeftHeaderCell.Value = "Motion Param";
            DGV_PortMotionParam.EnableHeadersVisualStyles = false;
            DGV_PortMotionParam.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
            DGV_PortMotionParam.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            DataGridViewCellStyle DGV_ColumnHeaderCellStyle = new DataGridViewCellStyle();
            DGV_ColumnHeaderCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            DGV_ColumnHeaderCellStyle.WrapMode = DataGridViewTriState.True;
            DGV_ColumnHeaderCellStyle.BackColor = System.Drawing.Color.DarkGray;
            DGV_ColumnHeaderCellStyle.ForeColor = System.Drawing.Color.Black;

            DGV_PortMotionParam.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            DGV_PortMotionParam.ColumnHeadersHeight = 50;
            DGV_PortMotionParam.ColumnHeadersDefaultCellStyle = DGV_ColumnHeaderCellStyle;

            DataGridViewCellStyle DGV_RowHeaderCellStyle = new DataGridViewCellStyle();
            DGV_RowHeaderCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            DGV_RowHeaderCellStyle.WrapMode = DataGridViewTriState.True;
            DGV_RowHeaderCellStyle.BackColor = System.Drawing.Color.LightGray;
            DGV_RowHeaderCellStyle.ForeColor = System.Drawing.Color.Black;

            DGV_PortMotionParam.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            DGV_PortMotionParam.RowHeadersDefaultCellStyle = DGV_RowHeaderCellStyle;
            DGV_PortMotionParam.RowHeadersWidth = 300;
            DGV_PortMotionParam.RowTemplate.Height = 25;

            foreach (var port in Master.m_Ports)
            {
                DataGridViewCellStyle dataGridViewCellStyle = new DataGridViewCellStyle();
                dataGridViewCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridViewCellStyle.BackColor = System.Drawing.Color.White;
                dataGridViewCellStyle.ForeColor = System.Drawing.Color.Black;
                dataGridViewCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle.SelectionForeColor = System.Drawing.Color.Black;

                DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
                dataGridViewTextBoxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                dataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle;
                dataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
                dataGridViewTextBoxColumn.MinimumWidth = 150;
                dataGridViewTextBoxColumn.Name = $"Port [{port.Value.GetParam().ID}]\n{port.Value.GetParam().ePortType}";

                DGV_PortMotionParam.Columns.Add(dataGridViewTextBoxColumn);
            }

            if (DGV_PortMotionParam.Columns.Count <= 0)
                return;

            for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(MotionParamRowIndex)).Length; nRowCount++)
            {
                MotionParamRowIndex eMotionParamRowIndex = (MotionParamRowIndex)nRowCount;
                DGV_PortMotionParam.Rows.Add();

                switch (eMotionParamRowIndex)
                {
                    case MotionParamRowIndex.BufferType:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer Type";
                        break;
                    case MotionParamRowIndex.PortDirection:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Port Direction";
                        break;
                    case MotionParamRowIndex.Shuttle_X_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Shuttle X Axis Type";
                        break;
                    case MotionParamRowIndex.Shuttle_Z_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Shuttle Z Axis Type";
                        break;
                    case MotionParamRowIndex.Shuttle_T_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Shuttle T Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_LP_X_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer LP X Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_LP_Y_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer LP Y Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_LP_Z_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer LP Z Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_LP_T_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer LP T Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_OP_X_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer OP X Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_OP_Y_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer OP Y Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_OP_Z_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer OP Z Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_OP_T_Type:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer OP T Axis Type";
                        break;
                    case MotionParamRowIndex.Buffer_LP_CV_Enable:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer LP Conveyor Enable";
                        break;
                    case MotionParamRowIndex.Buffer_OP_CV_Enable:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer OP Conveyor Enable";
                        break;
                    case MotionParamRowIndex.Buffer_BP1_CV_Enable:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer BP1 Conveyor Enable";
                        break;
                    case MotionParamRowIndex.Buffer_BP2_CV_Enable:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer BP2 Conveyor Enable";
                        break;
                    case MotionParamRowIndex.Buffer_BP3_CV_Enable:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer BP3 Conveyor Enable";
                        break;
                    case MotionParamRowIndex.Buffer_BP4_CV_Enable:
                        DGV_PortMotionParam.Rows[nRowCount].HeaderCell.Value = "Buffer BP4 Conveyor Enable";
                        break;
                }
            }

            foreach (var port in Master.m_Ports.Select((value, index) => (value, index)))
            {
                if (port.index >= DGV_PortMotionParam.Columns.Count)
                    continue;

                int nColumnCount = port.index;

                try
                {
                    ///Direction Cell
                    if (!port.value.Value.IsEQPort())
                    {
                        DataGridViewComboBoxCell cbxCell_PortDirection = new DataGridViewComboBoxCell();
                        cbxCell_PortDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        for (int nCount = 0; nCount < Enum.GetNames(typeof(Equipment.Port.Port.PortDirection)).Length; nCount++)
                            cbxCell_PortDirection.Items.Add(((Equipment.Port.Port.PortDirection)nCount).ToString());

                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.PortDirection].Cells[nColumnCount] = cbxCell_PortDirection;
                    }
                    else
                    {
                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.PortDirection].Cells[nColumnCount].Value = string.Empty;
                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.PortDirection].Cells[nColumnCount].Style.BackColor = Color.DarkGray;
                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.PortDirection].Cells[nColumnCount].ReadOnly = true;
                    }


                    ///Buffer Type Cell - 1BP, 2BP
                    if (port.value.Value.IsShuttleControlPort())
                    {
                        DataGridViewComboBoxCell cbxCell_PortBufferType = new DataGridViewComboBoxCell();
                        cbxCell_PortBufferType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                        for (int nCount = 0; nCount < Enum.GetNames(typeof(Equipment.Port.Port.ShuttleCtrlBufferType)).Length; nCount++)
                            cbxCell_PortBufferType.Items.Add(((Equipment.Port.Port.ShuttleCtrlBufferType)nCount).ToString());

                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.BufferType].Cells[nColumnCount] = cbxCell_PortBufferType;
                    }
                    else
                    {
                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.BufferType].Cells[nColumnCount].Value = string.Empty;
                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.BufferType].Cells[nColumnCount].Style.BackColor = Color.DarkGray;
                        DGV_PortMotionParam.Rows[(int)MotionParamRowIndex.BufferType].Cells[nColumnCount].ReadOnly = true;
                    }


                    ///Shuttle Axis Cell
                    for (int nCount = (int)MotionParamRowIndex.Shuttle_X_Type; nCount <= (int)MotionParamRowIndex.Shuttle_T_Type; nCount++)
                    {
                        if (port.value.Value.IsShuttleControlPort())
                        {
                            DataGridViewComboBoxCell cbxCell_AxisCtrlType = new DataGridViewComboBoxCell();
                            cbxCell_AxisCtrlType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nTypeCount = 0; nTypeCount < Enum.GetNames(typeof(Port.AxisCtrlType)).Length; nTypeCount++)
                                cbxCell_AxisCtrlType.Items.Add(((Port.AxisCtrlType)nTypeCount).ToString());

                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount] = cbxCell_AxisCtrlType;
                        }
                        else
                        {
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].Value = string.Empty;
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].Style.BackColor = Color.DarkGray;
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].ReadOnly = true;
                        }
                    }


                    ///Buffer Axis Cell
                    for (int nCount = (int)MotionParamRowIndex.Buffer_LP_X_Type; nCount <= (int)MotionParamRowIndex.Buffer_OP_T_Type; nCount++)
                    {
                        if (port.value.Value.IsBufferControlPort())
                        {
                            DataGridViewComboBoxCell cbxCell_AxisCtrlType = new DataGridViewComboBoxCell();
                            cbxCell_AxisCtrlType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nTypeCount = 0; nTypeCount < Enum.GetNames(typeof(Port.AxisCtrlType)).Length; nTypeCount++)
                                cbxCell_AxisCtrlType.Items.Add(((Port.AxisCtrlType)nTypeCount).ToString());

                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount] = cbxCell_AxisCtrlType;
                        }
                        else
                        {
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].Value = string.Empty;
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].Style.BackColor = Color.DarkGray;
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].ReadOnly = true;
                        }
                    }


                    ///Buffer CV Cell
                    for (int nCount = (int)MotionParamRowIndex.Buffer_LP_CV_Enable; nCount <= (int)MotionParamRowIndex.Buffer_BP4_CV_Enable; nCount++)
                    {
                        MotionParamRowIndex eMotionParamRowIndex = (MotionParamRowIndex)nCount;

                        if (port.value.Value.IsBufferControlPort())
                        {
                            DataGridViewComboBoxCell cbxCell_CVCtrlType = new DataGridViewComboBoxCell();
                            cbxCell_CVCtrlType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            for (int nTypeCount = 0; nTypeCount < Enum.GetNames(typeof(Port.CVCtrlEnable)).Length; nTypeCount++)
                                cbxCell_CVCtrlType.Items.Add(((Port.CVCtrlEnable)nTypeCount).ToString());

                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount] = cbxCell_CVCtrlType;
                        }
                        else
                        {
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].Value = string.Empty;
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].Style.BackColor = Color.DarkGray;
                            DGV_PortMotionParam.Rows[nCount].Cells[nColumnCount].ReadOnly = true;
                        }
                    }

                }
                catch
                {

                }
            }
        }

        private void InitMotionParam()
        {
            foreach (var port in Master.m_Ports.Select((value, index) => (value, index)))
            {
                int Index = port.index;
                var Port = port.value.Value;

                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(MotionParamRowIndex)).Length; nRowCount++)
                {
                    MotionParamRowIndex eMotionParamRowIndex = (MotionParamRowIndex)nRowCount;
                    var DGV_Cell = DGV_PortMotionParam.Rows[nRowCount].Cells[Index];

                    switch (eMotionParamRowIndex)
                    {
                        case MotionParamRowIndex.BufferType:
                            if (Port.IsShuttleControlPort())
                                DGV_Cell.Value = $"{Port.GetMotionParam().eBufferType}";
                            else
                                DGV_Cell.Value = string.Empty;
                            break;
                        case MotionParamRowIndex.PortDirection:
                            if (!Port.IsEQPort())
                                DGV_Cell.Value = $"{Port.GetOperationDirection()}";
                            else
                                DGV_Cell.Value = string.Empty;
                            break;
                        case MotionParamRowIndex.Shuttle_X_Type:
                        case MotionParamRowIndex.Shuttle_Z_Type:
                        case MotionParamRowIndex.Shuttle_T_Type:
                            if (Port.IsShuttleControlPort())
                            {
                                if(eMotionParamRowIndex == MotionParamRowIndex.Shuttle_X_Type)
                                    DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Shuttle_X)}";
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_Z_Type)
                                    DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Shuttle_Z)}";
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_T_Type)
                                    DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Shuttle_T)}";
                            }
                            else
                                DGV_Cell.Value = string.Empty;
                            break;

                        case MotionParamRowIndex.Buffer_LP_X_Type:
                        case MotionParamRowIndex.Buffer_LP_Y_Type:
                        case MotionParamRowIndex.Buffer_LP_Z_Type:
                        case MotionParamRowIndex.Buffer_LP_T_Type:
                        case MotionParamRowIndex.Buffer_OP_X_Type:
                        case MotionParamRowIndex.Buffer_OP_Y_Type:
                        case MotionParamRowIndex.Buffer_OP_Z_Type:
                        case MotionParamRowIndex.Buffer_OP_T_Type:
                            {
                                if (Port.IsBufferControlPort())
                                {
                                    if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_X_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_LP_X)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_Y_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_LP_Y)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_Z_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_LP_Z)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_T_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_LP_T)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_X_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_OP_X)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_Y_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_OP_Y)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_Z_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_OP_Z)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_T_Type)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetAxisControlType(Port.PortAxis.Buffer_OP_T)}";
                                }
                                else
                                    DGV_Cell.Value = string.Empty;
                            }
                            break;
                        
                        case MotionParamRowIndex.Buffer_LP_CV_Enable:
                        case MotionParamRowIndex.Buffer_OP_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP1_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP2_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP3_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP4_CV_Enable:
                            {
                                if (Port.IsBufferControlPort())
                                {
                                    if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_CV_Enable)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetBufferControlEnable(Port.BufferCV.Buffer_LP)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_CV_Enable)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetBufferControlEnable(Port.BufferCV.Buffer_OP)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP1_CV_Enable)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetBufferControlEnable(Port.BufferCV.Buffer_BP1)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP2_CV_Enable)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetBufferControlEnable(Port.BufferCV.Buffer_BP2)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP3_CV_Enable)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetBufferControlEnable(Port.BufferCV.Buffer_BP3)}";
                                    else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP4_CV_Enable)
                                        DGV_Cell.Value = $"{Port.GetMotionParam().GetBufferControlEnable(Port.BufferCV.Buffer_BP4)}";
                                }
                                else
                                    DGV_Cell.Value = string.Empty;
                            }
                            break;
                    }
                }
            }

            DGV_PortMotionParam.CurrentCell = null;
        }

        private void LaguageCheck()
        {
            FormFunc.SetText(this, SynusLangPack.GetLanguage("Frm_EquipMotionSettings_FormTitle"));
            ButtonFunc.SetText(btn_Save, SynusLangPack.GetLanguage("Btn_Apply"));
            ButtonFunc.SetText(btn_Close, SynusLangPack.GetLanguage("Btn_Cancel"));
        }

        private void PortMotionParamApply()
        {
            foreach (var port in Master.m_Ports.Select((value, index) => (value, index)))
            {
                int Index = port.index;
                var Port = port.value.Value;

                for (int nRowCount = 0; nRowCount < Enum.GetNames(typeof(MotionParamRowIndex)).Length; nRowCount++)
                {
                    MotionParamRowIndex eMotionParamRowIndex = (MotionParamRowIndex)nRowCount;
                    var DGV_Cell = DGV_PortMotionParam.Rows[nRowCount].Cells[Index];
                    string Value = Convert.ToString(DGV_Cell.Value ?? string.Empty);

                    switch (eMotionParamRowIndex)
                    {
                        case MotionParamRowIndex.BufferType:
                            {
                                if (Port.IsShuttleControlPort())
                                {
                                    Port.GetMotionParam().eBufferType = (Port.ShuttleCtrlBufferType)Enum.Parse(typeof(Port.ShuttleCtrlBufferType), Convert.ToString(DGV_Cell?.Value ?? Port.ShuttleCtrlBufferType.Two_Buffer.ToString()));
                                }
                                else
                                    Port.GetMotionParam().eBufferType = Port.ShuttleCtrlBufferType.Two_Buffer;
                            }
                            break;
                        case MotionParamRowIndex.PortDirection:
                            {
                                Port.GetMotionParam().ePortDirection = (Port.PortDirection)Enum.Parse(typeof(Port.PortDirection), Convert.ToString(DGV_Cell.Value ?? Port.PortDirection.Input.ToString()));
                            }
                            break;
                        case MotionParamRowIndex.Shuttle_X_Type:
                        case MotionParamRowIndex.Shuttle_Z_Type:
                        case MotionParamRowIndex.Shuttle_T_Type:
                            if (Port.IsShuttleControlPort())
                            {
                                var val = (Port.AxisCtrlType)Enum.Parse(typeof(Port.AxisCtrlType), Convert.ToString(DGV_Cell?.Value ?? Port.AxisCtrlType.None.ToString()));
                                
                                if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_X_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Shuttle_X, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_Z_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Shuttle_Z, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_T_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Shuttle_T, val);
                            }
                            else
                            {
                                if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_X_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Shuttle_X, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_Z_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Shuttle_Z, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Shuttle_T_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Shuttle_T, Port.AxisCtrlType.None);
                            }
                            break;
                        case MotionParamRowIndex.Buffer_LP_X_Type:
                        case MotionParamRowIndex.Buffer_LP_Y_Type:
                        case MotionParamRowIndex.Buffer_LP_Z_Type:
                        case MotionParamRowIndex.Buffer_LP_T_Type:
                        case MotionParamRowIndex.Buffer_OP_X_Type:
                        case MotionParamRowIndex.Buffer_OP_Y_Type:
                        case MotionParamRowIndex.Buffer_OP_Z_Type:
                        case MotionParamRowIndex.Buffer_OP_T_Type:
                            if (Port.IsBufferControlPort())
                            {
                                var val = (Port.AxisCtrlType)Enum.Parse(typeof(Port.AxisCtrlType), Convert.ToString(DGV_Cell?.Value ?? Port.AxisCtrlType.None.ToString()));

                                if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_X_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_X, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_Y_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_Y, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_Z_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_Z, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_T_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_T, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_X_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_X, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_Y_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_Y, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_Z_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_Z, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_T_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_T, val);
                            }
                            else
                            {
                                if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_X_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_X, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_Y_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_Y, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_Z_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_Z, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_T_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_LP_T, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_X_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_X, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_Y_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_Y, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_Z_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_Z, Port.AxisCtrlType.None);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_T_Type)
                                    Port.GetMotionParam().SetAxisControlType(Port.PortAxis.Buffer_OP_T, Port.AxisCtrlType.None);
                            }
                            break;
                        case MotionParamRowIndex.Buffer_LP_CV_Enable:
                        case MotionParamRowIndex.Buffer_OP_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP1_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP2_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP3_CV_Enable:
                        case MotionParamRowIndex.Buffer_BP4_CV_Enable:
                            if (Port.IsBufferControlPort())
                            {
                                var val = (Port.CVCtrlEnable)Enum.Parse(typeof(Port.CVCtrlEnable), Convert.ToString(DGV_Cell?.Value ?? Port.CVCtrlEnable.Disable.ToString()));

                                if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_LP, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_OP, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP1_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP1, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP2_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP2, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP3_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP3, val);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP4_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP4, val);
                            }
                            else
                            {
                                if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_LP_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_LP, Port.CVCtrlEnable.Disable);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_OP_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_OP, Port.CVCtrlEnable.Disable);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP1_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP1, Port.CVCtrlEnable.Disable);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP2_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP2, Port.CVCtrlEnable.Disable);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP3_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP3, Port.CVCtrlEnable.Disable);
                                else if (eMotionParamRowIndex == MotionParamRowIndex.Buffer_BP4_CV_Enable)
                                    Port.GetMotionParam().SetBufferControlEnable(Port.BufferCV.Buffer_BP4, Port.CVCtrlEnable.Disable);
                            }
                            break;
                    }
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            GlobalForm.Frm_AcceptSave frm_AcceptSave = new GlobalForm.Frm_AcceptSave(GlobalForm.Frm_AcceptSave.SaveSection.PortMotionAxisInfo, string.Empty);
            frm_AcceptSave.Location = this.Location;
            frm_AcceptSave.StartPosition = FormStartPosition.CenterScreen;
            DialogResult result = frm_AcceptSave.ShowDialog();

            if (result != DialogResult.OK || this.IsDisposed)
                return;

            PortMotionParamApply();

            this.DialogResult = DialogResult.Yes;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
