﻿using System;
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
using static Synustech.UnitConverter;

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
            dgParaView.Columns.Clear();
            foreach (DataGridViewColumn column in dgParaView.Columns)
            {
                if (column.HeaderText == "None")
                {
                    dgParaView.Columns.Remove(column);
                    break; // 첫 번째 "None" 열을 찾으면 삭제 후 루프 종료
                }
            }
            int i = 0;
            foreach (Conveyor conveyor in conveyors)
            {
                string Header = "ID: " + conveyor.ID.ToString("D3") + "\n" +"[Axis: " + conveyor.axis.ToString("D3") + "]";
                int ColumnIndex = dgParaView.Columns.Add(Header, Header.ToString());
                
                dgParaView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgParaView.Columns[i].Width = 130;
                i++;
                if (conveyor.type == "Turn")
                {
                    string headerTurnAxis = "ID: " + conveyor.ID.ToString("D3") +"\n"+ "[Turn Axis: " + conveyor.turnAxis.ToString("D3") + "]";
                    int turnAxisColumnIndex = dgParaView.Columns.Add(headerTurnAxis, headerTurnAxis);
                    dgParaView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgParaView.Columns[i].Width = 130;
                    i++;
                }
            }
            if (dgParaView.Columns.Count > 0)
            {
                foreach (string header in headers)
                {
                    int rowIndex = dgParaView.Rows.Add(); // 행 추가
                    dgParaView.Rows[rowIndex].HeaderCell.Value = header; // 행 머리글 텍스트 설정
                }
                for (int j = 0; j < 3; j++)
                {
                    dgParaView.Rows[j].ReadOnly = true;
                }
            }
            Header_Update_Timer.Enabled = false;
        }

        




        private void tb_Base_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnParaSave_Click(object sender, EventArgs e)
        {
            List<int> axes = new List<int>();

            // 모든 conveyor의 Axis와 TurnAxis 값 가져오기
            foreach (var conv in conveyors)
            {
                axes.Add(conv.axis);
                if (conv.type == "Turn")
                {
                    axes.Add(conv.turnAxis);
                }
            }
            string message = $"Do you want to save the parameters as they are?";
            DialogResult result = MessageBox.Show(message, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                for (int i = 0; i < axes.Count; i++)
                {
                    int axis = axes[i];

                    // 각 행의 값들을 m_axisParameter에 할당
                    //w_motion.m_axisParameter[axis].m_motorDirection = Convert.ToInt32(dgParaView.Rows[0].Cells[i].Value);
                    //w_motion.m_axisParameter[axis].m_homeType = Convert.ToInt32(dgParaView.Rows[1].Cells[i].Value);
                    //w_motion.m_axisParameter[axis].m_homeDirection = Convert.ToInt32(dgParaView.Rows[2].Cells[i].Value);
                    w_motion.m_axisParameter[axis].m_homeFastVelocity = InvertmmToum(Convert.ToDouble(dgParaView.Rows[3].Cells[i].Value));
                    w_motion.m_axisParameter[axis].m_homeFastAcc = InvertmmToum(Convert.ToDouble(dgParaView.Rows[4].Cells[i].Value));
                    w_motion.m_axisParameter[axis].m_homeFastDec = InvertmmToum(Convert.ToDouble(dgParaView.Rows[5].Cells[i].Value));
                    w_motion.m_axisParameter[axis].m_homeSlowVelocity = InvertmmToum(Convert.ToDouble(dgParaView.Rows[6].Cells[i].Value));
                    w_motion.m_axisParameter[axis].m_homeSlowAcc = InvertmmToum(Convert.ToDouble(dgParaView.Rows[7].Cells[i].Value));
                    w_motion.m_axisParameter[axis].m_homeSlowDec = InvertmmToum(Convert.ToDouble(dgParaView.Rows[8].Cells[i].Value));
                    w_motion.m_axisParameter[axis].m_homeShiftDistance = InvertmmToum(Convert.ToDouble(dgParaView.Rows[9].Cells[i].Value));
                }
                w_motion.GetAndSaveAxisParameter(G_Var.paramFullPath);
            }
        }

        private void btnParaLoad_Click(object sender, EventArgs e)
        {
            SetGridView();
        }

        private void SetGridView()
        {
            List <int> Axes = new List<int>();

            foreach(var conv in conveyors)
            {
                Axes.Add(conv.axis);
                if(conv.type == "Turn")
                {
                    Axes.Add(conv.turnAxis);
                }
            }
            int i = 0;
            foreach(var axis in Axes) 
            {
                dgParaView.Rows[0].Cells[i].Value = w_motion.m_axisParameter[axis].m_motorDirection;
                dgParaView.Rows[1].Cells[i].Value = w_motion.m_axisParameter[axis].m_homeType;
                dgParaView.Rows[2].Cells[i].Value = w_motion.m_axisParameter[axis].m_homeDirection;
                dgParaView.Rows[3].Cells[i].Value = InvertumTomm(w_motion.m_axisParameter[axis].m_homeFastVelocity);
                dgParaView.Rows[4].Cells[i].Value = InvertumTomm(w_motion.m_axisParameter[axis].m_homeFastAcc);
                dgParaView.Rows[5].Cells[i].Value = InvertumTomm(w_motion.m_axisParameter[axis].m_homeFastDec);
                dgParaView.Rows[6].Cells[i].Value = InvertumTomm(w_motion.m_axisParameter[axis].m_homeSlowVelocity);
                dgParaView.Rows[7].Cells[i].Value = InvertumTomm(w_motion.m_axisParameter[axis].m_homeSlowAcc);
                dgParaView.Rows[8].Cells[i].Value = InvertumTomm(w_motion.m_axisParameter[axis].m_homeSlowDec);
                dgParaView.Rows[9].Cells[i].Value = InvertumTomm(w_motion.m_axisParameter[axis].m_homeShiftDistance);
                i++;
            }

            Header_Update_Timer.Enabled = false;
        }

        private void Header_Update_Timer_Tick(object sender, EventArgs e)
        {
            InitialParameterGridView();
            //SetGridView();
        }
    }
}
