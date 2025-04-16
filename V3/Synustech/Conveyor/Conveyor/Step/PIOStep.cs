using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synustech
{
    partial class Conveyor
    {
        public enum PIOStep
        {
            Step000_Direction_Check = 0,

            Step200_Inmode_Check_Foup = 200,
            Step210_InMode_Await_PIO_CS = 210,                            //Carrier Load 요청 (AGV or OHT)
            Step220_InMode_Check_PIO_Valid = 220,
            //Step221_InMode_Call_AGV_Load_Req                = 221,      //AGV PIO
            Step230_InMode_Check_PIO_TR = 230,
            Step240_InMode_Check_PIO_Busy = 240,
            Step245_InMode_Check_PIO_BusyDone = 241,
            Step250_InMode_Check_PIO_Complete = 250,
            Step260_InMode_Check_PIO_End = 260,
            Step270_InMode_Check_CST_Load_And_Safe = 270,

            Step290_InMode_Await_MGV_CST_Load = 290,                      //Carrier Load 대기 (MGV), Carrier ID Clear


            Step300_Outmode_Check_Foup = 300,
            Step310_OutMode_Await_PIO_CS = 310,                          //Carrier Unload 대기 (AGV or OHT)
            Step320_OutMode_Check_PIO_Valid = 320,
            //Step321_OutMode_Call_AGV_Unload_Req             = 321,
            Step330_OutMode_Check_PIO_TR = 330,
            Step340_OutMode_Check_PIO_Busy = 340,
            Step350_OutMode_Check_PIO_Complete = 350,
            Step360_OutMode_Check_PIO_End = 360,
            Step370_OutMode_Check_LP_CST_Unload_And_Safe = 370,          //Carrier ID Clear

            Step390_OutMode_Await_MGV_CST_Unload = 390,                  //Carrier Unload 대기 (MGV), Carrier ID Clear
        }
        /// <summary>
        /// Interface
        /// </summary>
        public PIOStep CV_InterfaceStep = PIOStep.Step000_Direction_Check;
        public PIOStep preInterfaceStep;
    }
}
