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
    public partial class Frm_MapData : Form
    {
        public Frm_MapData()
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

            cbx_EquipType.Items.AddRange(new string[3]{ "Master", "RackMaster", "Port"});
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
                else
                    Refresh(DGV_BitMap);

                if ((string)DGV_WordMap.Tag != CurrentItemTag)
                {
                    ReLoad(DGV_WordMap);
                    DGV_WordMap.Tag = CurrentItemTag;
                }
                else
                    Refresh(DGV_WordMap);
            }
            catch { }
        }

        private void ReLoad(DataGridView DGV)
        {
            DGV.Rows.Clear();

            if(DGV == DGV_BitMap)
            {
                if(cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    for(int nCount = 0; nCount < Master.m_CIM_RecvBitMap.Length; nCount++)
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
            else if(DGV == DGV_WordMap)
            {
                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_RecvWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.CIM.CIM.ReceiveWordMapIndex)nCount}", $"0x{Master.m_CIM_RecvWordMap[nCount].ToString("x4")}", string.Empty, string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_SendWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.CIM.CIM.SendWordMapIndex)nCount}", $"0x{Master.m_CIM_SendWordMap[nCount].ToString("x4")}", string.Empty, string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_RecvWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.RackMaster.RackMaster.ReceiveWordMapIndex)(nCount % (Master.m_RackMaster_RecvWordMap.Length / Master.m_RackMasters.Count))}", $"0x{Master.m_RackMaster_RecvWordMap[nCount].ToString("x4")}", string.Empty, string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_SendWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.RackMaster.RackMaster.SendWordMapIndex)(nCount % (Master.m_RackMaster_SendWordMap.Length / Master.m_RackMasters.Count))}", $"0x{Master.m_RackMaster_SendWordMap[nCount].ToString("x4")}", string.Empty, string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_Port_RecvWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.Port.Port.ReceiveWordMapIndex)(nCount % (Master.m_Port_RecvWordMap.Length / Master.m_Ports.Count))}", $"0x{Master.m_Port_RecvWordMap[nCount].ToString("x4")}", string.Empty, string.Empty });
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_Port_SendWordMap.Length; nCount++)
                    {
                        DGV.Rows.Add(new string[4] { $"{(Equipment.Port.Port.SendWordMapIndex)(nCount % (Master.m_Port_SendWordMap.Length / Master.m_Ports.Count))}", $"0x{Master.m_Port_SendWordMap[nCount].ToString("x4")}", string.Empty, string.Empty });
                    }
                }
            }
        }

        private void Refresh(DataGridView DGV)
        {
            if (DGV == DGV_BitMap)
            {
                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_RecvBitMap.Length; nCount++)
                    {
                        if ((string)DGV.Rows[nCount].Cells[1].Value != $"{Master.m_CIM_RecvBitMap[nCount]}")
                        {
                            DGV.Rows[nCount].Cells[1].Value = $"{Master.m_CIM_RecvBitMap[nCount]}";
                            DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                        }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_SendBitMap.Length; nCount++)
                    {
                        if ((string)DGV.Rows[nCount].Cells[1].Value != $"{Master.m_CIM_SendBitMap[nCount]}")
                        {
                            DGV.Rows[nCount].Cells[1].Value = $"{Master.m_CIM_SendBitMap[nCount]}";
                            DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                        }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_RecvBitMap.Length; nCount++)
                    {
                        if ((string)DGV.Rows[nCount].Cells[1].Value != $"{Master.m_RackMaster_RecvBitMap[nCount]}")
                        {
                            DGV.Rows[nCount].Cells[1].Value = $"{Master.m_RackMaster_RecvBitMap[nCount]}";
                            DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                        }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_SendBitMap.Length; nCount++)
                    {
                        if ((string)DGV.Rows[nCount].Cells[1].Value != $"{Master.m_RackMaster_SendBitMap[nCount]}")
                        {
                            DGV.Rows[nCount].Cells[1].Value = $"{Master.m_RackMaster_SendBitMap[nCount]}";
                            DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                        }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_Port_RecvBitMap.Length; nCount++)
                    {
                        if ((string)DGV.Rows[nCount].Cells[1].Value != $"{Master.m_Port_RecvBitMap[nCount]}")
                        {
                            DGV.Rows[nCount].Cells[1].Value = $"{Master.m_Port_RecvBitMap[nCount]}";
                            DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                        }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_Port_SendBitMap.Length; nCount++)
                    {
                        if ((string)DGV.Rows[nCount].Cells[1].Value != $"{Master.m_Port_SendBitMap[nCount]}")
                        {
                            DGV.Rows[nCount].Cells[1].Value = $"{Master.m_Port_SendBitMap[nCount]}";
                            DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                        }
                    }
                }
            }
            else if (DGV == DGV_WordMap)
            {
                if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_RecvWordMap.Length; nCount++)
                    {
                        try
                        {
                            if ((string)DGV.Rows[nCount].Cells[1].Value != $"0x{Master.m_CIM_RecvWordMap[nCount].ToString("x4")}")
                            {
                                DGV.Rows[nCount].Cells[1].Value = $"0x{Master.m_CIM_RecvWordMap[nCount].ToString("x4")}";
                                DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                            }
                            if ((string)DGV.Rows[nCount].Cells[2].Value != $"{Master.m_CIM_RecvWordMap[nCount]}")
                            {
                                DGV.Rows[nCount].Cells[2].Value = $"{Master.m_CIM_RecvWordMap[nCount]}";
                            }
                            if ((string)DGV.Rows[nCount].Cells[3].Value != $"{Convert.ToChar(Master.m_CIM_RecvWordMap[nCount])}")
                            {
                                DGV.Rows[nCount].Cells[3].Value = $"{Convert.ToChar(Master.m_CIM_RecvWordMap[nCount])}";
                            }
                        }
                        catch { }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 0 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_CIM_SendWordMap.Length; nCount++)
                    {
                        try
                        {
                            if ((string)DGV.Rows[nCount].Cells[1].Value != $"0x{Master.m_CIM_SendWordMap[nCount].ToString("x4")}")
                            {
                                DGV.Rows[nCount].Cells[1].Value = $"0x{Master.m_CIM_SendWordMap[nCount].ToString("x4")}";
                                DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                            }
                            if ((string)DGV.Rows[nCount].Cells[2].Value != $"{Master.m_CIM_SendWordMap[nCount]}")
                            {
                                DGV.Rows[nCount].Cells[2].Value = $"{Master.m_CIM_SendWordMap[nCount]}";
                            }
                            if ((string)DGV.Rows[nCount].Cells[3].Value != $"{Convert.ToChar(Master.m_CIM_SendWordMap[nCount])}")
                            {
                                DGV.Rows[nCount].Cells[3].Value = $"{Convert.ToChar(Master.m_CIM_SendWordMap[nCount])}";
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_RecvWordMap.Length; nCount++)
                    {
                        try
                        {
                            if ((string)DGV.Rows[nCount].Cells[1].Value != $"0x{Master.m_RackMaster_RecvWordMap[nCount].ToString("x4")}")
                            {
                                DGV.Rows[nCount].Cells[1].Value = $"0x{Master.m_RackMaster_RecvWordMap[nCount].ToString("x4")}";
                                DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                            }
                            if ((string)DGV.Rows[nCount].Cells[2].Value != $"{Master.m_RackMaster_RecvWordMap[nCount]}")
                            {
                                DGV.Rows[nCount].Cells[2].Value = $"{Master.m_RackMaster_RecvWordMap[nCount]}";
                            }
                            if ((string)DGV.Rows[nCount].Cells[3].Value != $"{Convert.ToChar(Master.m_RackMaster_RecvWordMap[nCount])}")
                            {
                                DGV.Rows[nCount].Cells[3].Value = $"{Convert.ToChar(Master.m_RackMaster_RecvWordMap[nCount])}";
                            }
                        }
                        catch
                        {

                        }
                    }
                }
                else if (cbx_EquipType.SelectedIndex == 1 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_RackMaster_SendWordMap.Length; nCount++)
                    {
                        try
                        {
                            if ((string)DGV.Rows[nCount].Cells[1].Value != $"0x{Master.m_RackMaster_SendWordMap[nCount].ToString("x4")}")
                            {
                                DGV.Rows[nCount].Cells[1].Value = $"0x{Master.m_RackMaster_SendWordMap[nCount].ToString("x4")}";
                                DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                            }
                            if ((string)DGV.Rows[nCount].Cells[2].Value != $"{Master.m_RackMaster_SendWordMap[nCount]}")
                            {
                                DGV.Rows[nCount].Cells[2].Value = $"{Master.m_RackMaster_SendWordMap[nCount]}";
                            }
                            if ((string)DGV.Rows[nCount].Cells[3].Value != $"{Convert.ToChar(Master.m_RackMaster_SendWordMap[nCount])}")
                            {
                                DGV.Rows[nCount].Cells[3].Value = $"{Convert.ToChar(Master.m_RackMaster_SendWordMap[nCount])}";
                            }
                        }
                        catch
                        {

                        }

                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 0)
                {
                    for (int nCount = 0; nCount < Master.m_Port_RecvWordMap.Length; nCount++)
                    {
                        try
                        {
                            if ((string)DGV.Rows[nCount].Cells[1].Value != $"0x{Master.m_Port_RecvWordMap[nCount].ToString("x4")}")
                            {
                                DGV.Rows[nCount].Cells[1].Value = $"0x{Master.m_Port_RecvWordMap[nCount].ToString("x4")}";
                                DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                            }
                            if ((string)DGV.Rows[nCount].Cells[2].Value != $"{Master.m_Port_RecvWordMap[nCount]}")
                            {
                                DGV.Rows[nCount].Cells[2].Value = $"{Master.m_Port_RecvWordMap[nCount]}";
                            }
                            if ((string)DGV.Rows[nCount].Cells[3].Value != $"{Convert.ToChar(Master.m_Port_RecvWordMap[nCount])}")
                            {
                                DGV.Rows[nCount].Cells[3].Value = $"{Convert.ToChar(Master.m_Port_RecvWordMap[nCount])}";
                            }
                        }
                        catch
                        {

                        }

                    }
                }
                else if (cbx_EquipType.SelectedIndex == 2 && cbx_Direction.SelectedIndex == 1)
                {
                    for (int nCount = 0; nCount < Master.m_Port_SendWordMap.Length; nCount++)
                    {
                        try
                        {
                            if ((string)DGV.Rows[nCount].Cells[1].Value != $"0x{Master.m_Port_SendWordMap[nCount].ToString("x4")}")
                            {
                                DGV.Rows[nCount].Cells[1].Value = $"0x{Master.m_Port_SendWordMap[nCount].ToString("x4")}";
                                DGV.CurrentCell = DGV.Rows[nCount].Cells[1];
                            }
                            if ((string)DGV.Rows[nCount].Cells[2].Value != $"{Master.m_Port_SendWordMap[nCount]}")
                            {
                                DGV.Rows[nCount].Cells[2].Value = $"{Master.m_Port_SendWordMap[nCount]}";
                            }
                            if ((string)DGV.Rows[nCount].Cells[3].Value != $"{Convert.ToChar(Master.m_Port_SendWordMap[nCount])}")
                            {
                                DGV.Rows[nCount].Cells[3].Value = $"{Convert.ToChar(Master.m_Port_SendWordMap[nCount])}";
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
