using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMX3ApiCLR;

namespace Conv_Step_Simulation
{
    public static class WMXParam
    {
        public static int ErrorCode_None = 0;

        public enum m_engineState
        {
            Idle = EngineState.Idle,
            Running = EngineState.Running,
            Communicating = EngineState.Communicating,
            Shutdown = EngineState.Shutdown,
            Unknown = EngineState.Unknown
        }

        public enum m_homeType
        {
            CurPos = Config.HomeType.CurrentPos,
            ZPulse = Config.HomeType.ZPulse,
            HS = Config.HomeType.HS,
            HSHS = Config.HomeType.HSHS,
            HSZPulse = Config.HomeType.HSZPulse,
            HSRevZPulse = Config.HomeType.HSReverseZPulse,
            HSOff = Config.HomeType.HSOff,
            HSOffZPulse = Config.HomeType.HSOffZPulse,
            HSOffRevZPulse = Config.HomeType.HSOffReverseZPulse,
            LSRevZPulse = Config.HomeType.LSReverseZPulse,
            NearLSRevZPulse = Config.HomeType.NearLSReverseZPulse,
            ExtLSRevZPulse = Config.HomeType.ExternalLSReverseZPulse,
            TouchProbe = Config.HomeType.TouchProbe,
            HSTouchProbe = Config.HomeType.HSTouchProbe,
            LS = Config.HomeType.LS,
            NearLS = Config.HomeType.NearLS,
            ExtLS = Config.HomeType.ExternalLS,
            MechEnd = Config.HomeType.MechanicalEndDetection,
            MechEndHS = Config.HomeType.MechanicalEndDetectionHS,
            MechEndLS = Config.HomeType.MechanicalEndDetectionLS,
            MechEndRevZPulse = Config.HomeType.MechanicalEndDetectionReverseZPulse,
        }

        public enum m_triggerType
        {
            RemainingTime = TriggerType.RemainingTime,
            RemainginDistance = TriggerType.RemainingDistance,
            SameTimeCompletion = TriggerType.SameTimeCompletion,
            CompletedTime = TriggerType.CompletedTime,
            CompletedDistance = TriggerType.CompletedDistance,
            StaggeredTimeCompletion = TriggerType.StaggeredTimeCompletion,
            StaggeredDistanceCompletion = TriggerType.StaggeredDistanceCompletion,
            DistanceToTarget = TriggerType.DistanceToTarget
        }

        public enum m_waitType
        {
            AxisIdle = Motion.WaitConditionType.AxisIdle,
            MotionStarted = Motion.WaitConditionType.MotionStarted,
            MotionStartedOverrideReady = Motion.WaitConditionType.MotionStartedOverrideReady
        }

        public enum m_limitDirection
        {
            Normal = Config.LimitSwitchDirection.Normal,
            Reverse = Config.LimitSwitchDirection.Reverse
        }

        public enum m_motorDirection
        {
            Positive,
            Negative
        }

        public enum m_homeDirection
        {
            Positive = Config.HomeDirection.Positive,
            Negative = Config.HomeDirection.Negative
        }

        public enum m_profileType
        {
            Trapezoidal = ProfileType.Trapezoidal,
            SCurve = ProfileType.SCurve,
            JerkRatio = ProfileType.JerkRatio
        }

        public enum m_limitSwitchType
        {
            None = Config.LimitSwitchType.None,
            ServoOff = Config.LimitSwitchType.ServoOff,
            DecServoOff = Config.LimitSwitchType.DecServoOff,
            Dec = Config.LimitSwitchType.Dec,
            SlowDecServoOff = Config.LimitSwitchType.SlowDecServoOff,
            SlowDec = Config.LimitSwitchType.SlowDec
        }

        public enum m_linIntplCalcMode
        {
            AxisLimit = Config.LinearIntplProfileCalcMode.AxisLimit,
            MatchSlowestAxis = Config.LinearIntplProfileCalcMode.MatchSlowestAxis,
            MatchFarthestAxis = Config.LinearIntplProfileCalcMode.MatchFarthestAxis
        }

        public static class CommStatus
        {
            public static long m_cyclePeriod = 0;
            public static long m_cycleCount = 0;
            public static long m_DCDiffAvg = 0;
            public static long m_DCDiffMin = 0;
            public static long m_DCDiffMax = 0;

            public static ulong m_packetIntervalAvg = 0;
            public static ulong m_packetIntervalMin = 0;
            public static ulong m_packetIntervalMax = 0;

            public static uint m_packetTimeOut = 0;
            public static uint m_packetLoss = 0;
            public static uint m_TxDelayAvg = 0;
            public static uint m_TxDelayMin = 0;
            public static uint m_TxDelayMax = 0;
            public static uint m_numOfSlaves = 0;
            public static uint m_numOfAxes = 0;
        }
    }
}
