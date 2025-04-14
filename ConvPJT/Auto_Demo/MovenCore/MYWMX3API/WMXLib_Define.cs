using System;
using System.Drawing;
using System.IO;
using MYWMX3API.Classes;
using WMX3ApiCLR;
using WMX3ApiCLR.EcApiCLR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MYWMX3API
{
    partial class WMXLib
    {
        static public class Define
        {
            static public class EtherCAT
            {
                static public Color GetEcStateMachineColor(EcStateMachine ecStateMachine)
                {
                    switch(ecStateMachine)
                    {
                        case EcStateMachine.Op:
                            return Color.LimeGreen;
                        case EcStateMachine.SafeOp:
                            return Color.Purple;
                        case EcStateMachine.PreOp:
                            return Color.Blue;
                        case EcStateMachine.Init:
                            return Color.Orange;
                        case EcStateMachine.None:
                            return Color.Red;
                        case EcStateMachine.Boot:
                            return Color.HotPink;
                        default:
                            return Color.Red;
                    }
                }
                static public class ESC
                {
                    public enum RegisterIndex
                    {
                        CrcError0 = 0,  //0x300
                        RxError0,       //0x301
                        CrcError1,      //0x302
                        RxError1,       //0x303
                        CrcError2,      //0x304
                        RxError2,       //0x305
                        CrcError3,      //0x306
                        RxError3,       //0x307

                        FrwCrcError0,   //0x308
                        FrwCrcError1,   //0x309
                        FrwCrcError2,   //0x30a
                        FrwCrcError3,   //0x30b

                        ProUnitError,   //0x30c

                        LinkLostError0 = 16, //0x310
                        LinkLostError1,      //0x311
                        LinkLostError2,     //0x312
                        LinkLostError3,     //0x313
                    }
                }
                public enum DataType
                {
                    SIL_BOOLEAN = 0x0001,
                    SIL_INTEGER8 = 0x0002,
                    SIL_INTEGER16 = 0x0003,
                    SIL_INTEGER32 = 0x0004,
                    SIL_UNSIGNED8 = 0x0005,
                    SIL_UNSIGNED16 = 0x0006,
                    SIL_UNSIGNED32 = 0x0007,
                    SIL_FLOAT32 = 0x0008,
                    SIL_VISIBLE_STRING = 0x0009,
                    SIL_OCTET_STRING = 0x000A,
                    SIL_UNICODE_STRING = 0x000B,
                    SIL_TIME_OF_DAY = 0x000C,
                    SIL_TIME_DIFFERENCE = 0x000D,
                    SIL_DOMAIN = 0x000F,
                    SIL_INTEGER24 = 0x0010,
                    SIL_FLOAT64 = 0x0011,
                    SIL_INTEGER40 = 0x0012,
                    SIL_INTEGER48 = 0x0013,
                    SIL_INTEGER56 = 0x0014,
                    SIL_INTEGER64 = 0x0015,
                    SIL_UNSIGNED24 = 0x0016,
                    SIL_UNSIGNED40 = 0x0018,
                    SIL_UNSIGNED48 = 0x0019,
                    SIL_UNSIGNED56 = 0x001A,
                    SIL_UNSIGNED64 = 0x001B,
                    SIL_GUID = 0x001D,
                    SIL_BIT2 = 0x001F,
                    SIL_BIT3 = 0x0020,
                    SIL_BIT4 = 0x0021,
                    SIL_BIT5 = 0x0022,
                    SIL_BIT6 = 0x0023,
                    SIL_BIT7 = 0x0024,
                    SIL_BIT8 = 0x0025,
                    SIL_BITARR8 = 0x002D,
                    SIL_BITARR16 = 0x002E,
                    SIL_BITARR32 = 0x002F
                }
                public enum ObjectAccessType
                {
                    ReadPreOp = 1 << 0,
                    ReadSafeOp = 1 << 1,
                    ReadOp = 1 << 2,
                    WritePreOp = 1 << 3,
                    WriteSafeOp = 1 << 4,
                    WriteOp = 1 << 5,
                    RxPDO = 1 << 6,
                    TxPDO = 1 << 7,
                    BackUp = 1 << 8,
                    Setting = 1 << 9
                }

                static public string GetWMXDataType(ushort type)
                {
                    switch (type)
                    {
                        case (ushort)DataType.SIL_BIT2:
                            return "BIT2";
                        case (ushort)DataType.SIL_BIT3:
                            return "BIT3";
                        case (ushort)DataType.SIL_BIT4:
                            return "BIT4";
                        case (ushort)DataType.SIL_BIT5:
                            return "BIT5";
                        case (ushort)DataType.SIL_BIT6:
                            return "BIT6";
                        case (ushort)DataType.SIL_BIT7:
                            return "BIT7";
                        case (ushort)DataType.SIL_BIT8:
                            return "BIT8";
                        case (ushort)DataType.SIL_BITARR16:
                            return "BITARR16";
                        case (ushort)DataType.SIL_BITARR32:
                            return "BITARR32";
                        case (ushort)DataType.SIL_BITARR8:
                            return "BITARR8";
                        case (ushort)DataType.SIL_BOOLEAN:
                            return "Bool";
                        case (ushort)DataType.SIL_DOMAIN:
                            return "Domain";
                        case (ushort)DataType.SIL_FLOAT32:
                            return "F32";
                        case (ushort)DataType.SIL_FLOAT64:
                            return "F64";
                        case (ushort)DataType.SIL_GUID:
                            return "GUID";
                        case (ushort)DataType.SIL_INTEGER16:
                            return "I16";
                        case (ushort)DataType.SIL_INTEGER24:
                            return "I24";
                        case (ushort)DataType.SIL_INTEGER32:
                            return "I32";
                        case (ushort)DataType.SIL_INTEGER40:
                            return "I40";
                        case (ushort)DataType.SIL_INTEGER48:
                            return "I48";
                        case (ushort)DataType.SIL_INTEGER56:
                            return "I56";
                        case (ushort)DataType.SIL_INTEGER64:
                            return "I64";
                        case (ushort)DataType.SIL_INTEGER8:
                            return "I8";
                        case (ushort)DataType.SIL_OCTET_STRING:
                            return "String";
                        case (ushort)DataType.SIL_TIME_DIFFERENCE:
                            return "TimeDifference";
                        case (ushort)DataType.SIL_TIME_OF_DAY:
                            return "TimeOfDay";
                        case (ushort)DataType.SIL_UNICODE_STRING:
                            return "UniString";
                        case (ushort)DataType.SIL_UNSIGNED16:
                            return "U16";
                        case (ushort)DataType.SIL_UNSIGNED24:
                            return "U24";
                        case (ushort)DataType.SIL_UNSIGNED32:
                            return "U32";
                        case (ushort)DataType.SIL_UNSIGNED40:
                            return "U40";
                        case (ushort)DataType.SIL_UNSIGNED48:
                            return "U48";
                        case (ushort)DataType.SIL_UNSIGNED56:
                            return "U56";
                        case (ushort)DataType.SIL_UNSIGNED64:
                            return "U64";
                        case (ushort)DataType.SIL_UNSIGNED8:
                            return "U8";
                        case (ushort)DataType.SIL_VISIBLE_STRING:
                            return "VS";
                        default:
                            return "Unknown";
                    }
                }
                static public string GetWMXObjectAccess(ushort value)
                {
                    String data = String.Empty;

                    if ((value & (ushort)ObjectAccessType.RxPDO) == (ushort)ObjectAccessType.RxPDO)
                    {
                        return "RxPdo";
                    }
                    else if ((value & (ushort)ObjectAccessType.TxPDO) == (ushort)ObjectAccessType.TxPDO)
                    {
                        return "TxPdo";
                    }

                    if ((value & (ushort)ObjectAccessType.ReadSafeOp) == (ushort)ObjectAccessType.ReadSafeOp ||
                        (value & (ushort)ObjectAccessType.ReadPreOp) == (ushort)ObjectAccessType.ReadPreOp ||
                        (value & (ushort)ObjectAccessType.ReadOp) == (ushort)ObjectAccessType.ReadOp)
                    {
                        data += "r";
                    }
                    if ((value & (ushort)ObjectAccessType.WriteSafeOp) == (ushort)ObjectAccessType.WriteSafeOp ||
                        (value & (ushort)ObjectAccessType.WritePreOp) == (ushort)ObjectAccessType.WritePreOp ||
                        (value & (ushort)ObjectAccessType.WriteOp) == (ushort)ObjectAccessType.WriteOp)
                    {
                        data += "w";
                    }
                    if (data != String.Empty)
                    {
                        if (data.Length == 1)
                        {
                            data += "o";
                        }
                        return data;
                    }

                    return "Unknown";
                }
            }
        }
    }
}
