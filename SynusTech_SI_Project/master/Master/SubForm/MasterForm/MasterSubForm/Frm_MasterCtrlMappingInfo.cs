using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Master.SubForm.MasterForm.MasterSubForm
{
    public partial class Frm_MasterCtrlMappingInfo : Form
    {
        public Frm_MasterCtrlMappingInfo()
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
                GroupBoxFunc.SetText(gorupBox_MemoryMapInfo, SynusLangPack.GetLanguage("GorupBox_MemoryMapInfo"));

                Master.Update_DGV_CIMToMasterBitMap(ref DGV_CIMtoPortBitMap);
                Master.Update_DGV_MasterToCIMBitMap(ref DGV_PortToCIMBitMap);
                Master.Update_DGV_CIMToMasterWordMap(ref DGV_CIMtoPortWordMap);
                Master.Update_DGV_MasterToCIMWordMap(ref DGV_PortToCIMWordMap);

            }
            catch{ }
            finally
            {

            }
        }
    }
}
