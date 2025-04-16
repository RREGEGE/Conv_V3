using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static Synustech.G_Var;

namespace Synustech
{

    public class AnyThread
    {
        public Thread iothread;

        public void IoStopThread()
        {
            iothread.IsBackground = true;
        }

        public void IoThread()
        {
            Console.WriteLine("IO Input Thread Start");
            iothread = new Thread(() =>
            {
                while (IsMainForm)
                {
                    if (!WMX3.IsEngineCommunicating())
                    {
                        Console.WriteLine("IO communication lost. Waiting...");

                        // WMX3가 다시 통신할 때까지 대기
                        while (!WMX3.IsEngineCommunicating())
                        {
                            Thread.Sleep(500); // 1초마다 상태 확인
                        }

                        Console.WriteLine("IO communication restored.");
                    }
                    SafetyIoCheck();
                    AutoConditionCheck();
                    IoInputCheck();
                    IoOutputCheck();
                    Thread.Sleep(10);
                }
            });
            iothread.Name = "IOThread";
            iothread.Start();
        }
        private void IoInputCheck()
        {
            foreach (var conveyor in conveyors)
            {
                if (conveyor.Axis == -1) { }
                else if (conveyor.type == "Turn" && conveyor.Axis != -1)
                {
                    conveyor.AddrUpdate();
                    int i = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        WMX3.m_wmx3io.GetInBit(conveyor.addr, bit, ref G_Var.byInput[0]);
                        bool inputStatus = (G_Var.byInput[0] == 1) ? false : true;
                        conveyor.SCnvFoupDetect[i] = (inputStatus == false) ? SensorOnOff.On : SensorOnOff.Off;
                        i++;
                    }
                    int j = 0;
                    foreach (var bit in conveyor.bitsTurn)
                    {
                        WMX3.m_wmx3io.GetInBit(conveyor.addrTurn, bit, ref G_Var.byInput[0]);
                        bool inputStatus = (G_Var.byInput[0] == 1) ? true : false;
                        conveyor.POS[j] = (inputStatus == true) ? SensorOnOff.On : SensorOnOff.Off;
                        j++;
                    }
                    WMX3.m_wmx3io.GetInBit(conveyor.addr, conveyor.bitTurn, ref G_Var.byInput[0]);
                    bool inputStatus1 = (G_Var.byInput[0] == 1) ? true : false;
                    conveyor.POS[j] = (inputStatus1 == true) ? SensorOnOff.On : SensorOnOff.Off;
                }
                else if (conveyor.type == "Long" && conveyor.Axis != -1)
                {
                    conveyor.AddrUpdate();
                    int i = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        WMX3.m_wmx3io.GetInBit(conveyor.addr, bit, ref G_Var.byInput[0]);
                        bool inputStatus = (G_Var.byInput[0] == 1) ? false : true;
                        conveyor.LCnvFoupDetect[i] = (inputStatus == false) ? SensorOnOff.On : SensorOnOff.Off;
                        i++;
                    }
                }
                else
                {
                    conveyor.AddrUpdate();
                    int i = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        WMX3.m_wmx3io.GetInBit(conveyor.addr, bit, ref G_Var.byInput[0]);
                        bool inputStatus = (G_Var.byInput[0] == 1) ? false : true;
                        conveyor.SCnvFoupDetect[i] = (inputStatus == false) ? SensorOnOff.On : SensorOnOff.Off;
                        i++;
                    }
                }
            }
        }
        private void SafetyIoCheck()
        {
            WMX3.m_wmx3io.GetInBit(addrSafetyin, Safetyinbit, ref G_Var.byInput[1]);
            bool inputSafety = (G_Var.byInput[1] == 1) ? true : false;
            G_Var.Safety = (inputSafety == true) ? SensorOnOff.On : SensorOnOff.Off;

            WMX3.m_wmx3io.GetInBit(addrSafetyin, MainPowerbit, ref G_Var.byInput[7]);
            bool inputMainPower = (G_Var.byInput[7] == 1) ? true : false;
            G_Var.MainPower = (inputMainPower == true) ? SensorOnOff.On : SensorOnOff.Off;

            WMX3.m_wmx3io.GetInBit(addrSafetyin, EMObit, ref G_Var.byInput[2]);
            bool inputEMO = (G_Var.byInput[2] == 1) ? true : false;
            G_Var.EMO = (inputEMO == true) ? SensorOnOff.On : SensorOnOff.Off;

            WMX3.m_wmx3io.GetInBit(addrEMS_1, bitEMS, ref G_Var.byInput[3]);
            bool inputEMS_1 = (G_Var.byInput[3] == 1) ? true : false;
            G_Var.EMS_1 = (inputEMS_1 == true) ? SensorOnOff.On : SensorOnOff.Off;

            WMX3.m_wmx3io.GetInBit(addrEMS_2, bitEMS, ref G_Var.byInput[4]);
            bool inputEMS_2 = (G_Var.byInput[4] == 1) ? true : false;
            G_Var.EMS_2 = (inputEMS_2 == true) ? SensorOnOff.On : SensorOnOff.Off;

            int i = 0;
            foreach (var bit in OHTINbits)
            {
                WMX3.m_wmx3io.GetInBit(addrOHTin, bit, ref G_Var.byInput[5]);
                bool inOHT = (G_Var.byInput[5] == 1) ? true : false;
                G_Var.OHTPIOin[i++] = (inOHT == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
            int j = 0;
            foreach (var bit in AGVINbits)
            {
                WMX3.m_wmx3io.GetInBit(addrOHTin, bit, ref G_Var.byInput[6]);
                bool inAGV = (G_Var.byInput[6] == 1) ? true : false;
                G_Var.AGVPIOin[j++] = (inAGV == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
        }
        private void AutoConditionCheck()
        {
            if (IsSafety && (conveyors.All(conveyor => conveyor.servo == ServoOnOff.On)))
            {
                IsAuto = true;
            }
            else
            {
                IsAuto = false;
            }
        }
        public void IoOutputCheck()
        {
            WMX3.m_wmx3io.GetOutBit(addrSafety, bitSafety, ref byOutput[0]);
            bool outPutStatus = (G_Var.byOutput[0] == 1) ? true : false;
            SafetyReset = (outPutStatus == true) ? SensorOnOff.On : SensorOnOff.Off;
            int i = 0;
            foreach (var bit in Lampbits)
            {
                WMX3.m_wmx3io.GetOutBit(addrLamp_Buzz, bit, ref byOutput[0]);
                bool outPutStatus1 = (G_Var.byOutput[0] == 1) ? true : false;
                Lamp_Buzz[i++] = (outPutStatus1 == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
            int j = 0;
            foreach (var bit in OHTbits)
            {
                WMX3.m_wmx3io.GetOutBit(addrOHTPIO, bit, ref byOutput[0]);
                bool outPutStatus1 = (G_Var.byOutput[0] == 1) ? true : false;
                OHTpio[j++] = (outPutStatus1 == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
            int z = 0;
            foreach (var bit in AGVbits)
            {
                WMX3.m_wmx3io.GetOutBit(addrAGVPIO, bit, ref byOutput[0]);
                bool outPutStatus1 = (G_Var.byOutput[0] == 1) ? true : false;
                AGVpio[z++] = (outPutStatus1 == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
        }
    }
    public class LampThread
    {
        public Thread lampthread;
        public void LampStopThread()
        {
            lampthread.IsBackground = true;
        }
        public void LampCheckThread()
        {
            Console.WriteLine("Lamp Thread Start");
            lampthread = new Thread(() =>
            {
                while (IsMainForm)
                {
                    if (!WMX3.IsEngineCommunicating())
                    {
                        Console.WriteLine("Lamp communication lost. Waiting...");

                        // WMX3가 다시 통신할 때까지 대기
                        while (!WMX3.IsEngineCommunicating())
                        {
                            Thread.Sleep(500); // 1초마다 상태 확인
                        }

                        Console.WriteLine("Lamp communication restored.");
                    }


                    LampCheck();
                    Thread.Sleep(10);
                }
            });
            lampthread.Name = "Lamp Thread";
            lampthread.Start();
        }
        private void LampCheck()
        {
            bool allAuto = true;
            bool anyManual = false;
            bool anyAlarm = false;

            if (!IsSafety)
            {
                anyAlarm = true;
            }
            else
            {
                // 모든 컨베이어의 모드를 검사
                foreach (var conveyor in conveyors)
                {
                    if (conveyor.mode == Mode.Alarm)
                    {
                        anyAlarm = true;    // 하나라도 Alarm 모드가 있으면 알람 처리
                        return;              // 알람이 있으면 더 이상 검사할 필요가 없음
                    }
                    else if (conveyor.mode == Mode.Manual)
                    {
                        anyManual = true;   // 하나라도 Manual 모드가 있으면
                        allAuto = false;    // Manual 모드가 있으면 모든 컨베이어가 Auto일 수 없음
                    }
                    else if (conveyor.mode != Mode.Auto)
                    {
                        allAuto = false;    // Auto가 아닌 다른 모드가 있으면
                    }
                }
            }
            // 모든 컨베이어가 Auto 모드일 때
            // 하나라도 Alarm 모드가 있을 때 (우선 처리)
            if (anyAlarm)
            {
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[0], 1);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[1], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[2], 0); // Alarm 램프 켜기
                IsManual = false;
                IsAlarm = true;
                if (IsBuzzerSound)
                {
                    WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[7], 1);
                }
                else
                {
                    WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[7], 0);
                }
            }
            // 모든 컨베이어가 Auto 모드일 때
            else if (allAuto)
            {
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[0], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[1], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[2], 1); // Auto 램프 켜기
                IsManual = false;
                IsAlarm = false;
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[7], 0);
            }
            // 하나라도 Manual 모드가 있을 때
            else if (anyManual)
            {
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[0], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[1], 1);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[2], 0); // Manual 램프 켜기
                IsManual = true;
                IsAlarm = false;
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, Lampbits[7], 0);
            }
        }
    }
    public class SafetyThread
    {
        public Thread safetythread;

        public void SafetyStopThread()
        {
            safetythread.IsBackground = true;
        }

        public void SafetyCheckThread()
        {
            Console.WriteLine("Safety Thread Start");
            safetythread = new Thread(() =>
            {
                while (IsMainForm)
                {
                    if (!WMX3.IsEngineCommunicating())
                    {

                        Console.WriteLine("Safety communication lost. Waiting...");

                        // WMX3가 다시 통신할 때까지 대기
                        while (!WMX3.IsEngineCommunicating())
                        {
                            Thread.Sleep(500); // 1초마다 상태 확인
                        }

                        Console.WriteLine("Safety communication restored.");
                    }
                    SafetyCheck();

                    Thread.Sleep(10);
                }
            });
            safetythread.Name = "Safety Thread";
            safetythread.Start();
        }
        /// <summary>
        /// EMS Check 기능을 하는 함수
        /// </summary>
        public void SafetyCheck()
        {
            if ((G_Var.Safety == SensorOnOff.On) && (MainPower == SensorOnOff.On) && (EMO == SensorOnOff.Off) && (EMS_1 == SensorOnOff.Off) && (EMS_2 == SensorOnOff.Off))
            {
                IsSafety = true;
            }
            else
            {
                IsSafety = false;
            }
            if (IsSafety == false)
            {
                foreach (var conveyor in conveyors)
                {
                    if (conveyor.servo == ServoOnOff.On)
                    {
                        if (conveyor.type == "Turn")
                        {
                            m_WMXMotion.ServoOff(conveyor.Axis);
                            m_WMXMotion.ServoOff(conveyor.TurnAxis);
                        }
                        else
                        {
                            m_WMXMotion.ServoOff(conveyor.Axis);
                        }
                    }
                }
            }
        }
    }
    public class RectThread
    {
        public Thread rectthread;
        public void RectStopThread()
        {
            rectthread.IsBackground = true;
        }

        public void RectCheckThread()
        {
            Console.WriteLine("Rect Thread Start");
            rectthread = new Thread(() =>
            {
                while (IsMainForm)
                {
                    if (!WMX3.IsEngineCommunicating())
                    {
                        Console.WriteLine("Rect communication lost. Waiting...");

                        // WMX3가 다시 통신할 때까지 대기
                        while (!WMX3.IsEngineCommunicating())
                        {
                            Thread.Sleep(500); // 1초마다 상태 확인
                        }

                        Console.WriteLine("Rect communication restored.");
                    }
                    RectCheck();
                    ServoRunCheck();
                    Thread.Sleep(10);
                }
            });
            rectthread.Name = "Rect Thread";
            rectthread.Start();
        }
        public void RectCheck()
        {
            foreach (var rect in rectangles)
            {
                var conveyor = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                if (conveyor != null)
                {
                    if ((conveyor.servo != conveyor.previousServo) || (conveyor.mode != conveyor.previousMode))
                    {
                        if (conveyor.servo == ServoOnOff.Off)
                        {
                            rect.FillColor = rect.Offcolor;
                        }
                        else if ((conveyor.mode == Mode.Manual) && (conveyor.servo == ServoOnOff.On))
                        {
                            rect.FillColor = rect.Manualcolor;
                        }
                        else if ((conveyor.mode == Mode.Auto) && (conveyor.servo == ServoOnOff.On))
                        {
                            rect.FillColor = rect.Autocolor;
                        }
                        else if ((conveyor.mode == Mode.Alarm) && (conveyor.servo == ServoOnOff.On))
                        {
                            rect.FillColor = rect.Alarmcolor;
                        }
                        conveyor.previousServo = conveyor.servo;
                        conveyor.previousMode = conveyor.mode;
                    }
                }
            }
        }
        public void ServoRunCheck()
        {
            foreach (var conveyor in conveyors)
            {
                if (conveyor.Axis != -1)
                {
                    if (conveyor.type == "Turn")
                    {
                        if (m_WMXMotion.IsServoRun(conveyor.Axis) || m_WMXMotion.IsServoRun(conveyor.TurnAxis))
                        {
                            conveyor.run = Conveyor.CnvRun.Run;
                        }
                        else
                        {
                            conveyor.run = Conveyor.CnvRun.Stop;
                        }
                    }
                    else
                    {
                        if (m_WMXMotion.IsServoRun(conveyor.Axis))
                        {
                            conveyor.run = Conveyor.CnvRun.Run;
                        }
                        else
                        {
                            conveyor.run = Conveyor.CnvRun.Stop;
                        }
                    }
                }
            }
        }
    }
    public class ConvThread
    {
        public Thread convthread;

        public void ServoStopThread()
        {
            convthread.IsBackground = true;
        }
        public void ServoCheckThread()
        {
            convthread = new Thread(() =>
            {
                Console.WriteLine("Servo Thread Start");
                while (IsMainForm)
                {
                    if (!WMX3.IsEngineCommunicating())
                    {
                        Console.WriteLine("Servo communication lost. Waiting...");

                        // WMX3가 다시 통신할 때까지 대기
                        while (!WMX3.IsEngineCommunicating())
                        {
                            Thread.Sleep(500); // 1초마다 상태 확인
                        }

                        Console.WriteLine("Servo communication restored.");
                    }

                    // 통신이 가능하면 ServoCheck 호출
                    ServoCheck();

                    Thread.Sleep(10); // 10ms 대기 후 다시 체크
                }
            });
            convthread.Name = "ConvThread";
            convthread.Start();
        }
        public void ServoCheck()
        {
            foreach (var conveyor in conveyors)
            {
                if (conveyor.Axis != -1)
                {
                    if (conveyor.type == "Turn")
                    {
                        conveyor.servo = ((m_WMXMotion.IsServoOn(conveyor.Axis) == true) && (m_WMXMotion.IsServoOn(conveyor.TurnAxis) == true)) ? ServoOnOff.On : ServoOnOff.Off;
                    }
                    else
                    {
                        conveyor.servo = (m_WMXMotion.IsServoOn(conveyor.Axis) == true) ? ServoOnOff.On : ServoOnOff.Off;
                    }
                }
            }
        }
    }
    public class LineThread
    {
        public Thread linethread;

        public void LineStopThread()
        {
            linethread.IsBackground = true;
        }
        public void LineCheckThread()
        {
            linethread = new Thread(() =>
            {
                Console.WriteLine("Line Thread Start");
                while (IsMainForm)
                {
                    if (!WMX3.IsEngineCommunicating())
                    {
                        Console.WriteLine("Line communication lost. Waiting...");

                        // WMX3가 다시 통신할 때까지 대기
                        while (!WMX3.IsEngineCommunicating())
                        {
                            Thread.Sleep(500); // 1초마다 상태 확인
                        }

                        Console.WriteLine("Line communication restored.");
                    }
                    LineCheck();

                    Thread.Sleep(100);
                }
            });
            linethread.Name = "LineThread";
            linethread.Start();
        }
        public void LineCheck()
        {
            foreach (var line in lines)
            {
                line.StatusCheck();
            }
        }
    }

}
