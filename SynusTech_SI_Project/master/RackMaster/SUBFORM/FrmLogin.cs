using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SUBFORM {
    public partial class FrmLogin : Form {
        private enum Login_Duration {
            FiveMinute,
            TenMinute,
            FifteenMinute,
            ThirtyMinute,
        }

        public delegate void LoginDelegate(Utility.PasswordType passwordType);
        public delegate void LoginDelegateWithDuration(Utility.PasswordType passwordType, int durationMillisconds);
        public event LoginDelegate loginEvent;
        public event LoginDelegateWithDuration loginEventWithDuration;

        private bool durationMode;

        public FrmLogin(bool durationMode) {
            this.durationMode = durationMode;

            InitializeComponent();

            if (durationMode) {
                foreach (Login_Duration duration in Enum.GetValues(typeof(Login_Duration))) {
                    switch (duration) {
                        case Login_Duration.FiveMinute:
                            cboxDuration.Items.Add("5 min");
                            break;

                        case Login_Duration.TenMinute:
                            cboxDuration.Items.Add("10 min");
                            break;

                        case Login_Duration.FifteenMinute:
                            cboxDuration.Items.Add("15 min");
                            break;

                        case Login_Duration.ThirtyMinute:
                            cboxDuration.Items.Add("30 min");
                            break;
                    }
                }

                cboxDuration.SelectedIndex = 0;
            }
            else {
                lblDuration.Visible = false;
                cboxDuration.Visible = false;
            }

            txtPassword.KeyDown += KeyDownEvent;
        }

        private void btnEnter_Click(object sender, EventArgs e) {
            AccessLogin();
        }

        private void KeyDownEvent(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Enter) {
                AccessLogin();
            }
        }

        private void AccessLogin() {
            int password = 0;
            int duration = 0;

            if (durationMode) {
                if (cboxDuration.SelectedIndex == (int)Login_Duration.FiveMinute) {
                    duration = 5 * 60 * 1000;
                }
                else if (cboxDuration.SelectedIndex == (int)Login_Duration.TenMinute) {
                    duration = 10 * 60 * 1000;
                }
                else if (cboxDuration.SelectedIndex == (int)Login_Duration.FifteenMinute) {
                    duration = 15 * 60 * 1000;
                }
                else if (cboxDuration.SelectedIndex == (int)Login_Duration.ThirtyMinute) {
                    duration = 30 * 60 * 1000;
                }
            }

            if (int.TryParse(txtPassword.Text, out password)) {
                if (password == Utility.GetCurrentUserPassword()) {
                    if (durationMode) {
                        this.loginEventWithDuration(Utility.PasswordType.User, duration);
                    }
                    else {
                        this.loginEvent(Utility.PasswordType.User);
                    }
                    Close();
                    return;
                }else if(password == Utility.GetCurrentAdminPassword()) {
                    if (durationMode) {
                        this.loginEventWithDuration(Utility.PasswordType.Admin, duration);
                    }
                    else {
                        this.loginEvent(Utility.PasswordType.Admin);
                    }
                    Close();
                    return;
                }
            }

            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.Utility, Log.LogMessage_Main.Utility_PasswordDoesNotMatch));
            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.InvalidPassword}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
        }

        private void btnExit_Click(object sender, EventArgs e) {
            //if(MessageBox.Show("해당 창을 닫으시겠습니까?", "알림", MessageBoxButtons.YesNo) == DialogResult.Yes) {
            //    Close();
            //}
            Close();
        }
    }
}
