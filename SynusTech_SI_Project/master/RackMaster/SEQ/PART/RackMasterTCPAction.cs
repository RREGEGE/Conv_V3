using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.TCP;
using RackMaster.SEQ.COMMON;
using System.Threading;

namespace RackMaster.SEQ.PART {
    public partial class RackMasterTCP {
        private Motor m_motor;
        private RackMasterMotion m_rackMaster;

        private object m_localThreadObj = null;

        private static bool[] m_RackMaster_RecvBitMap;
        private static bool[] m_RackMaster_SendBitMap;
        private static short[] m_RackMaster_RecvWordMap;
        private static short[] m_RackMaster_SendWordMap;

        private static PortData m_teacingData = null;

        private static short m_maxOverloadX = 0;
        private static short m_maxOverloadZ = 0;
        private static short m_maxOverloadA = 0;
        private static short m_maxOverloadT = 0;

        private static bool m_isCompareCSTID = false;

        private static int m_sendCycleTime = 0;

        public void SetRackMasterInstance(RackMasterMotion rackMaster) {
            m_rackMaster = rackMaster;
        }

        public static bool GetReceiveBit(TcpDataDef.ReceiveBitMap bitMap) {
            return m_RackMaster_RecvBitMap[(int)bitMap];
        }

        public static void SetRecevieBit_TEST(TcpDataDef.ReceiveBitMap bitmap, bool value)
        {
            m_RackMaster_RecvBitMap[(int)bitmap] = value;
        }

        public static bool GetSendBit(TcpDataDef.SendBitMap bitMap) {
            return m_RackMaster_SendBitMap[(int)bitMap];
        }

        public static void SetSendBit(TcpDataDef.SendBitMap bitMap, bool value) {
            m_RackMaster_SendBitMap[(int)bitMap] = value;
        }

        public static object GetSendWord(TcpDataDef.SendWordMap wordMap) {
            return m_RackMaster_SendWordMap[(int)wordMap];
        }

        public void ReceiveAction(byte[] receivePackets) {
            sbyte dataType = ProtocolRoles.GetValue_DataType(receivePackets);
            short dataMapAddress = ProtocolRoles.GetValue_DataAddress(receivePackets);

            switch ((ProtocolRoles.DataType)dataType) {
                case ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Bit_Data:
                    Parsing_CIM_2_RM_Bit_Data(dataMapAddress, receivePackets);
                    break;

                case ProtocolRoles.DataType.STK_CIM_2_Controller_RM_Word_Data:
                    Parsing_CIM_2_RM_Word_Data(dataMapAddress, receivePackets);
                    break;

                case ProtocolRoles.DataType.ControlMSG:
                    Parsing_ControlMSG(receivePackets);
                    break;
            }
        }

        private void Parsing_ControlMSG(byte[] receivePackets) {
            byte[] receiveValues = ProtocolRoles.GetReceiveDataArray(receivePackets);

            m_sendCycleTime = BitConverter.ToInt16(receiveValues, 0);

            if(m_localThreadObj != null) {
                Thread threadPt = (Thread)m_localThreadObj;

                if (threadPt.IsAlive) {
                    threadPt.Join();
                    threadPt.Abort();
                }
                m_localThreadObj = null;
            }

            Thread localThread = new Thread(delegate () {
                while (isConnected) {
                    UpdateSendBitData();
                    UpdateSendWordData();
                    Send_RM_2_CIM_Bit_Data();
                    Send_RM_2_CIM_Word_Data();

                    Thread.Sleep(m_sendCycleTime);
                }
                ClearSendData();
                ClearReceiveData();
            });
            localThread.IsBackground = true;
            localThread.Start();

            m_localThreadObj = localThread;
        }

        /// <summary>
        /// CIM -> RM (BIT)
        /// </summary>
        /// <param name="dataMapAddress"></param>
        /// <param name="receivePackets"></param>
        private void Parsing_CIM_2_RM_Bit_Data(short dataMapAddress, byte[] receivePackets) {
            if (dataMapAddress >= m_RackMaster_RecvBitMap.Length)
                return;

            bool receiveValue = Convert.ToBoolean(receivePackets[ProtocolRoles.Recv_TCPHeaderLen]);
            bool currentValue = m_RackMaster_RecvBitMap[dataMapAddress];

            if (receiveValue != currentValue) {
                m_RackMaster_RecvBitMap[dataMapAddress] = receiveValue;
            }
            Action_CIM_2_RM_Bit_Data((TcpDataDef.ReceiveBitMap)dataMapAddress);
        }

        private void Parsing_CIM_2_RM_Word_Data(short dataMapAddress, byte[] receivePackets) {
            if (dataMapAddress >= m_RackMaster_RecvWordMap.Length)
                return;

            byte[] receive_values = ProtocolRoles.GetReceiveDataArray(receivePackets);
            byte[] current_values = ProtocolRoles.GetCurrentWordDataToByteArray(dataMapAddress, receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen, m_RackMaster_RecvWordMap);

            if (!ProtocolRoles.IsByteArrayCompare(receive_values, current_values)) {
                Buffer.BlockCopy(receive_values, 0, m_RackMaster_RecvWordMap, dataMapAddress * sizeof(short), receivePackets.Length - ProtocolRoles.Recv_TCPHeaderLen);
            }
            Action_CIM_2_RM_Word_Data((TcpDataDef.ReceiveWordMap)dataMapAddress);
        }

        private void ClearReceiveData() {
            for(int i = 0; i < m_RackMaster_RecvBitMap.Length; i++) {
                m_RackMaster_RecvBitMap[i] = false;
            }
            for(int i = 0; i < m_RackMaster_RecvWordMap.Length; i++) {
                m_RackMaster_RecvWordMap[i] = 0;
            }
        }

