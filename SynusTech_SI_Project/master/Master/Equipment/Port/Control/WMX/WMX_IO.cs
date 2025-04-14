using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using MovenCore;
using System.Diagnostics;

namespace Master.Equipment.Port
{
    /// <summary>
    /// Master Class WMX IO 참조
    /// </summary>
    public partial class Port
    {
        public enum OHT_InputItem
        {
            LP_Foup_Detect,
            OHT_Detect,
            OP_Foup_Detect,
            LP_Placement_Detect,
            LP_Placement_Detect_2,

            Shuttle_Placement_Detect_1,
            Shuttle_Placement_Detect_2,
            OP_Placement_Detect_1,
            OP_Placement_Detect_2,

            Port_Area_Sensor,
            Cart_Detect1,
            Cart_Detect2,

            RM_Fork_Detect,
            MGV_Port_1_EMS_SW,
            POS_Sensor,
            Wait_Pos_Sensor,
            Deg180_Sensor,
            Z_Up_Sensor,

            Valid,
            CS_0,
            TR_Request,
            Busy,
            Complete,

            Door_Close_Status,
            Door_Open_Key_Status,
            Maint_Door_Open,
            Maint_Door_Open2
        }
        public enum AGV_MGV_InputItem
        {
            OP_Foup_Detect,
            LP_Foup_Detect,
            LP_Placement_Detect_1,
            LP_Placement_Detect_2,
            Shuttle_Placement_Detect_1,
            Shuttle_Placement_Detect_2,
            OP_Placement_Detect_1,
            OP_Placement_Detect_2,
            MGV_Port_Area_Sensor,
            RM_Fork_Detect,
            Cart_Detect1,
            Cart_Detect2,
            MGV_Port_1_EMS_SW,
            POS_Sensor,
            Wait_Pos_Sensor,
            Deg180_Sensor,
            Z_Up_Sensor,
            Maint_Door_Open,
            Maint_Door_Open2,
            Valid,
            CS_0,
            TR_Request,
            Busy,
            Complete
        }
        public enum Conveyor_InputItem
        {
            LP_CV_In,
            LP_CV_Out,
            LP_CV_Error,
            LP_CV_FWD_Status,
            LP_CV_BWD_Status,
            LP_Foup_Detect,

            LP_Z_Axis_NOT,
            LP_Z_Axis_Down,
            LP_Z_Axis_Up,
            LP_Z_Axis_POT,
            LP_Z_Axis_Error,

            OP_CV_In,
            OP_CV_Out,
            OP_CV_Error,
            OP_CV_FWD_Status,
            OP_CV_BWD_Status,

            OP_Placement_Detect_1,
            OP_Placement_Detect_2,

            MGV_Port_1_EMS_SW,
            RM_Fork_Detect,

            Valid,
            CS_0,
            TR_Request,
            Busy,
            Complete
        }
        public enum EQ_InputItem
        {
            Load_Request,
            Unload_Request,
            Ready
        }

        public enum OHT_OutputItem
        {
            Red_LED_Lamp_On,
            Green_LED_Lamp_On,
            Buzzer,

            Load_Request,
            Unload_Request,
            Ready,
            HO_AVBL,
            ES,

            DoorOpenRelay
        }
        public enum AGV_MGV_OutputItem
        {
            Red_LED_Lamp_On,
            Green_LED_Lamp_On,
            Buzzer,

            Load_Request,
            Unload_Request,
            Ready,
            ES
        }
        public enum Conveyor_OutputItem
        {
            Red_LED_Lamp_On,
            Green_LED_Lamp_On,
            Buzzer,

            LP_CV_Reset,
            LP_Z_Reset,
            OP_CV_Reset,
            OP_Z_Reset,

            Load_Request,
            Unload_Request,
            Ready,
            ES
        }
        public enum EQ_OutputItem
        {
            TR_Request,
            Busy,
            Complete
        }


        //private bool bLightBarGreen = false;
        //private bool bLightBarRed = false;

        private object UpdateLock = new object();

        public object GetIOUpdateLock()
        {
            return UpdateLock;
        }
        private void WMX_IO_Update()
        {
            lock (UpdateLock)
            {
                WMX_IO_InputToBitMap();
                WMX_IO_BitMapToOutput();
            }
        }

