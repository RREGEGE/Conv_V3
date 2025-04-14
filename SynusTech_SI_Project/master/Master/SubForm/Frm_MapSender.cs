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

namespace Master.SubForm
{
    public partial class Frm_MapSender : Form
    {
        public Frm_MapSender()
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

            this.FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                this.Dispose();
            };
        }
        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);
            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_BitMap, new object[] { true });
            //typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_WordMap, new object[] { true });

            cbx_EquipType.Items.AddRange(new string[3] { "Master", "RackMaster", "Port" });
            cbx_EquipType.SelectedIndex = 0;
            cbx_Direction.Items.AddRange(new string[2] { "CIM->Equip(Recv)", "Equip->CIM(Send)" });
            cbx_Direction.SelectedIndex = 0;
        }
        private void UIUpdateTimer_Tick(object sender, EventArgs e)
        {
            string CurrentItemTag = cbx_EquipType.Text + "_" + cbx_Direction.Text;
            int EquipIndex = cbx_EquipType.SelectedIndex;
            int DirectionIndex = cbx_Direction.SelectedIndex;

            try
            {
                if ((string)DGV_BitMap.Tag != CurrentItemTag)
                {
                    ReLoad(DGV_BitMap);
                    DGV_BitMap.Tag = CurrentItemTag;
                }

                if ((string)DGV_WordMap.Tag != CurrentItemTag)
                {
                    ReLoad(DGV_WordMap);
                    DGV_WordMap.Tag = CurrentItemTag;
                }
            }
            catch { }
        }

        private void ReLoad(DataGridView DGV)
        {
            DGV.Rows.Clear();

            if (DGV == DGV_BitMap)
            {
                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_RecvBitMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[2] { $"{(Equipment.CIM.CIM.ReceiveBitMapIndex)nCount}", $"{Master.m_CIM_RecvBitMap[nCount]}" });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_SendBitMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[2] { $"{(Equipment.CIM.CIM.SendBitMapIndex)nCount}", $"{Master.m_CIM_SendBitMap[nCount]}" });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_RecvBitMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[2] { $"{(Equipment.RackMaster.RackMaster.ReceiveBitMapIndex)(nCount % (Master.m_RackMaster_RecvBitMap.Length / Master.m_RackMasters.Count))}", $"{Master.m_RackMaster_RecvBitMap[nCount]}" });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_SendBitMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[2] { $"{(Equipment.RackMaster.RackMaster.SendBitMapIndex)(nCount % (Master.m_RackMaster_SendBitMap.Length / Master.m_RackMasters.Count))}", $"{Master.m_RackMaster_SendBitMap[nCount]}" });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_Port_RecvBitMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[2] { $"{(Equipment.Port.Port.ReceiveBitMapIndex)(nCount % (Master.m_Port_RecvBitMap.Length / Master.m_Ports.Count))}", $"{Master.m_Port_RecvBitMap[nCount]}" });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_Port_SendBitMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[2] { $"{(Equipment.Port.Port.SendBitMapIndex)(nCount % (Master.m_Port_SendBitMap.Length / Master.m_Ports.Count))}", $"{Master.m_Port_SendWordMap[nCount]}" });
                    }
                }
            }
            else if (DGV == DGV_WordMap)
            {
                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_RecvWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.CIM.CIM.ReceiveWordMapIndex)nCount}", $"0x{Master.m_CIM_RecvWordMap[nCount].ToString("x4")}", $"{Master.m_CIM_RecvWordMap[nCount]}", string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_SendWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.CIM.CIM.SendWordMapIndex)nCount}", $"0x{Master.m_CIM_SendWordMap[nCount].ToString("x4")}", $"{Master.m_CIM_SendWordMap[nCount]}", string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_RecvWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.RackMaster.RackMaster.ReceiveWordMapIndex)(nCount % (Master.m_RackMaster_RecvWordMap.Length / Master.m_RackMasters.Count))}", $"0x{Master.m_RackMaster_RecvWordMap[nCount].ToString("x4")}", $"{Master.m_RackMaster_RecvWordMap[nCount]}", string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_SendWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.RackMaster.RackMaster.SendWordMapIndex)(nCount % (Master.m_RackMaster_SendWordMap.Length / Master.m_RackMasters.Count))}", $"0x{Master.m_RackMaster_SendWordMap[nCount].ToString("x4")}", $"{Master.m_RackMaster_SendWordMap[nCount]}", string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_Port_RecvWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.Port.Port.ReceiveWordMapIndex)(nCount % (Master.m_Port_RecvWordMap.Length / Master.m_Ports.Count))}", $"0x{Master.m_Port_RecvWordMap[nCount].ToString("x4")}", $"{Master.m_Port_RecvWordMap[nCount]}", string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_Port_SendWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.Port.Port.SendWordMapIndex)(nCount % (Master.m_Port_SendWordMap.Length / Master.m_Ports.Count))}", $"0x{Master.m_Port_SendWordMap[nCount].ToString("x4")}", $"{Master.m_Port_SendWordMap[nCount]}", string.Empty });
                    }
                }
            }
        }

        private void DGV_BitMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (e.RowIndex >= 0)
                    tbx_BitMapStartAddr.Text = e.RowIndex.ToString();
            }
        }

        private void DGV_WordMap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (e.RowIndex >= 0)
                    tbx_WordMapStartAddr.Text = e.RowIndex.ToString();
            }
        }

        private void DGV_BitMap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 0)
            {
                if (e.RowIndex >= 0)
                    tbx_BitMapStartAddr.Text = e.RowIndex.ToString();
            }
        }

        private void DGV_WordMap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (e.RowIndex >= 0)
                    tbx_WordMapStartAddr.Text = e.RowIndex.ToString();
            }
        }

        private void DGV_BitMap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex != 1)
                return;

            DataGridViewCell DGV_Cell = DGV_BitMap.Rows[e.RowIndex].Cells[e.ColumnIndex];

            try
            {
                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    Master.m_CIM_RecvBitMap[e.RowIndex] = Convert.ToBoolean(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    Master.m_CIM_SendBitMap[e.RowIndex] = Convert.ToBoolean(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    Master.m_RackMaster_RecvBitMap[e.RowIndex] = Convert.ToBoolean(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    Master.m_RackMaster_SendBitMap[e.RowIndex] = Convert.ToBoolean(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    Master.m_Port_RecvBitMap[e.RowIndex] = Convert.ToBoolean(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    Master.m_Port_SendBitMap[e.RowIndex] = Convert.ToBoolean(DGV_Cell.Value);
                }
            }
            catch {
                DGV_Cell.Value = string.Empty;
            }
            
        }

        private void DGV_WordMap_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.ColumnIndex != 1 && e.ColumnIndex != 2 && e.ColumnIndex != 3)
                return;

            DataGridViewCell DGV_Cell = DGV_WordMap.Rows[e.RowIndex].Cells[e.ColumnIndex];

            try
            {
                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string hex = (string)DGV_Cell.Value;
                        Master.m_CIM_RecvWordMap[e.RowIndex] = Convert.ToInt16(hex.Replace("0x", string.Empty), 16);
                    }
                    else if (e.ColumnIndex == 2)
                        Master.m_CIM_RecvWordMap[e.RowIndex] = Convert.ToInt16(DGV_Cell.Value);
                    else if (e.ColumnIndex == 3)
                        Master.m_CIM_RecvWordMap[e.RowIndex] = (short)Convert.ToChar(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string hex = (string)DGV_Cell.Value;
                        Master.m_CIM_SendWordMap[e.RowIndex] = Convert.ToInt16(hex.Replace("0x", string.Empty), 16);
                    }
                    else if (e.ColumnIndex == 2)
                        Master.m_CIM_SendWordMap[e.RowIndex] = Convert.ToInt16(DGV_Cell.Value);
                    else if (e.ColumnIndex == 3)
                        Master.m_CIM_SendWordMap[e.RowIndex] = (short)Convert.ToChar(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string hex = (string)DGV_Cell.Value;
                        Master.m_RackMaster_RecvWordMap[e.RowIndex] = Convert.ToInt16(hex.Replace("0x", string.Empty), 16);
                    }
                    else if (e.ColumnIndex == 2)
                        Master.m_RackMaster_RecvWordMap[e.RowIndex] = Convert.ToInt16(DGV_Cell.Value);
                    else if (e.ColumnIndex == 3)
                        Master.m_RackMaster_RecvWordMap[e.RowIndex] = (short)Convert.ToChar(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string hex = (string)DGV_Cell.Value;
                        Master.m_RackMaster_SendWordMap[e.RowIndex] = Convert.ToInt16(hex.Replace("0x", string.Empty), 16);
                    }
                    else if (e.ColumnIndex == 2)
                        Master.m_RackMaster_SendWordMap[e.RowIndex] = Convert.ToInt16(DGV_Cell.Value);
                    else if (e.ColumnIndex == 3)
                        Master.m_RackMaster_SendWordMap[e.RowIndex] = (short)Convert.ToChar(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string hex = (string)DGV_Cell.Value;
                        Master.m_Port_RecvWordMap[e.RowIndex] = Convert.ToInt16(hex.Replace("0x", string.Empty), 16);
                    }
                    else if (e.ColumnIndex == 2)
                        Master.m_Port_RecvWordMap[e.RowIndex] = Convert.ToInt16(DGV_Cell.Value);
                    else if (e.ColumnIndex == 3)
                        Master.m_Port_RecvWordMap[e.RowIndex] = (short)Convert.ToChar(DGV_Cell.Value);
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    if (e.ColumnIndex == 1)
                    {
                        string hex = (string)DGV_Cell.Value;
                        Master.m_Port_SendWordMap[e.RowIndex] = Convert.ToInt16(hex.Replace("0x", string.Empty), 16);
                    }
                    else if (e.ColumnIndex == 2)
                        Master.m_Port_SendWordMap[e.RowIndex] = Convert.ToInt16(DGV_Cell.Value);
                    else if (e.ColumnIndex == 3)
                        Master.m_Port_SendWordMap[e.RowIndex] = (short)Convert.ToChar(DGV_Cell.Value);
                }
            }
            catch {
                DGV_Cell.Value = string.Empty;
            }
        }

        private void btn_SendBitMap_Click(object sender, EventArgs e)
        {
            try
            {
                int StartAddr = Convert.ToInt32(tbx_BitMapStartAddr.Text);
                int Size = Convert.ToInt32(tbx_BitMapLength.Text);

                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    MessageBox.Show("This Type CIM -> Master, Do not send");
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    if (Master.m_CIM.IsConnected())
                        Master.m_CIM.Send_Master_2_CIM_Bit_Data((Equipment.CIM.CIM.SendBitMapIndex)StartAddr, Size);
                    else
                        MessageBox.Show("Not Connected");
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    if(Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.ReceiveBitMap, StartAddr).IsConnected())
                        Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.ReceiveBitMap, StartAddr).Send_CIM_2_RackMaster_Bit_Data((Equipment.RackMaster.RackMaster.ReceiveBitMapIndex)StartAddr, Size);
                    else
                        MessageBox.Show("Not Connected");
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    //if (Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.SendBitMap, StartAddr).IsConnected())
                    //    Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.SendBitMap, StartAddr).Send_RackMaster_2_CIM_Bit_Data((Equipment.RackMaster.RackMaster.SendBitMapIndex)StartAddr, Size);
                    //else
                    //    MessageBox.Show("Not Connected");
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    MessageBox.Show("This Type CIM -> Port, Do not send");
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    if (Master.m_CIM.IsConnected())
                        Master.ConvertMemoryAddressToPortPt(Master.MapType.SendBitMap, StartAddr).Send_Port_2_CIM_Bit_Data((Equipment.Port.Port.SendBitMapIndex)StartAddr, Size);
                    else
                        MessageBox.Show("Not Connected");
                }
            }
            catch
            {

            }
        }

        private void btn_SendWordMap_Click(object sender, EventArgs e)
        {
            try
            {
                int StartAddr = Convert.ToInt32(tbx_WordMapStartAddr.Text);
                int Size = Convert.ToInt32(tbx_WordMapLength.Text);

                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    MessageBox.Show("This Type CIM -> Master, Do not send");
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    if(Master.m_CIM.IsConnected())
                        Master.m_CIM.Send_Master_2_CIM_Word_Data((Equipment.CIM.CIM.SendWordMapIndex)StartAddr, Size);
                    else
                        MessageBox.Show("Not Connected");
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    if(Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.ReceiveWordMap, StartAddr).IsConnected())
                        Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.ReceiveWordMap, StartAddr).Send_CIM_2_RackMaster_Word_Data((Equipment.RackMaster.RackMaster.ReceiveWordMapIndex)StartAddr, Size);
                    else
                        MessageBox.Show("Not Connected");
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    //if(Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.SendWordMap, StartAddr).IsConnected())
                    //    Master.ConvertMemoryAddressToRackMasterPt(Master.MapType.SendWordMap, StartAddr).Send_RackMaster_2_CIM_Word_Data((Equipment.RackMaster.RackMaster.SendWordMapIndex)StartAddr, Size);
                    //else
                    //    MessageBox.Show("Not Connected");
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    MessageBox.Show("This Type CIM -> Port, Do not send");
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    if(Master.m_CIM.IsConnected())
                        Master.ConvertMemoryAddressToPortPt(Master.MapType.SendWordMap, StartAddr).Send_Port_2_CIM_Word_Data((Equipment.Port.Port.SendWordMapIndex)StartAddr, Size);
                    else
                        MessageBox.Show("Not Connected");
                }
            }
            catch
            {

            }
        }
    }
}
