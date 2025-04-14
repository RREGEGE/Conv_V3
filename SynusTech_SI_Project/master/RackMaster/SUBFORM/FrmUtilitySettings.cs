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
    public partial class FrmUtilitySettings : Form {
        public delegate void LangChangeDelegate();
        public event LangChangeDelegate LangChangeEvent;

        private enum SelectedMenuType {
            Password,
            Language,
            UI,
            EtherCAT,
            System,
        }

        private SEQ.CLS.Timer m_saveStopwatch;
        private SelectedMenuType m_menu;
        private int newPassword = 0;

        public FrmUtilitySettings() {
            InitializeComponent();

            pnlPassword.BringToFront();

            m_saveStopwatch = new SEQ.CLS.Timer();
            m_saveStopwatch.Stop();

            m_menu = SelectedMenuType.Password;

            btnPasswordSave.MouseDown   += SaveMouseDown;
            btnLangSave.MouseDown       += SaveMouseDown;
            btnUISave.MouseDown         += SaveMouseDown;
            btnEtherCATSave.MouseDown   += SaveMouseDown;
            btnSaveSytem.MouseDown      += SaveMouseDown;

            btnPasswordSave.MouseUp     += SaveMouseUp;
            btnLangSave.MouseUp         += SaveMouseUp;
            btnUISave.MouseUp           += SaveMouseUp;
            btnEtherCATSave.MouseUp     += SaveMouseUp;
            btnSaveSytem.MouseUp        += SaveMouseUp;

            switch (Utility.GetCurrentLanguageType()) {
                case SynusLangPack.LanguageType.Korean:
                    radioEnglish.Checked = false;
                    radioKorean.Checked = true;
                    break;

                case SynusLangPack.LanguageType.English:
                    radioEnglish.Checked = true;
                    radioKorean.Checked = false;
                    break;
            }

            switch (Utility.GetIOViewType()) {
                case Utility.UI_IOViewType.RawData:
                    radIO_Raw.Checked = true;
                    radIO_Convert.Checked = false;
                    break;

                case Utility.UI_IOViewType.ConvertData:
                    radIO_Raw.Checked = false;
                    radIO_Convert.Checked = true;
                    break;
            }

            cboxAutoRecovery.Checked    = Utility.GetEtherCAT_AutoRecovery();
            cboxModifySyncTime.Checked  = Utility.GetApp_ModifySyncTime();
        }

        private void SaveMouseDown(object sender, MouseEventArgs e) {
            Button btn = sender as Button;

            if(btn == btnPasswordSave) {
                int curPassword = 0;
                int newConfirmPassword = 0;

                if(!int.TryParse($"{txtCurrentPassword.Text}", out curPassword) || !int.TryParse($"{txtNewPassword.Text}", out newPassword) ||
                    !int.TryParse($"{txtNewPasswordConfirm.Text}", out newConfirmPassword)) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.PasswordIsOnlyNumber}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }

                if(curPassword != Utility.GetCurrentUserPassword()) {
                    MessageBox.Show($"{UI_MessageBoxLangPackList.CurrentPasswordNotCorrect}", SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }

                if(newPassword != newConfirmPassword) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.NewPasswordNotCorrect}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }
                m_menu = SelectedMenuType.Password;
                saveTimer.Start();
            }else if(btn == btnLangSave) {
                if(!radioKorean.Checked && !radioEnglish.Checked) {
                    MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.LanguageCheck}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Warning}"));
                    return;
                }

                m_menu = SelectedMenuType.Language;
                saveTimer.Start();
            }else if(btn == btnUISave) {
                if(!radIO_Convert.Checked && !radIO_Raw.Checked) {
                    return;
                }

                m_menu = SelectedMenuType.UI;
                saveTimer.Start();
            }else if(btn == btnEtherCATSave) {
                m_menu = SelectedMenuType.EtherCAT;
                saveTimer.Start();
            }else if(btn == btnSaveSytem) {
                m_menu = SelectedMenuType.System;
                saveTimer.Start();
            }
        }

        private void SaveMouseUp(object sender, MouseEventArgs e) {
            saveTimer.Stop();
            m_saveStopwatch.Stop();
        }

        private void saveTimer_Tick(object sender, EventArgs e) {
            if (!m_saveStopwatch.IsTimerStarted())
                m_saveStopwatch.Restart();

            if (!m_saveStopwatch.Delay(UICtrl.m_saveDelayTime))
                return;

            saveTimer.Stop();
            m_saveStopwatch.Stop();
            if (DialogResult.No == MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SureParameterSave}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"), MessageBoxButtons.YesNo)) {
                return;
            }

            switch (m_menu) {
                case SelectedMenuType.Password:
                    SetPassword();
                    break;

                case SelectedMenuType.Language:
                    SetLanguage();
                    break;

                case SelectedMenuType.UI:
                    SetUI();
                    break;

                case SelectedMenuType.EtherCAT:
                    SetEtherCAT();
                    break;

                case SelectedMenuType.System:
                    SetSystem();
                    break;
            }
        }

        private void SetPassword() {
            Utility.SetNewPassword(newPassword);

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.PasswordIsChanged}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
        }

        private void SetLanguage() {
            if (radioKorean.Checked) {
                Utility.SetLanguageType(SynusLangPack.LanguageType.Korean);
            }else if (radioEnglish.Checked) {
                Utility.SetLanguageType(SynusLangPack.LanguageType.English);
            }

            this.LangChangeEvent();

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.LanguageIsChanged}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
        }

        private void SetUI() {
            if (radIO_Raw.Checked) {
                Utility.SetIOViewType(Utility.UI_IOViewType.RawData);
            }else if (radIO_Convert.Checked) {
                Utility.SetIOViewType(Utility.UI_IOViewType.ConvertData);
            }

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.UISettingSaved}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
        }

        private void SetEtherCAT() {
            Utility.SetEtherCAT_AutoRecovery(cboxAutoRecovery.Checked);

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.EtherCATSettingSave}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
        }

        private void SetSystem() {
            Utility.SetApp_ModifySyncTime(cboxModifySyncTime.Checked);

            MessageBox.Show(SynusLangPack.GetLanguage($"{UI_MessageBoxLangPackList.SystemSettingSave}"), SynusLangPack.GetLanguage($"{UI_MessageBoxTitleLangPackList.Notify}"));
        }

        private void listMenu_SelectedIndexChanged(object sender, EventArgs e) {
            if(listMenu.SelectedIndex == (int)SelectedMenuType.Password) {
                pnlPassword.BringToFront();
            }else if(listMenu.SelectedIndex == (int)SelectedMenuType.Language) {
                pnlLang.BringToFront();
            }else if(listMenu.SelectedIndex == (int)SelectedMenuType.UI) {
                pnlUI.BringToFront();
            }else if(listMenu.SelectedIndex == (int)SelectedMenuType.EtherCAT) {
                pnlEtherCAT.BringToFront();
            }else if(listMenu.SelectedIndex == (int)SelectedMenuType.System) {
                pnlSystem.BringToFront();
            }
        }
    }
}
