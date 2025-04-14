using Master.ManagedFile;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Equipment.Port;

namespace Master.GlobalForm
{
    public partial class Frm_PortSafetyMapSettings : Form
    {
        enum SafetyGridColumns
        {
            Index,
            Name,
            XPos,
            XPosUp,
            XPosDown,
            YPos,
            YPosUp,
            YPosDown
        }
        public delegate void ApplyEventHandler(Port.Port_IO_TabPage TabPage, EquipPortMotionParam.Port_SafetyImageInfo _PortSafetyImageInfo);
        public event ApplyEventHandler ApplyEvent;

        EquipPortMotionParam.Port_UIParam Port_UIParam;
        Port.Port_IO_TabPage TabPage;
        Port port;

        public Frm_PortSafetyMapSettings(Port _port, Port.Port_IO_TabPage _TabPage, EquipPortMotionParam.Port_UIParam _Port_UIParam)
        {
            InitializeComponent();
            Port_UIParam = _Port_UIParam;
            TabPage = _TabPage;
            port = _port;
            ControlItemInit();

            this.Disposed += (object sender, EventArgs e) =>
            {
                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }

        private void ControlItemInit()
        {
            if ((int)TabPage < Port_UIParam.port_SafetyImageInfos.Length)
                UpdateIOPage(TabPage);

        }

        private void UpdateIOPage(Port.Port_IO_TabPage TabPage)
        {
            var PortSafetyInfo = Port_UIParam.port_SafetyImageInfos[(int)TabPage];
            tbx_MainImagePath.Text = PortSafetyInfo.MainImagePath;

            if(TabPage == Equipment.Port.Port.Port_IO_TabPage.OHT ||
                TabPage == Equipment.Port.Port.Port_IO_TabPage.AGV ||
                TabPage == Equipment.Port.Port.Port_IO_TabPage.EQ ||
                TabPage == Equipment.Port.Port.Port_IO_TabPage.OMRON)
            {
                tbx_EquipImagePath.Text = PortSafetyInfo.EquipmentImagePath;
                tbx_EquipImageLocation_X.Text = PortSafetyInfo.EquipmentImageLocation_X.ToString();
                tbx_EquipImageLocation_Y.Text = PortSafetyInfo.EquipmentImageLocation_Y.ToString();
                btn_SetEquipImagePath.Enabled = true;
            }
            else
            {
                tbx_EquipImagePath.Text = string.Empty;
                tbx_EquipImageLocation_X.Text = string.Empty;
                tbx_EquipImageLocation_Y.Text = string.Empty;
                btn_SetEquipImagePath.Enabled = false;
            }

            if(TabPage == Port.Port_IO_TabPage.TwoBuffer || 
                TabPage == Port.Port_IO_TabPage.OneBuffer || 
                TabPage == Port.Port_IO_TabPage.Conveyor)
            {
                List<Port.DGV_BufferSensorRow> RowList = port.Get_DGV_BufferSensorList();

                for(int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    Port.DGV_BufferSensorRow eRow = RowList[nCount];
                    int Index = nCount;
                    string Text = eRow.ToString();
                    string XPos = PortSafetyInfo.SafetyItems[(int)eRow].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[(int)eRow].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_X)
            {
                List<Port.DGV_ShuttleXAxisSensorRow> RowList = port.Get_DGV_ShuttleX_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    Port.DGV_ShuttleXAxisSensorRow eRow = RowList[nCount];
                    int Index = nCount;
                    string Text = eRow.ToString();
                    string XPos = PortSafetyInfo.SafetyItems[(int)eRow].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[(int)eRow].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_Z)
            {
                List<Port.DGV_ShuttleZAxisSensorRow> RowList = port.Get_DGV_ShuttleZ_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    Port.DGV_ShuttleZAxisSensorRow eRow = RowList[nCount];
                    int Index = nCount;
                    string Text = eRow.ToString();
                    string XPos = PortSafetyInfo.SafetyItems[(int)eRow].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[(int)eRow].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_T)
            {
                List<Port.DGV_ShuttleTAxisSensorRow> RowList = port.Get_DGV_ShuttleT_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    Port.DGV_ShuttleTAxisSensorRow eRow = RowList[nCount];
                    int Index = nCount;
                    string Text = eRow.ToString();
                    string XPos = PortSafetyInfo.SafetyItems[(int)eRow].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[(int)eRow].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.LP_Buffer_Z)
            {
                List<Port.DGV_BufferLP_ZAxisSensorRow> RowList = port.Get_DGV_BufferLP_Z_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    Port.DGV_BufferLP_ZAxisSensorRow eRow = RowList[nCount];
                    int Index = nCount;
                    string Text = eRow.ToString();
                    string XPos = PortSafetyInfo.SafetyItems[(int)eRow].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[(int)eRow].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.OP_Buffer_Z)
            {
                List<Port.DGV_BufferOP_ZAxisSensorRow> RowList = port.Get_DGV_BufferOP_Z_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    Port.DGV_BufferOP_ZAxisSensorRow eRow = RowList[nCount];
                    int Index = nCount;
                    string Text = eRow.ToString();
                    string XPos = PortSafetyInfo.SafetyItems[(int)eRow].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[(int)eRow].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.OP_Buffer_Y)
            {
                List<Port.DGV_BufferOP_YAxisSensorRow> RowList = port.Get_DGV_BufferOP_Y_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    Port.DGV_BufferOP_YAxisSensorRow eRow = RowList[nCount];
                    int Index = nCount;
                    string Text = eRow.ToString();
                    string XPos = PortSafetyInfo.SafetyItems[(int)eRow].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[(int)eRow].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
            else
            {
                for (int nCount = 0; nCount < 2; nCount++)
                {
                    int Index = nCount;
                    string Text = nCount == 0 ? "Port PIO" : "Equip PIO";
                    string XPos = PortSafetyInfo.SafetyItems[nCount].X.ToString();
                    string YPos = PortSafetyInfo.SafetyItems[nCount].Y.ToString();
                    DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-");
                }
            }
        }

        private void btn_SetMainImagePath_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.DefaultExt = "png";
                openFile.Title = "Image File Loader";
                openFile.Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
                                    "|PNG Portable Network Graphics (*.png)|*.png" +
                                    "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
                                    "|BMP Windows Bitmap (*.bmp)|*.bmp" +
                                    "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
                                    "|GIF Graphics Interchange Format (*.gif)|*.gif";
                openFile.InitialDirectory = ManagedFileInfo.StartUpPath;

                DialogResult pathresult = openFile.ShowDialog();
                string[] openFileNames = openFile.FileNames;
                openFile.Dispose();

                if (DialogResult.OK == pathresult && openFileNames.Length > 0)
                {
                    if (openFileNames[0] != null)
                    {
                        if (openFileNames[0].Length < 255)
                        {
                            tbx_MainImagePath.Text = openFileNames[0];
                        }
                        else
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_FilePathLengthOver"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Teaching File Load Cancel Click");
            }
            catch
            {

            }
        }
        private void btn_SetEquipImagePath_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.DefaultExt = "png";
                openFile.Title = "Image File Loader";
                openFile.Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
                                    "|PNG Portable Network Graphics (*.png)|*.png" +
                                    "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
                                    "|BMP Windows Bitmap (*.bmp)|*.bmp" +
                                    "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
                                    "|GIF Graphics Interchange Format (*.gif)|*.gif";
                openFile.InitialDirectory = ManagedFileInfo.StartUpPath;

                DialogResult pathresult = openFile.ShowDialog();
                string[] openFileNames = openFile.FileNames;
                openFile.Dispose();

                if (DialogResult.OK == pathresult && openFileNames.Length > 0)
                {
                    if (openFileNames[0] != null)
                    {
                        if (openFileNames[0].Length < 255)
                        {
                            tbx_EquipImagePath.Text = openFileNames[0];
                        }
                        else
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_FilePathLengthOver"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Teaching File Load Cancel Click");
            }
            catch
            {

            }
        }

        private EquipPortMotionParam.Port_SafetyImageInfo GetSafetyImageInfo()
        {
            EquipPortMotionParam.Port_SafetyImageInfo safetyimageinfo = new EquipPortMotionParam.Port_SafetyImageInfo();
            int Value;

            safetyimageinfo.MainImagePath = tbx_MainImagePath.Text;
            safetyimageinfo.EquipmentImagePath = tbx_EquipImagePath.Text;

            if (Int32.TryParse(tbx_EquipImageLocation_X.Text.ToString(), out Value))
                safetyimageinfo.EquipmentImageLocation_X = Value;

            if (Int32.TryParse(tbx_EquipImageLocation_Y.Text.ToString(), out Value))
                safetyimageinfo.EquipmentImageLocation_Y = Value;

            if (TabPage == Port.Port_IO_TabPage.TwoBuffer ||
                TabPage == Port.Port_IO_TabPage.OneBuffer ||
                TabPage == Port.Port_IO_TabPage.Conveyor)
            {
                List<Port.DGV_BufferSensorRow> RowList = port.Get_DGV_BufferSensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = (int)RowList[nCount];
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_X)
            {
                List<Port.DGV_ShuttleXAxisSensorRow> RowList = port.Get_DGV_ShuttleX_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = (int)RowList[nCount];
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_Z)
            {
                List<Port.DGV_ShuttleZAxisSensorRow> RowList = port.Get_DGV_ShuttleZ_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = (int)RowList[nCount];
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.Shuttle_T)
            {
                List<Port.DGV_ShuttleTAxisSensorRow> RowList = port.Get_DGV_ShuttleT_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = (int)RowList[nCount];
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.LP_Buffer_Z)
            {
                List<Port.DGV_BufferLP_ZAxisSensorRow> RowList = port.Get_DGV_BufferLP_Z_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = (int)RowList[nCount];
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.OP_Buffer_Z)
            {
                List<Port.DGV_BufferOP_ZAxisSensorRow> RowList = port.Get_DGV_BufferOP_Z_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = (int)RowList[nCount];
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }
            else if (TabPage == Port.Port_IO_TabPage.OP_Buffer_Y)
            {
                List<Port.DGV_BufferOP_YAxisSensorRow> RowList = port.Get_DGV_BufferOP_Y_SensorList();

                for (int nCount = 0; nCount < RowList.Count; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = (int)RowList[nCount];
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }
            else
            {
                for (int nCount = 0; nCount < 2; nCount++)
                {
                    if (nCount >= DGV_SafetyRow.Rows.Count)
                        continue;

                    string Name = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.Name].Value.ToString();
                    string XPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    string YPos = DGV_SafetyRow.Rows[nCount].Cells[(int)SafetyGridColumns.YPos].Value.ToString();

                    int ItemIndex = nCount;
                    safetyimageinfo.SafetyItems[ItemIndex].Text = $"{Name}";

                    if (Int32.TryParse(XPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].X = Value;
                    if (Int32.TryParse(YPos, out Value))
                        safetyimageinfo.SafetyItems[ItemIndex].Y = Value;
                }
            }

            return safetyimageinfo;
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            ApplyEvent(TabPage, GetSafetyImageInfo());
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            var SafetyImageInfo = GetSafetyImageInfo();
            ApplyEvent(TabPage, SafetyImageInfo);
            Port_UIParam.port_SafetyImageInfos[(int)TabPage] = SafetyImageInfo;
            
            if(port.GetUIParam().Save(port.GetParam().ID, Port_UIParam))
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private string GetUpText(string text)
        {
            int Data;

            if (int.TryParse(text, out Data))
            {
                Data = Data + (int)numericUpDown1.Value;
                return Data.ToString();
            }

            return Data.ToString();
        }
        private string GetDownText(string text)
        {
            int Data;

            if (int.TryParse(text, out Data))
            {
                Data = Data - (int)numericUpDown1.Value;
                return Data.ToString();
            }

            return Data.ToString();
        }

        private void DGV_SafetyRow_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                if (e.ColumnIndex == (int)SafetyGridColumns.XPosUp)
                {
                    string XPos = senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.XPos].Value = GetUpText(XPos);
                }
                else if (e.ColumnIndex == (int)SafetyGridColumns.XPosDown)
                {
                    string XPos = senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                    senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.XPos].Value = GetDownText(XPos);
                }
                else if (e.ColumnIndex == (int)SafetyGridColumns.YPosUp)
                {
                    string YPos = senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.YPos].Value.ToString();
                    senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.YPos].Value = GetUpText(YPos);
                }
                else if (e.ColumnIndex == (int)SafetyGridColumns.YPosDown)
                {
                    string YPos = senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.YPos].Value.ToString();
                    senderGrid.Rows[e.RowIndex].Cells[(int)SafetyGridColumns.YPos].Value = GetDownText(YPos);
                }

                ApplyEvent(TabPage, GetSafetyImageInfo());
            }
        }
    }
}
