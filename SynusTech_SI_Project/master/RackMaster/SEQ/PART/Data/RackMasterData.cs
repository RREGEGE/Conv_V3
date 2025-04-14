using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.COMMON;
using RackMaster.SEQ.CLS;

namespace RackMaster.SEQ.PART {
    public enum CycleDataList {
        CycleCount,         // Total Cycle Count
        AutoHomingCount,    // Auto Homing 을 진행하기까지의 Count. 에를 들어 Auto Homing을 10번 카운트마다 시행한다하면 Auto Homing Count가 10이 되었을 때 Auto Homing 진행 후 Auto Homing Count 클리어
        LastMode,           // 마지막 Cycle의 모드(From / To)

        From_CycleTime,
        From_XZT_Move_Time,
        From_ForkFWD_Time,
        From_Z_Up_Time,
        From_ForkBWD_Time,

        To_CycleTime,
        To_XZT_Move_Time,
        To_ForkFWD_Time,
        To_Z_Down_Time,
        To_ForkBWD_Time,
    }

    public enum AxisDataList {
        From_XZT_AverageTorque_X,
        From_XZT_AverageTorque_Z,
        From_XZT_AverageTorque_T,
        From_XZT_MaxTorque_X,
        From_XZT_MaxTorque_Z,
        From_XZT_MaxTorque_T,
        From_ForkFWD_AverageTorque_A,
        From_ForkFWD_MaxTorque_A,
        From_Z_Up_AverageTorque_Z,
        From_Z_Up_MaxTorque_Z,
        From_ForkBWD_AverageTorque_A,
        From_ForkBWD_MaxTorque_A,

        To_XZT_AverageTorque_X,
        To_XZT_AverageTorque_Z,
        To_XZT_AverageTorque_T,
        To_XZT_MaxTorque_X,
        To_XZT_MaxTorque_Z,
        To_XZT_MaxTorque_T,
        To_ForkFWD_AverageTorque_A,
        To_ForkFWD_MaxTorque_A,
        To_Z_Down_AverageTorque_Z,
        To_Z_Down_MaxTorque_Z,
        To_ForkBWD_AverageToruqe_A,
        To_ForkBWD_MaxTorque_A,
    }

    public enum CycleTime {
        From_TotalCycle = 0,
        From_XZT_Move,
        From_Fork_FWD,
        From_Z_Up,
        From_Fork_BWD,

        To_TotalCycle,
        To_XZT_Move,
        To_Fork_FWD,
        To_Z_Down,
        To_Fork_BWD,

        MAX_COUNT,
    }

    public partial class RackMasterMain {
        public class RackMasterCycleData {
            public long cycleCount;
            public long autoHomingCount;
            public MotionMode lastMode;

            // Time은 ms단위
            public int fromCycleTime;
            public int fromXZTMoveTime;
            public int fromForkFWDMoveTime;
            public int fromZUpTime;
            public int fromForkBWDMoveTime;

            public int toCycleTime;
            public int toXZTMoveTime;
            public int toForkFWDMoveTime;
            public int toZDownTime;
            public int toForkBWDMoveTime;

            public RackMasterCycleData() {
                cycleCount              = 0;
                autoHomingCount         = 0;
                lastMode                = MotionMode.From;

                fromCycleTime           = 0;
                fromXZTMoveTime         = 0;
                fromForkFWDMoveTime     = 0;
                fromZUpTime             = 0;
                fromForkBWDMoveTime     = 0;

                toCycleTime             = 0;
                toXZTMoveTime           = 0;
                toForkFWDMoveTime       = 0;
                toZDownTime             = 0;
                toForkBWDMoveTime       = 0;
            }

