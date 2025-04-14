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
    public partial class Frm_MotionSelect : Form
    {
        Frm_ServoMotion frm_ServoMotion = new Frm_ServoMotion() { TopLevel = false };
        Frm_InverterMotion frm_InverterMotion = new Frm_InverterMotion() { TopLevel = false };
        Frm_CylinderMotion frm_CylinderMotion = new Frm_CylinderMotion() { TopLevel = false };
        Frm_ConveyorMotion frm_ConveyorMotion = new Frm_ConveyorMotion() { TopLevel = false };

        object CurrentTag = null;
        public Frm_MotionSelect()
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

                    foreach(var port in Master.m_Ports)
                    {
                        port.Value.m_bInterlockOff = false;
                    }
                }
            };

            this.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                frm_ServoMotion.Visible = false;
                frm_InverterMotion.Visible = false;
                frm_CylinderMotion.Visible = false;
                frm_ConveyorMotion.Visible = false;

                frm_ServoMotion.Close();
                frm_InverterMotion.Close();
                frm_CylinderMotion.Close();
                frm_ConveyorMotion.Close();
            };

            this.Disposed += (object sender, EventArgs e) =>
            {
                UIUpdateTimer.Enabled = false;

                frm_ServoMotion.Dispose();
                frm_InverterMotion.Dispose();
                frm_CylinderMotion.Dispose();
                frm_ConveyorMotion.Dispose();

                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }

        private void ControlItemInit()
        {
            FormFunc.SetDoubleBuffer(this);

            ContextMenu ctx = new ContextMenu();

            MenuItem item = new MenuItem();
            item.Text = "Interlock Off";
            item.Click += btn_Interlock_Off;
            ctx.MenuItems.Add(item);
            MenuItem item1 = new MenuItem();
            item1.Text = "Interlock On";
            item1.Click += btn_Interlock_On;
            ctx.MenuItems.Add(item1);
            groupBox_PortControlCommand.ContextMenu = ctx;
        }
        public void SetAutoScale(float FactorX, float FactorY)
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.Scale(new SizeF(FactorX, FactorY));

            foreach (Control item in this.Controls)
                ControlFunc.ResizeFont(item,FactorY);

            frm_ServoMotion.SetAutoScale(FactorX, FactorY);
            frm_InverterMotion.SetAutoScale(FactorX, FactorY);
            frm_CylinderMotion.SetAutoScale(FactorX, FactorY);
            frm_ConveyorMotion.SetAutoScale(FactorX, FactorY);
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
                GroupBoxFunc.SetText(groupBox_ControlAxisSelection, SynusLangPack.GetLanguage("GorupBox_ControlAxisSelection"));
                GroupBoxFunc.SetText(groupBox_PortControlCommand, SynusLangPack.GetLanguage("GorupBox_PortControl"));
                Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

                if (CurrentPort != null)
                {
                    Update_PortControlList(CurrentPort);
                    Update_Btn_Focus();
                }

            }
            catch{ }
            finally
            {

            }
        }

        private void Update_PortControlList(Port port)
        {
            if(tableLayoutPanel_ControlListInfo.Tag != port)
            {
                if (tableLayoutPanel_ControlListInfo.Controls.Count > 0)
                {
                    for (int nCount = 0; nCount < tableLayoutPanel_ControlListInfo.Controls.Count; nCount++)
                        tableLayoutPanel_ControlListInfo.Controls[nCount].Dispose();

                    tableLayoutPanel_ControlListInfo.Controls.Clear();
                }

                tableLayoutPanel_ControlListInfo.Tag = port;
                pnl_MotionControl.Tag = null;
                pnl_MotionControl.Controls.Clear();

                List<Button> ControlButtonList = new List<Button>();

                foreach (Port.PortAxis ePortAxis in Enum.GetValues(typeof(Port.PortAxis)))
                {
                    if (!port.GetMotionParam().IsAxisUnUsed(ePortAxis))
                    {
                        Button btn = new Button();
                        if (port.GetMotionParam().IsServoType(ePortAxis))
                            btn.Text = $"{ePortAxis} [ Servo ]";
                        else if (port.GetMotionParam().IsCylinderType(ePortAxis))
                            btn.Text = $"{ePortAxis} [ Cylinder ]";
                        else if (port.GetMotionParam().IsInverterType(ePortAxis))
                            btn.Text = $"{ePortAxis} [ Inverter ]";

                        float FontSize = groupBox_ControlAxisSelection.Font.Size >= 10.0f ? 8.0f : 6.0f;

                        btn.Font = new Font("Segoe UI Semibold", FontSize, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                        btn.BackColor = Color.White;
                        btn.Margin = new Padding(1,1,1,1);
                        btn.Tag = ePortAxis;
                        btn.Click += Btn_Click;

                        ControlButtonList.Add(btn);
                    }
                }

                foreach (Port.BufferCV eBufferCV in Enum.GetValues(typeof(Port.BufferCV)))
                {
                    if (port.GetMotionParam().IsCVUsed(eBufferCV))
                    {
                        Button btn = new Button();
                        btn.Text = $"{eBufferCV} [ Conveyor ]";
                        btn.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                        btn.BackColor = Color.White;
                        btn.Margin = new Padding(1, 1, 1, 1);
                        btn.Tag = eBufferCV;
                        btn.Click += Btn_Click;
                        ControlButtonList.Add(btn);
                    }
                }

                //Column Setting
                tableLayoutPanel_ControlListInfo.ColumnCount = 4;
                for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_ControlListInfo.ColumnCount; nColumnCount++)
                {
                    tableLayoutPanel_ControlListInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                }
                //Row Setting
                tableLayoutPanel_ControlListInfo.RowCount = (ControlButtonList.Count % 4 == 0 ? ControlButtonList.Count / 4 : ControlButtonList.Count / 4 + 1);
                for (int nRowCount = 0; nRowCount < tableLayoutPanel_ControlListInfo.RowCount; nRowCount++)
                {
                    tableLayoutPanel_ControlListInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                }

                tableLayoutPanel_ControlListInfo.Dock = DockStyle.Fill;

                int nButtonCount = 0;
                for (int nRowCount = 0; nRowCount < tableLayoutPanel_ControlListInfo.RowCount; nRowCount++)
                {
                    for (int nColumnCount = 0; nColumnCount < tableLayoutPanel_ControlListInfo.ColumnCount; nColumnCount++)
                    {
                        if (nButtonCount >= ControlButtonList.Count)
                            continue;

                        tableLayoutPanel_ControlListInfo.Controls.Add(ControlButtonList[nButtonCount], nColumnCount, nRowCount);
                        ControlButtonList[nButtonCount].Dock = DockStyle.Fill;
                        nButtonCount++;
                    }
                }

                if (tableLayoutPanel_ControlListInfo.Controls.Count > 0)
                    Btn_Click((Button)tableLayoutPanel_ControlListInfo.Controls[0], new EventArgs());

                groupBox_ControlAxisSelection.Height = (tableLayoutPanel_ControlListInfo.RowCount * 40) + 30;
            }
        }
        private void btn_Interlock_Off(object sender, EventArgs eventArgs)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            CurrentPort.m_bInterlockOff = true;
        }
        private void btn_Interlock_On(object sender, EventArgs eventArgs)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            CurrentPort.m_bInterlockOff = false;
        }
        
        private void Btn_Click(object sender, EventArgs e)
        {
            Port CurrentPort = (Port)Frm_PortTPScreen.g_CurrentPort;

            if (CurrentPort == null)
                return;

            Button btn = (Button)sender;

            if (btn.Tag.GetType() == typeof(Port.PortAxis))
            {
                Port.PortAxis ePortAxis = (Port.PortAxis)btn.Tag;

                if (CurrentPort.GetMotionParam().IsServoType(ePortAxis))
                {
                    frm_ServoMotion.SetControlType(ePortAxis);

                    if (pnl_MotionControl.Tag != frm_ServoMotion)
                    {
                        pnl_MotionControl.Controls.Clear();
                        pnl_MotionControl.Tag = frm_ServoMotion;
                        pnl_MotionControl.Controls.Add(frm_ServoMotion);
                        pnl_MotionControl.Controls[0].Dock = DockStyle.Fill;
                        pnl_MotionControl.Controls[0].Show();
                    }
                }
                else if (CurrentPort.GetMotionParam().IsCylinderType(ePortAxis))
                {
                    frm_CylinderMotion.SetControlType(ePortAxis);

                    if (pnl_MotionControl.Tag != frm_CylinderMotion)
                    {
                        pnl_MotionControl.Controls.Clear();
                        pnl_MotionControl.Tag = frm_CylinderMotion;
                        pnl_MotionControl.Controls.Add(frm_CylinderMotion);
                        pnl_MotionControl.Controls[0].Dock = DockStyle.Fill;
                        pnl_MotionControl.Controls[0].Show();
                    }
                }
                else if (CurrentPort.GetMotionParam().IsInverterType(ePortAxis))
                {
                    frm_InverterMotion.SetControlType(ePortAxis);

                    if (pnl_MotionControl.Tag != frm_InverterMotion)
                    {
                        pnl_MotionControl.Controls.Clear();
                        pnl_MotionControl.Tag = frm_InverterMotion;
                        pnl_MotionControl.Controls.Add(frm_InverterMotion);
                        pnl_MotionControl.Controls[0].Dock = DockStyle.Fill;
                        pnl_MotionControl.Controls[0].Show();
                    }
                }
                else
                {
                    pnl_MotionControl.Tag = null;
                    pnl_MotionControl.Controls.Clear();
                }
            }
            else if(btn.Tag.GetType() == typeof(Port.BufferCV))
            {
                Port.BufferCV eBufferCV = (Port.BufferCV)btn.Tag;

                frm_ConveyorMotion.SetControlType(eBufferCV);

                if (pnl_MotionControl.Tag != frm_ConveyorMotion)
                {
                    pnl_MotionControl.Controls.Clear();
                    pnl_MotionControl.Tag = frm_ConveyorMotion;
                    pnl_MotionControl.Controls.Add(frm_ConveyorMotion);
                    pnl_MotionControl.Controls[0].Dock = DockStyle.Fill;
                    pnl_MotionControl.Controls[0].Show();
                }
            }
            else
            {
                pnl_MotionControl.Tag = null;
                pnl_MotionControl.Controls.Clear();
            }
        }
        private void Update_Btn_Focus()
        {
            if(pnl_MotionControl.Tag == null)
            {
                CurrentTag = null;
                return;
            }



            if (pnl_MotionControl.Tag.GetType() == typeof(Frm_ServoMotion))
            {
                CurrentTag = frm_ServoMotion.GetControlType();
            }
            else if (pnl_MotionControl.Tag.GetType() == typeof(Frm_InverterMotion))
            {
                CurrentTag = frm_InverterMotion.GetControlType();
            }
            else if (pnl_MotionControl.Tag.GetType() == typeof(Frm_CylinderMotion))
            {
                CurrentTag = frm_CylinderMotion.GetControlType();
            }
            else if (pnl_MotionControl.Tag.GetType() == typeof(Frm_ConveyorMotion))
            {
                CurrentTag = frm_ConveyorMotion.GetControlType();
            }
            else
                CurrentTag = null;

            for(int nCount = 0; nCount < tableLayoutPanel_ControlListInfo.Controls.Count; nCount++)
            {
                if(tableLayoutPanel_ControlListInfo.Controls[nCount].GetType() == typeof(Button))
                {
                    Button btn = (Button)tableLayoutPanel_ControlListInfo.Controls[nCount];

                    if (btn.Tag.GetType() == CurrentTag.GetType())
                    {
                        if(btn.Tag.GetType() == typeof(Port.PortAxis) && CurrentTag.GetType() == typeof(Port.PortAxis))
                        {
                            if ((Port.PortAxis)btn.Tag == (Port.PortAxis)CurrentTag)
                                btn.BackColor = Color.Lime;
                            else
                                btn.BackColor = Color.White;
                        }
                        else if (btn.Tag.GetType() == typeof(Port.BufferCV) && CurrentTag.GetType() == typeof(Port.BufferCV))
                        {
                            if ((Port.BufferCV)btn.Tag == (Port.BufferCV)CurrentTag)
                                btn.BackColor = Color.Lime;
                            else
                                btn.BackColor = Color.White;
                        }
                        else
                            btn.BackColor = Color.White;
                    }
                    else
                        btn.BackColor = Color.White;
                }
            }
        }
        public object GetCurrentTag()
        {
            return CurrentTag;
        }
        public object GetCurrentPortAxis()
        {
            if (pnl_MotionControl.Tag == null)
            {
                return null;
            }

            if (pnl_MotionControl.Tag.GetType() == typeof(Frm_ServoMotion))
            {
                return frm_ServoMotion.GetControlType();
            }
            else
            {
                return null;
            }
        }
    }
}
