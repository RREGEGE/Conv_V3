using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;
using System.Threading;

namespace MovenCore
{
    public class WMXIO
    {
        /// <summary>
        /// WMX Input 상태를 Bit 단위로 얻어옴
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <param name="Bit"></param>
        /// <returns></returns>
        public bool GetInputBit(int StartAddr, int Bit)
        {
            if (StartAddr < 0 || StartAddr >= 8000)
                return false;

            if (Bit < 0 || Bit >= 8)
                return false;

            if (WMX3.GetIOPt() != null)
            {
                byte pData = 0;
                WMX3.GetIOPt().GetInBitEx(StartAddr, Bit, ref pData);

                return pData == 1;
            }
            else
                return false;
        }
        
        /// <summary>
        /// WMX Input 상태를 Byte 단위로 얻어옴
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <returns></returns>
        public byte GetInputByte(int StartAddr)
        {
            byte[] retBytes = new byte[1];

            if (StartAddr < 0 || StartAddr >= 8000)
                return retBytes[0];

            if (WMX3.GetIOPt() != null)
            {
                byte pData = 0;
                WMX3.GetIOPt().GetInByteEx(StartAddr, ref pData);

                return pData;
            }
            else
                return 0;
        }
        
        /// <summary>
        /// WMX Input 상태를 Byte 배열 단위로 얻어옴
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public byte[] GetInputBytes(int StartAddr, int Length)
        {
            byte[] retBytes = new byte[Length];

            if (StartAddr < 0 || Length < 0)
                return retBytes;

            if (WMX3.GetIOPt() != null)
            {
                WMX3.GetIOPt().GetInBytesEx(StartAddr, Length, ref retBytes);

                return retBytes;
            }
            else
                return retBytes;
        }
        
        /// <summary>
        /// WMX Output 상태를 Bit 단위로 얻어옴
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <param name="Bit"></param>
        /// <returns></returns>
        public bool GetOutputBit(int StartAddr, int Bit)
        {
            if (StartAddr < 0 || StartAddr >= 8000)
                return false;

            if (Bit < 0 || Bit >= 8)
                return false;

            if (WMX3.GetIOPt() != null)
            {
                byte pData = 0;
                WMX3.GetIOPt().GetOutBitEx(StartAddr, Bit, ref pData);

                return pData == 1;
            }
            else
                return false;
        }
        
        /// <summary>
        /// WMX Output 상태를 Byte 단위로 얻어옴
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <returns></returns>
        public byte GetOutputByte(int StartAddr)
        {
            byte[] retBytes = new byte[1];

            if (StartAddr < 0 || StartAddr >= 8000)
                return retBytes[0];

            if (WMX3.GetIOPt() != null)
            {
                byte pData = 0;
                WMX3.GetIOPt().GetOutByteEx(StartAddr, ref pData);

                return pData;
            }
            else
                return (byte)0;
        }
        
        /// <summary>
        /// WMX Output 상태를 Byte 배열 단위로 얻어옴
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public byte[] GetOutputBytes(int StartAddr, int Length)
        {
            byte[] retBytes = new byte[Length];

            if (StartAddr < 0 || Length < 0)
                return retBytes;

            if (WMX3.GetIOPt() != null)
            {
                WMX3.GetIOPt().GetOutBytesEx(StartAddr, Length, ref retBytes);

                return retBytes;
            }
            else
                return retBytes;
        }
        
        /// <summary>
        /// WMX Output을 Bit 단위로 처리 진행
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <param name="Bit"></param>
        /// <param name="bEnable"></param>
        public void SetOutputBit(int StartAddr, int Bit, bool bEnable)
        {
            if (StartAddr < 0 || StartAddr >= 8000)
                return;

            if (Bit < 0 || Bit >= 8)
                return;

            if (WMX3.GetIOPt() != null)
            {
                WMX3.GetIOPt().SetOutBitEx(StartAddr, Bit, bEnable ? (byte)1 : (byte)0);
            }
        }
        
        /// <summary>
        /// WMX Output을 Byte 단위로 처리 진행
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <param name="_Byte"></param>
        public void SetOutputByte(int StartAddr, byte _Byte)
        {
            if (StartAddr < 0 || StartAddr >= 8000)
                return;

            if (WMX3.GetIOPt() != null)
            {
                WMX3.GetIOPt().SetOutByteEx(StartAddr, _Byte);
            }
        }
        
        /// <summary>
        /// WMX Output을 Byte 배열 단위로 처리 진행
        /// </summary>
        /// <param name="StartAddr"></param>
        /// <param name="Array"></param>
        public void SetOutputBytes(int StartAddr, byte[] Array)
        {
            if (StartAddr < 0 || Array.Length <= 0)
                return;

            if (WMX3.GetIOPt() != null)
            {
                WMX3.GetIOPt().SetOutBytesEx(StartAddr, Array.Length, Array);
            }
        }
    }
}