            public void ClearAllData() {
                cycleCount              = 0;
                autoHomingCount         = 0;

                fromCycleTime           = 0;
                fromXZTMoveTime         = 0;
                fromForkFWDMoveTime     = 0;
                fromZUpTime             = 0;
                fromForkBWDMoveTime     = 0;

                toCycleTime             = 0;
                toXZTMoveTime           = 0;
                toForkFWDMoveTime       = 0;
                toZDownTime             = 0;
                toForkBWDMoveTime       = 0;
            }

            public void ClearCycleCount() {
                cycleCount = 0;
            }

            public void ClearAutoHomingCount() {
                autoHomingCount = 0;
            }
        }

        public class RackMasterAxisData {
            public double fromXZT_AvrTorque_X;
            public double fromXZT_AvrTorque_Z;
            public double fromXZT_AvrTorque_T;

            public double fromXZT_MaxTorque_X;
            public double fromXZT_MaxTorque_Z;
            public double fromXZT_MaxTorque_T;

            public double fromForkFWD_AvrTorque_A;
            public double fromForkFWD_MaxTorque_A;

            public double fromZUp_AvrTorque_Z;
            public double fromZUp_MaxTorque_Z;

            public double fromForkBWD_AvrTorque_A;
            public double fromForkBWD_MaxTorque_A;

            public double toXZT_AvrTorque_X;
            public double toXZT_AvrTorque_Z;
            public double toXZT_AvrTorque_T;

            public double toXZT_MaxTorque_X;
            public double toXZT_MaxTorque_Z;
            public double toXZT_MaxTorque_T;

            public double toForkFWD_AvrTorque_A;
            public double toForkFWD_MaxTorque_A;

            public double toZDown_AvrTorque_Z;
            public double toZDown_MaxTorque_Z;

            public double toForkBWD_AvrTorque_A;
            public double toForkBWD_MaxTorque_A;

            public RackMasterAxisData() {
                fromXZT_AvrTorque_X         = 0;
                fromXZT_AvrTorque_Z         = 0;
                fromXZT_AvrTorque_T         = 0;
                fromXZT_MaxTorque_X         = 0;
                fromXZT_MaxTorque_Z         = 0;
                fromXZT_MaxTorque_T         = 0;
                fromForkFWD_AvrTorque_A     = 0;
                fromForkFWD_MaxTorque_A     = 0;
                fromZUp_AvrTorque_Z         = 0;
                fromZUp_MaxTorque_Z         = 0;
                fromForkBWD_AvrTorque_A     = 0;
                fromForkBWD_MaxTorque_A     = 0;

                toXZT_AvrTorque_X           = 0;
                toXZT_AvrTorque_Z           = 0;
                toXZT_AvrTorque_T           = 0;
                toXZT_MaxTorque_X           = 0;
                toXZT_MaxTorque_Z           = 0;
                toXZT_MaxTorque_T           = 0;
                toForkFWD_AvrTorque_A       = 0;
                toForkFWD_MaxTorque_A       = 0;
                toZDown_AvrTorque_Z         = 0;
                toZDown_MaxTorque_Z         = 0;
                toForkBWD_AvrTorque_A       = 0;
                toForkBWD_MaxTorque_A       = 0;
            }
        }

        public class RackMasterData {
            private enum SectionName {
                Cycle,
                Axis,
            }

            private RackMasterCycleData m_cycleData;
            private RackMasterAxisData m_axisData;
            private string m_filePath = $"{ManagedFileInfo.DataDirectory}\\{ManagedFileInfo.RackMasterDataFileName}";

