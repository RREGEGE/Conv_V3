using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Equipment.CPS
{
    /// <summary>
    /// CPSAction.cs는 CPS Data 구조 및 통신 관련 행동 작성
    /// </summary>
    public partial class CPS
    {
       /// <summary>
       /// Packet 구조 정의
       /// 각 Name은 Packet의 시작 주소
       /// 총 패킷은 98 byte(문서 기준)
       /// </summary>
        public enum PacketStruct
        {
            STX = 0,                        //byte
            ID = 1,                         //byte
            Status = 3,                     //2 byte
            Voltage = 6,                    //4byte, int16(Master->CIM Memory Map WordMap)
            DC_Current = 14,                //4byte, int16(Master->CIM Memory Map WordMap)
            IGBT_Current = 22,              //4byte, int16(Master->CIM Memory Map WordMap)
            Track_Current = 26,             //4byte, int16(Master->CIM Memory Map WordMap)
            OutputFreq = 30,                //4byte, float
            Converter_Heatsink_Temp = 34,   //4byte, float
            Covnerter_Inner_Temp = 38,      //4byte, float
            Converter_Error_Code = 42,      //4byte, int16(Master->CIM Memory Map WordMap)

            CheckSum = 96,                  //byte
            ETX = 97                        //byte
        }

        /// <summary>
        /// Packet 수신 후 파싱 작업 진행시 동작
        /// byte Array 그대로 카피 진행
        /// </summary>
        /// <param name="receivePackets"></param>
        void ReceiveAcition(byte[] receivePackets)
        {
            Buffer.BlockCopy(receivePackets, 0, Master.m_CPSByteMap, 0, receivePackets.Length);
        }

        /// <summary>
        /// CPS Data Get 함수(byte, string, int, float 등 타입에 맞게 리턴)
        /// </summary>
        /// <param name="ePacketStruct"></param>
        /// <returns></returns>
        public object Get_CPS_Data(PacketStruct ePacketStruct)
        {
            try
            {
                if ((int)ePacketStruct >= Master.m_CPSByteMap.Length || (int)ePacketStruct < 0)
                {
                    LogMsg.AddCPSLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                        $"[CPS->Master] ByteMap Index: {ePacketStruct} >= Length: {Master.m_CPSByteMap.Length}");
                    return (short)-1;
                }
                

                switch (ePacketStruct)
                {
                    case PacketStruct.STX:
                    case PacketStruct.CheckSum:
                    case PacketStruct.ETX:
                        {
                            int ByteSize = 1;
                            byte[] val = new byte[ByteSize];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, val, 0, val.Length);
                            return (byte)val[0];
                        }
                    case PacketStruct.ID:
                        {
                            int ByteSize = 2;
                            byte[] ID = new byte[ByteSize];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, ID, 0, ID.Length);
                            return ((string)Encoding.Default.GetString(ID).Trim('\0')).Replace(" ", string.Empty);
                        }
                    case PacketStruct.Status:
                        {
                            byte[] Status = new byte[3];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, Status, 0, Status.Length);
                            string strStatus = ((string)Encoding.Default.GetString(Status).Trim('\0')).Replace(" ", string.Empty);

                            return strStatus;
                        }
                    //Int
                    case PacketStruct.Voltage:
                    case PacketStruct.DC_Current:
                    case PacketStruct.IGBT_Current:
                    case PacketStruct.Track_Current:
                    case PacketStruct.Converter_Error_Code:
                        {
                            int ByteSize = 4;
                            byte[] StrData = new byte[ByteSize];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, StrData, 0, StrData.Length);
                            string value = ((string)Encoding.Default.GetString(StrData).Trim('\0')).Replace(" ", string.Empty);

                            if (string.IsNullOrEmpty(value))
                                value = "0";

                            return Convert.ToInt16(value);
                        }
                    case PacketStruct.OutputFreq:
                    case PacketStruct.Converter_Heatsink_Temp:
                    case PacketStruct.Covnerter_Inner_Temp:
                        {
                            int ByteSize = 4;
                            byte[] StrData = new byte[ByteSize];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, StrData, 0, StrData.Length);
                            string value = ((string)Encoding.Default.GetString(StrData).Trim('\0')).Replace(" ", string.Empty);

                            if (string.IsNullOrEmpty(value))
                                value = "0";
                            return (float)(Convert.ToSingle(value) / 10.0);
                        }
                    default:
                        return -1;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        /// <summary>
        /// CPS Data Get 함수(Byte Array 형태로 문자열 리턴)
        /// </summary>
        /// <param name="ePacketStruct"></param>
        /// <returns></returns>
        public string Get_CPS_Data_Array(PacketStruct ePacketStruct)
        {
            try
            {
                if ((int)ePacketStruct >= Master.m_CPSByteMap.Length || (int)ePacketStruct < 0)
                {
                    LogMsg.AddCPSLog(LogMsg.LogLevel.Error, LogMsg.MsgList.TCPIPMemoryMapOutofRange,
                        $"[CPS->Master] ByteMap Index: {ePacketStruct} >= Length: {Master.m_CPSByteMap.Length}");
                    return string.Empty;
                }

                switch (ePacketStruct)
                {
                    case PacketStruct.STX:
                    case PacketStruct.CheckSum:
                    case PacketStruct.ETX:
                        {
                            int ByteSize = 1;
                            byte[] val = new byte[ByteSize];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, val, 0, val.Length);
                            return BitConverter.ToString(val);
                        }
                    case PacketStruct.ID:
                        {
                            int ByteSize = 2;
                            byte[] ID = new byte[ByteSize];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, ID, 0, ID.Length);
                            return BitConverter.ToString(ID);
                        }
                    case PacketStruct.Status:
                        {
                            //짝수 바이트 ?
                            return string.Empty;
                        }
                    case PacketStruct.Voltage:
                    case PacketStruct.DC_Current:
                    case PacketStruct.IGBT_Current:
                    case PacketStruct.Track_Current:
                    case PacketStruct.OutputFreq:
                    case PacketStruct.Converter_Heatsink_Temp:
                    case PacketStruct.Covnerter_Inner_Temp:
                    case PacketStruct.Converter_Error_Code:
                        {
                            int ByteSize = 4;
                            byte[] StrData = new byte[ByteSize];
                            Buffer.BlockCopy(Master.m_CPSByteMap, (int)ePacketStruct, StrData, 0, StrData.Length);
                            return BitConverter.ToString(StrData);
                        }
                    default:
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
