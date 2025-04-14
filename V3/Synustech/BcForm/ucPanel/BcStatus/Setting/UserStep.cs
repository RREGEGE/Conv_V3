﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synustech.ucPanel.BcStatus.Setting
{
    public partial class UserStep : UserControl
    {
        DataTable dtMain;
        private float initialPanelWidth;
        private float initialPanelHeight;

        private float initialFontSize_lblStepName;
        private Size initialSize_____lblStepName;
        private Point initialLocation_lblStepName;

        private bool isResizing = false;

        public UserStep()
        {
            InitializeComponent();
            DataSet();

            initialPanelWidth = this.Width;
            initialPanelHeight = this.Height;

            initialFontSize_lblStepName = lblStepName.Font.Size;
            initialSize_____lblStepName = lblStepName.Size;
            initialLocation_lblStepName = lblStepName.Location;

            this.Resize += Panel_Resize;
        }
        private void Panel_Resize(object sender, EventArgs e)
        {
            // 이미 크기 조정 중이면 중복 처리 방지
            if (isResizing)
                return;
            try
            {
                isResizing = true;

                if (initialPanelWidth == 0 || initialPanelHeight == 0)
                {
                    return; // 초기 패널 크기가 0인 경우 아무 것도 하지 않음
                }

                // Panel 크기 비율 계산
                float widthRatio = (float)this.Width / initialPanelWidth;
                float heightRatio = (float)this.Height / initialPanelHeight;
                float ratio = Math.Min(widthRatio, heightRatio);

                // 라벨 폰트 사이즈 조절
                AdjustFontSize(lblStepName, initialFontSize_lblStepName, ratio);


                // 라벨 크기 및 위치 조절
                AdjustLabel(lblStepName, initialSize_____lblStepName, initialLocation_lblStepName, widthRatio, heightRatio);

            }
            finally
            {
                isResizing = false; // 크기 조정 완료
            }
        }

        private void AdjustFontSize(System.Windows.Forms.Label label, float initialFontSize, float ratio)
        {
            // 새로운 폰트 크기를 계산하고 유효한지 확인
            float newFontSize = (initialFontSize * ratio);
            if (newFontSize > 0 && newFontSize <= float.MaxValue)
            {
                label.Font = new Font(label.Font.FontFamily, newFontSize, label.Font.Style);
            }
            else
            {
                // 기본값으로 폰트 크기 설정 또는 경고 처리
                MessageBox.Show("폰트 크기는 0보다 크고 MaxValue보다 작거나 같아야 합니다.");
            }
        }

        private void AdjustLabel(System.Windows.Forms.Label label, Size initialSize, Point initialLocation, float widthRatio, float heightRatio)
        {
            try
            {
                // 라벨의 크기 조정
                label.Width = (int)(initialSize.Width * widthRatio);
                label.Height = (int)(initialSize.Height * heightRatio);

                // 라벨의 위치 조정
                label.Location = new Point((int)(initialLocation.X * widthRatio), (int)(initialLocation.Y * heightRatio));
            }
            catch (Exception ex)
            {
                // 예외가 발생하면 로그를 남기거나 디버깅을 위한 조치를 취합니다.
                MessageBox.Show($"라벨 크기 조정 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        private void DataSet()
        {
            dtMain = new DataTable();

            DataColumn colIndex = new DataColumn("index", typeof(string));
            DataColumn colSensor = new DataColumn("Sensor_Name", typeof(int));
            DataColumn colPower = new DataColumn("On/Off", typeof(int));



            //만든 열들을 테이블에 연결한다.
            dtMain.Columns.Add(colIndex);
            dtMain.Columns.Add(colSensor);
            dtMain.Columns.Add(colPower);


            Random rd = new Random();

            dgStep.DataSource = dtMain;
            dgStep.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11);
        }
    }
}