        private void Action_CIM_2_RM_Bit_Data(TcpDataDef.ReceiveBitMap receiveBitMap) {
            switch (receiveBitMap) {
                case TcpDataDef.ReceiveBitMap.MST_Emo_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Global.AUTO_STATE = false;
                        Global.MANUAL_STATE = true;
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Servo_On_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motor.AllServoOn();
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_Servo_Off_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motor.AllServoOff();
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_Auto_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Global.AUTO_STATE = true;
                        Global.MANUAL_STATE = false;
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_Auto_Stop_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Global.AUTO_STATE = false;
                        Global.MANUAL_STATE = true;
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_Error_Reset_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Alarm.ClearAlarm();
                        RackMasterMotion.SetIdle();
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.MST_Soft_Error_State:
                    if (GetReceiveBit(receiveBitMap)) {
                        Global.AUTO_STATE = false;
                        Global.MANUAL_STATE = true;
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Time_Sync:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                        RMParam.UpdateSyncTime();
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Auto_Teaching_Start_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                        RackMasterMotion.AutoTeachingStart();
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Auto_Teaching_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Auto_Teaching_Complete_Alarm_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_From_Start_Request:
                    if(Global.AUTO_STATE && !Global.MANUAL_STATE) {
                        if(RackMasterMotion.GetCurrentStep() == (int)RackMasterMotion.RM_STEP.Idle && GetReceiveBit(receiveBitMap)) {
                            Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                            RackMasterMotion.FromStart();
                        }
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_From_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_To_Start_Request:
                    if (Global.AUTO_STATE && !Global.MANUAL_STATE) {
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Idle && GetReceiveBit(receiveBitMap)) {
                            Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                            RackMasterMotion.ToStart();
                        }
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_To_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_Maint_Start_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                        RackMasterMotion.MaintMoveStart();
                    }

                    break;

                    // AutoMotion에서 처리
                case TcpDataDef.ReceiveBitMap.RM_Maint_Complete_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                // AutoMotion에서 처리
                case TcpDataDef.ReceiveBitMap.RM_Store_Alt_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                // AutoMotion에서 처리
                case TcpDataDef.ReceiveBitMap.RM_Source_Empty_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                // AutoMotion에서 처리
                case TcpDataDef.ReceiveBitMap.RM_Double_Storage_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                // AutoMotion에서 처리
                case TcpDataDef.ReceiveBitMap.RM_Resume_Request_ACK:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Teaching_Read_Write_Start:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                        if (m_teacingData != null) {
                            float xData = (float)m_teacingData.valX / 1000;
                            float zData = (float)m_teacingData.valZ / 1000;
                            float aData = (float)m_teacingData.valFork_A / 1000;
                            float tData = (float)m_teacingData.valT / 1000;

                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_X_Axis_Data_0, xData);
                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_Z_Axis_Data_0, zData);
                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_A_Axis_Data_0, aData);
                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_T_Axis_Data_0, tData);

