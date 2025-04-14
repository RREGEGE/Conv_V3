using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.TCP
{
    static public class ProtocolRoles
    {
        /// <summary>
        /// 초기 CIM <-> Master <-> STK 구조의 프로토콜 규칙
        /// </summary>

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

        public enum DataType{
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
        public enum ErrorType
        {
            None,
            ExceptionDataLen,
            ExceptionDataType,
            ExceptionDataAddress,
            ReadLengthOutofRange,
            InvalidDataType,
            ReadDataNotEnough
        }
        static public bool IsDataTypeValid(sbyte _DataType)
        {
            switch(_DataType)
            {
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
        static public bool IsPacketValid(ref byte[] _packets, ref int packet_length, out ErrorType eErrorType)
        {
            eErrorType              = ErrorType.None;
            int value_DataLen       = GetValue_DataLen(_packets);
            sbyte value_DataType    = GetValue_DataType(_packets);
            short value_DataAddress = GetValue_DataMapAddress(_packets);

            if(value_DataLen == -1)
            {
                eErrorType = ErrorType.ExceptionDataLen;
                return false;
            }
            if (value_DataType == -1)
            {
                eErrorType = ErrorType.ExceptionDataType;
                return false;
            }
            if (value_DataAddress == -1)
            {
                eErrorType = ErrorType.ExceptionDataAddress;
                return false;
            }

            if (value_DataLen <= 0 || value_DataLen > 40960)
            {
                eErrorType = ErrorType.ReadLengthOutofRange;
                return false;
            }

            if (!IsDataTypeValid(value_DataType))
            {
                eErrorType = ErrorType.InvalidDataType;
                return false;
            }

            if (packet_length < value_DataLen + Recv_DataLen)
            {
                eErrorType = ErrorType.ReadDataNotEnough;
                return false;
            }

            return true;
        }

        static public void SetPacketClear(ref byte[] _packets, ref int packet_length)
        {
            packet_length = 0;
            Array.Clear(_packets, 0, _packets.Length);
        }

        static public int GetValue_DataLen(byte[] _packets)
        {
            try
            {
                byte[] DataLenArray = new byte[Recv_DataLen];            //4byte array 생성
                Array.Copy(_packets, 0, DataLenArray, 0, Recv_DataLen);  //0~3byte 까지 복사

                if(Recv_LittleEndian)
                    Array.Reverse(DataLenArray);

                return BitConverter.ToInt32(DataLenArray, 0);
            }
            catch
            {
                return -1;
            }
        }
        static public sbyte GetValue_DataType(byte[] _packets)
        {
            try
            {
                int offset = Recv_DataLen;
                return Convert.ToSByte(_packets[offset]); //4byte 위치의 byte를 return
            }
            catch
            {
                return -1;
            }
        }
        static public short GetValue_DataMapAddress(byte[] _packets)
        {
            try
            {
                int offset = Recv_DataLen + Recv_DataTypeLen;
                byte[] DataAddressArr = new byte[Recv_DataAddrLen];                  //2byte Array 생성
                Array.Copy(_packets, offset, DataAddressArr, 0, Recv_DataAddrLen);   //5byte 위치부터 2byte 복사

                if (Recv_LittleEndian)
                    Array.Reverse(DataAddressArr);

                return BitConverter.ToInt16(DataAddressArr, 0);
            }
            catch
            {
                return -1;
            }
        }
        
        static public byte[] GetReceiveDataArray(byte[] _packets, bool bWord = true)
        {
            byte[] receive_values = new byte[_packets.Length - Recv_TCPHeaderLen];
            Buffer.BlockCopy(_packets, Recv_TCPHeaderLen, receive_values, 0, _packets.Length - Recv_TCPHeaderLen);

            if (Recv_LittleEndian && bWord)
            {
                for (int nCount = 0; nCount < receive_values.Length / sizeof(short); nCount++)
                    Array.Reverse(receive_values, nCount * sizeof(short), sizeof(short));
            }

            return receive_values;
        }
        static public byte[] GetCurrentWordDataToByteArray(short _wordmap_addr, int _bytesize, short[] _maps)
        {
            int byte_addr = _wordmap_addr * sizeof(short);

            byte[] current_values = new byte[_bytesize];
            Buffer.BlockCopy(_maps, byte_addr, current_values, 0, _bytesize);

            return current_values;
        }
        static public bool IsByteArrayCompare(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; ++i)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
    }
}
