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
using Master.ManagedFile;
using System.Net;
using Master.Equipment.Port.TagReader;

namespace Master.SubForm
{
    public partial class Frm_EquipNetworkSettings : Form
    {
        enum CPSColumnIndex
        {
            ServerPort,
            ByteSize
        }
        enum CIMColumnIndex
        {
            ServerPort,
            RecvBitMapStartAddr,
            RecvBitMapSize,
            SendBitMapStartAddr,
            SendBitMapSize,
            RecvWordMapStartAddr,
            RecvWordMapSize,
            SendWordMapStartAddr,
            SendWordMapSize
        }
        enum RackMasterColumnIndex
        {
            ID,
            ServerIP,
            ServerPort,
            RecvBitMapStartAddr,
            RecvBitMapSize,
            SendBitMapStartAddr,
            SendBitMapSize,
            RecvWordMapStartAddr,
            RecvWordMapSize,
            SendWordMapStartAddr,
            SendWordMapSize
        }
        enum PortColumnIndex
        {
            ID,
            PortType,
            RecvBitMapStartAddr,
            RecvBitMapSize,
            SendBitMapStartAddr,
            SendBitMapSize,
            RecvWordMapStartAddr,
            RecvWordMapSize,
            SendWordMapStartAddr,
            SendWordMapSize,
            TagReaderType,
            ReaderServerIP,
            ReaderServerPort,
        }

        public Frm_EquipNetworkSettings()
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

            LoadEquipParam();

            LaguageCheck();

            ButtonFunc.SetText(btn_Save, SynusLangPack.GetLanguage("Btn_Save"));
            ButtonFunc.SetText(btn_Close, SynusLangPack.GetLanguage("Btn_Cancel"));
        }
        private void LoadEquipParam()
        {
            DGV_CIM.Rows.Add(new object[] {
                    EquipNetworkParam.m_CIMNetworkParam.ServerPort         != -1 ? EquipNetworkParam.m_CIMNetworkParam.ServerPort.ToString() : string.Empty ,
                    EquipNetworkParam.m_CIMNetworkParam.RecvBitMapStartAddr     != -1 ? EquipNetworkParam.m_CIMNetworkParam.RecvBitMapStartAddr.ToString() : string.Empty,
                    EquipNetworkParam.m_CIMNetworkParam.RecvBitMapSize     != -1 ? EquipNetworkParam.m_CIMNetworkParam.RecvBitMapSize.ToString() : string.Empty,
                    EquipNetworkParam.m_CIMNetworkParam.SendBitMapStartAddr     != -1 ? EquipNetworkParam.m_CIMNetworkParam.SendBitMapStartAddr.ToString() : string.Empty,
                    EquipNetworkParam.m_CIMNetworkParam.SendBitMapSize     != -1 ? EquipNetworkParam.m_CIMNetworkParam.SendBitMapSize.ToString() : string.Empty,
                    EquipNetworkParam.m_CIMNetworkParam.RecvWordMapStartAddr    != -1 ? EquipNetworkParam.m_CIMNetworkParam.RecvWordMapStartAddr.ToString() : string.Empty,
                    EquipNetworkParam.m_CIMNetworkParam.RecvWordMapSize    != -1 ? EquipNetworkParam.m_CIMNetworkParam.RecvWordMapSize.ToString() : string.Empty,
                    EquipNetworkParam.m_CIMNetworkParam.SendWordMapStartAddr    != -1 ? EquipNetworkParam.m_CIMNetworkParam.SendWordMapStartAddr.ToString() : string.Empty,
                    EquipNetworkParam.m_CIMNetworkParam.SendWordMapSize    != -1 ? EquipNetworkParam.m_CIMNetworkParam.SendWordMapSize.ToString() : string.Empty
                });

            DGV_CPS.Rows.Add(new object[] {
                    EquipNetworkParam.m_CPSNetworkParam.ServerPort         != -1 ? EquipNetworkParam.m_CPSNetworkParam.ServerPort.ToString() : string.Empty,
                    EquipNetworkParam.m_CPSNetworkParam.PacketSize     != -1 ? EquipNetworkParam.m_CPSNetworkParam.PacketSize.ToString() : string.Empty
                });

            foreach (var RackMaster in EquipNetworkParam.m_RackMasterNetworkParams)
            {
                DGV_RackMasterList.Rows.Add(new object[] {
                    RackMaster.Value.ID,
                    RackMaster.Value.ServerIP,
                    RackMaster.Value.ServerPort         != -1 ? RackMaster.Value.ServerPort.ToString() : string.Empty,
                    RackMaster.Value.RecvBitMapStartAddr     != -1 ? RackMaster.Value.RecvBitMapStartAddr.ToString() : string.Empty,
                    RackMaster.Value.RecvBitMapSize     != -1 ? RackMaster.Value.RecvBitMapSize.ToString() : string.Empty,
                    RackMaster.Value.SendBitMapStartAddr     != -1 ? RackMaster.Value.SendBitMapStartAddr.ToString() : string.Empty,
                    RackMaster.Value.SendBitMapSize     != -1 ? RackMaster.Value.SendBitMapSize.ToString() : string.Empty,
                    RackMaster.Value.RecvWordMapStartAddr    != -1 ? RackMaster.Value.RecvWordMapStartAddr.ToString() : string.Empty,
                    RackMaster.Value.RecvWordMapSize    != -1 ? RackMaster.Value.RecvWordMapSize.ToString() : string.Empty,
                    RackMaster.Value.SendWordMapStartAddr    != -1 ? RackMaster.Value.SendWordMapStartAddr.ToString() : string.Empty,
                    RackMaster.Value.SendWordMapSize    != -1 ? RackMaster.Value.SendWordMapSize.ToString() : string.Empty
                });
            }

            foreach(var Port in EquipNetworkParam.m_PortNetworkParams)
            {
                DGV_PortList.Rows.Add(new object[]
                {
                    Port.Value.ID,
                    null,
                    Port.Value.RecvBitMapStartAddr       != -1 ? Port.Value.RecvBitMapStartAddr.ToString() : string.Empty,
                    Port.Value.RecvBitMapSize       != -1 ? Port.Value.RecvBitMapSize.ToString() : string.Empty,
                    Port.Value.SendBitMapStartAddr       != -1 ? Port.Value.SendBitMapStartAddr.ToString() : string.Empty,
                    Port.Value.SendBitMapSize       != -1 ? Port.Value.SendBitMapSize.ToString() : string.Empty,
                    Port.Value.RecvWordMapStartAddr      != -1 ? Port.Value.RecvWordMapStartAddr.ToString() : string.Empty,
                    Port.Value.RecvWordMapSize      != -1 ? Port.Value.RecvWordMapSize.ToString() : string.Empty,
                    Port.Value.SendWordMapStartAddr      != -1 ? Port.Value.SendWordMapStartAddr.ToString() : string.Empty,
                    Port.Value.SendWordMapSize      != -1 ? Port.Value.SendWordMapSize.ToString() : string.Empty,
                    null,
                    Port.Value.TagEquipServerIP,
                    Port.Value.TagEquipServerPort         != -1 ? Port.Value.TagEquipServerPort.ToString() : string.Empty
                });

                DataGridViewComboBoxCell cbxCell_PortType = new DataGridViewComboBoxCell();
                cbxCell_PortType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                for(int nCount = 0; nCount < Enum.GetNames(typeof(Equipment.Port.Port.PortType)).Length; nCount++)
                    cbxCell_PortType.Items.Add(((Equipment.Port.Port.PortType)nCount).ToString());

                DataGridViewComboBoxCell cbxCell_TagType = new DataGridViewComboBoxCell();
                cbxCell_TagType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                for (int nCount = 0; nCount < Enum.GetNames(typeof(TagReaderType)).Length; nCount++)
                    cbxCell_TagType.Items.Add(((TagReaderType)nCount).ToString());

                cbxCell_PortType.Value = Port.Value.ePortType.ToString();
                cbxCell_TagType.Value = Port.Value.eTagReaderType.ToString();

                int RowCount = DGV_PortList.Rows.Count;
                DGV_PortList.Rows[RowCount - 1].Cells[(int)PortColumnIndex.PortType] = cbxCell_PortType;
                DGV_PortList.Rows[RowCount - 1].Cells[(int)PortColumnIndex.TagReaderType] = cbxCell_TagType;
            }
        }

