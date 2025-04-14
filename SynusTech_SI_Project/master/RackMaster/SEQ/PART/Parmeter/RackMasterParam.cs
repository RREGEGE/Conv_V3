using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.CLS;
using RackMaster.SEQ.COMMON;
using MovenCore;

namespace RackMaster.SEQ.PART {
    public enum MotionParameter {
        ForkType,                   // Fork Type(Slide, Slide-No Turn, SCARA)
        ZOverridePercent,           // Z축이 오버라이드 할 때의 속도(Auto Speed에 대한)비율
        ZOverrideFromUpDistance,
        ZOverrideFromDownDistance,
        ZOverrideToUpDistance,
        ZOverrideToDownDistance,
        TurnCenterPosition,         // Turn축의 Left/Right 판단을 위한 위치(Unit : deg)
        ArmHomePositionRange,       // Arm축의 Home Position이라고 판단하는 범위
        UseInterpolation,
        StopMode,
        UseFullclosed,
        DistanceDetectSensor,       // 거리 감지 센서의 시작 Byte 주소
        ZAutoHomingCount,           // 몇 번의 Cycle 만에 Auto Homing을 진행할지
        PresenseSensorConditionType,
        InPlaceSensorConditionType,
        InPlaceSensorType,
        ZAxisBeltType,
        ZAxisBeltHomeOffset,
        ZAxisBeltFirstDia,
        ZAxisBeltIncrementDia,
        ToModeAutoSpeedPercent,
        UseRegulator,
        MaintTarget,
        UseMaint,
        PresenseSensorType,
    }

    public enum AutoTeachingParameter {
        SensorType,
        //AutoTeachingType,
        AutoTeachingSpeedX,
        AutoTeachingSpeedZ,
        AutoTeachingAccDecX,
        AutoTeachingAccDecZ,
        AutoTeachingDistX,
        AutoTeachingDistZ,
        AutoTeachingCompensationX,
        AutoTeachingCompensationZ,
        AutoTeachingFindSensorRangeX,
        AutoTeachingFindSensorRangeZ,
        AutoTeachingFindSensorSpeedX,
        AutoTeachingFindSensorSpeedZ,
        AutoTeachingFindSensorAccDecX,
        AutoTeachingFindSensorAccDecZ,
        AutoTeachingReflectorWidth,
        AutoTeachingReflectorHeight,
        AutoTeachingReflectorWidthErrorRange,
        AutoTeachingReflectorHeightErrorRange,
        AutoTeachingTargetSpeedX,
        AutoTeachingTargetSpeedZ,
        AutoTeachingTargetAccDecX,
        AutoTeachingTargetAccDecZ,
        DoubleStorageSensorCheck,
    }

    public enum SCARAParameter {
        RZLength,
        RXLength,
        RYLength
    }

    public enum TimerParameter {
        CIMTimerOver,
        StepTimerOver,
        PIOReadyTimerOver,
        HomeMoveTimerOver,
        IoTimer,
        EventTimer,
        AutoTeachingStepTimeOver,
    }

    public enum AxisParameter {
        AxisNumber,
        AutoSpeedPercent,
        MaxSpeed,
        MaxAccDec,
        JerkRatio,
        JogHighSpeedLimit,
        JogLowSpeedLimit,
        InchingLimit,
        ManualHighSpeed,
        ManualHighAccDec,
        ManualLowSpeed,
        ManualLowAccDec,
        MaxOverload,
        HomePositionRange,
        QuickStop,
        NormalStop,
        SlowStop,
        PosSensorByteAddr,
        PosSensorBitAddr,
        PosSensorEnabled,
        PosSensor2ByteAddr,
        PosSensor2BitAddr,
        PosSensor2Enabled,
        SoftwareLimitPositive,
        SoftwareLimitNegative,
    }

    public enum WMXParameterList {
        GearRatioNumerator,
        GearRatioDenominator,
        AbsoluteEncoderMode,
        AbsoluteEncoderHomeOffset,
        PosSetWidth,
        HomeType,
        HomeDirection,
        HomeSlowVelocity,
        HomeSlowAcc,
        HomeSlowDec,
        HomeFastVelocity,
        HomeFastAcc,
        HomeFastDec,
        HomeShiftVelocity,
        HomeShiftAcc,
        HomeShiftDec,
        HomeShiftDistance,
        LimitSwitchType,
        SoftLimitSwitchType,
        SoftLimitSwitchPosValue,
        SoftLimitSwitchNegValue,
        LimitDec,
        LinearInterpolationCalcMode,
        QuickStopDec,
    }

    public enum FullClosedParameterList {
        Axis,
        BarcodeStartAddress,
        BarcodeSize,
        BarcodeScale,
        HomeOffset,
        SpecInType,
        SpecInRange,
        SpecInTime,
        Error1_StartAddress,
        Error1_Bit,
        Error2_StartAddress,
        Error2_Bit,
        Error3_StartAddress,
        Error3_Bit,
        Error4_StartAddress,
        Error4_Bit,
        EMO1_StartAddress,
        EMO1_Bit,
        EMO2_StartAddress,
        EMO2_Bit,
        EMO3_StartAddress,
        EMO3_Bit,
        EMO4_StartAddress,
        EMO4_Bit,
        AlarmStopDecTimeSecond,
        StopDecTimeSecond,
        FollowingError,
        VelocityLimit,
        AccLimit,
        DecLimit,
        UseBarcodePositiveLimit,
        BarcodePositiveLimit,
        UseBarcodeNegativeLimit,
        BarcodeNegativeLimit,
        PGain,
        IGain,
    }

    public enum ParameterList {
        Motion,
        AutoTeaching,
        Scara,
        Timer,
        WMX,
        FullClosed,
    }

    public enum AxisList {
        X_Axis,
        Z_Axis,
        A_Axis,
        T_Axis,
    }

    public enum SyncTimeType {
        Year,
        Month,
        Day,
        Hour,
        Minute,
        Second,
    }

    public enum ForkType {
        Slide,
        Slide_NoTurn,
        SCARA,
    }

    public enum AxisStopMode {
        Quick,
        Normal,
        Slow,
    }

    public enum ZAxisBeltType {
        Normal,
        Drum
    }

    public enum AutoTeachingSensorType {
        Pick,
        Place
    }

    public enum SensorConditionType {
        Always,
        BeforeStepIn,
    }

    public enum InPlaceSensorType {
        Normal,
        Oven,
        DieBank,
    }

    //public enum AutoTeachingType {
    //    Absolute,
    //    Relative,
    //}

    public enum MaintTarget {
        HP,
        OP
    }

    public enum PresenseSensorType {
        AllTurn,
        LeftOrRight,
    }

    public partial class RackMasterMain {
        public partial class RackMasterParam {
            public class ScaraParameter {
                public int RZ_LENGTH;
                public int RX_LENGTH;
                public int RY_LENGTH;

                public ScaraParameter() {
                    RX_LENGTH = 220;
                    RY_LENGTH = 220;
                    RZ_LENGTH = 420;
                }
            }

            public class AxisParam {
                public int axisNumber;
                public float maxSpeed;
                public float maxAccDec;
                public float minAccDec;
                public float maxOverload;
                public float jerkRatio;
                public float autoSpeedPercent;
                public float homePositionRange;
                public float jogHighSpeedLimit;
                public float jogLowSpeedLimit;
                public float inchingLimit;
                public float manualHighSpeed;
                public float manualHighAccDec;
                public float manualLowSpeed;
                public float manualLowAccDec;
                public float quickStop;
                public float normalStop;
                public float slowStop;
                public int posSensorByteAddr;
                public int posSensorBitAddr;
                public bool posSensorEnabled;
                public int posSensor2ByteAddr;
                public int posSensor2BitAddr;
                public bool posSensor2Enabled;
                public float swLimitPositive;
                public float swLimitNegative;

                public AxisParam() {
                    axisNumber = 0;
                    maxSpeed = 20;
                    maxAccDec = 3;
                    maxOverload = 200;
                    jerkRatio = 70;
                    autoSpeedPercent = 50;
                    homePositionRange = 10;
                    jogHighSpeedLimit = 10;
                    jogLowSpeedLimit = 5;
                    inchingLimit = 500;
                    manualHighSpeed = 20;
                    manualHighAccDec = 3;
                    manualLowSpeed = 10;
                    manualLowAccDec = 3;
                    quickStop = 1;
                    normalStop = 2;
                    slowStop = 3;
                    posSensorByteAddr = 0;
                    posSensorBitAddr = 0;
                    posSensorEnabled = false;
                    posSensor2ByteAddr = 0;
                    posSensor2BitAddr = 0;
                    posSensor2Enabled = false;
                    swLimitPositive = 1000000;
                    swLimitNegative = 0;
                }
            }

            public class MotionParam {
                public ForkType forkType;
                public AxisStopMode stopMode;
                public float Z_OverridePercent;
                public float Z_OverrideFromUpDist; // Z축 Up 할 때
                public float Z_OverrideFromDownDist;  // Z축 Down 할 때
                public float Z_OverrideToUpDist;
                public float Z_OverrideToDownDist;
                public float turnCenterPosition;
                public float armHomePositionRange;
                public bool useInterpolation;
                public bool useFullClosed;
                public int distDetectSensor;
                public int Z_AutoHomingCount;
                public SensorConditionType presenseConditionType;
                public SensorConditionType inPlaceConditionType;
                public InPlaceSensorType inPlaceType;
                public ZAxisBeltType ZAxisBeltType;
                public double ZAxisBeltHomeOffset;
                public int ZAxisBeltFirstDia;
                public int ZAxisBeltDia;
                public float toModeAutoSpeedPercent;
                public bool useRegulator;
                public MaintTarget maintTarget;
                public bool useMaint;
                public PresenseSensorType presensType;

                public MotionParam() {
                    forkType = ForkType.Slide;
                    stopMode = AxisStopMode.Normal;
                    Z_OverridePercent = 3;
                    Z_OverrideFromUpDist = 15;
                    Z_OverrideFromDownDist = 15;
                    Z_OverrideToUpDist = 15;
                    Z_OverrideToDownDist = 15;
                    turnCenterPosition = 90;
                    armHomePositionRange = 10;
                    useInterpolation = false;
                    useFullClosed = false;
                    distDetectSensor = 0;
                    Z_AutoHomingCount = 0;
                    presenseConditionType = SensorConditionType.Always;
                    inPlaceConditionType = SensorConditionType.Always;
                    inPlaceType = InPlaceSensorType.Normal;
                    ZAxisBeltType = ZAxisBeltType.Normal;
                    ZAxisBeltHomeOffset = 0;
                    ZAxisBeltFirstDia = 270;
                    ZAxisBeltDia = 4;
                    toModeAutoSpeedPercent = 70;
                    useRegulator = false;
                    maintTarget = MaintTarget.HP;
                    useMaint = true;
                    presensType = PresenseSensorType.AllTurn;
                }
            }

