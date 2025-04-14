using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RackMaster.SEQ.PART;
using RackMaster.SUBFORM.SettingPage;

namespace RackMaster.SUBFORM {
    public partial class FrmSettings : Form {

        private RackMasterMain m_rackMaster;
        private SettingPage_RMParam pageRMParam;
        private SettingPage_PortShelf pagePortShelf;
        private SettingPage_IO pageIO;
        private SettingPage_Teaching pageTeaching;
        private SettingPage_WMX pageWMX;

        private UICtrl.ButtonCtrl m_btnCtrl;

        public FrmSettings(RackMasterMain rackMaster) {
            InitializeComponent();

            m_rackMaster = rackMaster;

            pageRMParam         = new SettingPage_RMParam(m_rackMaster);
            pagePortShelf       = new SettingPage_PortShelf(m_rackMaster);
            pageIO              = new SettingPage_IO(m_rackMaster);
            pageTeaching        = new SettingPage_Teaching(m_rackMaster);
            pageWMX             = new SettingPage_WMX(m_rackMaster);

            m_btnCtrl = new UICtrl.ButtonCtrl();

            pageRMParam.TopLevel        = false;
            pagePortShelf.TopLevel      = false;
            pageIO.TopLevel             = false;
            pageTeaching.TopLevel       = false;
            pageWMX.TopLevel            = false;

            pnlMain.Controls.Add(pageRMParam);
            pnlMain.Controls.Add(pagePortShelf);
            pnlMain.Controls.Add(pageIO);
            pnlMain.Controls.Add(pageTeaching);
            pnlMain.Controls.Add(pageWMX);

            LanguageChanged();

            HideAllPage();

            pageRMParam.Visible = true;
            m_btnCtrl.SetOnOffButtonStyle(ref btnParamPage, true);

            btnIOPage.MouseClick            += PageButtonClickEvent;
            btnParamPage.MouseClick         += PageButtonClickEvent;
            btnPortPage.MouseClick          += PageButtonClickEvent;
            btnTeachingPage.MouseClick      += PageButtonClickEvent;
            btnWmxPage.MouseClick           += PageButtonClickEvent;
        }

        private void HideAllPage() {
            pageRMParam.Visible     = false;
            pagePortShelf.Visible   = false;
            pageIO.Visible          = false;
            pageTeaching.Visible    = false;
            pageWMX.Visible         = false;

            m_btnCtrl.SetOnOffButtonStyle(ref btnIOPage, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnParamPage, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnPortPage, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnTeachingPage, false);
            m_btnCtrl.SetOnOffButtonStyle(ref btnWmxPage, false);
        }

        private void PageButtonClickEvent(object sender, EventArgs args) {
            Button btn = sender as Button;

            if(btn == btnIOPage) {
                HideAllPage();
                pageIO.Visible = true;
            }else if(btn == btnParamPage) {
                HideAllPage();
                pageRMParam.Visible = true;
            }else if(btn == btnPortPage) {
                HideAllPage();
                pagePortShelf.Visible = true;
            }else if(btn == btnTeachingPage) {
                HideAllPage();
                pageTeaching.Visible = true;
            }else if(btn == btnWmxPage) {
                HideAllPage();
                pageWMX.Visible = true;
                pageWMX.RefreshWMXParameter(false);
            }

            m_btnCtrl.SetOnOffButtonStyle(ref btn, true);
        }

        public void UpdateFormUI() {
            pagePortShelf.UpdateFormUI();
            pageTeaching.UpdateFormUI();
            pageRMParam.UpdateFormUI();
            pageIO.UpdateFormUI();
        }

        public void AccessLoginType_Admin() {
            HideAllPage();

            pageRMParam.AccessLoginType_Admin();
            pageTeaching.AccessLoginType_Admin();

            pageRMParam.Visible     = true;
            btnPortPage.Visible     = true;
            btnIOPage.Visible       = true;
            btnWmxPage.Visible      = true;

            m_btnCtrl.SetOnOffButtonStyle(ref btnParamPage, true);
            pageWMX.RefreshWMXParameter(false);
        }

        public void AccessLoginType_User() {
            HideAllPage();

            pageRMParam.AccessLoginType_User();
            pageTeaching.AccessLoginType_User();

            pageRMParam.Visible     = true;
            btnPortPage.Visible     = false;
            btnIOPage.Visible       = false;
            btnWmxPage.Visible      = false;

            m_btnCtrl.SetOnOffButtonStyle(ref btnParamPage, true);
            pageWMX.RefreshWMXParameter(false);
        }

        public void LanguageChanged() {
            //gboxMotionParam.Text            = SynusLangPack.GetLanguage(gboxMotionParam.Name);
            //gboxAxisParam.Text              = SynusLangPack.GetLanguage(gboxAxisParam.Name);
            //gboxAutoTeachingParam.Text      = SynusLangPack.GetLanguage(gboxAutoTeachingParam.Name);
            //gboxTimerParam.Text             = SynusLangPack.GetLanguage(gboxTimerParam.Name);
            //gboxScaraParam.Text             = SynusLangPack.GetLanguage(gboxScaraParam.Name);
            //gboxShelfSetting.Text           = SynusLangPack.GetLanguage(gboxShelfSetting.Name);
            //gboxPortSetting.Text            = SynusLangPack.GetLanguage(gboxPortSetting.Name);
            //gboxManualTeaching.Text         = SynusLangPack.GetLanguage(gboxManualTeaching.Name);
            //gboxAutoTeaching.Text           = SynusLangPack.GetLanguage(gboxAutoTeaching.Name);

            //m_btnCtrl.SetButtonText(ref btnParamPage, SynusLangPack.GetLanguage(btnParamPage.Name));
            //m_btnCtrl.SetButtonText(ref btnPortPage, SynusLangPack.GetLanguage(btnPortPage.Name));
            //m_btnCtrl.SetButtonText(ref btnTeachingPage, SynusLangPack.GetLanguage(btnTeachingPage.Name));
            //m_btnCtrl.SetButtonText(ref btnWmxPage, SynusLangPack.GetLanguage(btnWmxPage.Name));
            //m_btnCtrl.SetButtonText(ref btnParameterSave, SynusLangPack.GetLanguage($"{btnParameterSave.Tag}"));
            //m_btnCtrl.SetButtonText(ref btnAddPort, SynusLangPack.GetLanguage(btnAddPort.Name));
            //m_btnCtrl.SetButtonText(ref btnDeletePort, SynusLangPack.GetLanguage(btnDeletePort.Name));
            //m_btnCtrl.SetButtonText(ref btnRefreshWMXParam, SynusLangPack.GetLanguage(btnRefreshWMXParam.Name));
            //m_btnCtrl.SetButtonText(ref btnLoadWMXParam, SynusLangPack.GetLanguage(btnLoadWMXParam.Name));
            //m_btnCtrl.SetButtonText(ref btnSaveWMXParam, SynusLangPack.GetLanguage($"{btnSaveWMXParam.Tag}"));
        }
    }
}
