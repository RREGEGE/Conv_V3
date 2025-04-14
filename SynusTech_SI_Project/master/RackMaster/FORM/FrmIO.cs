using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;

namespace RackMaster.FORM {
    public partial class FrmIO : Form {
        private FrmLogin frmLogin;

        private XmlFile m_xml;

        private string m_xmlPath = ManagedFileInfo.DataDirectory + "\\" + ManagedFileInfo.IoListFileName;
        private string m_xmlRoot = "IoList";

        private bool m_isModifyMode = false;

        public FrmIO() {
            InitializeComponent();

            InitDataGridView();

            m_xml = new XmlFile(m_xmlPath, m_xmlRoot);

            if (!m_xml.m_isFileExist) {
                CreateIoList();
            }
            else {
                LoadIoList();
            }

            dgvInput.CellValueChanged += CellValueChaned;
            dgvOutput.CellValueChanged += CellValueChaned;
        }

        private void CreateIoList() {
            string xmlParent = m_xmlRoot;
            m_xml.SetNodeVal(xmlParent, "Input", null, true);
            xmlParent = m_xmlRoot + "/" + "Input";
            for (int i = 0; i < (Io.MAX_BYTE * 8); i++) {
                string nodeName = "Index" + i.ToString();
                m_xml.SetNodeVal(xmlParent, nodeName, null, true);
            }

            xmlParent = m_xmlRoot;
            m_xml.SetNodeVal(xmlParent, "Output", null, true);
            xmlParent = m_xmlRoot + "/" + "Output";
            for (int i = 0; i < (Io.MAX_BYTE * 8); i++) {
                string nodeName = "Index" + i.ToString();
                m_xml.SetNodeVal(xmlParent, nodeName, null, true);
            }

            m_xml.Save();
        }

        private void InitDataGridView() {
            for(int i = 0; i < (Io.MAX_BYTE * 8); i++) {
                dgvInput.Rows.Add(i, "", "OFF");
                dgvOutput.Rows.Add(i, "", "OFF");
            }

            foreach(DataGridViewColumn column in dgvInput.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            foreach(DataGridViewColumn column in dgvOutput.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void LoadIoList() {
            for(int i = 0; i < (Io.MAX_BYTE * 8); i++) {
                string inputNodeName = m_xmlRoot + "/" + "Input" + "/" + "Index" + i.ToString();
                string outputNodeName = m_xmlRoot + "/" + "Output" + "/" + "Index" + i.ToString();
                string inputValue = m_xml.GetNodeVal(inputNodeName);
                string outputValue = m_xml.GetNodeVal(outputNodeName);

                dgvInput[1, i].Value = inputValue;
                dgvOutput[1, i].Value = outputValue;
            }
        }

        public void UpdateFormUI() {
            for(int i = 0; i < (Io.MAX_BYTE * 8); i++) {
                if (Io.GetInputBit(i)) {
                    dgvInput[2, i].Value = "ON";
                    dgvInput[2, i].Style.BackColor = Color.OrangeRed;
                }
                else {
                    dgvInput[2, i].Value = "OFF";
                    dgvInput[2, i].Style.BackColor = Color.AliceBlue;
                }

                if (Io.GetOutputBit(i)) {
                    dgvOutput[2, i].Value = "ON";
                    dgvOutput[2, i].Style.BackColor = Color.OrangeRed;
                }
                else {
                    dgvOutput[2, i].Value = "OFF";
                    dgvOutput[2, i].Style.BackColor = Color.AliceBlue;
                }
            }
        }

        private void btnModifyMode_Click(object sender, EventArgs e) {
            if (m_isModifyMode) {
                btnModifyMode.BackColor = Color.AliceBlue;
                m_isModifyMode = false;

                dgvInput.Columns[1].ReadOnly = true;
                dgvOutput.Columns[1].ReadOnly = true;
            }
            else {
                frmLogin = new FrmLogin();
                frmLogin.loginEvent += AccessModifyMode;
                frmLogin.ShowDialog();
            }
        }

        private void AccessModifyMode() {
            btnModifyMode.BackColor = Color.OrangeRed;
            m_isModifyMode = true;

            dgvInput.Columns[1].ReadOnly = false;
            dgvOutput.Columns[1].ReadOnly = false;
        }

        private void CellValueChaned(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (e.ColumnIndex != 1) return;

            DataGridView dgv = sender as DataGridView;

            string parentName = "";

            if (dgv == dgvInput) {
                parentName = m_xmlRoot + "/" + "Input";
            }else if(dgv == dgvOutput) {
                parentName = m_xmlRoot + "/" + "Output";
            }

            string nodeName = "Index" + e.RowIndex.ToString();

            string value = dgv[e.ColumnIndex, e.RowIndex].Value.ToString();

            m_xml.SetNodeVal(parentName, nodeName, value);
            m_xml.Save();
        }

        private void dgvOutput_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (!m_isModifyMode) return;

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (e.ColumnIndex != 2) return;

            int bitAddr = e.RowIndex;

            if (Io.GetOutputBit(bitAddr)) {
                Io.SetOutputBit((bitAddr / 8), (bitAddr % 8), false);
            }
            else {
                Io.SetOutputBit((bitAddr / 8), (bitAddr % 8), true);
            }
        }
    }
}
