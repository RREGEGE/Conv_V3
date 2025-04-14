using Google.Protobuf.WellKnownTypes;
using Synustech.ucPanel.BcMotion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech.BcForm
{
    public partial class Calculator : Form
    {
        public delegate void ValueSendHandler(double value);
        public ValueSendHandler ValueSend_Control;
        public ValueSendHandler ValueSend_Teaching;
        public ValueSendHandler ValueSend_Cycle;
        public ValueSendHandler ValueSend_ID;


        enum Operators
        {
            None,
            Subtract,
            Result
        }

        Operators currentOperator = Operators.None;
        int firstOperand = 0;
        int secondOperand = 0;
        public Calculator()
        {   
            InitializeComponent();

        }

        private void btnX_Click(object sender, EventArgs e)
        {
            
            Close();
        }

        private void btnKey0_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "0";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }

        }

        private void btnKey1_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "1";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }

        }

        private void btnKey2_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "2";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }

        }

        private void btnKey3_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "3";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }
        }

        private void btnKey4_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "4";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }
        }

        private void btnKey5_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "5";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }
        }

        private void btnKey6_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "6";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }
        }

        private void btnKey7_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "7";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }
        }

        private void btnKey8_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "8";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }
        }

        private void btnKey9_Click(object sender, EventArgs e)
        {
            string strNumber = lblSendValue.Text += "9";
            if (double.TryParse(strNumber, out double doubleNumber))
            {
                lblSendValue.Text = doubleNumber.ToString();
            }
            else
            {
                lblSendValue.Text = "0"; // Set to "0" if parsing fails (e.g., invalid input)
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblSendValue.Text))
            {
                // lblSendValue의 텍스트에서 마지막 문자 제거
                lblSendValue.Text = lblSendValue.Text.Substring(0, lblSendValue.Text.Length - 1);

                // 만약 텍스트가 빈 문자열이 되면 "0"으로 설정
                if (string.IsNullOrEmpty(lblSendValue.Text))
                {
                    lblSendValue.Text = "0";
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            firstOperand = 0;
            secondOperand = 0;
            currentOperator = Operators.None;
            lblSendValue.Text = "0";
        }

        private void btnKeyEnter_Click(object sender, EventArgs e)
        {
            double value;
            if (double.TryParse(lblSendValue.Text, out value))
            {
                // control 델리게이트가 설정된 경우 호출
                ValueSend_Control?.Invoke(value);
                // teaching 델리게이트가 설정된 경우 호출
                ValueSend_Teaching?.Invoke(value);
                // cycle 델리게이트가 설정된 경우 호출
                ValueSend_Cycle?.Invoke(value);
                // ID 델리게이트가 설정된 경우 호출
                ValueSend_ID?.Invoke(value);
            }

            Close();
        }

        private void btnKeyDot_Click(object sender, EventArgs e)
        {
            if (!lblSendValue.Text.Contains("."))
            {
                lblSendValue.Text += ".";
            }
        }
    }
}
