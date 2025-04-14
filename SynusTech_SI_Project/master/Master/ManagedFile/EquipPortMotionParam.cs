using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Master.Interface.MyFileIO;
using Master.Equipment.Port;
using Master.Equipment.Port.TagReader.ReaderEquip;
using System.Xml.Serialization;
using MovenCore;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Master.ManagedFile
{
    public static class EquipPortMotionParam
    {
        /// <summary>
        /// Port Motion 제어와 관련된 파라미터 클래스
        /// 공정 방향, 축 정보, 컨베이어 정보, IO 정보, WatchDog 정보
        /// </summary>
        public class PortMotionParameter
        {
            /// <summary>
            /// 포트의 전반적인 모션은 아래의 정보를 참조하여 진행 됨
            /// </summary>
            public Port.ShuttleCtrlBufferType eBufferType   = Port.ShuttleCtrlBufferType.Two_Buffer;    //Port Buffer Type - One, Two
            public Port.PortDirection ePortDirection        = Port.PortDirection.Input;                 //Port Direction - In, Out
            public int AutoRun_Ratio                        = 50;                                       //Port 오토 공정 시 속도 비율
            public AxisControlParam[]   Ctrl_Axis           = new AxisControlParam[Enum.GetValues(typeof(Port.PortAxis)).Length];   //포트 축 할당(어떤 축을 사용할지, 어떤 축을 어떤 타입으로 사용할지, PortAxis Enum 참고)
            public BufferCVParam[]      Ctrl_CV             = new BufferCVParam[Enum.GetValues(typeof(Port.BufferCV)).Length];      //포트 컨베이어로 운용된느 경우 컨베이어 관련 파라미터, BufferCV Enum 참고
            public IOMap                Ctrl_IO             = new IOMap();                                                          //포트의 센서 맵 정보(IO 접점 주소)
            public RFIDReader.Model eRFIDModel              = RFIDReader.Model.CH2; //ANT 4CH 짜리 대비 하여 만든 옵션
            public int TagReadSize                          = 32;                   //ReadSize 조정 대비 만든 옵션
            public bool TagReadFailError                    = true;                                                                 //Tag Read 실패 시 공정 진행 유무
            public Port_WatchdogParam WatchdogDetectParam   = new Port_WatchdogParam();                                             //Watchdog 설정 값

            public PortMotionParameter()
            {
                for (int nCount = 0; nCount < Ctrl_Axis.Length; nCount++)
                    Ctrl_Axis[nCount] = new AxisControlParam();
                for (int nCount = 0; nCount < Ctrl_CV.Length; nCount++)
                    Ctrl_CV[nCount] = new BufferCVParam();
            }

            /// <summary>
            /// MasterSafetyImageInfo.cs의 Load, Save 과정 참고
            /// </summary>
            /// <param name="PortId"></param>
            /// <param name="portMotionParameter"></param>
            public void Load(string PortId, ref PortMotionParameter portMotionParameter)
            {
                try
                {
                    string filePath = ManagedFileInfo.EquipMotionParamDirectory + @"\" + $"{PortId}_{ManagedFileInfo.EquipMotionParamFileName}";

                    if (!File.Exists(filePath))
                    {
                        LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Error, LogMsg.MsgList.FileIsNotExist, $"Port Motion Param");
                        return;
                    }

                    portMotionParameter = (PortMotionParameter)MyXML.XmlToClass(filePath, typeof(PortMotionParameter));

                    if (portMotionParameter != null)
                    {
                        LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Normal, LogMsg.MsgList.FileLoadSuccess, $"Port Motion Param");
                    }
                    else
                        LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"Port Motion Param");
                }
                catch(Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{PortId}] Motion Param Load Error");
                }
            }
            public bool Save(string PortId, PortMotionParameter portMotionParameter, bool bWithBackup = true)
            {
                try
                {
                    string filePath = ManagedFileInfo.EquipMotionParamDirectory + @"\" + $"{PortId}_{ManagedFileInfo.EquipMotionParamFileName}";

                    if (File.Exists(filePath) && bWithBackup)
                    {
                        if (MyFile.BackupAndRemove(filePath))
                            LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Normal, LogMsg.MsgList.FileBackupSuccess, $"Port Motion Param");
                    }

                    if (File.Exists(filePath))
                    {
                        //backup 과정에서 file 삭제되는 경우도 있으므로 재 검사
                        File.SetAttributes(filePath, File.GetAttributes(filePath) & FileAttributes.Archive);
                    }

                    MyXML.ClassToXml(filePath, portMotionParameter, typeof(PortMotionParameter));
                    File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);

                    LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Normal, LogMsg.MsgList.FileSaveSuccess, $"Port Motion Param");
                    return true;
                }
                catch(Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{PortId}] Motion Param Save Error");
                    return false;
                }
            }
            
            public int GetServoAxisNum(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;
                return Ctrl_Axis[Index].servoParam.AxisNum;
            }
            public Port.AxisCtrlType GetAxisControlType(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return Port.AxisCtrlType.None;

                return Ctrl_Axis[Index].eAxisCtrlType;
            }
            public void SetAxisControlType(Port.PortAxis ePortAxis, Port.AxisCtrlType eAxisCtrlType)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return;

                Ctrl_Axis[Index].eAxisCtrlType = eAxisCtrlType;
            }
            public Port.CVCtrlEnable GetBufferControlEnable(Port.BufferCV eBufferCV)
            {
                int Index = (int)eBufferCV;

                if (Index >= Ctrl_CV.Length)
                    return Port.CVCtrlEnable.Disable;

                return Ctrl_CV[Index].eCVCtrlEnable;
            }
            public void SetBufferControlEnable(Port.BufferCV eBufferCV, Port.CVCtrlEnable eCVCtrlEnable)
            {
                int Index = (int)eBufferCV;

                if (Index >= Ctrl_CV.Length)
                    return;

                Ctrl_CV[Index].eCVCtrlEnable = eCVCtrlEnable;
            }
            public float GetTeachingPos(Port.PortAxis ePortAxis, int TeachingIndex)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return 0.0f;

                return Ctrl_Axis[Index].servoParam.TeachingPos[TeachingIndex];
            }
            public Port.PositionCheckType GetPositionCheckType(Port.PortAxis ePortAxis, int TeachingIndex)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return Port.PositionCheckType.Servo_and_Sensor;

                return Ctrl_Axis[Index].servoParam.PositionCheckType[TeachingIndex];
            }
            public ServoParam GetShuttleCtrl_ServoParam(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return null;

                return Ctrl_Axis[Index].servoParam;
            }
            public void SetShuttleCtrl_ServoParam(Port.PortAxis ePortAxis, ServoParam setParam)
            {
                int Index = (int)ePortAxis;
                Ctrl_Axis[Index].servoParam = new ServoParam();
                Ctrl_Axis[Index].servoParam = setParam;
            }
            public InverterParam GetShuttleCtrl_InvParam(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return null;

                return Ctrl_Axis[Index].inverterParam;
            }
            public void SetShuttleCtrl_InvParam(Port.PortAxis ePortAxis, InverterParam setParam)
            {
                int Index = (int)ePortAxis;
                Ctrl_Axis[Index].inverterParam = new InverterParam();
                Ctrl_Axis[Index].inverterParam = setParam;
            }
            public CylinderParam GetShuttleCtrl_CylParam(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return null;

                return Ctrl_Axis[Index].cylinderParam;
            }
            public void SetShuttleCtrl_CylParam(Port.PortAxis ePortAxis, CylinderParam setParam)
            {
                int Index = (int)ePortAxis;
                Ctrl_Axis[Index].cylinderParam = new CylinderParam();
                Ctrl_Axis[Index].cylinderParam = setParam;
            }

            public BufferCVParam GetBufferCVParam(Port.BufferCV eBufferCV)
            {
                int Index = (int)eBufferCV;
                return Ctrl_CV[Index];
            }
            public void SetBufferCVParam(Port.BufferCV eBufferCV, BufferCVParam setParam)
            {
                int Index = (int)eBufferCV;
                Ctrl_CV[Index] = new BufferCVParam();
                Ctrl_CV[Index] = setParam;
            }
            public InverterParam GetBufferCtrl_CVParam(Port.BufferCV eBufferCV)
            {
                int Index = (int)eBufferCV;
                return Ctrl_CV[Index].CVParam;
            }
            public void SetBufferCtrl_CVParam(Port.BufferCV eBufferCV, InverterParam setParam)
            {
                int Index = (int)eBufferCV;
                Ctrl_CV[Index].CVParam = new InverterParam();
                Ctrl_CV[Index].CVParam = setParam;
            }
            public CylinderParam GetBufferCtrl_StopperParam(Port.BufferCV eBufferCV)
            {
                int Index = (int)eBufferCV;
                return Ctrl_CV[Index].StopperParam;
            }
            public void SetBufferCtrl_StopperParam(Port.BufferCV eBufferCV, CylinderParam setParam)
            {
                int Index = (int)eBufferCV;
                Ctrl_CV[Index].StopperParam = new CylinderParam();
                Ctrl_CV[Index].StopperParam = setParam;
            }
            public CylinderParam GetBufferCtrl_CenteringParam(Port.BufferCV eBufferCV)
            {
                int Index = (int)eBufferCV;
                return Ctrl_CV[Index].CenteringParam;
            }
            public void SetBufferCtrl_CenteringParam(Port.BufferCV eBufferCV, CylinderParam setParam)
            {
                int Index = (int)eBufferCV;
                Ctrl_CV[Index].CenteringParam = new CylinderParam();
                Ctrl_CV[Index].CenteringParam = setParam;
            }
            public IOParam GetBufferCtrl_CSTDetectParam(Port.BufferCV eBufferCV)
            {
                int Index = (int)eBufferCV;
                return Ctrl_CV[Index].CST_DetectParam;
            }
            public void SetBufferCtrl_CSTDetectParam(Port.BufferCV eBufferCV, IOParam IOParam)
            {
                int Index = (int)eBufferCV;
                Ctrl_CV[Index].CST_DetectParam = new IOParam();
                Ctrl_CV[Index].CST_DetectParam = IOParam;
            }



            public bool IsAxisUnUsed(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;
                return Ctrl_Axis[Index].eAxisCtrlType == Port.AxisCtrlType.None;
            }
            public bool IsCVUsed(Port.BufferCV eBufferCV)
            {
                int Index = (int)eBufferCV;

                if (Index >= Ctrl_CV.Length)
                    return false;

                return Ctrl_CV[Index].eCVCtrlEnable == Port.CVCtrlEnable.Enable;
            }
            public bool IsBPCVUsed()
            {
                bool bEnable = Ctrl_CV[(int)Port.BufferCV.Buffer_BP1].eCVCtrlEnable == Port.CVCtrlEnable.Enable || Ctrl_CV[(int)Port.BufferCV.Buffer_BP2].eCVCtrlEnable == Port.CVCtrlEnable.Enable ||
                    Ctrl_CV[(int)Port.BufferCV.Buffer_BP3].eCVCtrlEnable == Port.CVCtrlEnable.Enable || Ctrl_CV[(int)Port.BufferCV.Buffer_BP4].eCVCtrlEnable == Port.CVCtrlEnable.Enable;
                return bEnable;
            }
            public Port.BufferCV GetLastUsedBP()
            {
                var origin = Enum.GetValues(typeof(Port.BufferCV));
                Array.Reverse(origin);
                foreach (Port.BufferCV eBufferCV in origin)
                {
                    if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP4)
                    {
                        return Port.BufferCV.Buffer_BP4;
                    }
                    else if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP3)
                    {
                        return Port.BufferCV.Buffer_BP3;
                    }
                    else if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP2)
                    {
                        return Port.BufferCV.Buffer_BP2;
                    }
                    else if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP1)
                    {
                        return Port.BufferCV.Buffer_BP1;
                    }
                }

                return Port.BufferCV.Buffer_BP1;
            }
            public Port.BufferCV GetFirstUsedBP()
            {
                var origin = Enum.GetValues(typeof(Port.BufferCV));
                foreach (Port.BufferCV eBufferCV in origin)
                {
                    if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP1)
                    {
                        return Port.BufferCV.Buffer_BP1;
                    }
                    else if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP2)
                    {
                        return Port.BufferCV.Buffer_BP2;
                    }
                    else if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP3)
                    {
                        return Port.BufferCV.Buffer_BP3;
                    }
                    else if (IsCVUsed(eBufferCV) && eBufferCV == Port.BufferCV.Buffer_BP4)
                    {
                        return Port.BufferCV.Buffer_BP4;
                    }
                }

                return Port.BufferCV.Buffer_BP1;
            }
            
            public bool IsServoType(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return false;

                return Ctrl_Axis[Index].eAxisCtrlType == Port.AxisCtrlType.Servo;
            }
            public bool IsInverterType(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return false;


                return Ctrl_Axis[Index].eAxisCtrlType == Port.AxisCtrlType.Inverter;
            }
            public bool IsCylinderType(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return false;

                return Ctrl_Axis[Index].eAxisCtrlType == Port.AxisCtrlType.Cylinder;
            }
            public bool IsValidServo(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                return (IsServoType(ePortAxis) && GetServoAxisNum(ePortAxis) != -1);
            }
            public bool IsRotaryAxis(Port.PortAxis ePortAxis)
            {
                if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T)
                    return true;
                else
                    return false;
            }
            public bool IsWaitPosEnable(Port.PortAxis ePortAxis)
            {
                int Index = (int)ePortAxis;

                if (Index >= Ctrl_Axis.Length)
                    return false;

                return Ctrl_Axis[Index].servoParam.WaitPosEnable == Port.WaitPosEnable.Enable;
            }

            public bool IsSlowSensorEnable(Port.BufferCV eBufferCV)
            {
                if (eBufferCV != Port.BufferCV.Buffer_LP && eBufferCV != Port.BufferCV.Buffer_OP)
                    return false;

                int Index = (int)eBufferCV;

                if (Index >= Ctrl_CV.Length)
                    return false;

                return Ctrl_CV[Index].eCVSlowSensorEnable == Port.CVSlowSensorEnable.Enable;
            }
            public bool IsStopperEnable(Port.BufferCV eBufferCV)
            {
                if (eBufferCV != Port.BufferCV.Buffer_LP && eBufferCV != Port.BufferCV.Buffer_OP)
                    return false;

                int Index = (int)eBufferCV;

                if (Index >= Ctrl_CV.Length)
                    return false;

                return Ctrl_CV[Index].eCVStopperEnable == Port.CVStopperEnable.Enable;
            }
            public bool IsCenteringEnable(Port.BufferCV eBufferCV)
            {
                if (eBufferCV != Port.BufferCV.Buffer_LP && eBufferCV != Port.BufferCV.Buffer_OP)
                    return false;

                int Index = (int)eBufferCV;

                if (Index >= Ctrl_CV.Length)
                    return false;

                return Ctrl_CV[Index].eCVCenteringEnable == Port.CVCenteringEnable.Enable;
            }
            public bool IsCSTDetectSensorEnable(Port.BufferCV eBufferCV)
            {
                if (eBufferCV == Port.BufferCV.Buffer_LP || eBufferCV == Port.BufferCV.Buffer_OP)
                    return false;

                int Index = (int)eBufferCV;

                if (Index >= Ctrl_CV.Length)
                    return false;

                return Ctrl_CV[Index].eCSTDetectSensorEnable == Port.CVCSTDetectSensorEnable.Enable;
            }
            public bool IsValidIO(IOParam IOParam)
            {
                if (IOParam == null)
                    return false;

                if (IOParam.Bit < 0)
                    return false;

                if (IOParam.StartAddr < 0 || IOParam.StartAddr >= 8000)
                    return false;

                return true;
            }
        
            private void InsertIO(ref Dictionary<int, List<string>> Dic, IOParam IOParam, string IOLocation)
            {
                int StartAddr = IOParam.StartAddr;
                int Bit = IOParam.Bit;

                if (StartAddr != -1 && Bit != -1)
                {
                    int TotalBitAddr = (StartAddr * 8) + Bit;

                    if (!Dic.ContainsKey(TotalBitAddr))
                        Dic.Add(TotalBitAddr, new List<string>() { IOLocation });
                    else
                        Dic[TotalBitAddr].Add(IOLocation);
                }
            }

            public void IOParamDupleCheck(string PortID, ref Dictionary<int, List<string>> OutputsMap, ref Dictionary<int, List<string>> InputsMap)
            {
                foreach(Port.PortAxis ePortAxis in Enum.GetValues(typeof(Port.PortAxis)))
                {
                    if(IsCylinderType(ePortAxis))
                    {
                        var OutParams = GetShuttleCtrl_CylParam(ePortAxis).Ctrl_IOParams;
                        var InParams = GetShuttleCtrl_CylParam(ePortAxis).PosSensor_IOParams;
                        for (int nCount = 0; nCount < OutParams.Length; nCount++)
                        {
                            InsertIO(ref OutputsMap, OutParams[nCount], $"{PortID}_{ePortAxis}_Cylinder_Output");
                        }
                        for (int nCount = 0; nCount < OutParams.Length; nCount++)
                        {
                            InsertIO(ref InputsMap, InParams[nCount], $"{PortID}_{ePortAxis}_Cylinder_Input");
                        }
                    }
                    if (IsInverterType(ePortAxis))
                    {
                        var OutParams = GetShuttleCtrl_InvParam(ePortAxis).Ctrl_IOParams;

                        for (int nCount = 0; nCount < OutParams.Length; nCount++)
                        {
                            InsertIO(ref OutputsMap, OutParams[nCount], $"{PortID}_{ePortAxis}_Inverter_Output");
                        }
                    }
                }

                foreach (Port.BufferCV eBufferCV in Enum.GetValues(typeof(Port.BufferCV)))
                {
                    if(IsCVUsed(eBufferCV))
                    {
                        if(IsStopperEnable(eBufferCV))
                        {
                            var OutParams = GetBufferCtrl_StopperParam(eBufferCV).Ctrl_IOParams;

                            for (int nCount = 0; nCount < OutParams.Length; nCount++)
                            {
                                InsertIO(ref OutputsMap, OutParams[nCount], $"{PortID}_{eBufferCV}_Stopper Output");
                            }
                        }
                        if (IsCenteringEnable(eBufferCV))
                        {
                            var OutParams = GetBufferCtrl_CenteringParam(eBufferCV).Ctrl_IOParams;

                            for (int nCount = 0; nCount < OutParams.Length; nCount++)
                            {
                                InsertIO(ref OutputsMap, OutParams[nCount], $"{PortID}_{eBufferCV}_Centering Output");
                            }
                        }
                        if (IsCSTDetectSensorEnable(eBufferCV))
                        {
                            var Param = GetBufferCtrl_CSTDetectParam(eBufferCV);

                            InsertIO(ref InputsMap, Param, $"{PortID}_{eBufferCV}_CST Detect Input");
                        }

                        var CVParams = GetBufferCtrl_CVParam(eBufferCV).Ctrl_IOParams;

                        for (int nCount = 0; nCount < CVParams.Length; nCount++)
                        {
                            InsertIO(ref OutputsMap, CVParams[nCount], $"{PortID}_{eBufferCV}_Conveyor Output");
                        }
                    }
                }

                var IOOutParams = Ctrl_IO.OutputMap;
                for (int nCount = 0; nCount < IOOutParams.Length; nCount++)
                {
                    InsertIO(ref OutputsMap, IOOutParams[nCount], $"{PortID}_Output Map_{IOOutParams[nCount].Name}");
                }

                var IOInParams = Ctrl_IO.InputMap;
                for (int nCount = 0; nCount < IOInParams.Length; nCount++)
                {
                    InsertIO(ref InputsMap, IOInParams[nCount], $"{PortID}_Input Map_{IOInParams[nCount].Name}");
                }


            }
        }
        
        /// <summary>
        /// 축 관련 제어 파라미터 클래스
        /// 축 -> None, Servo, Inverter, Cylinder
        /// </summary>
        public class AxisControlParam
        {
            public Port.AxisCtrlType eAxisCtrlType = Port.AxisCtrlType.None;    //Axis를 어떻게 사용할 것인가 지정
            public ServoParam servoParam = new ServoParam();                    //Servo로 사용하는 경우 Servo Param
            public InverterParam inverterParam = new InverterParam();           //Inverter로 사용하는 경우 Inverter Param
            public CylinderParam cylinderParam = new CylinderParam();           //Cylinder로 사용하는 경우 Cylinder Param
        }
        
        /// <summary>
        /// 컨베이어 제어 관련 파라미터 클래스
        /// </summary>
        public class BufferCVParam
        {
            public Port.CVCtrlEnable eCVCtrlEnable                      = Port.CVCtrlEnable.Disable; //컨베이어 사용 유무
            public Port.CVStopperEnable eCVStopperEnable                = Port.CVStopperEnable.Disable; //스토퍼 사용 유무
            public Port.CVCenteringEnable eCVCenteringEnable            = Port.CVCenteringEnable.Disable; //센터링 사용 유무
            public Port.CVSlowSensorEnable eCVSlowSensorEnable          = Port.CVSlowSensorEnable.Disable; //슬로우 센서 사용 유무
            public Port.CVCSTDetectSensorEnable eCSTDetectSensorEnable  = Port.CVCSTDetectSensorEnable.Disable; //카세트 감지 센서 추가 사용 유무

            public InverterParam CVParam            = new InverterParam(); //컨베이어 제어 관련 파라미터
            public CylinderParam StopperParam       = new CylinderParam(); //스토퍼 제어 관련 파라미터
            public CylinderParam CenteringParam     = new CylinderParam(); //센터링 제어 관련 파라미터
            public IOParam CST_DetectParam          = new IOParam(); //CST 감지 관련 파라미터
            public IOParam LP_CST_OppositeAngle     = new IOParam(); //LP 대각 관련 파라미터
            public IOParam OP_CST_OppositeAngle     = new IOParam(); //OP 대각 관련 파라미터

            private bool SyncMoveEnable             = false; //컨베이어 여러 축 동기 동작 관련 파라미터
            private bool CST_DetectStatus           = false; //카세트 감지 상태
            private bool bReset                     = false;

            public BufferCVParam()
            {
                eCVStopperEnable = Port.CVStopperEnable.Disable;
                eCVCenteringEnable = Port.CVCenteringEnable.Disable;
                eCVSlowSensorEnable = Port.CVSlowSensorEnable.Disable;
                eCSTDetectSensorEnable = Port.CVCSTDetectSensorEnable.Disable;

                CVParam = new InverterParam();
                StopperParam = new CylinderParam();
                CenteringParam = new CylinderParam();
                CST_DetectParam = new IOParam();
                LP_CST_OppositeAngle = new IOParam();
                OP_CST_OppositeAngle = new IOParam();

                SyncMoveEnable = false;
                CST_DetectStatus = false;
            }
            public bool GetSyncMoveEnable()
            {
                return SyncMoveEnable;
            }
            public void SetSyncMoveEnable(bool bEnable)
            {
                SyncMoveEnable = bEnable;
            }
            public bool GetCSTDetectStatus()
            {
                return CST_DetectStatus;
            }
            public void SetCSTDetectStatus(bool bEnable)
            {
                CST_DetectStatus = bEnable;
            }

            public void SetReset(bool bEnable)
            {
                bReset = bEnable;
            }
        }
        
        /// <summary>
        /// 포트에서 사용되는 In/Out 정보
        /// </summary>
        public class IOMap
        {
            public IOParam[] InputMap = new IOParam[32]; //32bit
            public IOParam[] OutputMap = new IOParam[32]; //32bit

            public IOMap()
            {
                for (int nCount = 0; nCount < InputMap.Length; nCount++)
                    InputMap[nCount] = new IOParam();
                for (int nCount = 0; nCount < OutputMap.Length; nCount++)
                    OutputMap[nCount] = new IOParam();
            }
        }

        /// <summary>
        /// 축 서보 사용시 제어 관련 파라미터
        /// </summary>
        public class ServoParam
        {
            public int AxisNum = -1;                                                //WMX 축 번호 지정
            public Port.WaitPosEnable WaitPosEnable = Port.WaitPosEnable.Disable;   //Wait Pos 사용 유무

            //Teaching 이동 시 Sensor Check Type 지정
            public Port.PositionCheckType[] PositionCheckType = new Port.PositionCheckType[4] {Port.PositionCheckType.Servo_and_Sensor, Port.PositionCheckType.Servo_and_Sensor, Port.PositionCheckType.Servo_and_Sensor, Port.PositionCheckType.Servo_and_Sensor };
            //Teaching 위치 (축 마다 배열 순서 다름)
            public float[] TeachingPos = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
            public float OverrideDistance = 0;              //Z축 사용 시 Profile Override 위치 (카세트와 맞 닿는 위치)
            public float OverrideDecPercent = 10.0f;        //Overrride 감속 비율 카세트 맞 닿는 경우 (지정 %까지 속도가 감소됨)
            public float Manual_Speed = 1;                  //Manual 조작 시 속도
            public float Manual_Acc = 1;
            public float Manual_Dec = 1;
            public float AutoRun_Speed = 1;                 //Auto 조작 시 속도
            public float AutoRun_Acc = 1;
            public float AutoRun_Dec = 1;
            public short MaxLoad = 100;
            public string CrashCheckID = string.Empty;      //T 축 사용 시 인접 포트와 충돌 방지 위한 인터락
            public string ManualPath = string.Empty;        //Manual File 등 서보 Status 창에서 알람 버튼 클릭 시 매뉴얼 출력 위함
            public WMXMotion.AxisParameter WMXParam = new WMXMotion.AxisParameter(); //WMX에 적용 할 Parameter 정보

            public static bool IsValidAxisNum(string Axis)
            {
                if (int.TryParse(Axis, out int Value))
                {
                    if (Value < 0 || Value > 128)
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisNumberOverRangeError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            public static bool IsValidTeachingPos(string Pos, Port.PortAxis ePortAxis)
            {
                if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T)
                {
                    if (float.TryParse(Pos, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        double TPosLowLimit = PortParameterInterlock.Port_T_Axis_Teaching_0Deg_Limit_deg;
                        double TPosUpLimit = PortParameterInterlock.Port_T_Axis_Teaching_180Deg_Limit_deg;

                        if (Value < TPosLowLimit || Value > TPosUpLimit)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisTeachingPosOverRangeError") + $"\n(Input Range : {TPosLowLimit} ~ {TPosUpLimit})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisTeachingPosStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if(ePortAxis == Port.PortAxis.Shuttle_Z || ePortAxis == Port.PortAxis.Buffer_LP_Z || ePortAxis == Port.PortAxis.Buffer_OP_Z ||
                     ePortAxis == Port.PortAxis.Buffer_LP_Y || ePortAxis == Port.PortAxis.Buffer_OP_Y)
                {
                    if (float.TryParse(Pos, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        double ZPosLowLimit = PortParameterInterlock.Port_Z_Axis_Teaching_Down_Limit_mm;
                        double ZPosUpLimit = PortParameterInterlock.Port_Z_Axis_Teaching_Up_Limit_mm;

                        if (Value < ZPosLowLimit || Value > ZPosUpLimit)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisTeachingPosOverRangeError") + $"\n(Input Range : {ZPosLowLimit} ~ {ZPosUpLimit})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisTeachingPosStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    if (float.TryParse(Pos, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        double XPosLowLimit = PortParameterInterlock.Port_X_Axis_Teaching_LP_Limit_mm;
                        double XPosUpLimit = PortParameterInterlock.Port_X_Axis_Teaching_OP_Limit_mm;

                        if (Value < XPosLowLimit || Value > XPosUpLimit)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisTeachingPosOverRangeError") + $"\n(Input Range : {XPosLowLimit} ~ {XPosUpLimit})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisTeachingPosStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            public static bool IsValidSpeed(string Speed, Port.PortAxis ePortAxis)
            {
                if(ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T)
                {
                    if (float.TryParse(Speed, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        if (Value < 0 || Value > PortParameterInterlock.Port_T_Axis_Vel_Limit_deg)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisSpeedOverRangeError") + $"\n(Input Range : 0 ~ {PortParameterInterlock.Port_T_Axis_Vel_Limit_deg})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisSpeedStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    bool bXType = ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X;

                    if (float.TryParse(Speed, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        double VelLimit = (bXType ? PortParameterInterlock.Port_X_Axis_Vel_Limit_mm : PortParameterInterlock.Port_Z_Axis_Vel_Limit_mm);

                        if (Value < 0 || Value > VelLimit)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisSpeedOverRangeError") + $"\n(Input Range : 0 ~ {VelLimit})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisSpeedStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            public static bool IsValidAcc(string Acc, Port.PortAxis ePortAxis)
            {
                if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T)
                {
                    if (float.TryParse(Acc, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        if (Value < 0 || Value > PortParameterInterlock.Port_T_Axis_Acc_Limit_deg)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisAccOverRangeError") + $"\n(Input Range : 0 ~ {PortParameterInterlock.Port_T_Axis_Acc_Limit_deg})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisAccStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    bool bXType = ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X;

                    if (float.TryParse(Acc, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        double AccLimit = (bXType ? PortParameterInterlock.Port_X_Axis_Acc_Limit_mm : PortParameterInterlock.Port_Z_Axis_Acc_Limit_mm);

                        if (Value < 0 || Value > AccLimit)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisAccOverRangeError") + $"\n(Input Range : 0 ~ {AccLimit})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisAccStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            public static bool IsValidDec(string Dec, Port.PortAxis ePortAxis)
            {
                if (ePortAxis == Port.PortAxis.Shuttle_T || ePortAxis == Port.PortAxis.Buffer_LP_T || ePortAxis == Port.PortAxis.Buffer_OP_T)
                {
                    if (float.TryParse(Dec, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        if (Value < 0 || Value > PortParameterInterlock.Port_T_Axis_Dec_Limit_deg)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisDecOverRangeError") + $"\n(Input Range : 0 ~ {PortParameterInterlock.Port_T_Axis_Dec_Limit_deg})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisDecStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    bool bXType = ePortAxis == Port.PortAxis.Shuttle_X || ePortAxis == Port.PortAxis.Buffer_LP_X || ePortAxis == Port.PortAxis.Buffer_OP_X;

                    if (float.TryParse(Dec, out float Value))
                    {
                        var PortParameterInterlock = ManagedFile.ApplicationParam.m_PortParameterInterLock;
                        double DecLimit = (bXType ? PortParameterInterlock.Port_X_Axis_Dec_Limit_mm : PortParameterInterlock.Port_Z_Axis_Dec_Limit_mm);

                        if (Value < 0 || Value > DecLimit)
                        {
                            MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisDecOverRangeError") + $"\n(Input Range : 0 ~ {DecLimit})", SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                            return true;
                    }
                    else
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_AxisDecStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            public static bool IsValidMaxLoad(string Load)
            {
                if (short.TryParse(Load, out short Value))
                {
                    if (Value < 10 || Value > 300)
                    {
                        MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_MaxLoadOverRangeError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;
                }
                else
                {
                    MessageBox.Show(SynusLangPack.GetLanguage("Message_Port_MaxLoadStrConvertError"), SynusLangPack.GetLanguage("ErrorMessage"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        
        /// <summary>
        /// 축 인버터 사용시 제어 관련 파라미터
        /// </summary>
        public class InverterParam
        {
            public Port.InvCtrlMode InvCtrlMode = Port.InvCtrlMode.IOControl; //Inverter Control Mode -> 주파수 제어(이더캣, 오므론), 접점 제어 2종류
            public IOParam[] Ctrl_IOParams = new IOParam[Enum.GetValues(typeof(Port.InvIOCtrlFlag)).Length];
            bool[] Ctrl_IOFlag = Enumerable.Repeat(false, Enum.GetValues(typeof(Port.InvCtrlType)).Length).ToArray();
            bool[] Ctrl_IOStatus = Enumerable.Repeat(false, Enum.GetValues(typeof(Port.InvCtrlType)).Length).ToArray();
            public int HzStartAddr = -1;        //주파수 제어 시 맵핑된 주파수 PDO의 Write 주소 지정
            public short HzTarget = 6000;       //PDO에 Write할 값 지정

            private bool bResetFlag = false;

            public InverterParam()
            {
                for (int nCount = 0; nCount < Ctrl_IOParams.Length; nCount++)
                    Ctrl_IOParams[nCount] = new IOParam();

                for (int nCount = 0; nCount < Ctrl_IOFlag.Length; nCount++)
                    Ctrl_IOFlag[nCount] = new bool();

                for (int nCount = 0; nCount < Ctrl_IOStatus.Length; nCount++)
                    Ctrl_IOStatus[nCount] = new bool();
            }
            public void SetRunFlag(Port.InvCtrlType eList, bool bEnable)
            {
                int Index = (int)eList;
                Ctrl_IOFlag[Index] = bEnable;
            }
            public bool GetRunFlag(Port.InvCtrlType eList)
            {
                int Index = (int)eList;
                return Ctrl_IOFlag[Index];
            }
            public void SetRunStatus(Port.InvCtrlType eList, bool bEnable)
            {
                int Index = (int)eList;
                Ctrl_IOStatus[Index] = bEnable;
            }
            public bool GetRunStatus(Port.InvCtrlType eList)
            {
                int Index = (int)eList;
                return Ctrl_IOStatus[Index];
            }
            public IOParam GetIOParam(Port.InvIOCtrlFlag eList)
            {
                int Index = (int)eList;
                return Ctrl_IOParams[Index];
            }
            public void SetResetFlag(bool bEnable)
            {
                bResetFlag = bEnable;
            }
            public bool GetResetFlag()
            {
                return bResetFlag;
            }

            public bool IsBusy()
            {
                for(int nCount = 0; nCount < Ctrl_IOStatus.Length; nCount++)
                {
                    if (Ctrl_IOStatus[nCount])
                        return true;
                }

                return false;
            }
        }
        
        /// <summary>
        /// 축 실린더 사용시 제어 관련 파라미터
        /// </summary>
        public class CylinderParam
        {
            public IOParam[] Ctrl_IOParams          = new IOParam[Enum.GetValues(typeof(Port.CylCtrlList)).Length];
            bool[] Ctrl_IOFlag                      = Enumerable.Repeat(false, Enum.GetValues(typeof(Port.CylCtrlList)).Length).ToArray();
            bool[] Ctrl_IOStatus                    = Enumerable.Repeat(false, Enum.GetValues(typeof(Port.CylCtrlList)).Length).ToArray();

            public IOParam[] PosSensor_IOParams     = new IOParam[Enum.GetValues(typeof(Port.CylCtrlList)).Length];
            bool[] PosSensor_IOStatus               = Enumerable.Repeat(false, Enum.GetValues(typeof(Port.CylCtrlList)).Length).ToArray();

            public CylinderParam()
            {
                for (int nCount = 0; nCount < Ctrl_IOParams.Length; nCount++)
                    Ctrl_IOParams[nCount] = new IOParam();

                for (int nCount = 0; nCount < Ctrl_IOFlag.Length; nCount++)
                    Ctrl_IOFlag[nCount] = new bool();

                for (int nCount = 0; nCount < Ctrl_IOStatus.Length; nCount++)
                    Ctrl_IOStatus[nCount] = new bool();

                for (int nCount = 0; nCount < PosSensor_IOParams.Length; nCount++)
                    PosSensor_IOParams[nCount] = new IOParam();

                for (int nCount = 0; nCount < PosSensor_IOStatus.Length; nCount++)
                    PosSensor_IOStatus[nCount] = new bool();
            }

            public void SetRunFlag(Port.CylCtrlList eList, bool bEnable)
            {
                int Index = (int)eList;
                Ctrl_IOFlag[Index] = bEnable;
            }
            public bool GetRunFlag(Port.CylCtrlList eList)
            {
                int Index = (int)eList;
                return Ctrl_IOFlag[Index];
            }
            public void SetRunStatus(Port.CylCtrlList eList, bool bEnable)
            {
                int Index = (int)eList;
                Ctrl_IOStatus[Index] = bEnable;
            }
            public bool GetRunStatus(Port.CylCtrlList eList)
            {
                int Index = (int)eList;
                return Ctrl_IOStatus[Index];
            }
            public IOParam GetCtrlIOParam(Port.CylCtrlList eList)
            {
                int Index = (int)eList;
                return Ctrl_IOParams[Index];
            }
            public IOParam GetPosSensorIOParam(Port.CylCtrlList eList)
            {
                int Index = (int)eList;
                return PosSensor_IOParams[Index];
            }
            public void SetPosSensorStatus(Port.CylCtrlList eList, bool bEnable)
            {
                int Index = (int)eList;
                PosSensor_IOStatus[Index] = bEnable;
            }
            public bool GetPosSensorStatus(Port.CylCtrlList eList)
            {
                int Index = (int)eList;
                return PosSensor_IOStatus[Index];
            }

            public bool IsBusy()
            {
                for (int nCount = 0; nCount < Ctrl_IOStatus.Length; nCount++)
                {
                    if (Ctrl_IOStatus[nCount])
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// IO Map의 IO Item 파라미터
        /// </summary>
        public class IOParam
        {
            public string Name = string.Empty;
            public int StartAddr = -1;
            public int Bit = -1;
            public bool bInvert = false;

            public bool IsValidBitRange()
            {
                if (Bit < -1 || Bit >= 8)
                    return false;

                return true;
            }
            public bool IsValidStartAddrRange()
            {
                if (StartAddr < -1 || StartAddr >= 8000)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Watchdog Parameter (msec 단위)
        /// </summary>
        public class Port_WatchdogParam
        {
            public int X_Axis_HomingTimer = 1000;
            public int X_Axis_Move_To_LPTimer = 1000;
            public int X_Axis_Move_To_WaitTimer = 1000;
            public int X_Axis_Move_To_OPTimer = 1000;
            public int Z_Axis_HomingTimer = 1000;
            public int Z_Axis_Move_To_UpTimer = 1000;
            public int Z_Axis_Move_To_DownTimer = 1000;
            public int T_Axis_HomingTimer = 1000;
            public int T_Axis_Move_To_0DegTimer = 1000;
            public int T_Axis_Move_To_180DegTimer = 1000;
            public int RM_ForkDetectTimer = 1000;
            public int OHT_HoistDetectTimer = 1000;
            public int OP_Placement_ErrorTimer = 1000;
            public int BP_Placement_ErrorTimer = 1000;
            public int LP_Placement_ErrorTimer = 1000;
            public int PortArea_Timer = 1000;
            public int PortArea_ReleaseTimer = 0;
            public int PortArea_And_ShuttleMovingTimer = 1000;
            public int LP_PIO_Timer = 1000;
            public int OP_PIO_Timer = 1000;
            public int LP_Step_Timer = 1000;
            public int BP_Step_Timer = 1000;
            public int OP_Step_Timer = 1000;
            public int EQ_Step_Timer = 1000;
            public int Buffer1_CV_Moving_Timer = 1000;
            public int Buffer2_CV_Moving_Timer = 1000;
        }

        /// <summary>
        /// Port IO Map 관련 파라미터
        /// UIParam에 저장된 파일 정보에서 로드 진행
        /// </summary>
        public class Port_SafetyImageInfo
        {
            public class SafetyItem
            {
                public string Text = string.Empty;
                public int X = 0;
                public int Y = 0;
            }

            Image DefaultImage = Properties.Resources.icons8_no_image_96;
            public string MainImagePath         = string.Empty; //배경 사진
            public string EquipmentImagePath    = string.Empty; //장비 사진(PIO 주체 들)
            public int EquipmentImageLocation_X = 0;
            public int EquipmentImageLocation_Y = 0;
            public SafetyItem[] SafetyItems = new SafetyItem[32];

            public Port_SafetyImageInfo()
            {
                for (int nCount = 0; nCount < SafetyItems.Length; nCount++)
                    SafetyItems[nCount] = new SafetyItem();
            }

            public Image GetDefaultImage()
            {
                return DefaultImage;
            }
        }
        
        /// <summary>
        /// Port 조작 시 마지막 제어 값 유지 하기 위한 File
        /// Port IO Map 편집 시 이미지 및 상태 패널 위치 지정
        /// </summary>
        public class Port_UIParam
        {
            public Port_SafetyImageInfo[] port_SafetyImageInfos  = new Port_SafetyImageInfo[(int)Enum.GetValues(typeof(Port.Port_IO_TabPage)).Length];
            /// <summary>
            /// Port 제어 시 마지막 성공 값을 기록
            /// </summary>
            public int PortManualSpeedRatio = 50;
            public float X_Axis_JogLowSpeed = 1;
            public float X_Axis_JogHighSpeed = 1;
            public float X_Axis_Inching = 1;
            public float X_Axis_Target = 10.0f;
            public float Z_Axis_JogLowSpeed = 1;
            public float Z_Axis_JogHighSpeed = 1;
            public float Z_Axis_Inching = 1;
            public float Z_Axis_Target = 10.0f;
            public float T_Axis_JogLowSpeed = 1;
            public float T_Axis_JogHighSpeed = 1;
            public float T_Axis_Inching = 1;
            public float T_Axis_Target = 10.0f;

            public Port_UIParam()
            {
                for (int nCount = 0; nCount < port_SafetyImageInfos.Length; nCount++)
                    port_SafetyImageInfos[nCount] = new Port_SafetyImageInfo();
            }


            /// <summary>
            /// MasterSafetyImageInfo.cs의 Load, Save 과정 참고
            /// </summary>
            /// <param name="PortId"></param>
            /// <param name="portUIParameter"></param>
            public void Load(string PortId, ref Port_UIParam portUIParameter)
            {
                try
                {
                    string filePath = ManagedFileInfo.PortUIParamDirectory + @"\" + $"{PortId}_{ManagedFileInfo.PortUIParamFileName}";


                    if (!File.Exists(filePath))
                    {
                        LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Error, LogMsg.MsgList.FileIsNotExist, $"Port UI Param");
                        Save(PortId, portUIParameter);
                        return;
                    }

                    portUIParameter = (Port_UIParam)MyXML.XmlToClass(filePath, typeof(Port_UIParam));

                    if (portUIParameter != null)
                        LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Normal, LogMsg.MsgList.FileLoadSuccess, $"Port UI Param");
                    else
                    {
                        LogMsg.AddPortLog(PortId, LogMsg.LogLevel.Error, LogMsg.MsgList.FileLoadFail, $"Port UI Param");
                        Save(PortId, portUIParameter);
                    }
                }
                catch(Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{PortId}] UI Param Load Error");
                }
            }

            public bool Save(string PortId, Port_UIParam portUIParameter)
            {
                try
                {
                    string filePath = ManagedFileInfo.PortUIParamDirectory + @"\" + $"{PortId}_{ManagedFileInfo.PortUIParamFileName}";

                    if (File.Exists(filePath))
                    {
                        MyFile.BackupAndRemove(filePath);
                    }

                    if (File.Exists(filePath))
                    {
                        //backup 과정에서 file 삭제되는 경우도 있으므로 재 검사
                        File.SetAttributes(filePath, File.GetAttributes(filePath) & FileAttributes.Archive);
                    }

                    MyXML.ClassToXml(filePath, portUIParameter, typeof(Port_UIParam));
                    File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
                    return true;
                }
                catch(Exception ex)
                {
                    LogMsg.AddExceptionLog(ex, $"Port[{PortId}] UI Param Save Error");
                    return false;
                }
            }
        }
    }

}
