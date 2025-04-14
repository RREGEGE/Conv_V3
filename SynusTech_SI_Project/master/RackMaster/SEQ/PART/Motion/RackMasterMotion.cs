using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RackMaster.SEQ.COMMON;
using MovenCore;

namespace RackMaster.SEQ.PART {
    /// <summary>
    /// 각 축의 Step을 나누어 여러 명령이 겹치지 않도록 한다.
    /// </summary>
    public enum AxisStep
    {
        Idle,                   // 유휴 상태
        Started,                // 시작 명령이 내려온 상태. 실제로 축이 움직인 경우 다음 스텝으로 전환
        ServoInpositionCheck,   // 해당 축이 지령 위치에 도달했는지 판단
        HomeStarted,            // Homing 시작
        HomeCheck,              // Homing 완료 되었는지 체크
        Finished,               // 해당 축이 지령 위치에 도착하여 끝난 경우
        Stop,                   // Stop 명령이 들어왔을 경우
    }

    public partial class RackMasterMain {
        public partial class RackMasterMotion {
            public enum ZOverrideType {
                Slow,
                Fast
            }

            public enum ForkMoveType {
                Forward,
                Backward
            }
            /// <summary>
            /// From/To 동작에서 X,Z,T 축 명령 함수
            /// </summary>
            /// <param name="id"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public bool XZTMove(int id, MotionStepType type) {
                if (!m_teaching.IsExistPortOrShelf(id) || !m_teaching.IsExistTeachingData(id) || !m_teaching.IsEnablePort(id))
                    return false;

