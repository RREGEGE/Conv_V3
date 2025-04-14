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
    public partial class Frm_MasterCtrlCPS : Form
    {
        public Frm_MasterCtrlCPS()
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
                GroupBoxFunc.SetText(groupBox_ErrorInfo, SynusLangPack.GetLanguage("GorupBox_ErrorInfo"));
                GroupBoxFunc.SetText(groupBox_ErrorDetail, SynusLangPack.GetLanguage("GorupBox_ErrorDetailInfo"));
                GroupBoxFunc.SetText(groupBox_CPSStatus, SynusLangPack.GetLanguage("GorupBox_StatusInfo"));
                GroupBoxFunc.SetText(gorupBox_CPSInfo, SynusLangPack.GetLanguage("GorupBox_CPSInfo"));

                Master.Update_DGV_CPSByteMap(ref DGV_CPSByteMap);
                Master.Update_DGV_CPSErrorInfo(ref DGV_ErrorInfo);
                Master.Update_DGV_CPSErrorDetailInfo(ref DGV_ErrorDetailInfo, DGV_ErrorInfo.CurrentRow.Index);
                Master.Update_DGV_CPSStatus(ref DGV_CPSStatusInfo);

                btn_CPSPowerOnReq.Visible = false;
                btn_CPSErrorResetReq.Visible = false;

                ButtonFunc.SetBackColor(btn_CPSPowerOnReq, Master.CMD_CPS_Power_On_REQ ? Color.Lime : Color.White);
                ButtonFunc.SetBackColor(btn_CPSErrorResetReq, Master.CMD_CPS_Error_Reset_REQ ? Color.Lime : Color.White);
            }
            catch { }
            finally
            {

            }
        }

        private void btn_CPSPowerOnReq_Click(object sender, EventArgs e)
        {
            Master.CMD_CPS_Power_On_REQ = !Master.CMD_CPS_Power_On_REQ;
        }

        private void btn_CPSErrorResetReq_Click(object sender, EventArgs e)
        {
            Master.CMD_CPS_Error_Reset_REQ = !Master.CMD_CPS_Error_Reset_REQ;
        }
    }
}
