using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.ManagedFile;

namespace Master.GlobalForm
{
    public partial class Frm_AcceptSave : Form
    {
        public enum SaveSection
        {
            None,
            EquipmentNetworkInfo,
            PortMotionAxisInfo,
            Master_IO,
            Port_MotionParam,
            Port_ConveryorParam,
            Port_WMXParam,
            Port_IO
        }

        SaveSection eSaveSection = SaveSection.None;
        string SubTitle = string.Empty;

        public Frm_AcceptSave(SaveSection _eSaveSection, string _SubTitle)
        {
            InitializeComponent();
            ControlItemInit();
            eSaveSection = _eSaveSection;
            SubTitle = _SubTitle;
            ParamSaveAdmin.Load();

            this.Disposed += (object sender, EventArgs e) =>
            {
                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }
        private void ControlItemInit()
        {
            this.ActiveControl = tbx_ID;

            LaguageCheck();
        }

        private void LaguageCheck()
        {
            FormFunc.SetText(this, SynusLangPack.GetLanguage("Frm_Login_FormTitle"));

            LabelFunc.SetText(lbl_IDTitle, SynusLangPack.GetLanguage("Label_AdminName"));
            LabelFunc.SetText(lbl_PasswordTitle, SynusLangPack.GetLanguage("Label_Password"));
            ButtonFunc.SetText(btn_SaveAccept, SynusLangPack.GetLanguage("Btn_SaveAccept"));
        }

        private void btn_SaveAccept_Click(object sender, EventArgs e)
        {
            string Admin = tbx_ID.Text;
            string password = tbx_Password.Text;

            if (string.IsNullOrEmpty(Admin) || !ParamSaveAdmin.GetAdminList().Contains(Admin))
            {
                MessageBox.Show(SynusLangPack.GetLanguage("Message_ParameterSaveAdminName"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.ParameterSavePasswordFail, $"Admin Name : {Admin}");
                return;
            }

            if (password == "723181")
            {
                if(SubTitle != string.Empty)
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ParameterSaveAccept, $"Admin Name: {Admin}, {SubTitle} Save Section: {eSaveSection}");
                else
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ParameterSaveAccept, $"Admin Name: {Admin}, Save Section: {eSaveSection}");

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.ParameterSavePasswordFail, $"Parameter Save Password Fail");
            }
        }
    }
}
