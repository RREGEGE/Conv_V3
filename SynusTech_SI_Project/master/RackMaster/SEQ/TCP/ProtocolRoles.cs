using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.COMMON;

namespace RackMaster.SEQ.TCP {
    public static class ProtocolRoles {
        //TCP Data Structure
        //LEN (4 byte) + DataType (1 byte) + Data Address (2 byte) + Data (N byte)

        //Mean
        //DataLen : Data Type + Data Address + Data의 모든 길이의 합
        //Data Type : DataType Enum
        //Data Address : Data의 시작 Address
        //TCPHeaderLen : Packet Parsing을 위한 최소한의 길이
        //Data : Bit Group or Word Group의 Data 값

        //byte[] myByteArray = myIntArray.Cast<byte>().ToArray();
        //byte[] myByteArray = myIntArray.Select(i => (byte)i).ToArray();

        //ReceivePacket
        public const int Recv_TCPHeaderLen = Recv_DataLen + Recv_DataTypeLen + Recv_DataAddrLen;
        public const int Recv_DataLen = 4;
        public const int Recv_DataTypeLen = 1;
        public const int Recv_DataAddrLen = 2;
        public const bool Recv_LittleEndian = true;

        //SendPacket
        public const int Send_TCPHeaderLen = Send_DataLen + Send_DataTypeLen + Send_DataOffsetLen;
        public const int Send_DataLen = 4;
        public const int Send_DataTypeLen = 1;
        public const int Send_DataOffsetLen = 2;
        public const bool Send_LittleEndian = true;

        public enum DataType {
            STK_CIM_2_Controller_Master_Bit_Data = 0,
            STK_CIM_2_Controller_Master_Word_Data,
            STK_CIM_2_Controller_RM_Bit_Data,
            STK_CIM_2_Controller_RM_Word_Data,
            STK_CIM_2_Controller_Port_Bit_Data,
            STK_CIM_2_Controller_Port_Word_Data,

