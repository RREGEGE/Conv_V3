using Master.Interface.Alarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_STK_PIO_Step.cs는 Auto 공정에서 STK와의 PIO 스텝에 사용 할 기능을 함수화 하여 작성
    /// </summary>
    partial class Port
    {
        /// <summary>
        /// Input 방향 TR, Busy, Complete, END 체크
        /// Await : 대기
        /// Check : 확인
        /// </summary>
        /// <returns></returns>
        private bool INPUT_DIR_STK_TR_REQ_Await()
        {
            bool ret = false;

            //자재가 있는 것을 확인
            //OMRON Type의 Diebank의 경우 자재를 뜰 땐 눌림 스위치를 봐야하고 아닐 땐 대각 센서를 봐야 함. 
            bool OP_CST_Exist = GetParam().ePortType != PortType.Conveyor_OMRON ? Carrier_CheckOP_ExistProduct(true) : (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2);

            PIOStatus_PortToSTK_Unload_Req = true;

            if (OP_CST_Exist && PIOStatus_STKToPort_TR_REQ)
                ret = true;

            if (ret)
            {
                Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                //Carrier_ClearPortToRM_CarrierID();
            }

            return ret;
        }

        private bool INPUT_DIR_STK_BUSY_Check()
        {
            bool ret = false;

            //자재가 있는 것을 확인
            //OMRON Type의 Diebank의 경우 자재를 뜰 땐 눌림 스위치를 봐야하고 아닐 땐 대각 센서를 봐야 함. 
            bool OP_CST_Exist = GetParam().ePortType != PortType.Conveyor_OMRON ? Carrier_CheckOP_ExistProduct(true) : (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2);

            if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
            {
                if (!PIOStatus_STKToPort_Busy)
                    AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
            }

            if (OP_CST_Exist && PIOStatus_STKToPort_Busy)
                ret = true;

            if (ret)
            {
                PIOStatus_PortToSTK_Ready = true;
                Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);

                m_RMCSTIDRWTimer.Stop();
                m_RMCSTIDRWTimer.Reset();
                m_RMCSTIDRWTimer.Start();
            }

            return ret;
        }

        private bool INPUT_DIR_STK_COMPLETE_Check()
        {
            bool ret = false;

            //자재가 사라진 것을 확인
            bool OP_CST_NotExist_Without_Presence  = GetParam().ePortType != PortType.Conveyor_OMRON ? Carrier_CheckOP_ExistProduct(false, false) : (!Sensor_OP_CST_Detect1 && !Sensor_OP_CST_Detect2);
            bool OP_CST_NotExist                   = GetParam().ePortType != PortType.Conveyor_OMRON ? Carrier_CheckOP_ExistProduct(false) : (!Sensor_OP_CST_Detect1 && !Sensor_OP_CST_Detect2);

            if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
            {
                if (!PIOStatus_STKToPort_Busy || !PIOStatus_STKToPort_Complete)
                    AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
            }

            if (PIOStatus_STKToPort_Busy && PIOStatus_STKToPort_Complete)
                Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);

            //New Code 2023-10-23 실물 자재가 사라진 순간 부터 ID 전송
            if (OP_CST_NotExist_Without_Presence)
            {
                string OPCarrierID = OP_CarrierID;
                if (!string.IsNullOrEmpty(OPCarrierID))
                {
                    //OP ID를 CIM->STK word 영역에 복사 후 삭제
                    Carrier_ACK_PortToRM_CarrierID(OPCarrierID);
                    OP_CarrierID = string.Empty;
                }
                else
                {
                    //CIM->STK word 영역의 CST ID를 계속 전송
                    string GetCarrierID = Carrier_GetPortToRM_SendMapCarrierID();
                    Carrier_ACK_PortToRM_CarrierID(GetCarrierID);
                }
            }


            if (OP_CST_NotExist &&
                PIOStatus_STKToPort_Busy &&
                PIOStatus_STKToPort_Complete)
            {
                if (Carrier_GetRMToPort_RecvMapCarrierID() == Carrier_GetPortToRM_SendMapCarrierID())
                    ret = true;
                else if (m_RMCSTIDRWTimer.Elapsed.TotalSeconds > 30.0)
                    ret = true;
            }

            if(ret)
            {
                PIOStatus_PortToSTK_Unload_Req = false;
                m_RMCSTIDRWTimer.Stop();

                if (m_RMCSTIDRWTimer.Elapsed.TotalSeconds > 30.0)
                {
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"Port -> RM CST ID Write Timeout");
                    m_RMCSTIDRWTimer.Reset();
                }
            }

            return ret;
        }

        private bool INPUT_DIR_STK_PIO_END_Check()
        {
            bool ret = false;

            if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
            {
                if (PIOStatus_STKToPort_TR_REQ || PIOStatus_STKToPort_Busy || PIOStatus_STKToPort_Complete)
                    AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
            }

            if (!PIOStatus_STKToPort_TR_REQ &&
                !PIOStatus_STKToPort_Busy &&
                !PIOStatus_STKToPort_Complete)
            {
                ret = true;
            }

            if(ret)
            {
                PIOStatus_PortToSTK_Ready = false;
                Carrier_ClearPortToRM_CarrierID();
            }

            return ret;
        }
    
        /// <summary>
        /// Output 방향 TR, Busy, Complete, END 체크
        /// Await : 대기
        /// Check : 확인
        /// </summary>
        /// <returns></returns>
        private bool OUTPUT_DIR_STK_PIO_TR_REQ_Await()
        {
            bool ret = false;

            PIOStatus_PortToSTK_Load_Req = true;

            if (PIOStatus_STKToPort_TR_REQ)
            {
                ret = true;
            }

            if(ret)
            {
                Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
            }

            return ret;
        }

        private bool OUTPUT_DIR_STK_PIO_BUSY_Check()
        {
            bool ret = false;

            if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
            {
                if (!PIOStatus_STKToPort_Busy)
                    AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
            }

            if (PIOStatus_STKToPort_Busy)
            {
                ret = true;
            }

            if(ret)
            {
                PIOStatus_PortToSTK_Ready = true;
                Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);
                m_RMCSTIDRWTimer.Stop();
                m_RMCSTIDRWTimer.Reset();
                m_RMCSTIDRWTimer.Start();
            }

            return ret;
        }

        private bool OUTPUT_DIR_STK_PIO_COMPLETE_Check()
        {
            bool ret = false;

            //자재가 생긴 것을 확인
            bool OP_CST_Exist_Without_Presence  = GetParam().ePortType != PortType.Conveyor_OMRON ? Carrier_CheckOP_ExistProduct(true, false) : (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2);
            bool OP_CST_Exist                   = GetParam().ePortType != PortType.Conveyor_OMRON ? Carrier_CheckOP_ExistProduct(true) : (Sensor_OP_CST_Detect1 && Sensor_OP_CST_Detect2);


            if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
            {
                if (!PIOStatus_STKToPort_Busy || !PIOStatus_STKToPort_Complete)
                    AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
            }

            if (PIOStatus_STKToPort_Busy && PIOStatus_STKToPort_Complete)
                Watchdog_Restart(WatchdogList.RackMaster_PIO_Timer);

            //New Code - 실물 자재가 확인된 순간 부터 ID 리딩
            if (OP_CST_Exist_Without_Presence)
            {
                string RMCarrierID = Carrier_GetRMToPort_RecvMapCarrierID();
                Carrier_ACK_PortToRM_CarrierID(RMCarrierID);

                if (RMCarrierID != string.Empty)
                    OP_CarrierID = RMCarrierID;
            }

            if (OP_CST_Exist &&
                PIOStatus_STKToPort_Busy &&
                PIOStatus_STKToPort_Complete)
            {
                if (OP_CarrierID != string.Empty)
                {
                    ret = true;
                }
                else if (m_RMCSTIDRWTimer.Elapsed.TotalSeconds > 30.0)
                {
                    ret = true;
                }
            }

            if(ret)
            {
                PIOStatus_PortToSTK_Load_Req = false;
                m_RMCSTIDRWTimer.Stop();

                if(m_RMCSTIDRWTimer.Elapsed.TotalSeconds > 30.0)
                {
                    LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Normal, LogMsg.MsgList.PortCSTInfo, $"RM -> Port CST ID Read Timeout");
                    m_RMCSTIDRWTimer.Reset();
                    OP_CarrierID = "CST_ID_READ_FAIL";
                }
            }

            return ret;
        }

        private bool OUTPUT_DIR_STK_PIO_END_Check()
        {
            bool ret = false;

            if (Watchdog_IsDetect(WatchdogList.RackMaster_PIO_Timer))
            {
                if (PIOStatus_STKToPort_TR_REQ || PIOStatus_STKToPort_Busy || PIOStatus_STKToPort_Complete)
                    AlarmInsert((short)PortAlarm.RM_PIO_IF_TimeOut_Error, AlarmLevel.Error);
            }

            if (!PIOStatus_STKToPort_TR_REQ &&
                !PIOStatus_STKToPort_Busy &&
                !PIOStatus_STKToPort_Complete)
            {
                ret = true;
            }

            if(ret)
            {
                Carrier_ClearPortToRM_CarrierID();
                PIOStatus_PortToSTK_Ready = false;
                Watchdog_Stop(WatchdogList.RackMaster_PIO_Timer, true);
            }

            return ret;
        }
    }
}
