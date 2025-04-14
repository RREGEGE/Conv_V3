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
    public partial class Frm_LogSet : Form
    {
        public Frm_LogSet()
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
            for(int nCount = 1; nCount <= 365; nCount++)
            {
                cbx_CompressionCycle.Items.Add($"{nCount} Days");
                cbx_DeleteCycle.Items.Add($"{nCount} Days");
            }

            if (ApplicationParam.m_LogParam.CompressionCycle - 1 < 0)
                ApplicationParam.m_LogParam.CompressionCycle = 1;
            else if (ApplicationParam.m_LogParam.CompressionCycle > 365)
                ApplicationParam.m_LogParam.CompressionCycle = 365;

            if (ApplicationParam.m_LogParam.ZIPDeleteCycle - 1 < 0)
                ApplicationParam.m_LogParam.ZIPDeleteCycle = 1;
            else if (ApplicationParam.m_LogParam.ZIPDeleteCycle > 365)
                ApplicationParam.m_LogParam.ZIPDeleteCycle = 365;


            cbx_CompressionCycle.SelectedIndex = (ApplicationParam.m_LogParam.CompressionCycle - 1);
            cbx_DeleteCycle.SelectedIndex = (ApplicationParam.m_LogParam.ZIPDeleteCycle - 1);

            chx_Application.Checked = Log.ShowType[LogMsg.LogType.Application];
            chx_Exception.Checked = Log.ShowType[LogMsg.LogType.Exception];
            chx_Port.Checked = Log.ShowType[LogMsg.LogType.Port];
            chx_RackMaster.Checked = Log.ShowType[LogMsg.LogType.RackMaster];
            chx_CIM.Checked = Log.ShowType[LogMsg.LogType.CIM];
            chx_Master.Checked = Log.ShowType[LogMsg.LogType.Master];
            chx_WMX.Checked = Log.ShowType[LogMsg.LogType.WMX];
            chx_CPS.Checked = Log.ShowType[LogMsg.LogType.CPS];

            chx_Normal.Checked = Log.ShowLevel[LogMsg.LogLevel.Normal];
            chx_Warning.Checked = Log.ShowLevel[LogMsg.LogLevel.Warning];
            chx_Error.Checked = Log.ShowLevel[LogMsg.LogLevel.Error];

            LaguageCheck();
        }

        private void LaguageCheck()
        {
            FormFunc.SetText(this, SynusLangPack.GetLanguage("Frm_LogSet_FormTitle"));

            LabelFunc.SetText(lbl_CompressionCycle, SynusLangPack.GetLanguage("Label_CompressionCycle"));
            LabelFunc.SetText(lbl_DeleteCycle, SynusLangPack.GetLanguage("Label_DeleteCycle"));
            ButtonFunc.SetText(btn_Apply, SynusLangPack.GetLanguage("Btn_Apply"));
            ButtonFunc.SetText(btn_Close, SynusLangPack.GetLanguage("Btn_Cancel"));
        }

        private void btn_Apply_Click(object sender, EventArgs e)
        {
            bool bSave = false;

            if (ApplicationParam.m_LogParam.CompressionCycle != (cbx_CompressionCycle.SelectedIndex + 1))
            {
                ApplicationParam.m_LogParam.CompressionCycle = cbx_CompressionCycle.SelectedIndex + 1;
                bSave = true;
            }

            if (ApplicationParam.m_LogParam.ZIPDeleteCycle != (cbx_DeleteCycle.SelectedIndex + 1))
            {
                ApplicationParam.m_LogParam.ZIPDeleteCycle = cbx_DeleteCycle.SelectedIndex + 1;
                bSave = true;
            }

            if(Log.ShowType[LogMsg.LogType.Application] != chx_Application.Checked)
            {
                Log.ShowType[LogMsg.LogType.Application] = chx_Application.Checked;
                bSave = true;
            }

            if (Log.ShowType[LogMsg.LogType.Exception] != chx_Exception.Checked)
            {
                Log.ShowType[LogMsg.LogType.Exception] = chx_Exception.Checked;
                bSave = true;
            }

            if (Log.ShowType[LogMsg.LogType.Port] != chx_Port.Checked)
            {
                Log.ShowType[LogMsg.LogType.Port] = chx_Port.Checked;
                bSave = true;
            }

            if (Log.ShowType[LogMsg.LogType.RackMaster] != chx_RackMaster.Checked)
            {
                Log.ShowType[LogMsg.LogType.RackMaster] = chx_RackMaster.Checked;
                bSave = true;
            }

            if (Log.ShowType[LogMsg.LogType.CIM] != chx_CIM.Checked)
            {
                Log.ShowType[LogMsg.LogType.CIM] = chx_CIM.Checked;
                bSave = true;
            }

            if (Log.ShowType[LogMsg.LogType.Master] != chx_Master.Checked)
            {
                Log.ShowType[LogMsg.LogType.Master] = chx_Master.Checked;
                bSave = true;
            }

            if (Log.ShowType[LogMsg.LogType.WMX] != chx_WMX.Checked)
            {
                Log.ShowType[LogMsg.LogType.WMX] = chx_WMX.Checked;
                bSave = true;
            }

            if (Log.ShowType[LogMsg.LogType.CPS] != chx_CPS.Checked)
            {
                Log.ShowType[LogMsg.LogType.CPS] = chx_CPS.Checked;
                bSave = true;
            }

            if (Log.ShowLevel[LogMsg.LogLevel.Normal] != chx_Normal.Checked)
            {
                Log.ShowLevel[LogMsg.LogLevel.Normal] = chx_Normal.Checked;
                bSave = true;
            }

            if (Log.ShowLevel[LogMsg.LogLevel.Warning] != chx_Warning.Checked)
            {
                Log.ShowLevel[LogMsg.LogLevel.Warning] = chx_Warning.Checked;
                bSave = true;
            }

            if (Log.ShowLevel[LogMsg.LogLevel.Error] != chx_Error.Checked)
            {
                Log.ShowLevel[LogMsg.LogLevel.Error] = chx_Error.Checked;
                bSave = true;
            }


            if (bSave)
            {
                ApplicationParam.m_LogParam.LogShowType_Application = Log.ShowType[LogMsg.LogType.Application];
                ApplicationParam.m_LogParam.LogShowType_Exception = Log.ShowType[LogMsg.LogType.Exception];
                ApplicationParam.m_LogParam.LogShowType_Port = Log.ShowType[LogMsg.LogType.Port];
                ApplicationParam.m_LogParam.LogShowType_RackMaster = Log.ShowType[LogMsg.LogType.RackMaster];
                ApplicationParam.m_LogParam.LogShowType_CIM = Log.ShowType[LogMsg.LogType.CIM];
                ApplicationParam.m_LogParam.LogShowType_Master = Log.ShowType[LogMsg.LogType.Master];
                ApplicationParam.m_LogParam.LogShowType_WMX = Log.ShowType[LogMsg.LogType.WMX];
                ApplicationParam.m_LogParam.LogShowType_CPS = Log.ShowType[LogMsg.LogType.CPS];


                ApplicationParam.m_LogParam.LogShowType_Normal = Log.ShowLevel[LogMsg.LogLevel.Normal];
                ApplicationParam.m_LogParam.LogShowType_Warning = Log.ShowLevel[LogMsg.LogLevel.Warning];
                ApplicationParam.m_LogParam.LogShowType_Error = Log.ShowLevel[LogMsg.LogLevel.Error];

                ApplicationParam.SaveFile(ManagedFileInfo.AppSettingsDirectory, ManagedFileInfo.AppSettingsFileName);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
