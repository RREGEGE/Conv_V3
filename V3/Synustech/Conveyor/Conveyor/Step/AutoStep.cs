using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synustech
{
    /// <summary>
    /// AutoStep.cs는 
    /// </summary>
    partial class Conveyor
    {
        public enum AutoStep
        {
            Step000_Check_Direction = 0,
            // InMode Step
            Step100_InMode_Check_CST_Load_Status = 100,
            Step150_InMode_Waitting_Before_Conv_Unload = 150,
            Step155_InMode_Change_210 = 155,

            // 구동 Step 정의
            Step210_InMode_Move_CV_Rolling_In = 210,
            Step215_InMode_Move_CV_Rolling_In_Slow = 215,
            Step220_InMode_Run_Condition_Check = 220,
            Step250_InMode_Move_CV_Rolling_Out = 250,
            Step255_InMode_Wait_NextConv = 255,
            Step260_InMode_Unload_Turn_Stop = 260,
            Step270_InMode_Move_CST_Pass_CV_to_TCV = 270,
            Step280_InMode_Alarm_Condition_280 = 280,
            Step290_InMode_Alarm_Wait_290 = 290,

            // Turn Axis Step 정의
            Step310_InMode_T_Init_Load = 310, // Loading Pos 외 다른 Pos Sensor Check시 int();
            Step320_InMode_T_Check_Init_Load = 320,
            Step330_InMode_Unload_Move = 330,
            Step340_InMode_T_Check_Unload_Location = 340,
            Step350_InMode_T_Return_To_Load = 350,
            Step360_InMode_T_Check_Return_To_Load = 360,

            // OutMode Step
            Step500_OutMode_Check_CST_Load_Status = 500,
            Step550_OutMode_Waitting_Before_Conv_Unload = 550,
            Step555_OutMode_Change_610 = 555,

            // 구동 Step 정의
            Step610_OutMode_Move_CV_Rolling_In = 610,
            Step615_OutMode_Move_CV_Rolling_In_Slow = 615,
            Step620_OutMode_Run_Condition_Check = 620,
            Step650_OutMode_Move_CV_Rolling_Out = 650,
            Step655_OutMode_Wait_NextConv = 655,
            Step660_OutMode_Unload_Turn_Stop = 660,
            Step670_OutMode_Move_CST_Pass_CV_to_TCV = 670,
            Step680_OutMode_Alarm_Condition_680 = 680,
            Step690_OutMode_Alarm_Wait_690 = 690,

            // Turn Axis Step 정의
            Step710_OutMode_T_Init_Unload = 710,
            Step720_OutMode_T_Check_Init_Unload = 720,
            Step730_OutMode_Load_Move = 730,
            Step740_OutMode_Check_Load_Location = 740,
            Step750_OutMode_T_Return_To_Unload = 750,
            Step760_OutMode_T_Check_Return_To_Unload = 760,

            Step900_Final_CV_Wait = 900,

            Step1000_FinalCVN_Wait = 1000
        }

        // Auto 관련 변수
        public AutoStep prestep;
        public AutoStep CV_AutoStep = AutoStep.Step000_Check_Direction;
        public LineDirection runDirection = LineDirection.Input;

        public bool AutoStopEnable = false;
        public bool AutoRunning = false;
        public bool CycleRunning = false;

        private void Auto_StopEnableUpdate()
        {

        }

    }
}
