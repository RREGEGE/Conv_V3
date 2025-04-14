using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Equipment.RackMaster;

namespace Master.SubForm.RMTPForm.RMTPSubForm
{
    public partial class Frm_RackMasterTPMappingInfo : Form
    {
        public Frm_RackMasterTPMappingInfo()
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
                GroupBoxFunc.SetText(groupBox_AutoRunStatus, SynusLangPack.GetLanguage("GorupBox_MemoryMapInfo"));

                RackMaster rackMaster = (RackMaster)Frm_RackMasterTPScreen.g_CurrentRM;

                if (rackMaster != null)
                {
                    rackMaster.Update_DGV_CIMToRMBitMap(ref DGV_CIMtoPortBitMap);
                    rackMaster.Update_DGV_RMToCIMBitMap(ref DGV_PortToCIMBitMap);
                    rackMaster.Update_DGV_CIMToRMWordMap(ref DGV_CIMtoPortWordMap);
                    rackMaster.Update_DGV_RMToCIMWordMap(ref DGV_PortToCIMWordMap);
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
