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
    public partial class Frm_Language : Form
    {
        public Frm_Language()
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
            cbx_LanguageNation.Items.AddRange(Enum.GetNames(typeof(SynusLangPack.LanguageType)));
            cbx_LanguageNation.SelectedIndex = cbx_LanguageNation.FindString($"{ApplicationParam.m_ApplicationParam.eLangType}");

            LaguageCheck();
        }

        private void LaguageCheck()
        {
            FormFunc.SetText(this, SynusLangPack.GetLanguage("Frm_Language_FormTitle"));
            LabelFunc.SetText(lbl_LanguageTitle, SynusLangPack.GetLanguage("Label_Language"));
            ButtonFunc.SetText(btn_Apply, SynusLangPack.GetLanguage("Btn_Apply"));
            ButtonFunc.SetText(btn_LangPackReload, SynusLangPack.GetLanguage("Btn_LanguagePackReload"));
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            if (ApplicationParam.m_ApplicationParam.eLangType != (SynusLangPack.LanguageType)Enum.Parse(typeof(SynusLangPack.LanguageType), cbx_LanguageNation.SelectedItem.ToString()))
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.Process, $"Language Type Change {ApplicationParam.m_ApplicationParam.eLangType}");
                ApplicationParam.m_ApplicationParam.eLangType = (SynusLangPack.LanguageType)Enum.Parse(typeof(SynusLangPack.LanguageType), cbx_LanguageNation.SelectedItem.ToString());
                ApplicationParam.SaveFile(ManagedFileInfo.AppSettingsDirectory, ManagedFileInfo.AppSettingsFileName);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_LangPackReload_Click(object sender, EventArgs e)
        {
            LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Language Settings Form-LangPack Reload Click");
            SynusLangPack.LoadFile(ManagedFileInfo.LangPackDirectory, ManagedFileInfo.LangPackFileName);
        }
    }
}
