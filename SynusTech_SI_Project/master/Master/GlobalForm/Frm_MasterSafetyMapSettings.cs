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

namespace Master.GlobalForm
{
    public partial class Frm_MasterSafetyMapSettings : Form
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
            YPosDown,
            MapVisible,
            GridVisible
        }
        public delegate void ApplyEventHandler(MasterSafetyImageInfo _SafetyImageInfo);
        public event ApplyEventHandler ApplyEvent;

        MasterSafetyImageInfo ImageInfo;

        public Frm_MasterSafetyMapSettings(ref MasterSafetyImageInfo _ImageInfo)
        {
            InitializeComponent();
            ImageInfo = _ImageInfo;
            ControlItemInit();

            this.Disposed += (object sender, EventArgs e) =>
            {
                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }
        private void ControlItemInit()
        {
            tbx_ImagePath.Text = ImageInfo.WorkZoneImagePath;

            foreach (Master.DGV_SaftyIOStatusRow eSafetyItemList in Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)))
            {
                int Index = (int)eSafetyItemList;
                string Text = Master.GetMasterSafetyStr(eSafetyItemList);
                string XPos = ImageInfo.SafetyItems[(int)eSafetyItemList].X.ToString();
                string YPos = ImageInfo.SafetyItems[(int)eSafetyItemList].Y.ToString();
                bool bMapCheck = ImageInfo.SafetyItems[(int)eSafetyItemList].MapVisible;
                bool bGridCheck = ImageInfo.SafetyItems[(int)eSafetyItemList].GridVisible;
                DGV_SafetyRow.Rows.Add(Index.ToString(), Text, XPos, "+", "-", YPos, "+", "-", bMapCheck, bGridCheck);
            }
        }

        private void btn_SetImagePath_Click(object sender, EventArgs e)
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
                            tbx_ImagePath.Text = openFileNames[0];
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

        private MasterSafetyImageInfo GetSafetyImageInfo()
        {
            MasterSafetyImageInfo safetyimageinfo = new MasterSafetyImageInfo();

            safetyimageinfo.WorkZoneImagePath = tbx_ImagePath.Text;

            foreach (var eSafetyItemList in Enum.GetValues(typeof(Master.DGV_SaftyIOStatusRow)))
            {
                int Index = (int)eSafetyItemList;

                if (Index >= DGV_SafetyRow.Rows.Count)
                    continue;

                string XPos = DGV_SafetyRow.Rows[Index].Cells[(int)SafetyGridColumns.XPos].Value.ToString();
                string YPos = DGV_SafetyRow.Rows[Index].Cells[(int)SafetyGridColumns.YPos].Value.ToString();
                bool bMapVisible = (bool)DGV_SafetyRow.Rows[Index].Cells[(int)SafetyGridColumns.MapVisible].Value;
                bool bGridVisible = (bool)DGV_SafetyRow.Rows[Index].Cells[(int)SafetyGridColumns.GridVisible].Value;

                int Value;

                safetyimageinfo.SafetyItems[(int)eSafetyItemList].Text = $"{eSafetyItemList}";

                if (Int32.TryParse(XPos, out Value))
                    safetyimageinfo.SafetyItems[(int)eSafetyItemList].X = Value;
                if (Int32.TryParse(YPos, out Value))
                    safetyimageinfo.SafetyItems[(int)eSafetyItemList].Y = Value;

                safetyimageinfo.SafetyItems[(int)eSafetyItemList].MapVisible = bMapVisible;
                safetyimageinfo.SafetyItems[(int)eSafetyItemList].GridVisible = bGridVisible;
            }

            return safetyimageinfo;
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            ApplyEvent(GetSafetyImageInfo());
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            var SafetyImageInfo = GetSafetyImageInfo();
            ApplyEvent(SafetyImageInfo);

            if(MasterSafetyImageInfo.Save(SafetyImageInfo))
                MessageBox.Show(SynusLangPack.GetLanguage("Message_SaveSuccess"), SynusLangPack.GetLanguage("InfoMessage"), MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private string GetUpText(string text)
        {
            int Data;

            if(int.TryParse(text, out Data))
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

                ApplyEvent(GetSafetyImageInfo());
            }
        }
    }
}