            public class AutoTeachingParam {
                public AutoTeachingSensorType sensorType;
                //public AutoTeachingType autoTeachingType;
                public float autoTeachingSpeedX;
                public float autoTeachingSpeedZ;
                public float autoTeachingAccDecX;
                public float autoTeachingAccDecZ;
                public float autoTeachingDistX;
                public float autoTeachingDistZ;
                public float autoTeachingCompensationX;
                public float autoTeachingCompensationZ;
                public float autoTeachingFindSensorSpeedX;
                public float autoTeachingFindSensorSpeedZ;
                public float autoTeachingFindSensorAccDecX;
                public float autoTeachingFindSensorAccDecZ;
                public float autoTeachingFindSensorRangeX;
                public float autoTeachingFindSensorRangeZ;
                public float autoTeachingReflectorWidth;
                public float autoTeachingReflectorHeight;
                public float autoTeachingReflectorErrorRangeWidth;
                public float autoTeachingReflectorErrorRangeHeight;
                public float autoTeachingTargetSpeedX;
                public float autoTeachingTargetSpeedZ;
                public float autoTeachingTargetAccDecX;
                public float autoTeachingTargetAccDecZ;
                public bool doubleStorageSensorCheck;

                public AutoTeachingParam() {
                    sensorType = AutoTeachingSensorType.Pick;
                    //autoTeachingType = AutoTeachingType.Absolute;
                    autoTeachingSpeedX = 10;
                    autoTeachingSpeedZ = 10;
                    autoTeachingAccDecX = 1;
                    autoTeachingAccDecZ = 1;
                    autoTeachingDistX = 15;
                    autoTeachingDistZ = 15;
                    autoTeachingCompensationX = 0;
                    autoTeachingCompensationZ = 0;
                    autoTeachingFindSensorSpeedX = 15;
                    autoTeachingFindSensorSpeedZ = 15;
                    autoTeachingFindSensorAccDecX = 1;
                    autoTeachingFindSensorAccDecZ = 1;
                    autoTeachingFindSensorRangeX = 10;
                    autoTeachingFindSensorRangeZ = 10;
                    autoTeachingReflectorWidth = 15;
                    autoTeachingReflectorHeight = 15;
                    autoTeachingReflectorErrorRangeWidth = 5;
                    autoTeachingReflectorErrorRangeHeight = 5;
                    autoTeachingTargetSpeedX = 100;
                    autoTeachingTargetSpeedZ = 100;
                    autoTeachingTargetAccDecX = 1;
                    autoTeachingTargetAccDecZ = 1;
                    doubleStorageSensorCheck = false;
                }
            }

            public class TimerParameter {
                public int CIM_TIMEOVER;
                public int STEP_TEIMOVER;
                public int PIO_READY_TIMOVER;
                public int HOME_MOVE_TIMEOVER;
                public int IO_TIMER;
                public int EVENT_TIMEROVER;
                public int AUTO_TEACHING_STEP_TIMEOVER;

