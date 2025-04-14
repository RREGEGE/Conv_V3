using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Master.GlobalForm
{
    public partial class Frm_Keypad : Form
    {
        public string InsertResult = string.Empty;
        public bool DecimalType = true;
        public Frm_Keypad(string OrgData, bool _DecimalType)
        {
            InitializeComponent();
            this.ActiveControl = tbx_InsertValue;
            tbx_InsertValue.Text = OrgData;
            tbx_InsertValue.Select(tbx_InsertValue.Text.Length, 0);
            DecimalType = _DecimalType;

            if (!DecimalType)
            {
                btn_DecimalPoint_Insert.Visible = false;
                btn_DecimalPoint_Insert.Enabled = false;
            }

            this.Disposed += (object sender, EventArgs e) =>
            {
                foreach (Control Item in this.Controls)
                    ControlFunc.Dispose(Item);
            };
        }

        private void btn_0_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "0";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_1_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "1";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_2_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "2";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_3_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "3";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_4_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "4";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_5_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "5";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_6_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "6";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_7_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "7";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_8_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "8";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_9_Insert_Click(object sender, EventArgs e)
        {
            var insertText = "9";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_DecimalPoint_Insert_Click(object sender, EventArgs e)
        {
            if (tbx_InsertValue.Text.Contains("."))
                return;

            var insertText = ".";
            var selectionIndex = tbx_InsertValue.SelectionStart;
            tbx_InsertValue.Text = tbx_InsertValue.Text.Insert(selectionIndex, insertText);
            tbx_InsertValue.SelectionStart = selectionIndex + insertText.Length;
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            tbx_InsertValue.Text = string.Empty;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_Set_Click(object sender, EventArgs e)
        {
            try
            {
                if (DecimalType)
                {
                    float Text = Convert.ToSingle(tbx_InsertValue.Text);
                }
                else
                {
                    int Text = Convert.ToInt32(tbx_InsertValue.Text);
                }

                this.DialogResult = DialogResult.OK;
                InsertResult = tbx_InsertValue.Text;
                this.Close();
            }
            catch(Exception ex)
            {
                InsertResult = string.Empty;
                MessageBox.Show(ex.ToString(), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
