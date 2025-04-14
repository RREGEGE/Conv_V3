using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using RackMaster.SEQ.CLS;
using MovenCore;

namespace RackMaster.SEQ.COMMON
{
    public static class Io
    {
        private static WMXIO m_io;

        private static byte[] m_inputs;
        private static byte[] m_outputs;

        public const int INPUT_ADDR = 0;
        public const int OUTPUT_ADDR = 0;
        public const int MAX_BYTE = 100;

        public static void Init()
        {
            m_io = new WMXIO();
            m_inputs = new byte[MAX_BYTE];
            m_outputs = new byte[MAX_BYTE];
        }
        /// <summary>
        /// 현재 WMX 상의 Input 상태 업데이트
        /// </summary>
        private static void UpdateInputs()
        {
            m_inputs = m_io.GetInputBytes(INPUT_ADDR, MAX_BYTE);
        }
        /// <summary>
        /// 현재 WMX 상의 Output 상태 업데이트
        /// </summary>
        private static void UpdateOutputs()
        {
            m_outputs = m_io.GetOutputBytes(OUTPUT_ADDR, MAX_BYTE);
        }
        /// <summary>
        /// 위 두 함수를 호출
        /// </summary>
        public static void Update() {
            UpdateInputs();
            UpdateOutputs();
        }
        /// <summary>
        /// 현재 WMX 상의 특정 Input Bit Get
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static bool GetInputBit(int addr) {
            BitArray bitArr = new BitArray(m_inputs);

            return bitArr[addr];
        }
        /// <summary>
        /// 현재 WMX 상의 특정 Byte, Bit 주소에 맞는 Bit Get
        /// </summary>
        /// <param name="byteAddr"></param>
        /// <param name="bitAddr"></param>
        /// <returns></returns>
        public static bool GetInputBit(int byteAddr, int bitAddr) {
            return m_io.GetInputBit(byteAddr, bitAddr);
        }
        /// <summary>
        /// 현재 WMX 상의 특정 주소의 Output bit Get
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public static bool GetOutputBit(int addr) {
            BitArray bitArr = new BitArray(m_outputs);

            return bitArr[addr];
        }
        /// <summary>
        /// 현재 WMX 상의 특정 Byte, Bit 주소의 Output Bit 값을 설정
        /// </summary>
        /// <param name="byteAddr"></param>
        /// <param name="bitAddr"></param>
        /// <param name="value"></param>
        public static void SetOutputBit(int byteAddr, int bitAddr, bool value)
        {
            m_io.SetOutputBit(byteAddr, bitAddr, value);
        }
        /// <summary>
        /// 현재 WMX 상의 특정 Output Bit 값을 설정
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="value"></param>
        public static void SetOutputBit(int addr, bool value) {
            int byteAddr = addr / 8;
            int bitAddr = addr % 8;

            m_io.SetOutputBit(byteAddr, bitAddr, value);
        }
        /// <summary>
        /// 현재 WMX 상의 특정 주소의 Input Byte 값을 Int 형으로 반환
        /// </summary>
        /// <param name="byteAddr"></param>
        /// <returns></returns>
        public static int GetInputData_Int(int byteAddr) {
            byte[] data = m_io.GetInputBytes(byteAddr, sizeof(int));

            return BitConverter.ToInt32(data, 0);
        }
    }
}
