using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovenCore;
using System.Threading;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Buffer도 Inverter 동작이므로 Inverter 제어 영역 참조.
    /// </summary>
    partial class Port
    {
        private bool Check_ConveyorType(BufferCV eBufferCV)
        {
            if (GetParam().ePortType != PortType.Conveyor_AGV && GetParam().ePortType != PortType.Conveyor_OMRON)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidOperationType, $"Operation Type: {GetParam().ePortType}");
                return false;
            }

            return true;
        }
        private bool Check_ConveyorEnable(BufferCV eBufferCV)
        {
            if (GetMotionParam().Ctrl_CV[(int)eBufferCV].eCVCtrlEnable != CVCtrlEnable.Enable)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidState, $"CV Control Not Enable");
                return false;
            }

            return true;
        }
        
        
        public void BufferCtrl_CV_SetRunFlag(BufferCV eBufferCV, InvCtrlType eInvCtrlType, bool bEnable)
        {
            if (!Check_ConveyorType(eBufferCV))
                return;

            if (!Check_ConveyorEnable(eBufferCV))
                return;

            //Bit Map Mapping

            if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
            {
                if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.HighSpeedFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_FWD_HighSpeed_Move, bEnable);
                else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.HighSpeedFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_FWD_HighSpeed_Move, bEnable);

                if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.LowSpeedFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_FWD_LowSpeed_Move, bEnable);
                else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.LowSpeedFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_FWD_LowSpeed_Move, bEnable);

                if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.HighSpeedBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_BWD_HighSpeed_Move, bEnable);
                else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.HighSpeedBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_BWD_HighSpeed_Move, bEnable);

                if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.LowSpeedBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_BWD_LowSpeed_Move, bEnable);
                else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.LowSpeedBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_BWD_LowSpeed_Move, bEnable);
            }
            else
            {
                if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.FreqFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_FWD_HighSpeed_Move, bEnable);
                else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.FreqFWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_FWD_HighSpeed_Move, bEnable);

                //if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.LowSpeedFWD)
                //    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_FWD_LowSpeed_Move, bEnable);
                //else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.LowSpeedFWD)
                //    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_FWD_LowSpeed_Move, bEnable);

                if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.FreqBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_BWD_HighSpeed_Move, bEnable);
                else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.FreqBWD)
                    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_BWD_HighSpeed_Move, bEnable);

                //if (eBufferCV == BufferCV.Buffer_LP && eInvCtrlType == InvCtrlType.LowSpeedBWD)
                //    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer2_Control_0, (int)Buffer2_ControlStatusBitMap.CV_BWD_LowSpeed_Move, bEnable);
                //else if (eBufferCV == BufferCV.Buffer_OP && eInvCtrlType == InvCtrlType.LowSpeedBWD)
                //    BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer1_Control_0, (int)Buffer1_ControlStatusBitMap.CV_BWD_LowSpeed_Move, bEnable);
            }


            GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunFlag(eInvCtrlType, bEnable);
        }
        public bool BufferCtrl_CV_GetRunStatus(BufferCV eBufferCV, InvCtrlType eInvCtrlType)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetRunStatus(eInvCtrlType);
        }
        public void BufferCtrl_CV_MotionStop(BufferCV eBufferCV)
        {
            foreach(InvCtrlType eInvCtrlList in Enum.GetValues(typeof(InvCtrlType)))
            {
                BufferCtrl_CV_SetRunFlag(eBufferCV, eInvCtrlList, false);
            }
        }
        public void BufferCtrl_CV_SetResetFlag(BufferCV eBufferCV, bool bEnable)
        {
            GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetResetFlag(bEnable);
        }
        public bool BufferCtrl_CV_GetResetFlag(BufferCV eBufferCV)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetResetFlag();
        }

        public void BufferCtrl_CV_SetSyncEnable(BufferCV eBufferCV, bool bEnable)
        {
            if (!Check_ConveyorType(eBufferCV))
                return;

            if (!Check_ConveyorEnable(eBufferCV))
                return;

            if (eBufferCV == BufferCV.Buffer_LP)
                BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer_SyncMove, (int)BufferSync_ControlStatusBitMap.Buffer2_Select, bEnable);
            else if (eBufferCV == BufferCV.Buffer_OP)
                BufferCtrl_SetBitInWord(ReceiveWordMapIndex.Buffer_SyncMove, (int)BufferSync_ControlStatusBitMap.Buffer1_Select, bEnable);

            GetMotionParam().Ctrl_CV[(int)eBufferCV].SetSyncMoveEnable(bEnable);
        }
        public bool BufferCtrl_CV_Is_SyncMoveSelect(BufferCV eBufferCV)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].GetSyncMoveEnable();
        }
        public void BufferCtrl_CV_Set_SyncMoveForward(bool bEnable)
        {
            foreach(BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
            {
                if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
                {
                    if (BufferCtrl_CV_Is_SyncMoveSelect(eBufferCV))
                        Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedFWD, true, false, InterlockFrom.UI_Event);
                }
                else
                {
                    if (BufferCtrl_CV_Is_SyncMoveSelect(eBufferCV))
                        Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqFWD, true, false, InterlockFrom.UI_Event);
                }
            }
        }
        public void BufferCtrl_CV_Set_SyncMoveBackward(bool bEnable)
        {
            foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
            {
                if (GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl)
                {
                    if (BufferCtrl_CV_Is_SyncMoveSelect(eBufferCV))
                        Interlock_SetConveyorMove(eBufferCV, InvCtrlType.LowSpeedBWD, true, false, InterlockFrom.UI_Event);
                }
                else
                {
                    if (BufferCtrl_CV_Is_SyncMoveSelect(eBufferCV))
                        Interlock_SetConveyorMove(eBufferCV, InvCtrlType.FreqBWD, true, false, InterlockFrom.UI_Event);
                }
            }
        }
        public void BufferCtrl_CV_SyncMotionStop()
        {
            foreach (BufferCV eBufferCV in Enum.GetValues(typeof(BufferCV)))
            {
                if (BufferCtrl_CV_Is_SyncMoveSelect(eBufferCV))
                {
                    Interlock_ConveyorMotionStop(eBufferCV, false, InterlockFrom.UI_Event);
                }
            }
        }
        public bool BufferCtrl_CV_Is_Busy(BufferCV eBufferCV)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.IsBusy();
        }


        public void BufferCtrl_Stopper_SetRunFlag(BufferCV eBufferCV, CylCtrlList eCylCtrlList, bool bEnable)
        {
            if (!GetMotionParam().IsStopperEnable(eBufferCV))
                return;

            //Bit Map Mapping


            GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.SetRunFlag(eCylCtrlList, bEnable);
        }
        public void BufferCtrl_Stopper_MotionStop(BufferCV eBufferCV)
        {
            foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
            {
                BufferCtrl_Stopper_SetRunFlag(eBufferCV, eCylCtrlList, false);
            }
        }
        public bool BufferCtrl_Stopper_Is_Busy(BufferCV eBufferCV)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.IsBusy();
        }
        public bool BufferCtrl_Stopper_GetRunStatus(BufferCV eBufferCV, CylCtrlList eCylCtrlList)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetRunStatus(eCylCtrlList);
        }

        public void BufferCtrl_Centering_SetRunFlag(BufferCV eBufferCV, CylCtrlList eCylCtrlList, bool bEnable)
        {
            if (!GetMotionParam().IsCenteringEnable(eBufferCV))
                return;

            //Bit Map Mapping


            GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.SetRunFlag(eCylCtrlList, bEnable);
        }
        public void BufferCtrl_Centering_MotionStop(BufferCV eBufferCV)
        {
            foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
            {
                BufferCtrl_Centering_SetRunFlag(eBufferCV, eCylCtrlList, false);
            }
        }
        public bool BufferCtrl_Centering_Is_Busy(BufferCV eBufferCV)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.IsBusy();
        }
        public bool BufferCtrl_Centering_GetRunStatus(BufferCV eBufferCV, CylCtrlList eCylCtrlList)
        {
            return GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.GetRunStatus(eCylCtrlList);
        }
        
        public bool BufferCtrl_BP_CSTDetect_Status(BufferCV eBufferCV)
        {
            int index = (int)eBufferCV;

            if (index >= GetMotionParam().Ctrl_CV.Length)
                return false;

            return GetMotionParam().Ctrl_CV[(int)eBufferCV].GetCSTDetectStatus();
        }

        /// <summary>
        /// Inverter Status to Bit, Word Map
        /// </summary>
        private void BufferCtrl_BitWordUpdate(BufferCV eBufferCV)
        {
            if (Status_EStop && BufferCtrl_CV_Is_Busy(eBufferCV))
                BufferCtrl_CV_MotionStop(eBufferCV);

            if (Status_EStop && BufferCtrl_Stopper_Is_Busy(eBufferCV))
                BufferCtrl_Stopper_MotionStop(eBufferCV);

            if (Status_EStop && BufferCtrl_Centering_Is_Busy(eBufferCV))
                BufferCtrl_Centering_MotionStop(eBufferCV);


            bool bIOControl = GetMotionParam().GetBufferCVParam(eBufferCV).CVParam.InvCtrlMode == InvCtrlMode.IOControl;

            if (bIOControl)
            {
                var HighSpeedIOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(InvIOCtrlFlag.HighSpeed);
                var LowSpeedIOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(InvIOCtrlFlag.LowSpeed);
                var FWDIOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(InvIOCtrlFlag.FWD);
                var BWDIOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(InvIOCtrlFlag.BWD);

                bool bErrorStatus = false;
                if (eBufferCV == BufferCV.Buffer_LP)
                {
                    bErrorStatus = Sensor_LP_CV_Error;
                }
                else if (eBufferCV == BufferCV.Buffer_OP)
                {
                    bErrorStatus = Sensor_OP_CV_Error;
                }

                if (GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetRunFlag(InvCtrlType.HighSpeedFWD) && !bErrorStatus)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }
                else if (GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetRunFlag(InvCtrlType.LowSpeedFWD) && !bErrorStatus)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }
                else if (GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetRunFlag(InvCtrlType.HighSpeedBWD) && !bErrorStatus)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, true);
                }
                else if (GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetRunFlag(InvCtrlType.LowSpeedBWD) && !bErrorStatus)
                {
                    m_WMXIO.SetOutputBit(HighSpeedIOParam.StartAddr, HighSpeedIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(LowSpeedIOParam.StartAddr, LowSpeedIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, true);
                }
                else
                {
                    if (bErrorStatus)
                    {
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunFlag(InvCtrlType.HighSpeedFWD, false);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunFlag(InvCtrlType.LowSpeedFWD, false);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunFlag(InvCtrlType.HighSpeedBWD, false);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunFlag(InvCtrlType.LowSpeedBWD, false);
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

                GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunStatus(InvCtrlType.HighSpeedFWD, HighSpeedFlag && FWDFlag);
                GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunStatus(InvCtrlType.LowSpeedFWD, LowSpeedFlag && FWDFlag);
                GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunStatus(InvCtrlType.HighSpeedBWD, HighSpeedFlag && BWDFlag);
                GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunStatus(InvCtrlType.LowSpeedBWD, LowSpeedFlag && BWDFlag);

                //BP단 Reset 미구현
                bool bResetFlag = BufferCtrl_CV_GetResetFlag(eBufferCV);

                if (eBufferCV == BufferCV.Buffer_LP)
                {
                    Sensor_LP_CV_FWD_Status = (HighSpeedFlag && FWDFlag) || (LowSpeedFlag && FWDFlag);
                    Sensor_LP_CV_BWD_Status = (HighSpeedFlag && BWDFlag) || (LowSpeedFlag && BWDFlag);

                    Sensor_LP_CV_Reset = bResetFlag;
                }
                else if (eBufferCV == BufferCV.Buffer_OP)
                {
                    Sensor_OP_CV_FWD_Status = (HighSpeedFlag && FWDFlag) || (LowSpeedFlag && FWDFlag);
                    Sensor_OP_CV_BWD_Status = (HighSpeedFlag && BWDFlag) || (LowSpeedFlag && BWDFlag);

                    Sensor_OP_CV_Reset = bResetFlag;
                }


                foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
                {
                    var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetCtrlIOParam(eCylCtrlList);

                    if (GetMotionParam().IsValidIO(IOParam))
                    {
                        int StartAddr = IOParam.StartAddr;
                        int Bit = IOParam.Bit;
                        bool ret = GetOutBit(StartAddr, Bit);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.SetRunStatus(eCylCtrlList, ret);

                        var Status = GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetRunStatus(eCylCtrlList);
                        var Flag = GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetRunFlag(eCylCtrlList);

                        if (Flag != Status)
                            m_WMXIO.SetOutputBit(StartAddr, Bit, Flag);
                    }
                }

                foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
                {
                    var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.GetCtrlIOParam(eCylCtrlList);

                    if (GetMotionParam().IsValidIO(IOParam))
                    {
                        int StartAddr = IOParam.StartAddr;
                        int Bit = IOParam.Bit;
                        bool ret = GetOutBit(StartAddr, Bit);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.SetRunStatus(eCylCtrlList, ret);

                        var Status = GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.GetRunStatus(eCylCtrlList);
                        var Flag = GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.GetRunFlag(eCylCtrlList);

                        if (Flag != Status)
                            m_WMXIO.SetOutputBit(StartAddr, Bit, Flag);
                    }
                }

                if (eBufferCV == BufferCV.Buffer_BP1 || eBufferCV == BufferCV.Buffer_BP2 || eBufferCV == BufferCV.Buffer_BP3 || eBufferCV == BufferCV.Buffer_BP4)
                {
                    var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CST_DetectParam;

                    if (GetMotionParam().IsValidIO(IOParam))
                    {
                        int StartAddr = IOParam.StartAddr;
                        int Bit = IOParam.Bit;
                        bool ret = GetInputBit(StartAddr, Bit);

                        GetMotionParam().Ctrl_CV[(int)eBufferCV].SetCSTDetectStatus(ret);
                    }
                }
                //else
                //{
                //    if(eBufferCV == BufferCV.Buffer_LP || eBufferCV == BufferCV.Buffer_OP)
                //    {
                //        var IOParam = eBufferCV == BufferCV.Buffer_LP ? GetMotionParam().Ctrl_CV[(int)eBufferCV].LP_CST_OppositeAngle : GetMotionParam().Ctrl_CV[(int)eBufferCV].OP_CST_OppositeAngle;

                //        if (GetMotionParam().IsValidIO(IOParam))
                //        {
                //            int StartAddr = IOParam.StartAddr;
                //            int Bit = IOParam.Bit;
                //            bool ret = m_WMXIO.GetInputBit(StartAddr, Bit);

                //            if(eBufferCV == BufferCV.Buffer_LP)
                //                GetMotionParam().Ctrl_CV[(int)eBufferCV].Set_LP_CST_OppositeAngleStatus(ret);
                //            else if (eBufferCV == BufferCV.Buffer_OP)
                //                GetMotionParam().Ctrl_CV[(int)eBufferCV].Set_OP_CST_OppositeAngleStatus(ret);
                //        }
                //    }
                //}
            }
            else
            {
                //Hz Control

                int HzCtrlStartAddr = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.HzStartAddr;
                short HzTarget = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.HzTarget;

                if (HzCtrlStartAddr != -1 && HzTarget > 0)
                {
                    if (eBufferCV == BufferCV.Buffer_OP && GetOperationDirection() == PortDirection.Input && Sensor_OP_CV_IN ||
                        eBufferCV == BufferCV.Buffer_LP && GetOperationDirection() == PortDirection.Input && Sensor_OP_CV_IN)
                        HzTarget = (short)(HzTarget / 4);

                    byte[] HzTargetArray = BitConverter.GetBytes(HzTarget);

                    m_WMXIO.SetOutputBytes(HzCtrlStartAddr, HzTargetArray);
                }

                var FWDIOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(InvIOCtrlFlag.FWD);
                var BWDIOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetIOParam(InvIOCtrlFlag.BWD);

                bool bErrorStatus = false;
                if (eBufferCV == BufferCV.Buffer_LP)
                {
                    bErrorStatus = Sensor_LP_CV_Error;
                }
                else if (eBufferCV == BufferCV.Buffer_OP)
                {
                    bErrorStatus = Sensor_OP_CV_Error;
                }

                if (GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetRunFlag(InvCtrlType.FreqFWD) && !bErrorStatus)
                {
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, true);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }
                else if (GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.GetRunFlag(InvCtrlType.FreqBWD) && !bErrorStatus)
                {
                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, true);
                }
                else
                {
                    if (bErrorStatus)
                    {
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunFlag(InvCtrlType.FreqFWD, false);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunFlag(InvCtrlType.FreqBWD, false);
                    }

                    m_WMXIO.SetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit, false);
                    m_WMXIO.SetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit, false);
                }


                //Hz Control은 ECAT Type 제품으로 출력 신호 값이 아닌 PDO I/O 값으로 피드백 받음
                //bool FWDFlag = m_WMXIO.GetOutputBit(FWDIOParam.StartAddr, FWDIOParam.Bit);
                //bool BWDFlag = m_WMXIO.GetOutputBit(BWDIOParam.StartAddr, BWDIOParam.Bit);

                //GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunStatus(InvCtrlType.FreqFWD, FWDFlag);
                //GetMotionParam().Ctrl_CV[(int)eBufferCV].CVParam.SetRunStatus(InvCtrlType.FreqBWD, BWDFlag);

                //if (eBufferCV == BufferCV.Buffer_LP)
                //{
                //    Sensor_LP_CV_FWD_Status = FWDFlag;
                //    Sensor_LP_CV_BWD_Status = BWDFlag;
                //}
                //else if (eBufferCV == BufferCV.Buffer_OP)
                //{
                //    Sensor_OP_CV_FWD_Status = FWDFlag;
                //    Sensor_OP_CV_BWD_Status = BWDFlag;
                //}

                //BP단 Reset 미구현
                bool bResetFlag = BufferCtrl_CV_GetResetFlag(eBufferCV);

                if (eBufferCV == BufferCV.Buffer_LP)
                {
                    Sensor_LP_CV_Reset = bResetFlag;
                }
                else if (eBufferCV == BufferCV.Buffer_OP)
                {
                    Sensor_OP_CV_Reset = bResetFlag;
                }




                foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
                {
                    var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetCtrlIOParam(eCylCtrlList);

                    if (GetMotionParam().IsValidIO(IOParam))
                    {
                        int StartAddr = IOParam.StartAddr;
                        int Bit = IOParam.Bit;
                        bool ret = GetOutBit(StartAddr, Bit);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.SetRunStatus(eCylCtrlList, ret);

                        var Status = GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetRunStatus(eCylCtrlList);
                        var Flag = GetMotionParam().Ctrl_CV[(int)eBufferCV].StopperParam.GetRunFlag(eCylCtrlList);

                        if (Flag != Status)
                            m_WMXIO.SetOutputBit(StartAddr, Bit, Flag);
                    }
                }

                foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
                {
                    var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.GetCtrlIOParam(eCylCtrlList);

                    if (GetMotionParam().IsValidIO(IOParam))
                    {
                        int StartAddr = IOParam.StartAddr;
                        int Bit = IOParam.Bit;
                        bool ret = GetOutBit(StartAddr, Bit);
                        GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.SetRunStatus(eCylCtrlList, ret);

                        var Status = GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.GetRunStatus(eCylCtrlList);
                        var Flag = GetMotionParam().Ctrl_CV[(int)eBufferCV].CenteringParam.GetRunFlag(eCylCtrlList);

                        if (Flag != Status)
                            m_WMXIO.SetOutputBit(StartAddr, Bit, Flag);
                    }
                }

                if (eBufferCV == BufferCV.Buffer_BP1 || eBufferCV == BufferCV.Buffer_BP2 || eBufferCV == BufferCV.Buffer_BP3 || eBufferCV == BufferCV.Buffer_BP4)
                {
                    var IOParam = GetMotionParam().Ctrl_CV[(int)eBufferCV].CST_DetectParam;

                    if (GetMotionParam().IsValidIO(IOParam))
                    {
                        int StartAddr = IOParam.StartAddr;
                        int Bit = IOParam.Bit;
                        bool ret = GetInputBit(StartAddr, Bit);

                        GetMotionParam().Ctrl_CV[(int)eBufferCV].SetCSTDetectStatus(ret);
                    }
                }
                //else
                //{
                //    if (eBufferCV == BufferCV.Buffer_LP || eBufferCV == BufferCV.Buffer_OP)
                //    {
                //        var IOParam = eBufferCV == BufferCV.Buffer_LP ? GetMotionParam().Ctrl_CV[(int)eBufferCV].LP_CST_OppositeAngle : GetMotionParam().Ctrl_CV[(int)eBufferCV].OP_CST_OppositeAngle;

                //        if (GetMotionParam().IsValidIO(IOParam))
                //        {
                //            int StartAddr = IOParam.StartAddr;
                //            int Bit = IOParam.Bit;
                //            bool ret = m_WMXIO.GetInputBit(StartAddr, Bit);

                //            if (eBufferCV == BufferCV.Buffer_LP)
                //                GetMotionParam().Ctrl_CV[(int)eBufferCV].Set_LP_CST_OppositeAngleStatus(ret);
                //            else if (eBufferCV == BufferCV.Buffer_OP)
                //                GetMotionParam().Ctrl_CV[(int)eBufferCV].Set_OP_CST_OppositeAngleStatus(ret);
                //        }
                //    }
                //}
            }
        }
    }
}
