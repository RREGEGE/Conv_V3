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

namespace RackMaster.SUBFORM {
    public partial class FrmMemoryMap : Form {
        private enum DgvColumnList {
            No,
            Name,
            Data,
        }

        private RackMasterMain m_rackMaster;
        private UICtrl.DataGridViewCtrl m_dgvCtrl;

        public FrmMemoryMap(RackMasterMain rackMaster) {
            m_rackMaster = rackMaster;
            m_dgvCtrl = new UICtrl.DataGridViewCtrl();

            InitializeComponent();

            InitDataGridView();
        }

        private void InitDataGridView() {
            m_dgvCtrl.DisableUserControl(ref dgvCIM2RMBit);
            m_dgvCtrl.DisableUserControl(ref dgvRM2CIMBit);
            m_dgvCtrl.DisableUserControl(ref dgvCIM2RMWord);
            m_dgvCtrl.DisableUserControl(ref dgvRM2CIMWord);

            InitDataGridView_CIM2RMBit();
            InitDataGridView_RM2CIMBit();
            InitDataGridView_CIM2RMWord();
            InitDataGridView_RM2CIMWord();
        }

        private void InitDataGridView_CIM2RMBit() {
            for(int idx = 0; idx < m_rackMaster.GetMemoryMapSize_CIM2RMBit(); idx++) {
                m_dgvCtrl.AddRow(ref dgvCIM2RMBit, $"0x{idx:X2}", (int)DgvColumnList.No);

                if (Enum.IsDefined(typeof(ReceiveBitMap), idx)) {
                    m_dgvCtrl.SetCellText(ref dgvCIM2RMBit, idx, (int)DgvColumnList.Name, $"{Enum.GetName(typeof(ReceiveBitMap), idx)}");
                }
            }

            foreach(DataGridViewColumn column in dgvCIM2RMBit.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void InitDataGridView_RM2CIMBit() {
            for(int idx = 0; idx < m_rackMaster.GetMemoryMapSize_RM2CIMBit(); idx++) {
                m_dgvCtrl.AddRow(ref dgvRM2CIMBit, $"0x{idx:x2}", (int)DgvColumnList.No);

                if(Enum.IsDefined(typeof(SendBitMap), idx)) {
                    m_dgvCtrl.SetCellText(ref dgvRM2CIMBit, idx, (int)DgvColumnList.Name, $"{Enum.GetName(typeof(SendBitMap), idx)}");
                }
            }

            foreach(DataGridViewColumn column in dgvRM2CIMBit.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void InitDataGridView_CIM2RMWord() {
            for(int idx = 0; idx < m_rackMaster.GetMemoryMapSize_CIM2RMWord(); idx++) {
                m_dgvCtrl.AddRow(ref dgvCIM2RMWord, $"0x{idx:x2}", (int)DgvColumnList.No);

                if(Enum.IsDefined(typeof(ReceiveWordMap), idx)) {
                    m_dgvCtrl.SetCellText(ref dgvCIM2RMWord, idx, (int)DgvColumnList.Name, $"{Enum.GetName(typeof(ReceiveWordMap), idx)}");
                }
            }

            foreach (DataGridViewColumn column in dgvCIM2RMWord.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void InitDataGridView_RM2CIMWord() {
            for (int idx = 0; idx < m_rackMaster.GetMemoryMapSize_RM2CIMWord(); idx++) {
                m_dgvCtrl.AddRow(ref dgvRM2CIMWord, $"0x{idx:x2}", (int)DgvColumnList.No);

                if (Enum.IsDefined(typeof(SendWordMap), idx)) {
                    m_dgvCtrl.SetCellText(ref dgvRM2CIMWord, idx, (int)DgvColumnList.Name, $"{Enum.GetName(typeof(SendWordMap), idx)}");
                }
            }

            foreach (DataGridViewColumn column in dgvRM2CIMWord.Columns) {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }
        }

        private void FrmMemoryMap_Load(object sender, EventArgs e) {
            statusTimer.Start();
        }

        private void FrmMemoryMap_FormClosing(object sender, FormClosingEventArgs e) {
            statusTimer.Stop();
        }

        private void statusTimer_Tick(object sender, EventArgs e) {
            foreach (ReceiveBitMap recvBit in Enum.GetValues(typeof(ReceiveBitMap))) {
                if (recvBit == ReceiveBitMap.MAX_COUNT)
                    continue;

                m_dgvCtrl.SetOnOffCell(ref dgvCIM2RMBit, (int)recvBit, (int)DgvColumnList.Data, m_rackMaster.GetReceiveBit(recvBit));
            }

            foreach(SendBitMap sendBit in Enum.GetValues(typeof(SendBitMap))) {
                if (sendBit == SendBitMap.MAX_COUNT)
                    continue;

                m_dgvCtrl.SetOnOffCell(ref dgvRM2CIMBit, (int)sendBit, (int)DgvColumnList.Data, m_rackMaster.GetSendBit(sendBit));
            }

            foreach (ReceiveWordMap recvWord in Enum.GetValues(typeof(ReceiveWordMap))) {
                if (recvWord == ReceiveWordMap.MAX_COUNT)
                    continue;

                m_dgvCtrl.SetCellText(ref dgvCIM2RMWord, (int)recvWord, (int)DgvColumnList.Data, $"{m_rackMaster.GetReceiveWord(recvWord)}");
            }

            foreach (SendWordMap sendWord in Enum.GetValues(typeof(SendWordMap))) {
                if (sendWord == SendWordMap.MAX_COUNT)
                    continue;

                m_dgvCtrl.SetCellText(ref dgvRM2CIMWord, (int)sendWord, (int)DgvColumnList.Data, $"{m_rackMaster.GetSendWord(sendWord)}");
            }
        }
    }
}