        public bool IsValidInputItemMapping(string Name)
        {
            var PortType = GetParam().ePortType;
            var InputMap = GetMotionParam().Ctrl_IO.InputMap;

            for (int nCount = 0; nCount < InputMap.Length; nCount++)
            {
                var IOMap = InputMap[nCount];

                if (Name != IOMap.Name)
                    continue;

                if (PortType == PortType.OHT || PortType == PortType.MGV_OHT)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                    !string.IsNullOrEmpty(Name) &&
                    Enum.IsDefined(typeof(OHT_InputItem), Name) &&
                    Enum.IsDefined(typeof(OHT_InputItem), IOMap.Name))
                    {
                        return GetMotionParam().IsValidIO(IOMap);
                    }
                }
                else if (PortType == PortType.AGV || PortType == PortType.MGV_AGV || PortType == PortType.MGV)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                   !string.IsNullOrEmpty(Name) &&
                   Enum.IsDefined(typeof(AGV_MGV_InputItem), Name) &&
                   Enum.IsDefined(typeof(AGV_MGV_InputItem), IOMap.Name))
                    {
                        return GetMotionParam().IsValidIO(IOMap);
                    }
                }
                else if (PortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        !string.IsNullOrEmpty(Name) &&
                        Enum.IsDefined(typeof(Conveyor_InputItem), Name) &&
                        Enum.IsDefined(typeof(Conveyor_InputItem), IOMap.Name))
                    {
                        return GetMotionParam().IsValidIO(IOMap);
                    }
                }
                else if (PortType == PortType.EQ)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        !string.IsNullOrEmpty(Name) &&
                        Enum.IsDefined(typeof(EQ_InputItem), Name) &&
                        Enum.IsDefined(typeof(EQ_InputItem), IOMap.Name))
                    {
                        return GetMotionParam().IsValidIO(IOMap);
                    }
                }
            }
            return false;
        }

        private void WMX_IO_InputToBitMap()
        {
            var PortType = GetParam().ePortType;
            var InputMap = GetMotionParam().Ctrl_IO.InputMap;

            for (int nCount = 0; nCount < InputMap.Length; nCount++)
            {
                var IOMap = InputMap[nCount];

                int StartAddr = IOMap.StartAddr;
                int Bit = IOMap.Bit;

                if (StartAddr < 0 || Bit < 0)
                    continue;

                bool bEnable = GetInputBit(StartAddr, Bit);

                if (PortType == PortType.OHT || PortType == PortType.MGV_OHT)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                    Enum.IsDefined(typeof(OHT_InputItem), IOMap.Name))
                    {
                        OHT_InputItem Item = (OHT_InputItem)Enum.Parse(typeof(OHT_InputItem), IOMap.Name);
                        WMX_IO_ItemToMapAction(Item, (IOMap.bInvert ? !bEnable : bEnable));
                    }
                }
                else if (PortType == PortType.AGV || PortType == PortType.MGV_AGV || PortType == PortType.MGV)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        Enum.IsDefined(typeof(AGV_MGV_InputItem), IOMap.Name))
                    {
                        AGV_MGV_InputItem Item = (AGV_MGV_InputItem)Enum.Parse(typeof(AGV_MGV_InputItem), IOMap.Name);
                        WMX_IO_ItemToMapAction(Item, (IOMap.bInvert ? !bEnable : bEnable));
                    }
                }
                else if (PortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        Enum.IsDefined(typeof(Conveyor_InputItem), IOMap.Name))
                    {
                        Conveyor_InputItem Item = (Conveyor_InputItem)Enum.Parse(typeof(Conveyor_InputItem), IOMap.Name);
                        WMX_IO_ItemToMapAction(Item, (IOMap.bInvert ? !bEnable : bEnable));
                    }
                }
                else if (PortType == PortType.EQ)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        Enum.IsDefined(typeof(EQ_InputItem), IOMap.Name))
                    {
                        EQ_InputItem Item = (EQ_InputItem)Enum.Parse(typeof(EQ_InputItem), IOMap.Name);
                        WMX_IO_ItemToMapAction(Item, (IOMap.bInvert ? !bEnable : bEnable));
                    }
                }
            }
        }
        private void WMX_IO_BitMapToOutput()
        {
            var PortType = GetParam().ePortType;
            var OutputMap = GetMotionParam().Ctrl_IO.OutputMap;

            for (int nCount = 0; nCount < OutputMap.Length; nCount++)
            {
                var IOMap = OutputMap[nCount];

                int StartAddr = IOMap.StartAddr;
                int Bit = IOMap.Bit;

                if (StartAddr < 0 || Bit < 0)
                    continue;

                if (PortType == PortType.OHT || PortType == PortType.MGV_OHT)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        Enum.IsDefined(typeof(OHT_OutputItem), IOMap.Name))
                    {
                        OHT_OutputItem Item = (OHT_OutputItem)Enum.Parse(typeof(OHT_OutputItem), IOMap.Name);
                        object MapData = WMX_IO_ItemToMapData(Item);
                        if (MapData != null)
                        {
                            bool bEnable = IOMap.bInvert ? !(bool)MapData : (bool)MapData;
                            m_WMXIO.SetOutputBit(StartAddr, Bit, bEnable);
                        }
                    }
                }
                else if (PortType == PortType.AGV || PortType == PortType.MGV_AGV || PortType == PortType.MGV)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        Enum.IsDefined(typeof(AGV_MGV_OutputItem), IOMap.Name))
                    {
                        AGV_MGV_OutputItem Item = (AGV_MGV_OutputItem)Enum.Parse(typeof(AGV_MGV_OutputItem), IOMap.Name);
                        object MapData = WMX_IO_ItemToMapData(Item);
                        if (MapData != null)
                        {
                            bool bEnable = IOMap.bInvert ? !(bool)MapData : (bool)MapData;
                            m_WMXIO.SetOutputBit(StartAddr, Bit, bEnable);
                        }
                    }
                }
                else if (PortType == PortType.Conveyor_AGV || GetParam().ePortType == PortType.Conveyor_OMRON)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        Enum.IsDefined(typeof(Conveyor_OutputItem), IOMap.Name))
                    {
                        Conveyor_OutputItem Item = (Conveyor_OutputItem)Enum.Parse(typeof(Conveyor_OutputItem), IOMap.Name);
                        object MapData = WMX_IO_ItemToMapData(Item);
                        if (MapData != null)
                        {
                            bool bEnable = IOMap.bInvert ? !(bool)MapData : (bool)MapData;
                            m_WMXIO.SetOutputBit(StartAddr, Bit, bEnable);
                        }
                    }
                }
                else if (PortType == PortType.EQ)
                {
                    if (!string.IsNullOrEmpty(IOMap.Name) &&
                        Enum.IsDefined(typeof(EQ_OutputItem), IOMap.Name))
                    {
                        EQ_OutputItem Item = (EQ_OutputItem)Enum.Parse(typeof(EQ_OutputItem), IOMap.Name);
                        object MapData = WMX_IO_ItemToMapData(Item);
                        if (MapData != null)
                        {
                            bool bEnable = IOMap.bInvert ? !(bool)MapData : (bool)MapData;
                            m_WMXIO.SetOutputBit(StartAddr, Bit, bEnable);
                        }
                    }
                }
            }
        }

        public bool GetInputBit(int StartAddr, int Bit)
        {
            return m_WMXIO.GetInputBit(StartAddr, Bit);
        }
        public bool GetOutBit(int StartAddr, int Bit)
        {
            return m_WMXIO.GetOutputBit(StartAddr, Bit);
        }

        public void WMX_IO_ItemToMapAction(OHT_InputItem eOHT_InputItem, bool bEnable)
        {
            switch (eOHT_InputItem)
            {
                case OHT_InputItem.LP_Foup_Detect:
                    Sensor_LP_CST_Presence = !bEnable;
                    break;
                case OHT_InputItem.OHT_Detect:
                    Sensor_LP_Hoist_Detect = !bEnable;
                    break;
                case OHT_InputItem.OP_Foup_Detect:
                    Sensor_OP_CST_Presence = !bEnable;
                    break;
                case OHT_InputItem.LP_Placement_Detect:
                    Sensor_LP_CST_Detect1 = !bEnable;
                    break;
                case OHT_InputItem.LP_Placement_Detect_2:
                    Sensor_LP_CST_Detect2 = !bEnable;
                    break;
                case OHT_InputItem.Shuttle_Placement_Detect_1:
                    Sensor_Shuttle_CSTDetect1 = !bEnable;
                    break;
                case OHT_InputItem.Shuttle_Placement_Detect_2:
                    Sensor_Shuttle_CSTDetect2 = !bEnable;
                    break;
                case OHT_InputItem.OP_Placement_Detect_1:
                    Sensor_OP_CST_Detect1 = !bEnable;
                    break;
                case OHT_InputItem.OP_Placement_Detect_2:
                    Sensor_OP_CST_Detect2 = !bEnable;
                    break;
                case OHT_InputItem.MGV_Port_1_EMS_SW:
                    Sensor_HWEStop = !bEnable;
                    break;
                case OHT_InputItem.Port_Area_Sensor:
                    Sensor_LightCurtain = !bEnable;
                    break;
                case OHT_InputItem.Cart_Detect1:
                    Sensor_LP_Cart_Detect1 = bEnable;
                    break;
                case OHT_InputItem.Cart_Detect2:
                    Sensor_LP_Cart_Detect2 = bEnable;
                    break;
                case OHT_InputItem.RM_Fork_Detect:
                    Sensor_OP_Fork_Detect = !bEnable;
                    break;
                case OHT_InputItem.POS_Sensor:
                    Sensor_X_Axis_POS = bEnable;
                    break;
                case OHT_InputItem.Wait_Pos_Sensor:
                    Sensor_X_Axis_WaitPosSensor = bEnable;
                    break;
                case OHT_InputItem.Deg180_Sensor:
                    Sensor_T_Axis_180DegSensor = bEnable;
                    break;
                case OHT_InputItem.Z_Up_Sensor:
                    Sensor_Z_Axis_POS = bEnable;
                    break;
                case OHT_InputItem.Valid:
                    PIOStatus_OHTToPort_Valid = bEnable;
                    break;
                case OHT_InputItem.CS_0:
                    PIOStatus_OHTToPort_CS0 = bEnable;
                    break;
                case OHT_InputItem.TR_Request:
                    PIOStatus_OHTToPort_TR_Req = bEnable;
                    break;
                case OHT_InputItem.Busy:
                    PIOStatus_OHTToPort_Busy = bEnable;
                    break;
                case OHT_InputItem.Complete:
                    PIOStatus_OHTToPort_Complete = bEnable;
                    break;
                case OHT_InputItem.Door_Close_Status:
                    Status_OHT_Door_Close = bEnable;
                    break;
                case OHT_InputItem.Door_Open_Key_Status:
                    Status_OHT_Key_Open_Status = bEnable;
                    break;
                case OHT_InputItem.Maint_Door_Open:
                    Status_Maint_Door_Open_Status = !bEnable;
                    break;
                case OHT_InputItem.Maint_Door_Open2:
                    Status_Maint_Door_Open_Status2 = !bEnable;
                    break;
            }
        }
        public void WMX_IO_ItemToMapAction(AGV_MGV_InputItem eAGV_MGV_InputItem, bool bEnable)
        {
            switch (eAGV_MGV_InputItem)
            {
                case AGV_MGV_InputItem.LP_Foup_Detect:
                    Sensor_LP_CST_Presence = !bEnable;
                    break;
                case AGV_MGV_InputItem.OP_Foup_Detect:
                    Sensor_OP_CST_Presence = !bEnable;
                    break;
                case AGV_MGV_InputItem.OP_Placement_Detect_1:
                    Sensor_OP_CST_Detect1 = !bEnable;
                    break;
                case AGV_MGV_InputItem.OP_Placement_Detect_2:
                    Sensor_OP_CST_Detect2 = !bEnable;
                    break;
                case AGV_MGV_InputItem.Shuttle_Placement_Detect_1:
                    Sensor_Shuttle_CSTDetect1 = !bEnable;
                    break;
                case AGV_MGV_InputItem.Shuttle_Placement_Detect_2:
                    Sensor_Shuttle_CSTDetect2 = !bEnable;
                    break;
                case AGV_MGV_InputItem.LP_Placement_Detect_1:
                    Sensor_LP_CST_Detect1 = !bEnable;
                    break;
                case AGV_MGV_InputItem.LP_Placement_Detect_2:
                    Sensor_LP_CST_Detect2 = !bEnable;
                    break;
                case AGV_MGV_InputItem.MGV_Port_Area_Sensor:
                    Sensor_LightCurtain = !bEnable;
                    break;
                case AGV_MGV_InputItem.RM_Fork_Detect:
                    Sensor_OP_Fork_Detect = !bEnable;
                    break;
                case AGV_MGV_InputItem.Cart_Detect1:
                    Sensor_LP_Cart_Detect1 = bEnable;
                    break;
                case AGV_MGV_InputItem.Cart_Detect2:
                    Sensor_LP_Cart_Detect2 = bEnable;
                    break;
                case AGV_MGV_InputItem.MGV_Port_1_EMS_SW:
                    Sensor_HWEStop = !bEnable;
                    break;
                case AGV_MGV_InputItem.POS_Sensor:
                    Sensor_X_Axis_POS = bEnable;
                    break;
                case AGV_MGV_InputItem.Wait_Pos_Sensor:
                    Sensor_X_Axis_WaitPosSensor = bEnable;
                    break;
                case AGV_MGV_InputItem.Deg180_Sensor:
                    Sensor_T_Axis_180DegSensor = bEnable;
                    break;
                case AGV_MGV_InputItem.Z_Up_Sensor:
                    Sensor_Z_Axis_POS = bEnable;
                    break;
                case AGV_MGV_InputItem.Maint_Door_Open:
                    Status_Maint_Door_Open_Status = !bEnable;
                    break;
                case AGV_MGV_InputItem.Maint_Door_Open2:
                    Status_Maint_Door_Open_Status2 = !bEnable;
                    break;
                case AGV_MGV_InputItem.Valid:
                    PIOStatus_AGVToPort_Valid = bEnable;
                    break;
                case AGV_MGV_InputItem.CS_0:
                    PIOStatus_AGVToPort_CS0 = bEnable;
                    break;
                case AGV_MGV_InputItem.TR_Request:
                    PIOStatus_AGVToPort_TR_Req = bEnable;
                    break;
                case AGV_MGV_InputItem.Busy:
                    PIOStatus_AGVToPort_Busy = bEnable;
                    break;
                case AGV_MGV_InputItem.Complete:
                    PIOStatus_AGVToPort_Complete = bEnable;
                    break;

                //case AGV_MGV_InputItem.Door_Open:
                //    return Sensor_Port_Set_DoorOpen_Status;
                //case AGV_MGV_InputItem.Door_E_Stop:
                //    return Sensor_Port_Set_Door_E_Stop_Status;
                //case AGV_MGV_InputItem.Auto_Key_Switch:
                //    return Sensor_Port_Set_AutoKeySwitch_Status;

            }
        }
        public void WMX_IO_ItemToMapAction(Conveyor_InputItem eConveyor_InputItem, bool bEnable)
        {
            switch (eConveyor_InputItem)
            {
                case Conveyor_InputItem.LP_Foup_Detect:
                    Sensor_LP_CST_Presence = !bEnable;
                    break;
                case Conveyor_InputItem.LP_CV_In:
                    Sensor_LP_CV_IN = !bEnable;
                    break;
                case Conveyor_InputItem.LP_CV_Out:
                    Sensor_LP_CV_STOP= !bEnable;
                    break;
                case Conveyor_InputItem.LP_CV_Error:
                    Sensor_LP_CV_Error = bEnable;
                    break;

                case Conveyor_InputItem.LP_CV_FWD_Status:
                    if(GetParam().ePortType == PortType.Conveyor_OMRON)
                    {
                        Sensor_LP_CV_FWD_Status = bEnable;
                    }
                    break;
                case Conveyor_InputItem.LP_CV_BWD_Status:
                    if (GetParam().ePortType == PortType.Conveyor_OMRON)
                    {
                        Sensor_LP_CV_BWD_Status = bEnable;
                    }
                    break;
                case Conveyor_InputItem.LP_Z_Axis_NOT:
                    Sensor_LP_Z_NOT = bEnable;
                    break;
                case Conveyor_InputItem.LP_Z_Axis_Down:
                    Sensor_LP_Z_POS1 = bEnable;
                    break;
                case Conveyor_InputItem.LP_Z_Axis_Up:
                    Sensor_LP_Z_POS2 = bEnable;
                    break;
                case Conveyor_InputItem.LP_Z_Axis_POT:
                    Sensor_LP_Z_POT = bEnable;
                    break;
                case Conveyor_InputItem.LP_Z_Axis_Error:
                    Sensor_LP_Z_Error = bEnable;
                    break;

                case Conveyor_InputItem.OP_CV_In:
                    Sensor_OP_CV_IN = !bEnable;
                    break;
                case Conveyor_InputItem.OP_CV_Out:
                    Sensor_OP_CV_STOP = !bEnable;
                    break;
                case Conveyor_InputItem.OP_CV_Error:
                    Sensor_OP_CV_Error = bEnable;
                    break;
                case Conveyor_InputItem.OP_CV_FWD_Status:
                    if (GetParam().ePortType == PortType.Conveyor_OMRON)
                    {
                        Sensor_OP_CV_FWD_Status = bEnable;
                    }
                    break;
                case Conveyor_InputItem.OP_CV_BWD_Status:
                    if (GetParam().ePortType == PortType.Conveyor_OMRON)
                    {
                        Sensor_OP_CV_BWD_Status = bEnable;
                    }
                    break;
                //case Conveyor_InputItem.OP_Z_Axis_NOT:
                //    Sensor_OP_Z_NOT = bEnable;
                //    break;
                //case Conveyor_InputItem.OP_Z_Axis_Down:
                //    Sensor_OP_Z_POS1 = bEnable;
                //    break;
                //case Conveyor_InputItem.OP_Z_Axis_Up:
                //    Sensor_OP_Z_POS2 = bEnable;
                //    break;
                //case Conveyor_InputItem.OP_Z_Axis_POT:
                //    Sensor_OP_Z_POT = bEnable;
                //    break;
                //case Conveyor_InputItem.OP_Z_Axis_Error:
                //    Sensor_OP_Z_Error = bEnable;
                //    break;
                case Conveyor_InputItem.OP_Placement_Detect_1:
                    Sensor_OP_CST_Detect1 = !bEnable;
                    break;
                case Conveyor_InputItem.OP_Placement_Detect_2:
                    Sensor_OP_CST_Detect2 = !bEnable;
                    break;

                //case Conveyor_InputItem.POS_Sensor:
                //    Sensor_X_Axis_POS = bEnable;
                //    break;
                //case Conveyor_InputItem.Wait_Pos_Sensor:
                //    Sensor_X_Axis_WaitPosSensor = bEnable;
                //    break;
                //case Conveyor_InputItem.Deg180_Sensor:
                //    Sensor_T_Axis_180DegSensor = bEnable;
                //    break;

                //case Conveyor_InputItem.MGV_Port_Area_Sensor:
                //    Sensor_LightCurtain = bEnable;
                //    break;
                case Conveyor_InputItem.MGV_Port_1_EMS_SW:
                    Sensor_HWEStop = !bEnable;
                    break;
                case Conveyor_InputItem.RM_Fork_Detect:
                    Sensor_OP_Fork_Detect = !bEnable;
                    break;

                case Conveyor_InputItem.Valid:
                    PIOStatus_AGVToPort_Valid = bEnable;
                    break;
                case Conveyor_InputItem.CS_0:
                    PIOStatus_AGVToPort_CS0 = bEnable;
                    break;
                case Conveyor_InputItem.TR_Request:
                    PIOStatus_AGVToPort_TR_Req = bEnable;
                    break;
                case Conveyor_InputItem.Busy:
                    PIOStatus_AGVToPort_Busy = bEnable;
                    break;
                case Conveyor_InputItem.Complete:
                    PIOStatus_AGVToPort_Complete = bEnable;
                    break;
            }
        }
        public void WMX_IO_ItemToMapAction(EQ_InputItem eEQ_InputItem, bool bEnable)
        {
            switch (eEQ_InputItem)
            {
                case EQ_InputItem.Load_Request:
                    PIOStatus_EQToRM_Load_Req = bEnable;
                    break;
                case EQ_InputItem.Unload_Request:
                    PIOStatus_EQToRM_Unload_Req = bEnable;
                    break;
                case EQ_InputItem.Ready:
                    PIOStatus_EQToRM_Ready = bEnable;
                    break;
            }
        }
        
        public object WMX_IO_ItemToMapData(OHT_InputItem eOHT_InputItem)
        {
            switch (eOHT_InputItem)
            {
                case OHT_InputItem.LP_Foup_Detect:
                    return Sensor_LP_CST_Presence;
                case OHT_InputItem.OHT_Detect:
                    return Sensor_LP_Hoist_Detect;
                case OHT_InputItem.OP_Foup_Detect:
                    return Sensor_OP_CST_Presence;
                case OHT_InputItem.LP_Placement_Detect:
                    return Sensor_LP_CST_Detect1;
                case OHT_InputItem.LP_Placement_Detect_2:
                    return Sensor_LP_CST_Detect2;
                case OHT_InputItem.Shuttle_Placement_Detect_1:
                    return Sensor_Shuttle_CSTDetect1;
                case OHT_InputItem.Shuttle_Placement_Detect_2:
                    return Sensor_Shuttle_CSTDetect2;
                case OHT_InputItem.OP_Placement_Detect_1:
                    return Sensor_OP_CST_Detect1;
                case OHT_InputItem.OP_Placement_Detect_2:
                    return Sensor_OP_CST_Detect2;
                case OHT_InputItem.MGV_Port_1_EMS_SW:
                    return Sensor_HWEStop;
                case OHT_InputItem.Port_Area_Sensor:
                    return Sensor_LightCurtain;
                case OHT_InputItem.Cart_Detect1:
                    return Sensor_LP_Cart_Detect1;
                case OHT_InputItem.Cart_Detect2:
                    return Sensor_LP_Cart_Detect2;
                case OHT_InputItem.RM_Fork_Detect:
                    return Sensor_OP_Fork_Detect;
                case OHT_InputItem.POS_Sensor:
                    return Sensor_X_Axis_POS;
                case OHT_InputItem.Wait_Pos_Sensor:
                    return Sensor_X_Axis_WaitPosSensor;
                case OHT_InputItem.Deg180_Sensor:
                    return Sensor_T_Axis_180DegSensor;
                case OHT_InputItem.Z_Up_Sensor:
                    return Sensor_Z_Axis_POS;
                case OHT_InputItem.Valid:
                    return PIOStatus_OHTToPort_Valid;
                case OHT_InputItem.CS_0:
                    return PIOStatus_OHTToPort_CS0;
                case OHT_InputItem.TR_Request:
                    return PIOStatus_OHTToPort_TR_Req;
                case OHT_InputItem.Busy:
                    return PIOStatus_OHTToPort_Busy;
                case OHT_InputItem.Complete:
                    return PIOStatus_OHTToPort_Complete;

                case OHT_InputItem.Door_Close_Status:
                    return Status_OHT_Door_Close;
                case OHT_InputItem.Door_Open_Key_Status:
                    return Status_OHT_Key_Open_Status;
                case OHT_InputItem.Maint_Door_Open:
                    return Status_Maint_Door_Open_Status;
                case OHT_InputItem.Maint_Door_Open2:
                    return Status_Maint_Door_Open_Status2;

                default:
                    return null;
            }
        }
        public object WMX_IO_ItemToMapData(OHT_OutputItem eOHT_OutputItem)
        {
            switch (eOHT_OutputItem)
            {
                case OHT_OutputItem.Red_LED_Lamp_On:
                    return Sensor_LP_LEDBar_Red;
                case OHT_OutputItem.Green_LED_Lamp_On:
                    return Sensor_LP_LEDBar_Green;
                case OHT_OutputItem.Buzzer:
                    return Sensor_Buzzer;

                case OHT_OutputItem.Load_Request:
                    return PIOStatus_PortToOHT_Load_Req;
                case OHT_OutputItem.Unload_Request:
                    return PIOStatus_PortToOHT_Unload_Req;
                case OHT_OutputItem.Ready:
                    return PIOStatus_PortToOHT_Ready;
                case OHT_OutputItem.HO_AVBL:
                    return PIOStatus_PortToOHT_HO_AVBL;
                case OHT_OutputItem.ES:
                    return PIOStatus_PortToOHT_ES;
                case OHT_OutputItem.DoorOpenRelay:
                    return CMD_OHT_Door_Open;
                default:
                    return null;
            }
        }
        
        public object WMX_IO_ItemToMapData(AGV_MGV_InputItem eAGV_MGV_InputItem)
        {
            switch (eAGV_MGV_InputItem)
            {
                case AGV_MGV_InputItem.LP_Foup_Detect:
                    return Sensor_LP_CST_Presence;
                case AGV_MGV_InputItem.OP_Foup_Detect:
                    return Sensor_OP_CST_Presence;
                case AGV_MGV_InputItem.OP_Placement_Detect_1:
                    return Sensor_OP_CST_Detect1;
                case AGV_MGV_InputItem.OP_Placement_Detect_2:
                    return Sensor_OP_CST_Detect2;
                case AGV_MGV_InputItem.Shuttle_Placement_Detect_1:
                    return Sensor_Shuttle_CSTDetect1;
                case AGV_MGV_InputItem.Shuttle_Placement_Detect_2:
                    return Sensor_Shuttle_CSTDetect2;
                case AGV_MGV_InputItem.LP_Placement_Detect_1:
                    return Sensor_LP_CST_Detect1;
                case AGV_MGV_InputItem.LP_Placement_Detect_2:
                    return Sensor_LP_CST_Detect2;
                case AGV_MGV_InputItem.MGV_Port_Area_Sensor:
                    return Sensor_LightCurtain;
                case AGV_MGV_InputItem.RM_Fork_Detect:
                    return Sensor_OP_Fork_Detect;
                case AGV_MGV_InputItem.Cart_Detect1:
                    return Sensor_LP_Cart_Detect1;
                case AGV_MGV_InputItem.Cart_Detect2:
                    return Sensor_LP_Cart_Detect2;
                case AGV_MGV_InputItem.MGV_Port_1_EMS_SW:
                    return Sensor_HWEStop;
                case AGV_MGV_InputItem.POS_Sensor:
                    return Sensor_X_Axis_POS;
                case AGV_MGV_InputItem.Z_Up_Sensor:
                    return Sensor_Z_Axis_POS;
                case AGV_MGV_InputItem.Wait_Pos_Sensor:
                    return Sensor_X_Axis_WaitPosSensor;
                case AGV_MGV_InputItem.Deg180_Sensor:
                    return Sensor_T_Axis_180DegSensor;
                case AGV_MGV_InputItem.Maint_Door_Open:
                    return Status_Maint_Door_Open_Status;
                case AGV_MGV_InputItem.Maint_Door_Open2:
                    return Status_Maint_Door_Open_Status2;
                case AGV_MGV_InputItem.Valid:
                    return PIOStatus_AGVToPort_Valid;
                case AGV_MGV_InputItem.CS_0:
                    return PIOStatus_AGVToPort_CS0;
                case AGV_MGV_InputItem.TR_Request:
                    return PIOStatus_AGVToPort_TR_Req;
                case AGV_MGV_InputItem.Busy:
                    return PIOStatus_AGVToPort_Busy;
                case AGV_MGV_InputItem.Complete:
                    return PIOStatus_AGVToPort_Complete;

                //case AGV_MGV_InputItem.Door_Open:
                //    return Sensor_Port_Is_DoorOpen_Status();
                //case AGV_MGV_InputItem.Door_E_Stop:
                //    return Sensor_Port_Is_Door_E_Stop_Status();
                //case AGV_MGV_InputItem.Auto_Key_Switch:
                //    return Sensor_Port_Is_AutoKeySwitch_Status();

                default:
                    return null;
            }
        }
        public object WMX_IO_ItemToMapData(AGV_MGV_OutputItem eAGV_MGV_OutputItem)
        {
            switch (eAGV_MGV_OutputItem)
            {
                case AGV_MGV_OutputItem.Red_LED_Lamp_On:
                    return Sensor_LP_LEDBar_Red;
                case AGV_MGV_OutputItem.Green_LED_Lamp_On:
                    return Sensor_LP_LEDBar_Green;
                case AGV_MGV_OutputItem.Buzzer:
                    return Sensor_Buzzer;

                case AGV_MGV_OutputItem.Load_Request:
                    return PIOStatus_PortToAGV_Load_Req;
                case AGV_MGV_OutputItem.Unload_Request:
                    return PIOStatus_PortToAGV_Unload_Req;
                case AGV_MGV_OutputItem.Ready:
                    return PIOStatus_PortToAGV_Ready;
                case AGV_MGV_OutputItem.ES:
                    return PIOStatus_PortToAGV_ES;
                default:
                    return null;
            }
        }

        public object WMX_IO_ItemToMapData(Conveyor_InputItem eConveyor_InputItem)
        {
            switch (eConveyor_InputItem)
            {
                case Conveyor_InputItem.LP_Foup_Detect:
                    return Sensor_LP_CST_Presence;
                case Conveyor_InputItem.LP_CV_In:
                    return Sensor_LP_CV_IN;
                case Conveyor_InputItem.LP_CV_Out:
                    return Sensor_LP_CV_STOP;
                case Conveyor_InputItem.LP_CV_Error:
                    return Sensor_LP_CV_Error;
                case Conveyor_InputItem.LP_CV_FWD_Status:
                    return Sensor_LP_CV_FWD_Status;
                case Conveyor_InputItem.LP_CV_BWD_Status:
                    return Sensor_LP_CV_BWD_Status;

                case Conveyor_InputItem.LP_Z_Axis_NOT:
                    return Sensor_LP_Z_NOT;
                case Conveyor_InputItem.LP_Z_Axis_Down:
                    return Sensor_LP_Z_POS1;
                case Conveyor_InputItem.LP_Z_Axis_Up:
                    return Sensor_LP_Z_POS2;
                case Conveyor_InputItem.LP_Z_Axis_POT:
                    return Sensor_LP_Z_POT;
                case Conveyor_InputItem.LP_Z_Axis_Error:
                    return Sensor_LP_Z_Error;

                case Conveyor_InputItem.OP_CV_In:
                    return Sensor_OP_CV_IN;
                case Conveyor_InputItem.OP_CV_Out:
                    return Sensor_OP_CV_STOP;
                case Conveyor_InputItem.OP_CV_Error:
                    return Sensor_OP_CV_Error;
                case Conveyor_InputItem.OP_CV_FWD_Status:
                    return Sensor_OP_CV_FWD_Status;
                case Conveyor_InputItem.OP_CV_BWD_Status:
                    return Sensor_OP_CV_BWD_Status;
                case Conveyor_InputItem.OP_Placement_Detect_1:
                    return Sensor_OP_CST_Detect1;
                case Conveyor_InputItem.OP_Placement_Detect_2:
                    return Sensor_OP_CST_Detect2;
                //case Conveyor_InputItem.OP_Z_Axis_NOT:
                //    return Sensor_OP_Z_NOT;
                //case Conveyor_InputItem.OP_Z_Axis_Down:
                //    return Sensor_OP_Z_POS1;
                //case Conveyor_InputItem.OP_Z_Axis_Up:
                //    return Sensor_OP_Z_POS2;
                //case Conveyor_InputItem.OP_Z_Axis_POT:
                //    return Sensor_OP_Z_POT;
                //case Conveyor_InputItem.OP_Z_Axis_Error:
                //    return Sensor_OP_Z_Error;

                //case Conveyor_InputItem.POS_Sensor:
                //    return Sensor_X_Axis_POS;
                //case Conveyor_InputItem.Wait_Pos_Sensor:
                //    return Sensor_X_Axis_WaitPosSensor;
                //case Conveyor_InputItem.Deg180_Sensor:
                //    return Sensor_T_Axis_180DegSensor;

                //case Conveyor_InputItem.MGV_Port_Area_Sensor:
                //    return Sensor_LightCurtain;
                case Conveyor_InputItem.MGV_Port_1_EMS_SW:
                    return Sensor_HWEStop;
                case Conveyor_InputItem.RM_Fork_Detect:
                    return Sensor_OP_Fork_Detect;

               
                case Conveyor_InputItem.Valid:
                    return PIOStatus_AGVToPort_Valid;
                case Conveyor_InputItem.CS_0:
                    return PIOStatus_AGVToPort_CS0;
                case Conveyor_InputItem.TR_Request:
                    return PIOStatus_AGVToPort_TR_Req;
                case Conveyor_InputItem.Busy:
                    return PIOStatus_AGVToPort_Busy;
                case Conveyor_InputItem.Complete:
                    return PIOStatus_AGVToPort_Complete;

                //case Conveyor_InputItem.Door_Open:
                //    return Sensor_Port_Is_DoorOpen_Status();
                //case Conveyor_InputItem.Door_E_Stop:
                //    return Sensor_Port_Is_Door_E_Stop_Status();
                //case Conveyor_InputItem.Auto_Key_Switch:
                //    return Sensor_Port_Is_AutoKeySwitch_Status();

                default:
                    return null;
            }
        }
        public object WMX_IO_ItemToMapData(Conveyor_OutputItem eConveyor_OutputItem)
        {
            switch (eConveyor_OutputItem)
            {
                case Conveyor_OutputItem.Red_LED_Lamp_On:
                    return Sensor_LP_LEDBar_Red;
                case Conveyor_OutputItem.Green_LED_Lamp_On:
                    return Sensor_LP_LEDBar_Green;
                case Conveyor_OutputItem.Buzzer:
                    return Sensor_Buzzer;


                case Conveyor_OutputItem.LP_CV_Reset:
                    return Sensor_LP_CV_Reset;
                case Conveyor_OutputItem.LP_Z_Reset:
                    return Sensor_LP_Z_Reset;
                case Conveyor_OutputItem.OP_CV_Reset:
                    return Sensor_OP_CV_Reset;
                case Conveyor_OutputItem.OP_Z_Reset:
                    return Sensor_OP_Z_Reset;

                case Conveyor_OutputItem.Load_Request:
                    return PIOStatus_PortToAGV_Load_Req;
                case Conveyor_OutputItem.Unload_Request:
                    return PIOStatus_PortToAGV_Unload_Req;
                case Conveyor_OutputItem.Ready:
                    return PIOStatus_PortToAGV_Ready;
                case Conveyor_OutputItem.ES:
                    return PIOStatus_PortToAGV_ES;
                default:
                    return null;
            }
        }

        public object WMX_IO_ItemToMapData(EQ_InputItem eEQ_InputItem)
        {
            switch (eEQ_InputItem)
            {
                case EQ_InputItem.Load_Request:
                    return PIOStatus_EQToRM_Load_Req;
                case EQ_InputItem.Unload_Request:
                    return PIOStatus_EQToRM_Unload_Req;
                case EQ_InputItem.Ready:
                    return PIOStatus_EQToRM_Ready;
                default:
                    return null;
            }
        }
        public object WMX_IO_ItemToMapData(EQ_OutputItem eEQ_OutputItem)
        {
            switch (eEQ_OutputItem)
            {
                case EQ_OutputItem.TR_Request:
                    return PIOStatus_STKToPort_TR_REQ;
                case EQ_OutputItem.Busy:
                    return PIOStatus_STKToPort_Busy;
                case EQ_OutputItem.Complete:
                    return PIOStatus_STKToPort_Complete;
                default:
                    return null;
            }
        }
    }
}
