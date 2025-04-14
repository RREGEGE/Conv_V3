using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Master.Equipment.Port;

namespace Master.SubForm.PortTPForm.PortTPSubForm.ControlForm
{
    public partial class Frm_CylinderMotion : Form
    {
        Port.PortAxis m_eSelectedPortAxis;
        public Frm_CylinderMotion()
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
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);
        }
        public void SetControlType(Port.PortAxis type)
        {
            m_eSelectedPortAxis = type;
        }
        public Port.PortAxis GetControlType()
        {
            return m_eSelectedPortAxis;
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
                GroupBoxFunc.SetText(groupBox_MotionCommand, SynusLangPack.GetLanguage("GorupBox_CylinderCommand"));
                GroupBoxFunc.SetText(groupBox_Move, SynusLangPack.GetLanguage("GorupBox_Move"));
                GroupBoxFunc.SetText(groupBox_CylinderFlag, SynusLangPack.GetLanguage("GorupBox_Flag"));

                LabelFunc.SetText(lbl_BWD1, SynusLangPack.GetLanguage("Label_BWD"));
                LabelFunc.SetText(lbl_FWD1, SynusLangPack.GetLanguage("Label_FWD"));
                ButtonFunc.SetText(btn_Stop, SynusLangPack.GetLanguage("Btn_CylinderStop"));
            }
            catch{ }
            finally
            {

            }
        }

        private void btn_CylinderMove_MouseDown(object sender, MouseEventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = "Push";

            if (btn == btn_FWD1)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Cyl FWD Run Click");
                CurrentPort.Interlock_SetCylinderMove(m_eSelectedPortAxis, Port.CylCtrlList.FWD, true, false, Port.InterlockFrom.UI_Event);
            }
            else if (btn == btn_BWD1)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Cyl BWD Run Click");
                CurrentPort.Interlock_SetCylinderMove(m_eSelectedPortAxis, Port.CylCtrlList.BWD, true, false, Port.InterlockFrom.UI_Event);
            }
        }

        private void btn_CylinderMove_MouseUp(object sender, MouseEventArgs e)
        {
            btn_Stop_Click(sender, e);
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;
            btn.Tag = null;

            if (btn == btn_FWD1)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Cyl FWD Stop Click");
            }
            else if (btn == btn_BWD1)
            {
                LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Cyl BWD Stop Click");
            }

            if (CurrentPort.GetMotionParam().IsCylinderType(m_eSelectedPortAxis))
                CurrentPort.Interlock_CylinderMotionStop(m_eSelectedPortAxis, false, Port.InterlockFrom.UI_Event);
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if ((string)btn.Tag == "Push")
            {
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort == null)
                    return;

                btn.Tag = null;

                if (btn == btn_FWD1 || btn == btn_BWD1)
                {
                    LogMsg.AddApplicationLog(LogMsg.LogLevel.Normal, LogMsg.MsgList.ButtonClick, $"Port Control Form-Port[{CurrentPort.GetParam().ID}] {m_eSelectedPortAxis} Cyl Stop (Mouse Leave)");
                    
                    if (CurrentPort.GetMotionParam().IsCylinderType(m_eSelectedPortAxis))
                        CurrentPort.Interlock_CylinderMotionStop(m_eSelectedPortAxis, false, Port.InterlockFrom.UI_Event);
                }
            }
        }
    }
}
