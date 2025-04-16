using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMX3ApiCLR;
using static Synustech.G_Var;
using static Synustech.MathCalculator;

namespace Synustech.BcForm.ucPanel.BcSetting
{
    public partial class Operation : UserControl
    {


        public Operation()
        {



            InitializeComponent();
            InitializeDataGridViewHeaders();


        }                                        
        private void InitializeDataGridViewHeaders()
        {
            // RowHeaderVisible 설정
            dgOperationView.RowHeadersVisible = true;


            // 설정할 행 머리글 텍스트
            string[] headers = {
            "AutoRun_Speed[mm/s]",
            "AutoRun_Acc[mm/s]",
            "AutoRun_Dcc[mm/s]",
            "T_Move_AutoRun_Speed[degree/s]",
            "T_Move_AutoRun_Acc[degree/s]",
            "T_Move_AutoRun_Dcc[degree/s]"
            };

            // 행 추가 및 행 머리글 텍스트 설정
            foreach (string header in headers)
            {
                int rowIndex = dgOperationView.Rows.Add(); // 행 추가
                dgOperationView.Rows[rowIndex].HeaderCell.Value = header; // 행 머리글 텍스트 설정
            }

            dgOperationView.Rows[3].Cells[1].Value = 0;
            dgOperationView.Rows[4].Cells[1].Value = 0;
            dgOperationView.Rows[5].Cells[1].Value = 0;
            dgOperationView.Columns[0].Width = 200;
            dgOperationView.Columns[1].Width = 200;
            dgOperationView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);




        }

        private void tb_Op_Base_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnOperationSave_Click(object sender, EventArgs e)
        {

            dgOperationView.Rows[3].Cells[0].Value = dgOperationView.Rows[3].Cells[1].Value;
            dgOperationView.Rows[4].Cells[0].Value = dgOperationView.Rows[4].Cells[1].Value;
            dgOperationView.Rows[5].Cells[0].Value = dgOperationView.Rows[5].Cells[1].Value;

            double velocity = degreeToum(Convert.ToDouble(dgOperationView.Rows[3].Cells[0].Value));
            double acc = degreeToum(Convert.ToDouble(dgOperationView.Rows[4].Cells[0].Value));
            double dec = degreeToum(Convert.ToDouble(dgOperationView.Rows[5].Cells[0].Value));

            for (int i = 0; i < Constants.MaxAxes; i++)
            {
                m_WMXMotion.m_AxisProfile[i].m_velocity = velocity;
                m_WMXMotion.m_AxisProfile[i].m_acc = acc;
                m_WMXMotion.m_AxisProfile[i].m_dec = dec;
                m_WMXMotion.m_AxisProfile[i].m_axis = i;
            }
            _xml.SetProfileParameter(ProfileFullPath);
            Console.WriteLine(m_WMXMotion.m_AxisProfile[0].m_velocity);
        }

        private void btnOperationLoad_Click(object sender, EventArgs e)
        {

            _xml.GetProfileParameter(ProfileFullPath);

            dgOperationView.Rows[3].Cells[0].Value = umTodegree(m_WMXMotion.m_AxisProfile[0].m_velocity);
            dgOperationView.Rows[4].Cells[0].Value = umTodegree(m_WMXMotion.m_AxisProfile[0].m_acc);
            dgOperationView.Rows[5].Cells[0].Value = umTodegree(m_WMXMotion.m_AxisProfile[0].m_dec);

            foreach (var axis in m_WMXMotion.m_AxisProfile)
            {
                Console.WriteLine(axis.m_axis);
            }
        }
    }
}