            Controller_2_STK_CIM_Master_Bit_Data = 6,
            Controller_2_STK_CIM_Master_Word_Data,
            Controller_2_STK_CIM_RM_Bit_Data,
            Controller_2_STK_CIM_RM_Word_Data,
            Controller_2_STK_CIM_Port_Bit_Data,
            Controller_2_STK_CIM_Port_Word_Data,
            ControlMSG = 100
        }
        /// <summary>
        /// CIM으로부터 받은 데이터 타입이 존재하는 데이터 타입인지 체크
        /// </summary>
        /// <param name="_DataType"></param>
        /// <returns></returns>
        static public bool IsDataTypeValid(sbyte _DataType) {
            switch (_DataType) {
                case (sbyte)DataType.STK_CIM_2_Controller_Master_Bit_Data:
                case (sbyte)DataType.STK_CIM_2_Controller_Master_Word_Data:
                case (sbyte)DataType.STK_CIM_2_Controller_RM_Bit_Data:
                case (sbyte)DataType.STK_CIM_2_Controller_RM_Word_Data:
                case (sbyte)DataType.STK_CIM_2_Controller_Port_Bit_Data:
                case (sbyte)DataType.STK_CIM_2_Controller_Port_Word_Data:

                case (sbyte)DataType.Controller_2_STK_CIM_Master_Bit_Data:
                case (sbyte)DataType.Controller_2_STK_CIM_Master_Word_Data:
                case (sbyte)DataType.Controller_2_STK_CIM_RM_Bit_Data:
                case (sbyte)DataType.Controller_2_STK_CIM_RM_Word_Data:
                case (sbyte)DataType.Controller_2_STK_CIM_Port_Bit_Data:
                case (sbyte)DataType.Controller_2_STK_CIM_Port_Word_Data:

                case (sbyte)DataType.ControlMSG:
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        /// CIM으로부터 받은 Packet이 Protocol Role에 맞춰진 Packet인지 확인
        /// </summary>
        /// <param name="_packets"></param>
        /// <param name="packet_length"></param>
        /// <param name="Title"></param>
        /// <returns></returns>
        static public bool IsPacketValid(ref byte[] _packets, ref int packet_length, string Title) {
            int value_DataLen = GetValue_DataLen(_packets);
            sbyte value_DataType = GetValue_DataType(_packets);
            short value_DataAddress = GetValue_DataAddress(_packets);

            if (value_DataLen < 0 || value_DataLen > 40960) {
                byte[] CurrentPacket = new byte[packet_length];
                Array.Copy(_packets, 0, CurrentPacket, 0, packet_length);
                //Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.TCPIP, $"{Title} Warning Packet Length Data : {value_DataLen}, {BitConverter.ToString(CurrentPacket)}"));
                SetPacketClear(ref _packets, ref packet_length);
                return false;
            }

            if (!IsDataTypeValid(value_DataType)) {
                byte[] CurrentPacket = new byte[packet_length];
                Array.Copy(_packets, 0, CurrentPacket, 0, packet_length);
                //Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.TCPIP, $"{Title} Invalid Data Type : {BitConverter.ToString(CurrentPacket)}"));
                SetPacketClear(ref _packets, ref packet_length);
                return false;
            }

            if (packet_length < value_DataLen + Recv_DataLen) {
                byte[] CurrentPacket = new byte[packet_length];
                Array.Copy(_packets, 0, CurrentPacket, 0, packet_length);
                //Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.TCPIP, $"{Title} Packet Not Enough : {BitConverter.ToString(CurrentPacket)}"));
                return false;
            }

            if (value_DataLen != -1 &&
                value_DataType != -1 &&
                value_DataAddress != -1) {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Packet이 올바르지 않을 때 packet array를 클리어해준다.
        /// </summary>
        /// <param name="_packets"></param>
        /// <param name="packet_length"></param>
        static public void SetPacketClear(ref byte[] _packets, ref int packet_length) {
            packet_length = 0;
            Array.Clear(_packets, 0, _packets.Length);
        }
        /// <summary>
        /// CIM으로부터 받은 packet 중 Data Length에 해당하는 부분만 반환
        /// </summary>
        /// <param name="_packets"></param>
        /// <returns></returns>
        static public int GetValue_DataLen(byte[] _packets) {
            try {
                byte[] DataLenArray = new byte[Recv_DataLen];            //4byte array 생성
                Array.Copy(_packets, 0, DataLenArray, 0, Recv_DataLen);  //0~3byte 까지 복사

                if (Recv_LittleEndian)
                    Array.Reverse(DataLenArray);

                return BitConverter.ToInt32(DataLenArray, 0);
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, $"TCP Get Value Data Len Fail", ex));
                return -1;
            }
        }
        /// <summary>
        /// CIM으로부터 받은 packet 중 Data Type에 해당하는 부분만 반환
        /// </summary>
        /// <param name="_packets"></param>
        /// <returns></returns>
        static public sbyte GetValue_DataType(byte[] _packets) {
            try {
                int offset = Recv_DataLen;
                return Convert.ToSByte(_packets[offset]); //4byte 위치의 byte를 return
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, $"TCP Get Value Data Type Fail", ex));
                return -1;
            }
        }
        /// <summary>
        /// CIM으로부터 받은 packet 중 Data 시작 Address 부분만 반환
        /// </summary>
        /// <param name="_packets"></param>
        /// <returns></returns>
        static public short GetValue_DataAddress(byte[] _packets) {
            try {
                int offset = Recv_DataLen + Recv_DataTypeLen;
                byte[] DataAddressArr = new byte[Recv_DataAddrLen];                  //2byte Array 생성
                Array.Copy(_packets, offset, DataAddressArr, 0, Recv_DataAddrLen);   //5byte 위치부터 2byte 복사

                if (Recv_LittleEndian)
                    Array.Reverse(DataAddressArr);

                return BitConverter.ToInt16(DataAddressArr, 0);
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, $"TCP Get Value Data Address Fail", ex));
                return -1;
            }
        }
        /// <summary>
        /// CIM으로부터 받은 packet 중 실제 Data 부분만 반환
        /// </summary>
        /// <param name="_packets"></param>
        /// <returns></returns>
        static public byte[] GetReceiveDataArray(byte[] _packets) {
            byte[] receive_values = new byte[_packets.Length - Recv_TCPHeaderLen];
            Buffer.BlockCopy(_packets, Recv_TCPHeaderLen, receive_values, 0, _packets.Length - Recv_TCPHeaderLen);

            if (Recv_LittleEndian) {
                for (int nCount = 0; nCount < receive_values.Length / sizeof(short); nCount++)
                    Array.Reverse(receive_values, nCount * sizeof(short), sizeof(short));
            }

            return receive_values;
        }
        /// <summary>
        /// CIM으로부터 받은 word Data를 byte array로 복사
        /// </summary>
        /// <param name="_wordmap_addr"></param>
        /// <param name="_bytesize"></param>
        /// <param name="_maps"></param>
        /// <returns></returns>
        static public byte[] GetCurrentWordDataToByteArray(short _wordmap_addr, int _bytesize, short[] _maps) {
            int byte_addr = _wordmap_addr * sizeof(short);

            byte[] current_values = new byte[_bytesize];
            Buffer.BlockCopy(_maps, byte_addr, current_values, 0, _bytesize);

            return current_values;
        }
        /// <summary>
        /// byte array 2개를 비교해서 다를 경우 false 반환, 같을 경우 true 반환
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static public bool IsByteArrayCompare(byte[] a, byte[] b) {
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; ++i) {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
    }

    public static class ProtocolRoles_Regulator {
        // Protocol Role
        // 0 Byte               = Start Data,                   Len = 1Byte
        // 1,2,3 Byte           = Status,                       Len = 3Byte
        // 4,5,6,7 Byte         = Regulator Boost 전압[V],       Len = 4Byte
        // 8,9,10,11 Byte       = Regulator 출력 전압[V],        Len = 4Byte
        // 12,13,14,15 Byte     = Regulator IBoost 전류[A],      Len = 4Byte
        // 16,17,18,19 Byte     = Regulator 출력 전류[A],        Len = 4Byte
        // 20,21,22,23 Byte     = Reserved,                     Len = 4Byte
        // 24,25,26,27 Byte     = Reserved,                     Len = 4Byte
        // 28,29,30,31 Byte     = Regulator Pickup NTC[C],      Len = 4Byte
        // 32,33,34,35 Byte     = Error Code,                   Len = 4Byte
        // 36,37,38,39 Byte     = Regulator 정류기 온도[C],      Len = 4Byte
        // 40,41,42,43 Byte     = Regulator IGBT 온도[C],        Len = 4Byte
        // 44,45,46,47 Byte     = Regulator Reactor 온도[C],     Len = 4Byte
        // 48,49,50,51 Byte     = Regulator 압착단자1,           Len = 4Byte
        // 52,53,54,55 Byte     = Regulator 압착단자2,           Len = 4Byte
        // 56Byte               = Check Sum,                    Len = 1Byte
        // 57Byte               = End,                          Len = 1Byte
        public enum RegulatorStatus {
            Stop,
            Run,
            Fault,
            Warning,
            Error,
        }

        public enum RegulatorToRackMaster {
            STX = 0,
            Status = 1,
            BoostVoltage = 4,
            OutputVoltage = 8,
            BoostCurrent = 12,
            OutputCurrent = 16,
            HitSinkTemperature = 20,
            InsideNTC = 24,
            PickupNTC = 28,
            ErrorCode = 36,
            CheckSum = 38,
            End = 39,
        }

        public enum RackMasterToRegulator {
            Start,
            Command,
            WPS_Regulator_ID = 2,
            Year = 4,
            Month = 6,
            Date = 8,
            Hour = 10,
            Minute = 12,
            Second = 14,
            CheckSum = 16,
            End = 17
        }

        public const bool m_receiveLittleEndian = false;

        public const byte m_STXFormat = 2;
        public const byte m_EndFormat = 3;
        /// <summary>
        /// Regulator으로부터 받은 packet 중 regulator status만 반환
        /// </summary>
        /// <param name="packets"></param>
        /// <returns></returns>
        public static RegulatorStatus GetRegulatorStatus(byte[] packets) {
            try {
                int status;
                if (m_receiveLittleEndian) {
                    status = (short)((packets[(int)RegulatorToRackMaster.Status] * 100) + (packets[(int)RegulatorToRackMaster.Status + 1] * 10) +
                        packets[(int)RegulatorToRackMaster.Status + 2]);
                }
                else {
                    status = (short)(packets[(int)RegulatorToRackMaster.Status] + (packets[(int)RegulatorToRackMaster.Status + 1] * 10) +
                    (packets[(int)RegulatorToRackMaster.Status + 2] * 100));
                }

                return (RegulatorStatus)status;
            }catch(Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, $"Convert Regulator Status Fail", ex));
                return RegulatorStatus.Error;
            }
        }
        /// <summary>
        /// Regulator로부터 받은 packet이 Protocol Role에 맞는지 체크
        /// </summary>
        /// <param name="packets"></param>
        /// <param name="packetLength"></param>
        /// <returns></returns>
        public static bool IsPacketValid(byte[] packets, int packetLength) {
            if(packets.Length != packetLength) {
                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.TCP, $"Regulator Packet Length Not Correct, length={packets.Length}"));
                return false;
            }

            if(GetCheckSum_Receive(packets) != GetRegulatorData(packets, RegulatorToRackMaster.CheckSum)) {
                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.TCP, $"Regulator Checksum Error, Checksum={GetCheckSum_Receive(packets)}, PacketCheckSum={GetRegulatorData(packets, RegulatorToRackMaster.CheckSum)}"));
                return false;
            }

            if(GetRegulatorData(packets, RegulatorToRackMaster.STX) != m_STXFormat) {
                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.TCP, $"Regulator STX Error, STX={GetRegulatorData(packets, RegulatorToRackMaster.STX)}"));
                return false;
            }

            if(GetRegulatorData(packets, RegulatorToRackMaster.End) != m_EndFormat) {
                Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.TCP, $"Regulator End Error, STX={GetRegulatorData(packets, RegulatorToRackMaster.End)}"));
                return false;
            }

            return true;
        }
        /// <summary>
        /// Regulator로부터 받은 packet 중 실제 data에 해당하는 부분만 반환
        /// </summary>
        /// <param name="packets"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static short GetRegulatorData(byte[] packets, RegulatorToRackMaster order) {
            try {
                short data = 0;
                if (m_receiveLittleEndian) {
                    Array.Reverse(packets);
                }
                switch (order) {
                    case RegulatorToRackMaster.BoostVoltage:
                    case RegulatorToRackMaster.OutputVoltage:
                    case RegulatorToRackMaster.BoostCurrent:
                    case RegulatorToRackMaster.OutputCurrent:
                    case RegulatorToRackMaster.HitSinkTemperature:
                    case RegulatorToRackMaster.InsideNTC:
                    case RegulatorToRackMaster.PickupNTC:
                        if (!m_receiveLittleEndian) {
                            data = (short)((GetASCIIData(packets[(int)order]) * 1000) + (GetASCIIData(packets[(int)order + 1]) * 100) +
                                (GetASCIIData(packets[(int)order + 2]) * 10) + GetASCIIData(packets[(int)order]));
                        }
                        else {
                            data = (short)(GetASCIIData(packets[(int)order]) + (GetASCIIData(packets[(int)order + 1]) * 10) +
                            (GetASCIIData(packets[(int)order + 2]) * 100) + (GetASCIIData(packets[(int)order * 1000]) * 1000));
                        }
                        return data;

                    case RegulatorToRackMaster.ErrorCode:
                        if (!m_receiveLittleEndian) {
                            data = (short)((GetASCIIData(packets[(int)order]) * 10) + GetASCIIData(packets[(int)order + 1]));
                        }
                        else {
                            data = (short)((GetASCIIData(packets[(int)order])) + (GetASCIIData(packets[(int)order + 1]) * 10));
                        }
                        return data;

                    case RegulatorToRackMaster.CheckSum:
                        //data = (short)GetASCIIData(packets[(int)order]);
                        data = (short)packets[(int)order];
                        return data;

                    case RegulatorToRackMaster.STX:
                    case RegulatorToRackMaster.End:
                        data = packets[(int)order];
                        return data;
                    default:
                        return 0;
                }
            }
            catch (Exception ex) {
                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.TCP, $"Convert Boost Voltage Fail", ex));
                return -1;
            }
        }
        /// <summary>
        /// short 데이터를 ascii 코드로 반환
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int GetASCIIData(short data) {
            char convData = (char)data;
            return (convData - '0');
        }
        /// <summary>
        /// chart 데이터를 ascii 코드로 반환
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int CharToASCII(char data) {
            return (int)data;
        }
        /// <summary>
        /// int 데이터를 ascii 코드로 반환
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int IntToASCII(int data) {
            char charData = (char)(data + '0');

            return (int)charData;
        }
        /// <summary>
        /// Regulator로 받은 packet의 체크섬 반환
        /// </summary>
        /// <param name="packets"></param>
        /// <returns></returns>
        public static short GetCheckSum_Receive(byte[] packets) {
            int sum = 0;
            for (int i = (int)RegulatorToRackMaster.Status; i < ((int)RegulatorToRackMaster.CheckSum); i++) {
                sum += packets[i];
                //sum += GetASCIIData(packets[i]);
            }
            return (short)(sum & 0xff);
        }
        /// <summary>
        /// Regulator로 보낼 packet의 체크섬 반환
        /// </summary>
        /// <param name="packets"></param>
        /// <returns></returns>
        public static short GetCheckSum_Send(byte[] packets) {
            int sum = 0;
            for(int i = (int)RackMasterToRegulator.Command; i < (int)RackMasterToRegulator.CheckSum; i++) {
                sum += packets[i];
                //sum += GetASCIIData(packets[i]);
            }
            return (short)(sum & 0xff);
        }
    }
}
