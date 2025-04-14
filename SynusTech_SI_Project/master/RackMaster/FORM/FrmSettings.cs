using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ;

namespace RackMaster.FORM {
    public partial class FrmSettings : Form {
        private Motor m_motor;

        private Nullable<int> m_portId = 0;
        private Nullable<float> m_xPos = 0;
        private Nullable<float> m_zPos = 0;
        private Nullable<float> m_aPos = 0;
        private Nullable<float> m_tPos = 0;
        private Nullable<float> m_zUpPos = 0;
        private Nullable<float> m_zdownPos = 0;

        private Nullable<int> m_curPortIndex = 0;

        private RackMaster.SEQ.CLS.Timer m_blinkTimer;

        private DataGridViewComboBoxCell m_cboxForkType;
        private DataGridViewComboBoxCell m_cboxPortType;
        private DataGridViewComboBoxCell[] m_cboxProfileType;

        public FrmSettings() {
            m_motor = Motor.Instance;
            m_blinkTimer = new SEQ.CLS.Timer();

            InitializeComponent();

            m_blinkTimer.Restart();

            ButtonEnabledTeachingPanel(false);

            InitWMXPanel();
            InitRackMasterPanel();
        }

        /// <summary>
        /// WMX Panel 초기화
        /// </summary>
        public void InitWMXPanel() {

        }

        /// <summary>
        /// RackMaster Panel 초기화
        /// </summary>
        public void InitRackMasterPanel() {
            m_cboxForkType = new DataGridViewComboBoxCell();
            m_cboxForkType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            m_cboxForkType.Items.Add(Global.ForkType.Slide.ToString());
            m_cboxForkType.Items.Add(Global.ForkType.SCARA.ToString());

            int numberRow = dgvRMSettings.Rows.Add(null, "", "");
            dgvRMSettings.Rows[numberRow].Cells[0] = m_cboxForkType;
            dgvRMSettings.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold);

            m_cboxPortType = new DataGridViewComboBoxCell();
            m_cboxPortType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            m_cboxPortType.Items.Add("MGV & AGV Port");
            m_cboxPortType.Items.Add("OHT & MGV Port");
            m_cboxPortType.Items.Add("Auto Port");
            m_cboxPortType.Items.Add("OVEN Port");
            m_cboxPortType.Items.Add("SORTER Port");

