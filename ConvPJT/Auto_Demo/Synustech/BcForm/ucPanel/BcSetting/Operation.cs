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
using static Synustech.UnitConverter;

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
            "AutoRun_Dec[mm/s]",
            "T_Move_AutoRun_Speed[degree/s]",
            "T_Move_AutoRun_Acc[degree/s]",
            "T_Move_AutoRun_Dec[degree/s]"
            };

            // 행 추가 및 행 머리글 텍스트 설정
            foreach (string header in headers)
            {
                int rowIndex = dgOperationView.Rows.Add(); // 행 추가
                dgOperationView.Rows[rowIndex].HeaderCell.Value = header; // 행 머리글 텍스트 설정
            }

            dgOperationView.Rows[0].Cells[1].Value = 0;
            dgOperationView.Rows[1].Cells[1].Value = 0;
            dgOperationView.Rows[2].Cells[1].Value = 0;

            dgOperationView.Rows[3].Cells[1].Value = 0;
            dgOperationView.Rows[4].Cells[1].Value = 0;
            dgOperationView.Rows[5].Cells[1].Value = 0;
            dgOperationView.Columns[0].Width = 200;
            dgOperationView.Columns[1].Width = 200;
            dgOperationView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);

        }

        private void btnOperationSave_Click(object sender, EventArgs e)
        {

            double autoVelocity = Convert.ToDouble(dgOperationView.Rows[0].Cells[1].Value);
            double autoAcc = Convert.ToDouble(dgOperationView.Rows[1].Cells[1].Value);
            double autoDec = Convert.ToDouble(dgOperationView.Rows[2].Cells[1].Value);

            double velocity = InvertdegreeToum(Convert.ToDouble(dgOperationView.Rows[3].Cells[1].Value));
            double acc = InvertdegreeToum((Convert.ToDouble(dgOperationView.Rows[4].Cells[1].Value)));
            double dec = InvertdegreeToum((Convert.ToDouble(dgOperationView.Rows[5].Cells[1].Value)));

            if(autoVelocity > 8000 || velocity > 170000 || acc > 170000 || dec > 170000)
            {
                MessageBox.Show("Please check Value\nMaximum Value\nAutoRun_Speed: 800\nAutoRun_Acc: 1200\nAutoRun_Dec: 1200\nT_Move_AutoRun_Speed: 61.2\nT_Move_AutoRun_Acc: 61.2\nT_Move_AutoRun_Dec: 61.2");
                return;
                
            }
            string message = $"Do you want to save the parameters as they are?";
            DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
            {
                dgOperationView.Rows[0].Cells[0].Value = dgOperationView.Rows[0].Cells[1].Value;
                dgOperationView.Rows[1].Cells[0].Value = dgOperationView.Rows[1].Cells[1].Value;
                dgOperationView.Rows[2].Cells[0].Value = dgOperationView.Rows[2].Cells[1].Value;

                dgOperationView.Rows[3].Cells[0].Value = dgOperationView.Rows[3].Cells[1].Value;
                dgOperationView.Rows[4].Cells[0].Value = dgOperationView.Rows[4].Cells[1].Value;
                dgOperationView.Rows[5].Cells[0].Value = dgOperationView.Rows[5].Cells[1].Value;
                for (int i = 0; i < Constants.MaxAxes; i++)
                {
                    w_motion.m_AxisProfile[i].m_velocity = velocity;
                    w_motion.m_AxisProfile[i].m_acc = acc;
                    w_motion.m_AxisProfile[i].m_dec = dec;
                    w_motion.m_AxisProfile[i].m_axis = i;
                }
                foreach (var conv in conveyors)
                {
                    conv.autoVelocity = InvertmmTospeed(autoVelocity);
                    conv.autoAcc = InvertmmTospeed(autoAcc);
                    conv.autoDec = InvertmmTospeed(autoDec);
                    Console.WriteLine(conv.ID + ":" + conv.autoVelocity);
                }
                xmlControl.SaveConveyorToXML(convFullPath);
                xmlControl.SetProfileParameter(profileFullPath);
                
            }
        }

        private void btnOperationLoad_Click(object sender, EventArgs e)
        {
            xmlControl.GetProfileParameter(profileFullPath);
            if (conveyors.Count > 0)
            {
                dgOperationView.Rows[0].Cells[0].Value = InvertspeedTomm(conveyors[0].autoVelocity);
                dgOperationView.Rows[1].Cells[0].Value = InvertspeedTomm(conveyors[0].autoAcc);
                dgOperationView.Rows[2].Cells[0].Value = InvertspeedTomm(conveyors[0].autoDec);

                dgOperationView.Rows[3].Cells[0].Value = InvertumTodegree(w_motion.m_AxisProfile[0].m_velocity);
                dgOperationView.Rows[4].Cells[0].Value = InvertumTodegree(w_motion.m_AxisProfile[0].m_acc);
                dgOperationView.Rows[5].Cells[0].Value = InvertumTodegree(w_motion.m_AxisProfile[0].m_dec);

                dgOperationView.Rows[0].Cells[1].Value = InvertspeedTomm(conveyors[0].autoVelocity);
                dgOperationView.Rows[1].Cells[1].Value = InvertspeedTomm(conveyors[0].autoAcc);
                dgOperationView.Rows[2].Cells[1].Value = InvertspeedTomm(conveyors[0].autoDec);

                dgOperationView.Rows[3].Cells[1].Value = InvertumTodegree(w_motion.m_AxisProfile[0].m_velocity);
                dgOperationView.Rows[4].Cells[1].Value = InvertumTodegree(w_motion.m_AxisProfile[0].m_acc);
                dgOperationView.Rows[5].Cells[1].Value = InvertumTodegree(w_motion.m_AxisProfile[0].m_dec);

                foreach (var axis in w_motion.m_AxisProfile)
                {
                    Console.WriteLine(axis.m_axis);
                }
            }
        }
    }
}
