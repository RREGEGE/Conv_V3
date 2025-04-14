using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master.Interface.Math
{
    /// <summary>
    /// Bit 연산을 위한 클래스
    /// </summary>
    static class BitOperation
    {
        static public bool GetBit(byte value, int BitIndex)
        {
            int val = (0x1 << BitIndex);
            return (value & val) == val;
        }
        static public bool GetBit(short value, int BitIndex)
        {
            int val = (0x1 << BitIndex);
            return (value & val) == val;
        }
        static public bool GetBit(int value, int BitIndex)
        {
            int val = (0x1 << BitIndex);
            return (value & val) == val;
        }
        static public void SetBit(ref byte value, int BitIndex, bool Enable)
        {
            if (Enable)
                value = (byte)(value | (byte)(0x01 << BitIndex));
            else
                value = (byte)(value & ~(0x01 << BitIndex));
        }
        static public void SetBit(ref short value, int BitIndex, bool Enable)
        {
            if (Enable)
                value = (short)(value | (short)(0x01 << BitIndex));
            else
                value = (short)(value & ~(0x01 << BitIndex));
        }
        static public void SetBit(ref int value, int BitIndex, bool Enable)
        {
            if (Enable)
                value = value | (0x01 << BitIndex);
            else
                value = value & ~(0x01 << BitIndex);
        }
    }
}
