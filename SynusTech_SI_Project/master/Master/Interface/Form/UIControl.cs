using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System;

class ControlFunc
{
    /// <summary>
    /// 모든 UI 객체 메모리 정리 위함
    /// </summary>
    /// <param name="control"></param>
    static public void Dispose(Control control)
    {
        try
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control Item in control.Controls)
                {
                    Dispose(Item);
                }
                control.Dispose();
            }
            else
                control.Dispose();
        }
        catch (Exception ex)
        {

        }
    }

    static public void ResizeFont(Control control, float fAdjFactor = 1.0f)
    {
        try
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control Item in control.Controls)
                {
                    ResizeFont(Item, fAdjFactor);
                }
                control.Font = new Font(control.Font.FontFamily, control.Font.Size * fAdjFactor, control.Font.Style); //- fAdj
            }
            else
                control.Font = new Font(control.Font.FontFamily, control.Font.Size * fAdjFactor, control.Font.Style); //- fAdj
        }
        catch(Exception ex)
        {

        }
    }
}
class FormFunc
{
    static public void SetText(Form form, string text)
    {
        if (form.Text != text)
            form.Text = text;
    }

    /// <summary>
    /// 모든 UI 객체 Double Buffer 적용 (UI 가속화)
    /// </summary>
    /// <param name="control"></param>
    static public void SetDoubleBuffer(Control control)
    {
        try
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control item in control.Controls)
                {
                    SetDoubleBuffer((Control)item);
                }
                control.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, control, new object[] { true });
            }
            else
                control.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, control, new object[] { true });
        }
        catch(Exception ex)
        {

        }
        //foreach(var item in control.Controls)
        //{
        //    item.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, item, new object[] { true });

        //    if (((Control)item).Controls.Count > 0)
        //        SetDoubleBuffer((Control)item);
        //}
    }
}

class StopWatchFunc
{
    /// <summary>
    /// Stop Watch 경과 시간 출력
    /// </summary>
    /// <param name="st"></param>
    /// <returns></returns>
    static public string GetRunningTime(Stopwatch st)
    {
        var RunningTime = st.Elapsed;

        return $"{RunningTime.TotalHours.ToString("00")}:{ RunningTime.Minutes.ToString("00")}:{ RunningTime.Seconds.ToString("00")}:{ RunningTime.Milliseconds.ToString("000")}";
    }
}
class DataGridViewFunc
{
    static public void AutoRowSize(DataGridView DGV, int ColumnHeight, int MinHeight, int MaxHeight)
    {
        int ColumnHeaderHeight = ColumnHeight;
        DGV.ColumnHeadersHeight = ColumnHeaderHeight;

        if (DGV.Rows.Count == 0)
            return;

        int RowHeight = ((DGV.Height - ColumnHeaderHeight) / (DGV.Rows.Count));

        if(RowHeight < MinHeight)
        {
            RowHeight = MinHeight;
            for (int nCount = 0; nCount < DGV.Rows.Count; nCount++)
                DGV.Rows[nCount].Height = RowHeight;
        }
        else if (RowHeight >= MaxHeight)
        {
            RowHeight = MaxHeight;
            for (int nCount = 0; nCount < DGV.Rows.Count; nCount++)
                DGV.Rows[nCount].Height = RowHeight;
        }
        else
        {
            for (int nCount = 0; nCount < DGV.Rows.Count; nCount++)
                DGV.Rows[nCount].Height = RowHeight;
        }
    }
    static public void SetSize(DataGridView DGV, int ColumnHeaderHeight, int RowHeight, int Offset)
    {
        DGV.Height = ColumnHeaderHeight + RowHeight * DGV.Rows.Count + Offset;

        if(ColumnHeaderHeight != 0)
            DGV.ColumnHeadersHeight = ColumnHeaderHeight;

        for (int nCount = 0; nCount < DGV.Rows.Count; nCount++)
            DGV.Rows[nCount].Height = RowHeight;
    }
}
class TabPageFunc
{
    static public void SetText(TabPage tabpage, string text)
    {
        if (tabpage.Text != text)
            tabpage.Text = text;
    }
}
class LabelFunc
{
    static public void SetText(Label label, string text)
    {
        if (label.Text != text)
            label.Text = text;
    }
    static public void SetForeColor(Label label, Color color)
    {
        if (label.ForeColor != color)
            label.ForeColor = color;
    }
    static public void SetBackColor(Label label, Color color)
    {
        if (label.BackColor != color)
            label.BackColor = color;
    }
    static public void SetLocation(Label label, Point point)
    {
        if (label.Location != point)
            label.Location = point;
    }
    static public void SetImage(Label label, Image image)
    {
        if (label.Image != image)
            label.Image = image;
    }
    static public void SetVisible(Label label, bool bEnable)
    {
        if (label.Visible != bEnable)
            label.Visible = bEnable;
    }
}
class ButtonFunc
{
    static public void SetText(Button button, string text)
    {
        if (button.Text != text)
            button.Text = text;
    }
    static public void SetImage(Button button, Image image)
    {
        if (button.Image != image)
            button.Image = image;
    }
    static public void SetBackColor(Button button, Color color)
    {
        if (button.BackColor != color)
            button.BackColor = color;
    }
    static public void SetEnable(Button button, bool bEnable)
    {
        if (button.Enabled != bEnable)
            button.Enabled = bEnable;
    }
    static public void SetVisible(Button button, bool bEnable)
    {
        if (button.Visible != bEnable)
            button.Visible = bEnable;
    }
}
class GroupBoxFunc
{
    static public void SetText(GroupBox groupbox, string text)
    {
        if (groupbox.Text != text)
            groupbox.Text = text;
    }
    static public void SetVisible(GroupBox groupbox, bool bEnable)
    {
        if (groupbox.Visible != bEnable)
            groupbox.Visible = bEnable;
    }
    static public void SetEnable(GroupBox groupbox, bool bEnable)
    {
        if (groupbox.Enabled != bEnable)
            groupbox.Enabled = bEnable;
    }
    static public void SetHeight(GroupBox groupbox, int Height)
    {
        if (groupbox.Height != Height)
            groupbox.Height = Height;
    }
}
class TableLayoutPanelFunc
{
    static public void SetHeight(TableLayoutPanel tablelayoutpanel, int Height)
    {
        if (tablelayoutpanel.Height != Height)
            tablelayoutpanel.Height = Height;
    }
}
class PanelFunc
{
    static public void SetVisible(Panel panel, bool bEnable)
    {
        if (panel.Visible != bEnable)
            panel.Visible = bEnable;
    }
}


