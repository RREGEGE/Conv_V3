using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Master.GlobalForm
{
    public partial class Frm_OmronTable : Form
    {
        public Frm_OmronTable()
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

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);
            STK_To_CV_BitMapLoad();
            STK_To_CV_WordMapLoad();
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
                GroupBoxFunc.SetText(gorupBox_MemoryMapInfo, SynusLangPack.GetLanguage("GorupBox_MemoryMapInfo"));

                CV_To_STK_BitMapUpdate();
                CV_To_STK_WordMapUpdate();
            }
            catch { }
            finally
            {

            }
        }

        private void CV_To_STK_BitMapUpdate()
        {
            if(DGV_CV_STK_Bit_Map.Rows.Count != Master.m_ReadOmronBitMap.Length)
            {
                DGV_CV_STK_Bit_Map.Rows.Clear();

                for(int nCount =0;nCount < Master.m_ReadOmronBitMap.Length; nCount++)
                {
                    int nRemainNum = nCount % 64;
                    int nObjectNum = nCount / 64;

                    DGV_CV_STK_Bit_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{Master.m_ReadOmronBitMap[nCount]}" });
                }
            }
            else
            {
                for (int nCount = 0; nCount < Master.m_ReadOmronBitMap.Length; nCount++)
                {
                    DataGridViewCell cell = DGV_CV_STK_Bit_Map.Rows[nCount].Cells[2];
                    cell.Value = $"{Master.m_ReadOmronBitMap[nCount]}";
                }
            }
        }
        private void CV_To_STK_WordMapUpdate()
        {
            if (DGV_CV_STK_Word_Map.Rows.Count != Master.m_ReadOmronWordMap.Length)
            {
                DGV_CV_STK_Word_Map.Rows.Clear();

                for (int nCount = 0; nCount < Master.m_ReadOmronWordMap.Length; nCount++)
                {
                    int nRemainNum = nCount % 64;
                    int nObjectNum = nCount / 64;

                    DGV_CV_STK_Word_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{Master.m_ReadOmronWordMap[nCount]}" });
                }
            }
            else
            {
                for (int nCount = 0; nCount < Master.m_ReadOmronWordMap.Length; nCount++)
                {
                    DataGridViewCell cell = DGV_CV_STK_Word_Map.Rows[nCount].Cells[2];
                    cell.Value = $"{Master.m_ReadOmronWordMap[nCount]}";
                }
            }
        }
    
        private void STK_To_CV_BitMapLoad()
        {
            DGV_STK_CV_Bit_Map.Rows.Clear();

            for (int nCount = 0; nCount < Master.m_WriteOmronBitMap.Length; nCount++)
            {
                int nRemainNum = nCount % 64;
                int nObjectNum = nCount / 64;

                DGV_STK_CV_Bit_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{Master.m_WriteOmronBitMap[nCount]}", $"Set" });
            }
        }
        private void STK_To_CV_WordMapLoad()
        {
            DGV_STK_CV_Word_Map.Rows.Clear();

            for (int nCount = 0; nCount < Master.m_WriteOmronWordMap.Length; nCount++)
            {
                int nRemainNum = nCount % 64;
                int nObjectNum = nCount / 64;

                DGV_STK_CV_Word_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{Master.m_WriteOmronWordMap[nCount]}", $"Set" });
            }
        }

        private void DGV_STK_CV_Bit_Map_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataGridViewCell DGV_Cell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string value = (string)DGV_Cell.Value;

                if (value.Contains("Set"))
                {
                    DataGridViewCell DGV_WriteCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1];

                    if (e.RowIndex < Master.m_WriteOmronBitMap.Length)
                    {
                        Master.m_WriteOmronBitMap[e.RowIndex] = Convert.ToBoolean(DGV_WriteCell.Value);
                    }
                }
            }
        }

        private void DGV_STK_CV_Word_Map_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DataGridViewCell DGV_Cell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                string value = (string)DGV_Cell.Value;

                if (value.Contains("Set"))
                {
                    DataGridViewCell DGV_WriteCell = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex - 1];

                    if (e.RowIndex < Master.m_WriteOmronWordMap.Length)
                    {
                        Master.m_WriteOmronWordMap[e.RowIndex] = Convert.ToInt16(DGV_WriteCell.Value);
                    }
                }
            }
        }
    }
}
