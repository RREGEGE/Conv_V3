using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OMRON.Compolet.Variable;
using System.Reflection;
using System.Collections;

namespace OmronTagCommunicator
{
    public partial class Form1 : Form
    {
        VariableCompolet variableCompolet1;
        static public bool m_bValidState = false;
        static public bool[] m_ReadOmronBitMap = new bool[64 * 4];
        static public short[] m_ReadOmronWordMap = new short[64 * 4];
        static public bool[] m_WriteOmronBitMap = new bool[64 * 4];
        static public short[] m_WriteOmronWordMap = new short[64 * 4];
        string[] m_ReadVariableName = new string[] { "CV_STK_01_Send", "CV_STK_02_Send", "CV_STK_03_Send", "CV_STK_04_Send" };
        string[] m_WriteVariableName = new string[] { "STK_CV_01_Send", "STK_CV_02_Send", "STK_CV_03_Send", "STK_CV_04_Send" };
        DataSender dtSender;
        bool m_bShutdown = false;
        public Form1()
        {
            InitializeComponent();
            ControlItemInit();
            ContextMenuItemInit();
            variableCompolet1 = new VariableCompolet();
            variableCompolet1.Active = false;
            variableCompolet1.PlcEncoding = System.Text.Encoding.GetEncoding("utf-8");
            timer1.Enabled = true;

            dtSender = new DataSender();

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                if (!m_bShutdown)
                {
                    //종료가 아닌 경우 최소화
                    e.Cancel = true;
                    this.Visible = false;
                }
            };
        }
        private void ControlItemInit()
        {
            foreach (var item in this.Controls)
            {
                item.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, item, new object[] { true });

                if (((Control)item).Controls.Count > 0)
                    SetDoubleBuffer((Control)item);
            }

            //m_ReadOmronBitMap = new bool[4 * 64];
            //m_ReadOmronWordMap = new short[4 * 64];
            //m_WriteOmronBitMap = new bool[4 * 64];
            //m_WriteOmronWordMap = new short[4 * 64];