        private void LaguageCheck()
        {
            FormFunc.SetText(this, SynusLangPack.GetLanguage("Frm_EquipNetworkSettings_FormTitle"));
            ButtonFunc.SetText(btn_RackMasterAdd, SynusLangPack.GetLanguage("Btn_Add"));
            ButtonFunc.SetText(btn_PortAdd, SynusLangPack.GetLanguage("Btn_Add"));
            ButtonFunc.SetText(btn_RackMasterDelete, SynusLangPack.GetLanguage("Btn_Delete"));
            ButtonFunc.SetText(btn_PortDelete, SynusLangPack.GetLanguage("Btn_Delete"));
            ButtonFunc.SetText(btn_Save, SynusLangPack.GetLanguage("Btn_Apply"));
            ButtonFunc.SetText(btn_Close, SynusLangPack.GetLanguage("Btn_Cancel"));

            GroupBoxFunc.SetText(groupBox_CIMSettings, SynusLangPack.GetLanguage("GorupBox_CIMSettings"));
            GroupBoxFunc.SetText(groupBox_CPSSettings, SynusLangPack.GetLanguage("GorupBox_CPSSettings"));
            GroupBoxFunc.SetText(groupBox_RackMasterSettings, SynusLangPack.GetLanguage("GorupBox_RackMasterSettings"));
            GroupBoxFunc.SetText(groupBox_PortSettings, SynusLangPack.GetLanguage("GorupBox_PortSettings"));
        }
        private bool IsCIMParamValid()
        {
            for (int nRowCount = 0; nRowCount < DGV_CIM.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < DGV_CIM.ColumnCount; nColumnCount++)
                {
                    CIMColumnIndex eCIMColumnIndex = (CIMColumnIndex)nColumnCount;
                    DataGridViewCell DGVCell = DGV_CIM.Rows[nRowCount].Cells[nColumnCount];
                    DGV_CIM.CurrentCell = DGVCell;
                    try
                    {
                        switch (eCIMColumnIndex)
                        {
                            case CIMColumnIndex.ServerPort:
                                int ServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);

                                if (ServerPort < 0)
                                    continue;

                                if (!EquipNetworkParam.IsValidPort(ServerPort))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_PortNumberRangeError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                break;
                            case CIMColumnIndex.RecvBitMapStartAddr:
                            case CIMColumnIndex.SendBitMapStartAddr:
                            case CIMColumnIndex.RecvWordMapStartAddr:
                            case CIMColumnIndex.SendWordMapStartAddr:
                                {
                                    int StartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    if (!EquipNetworkParam.IsValidStartAddr(StartAddr))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_MemoryMapStartAddrRangeError") + $"(0 <= StartAddr)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case CIMColumnIndex.RecvBitMapSize:
                                {
                                    int BitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int BitMapMinSize = Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveBitMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidBitMapSize(BitMapSize, BitMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_BitMapSizeRangeError") + $"({BitMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case CIMColumnIndex.SendBitMapSize:
                                {
                                    int BitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int BitMapMinSize = Enum.GetValues(typeof(Equipment.CIM.CIM.SendBitMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidBitMapSize(BitMapSize, BitMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_BitMapSizeRangeError") + $"({BitMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case CIMColumnIndex.RecvWordMapSize:
                                {
                                    int WordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int WordMapMinSize = Enum.GetValues(typeof(Equipment.CIM.CIM.ReceiveWordMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidWordMapSize(WordMapSize, WordMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_WordMapSizeRangeError") + $"({WordMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case CIMColumnIndex.SendWordMapSize:
                                {
                                    int WordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int WordMapMinSize = Enum.GetValues(typeof(Equipment.CIM.CIM.SendWordMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidWordMapSize(WordMapSize, WordMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_WordMapSizeRangeError") + $"({WordMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMsg.AddExceptionLog(ex, $"CIM Network Parameter: {eCIMColumnIndex} / Value: {DGVCell.Value}");
                        return false;
                    }
                }
            }

            DGV_CIM.CurrentCell = null;
            return true;
        }
        private bool IsCPSParamValid()
        {
            for (int nRowCount = 0; nRowCount < DGV_CPS.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < DGV_CPS.ColumnCount; nColumnCount++)
                {
                    CPSColumnIndex eCPSColumnIndex = (CPSColumnIndex)nColumnCount;

                    if(!Enum.IsDefined(typeof(CPSColumnIndex), eCPSColumnIndex))
                        continue;

                    DataGridViewCell DGVCell = DGV_CPS.Rows[nRowCount].Cells[nColumnCount];
                    DGV_CPS.CurrentCell = DGVCell;
                    try
                    {
                        switch (eCPSColumnIndex)
                        {
                            case CPSColumnIndex.ServerPort:
                                int ServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);

                                if (ServerPort < 0)
                                    continue;

                                if (!EquipNetworkParam.IsValidPort(ServerPort))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_PortNumberRangeError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                break;
                            case CPSColumnIndex.ByteSize:
                                {
                                    int ByteSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int ByteMinSize = Enum.GetValues(typeof(Equipment.CPS.CPS.PacketStruct)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidBitMapSize(ByteSize, ByteMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_ByteSizeError") + $"({ByteMinSize} <= ByteSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMsg.AddExceptionLog(ex, $"CPS Network Parameter: {eCPSColumnIndex} / Value: {DGVCell.Value}");
                        return false;
                    }
                }
            }

            DGV_CPS.CurrentCell = null;
            return true;
        }
        private bool IsRackMasterParamValid()
        {
            List<string> IDDupleCheck = new List<string>();

            for (int nRowCount = 0; nRowCount < DGV_RackMasterList.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < DGV_RackMasterList.ColumnCount; nColumnCount++)
                {
                    RackMasterColumnIndex eRackMasterColumnIndex = (RackMasterColumnIndex)nColumnCount;
                    DataGridViewCell DGVCell = DGV_RackMasterList.Rows[nRowCount].Cells[nColumnCount];
                    DGV_RackMasterList.CurrentCell = DGVCell;
                    try
                    {
                        switch (eRackMasterColumnIndex)
                        {
                            case RackMasterColumnIndex.ID:
                                string ID = Convert.ToString(DGVCell.Value ?? string.Empty);
                                if(!EquipNetworkParam.IsRMValidID(ID))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidIDError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                if(!IDDupleCheck.Contains(ID))
                                    IDDupleCheck.Add(ID);
                                else
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_IDDupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                break;
                            case RackMasterColumnIndex.ServerPort:
                                int ServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);

                                if (ServerPort < 0)
                                    continue;

                                if (!EquipNetworkParam.IsValidPort(ServerPort))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_PortNumberRangeError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                break;
                            case RackMasterColumnIndex.ServerIP:
                                if (string.IsNullOrEmpty((string)DGVCell.Value))
                                    break;
                                
                                string ServerIP = Convert.ToString(DGVCell.Value ?? string.Empty);

                                if (!EquipNetworkParam.IsValidIP(ServerIP))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidIPAddressError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }

                                try
                                {
                                    IPAddress IpAddr = IPAddress.Parse(ServerIP);
                                }
                                catch
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidIPAddressError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                break;
                            case RackMasterColumnIndex.RecvBitMapStartAddr:
                            case RackMasterColumnIndex.SendBitMapStartAddr:
                            case RackMasterColumnIndex.RecvWordMapStartAddr:
                            case RackMasterColumnIndex.SendWordMapStartAddr:
                                {
                                    int StartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    if (!EquipNetworkParam.IsValidStartAddr(StartAddr))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_MemoryMapStartAddrRangeError") + $"(0 <= StartAddr)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case RackMasterColumnIndex.RecvBitMapSize:
                                {
                                    int BitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int BitMapMinSize = Enum.GetValues(typeof(Equipment.RackMaster.RackMaster.ReceiveBitMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidBitMapSize(BitMapSize, BitMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_BitMapSizeRangeError") + $"({BitMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case RackMasterColumnIndex.SendBitMapSize:
                                {
                                    int BitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int BitMapMinSize = Enum.GetValues(typeof(Equipment.RackMaster.RackMaster.SendBitMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidBitMapSize(BitMapSize, BitMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_BitMapSizeRangeError") + $"({BitMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case RackMasterColumnIndex.RecvWordMapSize:
                                {
                                    int WordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int WordMapMinSize = Enum.GetValues(typeof(Equipment.RackMaster.RackMaster.ReceiveWordMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidWordMapSize(WordMapSize, WordMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_WordMapSizeRangeError") + $"({WordMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case RackMasterColumnIndex.SendWordMapSize:
                                {
                                    int WordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int WordMapMinSize = Enum.GetValues(typeof(Equipment.RackMaster.RackMaster.SendWordMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidWordMapSize(WordMapSize, WordMapMinSize))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_WordMapSizeRangeError") + $"({WordMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMsg.AddExceptionLog(ex, $"RackMaster Network Parameter: {eRackMasterColumnIndex} / Value: {DGVCell.Value}");
                        return false;
                    }
                }
            }

            DGV_RackMasterList.CurrentCell = null;
            return true;
        }
        private bool IsPortParamValid()
        {
            List<string> IDDupleCheck = new List<string>();

            for (int nRowCount = 0; nRowCount < DGV_PortList.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < DGV_PortList.ColumnCount; nColumnCount++)
                {
                    PortColumnIndex ePortColumnIndex = (PortColumnIndex)nColumnCount;
                    DataGridViewCell DGVCell = DGV_PortList.Rows[nRowCount].Cells[nColumnCount];
                    DGV_PortList.CurrentCell = DGVCell;

                    try
                    {
                        switch (ePortColumnIndex)
                        {
                            case PortColumnIndex.ID:
                                string ID = Convert.ToString(DGVCell.Value ?? string.Empty);
                                if (!EquipNetworkParam.IsPortValidID(ID))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidIDError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                if (!IDDupleCheck.Contains(ID))
                                    IDDupleCheck.Add(ID);
                                else
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_IDDupleError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                break;
                            case PortColumnIndex.PortType:
                                Equipment.Port.Port.PortType ePortType = (Equipment.Port.Port.PortType)Enum.Parse(typeof(Equipment.Port.Port.PortType), Convert.ToString(DGVCell.Value ?? Equipment.Port.Port.PortType.MGV.ToString()));
                                break;

                            case PortColumnIndex.RecvBitMapStartAddr:
                            case PortColumnIndex.SendBitMapStartAddr:
                            case PortColumnIndex.RecvWordMapStartAddr:
                            case PortColumnIndex.SendWordMapStartAddr:
                                {
                                    int StartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    if (!EquipNetworkParam.IsValidStartAddr(StartAddr))
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_MemoryMapStartAddrRangeError") + $"(0 <= StartAddr)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case PortColumnIndex.RecvBitMapSize:
                                {
                                    int BitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int BitMapMinSize = Enum.GetValues(typeof(Equipment.Port.Port.ReceiveBitMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidBitMapSize(BitMapSize, BitMapMinSize) && BitMapSize != 0)
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_BitMapSizeRangeError") + $"({BitMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case PortColumnIndex.SendBitMapSize:
                                {
                                    int BitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int BitMapMinSize = Enum.GetValues(typeof(Equipment.Port.Port.SendBitMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidBitMapSize(BitMapSize, BitMapMinSize) && BitMapSize != 0)
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_BitMapSizeRangeError") + $"({BitMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case PortColumnIndex.RecvWordMapSize:
                                {
                                    int WordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int WordMapMinSize = Enum.GetValues(typeof(Equipment.Port.Port.ReceiveWordMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidWordMapSize(WordMapSize, WordMapMinSize) && WordMapSize != 0)
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_WordMapSizeRangeError") + $"({WordMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case PortColumnIndex.SendWordMapSize:
                                {
                                    int WordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                    int WordMapMinSize = Enum.GetValues(typeof(Equipment.Port.Port.SendWordMapIndex)).Cast<int>().Max();
                                    if (!EquipNetworkParam.IsValidWordMapSize(WordMapSize, WordMapMinSize) && WordMapSize != 0)
                                    {
                                        MessageBox.Show(SynusLangPack.GetLanguage("Message_WordMapSizeRangeError") + $"({WordMapMinSize} <= MapSize)", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return false;
                                    }
                                }
                                break;
                            case PortColumnIndex.TagReaderType:
                                TagReaderType eTagReaderType = (TagReaderType)Enum.Parse(typeof(TagReaderType), Convert.ToString(DGVCell.Value ?? TagReaderType.None.ToString()));
                                break;
                            case PortColumnIndex.ReaderServerPort:
                                int ServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);

                                if (ServerPort < 0)
                                    continue;

                                if (!EquipNetworkParam.IsValidPort(ServerPort))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_PortNumberRangeError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }
                                break;
                            case PortColumnIndex.ReaderServerIP:
                                if (string.IsNullOrEmpty((string)DGVCell.Value))
                                    break;

                                string ServerIP = Convert.ToString(DGVCell.Value ?? string.Empty);

                                if (!EquipNetworkParam.IsValidIP(ServerIP))
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidIPAddressError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }

                                try
                                {
                                    IPAddress IpAddr = IPAddress.Parse(ServerIP);
                                }
                                catch
                                {
                                    MessageBox.Show(SynusLangPack.GetLanguage("Message_InvalidIPAddressError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return false;
                                }

                                break;
                        }
                    }
                    catch(Exception ex) 
                    {
                        LogMsg.AddExceptionLog(ex, $"Port Network Parameter: {ePortColumnIndex} / Value: {DGVCell.Value}");
                        return false; 
                    }
                }
            }

            DGV_PortList.CurrentCell = null;
            return true;
        }
        private void CIMParamApply()
        {
            for (int nRowCount = 0; nRowCount < DGV_CIM.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < DGV_CIM.ColumnCount; nColumnCount++)
                {
                    CIMColumnIndex eCIMColumnIndex = (CIMColumnIndex)nColumnCount;
                    DataGridViewCell DGVCell = DGV_CIM.Rows[nRowCount].Cells[nColumnCount];

                    try
                    {
                        switch (eCIMColumnIndex)
                        {
                            case CIMColumnIndex.ServerPort:
                                EquipNetworkParam.m_CIMNetworkParam.ServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);
                                break;
                            case CIMColumnIndex.RecvBitMapStartAddr:
                                EquipNetworkParam.m_CIMNetworkParam.RecvBitMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case CIMColumnIndex.RecvBitMapSize:
                                EquipNetworkParam.m_CIMNetworkParam.RecvBitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case CIMColumnIndex.SendBitMapStartAddr:
                                EquipNetworkParam.m_CIMNetworkParam.SendBitMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case CIMColumnIndex.SendBitMapSize:
                                EquipNetworkParam.m_CIMNetworkParam.SendBitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case CIMColumnIndex.RecvWordMapStartAddr:
                                EquipNetworkParam.m_CIMNetworkParam.RecvWordMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case CIMColumnIndex.RecvWordMapSize:
                                EquipNetworkParam.m_CIMNetworkParam.RecvWordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case CIMColumnIndex.SendWordMapStartAddr:
                                EquipNetworkParam.m_CIMNetworkParam.SendWordMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case CIMColumnIndex.SendWordMapSize:
                                EquipNetworkParam.m_CIMNetworkParam.SendWordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                        }
                    }
                    catch { }
                }
            }
        }

        private void CPSParamApply()
        {
            for (int nRowCount = 0; nRowCount < DGV_CPS.Rows.Count; nRowCount++)
            {
                for (int nColumnCount = 0; nColumnCount < DGV_CPS.ColumnCount; nColumnCount++)
                {
                    CPSColumnIndex eCPSColumnIndex = (CPSColumnIndex)nColumnCount;

                    if (!Enum.IsDefined(typeof(CPSColumnIndex), eCPSColumnIndex))
                        continue;

                    DataGridViewCell DGVCell = DGV_CPS.Rows[nRowCount].Cells[nColumnCount];

                    try
                    {
                        switch (eCPSColumnIndex)
                        {
                            case CPSColumnIndex.ServerPort:
                                EquipNetworkParam.m_CPSNetworkParam.ServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);
                                break;
                            case CPSColumnIndex.ByteSize:
                                EquipNetworkParam.m_CPSNetworkParam.PacketSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                        }
                    }
                    catch { }
                }
            }
        }
        private void RackMasterApply()
        {
            EquipNetworkParam.m_RackMasterNetworkParams.Clear();
            for (int nRowCount = 0; nRowCount < DGV_RackMasterList.Rows.Count; nRowCount++)
            {
                EquipNetworkParam.RackMasterNetworkParam rackMasterParameter = new EquipNetworkParam.RackMasterNetworkParam();
                for (int nColumnCount = 0; nColumnCount < DGV_RackMasterList.ColumnCount; nColumnCount++)
                {
                    RackMasterColumnIndex eRackMasterColumnIndex = (RackMasterColumnIndex)nColumnCount;
                    DataGridViewCell DGVCell = DGV_RackMasterList.Rows[nRowCount].Cells[nColumnCount];

                    try
                    {
                        switch (eRackMasterColumnIndex)
                        {
                            case RackMasterColumnIndex.ID:
                                rackMasterParameter.ID = Convert.ToString(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.ServerPort:
                                rackMasterParameter.ServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);
                                break;
                            case RackMasterColumnIndex.ServerIP:
                                rackMasterParameter.ServerIP = Convert.ToString(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.RecvBitMapStartAddr:
                                rackMasterParameter.RecvBitMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.RecvBitMapSize:
                                rackMasterParameter.RecvBitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.SendBitMapStartAddr:
                                rackMasterParameter.SendBitMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.SendBitMapSize:
                                rackMasterParameter.SendBitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.RecvWordMapStartAddr:
                                rackMasterParameter.RecvWordMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.RecvWordMapSize:
                                rackMasterParameter.RecvWordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.SendWordMapStartAddr:
                                rackMasterParameter.SendWordMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case RackMasterColumnIndex.SendWordMapSize:
                                rackMasterParameter.SendWordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                        }
                    }
                    catch { }
                }

                EquipNetworkParam.m_RackMasterNetworkParams.Add(nRowCount, rackMasterParameter);
            }
        }

        private void PortApply()
        {
            EquipNetworkParam.m_PortNetworkParams.Clear();
            for (int nRowCount = 0; nRowCount < DGV_PortList.Rows.Count; nRowCount++)
            {
                EquipNetworkParam.PortNetworkParam portParameter = new EquipNetworkParam.PortNetworkParam();
                for (int nColumnCount = 0; nColumnCount < DGV_PortList.ColumnCount; nColumnCount++)
                {
                    PortColumnIndex ePortColumnIndex = (PortColumnIndex)nColumnCount;
                    DataGridViewCell DGVCell = DGV_PortList.Rows[nRowCount].Cells[nColumnCount];

                    try
                    {
                        switch (ePortColumnIndex)
                        {
                            case PortColumnIndex.ID:
                                portParameter.ID = Convert.ToString(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.PortType:
                                portParameter.ePortType = (Equipment.Port.Port.PortType)Enum.Parse(typeof(Equipment.Port.Port.PortType), Convert.ToString(DGVCell.Value ?? Equipment.Port.Port.PortType.MGV.ToString()));
                                break;
                            case PortColumnIndex.RecvBitMapStartAddr:
                                portParameter.RecvBitMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.RecvBitMapSize:
                                portParameter.RecvBitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.SendBitMapStartAddr:
                                portParameter.SendBitMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.SendBitMapSize:
                                portParameter.SendBitMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.RecvWordMapStartAddr:
                                portParameter.RecvWordMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.RecvWordMapSize:
                                portParameter.RecvWordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.SendWordMapStartAddr:
                                portParameter.SendWordMapStartAddr = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.SendWordMapSize:
                                portParameter.SendWordMapSize = Convert.ToInt32(DGVCell.Value ?? string.Empty);
                                break;
                            case PortColumnIndex.TagReaderType:
                                portParameter.eTagReaderType = (TagReaderType)Enum.Parse(typeof(TagReaderType), Convert.ToString(DGVCell.Value ?? TagReaderType.None.ToString()));
                                break;
                            case PortColumnIndex.ReaderServerPort:
                                portParameter.TagEquipServerPort = Convert.ToInt32(string.IsNullOrEmpty((string)DGVCell.Value) ? -1 : DGVCell.Value);
                                break;
                            case PortColumnIndex.ReaderServerIP:
                                portParameter.TagEquipServerIP = Convert.ToString(DGVCell.Value ?? string.Empty);
                                break;
                        }
                    }
                    catch { }
                }

                EquipNetworkParam.m_PortNetworkParams.Add(nRowCount, portParameter);
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            GlobalForm.Frm_AcceptSave frm_AcceptSave = new GlobalForm.Frm_AcceptSave(GlobalForm.Frm_AcceptSave.SaveSection.EquipmentNetworkInfo, string.Empty);
            frm_AcceptSave.Location = this.Location;
            frm_AcceptSave.StartPosition = FormStartPosition.CenterScreen;
            DialogResult result = frm_AcceptSave.ShowDialog();

            if (result != DialogResult.OK || this.IsDisposed)
                return;


            if (!IsCIMParamValid())
                return;

            if (!IsCPSParamValid())
                return;

            if (!IsRackMasterParamValid())
                return;

            if (!IsPortParamValid())
                return;

            CIMParamApply();
            CPSParamApply();
            RackMasterApply();
            PortApply();

            this.DialogResult = DialogResult.Yes;
        }
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btn_RackMasterAdd_Click(object sender, EventArgs e)
        {
            if(DGV_RackMasterList.Rows.Count >= 2)
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_RackMaster_Count_Add_Error"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DGV_RackMasterList.Rows.Add(new object[] {
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            });
        }
        private void btn_RackMasterDelete_Click(object sender, EventArgs e)
        {
            int SelectedIndex = DGV_RackMasterList.CurrentCell?.RowIndex ?? -1;

            if (SelectedIndex == -1 || SelectedIndex >= DGV_RackMasterList.Rows.Count)
                return;

            DGV_RackMasterList.Rows.RemoveAt(SelectedIndex);
        }

        private void btn_PortAdd_Click(object sender, EventArgs e)
        {
            DataGridViewComboBoxCell cbxCell_PortType = new DataGridViewComboBoxCell();
            cbxCell_PortType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            for (int nCount = 0; nCount < Enum.GetNames(typeof(Equipment.Port.Port.PortType)).Length; nCount++)
                cbxCell_PortType.Items.Add(((Equipment.Port.Port.PortType)nCount).ToString());
            cbxCell_PortType.Value = Equipment.Port.Port.PortType.MGV.ToString();

            DataGridViewComboBoxCell cbxCell_TagType = new DataGridViewComboBoxCell();
            cbxCell_TagType.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
            for (int nCount = 0; nCount < Enum.GetNames(typeof(TagReaderType)).Length; nCount++)
                cbxCell_TagType.Items.Add(((TagReaderType)nCount).ToString());
            cbxCell_TagType.Value = TagReaderType.None.ToString();



            DGV_PortList.Rows.Add(new object[]
            {
                string.Empty,
                null,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty,
                null,
                string.Empty,
                string.Empty
            });
            int RowCount = DGV_PortList.Rows.Count;
            DGV_PortList.Rows[RowCount - 1].Cells[(int)PortColumnIndex.PortType] = cbxCell_PortType;
            DGV_PortList.Rows[RowCount - 1].Cells[(int)PortColumnIndex.TagReaderType] = cbxCell_TagType;
        }
        private void btn_PortDelete_Click(object sender, EventArgs e)
        {
            int SelectedIndex = DGV_PortList.CurrentCell?.RowIndex ?? -1;

            if (SelectedIndex == -1 || SelectedIndex >= DGV_PortList.Rows.Count)
                return;

            DGV_PortList.Rows.RemoveAt(SelectedIndex);
        }

        private void btn_RecvBitMapStartAddr_AutoCal_Click(object sender, EventArgs e)
        {
            try
            {
                int Size = 0;
                int StartAddr = 0;
                for (int nRowCount = 0; nRowCount < DGV_PortList.Rows.Count; nRowCount++)
                {
                    DataGridViewCell DGV_SizeCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.RecvBitMapSize];
                    DataGridViewCell DGV_StartAddrCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.RecvBitMapStartAddr];

                    if (nRowCount == 0)
                    {
                        try
                        {
                            Size = !string.IsNullOrEmpty((string)DGV_SizeCell.Value) ? Convert.ToInt32(DGV_SizeCell.Value) : 0;
                        }
                        catch
                        {
                            Size = 0;
                        }
                    }

                    if (!string.IsNullOrEmpty((string)DGV_StartAddrCell.Value))
                        StartAddr = Convert.ToInt32(DGV_StartAddrCell.Value.ToString());

                    DGV_SizeCell.Value = Size.ToString();
                    DGV_StartAddrCell.Value = StartAddr.ToString();

                    StartAddr += Size;
                }

                DGV_PortList.EndEdit();
                DGV_PortList.CurrentCell = null;
            }
            catch
            {

            }
        }
        private void btn_RecvWordMapStartAddr_AutoCal_Click(object sender, EventArgs e)
        {
            try
            {
                int Size = 0;
                int StartAddr = 0;
                for (int nRowCount = 0; nRowCount < DGV_PortList.Rows.Count; nRowCount++)
                {
                    DataGridViewCell DGV_SizeCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.RecvWordMapSize];
                    DataGridViewCell DGV_StartAddrCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.RecvWordMapStartAddr];

                    if (nRowCount == 0)
                    {
                        try
                        {
                            Size = !string.IsNullOrEmpty((string)DGV_SizeCell.Value) ? Convert.ToInt32(DGV_SizeCell.Value) : 0;
                        }
                        catch
                        {
                            Size = 0;
                        }
                    }

                    if (!string.IsNullOrEmpty((string)DGV_StartAddrCell.Value))
                        StartAddr = Convert.ToInt32(DGV_StartAddrCell.Value.ToString());

                    DGV_SizeCell.Value = Size.ToString();
                    DGV_StartAddrCell.Value = StartAddr.ToString();

                    StartAddr += Size;
                }

                DGV_PortList.EndEdit();
                DGV_PortList.CurrentCell = null;
            }
            catch
            {

            }
        }
        private void btn_SendBitMapStartAddr_AutoCal_Click(object sender, EventArgs e)
        {
            try
            {
                int Size = 0;
                int StartAddr = 0;
                for (int nRowCount = 0; nRowCount < DGV_PortList.Rows.Count; nRowCount++)
                {
                    DataGridViewCell DGV_SizeCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.SendBitMapSize];
                    DataGridViewCell DGV_StartAddrCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.SendBitMapStartAddr];

                    if (nRowCount == 0)
                    {
                        try
                        {
                            Size = !string.IsNullOrEmpty((string)DGV_SizeCell.Value) ? Convert.ToInt32(DGV_SizeCell.Value) : 0;
                        }
                        catch
                        {
                            Size = 0;
                        }
                    }

                    if (!string.IsNullOrEmpty((string)DGV_StartAddrCell.Value))
                        StartAddr = Convert.ToInt32(DGV_StartAddrCell.Value.ToString());

                    DGV_SizeCell.Value = Size.ToString();
                    DGV_StartAddrCell.Value = StartAddr.ToString();

                    StartAddr += Size;
                }

                DGV_PortList.EndEdit();
                DGV_PortList.CurrentCell = null;
            }
            catch
            {

            }
        }
        private void btn_SendWordMapStartAddr_AutoCal_Click(object sender, EventArgs e)
        {
            try
            {
                int Size = 0;
                int StartAddr = 0;
                for (int nRowCount = 0; nRowCount < DGV_PortList.Rows.Count; nRowCount++)
                {
                    DataGridViewCell DGV_SizeCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.SendWordMapSize];
                    DataGridViewCell DGV_StartAddrCell = DGV_PortList.Rows[nRowCount].Cells[(int)PortColumnIndex.SendWordMapStartAddr];

                    if (nRowCount == 0)
                    {
                        try
                        {
                            Size = !string.IsNullOrEmpty((string)DGV_SizeCell.Value) ? Convert.ToInt32(DGV_SizeCell.Value) : 0;
                        }
                        catch
                        {
                            Size = 0;
                        }
                    }

                    if (!string.IsNullOrEmpty((string)DGV_StartAddrCell.Value))
                        StartAddr = Convert.ToInt32(DGV_StartAddrCell.Value.ToString());

                    DGV_SizeCell.Value = Size.ToString();
                    DGV_StartAddrCell.Value = StartAddr.ToString();

                    StartAddr += Size;
                }

                DGV_PortList.EndEdit();
                DGV_PortList.CurrentCell = null;
            }
            catch
            {

            }
        }
        private void DGV_PortList_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex < 0)
                    return;

                int ColumnIndex = e.ColumnIndex;
                PortColumnIndex ePortColumnIndex = (PortColumnIndex)ColumnIndex;

                if (ePortColumnIndex == PortColumnIndex.RecvBitMapStartAddr)
                {
                    DataGridView grid = sender as DataGridView;
                    ContextMenu cm = new ContextMenu();

                    MenuItem item1 = new MenuItem();
                    item1.Name = "Auto Calculation";
                    item1.Text = "Auto Calculation";
                    item1.Click += btn_RecvBitMapStartAddr_AutoCal_Click;
                    cm.MenuItems.Add(item1);

                    Point pt = grid.PointToClient(Control.MousePosition);
                    cm.Show(grid, pt);
                }
                else if (ePortColumnIndex == PortColumnIndex.RecvWordMapStartAddr)
                {
                    DataGridView grid = sender as DataGridView;
                    ContextMenu cm = new ContextMenu();

                    MenuItem item1 = new MenuItem();
                    item1.Name = "Auto Calculation";
                    item1.Text = "Auto Calculation";
                    item1.Click += btn_RecvWordMapStartAddr_AutoCal_Click;
                    cm.MenuItems.Add(item1);

                    Point pt = grid.PointToClient(Control.MousePosition);
                    cm.Show(grid, pt);
                }
                else if (ePortColumnIndex == PortColumnIndex.SendBitMapStartAddr)
                {
                    DataGridView grid = sender as DataGridView;
                    ContextMenu cm = new ContextMenu();

                    MenuItem item1 = new MenuItem();
                    item1.Name = "Auto Calculation";
                    item1.Text = "Auto Calculation";
                    item1.Click += btn_SendBitMapStartAddr_AutoCal_Click;
                    cm.MenuItems.Add(item1);

                    Point pt = grid.PointToClient(Control.MousePosition);
                    cm.Show(grid, pt);
                }
                else if (ePortColumnIndex == PortColumnIndex.SendWordMapStartAddr)
                {
                    DataGridView grid = sender as DataGridView;
                    ContextMenu cm = new ContextMenu();

                    MenuItem item1 = new MenuItem();
                    item1.Name = "Auto Calculation";
                    item1.Text = "Auto Calculation";
                    item1.Click += btn_SendWordMapStartAddr_AutoCal_Click;
                    cm.MenuItems.Add(item1);

                    Point pt = grid.PointToClient(Control.MousePosition);
                    cm.Show(grid, pt);
                }
            }
        }

        private void DGV_PortList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            return;
        }
    }
}
