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

namespace Master.SubForm
{
    public partial class Frm_Login : Form
    {
        Dictionary<string, string> PasswordDic = new Dictionary<string, string>();

        public Frm_Login()
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
            cbx_LoginDuration.Items.AddRange(new string[] { "5 min", "10 min", "30 min" });
            cbx_LoginDuration.SelectedIndex = LoginDurationToIndex(ApplicationParam.m_ApplicationParam.LoginDuration);

            PasswordDic.Add("admin", "admin");
            PasswordDic.Add("maint", "safe");
            PasswordDic.Add("user", "1234");

            this.ActiveControl = tbx_ID;

            LaguageCheck();
        }

        private void LaguageCheck()
        {
            FormFunc.SetText(this, SynusLangPack.GetLanguage("Frm_Login_FormTitle"));

            LabelFunc.SetText(lbl_IDTitle,         SynusLangPack.GetLanguage("Label_ID"));
            LabelFunc.SetText(lbl_PasswordTitle,   SynusLangPack.GetLanguage("Label_Password"));
            LabelFunc.SetText(lbl_DurationTimeTitle, SynusLangPack.GetLanguage("Label_DurationTime"));
            ButtonFunc.SetText(btn_LogIn, SynusLangPack.GetLanguage("Btn_LogIn"));
        }

        private void btn_LogIn_Click(object sender, EventArgs e)
        {
            bool bLogin = false;
            string ID = tbx_ID.Text;
            string Password = tbx_Password.Text;

            if(PasswordDic.ContainsKey(ID.ToLower()))
            {
                if (Password.ToLower() == PasswordDic[ID])
                    bLogin = true;
            }

            if (bLogin)
            {
                //Login Duration Settings
                if(ApplicationParam.m_ApplicationParam.LoginDuration != LoginDurationIndexToDouble())
                {
                    ApplicationParam.m_ApplicationParam.LoginDuration = LoginDurationIndexToDouble();
                    ApplicationParam.SaveFile(ManagedFileInfo.AppSettingsDirectory, ManagedFileInfo.AppSettingsFileName);
                }

                if (Password == "admin")
                {
                    LogIn.SetLogIn(LogIn.LogInLevel.Admin);
                }
                else if (Password == "safe")
                {
                    LogIn.SetLogIn(LogIn.LogInLevel.Maint);
                }
                else
                {
                    LogIn.SetLogIn(LogIn.LogInLevel.User);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Error, LogMsg.MsgList.LogIn, $"LogIn Fail");

                LogIn.SetLogout();

                if (!PasswordDic.ContainsKey(ID.ToLower()))
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_IDErrorMessageStr"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_PasswordErrorMessageStr"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_LogIn_Click(sender, e);
            else if (e.KeyCode == Keys.F1)
            {
                if (ApplicationParam.m_ApplicationParam.LoginDuration != LoginDurationIndexToDouble())
                {
                    ApplicationParam.m_ApplicationParam.LoginDuration = LoginDurationIndexToDouble();
                    ApplicationParam.SaveFile(ManagedFileInfo.AppSettingsDirectory, ManagedFileInfo.AppSettingsFileName);
                }
                LogIn.SetLogIn(LogIn.LogInLevel.Admin);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private int LoginDurationIndexToDouble()
        {
            switch (cbx_LoginDuration.SelectedIndex)
            {
                case 0:
                    return 300;
                case 1:
                    return 600;
                case 2:
                    return 1800;
                default:
                    return 300;
            }
        }
        private int LoginDurationToIndex(double loginDuration)
        {
            if (loginDuration <= 300)
                return 0;
            else if (loginDuration <= 600)
                return 1;
            else if (loginDuration <= 1800)
                return 2;
            else
                return 0;
        }
    }
}