            STK_To_CV_BitMapLoad();
            STK_To_CV_WordMapLoad();
        }
        public void SetDoubleBuffer(Control control)
        {
            foreach (var item in control.Controls)
            {
                item.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, item, new object[] { true });

                if (((Control)item).Controls.Count > 0)
                    SetDoubleBuffer((Control)item);
            }
        }

        private void btn_Activate_Click(object sender, EventArgs e)
        {
            if (!variableCompolet1.Active)
            {
                variableCompolet1.Active = true;
                variableCompolet1.WindowHandle = this.Handle;
            }
            else
            {
                variableCompolet1.Active = false;
                variableCompolet1.WindowHandle = this.Handle;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                bool bTCPState = dtSender.IsConnected();
                lbl_TCPIPState.Text = bTCPState ? "Connect" : "DisConnect";
                lbl_TCPIPState.BackColor = bTCPState ? Color.Lime : Color.Red;

                if (variableCompolet1 == null)
                {
                    m_bValidState = false;
                    return;
                }
                bool bActivation = variableCompolet1.Active;
                lbl_Activate.Text = bActivation ? "Activation" : "Not Activation";
                lbl_Activate.BackColor = bActivation ? Color.Lime : Color.Red;


                if (variableCompolet1.Active)
                {
                    int VariableCount = variableCompolet1.VariableNames.Length;
                    lbl_VariableNamesCount.Text = $"{VariableCount}";
                    lbl_VariableNamesCount.BackColor = VariableCount == 0 ? Color.Red : Color.Lime;

                    m_bValidState = OmronUpdate();

                    CV_To_STK_BitMapUpdate();
                    CV_To_STK_WordMapUpdate();

                    if (dtSender.IsConnected())
                    {
                        STK_To_CV_BitMapUpdate();
                        STK_To_CV_WordMapUpdate();
                    }
                }
                else
                {
                    m_bValidState = false;
                }
            }
            catch(Exception ex)
            {
                lbl_Exception.Text = ex.ToString();
                richTextBox1.Text = ex.ToString();
                m_bValidState = false;
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            m_bShutdown = false;
            this.Visible = true;
        }
        private void btn_Show_Click(object sender, EventArgs eventArgs)
        {
            m_bShutdown = false;
            this.Visible = true;
        }

        private void btn_Exit_Click(object sender, EventArgs eventArgs)
        {
            m_bShutdown = true;
            this.Close();
        }
        private void ContextMenuItemInit()
        {
            ContextMenu ctx = new ContextMenu();

            MenuItem item = new MenuItem();
            item.Text = "Open";
            item.Click += btn_Show_Click;

            ctx.MenuItems.Add(item);
            ctx.MenuItems.Add("-");

            MenuItem item2 = new MenuItem();
            item2.Text = "Close";
            item2.Click += btn_Exit_Click;
            ctx.MenuItems.Add(item2);

            notifyIcon1.ContextMenu = ctx;
        }
        private void CV_To_STK_BitMapUpdate()
        {
            if (DGV_CV_STK_Bit_Map.Rows.Count != m_ReadOmronBitMap.Length)
            {
                DGV_CV_STK_Bit_Map.Rows.Clear();

                for (int nCount = 0; nCount < m_ReadOmronBitMap.Length; nCount++)
                {
                    int nRemainNum = nCount % 64;
                    int nObjectNum = nCount / 64;

                    DGV_CV_STK_Bit_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{m_ReadOmronBitMap[nCount]}" });
                }
            }
            else
            {
                for (int nCount = 0; nCount < m_ReadOmronBitMap.Length; nCount++)
                {
                    DataGridViewCell cell = DGV_CV_STK_Bit_Map.Rows[nCount].Cells[2];
                    cell.Value = $"{m_ReadOmronBitMap[nCount]}";
                }
            }
        }
        private void CV_To_STK_WordMapUpdate()
        {
            if (DGV_CV_STK_Word_Map.Rows.Count != m_ReadOmronWordMap.Length)
            {
                DGV_CV_STK_Word_Map.Rows.Clear();

                for (int nCount = 0; nCount < m_ReadOmronWordMap.Length; nCount++)
                {
                    int nRemainNum = nCount % 64;
                    int nObjectNum = nCount / 64;

                    DGV_CV_STK_Word_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{m_ReadOmronWordMap[nCount]}" });
                }
            }
            else
            {
                for (int nCount = 0; nCount < m_ReadOmronWordMap.Length; nCount++)
                {
                    DataGridViewCell cell = DGV_CV_STK_Word_Map.Rows[nCount].Cells[2];
                    cell.Value = $"{m_ReadOmronWordMap[nCount]}";
                }
            }
        }
        private void STK_To_CV_BitMapUpdate()
        {
            if (DGV_STK_CV_Bit_Map.Rows.Count != m_WriteOmronBitMap.Length)
            {
                DGV_STK_CV_Bit_Map.Rows.Clear();

                for (int nCount = 0; nCount < m_WriteOmronBitMap.Length; nCount++)
                {
                    int nRemainNum = nCount % 64;
                    int nObjectNum = nCount / 64;

                    DGV_STK_CV_Bit_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{m_ReadOmronBitMap[nCount]}" });
                }
            }
            else
            {
                for (int nCount = 0; nCount < m_WriteOmronBitMap.Length; nCount++)
                {
                    DataGridViewCell cell = DGV_STK_CV_Bit_Map.Rows[nCount].Cells[2];
                    cell.Value = $"{m_WriteOmronBitMap[nCount]}";
                }
            }
        }
        private void STK_To_CV_WordMapUpdate()
        {
            if (DGV_STK_CV_Word_Map.Rows.Count != m_WriteOmronWordMap.Length)
            {
                DGV_STK_CV_Word_Map.Rows.Clear();

                for (int nCount = 0; nCount < m_WriteOmronWordMap.Length; nCount++)
                {
                    int nRemainNum = nCount % 64;
                    int nObjectNum = nCount / 64;

                    DGV_STK_CV_Word_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{m_ReadOmronWordMap[nCount]}" });
                }
            }
            else
            {
                for (int nCount = 0; nCount < m_WriteOmronWordMap.Length; nCount++)
                {
                    DataGridViewCell cell = DGV_STK_CV_Word_Map.Rows[nCount].Cells[2];
                    cell.Value = $"{m_WriteOmronWordMap[nCount]}";
                }
            }
        }

        private void STK_To_CV_BitMapLoad()
        {
            DGV_STK_CV_Bit_Map.Rows.Clear();

            for (int nCount = 0; nCount < m_WriteOmronBitMap.Length; nCount++)
            {
                int nRemainNum = nCount % 64;
                int nObjectNum = nCount / 64;

                DGV_STK_CV_Bit_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{m_WriteOmronBitMap[nCount]}", $"Set" });
            }
        }
        private void STK_To_CV_WordMapLoad()
        {
            DGV_STK_CV_Word_Map.Rows.Clear();

            for (int nCount = 0; nCount < m_WriteOmronWordMap.Length; nCount++)
            {
                int nRemainNum = nCount % 64;
                int nObjectNum = nCount / 64;

                DGV_STK_CV_Word_Map.Rows.Add(new string[] { $"{nRemainNum}", $"{nObjectNum}", $"{m_WriteOmronWordMap[nCount]}", $"Set" });
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

                    if (e.RowIndex < m_WriteOmronBitMap.Length)
                    {
                        m_WriteOmronBitMap[e.RowIndex] = Convert.ToBoolean(DGV_WriteCell.Value);
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

                    if (e.RowIndex < m_WriteOmronWordMap.Length)
                    {
                        m_WriteOmronWordMap[e.RowIndex] = Convert.ToInt16(DGV_WriteCell.Value);
                    }
                }
            }
        }


        private bool OmronUpdate()
        {
            //try
            //{
                for (int nLength = 0; nLength < m_ReadVariableName.Length; nLength++)
                {
                    object value = variableCompolet1.ReadVariable(m_ReadVariableName[nLength]);

                    byte[] Data = value as byte[];
                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                    byte[] BoolSection = new byte[8];
                    Buffer.BlockCopy(Data, 0, BoolSection, 0, 8);
                    BitArray ba = new BitArray(BoolSection);

                    for (int nCount = 0; nCount < ba.Length; nCount++)
                        m_ReadOmronBitMap[nCount + (nLength * 64)] = ba[nCount];

                    Buffer.BlockCopy(Data, 8, m_ReadOmronWordMap, nLength * 64 * sizeof(short), 64 * sizeof(short));
                }


                for (int nLength = 0; nLength < m_WriteVariableName.Length; nLength++)
                {
                    byte[] Data = new byte[136];

                    bool[] GetBoolArray = new bool[64];

                    for (int nCount = 0; nCount < GetBoolArray.Length; nCount++)
                        GetBoolArray[nCount] = m_WriteOmronBitMap[nCount + (nLength * 64)];

                    BitArray ba = new BitArray(GetBoolArray);
                    ba.CopyTo(Data, 0);
                    //Buffer.BlockCopy(m_WriteOmronBitMap, nLength * 8, Data, 0, 8);
                    Buffer.BlockCopy(m_WriteOmronWordMap, nLength * 64 * sizeof(short), Data, 8, 64 * sizeof(short));

                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++)
                    {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }

                    if (nLength == 0)
                    {
                        richTextBox1.Clear();
                        richTextBox1.Text = BitConverter.ToString(Data) + "\n";
                    }
                    else
                    {
                        richTextBox1.AppendText(BitConverter.ToString(Data) + "\n");
                    }

                    object DtArray = Data;
                    variableCompolet1.WriteVariable(m_WriteVariableName[nLength], DtArray);
                }

                lbl_Exception.Text = string.Empty;
                return true;
            //}
            //catch(Exception ex)
            //{
            //    lbl_Exception.Text = ex.ToString();
            //    if(richTextBox1.Text != ex.ToString())
            //        richTextBox1.Text = ex.ToString();
            //    return false;
            //}
        }

        private void btn_WriteMapRefresh_Click(object sender, EventArgs e)
        {
            STK_To_CV_BitMapLoad();
            STK_To_CV_WordMapLoad();
        }
    }
}
