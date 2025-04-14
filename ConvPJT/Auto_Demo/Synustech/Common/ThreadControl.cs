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

    public class ThreadControl
    {
        public Thread iothread;
        public void IoThread()
        {
            Console.WriteLine("IO Thread Start");
            iothread = new Thread(() =>
            {
                while (isMainFormOpen)
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
                    CheckSafetyIo();
                    CheckAutoCondition();
                    CheckIoInput();
                    CheckIoOutput();
                    CheckLamp();
                    CheckSafety();
                    CheckPIO();
                    Thread.Sleep(10);
                }
            });
            iothread.Name = "IOThread";
            iothread.Start();
        }
        /// <summary>
        /// Conveyor Input IO Check
        /// </summary>
        private void CheckIoInput()
        {
            foreach (var conveyor in conveyors.ToList())
            {
                if (conveyor.axis == -1) { }
                else if (conveyor.type == "Turn" && conveyor.axis != -1)
                {
                    conveyor.AddrUpdate();
                    int i = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        WMX3.m_wmx3io.GetInBit(conveyor.addr, bit, ref G_Var.byInput[0]);
                        bool inputStatus = (G_Var.byInput[0] == 1) ? false : true;
                        conveyor.normalCnvFoupDetect[i] = (inputStatus == false) ? SensorOnOff.On : SensorOnOff.Off;
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
                else if (conveyor.type == "Long" && conveyor.axis != -1)
                {
                    conveyor.AddrUpdate();
                    int i = 0;
                    foreach (var bit in conveyor.bits)
                    {
                        WMX3.m_wmx3io.GetInBit(conveyor.addr, bit, ref G_Var.byInput[0]);
                        bool inputStatus = (G_Var.byInput[0] == 1) ? false : true;
                        conveyor.longCnvFoupDetect[i] = (inputStatus == false) ? SensorOnOff.On : SensorOnOff.Off;
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
                        conveyor.normalCnvFoupDetect[i] = (inputStatus == false) ? SensorOnOff.On : SensorOnOff.Off;
                        i++;
                    }
                }
                switch (conveyor.type)
                {
                    case "Normal":
                        if (w_motion.m_axisParameter[conveyor.axis].m_motorDirection == WMXParam.m_motorDirection.Positive)
                            (conveyor.normalCnvFoupDetect[0], conveyor.normalCnvFoupDetect[1]) = (conveyor.normalCnvFoupDetect[1], conveyor.normalCnvFoupDetect[0]);
                        break;
                    case "Turn":
                        if (w_motion.m_axisParameter[conveyor.axis].m_motorDirection == WMXParam.m_motorDirection.Negative)
                            (conveyor.normalCnvFoupDetect[0], conveyor.normalCnvFoupDetect[1]) = (conveyor.normalCnvFoupDetect[1], conveyor.normalCnvFoupDetect[0]);
                        break;
                    case "Long":
                        if (w_motion.m_axisParameter[conveyor.axis].m_motorDirection == WMXParam.m_motorDirection.Positive)
                            (conveyor.longCnvFoupDetect[0], conveyor.longCnvFoupDetect[2]) = (conveyor.longCnvFoupDetect[2], conveyor.longCnvFoupDetect[0]);
                        break;
                }
            }
        }
        /// <summary>
        /// Safety 관련 IO Check
        /// </summary>
        private void CheckSafetyIo()
        {
            WMX3.m_wmx3io.GetInBit(addrSafetyIn, bitSafetyIn, ref G_Var.byInput[1]);
            bool inputSafety = (G_Var.byInput[1] == 1) ? true : false;
            G_Var.Safety = (inputSafety == true) ? SensorOnOff.On : SensorOnOff.Off;
            
            WMX3.m_wmx3io.GetInBit(addrSafetyIn, bitMainPower, ref G_Var.byInput[7]);
            bool inputMainPower = (G_Var.byInput[7] == 1) ? true : false;
            G_Var.MainPower = (inputMainPower == true) ? SensorOnOff.On : SensorOnOff.Off;
            if(G_Var.MainPower == SensorOnOff.Off && !G_Var.isMainPowerAlarm)
            {
                G_Var.isMainPowerAlarm = true;
                AddAlarm_Master(ConveyorAlarm.Power_Off_Error);
                del_alarm?. Invoke();
                isAlarm = true;
                Console.WriteLine("MainPower_Error");
            }

            WMX3.m_wmx3io.GetInBit(addrSafetyIn, bitModeChange, ref G_Var.byInput[8]);
            bool inputModeChange = (G_Var.byInput[8] == 1) ? true : false;
            G_Var.Mode_Change = (inputModeChange == true) ? SensorOnOff.On :SensorOnOff.Off;

            WMX3.m_wmx3io.GetInBit(addrSafetyIn, bitEMO, ref G_Var.byInput[2]);
            bool inputEMO = (G_Var.byInput[2] == 1) ? true : false;
            G_Var.EMO = (inputEMO == true) ? SensorOnOff.On : SensorOnOff.Off;
            if(G_Var.EMO == SensorOnOff.On && !G_Var.isEMOAlarm)
            {
                G_Var.isEMOAlarm = true;
                AddAlarm_Master(ConveyorAlarm.MainPanel_EMO);
                del_alarm?. Invoke();
                isAlarm = true;
                Console.WriteLine("EMO Error");
            }

            WMX3.m_wmx3io.GetInBit(addrEMS_1, bitEMS, ref G_Var.byInput[3]);
            bool inputEMS_1 = (G_Var.byInput[3] == 1) ? true : false;
            G_Var.EMS_1 = (inputEMS_1 == true) ? SensorOnOff.On : SensorOnOff.Off;

            WMX3.m_wmx3io.GetInBit(addrEMS_2, bitEMS, ref G_Var.byInput[4]);
            bool inputEMS_2 = (G_Var.byInput[4] == 1) ? true : false;
            G_Var.EMS_2 = (inputEMS_2 == true) ? SensorOnOff.On : SensorOnOff.Off;
            if((G_Var.EMS_1 == SensorOnOff.On || G_Var.EMS_2 == SensorOnOff.On) && !G_Var.isEMSAlarm)
            {
                G_Var.isEMSAlarm = true;
                AddAlarm_Master(ConveyorAlarm.Conveyor_E_Stop);
                del_alarm?.Invoke();
                isAlarm = true;
                Console.WriteLine("EMS Error");
            }
            int j = 0;
            foreach (var bit in OHTReceiveBits)
            {
                WMX3.m_wmx3io.GetInBit(addrOHTPIO, bit, ref byInput[0]);
                bool outPutStatus1 = (G_Var.byInput[0] == 1) ? true : false;
                OHTPIOReceive[j++] = (outPutStatus1 == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
            int z = 0;
            foreach (var bit in AGVReceiveBits)
            {
                WMX3.m_wmx3io.GetInBit(addrAGVPIO, bit, ref byInput[0]);
                bool outPutStatus1 = (G_Var.byInput[0] == 1) ? true : false;
                AGVPIOReceive[z++] = (outPutStatus1 == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
            
        }
        /// <summary>
        /// Auto Condition Check
        /// </summary>
        private void CheckAutoCondition()
        {
            if (isSafety && (conveyors.All(conveyor => conveyor.servo == ServoOnOff.On)) && !isAlarm && G_Var.Mode_Change == SensorOnOff.On)
            {
                isAutoEnable = true;
            }
            else
            {
                isAutoEnable = false;
            }
            //if (IsAlarm && (IsAutoRun || IsCycleRun))
            //{
            //    IsAuto = false;
            //    IsCycleRun = false;
            //}
        }
        /// <summary>
        /// OutPut IO Check
        /// </summary>
        public void CheckIoOutput()
        {
            WMX3.m_wmx3io.GetOutBit(addrSafetyReset, bitSafetyReset, ref byOutput[0]);
            bool outPutStatus = (G_Var.byOutput[0] == 1) ? true : false;
            SafetyReset = (outPutStatus == true) ? SensorOnOff.On : SensorOnOff.Off;
            int i = 0;
            foreach (var bit in bitsLamp)
            {
                WMX3.m_wmx3io.GetOutBit(addrLamp_Buzz, bit, ref byOutput[0]);
                bool outPutStatus1 = (G_Var.byOutput[0] == 1) ? true : false;
                Lamp_Buzz[i++] = (outPutStatus1 == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
            int z = 0;
            foreach (var bit in OHTSendBits)
            {
                WMX3.m_wmx3io.GetOutBit(addrOHTPIO, bit, ref G_Var.byOutput[0]);
                bool outOHT = (G_Var.byOutput[0] == 1) ? true : false;
                G_Var.OHTPIOSend[z++] = (outOHT == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
            int j = 0;
            foreach (var bit in AGVSendBits)
            {
                WMX3.m_wmx3io.GetOutBit(addrAGVPIO, bit, ref G_Var.byOutput[0]);
                bool outAGV = (G_Var.byOutput[0] == 1) ? true : false;
                G_Var.AGVPIOSend[j++] = (outAGV == true) ? SensorOnOff.On : SensorOnOff.Off;
            }
        }
        private void CheckLamp()
        {
            // 모든 컨베이어의 모드를 검사
            foreach (var conveyor in conveyors)
            {
                if (conveyor.mode == Mode.Alarm)
                {
                    isAlarm = true;    // 하나라도 Alarm 모드가 있으면 알람 처리
                    break;              // 알람이 있으면 더 이상 검사할 필요가 없음
                }
            }

            // 모든 컨베이어가 Auto 모드일 때
            // 하나라도 Alarm 모드가 있을 때 (우선 처리)
            if (isAlarm)
            {
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[0], 1);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[1], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[2], 0); // Alarm 램프 켜기
                if (isBuzzerSound)
                {
                    WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 1);
                }
                else
                {
                    WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 0);
                }
            }
            // 모든 컨베이어가 Auto 모드일 때
            else if (isAutoRun || isCycleRun)
            {
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[0], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[1], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[2], 1); // Auto 램프 켜기
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 0);
            }
            // 하나라도 Manual 모드가 있을 때
            else if (!isAutoRun && !isCycleRun)
            {
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[0], 0);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[1], 1);
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[2], 0); // Manual 램프 켜기
                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 0);
            }
        }
        /// <summary>
        /// EMS Check 기능을 하는 함수
        /// </summary>
        public void CheckSafety()
        {
            if ((G_Var.Safety == SensorOnOff.On) && (MainPower == SensorOnOff.On) && (EMO == SensorOnOff.Off) && (EMS_1 == SensorOnOff.Off) && (EMS_2 == SensorOnOff.Off))
            {
                isSafety = true;
            }
            else
            {
                isSafety = false;
            }
            if (isSafety == false)
            {
                isAutoRun = false;
                isCycleRun = false;
                if (EMO == SensorOnOff.On)
                {
                    foreach (var conveyor in conveyors)
                    {
                        if (conveyor.servo == ServoOnOff.On)
                        {
                            if (conveyor.type == "Turn")
                            {
                                w_motion.ServoOff(conveyor.axis);
                                w_motion.ServoOff(conveyor.turnAxis);
                            }
                            else
                            {
                                w_motion.ServoOff(conveyor.axis);
                            }

                        }
                    }
                }
                else if (EMO == SensorOnOff.Off && (EMS_1 == SensorOnOff.On || EMS_2 == SensorOnOff.On))
                {
                    foreach (var conv in conveyors)
                    {
                        if (conv.servo == ServoOnOff.On)
                        {
                            conv.StopConveyor();
                            if (conv.type == "Turn")
                            {
                                conv.TurnJogSTOP();
                            }
                        }
                        conv.mode = Mode.Alarm;
                    }
                }
            }
        }
        public void CheckPIO()
        {
            L_REQ = (G_Var.OHTPIOSend[0] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
            UL_REQ = (G_Var.OHTPIOSend[1] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
            READY = (G_Var.OHTPIOSend[2] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
            HO_AVBL = (G_Var.OHTPIOSend[3] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;

            VALID = (G_Var.OHTPIOReceive[0] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
            CS_0 = (G_Var.OHTPIOReceive[1] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
            TR_REQ = (G_Var.OHTPIOReceive[3] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
            BUSY = (G_Var.OHTPIOReceive[4] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
            COMPT = (G_Var.OHTPIOReceive[5] == SensorOnOff.On) ? SensorOnOff.On : SensorOnOff.Off;
        }
        public Thread convthread;
        public void ServoCheckThread()
        {
            convthread = new Thread(() =>
            {
                Console.WriteLine("Conv Thread Start");
                while (isMainFormOpen)
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
                    CheckServoOnOff();
                    CheckServoRun();

                    Thread.Sleep(10); // 10ms 대기 후 다시 체크
                }
            });
            convthread.Name = "ConvThread";
            convthread.Start();
        }
        public void CheckServoOnOff()
        {
            foreach (var conveyor in conveyors)
            {
                if (conveyor.axis != -1)
                {
                    if (conveyor.type == "Turn")
                    {
                        conveyor.servo = ((w_motion.IsServoOn(conveyor.axis) == true) && (w_motion.IsServoOn(conveyor.turnAxis) == true)) ? ServoOnOff.On : ServoOnOff.Off;
                    }
                    else
                    {
                        conveyor.servo = (w_motion.IsServoOn(conveyor.axis) == true) ? ServoOnOff.On : ServoOnOff.Off;
                    }
                }
            }
        }
        public void CheckServoRun()
        {
            foreach (var conveyor in conveyors)
            {
                if (conveyor.axis != -1)
                {
                    if (conveyor.type == "Turn")
                    {
                        if (w_motion.IsServoRun(conveyor.axis) || w_motion.IsServoRun(conveyor.turnAxis))
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
                        if (w_motion.IsServoRun(conveyor.axis))
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

        public Thread linethread;
        public void LineCheckThread()
        {
            linethread = new Thread(() =>
            {
                Console.WriteLine("Line Thread Start");
                while (isMainFormOpen)
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
                    CheckLine();
                    CheckRect();

                    Thread.Sleep(100);
                }
            });
            linethread.Name = "LineThread";
            linethread.Start();
        }
        public void CheckLine()
        {
            foreach (var line in lines)
            {
                line.StatusCheck();
            }
        }
        public void CheckRect()
        {
            foreach (var rect in rectangles.ToList())
            {
                var conveyor = conveyors.FirstOrDefault(c => c.ID == rect.ID);
                if (conveyor != null)
                {
                    if ((conveyor.servo != conveyor.preServo) || (conveyor.mode != conveyor.preMode))
                    {
                        if (conveyor.servo == ServoOnOff.Off)
                        {
                            rect.fillColor = rect.offColor;
                        }
                        else if ((conveyor.mode == Mode.Manual) && (conveyor.servo == ServoOnOff.On))
                        {
                            rect.fillColor = rect.manualColor;
                        }
                        else if ((conveyor.mode == Mode.Auto) && (conveyor.servo == ServoOnOff.On))
                        {
                            rect.fillColor = rect.autoColor;
                        }
                        else if ((conveyor.mode == Mode.Alarm) && (conveyor.servo == ServoOnOff.On))
                        {
                            rect.fillColor = rect.alarmColor;
                        }
                        conveyor.preServo = conveyor.servo;
                        conveyor.preMode = conveyor.mode;
                    }
                }
            }
        }
    }
    //public class LampThread
    //{
    //    public Thread lampthread;
    //    public void LampStopThread()
    //    {
    //        lampthread.IsBackground = true;
    //    }
    //    public void LampCheckThread()
    //    {
    //        Console.WriteLine("Lamp Thread Start");
    //        lampthread = new Thread(() =>
    //        {
    //            while (isMainFormOpen)
    //            {
    //                if (!WMX3.IsEngineCommunicating())
    //                {
    //                    Console.WriteLine("Lamp communication lost. Waiting...");

    //                    // WMX3가 다시 통신할 때까지 대기
    //                    while (!WMX3.IsEngineCommunicating())
    //                    {
    //                        Thread.Sleep(500); // 1초마다 상태 확인
    //                    }

    //                    Console.WriteLine("Lamp communication restored.");
    //                }


    //                LampCheck();
    //                Thread.Sleep(10);
    //            }
    //        });
    //        lampthread.Name = "Lamp Thread";
    //        lampthread.Start();
    //    }
    //    private void LampCheck()
    //    {

    //        // 모든 컨베이어의 모드를 검사
    //        foreach (var conveyor in conveyors)
    //        {
    //            if (conveyor.mode == Mode.Alarm)
    //            {
    //                IsAlarm = true;    // 하나라도 Alarm 모드가 있으면 알람 처리
    //                break;              // 알람이 있으면 더 이상 검사할 필요가 없음
    //            }
    //        }

    //        // 모든 컨베이어가 Auto 모드일 때
    //        // 하나라도 Alarm 모드가 있을 때 (우선 처리)
    //        if (IsAlarm)
    //        {
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[0], 1);
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[1], 0);
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[2], 0); // Alarm 램프 켜기
    //            if (isBuzzerSound)
    //            {
    //                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 1);
    //            }
    //            else
    //            {
    //                WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 0);
    //            }
    //        }
    //        // 모든 컨베이어가 Auto 모드일 때
    //        else if (isAutoRun || isCycleRun)
    //        {
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[0], 0);
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[1], 0);
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[2], 1); // Auto 램프 켜기
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 0);
    //        }
    //        // 하나라도 Manual 모드가 있을 때
    //        else if (!isAutoRun && !isCycleRun)
    //        {
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[0], 0);
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[1], 1);
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[2], 0); // Manual 램프 켜기
    //            WMX3.m_wmx3io.SetOutBit(addrLamp_Buzz, bitsLamp[7], 0);
    //        }
    //    }
    //}
    //public class SafetyThread
    //{
    //    public Thread safetythread;

    //    public void SafetyStopThread()
    //    {
    //        safetythread.IsBackground = true;
    //    }

    //    public void SafetyCheckThread()
    //    {
    //        Console.WriteLine("Safety Thread Start");
    //        safetythread = new Thread(() =>
    //        {
    //            while (isMainFormOpen)
    //            {
    //                if (!WMX3.IsEngineCommunicating())
    //                {

    //                    Console.WriteLine("Safety communication lost. Waiting...");

    //                    // WMX3가 다시 통신할 때까지 대기
    //                    while (!WMX3.IsEngineCommunicating())
    //                    {
    //                        Thread.Sleep(500); // 1초마다 상태 확인
    //                    }

    //                    Console.WriteLine("Safety communication restored.");
    //                }
    //                SafetyCheck();

    //                Thread.Sleep(10);
    //            }
    //        });
    //        safetythread.Name = "Safety Thread";
    //        safetythread.Start();
    //    }
    //    /// <summary>
    //    /// EMS Check 기능을 하는 함수
    //    /// </summary>
    //    public void SafetyCheck()
    //    {
    //        if ((G_Var.Safety == SensorOnOff.On) && (MainPower == SensorOnOff.On) && (EMO == SensorOnOff.Off) && (EMS_1 == SensorOnOff.Off) && (EMS_2 == SensorOnOff.Off))
    //        {
    //            isSafety = true;
    //        }
    //        else
    //        {
    //            isSafety = false;
    //        }
    //        if (isSafety == false)
    //        {
    //            isAutoRun = false;
    //            isCycleRun = false;
    //            if (EMO == SensorOnOff.On)
    //            {
    //                foreach (var conveyor in conveyors)
    //                {
    //                    if (conveyor.servo == ServoOnOff.On)
    //                    {
    //                        if (conveyor.type == "Turn")
    //                        {
    //                            w_motion.ServoOff(conveyor.Axis);
    //                            w_motion.ServoOff(conveyor.TurnAxis);
    //                        }
    //                        else
    //                        {
    //                            w_motion.ServoOff(conveyor.Axis);
    //                        }

    //                    }
    //                }
    //            }
    //            else if(EMO == SensorOnOff.Off && (EMS_1 == SensorOnOff.On || EMS_2 == SensorOnOff.On))
    //            {
    //                foreach(var conv in conveyors)
    //                {
    //                    if (conv.servo == ServoOnOff.On)
    //                    {
    //                        conv.StopConveyor();
    //                        if (conv.type == "Turn")
    //                        {
    //                            conv.TurnJogSTOP();
    //                        }
    //                    }
    //                    conv.mode = Mode.Alarm;
    //                }
    //            }
    //        }
    //    }
    //}
    //public class RectThread
    //{
    //    public Thread rectthread;
    //    public void RectStopThread()
    //    {
    //        rectthread.IsBackground = true;
    //    }

    //    public void RectCheckThread()
    //    {
    //        Console.WriteLine("Rect Thread Start");
    //        rectthread = new Thread(() =>
    //        {
    //            while (isMainFormOpen)
    //            {
    //                if (!WMX3.IsEngineCommunicating())
    //                {
    //                    Console.WriteLine("Rect communication lost. Waiting...");

    //                    // WMX3가 다시 통신할 때까지 대기
    //                    while (!WMX3.IsEngineCommunicating())
    //                    {
    //                        Thread.Sleep(500); // 1초마다 상태 확인
    //                    }

    //                    Console.WriteLine("Rect communication restored.");
    //                }
    //                RectCheck();
    //                ServoRunCheck();
    //                Thread.Sleep(10);
    //            }
    //        });
    //        rectthread.Name = "Rect Thread";
    //        rectthread.Start();
    //    }
    //    public void RectCheck()
    //    {
    //        foreach (var rect in rectangles.ToList())
    //        {
    //            var conveyor = conveyors.FirstOrDefault(c => c.ID == rect.ID);
    //            if (conveyor != null)
    //            {
    //                if ((conveyor.servo != conveyor.previousServo) || (conveyor.mode != conveyor.previousMode))
    //                {
    //                    if (conveyor.servo == ServoOnOff.Off)
    //                    {
    //                        rect.FillColor = rect.Offcolor;
    //                    }
    //                    else if ((conveyor.mode == Mode.Manual) && (conveyor.servo == ServoOnOff.On))
    //                    {
    //                        rect.FillColor = rect.Manualcolor;
    //                    }
    //                    else if ((conveyor.mode == Mode.Auto) && (conveyor.servo == ServoOnOff.On))
    //                    {
    //                        rect.FillColor = rect.Autocolor;
    //                    }
    //                    else if ((conveyor.mode == Mode.Alarm) && (conveyor.servo == ServoOnOff.On))
    //                    {
    //                        rect.FillColor = rect.Alarmcolor;
    //                    }
    //                    conveyor.previousServo = conveyor.servo;
    //                    conveyor.previousMode = conveyor.mode;
    //                }
    //            }
    //        }
    //    }
    //    public void ServoRunCheck()
    //    {
    //        foreach (var conveyor in conveyors)
    //        {
    //            if (conveyor.Axis != -1)
    //            {
    //                if (conveyor.type == "Turn")
    //                {
    //                    if (w_motion.IsServoRun(conveyor.Axis) || w_motion.IsServoRun(conveyor.TurnAxis))
    //                    {
    //                        conveyor.run = Conveyor.CnvRun.Run;
    //                    }
    //                    else
    //                    {
    //                        conveyor.run = Conveyor.CnvRun.Stop;
    //                    }
    //                }
    //                else
    //                {
    //                    if (w_motion.IsServoRun(conveyor.Axis))
    //                    {
    //                        conveyor.run = Conveyor.CnvRun.Run;
    //                    }
    //                    else
    //                    {
    //                        conveyor.run = Conveyor.CnvRun.Stop;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //public class ConvThread
    //{
    //    public Thread convthread;

    //    public void ServoStopThread()
    //    {
    //        convthread.IsBackground = true;
    //    }
    //    public void ServoCheckThread()
    //    {
    //        convthread = new Thread(() =>
    //        {
    //            Console.WriteLine("Servo Thread Start");
    //            while (isMainFormOpen)
    //            {
    //                if (!WMX3.IsEngineCommunicating())
    //                {
    //                    Console.WriteLine("Servo communication lost. Waiting...");

    //                    // WMX3가 다시 통신할 때까지 대기
    //                    while (!WMX3.IsEngineCommunicating())
    //                    {
    //                        Thread.Sleep(500); // 1초마다 상태 확인
    //                    }

    //                    Console.WriteLine("Servo communication restored.");
    //                }

    //                // 통신이 가능하면 ServoCheck 호출
    //                ServoCheck();

    //                Thread.Sleep(10); // 10ms 대기 후 다시 체크
    //            }
    //        });
    //        convthread.Name = "ConvThread";
    //        convthread.Start();
    //    }
    //    public void ServoCheck()
    //    {
    //        foreach (var conveyor in conveyors)
    //        {
    //            if (conveyor.Axis != -1)
    //            {
    //                if (conveyor.type == "Turn")
    //                {
    //                    conveyor.servo = ((w_motion.IsServoOn(conveyor.Axis) == true) && (w_motion.IsServoOn(conveyor.TurnAxis) == true)) ? ServoOnOff.On : ServoOnOff.Off;
    //                }
    //                else
    //                {
    //                    conveyor.servo = (w_motion.IsServoOn(conveyor.Axis) == true) ? ServoOnOff.On : ServoOnOff.Off;
    //                }
    //            }
    //        }
    //    }
    //}
    //public class LineThread
    //{
    //    public Thread linethread;

    //    public void LineStopThread()
    //    {
    //        linethread.IsBackground = true;
    //    }
    //    public void LineCheckThread()
    //    {
    //        linethread = new Thread(() =>
    //        {
    //            Console.WriteLine("Line Thread Start");
    //            while (isMainFormOpen)
    //            {
    //                if (!WMX3.IsEngineCommunicating())
    //                {
    //                    Console.WriteLine("Line communication lost. Waiting...");

    //                    // WMX3가 다시 통신할 때까지 대기
    //                    while (!WMX3.IsEngineCommunicating())
    //                    {
    //                        Thread.Sleep(500); // 1초마다 상태 확인
    //                    }

    //                    Console.WriteLine("Line communication restored.");
    //                }
    //                LineCheck();

    //                Thread.Sleep(100);
    //            }
    //        });
    //        linethread.Name = "LineThread";
    //        linethread.Start();
    //    }
    //    public void LineCheck()
    //    {
    //        foreach (var line in lines)
    //        {
    //            line.StatusCheck();
    //        }
    //    }
    //}

}