                            m_teacingData = null;
                        }
                        else {
                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_X_Axis_Data_0, 0);
                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_Z_Axis_Data_0, 0);
                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_A_Axis_Data_0, 0);
                            UpdateTeachingData(TcpDataDef.SendWordMap.Teaching_T_Axis_Data_0, 0);
                        }
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_HP_Door_Open:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_OP_Door_Open:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_HP_EMO:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_OP_EMO:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_HP_Escape:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_OP_Escape:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_Key_Auto:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_X_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motor.ClearOverload_Alarm(RMParam.RMAxis.X_Axis);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Z_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motor.ClearOverload_Alarm(RMParam.RMAxis.Z_Axis);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_A_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motor.ClearOverload_Alarm(RMParam.RMAxis.A_Axis);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_T_Axis_Max_OverLoad_Reset:
                    if (GetReceiveBit(receiveBitMap)) {
                        m_motor.ClearOverload_Alarm(RMParam.RMAxis.T_Axis);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_X_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        RMParam.SetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.X_Axis, m_maxOverloadX);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_Z_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        RMParam.SetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.Z_Axis, m_maxOverloadZ);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }

                    break;

                case TcpDataDef.ReceiveBitMap.RM_A_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        RMParam.SetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.A_Axis, m_maxOverloadA);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.RM_T_Axis_Max_OverLoad_Apply:
                    if (GetReceiveBit(receiveBitMap)) {
                        RMParam.SetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.T_Axis, m_maxOverloadT);
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.PIO_Load_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.PIO_Unload_Request:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.PIO_Ready:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;

                case TcpDataDef.ReceiveBitMap.PIO_Port_Error:
                    if (GetReceiveBit(receiveBitMap)) {
                        Log.Add(new Log.LogItem(Log.LogType.CIM, $"{receiveBitMap}"));
                    }
                    break;
            }
        }

        private void Action_CIM_2_RM_Word_Data(TcpDataDef.ReceiveWordMap receiveWordMap) {
            if (GetSendBit(TcpDataDef.SendBitMap.From_State)) {
                CopyCassetteID(receiveWordMap);
            }else if (GetSendBit(TcpDataDef.SendBitMap.To_State)) {
                if (CompareCassetteID(receiveWordMap)) {
                    m_isCompareCSTID = true;
                }
            }

            switch (receiveWordMap) {
                case TcpDataDef.ReceiveWordMap.Year:
                case TcpDataDef.ReceiveWordMap.Month:
                case TcpDataDef.ReceiveWordMap.Day:
                case TcpDataDef.ReceiveWordMap.Hour:
                case TcpDataDef.ReceiveWordMap.Minute:
                case TcpDataDef.ReceiveWordMap.Second:
                    SetSyncTime(receiveWordMap);
                    break;

                case TcpDataDef.ReceiveWordMap.RM_Speed_X_Axis_Percent:
                case TcpDataDef.ReceiveWordMap.RM_Speed_Z_Axis_Percent:
                case TcpDataDef.ReceiveWordMap.RM_Speed_A_Axis_Percent:
                case TcpDataDef.ReceiveWordMap.RM_Speed_T_Axis_Percent:
                    SetAutoSpeedPercent(receiveWordMap);
                    break;

                case TcpDataDef.ReceiveWordMap.X_Axis_Max_OverLoad_Limit:
                case TcpDataDef.ReceiveWordMap.Z_Axis_Max_OverLoad_Limit:
                case TcpDataDef.ReceiveWordMap.A_Axis_Max_OverLoad_Limit:
                case TcpDataDef.ReceiveWordMap.T_Axis_Max_OverLoad_Limit:
                    SetOverload(receiveWordMap);
                    break;

                case TcpDataDef.ReceiveWordMap.Auto_Teaching_ID_0:
                case TcpDataDef.ReceiveWordMap.Auto_Teaching_X_Axis_Data:
                case TcpDataDef.ReceiveWordMap.Auto_Teaching_Z_Axis_Data:
                    SetAutoTeaching(receiveWordMap);
                    break;

                case TcpDataDef.ReceiveWordMap.RM_FROM_SHELF_ID_0:
                    byte[] fromShelfIdData = new byte[sizeof(int)];
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)receiveWordMap * sizeof(short)), fromShelfIdData, 0, sizeof(int));
                    RackMasterMotion.SetFromID((int)BitConverter.ToInt32(fromShelfIdData, 0));
                    break;

                case TcpDataDef.ReceiveWordMap.RM_TO_SHELF_ID_0:
                    byte[] toShelfIdData = new byte[sizeof(int)];
                    Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)receiveWordMap * sizeof(short)), toShelfIdData, 0, sizeof(int));
                    RackMasterMotion.SetToID((int)BitConverter.ToInt32(toShelfIdData, 0));
                    break;
            }
        }

        private void CopyCassetteID(TcpDataDef.ReceiveWordMap receiveWordMap)
        {
            if(receiveWordMap >= TcpDataDef.ReceiveWordMap.CST_ID_PIO_Word_0 &&
                receiveWordMap <= TcpDataDef.ReceiveWordMap.CST_ID_PIO_Word_63)
            {
                foreach (TcpDataDef.SendWordMap item in Enum.GetValues(typeof(TcpDataDef.SendWordMap)))
                {
                    if (item >= TcpDataDef.SendWordMap.CST_ID_PIO_Word_0 &&
                        item <= TcpDataDef.SendWordMap.CST_ID_PIO_Word_63)
                    {
                        m_RackMaster_SendWordMap[(int)item] = m_RackMaster_RecvWordMap[(int)item];
                    }
                }
            }
        }

        private bool CompareCassetteID(TcpDataDef.ReceiveWordMap receiveWordMap)
        {
            foreach (TcpDataDef.SendWordMap item in Enum.GetValues(typeof(TcpDataDef.SendWordMap)))
            {
                if (receiveWordMap >= TcpDataDef.ReceiveWordMap.CST_ID_PIO_Word_0 &&
                receiveWordMap <= TcpDataDef.ReceiveWordMap.CST_ID_PIO_Word_63)
                {
                    if (m_RackMaster_SendWordMap[(int)item] != m_RackMaster_RecvWordMap[(int)item])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool CompareCassetteID()
        {
            foreach (TcpDataDef.SendWordMap item in Enum.GetValues(typeof(TcpDataDef.SendWordMap)))
            {
                if (item >= TcpDataDef.SendWordMap.CST_ID_PIO_Word_0 &&
                item <= TcpDataDef.SendWordMap.CST_ID_PIO_Word_63)
                {
                    if (m_RackMaster_SendWordMap[(int)item] != m_RackMaster_RecvWordMap[(int)item])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void ClearCompareID()
        {
            m_isCompareCSTID = false;
        }

        public static bool GetCompareCasseteID()
        {
            return m_isCompareCSTID;
        }

        private void SetSyncTime(TcpDataDef.ReceiveWordMap receiveWordMap) {
            byte[] shortData = new byte[sizeof(short)];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)receiveWordMap * sizeof(short)), shortData, 0, sizeof(short));
            switch (receiveWordMap) {
                case TcpDataDef.ReceiveWordMap.Year:
                    RMParam.SetSyncTimeData(RMParam.SyncTimeType.Year, BitConverter.ToInt16(shortData, 0));
                    break;

                case TcpDataDef.ReceiveWordMap.Month:
                    RMParam.SetSyncTimeData(RMParam.SyncTimeType.Month, BitConverter.ToInt16(shortData, 0));
                    break;

                case TcpDataDef.ReceiveWordMap.Day:
                    RMParam.SetSyncTimeData(RMParam.SyncTimeType.Day, BitConverter.ToInt16(shortData, 0));
                    break;

                case TcpDataDef.ReceiveWordMap.Hour:
                    RMParam.SetSyncTimeData(RMParam.SyncTimeType.Hour, BitConverter.ToInt16(shortData, 0));
                    break;

                case TcpDataDef.ReceiveWordMap.Minute:
                    RMParam.SetSyncTimeData(RMParam.SyncTimeType.Minute, BitConverter.ToInt16(shortData, 0));
                    break;

                case TcpDataDef.ReceiveWordMap.Second:
                    RMParam.SetSyncTimeData(RMParam.SyncTimeType.Second, BitConverter.ToInt16(shortData, 0));
                    break;
            }
        }

        private void SetAutoSpeedPercent(TcpDataDef.ReceiveWordMap receiveWordMap) {
            byte[] shortData = new byte[sizeof(short)];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)receiveWordMap * sizeof(short)), shortData, 0, sizeof(short));
            short percentData = BitConverter.ToInt16(shortData, 0);

            switch (receiveWordMap) {
                case TcpDataDef.ReceiveWordMap.RM_Speed_X_Axis_Percent:
                    RMParam.SetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.X_Axis, percentData / 100.0);
                    break;

                case TcpDataDef.ReceiveWordMap.RM_Speed_Z_Axis_Percent:
                    RMParam.SetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.Z_Axis, percentData / 100.0);
                    break;

                case TcpDataDef.ReceiveWordMap.RM_Speed_A_Axis_Percent:
                    RMParam.SetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.A_Axis, percentData / 100.0);
                    break;

                case TcpDataDef.ReceiveWordMap.RM_Speed_T_Axis_Percent:
                    RMParam.SetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.T_Axis, percentData / 100.0);
                    break;
            }
        }

        private void SetOverload(TcpDataDef.ReceiveWordMap receiveWordMap) {
            byte[] shortData = new byte[sizeof(short)];
            Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)receiveWordMap * sizeof(short)), shortData, 0, sizeof(short));

            switch (receiveWordMap) {
                case TcpDataDef.ReceiveWordMap.X_Axis_Max_OverLoad_Limit:
                    m_maxOverloadX = BitConverter.ToInt16(shortData, 0);
                    break;

                case TcpDataDef.ReceiveWordMap.Z_Axis_Max_OverLoad_Limit:
                    m_maxOverloadZ = BitConverter.ToInt16(shortData, 0);
                    break;

                case TcpDataDef.ReceiveWordMap.A_Axis_Max_OverLoad_Limit:
                    m_maxOverloadA = BitConverter.ToInt16(shortData, 0);
                    break;

                case TcpDataDef.ReceiveWordMap.T_Axis_Max_OverLoad_Limit:
                    m_maxOverloadT = BitConverter.ToInt16(shortData, 0);
                    break;
            }
        }

        private void SetAutoTeaching(TcpDataDef.ReceiveWordMap receiveWordMap) {
            if(receiveWordMap == TcpDataDef.ReceiveWordMap.Auto_Teaching_ID_0) {
                byte[] intData = new byte[sizeof(int)];
                Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)receiveWordMap * sizeof(short)), intData, 0, sizeof(int));
                RackMasterMotion.SetAutoTeacingTargetID((int)BitConverter.ToInt32(intData, 0));
            }
            else {
                byte[] shortData = new byte[sizeof(short)];
                Buffer.BlockCopy(m_RackMaster_RecvWordMap, ((int)receiveWordMap * sizeof(short)), shortData, 0, sizeof(short));
                if(receiveWordMap == TcpDataDef.ReceiveWordMap.Auto_Teaching_X_Axis_Data) {
                    RackMasterMotion.SetAutoTeachingTargetX((double)BitConverter.ToInt16(shortData, 0));
                }else if(receiveWordMap == TcpDataDef.ReceiveWordMap.Auto_Teaching_Z_Axis_Data) {
                    RackMasterMotion.SetAutoTeachingTargetZ((double)BitConverter.ToInt16(shortData, 0));
                }
            }
        }

        private void UpdateSendBitData() {
            foreach (TcpDataDef.SendBitMap item in Enum.GetValues(typeof(TcpDataDef.SendBitMap))) {
                switch (item) {
                    case TcpDataDef.SendBitMap.Servo_On_Ready:
                        // RM MC on도 보나?
                        if (!m_motor.IsAllServoOn()) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Servo_On_State:
                        if (m_motor.IsAllServoOn()) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Servo_Off_Ready:
                        if (!m_motor.IsAllServoOn()) {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Servo_Off_State:
                        if (m_motor.IsAllServoOn()) {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Auto_Ready:
                        // Key가 Auto일 때인데 RM에 Key가 있나?
                        if (!m_motor.IsAllServoOn() || RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Error ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.EMO) {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        } else if (Global.AUTO_STATE && !Global.MANUAL_STATE) {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Auto_State:
                        if (Global.AUTO_STATE) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Manual_Ready:
                        if (Global.AUTO_STATE && RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Idle) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Manual_State:
                        if (Global.MANUAL_STATE) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Error_State:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Error || Global.ERROR_STATE) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Home_State:
                        // 전 축 홈 상태인건가?
                        break;

                    case TcpDataDef.SendBitMap.Fan1_On_State:
                        break;

                    case TcpDataDef.SendBitMap.Fan2_On_State:
                        break;

                    case TcpDataDef.SendBitMap.From_Start_ACK:
                        if(m_RackMaster_RecvBitMap[(int)TcpDataDef.ReceiveBitMap.RM_From_Start_Request] &&
                            RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Idle) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.From_Complete:
                        if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.To_Start_ACK:
                        if(m_RackMaster_RecvBitMap[(int)TcpDataDef.ReceiveBitMap.RM_To_Start_Request] &&
                            RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Idle) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.To_Complete:
                        if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Maint_Move_Start_ACK:
                        if(m_RackMaster_RecvBitMap[(int)TcpDataDef.ReceiveBitMap.RM_Maint_Start_Request] &&
                            (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Maint_Move ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Maint_Move_Check)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Maint_Move_Complete:
                        break;

                    case TcpDataDef.SendBitMap.Store_Alt_Request:
                        break;

                    case TcpDataDef.SendBitMap.Source_Empty_Request:
                        break;

                    case TcpDataDef.SendBitMap.Double_Storage_Request:
                        break;

                    case TcpDataDef.SendBitMap.Resume_Request_Request:
                        break;

                    case TcpDataDef.SendBitMap.Teaching_RW_Ready:
                        if(!Global.AUTO_STATE && Global.MANUAL_STATE) {
                            if(RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Error && 
                                !m_RackMaster_RecvBitMap[(int)TcpDataDef.ReceiveBitMap.RM_Teaching_Read_Write_Start]) {
                                m_RackMaster_SendBitMap[(int)item] = true;
                            }
                            else {
                                m_RackMaster_SendBitMap[(int)item] = false;
                            }
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Teaching_Read_Complete:
                        if(m_teacingData == null) {
                            if(!m_RackMaster_SendBitMap[(int)TcpDataDef.SendBitMap.Teaching_RW_Ready] && 
                                m_RackMaster_RecvBitMap[(int)TcpDataDef.ReceiveBitMap.RM_Teaching_Read_Write_Start]) {
                                m_RackMaster_SendBitMap[(int)item] = true;
                            }
                            else {
                                m_RackMaster_SendBitMap[(int)item] = false;
                            }
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Accessing_RM_Down_From:
                        if(RackMasterMotion.GetCurrentStep() > RackMasterMotion.RM_STEP.From_ID_Check &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Accessing_RM_Dwon_To:
                        if(RackMasterMotion.GetCurrentStep() > RackMasterMotion.RM_STEP.To_ID_Check &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.From_Ready:
                        if (RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Idle) {
                            if(!Global.CST_ON)
                                m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.To_Ready:
                        if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Idle) {
                            if (Global.CST_ON) {
                                m_RackMaster_SendBitMap[(int)item] = true;
                            }
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Idle_State:
                        if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Idle) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Active_State:
                        if(RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Idle &&
                            RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.Error &&
                            RackMasterMotion.GetCurrentStep() != RackMasterMotion.RM_STEP.EMO) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.XZT_Going_State:
                        if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_XZT_From_Move ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.From_XZT_From_Complete ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_XZT_To_Move ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.To_XZT_To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Maint_Move_State:
                        if(RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Maint_Move ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Maint_Move_Check ||
                            RackMasterMotion.GetCurrentStep() == RackMasterMotion.RM_STEP.Maint_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.From_State:
                        if (RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_ID_Check &&
                            RackMasterMotion.GetCurrentStep() < RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.To_State:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.To_ID_Check &&
                            RackMasterMotion.GetCurrentStep() < RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_FWD_State:
                        if(m_motor.GetServoStatus(Motor.ServoStatusType.vel_cmd, RMParam.RMAxis.X_Axis) > 0) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.X_Axis_BWD_State:
                        if (m_motor.GetServoStatus(Motor.ServoStatusType.vel_cmd, RMParam.RMAxis.X_Axis) < 0) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_Max_Speed_State:
                        if(m_motor.GetServoStatus(Motor.ServoStatusType.vel_cmd, RMParam.RMAxis.X_Axis) >= RMParam.GetServoParam(RMParam.ServoParam.Max_Speed, RMParam.RMAxis.X_Axis)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Arm_Home_Position_State:
                        if (m_motor.GetSensorStatus(Motor.SensorType.Home, RMParam.RMAxis.A_Axis)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Turn_Left_Position_State:
                        if(m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, RMParam.RMAxis.T_Axis) > RMParam.GetParam(RMParam.Param.Turn_Left_Right_Degree)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Turn_Right_Position_State:
                        if (m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, RMParam.RMAxis.T_Axis) < RMParam.GetParam(RMParam.Param.Turn_Left_Right_Degree)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.CST_Empty:
                        m_RackMaster_SendBitMap[(int)item] = !Global.CST_ON;
                        break;

                    case TcpDataDef.SendBitMap.CST_ON:
                        m_RackMaster_SendBitMap[(int)item] = Global.CST_ON;
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_Power_On:
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Servo_On, RMParam.RMAxis.X_Axis);
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.HomeDone, RMParam.RMAxis.X_Axis);
                        break;

                    case TcpDataDef.SendBitMap.Z_Axis_Power_On:
                        break;

                    case TcpDataDef.SendBitMap.Z_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Servo_On, RMParam.RMAxis.Z_Axis);
                        break;

                    case TcpDataDef.SendBitMap.Z_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.HomeDone, RMParam.RMAxis.Z_Axis);
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_Power_On:
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Servo_On, RMParam.RMAxis.A_Axis);
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.HomeDone, RMParam.RMAxis.A_Axis);
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_Power_On:
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_Servo_On:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Servo_On, RMParam.RMAxis.T_Axis);
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_HomeDone:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.HomeDone, RMParam.RMAxis.T_Axis);
                        break;

                    case TcpDataDef.SendBitMap.EMO_On:
                        if(Io.GetInputBit((int)IoList.InputList.EMO_HP) ||
                            Io.GetInputBit((int)IoList.InputList.EMO_OP)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.GOT_EMO_On:
                        m_RackMaster_SendBitMap[(int)item] = Io.GetInputBit((int)IoList.InputList.Touch_EMO_SW);
                        break;

                    case TcpDataDef.SendBitMap.Double_Storage_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Double_Storage_Sensor)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Arm_CST_All_Undetected:
                        if (!Global.CST_ON) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Arm_CST_1_Detected_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Placement_Sensor_1)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Arm_CST_2_Detected_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Placement_Sensor_2)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Arm_CST_Diagonal_Detected_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Presence_Sensor)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }
                        break;

                    case TcpDataDef.SendBitMap.Left_From_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Pick_Sensor)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Left_To_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Place_Sensor)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Right_From_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Pick_Sensor)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Right_To_Sensor:
                        if (Io.GetInputBit((int)IoList.InputList.Fork_Place_Sensor)) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Left_1_Projecting_Sensor_Front:
                        break;

                    case TcpDataDef.SendBitMap.Left_2_Projecting_Sensor_Rear:
                        break;

                    case TcpDataDef.SendBitMap.Right_1_Projecting_Sensor_Front:
                        break;

                    case TcpDataDef.SendBitMap.Right_2_Projecting_Sensor_Rear:
                        break;

                    case TcpDataDef.SendBitMap.Z_Axis_HP_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Home, RMParam.RMAxis.Z_Axis);

                        break;

                    case TcpDataDef.SendBitMap.Z_Axis_HP_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Negative_Limit, RMParam.RMAxis.Z_Axis);

                        break;

                    case TcpDataDef.SendBitMap.Z_Axis_HP_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Positive_Limit, RMParam.RMAxis.Z_Axis);

                        break;

                    case TcpDataDef.SendBitMap.HP_EMO_Push:
                        m_RackMaster_SendBitMap[(int)item] = Io.GetInputBit((int)IoList.InputList.EMO_HP);
                        break;

                    case TcpDataDef.SendBitMap.OP_EMO_Push:
                        m_RackMaster_SendBitMap[(int)item] = Io.GetInputBit((int)IoList.InputList.EMO_OP);
                        break;

                    case TcpDataDef.SendBitMap.GOT_Detect:
                        m_RackMaster_SendBitMap[(int)item] = Io.GetInputBit((int)IoList.InputList.Touch_Connection_Check);

                        break;

                    case TcpDataDef.SendBitMap.Position_Sensor_1:
                        break;

                    case TcpDataDef.SendBitMap.Position_Sensor_2:
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_HP_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Home, RMParam.RMAxis.X_Axis);
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_HP_Slow_Sensor:
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_HP_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Negative_Limit, RMParam.RMAxis.X_Axis);
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_OP_Slow_Sensor:
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_OP_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Positive_Limit, RMParam.RMAxis.X_Axis);
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Home, RMParam.RMAxis.A_Axis);
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Negative_Limit, RMParam.RMAxis.A_Axis);
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Positive_Limit, RMParam.RMAxis.A_Axis);
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_POS_1_Sensor:
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_POS_2_Sensor:
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_POS_3_Sensor:
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_Home_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Home, RMParam.RMAxis.T_Axis);
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_NOT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Negative_Limit, RMParam.RMAxis.T_Axis);
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_POT_Sensor:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetSensorStatus(Motor.SensorType.Positive_Limit, RMParam.RMAxis.T_Axis);
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_POS_Sensor:
                        break;

                    case TcpDataDef.SendBitMap.CPS_Second_Run:
                        break;

                    case TcpDataDef.SendBitMap.CPS_Second_Fault:
                        break;

                    case TcpDataDef.SendBitMap.From_Step_0_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_XZT_From_Move &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.From_Step_1_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_Shelf_Port_Check &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.From_Step_2_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_Z_Up &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.From_Step_3_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_CST_Check_Sensor &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.From_Step_4_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_Port_Ready_Off_Check &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.To_Step_0_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.To_XZT_To_Move &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.To_Step_1_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.To_Double_Storage_Check &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.To_Step_2_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.To_Z_Down &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.To_Step_3_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.To_CST_Fork_Placement_Check &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.To_Step_4_Complete:
                        if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.To_Port_Ready_Off_Check &&
                            RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.To_Complete) {
                            m_RackMaster_SendBitMap[(int)item] = true;
                        }
                        else {
                            m_RackMaster_SendBitMap[(int)item] = false;
                        }

                        break;

                    case TcpDataDef.SendBitMap.Communication_Check:
                        break;

                    case TcpDataDef.SendBitMap.X_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Run, RMParam.RMAxis.X_Axis);
                        break;

                    case TcpDataDef.SendBitMap.Z_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Run, RMParam.RMAxis.Z_Axis);
                        break;

                    case TcpDataDef.SendBitMap.A_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Run, RMParam.RMAxis.A_Axis);
                        break;

                    case TcpDataDef.SendBitMap.T_Axis_Busy:
                        m_RackMaster_SendBitMap[(int)item] = m_motor.GetServoFlag(Motor.ServoFlagType.Run, RMParam.RMAxis.T_Axis);
                        break;
                }
            }
        }

        public void UpdateSendWordData() {
            float curPosX = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, RMParam.RMAxis.X_Axis) / 1000);
            float curPosZ = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, RMParam.RMAxis.Z_Axis) / 1000);
            float curPosA = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, RMParam.RMAxis.A_Axis) / 1000);
            float curPosT = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.pos_act, RMParam.RMAxis.T_Axis) / 1000);

            float curTargetX = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.Profile_Traget_Pisition, RMParam.RMAxis.X_Axis) / 1000);
            float curTargetZ = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.Profile_Traget_Pisition, RMParam.RMAxis.Z_Axis) / 1000);
            float curTargetA = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.Profile_Traget_Pisition, RMParam.RMAxis.A_Axis) / 1000);
            float curTargetT = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.Profile_Traget_Pisition, RMParam.RMAxis.T_Axis) / 1000);

            float curVelX = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.vel_act, RMParam.RMAxis.X_Axis) / 1000000 * 60);
            float curVelZ = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.vel_act, RMParam.RMAxis.Z_Axis) / 1000000 * 60);
            float curVelA = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.vel_act, RMParam.RMAxis.A_Axis) / 1000 * 60);
            float curVelT = (float)(m_motor.GetServoStatus(Motor.ServoStatusType.vel_act, RMParam.RMAxis.T_Axis) / 1000 * 60);

            short torqueX = (short)(m_motor.GetServoStatus(Motor.ServoStatusType.trq_act, RMParam.RMAxis.X_Axis));
            short torqueZ = (short)(m_motor.GetServoStatus(Motor.ServoStatusType.trq_act, RMParam.RMAxis.Z_Axis));
            short torqueA = (short)(m_motor.GetServoStatus(Motor.ServoStatusType.trq_act, RMParam.RMAxis.A_Axis));
            short torqueT = (short)(m_motor.GetServoStatus(Motor.ServoStatusType.trq_act, RMParam.RMAxis.T_Axis));

            short overloadSettingX = (short)RMParam.GetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.X_Axis);
            short overloadSettingZ = (short)RMParam.GetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.Z_Axis);
            short overloadSettingA = (short)RMParam.GetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.A_Axis);
            short overloadSettingT = (short)RMParam.GetServoParam(RMParam.ServoParam.Max_Overload, RMParam.RMAxis.T_Axis);

            short overloadAlarmX = (short)m_motor.GetOverload_Alarm(RMParam.RMAxis.X_Axis);
            short overloadAlarmZ = (short)m_motor.GetOverload_Alarm(RMParam.RMAxis.Z_Axis);
            short overloadAlarmA = (short)m_motor.GetOverload_Alarm(RMParam.RMAxis.A_Axis);
            short overloadAlarmT = (short)m_motor.GetOverload_Alarm(RMParam.RMAxis.T_Axis);

            short autoSpeedPercentX = (short)(RMParam.GetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.X_Axis) * 100);
            short autoSpeedPercentZ = (short)(RMParam.GetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.Z_Axis) * 100);
            short autoSpeedPercentA = (short)(RMParam.GetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.A_Axis) * 100);
            short autoSpeedPercentT = (short)(RMParam.GetServoParam(RMParam.ServoParam.Auto_Speed_Percent, RMParam.RMAxis.T_Axis) * 100);

            int autoTeachingId = RackMasterMotion.GetAutoTeachingTargetID();
            short autoTeachingTargetX = (short)RackMasterMotion.GetAutoTeachingTargetX();
            short autoTeachingTargetZ = (short)RackMasterMotion.GetAutoTeachingTargetZ();

            UpdateServoStatus(TcpDataDef.SendWordMap.X_Axis_Cur_Position_0, curPosX);
            UpdateServoStatus(TcpDataDef.SendWordMap.Z_Axis_Cur_Position_0, curPosZ);
            UpdateServoStatus(TcpDataDef.SendWordMap.A_Axis_Cur_Position_0, curPosA);
            UpdateServoStatus(TcpDataDef.SendWordMap.T_Axis_Cur_Position_0, curPosT);

            UpdateServoStatus(TcpDataDef.SendWordMap.X_Axis_Target_Position_0, curTargetX);
            UpdateServoStatus(TcpDataDef.SendWordMap.Z_Axis_Target_Position_0, curTargetZ);
            UpdateServoStatus(TcpDataDef.SendWordMap.A_Axis_Target_Position_0, curTargetA);
            UpdateServoStatus(TcpDataDef.SendWordMap.T_Axis_Target_Position_0, curTargetT);

            UpdateServoStatus(TcpDataDef.SendWordMap.X_Axis_Cur_Velocity_0, curVelX);
            UpdateServoStatus(TcpDataDef.SendWordMap.Z_Axis_Cur_Velocity_0, curVelZ);
            UpdateServoStatus(TcpDataDef.SendWordMap.A_Axis_Cur_Velocity_0, curVelA);
            UpdateServoStatus(TcpDataDef.SendWordMap.T_Axis_Cur_Velocity_0, curVelT);

            UpdateServoStatus(TcpDataDef.SendWordMap.X_Axis_Cur_Torque, torqueX);
            UpdateServoStatus(TcpDataDef.SendWordMap.Z_Axis_Cur_Torque, torqueZ);
            UpdateServoStatus(TcpDataDef.SendWordMap.A_Axis_Cur_Torque, torqueA);
            UpdateServoStatus(TcpDataDef.SendWordMap.T_Axis_Cur_Torque, torqueT);

            UpdateServoStatus(TcpDataDef.SendWordMap.X_Axis_Setting_Max_OverLoad, overloadSettingX);
            UpdateServoStatus(TcpDataDef.SendWordMap.Z_Axis_Setting_Max_OverLoad, overloadSettingZ);
            UpdateServoStatus(TcpDataDef.SendWordMap.A_Axis_Setting_Max_OverLoad, overloadSettingA);
            UpdateServoStatus(TcpDataDef.SendWordMap.T_Axis_Setting_Max_OverLoad, overloadSettingT);

            UpdateServoStatus(TcpDataDef.SendWordMap.X_Axis_Cur_Max_OverLoad, overloadAlarmX);
            UpdateServoStatus(TcpDataDef.SendWordMap.Z_Axis_Cur_Max_OverLoad, overloadAlarmZ);
            UpdateServoStatus(TcpDataDef.SendWordMap.A_Axis_Cur_Max_OverLoad, overloadAlarmA);
            UpdateServoStatus(TcpDataDef.SendWordMap.T_Axis_Cur_Max_OverLoad, overloadAlarmT);

            UpdateAutoSpeedPecent(TcpDataDef.SendWordMap.Auto_Speed_Percent_X, autoSpeedPercentX);
            UpdateAutoSpeedPecent(TcpDataDef.SendWordMap.Auto_Speed_Percent_Z, autoSpeedPercentZ);
            UpdateAutoSpeedPecent(TcpDataDef.SendWordMap.Auto_Speed_Percent_A, autoSpeedPercentA);
            UpdateAutoSpeedPecent(TcpDataDef.SendWordMap.Auto_Speed_Percent_T, autoSpeedPercentT);

            UpdateAutoTeachingData(TcpDataDef.SendWordMap.Auto_Teaching_Id_0, autoTeachingId);
            UpdateAutoTeachingData(TcpDataDef.SendWordMap.Auto_Teaching_Target_X, autoTeachingTargetX);
            UpdateAutoTeachingData(TcpDataDef.SendWordMap.Auto_Teaching_Target_Z, autoTeachingTargetZ);

            int fromId = RackMasterMotion.GetFromID();
            int toID = RackMasterMotion.GetToID();

            UpdateShelfID(TcpDataDef.SendWordMap.FROM_SHELF_ID_0, fromId);
            UpdateShelfID(TcpDataDef.SendWordMap.TO_SHELF_ID_0, toID);

            if(Global.ERROR_STATE)
                UpdateErrorCode();

            UpdateAccessShelfID();
        }

        private void UpdateServoStatus(TcpDataDef.SendWordMap wordMapIndex, float value) {
            switch (wordMapIndex) {
                case TcpDataDef.SendWordMap.X_Axis_Cur_Position_0:
                case TcpDataDef.SendWordMap.Z_Axis_Cur_Position_0:
                case TcpDataDef.SendWordMap.A_Axis_Cur_Position_0:
                case TcpDataDef.SendWordMap.T_Axis_Cur_Position_0:
                case TcpDataDef.SendWordMap.X_Axis_Target_Position_0:
                case TcpDataDef.SendWordMap.Z_Axis_Target_Position_0:
                case TcpDataDef.SendWordMap.A_Axis_Target_Position_0:
                case TcpDataDef.SendWordMap.T_Axis_Target_Position_0:
                case TcpDataDef.SendWordMap.X_Axis_Cur_Velocity_0:
                case TcpDataDef.SendWordMap.Z_Axis_Cur_Velocity_0:
                case TcpDataDef.SendWordMap.A_Axis_Cur_Velocity_0:
                case TcpDataDef.SendWordMap.T_Axis_Cur_Velocity_0:
                    byte[] floatData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(floatData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(float));
                    break;
            }
        }
        private void UpdateServoStatus(TcpDataDef.SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case TcpDataDef.SendWordMap.X_Axis_Cur_Torque:
                case TcpDataDef.SendWordMap.Z_Axis_Cur_Torque:
                case TcpDataDef.SendWordMap.A_Axis_Cur_Torque:
                case TcpDataDef.SendWordMap.T_Axis_Cur_Torque:
                case TcpDataDef.SendWordMap.X_Axis_Avr_Torque:
                case TcpDataDef.SendWordMap.Z_Axis_Avr_Torque:
                case TcpDataDef.SendWordMap.A_Axis_Avr_Torque:
                case TcpDataDef.SendWordMap.T_Axis_Avr_Torque:
                case TcpDataDef.SendWordMap.X_Axis_Peak_Torque:
                case TcpDataDef.SendWordMap.Z_Axis_Peak_Torque:
                case TcpDataDef.SendWordMap.A_Axis_Peak_Torque:
                case TcpDataDef.SendWordMap.T_Axis_Peak_Torque:
                case TcpDataDef.SendWordMap.X_Axis_Setting_Max_OverLoad:
                case TcpDataDef.SendWordMap.Z_Axis_Setting_Max_OverLoad:
                case TcpDataDef.SendWordMap.A_Axis_Setting_Max_OverLoad:
                case TcpDataDef.SendWordMap.T_Axis_Setting_Max_OverLoad:
                case TcpDataDef.SendWordMap.X_Axis_Cur_Max_OverLoad:
                case TcpDataDef.SendWordMap.Z_Axis_Cur_Max_OverLoad:
                case TcpDataDef.SendWordMap.A_Axis_Cur_Max_OverLoad:
                case TcpDataDef.SendWordMap.T_Axis_Cur_Max_OverLoad:
                    byte[] shortData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(shortData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    break;
            }
        }

        public static void SetCassetteID()
        {
            m_RackMaster_SendWordMap[0] = 0x3130;
        }

        public static void ClearCassetteID() {
            foreach (TcpDataDef.SendWordMap item in Enum.GetValues(typeof(TcpDataDef.SendWordMap))) {
                if (item >= TcpDataDef.SendWordMap.CST_ID_PIO_Word_0 &&
                    item <= TcpDataDef.SendWordMap.CST_ID_PIO_Word_63) {
                    m_RackMaster_SendWordMap[(int)item] = 0;
                }
            }
        }

        private void UpdateShelfID(TcpDataDef.SendWordMap wordMapIndex, int value) {
            switch (wordMapIndex) {
                case TcpDataDef.SendWordMap.FROM_SHELF_ID_0:
                    byte[] fromIdData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(fromIdData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(int));
                    break;

                case TcpDataDef.SendWordMap.TO_SHELF_ID_0:
                    byte[] toIdData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(toIdData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(int));
                    break;
            }
        }

        private void UpdateTeachingData(TcpDataDef.SendWordMap wordMapIndex, float value) {
            switch (wordMapIndex) {
                case TcpDataDef.SendWordMap.Teaching_X_Axis_Data_0:
                case TcpDataDef.SendWordMap.Teaching_Z_Axis_Data_0:
                case TcpDataDef.SendWordMap.Teaching_A_Axis_Data_0:
                case TcpDataDef.SendWordMap.Teaching_T_Axis_Data_0:
                    byte[] floatData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(floatData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(float));
                    break;

            }
        }

        private void UpdateAutoSpeedPecent(TcpDataDef.SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case TcpDataDef.SendWordMap.Auto_Speed_Percent_X:
                case TcpDataDef.SendWordMap.Auto_Speed_Percent_Z:
                case TcpDataDef.SendWordMap.Auto_Speed_Percent_A:
                case TcpDataDef.SendWordMap.Auto_Speed_Percent_T:
                    byte[] shortData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(shortData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    break;
            }
        }

        private void UpdateAutoTeachingData(TcpDataDef.SendWordMap wordMapIndex, int value) {
            switch (wordMapIndex) {
                case TcpDataDef.SendWordMap.Auto_Teaching_Id_0:
                    byte[] intData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(intData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(int));
                    break;
            }
        }

        private void UpdateAutoTeachingData(TcpDataDef.SendWordMap wordMapIndex, short value) {
            switch (wordMapIndex) {
                case TcpDataDef.SendWordMap.Auto_Teaching_Target_X:
                case TcpDataDef.SendWordMap.Auto_Teaching_Target_Z:
                    byte[] shortData = BitConverter.GetBytes(value);
                    Buffer.BlockCopy(shortData, 0, m_RackMaster_SendWordMap, (int)wordMapIndex * sizeof(short), sizeof(short));
                    break;
            }
        }

        private void UpdateErrorCode() {
            if (Alarm.GetCurrentAlarmCount() <= 0)
                return;

            byte[] curErrorWord = new byte[sizeof(short)];
            Buffer.BlockCopy(m_RackMaster_SendWordMap, ((int)TcpDataDef.SendWordMap.Error_Word_0 * sizeof(short)), curErrorWord, 0, sizeof(short));

            if (Alarm.GetAlarmCode(0) == BitConverter.ToInt16(curErrorWord, 0))
                return;

            for(int i = 0; i < Alarm.GetCurrentAlarmCount(); i++) {
                int wordIndex = (int)TcpDataDef.SendWordMap.Error_Word_11;
                while (true) {
                    if (wordIndex == (int)TcpDataDef.SendWordMap.Error_Word_0) {
                        byte[] errorData = BitConverter.GetBytes(Alarm.GetAlarmCode(i));
                        Buffer.BlockCopy(errorData, 0, m_RackMaster_SendWordMap, (int)TcpDataDef.SendWordMap.Error_Word_0 * sizeof(short), sizeof(short));
                        break;
                    }

                    m_RackMaster_SendWordMap[wordIndex + 1] = m_RackMaster_SendWordMap[wordIndex];
                    wordIndex--;
                }
            }
        }

        private void UpdateAccessShelfID() {
            if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.From_ID_Check &&
                RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.From_Complete) {
                byte[] intData = BitConverter.GetBytes(RackMasterMotion.GetFromID());
                Buffer.BlockCopy(intData, 0, m_RackMaster_SendWordMap, (int)TcpDataDef.SendWordMap.Access_Shelf_ID_0 * sizeof(short), sizeof(int));
            }
            else if(RackMasterMotion.GetCurrentStep() >= RackMasterMotion.RM_STEP.To_ID_Check &&
                RackMasterMotion.GetCurrentStep() <= RackMasterMotion.RM_STEP.To_Complete) {
                byte[] intData = BitConverter.GetBytes(RackMasterMotion.GetToID());
                Buffer.BlockCopy(intData, 0, m_RackMaster_SendWordMap, (int)TcpDataDef.SendWordMap.Access_Shelf_ID_0 * sizeof(short), sizeof(int));
            }
        }

        private void ClearSendData() {
            for(int i = 0; i < m_RackMaster_SendBitMap.Length; i++) {
                m_RackMaster_SendBitMap[i] = false;
            }
            for(int i = 0; i < m_RackMaster_SendWordMap.Length; i++) {
                m_RackMaster_SendWordMap[i] = 0;
            }
        }

        private void Send_RM_2_CIM_Bit_Data() {
            try {
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + RM_2_CIM_BIT_SIZE);
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Bit_Data };
                byte[] DataOffset = { 0, 0 };
                byte[] Data = new byte[RM_2_CIM_BIT_SIZE];
                Buffer.BlockCopy(m_RackMaster_SendBitMap, 0, Data, 0, RM_2_CIM_BIT_SIZE);

                if (ProtocolRoles.Send_LittleEndian) {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch {

            }
        }

        public void Send_RM_2_CIM_Word_Data() {
            try {
                byte[] DataLength = BitConverter.GetBytes(ProtocolRoles.Send_DataTypeLen + ProtocolRoles.Send_DataOffsetLen + (RM_2_CIM_WORD_SIZE * sizeof(short)));
                byte[] DataType = new byte[1] { (byte)ProtocolRoles.DataType.Controller_2_STK_CIM_RM_Word_Data };
                byte[] DataOffset = { 0, 0 };
                byte[] Data = new byte[RM_2_CIM_WORD_SIZE * sizeof(short)];
                Buffer.BlockCopy(m_RackMaster_SendWordMap, 0, Data, 0, RM_2_CIM_WORD_SIZE * sizeof(short));

                if (ProtocolRoles.Send_LittleEndian) {
                    Array.Reverse(DataLength);
                    Array.Reverse(DataOffset);

                    for (int nCount = 0; nCount < Data.Length / sizeof(short); nCount++) {
                        Array.Reverse(Data, nCount * sizeof(short), sizeof(short));
                    }
                }

                IEnumerable<byte> send = DataLength.Concat(DataType).Concat(DataOffset).Concat(Data);
                SendData(send.ToArray());
            }
            catch {

            }
        }
    }
}