                TeachingValueData target = m_teaching.GetTeachingData(id);
                // Interpolation 모션 사용 할 경우
                if (m_param.GetMotionParam().useInterpolation) {
                    // X축을 Full Closed 모션으로 사용할 경우
                    if (m_param.GetMotionParam().useFullClosed) {
                        AxesProfile profile = new AxesProfile();
                        AxisProfile profile_noTurn = new AxisProfile();
                        
                        // Fork Type이 Slide-No Turn인 경우 Turn축이 없으므로 따로 지령 저장
                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            profile_noTurn.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                            profile_noTurn.m_profileType = WMXParam.m_profileType.JerkRatio;
                            if (m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                if (type == MotionStepType.From) {
                                    profile_noTurn.m_dest = (float)(target.valZ - target.valZDown);
                                }
                                else if (type == MotionStepType.To) {
                                    profile_noTurn.m_dest = (float)(target.valZ + target.valZUp);
                                }
                            }
                            else {
                                int dia = RadianToCalculateDia((double)target.valZ);
                                if (type == MotionStepType.From) {
                                    profile_noTurn.m_dest = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                                }
                                else if (type == MotionStepType.To) {
                                    profile_noTurn.m_dest = (float)(target.valZ + DistanceToRadian(dia, (double)target.valZUp));
                                }
                            }
                            profile_noTurn.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                            profile_noTurn.m_acc = profile_noTurn.m_dec = m_main.IsAutoState() ? profile_noTurn.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                                (m_speedType == ManualSpeedType.High) ? profile_noTurn.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile_noTurn.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;
                        }
                        else {
                            profile.m_axisCount = 2;
                            profile.m_axisArray[0] = m_param.GetAxisNumber(AxisList.Z_Axis);
                            profile.m_axisArray[1] = m_param.GetAxisNumber(AxisList.T_Axis);

                            profile.m_profileType = WMXParam.m_profileType.SCurve;
                            if (m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                                if (type == MotionStepType.From) {
                                    profile.m_dest[0] = (float)(target.valZ - target.valZDown);
                                }
                                else if (type == MotionStepType.To) {
                                    profile.m_dest[0] = (float)(target.valZ + target.valZUp);
                                }
                            }
                            else {
                                int dia = RadianToCalculateDia((double)target.valZ);
                                if (type == MotionStepType.From) {
                                    profile.m_dest[0] = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                                }
                                else if (type == MotionStepType.To) {
                                    profile.m_dest[0] = (float)(target.valZ + DistanceToRadian(dia, (double)target.valZUp));
                                }
                            }
                            profile.m_dest[1] = (float)target.valT;

                            profile.m_maxVel[0] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                            profile.m_maxVel[1] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.T_Axis) : GetManualSpeed(AxisList.T_Axis, m_speedType);

                            profile.m_maxAcc[0] = profile.m_maxDec[0] = m_main.IsAutoState() ? profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                                (m_speedType == ManualSpeedType.High) ? profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;
                            profile.m_maxAcc[1] = profile.m_maxDec[1] = m_main.IsAutoState() ? profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec :
                                (m_speedType == ManualSpeedType.High) ? profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.T_Axis).manualHighAccDec : profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.T_Axis).manualLowAccDec;

                        }
                        BarcodeClosedLoopCommand fcCmd = new BarcodeClosedLoopCommand();
                        fcCmd.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.X_Axis) : GetManualSpeed(AxisList.X_Axis, m_speedType);
                        fcCmd.m_acc = fcCmd.m_dec = m_main.IsAutoState() ? fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).manualHighAccDec : fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).manualLowAccDec;
                        fcCmd.m_pGain = m_param.GetFullClosedPGain();
                        fcCmd.m_iGain = m_param.GetFullClosedIGain();
                        fcCmd.m_targetDistance = (double)target.valX;

                        bool ret = false;
                        if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            ret = m_axis.AbsoluteMove(profile_noTurn);
                        }
                        else {
                            ret = m_axis.InterpolationMove(profile);
                        }
                        bool retFullClosed = m_axis.StartFullClosedMotion(fcCmd);
                        if (ret && retFullClosed) {
                            SetAxisStep(AxisStep.Started, AxisList.X_Axis, fcCmd.m_targetDistance);
                            if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                                SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile_noTurn.m_dest);
                            }
                            else {
                                SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest[0]);
                                SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile.m_dest[1]);
                            }

                            return true;
                        }

                        return false;
                    }
                    else { // X축을 Full Closed로 사용하지 않을 경우
                        AxesProfile profile = new AxesProfile();
                        //profile.m_axisCount = 3;
                        profile.m_axisArray[0] = m_param.GetAxisNumber(AxisList.X_Axis);
                        profile.m_axisArray[1] = m_param.GetAxisNumber(AxisList.Z_Axis);
                        profile.m_axisArray[2] = m_param.GetAxisNumber(AxisList.T_Axis);

                        profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.X_Axis).jerkRatio / 100;

                        profile.m_dest[0] = (float)target.valX;
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            if (type == MotionStepType.From) {
                                profile.m_dest[1] = (float)(target.valZ - target.valZDown);
                            }
                            else if (type == MotionStepType.To) {
                                profile.m_dest[1] = (float)(target.valZ + target.valZUp);
                            }
                        }
                        else {
                            int dia = RadianToCalculateDia((double)target.valZ);
                            if (type == MotionStepType.From) {
                                profile.m_dest[1] = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                            }
                            else if (type == MotionStepType.To) {
                                profile.m_dest[1] = (float)(target.valZ + DistanceToRadian(dia, (double)target.valZUp));
                            }
                        }
                        profile.m_dest[2] = (float)target.valT;

                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"X-Axis Target Position : {profile.m_dest[0]}"));
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Z-Axis Target Position : {profile.m_dest[1]}"));
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"T-Axis Target Position : {profile.m_dest[2]}"));
                        profile.m_maxVel[0] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.X_Axis) : GetManualSpeed(AxisList.X_Axis, m_speedType);
                        profile.m_maxVel[1] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                        profile.m_maxVel[2] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.T_Axis) : GetManualSpeed(AxisList.T_Axis, m_speedType);
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"X-Axis Max Velocity : {profile.m_maxVel[0]}"));
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Z-Axis Max Velocity : {profile.m_maxVel[1]}"));
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"T-Axis Max Velocity : {profile.m_maxVel[2]}"));

                        profile.m_maxAcc[0] = profile.m_maxDec[0] = m_main.IsAutoState() ? profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.X_Axis).manualHighAccDec : profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.X_Axis).manualLowAccDec;
                        profile.m_maxAcc[1] = profile.m_maxDec[1] = m_main.IsAutoState() ? profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;
                        profile.m_maxAcc[2] = profile.m_maxDec[2] = m_main.IsAutoState() ? profile.m_maxVel[2] / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile.m_maxVel[2] / m_param.GetAxisParameter(AxisList.T_Axis).manualHighAccDec : profile.m_maxVel[2] / m_param.GetAxisParameter(AxisList.T_Axis).manualLowAccDec;
                       
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"X-Axis Max Acc : {profile.m_maxAcc[0]}"));
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"Z-Axis Max Acc : {profile.m_maxAcc[1]}"));
                        Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"T-Axis Max Acc : {profile.m_maxAcc[2]}"));
                        
                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            profile.m_axisCount = 2;
                        }
                        else {
                            profile.m_axisCount = 3;
                        }

                        bool ret = m_axis.InterpolationMove(profile);
                        if (ret) {
                            SetAxisStep(AxisStep.Started, AxisList.X_Axis, profile.m_dest[0]);
                            SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest[1]);
                            if(m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile.m_dest[2]);
                            }
                        }
                        return ret;
                    }
                }
                else { // Interpolation 모션을 사용하지 않을 경우
                    // X축을 Full Closed로 사용할 경우
                    if (m_param.GetMotionParam().useFullClosed) {
                        AxisProfile[] profile = new AxisProfile[2];
                        for (int i = 0; i < profile.Length; i++) {
                            profile[i] = new AxisProfile();
                        }

                        BarcodeClosedLoopCommand fcCmd = new BarcodeClosedLoopCommand();
                        fcCmd.m_targetDistance = (float)target.valX;
                        fcCmd.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.X_Axis) : GetManualSpeed(AxisList.X_Axis, m_speedType);
                        fcCmd.m_acc = fcCmd.m_dec = m_main.IsAutoState() ? fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).manualHighAccDec : fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).manualLowAccDec;
                        fcCmd.m_pGain = m_param.GetFullClosedPGain();
                        fcCmd.m_iGain = m_param.GetFullClosedIGain();

                        profile[0].m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            if (type == MotionStepType.From) {
                                profile[0].m_dest = (float)(target.valZ - target.valZDown);
                            }
                            else if (type == MotionStepType.To) {
                                profile[0].m_dest = (float)(target.valZ + target.valZUp);
                            }
                        }
                        else {
                            int dia = RadianToCalculateDia((double)target.valZ);
                            if (type == MotionStepType.From) {
                                profile[0].m_dest = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                            }
                            else if (type == MotionStepType.To) {
                                profile[0].m_dest = (float)(target.valZ + DistanceToRadian(dia, (double)target.valZUp));
                            }
                        }
                        profile[0].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[0].m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;

                        profile[1].m_axis = m_param.GetAxisNumber(AxisList.T_Axis);
                        profile[1].m_dest = (float)target.valT;
                        profile[1].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[1].m_jerkRatio = m_param.GetAxisParameter(AxisList.T_Axis).jerkRatio / 100;

                        profile[0].m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                        profile[1].m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.T_Axis) : GetManualSpeed(AxisList.T_Axis, m_speedType);

                        profile[0].m_acc = profile[0].m_dec = m_main.IsAutoState() ? profile[0].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile[0].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile[0].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;
                        profile[1].m_acc = profile[1].m_dec = m_main.IsAutoState() ? profile[1].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile[1].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).manualHighAccDec : profile[1].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).manualLowAccDec;

                        bool ret = false;

                        if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            ret = m_axis.AbsoluteMove(profile[0]);
                        }
                        else {
                            for (int i = 0; i < profile.Length; i++) {
                                ret = m_axis.AbsoluteMove(profile[i]);
                                if (!ret) {
                                    return false;
                                }
                            }
                        }
                        ret = m_axis.StartFullClosedMotion(fcCmd);
                        if (!ret) {
                            return false;
                        }

                        SetAxisStep(AxisStep.Started, AxisList.X_Axis, fcCmd.m_targetDistance);
                        SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile[0].m_dest);
                        if(m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                            SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile[1].m_dest);
                        }

                        return true;
                    }
                    else { // X축을 Full Closed로 사용하지 않을 경우
                        AxisProfile[] profile = new AxisProfile[3];
                        for (int i = 0; i < profile.Length; i++) {
                            profile[i] = new AxisProfile();
                        }

                        profile[0].m_axis = m_param.GetAxisNumber(AxisList.X_Axis);
                        profile[0].m_dest = (float)target.valX;
                        profile[0].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[0].m_jerkRatio = m_param.GetAxisParameter(AxisList.X_Axis).jerkRatio / 100;

                        profile[1].m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            if (type == MotionStepType.From) {
                                profile[1].m_dest = (float)(target.valZ - target.valZDown);
                            }
                            else if (type == MotionStepType.To) {
                                profile[1].m_dest = (float)(target.valZ + target.valZUp);
                            }
                        }
                        else {
                            int dia = RadianToCalculateDia((double)target.valZ);
                            if (type == MotionStepType.From) {
                                profile[1].m_dest = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                            }
                            else if (type == MotionStepType.To) {
                                profile[1].m_dest = (float)(target.valZ + DistanceToRadian(dia, (double)target.valZUp));
                            }
                        }
                        profile[1].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[1].m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;

                        profile[2].m_axis = m_param.GetAxisNumber(AxisList.T_Axis);
                        profile[2].m_dest = (float)target.valT;
                        profile[2].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[2].m_jerkRatio = m_param.GetAxisParameter(AxisList.T_Axis).jerkRatio / 100;

                        profile[0].m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.X_Axis) : GetManualSpeed(AxisList.X_Axis, m_speedType);
                        profile[1].m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                        profile[2].m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.T_Axis) : GetManualSpeed(AxisList.T_Axis, m_speedType);

                        profile[0].m_acc = profile[0].m_dec = m_main.IsAutoState() ? profile[0].m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile[0].m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).manualHighAccDec : profile[0].m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).manualLowAccDec;
                        profile[1].m_acc = profile[1].m_dec = m_main.IsAutoState() ? profile[1].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile[1].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile[1].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;
                        profile[2].m_acc = profile[2].m_dec = m_main.IsAutoState() ? profile[2].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile[2].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).manualHighAccDec : profile[2].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).manualLowAccDec;

                        bool ret = false;

                        for (int i = 0; i < profile.Length; i++) {
                            if(m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && i == 2) {
                                continue;
                            }

                            ret = m_axis.AbsoluteMove(profile[i]);
                            if (!ret) {
                                return false;
                            }
                        }
                        SetAxisStep(AxisStep.Started, AxisList.X_Axis, profile[0].m_dest);
                        SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile[1].m_dest);
                        if(m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                            SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile[2].m_dest);
                        }

                        return true;
                    }
                }
            }
            /// <summary>
            /// From/To 동작에서의 Z축 Up/Down 명령
            /// </summary>
            /// <param name="id"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public bool ZMove(int id, MotionStepType type) {
                if (!m_teaching.IsEnablePort(id) || !m_teaching.IsExistPortOrShelf(id) || !m_teaching.IsExistTeachingData(id))
                    return false;

                TeachingValueData target = m_teaching.GetTeachingData(id);

                AxisProfile profile = new AxisProfile();

                profile.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                //profile.m_dest = (float)target.valZ;

                // Z축 타입이 Drum Belt에 따라 지령 데이터 계산
                if (m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                    if (type == MotionStepType.From) {
                        profile.m_dest = (float)(target.valZ + target.valZUp);
                    }
                    else if (type == MotionStepType.To) {
                        profile.m_dest = (float)(target.valZ - target.valZDown);
                    }
                }
                else {
                    int dia = RadianToCalculateDia((double)target.valZ);
                    if (type == MotionStepType.From) {
                        profile.m_dest = (float)(target.valZ + DistanceToRadian(dia, (double)target.valZUp));
                    }
                    else if (type == MotionStepType.To) {
                        profile.m_dest = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                    }
                }

                profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;
                profile.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) / 3 : GetManualSpeed(AxisList.Z_Axis, m_speedType) / 3;
                profile.m_acc = profile.m_dec = m_main.IsAutoState() ? profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                    (m_speedType == ManualSpeedType.High) ? profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;

                bool ret = m_axis.AbsoluteMove(profile);
                if (ret)
                    SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest);
                return ret;
            }
            /// <summary>
            /// From/To 동작에서의 Z축 Override 명령
            /// </summary>
            /// <param name="id"></param>
            /// <param name="type"></param>
            /// <param name="overrideType"></param>
            /// <returns></returns>
            public bool ZMoveOverride(int id, MotionStepType type, ZOverrideType overrideType) {
                if (!m_teaching.IsEnablePort(id) || !m_teaching.IsExistPortOrShelf(id) || !m_teaching.IsExistTeachingData(id))
                    return false;

                TeachingValueData target = m_teaching.GetTeachingData(id);

                AxisProfile profile = new AxisProfile();
                TriggerCondition trig = new TriggerCondition();

                profile.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;
                if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                    if (type == MotionStepType.From) {
                        profile.m_dest = (float)(target.valZ + target.valZUp);
                    }
                    else if (type == MotionStepType.To) {
                        profile.m_dest = (float)(target.valZ - target.valZDown);
                    }
                }
                else {
                    int dia = RadianToCalculateDia((double)target.valZ);
                    if (type == MotionStepType.From) {
                        profile.m_dest = (float)(target.valZ + DistanceToRadian(dia, (double)target.valZUp));
                    }
                    else if (type == MotionStepType.To) {
                        profile.m_dest = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                    }
                }
                trig.m_triggerAxis = m_param.GetAxisNumber(AxisList.Z_Axis);

                float overridePercent = m_param.GetMotionParam().Z_OverridePercent / 100;
                switch (overrideType) {
                    case ZOverrideType.Slow:
                        profile.m_velocity = m_main.IsAutoState() ? (GetAutoSpeed(AxisList.Z_Axis) * overridePercent) : (GetManualSpeed(AxisList.Z_Axis, m_speedType) * overridePercent);
                        profile.m_acc = profile.m_dec = m_main.IsAutoState() ? (m_param.GetWMX3AxisAutoVelocity(AxisList.Z_Axis) / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec) : 
                           (m_speedType == ManualSpeedType.High) ? m_param.GetWMX3AxisManualVelocity(AxisList.Z_Axis, m_speedType) / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : m_param.GetWMX3AxisManualVelocity(AxisList.Z_Axis, m_speedType) / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            if (type == MotionStepType.From) {
                                //trig.m_triggerValue = m_param.GetMotionParam().Z_OverrideFromDownDist * 1000;
                                //trig.m_triggerValue = ((float)target.valZUp + (m_param.GetMotionParam().Z_OverrideFromDownDist * 1000));
                                trig.m_triggerType = WMXParam.m_triggerType.CompletedDistance;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    trig.m_triggerValue = GetPortParameter(id).fromDownPosition * 1000;
                                }
                                else {
                                    trig.m_triggerValue = m_param.GetMotionParam().Z_OverrideFromDownDist * 1000;
                                }
                            }
                            else if (type == MotionStepType.To) {
                                //trig.m_triggerValue = m_param.GetMotionParam().Z_OverrideToUpDist * 1000;
                                //trig.m_triggerValue = ((float)target.valZDown + (m_param.GetMotionParam().Z_OverrideFromUpDist * 1000));
                                trig.m_triggerType = WMXParam.m_triggerType.RemainginDistance;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    trig.m_triggerValue = GetPortParameter(id).toUpPosition * 1000;
                                }
                                else {
                                    trig.m_triggerValue = m_param.GetMotionParam().Z_OverrideToUpDist * 1000;
                                }
                            }
                        }
                        else {
                            int dia = RadianToCalculateDia((double)target.valZ);
                            if (type == MotionStepType.From) {
                                double downDist = 0;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    downDist = GetPortParameter(id).fromDownPosition * 1000;
                                }
                                else {
                                    downDist = m_param.GetMotionParam().Z_OverrideFromDownDist * 1000;
                                }
                                //trig.m_triggerValue = DistanceToRadian(dia, (double)target.valZUp) + DistanceToRadian(dia, downDist);
                                trig.m_triggerType = WMXParam.m_triggerType.CompletedDistance;
                                trig.m_triggerValue = DistanceToRadian(dia, downDist);
                            }
                            else if (type == MotionStepType.To) {
                                double upDist = 0;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    upDist = GetPortParameter(id).toUpPosition * 1000;
                                }
                                else {
                                    upDist = m_param.GetMotionParam().Z_OverrideToUpDist * 1000;
                                }
                                trig.m_triggerType = WMXParam.m_triggerType.RemainginDistance;
                                //trig.m_triggerValue = DistanceToRadian(dia, (double)target.valZDown) + DistanceToRadian(dia, upDist);
                                trig.m_triggerValue = DistanceToRadian(dia, upDist);
                            }
                        }
                        break;

                    case ZOverrideType.Fast:
                        profile.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            if (type == MotionStepType.From) {
                                trig.m_triggerType = WMXParam.m_triggerType.CompletedDistance;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    trig.m_triggerValue = (GetPortParameter(id).fromUpPosition - GetPortParameter(id).fromDownPosition) * 1000;
                                }
                                else {
                                    trig.m_triggerValue = (m_param.GetMotionParam().Z_OverrideFromUpDist - m_param.GetMotionParam().Z_OverrideFromDownDist) * 1000;
                                }
                            }
                            else if (type == MotionStepType.To) {
                                trig.m_triggerType = WMXParam.m_triggerType.RemainginDistance;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    trig.m_triggerValue = GetPortParameter(id).toDownPosition * 1000;
                                }
                                else {
                                    trig.m_triggerValue = m_param.GetMotionParam().Z_OverrideToDownDist * 1000;
                                }
                            }
                        }
                        else {
                            int dia = RadianToCalculateDia((double)target.valZ);
                            if (type == MotionStepType.From) {
                                double fromDist = 0;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    fromDist = (GetPortParameter(id).fromUpPosition * 1000) - (GetPortParameter(id).fromDownPosition * 1000);
                                }
                                else {
                                    fromDist = (m_param.GetMotionParam().Z_OverrideFromUpDist * 1000) - (m_param.GetMotionParam().Z_OverrideFromDownDist * 1000);
                                }
                                trig.m_triggerType = WMXParam.m_triggerType.CompletedDistance;
                                trig.m_triggerValue = DistanceToRadian(dia, fromDist);
                            }
                            else if (type == MotionStepType.To) {
                                double toDist = 0;
                                if(m_teaching.GetPortType(id) != PortType.SHELF) {
                                    toDist = GetPortParameter(id).toDownPosition * 1000;
                                }
                                else {
                                    toDist = m_param.GetMotionParam().Z_OverrideToDownDist * 1000;
                                }
                                trig.m_triggerType = WMXParam.m_triggerType.RemainginDistance;
                                trig.m_triggerValue = DistanceToRadian(dia, toDist);
                            }
                        }
                        break;
                }
                bool ret = m_axis.AbsoluteMove(profile, trig);
                if (ret)
                    SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest);
                return ret;
            }
            /// <summary>
            /// Source Empty 발생 시 Z축 Down 명령
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool ZDown(int id) {
                if (!m_main.Interlock_PortOrShelfReady(id)) return false;

                TeachingValueData target = m_teaching.GetTeachingData(id);

                AxisProfile profile = new AxisProfile();
                profile.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;
                profile.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                profile.m_acc = profile.m_dec = m_main.IsAutoState() ? profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                    (m_speedType == ManualSpeedType.High) ? profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec;
                if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                    profile.m_dest = (float)(target.valZ - target.valZDown);
                }
                else {
                    int dia = RadianToCalculateDia((double)target.valZ);
                    profile.m_dest = (float)(target.valZ - DistanceToRadian(dia, (double)target.valZDown));
                }

                bool ret = m_axis.AbsoluteMove(profile);
                if (ret)
                    SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest);
                return ret;
            }
            /// <summary>
            /// From/To 동작 중 Fork 전진/후진
            /// </summary>
            /// <param name="id"></param>
            /// <param name="moveType"></param>
            /// <returns></returns>
            public bool ForkMove(int id, ForkMoveType moveType) {
                if (!m_teaching.IsEnablePort(id) || !m_teaching.IsExistPortOrShelf(id) || !m_teaching.IsExistTeachingData(id))
                    return false;

                if (id == 0)
                    return false;

                TeachingValueData target = m_teaching.GetTeachingData(id);
                // Fork Type이 SCARA 인 경우
                if (m_param.GetMotionParam().forkType == ForkType.SCARA) {
                    AxesProfile profile = new AxesProfile();

                    profile.m_axisCount = 2;
                    profile.m_axisArray[0] = m_param.GetAxisNumber(AxisList.A_Axis);
                    profile.m_axisArray[1] = m_param.GetAxisNumber(AxisList.T_Axis);
                    profile.m_profileType = WMXParam.m_profileType.JerkRatio;

                    profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.A_Axis).jerkRatio / 100;

                    if (moveType == ForkMoveType.Forward) {
                        profile.m_dest[0] = (float)target.valFork_A;
                        profile.m_dest[1] = (float)target.valFork_T;
                    }
                    else if (moveType == ForkMoveType.Backward) {
                        profile.m_dest[0] = 0;
                        profile.m_dest[1] = (float)target.valT;
                    }

                    profile.m_maxVel[0] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.A_Axis) : GetManualSpeed(AxisList.A_Axis, m_speedType);
                    profile.m_maxVel[1] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.A_Axis) : GetManualSpeed(AxisList.A_Axis, m_speedType);

                    profile.m_maxAcc[0] = profile.m_maxDec[0] = m_main.IsAutoState() ? profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.A_Axis).maxAccDec :
                        (m_speedType == ManualSpeedType.High) ? profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.A_Axis).manualHighAccDec : profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.A_Axis).manualLowAccDec;
                    profile.m_maxAcc[1] = profile.m_maxDec[1] = m_main.IsAutoState() ? profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.A_Axis).maxAccDec :
                        (m_speedType == ManualSpeedType.High) ? profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.A_Axis).manualHighAccDec : profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.A_Axis).manualLowAccDec;

                    bool ret = m_axis.InterpolationMove(profile);
                    if (ret) {
                        SetAxisStep(AxisStep.Started, AxisList.A_Axis, profile.m_dest[0]);
                        SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile.m_dest[1]);
                    }
                    //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"A Speed:{profile.m_maxVel[0]}, T Speed:{profile.m_maxVel[1]}"));
                    return ret;
                }
                else { // Fork Type이 SCARA가 아닌 경우
                    AxisProfile profile = new AxisProfile();

                    profile.m_axis = m_param.GetAxisNumber(AxisList.A_Axis);
                    profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                    profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.A_Axis).jerkRatio / 100;
                    if (moveType == ForkMoveType.Forward) {
                        profile.m_dest = (double)target.valFork_A;
                    }
                    else if (moveType == ForkMoveType.Backward) {
                        profile.m_dest = 0;
                    }
                    profile.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.A_Axis) : GetManualSpeed(AxisList.A_Axis, m_speedType);
                    profile.m_acc = profile.m_dec = m_main.IsAutoState() ? profile.m_velocity / m_param.GetAxisParameter(AxisList.A_Axis).maxAccDec :
                        (m_speedType == ManualSpeedType.High) ? profile.m_velocity / m_param.GetAxisParameter(AxisList.A_Axis).manualHighAccDec : profile.m_velocity / m_param.GetAxisParameter(AxisList.A_Axis).manualLowAccDec;

                    bool ret = m_axis.AbsoluteMove(profile);
                    if (ret) {
                        SetAxisStep(AxisStep.Started, AxisList.A_Axis, profile.m_dest);
                    }
                    //Log.Add(new Log.LogItem(Log.LogLevel.Normal, Log.LogType.RackMaster, $"No SCARA, Fork Move, Dest={profile.m_dest:F3}"));
                    return ret;
                }
            }
            /// <summary>
            /// Fork 축을 Home 위치로 이동
            /// </summary>
            /// <returns></returns>
            public bool ForkHoming() {
                switch (m_param.GetMotionParam().forkType) {
                    case ForkType.SCARA:
                        AxesProfile profiles = new AxesProfile();

                        profiles.m_axisCount = 2;
                        profiles.m_axisArray[0] = m_param.GetAxisNumber(AxisList.A_Axis);
                        profiles.m_axisArray[1] = m_param.GetAxisNumber(AxisList.T_Axis);
                        profiles.m_profileType = WMXParam.m_profileType.JerkRatio;
                        profiles.m_jerkRatio = m_param.GetAxisParameter(AxisList.A_Axis).jerkRatio / 100;
                        profiles.m_dest[0] = 0;
                        profiles.m_dest[1] = (GetAxisStatus(AxisStatusType.pos_act, AxisList.T_Axis) - GetAxisStatus(AxisStatusType.pos_act, AxisList.A_Axis));
                        profiles.m_maxVel[0] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.A_Axis) : GetManualSpeed(AxisList.A_Axis, m_speedType);
                        profiles.m_maxVel[1] = m_main.IsAutoState() ? GetAutoSpeed(AxisList.A_Axis) : GetManualSpeed(AxisList.A_Axis, m_speedType);

                        profiles.m_maxAcc[0] = profiles.m_maxDec[0] = m_main.IsAutoState() ? profiles.m_maxVel[0] / m_param.GetAxisParameter(AxisList.A_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profiles.m_maxVel[0] / m_param.GetAxisParameter(AxisList.A_Axis).manualHighAccDec : profiles.m_maxVel[0] / m_param.GetAxisParameter(AxisList.A_Axis).manualLowAccDec;
                        profiles.m_maxAcc[1] = profiles.m_maxDec[1] = m_main.IsAutoState() ? profiles.m_maxVel[1] / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profiles.m_maxVel[1] / m_param.GetAxisParameter(AxisList.T_Axis).manualHighAccDec : profiles.m_maxVel[1] / m_param.GetAxisParameter(AxisList.T_Axis).manualLowAccDec;

                        if (!m_axis.InterpolationMove(profiles))
                            return false;

                        SetAxisStep(AxisStep.Started, AxisList.A_Axis, profiles.m_dest[0]);
                        SetAxisStep(AxisStep.Started, AxisList.T_Axis, profiles.m_dest[1]);
                        return true;

                    case ForkType.Slide:
                    case ForkType.Slide_NoTurn:
                        AxisProfile profile = new AxisProfile();

                        profile.m_axis = m_param.GetAxisNumber(AxisList.A_Axis);
                        profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.A_Axis).jerkRatio / 100;
                        profile.m_dest = 0;
                        profile.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.A_Axis) : GetManualSpeed(AxisList.A_Axis, m_speedType);
                        profile.m_acc = profile.m_dec = m_main.IsAutoState() ? profile.m_velocity / m_param.GetAxisParameter(AxisList.A_Axis).maxAccDec :
                            (m_speedType == ManualSpeedType.High) ? profile.m_velocity / m_param.GetAxisParameter(AxisList.A_Axis).manualHighAccDec : profile.m_velocity / m_param.GetAxisParameter(AxisList.A_Axis).manualLowAccDec;

                        if (!m_axis.AbsoluteMove(profile))
                            return false;

                        SetAxisStep(AxisStep.Started, AxisList.A_Axis, profile.m_dest);
                        return true;
                }

                return false;
            }
            /// <summary>
            /// Fork Home 위치 이동 명령 스탑
            /// </summary>
            public void ForkHomingStop() {
                if(m_param.GetMotionParam().forkType == ForkType.SCARA) {
                    m_axis.Stop((int)m_param.GetAxisNumber(AxisList.A_Axis));
                    m_axis.Stop((int)m_param.GetAxisNumber(AxisList.T_Axis));
                }
                else {
                    m_axis.Stop((int)m_param.GetAxisNumber(AxisList.A_Axis));
                }
            }
            /// <summary>
            /// Z축 Auto Homing이 진행 될 때 Home 위치에 가까운 위치로 이동
            /// </summary>
            /// <returns></returns>
            public bool ZAxisAutoHomingMove() {
                if (!ForkHomeCheck())
                    return false;

                AxisProfile profile = new AxisProfile();

                profile.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                profile.m_dest = 30;
                profile.m_velocity = m_main.IsAutoState() ? GetAutoSpeed(AxisList.Z_Axis) : GetManualSpeed(AxisList.Z_Axis, m_speedType);
                profile.m_acc = profile.m_dec = m_main.IsAutoState() ? profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec :
                    (m_speedType == ManualSpeedType.High ? profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualHighAccDec : profile.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).manualLowAccDec);
                profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;

                if (!m_axis.AbsoluteMove(profile))
                    return false;

                SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest);
                return true;
            }
            /// <summary>
            /// Z축 Auto Homing 시작
            /// </summary>
            /// <returns></returns>
            public bool ZAxisAutoHoming() {
                if (!ForkHomeCheck())
                    return false;

                bool ret = m_axis.HomeStart(m_param.GetAxisNumber(AxisList.Z_Axis));

                if (!ret)
                    return false;

                SetAxisStep(AxisStep.HomeStarted, AxisList.Z_Axis);

                return true;
            }
            /// <summary>
            /// Auto Teaching 시 X,Z축 타겟 위치로 이동
            /// </summary>
            private void AutoTeachingXZMove() {
                // X축 Full Closed 사용할 경우
                if (m_param.GetMotionParam().useFullClosed) {
                    AxisProfile profile = new AxisProfile();

                    profile.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                    profile.m_profileType = WMXParam.m_profileType.SCurve;
                    if (m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                        profile.m_velocity = m_param.GetAutoTeachingParam().autoTeachingTargetSpeedZ * 1000;
                        profile.m_dest = m_autoTeachingTarget.targetZ;
                    }
                    else {
                        double speed = m_param.GetAutoTeachingParam().autoTeachingTargetSpeedZ * 1000;
                        profile.m_velocity = DistanceToRadian(m_param.GetMotionParam().ZAxisBeltFirstDia, speed);
                        profile.m_dest = DistanceToCalculatedRadian(m_autoTeachingTarget.targetZ);
                    }
                    profile.m_acc = profile.m_dec = profile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingTargetAccDecZ;

                    BarcodeClosedLoopCommand xProfile = new BarcodeClosedLoopCommand();
                    xProfile.m_targetDistance = m_autoTeachingTarget.targetX;
                    xProfile.m_velocity = m_param.GetAutoTeachingParam().autoTeachingTargetSpeedX * 1000;
                    xProfile.m_acc = xProfile.m_dec = xProfile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingTargetAccDecX;
                    xProfile.m_pGain = m_param.GetFullClosedPGain();
                    xProfile.m_iGain = m_param.GetFullClosedIGain();

                    m_axis.AbsoluteMove(profile);
                    m_axis.StartFullClosedMotion(xProfile);
                    SetAxisStep(AxisStep.Started, AxisList.X_Axis, xProfile.m_targetDistance);
                    SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest);
                }
                else { // X축 Full Closed 사용하지 않을 경우
                    AxisProfile[] profiles = new AxisProfile[2];
                    for (int i = 0; i < profiles.Length; i++)
                        profiles[i] = new AxisProfile();

                    profiles[0].m_axis = m_param.GetAxisNumber(AxisList.X_Axis);
                    profiles[0].m_profileType = WMXParam.m_profileType.JerkRatio;
                    profiles[0].m_dest = m_autoTeachingTarget.targetX;
                    profiles[0].m_velocity = m_param.GetAutoTeachingParam().autoTeachingTargetSpeedX * 1000;
                    profiles[0].m_acc = profiles[0].m_velocity / m_param.GetAutoTeachingParam().autoTeachingTargetAccDecX;
                    profiles[0].m_dec = profiles[0].m_acc;

                    profiles[0].m_jerkRatio = m_param.GetAxisParameter(AxisList.X_Axis).jerkRatio / 100;

                    profiles[1].m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                    profiles[1].m_profileType = WMXParam.m_profileType.JerkRatio;
                    if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                        profiles[1].m_velocity = m_param.GetAutoTeachingParam().autoTeachingTargetSpeedZ * 1000;
                        profiles[1].m_dest = m_autoTeachingTarget.targetZ;
                    }
                    else {
                        double speed = m_param.GetAutoTeachingParam().autoTeachingTargetSpeedZ * 1000;
                        profiles[1].m_velocity = DistanceToRadian(m_param.GetMotionParam().ZAxisBeltFirstDia, speed);
                        profiles[1].m_dest = DistanceToCalculatedRadian(m_autoTeachingTarget.targetZ);
                    }
                    profiles[1].m_acc = profiles[0].m_velocity / m_param.GetAutoTeachingParam().autoTeachingTargetAccDecZ;
                    profiles[1].m_dec = profiles[0].m_acc;
                    profiles[1].m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;

                    m_axis.AbsoluteMove(profiles[0]);
                    m_axis.AbsoluteMove(profiles[1]);
                    SetAxisStep(AxisStep.Started, AxisList.X_Axis, profiles[0].m_dest);
                    SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profiles[1].m_dest);
                }
            }
            /// <summary>
            /// Auto Teaching 시 Turn 축 이동
            /// </summary>
            /// <returns></returns>
            private bool AutoTeachingTurnMove() {
                AxisProfile profile = new AxisProfile();
                TeachingValueData port = m_teaching.GetTeachingData(m_autoTeachingTarget.id);

                if (port.valT == null) {
                    return false;
                }

                profile.m_axis = m_param.GetAxisNumber(AxisList.T_Axis);
                profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.T_Axis).jerkRatio / 100;
                profile.m_velocity = m_param.GetWMX3AxisAutoVelocity(AxisList.T_Axis);
                profile.m_acc = profile.m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec;
                profile.m_dec = profile.m_acc;
                profile.m_dest = (double)port.valT;

                m_axis.AbsoluteMove(profile);
                SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile.m_dest);
                return true;
            }
            /// <summary>
            /// Auto Teaching 할 때 X, Z축을 Neagtive, Positive, Center로 이동시키는 함수
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="dir"></param>
            /// <param name="targetDouble"></param>
            private void AutoTeachingMove(AutoTeachingAxis axis, AutoTeachingMoveDirection dir, bool targetDouble = false) {
                AxisProfile profile = new AxisProfile();
                BarcodeClosedLoopCommand xProfile = new BarcodeClosedLoopCommand();
                profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                AxisList axisIndex = AxisList.X_Axis;

                if (axis == AutoTeachingAxis.Axis_X) {
                    axisIndex = AxisList.X_Axis;
                    if (m_param.GetMotionParam().useFullClosed) {
                        xProfile.m_velocity = m_param.GetWMX3AxisAutoTeachingVelocity(AxisList.X_Axis);
                        xProfile.m_acc = xProfile.m_dec = xProfile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingAccDecX;
                        xProfile.m_pGain = m_param.GetFullClosedPGain();
                        xProfile.m_iGain = m_param.GetFullClosedIGain();
                    }
                    else {
                        profile.m_axis = m_param.GetAxisNumber(AxisList.X_Axis);
                        profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.X_Axis).jerkRatio / 100;
                        profile.m_velocity = m_param.GetWMX3AxisAutoTeachingVelocity(AxisList.X_Axis);
                        profile.m_acc = profile.m_dec = profile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingAccDecX;
                    }
                }
                else if (axis == AutoTeachingAxis.Axis_Z) {
                    axisIndex = AxisList.Z_Axis;
                    profile.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                    profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;
                    profile.m_velocity = m_param.GetWMX3AxisAutoTeachingVelocity(AxisList.Z_Axis);
                    profile.m_acc = profile.m_dec = profile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingAccDecZ;
                }

                if (dir == AutoTeachingMoveDirection.Positive) {
                    if (axis == AutoTeachingAxis.Axis_X)
                        if (m_param.GetMotionParam().useFullClosed) {
                            if (targetDouble) {
                                xProfile.m_targetDistance = m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance + (m_param.GetAutoTeachingParam().autoTeachingDistX * 1000 * 2);
                            }
                            else {
                                xProfile.m_targetDistance = m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance + (m_param.GetAutoTeachingParam().autoTeachingDistX * 1000);
                            }
                            m_axis.StartFullClosedMotion(xProfile);
                            SetAxisStep(AxisStep.Started, axisIndex, xProfile.m_targetDistance);
                            return;
                        }
                        else {
                            if (targetDouble) {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistX * 1000 * 2;
                            }
                            else {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistX * 1000;
                            }
                            SetAxisStep(AxisStep.Started, axisIndex, profile.m_dest + GetAxisStatus(AxisStatusType.pos_cmd, axisIndex));
                            m_axis.RelativeMove(profile);
                            return;
                        }
                    else if (axis == AutoTeachingAxis.Axis_Z) {
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            if (targetDouble) {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000 * 2;
                            }
                            else {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000;
                            }
                        }
                        else {
                            double dest = 0;
                            if (targetDouble) {
                                dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000 * 2;
                            }
                            else {
                                dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000;
                            }
                            profile.m_dest = DistanceToRadian(GetCurrentZAxisDia(), dest);
                        }
                        SetAxisStep(AxisStep.Started, axisIndex, profile.m_dest + GetAxisStatus(AxisStatusType.pos_cmd, axisIndex));
                        m_axis.RelativeMove(profile);
                        return;
                    }
                }
                else if (dir == AutoTeachingMoveDirection.Negative) {
                    if (axis == AutoTeachingAxis.Axis_X)
                        if (m_param.GetMotionParam().useFullClosed) {
                            if (targetDouble) {
                                xProfile.m_targetDistance = m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance + (m_param.GetAutoTeachingParam().autoTeachingDistX * 1000 * (-2));
                            }
                            else {
                                xProfile.m_targetDistance = m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance + (m_param.GetAutoTeachingParam().autoTeachingDistX * 1000 * (-1));
                            }
                            m_axis.StartFullClosedMotion(xProfile);
                            SetAxisStep(AxisStep.Started, axisIndex, xProfile.m_targetDistance);
                            return;
                        }
                        else {
                            if (targetDouble) {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistX * 1000 * (-2);
                            }
                            else {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistX * 1000 * (-1);
                            }
                            SetAxisStep(AxisStep.Started, axisIndex, profile.m_dest + GetAxisStatus(AxisStatusType.pos_cmd, axisIndex));
                            m_axis.RelativeMove(profile);
                            return;
                        }
                    else if (axis == AutoTeachingAxis.Axis_Z) {
                        if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                            if (targetDouble) {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000 * (-2);
                            }
                            else {
                                profile.m_dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000 * (-1);
                            }
                        }
                        else {
                            double dest = 0;
                            if (targetDouble) {
                                dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000 * (-2);
                            }
                            else {
                                dest = m_param.GetAutoTeachingParam().autoTeachingDistZ * 1000 * (-1);
                            }
                            profile.m_dest = DistanceToRadian(GetCurrentZAxisDia(), dest);
                        }
                        SetAxisStep(AxisStep.Started, axisIndex, profile.m_dest + GetAxisStatus(AxisStatusType.pos_cmd, axisIndex));
                        m_axis.RelativeMove(profile);
                        return;
                    }
                }
                else if (dir == AutoTeachingMoveDirection.Center) {
                    if (axis == AutoTeachingAxis.Axis_X) {
                        if (m_param.GetMotionParam().useFullClosed) {
                            xProfile.m_targetDistance = m_autoTeachingSensorData.x_centerPos;

                            m_axis.StartFullClosedMotion(xProfile);
                            SetAxisStep(AxisStep.Started, axisIndex, xProfile.m_targetDistance);
                            return;
                        }
                        else {
                            profile.m_velocity = m_param.GetWMX3AxisAutoTeachingVelocity(AxisList.X_Axis);
                            profile.m_acc = profile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingAccDecX;
                            profile.m_dec = profile.m_acc;
                            profile.m_dest = m_autoTeachingSensorData.x_centerPos;

                            SetAxisStep(AxisStep.Started, axisIndex, profile.m_dest);
                            m_axis.AbsoluteMove(profile);
                            return;
                        }
                    }
                    else if (axis == AutoTeachingAxis.Axis_Z) {
                        profile.m_velocity = m_param.GetWMX3AxisAutoTeachingVelocity(AxisList.Z_Axis);
                        profile.m_acc = profile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingAccDecZ;
                        profile.m_dec = profile.m_acc;
                        profile.m_dest = m_autoTeachingSensorData.z_centerPos;

                        SetAxisStep(AxisStep.Started, axisIndex, profile.m_dest);
                        m_axis.AbsoluteMove(profile);
                        return;
                    }
                }
            }
            /// <summary>
            /// Auto Teaching에서 Sensor가 검출되지 않았을 때 Sensor 검출하는 이동 명령 함수
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="dir"></param>
            /// <param name="targetDouble"></param>
            private void AutoTeachingSensorCheckMove(AutoTeachingAxis axis, AutoTeachingMoveDirection dir, bool targetDouble) {
                AxisProfile profile = new AxisProfile();
                profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                AxisList axisIndex = AxisList.X_Axis;

                if (axis == AutoTeachingAxis.Axis_X) {
                    if (m_param.GetMotionParam().useFullClosed) {
                        BarcodeClosedLoopCommand fcCmd = new BarcodeClosedLoopCommand();

                        if(dir == AutoTeachingMoveDirection.Negative) {
                            if (targetDouble) {
                                fcCmd.m_targetDistance = GetAxisStatus(AxisStatusType.pos_act, axisIndex) - (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeX * 1000 * 2);
                            }
                            else {
                                fcCmd.m_targetDistance = GetAxisStatus(AxisStatusType.pos_act, axisIndex) - (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeX * 1000);
                            }
                        }
                        else {
                            if (targetDouble) {
                                fcCmd.m_targetDistance = GetAxisStatus(AxisStatusType.pos_act, axisIndex) + (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeX * 1000 * 2);
                            }
                            else {
                                fcCmd.m_targetDistance = GetAxisStatus(AxisStatusType.pos_act, axisIndex) + (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeX * 1000);
                            }
                        }
                        fcCmd.m_velocity = m_param.GetAutoTeachingParam().autoTeachingFindSensorSpeedX * 1000;
                        fcCmd.m_acc = fcCmd.m_dec = fcCmd.m_velocity / m_param.GetAutoTeachingParam().autoTeachingFindSensorAccDecX;
                        fcCmd.m_pGain = m_param.GetFullClosedPGain();
                        fcCmd.m_iGain = m_param.GetFullClosedIGain();

                        m_axis.StartFullClosedMotion(fcCmd);
                        SetAxisStep(AxisStep.Started, axisIndex, fcCmd.m_targetDistance);

                        return;
                    }
                    else {
                        axisIndex = AxisList.X_Axis;
                        profile.m_axis = m_param.GetAxisNumber(AxisList.X_Axis);
                        profile.m_velocity = m_param.GetAutoTeachingParam().autoTeachingFindSensorSpeedX * 1000;
                        profile.m_acc = profile.m_dec = profile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingFindSensorAccDecX;
                        profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.X_Axis).jerkRatio / 100;
                        if (dir == AutoTeachingMoveDirection.Negative) {
                            profile.m_dest = (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeX * 1000 * (-1));
                        }
                        else if (dir == AutoTeachingMoveDirection.Positive) {
                            profile.m_dest = (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeX * 1000);
                        }
                    }
                }
                else if (axis == AutoTeachingAxis.Axis_Z) {
                    axisIndex = AxisList.Z_Axis;
                    profile.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                    profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;
                    if(m_param.GetMotionParam().ZAxisBeltType == ZAxisBeltType.Normal) {
                        profile.m_velocity = m_param.GetAutoTeachingParam().autoTeachingFindSensorSpeedZ * 1000;
                        if (dir == AutoTeachingMoveDirection.Negative) {
                            profile.m_dest = (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeZ * 1000 * (-1));
                        }
                        else if (dir == AutoTeachingMoveDirection.Positive) {
                            profile.m_dest = (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeZ * 1000);
                        }
                    }
                    else {
                        double speed = m_param.GetAutoTeachingParam().autoTeachingFindSensorSpeedZ * 1000;
                        profile.m_velocity = DistanceToRadian(m_param.GetMotionParam().ZAxisBeltFirstDia, speed);
                        double dest = (m_param.GetAutoTeachingParam().autoTeachingFindSensorRangeZ * 1000);
                        if(dir == AutoTeachingMoveDirection.Negative) {
                            profile.m_dest = DistanceToRadian(GetCurrentZAxisDia(), dest) * (-1);
                        }else if(dir == AutoTeachingMoveDirection.Positive) {
                            profile.m_dest = DistanceToRadian(GetCurrentZAxisDia(), dest);
                        }
                    }
                    profile.m_acc = profile.m_dec = profile.m_velocity / m_param.GetAutoTeachingParam().autoTeachingFindSensorAccDecZ;
                }

                if (targetDouble) {
                    profile.m_dest *= 2;
                }
                SetAxisStep(AxisStep.Started, axisIndex, profile.m_dest + GetAxisStatus(AxisStatusType.pos_cmd, axisIndex));
                m_axis.RelativeMove(profile);
            }
            /// <summary>
            /// Auto Teaching 진행중인 전 축 정지
            /// </summary>
            private void AutoTeachingAxisStop() {
                AllStop();
            }
            /// <summary>
            /// 각 축의 Step을 지정하는 함수
            /// </summary>
            /// <param name="step"></param>
            /// <param name="axis"></param>
            /// <param name="target"></param>
            private void SetAxisStep(AxisStep step, AxisList axis, double target) {
                int index = (int)axis;
                m_currentTarget[index] = target;
                m_motionStep[index] = step;
            }
            /// <summary>
            /// 위 함수와 동일하나 Target 위치를 따로 지정하지 않는 함수
            /// </summary>
            /// <param name="step"></param>
            /// <param name="axis"></param>
            private void SetAxisStep(AxisStep step, AxisList axis) {
                int index = (int)axis;
                m_motionStep[index] = step;
            }
            /// <summary>
            /// 각 축의 Step을 업데이트
            /// </summary>
            private void UpdateAxisStep() {
                foreach (AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                        if (axis == AxisList.T_Axis)
                            continue;
                    }
                    switch (m_motionStep[(int)axis]) {
                        case AxisStep.Idle:
                            m_currentTarget[(int)axis] = 0;
                            break;

                        case AxisStep.Started:
                            if(axis == AxisList.X_Axis && m_param.GetMotionParam().useFullClosed) {
                                if(m_axis.GetFullClosedStatus().m_fbStatus.m_op == BarcodeOpState.POS_CLOSE_LOOP_MOVE ||
                                    (m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance >= ((int)m_currentTarget[(int)axis] - 500) &&
                                    m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance <= ((int)m_currentTarget[(int)axis] + 500))) {
                                    SetAxisStep(AxisStep.ServoInpositionCheck, axis);
                                }
                            }
                            else {
                                if (GetAxisFlag(AxisFlagType.Run, axis) ||
                                ((int)GetAxisStatus(AxisStatusType.pos_cmd, axis) == (int)m_currentTarget[(int)axis])) {
                                    SetAxisStep(AxisStep.ServoInpositionCheck, axis);
                                }
                            }
                            break;

                        case AxisStep.ServoInpositionCheck:
                            if(axis == AxisList.X_Axis && m_param.GetMotionParam().useFullClosed) {
                                if(m_axis.GetFullClosedStatus().m_fbStatus.m_op == BarcodeOpState.IDLE) {
                                    if((m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance >= ((int)m_currentTarget[(int)axis] - 1000) &&
                                    m_axis.GetFullClosedStatus().m_fbStatus.m_actualDistance <= ((int)m_currentTarget[(int)axis] + 1000)))
                                        SetAxisStep(AxisStep.Finished, axis);
                                }
                            }
                            else {
                                if (GetAxisFlag(AxisFlagType.Poset, axis) && (int)GetAxisStatus(AxisStatusType.pos_cmd, axis) == (int)m_currentTarget[(int)axis]) {
                                    SetAxisStep(AxisStep.Finished, axis);
                                }
                            }
                            break;

                        case AxisStep.HomeStarted:
                            if(GetAxisFlag(AxisFlagType.Homing, axis) || !GetAxisFlag(AxisFlagType.HomeDone, axis) || (int)GetAxisStatus(AxisStatusType.pos_cmd, axis) == 0) {
                                SetAxisStep(AxisStep.HomeCheck, axis);
                            }

                            break;

                        case AxisStep.HomeCheck:
                            if(GetAxisFlag(AxisFlagType.HomeDone, axis)) {
                                SetAxisStep(AxisStep.Finished, axis);
                            }

                            break;

                        case AxisStep.Finished:
                            break;

                        case AxisStep.Stop:
                            if(axis == AxisList.X_Axis && m_param.GetMotionParam().useFullClosed) {
                                if(m_axis.GetFullClosedStatus().m_fbStatus.m_op == BarcodeOpState.IDLE) {
                                    SetAxisStep(AxisStep.Idle, axis);
                                }
                            }
                            else {
                                if (!GetAxisFlag(AxisFlagType.Run, axis) && GetAxisFlag(AxisFlagType.Poset, axis)) {
                                    SetAxisStep(AxisStep.Idle, axis);
                                }
                            }
                            break;
                    }
                }
            }
            /// <summary>
            /// 현재 축의 Step을 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public AxisStep GetAxisStep(AxisList axis) {
                return m_motionStep[(int)axis];
            }
            /// <summary>
            /// 전체 축 Stop
            /// </summary>
            public void AllStop() {
                foreach (AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && axis == AxisList.T_Axis)
                        continue;

                    if(m_param.GetMotionParam().useFullClosed && axis == AxisList.X_Axis) {
                        m_axis.StopFullClosed();
                    }
                    else {
                        m_axis.Stop(m_param.GetAxisNumber(axis));
                    }
                    SetAxisStep(AxisStep.Stop, axis);
                }
            }
            /// <summary>
            /// 특정 축 Stop
            /// </summary>
            /// <param name="axis"></param>
            public void AxisStop(AxisList axis) {
                if(m_param.GetMotionParam().useFullClosed && axis == AxisList.X_Axis) {
                    m_axis.StopFullClosed();
                }
                else {
                    m_axis.Stop(m_param.GetAxisNumber(axis));
                }
            }
            /// <summary>
            /// Jog Stop
            /// </summary>
            /// <param name="axis"></param>
            public void JogStop(AxisList axis) {
                m_axis.Stop(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// Inching Stop
            /// </summary>
            /// <param name="axis"></param>
            public void InchingStop(AxisList axis) {
                m_axis.Stop(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// Homing Stop
            /// </summary>
            /// <param name="axis"></param>
            public void HomeStop(AxisList axis) {
                m_axis.Stop(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// 특정 축 Homing 시작
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool AxisHomeStart(AxisList axis) {
                return m_axis.HomeStart(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// Home Done Flag 임의로 설정
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="homeDone"></param>
            /// <returns></returns>
            public bool SetHomeDoneFlag(AxisList axis, bool homeDone) {
                return m_axis.SetHomeDoneFlag(m_param.GetAxisNumber(axis), homeDone);
            }
            /// <summary>
            /// E-Stop
            /// </summary>
            public void EmergencyStop() {
                foreach (AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && axis == AxisList.T_Axis)
                        continue;

                    if(m_param.GetMotionParam().useFullClosed && axis == AxisList.X_Axis) {
                        m_axis.StopFullClosed();
                    }
                    else {
                        double decSecond = 0;
                        switch (m_param.GetMotionParam().stopMode) {
                            case AxisStopMode.Quick:
                                decSecond = (double)m_param.GetAxisParameter(axis).quickStop;
                                break;

                            case AxisStopMode.Normal:
                                decSecond = (double)m_param.GetAxisParameter(axis).normalStop;
                                break;

                            case AxisStopMode.Slow:
                                decSecond = (double)m_param.GetAxisParameter(axis).slowStop;
                                break;
                        }

                        double dec = m_param.GetWMX3AxisAutoVelocity(axis) / decSecond;
                        if (dec == 0) {
                            m_axis.EmergencyStop(m_param.GetAxisNumber(axis));
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"WMX3 Emergency Stop, Axis={axis}"));
                        }
                        else {
                            m_axis.Stop(m_param.GetAxisNumber(axis), dec);
                            Log.Add(new Log.LogItem(Log.LogLevel.Warning, Log.LogType.RackMaster, $"Emergency Stop={dec / 1000}mm/s^2, Axis={axis}"));
                        }
                    }
                    SetAutoMotionStep(AutoStep.Step500_Error);
                    SetAxisStep(AxisStep.Stop, axis);
                }
            }
            /// <summary>
            /// 전체 축 Servo On
            /// </summary>
            public void AllServoOn() {
                foreach (AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    m_axis.ServoOn(m_param.GetAxisNumber(axis));
                }
            }
            /// <summary>
            /// 전체 축 Servo Off
            /// </summary>
            public void AllServoOff() {
                foreach (AxisList axis in Enum.GetValues(typeof(AxisList))) {
                    m_axis.ServoOff(m_param.GetAxisNumber(axis));
                }
            }
            /// <summary>
            /// 특정 축 Servo On
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            public bool ServoOn(AxisList axis) {
                return m_axis.ServoOn(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// 특정 축 Servo Off
            /// </summary>
            /// <param name="axis"></param>
            public void ServoOff(AxisList axis) {
                m_axis.ServoOff(m_param.GetAxisNumber(axis));
            }
            /// <summary>
            /// 특정 축 Amp Alarm Clear
            /// </summary>
            /// <param name="axis"></param>
            public void AlarmClear(AxisList axis) {
                m_axis.AlarmClear(m_param.GetAxisNumber(axis));
                if(m_param.GetMotionParam().useFullClosed && axis == AxisList.X_Axis) {
                    m_axis.AlarmClearFullClosed();
                }
            }
            /// <summary>
            /// CIM에게 전달할 Bit 값 설정
            /// </summary>
            /// <param name="bit"></param>
            /// <param name="value"></param>
            private void SetSendBit(SendBitMap bit, bool value) {
                m_main.SetSendBit(bit, value);
            }
            /// <summary>
            /// 현재 특정 축의 Auto Speed(Max Speed * Auto Percent) 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <returns></returns>
            private float GetAutoSpeed(AxisList axis) {
                switch (axis) {
                    case AxisList.X_Axis:
                    case AxisList.Z_Axis:
                    case AxisList.T_Axis:
                        return m_param.GetWMX3AxisAutoVelocity(axis);

                    case AxisList.A_Axis:
                        if(GetCurrentAutoStep() == AutoStep.Step142_From_Fork_BWD_Move || GetCurrentAutoStep() == AutoStep.Step210_To_Fork_FWD_Move) {
                            return m_param.GetWMX3AxisAutoVelocity(axis) * (m_param.GetMotionParam().toModeAutoSpeedPercent / 100);
                        }
                        else {
                            return m_param.GetWMX3AxisAutoVelocity(axis);
                        }
                }

                return 0;
            }
            /// <summary>
            /// 현재 특정 축의 Manual Speed 반환
            /// </summary>
            /// <param name="axis"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            private float GetManualSpeed(AxisList axis, ManualSpeedType type) {
                switch (axis) {
                    case AxisList.X_Axis:
                    case AxisList.Z_Axis:
                    case AxisList.T_Axis:
                        return m_param.GetWMX3AxisManualVelocity(axis, type);

                    case AxisList.A_Axis:
                        if(GetCurrentAutoStep() == AutoStep.Step142_From_Fork_BWD_Move || GetCurrentAutoStep() == AutoStep.Step210_To_Fork_FWD_Move) {
                            return m_param.GetWMX3AxisManualVelocity(axis, type) * (m_param.GetMotionParam().toModeAutoSpeedPercent / 100);
                        }
                        else {
                            return m_param.GetWMX3AxisManualVelocity(axis, type);
                        }
                }

                return 0;
            }
            /// <summary>
            /// Maint Move 명령(HP or OP)
            /// </summary>
            /// <returns></returns>
            public bool MaintMove() {
                if (m_param.GetMotionParam().useInterpolation) {
                    if (m_param.GetMotionParam().useFullClosed) {
                        AxesProfile profile = new AxesProfile();
                        AxisProfile profile_noTurn = new AxisProfile();

                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            profile_noTurn.m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                            profile_noTurn.m_profileType = WMXParam.m_profileType.JerkRatio;
                            profile_noTurn.m_dest = 0;
                            profile_noTurn.m_velocity = GetAutoSpeed(AxisList.Z_Axis);
                            profile_noTurn.m_acc = profile_noTurn.m_dec = profile_noTurn.m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec;
                        }
                        else {
                            profile.m_axisCount = 2;
                            profile.m_axisArray[0] = m_param.GetAxisNumber(AxisList.Z_Axis);
                            profile.m_axisArray[1] = m_param.GetAxisNumber(AxisList.T_Axis);

                            profile.m_profileType = WMXParam.m_profileType.SCurve;
                            profile.m_dest[0] = 0;
                            profile.m_dest[1] = (float)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valT;

                            profile.m_maxVel[0] = GetAutoSpeed(AxisList.Z_Axis);
                            profile.m_maxVel[1] = GetAutoSpeed(AxisList.T_Axis);

                            profile.m_maxAcc[0] = profile.m_maxDec[0] = profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec;
                            profile.m_maxAcc[1] = profile.m_maxDec[1] = profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec;

                        }
                        BarcodeClosedLoopCommand fcCmd = new BarcodeClosedLoopCommand();
                        fcCmd.m_velocity = GetAutoSpeed(AxisList.X_Axis);
                        fcCmd.m_acc = fcCmd.m_dec = fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec;
                        fcCmd.m_pGain = m_param.GetFullClosedPGain();
                        fcCmd.m_iGain = m_param.GetFullClosedIGain();
                        fcCmd.m_targetDistance = (double)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valX;

                        bool ret = false;
                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            ret = m_axis.AbsoluteMove(profile_noTurn);
                        }
                        else {
                            ret = m_axis.InterpolationMove(profile);
                        }
                        bool retFullClosed = m_axis.StartFullClosedMotion(fcCmd);
                        if (ret && retFullClosed) {
                            SetAxisStep(AxisStep.Started, AxisList.X_Axis, fcCmd.m_targetDistance);
                            if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                                SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile_noTurn.m_dest);
                            }
                            else {
                                SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest[0]);
                                SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile.m_dest[1]);
                            }

                            return true;
                        }

                        return false;
                    }
                    else {
                        AxesProfile profile = new AxesProfile();
                        profile.m_axisArray[0] = m_param.GetAxisNumber(AxisList.X_Axis);
                        profile.m_axisArray[1] = m_param.GetAxisNumber(AxisList.Z_Axis);
                        profile.m_axisArray[2] = m_param.GetAxisNumber(AxisList.T_Axis);

                        profile.m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile.m_jerkRatio = m_param.GetAxisParameter(AxisList.X_Axis).jerkRatio / 100;

                        profile.m_dest[0] = (float)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valX;
                        profile.m_dest[1] = 0;
                        profile.m_dest[2] = (float)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valT;

                        profile.m_maxVel[0] = GetAutoSpeed(AxisList.X_Axis);
                        profile.m_maxVel[1] = GetAutoSpeed(AxisList.Z_Axis);
                        profile.m_maxVel[2] = GetAutoSpeed(AxisList.T_Axis);

                        profile.m_maxAcc[0] = profile.m_maxDec[0] = profile.m_maxVel[0] / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec;
                        profile.m_maxAcc[1] = profile.m_maxDec[1] = profile.m_maxVel[1] / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec;
                        profile.m_maxAcc[2] = profile.m_maxDec[2] = profile.m_maxVel[2] / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec;

                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            profile.m_axisCount = 2;
                        }
                        else {
                            profile.m_axisCount = 3;
                        }

                        bool ret = m_axis.InterpolationMove(profile);
                        if (ret) {
                            SetAxisStep(AxisStep.Started, AxisList.X_Axis, profile.m_dest[0]);
                            SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile.m_dest[1]);
                            if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                                SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile.m_dest[2]);
                            }
                        }
                        return ret;
                    }
                }
                else {
                    if (m_param.GetMotionParam().useFullClosed) {
                        AxisProfile[] profile = new AxisProfile[2];
                        for (int i = 0; i < profile.Length; i++) {
                            profile[i] = new AxisProfile();
                        }

                        BarcodeClosedLoopCommand fcCmd = new BarcodeClosedLoopCommand();
                        fcCmd.m_targetDistance = (float)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valX;
                        fcCmd.m_velocity = GetAutoSpeed(AxisList.X_Axis);
                        fcCmd.m_acc = fcCmd.m_dec = fcCmd.m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec;
                        fcCmd.m_pGain = m_param.GetFullClosedPGain();
                        fcCmd.m_iGain = m_param.GetFullClosedIGain();

                        profile[0].m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                        profile[0].m_dest = 0;
                        profile[0].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[0].m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;

                        profile[1].m_axis = m_param.GetAxisNumber(AxisList.T_Axis);
                        profile[1].m_dest = (float)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valT;
                        profile[1].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[1].m_jerkRatio = m_param.GetAxisParameter(AxisList.T_Axis).jerkRatio / 100;

                        profile[0].m_velocity = GetAutoSpeed(AxisList.Z_Axis);
                        profile[1].m_velocity = GetAutoSpeed(AxisList.T_Axis);

                        profile[0].m_acc = profile[0].m_dec = profile[0].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec;
                        profile[1].m_acc = profile[1].m_dec = profile[1].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec;

                        bool ret = false;

                        if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn) {
                            ret = m_axis.AbsoluteMove(profile[0]);
                        }
                        else {
                            for (int i = 0; i < profile.Length; i++) {
                                ret = m_axis.AbsoluteMove(profile[i]);
                                if (!ret) {
                                    return false;
                                }
                            }
                        }
                        ret = m_axis.StartFullClosedMotion(fcCmd);
                        if (!ret) {
                            return false;
                        }

                        SetAxisStep(AxisStep.Started, AxisList.X_Axis, fcCmd.m_targetDistance);
                        SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile[0].m_dest);
                        if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                            SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile[1].m_dest);
                        }

                        return true;
                    }
                    else {
                        AxisProfile[] profile = new AxisProfile[3];
                        for (int i = 0; i < profile.Length; i++) {
                            profile[i] = new AxisProfile();
                        }

                        profile[0].m_axis = m_param.GetAxisNumber(AxisList.X_Axis);
                        profile[0].m_dest = (float)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valX;
                        profile[0].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[0].m_jerkRatio = m_param.GetAxisParameter(AxisList.X_Axis).jerkRatio / 100;

                        profile[1].m_axis = m_param.GetAxisNumber(AxisList.Z_Axis);
                        profile[1].m_dest = 0;
                        profile[1].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[1].m_jerkRatio = m_param.GetAxisParameter(AxisList.Z_Axis).jerkRatio / 100;

                        profile[2].m_axis = m_param.GetAxisNumber(AxisList.T_Axis);
                        profile[2].m_dest = (float)m_teaching.GetMaintTeachingData(m_param.GetMotionParam().maintTarget).valT;
                        profile[2].m_profileType = WMXParam.m_profileType.JerkRatio;
                        profile[2].m_jerkRatio = m_param.GetAxisParameter(AxisList.T_Axis).jerkRatio / 100;

                        profile[0].m_velocity = GetAutoSpeed(AxisList.X_Axis);
                        profile[1].m_velocity = GetAutoSpeed(AxisList.Z_Axis);
                        profile[2].m_velocity = GetAutoSpeed(AxisList.T_Axis);

                        profile[0].m_acc = profile[0].m_dec = profile[0].m_velocity / m_param.GetAxisParameter(AxisList.X_Axis).maxAccDec;
                        profile[1].m_acc = profile[1].m_dec = profile[1].m_velocity / m_param.GetAxisParameter(AxisList.Z_Axis).maxAccDec;
                        profile[2].m_acc = profile[2].m_dec = profile[2].m_velocity / m_param.GetAxisParameter(AxisList.T_Axis).maxAccDec;

                        bool ret = false;

                        for (int i = 0; i < profile.Length; i++) {
                            if (m_param.GetMotionParam().forkType == ForkType.Slide_NoTurn && i == 2) {
                                continue;
                            }

                            ret = m_axis.AbsoluteMove(profile[i]);
                            if (!ret) {
                                return false;
                            }
                        }
                        SetAxisStep(AxisStep.Started, AxisList.X_Axis, profile[0].m_dest);
                        SetAxisStep(AxisStep.Started, AxisList.Z_Axis, profile[1].m_dest);
                        if (m_param.GetMotionParam().forkType != ForkType.Slide_NoTurn) {
                            SetAxisStep(AxisStep.Started, AxisList.T_Axis, profile[2].m_dest);
                        }

                        return true;
                    }
                }
            }
        }
    }
}