            public RackMasterData() {
                m_cycleData = new RackMasterCycleData();
                m_axisData = new RackMasterAxisData();

                LoadRackMasterData();
            }
            /// <summary>
            /// 랙마스터 Cycle Data 로드
            /// </summary>
            /// <returns></returns>
            public bool LoadRackMasterData() {
                try {
                    if (!Ini.IsFileExist(m_filePath)) {
                        Ini.CreateIniFile($"{ManagedFileInfo.RackMasterDataFileName}", $"{ManagedFileInfo.DataDirectory}");

                        SaveRackMasterData();

                        return true;
                    }
                    else {
                        foreach (SectionName section in Enum.GetValues(typeof(SectionName))) {
                            switch (section) {
                                case SectionName.Cycle:
                                    try {
                                        foreach (CycleDataList key in Enum.GetValues(typeof(CycleDataList))) {
                                            switch (key) {
                                                case CycleDataList.CycleCount:
                                                    long.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.cycleCount);
                                                    break;

                                                case CycleDataList.AutoHomingCount:
                                                    long.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.autoHomingCount);
                                                    break;

                                                case CycleDataList.LastMode:
                                                    m_cycleData.lastMode = (MotionMode)Enum.Parse(typeof(MotionMode), Ini.GetValueString($"{section}", $"{key}", m_filePath));
                                                    break;


                                                case CycleDataList.From_CycleTime:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.fromCycleTime);
                                                    break;

                                                case CycleDataList.From_XZT_Move_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.fromXZTMoveTime);
                                                    break;

                                                case CycleDataList.From_ForkFWD_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.fromForkFWDMoveTime);
                                                    break;

                                                case CycleDataList.From_Z_Up_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.fromZUpTime);
                                                    break;

                                                case CycleDataList.From_ForkBWD_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.fromForkBWDMoveTime);
                                                    break;


                                                case CycleDataList.To_CycleTime:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.toCycleTime);
                                                    break;

                                                case CycleDataList.To_XZT_Move_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.toXZTMoveTime);
                                                    break;

                                                case CycleDataList.To_ForkFWD_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.toForkFWDMoveTime);
                                                    break;

