using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MovenCore;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Port_ControlInverter.cs 는 Port의 축 타입 중 Inverter 축 관련 제어 기능이 작성된 페이지
    /// Flag -> 명령, Status -> 상태
    /// </summary>
    partial class Port
    {
        /// <summary>
        /// 축이 Inverter Type인지 체크
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <returns></returns>
        private bool Check_InverterType(PortAxis ePortAxis)
        {
            if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].eAxisCtrlType != AxisCtrlType.Inverter)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidControlType, $"Control Type: {GetMotionParam().Ctrl_Axis[(int)ePortAxis].eAxisCtrlType}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 인버터 구동 동작이며 CtrlType에 따라 Bit On/Off 처리
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eInvCtrlType"></param>
        /// <param name="bEnable"></param>
        public void InverterCtrl_SetRunFlag(PortAxis ePortAxis, InvCtrlType eInvCtrlType, bool bEnable)
        {
            if (!Check_InverterType(ePortAxis))
                return;

            //Bit Map Mapping
            if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
            {
                if (ePortAxis == PortAxis.Buffer_LP_Z && eInvCtrlType == InvCtrlType.HighSpeedFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_Up_HighSpeed_Move, bEnable);
                if (ePortAxis == PortAxis.Buffer_LP_Z && eInvCtrlType == InvCtrlType.LowSpeedFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_Up_LowSpeed_Move, bEnable);
                if (ePortAxis == PortAxis.Buffer_LP_Z && eInvCtrlType == InvCtrlType.HighSpeedBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_Down_HighSpeed_Move, bEnable);
                if (ePortAxis == PortAxis.Buffer_LP_Z && eInvCtrlType == InvCtrlType.LowSpeedBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_Down_LowSpeed_Move, bEnable);
            }
            else
            {
                if (ePortAxis == PortAxis.Buffer_LP_Z && eInvCtrlType == InvCtrlType.FreqFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_Up_HighSpeed_Move, bEnable);
                if (ePortAxis == PortAxis.Buffer_LP_Z && eInvCtrlType == InvCtrlType.FreqBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_Down_HighSpeed_Move, bEnable);
            }

            GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunFlag(eInvCtrlType, bEnable);
        }
        
        /// <summary>
        /// 인버터 정지 동작이며 모든 Bit Off
        /// </summary>
        /// <param name="ePortAxis"></param>
        public void InverterCtrl_MotionStop(PortAxis ePortAxis)
        {
            foreach (InvCtrlType eInvCtrlType in Enum.GetValues(typeof(InvCtrlType)))
            {
                InverterCtrl_SetRunFlag(ePortAxis, eInvCtrlType, false);
            }
        }

        /// <summary>
        /// 인버터 리셋 동작이며 Reset Bit 연동되어 있는 경우 On
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="bEnable"></param>
        public void InverterCtrl_SetResetFlag(PortAxis ePortAxis, bool bEnable)
        {
            GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetResetFlag(bEnable);
        }

        /// <summary>
        /// 인버터 동작 중 여부 확인
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <returns></returns>
        public bool InverterCtrl_Is_Busy(PortAxis ePortAxis)
        {
            return GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.IsBusy();
        }

        /// <summary>
        /// 인버터 구동 타입에 따라 업데이트 영역에서 런 상태가 반영되며 런 상태 체크 함수
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eInvCtrlType"></param>
        /// <returns></returns>
        public bool InverterCtrl_GetRunStatus(PortAxis ePortAxis, InvCtrlType eInvCtrlType)
        {
            return GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetRunStatus(eInvCtrlType);
        }

        /// <summary>
        /// Reset Flag의 상태를 가져옴
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <returns></returns>
        public bool InverterCtrl_GetResetFlag(PortAxis ePortAxis)
        {
            return GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetResetFlag();
        }

        /// <summary>
        /// Inverter Status to Bit, Word Map
        /// </summary>
        private void InverterCtrl_BitWordUpdate(PortAxis ePortAxis)
        {
            bool bBusy = InverterCtrl_Is_Busy(ePortAxis);

            if (Status_EStop && bBusy)
            {
                InverterCtrl_MotionStop(ePortAxis);
            }

            if (Status_EStop)
                return;

            //Inverter는 I/O or Hz Control로 제어

            if (GetMotionParam().GetShuttleCtrl_InvParam(ePortAxis).InvCtrlMode == InvCtrlMode.IOControl)
            {
                var HighSpeedIOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(InvIOCtrlFlag.HighSpeed);
                var LowSpeedIOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(InvIOCtrlFlag.LowSpeed);
                var FWDIOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(InvIOCtrlFlag.FWD);
                var BWDIOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(InvIOCtrlFlag.BWD);

                bool bErrorStatus = false;
                bool bPOT = false;
                bool bNOT = false;
                if (ePortAxis == PortAxis.Buffer_LP_Z)
                {
                    bErrorStatus = Sensor_LP_Z_Error;
                    bPOT = Sensor_LP_Z_POT;
                    bNOT = Sensor_LP_Z_NOT;
                }
                else if (ePortAxis == PortAxis.Buffer_OP_Z)
                {
                    bErrorStatus = Sensor_OP_Z_Error;
                    bPOT = Sensor_OP_Z_POT;
                    bNOT = Sensor_OP_Z_NOT;
                }

                if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetRunFlag(InvCtrlType.HighSpeedFWD) && !bErrorStatus && !bPOT)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }
                else if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetRunFlag(InvCtrlType.LowSpeedFWD) && !bErrorStatus && !bPOT)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }
                else if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetRunFlag(InvCtrlType.HighSpeedBWD) && !bErrorStatus && !bNOT)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, true);
                }
                else if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetRunFlag(InvCtrlType.LowSpeedBWD) && !bErrorStatus && !bNOT)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, true);
                }
                else
                {
                    if (bPOT || bErrorStatus)
                    {
                        GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunFlag(InvCtrlType.HighSpeedFWD, false);
                        GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunFlag(InvCtrlType.LowSpeedFWD, false);
                    }
                    if (bNOT || bErrorStatus)
                    {
                        GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunFlag(InvCtrlType.HighSpeedBWD, false);
                        GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunFlag(InvCtrlType.HighSpeedBWD, false);
                    }

                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }

                bool HighSpeedFlag = GetOutBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit);
                bool LowSpeedFlag = GetOutBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit);
                bool FWDFlag = GetOutBit(FWDIOParam.StartAddr, FWDIOParam.Bit);
                bool BWDFlag = GetOutBit(BWDIOParam.StartAddr, BWDIOParam.Bit);

                ///Output Bit의 상태에 따라 Running 중인지 체크 (Input Status 아니고 Output Status를 읽는 것임)
                GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunStatus(InvCtrlType.HighSpeedFWD, HighSpeedFlag && FWDFlag);
                GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunStatus(InvCtrlType.LowSpeedFWD, LowSpeedFlag && FWDFlag);
                GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunStatus(InvCtrlType.HighSpeedBWD, HighSpeedFlag && BWDFlag);
                GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunStatus(InvCtrlType.LowSpeedBWD, LowSpeedFlag && BWDFlag);

                //Reset 연동 미구현
                bool bResetFlag = InverterCtrl_GetResetFlag(ePortAxis);

                switch (ePortAxis)
                {
                    case PortAxis.Shuttle_X:
                        Sensor_X_Axis_Busy = bBusy;
                        break;
                    case PortAxis.Shuttle_Z:
                        Sensor_Z_Axis_Busy = bBusy;
                        break;
                    case PortAxis.Shuttle_T:
                        Sensor_T_Axis_Busy = bBusy;
                        break;
                    case PortAxis.Buffer_LP_Z:
                        Sensor_LP_Z_Reset = bResetFlag;
                        break;
                    case PortAxis.Buffer_OP_Z:
                        Sensor_OP_Z_Reset = bResetFlag;
                        break;
                    default:
                        bPortAxisBusy[(int)ePortAxis] = bBusy;
                        break;
                }
            }
            else
            {
                //Hz Control
                int HzCtrlStartAddr = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.HzStartAddr;
                short HzTarget = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.HzTarget;

                if(HzCtrlStartAddr != -1 && HzTarget > 0)
                {
                    byte[] HzTargetArray = BitConverter.GetBytes(HzTarget);

                    m_WMXIO.SetOutputBytes(HzCtrlStartAddr, HzTargetArray);
                }


                var FWDIOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(InvIOCtrlFlag.FWD);
                var BWDIOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetIOParam(InvIOCtrlFlag.BWD);

                bool bErrorStatus = false;
                bool bPOT = false;
                bool bNOT = false;
                if (ePortAxis == PortAxis.Buffer_LP_Z)
                {
                    bErrorStatus = Sensor_LP_Z_Error;
                    bPOT = Sensor_LP_Z_POT;
                    bNOT = Sensor_LP_Z_NOT;
                }
                else if (ePortAxis == PortAxis.Buffer_OP_Z)
                {
                    bErrorStatus = Sensor_OP_Z_Error;
                    bPOT = Sensor_OP_Z_POT;
                    bNOT = Sensor_OP_Z_NOT;
                }

                if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetRunFlag(InvCtrlType.FreqFWD) && !bErrorStatus && !bPOT)
                {
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }
                else if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.GetRunFlag(InvCtrlType.FreqBWD) && !bErrorStatus && !bPOT)
                {
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, true);
                }
                else
                {
                    if (bPOT || bErrorStatus)
                    {
                        GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunFlag(InvCtrlType.FreqFWD, false);
                    }
                    if (bNOT || bErrorStatus)
                    {
                        GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunFlag(InvCtrlType.FreqBWD, false);
                    }

                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }

                bool FWDFlag = GetOutBit(FWDIOParam.StartAddr, FWDIOParam.Bit);
                bool BWDFlag = GetOutBit(BWDIOParam.StartAddr, BWDIOParam.Bit);

                GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunStatus(InvCtrlType.FreqFWD, FWDFlag);
                GetMotionParam().Ctrl_Axis[(int)ePortAxis].inverterParam.SetRunStatus(InvCtrlType.FreqBWD, BWDFlag);

                //Reset 연동 미구현
                bool bResetFlag = InverterCtrl_GetResetFlag(ePortAxis);


                switch (ePortAxis)
                {
                    case PortAxis.Shuttle_X:
                        Sensor_X_Axis_Busy = bBusy;
                        break;
                    case PortAxis.Shuttle_Z:
                        Sensor_Z_Axis_Busy = bBusy;
                        break;
                    case PortAxis.Shuttle_T:
                        Sensor_T_Axis_Busy = bBusy;
                        break;
                    case PortAxis.Buffer_LP_Z:
                        Sensor_LP_Z_Reset = bResetFlag;
                        break;
                    case PortAxis.Buffer_OP_Z:
                        Sensor_OP_Z_Reset = bResetFlag;
                        break;
                    default:
                        bPortAxisBusy[(int)ePortAxis] = bBusy;
                        break;
                }
            }
        }
    }
}
