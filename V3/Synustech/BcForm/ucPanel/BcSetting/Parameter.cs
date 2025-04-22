using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Synustech.G_Var;
using WMX3ApiCLR;
using System.Data.Common;
using static Synustech.MathCalculator;

namespace Synustech.BcForm.ucPanel.BcSetting
{
    public partial class Parameter : UserControl
    {
        // 설정할 행 머리글 텍스트
        string[] headers =
            {
                 "motorDirection",
                 "homeType",
                 "homeDirection",
                 "homeFastVelocity[mm/s]",
                 "homeFastAcc[mm/s]",
                 "homeFastDec[mm/s]",
                 "homeSlowVelocity[mm/s]",
                 "homeSlowAcc[mm/s]",
                 "homeSlowDec[mm/s]",
                 "homeShiftDistance[mm]"
            };
        public Parameter()
        {
            InitializeComponent();

            // RowHeaderVisible 설정
            dgParaView.RowHeadersVisible = true;
        }

        public void InitialParameterGridView()
        {
            
        }

        




        private void tb_Base_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnParaSave_Click(object sender, EventArgs e)
        {
            
        }

        private void btnParaLoad_Click(object sender, EventArgs e)
        {
            SetGridView();
        }

        private void GetGridView()
        {

        }

        private void SetGridView()
        {
            
        }

        private void Header_Update_Timer_Tick(object sender, EventArgs e)
        {
            InitialParameterGridView();
            //SetGridView();
        }
    }
}
