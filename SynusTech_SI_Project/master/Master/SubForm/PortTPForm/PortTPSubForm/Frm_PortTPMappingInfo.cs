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

namespace Master.SubForm.PortTPForm.PortTPSubForm
{
    public partial class Frm_PortTPMappingInfo : Form
    {
        public Frm_PortTPMappingInfo()
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
        }
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);
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

                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    if(gorupBox_MemoryMapInfo.Tag != CurrentPort)
                    {
                        DGV_CIMtoPortBitMap.Rows.Clear();
                        DGV_PortToCIMBitMap.Rows.Clear();
                        DGV_CIMtoPortWordMap.Rows.Clear();
                        DGV_PortToCIMWordMap.Rows.Clear();
                        gorupBox_MemoryMapInfo.Tag = CurrentPort;
                    }
                    CurrentPort.Update_DGV_CIMToPortBitMap(ref DGV_CIMtoPortBitMap);
                    CurrentPort.Update_DGV_PortToCIMBitMap(ref DGV_PortToCIMBitMap);
                    CurrentPort.Update_DGV_CIMToPortWordMap(ref DGV_CIMtoPortWordMap);
                    CurrentPort.Update_DGV_PortToCIMWordMap(ref DGV_PortToCIMWordMap);
                }
                else
                {
                    if (DGV_CIMtoPortBitMap.Rows.Count > 0)
                        DGV_CIMtoPortBitMap.Rows.Clear();

                    if (DGV_PortToCIMBitMap.Rows.Count > 0)
                        DGV_PortToCIMBitMap.Rows.Clear();

                    if (DGV_CIMtoPortWordMap.Rows.Count > 0)
                        DGV_CIMtoPortWordMap.Rows.Clear();

                    if (DGV_PortToCIMWordMap.Rows.Count > 0)
                        DGV_PortToCIMWordMap.Rows.Clear();
                }
            }
            catch{ }
            finally
            {

            }
        }
    }
}