                public TimerParameter() {
                    CIM_TIMEOVER = 60000;
                    STEP_TEIMOVER = 70000;
                    PIO_READY_TIMOVER = 30000;
                    HOME_MOVE_TIMEOVER = 60000;
                    IO_TIMER = 100;
                    EVENT_TIMEROVER = 3000;
                    AUTO_TEACHING_STEP_TIMEOVER = 15000;
                }
            }
            /// <summary>
            /// WMX 파라미터 파일 로드
            /// </summary>
            /// <returns></returns>
            public bool LoadWMXParameterFile() {
                if (Ini.IsFileExist(WMXParameterPath)) {
                    byte[] buffer = new byte[Ini.MAX_BUFFER_SIZE];
                    buffer = Ini.GetSectionNames(WMXParameterPath);
                    string allSections = System.Text.Encoding.Default.GetString(buffer);
                    string[] sectionNames = allSections.Split('\0');
                    foreach (string section in sectionNames) {
                        if (section == "")
                            continue;

                        foreach (AxisList axisSection in Enum.GetValues(typeof(AxisList))) {
                            try {
                                int axisIndex = GetAxisNumber(axisSection);
                                WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                                m_axis.GetCurrentWMXParameter(axisIndex, ref param);

                                if (section.Equals($"{axisSection}")) {
                                    foreach(WMXParameterList axisKey in Enum.GetValues(typeof(WMXParameterList))) {
                                        switch (axisKey) {
                                            case WMXParameterList.GearRatioNumerator:
                                                param.m_gearRatioNum = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.GearRatioDenominator:
                                                param.m_gearRatioDen = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.AbsoluteEncoderMode:
                                                Boolean.TryParse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath), out param.m_absEncoderMode);
                                                break;

                                            case WMXParameterList.AbsoluteEncoderHomeOffset:
                                                param.m_absEncoderHomeOffset = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.PosSetWidth:
                                                param.m_posSetWidth = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeType:
                                                param.m_homeType = (WMXParam.m_homeType)Enum.Parse(typeof(WMXParam.m_homeType), Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeDirection:
                                                param.m_homeDirection = (WMXParam.m_homeDirection)Enum.Parse(typeof(WMXParam.m_homeDirection), Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeSlowVelocity:
                                                param.m_homeSlowVelocity = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeSlowAcc:
                                                param.m_homeSlowAcc = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeSlowDec:
                                                param.m_homeSlowDec = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeFastVelocity:
                                                param.m_homeFastVelocity = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeFastAcc:
                                                param.m_homeFastAcc = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeFastDec:
                                                param.m_homeFastDec = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeShiftVelocity:
                                                param.m_homeShiftVelocity = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeShiftAcc:
                                                param.m_homeShiftAcc = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeShiftDec:
                                                param.m_homeShiftDec = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.HomeShiftDistance:
                                                param.m_homeShiftDistance = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.LimitSwitchType:
                                                param.m_limitSwitchType = (WMXParam.m_limitSwitchType)Enum.Parse(typeof(WMXParam.m_limitSwitchType), Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.SoftLimitSwitchType:
                                                param.m_softLimitSwitchType = (WMXParam.m_limitSwitchType)Enum.Parse(typeof(WMXParam.m_limitSwitchType), Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.SoftLimitSwitchPosValue:
                                                param.m_softLimitPosValue = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.SoftLimitSwitchNegValue:
                                                param.m_softLimitNegValue = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.LimitDec:
                                                param.m_limitDec = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.LinearInterpolationCalcMode:
                                                param.m_linintplCalcMode = (WMXParam.m_linIntplCalcMode)Enum.Parse(typeof(WMXParam.m_linIntplCalcMode), Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;

                                            case WMXParameterList.QuickStopDec:
                                                param.m_quickStopDecel = double.Parse(Ini.GetValueString(section, $"{axisKey}", WMXParameterPath));
                                                break;
                                        }
                                    }

                                    if(!m_axis.SetWMXParameter(axisIndex, param)){
                                        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, Log.LogMessage_Main.WMX_ParameterSaveFail));
                                        return false;
                                    }
                                }
                            }
                            catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.WMX_ParameterLoadFail, ex));
                                return false;
                            }
                        }
                    }
                    return true;
                }
                else {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.WMX_ParameterLoadFail));
                    return false;
                }
            }
            /// <summary>
            /// Absolute 타입인 경우 현재 포지션을 Home Shift Distance와 Encoder Home Offset 값을 계산하여 확인
            /// </summary>
            public void SetCommandPoisitionHomeShift() {
                m_axis.UpdateMotorStatus();
                foreach (AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    int axisIndex = GetAxisNumber(axis);
                    WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                    m_axis.GetCurrentWMXParameter(axisIndex, ref param);

                    double pulsePerUnit = param.m_gearRatioNum / param.m_gearRatioDen;
                    double EncoderCMDPos = m_axis.GetAxisStatus(axisIndex).m_cmdEncoder;
                    double AbsHomeOffset = param.m_absEncoderHomeOffset;
                    double HomeShiftDist = param.m_homeShiftDistance;
                    int WMXDirection = param.m_motorDirection == WMXParam.m_motorDirection.Positive ? 1 : -1;
                    double curPositionCalculated = (((EncoderCMDPos - AbsHomeOffset) * WMXDirection) / pulsePerUnit) + HomeShiftDist;

                    if (m_axis.GetAxisStatus(GetAxisNumber(axis)).m_cmdPos >= (curPositionCalculated + 10) ||
                        m_axis.GetAxisStatus(GetAxisNumber(axis)).m_cmdPos <= (curPositionCalculated - 10)) {
                        m_axis.SetAxisCommandPos(GetAxisNumber(axis), curPositionCalculated);
                    }

                    m_axis.SetHomeDoneFlag(GetAxisNumber(axis), true);
                }
            }
            /// <summary>
            /// Setting 파라미터 파일 로드
            /// </summary>
            /// <returns></returns>
            public bool LoadSettingParameterFile() {
                if (Ini.IsFileExist(SettingParameterPath)) {
                    byte[] buffer = new byte[Ini.MAX_BUFFER_SIZE];
                    buffer = Ini.GetSectionNames(SettingParameterPath);
                    string allSections = System.Text.Encoding.Default.GetString(buffer);
                    string[] sectionNames = allSections.Split('\0');

                    foreach (string section in sectionNames) {
                        if (section == "")
                            continue;

                        if (section.Equals($"{ParameterList.Motion}")) {
                            try {
                                foreach (MotionParameter motionParam in Enum.GetValues(typeof(MotionParameter))) {
                                    switch (motionParam) {
                                        case MotionParameter.ForkType:
                                            m_motionParam.forkType = (ForkType)Enum.Parse(typeof(ForkType), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.StopMode:
                                            m_motionParam.stopMode = (AxisStopMode)Enum.Parse(typeof(AxisStopMode), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZOverridePercent:
                                            m_motionParam.Z_OverridePercent = float.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZOverrideFromUpDistance:
                                            m_motionParam.Z_OverrideFromUpDist = float.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZOverrideFromDownDistance:
                                            m_motionParam.Z_OverrideFromDownDist = float.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZOverrideToUpDistance:
                                            m_motionParam.Z_OverrideToUpDist = float.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZOverrideToDownDistance:
                                            m_motionParam.Z_OverrideToDownDist = float.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.TurnCenterPosition:
                                            m_motionParam.turnCenterPosition = float.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ArmHomePositionRange:
                                            m_motionParam.armHomePositionRange = float.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.UseInterpolation:
                                            Boolean.TryParse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath), out m_motionParam.useInterpolation);
                                            break;

                                        case MotionParameter.UseFullclosed:
                                            Boolean.TryParse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath), out m_motionParam.useFullClosed);
                                            break;

                                        case MotionParameter.DistanceDetectSensor:
                                            m_motionParam.distDetectSensor = int.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZAutoHomingCount:
                                            m_motionParam.Z_AutoHomingCount = int.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.PresenseSensorConditionType:
                                            m_motionParam.presenseConditionType = (SensorConditionType)Enum.Parse(typeof(SensorConditionType), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.InPlaceSensorConditionType:
                                            m_motionParam.inPlaceConditionType = (SensorConditionType)Enum.Parse(typeof(SensorConditionType), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.InPlaceSensorType:
                                            m_motionParam.inPlaceType = (InPlaceSensorType)Enum.Parse(typeof(InPlaceSensorType), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZAxisBeltType:
                                            m_motionParam.ZAxisBeltType = (ZAxisBeltType)Enum.Parse(typeof(ZAxisBeltType), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZAxisBeltHomeOffset:
                                            m_motionParam.ZAxisBeltHomeOffset = double.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZAxisBeltFirstDia:
                                            m_motionParam.ZAxisBeltFirstDia = int.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ZAxisBeltIncrementDia:
                                            m_motionParam.ZAxisBeltDia = int.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.ToModeAutoSpeedPercent:
                                            m_motionParam.toModeAutoSpeedPercent = int.Parse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.UseRegulator:
                                            Boolean.TryParse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath), out m_motionParam.useRegulator);
                                            break;

                                        case MotionParameter.MaintTarget:
                                            m_motionParam.maintTarget = (MaintTarget)Enum.Parse(typeof(MaintTarget), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;

                                        case MotionParameter.UseMaint:
                                            Boolean.TryParse(Ini.GetValueString(section, $"{motionParam}", SettingParameterPath), out m_motionParam.useMaint);
                                            break;

                                        case MotionParameter.PresenseSensorType:
                                            m_motionParam.presensType = (PresenseSensorType)Enum.Parse(typeof(PresenseSensorType), Ini.GetValueString(section, $"{motionParam}", SettingParameterPath));
                                            break;
                                    }
                                }
                            } catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "Motion Parameter Load Fail", ex));
                            }
                        }
                        else if (section.Equals($"{ParameterList.AutoTeaching}")) {
                            try {
                                foreach (AutoTeachingParameter autoTeachingParam in Enum.GetValues(typeof(AutoTeachingParameter))) {
                                    switch (autoTeachingParam) {
                                        case AutoTeachingParameter.SensorType:
                                            m_autoTeachingParam.sensorType = (AutoTeachingSensorType)Enum.Parse(typeof(AutoTeachingSensorType), Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        //case AutoTeachingParameter.AutoTeachingType:
                                        //    m_autoTeachingParam.autoTeachingType = (AutoTeachingType)Enum.Parse(typeof(AutoTeachingType), Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                        //    break;

                                        case AutoTeachingParameter.AutoTeachingSpeedX:
                                            m_autoTeachingParam.autoTeachingSpeedX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingSpeedZ:
                                            m_autoTeachingParam.autoTeachingSpeedZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingAccDecX:
                                            m_autoTeachingParam.autoTeachingAccDecX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingAccDecZ:
                                            m_autoTeachingParam.autoTeachingAccDecZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingDistX:
                                            m_autoTeachingParam.autoTeachingDistX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingDistZ:
                                            m_autoTeachingParam.autoTeachingDistZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingCompensationX:
                                            m_autoTeachingParam.autoTeachingCompensationX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingCompensationZ:
                                            m_autoTeachingParam.autoTeachingCompensationZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorSpeedX:
                                            m_autoTeachingParam.autoTeachingFindSensorSpeedX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorSpeedZ:
                                            m_autoTeachingParam.autoTeachingFindSensorSpeedZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorAccDecX:
                                            m_autoTeachingParam.autoTeachingFindSensorAccDecX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorAccDecZ:
                                            m_autoTeachingParam.autoTeachingFindSensorAccDecZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorRangeX:
                                            m_autoTeachingParam.autoTeachingFindSensorRangeX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorRangeZ:
                                            m_autoTeachingParam.autoTeachingFindSensorRangeZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorWidth:
                                            m_autoTeachingParam.autoTeachingReflectorWidth = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorHeight:
                                            m_autoTeachingParam.autoTeachingReflectorHeight = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorWidthErrorRange:
                                            m_autoTeachingParam.autoTeachingReflectorErrorRangeWidth = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorHeightErrorRange:
                                            m_autoTeachingParam.autoTeachingReflectorErrorRangeHeight = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetSpeedX:
                                            m_autoTeachingParam.autoTeachingTargetSpeedX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetSpeedZ:
                                            m_autoTeachingParam.autoTeachingTargetSpeedZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetAccDecX:
                                            m_autoTeachingParam.autoTeachingTargetAccDecX = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetAccDecZ:
                                            m_autoTeachingParam.autoTeachingTargetAccDecZ = float.Parse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath));
                                            break;

                                        case AutoTeachingParameter.DoubleStorageSensorCheck:
                                            Boolean.TryParse(Ini.GetValueString(section, $"{autoTeachingParam}", SettingParameterPath), out m_autoTeachingParam.doubleStorageSensorCheck);
                                            break;
                                    }
                                }
                            } catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "Auto Teaching Parameter Load Fail", ex));
                            }
                        }
                        else if (section.Equals($"{ParameterList.Scara}")) {
                            try {
                                foreach (SCARAParameter kinematics in Enum.GetValues(typeof(SCARAParameter))) {
                                    switch (kinematics) {
                                        case SCARAParameter.RZLength:
                                            m_scara.RZ_LENGTH = Ini.GetValueInt(section, $"{kinematics}", SettingParameterPath);
                                            break;

                                        case SCARAParameter.RXLength:
                                            m_scara.RX_LENGTH = Ini.GetValueInt(section, $"{kinematics}", SettingParameterPath);
                                            break;

                                        case SCARAParameter.RYLength:
                                            m_scara.RY_LENGTH = Ini.GetValueInt(section, $"{kinematics}", SettingParameterPath);
                                            break;
                                    }
                                }
                            } catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "Scara Parameter Load Fail", ex));
                            }
                        }
                        else if (section.Equals($"{ParameterList.Timer}")) {
                            try {
                                foreach (PART.TimerParameter stepTime in Enum.GetValues(typeof(PART.TimerParameter))) {
                                    switch (stepTime) {
                                        case PART.TimerParameter.CIMTimerOver:
                                            m_timerParam.CIM_TIMEOVER = Ini.GetValueInt(section, $"{stepTime}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.StepTimerOver:
                                            m_timerParam.STEP_TEIMOVER = Ini.GetValueInt(section, $"{stepTime}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.PIOReadyTimerOver:
                                            m_timerParam.PIO_READY_TIMOVER = Ini.GetValueInt(section, $"{stepTime}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.HomeMoveTimerOver:
                                            m_timerParam.HOME_MOVE_TIMEOVER = Ini.GetValueInt(section, $"{stepTime}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.EventTimer:
                                            m_timerParam.EVENT_TIMEROVER = Ini.GetValueInt(section, $"{stepTime}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.AutoTeachingStepTimeOver:
                                            m_timerParam.AUTO_TEACHING_STEP_TIMEOVER = Ini.GetValueInt(section, $"{stepTime}", SettingParameterPath);
                                            break;
                                    }
                                }
                            } catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "Timer Parameter Load Fail", ex));
                            }
                        }
                        else if (section.Equals($"{ParameterList.FullClosed}")) {
                            BarcodeParameter barcodeParam = new BarcodeParameter();
                            BarcodeSafetyParameter safetyParam = new BarcodeSafetyParameter();
                            try {
                                foreach (FullClosedParameterList barcode in Enum.GetValues(typeof(FullClosedParameterList))) {
                                    switch (barcode) {
                                        case FullClosedParameterList.Axis:
                                            barcodeParam.m_axis = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.BarcodeStartAddress:
                                            barcodeParam.m_startAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.BarcodeSize:
                                            barcodeParam.m_size = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.BarcodeScale:
                                            barcodeParam.m_barcodeScale = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.HomeOffset:
                                            barcodeParam.m_homeOffset = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.SpecInType:
                                            barcodeParam.m_specInType = (BarcodeSpecInType)Enum.Parse(typeof(BarcodeSpecInType), Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.SpecInRange:
                                            barcodeParam.m_specInRange = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.SpecInTime:
                                            barcodeParam.m_specInTimeSec = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error1_StartAddress:
                                            safetyParam.m_error1_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error1_Bit:
                                            safetyParam.m_error1_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error2_StartAddress:
                                            safetyParam.m_error2_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error2_Bit:
                                            safetyParam.m_error2_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error3_StartAddress:
                                            safetyParam.m_error3_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error3_Bit:
                                            safetyParam.m_error3_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error4_StartAddress:
                                            safetyParam.m_error4_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.Error4_Bit:
                                            safetyParam.m_error4_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO1_StartAddress:
                                            safetyParam.m_EMO1_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO1_Bit:
                                            safetyParam.m_EMO1_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO2_StartAddress:
                                            safetyParam.m_EMO2_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO2_Bit:
                                            safetyParam.m_EMO2_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO3_StartAddress:
                                            safetyParam.m_EMO3_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO3_Bit:
                                            safetyParam.m_EMO3_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO4_StartAddress:
                                            safetyParam.m_EMO4_StartAddr = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.EMO4_Bit:
                                            safetyParam.m_EMO4_Bit = int.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.AlarmStopDecTimeSecond:
                                            safetyParam.m_alarmStopDecTimeSec = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.StopDecTimeSecond:
                                            safetyParam.m_stopDecTimeSec = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.FollowingError:
                                            safetyParam.m_followingError = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.VelocityLimit:
                                            safetyParam.m_velocityLimit = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.AccLimit:
                                            safetyParam.m_accLimit = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.DecLimit:
                                            safetyParam.m_decLimit = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.UseBarcodePositiveLimit:
                                            Boolean.TryParse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath), out safetyParam.m_useBarcodePositiveLimit);
                                            break;

                                        case FullClosedParameterList.BarcodePositiveLimit:
                                            safetyParam.m_barcodePositiveLimit = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.UseBarcodeNegativeLimit:
                                            Boolean.TryParse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath), out safetyParam.m_useBarcodeNegativeLimit);
                                            break;

                                        case FullClosedParameterList.BarcodeNegativeLimit:
                                            safetyParam.m_barcodeNegativeLimit = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.PGain:
                                            m_PGain = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;

                                        case FullClosedParameterList.IGain:
                                            m_IGain = double.Parse(Ini.GetValueString(section, $"{barcode}", SettingParameterPath));
                                            break;
                                    }
                                }

                                if (GetMotionParam().useFullClosed) {
                                    m_axis.SetFullClosedParameter(barcodeParam);
                                    m_axis.SetFullClosedSafetyParameter(safetyParam);
                                }
                            } catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, "Full Closed Parameter Load Fail", ex));
                            }
                        }
                    }
                    return true;
                }
                else {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterFileNotFound));
                    return false;
                }
            }
            /// <summary>
            /// Axis 파라미터 파일 로드
            /// </summary>
            /// <returns></returns>
            public bool LoadAxisParameterFile() {
                if (Ini.IsFileExist(AxisParameterPath)) {
                    byte[] buffer = new byte[Ini.MAX_BUFFER_SIZE];
                    buffer = Ini.GetSectionNames(AxisParameterPath);
                    string allSections = System.Text.Encoding.Default.GetString(buffer);
                    string[] sectionNames = allSections.Split('\0');
                    foreach (string section in sectionNames) {
                        if (section == "")
                            continue;

                        foreach (AxisList axisSection in Enum.GetValues(typeof(AxisList))) {
                            try {
                                int arrayIndex = 0;
                                if (axisSection == AxisList.X_Axis)
                                    arrayIndex = (int)AxisList.X_Axis;
                                else if (axisSection == AxisList.Z_Axis)
                                    arrayIndex = (int)AxisList.Z_Axis;
                                else if (axisSection == AxisList.A_Axis)
                                    arrayIndex = (int)AxisList.A_Axis;
                                else if (axisSection == AxisList.T_Axis)
                                    arrayIndex = (int)AxisList.T_Axis;

                                if (section.Equals($"{axisSection}")) {
                                    foreach (AxisParameter axisKey in Enum.GetValues(typeof(AxisParameter))) {
                                        switch (axisKey) {
                                            case AxisParameter.AxisNumber:
                                                m_axisParam[arrayIndex].axisNumber = Ini.GetValueInt(section, $"{axisKey}", AxisParameterPath);
                                                break;

                                            case AxisParameter.MaxSpeed:
                                                m_axisParam[arrayIndex].maxSpeed = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.MaxAccDec:
                                                m_axisParam[arrayIndex].maxAccDec = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.MaxOverload:
                                                m_axisParam[arrayIndex].maxOverload = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.JerkRatio:
                                                m_axisParam[arrayIndex].jerkRatio = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.AutoSpeedPercent:
                                                m_axisParam[arrayIndex].autoSpeedPercent = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.HomePositionRange:
                                                m_axisParam[arrayIndex].homePositionRange = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.JogHighSpeedLimit:
                                                m_axisParam[arrayIndex].jogHighSpeedLimit = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.JogLowSpeedLimit:
                                                m_axisParam[arrayIndex].jogLowSpeedLimit = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.InchingLimit:
                                                m_axisParam[arrayIndex].inchingLimit = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.ManualHighSpeed:
                                                m_axisParam[arrayIndex].manualHighSpeed = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.ManualHighAccDec:
                                                m_axisParam[arrayIndex].manualHighAccDec = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.ManualLowSpeed:
                                                m_axisParam[arrayIndex].manualLowSpeed = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.ManualLowAccDec:
                                                m_axisParam[arrayIndex].manualLowAccDec = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.QuickStop:
                                                m_axisParam[arrayIndex].quickStop = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.NormalStop:
                                                m_axisParam[arrayIndex].normalStop = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.SlowStop:
                                                m_axisParam[arrayIndex].slowStop = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.PosSensorByteAddr:
                                                m_axisParam[arrayIndex].posSensorByteAddr = int.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.PosSensorBitAddr:
                                                m_axisParam[arrayIndex].posSensorBitAddr = int.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.PosSensorEnabled:
                                                Boolean.TryParse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath), out m_axisParam[arrayIndex].posSensorEnabled);
                                                break;

                                            case AxisParameter.PosSensor2ByteAddr:
                                                m_axisParam[arrayIndex].posSensor2ByteAddr = int.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.PosSensor2BitAddr:
                                                m_axisParam[arrayIndex].posSensor2BitAddr = int.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.PosSensor2Enabled:
                                                Boolean.TryParse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath), out m_axisParam[arrayIndex].posSensor2Enabled);
                                                break;

                                            case AxisParameter.SoftwareLimitPositive:
                                                m_axisParam[arrayIndex].swLimitPositive = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;

                                            case AxisParameter.SoftwareLimitNegative:
                                                m_axisParam[arrayIndex].swLimitNegative = float.Parse(Ini.GetValueString(section, $"{axisKey}", AxisParameterPath));
                                                break;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) {
                                Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.AxisParameterLoadFail, ex));
                            }
                        }
                    }
                    return true;
                }
                else {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.AxisParameterFileNotFound));
                    return false;
                }
            }
            /// <summary>
            /// Setting 파라미터 저장
            /// </summary>
            /// <returns></returns>
            public bool SaveSettingParameterFile() {
                if (!Ini.IsFileExist(SettingParameterPath)) {
                    Ini.CreateIniFile(SettingParameterPath, ManagedFileInfo.SettingsDirectory);
                }

                try {
                    foreach (ParameterList section in Enum.GetValues(typeof(ParameterList))) {
                        switch (section) {
                            case ParameterList.Motion:
                                foreach (MotionParameter key in Enum.GetValues(typeof(MotionParameter))) {
                                    switch (key) {
                                        case MotionParameter.ForkType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.forkType}", SettingParameterPath);
                                            break;

                                        case MotionParameter.StopMode:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.stopMode}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZOverridePercent:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.Z_OverridePercent}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZOverrideFromUpDistance:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.Z_OverrideFromUpDist}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZOverrideFromDownDistance:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.Z_OverrideFromDownDist}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZOverrideToUpDistance:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.Z_OverrideToUpDist}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZOverrideToDownDistance:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.Z_OverrideToDownDist}", SettingParameterPath);
                                            break;

                                        case MotionParameter.TurnCenterPosition:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.turnCenterPosition}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ArmHomePositionRange:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.armHomePositionRange}", SettingParameterPath);
                                            break;

                                        case MotionParameter.UseInterpolation:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.useInterpolation}", SettingParameterPath);
                                            break;

                                        case MotionParameter.UseFullclosed:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.useFullClosed}", SettingParameterPath);
                                            break;

                                        case MotionParameter.DistanceDetectSensor:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.distDetectSensor}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZAutoHomingCount:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.Z_AutoHomingCount}", SettingParameterPath);
                                            break;

                                        case MotionParameter.PresenseSensorConditionType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.presenseConditionType}", SettingParameterPath);
                                            break;

                                        case MotionParameter.InPlaceSensorConditionType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.inPlaceConditionType}", SettingParameterPath);
                                            break;

                                        case MotionParameter.InPlaceSensorType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.inPlaceType}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZAxisBeltType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.ZAxisBeltType}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZAxisBeltHomeOffset:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.ZAxisBeltHomeOffset}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZAxisBeltFirstDia:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.ZAxisBeltFirstDia}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ZAxisBeltIncrementDia:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.ZAxisBeltDia}", SettingParameterPath);
                                            break;

                                        case MotionParameter.ToModeAutoSpeedPercent:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.toModeAutoSpeedPercent}", SettingParameterPath);
                                            break;

                                        case MotionParameter.UseRegulator:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.useRegulator}", SettingParameterPath);
                                            break;

                                        case MotionParameter.MaintTarget:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.maintTarget}", SettingParameterPath);
                                            break;

                                        case MotionParameter.UseMaint:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.useMaint}", SettingParameterPath);
                                            break;

                                        case MotionParameter.PresenseSensorType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_motionParam.presensType}", SettingParameterPath);
                                            break;
                                    }
                                }
                                break;

                            case ParameterList.AutoTeaching:
                                foreach (AutoTeachingParameter key in Enum.GetValues(typeof(AutoTeachingParameter))) {
                                    switch (key) {
                                        case AutoTeachingParameter.SensorType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.sensorType}", SettingParameterPath);
                                            break;

                                        //case AutoTeachingParameter.AutoTeachingType:
                                        //    Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingType}", SettingParameterPath);
                                        //    break;

                                        case AutoTeachingParameter.AutoTeachingSpeedX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingSpeedX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingSpeedZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingSpeedZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingAccDecX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingAccDecX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingAccDecZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingAccDecZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingDistX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingDistX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingDistZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingDistZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingCompensationX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingCompensationX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingCompensationZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingCompensationZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorRangeX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingFindSensorRangeX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorRangeZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingFindSensorRangeZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorSpeedX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingFindSensorSpeedX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorSpeedZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingFindSensorSpeedZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorAccDecX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingFindSensorAccDecX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingFindSensorAccDecZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingFindSensorAccDecZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorWidth:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingReflectorWidth}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorHeight:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingReflectorHeight}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorWidthErrorRange:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingReflectorErrorRangeWidth}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingReflectorHeightErrorRange:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingReflectorErrorRangeHeight}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetSpeedX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingTargetSpeedX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetSpeedZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingTargetSpeedZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetAccDecX:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingTargetAccDecX}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.AutoTeachingTargetAccDecZ:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.autoTeachingTargetAccDecZ}", SettingParameterPath);
                                            break;

                                        case AutoTeachingParameter.DoubleStorageSensorCheck:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_autoTeachingParam.doubleStorageSensorCheck}", SettingParameterPath);
                                            break;
                                    }
                                }
                                break;

                            case ParameterList.Scara:
                                foreach (SCARAParameter key in Enum.GetValues(typeof(SCARAParameter))) {
                                    switch (key) {
                                        case SCARAParameter.RZLength:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_scara.RZ_LENGTH}", SettingParameterPath);
                                            break;

                                        case SCARAParameter.RXLength:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_scara.RX_LENGTH}", SettingParameterPath);
                                            break;

                                        case SCARAParameter.RYLength:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_scara.RY_LENGTH}", SettingParameterPath);
                                            break;
                                    }
                                }
                                break;

                            case ParameterList.Timer:
                                foreach (PART.TimerParameter key in Enum.GetValues(typeof(PART.TimerParameter))) {
                                    switch (key) {
                                        case PART.TimerParameter.CIMTimerOver:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_timerParam.CIM_TIMEOVER}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.StepTimerOver:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_timerParam.STEP_TEIMOVER}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.PIOReadyTimerOver:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_timerParam.PIO_READY_TIMOVER}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.HomeMoveTimerOver:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_timerParam.HOME_MOVE_TIMEOVER}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.EventTimer:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_timerParam.EVENT_TIMEROVER}", SettingParameterPath);
                                            break;

                                        case PART.TimerParameter.AutoTeachingStepTimeOver:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_timerParam.AUTO_TEACHING_STEP_TIMEOVER}", SettingParameterPath);
                                            break;
                                    }
                                }
                                break;

                            case ParameterList.FullClosed:
                                foreach (FullClosedParameterList key in Enum.GetValues(typeof(FullClosedParameterList))) {
                                    switch (key) {
                                        case FullClosedParameterList.Axis:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_axis}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.BarcodeStartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_startAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.BarcodeSize:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_size}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.BarcodeScale:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_barcodeScale}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.HomeOffset:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_homeOffset}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.SpecInType:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_specInType}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.SpecInRange:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_specInRange}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.SpecInTime:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedParameter().m_specInTimeSec}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error1_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error1_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error1_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error1_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error2_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error2_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error2_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error2_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error3_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error3_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error3_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error3_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error4_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error4_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.Error4_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_error4_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO1_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO1_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO1_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO1_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO2_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO2_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO2_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO2_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO3_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO3_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO3_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO3_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO4_StartAddress:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO4_StartAddr}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.EMO4_Bit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_EMO4_Bit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.AlarmStopDecTimeSecond:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_alarmStopDecTimeSec}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.StopDecTimeSecond:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_stopDecTimeSec}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.FollowingError:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_followingError}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.VelocityLimit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_velocityLimit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.AccLimit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_accLimit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.DecLimit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_decLimit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.UseBarcodePositiveLimit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_useBarcodePositiveLimit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.BarcodePositiveLimit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_barcodePositiveLimit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.UseBarcodeNegativeLimit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_useBarcodeNegativeLimit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.BarcodeNegativeLimit:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_axis.GetFullClosedSafetyParameter().m_barcodeNegativeLimit}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.PGain:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_PGain}", SettingParameterPath);
                                            break;

                                        case FullClosedParameterList.IGain:
                                            Ini.SetValueString($"{section}", $"{key}", $"{m_IGain}", SettingParameterPath);
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveSuccess));
                    m_isSettingSuccess_RM = true;
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail, ex));
                    m_isSettingSuccess_RM = false;
                    return false;
                }
            }
            /// <summary>
            /// Axis 파라미터 저장
            /// </summary>
            /// <returns></returns>
            public bool SaveAxisParameterFile() {
                if (!Ini.IsFileExist(AxisParameterPath)) {
                    Ini.CreateIniFile(AxisParameterPath, ManagedFileInfo.SettingsDirectory);
                }

                try {
                    foreach (AxisList section in Enum.GetValues(typeof(AxisList))) {
                        int arrayIndex = 0;
                        if (section == AxisList.X_Axis)
                            arrayIndex = (int)AxisList.X_Axis;
                        else if (section == AxisList.Z_Axis)
                            arrayIndex = (int)AxisList.Z_Axis;
                        else if (section == AxisList.A_Axis)
                            arrayIndex = (int)AxisList.A_Axis;
                        else if (section == AxisList.T_Axis)
                            arrayIndex = (int)AxisList.T_Axis;

                        foreach (AxisParameter key in Enum.GetValues(typeof(AxisParameter))) {
                            switch (key) {
                                case AxisParameter.AxisNumber:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].axisNumber}", AxisParameterPath);
                                    break;

                                case AxisParameter.AutoSpeedPercent:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].autoSpeedPercent}", AxisParameterPath);
                                    break;

                                case AxisParameter.MaxSpeed:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].maxSpeed}", AxisParameterPath);
                                    break;

                                case AxisParameter.MaxAccDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].maxAccDec}", AxisParameterPath);
                                    break;

                                case AxisParameter.JerkRatio:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].jerkRatio}", AxisParameterPath);
                                    break;

                                case AxisParameter.JogHighSpeedLimit:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].jogHighSpeedLimit}", AxisParameterPath);
                                    break;

                                case AxisParameter.JogLowSpeedLimit:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].jogLowSpeedLimit}", AxisParameterPath);
                                    break;

                                case AxisParameter.InchingLimit:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].inchingLimit}", AxisParameterPath);
                                    break;

                                case AxisParameter.ManualHighSpeed:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].manualHighSpeed}", AxisParameterPath);
                                    break;

                                case AxisParameter.ManualHighAccDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].manualHighAccDec}", AxisParameterPath);
                                    break;

                                case AxisParameter.ManualLowSpeed:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].manualLowSpeed}", AxisParameterPath);
                                    break;

                                case AxisParameter.ManualLowAccDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].manualLowAccDec}", AxisParameterPath);
                                    break;

                                case AxisParameter.MaxOverload:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].maxOverload}", AxisParameterPath);
                                    break;

                                case AxisParameter.HomePositionRange:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].homePositionRange}", AxisParameterPath);
                                    break;

                                case AxisParameter.QuickStop:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].quickStop}", AxisParameterPath);
                                    break;

                                case AxisParameter.NormalStop:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].normalStop}", AxisParameterPath);
                                    break;

                                case AxisParameter.SlowStop:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].slowStop}", AxisParameterPath);
                                    break;

                                case AxisParameter.PosSensorByteAddr:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].posSensorByteAddr}", AxisParameterPath);
                                    break;

                                case AxisParameter.PosSensorBitAddr:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].posSensorBitAddr}", AxisParameterPath);
                                    break;

                                case AxisParameter.PosSensorEnabled:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].posSensorEnabled}", AxisParameterPath);
                                    break;

                                case AxisParameter.PosSensor2ByteAddr:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].posSensor2ByteAddr}", AxisParameterPath);
                                    break;

                                case AxisParameter.PosSensor2BitAddr:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].posSensor2BitAddr}", AxisParameterPath);
                                    break;

                                case AxisParameter.PosSensor2Enabled:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].posSensor2Enabled}", AxisParameterPath);
                                    break;

                                case AxisParameter.SoftwareLimitPositive:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].swLimitPositive}", AxisParameterPath);
                                    SaveSoftLimitPositiveValue(section);
                                    break;

                                case AxisParameter.SoftwareLimitNegative:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_axisParam[arrayIndex].swLimitNegative}", AxisParameterPath);
                                    SaveSoftLimitNegativeValue(section);
                                    break;
                            }
                        }
                    }
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.AxisParameterSaveSuccess));
                    m_isSettingSuccess_Axis = true;
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.AxisParameterSaveFail, ex));
                    m_isSettingSuccess_Axis = false;
                    return false;
                }
            }
            /// <summary>
            /// WMX 파라미터 저장
            /// </summary>
            /// <returns></returns>
            public bool SaveWMXParameter() {
                if (!Ini.IsFileExist(WMXParameterPath)) {
                    Ini.CreateIniFile(WMXParameterPath, ManagedFileInfo.SettingsDirectory);
                }

                try {
                    foreach (AxisList section in Enum.GetValues(typeof(AxisList))) {
                        int axisIndex = GetAxisNumber(section);
                        WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                        m_axis.GetCurrentWMXParameter(axisIndex, ref param);

                        foreach (WMXParameterList key in Enum.GetValues(typeof(WMXParameterList))) {
                            switch (key) {
                                case WMXParameterList.GearRatioNumerator:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_gearRatioNum}", WMXParameterPath);
                                    break;

                                case WMXParameterList.GearRatioDenominator:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_gearRatioDen}", WMXParameterPath);
                                    break;

                                case WMXParameterList.AbsoluteEncoderMode:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_absEncoderMode}", WMXParameterPath);
                                    break;

                                case WMXParameterList.AbsoluteEncoderHomeOffset:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_absEncoderHomeOffset}", WMXParameterPath);
                                    break;

                                case WMXParameterList.PosSetWidth:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_posSetWidth}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeType:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeType}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeDirection:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeDirection}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeSlowVelocity:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeSlowVelocity}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeSlowAcc:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeSlowAcc}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeSlowDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeSlowDec}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeFastVelocity:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeFastVelocity}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeFastAcc:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeFastAcc}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeFastDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeFastDec}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeShiftVelocity:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeShiftVelocity}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeShiftAcc:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeShiftAcc}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeShiftDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeShiftDec}", WMXParameterPath);
                                    break;

                                case WMXParameterList.HomeShiftDistance:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_homeShiftDistance}", WMXParameterPath);
                                    break;

                                case WMXParameterList.LimitSwitchType:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_limitSwitchType}", WMXParameterPath);
                                    break;

                                case WMXParameterList.SoftLimitSwitchType:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_softLimitSwitchType}", WMXParameterPath);
                                    break;

                                case WMXParameterList.SoftLimitSwitchPosValue:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_softLimitPosValue}", WMXParameterPath);
                                    break;

                                case WMXParameterList.SoftLimitSwitchNegValue:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_softLimitNegValue}", WMXParameterPath);
                                    break;

                                case WMXParameterList.LimitDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_limitDec}", WMXParameterPath);
                                    break;

                                case WMXParameterList.LinearInterpolationCalcMode:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_linintplCalcMode}", WMXParameterPath);
                                    break;

                                case WMXParameterList.QuickStopDec:
                                    Ini.SetValueString($"{section}", $"{key}", $"{param.m_quickStopDecel}", WMXParameterPath);
                                    break;
                            }
                        }
                    }
                    Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, Log.LogMessage_Main.WMX_ParameterSaveSuccess));
                    m_isSettingSuccess_WMX = true;
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.WMX_ParameterSaveFail, ex));
                    m_isSettingSuccess_WMX = false;
                    return false;
                }
            }
            /// <summary>
            /// 앱솔루트 타입의 경우 Homing 완료 후 WMX 파라미터 저장
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool SaveAbsoluteHomeDone(AxisList axis) {
                try {
                    int axisIndex = GetAxisNumber(axis);
                    WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                    m_axis.GetCurrentWMXParameter(axisIndex, ref param);

                    Ini.SetValueString($"{axis}", $"{WMXParameterList.AbsoluteEncoderHomeOffset}", $"{param.m_absEncoderHomeOffset}", WMXParameterPath);

                    return true;
                }catch(Exception ex) {
                    return false;
                }
            }
            /// <summary>
            /// Soft Limit Positive Value 저장
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool SaveSoftLimitPositiveValue(AxisList axis) {
                try {
                    int axisIndex = GetAxisNumber(axis);
                    WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                    m_axis.GetCurrentWMXParameter(axisIndex, ref param);
                    Ini.SetValueString($"{axis}", $"{WMXParameterList.SoftLimitSwitchPosValue}", $"{param.m_softLimitPosValue}", WMXParameterPath);

                    return true;
                }catch(Exception ex) {
                    return false;
                }
            }
            /// <summary>
            /// Soft Limit Negative Value 저장
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool SaveSoftLimitNegativeValue(AxisList axis) {
                try {
                    int axisIndex = GetAxisNumber(axis);
                    WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                    m_axis.GetCurrentWMXParameter(axisIndex, ref param);
                    Ini.SetValueString($"{axis}", $"{WMXParameterList.SoftLimitSwitchPosValue}", $"{param.m_softLimitPosValue}", WMXParameterPath);

                    return true;
                }
                catch (Exception ex) {
                    return false;
                }
            }
            /// <summary>
            /// Soft Limit Positive Value 설정
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool SetSoftwareLimitPositiveValue(AxisList axis, float value) {
                int axisIndex = GetAxisNumber(axis);
                WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                m_axis.GetCurrentWMXParameter(axisIndex, ref param);
                param.m_softLimitPosValue = value;
                m_axis.SetWMXParameter(axisIndex, param);

                return true;
            }
            /// <summary>
            /// Soft Limit Negative Value 설정
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool SetSoftwareLimitNegativeValue(AxisList axis, float value) {
                int axisIndex = GetAxisNumber(axis);
                WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                m_axis.GetCurrentWMXParameter(axisIndex, ref param);
                param.m_softLimitNegValue = value;
                m_axis.SetWMXParameter(axisIndex, param);

                return true;
            }
            /// <summary>
            /// Full Closed 파라미터 설정
            /// </summary>
            /// <param name="barcodeParam"></param>
            /// <param name="safetyParam"></param>
            /// <returns></returns>
            public bool SetFullClosedParam(BarcodeParameter barcodeParam, BarcodeSafetyParameter safetyParam) {
                if (!GetMotionParam().useFullClosed)
                    return true;

                if (barcodeParam == null || safetyParam == null) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.ObjectIsNull));
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail));
                    return false;
                }

                if (!m_axis.SetFullClosedParameter(barcodeParam) || !m_axis.SetFullClosedSafetyParameter(safetyParam)) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail));
                    return false;
                }

                Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Barcode Parameter Set Success"));

                return true;
            }
            /// <summary>
            /// Motion 파라미터 설정
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public bool SetMotionParam(MotionParam param) {
                if (param == null) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.ObjectIsNull));
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail));
                    return false;
                }

                try {
                    foreach (MotionParameter key in Enum.GetValues(typeof(MotionParameter))) {
                        switch (key) {
                            case MotionParameter.ForkType:
                                m_motionParam.forkType = param.forkType;
                                break;

                            case MotionParameter.StopMode:
                                m_motionParam.stopMode = param.stopMode;
                                break;

                            case MotionParameter.ZOverridePercent:
                                m_motionParam.Z_OverridePercent = param.Z_OverridePercent;
                                break;

                            case MotionParameter.ZOverrideFromUpDistance:
                                m_motionParam.Z_OverrideFromUpDist = param.Z_OverrideFromUpDist;
                                break;

                            case MotionParameter.ZOverrideFromDownDistance:
                                m_motionParam.Z_OverrideFromDownDist = param.Z_OverrideFromDownDist;
                                break;

                            case MotionParameter.ZOverrideToUpDistance:
                                m_motionParam.Z_OverrideToUpDist = param.Z_OverrideToUpDist;
                                break;

                            case MotionParameter.ZOverrideToDownDistance:
                                m_motionParam.Z_OverrideToDownDist = param.Z_OverrideToDownDist;
                                break;

                            case MotionParameter.TurnCenterPosition:
                                m_motionParam.turnCenterPosition = param.turnCenterPosition;
                                break;

                            case MotionParameter.ArmHomePositionRange:
                                m_motionParam.armHomePositionRange = param.armHomePositionRange;
                                break;

                            case MotionParameter.UseInterpolation:
                                m_motionParam.useInterpolation = param.useInterpolation;
                                break;

                            case MotionParameter.UseFullclosed:
                                m_motionParam.useFullClosed = param.useFullClosed;
                                break;

                            case MotionParameter.DistanceDetectSensor:
                                m_motionParam.distDetectSensor = param.distDetectSensor;
                                break;

                            case MotionParameter.ZAutoHomingCount:
                                m_motionParam.Z_AutoHomingCount = param.Z_AutoHomingCount;
                                break;

                            case MotionParameter.PresenseSensorConditionType:
                                m_motionParam.presenseConditionType = param.presenseConditionType;
                                break;

                            case MotionParameter.InPlaceSensorConditionType:
                                m_motionParam.inPlaceConditionType = param.inPlaceConditionType;
                                break;

                            case MotionParameter.InPlaceSensorType:
                                m_motionParam.inPlaceType = param.inPlaceType;
                                break;

                            case MotionParameter.ZAxisBeltType:
                                m_motionParam.ZAxisBeltType = param.ZAxisBeltType;
                                break;

                            case MotionParameter.ZAxisBeltHomeOffset:
                                m_motionParam.ZAxisBeltHomeOffset = param.ZAxisBeltHomeOffset;
                                break;

                            case MotionParameter.ZAxisBeltFirstDia:
                                m_motionParam.ZAxisBeltFirstDia = param.ZAxisBeltFirstDia;
                                break;

                            case MotionParameter.ZAxisBeltIncrementDia:
                                m_motionParam.ZAxisBeltDia = param.ZAxisBeltDia;
                                break;

                            case MotionParameter.ToModeAutoSpeedPercent:
                                m_motionParam.toModeAutoSpeedPercent = param.toModeAutoSpeedPercent;
                                break;

                            case MotionParameter.UseRegulator:
                                m_motionParam.useRegulator = param.useRegulator;
                                break;

                            case MotionParameter.MaintTarget:
                                m_motionParam.maintTarget = param.maintTarget;
                                break;

                            case MotionParameter.UseMaint:
                                m_motionParam.useMaint = param.useMaint;
                                break;

                            case MotionParameter.PresenseSensorType:
                                m_motionParam.presensType = param.presensType;
                                break;
                        }
                    }
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail, ex));
                    return false;
                }
            }
            /// <summary>
            /// Auto Teaching 파라미터 설정
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public bool SetAutoTeachingParam(AutoTeachingParam param) {
                if (param == null) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.ObjectIsNull));
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail));
                    return false;
                }

                try {
                    foreach (AutoTeachingParameter key in Enum.GetValues(typeof(AutoTeachingParameter))) {
                        switch (key) {
                            case AutoTeachingParameter.SensorType:
                                m_autoTeachingParam.sensorType = param.sensorType;
                                break;

                            //case AutoTeachingParameter.AutoTeachingType:
                            //    m_autoTeachingParam.autoTeachingType = param.autoTeachingType;
                            //    break;

                            case AutoTeachingParameter.AutoTeachingSpeedX:
                                m_autoTeachingParam.autoTeachingSpeedX = param.autoTeachingSpeedX;
                                break;

                            case AutoTeachingParameter.AutoTeachingSpeedZ:
                                m_autoTeachingParam.autoTeachingSpeedZ = param.autoTeachingSpeedZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingAccDecX:
                                m_autoTeachingParam.autoTeachingAccDecX = param.autoTeachingAccDecX;
                                break;

                            case AutoTeachingParameter.AutoTeachingAccDecZ:
                                m_autoTeachingParam.autoTeachingAccDecZ = param.autoTeachingAccDecZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingDistX:
                                m_autoTeachingParam.autoTeachingDistX = param.autoTeachingDistX;
                                break;

                            case AutoTeachingParameter.AutoTeachingDistZ:
                                m_autoTeachingParam.autoTeachingDistZ = param.autoTeachingDistZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingCompensationX:
                                m_autoTeachingParam.autoTeachingCompensationX = param.autoTeachingCompensationX;
                                break;

                            case AutoTeachingParameter.AutoTeachingCompensationZ:
                                m_autoTeachingParam.autoTeachingCompensationZ = param.autoTeachingCompensationZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorRangeX:
                                m_autoTeachingParam.autoTeachingFindSensorRangeX = param.autoTeachingFindSensorRangeX;
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorRangeZ:
                                m_autoTeachingParam.autoTeachingFindSensorRangeZ = param.autoTeachingFindSensorRangeZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorSpeedX:
                                m_autoTeachingParam.autoTeachingFindSensorSpeedX = param.autoTeachingFindSensorSpeedX;
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorSpeedZ:
                                m_autoTeachingParam.autoTeachingFindSensorSpeedZ = param.autoTeachingFindSensorSpeedZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorAccDecX:
                                m_autoTeachingParam.autoTeachingFindSensorAccDecX = param.autoTeachingFindSensorAccDecX;
                                break;

                            case AutoTeachingParameter.AutoTeachingFindSensorAccDecZ:
                                m_autoTeachingParam.autoTeachingFindSensorAccDecZ = param.autoTeachingFindSensorAccDecZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorWidth:
                                m_autoTeachingParam.autoTeachingReflectorWidth = param.autoTeachingReflectorWidth;
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorHeight:
                                m_autoTeachingParam.autoTeachingReflectorHeight = param.autoTeachingReflectorHeight;
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorWidthErrorRange:
                                m_autoTeachingParam.autoTeachingReflectorErrorRangeWidth = param.autoTeachingReflectorErrorRangeWidth;
                                break;

                            case AutoTeachingParameter.AutoTeachingReflectorHeightErrorRange:
                                m_autoTeachingParam.autoTeachingReflectorErrorRangeHeight = param.autoTeachingReflectorErrorRangeHeight;
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetSpeedX:
                                m_autoTeachingParam.autoTeachingTargetSpeedX = param.autoTeachingTargetSpeedX;
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetSpeedZ:
                                m_autoTeachingParam.autoTeachingTargetSpeedZ = param.autoTeachingTargetSpeedZ;
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetAccDecX:
                                m_autoTeachingParam.autoTeachingTargetAccDecX = param.autoTeachingTargetAccDecX;
                                break;

                            case AutoTeachingParameter.AutoTeachingTargetAccDecZ:
                                m_autoTeachingParam.autoTeachingTargetAccDecZ = param.autoTeachingTargetAccDecZ;
                                break;

                            case AutoTeachingParameter.DoubleStorageSensorCheck:
                                m_autoTeachingParam.doubleStorageSensorCheck = param.doubleStorageSensorCheck;
                                break;
                        }
                    }
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail, ex));
                    return false;
                }
            }
            /// <summary>
            /// SCARA 파라미터 설정
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public bool SetScaraParameter(ScaraParameter param) {
                if (param == null) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.ObjectIsNull));
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail));
                    return false;
                }

                try {
                    foreach (SCARAParameter key in Enum.GetValues(typeof(SCARAParameter))) {
                        switch (key) {
                            case SCARAParameter.RZLength:
                                m_scara.RZ_LENGTH = param.RZ_LENGTH;
                                break;

                            case SCARAParameter.RXLength:
                                m_scara.RX_LENGTH = param.RX_LENGTH;
                                break;

                            case SCARAParameter.RYLength:
                                m_scara.RY_LENGTH = param.RY_LENGTH;
                                break;
                        }
                    }
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail, ex));
                    return false;
                }
            }
            /// <summary>
            /// Timer 파라미터 설정
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public bool SetTimerOverParam(TimerParameter param) {
                if (param == null) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.ObjectIsNull));
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail));
                    return false;
                }

                try {
                    foreach (PART.TimerParameter key in Enum.GetValues(typeof(PART.TimerParameter))) {
                        switch (key) {
                            case PART.TimerParameter.CIMTimerOver:
                                m_timerParam.CIM_TIMEOVER = param.CIM_TIMEOVER;
                                break;

                            case PART.TimerParameter.StepTimerOver:
                                m_timerParam.STEP_TEIMOVER = param.STEP_TEIMOVER;
                                break;

                            case PART.TimerParameter.PIOReadyTimerOver:
                                m_timerParam.PIO_READY_TIMOVER = param.PIO_READY_TIMOVER;
                                break;

                            case PART.TimerParameter.HomeMoveTimerOver:
                                m_timerParam.HOME_MOVE_TIMEOVER = param.HOME_MOVE_TIMEOVER;
                                break;

                            case PART.TimerParameter.EventTimer:
                                m_timerParam.EVENT_TIMEROVER = param.EVENT_TIMEROVER;
                                break;

                            case PART.TimerParameter.AutoTeachingStepTimeOver:
                                m_timerParam.AUTO_TEACHING_STEP_TIMEOVER = param.AUTO_TEACHING_STEP_TIMEOVER;
                                break;
                        }
                    }
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.RackMasterParameterSaveFail, ex));
                    return false;
                }
            }
            /// <summary>
            /// Axis 파라미터 설정
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public bool SetAxisParameter(AxisList axis, AxisParam param) {
                int idx = (int)axis;

                try {
                    foreach (AxisParameter key in Enum.GetValues(typeof(AxisParameter))) {
                        switch (key) {
                            case AxisParameter.AxisNumber:
                                m_axisParam[idx].axisNumber = param.axisNumber;
                                break;

                            case AxisParameter.AutoSpeedPercent:
                                m_axisParam[idx].autoSpeedPercent = param.autoSpeedPercent;
                                break;

                            case AxisParameter.MaxSpeed:
                                m_axisParam[idx].maxSpeed = param.maxSpeed;
                                break;

                            case AxisParameter.MaxAccDec:
                                m_axisParam[idx].maxAccDec = param.maxAccDec;
                                break;

                            case AxisParameter.JerkRatio:
                                m_axisParam[idx].jerkRatio = param.jerkRatio;
                                break;

                            case AxisParameter.JogHighSpeedLimit:
                                m_axisParam[idx].jogHighSpeedLimit = param.jogHighSpeedLimit;
                                break;

                            case AxisParameter.JogLowSpeedLimit:
                                m_axisParam[idx].jogLowSpeedLimit = param.jogLowSpeedLimit;
                                break;

                            case AxisParameter.InchingLimit:
                                m_axisParam[idx].inchingLimit = param.inchingLimit;
                                break;

                            case AxisParameter.ManualHighSpeed:
                                m_axisParam[idx].manualHighSpeed = param.manualHighSpeed;
                                break;

                            case AxisParameter.ManualHighAccDec:
                                m_axisParam[idx].manualHighAccDec = param.manualHighAccDec;
                                break;

                            case AxisParameter.ManualLowSpeed:
                                m_axisParam[idx].manualLowSpeed = param.manualLowSpeed;
                                break;

                            case AxisParameter.ManualLowAccDec:
                                m_axisParam[idx].manualLowAccDec = param.manualLowAccDec;
                                break;

                            case AxisParameter.MaxOverload:
                                m_axisParam[idx].maxOverload = param.maxOverload;
                                break;

                            case AxisParameter.HomePositionRange:
                                m_axisParam[idx].homePositionRange = param.homePositionRange;
                                break;

                            case AxisParameter.QuickStop:
                                m_axisParam[idx].quickStop = param.quickStop;
                                break;

                            case AxisParameter.NormalStop:
                                m_axisParam[idx].normalStop = param.normalStop;
                                break;

                            case AxisParameter.SlowStop:
                                m_axisParam[idx].slowStop = param.slowStop;
                                break;

                            case AxisParameter.PosSensorByteAddr:
                                m_axisParam[idx].posSensorByteAddr = param.posSensorByteAddr;
                                break;

                            case AxisParameter.PosSensorBitAddr:
                                m_axisParam[idx].posSensorBitAddr = param.posSensorBitAddr;
                                break;

                            case AxisParameter.PosSensorEnabled:
                                m_axisParam[idx].posSensorEnabled = param.posSensorEnabled;
                                break;

                            case AxisParameter.PosSensor2ByteAddr:
                                m_axisParam[idx].posSensor2ByteAddr = param.posSensor2ByteAddr;
                                break;

                            case AxisParameter.PosSensor2BitAddr:
                                m_axisParam[idx].posSensor2BitAddr = param.posSensor2BitAddr;
                                break;

                            case AxisParameter.PosSensor2Enabled:
                                m_axisParam[idx].posSensor2Enabled = param.posSensor2Enabled;
                                break;

                            case AxisParameter.SoftwareLimitPositive:
                                m_axisParam[idx].swLimitPositive = param.swLimitPositive;
                                //m_axis.GetCurrentWMXParameter(GetAxisNumber(axis)).m_softLimitPosValue = param.swLimitPositive;
                                //m_axis.SetWMXParameter();
                                break;

                            case AxisParameter.SoftwareLimitNegative:
                                m_axisParam[idx].swLimitNegative = param.swLimitNegative;
                                //m_axis.GetCurrentWMXParameter(GetAxisNumber(axis)).m_softLimitNegValue = param.swLimitNegative;
                                //m_axis.SetWMXParameter();
                                break;
                        }
                    }
                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.AxisParameterSaveFail, ex));
                    return false;
                }
            }
            /// <summary>
            /// WMX 파라미터 설정
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="param"></param>
            /// <returns></returns>
            public bool SetWMXParameter(AxisList axis, WMXMotion.AxisParameter param) {
                int axisIndex = GetAxisNumber(axis);
                if(!m_axis.SetWMXParameter(axisIndex, param)) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, Log.LogMessage_Main.WMX_ParameterSaveFail));
                    return false;
                }

                return true;
                //try {
                //    foreach (WMXParameterList wmx in Enum.GetValues(typeof(WMXParameterList))) {
                //        switch (wmx) {
                //            case WMXParameterList.GearRatioNumerator:
                //                m_axis.GetCurrentWMXParameter(idx).m_gearRatioNum = param.m_gearRatioNum;
                //                break;

                //            case WMXParameterList.GearRatioDenominator:
                //                m_axis.GetCurrentWMXParameter(idx).m_gearRatioDen = param.m_gearRatioDen;
                //                break;

                //            case WMXParameterList.AbsoluteEncoderMode:
                //                m_axis.GetCurrentWMXParameter(idx).m_absEncoderMode = param.m_absEncoderMode;
                //                break;

                //            case WMXParameterList.AbsoluteEncoderHomeOffset:
                //                m_axis.GetCurrentWMXParameter(idx).m_absEncoderHomeOffset = param.m_absEncoderHomeOffset;
                //                break;

                //            case WMXParameterList.PosSetWidth:
                //                m_axis.GetCurrentWMXParameter(idx).m_posSetWidth = param.m_posSetWidth;
                //                break;

                //            case WMXParameterList.HomeType:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeType = param.m_homeType;
                //                break;

                //            case WMXParameterList.HomeDirection:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeDirection = param.m_homeDirection;
                //                break;

                //            case WMXParameterList.HomeSlowVelocity:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeSlowVelocity = param.m_homeSlowVelocity;
                //                break;

                //            case WMXParameterList.HomeSlowAcc:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeSlowAcc = param.m_homeSlowAcc;
                //                break;

                //            case WMXParameterList.HomeSlowDec:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeSlowDec = param.m_homeSlowDec;
                //                break;

                //            case WMXParameterList.HomeFastVelocity:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeFastVelocity = param.m_homeFastVelocity;
                //                break;

                //            case WMXParameterList.HomeFastAcc:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeFastAcc = param.m_homeFastAcc;
                //                break;

                //            case WMXParameterList.HomeFastDec:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeFastDec = param.m_homeFastDec;
                //                break;

                //            case WMXParameterList.HomeShiftVelocity:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeShiftVelocity = param.m_homeShiftVelocity;
                //                break;

                //            case WMXParameterList.HomeShiftAcc:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeShiftAcc = param.m_homeShiftAcc;
                //                break;

                //            case WMXParameterList.HomeShiftDec:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeShiftDec = param.m_homeShiftDec;
                //                break;

                //            case WMXParameterList.HomeShiftDistance:
                //                m_axis.GetCurrentWMXParameter(idx).m_homeShiftDistance = param.m_homeShiftDistance;
                //                break;

                //            case WMXParameterList.LimitSwitchType:
                //                m_axis.GetCurrentWMXParameter(idx).m_limitSwitchType = param.m_limitSwitchType;
                //                break;

                //            case WMXParameterList.SoftLimitSwitchType:
                //                m_axis.GetCurrentWMXParameter(idx).m_softLimitSwitchType = param.m_softLimitSwitchType;
                //                break;

                //            case WMXParameterList.SoftLimitSwitchPosValue:
                //                m_axis.GetCurrentWMXParameter(idx).m_softLimitPosValue = param.m_softLimitPosValue;
                //                break;

                //            case WMXParameterList.SoftLimitSwitchNegValue:
                //                m_axis.GetCurrentWMXParameter(idx).m_softLimitNegValue = param.m_softLimitNegValue;
                //                break;

                //            case WMXParameterList.LimitDec:
                //                m_axis.GetCurrentWMXParameter(idx).m_limitDec = param.m_limitDec;
                //                break;

                //            case WMXParameterList.LinearInterpolationCalcMode:
                //                m_axis.GetCurrentWMXParameter(idx).m_linintplCalcMode = param.m_linintplCalcMode;
                //                break;

                //            case WMXParameterList.QuickStopDec:
                //                m_axis.GetCurrentWMXParameter(idx).m_quickStopDecel = param.m_quickStopDecel;
                //                break;
                //        }
                //    }
                //    if (!m_axis.SetWMXParameter()) {
                //        Log.Add(new Log.LogItem(Log.LogLevel.Error, Log.LogType.WMX, Log.LogMessage_Main.WMX_ParameterSaveFail));
                //        return false;
                //    }
                //    //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.WMX, Log.LogMessage_Main.WMX_ParameterSaveSuccess));
                //    return true;
                //} catch (Exception ex) {
                //    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.WMX, Log.LogMessage_Main.WMX_ParameterSaveFail, ex));
                //    return false;
                //}
            }
            /// <summary>
            /// 설정된 Motion 파라미터 반환
            /// </summary>
            /// <returns></returns>
            public MotionParam GetMotionParam() {
                return m_motionParam;
            }
            /// <summary>
            /// 설정된 AutoTeaching 파라미터 반환
            /// </summary>
            /// <returns></returns>
            public AutoTeachingParam GetAutoTeachingParam() {
                return m_autoTeachingParam;
            }
            /// <summary>
            /// 설정된 SCARA 파라미터 반환
            /// </summary>
            /// <returns></returns>
            public ScaraParameter GetSCARAParamter() {
                return m_scara;
            }
            /// <summary>
            /// 설정된 Timer 파라미터 반환
            /// </summary>
            /// <returns></returns>
            public TimerParameter GetTimerParam() {
                return m_timerParam;
            }
            /// <summary>
            /// Full Closed 파라미터 반환
            /// </summary>
            /// <returns></returns>
            public BarcodeParameter GetBarcodeParam() {
                return m_axis.GetFullClosedParameter();
            }
            /// <summary>
            /// Full Closed Safety 파라미터 반환
            /// </summary>
            /// <returns></returns>
            public BarcodeSafetyParameter GetBarcodeSafetyParam() {
                return m_axis.GetFullClosedSafetyParameter();
            }
            /// <summary>
            /// 축 파라미터 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public AxisParam GetAxisParameter(AxisList axis) {
                return m_axisParam[(int)axis];
            }
            /// <summary>
            /// WMX 파라미터 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public WMXMotion.AxisParameter GetWMXParameter(AxisList axis) {
                int axisIndex = GetAxisNumber(axis);
                WMXMotion.AxisParameter param = new WMXMotion.AxisParameter();
                m_axis.GetCurrentWMXParameter(axisIndex, ref param);
                return param;
            }

            public void GetWMXParameter(AxisList axis, ref WMXMotion.AxisParameter axisParameter) {
                int axisIndex = GetAxisNumber(axis);
                m_axis.GetCurrentWMXParameter(axisIndex, ref axisParameter);
            }
            /// <summary>
            /// 랙마스터의 총 축 개수 반환
            /// </summary>
            /// <returns></returns>
            public int GetAxisCount() {
                return m_MaxAxisCount;
            }
            /// <summary>
            /// 해당 축의 축 번호를 반환하는 함수
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public int GetAxisNumber(AxisList axis) {
                try {
                    return m_axisParam[(int)axis].axisNumber;
                } catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, Log.LogMessage_Main.GetParameterFail, ex));
                    return -1;
                }
            }
            /// <summary>
            /// 해당 축의 Auto Speed를 반환하는 함수
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public float GetWMX3AxisAutoVelocity(AxisList axis) {
                int idx = (int)axis;

                switch (axis) {
                    case AxisList.X_Axis:
                        return (m_axisParam[idx].maxSpeed * 1000000 / 60) * (m_axisParam[idx].autoSpeedPercent / 100);

                    case AxisList.Z_Axis:
                        if (GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            return (m_axisParam[idx].maxSpeed * 1000000 / 60) * (m_axisParam[idx].autoSpeedPercent / 100);
                        }
                        else {
                            double speed = (m_axisParam[idx].maxSpeed * 1000000 / 60) * (m_axisParam[idx].autoSpeedPercent / 100);
                            double calculatedSpeed = RackMasterMotion.DistanceToRadian(GetMotionParam().ZAxisBeltFirstDia, speed);

                            return (float)calculatedSpeed;
                        }

                    case AxisList.A_Axis:
                        if (m_motionParam.forkType == ForkType.SCARA) {
                            return (m_axisParam[idx].maxSpeed * 1000 / 60) * (m_axisParam[idx].autoSpeedPercent / 100);
                        }
                        else {
                            return (m_axisParam[idx].maxSpeed * 1000000 / 60) * (m_axisParam[idx].autoSpeedPercent / 100);
                        }

                    case AxisList.T_Axis:
                        //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"T maxSpeed={m_axisParam[idx].maxSpeed}, percent={m_axisParam[idx].autoSpeedPercent}"));
                        return (m_axisParam[idx].maxSpeed * 1000 / 60) * (m_axisParam[idx].autoSpeedPercent / 100);
                }

                return 0;
            }
            /// <summary>
            /// 해당 축의 Manual Speed를 반환하는 함수
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="speedType"></param>
            /// <returns></returns>
            public float GetWMX3AxisManualVelocity(AxisList axis, ManualSpeedType speedType) {
                int idx = (int)axis;


                switch (axis) {
                    case AxisList.X_Axis:
                        if (speedType == ManualSpeedType.High) {
                            return (m_axisParam[idx].manualHighSpeed * 1000000 / 60);
                        }
                        else {
                            return (m_axisParam[idx].manualLowSpeed * 1000000 / 60);
                        }

                    case AxisList.Z_Axis:
                        if (speedType == ManualSpeedType.High) {
                            if (GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                return (m_axisParam[idx].manualHighSpeed * 1000000 / 60);
                            }
                            else {
                                double speedHigh = (m_axisParam[idx].manualHighSpeed * 1000000 / 60);
                                double calculatedSpeedHigh = RackMasterMotion.DistanceToRadian(GetMotionParam().ZAxisBeltFirstDia, speedHigh);

                                return (float)calculatedSpeedHigh;
                            }
                        }
                        else {
                            if (GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                return (m_axisParam[idx].manualLowSpeed * 1000000 / 60);
                            }
                            else {
                                double speedLow = (m_axisParam[idx].manualLowSpeed * 1000000 / 60);
                                double calculatedSpeedLow = RackMasterMotion.DistanceToRadian(GetMotionParam().ZAxisBeltFirstDia, speedLow);

                                return (float)calculatedSpeedLow;
                            }
                        }

                    case AxisList.A_Axis:
                        if (m_motionParam.forkType == ForkType.SCARA) {
                            if (speedType == ManualSpeedType.High) {
                                return (m_axisParam[idx].manualHighSpeed * 1000 / 60);
                            }
                            else {
                                return (m_axisParam[idx].manualLowSpeed * 1000 / 60);
                            }
                        }
                        else {
                            if (speedType == ManualSpeedType.High) {
                                return (m_axisParam[idx].manualHighSpeed * 1000000 / 60);
                            }
                            else {
                                return (m_axisParam[idx].manualLowSpeed * 1000000 / 60);
                            }
                        }

                    case AxisList.T_Axis:
                        if (speedType == ManualSpeedType.High) {
                            return (m_axisParam[idx].manualHighSpeed * 1000 / 60);
                        }
                        else {
                            return (m_axisParam[idx].manualLowSpeed * 1000 / 60);
                        }
                }
                return 0;
            }
            /// <summary>
            /// 해당 축의 Auto Teaching 속도를 반환하는 함수
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public float GetWMX3AxisAutoTeachingVelocity(AxisList axis) {
                switch (axis) {
                    case AxisList.X_Axis:
                        return (m_autoTeachingParam.autoTeachingSpeedX * 1000);

                    case AxisList.Z_Axis:
                        if (GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            return (m_autoTeachingParam.autoTeachingSpeedZ * 1000);
                        }
                        else {
                            double speed = m_autoTeachingParam.autoTeachingSpeedZ * 1000;
                            double calculatedSpeed = RackMasterMotion.DistanceToRadian(GetMotionParam().ZAxisBeltFirstDia, speed);

                            return (float)calculatedSpeed;
                        }
                }

                return 0;
            }
            /// <summary>
            /// Full Closed 사용시 P Gain을 설정
            /// </summary>
            /// <param name="pGain"></param>
            public void SetFullClosedPGain(double pGain) {
                m_PGain = pGain;
            }
            /// <summary>
            /// Full Closed 사용시 I Gain을 설정
            /// </summary>
            /// <param name="iGain"></param>
            public void SetFullClosedIGain(double iGain) {
                m_IGain = iGain;
            }
            /// <summary>
            /// Full Closed 사용시 P Gain을 반환
            /// </summary>
            /// <returns></returns>
            public double GetFullClosedPGain() {
                return m_PGain;
            }
            /// <summary>
            /// Full Closed 사용시 I Gain을 반환
            /// </summary>
            /// <returns></returns>
            public double GetFullClosedIGain() {
                return m_IGain;
            }
            /// <summary>
            /// WMX Parameter 세팅이 성공했는지 여부 반환
            /// </summary>
            /// <returns></returns>
            public bool IsSettingSuccess_WMX() {
                return m_isSettingSuccess_WMX;
            }
            /// <summary>
            /// Rack Master Setting Parameter 세팅이 성공했는지 여부 반환
            /// </summary>
            /// <returns></returns>
            public bool IsSettingSuccess_RM() {
                return m_isSettingSuccess_RM;
            }
            /// <summary>
            /// Axis Parameter 세팅이 성공했는지 여부 반환
            /// </summary>
            /// <returns></returns>
            public bool IsSettingSuccess_Axis() {
                return m_isSettingSuccess_Axis;
            }
            /// <summary>
            /// Port Parameter 세팅이 성공했는지 여부 반환
            /// </summary>
            /// <returns></returns>
            public bool IsSettingSuccess_Port() {
                return m_isSettingSuccess_Port;
            }
        }
    }
}