            m_cboxProfileType = new DataGridViewComboBoxCell[(int)MtList.MAX_COUNT];
            for(int i = 0; i < m_cboxProfileType.Length; i++) {
                m_cboxProfileType[i] = new DataGridViewComboBoxCell();
                m_cboxProfileType[i].DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                m_cboxProfileType[i].Items.Add("Trapezoidal");
                m_cboxProfileType[i].Items.Add("SCurve");
                m_cboxProfileType[i].Items.Add("JerkRatio");

                numberRow = dgvServoSettings.Rows.Add("", "", "", "", "", "", "", null, "", "", "");
                dgvServoSettings.Rows[numberRow].Cells[7] = m_cboxProfileType[i];
            }
            dgvServoSettings.Rows[(int)MtList.STK_X].Cells[0].Value = "STK X";
            dgvServoSettings.Rows[(int)MtList.STK_Z].Cells[0].Value = "STK Z";
            dgvServoSettings.Rows[(int)MtList.STK_A].Cells[0].Value = "STK A";
            dgvServoSettings.Rows[(int)MtList.STK_T].Cells[0].Value = "STK T";
            dgvServoSettings.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold);
        }

        /// <summary>
        /// UI Form Update
        /// </summary>
        public void UpdateFormUI() {
            if(NullableFunction.TryParseNullable(txtID.Text, out m_portId)) {
                if (Port.IsExistPort((int)m_portId)) {
                    txtIDError.BackColor = Color.White;
                    ButtonEnabledTeachingPanel(true);

                    if (Port.IsExistPortData((int)m_portId)) {
                        m_curPortIndex = Port.GetPortIndex((int)m_portId);

                        m_xPos = Port.m_port[(int)m_curPortIndex].valX / 1000;
                        m_zPos = Port.m_port[(int)m_curPortIndex].valZ / 1000;
                        m_aPos = Port.m_port[(int)m_curPortIndex].valFork_A / 1000;
                        m_tPos = Port.m_port[(int)m_curPortIndex].valT / 1000;
                        m_zUpPos = Port.m_port[(int)m_curPortIndex].valZUp / 1000;
                        m_zdownPos = Port.m_port[(int)m_curPortIndex].valZDown / 1000;

                        txtTeachXPos.Text = m_xPos.ToString();
                        txtTeachZPos.Text = m_zPos.ToString();
                        txtTeachAPos.Text = m_aPos.ToString();
                        txtTeachTPos.Text = m_tPos.ToString();
                        txtTeachZUp.Text = m_zUpPos.ToString();
                        txtTeachZDown.Text = m_zdownPos.ToString();
                    }
                }
                else {
                    BlinkingIDError();
                }
            }
            else {
                BlinkingIDError();
            }

            txtCurXPos.Text = (m_motor.m_status[(int)MtList.STK_X].m_actualPos / 1000).ToString("F3");
            txtCurZPos.Text = (m_motor.m_status[(int)MtList.STK_Z].m_actualPos / 1000).ToString("F3");
            txtCurAPos.Text = (m_motor.m_status[(int)MtList.STK_A].m_actualPos / 1000).ToString("F3");
            txtCurTPos.Text = (m_motor.m_status[(int)MtList.STK_T].m_actualPos / 1000).ToString("F3");
        }

        /// <summary>
        /// Port ID가 존재하지 않으면 점등
        /// </summary>
        private void BlinkingIDError() {
            ButtonEnabledTeachingPanel(false);
            if (m_blinkTimer.Delay(700)) {
                if(txtIDError.BackColor == Color.White) {
                    txtIDError.BackColor = Color.GreenYellow;
                }

                if(txtIDError.BackColor == Color.Red) {
                    txtIDError.BackColor = Color.GreenYellow;
                }else if(txtIDError.BackColor == Color.GreenYellow) {
                    txtIDError.BackColor = Color.Red;
                }
                m_blinkTimer.Restart();
            }
        }

        /// <summary>
        /// Port의 Column과 Row가 정의되지 않으면 Teaching관련 버튼은 모두 비활성화
        /// </summary>
        /// <param name="enabled"></param>
        private void ButtonEnabledTeachingPanel(bool enabled) {
            if (enabled) {
                btnAAllTeaching.Enabled = true;
                btnOneShelfTeaching.Enabled = true;
                btnTAllTeacing.Enabled = true;
                btnXZTeaching.Enabled = true;
                btnZUpDownTeaching.Enabled = true;
            }
            else {
                btnAAllTeaching.Enabled = false;
                btnOneShelfTeaching.Enabled = false;
                btnTAllTeacing.Enabled = false;
                btnXZTeaching.Enabled = false;
                btnZUpDownTeaching.Enabled = false;
            }
        }

        /// <summary>
        /// One Shelf Teaching
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOneShelfTeaching_Click(object sender, EventArgs e) {
            float zUpData = 0;
            float zDownData = 0;

            if (!float.TryParse(txtCurZUp.Text, out zUpData) || !float.TryParse(txtCurZDown.Text, out zDownData)) {
                MessageBox.Show("올바른 값을 입력하세요.");
                return;
            }

            if(DialogResult.No != MessageBox.Show("저장하시겠습니까?", "경고", MessageBoxButtons.YesNo)){
                return;
            }

            float xData = (float)m_motor.m_status[(int)MtList.STK_X].m_actualPos;
            float zData = (float)m_motor.m_status[(int)MtList.STK_Z].m_actualPos;
            float forkAData = (float)m_motor.m_status[(int)MtList.STK_A].m_actualPos;
            float forkTData = (float)m_motor.m_status[(int)MtList.STK_T].m_actualPos;
            float tData = forkTData - forkAData;
            zUpData *= 1000;
            zDownData *= 1000;

            Port.SetAllValue((int)m_portId, xData, tData, zData, forkAData, forkTData);
            Port.SetValZ_UpDown((int)m_curPortIndex, zUpData, zDownData);
            Port.SaveDataFile();
        }

        /// <summary>
        /// 해당 방향에 있는 모든 포트의 Turn 데이터 티칭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTAllTeacing_Click(object sender, EventArgs e) {
            if (DialogResult.No != MessageBox.Show("저장하시겠습니까?", "경고", MessageBoxButtons.YesNo)){
                return;
            }

            float forkAData = (float)m_motor.m_status[(int)MtList.STK_A].m_actualPos;
            float forkTData = (float)m_motor.m_status[(int)MtList.STK_T].m_actualPos;
            float tData = forkTData - forkAData;

            int direction = (int)Port.m_port[(int)m_curPortIndex].direction;

            Port.SetAllTurn(direction, tData);
            Port.SaveDataFile();
        }

        /// <summary>
        /// 해당 방향에 있는 모든 포트의 Fork 데이터 티칭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAAllTeaching_Click(object sender, EventArgs e) {
            if (DialogResult.No != MessageBox.Show("저장하시겠습니까?", "경고", MessageBoxButtons.YesNo)){
                return;
            }

            float forkAData = (float)m_motor.m_status[(int)MtList.STK_A].m_actualPos;
            float forkTData = (float)m_motor.m_status[(int)MtList.STK_T].m_actualPos;

            int direction = (int)Port.m_port[(int)m_curPortIndex].direction;

            Port.SetAllFork(direction, forkAData, forkTData);
            Port.SaveDataFile();
        }

        /// <summary>
        /// 모든 포트의 Z축의 Form, To 범위 티칭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnZUpDownTeaching_Click(object sender, EventArgs e) {
            float zUpData = 0;
            float zDownData = 0;

            if (!float.TryParse(txtCurZUp.Text, out zUpData) || !float.TryParse(txtCurZDown.Text, out zDownData)) {
                MessageBox.Show("올바른 값을 입력하세요.");
                return;
            }

            if (DialogResult.No != MessageBox.Show("저장하시겠습니까?", "경고", MessageBoxButtons.YesNo)){
                return;
            }

            zUpData *= 1000;
            zDownData *= 1000;

            Port.SetAllValZ_UpDown(zUpData, zDownData);
            Port.SaveDataFile();
        }

        /// <summary>
        /// 해당 포트의 X, Z 데이터 티칭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXZTeaching_Click(object sender, EventArgs e) {
            if (DialogResult.No != MessageBox.Show("저장하시겠습니까?", "경고", MessageBoxButtons.YesNo)){
                return;
            }

            float xData = (float)m_motor.m_status[(int)MtList.STK_X].m_actualPos;
            float zData = (float)m_motor.m_status[(int)MtList.STK_Z].m_actualPos;

            Port.SetValXZ((int)m_curPortIndex, xData, zData);
            Port.SaveDataFile();
        }

        private void btnTeachingPage_Click(object sender, EventArgs e) {
            pnlTeaching.Visible = true;
            pnlRM.Visible = false;
            pnlWMX.Visible = false;
        }

        private void btnWmxPage_Click(object sender, EventArgs e) {
            pnlTeaching.Visible = false;
            pnlRM.Visible = false;
            pnlWMX.Visible = true;
        }

        private void btnMotorPage_Click(object sender, EventArgs e) {
            pnlTeaching.Visible = false;
            pnlRM.Visible = true;
            pnlWMX.Visible = false;
        }

        private void btnRMApply_Click(object sender, EventArgs e) {

        }

        private void btnAutoTeachingStart_Click(object sender, EventArgs e) {

        }
    }
}
