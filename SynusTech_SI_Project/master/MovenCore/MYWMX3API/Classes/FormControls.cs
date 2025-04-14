using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace MYWMX3API.Classes
{
    static public class FormControls
    {
        static public void ApplyDoubleBuffer(Form form)
        {
            form.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, form, new object[] { true });

            foreach (var Controlitem in form.Controls)
            {
                ApplyDoubleBuffer((Control)Controlitem);
            }
        }

        static public void ApplyDoubleBuffer(Control item)
        {
            item.GetType().InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, item, new object[] { true });

            foreach (var Controlitem in item.Controls)
            {
                ApplyDoubleBuffer((Control)Controlitem);
            }
        }

        static public void LabelTextChange(Label pLabel, string Text)
        {
            if (pLabel == null)
                return;

            if (pLabel.Text != Text)
                pLabel.Text = Text;
        }
        static public void LabelForeColorChange(Label pLabel, Color color)
        {
            if (pLabel == null)
                return;

            if (pLabel.ForeColor != color)
                pLabel.ForeColor = color;
        }
        static public void LabelBackColorChange(Label pLabel, Color color)
        {
            if (pLabel == null)
                return;

            if (pLabel.BackColor != color)
                pLabel.BackColor = color;
        }
        static public void LabelVisibleChange(Label pLabel, bool bEnable)
        {
            if (pLabel == null)
                return;

            if (pLabel.Visible != bEnable)
                pLabel.Visible = bEnable;
        }

        static public void CheckBoxEnableChange(CheckBox pCheckBox, bool bEnable)
        {
            if (pCheckBox == null)
                return;

            if (pCheckBox.Enabled != bEnable)
                pCheckBox.Enabled = bEnable;
        }
        static public void ButtonTextChange(Button pButton, string text)
        {
            if (pButton == null)
                return;

            if (pButton.Text != text)
                pButton.Text = text;
        }
        static public void ButtonForeColorChange(Button pButton, Color color)
        {
            if (pButton == null)
                return;

            if (pButton.ForeColor != color)
                pButton.ForeColor = color;
        }
        static public void ButtonBackColorChange(Button pButton, Color color)
        {
            if (pButton == null)
                return;

            if (pButton.BackColor != color)
                pButton.BackColor = color;
        }
        static public void ButtonImageChange(Button pButton, Image image)
        {
            if (pButton == null)
                return;

            if (pButton.Image != image)
                pButton.Image = image;
        }
        static public void ButtonFlatAppearanceBorderColorChange(Button pButton, Color color)
        {
            if (pButton == null)
                return;

            if (pButton.FlatAppearance.BorderColor != color)
                pButton.FlatAppearance.BorderColor = color;
        }
        static public void ButtonEnableChange(Button pButton, bool bEnable)
        {
            if (pButton == null)
                return;

            if (pButton.Enabled != bEnable)
                pButton.Enabled = bEnable;
        }
        static public void TextBoxTextChange(TextBox pTextBox, string text)
        {
            if (pTextBox == null)
                return;

            if (pTextBox.Text != text)
                pTextBox.Text = text;
        }
        static public void TextBoxForeColorChange(TextBox pTextBox, Color color)
        {
            if (pTextBox == null)
                return;

            if (pTextBox.ForeColor != color)
                pTextBox.ForeColor = color;
        }

        static public void DataGridViewVisibleChange(DataGridView pDataGridView, bool bEnable)
        {
            if (pDataGridView == null)
                return;

            if (pDataGridView.Visible != bEnable)
                pDataGridView.Visible = bEnable;
        }
        static public void DataGridViewCellBackColorChange(DataGridViewCell pDgvCell, Color color)
        {
            if (pDgvCell == null)
                return;

            if (pDgvCell.Style.BackColor != color)
                pDgvCell.Style.BackColor = color;
        }
        static public void DataGridViewCellForeColorChange(DataGridViewCell pDgvCell, Color color)
        {
            if (pDgvCell == null)
                return;

            if (pDgvCell.Style.ForeColor != color)
                pDgvCell.Style.ForeColor = color;
        }

        delegate void Button_Enable_Callback(ref Button pButton, bool bEnable);
        public static void Button_Enable_Handler(ref Button pButton, bool bEnable)
        {
            if (pButton.InvokeRequired)
            {
                pButton.Invoke(new Button_Enable_Callback(Button_SetEnable), new object[] { pButton, bEnable });
            }
            else
                Button_SetEnable(ref pButton, bEnable);
        }
        private static void Button_SetEnable(ref Button pButton, bool bEnable)
        {
            if (pButton.Enabled != bEnable)
                pButton.Enabled = bEnable;
        }

        delegate void Button_TextChange_Callback(ref Button pButton, string text);
        public static void Button_TextChange_Handler(ref Button pButton, string text)
        {
            if (pButton.InvokeRequired)
            {
                pButton.Invoke(new Button_TextChange_Callback(Button_TextChange), new object[] { pButton, text });
            }
            else
                Button_TextChange(ref pButton, text);
        }
        private static void Button_TextChange(ref Button pButton, string text)
        {
            if (pButton.Text != text)
                pButton.Text = text;
        }

        delegate void Button_Image_Callback(ref Button pButton, Bitmap bitmap);
        public static void Button_Image_Handler(ref Button pButton, Bitmap bitmap)
        {
            if (pButton.InvokeRequired)
            {
                pButton.Invoke(new Button_Image_Callback(Button_Image), new object[] { pButton, bitmap });
            }
            else
                Button_Image(ref pButton, bitmap);
        }
        private static void Button_Image(ref Button pButton, Bitmap bitmap)
        {
            if (pButton.Image != bitmap)
                pButton.Image = bitmap;
        }

        delegate void Label_Text_Callback(ref Label pLabel, string value);
        public static void Label_Text_Handler(ref Label pLabel, string value)
        {
            if (pLabel.InvokeRequired)
            {
                pLabel.Invoke(new Label_Text_Callback(Label_Text), new object[] { pLabel, value });
            }
            else
                Label_Text(ref pLabel, value);
        }
        private static void Label_Text(ref Label pLabel, string value)
        {
            if (pLabel.Text != value)
                pLabel.Text = value;
        }

        delegate void Label_BackColor_Callback(ref Label pLabel, Color color);
        public static void Label_BackColor_Handler(ref Label pLabel, Color color)
        {
            if (pLabel.InvokeRequired)
            {
                pLabel.Invoke(new Label_BackColor_Callback(Label_BackColor), new object[] { pLabel, color });
            }
            else
                Label_BackColor(ref pLabel, color);
        }
        private static void Label_BackColor(ref Label pLabel, Color color)
        {
            if (pLabel.BackColor != color)
                pLabel.BackColor = color;
        }

        delegate void Label_ForeColor_Callback(ref Label pLabel, Color color);
        public static void Label_ForeColor_Handler(ref Label pLabel, Color color)
        {
            if (pLabel.InvokeRequired)
            {
                pLabel.Invoke(new Label_ForeColor_Callback(Label_ForeColor), new object[] { pLabel, color });
            }
            else
                Label_ForeColor(ref pLabel, color);
        }
        private static void Label_ForeColor(ref Label pLabel, Color color)
        {
            if (pLabel.ForeColor != color)
                pLabel.ForeColor = color;
        }

        delegate void Progress_ValueChanged_Callback(ref ProgressBar pProgress, int value);
        public static void ProgressBar_value_Handler(ref ProgressBar pProgress, int value)
        {
            if (pProgress.InvokeRequired)
            {
                pProgress.Invoke(new Progress_ValueChanged_Callback(ProgressBar_SetValue), new object[] { pProgress, value });
            }
            else
                ProgressBar_SetValue(ref pProgress, value);
        }
        private static void ProgressBar_SetValue(ref ProgressBar pProgress, int value)
        {
            pProgress.Value = value;
        }

        delegate string ComboBox_GetItemText_Callback(ref ComboBox pComboBox);

        public static string ComboBox_GetItemText_Handler(ref ComboBox pComboBox)
        {
            if(pComboBox.InvokeRequired)
            {
                return (string)pComboBox.Invoke(new ComboBox_GetItemText_Callback(ComboBox_GetItemText), new object[] { pComboBox });
            }
            else
                return ComboBox_GetItemText(ref pComboBox);
        }
        private static string ComboBox_GetItemText(ref ComboBox pComboBox)
        {
            return pComboBox.GetItemText(pComboBox.SelectedItem);
        }

        delegate void Panel_LocationChanged_Callback(ref Panel pPanel, int X, int Y);
        public static void Panel_Location_Handler(ref Panel pPanel, int X, int Y)
        {
            if (pPanel.InvokeRequired)
            {
                pPanel.Invoke(new Panel_LocationChanged_Callback(Panel_SetLocation), new object[] { pPanel, X, Y });
            }
            else
                Panel_SetLocation(ref pPanel, X, Y);
        }
        private static void Panel_SetLocation(ref Panel pPanel, int X, int Y)
        {
            pPanel.Location = new System.Drawing.Point(X, Y);
        }

        delegate void Panel_Enable_Callback(ref Panel pPanel, bool bEnable);
        public static void Panel_Enable_Handler(ref Panel pPanel, bool bEnable)
        {
            if (pPanel.InvokeRequired)
            {
                pPanel.Invoke(new Panel_Enable_Callback(Panel_Enable), new object[] { pPanel, bEnable });
            }
            else
                Panel_Enable(ref pPanel, bEnable);
        }
        private static void Panel_Enable(ref Panel pPanel, bool bEnable)
        {
            if (pPanel.Enabled != bEnable)
                pPanel.Enabled = bEnable;
        }

        delegate int ListBox_SelectedIndex_Callback(ref ListBox pListBox);
        public static int ListBox_SeletedIdex_Handler(ref ListBox pListBox)
        {
            if (pListBox.InvokeRequired)
            {
                return (int)pListBox.Invoke(new ListBox_SelectedIndex_Callback(ListBox_SelectedIndex), new object[] { pListBox });
            }
            else
                return ListBox_SelectedIndex(ref pListBox);
        }
        private static int ListBox_SelectedIndex(ref ListBox pListBox)
        {
            return pListBox.SelectedIndex;
        }

        delegate void ListBox_AddItem_Callback(ref ListBox pListBox, string text);
        public static void ListBox_AddItem_Handler(ref ListBox pListBox, string text)
        {
            if (pListBox.InvokeRequired)
            {
                pListBox.Invoke(new ListBox_AddItem_Callback(ListBox_AddItem), new object[] { pListBox, text});
            }
            else
                ListBox_AddItem(ref pListBox, text);
        }
        private static void ListBox_AddItem(ref ListBox pListBox, string text)
        {
            pListBox.Items.Add(text);
        }

        delegate void ListBox_Clear_Callback(ref ListBox pListBox);
        public static void ListBox_Clear_Handler(ref ListBox pListBox)
        {
            if (pListBox.InvokeRequired)
            {
                pListBox.Invoke(new ListBox_Clear_Callback(ListBox_Clear), new object[] { pListBox });
            }
            else
                ListBox_Clear(ref pListBox);
        }
        private static void ListBox_Clear(ref ListBox pListBox)
        {
            pListBox.Items.Clear();
        }

        delegate void RichTextBox_Text_Callback(ref RichTextBox prichTextBox, string text);
        public static void RichTextBox_Text_Handler(ref RichTextBox prichTextBox, string text)
        {
            if (prichTextBox.InvokeRequired)
            {
                prichTextBox.Invoke(new RichTextBox_Text_Callback(RichTextBox_SetText), new object[] { prichTextBox, text });
            }
            else
                RichTextBox_SetText(ref prichTextBox, text);
        }
        private static void RichTextBox_SetText(ref RichTextBox richTextBox, string text)
        {
            richTextBox.AppendText($"{text}\n");
            richTextBox.ScrollToCaret();
        }
        delegate void TreeView_Clear_Callback(ref TreeView pTreeView);
        public static void TreeView_Clear_Handler(ref TreeView pTreeView)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_Clear_Callback(TreeView_Clear), new object[] { pTreeView });
            }
            else
                TreeView_Clear(ref pTreeView);
        }
        private static void TreeView_Clear(ref TreeView pTreeView)
        {
            pTreeView.Nodes.Clear();
            //pTreeView.Refresh();
        }

        delegate void TreeView_NodeAdd_Callback(ref TreeView pTreeView, TreeNode tNode);
        public static void TreeView_NodeAdd_Handler(ref TreeView pTreeView, TreeNode tNode)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_NodeAdd_Callback(TreeView_NodeAdd), new object[] { pTreeView, tNode });
            }
            else
                TreeView_NodeAdd(ref pTreeView, tNode);
        }
        private static void TreeView_NodeAdd(ref TreeView pTreeView, TreeNode tNode)
        {
            pTreeView.Nodes.Add(tNode);
        }

        delegate void TreeNode_RemoveAt_Callback(ref TreeNode pTreeNode, int index);
        public static void TreeNode_NodeRemoveAt_Handler(ref TreeNode pTreeNode, int index)
        {
            if (pTreeNode.TreeView.InvokeRequired)
            {
                pTreeNode.TreeView.Invoke(new TreeNode_RemoveAt_Callback(TreeNode_NodeRemoveAt), new object[] { pTreeNode, index });
            }
            else
                TreeNode_NodeRemoveAt(ref pTreeNode, index);
        }
        private static void TreeNode_NodeRemoveAt(ref TreeNode pTreeNode, int index)
        {
            pTreeNode.Nodes.RemoveAt(index);
        }

        delegate void TreeView_BeginUpdate_Callback(ref TreeView pTreeView);
        public static void TreeView_BeginUpdate_Handler(ref TreeView pTreeView)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_BeginUpdate_Callback(TreeView_BeginUpdate), new object[] { pTreeView });
            }
            else
                TreeView_BeginUpdate(ref pTreeView);
        }
        private static void TreeView_BeginUpdate(ref TreeView pTreeView)
        {
            pTreeView.BeginUpdate();
        }

        delegate void TreeView_EndUpdate_Callback(ref TreeView pTreeView);
        public static void TreeView_EndUpdate_Handler(ref TreeView pTreeView)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_EndUpdate_Callback(TreeView_EndUpdate), new object[] { pTreeView });
            }
            else
                TreeView_EndUpdate(ref pTreeView);
        }
        private static void TreeView_EndUpdate(ref TreeView pTreeView)
        {
            pTreeView.EndUpdate();
        }

        delegate void TreeView_NodeAddKeyText_Callback(ref TreeView pTreeView, string key, string text);
        public static void TreeView_NodeAddKeyText_Handler(ref TreeView pTreeView, string key, string text)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_NodeAddKeyText_Callback(TreeView_NodeAddKeyText), new object[] { pTreeView, key, text });
            }
            else
                TreeView_NodeAddKeyText(ref pTreeView, key, text);
        }
        private static void TreeView_NodeAddKeyText(ref TreeView pTreeView, string key, string text)
        {
            pTreeView.Nodes.Add(key, text);
        }

        delegate void TreeView_ExpandAll_Callback(ref TreeView pTreeView);
        public static void TreeView_ExpandAll_Handler(ref TreeView pTreeView)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_ExpandAll_Callback(TreeView_ExpandAll), new object[] { pTreeView });
            }
            else
                TreeView_ExpandAll(ref pTreeView);
        }
        private static void TreeView_ExpandAll(ref TreeView pTreeView)
        {
            pTreeView.ExpandAll();
        }
        delegate void TreeView_EnsureVisible_Callback(ref TreeView pTreeView);
        public static void TreeView_EnsureVisible_Handler(ref TreeView pTreeView)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_EnsureVisible_Callback(TreeView_EnsureVisible), new object[] { pTreeView });
            }
            else
                TreeView_EnsureVisible(ref pTreeView);
        }
        private static void TreeView_EnsureVisible(ref TreeView pTreeView)
        {
            if (pTreeView.Nodes.Count > 0)
                pTreeView.Nodes[0].EnsureVisible();
        }

        delegate void TreeNode_Expand_Callback(ref TreeNode pTreeNode);
        public static void TreeNode_Expand_Handler(ref TreeNode pTreeNode)
        {
            if (pTreeNode.TreeView.InvokeRequired)
            {
                pTreeNode.TreeView.Invoke(new TreeNode_Expand_Callback(TreeNode_Expand), new object[] { pTreeNode });
            }
            else
                TreeNode_Expand(ref pTreeNode);
        }
        private static void TreeNode_Expand(ref TreeNode pTreeNode)
        {
            pTreeNode.Expand();
        }

        delegate void TreeNode_Add_Callback(ref TreeNode pTreeNode, TreeNode tNode);
        public static void TreeNode_Add_Handler(ref TreeNode pTreeNode, TreeNode tNode)
        {
            if (pTreeNode.TreeView.InvokeRequired)
            {
                pTreeNode.TreeView.Invoke(new TreeNode_Add_Callback(TreeNode_Add), new object[] { pTreeNode, tNode });
            }
            else
                TreeNode_Add(ref pTreeNode, tNode);
        }
        private static void TreeNode_Add(ref TreeNode pTreeNode, TreeNode tNode)
        {
            pTreeNode.Nodes.Add(tNode);
        }

        delegate void TreeNode_AddKeyText_Callback(ref TreeNode pTreeNode, string key, string text);
        public static void TreeNode_AddKeyText_Handler(ref TreeNode pTreeNode, string key, string text)
        {
            if (pTreeNode.TreeView.InvokeRequired)
            {
                pTreeNode.TreeView.Invoke(new TreeNode_AddKeyText_Callback(TreeNode_AddKeyText), new object[] { pTreeNode, key, text });
            }
            else
                TreeNode_AddKeyText(ref pTreeNode, key, text);
        }
        private static void TreeNode_AddKeyText(ref TreeNode pTreeNode, string key, string text)
        {
            pTreeNode.Nodes.Add(key, text);
        }

        delegate void TreeNode_SetText_Callback(ref TreeNode pTreeNode, string text);
        public static void TreeNode_SetText_Handler(ref TreeNode pTreeNode, string text)
        {
            if (pTreeNode.TreeView.InvokeRequired)
            {
                pTreeNode.TreeView.Invoke(new TreeNode_SetText_Callback(TreeNode_SetText), new object[] { pTreeNode, text });
            }
            else
                TreeNode_SetText(ref pTreeNode, text);
        }
        private static void TreeNode_SetText(ref TreeNode pTreeNode, string text)
        {
            pTreeNode.Text = text;
        }

        delegate void TreeView_NodeSelect_Callback(ref TreeView pTreeView, int Master, int Slave);
        public static void TreeView_NodeSelect_Handler(ref TreeView pTreeView, int Master, int Slave)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_NodeSelect_Callback(TreeView_NodeSelect), new object[] { pTreeView, Master, Slave });
            }
            else
                TreeView_NodeSelect(ref pTreeView, Master, Slave);
        }
        private static void TreeView_NodeSelect(ref TreeView pTreeView, int Master, int Slave)
        {
            if (Master < pTreeView.Nodes.Count)
            {
                if(Slave < pTreeView.Nodes[Master].Nodes.Count)
                    pTreeView.SelectedNode = pTreeView.Nodes[Master].Nodes[Slave];
            }
        }

        delegate void TreeView_Enable_Callback(ref TreeView pTreeView, bool bEnable);
        public static void TreeView_Enable_Handler(ref TreeView pTreeView, bool bEnable)
        {
            if (pTreeView.InvokeRequired)
            {
                pTreeView.Invoke(new TreeView_Enable_Callback(TreeView_Enable), new object[] { pTreeView, bEnable });
            }
            else
                TreeView_Enable(ref pTreeView, bEnable);
        }
        private static void TreeView_Enable(ref TreeView pTreeView, bool bEnable)
        {
            if (pTreeView.Enabled != bEnable)
                pTreeView.Enabled = bEnable;
        }

        delegate void TableLayoutPanel_Enable_Callback(ref TableLayoutPanel pTableLayoutPanel, bool bEnable);
        public static void TableLayoutPanel_Enable_Handler(ref TableLayoutPanel pTableLayoutPanel, bool bEnable)
        {
            if (pTableLayoutPanel.InvokeRequired)
            {
                pTableLayoutPanel.Invoke(new TableLayoutPanel_Enable_Callback(TableLayoutPanel_Enable), new object[] { pTableLayoutPanel, bEnable });
            }
            else
                TableLayoutPanel_Enable(ref pTableLayoutPanel, bEnable);
        }
        private static void TableLayoutPanel_Enable(ref TableLayoutPanel pTableLayoutPanel, bool bEnable)
        {
            if (pTableLayoutPanel.Enabled != bEnable)
                pTableLayoutPanel.Enabled = bEnable;
        }

        delegate void DataGridView_Cell_SetValue_Callback(ref DataGridView pDataGridView, int Row, int Column, string value);
        public static void DataGridView_Cell_SetValue_Handler(ref DataGridView pDataGridView, int Row, int Column, string value)
        {
            if (pDataGridView.InvokeRequired)
            {
                pDataGridView.Invoke(new DataGridView_Cell_SetValue_Callback(DataGridView_Cell_SetValue), new object[] { pDataGridView, Row, Column, value });
            }
            else
                DataGridView_Cell_SetValue(ref pDataGridView, Row, Column, value);
        }
        private static void DataGridView_Cell_SetValue(ref DataGridView pDataGridView, int Row, int Column, string value)
        {
            if(pDataGridView?.Rows?[Row]?.Cells?[Column] != null)
            {
                if((string)pDataGridView.Rows[Row].Cells[Column].Value != value)
                    pDataGridView.Rows[Row].Cells[Column].Value = value;
            }
        }

        delegate void DataGridView_Cell_SetForeColor_Callback(ref DataGridView pDataGridView, int Row, int Column, Color color);
        public static void DataGridView_Cell_SetForeColor_Handler(ref DataGridView pDataGridView, int Row, int Column, Color color)
        {
            if (pDataGridView.InvokeRequired)
            {
                pDataGridView.Invoke(new DataGridView_Cell_SetForeColor_Callback(DataGridView_Cell_SetForeColor), new object[] { pDataGridView, Row, Column, color });
            }
            else
                DataGridView_Cell_SetForeColor(ref pDataGridView, Row, Column, color);
        }
        private static void DataGridView_Cell_SetForeColor(ref DataGridView pDataGridView, int Row, int Column, Color color)
        {
            if (pDataGridView?.Rows?[Row]?.Cells?[Column] != null)
            {
                if (pDataGridView.Rows[Row].Cells[Column].Style.ForeColor != color)
                    pDataGridView.Rows[Row].Cells[Column].Style.ForeColor = color;
            }
        }

        delegate void DataGridViewCell_SetForeColor_Callback(ref DataGridViewCell pDataGridViewCell, Color color);
        public static void DataGridViewCell_SetForeColor_Handler(ref DataGridViewCell pDataGridViewCell, Color color)
        {
            if (pDataGridViewCell.DataGridView.InvokeRequired)
            {
                pDataGridViewCell.DataGridView.Invoke(new DataGridViewCell_SetForeColor_Callback(DataGridViewCell_SetForeColor), new object[] { pDataGridViewCell, color });
            }
            else
                DataGridViewCell_SetForeColor(ref pDataGridViewCell, color);
        }
        private static void DataGridViewCell_SetForeColor(ref DataGridViewCell pDataGridViewCell, Color color)
        {
            if (pDataGridViewCell != null)
            {
                if (pDataGridViewCell.Style.ForeColor != color)
                    pDataGridViewCell.Style.ForeColor = color;
            }
        }

        delegate void DataGridViewCell_SetValue_Callback(ref DataGridViewCell pDataGridViewCell, string value);
        public static void DataGridViewCell_SetValue_Handler(ref DataGridViewCell pDataGridViewCell, string value)
        {
            if (pDataGridViewCell.DataGridView.InvokeRequired)
            {
                pDataGridViewCell.DataGridView.Invoke(new DataGridViewCell_SetValue_Callback(DataGridViewCell_SetValue), new object[] { pDataGridViewCell, value });
            }
            else
                DataGridViewCell_SetValue(ref pDataGridViewCell, value);
        }
        private static void DataGridViewCell_SetValue(ref DataGridViewCell pDataGridViewCell, string value)
        {
            if (pDataGridViewCell != null)
            {
                if ((string)pDataGridViewCell.Value != value)
                    pDataGridViewCell.Value = value;
            }
        }

        delegate void DataGridView_Cell_SetBackColor_Callback(ref DataGridView pDataGridView, int Row, int Column, Color color);
        public static void DataGridView_Cell_SetBackColor_Handler(ref DataGridView pDataGridView, int Row, int Column, Color color)
        {
            if (pDataGridView.InvokeRequired)
            {
                pDataGridView.Invoke(new DataGridView_Cell_SetBackColor_Callback(DataGridView_Cell_SetBackColor), new object[] { pDataGridView, Row, Column, color });
            }
            else
                DataGridView_Cell_SetBackColor(ref pDataGridView, Row, Column, color);
        }
        private static void DataGridView_Cell_SetBackColor(ref DataGridView pDataGridView, int Row, int Column, Color color)
        {
            if (pDataGridView?.Rows?[Row]?.Cells?[Column] != null)
            {
                if (pDataGridView.Rows[Row].Cells[Column].Style.BackColor != color)
                    pDataGridView.Rows[Row].Cells[Column].Style.BackColor = color;
            }
        }

        delegate void DataGridView_CurrentCell_Callback(ref DataGridView pDataGridView, DataGridViewCell cell);
        public static void DataGridView_CurrentCell_Handler(ref DataGridView pDataGridView, DataGridViewCell cell)
        {
            if (pDataGridView.InvokeRequired)
            {
                pDataGridView.Invoke(new DataGridView_CurrentCell_Callback(DataGridView_CurrentCell), new object[] { pDataGridView, cell });
            }
            else
                DataGridView_CurrentCell(ref pDataGridView, cell);
        }
        private static void DataGridView_CurrentCell(ref DataGridView pDataGridView, DataGridViewCell cell)
        {
            pDataGridView.CurrentCell = cell;
            pDataGridView.Refresh();
        }

        delegate void DataGridView_RowClear_Callback(ref DataGridView pDataGridView);
        public static void DataGridView_RowClear_Handler(ref DataGridView pDataGridView)
        {
            if (pDataGridView.InvokeRequired)
            {
                pDataGridView.Invoke(new DataGridView_RowClear_Callback(DataGridView_RowClear), new object[] { pDataGridView });
            }
            else
                DataGridView_RowClear(ref pDataGridView);
        }
        private static void DataGridView_RowClear(ref DataGridView pDataGridView)
        {
            pDataGridView.Rows.Clear();
            pDataGridView.Refresh();
        }

        delegate void DataGridView_AddRow_Callback(ref DataGridView pDataGridView, params object[] obj);
        public static void DataGridView_AddRow_Handler(ref DataGridView pDataGridView, params object[] obj)
        {
            if (pDataGridView.InvokeRequired)
            {
                pDataGridView.Invoke(new DataGridView_AddRow_Callback(DataGridView_AddRow), new object[] { pDataGridView, obj });
            }
            else
                DataGridView_AddRow(ref pDataGridView, obj);
        }
        private static void DataGridView_AddRow(ref DataGridView pDataGridView, params object[] obj)
        {
            pDataGridView.Rows.Add((string[])obj);
        }

        delegate void FormClose_Callback(ref Form pForm);
        public static void FormClose_Handler(ref Form pForm)
        {
            if (pForm.InvokeRequired)
            {
                pForm.Invoke(new FormClose_Callback(FormClose), new object[] { pForm });
            }
            else
                FormClose(ref pForm);
        }
        private static void FormClose(ref Form pForm)
        {
            pForm.Close();
        }

        delegate void TextBox_Text_Callback(ref TextBox pTextBox, string text);
        public static void TextBox_Text_Handler(ref TextBox pTextBox, string text)
        {
            if (pTextBox.InvokeRequired)
            {
                pTextBox.Invoke(new TextBox_Text_Callback(TextBox_Settext), new object[] { pTextBox, text });
            }
            else
                TextBox_Settext(ref pTextBox, text);
        }
        private static void TextBox_Settext(ref TextBox pTextBox, string text)
        {
            if(pTextBox.Text != text)
                pTextBox.Text = text;
        }

        delegate void Panel_SuspendLayOut_Callback(ref Panel pPanel);
        public static void Panel_SuspendLayOut_Handler(ref Panel pPanel)
        {
            if (pPanel.InvokeRequired)
            {
                pPanel.Invoke(new Panel_SuspendLayOut_Callback(Panel_SuspendLayOut), new object[] { pPanel });
            }
            else
                Panel_SuspendLayOut(ref pPanel);
        }
        private static void Panel_SuspendLayOut(ref Panel pPanel)
        {
            pPanel.SuspendLayout();
        }

        delegate void Panel_ResumeLayOut_Callback(ref Panel pPanel);
        public static void Panel_ResumeLayOut_Handler(ref Panel pPanel)
        {
            if (pPanel.InvokeRequired)
            {
                pPanel.Invoke(new Panel_ResumeLayOut_Callback(Panel_ResumeLayOut), new object[] { pPanel });
            }
            else
                Panel_ResumeLayOut(ref pPanel);
        }
        private static void Panel_ResumeLayOut(ref Panel pPanel)
        {
            pPanel.ResumeLayout();
        }

        delegate void Panel_RemoveControl_Callback(ref Panel pPanel, Control ControlItem);
        public static void Panel_RemoveControl_Handler(ref Panel pPanel, Control ControlItem)
        {
            if (pPanel.InvokeRequired)
            {
                pPanel.Invoke(new Panel_RemoveControl_Callback(Panel_RemoveControl), new object[] { pPanel, ControlItem });
            }
            else
                Panel_RemoveControl(ref pPanel, ControlItem);
        }
        private static void Panel_RemoveControl(ref Panel pPanel, Control ControlItem)
        {
            pPanel.Controls.Remove(ControlItem);
        }

        delegate void Panel_AddControl_Callback(ref Panel pPanel, Control ControlItem);
        public static void Panel_AddControl_Handler(ref Panel pPanel, Control ControlItem)
        {
            if (pPanel.InvokeRequired)
            {
                pPanel.Invoke(new Panel_AddControl_Callback(Panel_AddControl), new object[] { pPanel, ControlItem });
            }
            else
                Panel_AddControl(ref pPanel, ControlItem);
        }
        private static void Panel_AddControl(ref Panel pPanel, Control ControlItem)
        {
            pPanel.Controls.Add(ControlItem);
        }
    }
}