                                                case CycleDataList.To_Z_Down_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.toZDownTime);
                                                    break;

                                                case CycleDataList.To_ForkBWD_Time:
                                                    int.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_cycleData.toForkBWDMoveTime);
                                                    break;
                                            }
                                        }
                                    }
                                    catch(Exception ex) {
                                        Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"Cycle Data Load Fail", ex));
                                    }
                                    break;

                                //case SectionName.Axis:
                                //    try {
                                //        foreach(AxisDataList key in Enum.GetValues(typeof(AxisDataList))) {
                                //            switch (key) {
                                //                case AxisDataList.From_XZT_AverageTorque_X:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromXZT_AvrTorque_X);
                                //                    break;

                                //                case AxisDataList.From_XZT_AverageTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromXZT_AvrTorque_Z);
                                //                    break;

                                //                case AxisDataList.From_XZT_AverageTorque_T:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromXZT_AvrTorque_T);
                                //                    break;

                                //                case AxisDataList.From_XZT_MaxTorque_X:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromXZT_MaxTorque_X);
                                //                    break;

                                //                case AxisDataList.From_XZT_MaxTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromXZT_MaxTorque_Z);
                                //                    break;

                                //                case AxisDataList.From_XZT_MaxTorque_T:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromXZT_MaxTorque_T);
                                //                    break;

                                //                case AxisDataList.From_ForkFWD_AverageTorque_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromForkFWD_AvrTorque_A);
                                //                    break;

                                //                case AxisDataList.From_ForkFWD_MaxTorque_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromForkFWD_MaxTorque_A);
                                //                    break;

                                //                case AxisDataList.From_Z_Up_AverageTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromZUp_AvrTorque_Z);
                                //                    break;

                                //                case AxisDataList.From_Z_Up_MaxTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromZUp_MaxTorque_Z);
                                //                    break;

                                //                case AxisDataList.From_ForkBWD_AverageTorque_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromForkBWD_AvrTorque_A);
                                //                    break;

                                //                case AxisDataList.From_ForkBWD_MaxTorque_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.fromForkBWD_MaxTorque_A);
                                //                    break;

                                //                case AxisDataList.To_XZT_AverageTorque_X:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toXZT_AvrTorque_X);
                                //                    break;

                                //                case AxisDataList.To_XZT_AverageTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toXZT_AvrTorque_Z);
                                //                    break;

                                //                case AxisDataList.To_XZT_AverageTorque_T:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toXZT_AvrTorque_T);
                                //                    break;

                                //                case AxisDataList.To_XZT_MaxTorque_X:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toXZT_MaxTorque_X);
                                //                    break;

                                //                case AxisDataList.To_XZT_MaxTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toXZT_MaxTorque_Z);
                                //                    break;

                                //                case AxisDataList.To_XZT_MaxTorque_T:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toXZT_MaxTorque_T);
                                //                    break;

                                //                case AxisDataList.To_ForkFWD_AverageTorque_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toForkFWD_AvrTorque_A);
                                //                    break;

                                //                case AxisDataList.To_ForkFWD_MaxTorque_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toForkFWD_MaxTorque_A);
                                //                    break;

                                //                case AxisDataList.To_Z_Down_AverageTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toZDown_AvrTorque_Z);
                                //                    break;

                                //                case AxisDataList.To_Z_Down_MaxTorque_Z:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toZDown_MaxTorque_Z);
                                //                    break;

                                //                case AxisDataList.To_ForkBWD_AverageToruqe_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toForkBWD_AvrTorque_A);
                                //                    break;

                                //                case AxisDataList.To_ForkBWD_MaxTorque_A:
                                //                    double.TryParse(Ini.GetValueString($"{section}", $"{key}", m_filePath), out m_axisData.toForkBWD_MaxTorque_A);
                                //                    break;
                                //            }
                                //        }
                                //    }catch(Exception ex) {
                                //        Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"Axis Data Load Fail", ex));
                                //    }
                                //    break;
                            }
                        }

                        return true;
                    }
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"RackMaster Data Load Fail", ex));

                    return false;
                }
            }
            /// <summary>
            /// 랙마스터 Cycle 데이터 저장
            /// </summary>
            /// <returns></returns>
            public bool SaveRackMasterData() {
                try {
                    if (!Ini.IsFileExist(m_filePath))
                        Ini.CreateIniFile($"{ManagedFileInfo.RackMasterDataFileName}", $"{ManagedFileInfo.DataDirectory}");

                    foreach (SectionName section in Enum.GetValues(typeof(SectionName))) {
                        foreach (CycleDataList key in Enum.GetValues(typeof(CycleDataList))) {
                            switch (key) {
                                case CycleDataList.CycleCount:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.cycleCount}", m_filePath);
                                    break;

                                case CycleDataList.AutoHomingCount:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.autoHomingCount}", m_filePath);
                                    break;

                                case CycleDataList.LastMode:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.lastMode}", m_filePath);
                                    break;


                                case CycleDataList.From_CycleTime:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.fromCycleTime}", m_filePath);
                                    break;

                                case CycleDataList.From_XZT_Move_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.fromXZTMoveTime}", m_filePath);
                                    break;

                                case CycleDataList.From_ForkFWD_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.fromForkFWDMoveTime}", m_filePath);
                                    break;

                                case CycleDataList.From_Z_Up_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.fromZUpTime}", m_filePath);
                                    break;

                                case CycleDataList.From_ForkBWD_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.fromForkBWDMoveTime}", m_filePath);
                                    break;


                                case CycleDataList.To_CycleTime:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.toCycleTime}", m_filePath);
                                    break;

                                case CycleDataList.To_XZT_Move_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.toXZTMoveTime}", m_filePath);
                                    break;

                                case CycleDataList.To_ForkFWD_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.toForkFWDMoveTime}", m_filePath);
                                    break;

                                case CycleDataList.To_Z_Down_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.toZDownTime}", m_filePath);
                                    break;

                                case CycleDataList.To_ForkBWD_Time:
                                    Ini.SetValueString($"{section}", $"{key}", $"{m_cycleData.toForkBWDMoveTime}", m_filePath);
                                    break;
                            }
                        }
                    }

                    return true;
                }
                catch (Exception ex) {
                    Log.Add(new Log.LogItem(Log.LogLevel.Exception, Log.LogType.RackMaster, $"RackMaster Data Save Fail", ex));
                    return false;
                }
            }
            /// <summary>
            /// Total Cycle Count 증가
            /// </summary>
            public void IncreaseCycleCount() {
                m_cycleData.cycleCount++;
                Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.CycleCount}", $"{m_cycleData.cycleCount}", m_filePath);
            }
            /// <summary>
            /// Auto Homing Count 증가
            /// </summary>
            public void IncreaseAutoHomingCount() {
                m_cycleData.autoHomingCount++;
                Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.AutoHomingCount}", $"{m_cycleData.autoHomingCount}", m_filePath);
            }
            /// <summary>
            /// Auto Homing Count 클리어
            /// </summary>
            public void ClearAutoHomingCount() {
                m_cycleData.autoHomingCount = 0;
                Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.AutoHomingCount}", $"{m_cycleData.autoHomingCount}", m_filePath);
            }
            /// <summary>
            /// 마지막 Cycle Mode 설정
            /// </summary>
            /// <param name="lastMode"></param>
            /// <returns></returns>
            public bool SetRackMasterCycleMode(MotionMode lastMode) {
                m_cycleData.lastMode = lastMode;
                Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.LastMode}", $"{m_cycleData.lastMode}", m_filePath);
                return true;
            }
            /// <summary>
            /// 랙마스터 구간별 Cycle Time 설정
            /// </summary>
            /// <param name="timeType"></param>
            /// <param name="time"></param>
            /// <returns></returns>
            public bool SetRackMasterCycleTime(CycleTime timeType, int time) {
                switch (timeType) {
                    case CycleTime.From_TotalCycle:
                        m_cycleData.fromCycleTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.From_CycleTime}", $"{m_cycleData.fromCycleTime}", m_filePath);
                        break;

                    case CycleTime.From_XZT_Move:
                        m_cycleData.fromXZTMoveTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.From_XZT_Move_Time}", $"{m_cycleData.fromXZTMoveTime}", m_filePath);
                        break;

                    case CycleTime.From_Fork_FWD:
                        m_cycleData.fromForkFWDMoveTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.From_ForkFWD_Time}", $"{m_cycleData.fromForkFWDMoveTime}", m_filePath);
                        break;

                    case CycleTime.From_Z_Up:
                        m_cycleData.fromZUpTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.From_Z_Up_Time}", $"{m_cycleData.fromZUpTime}", m_filePath);
                        break;

                    case CycleTime.From_Fork_BWD:
                        m_cycleData.fromForkBWDMoveTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.From_ForkBWD_Time}", $"{m_cycleData.fromForkBWDMoveTime}", m_filePath);
                        break;

                    case CycleTime.To_TotalCycle:
                        m_cycleData.toCycleTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.To_CycleTime}", $"{m_cycleData.toCycleTime}", m_filePath);
                        break;

                    case CycleTime.To_XZT_Move:
                        m_cycleData.toXZTMoveTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.To_XZT_Move_Time}", $"{m_cycleData.toXZTMoveTime}", m_filePath);
                        break;

                    case CycleTime.To_Fork_FWD:
                        m_cycleData.toForkFWDMoveTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.To_ForkFWD_Time}", $"{m_cycleData.toForkFWDMoveTime}", m_filePath);
                        break;

                    case CycleTime.To_Z_Down:
                        m_cycleData.toZDownTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.To_Z_Down_Time}", $"{m_cycleData.toZDownTime}", m_filePath);
                        break;

                    case CycleTime.To_Fork_BWD:
                        m_cycleData.toForkBWDMoveTime = time;
                        Ini.SetValueString($"{(int)SectionName.Cycle}", $"{(int)CycleDataList.To_ForkBWD_Time}", $"{m_cycleData.toForkBWDMoveTime}", m_filePath);
                        break;
                }

                return true;
            }
            /// <summary>
            /// 현재 저장된 Cycle Data 로드
            /// </summary>
            /// <returns></returns>
            public RackMasterCycleData GetRackMasterCycleData() {
                return m_cycleData;
            }
            /// <summary>
            /// 랙마스터 Axis 데이터 저장
            /// </summary>
            /// <param name="type"></param>
            /// <param name="data"></param>
            /// <returns></returns>
            public bool SetRackMasterAxisData(AxisDataList type, double data) {
                switch (type) {
                    case AxisDataList.From_XZT_AverageTorque_X:
                        m_axisData.fromXZT_AvrTorque_X = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromXZT_AvrTorque_X}", m_filePath);
                        break;

                    case AxisDataList.From_XZT_AverageTorque_Z:
                        m_axisData.fromXZT_AvrTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromXZT_AvrTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.From_XZT_AverageTorque_T:
                        m_axisData.fromXZT_AvrTorque_T = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromXZT_AvrTorque_T}", m_filePath);
                        break;

                    case AxisDataList.From_XZT_MaxTorque_X:
                        m_axisData.fromXZT_MaxTorque_X = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromXZT_MaxTorque_X}", m_filePath);
                        break;

                    case AxisDataList.From_XZT_MaxTorque_Z:
                        m_axisData.fromXZT_MaxTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromXZT_MaxTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.From_XZT_MaxTorque_T:
                        m_axisData.fromXZT_MaxTorque_T = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromXZT_MaxTorque_T}", m_filePath);
                        break;

                    case AxisDataList.From_ForkFWD_AverageTorque_A:
                        m_axisData.fromForkFWD_AvrTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromForkFWD_AvrTorque_A}", m_filePath);
                        break;

                    case AxisDataList.From_ForkFWD_MaxTorque_A:
                        m_axisData.fromForkFWD_MaxTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromForkFWD_MaxTorque_A}", m_filePath);
                        break;

                    case AxisDataList.From_Z_Up_AverageTorque_Z:
                        m_axisData.fromZUp_AvrTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromZUp_AvrTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.From_Z_Up_MaxTorque_Z:
                        m_axisData.fromZUp_MaxTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromZUp_MaxTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.From_ForkBWD_AverageTorque_A:
                        m_axisData.fromForkBWD_AvrTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromForkBWD_AvrTorque_A}", m_filePath);
                        break;

                    case AxisDataList.From_ForkBWD_MaxTorque_A:
                        m_axisData.fromForkBWD_MaxTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.fromForkBWD_MaxTorque_A}", m_filePath);
                        break;

                    case AxisDataList.To_XZT_AverageTorque_X:
                        m_axisData.toXZT_AvrTorque_X = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toXZT_AvrTorque_X}", m_filePath);
                        break;

                    case AxisDataList.To_XZT_AverageTorque_Z:
                        m_axisData.toXZT_AvrTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toXZT_AvrTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.To_XZT_AverageTorque_T:
                        m_axisData.toXZT_AvrTorque_T = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toXZT_AvrTorque_T}", m_filePath);
                        break;

                    case AxisDataList.To_XZT_MaxTorque_X:
                        m_axisData.toXZT_MaxTorque_X = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toXZT_MaxTorque_X}", m_filePath);
                        break;

                    case AxisDataList.To_XZT_MaxTorque_Z:
                        m_axisData.toXZT_MaxTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toXZT_MaxTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.To_XZT_MaxTorque_T:
                        m_axisData.toXZT_MaxTorque_T = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toXZT_MaxTorque_T}", m_filePath);
                        break;

                    case AxisDataList.To_ForkFWD_AverageTorque_A:
                        m_axisData.toForkFWD_AvrTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toForkFWD_AvrTorque_A}", m_filePath);
                        break;

                    case AxisDataList.To_ForkFWD_MaxTorque_A:
                        m_axisData.toForkFWD_MaxTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toForkFWD_MaxTorque_A}", m_filePath);
                        break;

                    case AxisDataList.To_Z_Down_AverageTorque_Z:
                        m_axisData.toZDown_AvrTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toZDown_AvrTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.To_Z_Down_MaxTorque_Z:
                        m_axisData.toZDown_MaxTorque_Z = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toZDown_MaxTorque_Z}", m_filePath);
                        break;

                    case AxisDataList.To_ForkBWD_AverageToruqe_A:
                        m_axisData.toForkBWD_AvrTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toForkBWD_AvrTorque_A}", m_filePath);
                        break;

                    case AxisDataList.To_ForkBWD_MaxTorque_A:
                        m_axisData.toForkBWD_MaxTorque_A = data;
                        //Ini.SetValueString($"{SectionName.Axis}", $"{type}", $"{m_axisData.toForkBWD_MaxTorque_A}", m_filePath);
                        break;
                }

                return true;
            }
            /// <summary>
            /// 현재 저장된 랙마스터 Axis 데이터 반환
            /// </summary>
            /// <returns></returns>
            public RackMasterAxisData GetRackMasterAxisData() {
                return m_axisData;
            }

            public double GetAxisPeakTorque(AxisList axis) {
                switch (axis) {
                    case AxisList.X_Axis:
                        return MathInterface.GetMax(GetRackMasterAxisData().fromXZT_MaxTorque_X, GetRackMasterAxisData().toXZT_MaxTorque_X);

                    case AxisList.Z_Axis:
                        return MathInterface.GetMax(GetRackMasterAxisData().fromXZT_MaxTorque_Z, GetRackMasterAxisData().fromZUp_MaxTorque_Z,
                            GetRackMasterAxisData().toXZT_MaxTorque_Z, GetRackMasterAxisData().toZDown_MaxTorque_Z);

                    case AxisList.A_Axis:
                        return MathInterface.GetMax(GetRackMasterAxisData().fromForkFWD_MaxTorque_A, GetRackMasterAxisData().fromForkBWD_MaxTorque_A,
                            GetRackMasterAxisData().toForkFWD_MaxTorque_A, GetRackMasterAxisData().toForkBWD_MaxTorque_A);

                    case AxisList.T_Axis:
                        return MathInterface.GetMax(GetRackMasterAxisData().fromXZT_MaxTorque_T, GetRackMasterAxisData().toXZT_MaxTorque_T);
                }

                return 0;
            }

            public double GetAxisAverageTorque(AxisList axis) {
                switch (axis) {
                    case AxisList.X_Axis:
                        return MathInterface.GetAverage(GetRackMasterAxisData().fromXZT_AvrTorque_X, GetRackMasterAxisData().toXZT_AvrTorque_X);

                    case AxisList.Z_Axis:
                        return MathInterface.GetAverage(GetRackMasterAxisData().fromXZT_AvrTorque_Z, GetRackMasterAxisData().fromZUp_AvrTorque_Z,
                            GetRackMasterAxisData().toXZT_AvrTorque_Z, GetRackMasterAxisData().toZDown_AvrTorque_Z);

                    case AxisList.A_Axis:
                        return MathInterface.GetAverage(GetRackMasterAxisData().fromForkFWD_AvrTorque_A, GetRackMasterAxisData().fromForkBWD_AvrTorque_A,
                            GetRackMasterAxisData().toForkFWD_AvrTorque_A, GetRackMasterAxisData().toForkBWD_AvrTorque_A);

                    case AxisList.T_Axis:
                        return MathInterface.GetAverage(GetRackMasterAxisData().fromXZT_AvrTorque_T, GetRackMasterAxisData().toXZT_AvrTorque_T);
                }

                return 0;
            }
        }
    }
}
