using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ.COMMON {
    public static class FileParameter {
        public enum RMSettingsGeneralKey {
            ForkType,
            RowsCount,
            ColumnsCount,
            ZOverridePercent,
            ZOverrideDownDistance,
            ZOVerrideUpDistance,
            ArmHomePosition,
        }

        public enum ServoKey {
            AxisNumber,
            AutoSpeedPercent,
            MaxSpeed,
            MaxAccDec,
            MinAccDec,
            JerkRatio,
            SWPositiveLimit,
            SWNegativeLimit,
            JogHighSpeedLimit,
            JogLowSpeedLimit,
            InchingLimit,
            ManualHighSpeed,
            ManualHighAccDec,
            ManualLowSpeed,
            ManualLowAccDec,
            MaxOverload,
        }

        public enum PortKey {
            PortType,
            PortID,
            RowIndex,
            ColumnIndex,
            Direction
        }

        public enum ServoSection {
            RackMasterX,
            RackMasterZ,
            RackMasterA,
            RackMasterT
        }

        public enum RMSettingsSection {
            General
        }

        public enum PortSection {
            Port,
        }
    }
}
