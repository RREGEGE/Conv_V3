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
    /// Port_ControlCylinder.cs 는 Port의 축 타입 중 Cylinder 축 관련 제어 기능이 작성된 페이지
    /// Flag -> 명령, Status -> 상태
    /// </summary>
    partial class Port
    {
        /// <summary>
        /// 축이 실린더 타입인지 체크
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <returns></returns>
        private bool Check_CylinderType(PortAxis ePortAxis)
        {
            if (GetMotionParam().Ctrl_Axis[(int)ePortAxis].eAxisCtrlType != AxisCtrlType.Cylinder)
            {
                LogMsg.AddPortLog(GetParam().ID, LogMsg.LogLevel.Error, LogMsg.MsgList.InvalidControlType, $"Control Type: {GetMotionParam().Ctrl_Axis[(int)ePortAxis].eAxisCtrlType}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 실린더 제어 명령
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eCylCtrlList"></param>
        /// <param name="bEnable"></param>
        public void CylinderCtrl_SetRunFlag(PortAxis ePortAxis, CylCtrlList eCylCtrlList, bool bEnable)
        {
            if (!Check_CylinderType(ePortAxis))
                return;

            GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.SetRunFlag(eCylCtrlList, bEnable);
        }

        /// <summary>
        /// 실린더 정지 명령
        /// </summary>
        /// <param name="ePortAxis"></param>
        public void CylinderCtrl_MotionStop(PortAxis ePortAxis)
        {
            foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
            {
                CylinderCtrl_SetRunFlag(ePortAxis, eCylCtrlList, false);
            }
        }

        /// <summary>
        /// 실린더 구동 중 여부 체크 (Run Status와 사실 상 같은 기능)
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <returns></returns>
        public bool CylinderCtrl_Is_Busy(PortAxis ePortAxis)
        {
            return GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.IsBusy();
        }

        /// <summary>
        /// 실린더 동작 상태 체크 (출력 비트 기반)
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eCylCtrlList"></param>
        /// <returns></returns>
        public bool CylinderCtrl_GetRunStatus(PortAxis ePortAxis, CylCtrlList eCylCtrlList)
        {
            return GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.GetRunStatus(eCylCtrlList);
        }

        /// <summary>
        /// 실린더 Target에 대한 위치 감지 센서 On 여부 체크
        /// </summary>
        /// <param name="ePortAxis"></param>
        /// <param name="eCylCtrlList"></param>
        /// <returns></returns>
        public bool CylinderCtrl_GetPosSensorOn(PortAxis ePortAxis, CylCtrlList eCylCtrlList)
        {
            return GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.GetPosSensorStatus(eCylCtrlList);
        }
        /// <summary>
        /// Inverter Status to Bit, Word Map
        /// </summary>
        private void CylinderCtrl_BitWordUpdate(PortAxis ePortAxis)
        {
            bool bBusy = CylinderCtrl_Is_Busy(ePortAxis);

            if (Status_EStop && bBusy)
                CylinderCtrl_MotionStop(ePortAxis);

            foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
            {
                var IOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.GetCtrlIOParam(eCylCtrlList);

                if (GetMotionParam().IsValidIO(IOParam))
                {
                    int StartAddr = IOParam.StartAddr;
                    int Bit = IOParam.Bit;
                    bool ret = GetOutBit(StartAddr, Bit);
                    GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.SetRunStatus(eCylCtrlList, ret);

                    var Status = GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.GetRunStatus(eCylCtrlList);
                    var Flag = GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.GetRunFlag(eCylCtrlList);

                    if (Flag != Status)
                        m_WMXIO.SetOutputBit(StartAddr, Bit, Flag);
                }
            }

            foreach (CylCtrlList eCylCtrlList in Enum.GetValues(typeof(CylCtrlList)))
            {
                var IOParam = GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.GetPosSensorIOParam(eCylCtrlList);

                if (GetMotionParam().IsValidIO(IOParam))
                {
                    int StartAddr = IOParam.StartAddr;
                    int Bit = IOParam.Bit;
                    bool ret = GetInputBit(StartAddr, Bit);

                    GetMotionParam().Ctrl_Axis[(int)ePortAxis].cylinderParam.SetPosSensorStatus(eCylCtrlList, ret);
                }
            }

            switch (ePortAxis)
            {
                case PortAxis.Shuttle_X:
                    Sensor_X_Axis_Busy = bBusy;
                    break;
                case PortAxis.Shuttle_Z:
                    Sensor_Z_Axis_Busy = bBusy;
                    Sensor_Z_Axis_FWDSensor = CylinderCtrl_GetPosSensorOn(ePortAxis, CylCtrlList.FWD);
                    Sensor_Z_Axis_BWDSensor = CylinderCtrl_GetPosSensorOn(ePortAxis, CylCtrlList.BWD);
                    break;
                case PortAxis.Shuttle_T:
                    Sensor_T_Axis_Busy = bBusy;
                    break;
                default:
                    bPortAxisBusy[(int)ePortAxis] = bBusy;
                    break;
            }
        }
    }
}
